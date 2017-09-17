if exists(select * from sys.columns where Name = N'ProviderName' and Object_ID = Object_ID(N'tTransaction'))
begin
ALTER TABLE [dbo].[tTransaction]
ALTER COLUMN ProviderName varchar(100)

ALTER TABLE [dbo].[tTransaction]
ALTER COLUMN ClientName varchar(100)

ALTER TABLE [dbo].[tTransaction]
ALTER COLUMN BranchId varchar(100)
end
else
begin
ALTER TABLE [dbo].[tTransaction]
ADD ProviderName varchar(100),
	ClientName varchar(100),
	BranchId varchar(100)
end