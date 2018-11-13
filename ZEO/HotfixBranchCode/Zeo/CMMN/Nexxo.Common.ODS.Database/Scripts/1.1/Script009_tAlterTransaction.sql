if exists(select * from sys.columns where Name = N'CustomerName' and Object_ID = Object_ID(N'tTransaction'))
begin
ALTER TABLE tTransaction DROP COLUMN CustomerName
end
Go
if not exists(select * from sys.columns where Name = N'FirstName' and Object_ID = Object_ID(N'tTransaction'))
begin
ALTER TABLE tTransaction ADD FirstName nvarchar(255), LastName nvarchar(255)
end
go