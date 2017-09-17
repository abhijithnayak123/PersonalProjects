-- ============================================================
-- Author:        <Chinar Kulkarni>
-- Create date:   <06/24/2015>
-- Description:   <Added New Columns in 'tCustomer' for ODS> 
-- Rally ID:      <AL-593>
-- ============================================================
IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'DTLocal' AND Object_ID = Object_ID(N'tCustomer'))
	BEGIN 
		ALTER TABLE tCustomer
		ADD DTLocal DATETIME NULL
	END
GO

IF EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'BankId' AND Object_ID = Object_ID(N'tCustomer'))
	BEGIN 
		EXEC sp_rename @objname = 'tCustomer.BankId'
		,@newname = 'HomeBankId'
		,@objtype = 'COLUMN'
	END
GO

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'AlloyBankId' AND Object_ID = Object_ID(N'tCustomer'))
	BEGIN 
		ALTER TABLE tCustomer
		ADD AlloyBankId VARCHAR(40) NULL
	END
GO

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'AlloyBranchId' AND Object_ID = Object_ID(N'tCustomer'))
	BEGIN 
		ALTER TABLE tCustomer
		ADD AlloyBranchId VARCHAR(40) NULL
	END
GO

--IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'OccupationDL' AND Object_ID = Object_ID(N'tCustomer'))
--	BEGIN 
--		ALTER TABLE tCustomer
--		ADD OccupationDL varchar(255) NULL
--	END
--GO

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'EmployerName' AND Object_ID = Object_ID(N'tCustomer'))
	BEGIN 
		ALTER TABLE tCustomer
		ADD EmployerName NVARCHAR(255) NULL
	END
GO

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'EmployerPhone' AND Object_ID = Object_ID(N'tCustomer'))
	BEGIN 
		ALTER TABLE tCustomer
		ADD EmployerPhone NVARCHAR(255) NULL
	END
GO

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'CountryOfCitizenship1' AND Object_ID = Object_ID(N'tCustomer'))
	BEGIN 
		ALTER TABLE tCustomer
		ADD CountryOfCitizenship1 VARCHAR(5) NULL
	END
GO

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'CountryOfCitizenship2' AND Object_ID = Object_ID(N'tCustomer'))
	BEGIN 
		ALTER TABLE tCustomer
		ADD CountryOfCitizenship2 VARCHAR(5) NULL
	END
GO

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'CountryOfBirth' AND Object_ID = Object_ID(N'tCustomer'))
	BEGIN 
		ALTER TABLE tCustomer
		ADD CountryOfBirth VARCHAR(5) NULL
	END
GO

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'LegalCode' AND Object_ID = Object_ID(N'tCustomer'))
	BEGIN 
		ALTER TABLE tCustomer
		ADD LegalCode CHAR(1) NULL
	END
GO