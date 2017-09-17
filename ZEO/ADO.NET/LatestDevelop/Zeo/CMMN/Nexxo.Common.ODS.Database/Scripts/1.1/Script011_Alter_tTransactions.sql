if not exists(select * from sys.columns where Name = N'ProviderName' and Object_ID = Object_ID(N'tTransaction'))
begin
ALTER TABLE [dbo].[tTransaction]
ADD ProviderName varchar(100),
	ClientName varchar(100),
	BranchId varchar(100)
end
GO