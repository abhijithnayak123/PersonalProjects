IF not exists(select * from sys.columns where Name = N'ReferenceNo' and Object_ID = Object_ID(N'tWUnion_Trx'))
Begin
   ALTER TABLE [dbo].tWUnion_Trx ADD ReferenceNo varchar(50)
End