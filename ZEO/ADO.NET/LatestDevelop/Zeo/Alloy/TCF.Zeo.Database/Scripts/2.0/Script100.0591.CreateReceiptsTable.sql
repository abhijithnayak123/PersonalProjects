--- ===============================================================================
-- Author:		 M.Purna Pushkal
-- Description: Creating the receipts table
-- ================================================================================

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'tReceipts')
BEGIN
	CREATE TABLE tReceipts
	(
		 Id INT IDENTITY(100000000,1) PRIMARY KEY,
		 TemplateName NVARCHAR(250) NOT NULL,
		 ReceiptData VARBINARY(MAX) NOT NULL,
		 DtServerCreate DATETIME NOT NULL,
		 DtserverLastModified DATETIME,
		 CONSTRAINT UC_TemplateName UNIQUE (TemplateName)
	)	 
END
GO

