if not exists(select * from sys.columns where Name = N'RecieverFirstName' and Object_ID = Object_ID(N'tWUnion_Trx'))
begin
ALTER TABLE tWUnion_Trx
ADD  RecieverFirstName varchar(100)
END
GO
if not exists(select * from sys.columns where Name = N'RecieverLastName' and Object_ID = Object_ID(N'tWUnion_Trx'))
begin
ALTER TABLE tWUnion_Trx
ADD  RecieverLastName varchar(100)
END
GO
if not exists(select * from sys.columns where Name = N'RecieverSecondLastName' and Object_ID = Object_ID(N'tWUnion_Trx'))
begin
ALTER TABLE tWUnion_Trx
ADD  RecieverSecondLastName varchar(100)
END
GO