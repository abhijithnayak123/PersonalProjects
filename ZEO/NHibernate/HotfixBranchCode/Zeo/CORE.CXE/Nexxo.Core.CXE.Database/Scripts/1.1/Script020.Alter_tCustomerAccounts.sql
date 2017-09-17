
alter table tCustomerAccounts
add CustomerPK uniqueidentifier not null

alter table tCustomerAccounts
drop column PAN

ALTER TABLE [dbo].[tCustomerAccounts]  WITH CHECK ADD  CONSTRAINT [FK_tCustomerAccounts_tCustomer] FOREIGN KEY([CustomerPK])
REFERENCES [dbo].[tCustomers] ([rowguid])
GO

ALTER TABLE [dbo].[tCustomerAccounts] CHECK CONSTRAINT [FK_tCustomerAccounts_tCustomer]
GO
