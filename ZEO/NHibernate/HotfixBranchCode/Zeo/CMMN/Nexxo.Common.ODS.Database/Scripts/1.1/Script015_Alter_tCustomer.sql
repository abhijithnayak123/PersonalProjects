if exists(select * from sys.columns where Name = N'FISConnectsAccountId' and Object_ID = Object_ID(N'tCustomer'))
begin
ALTER TABLE tCustomer
DROP COLUMN FISConnectsAccountId
end