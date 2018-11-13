IF not exists(select * from sys.columns where Name = N'BillerShortName' and Object_ID = Object_ID(N'tTransaction'))
Begin
   ALTER TABLE [dbo].tTransaction ADD BillerShortName varchar(12)
End
IF not exists(select * from sys.columns where Name = N'BillAccountNumber' and Object_ID = Object_ID(N'tTransaction'))
Begin
   ALTER TABLE [dbo].tTransaction ADD BillAccountNumber varchar(50)
End
IF not exists(select * from sys.columns where Name = N'SenderName' and Object_ID = Object_ID(N'tTransaction'))
Begin
   ALTER TABLE [dbo].tTransaction ADD SenderName varchar(50)
End
IF not exists(select * from sys.columns where Name = N'RecepientName' and Object_ID = Object_ID(N'tTransaction'))
Begin
   ALTER TABLE [dbo].tTransaction ADD RecepientName varchar(255)
End
IF not exists(select * from sys.columns where Name = N'DestinationCountry' and Object_ID = Object_ID(N'tTransaction'))
Begin
   ALTER TABLE [dbo].tTransaction ADD DestinationCountry varchar(200)
End
IF not exists(select * from sys.columns where Name = N'MoneyOrderCheckNumber' and Object_ID = Object_ID(N'tTransaction'))
Begin
   ALTER TABLE [dbo].tTransaction ADD MoneyOrderCheckNumber varchar(50)
End