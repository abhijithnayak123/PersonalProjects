DECLARE @partner_id bigint 
DECLARE @myCursor CURSOR
SET @myCursor = CURSOR FOR 
        select  
		 ptnr.id
		 From tAccounts act
		 inner join tPartnerCustomers cust on act.CustomerPK = cust.rowguid
		 inner join tChannelPartners ptnr on cust.ChannelPartnerId = ptnr.rowguid 
		 inner join tChxr_Account chxr on chxr.Id = act.CxnId
		 inner join tChxr_Trx trx on chxr.rowguid = trx.ChxrAccountPK
OPEN @myCursor
	FETCH NEXT FROM @myCursor 
	INTO @partner_id 
	WHILE @@FETCH_STATUS = 0 
	BEGIN
		Update  tChxr_Trx set ChannelPartnerId=@partner_id where current of @myCursor
		FETCH NEXT FROM @myCursor
		INTO @partner_id    
END 
CLOSE @myCursor
DEALLOCATE @myCursor
GO