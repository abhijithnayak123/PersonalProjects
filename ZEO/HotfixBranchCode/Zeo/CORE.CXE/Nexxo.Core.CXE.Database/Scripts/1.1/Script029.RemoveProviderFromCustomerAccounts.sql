
-- this is a little tricky if we want to preserve the Provider info from CXE to Partner
-- this script needs to run before Partner Script001 !!

--if exists(select * from sys.columns 
--            where Name = N'Provider' and Object_ID = Object_ID(N'tCustomerAccounts'))    
--begin

--	if not exists(select * from Partner.sys.columns 
--	where Name = N'ProviderId' and Object_ID = Object_ID(N'Partner..tAccounts'))
--	begin
--		create table tmpAccountProviders(
--			ProviderId int,
--			CXEId long
--			);

--		insert tmpAccountProviders(ProviderId, CXEId)
--		select Provider, Id
--		from tCustomerAccounts
--	end

	alter table tCustomerAccounts
	drop column Provider
--end