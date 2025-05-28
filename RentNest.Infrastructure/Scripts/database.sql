use RentNestSystem;

CREATE TABLE Account (
    account_id INT IDENTITY(1,1) PRIMARY KEY,
    username NVARCHAR(100) NULL,
    email NVARCHAR(255) NOT NULL UNIQUE,
    password NVARCHAR(255) NULL,
    is_active CHAR(1) NOT NULL DEFAULT 'A' CHECK (is_active IN ('A', 'D')),  --A is active, D is disabled
    auth_provider VARCHAR(20) NOT NULL CHECK (auth_provider IN ('local', 'google', 'facebook')),
    auth_provider_id VARCHAR(255),
	is_online BIT DEFAULT 0,
	last_active_at DATETIME NULL,
    created_at DATETIME NOT NULL DEFAULT GETDATE(),
    updated_at DATETIME NOT NULL DEFAULT GETDATE(),
    role CHAR(1) NOT NULL CHECK (role IN ('U', 'A', 'S', 'L')) -- U=User, S = Staff, A=Admin, L=Landlord
);

CREATE UNIQUE INDEX UX_Account_Username
ON Account (Username)
WHERE Username IS NOT NULL;

-- Chỉ đánh UNIQUE khi auth_provider_id không phải NULL
CREATE UNIQUE INDEX UX_Account_AuthProviderId
ON Account (auth_provider_id)
WHERE auth_provider_id IS NOT NULL;


CREATE TABLE AccommodationType (
    type_id INT IDENTITY(1,1) PRIMARY KEY,
    type_name NVARCHAR(100) NOT NULL UNIQUE,
    description NVARCHAR(255)
);

CREATE TABLE PaymentMethod (
    method_id INT IDENTITY(1,1) PRIMARY KEY,
    method_name NVARCHAR(100) not null,
    description NVARCHAR(MAX),
    is_active BIT DEFAULT 1,
    icon_url VARCHAR(255)
);

CREATE TABLE TimeUnitPackage (
    time_unit_id INT IDENTITY(1,1) PRIMARY KEY,
    time_unit_name NVARCHAR(20) NOT NULL CHECK (time_unit_name IN (N'Ngày', N'Tuần', N'Tháng')),
    description NVARCHAR(255),
	data varchar(50)
);

CREATE TABLE PostPackageType (
    package_type_id INT IDENTITY(1,1) PRIMARY KEY,
    package_type_name NVARCHAR(100) NOT NULL,
    priority INT NOT NULL,
    description NVARCHAR(255)
);

CREATE TABLE UserProfile (
    profile_id INT IDENTITY(1,1) PRIMARY KEY,
    first_name NVARCHAR(100),
    last_name NVARCHAR(100),
	phone_number nvarchar(20),
    gender CHAR(1) CHECK (gender IN ('M', 'F', 'O')), -- M=Male, F=Female, O=Other
    date_of_birth DATE,
    avatar_url VARCHAR(255),
    occupation NVARCHAR(100),
    address NVARCHAR(MAX),
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    account_id INT UNIQUE,
    CONSTRAINT FK_UserProfile_Account FOREIGN KEY (account_id)
        REFERENCES Account(account_id)
        ON DELETE CASCADE
);

CREATE TABLE Accommodation (
    accommodation_id INT IDENTITY(1,1) PRIMARY KEY,
    title NVARCHAR(150) NOT NULL,
    description NVARCHAR(MAX),
    address NVARCHAR(255) NOT NULL,
    price DECIMAL(10, 2) CHECK (price >= 0),
    deposit_amount DECIMAL(10, 2) CHECK (deposit_amount >= 0),
    area INT CHECK (area > 0),
    max_occupancy INT CHECK (max_occupancy > 0),
    video_url VARCHAR(255),
    status CHAR(1) NOT NULL DEFAULT 'A' CHECK (status IN ('A', 'R', 'I')), -- available, rented, inactive
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    owner_id INT NOT NULL,
    [type_id] INT NOT NULL,
    CONSTRAINT FK_Accommodation_Owner FOREIGN KEY (owner_id)
        REFERENCES Account(account_id)
        ON DELETE CASCADE,
    CONSTRAINT FK_Accommodation_Type FOREIGN KEY (type_id)
        REFERENCES AccommodationType(type_id)
);
ALTER TABLE Accommodation
ADD district_name NVARCHAR(255) NULL,
    ward_name NVARCHAR(255) NULL,
    province_name NVARCHAR(255) NULL;


