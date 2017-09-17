﻿IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].tMoneyOrderImage') AND type in (N'U'))
BEGIN
	DROP TABLE [dbo].tMoneyOrderImage
END
GO
CREATE TABLE [dbo].tMoneyOrderImage
(
	rowguid UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	Id BIGINT IDENTITY (1,1),
	TrxId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES tTxn_MoneyOrder(txnRowguid),
	CheckFrontImage VARBINARY(MAX) NOT NULL,
	CheckBackImage  VARBINARY(MAX),
	DTCreate DATETIME NOT NULL,
	DTLastMod DATETIME	
)