if not exists(select * from sys.columns where Name = N'BankId' and Object_ID = Object_ID(N'tCustomers'))
begin
Alter Table dbo.tCustomers add BankId nvarchar(40) NULL
end
GO