-- 4. T?o b?ng giá gói tin
CREATE TABLE PackagePricing (
    pricing_id INT IDENTITY(1,1) PRIMARY KEY,
    time_unit_id INT NOT NULL,
    package_type_id INT NOT NULL,
    duration_value INT CHECK (duration_value > 0) NOT NULL, -- S? lu?ng th?i gian (1, 2, 3,...)
    unit_price DECIMAL(10,2) CHECK (unit_price >= 0) NOT NULL, -- Giá ti?n cho m?i don v?
    total_price DECIMAL(10,2) CHECK (total_price >= 0) NOT NULL, -- T?ng giá tr?n gói (có th? có gi?m giá)
    is_active BIT DEFAULT 1,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_PackagePricing_TimeUnit FOREIGN KEY (time_unit_id)
        REFERENCES TimeUnitPackage(time_unit_id),
    CONSTRAINT FK_PackagePricing_Type FOREIGN KEY (package_type_id)
        REFERENCES PostPackageType(package_type_id),
    CONSTRAINT UQ_PackagePricing_Combination UNIQUE (time_unit_id, package_type_id, duration_value)
);

CREATE TABLE AccommodationDetails (
    detail_id INT IDENTITY(1,1) PRIMARY KEY,

    has_kitchen_cabinet BIT DEFAULT 0,
    has_air_conditioner BIT DEFAULT 0,
    has_refrigerator BIT DEFAULT 0,
    has_washing_machine BIT DEFAULT 0,
    has_loft BIT DEFAULT 0,
    furniture_status NVARCHAR(100),

    bedroom_count INT DEFAULT 0,
    bathroom_count INT DEFAULT 0,

    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    accommodation_id INT UNIQUE NOT NULL,
    CONSTRAINT FK_Details_Accommodation FOREIGN KEY (accommodation_id)
        REFERENCES Accommodation(accommodation_id)
        ON DELETE CASCADE
);

CREATE TABLE Amenities (
    amenity_id INT IDENTITY(1,1) PRIMARY KEY,
    amenity_name NVARCHAR(100) NOT NULL,
	iconSvg varchar(max)
);

CREATE TABLE AccommodationAmenities (
    id INT IDENTITY(1,1) PRIMARY KEY,
    accommodation_id INT NOT NULL,
    amenity_id INT NOT NULL,
    CONSTRAINT FK_Amenities_Accommodation FOREIGN KEY (accommodation_id)
        REFERENCES Accommodation(accommodation_id)
        ON DELETE CASCADE,
    CONSTRAINT FK_Amenities FOREIGN KEY (amenity_id)
        REFERENCES Amenities(amenity_id)
        ON DELETE CASCADE
);


CREATE TABLE AccommodationImage (
    image_id INT IDENTITY(1,1) PRIMARY KEY,
    image_url VARCHAR(255) NOT NULL,
    caption VARCHAR(255),
    created_at DATETIME DEFAULT GETDATE(),
    accommodation_id INT NOT NULL,
    CONSTRAINT FK_AccommodationImage_Accommodation FOREIGN KEY (accommodation_id)
        REFERENCES Accommodation(accommodation_id)
        ON DELETE CASCADE
);

CREATE TABLE Post (
    post_id INT IDENTITY(1,1) PRIMARY KEY,
    title NVARCHAR(150) NOT NULL,
    content NVARCHAR(MAX) NULL,
    current_status CHAR(1) NOT NULL DEFAULT 'P' CHECK (current_status IN ('P', 'A', 'R')),
    view_count INT DEFAULT 0 CHECK (view_count >= 0),
    published_at DATETIME,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    accommodation_id INT NOT NULL,
    account_id INT NOT NULL,
    CONSTRAINT FK_Post_Accommodation FOREIGN KEY (accommodation_id)
        REFERENCES Accommodation(accommodation_id)
        ON DELETE CASCADE,
    CONSTRAINT FK_Post_Account FOREIGN KEY (account_id)
        REFERENCES Account(account_id)
        ON DELETE NO ACTION
);

CREATE TABLE PostPackageDetails (
    id INT IDENTITY(1,1) PRIMARY KEY,
    post_id INT NOT NULL,
    pricing_id INT NOT NULL,
    total_price DECIMAL(10, 2) CHECK (total_price >= 0) NOT NULL,
    start_date DATETIME NOT NULL,
    end_date DATETIME NOT NULL,
    payment_status VARCHAR(20) DEFAULT 'Pending' CHECK (payment_status IN ('Pending', 'Completed', 'Failed', 'Refunded')),
    payment_transaction_id VARCHAR(100),
    created_at DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_PostPackageDetails_Post FOREIGN KEY (post_id)
        REFERENCES Post(post_id)
        ON DELETE CASCADE,
    CONSTRAINT FK_PostPackageDetails_Pricing FOREIGN KEY (pricing_id)
        REFERENCES PackagePricing(pricing_id)
);

