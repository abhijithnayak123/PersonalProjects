IF NOT exists(select * from sys.columns where Name = N'PdsRequiredFlag' and Object_ID = Object_ID(N'tWUnion_Trx'))
begin
   ALTER TABLE [dbo].tWUnion_Trx ADD PdsRequiredFlag bit
End
if not exists(select * from sys.columns where Name = N'DfTransactionFlag' and Object_ID = Object_ID(N'tWUnion_Trx'))
begin
   ALTER TABLE [dbo].tWUnion_Trx ADD DfTransactionFlag bit
End
if not exists(select * from sys.columns where Name = N'DeliveryServiceName' and Object_ID = Object_ID(N'tWUnion_Trx'))
begin
   ALTER TABLE [dbo].tWUnion_Trx ADD DeliveryServiceName varchar(100)
End
if not exists(select * from sys.columns where Name = N'DTAvailableForPickup' and Object_ID = Object_ID(N'tWUnion_Trx'))
begin
   ALTER TABLE [dbo].tWUnion_Trx ADD DTAvailableForPickup datetime
End
