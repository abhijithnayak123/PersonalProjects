IF NOT EXISTS(SELECT * FROM SYS.COLUMNS WHERE NAME = N'OriginalDestinationCountryCode' AND OBJECT_ID = OBJECT_ID(N'tWUnion_Trx'))
BEGIN
   ALTER TABLE [dbo].tWUnion_Trx ADD OriginalDestinationCountryCode VARCHAR(10)
END
IF NOT EXISTS(SELECT * FROM SYS.COLUMNS WHERE  NAME = N'OriginalDestinationCurrencyCode' and Object_ID = Object_ID(N'tWUnion_Trx'))
BEGIN
   ALTER TABLE [dbo].tWUnion_Trx ADD  OriginalDestinationCurrencyCode VARCHAR(10)
END