ALTER Table tTxn_BillPay ADD ConfirmationNumber VARCHAR(50)
GO
ALTER Table tTxn_Check ADD ConfirmationNumber VARCHAR(50)
GO
ALTER Table tTxn_MoneyTransfer ADD ConfirmationNumber VARCHAR(50)
GO
ALTER Table tTxn_MoneyOrder ADD ConfirmationNumber VARCHAR(50)
GO
ALTER Table tTxn_Funds ADD ConfirmationNumber VARCHAR(50)
GO
ALTER Table tTxn_Funds ADD FundType Int
GO
ALTER Table tTxn_Cash ADD ConfirmationNumber VARCHAR(50)
GO
ALTER table tTxn_Cash ADD CashType Int
GO