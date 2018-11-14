--- ===============================================================================
-- Author     :	 Nitish Biradar
-- Description:  Contact information displayed in messages
-- Creatd Date:  27-03-2018
-- Story Id   :  B-13192
-- ================================================================================


IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tSupportInformation' AND COLUMN_NAME = 'ContactType')
BEGIN
	ALTER TABLE tSupportInformation
	ADD ContactType NVARCHAR(50) NULL
END
GO


IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tMessageStore' AND COLUMN_NAME = 'ContactType')
BEGIN
	ALTER TABLE tMessageStore
	ADD ContactType NVARCHAR(50) NULL
END
GO

IF EXISTS(SELECT 1 FROM tSupportInformation WHERE ContactType IS NULL)
BEGIN
	UPDATE 
		tSupportInformation 
	SET 
		ContactType = 'RBS'
END

IF NOT EXISTS(SELECT 1 FROM tSupportInformation WHERE ContactType = 'ITServiceDesk')
BEGIN
	INSERT INTO tSupportInformation
	(
	    Phone1,
	    Phone2,
	    DTServerCreate,
		ContactType
	)
	VALUES
	(
	    N'1-800-TCF-DESK(823-3375)', 
	    NULL, 
		GETDATE(),
		'ITServiceDesk'
	) 
END

IF NOT EXISTS(SELECT 1 FROM tSupportInformation WHERE ContactType = 'VISA')
BEGIN
	INSERT INTO tSupportInformation
	(
	    Phone1,
	    Phone2,
	    DTServerCreate,
		ContactType
	)
	VALUES
	(
	    N'855-477-1135', 
	    NULL, 
		GETDATE(),
		'VISA'
	) 
END

IF NOT EXISTS(SELECT 1 FROM tSupportInformation WHERE ContactType = 'WU')
BEGIN
	INSERT INTO tSupportInformation
	(
	    Phone1,
	    Phone2,
	    DTServerCreate,
		ContactType
	)
	VALUES
	(
	    N'1-800-325-6000', 
	    NULL, 
		GETDATE(),
		'WU'
	) 
END

IF NOT EXISTS(SELECT 1 FROM tSupportInformation WHERE ContactType = 'BSA')
BEGIN
	INSERT INTO tSupportInformation
	(
	    Phone1,
	    Phone2,
	    DTServerCreate,
		ContactType
	)
	VALUES
	(
	    N'763-337-7881', 
	    NULL, 
		GETDATE(),
		'BSA'
	) 
END

IF NOT EXISTS(SELECT 1 FROM tSupportInformation WHERE ContactType = 'INGO')
BEGIN
	INSERT INTO tSupportInformation
	(
	    Phone1,
	    Phone2,
	    DTServerCreate,
		ContactType
	)
	VALUES
	(
	    N'Use the chat option for Ingo or call RBS for assistance', 
	    NULL, 
		GETDATE(),
		'INGO'
	) 
END