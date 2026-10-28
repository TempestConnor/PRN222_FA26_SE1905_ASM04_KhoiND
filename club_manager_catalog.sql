/* =====================================================================
   Club Manager - Master Catalog schema (Microsoft SQL Server)
   Scope: Horses, Owners, Staff, Supply catalog (feed + medical)
   Conventions:
     - GUID primary keys (NEWSEQUENTIALID to limit index fragmentation)
     - Soft delete via IsDeleted / DeletedAt
     - Audit columns on every table
     - Enums stored as TINYINT with CHECK constraints
   ===================================================================== */

IF DB_ID(N'HorseClub') IS NULL
    CREATE DATABASE HorseClub;
GO
USE HorseClub;
GO

/* ---------------------------------------------------------------------
   Owners
   UserId: link to the Identity user (AspNetUsers.Id, Guid key) if owners
   log in. FK is added after ASP.NET Core Identity tables exist.
   --------------------------------------------------------------------- */
CREATE TABLE dbo.Owners (
    Id          UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_Owners_Id DEFAULT NEWSEQUENTIALID(),
    FullName    NVARCHAR(200)    NOT NULL,
    Phone       NVARCHAR(30)     NULL,
    Email       NVARCHAR(256)    NULL,
    Address     NVARCHAR(500)    NULL,
    UserId      UNIQUEIDENTIFIER NULL,
    CreatedAt   DATETIME2(0)     NOT NULL CONSTRAINT DF_Owners_CreatedAt DEFAULT SYSUTCDATETIME(),
    CreatedBy   NVARCHAR(256)    NULL,
    UpdatedAt   DATETIME2(0)     NULL,
    UpdatedBy   NVARCHAR(256)    NULL,
    IsDeleted   BIT              NOT NULL CONSTRAINT DF_Owners_IsDeleted DEFAULT 0,
    DeletedAt   DATETIME2(0)     NULL,
    CONSTRAINT PK_Owners PRIMARY KEY CLUSTERED (Id)
);
GO

CREATE UNIQUE INDEX UX_Owners_UserId
    ON dbo.Owners (UserId)
    WHERE UserId IS NOT NULL AND IsDeleted = 0;
GO

/* ---------------------------------------------------------------------
   Horses
   Sex:    0 = Stallion, 1 = Mare, 2 = Gelding
   Status: 0 = Active, 1 = Retired, 2 = Sold, 3 = Deceased
   --------------------------------------------------------------------- */
CREATE TABLE dbo.Horses (
    Id           UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_Horses_Id DEFAULT NEWSEQUENTIALID(),
    Name         NVARCHAR(150)    NOT NULL,
    MicrochipNo  NVARCHAR(30)     NOT NULL,
    PassportNo   NVARCHAR(50)     NULL,
    Sex          TINYINT          NOT NULL,
    Color        NVARCHAR(50)     NULL,
    Breed        NVARCHAR(100)    NULL,
    BirthDate    DATE             NOT NULL,
    Status       TINYINT          NOT NULL CONSTRAINT DF_Horses_Status DEFAULT 0,
    OwnerId      UNIQUEIDENTIFIER NOT NULL,
    SireId       UNIQUEIDENTIFIER NULL,
    DamId        UNIQUEIDENTIFIER NULL,
    CreatedAt    DATETIME2(0)     NOT NULL CONSTRAINT DF_Horses_CreatedAt DEFAULT SYSUTCDATETIME(),
    CreatedBy    NVARCHAR(256)    NULL,
    UpdatedAt    DATETIME2(0)     NULL,
    UpdatedBy    NVARCHAR(256)    NULL,
    IsDeleted    BIT              NOT NULL CONSTRAINT DF_Horses_IsDeleted DEFAULT 0,
    DeletedAt    DATETIME2(0)     NULL,
    CONSTRAINT PK_Horses PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_Horses_Owners FOREIGN KEY (OwnerId) REFERENCES dbo.Owners (Id),
    CONSTRAINT FK_Horses_Sire   FOREIGN KEY (SireId)  REFERENCES dbo.Horses (Id),
    CONSTRAINT FK_Horses_Dam    FOREIGN KEY (DamId)   REFERENCES dbo.Horses (Id),
    CONSTRAINT CK_Horses_Sex    CHECK (Sex    IN (0, 1, 2)),
    CONSTRAINT CK_Horses_Status CHECK (Status IN (0, 1, 2, 3)),
    CONSTRAINT CK_Horses_NotOwnParent CHECK (SireId <> Id AND DamId <> Id)
);
GO

