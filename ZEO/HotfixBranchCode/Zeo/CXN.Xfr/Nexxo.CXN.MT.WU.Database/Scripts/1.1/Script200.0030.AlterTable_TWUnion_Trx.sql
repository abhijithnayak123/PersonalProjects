IF not exists(select * from sys.columns where Name = N'PaySideCharges' and Object_ID = Object_ID(N'tWUnion_Trx'))
Begin
   ALTER TABLE [dbo].tWUnion_Trx ADD PaySideCharges decimal(18,2)
End
IF not exists(select * from sys.columns where Name = N'PaySideTax' and Object_ID = Object_ID(N'tWUnion_Trx'))
Begin
   ALTER TABLE [dbo].tWUnion_Trx ADD PaySideTax decimal(18,2)
End
IF not exists(select * from sys.columns where Name = N'AmountToReceiver' and Object_ID = Object_ID(N'tWUnion_Trx'))
Begin
   ALTER TABLE [dbo].tWUnion_Trx ADD AmountToReceiver decimal(18,2)
End
IF not exists(select * from sys.columns where Name = N'SMSNotificationFlag' and Object_ID = Object_ID(N'tWUnion_Trx'))
Begin
   ALTER TABLE [dbo].tWUnion_Trx ADD SMSNotificationFlag varchar(10)
End
IF not exists(select * from sys.columns where Name = N'PersonalMessage' and Object_ID = Object_ID(N'tWUnion_Trx'))
Begin
ALTER TABLE dbo.tWUnion_Trx
ADD PersonalMessage NVARCHAR(1000)
End
