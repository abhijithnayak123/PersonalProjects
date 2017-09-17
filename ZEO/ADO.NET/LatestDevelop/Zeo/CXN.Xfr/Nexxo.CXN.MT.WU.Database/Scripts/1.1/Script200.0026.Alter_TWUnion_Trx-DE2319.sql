IF not exists(select * from sys.columns where Name = N'PromoCodeDescription' and Object_ID = Object_ID(N'tWUnion_Trx'))
Begin
   ALTER TABLE [dbo].tWUnion_Trx ADD PromoCodeDescription nvarchar(80)
End
IF not exists(select * from sys.columns where Name = N'PromoName' and Object_ID = Object_ID(N'tWUnion_Trx'))
Begin
   ALTER TABLE [dbo].tWUnion_Trx ADD PromoName nvarchar(80)
End
IF not exists(select * from sys.columns where Name = N'PromoMessage' and Object_ID = Object_ID(N'tWUnion_Trx'))
Begin
   ALTER TABLE [dbo].tWUnion_Trx ADD PromoMessage nvarchar(80)
End
IF not exists(select * from sys.columns where Name = N'PromotionError' and Object_ID = Object_ID(N'tWUnion_Trx'))
Begin
   ALTER TABLE [dbo].tWUnion_Trx ADD PromotionError nvarchar(80)
End