CREATE UNIQUE INDEX UX_Horses_MicrochipNo
    ON dbo.Horses (MicrochipNo)
    WHERE IsDeleted = 0;

CREATE UNIQUE INDEX UX_Horses_PassportNo
    ON dbo.Horses (PassportNo)
    WHERE PassportNo IS NOT NULL AND IsDeleted = 0;

CREATE INDEX IX_Horses_OwnerId ON dbo.Horses (OwnerId);
CREATE INDEX IX_Horses_SireId  ON dbo.Horses (SireId) WHERE SireId IS NOT NULL;
CREATE INDEX IX_Horses_DamId   ON dbo.Horses (DamId)  WHERE DamId  IS NOT NULL;
CREATE INDEX IX_Horses_Name    ON dbo.Horses (Name);
GO

/* ---------------------------------------------------------------------
   StaffProfiles
   Login and role (RBAC) live in ASP.NET Core Identity.
   JobTitle: 0 = HeadTrainer, 1 = Veterinarian, 2 = Groom, 3 = ClubManager
   UserId: Identity user (AspNetUsers.Id, Guid key); FK added after
   Identity tables exist.
   --------------------------------------------------------------------- */
CREATE TABLE dbo.StaffProfiles (
    Id          UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_StaffProfiles_Id DEFAULT NEWSEQUENTIALID(),
    UserId      UNIQUEIDENTIFIER NOT NULL,
    FullName    NVARCHAR(200)    NOT NULL,
    Phone       NVARCHAR(30)     NULL,
    JobTitle    TINYINT          NOT NULL,
    HireDate    DATE             NULL,
    IsActive    BIT              NOT NULL CONSTRAINT DF_StaffProfiles_IsActive DEFAULT 1,
    CreatedAt   DATETIME2(0)     NOT NULL CONSTRAINT DF_StaffProfiles_CreatedAt DEFAULT SYSUTCDATETIME(),
    CreatedBy   NVARCHAR(256)    NULL,
    UpdatedAt   DATETIME2(0)     NULL,
    UpdatedBy   NVARCHAR(256)    NULL,
    IsDeleted   BIT              NOT NULL CONSTRAINT DF_StaffProfiles_IsDeleted DEFAULT 0,
    DeletedAt   DATETIME2(0)     NULL,
    CONSTRAINT PK_StaffProfiles PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT CK_StaffProfiles_JobTitle CHECK (JobTitle IN (0, 1, 2, 3))
);
GO

CREATE UNIQUE INDEX UX_StaffProfiles_UserId
    ON dbo.StaffProfiles (UserId)
    WHERE IsDeleted = 0;

CREATE INDEX IX_StaffProfiles_JobTitle ON dbo.StaffProfiles (JobTitle);
GO

/* ---------------------------------------------------------------------
   Suppliers
   --------------------------------------------------------------------- */
CREATE TABLE dbo.Suppliers (
    Id          UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_Suppliers_Id DEFAULT NEWSEQUENTIALID(),
    Name        NVARCHAR(200)    NOT NULL,
    Phone       NVARCHAR(30)     NULL,
    Email       NVARCHAR(256)    NULL,
    CreatedAt   DATETIME2(0)     NOT NULL CONSTRAINT DF_Suppliers_CreatedAt DEFAULT SYSUTCDATETIME(),
    CreatedBy   NVARCHAR(256)    NULL,
    UpdatedAt   DATETIME2(0)     NULL,
    UpdatedBy   NVARCHAR(256)    NULL,
    IsDeleted   BIT              NOT NULL CONSTRAINT DF_Suppliers_IsDeleted DEFAULT 0,
    DeletedAt   DATETIME2(0)     NULL,
    CONSTRAINT PK_Suppliers PRIMARY KEY CLUSTERED (Id)
);
GO

CREATE UNIQUE INDEX UX_Suppliers_Name
    ON dbo.Suppliers (Name)
    WHERE IsDeleted = 0;
GO

/* ---------------------------------------------------------------------
   SupplyCategories
   Kind: 0 = Feed, 1 = Medical, 2 = Equipment
   --------------------------------------------------------------------- */
