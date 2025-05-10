create DATABASE RentNestSystem;
GO

USE RentNestSystem;
GO

-- Table: Account
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

-- Table: UserProfile
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

-- Table: PaymentMethod
CREATE TABLE PaymentMethod (
    method_id INT IDENTITY(1,1) PRIMARY KEY,
    method_name VARCHAR(100) not null,
    description NVARCHAR(MAX),
    is_active BIT DEFAULT 1,
    icon_url VARCHAR(255)
);

-- Table: Payment
CREATE TABLE Payment (
    payment_id INT IDENTITY(1,1) PRIMARY KEY,
    total_price DECIMAL(18, 2) CHECK (total_price >= 0),
    purpose VARCHAR(255) CHECK (purpose in ('post_package', 'subscription', 'promotion', 'other')), 
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
        ON DELETE CASCADE
);

-- Table: PaymentItem
CREATE TABLE PaymentItem (
    item_id INT IDENTITY(1,1) PRIMARY KEY,
    item_type VARCHAR(100) CHECK (item_type in ('post_package', 'subscription', 'promotion', 'other')) NOT NULL,
    amount DECIMAL(18, 2) CHECK (amount >= 0),
    payment_id INT,
    CONSTRAINT FK_PaymentItem_Payment FOREIGN KEY (payment_id)
        REFERENCES Payment(payment_id)
        ON DELETE CASCADE
);

-- Table: AccommodationType
CREATE TABLE AccommodationType (
    type_id INT IDENTITY(1,1) PRIMARY KEY,
    type_name VARCHAR(100) NOT NULL UNIQUE,
    description VARCHAR(255)
);

-- Table: Accommodation
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

-- Table: AccommodationDetails
CREATE TABLE AccommodationDetails (
    detail_id INT IDENTITY(1,1) PRIMARY KEY,
    has_kitchen BIT DEFAULT 0,
    has_air_conditioner BIT DEFAULT 0,
    has_parking BIT DEFAULT 0,
    has_security BIT DEFAULT 0,
    has_elevator BIT DEFAULT 0,
    furniture_status VARCHAR(100) ,
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

-- Table: AccommodationImage
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

-- Table: PostPackage
CREATE TABLE PostPackage (
    package_id INT IDENTITY(1,1) PRIMARY KEY,
    package_name VARCHAR(100) NOT NULL,
    description VARCHAR(255),
    price DECIMAL(10, 2) CHECK (price >= 0) NOT NULL,
    duration_days INT CHECK (duration_days > 0),
    image VARCHAR(255),
    is_active BIT DEFAULT 1,
    is_featured BIT DEFAULT 0,
    is_priority BIT DEFAULT 0,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE()
);

-- Table: Post
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

-- Table: PostPackageDetails
CREATE TABLE PostPackageDetails (
    id INT IDENTITY(1,1) PRIMARY KEY,
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    actual_price DECIMAL(10, 2) CHECK (actual_price >= 0),
    post_id INT NOT NULL,
    post_package_id INT NOT NULL,
    CONSTRAINT FK_PostPackageDetails_Post FOREIGN KEY (post_id)
        REFERENCES Post(post_id)
        ON DELETE CASCADE,
    CONSTRAINT FK_PostPackageDetails_Package FOREIGN KEY (post_package_id)
        REFERENCES PostPackage(package_id)
        ON DELETE CASCADE
);

-- Table: PostApprovals
CREATE TABLE PostApprovals (
    approval_id INT IDENTITY(1,1) PRIMARY KEY,
    status CHAR(1)  NOT NULL DEFAULT 'P' CHECK (status IN ('P', 'A', 'R')), -- A=Approved, R=Rejected, P=Pending
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