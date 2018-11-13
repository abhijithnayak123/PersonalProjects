if exists(select * from sys.columns where Name = N'ProviderName' and Object_ID = Object_ID(N'tTransaction'))
begin
ALTER TABLE [dbo].[tTransaction]
ALTER COLUMN ProviderName nvarchar(100)

ALTER TABLE [dbo].[tTransaction]
ALTER COLUMN ClientName nvarchar(100)

ALTER TABLE [dbo].[tTransaction]
ALTER COLUMN BranchId nvarchar(100)
end
else
begin
ALTER TABLE [dbo].[tTransaction]
ADD ProviderName nvarchar(100),
	ClientName nvarchar(100),
	BranchId nvarchar(100)
end