CREATE TABLE PostApprovals (
    approval_id INT IDENTITY(1,1) PRIMARY KEY,
    status CHAR(1) NOT NULL DEFAULT 'P' CHECK (status IN ('P', 'A', 'R')),
    rejection_reason NVARCHAR(255),
    note NVARCHAR(255),
    processed_at DATETIME DEFAULT GETDATE(),
    post_id INT NOT NULL,
    approved_by_account_id INT,
    CONSTRAINT FK_PostApprovals_Post FOREIGN KEY (post_id)
        REFERENCES Post(post_id)
        ON DELETE CASCADE,
    CONSTRAINT FK_PostApprovals_Approver FOREIGN KEY (approved_by_account_id)
        REFERENCES Account(account_id)
        ON DELETE NO ACTION
);

CREATE TABLE FavoritePost (
    favorite_id INT IDENTITY(1,1) PRIMARY KEY,
    account_id INT NOT NULL,
    post_id INT NOT NULL,
    created_at DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_FavoritePost_Account FOREIGN KEY (account_id)
        REFERENCES Account(account_id)
        ON DELETE CASCADE,
   CONSTRAINT FK_FavoritePost_Post FOREIGN KEY (post_id)
		REFERENCES Post(post_id)
		ON DELETE NO ACTION,
    CONSTRAINT UQ_FavoritePost_Account_Post UNIQUE (account_id, post_id)
);

CREATE TABLE PromoCode (
    promo_id INT IDENTITY(1,1) PRIMARY KEY,
    code NVARCHAR(50) NOT NULL UNIQUE,
    description NVARCHAR(255),
    discount_percent DECIMAL(5,2) NULL,
    discount_amount DECIMAL(5,2) NULL,
    duration_days INT NULL, -- s? ngày áp d?ng cho bài dang (ví d?: 15)
    start_date DATETIME,
    end_date DATETIME,
    is_new_user_only BIT DEFAULT 0,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE()
);

INSERT INTO PromoCode (code, description, discount_amount, duration_days, start_date, end_date, is_new_user_only)
VALUES ('FREEPOST15', N'Miễn phí 1 tin thường 15 ngày cho khách hàng mới', 100.00, 15, GETDATE(), DATEADD(YEAR, 1, GETDATE()), 1);

CREATE TABLE PromoUsage (
    promo_usage_id INT IDENTITY(1,1) PRIMARY KEY,
    account_id INT NOT NULL,
    promo_id INT NOT NULL,
    post_id INT NOT NULL, 
    used_at DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_PromoUsage_Account FOREIGN KEY (account_id)
        REFERENCES Account(account_id)
        ON DELETE CASCADE,
    CONSTRAINT FK_PromoUsage_Promo FOREIGN KEY (promo_id)
        REFERENCES PromoCode(promo_id)
        ON DELETE CASCADE,
    CONSTRAINT FK_PromoUsage_Post FOREIGN KEY (post_id)
		REFERENCES Post(post_id)
		ON DELETE NO ACTION
);


CREATE TABLE Payment (
    payment_id INT IDENTITY(1,1) PRIMARY KEY,
    post_package_details_id INT NOT NULL,
    total_price DECIMAL(18, 2) CHECK (total_price >= 0),
    status CHAR(1) CHECK (status IN ('S', 'P', 'F')), -- S=Success, P=Pending, F=Failed
    payment_date DATETIME,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    method_id INT,
    account_id INT,
    CONSTRAINT FK_Payment_Method FOREIGN KEY (method_id)
        REFERENCES PaymentMethod(method_id)
        ON DELETE SET NULL,
    CONSTRAINT FK_Payment_Account FOREIGN KEY (account_id)
        REFERENCES Account(account_id)
        ON DELETE CASCADE,
    CONSTRAINT FK_Payment_PostPackageDetails FOREIGN KEY (post_package_details_id)
        REFERENCES PostPackageDetails(id)
        ON DELETE NO ACTION
);


INSERT INTO AccommodationType (type_name, description)
VALUES 
    (N'Phòng trọ, Nhà trọ', N'Loại hình cho thuê phòng trọ hoặc nhà trọ, phù hợp cho sinh viên, người đi làm'),
    (N'Nhà thuê nguyên căn', N'Cho thuê toàn bộ căn nhà, phù hợp cho gia đình hoặc nhóm người'),
    (N'Căn hộ chung cư', N'Cho thuê căn hộ trong các tòa nhà chung cư, tiện nghi đầy đủ');