CREATE TABLE dbo.SupplyCategories (
    Id          UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_SupplyCategories_Id DEFAULT NEWSEQUENTIALID(),
    Name        NVARCHAR(100)    NOT NULL,
    Kind        TINYINT          NOT NULL,
    CreatedAt   DATETIME2(0)     NOT NULL CONSTRAINT DF_SupplyCategories_CreatedAt DEFAULT SYSUTCDATETIME(),
    CreatedBy   NVARCHAR(256)    NULL,
    UpdatedAt   DATETIME2(0)     NULL,
    UpdatedBy   NVARCHAR(256)    NULL,
    IsDeleted   BIT              NOT NULL CONSTRAINT DF_SupplyCategories_IsDeleted DEFAULT 0,
    DeletedAt   DATETIME2(0)     NULL,
    CONSTRAINT PK_SupplyCategories PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT CK_SupplyCategories_Kind CHECK (Kind IN (0, 1, 2))
);
GO

CREATE UNIQUE INDEX UX_SupplyCategories_Kind_Name
    ON dbo.SupplyCategories (Kind, Name)
    WHERE IsDeleted = 0;
GO

/* ---------------------------------------------------------------------
   SupplyItems  (catalog only; stock quantities belong to a later
   Inventory module)
   Kind is kept on the item for fast filtering and must match its
   category's Kind (enforce in application code).
   --------------------------------------------------------------------- */
CREATE TABLE dbo.SupplyItems (
    Id            UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_SupplyItems_Id DEFAULT NEWSEQUENTIALID(),
    CategoryId    UNIQUEIDENTIFIER NOT NULL,
    SupplierId    UNIQUEIDENTIFIER NULL,
    Code          NVARCHAR(50)     NOT NULL,
    Name          NVARCHAR(200)    NOT NULL,
    Kind          TINYINT          NOT NULL,
    Unit          NVARCHAR(20)     NOT NULL,       -- kg, ml, dose, piece...
    ReorderLevel  DECIMAL(18, 3)   NOT NULL CONSTRAINT DF_SupplyItems_Reorder DEFAULT 0,
    Notes         NVARCHAR(1000)   NULL,
    CreatedAt     DATETIME2(0)     NOT NULL CONSTRAINT DF_SupplyItems_CreatedAt DEFAULT SYSUTCDATETIME(),
    CreatedBy     NVARCHAR(256)    NULL,
    UpdatedAt     DATETIME2(0)     NULL,
    UpdatedBy     NVARCHAR(256)    NULL,
    IsDeleted     BIT              NOT NULL CONSTRAINT DF_SupplyItems_IsDeleted DEFAULT 0,
    DeletedAt     DATETIME2(0)     NULL,
    CONSTRAINT PK_SupplyItems PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_SupplyItems_Categories FOREIGN KEY (CategoryId) REFERENCES dbo.SupplyCategories (Id),
    CONSTRAINT FK_SupplyItems_Suppliers  FOREIGN KEY (SupplierId) REFERENCES dbo.Suppliers (Id),
    CONSTRAINT CK_SupplyItems_Kind    CHECK (Kind IN (0, 1, 2)),
    CONSTRAINT CK_SupplyItems_Reorder CHECK (ReorderLevel >= 0)
);
GO

CREATE UNIQUE INDEX UX_SupplyItems_Code
    ON dbo.SupplyItems (Code)
    WHERE IsDeleted = 0;

CREATE INDEX IX_SupplyItems_CategoryId ON dbo.SupplyItems (CategoryId);
CREATE INDEX IX_SupplyItems_SupplierId ON dbo.SupplyItems (SupplierId) WHERE SupplierId IS NOT NULL;
CREATE INDEX IX_SupplyItems_Kind_Name  ON dbo.SupplyItems (Kind, Name);
GO

/* ---------------------------------------------------------------------
   Seed reference data
   --------------------------------------------------------------------- */
INSERT INTO dbo.SupplyCategories (Name, Kind) VALUES
    (N'Grain',        0),
    (N'Hay & Forage', 0),
    (N'Vitamins & Supplements', 0),
    (N'Medication',   1),
    (N'Vaccine',      1),
    (N'Dewormer',     1),
    (N'Wound Care',   1),
    (N'Grooming Tools', 2),
    (N'Tack',         2);
GO

/* ---------------------------------------------------------------------
   After creating ASP.NET Core Identity tables (Guid keys), add:

   ALTER TABLE dbo.Owners
       ADD CONSTRAINT FK_Owners_AspNetUsers
       FOREIGN KEY (UserId) REFERENCES dbo.AspNetUsers (Id);

   ALTER TABLE dbo.StaffProfiles
       ADD CONSTRAINT FK_StaffProfiles_AspNetUsers
       FOREIGN KEY (UserId) REFERENCES dbo.AspNetUsers (Id);
   --------------------------------------------------------------------- */
