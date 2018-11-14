-- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <06-01-2018>
-- Description:	Altering the tRCIF_Credentials table to add RCIFFinalCommitURL. 
-- ================================================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tRCIF_Credential' AND COLUMN_NAME = 'RCIFFinalCommitURL')
BEGIN
    ALTER TABLE tRCIF_Credential
	ADD RCIFFinalCommitURL NVARCHAR(MAX)
END
GO

--Updating the RCIF Final Commit URL.
UPDATE tRCIF_Credential
SET RCIFFinalCommitURL = 'https://hera.tcfbank.com:2008/CICSTST2/CIF7454'
WHERE ChannelPartnerId = 34
GO