IF NOT EXISTS(SELECT * FROM SYS.COLUMNS WHERE  NAME = N'AvailableForPickup' and Object_ID = Object_ID(N'tWUnion_Trx'))
BEGIN
   ALTER TABLE [dbo].tWUnion_Trx ADD  AvailableForPickup NVARCHAR(50)
END
IF NOT EXISTS(SELECT * FROM SYS.COLUMNS WHERE NAME = N'DelayHours' AND OBJECT_ID = OBJECT_ID(N'tWUnion_Trx'))
BEGIN
   ALTER TABLE [dbo].tWUnion_Trx ADD DelayHours VARCHAR(10)
END
IF NOT EXISTS(SELECT * FROM SYS.COLUMNS WHERE  NAME = N'AvailableForPickupEST' and Object_ID = Object_ID(N'tWUnion_Trx'))
BEGIN
   ALTER TABLE [dbo].tWUnion_Trx ADD  AvailableForPickupEST VARCHAR(10)
END
