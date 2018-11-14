-- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <06-01-2018>
-- Description:	Altering the tRCIF_Credentials table to add Customer registration URLs. 
-- ================================================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tRCIF_Credential' AND COLUMN_NAME = 'EWSPreScanURL')
BEGIN
    ALTER TABLE tRCIF_Credential
	ADD EWSPreScanURL NVARCHAR(MAX)
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tRCIF_Credential' AND COLUMN_NAME = 'RCIFCustomerRegURL')
BEGIN
    ALTER TABLE tRCIF_Credential
	ADD RCIFCustomerRegURL NVARCHAR(MAX)
END
GO

--Updating the EWS Pre Scan URL.
UPDATE tRCIF_Credential
SET EWSPreScanURL = 'https://tellertest.tcfbank.com/customerws/customerws.asmx'
WHERE ChannelPartnerId = 34

--Updating the RCIF Customer Reg URL.
UPDATE tRCIF_Credential
SET RCIFCustomerRegURL = 'https://hera.tcfbank.com:2008/CICSTST2/CIF7450'
WHERE ChannelPartnerId = 34
GO