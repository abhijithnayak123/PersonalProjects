--- ===============================================================================
-- Author:		 M.Purna Pushkal
-- Description: Creating the table to store the Government Id mappings
-- Version-One: B-20520: Tech Debt - Moving the Customer Id Mapping to Database from Code
-- ================================================================================

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'tGovtIdMapping')
BEGIN
	CREATE TABLE tGovtIdMapping
	(
		 Id INT IDENTITY(100000000,1) PRIMARY KEY,
		 IdType NVARCHAR(500) NOT NULL,
		 IdTypeValue NVARCHAR(100) NOT NULL,
		 ProviderId INT,
		 DtServerCreate DATETIME NOT NULL,
		 DtserverLastModified DATETIME
	)	 
END
GO

IF NOT EXISTS(SELECT 1 FROM tGovtIdMapping)
BEGIN
	INSERT INTO tGovtIdMapping
	(
		IdType, IdTypeValue, ProviderId, DTServerCreate
	)
	VALUES
	(
		N'DRIVER''S LICENSE',
		N'D',
		602,
		GETDATE()
	),
	(
		N'MILITARY ID',
		N'U',
		602,
		GETDATE()
	),
	(
		N'PASSPORT',
		N'P',
		602,
		GETDATE()
	),
	(
		N'U.S. STATE IDENTITY CARD',
		N'S',
		602,
		GETDATE()
	),
	(
		N'MATRICULA CONSULAR',
		N'M',
		602,
		GETDATE()
	),
	(
		N'PERMANENT RESIDENT CARD',
		N'R',
		602,
		GETDATE()
	),
	(
		N'SSN',
		N'1',
		301,
		GETDATE()
	),
	(
		N'DRIVER''S LICENSE',
		N'1',
		301,
		GETDATE()
	),
	(
		N'EMPLOYMENT AUTHORIZATION CARD (EAD)',
		N'4',
		301,
		GETDATE()
	),
	(
		N'PERMANENT RESIDENT CARD',
		N'5',
		301,
		GETDATE()
	),
	(
		N'MILITARY ID',
		N'7',
		301,
		GETDATE()
	),
	(
		N'PASSPORT',
		N'2',
		301,
		GETDATE()
	),
	(
		N'U.S. STATE IDENTITY CARD',
		N'3',
		301,
		GETDATE()
	),
	(
		N'INSTITUTO FEDERAL ELECTORAL',
		N'8',
		301,
		GETDATE()
	),
	(
		N'LICENCIA DE CONDUCIR',
		N'6',
		301,
		GETDATE()
	),
	(
		N'MATRICULA CONSULAR',
		N'9',
		301,
		GETDATE()
	),
	(
		N'NEW YORK BENEFITS ID',
		N'3',
		301,
		GETDATE()
	),
	(
		N'NEW YORK CITY ID',
		N'3',
		301,
		GETDATE()
	)
END
GO