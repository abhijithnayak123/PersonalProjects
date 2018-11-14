--- ===============================================================================
-- Author     :	 Nitish Biradar
-- Description:  Contact information displayed in messages
-- Creatd Date:  03-04-2018
-- Story Id   :  B-13192
-- ================================================================================


IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tSupportInformation' AND COLUMN_NAME = 'Sequence')
BEGIN
	ALTER TABLE tSupportInformation
	ADD Sequence INT NULL
END
GO

UPDATE 
	tSupportInformation 
SET 
	Sequence = '1'
WHERE 
	ContactType = 'ITServiceDesk'


UPDATE 
	tSupportInformation 
SET 
	Sequence = '2'
WHERE 
	ContactType = 'BSA'


UPDATE 
	tSupportInformation 
SET 
	Sequence = '3'
WHERE 
	ContactType = 'VISA'


UPDATE 
	tSupportInformation 
SET 
	Sequence = '4'
WHERE 
	ContactType = 'WU'


UPDATE 
	tSupportInformation 
SET 
	Sequence = '5'
WHERE 
	ContactType = 'INGO'