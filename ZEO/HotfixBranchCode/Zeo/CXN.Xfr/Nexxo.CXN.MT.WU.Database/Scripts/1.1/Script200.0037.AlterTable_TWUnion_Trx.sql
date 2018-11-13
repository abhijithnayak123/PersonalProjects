IF NOT EXISTS(SELECT * FROM SYS.COLUMNS WHERE  NAME = N'PaidDateTime' and Object_ID = Object_ID(N'tWUnion_Trx'))
BEGIN
   ALTER TABLE [dbo].tWUnion_Trx ADD  PaidDateTime NVARCHAR(50)
END