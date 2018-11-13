if not exists(select * from sys.columns where Name = N'BankId' and Object_ID = Object_ID(N'tFIS_Account'))
begin
Alter Table dbo.tFIS_Account add BankId nvarchar(40) NULL, BranchId nvarchar(40) NULL
end
GO