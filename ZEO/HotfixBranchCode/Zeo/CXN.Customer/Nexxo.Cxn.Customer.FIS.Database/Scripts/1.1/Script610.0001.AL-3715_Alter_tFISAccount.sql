-- ================================================================================
-- Author:		<Abhijith Nayak>
-- Create date: <02/09/2016>
-- Description:	<Synovus: As Synovus, I want an exception report for any customer whose MISC account creation failed>
-- Jira ID:		<AL-3715>
-- ================================================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tFIS_Account' AND COLUMN_NAME = 'IsCISSuccess')
BEGIN
	ALTER TABLE tFIS_Account 
	ADD IsCISSuccess BIT NULL
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tFIS_Account' AND COLUMN_NAME = 'IsCNECTSuccess')
BEGIN
	ALTER TABLE tFIS_Account 
	ADD IsCNECTSuccess BIT NULL
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tFIS_Account' AND COLUMN_NAME = 'IsPREPDSuccess')
BEGIN
	ALTER TABLE tFIS_Account 
	ADD IsPREPDSuccess BIT NULL
END
GO