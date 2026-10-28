/* Sample data for HorseClub (run after club_manager_catalog.sql).
   The MVC app has no Owner screens, so owners must exist before horses can be created.
   Run with: sqlcmd -S localhost -U sa -P <pwd> -C -I -i sample_data.sql */
USE HorseClub;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Owners)
BEGIN
    INSERT INTO dbo.Owners (FullName, Phone, Email, Address) VALUES
        (N'Nguyen Van An',  N'0901234567', N'an.nguyen@example.com',  N'12 Le Loi, District 1, HCMC'),
        (N'Tran Thi Binh',  N'0912345678', N'binh.tran@example.com',  N'45 Tran Hung Dao, Hanoi'),
        (N'Le Hoang Cuong', N'0923456789', N'cuong.le@example.com',   N'8 Bach Dang, Da Nang');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Horses)
BEGIN
    DECLARE @an    UNIQUEIDENTIFIER = (SELECT Id FROM dbo.Owners WHERE FullName = N'Nguyen Van An');
    DECLARE @binh  UNIQUEIDENTIFIER = (SELECT Id FROM dbo.Owners WHERE FullName = N'Tran Thi Binh');
    DECLARE @cuong UNIQUEIDENTIFIER = (SELECT Id FROM dbo.Owners WHERE FullName = N'Le Hoang Cuong');

    -- Sex: 0 Stallion, 1 Mare, 2 Gelding.  Status: 0 Active, 1 Retired, 2 Sold, 3 Deceased.
    INSERT INTO dbo.Horses (Name, MicrochipNo, PassportNo, Sex, Color, Breed, BirthDate, Status, OwnerId) VALUES
        (N'Thunder',  N'985000000000001', N'VN-P-0001', 0, N'Bay',      N'Thoroughbred', '2012-04-10', 1, @an),
        (N'Luna',     N'985000000000002', N'VN-P-0002', 1, N'Grey',     N'Arabian',      '2013-06-21', 0, @binh),
        (N'Shadow',   N'985000000000003', NULL,         2, N'Black',    N'Quarter Horse','2016-09-02', 0, @cuong);

    INSERT INTO dbo.Horses (Name, MicrochipNo, PassportNo, Sex, Color, Breed, BirthDate, Status, OwnerId, SireId, DamId)
    SELECT N'Storm', N'985000000000004', N'VN-P-0004', 0, N'Dark Bay', N'Thoroughbred', '2019-03-15', 0, @an,
           (SELECT Id FROM dbo.Horses WHERE Name = N'Thunder'),
           (SELECT Id FROM dbo.Horses WHERE Name = N'Luna');
END
GO