INSERT INTO Amenities (amenity_name, iconSvg) 
VALUES (N'Camera', '<svg width="20" height="20" viewBox="0 0 24 24" fill="#000000" xmlns="http://www.w3.org/2000/svg"><path d="M18.15,4.94A2.09,2.09,0,0,0,17,5.2l-8.65,5a2,2,0,0,0-.73,2.74l1.5,2.59a2,2,0,0,0,2.73.74l1.8-1a2.49,2.49,0,0,0,1.16,1V18a2,2,0,0,0,2,2H22V18H16.81V16.27A2.49,2.49,0,0,0,18,12.73l2.53-1.46a2,2,0,0,0,.74-2.74l-1.5-2.59a2,2,0,0,0-1.59-1M6.22,13.17,2,13.87l.75,1.3,2,3.46.75,1.3,2.72-3.3Z" /></svg>'), 
	   (N'Bảo vệ', '<svg width="20" height="20" viewBox="0 0 512 512" version="1.1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" xml:space="preserve" fill="#000000"><path class="st0" d="M429.823,409.401c-11.741-17.577-29.574-27.295-46.322-33.618c-8.41-3.157-16.624-5.492-23.859-7.474 c-7.227-1.965-13.508-3.618-17.725-5.213c-7.384-2.738-15.186-6.289-20.628-10.112c-2.722-1.899-4.81-3.856-6.027-5.517 c-1.225-1.702-1.562-2.878-1.578-3.906c0-7.103,0-15.974,0-27.666c9.447-10.508,23.021-26.794,28.595-52.618 c1.949-0.88,3.873-1.875,5.731-3.166c4.62-3.19,8.542-7.819,11.864-14.116c3.346-6.323,6.306-14.405,9.463-25.463 c1.603-5.607,2.343-10.474,2.343-14.815c0.008-4.999-1.028-9.332-2.911-12.85c-2.482-4.678-6.29-7.457-9.701-9.002l7.703-11.478 v-12.727c0.732-0.995,1.2-1.908,1.085-2.631v-27.567l19.6-39.184c4.966-9.932,1.127-22.018-8.666-27.263L265.693,2.43 c-6.05-3.24-13.327-3.24-19.378,0L133.21,63.015c-9.784,5.245-13.624,17.331-8.657,27.263l19.6,39.209v27.543 c-0.123,0.748,0.288,1.66,1.085,2.696v12.662l7.704,11.46c-0.798,0.354-1.62,0.757-2.467,1.275 c-2.664,1.644-5.352,4.218-7.235,7.745c-1.883,3.519-2.919,7.851-2.91,12.85c0,4.341,0.74,9.208,2.343,14.815 c4.226,14.726,8.041,24.238,12.982,31.218c2.475,3.461,5.287,6.24,8.345,8.361c1.858,1.29,3.783,2.286,5.731,3.166 c5.574,25.824,19.148,42.111,28.595,52.618c0,11.692,0,20.562,0,27.666c0,0.872-0.354,2.13-1.702,3.93 c-1.989,2.672-6.108,5.912-11,8.657c-4.876,2.771-10.483,5.18-15.391,6.907c-5.764,2.047-15.054,4.168-25.455,7.153 c-15.629,4.522-34.111,11.033-49.149,23.925c-7.506,6.446-14.092,14.544-18.729,24.607c-4.636,10.056-7.301,22.01-7.292,35.986 c0,3.248,0.14,6.602,0.428,10.072c0.222,2.434,1.142,4.407,2.236,6.034c2.064,3.042,4.81,5.295,8.246,7.622 c6.019,3.979,14.364,7.876,25.043,11.699c31.958,11.396,84.881,21.829,150.449,21.846c53.268,0,98.232-6.914,130.329-15.605 c16.057-4.349,28.875-9.118,38.141-13.878c4.646-2.393,8.394-4.768,11.37-7.358c1.488-1.316,2.795-2.688,3.889-4.325 c1.094-1.627,2.015-3.6,2.228-6.034c0.288-3.461,0.428-6.816,0.428-10.046C442.419,436.114,437.633,421.093,429.823,409.401z M231.936,137.544c0-13.294,10.77-24.065,24.073-24.065c13.286,0,24.056,10.77,24.056,24.065v2.31 c0,13.287-10.77,24.065-24.056,24.065c-13.303,0-24.073-10.778-24.073-24.065V137.544z M236.466,460.639l-54.806-86.517 c4.991-2.088,10.104-4.727,14.84-7.777l31.366,45.104l15.128-28.455L236.466,460.639z M227.117,398.837l-25.273-36.332 c2.31-1.891,4.505-3.897,6.306-6.256c2.54-3.33,4.472-7.49,4.596-12.218l35.386,15.276L227.117,398.837z M212.795,336.887 c0-6.462,0-14.142,0-23.761v-2.77l-1.858-2.072c-9.874-10.993-23.234-25.586-27.871-51.516l-0.732-4.144l-3.955-1.414 c-2.524-0.896-4.439-1.817-6.116-2.984c-2.475-1.743-4.72-4.127-7.342-9.011c-2.59-4.86-5.328-12.119-8.328-22.65 c-1.324-4.604-1.792-8.181-1.792-10.845c0.008-3.083,0.6-4.884,1.2-6.026c0.913-1.653,2.032-2.36,3.454-2.936 c0.642-0.246,1.29-0.378,1.842-0.468l14.873,22.157l6.363-37.285l0.699-2.336c19.42,6.159,44.644,11.1,72.779,11.1 c28.06,0,53.366-4.9,72.794-11.041l0.674,2.277l6.364,37.285l14.873-22.141c0.937,0.148,2.195,0.46,3.166,1.102 c0.83,0.535,1.504,1.151,2.121,2.286c0.6,1.142,1.192,2.943,1.209,6.026c0,2.664-0.477,6.24-1.792,10.845 c-3.988,14.059-7.564,22.231-10.853,26.77c-1.644,2.293-3.149,3.724-4.818,4.892c-1.677,1.168-3.592,2.089-6.116,2.984 l-3.963,1.414l-0.724,4.144c-4.636,25.93-17.998,40.524-27.871,51.516l-1.858,2.072v2.77c0,9.62,0,17.299,0,23.761l-43.204,18.655 L212.795,336.887z M299.262,344.031c0.107,4.572,1.883,8.707,4.342,12.012c1.841,2.483,4.053,4.636,6.47,6.585l-25.191,36.208 l-21.006-39.53L299.262,344.031z M275.543,460.655l-6.528-77.653l15.12,28.447l31.308-45.022c1.044,0.667,2.089,1.324,3.182,1.949 c3.789,2.154,7.794,3.98,11.798,5.664L275.543,460.655z"></path> </g> </g></svg>'),
	   (N'PCCC', '<svg fill="#000000" width="20" height="20" viewBox="0 0 30 30" version="1.1" xmlns="http://www.w3.org/2000/svg"><path d="M3.769 2.219c0 12.51 2.325 21.765 12.301 27.524 9.74-5.624 12.333-14.98 12.333-27.524-8.027 1.434-16.13 1.605-24.634 0zM15.418 26.291c-3.641-0.127-10.029-10.743-4.945-16.553v0c-1.002 5.844 2.052 6.275 3.534 4.025 1.415-2.147-1.625-6.169 3.935-7.828-2.974 3.633 2.465 13.228 3.773 6.499 3.858 7.819-2.473 13.99-6.298 13.857zM19.736 21.46c0 2.845-1.681 5.152-3.755 5.152s-3.755-2.306-3.755-5.152 1.681-5.152 3.755-5.152 3.755 2.306 3.755 5.152z"></path></svg>'),
	   (N'Thang máy', '<svg fill="#000000" width="20" height="20" viewBox="0 0 38 38" version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" viewBox="0 0 37.383 49.85" xml:space="preserve" stroke="#000000"><path d="M0,17.307v30.678c0,1.028,0.827,1.865,1.846,1.865h15.738V15.441H1.846C0.827,15.441,0,16.278,0,17.307z"></path> <path d="M35.536,15.441H19.798v34.408h15.738c1.019,0,1.847-0.837,1.847-1.864V17.307C37.383,16.278,36.555,15.441,35.536,15.441z"></path> <path d="M6.663,6.128c0.303,0.529,0.801,1.397,1.103,1.927L8.57,9.47c0.308,0.531,0.802,0.531,1.104,0l0.81-1.415 c0.303-0.529,0.801-1.397,1.103-1.927l0.805-1.415c0.308-0.531,0.058-0.962-0.551-0.962h-1.009V0.237 C10.832,0.108,10.725,0,10.599,0H7.651c-0.13,0-0.232,0.107-0.232,0.236v3.515H6.406c-0.606,0-0.856,0.431-0.553,0.962L6.663,6.128 z"></path> <path d="M25.541,6.119h1.012v3.514c0,0.129,0.105,0.233,0.23,0.233h2.947c0.129,0,0.233-0.104,0.233-0.233V6.119h1.013 c0.604,0,0.855-0.436,0.553-0.967l-0.809-1.41c-0.303-0.529-0.802-1.401-1.104-1.933L28.811,0.4c-0.307-0.53-0.801-0.53-1.103,0 l-0.81,1.41c-0.303,0.531-0.801,1.403-1.104,1.933l-0.805,1.41C24.684,5.684,24.932,6.119,25.541,6.119z"></path></svg>'), 
	   (N'Hầm để xe', '<svg fill="#000000" width="20" height="20" viewBox="0 0 15 15" version="1.1" id="parking-garage" xmlns="http://www.w3.org/2000/svg"><path d="M10.5,10.14c-0.6637,0.4788-1.4732,0.7121-2.29,0.66h-1.9V14h-1.9V5h3.92c0.7801-0.0414,1.5484,0.2041,2.16,0.69c0.5779,0.5595,0.875,1.3483,0.81,2.15C11.4042,8.6892,11.1088,9.5388,10.5,10.14z M9,6.9C8.711,6.6881,8.3579,6.5822,8,6.6H6.31v2.65H8c0.3612,0.0191,0.717-0.0947,1-0.32c0.2559-0.2675,0.3867-0.6308,0.36-1C9.4072,7.5493,9.274,7.1684,9,6.9z M14.41,4.21c0.114-0.2486,0.007-0.5427-0.24-0.66L7.5,0.45l-6.71,3.1C0.5387,3.666,0.429,3.9637,0.545,4.215C0.661,4.4663,0.9587,4.576,1.21,4.46l0,0L7.5,1.55l6.29,2.9c0.2486,0.114,0.5427,0.007,0.66-0.24H14.41z"></path></svg>');

