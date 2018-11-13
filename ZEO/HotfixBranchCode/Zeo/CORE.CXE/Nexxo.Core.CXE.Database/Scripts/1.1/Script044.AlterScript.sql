if exists(select * from sys.columns where Name = N'BankId' and Object_ID = Object_ID(N'tCustomers'))
begin
Alter Table dbo.tCustomers DROP column BankId
end
GO 