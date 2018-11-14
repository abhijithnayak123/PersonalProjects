IF not exists(select * from sys.columns where Name = N'recordingCountryCode' and Object_ID = Object_ID(N'tWUnion_Trx'))
Begin
   ALTER TABLE [dbo].tWUnion_Trx ADD recordingCountryCode nvarchar(20)
End
IF not exists(select * from sys.columns where Name = N'recordingCurrencyCode' and Object_ID = Object_ID(N'tWUnion_Trx'))
Begin
   ALTER TABLE [dbo].tWUnion_Trx ADD recordingCurrencyCode nvarchar(20)
End
IF not exists(select * from sys.columns where Name = N'originating_city' and Object_ID = Object_ID(N'tWUnion_Trx'))
Begin
   ALTER TABLE [dbo].tWUnion_Trx ADD originating_city nvarchar(100)
End
IF not exists(select * from sys.columns where Name = N'originating_state' and Object_ID = Object_ID(N'tWUnion_Trx'))
Begin
   ALTER TABLE [dbo].tWUnion_Trx ADD originating_state nvarchar(100)
End
IF not exists(select * from sys.columns where Name = N'municipal_tax' and Object_ID = Object_ID(N'tWUnion_Trx'))
Begin
   ALTER TABLE [dbo].tWUnion_Trx ADD municipal_tax decimal(18,2)
End
IF not exists(select * from sys.columns where Name = N'state_tax' and Object_ID = Object_ID(N'tWUnion_Trx'))
Begin
   ALTER TABLE [dbo].tWUnion_Trx ADD state_tax decimal(18,2)
End
IF not exists(select * from sys.columns where Name = N'county_tax' and Object_ID = Object_ID(N'tWUnion_Trx'))
Begin
   ALTER TABLE [dbo].tWUnion_Trx ADD county_tax decimal(18,2)
End
IF not exists(select * from sys.columns where Name = N'plus_charges_amount' and Object_ID = Object_ID(N'tWUnion_Trx'))
Begin
   ALTER TABLE [dbo].tWUnion_Trx ADD plus_charges_amount decimal(18,2)
End
IF not exists(select * from sys.columns where Name = N'message_charge' and Object_ID = Object_ID(N'tWUnion_Trx'))
Begin
   ALTER TABLE [dbo].tWUnion_Trx ADD message_charge decimal(18,2)
End
IF not exists(select * from sys.columns where Name = N'total_undiscounted_charges' and Object_ID = Object_ID(N'tWUnion_Trx'))
Begin
   ALTER TABLE [dbo].tWUnion_Trx ADD total_undiscounted_charges decimal(18,2)
End
IF not exists(select * from sys.columns where Name = N'total_discount' and Object_ID = Object_ID(N'tWUnion_Trx'))
Begin
   ALTER TABLE [dbo].tWUnion_Trx ADD total_discount decimal(18,2)
End
IF not exists(select * from sys.columns where Name = N'total_discounted_charges' and Object_ID = Object_ID(N'tWUnion_Trx'))
Begin
   ALTER TABLE [dbo].tWUnion_Trx ADD total_discounted_charges decimal(18,2)
End
IF not exists(select * from sys.columns where Name = N'instant_notification_addl_service_charges' and Object_ID = Object_ID(N'tWUnion_Trx'))
Begin
   ALTER TABLE [dbo].tWUnion_Trx ADD instant_notification_addl_service_charges nvarchar(300)
End