INSERT INTO PostPackageType (package_type_name, priority, description)
VALUES 
(N'Tin thường', 1, N'Gói cơ bản, ít lượt liên hệ'),
(N'VIP Bạc', 2, N'Tin ưu tiên cấp thấp, x8 lượt liên hệ'),
(N'VIP Vàng', 3, N'Tin ưu tiên cấp trung, x15 lượt liên hệ'),
(N'VIP Kim Cương', 4, N'Tin nổi bật nhất, x30 lượt liên hệ');

INSERT INTO TimeUnitPackage (time_unit_name, description, data)
VALUES (N'Ngày', N'Tính theo đơn vị ngày', 'ngay'),(N'Tuần', N'Tính theo đơn vị tuần', 'tuan'),(N'Tháng', N'Tính theo đơn vị tháng', 'thang');

INSERT INTO PackagePricing (time_unit_id, package_type_id, duration_value, unit_price, total_price)
VALUES
(1, 1, 7, 2000, 14000),      -- 0% discount
(1, 1, 10, 1900, 19000),     -- ~5%
(1, 1, 15, 1800, 27000),     -- ~10%
(1, 1, 30, 1700, 51000);     -- ~15%

INSERT INTO PackagePricing (time_unit_id, package_type_id, duration_value, unit_price, total_price)
VALUES
(1, 2, 7, 4500, 31500),
(1, 2, 10, 4275, 42750),      -- ~5%
(1, 2, 15, 4050, 60750),      -- ~10%
(1, 2, 30, 3825, 114750);     -- ~15%

