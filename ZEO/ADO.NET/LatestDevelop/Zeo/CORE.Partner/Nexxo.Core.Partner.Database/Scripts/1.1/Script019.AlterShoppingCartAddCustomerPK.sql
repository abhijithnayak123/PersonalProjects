
ALTER TABLE tShoppingCarts
ADD CustomerPK [uniqueidentifier] NULL

ALTER TABLE [dbo].[tShoppingCarts]  WITH CHECK ADD CONSTRAINT [FK_tShoppingCarts_tCustomer] FOREIGN KEY([CustomerPK])
REFERENCES [dbo].[tPartnerCustomers] ([rowguid])
GO
 
ALTER TABLE [dbo].[tShoppingCarts] CHECK CONSTRAINT [FK_tShoppingCarts_tCustomer]
GO
