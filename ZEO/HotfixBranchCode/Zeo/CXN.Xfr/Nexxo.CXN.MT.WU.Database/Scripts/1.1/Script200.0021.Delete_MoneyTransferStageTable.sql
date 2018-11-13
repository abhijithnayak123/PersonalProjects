IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tTxn_MoneyTransfer_Stage_Aud]') AND type in (N'U'))
DROP TABLE [dbo].[tTxn_MoneyTransfer_Stage_Aud]
GO