INSERT INTO PackagePricing (time_unit_id, package_type_id, duration_value, unit_price, total_price)
VALUES
(1, 3, 7, 7900, 55300),
(1, 3, 10, 7505, 75050),      -- ~5%
(1, 3, 15, 7110, 106650),     -- ~10%
(1, 3, 30, 6715, 201450);     -- ~15%

INSERT INTO PackagePricing (time_unit_id, package_type_id, duration_value, unit_price, total_price)
VALUES
(1, 4, 7, 11700, 81900),
(1, 4, 10, 11115, 111150),    -- ~5%
(1, 4, 15, 10530, 157950),    -- ~10%
(1, 4, 30, 9945, 298350);     -- ~15%

-- Tin thường
INSERT INTO PackagePricing (time_unit_id, package_type_id, duration_value, unit_price, total_price)
VALUES
(2, 1, 1, 14000.00, 14000.00),
(2, 1, 2, 13300.00, 26600.00),
(2, 1, 3, 12600.00, 37800.00),
(2, 1, 4, 11900.00, 47600.00);

-- VIP Bạc
INSERT INTO PackagePricing (time_unit_id, package_type_id, duration_value, unit_price, total_price)
VALUES
(2, 2, 1, 31500.00, 31500.00),
(2, 2, 2, 29925.00, 59850.00),
(2, 2, 3, 28350.00, 85050.00),
(2, 2, 4, 26775.00, 107100.00);

-- VIP Vàng
INSERT INTO PackagePricing (time_unit_id, package_type_id, duration_value, unit_price, total_price)
VALUES
(2, 3, 1, 55300.00, 55300.00),
(2, 3, 2, 52535.00, 105070.00),
(2, 3, 3, 49770.00, 149310.00),
(2, 3, 4, 47005.00, 188020.00);

-- VIP Kim Cương
INSERT INTO PackagePricing (time_unit_id, package_type_id, duration_value, unit_price, total_price)
VALUES
(2, 4, 1, 81900.00, 81900.00),
(2, 4, 2, 77805.00, 155610.00),
(2, 4, 3, 73710.00, 221130.00),
(2, 4, 4, 69615.00, 278460.00);

