
ALTER TABLE tTxn_Cash
ADD CustomerSessionPK [uniqueidentifier] NULL,
Amount [money] NULL,
Fee [money] NULL,
[Description] [nvarchar](255) NULL

ALTER TABLE [dbo].[tTxn_Cash]  WITH CHECK ADD CONSTRAINT [FK_tTxn_Cash_tCustomerSessions] FOREIGN KEY([CustomerSessionPK])
REFERENCES [dbo].[tCustomerSessions] ([CustomerSessionRowguid])
GO
 
ALTER TABLE [dbo].[tTxn_Cash] CHECK CONSTRAINT [FK_tTxn_Cash_tCustomerSessions]
GO

---

ALTER TABLE tTxn_Check
ADD CustomerSessionPK [uniqueidentifier] NULL,
Amount [money] NULL,
Fee [money] NULL,
[Description] [nvarchar](255) NULL

ALTER TABLE [dbo].[tTxn_Check]  WITH CHECK ADD CONSTRAINT [FK_tTxn_Check_tCustomerSessions] FOREIGN KEY([CustomerSessionPK])
REFERENCES [dbo].[tCustomerSessions] ([CustomerSessionRowguid])
GO
 
ALTER TABLE [dbo].[tTxn_Check] CHECK CONSTRAINT [FK_tTxn_Check_tCustomerSessions]
GO

---

ALTER TABLE tTxn_Funds
ADD CustomerSessionPK [uniqueidentifier] NULL,
Amount [money] NULL,
Fee [money] NULL,
[Description] [nvarchar](255) NULL

ALTER TABLE [dbo].[tTxn_Funds]  WITH CHECK ADD CONSTRAINT [FK_tTxn_Funds_tCustomerSessions] FOREIGN KEY([CustomerSessionPK])
REFERENCES [dbo].[tCustomerSessions] ([CustomerSessionRowguid])
GO
 
ALTER TABLE [dbo].[tTxn_Funds] CHECK CONSTRAINT [FK_tTxn_Funds_tCustomerSessions]
GO

---

ALTER TABLE tTxn_BillPay
ADD CustomerSessionPK [uniqueidentifier] NULL,
Amount [money] NULL,
Fee [money] NULL,
[Description] [nvarchar](255) NULL

ALTER TABLE [dbo].[tTxn_BillPay]  WITH CHECK ADD CONSTRAINT [FK_tTxn_BillPay_tCustomerSessions] FOREIGN KEY([CustomerSessionPK])
REFERENCES [dbo].[tCustomerSessions] ([CustomerSessionRowguid])
GO
 
ALTER TABLE [dbo].[tTxn_BillPay] CHECK CONSTRAINT [FK_tTxn_BillPay_tCustomerSessions]
GO

---

ALTER TABLE tTxn_MoneyOrder
ADD CustomerSessionPK [uniqueidentifier] NULL,
Amount [money] NULL,
Fee [money] NULL,
[Description] [nvarchar](255) NULL

ALTER TABLE [dbo].[tTxn_MoneyOrder]  WITH CHECK ADD CONSTRAINT [FK_tTxn_MoneyOrder_tCustomerSessions] FOREIGN KEY([CustomerSessionPK])
REFERENCES [dbo].[tCustomerSessions] ([CustomerSessionRowguid])
GO
 
ALTER TABLE [dbo].[tTxn_MoneyOrder] CHECK CONSTRAINT [FK_tTxn_MoneyOrder_tCustomerSessions]
GO

---

ALTER TABLE tTxn_MoneyTransfer
ADD CustomerSessionPK [uniqueidentifier] NULL,
Amount [money] NULL,
Fee [money] NULL,
[Description] [nvarchar](255) NULL

ALTER TABLE [dbo].[tTxn_MoneyTransfer]  WITH CHECK ADD CONSTRAINT [FK_tTxn_MoneyTransfer_tCustomerSessions] FOREIGN KEY([CustomerSessionPK])
REFERENCES [dbo].[tCustomerSessions] ([CustomerSessionRowguid])
GO
 
ALTER TABLE [dbo].[tTxn_MoneyTransfer] CHECK CONSTRAINT [FK_tTxn_MoneyTransfer_tCustomerSessions]
GO
