-- Alter script for X9 tables

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartner_X9_Audit_Detail' AND COLUMN_NAME = 'ItemId')
BEGIN
    ALTER TABLE tChannelPartner_X9_Audit_Detail 
	ADD ItemId BIGINT NULL
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartner_X9_Audit_Detail' AND COLUMN_NAME = 'ItemPK')
BEGIN
    ALTER TABLE tChannelPartner_X9_Audit_Detail 
	ALTER COLUMN ItemPK UNIQUEIDENTIFIER NULL
END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartner_X9_Audit_Header' AND COLUMN_NAME ='ChannelPartnerPK')
BEGIN
	ALTER TABLE tChannelPartner_X9_Audit_Header
	ALTER COLUMN ChannelPartnerPK UNIQUEIDENTIFIER NULL
END
GO

-- Migration script for X9 tables

UPDATE 
	dbo.tChannelPartner_X9_Audit_Header
SET
    ChannelPartnerID = tcp.ChannelPartnerId
FROM 
	dbo.tChannelPartner_X9_Audit_Header tcpxah 
	INNER JOIN dbo.tChannelPartners tcp ON tcpxah.ChannelPartnerPK = tcp.ChannelPartnerPK

UPDATE 
	tChannelPartner_X9_Audit_Detail 
SET 
	 itemId = ttmo.TransactionID
FROM 
	dbo.tTxn_MoneyOrder ttmo 
	INNER JOIN dbo.tChannelPartner_X9_Audit_Detail tcpxad ON ttmo.TxnPK = tcpxad.ItemPK
WHERE 
	tcpxad.ItemType = 'MoneyOrder'

UPDATE 
	tChannelPartner_X9_Audit_Detail
SET 
	 itemId = ttc.TransactionID
FROM 
	dbo.tTxn_Check ttc 
	INNER JOIN dbo.tChannelPartner_X9_Audit_Detail tcpxad ON ttc.TxnPK = tcpxad.ItemPK
WHERE 
	tcpxad.ItemType = 'Check'

--- Alter Tables

IF EXISTS (
	SELECT 1 
	FROM INFORMATION_SCHEMA.COLUMNS 
	WHERE TABLE_NAME = 'tChannelPartner_X9_Audit_Detail' 
	AND COLUMN_NAME = 'ItemId'
)
BEGIN
	ALTER TABLE 
		tChannelPartner_X9_Audit_Detail 
	ALTER COLUMN 
		ItemId BIGINT NOT NULL
END

IF EXISTS  (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartner_X9_Audit_Header_tChannelPartners]')
		 AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartner_X9_Audit_Header]'))
BEGIN
	ALTER TABLE [dbo].[tChannelPartner_X9_Audit_Header] 
	DROP CONSTRAINT FK_tChannelPartner_X9_Audit_Header_tChannelPartners
END
GO

IF EXISTS (
	SELECT 1 
	FROM INFORMATION_SCHEMA.COLUMNS 
	WHERE TABLE_NAME = 'tChannelPartner_X9_Audit_Header' 
	AND COLUMN_NAME = 'ChannelPartnerID'
)
BEGIN
	ALTER TABLE 
		tChannelPartner_X9_Audit_Header 
	ALTER COLUMN 
		ChannelPartnerID smallint NOT NULL
END

IF NOT EXISTS  (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartner_X9_Audit_Header_tChannelPartners]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[tChannelPartner_X9_Audit_Header]'))
BEGIN
    ALTER TABLE [dbo].[tChannelPartner_X9_Audit_Header]  
	WITH CHECK ADD  CONSTRAINT [FK_tChannelPartner_X9_Audit_Header_tChannelPartners] 
	FOREIGN KEY([ChannelPartnerID])
	REFERENCES [dbo].[tChannelPartners] ([ChannelPartnerId])
END
GO