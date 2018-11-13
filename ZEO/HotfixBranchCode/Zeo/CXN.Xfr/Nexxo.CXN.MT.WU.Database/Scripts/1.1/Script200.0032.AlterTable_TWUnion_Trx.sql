IF not exists(select * from sys.columns where Name = N'DeliveryServiceDesc' and Object_ID = Object_ID(N'tWUnion_Trx'))
Begin
alter table tWUnion_Trx
Add DeliveryServiceDesc varchar(100)
End
