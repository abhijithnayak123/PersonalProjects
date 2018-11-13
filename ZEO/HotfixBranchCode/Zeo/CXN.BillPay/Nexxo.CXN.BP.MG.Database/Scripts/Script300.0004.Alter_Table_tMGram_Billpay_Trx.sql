-- ============================================================
-- Author:		<Ratheesh PK>
-- Create date: <21/08/2014>
-- Description:	<Adding AccountPK field> 
-- Rally ID:	<NA>
-- ============================================================
IF NOT EXISTS(select * from sys.columns 
            where Name = N'AccountPK' and Object_ID = Object_ID(N'tMGram_BillPay_Trx'))
BEGIN
	ALTER TABLE tMGram_BillPay_Trx
    ADD AccountPK uniqueidentifier
END

IF NOT EXISTS(SELECT * FROM sys.foreign_keys WHERE object_id = object_id(N'[dbo].[FK_tMGram_BillPay_Trx_tMGram_BillPay_Account]') and parent_object_id = object_id(N'[dbo].[tMGram_BillPay_Trx]'))
BEGIN
	ALTER TABLE [dbo].[tMGram_BillPay_Trx] WITH CHECK ADD CONSTRAINT [FK_tMGram_BillPay_Trx_tMGram_BillPay_Account] FOREIGN KEY([AccountPK])
	REFERENCES [dbo].[tMGram_BillPay_Account]([rowguid])
END
