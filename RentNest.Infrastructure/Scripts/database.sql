USE RentNestSystem;

CREATE TABLE Account (
    account_id INT IDENTITY(1,1) PRIMARY KEY,
    username NVARCHAR(100) NULL,
    email NVARCHAR(255) NOT NULL UNIQUE,
    password NVARCHAR(255) NULL,
    is_active CHAR(1) NOT NULL DEFAULT 'A' CHECK (is_active IN ('A', 'D')),  --A is active, D is disabled
    auth_provider VARCHAR(20) NOT NULL CHECK (auth_provider IN ('local', 'google', 'facebook')),
    auth_provider_id VARCHAR(255) UNIQUE, -- nullable only for 'local'
    created_at DATETIME NOT NULL DEFAULT GETDATE(),
    updated_at DATETIME NOT NULL DEFAULT GETDATE(),
    role CHAR(1) NOT NULL CHECK (role IN ('U', 'A', 'S', 'L')) -- U=User, S = Staff, A=Admin, L=Landlord
);

CREATE UNIQUE INDEX UX_Account_Username
ON Account (Username)
WHERE Username IS NOT NULL;

CREATE TABLE AccommodationType (
    type_id INT IDENTITY(1,1) PRIMARY KEY,
    type_name VARCHAR(100) NOT NULL UNIQUE,
    description VARCHAR(255)
);

CREATE TABLE PaymentMethod (
    method_id INT IDENTITY(1,1) PRIMARY KEY,
    method_name VARCHAR(100) not null,
    description NVARCHAR(MAX),
    is_active BIT DEFAULT 1,
    icon_url VARCHAR(255)
);

CREATE TABLE TimeUnitPackage (
    time_unit_id INT IDENTITY(1,1) PRIMARY KEY,
    time_unit_name VARCHAR(20) NOT NULL CHECK (time_unit_name IN ('Ngày', 'Tuần', 'Tháng')),
    description NVARCHAR(255)
);

CREATE TABLE PostPackageType (
    package_type_id INT IDENTITY(1,1) PRIMARY KEY,
    package_type_name VARCHAR(100) NOT NULL, -- Tin thường, VIP Đồng, VIP Bạc, VIP Vàng, VIP Kim Cương
    priority INT NOT NULL,
    description NVARCHAR(255)
);

CREATE TABLE UserProfile (
    profile_id INT IDENTITY(1,1) PRIMARY KEY,
    first_name NVARCHAR(100),
    last_name NVARCHAR(100),
    gender CHAR(1) CHECK (gender IN ('M', 'F', 'O')), -- M=Male, F=Female, O=Other
    date_of_birth DATE,
    avatar_url VARCHAR(255),
    occupation VARCHAR(100),
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
    title VARCHAR(150) NOT NULL,
    description VARCHAR(MAX),
    address VARCHAR(255) NOT NULL,
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

-- 4. Tạo bảng giá gói tin
CREATE TABLE PackagePricing (
    pricing_id INT IDENTITY(1,1) PRIMARY KEY,
    time_unit_id INT NOT NULL,
    package_type_id INT NOT NULL,
    duration_value INT CHECK (duration_value > 0) NOT NULL, -- Số lượng thời gian (1, 2, 3,...)
    unit_price DECIMAL(10,2) CHECK (unit_price >= 0) NOT NULL, -- Giá tiền cho mỗi đơn vị
    total_price DECIMAL(10,2) CHECK (total_price >= 0) NOT NULL, -- Tổng giá trọn gói (có thể có giảm giá)
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
    has_kitchen BIT DEFAULT 0,
    has_air_conditioner BIT DEFAULT 0,
    has_parking BIT DEFAULT 0,
    has_security BIT DEFAULT 0,
    has_elevator BIT DEFAULT 0,
    furniture_status VARCHAR(100),
    is_pets_allowed BIT DEFAULT 0,
    is_smoking_allowed BIT DEFAULT 0,
    has_washing_machine BIT DEFAULT 0,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    accommodation_id INT UNIQUE NOT NULL,
    CONSTRAINT FK_AccommodationDetails_Accommodation FOREIGN KEY (accommodation_id)
        REFERENCES Accommodation(accommodation_id)
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
    current_status CHAR(1) NOT NULL DEFAULT 'P' CHECK (current_status IN ('P', 'A', 'R')), -- P=Pending, A=Approved, R=Rejected
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
    pricing_id INT NOT NULL, -- Liên kết với bảng PackagePricing
    total_price DECIMAL(10, 2) CHECK (total_price >= 0) NOT NULL, -- Giá thực tế trả
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
    status CHAR(1) NOT NULL DEFAULT 'P' CHECK (status IN ('P', 'A', 'R')), -- A=Approved, R=Rejected, P=Pending
    rejection_reason NVARCHAR(255),
    note VARCHAR(255),
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
    duration_days INT NULL, -- số ngày áp dụng cho bài đăng (ví dụ: 15)
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