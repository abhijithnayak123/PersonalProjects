Alter table tLedgerEntries
drop constraint FK_tLedgerEntries_tAccounts

alter table tTxn_BillPay
drop constraint FK_tTxn_BillPay_tAccounts

alter table tTxn_Cash
drop constraint FK_tTxn_Cash_tAccounts

alter table tTxn_Check
drop constraint FK_tTxn_Check_tAccounts

alter table tTxn_Funds
drop constraint FK_tTxn_Funds_tAccounts

alter table tTxn_MoneyOrder
drop constraint FK_tTxn_MoneyOrder_tAccounts

alter table tTxn_MoneyTransfer
drop constraint FK_tTxn_MoneyTransfer_tAccounts
GO

drop table tAccounts
GO

CREATE TABLE tAccounts
	(
	rowguid uniqueidentifier NOT NULL,
	Id bigint NOT NULL,
	CXEId bigint NOT NULL,
	CXNId bigint NULL,
	CustomerPK uniqueidentifier NOT NULL,
	DTCreate datetime NOT NULL,
	DTLastMod datetime NULL
	)  ON [PRIMARY]
GO
ALTER TABLE tAccounts ADD CONSTRAINT
	PK_tAccounts PRIMARY KEY CLUSTERED 
	(
	rowguid
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE tAccounts SET (LOCK_ESCALATION = TABLE)
GO

ALTER TABLE [dbo].[tAccounts]  WITH CHECK ADD CONSTRAINT [FK_tAccounts_tCustomers] FOREIGN KEY([CustomerPK])
REFERENCES [dbo].[tCustomers] ([rowguid])
GO
 
ALTER TABLE [dbo].[tAccounts] CHECK CONSTRAINT [FK_tAccounts_tCustomers]
GO

ALTER TABLE [dbo].[tLedgerEntries]  WITH CHECK ADD CONSTRAINT [FK_tLedgerEntries_tAccounts] FOREIGN KEY([AccountPK])
REFERENCES [dbo].[tAccounts] ([rowguid])
GO
 
ALTER TABLE [dbo].[tLedgerEntries] CHECK CONSTRAINT [FK_tLedgerEntries_tAccounts]
GO

ALTER TABLE [dbo].[tTxn_Cash]  WITH CHECK ADD CONSTRAINT [FK_tTxn_Cash_tAccounts] FOREIGN KEY([AccountPK])
REFERENCES [dbo].[tAccounts] ([rowguid])
GO
 
ALTER TABLE [dbo].[tTxn_Cash] CHECK CONSTRAINT [FK_tTxn_Cash_tAccounts]
GO

ALTER TABLE [dbo].[tTxn_Check]  WITH CHECK ADD CONSTRAINT [FK_tTxn_Check_tAccounts] FOREIGN KEY([AccountPK])
REFERENCES [dbo].[tAccounts] ([rowguid])
GO
 
ALTER TABLE [dbo].[tTxn_Check] CHECK CONSTRAINT [FK_tTxn_Check_tAccounts]
GO
ALTER TABLE [dbo].[tTxn_BillPay]  WITH CHECK ADD CONSTRAINT [FK_tTxn_BillPay_tAccounts] FOREIGN KEY([AccountPK])
REFERENCES [dbo].[tAccounts] ([rowguid])
GO
 
ALTER TABLE [dbo].[tTxn_BillPay] CHECK CONSTRAINT [FK_tTxn_BillPay_tAccounts]
GO
ALTER TABLE [dbo].[tTxn_Funds]  WITH CHECK ADD CONSTRAINT [FK_tTxn_Funds_tAccounts] FOREIGN KEY([AccountPK])
REFERENCES [dbo].[tAccounts] ([rowguid])
GO
 
ALTER TABLE [dbo].[tTxn_Funds] CHECK CONSTRAINT [FK_tTxn_Funds_tAccounts]
GO
ALTER TABLE [dbo].[tTxn_MoneyOrder]  WITH CHECK ADD CONSTRAINT [FK_tTxn_MoneyOrder_tAccounts] FOREIGN KEY([AccountPK])
REFERENCES [dbo].[tAccounts] ([rowguid])
GO
 
ALTER TABLE [dbo].[tTxn_MoneyOrder] CHECK CONSTRAINT [FK_tTxn_MoneyOrder_tAccounts]
GO
ALTER TABLE [dbo].[tTxn_MoneyTransfer]  WITH CHECK ADD CONSTRAINT [FK_tTxn_MoneyTransfer_tAccounts] FOREIGN KEY([AccountPK])
REFERENCES [dbo].[tAccounts] ([rowguid])
GO
 
ALTER TABLE [dbo].[tTxn_MoneyTransfer] CHECK CONSTRAINT [FK_tTxn_MoneyTransfer_tAccounts]
GO