INSERT INTO PackagePricing (time_unit_id, package_type_id, duration_value, unit_price, total_price)
VALUES
(3, 1, 1, 43350.00, 43350.00),
(3, 1, 2, 40800.00, 81600.00),
(3, 1, 3, 38250.00, 114750.00),
(3, 1, 4, 35700.00, 142800.00);

INSERT INTO PackagePricing (time_unit_id, package_type_id, duration_value, unit_price, total_price)
VALUES
(3, 2, 1, 91800.00, 91800.00),
(3, 2, 2, 86400.00, 172800.00),
(3, 2, 3, 81000.00, 243000.00),
(3, 2, 4, 75600.00, 302400.00);

INSERT INTO PackagePricing (time_unit_id, package_type_id, duration_value, unit_price, total_price)
VALUES
(3, 3, 1, 151088.00, 151088.00),
(3, 3, 2, 142200.00, 284400.00),
(3, 3, 3, 133312.00, 399938.00),
(3, 3, 4, 124425.00, 497700.00);

INSERT INTO PackagePricing (time_unit_id, package_type_id, duration_value, unit_price, total_price)
VALUES
(3, 4, 1, 208845.00, 208845.00),
(3, 4, 2, 196560.00, 393120.00),
(3, 4, 3, 184275.00, 552825.00),
(3, 4, 4, 171990.00, 687960.00);

INSERT INTO Account (username, email, password, is_active, auth_provider, auth_provider_id, role)
VALUES 
('tuan123', 'tuan@gmail.com', '$2a$12$Z4AJnkoMRcybTNPk8HROWuq7l3q7e21AZ/nTabx0XcbTtj9AVrgHO', 'A', 'local', NULL, 'L'),  --123123qwe
('minhtuns231', 'tuanvip231@gmail.com', '$2a$12$Z4AJnkoMRcybTNPk8HROWuq7l3q7e21AZ/nTabx0XcbTtj9AVrgHO', 'A', 'local', NULL, 'L'),
('staff01', 'staff01@fpt.edu.vn', '$2a$12$Z4AJnkoMRcybTNPk8HROWuq7l3q7e21AZ/nTabx0XcbTtj9AVrgHO', 'A', 'local', NULL, 'S'),
('admin01', 'admin@system.com', '$2a$12$Z4AJnkoMRcybTNPk8HROWuq7l3q7e21AZ/nTabx0XcbTtj9AVrgHO', 'A', 'local', NULL, 'A'),
('user01', 'user01@gmail.com', '$2a$12$Z4AJnkoMRcybTNPk8HROWuq7l3q7e21AZ/nTabx0XcbTtj9AVrgHO', 'A', 'local', NULL, 'U');

INSERT INTO UserProfile (first_name, last_name, phone_number, gender, date_of_birth, avatar_url, occupation, address, account_id)
VALUES 
('Minh', 'Tuan', '0941673660', 'M', '2003-11-23', '/images/tuan3.jpg', 'Chủ trọ', 'KTX FPT, Quảng Bình', 1),
('My', 'Hanh', '0987654321', 'F', '1998-11-15', '/images/team-2.jpg', 'Chủ trọ', 'Dương Nội, Hà Đông, Hà Nội', 2),
('Nguyen', 'Lam', '0909090909', 'M', '1995-08-12', '/images/team-3.jpg', 'Nhân viên quản lý', 'FPT Complex, Đà Nẵng', 3),
('Admin', 'System', '0868686868', 'O', '1990-01-01', '/images/team-4.jpg', 'Quản trị viên', 'Hòa Lạc, Hà Nội', 4),
('Lan', 'Nguyen', '0912345678', 'F', '1985-05-05', '/images/person_2.jpg', 'Sinh viên', 'Trần Duy Hưng, Hà Nội', 5);

INSERT INTO Accommodation (
    title, description, ward_name, district_name, province_name, address,
    price, deposit_amount, area, max_occupancy, video_url, status,
    owner_id, type_id
)
VALUES 
(N'Phòng trọ cao cấp gần đại học FPT', N'Phòng trọ rộng rãi, thoáng mát, gần trường và đầy đủ tiện nghi.', 
N'Phường Hòa Hải', N'Quận Ngũ Hành Sơn', N'Đà Nẵng', 
N'12 Nguyễn Văn Thoại', 
3500000, 1000000, 30, 2, null, 'A', 1, 1);

INSERT INTO AccommodationDetails (
    has_kitchen_cabinet, has_air_conditioner, has_refrigerator, has_washing_machine, has_loft,
    furniture_status, bedroom_count, bathroom_count, accommodation_id
)
VALUES 
(1, 1, 1, 0, 1, N'Đầy đủ nội thất', 1, 1, 1);

