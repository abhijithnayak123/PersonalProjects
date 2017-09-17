if not exists(select * from sys.columns where Name = N'DTServerCreate' and Object_ID = Object_ID(N'tWUnion_Trx'))
begin
ALTER TABLE tWUnion_Trx
ADD DTServerCreate DATETIME
END
GO

if not exists(select * from sys.columns where Name = N'DTServerLastMod' and Object_ID = Object_ID(N'tWUnion_Trx'))
begin
ALTER TABLE tWUnion_Trx
ADD  DTServerLastMod DATETIME
END
GO