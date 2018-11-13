if exists(select * from sys.columns where Name = N'FISConnectsAccountId' and Object_ID = Object_ID(N'tCustomer'))
begin
ALTER TABLE [dbo].[tCustomer]
ALTER COLUMN FISConnectsAccountId nvarchar(100)

ALTER TABLE [dbo].[tCustomer]
ALTER COLUMN FISConnectionsAccountId nvarchar(100)
end
ELSE
begin
ALTER TABLE [dbo].[tCustomer]
ADD FISConnectsAccountId nvarchar(100),
	FISConnectionsAccountId nvarchar(100)
end
