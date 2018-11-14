if not exists(select * from sys.columns where Name = N'FISConnectsAccountId' and Object_ID = Object_ID(N'tCustomer'))
begin
ALTER TABLE [dbo].[tCustomer]
ADD FISConnectsAccountId varchar(100),
	FISConnectionsAccountId varchar(100)
end
GO