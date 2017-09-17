-- ============================================================
-- Author:		Lokesh
-- Create date: <12/01/2014>
-- Description:	<Created Table tMoneyOrderImage to persist MO images>
-- Rally ID:	<US2291>
-- ============================================================

IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].tMoneyOrderImage') AND type in (N'U'))
BEGIN
	DROP TABLE [dbo].tMoneyOrderImage
END
GO
CREATE TABLE [dbo].tMoneyOrderImage
(
	rowguid UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	TrxId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES tTxn_MoneyOrder(txnRowguid),
	CheckFrontImage VARBINARY(MAX) NOT NULL,
	CheckBackImage  VARBINARY(MAX),
	DTCreate DATETIME NOT NULL,
	DTLastMod DATETIME	
)