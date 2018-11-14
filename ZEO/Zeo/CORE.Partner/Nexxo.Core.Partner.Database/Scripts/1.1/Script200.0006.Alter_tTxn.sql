ALTER TABLE dbo.tTxn_BillPay ADD
DTServerCreate DateTime,DTServerLastMod DateTime
GO

ALTER TABLE dbo.tTxn_Cash ADD
DTServerCreate DateTime,DTServerLastMod DateTime
GO

ALTER TABLE dbo.tTxn_Check ADD
DTServerCreate DateTime,DTServerLastMod DateTime
GO

ALTER TABLE dbo.tTxn_Funds ADD
DTServerCreate DateTime,DTServerLastMod DateTime 
GO

ALTER TABLE dbo.tTxn_MoneyOrder ADD
DTServerCreate DateTime,DTServerLastMod DateTime
GO

ALTER TABLE dbo.tTxn_MoneyTransfer ADD
DTServerCreate DateTime,DTServerLastMod DateTime
GO