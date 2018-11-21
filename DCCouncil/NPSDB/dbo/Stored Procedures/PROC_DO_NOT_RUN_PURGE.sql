CREATE PROC [dbo].[PROC_DO_NOT_RUN_PURGE]
AS
BEGIN

DELETE FROM dbo.StaffLevelExpenditures
DELETE FROM dbo.Expenditures
DELETE FROM dbo.Staffs
DELETE FROM dbo.OfficeAttributes
DELETE FROM dbo.PurchaseOrders
DELETE FROM dbo.PurchaseOrderImportItems
DELETE FROM dbo.PurchaseOrderImportSummary
DELETE FROM dbo.PurchaseOrderImports
DELETE FROM dbo.PurchaseOrderDescriptions
DELETE FROM dbo.Vendors
DELETE FROM dbo.UserProfiles
--DELETE FROM dbo.Users
DELETE FROM dbo.Budgets
DELETE FROM dbo.Offices


--INSERT INTO dbo.Users
--        ( UserGuid ,
--          Username ,
--          PasswordHash ,
--          IsActive ,
--          IsDeleted ,
--          LastFiscalYearSelectedID ,
--          CreatedDate ,
--          UpdatedDate
--        )
--VALUES  ( NEWID() , -- UserGuid - uniqueidentifier
--          N'siteadmin' , -- Username - nvarchar(50)
--          N'a1e0476879cab2a76cc22c80bbf364dd' , -- PasswordHash - nvarchar(max)
--          1 , -- IsActive - bit
--          0 , -- IsDeleted - bit
--          (SELECT TOP 1 FiscalYearID FROM dbo.FiscalYears) , -- LastFiscalYearSelectedID - bigint
--          GETDATE() , -- CreatedDate - datetime
--          GETDATE()  -- UpdatedDate - datetime
--        )


--INSERT INTO dbo.UserProfiles
--        ( UserID ,
--          FirstName ,
--          LastName ,
--          CreatedDate ,
--          UpdatedDate
--        )
--VALUES  ( (SELECT TOP 1 UserID FROM dbo.Users) , -- UserID - bigint
--          N'Site' , -- FirstName - nvarchar(250)
--          N'Administrator' , -- LastName - nvarchar(250)
--          GETDATE() , -- CreatedDate - datetime
--          GETDATE()  -- UpdatedDate - datetime
--        )


END