INSERT INTO AccommodationAmenities (accommodation_id, amenity_id)
VALUES 
(1, 1),
(1, 2),
(1, 3),
(1, 4),
(1, 5);

INSERT INTO Post (
    title, content, current_status, view_count, published_at, accommodation_id, account_id
)
VALUES 
(N'Cho thuê phòng trọ gần FPT Đà Nẵng', 
N'Phòng sạch sẽ, an ninh, có gác lửng, đầy đủ tiện nghi như máy lạnh, tủ lạnh, bếp.', 
'A', 10, GETDATE(), 1, 1);

INSERT INTO AccommodationImage (image_url, caption, accommodation_id)
VALUES
('/images/work-1.jpg', N'Phòng trọ nhìn từ cửa ra vào', 1),
('/images/work-2.jpg', N'Góc nhìn phòng khách', 1),
('/images/work-3.jpg', N'Góc nhìn phòng khách', 1),
('/images/work-4.jpg', N'Góc nhìn phòng khách', 1),
('/images/work-5.jpg', N'Góc nhìn phòng khách', 1),
('/images/work-6.jpg', N'Góc nhìn phòng khách', 1),
('/images/work-7.jpg', N'Góc nhìn phòng khách', 1),
('/images/work-8.jpg', N'Góc nhìn phòng khách', 1)

CREATE TABLE Conversation (
    conversation_id INT IDENTITY(1,1) PRIMARY KEY,
    sender_id INT NOT NULL,     
    receiver_id INT NOT NULL,    
    post_id INT NULL,          
    started_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Conversation_Sender FOREIGN KEY (sender_id) REFERENCES Account(account_id),
    CONSTRAINT FK_Conversation_Receiver FOREIGN KEY (receiver_id) REFERENCES Account(account_id),
    CONSTRAINT FK_Conversation_Post FOREIGN KEY (post_id) REFERENCES Post(post_id)
);

CREATE TABLE Message (
    message_id INT IDENTITY(1,1) PRIMARY KEY,
    conversation_id INT NOT NULL,
    sender_id INT NOT NULL,
    content NVARCHAR(MAX),
    image_url VARCHAR(255),     
    is_read BIT DEFAULT 0,       
    sent_at DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Message_Conversation FOREIGN KEY (conversation_id) REFERENCES Conversation(conversation_id),
    CONSTRAINT FK_Message_Sender FOREIGN KEY (sender_id) REFERENCES Account(account_id)
);

CREATE INDEX IX_Message_ConversationId ON Message(conversation_id);

CREATE INDEX IX_Conversation_Users ON Conversation(sender_id, receiver_id);

CREATE UNIQUE INDEX UX_Conversation_Uniqueness
ON Conversation (sender_id, receiver_id, post_id);

INSERT INTO Conversation (sender_id, receiver_id, post_id)
VALUES (5, 1, 1);

INSERT INTO Conversation (sender_id, receiver_id, post_id)
VALUES (5, 2, null);

CREATE TABLE QuickReplyTemplate (
    template_id INT IDENTITY(1,1) PRIMARY KEY,
    message NVARCHAR(255) NOT NULL,
    is_active BIT DEFAULT 1,
    is_default BIT DEFAULT 1,             
    account_id INT NULL,                  
    target_role NVARCHAR(20) NULL,        
    created_at DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_QuickReplyTemplate_Account FOREIGN KEY (account_id) REFERENCES Account(account_id)
);

INSERT INTO QuickReplyTemplate (message, is_default, account_id, target_role)
VALUES 
(N'Phòng này còn cho thuê không ạ?', 1, NULL, 'tenant'),
(N'Giờ giấc ra vào có tự do không ạ?', 1, NULL, 'tenant'),
(N'Chi phí điện nước tính như thế nào ạ?', 1, NULL, 'tenant'),
(N'Có chỗ để xe không ạ?', 1, NULL, 'tenant'),
(N'Cho nuôi thú cưng không ạ?', 1, NULL, 'tenant');

INSERT INTO QuickReplyTemplate (message, is_default, account_id, target_role)
VALUES 
(N'Phòng vẫn còn, bạn muốn xem phòng khi nào?', 1, NULL, 'landlord'),
(N'Chi phí điện nước theo giá nhà nước nha bạn.', 1, NULL, 'landlord'),
(N'Phòng có chỗ để xe máy và an ninh 24/7.', 1, NULL, 'landlord'),
(N'Giờ giấc ra vào thoải mái, không giới hạn.', 1, NULL, 'landlord'),
(N'Phòng không hỗ trợ nuôi thú cưng nha bạn.', 1, NULL, 'landlord');
