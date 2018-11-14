-- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <05-25-2018>
-- Description:	Altering the tRCIF_Credentials table to add TellerInquiryURL. 
-- ================================================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tRCIF_Credential' AND COLUMN_NAME = 'TellerInquiryURL')
BEGIN
    ALTER TABLE tRCIF_Credential
	ADD TellerInquiryURL NVARCHAR(MAX)
END
GO

--Updating the Teller Inquiry URL.
UPDATE tRCIF_Credential
SET TellerInquiryURL = 'https://hera.tcfbank.com:2008/CICSBA51/TEL7770'
WHERE ChannelPartnerId = 34
GO