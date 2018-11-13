-- step #1
-- This gives the customer records for Tsys
select * from tTSys_Account 
select * from tTSys_Account where activated = 1

-- step #2
-- This gives the records of customers where the proxyid and external key matches between visa and tsys
select tv.* from tVisa_Account tv inner join tTSys_Account ta on tv.ProxyId = RIGHT(REPLICATE('0',19)+ta.ExternalKey,19) where ta.activated = 1

-- step #3
-- select the accounts from PTNR for provider id 103 and converted visa accounts from tsys
select * from COPY_RC_PTNR..tAccounts ta
inner join tVisa_Account tv on ta.CXNId = tv.VisaAccountID
inner join tTSys_Account taa on tv.ProxyId = RIGHT(REPLICATE('0',19)+taa.ExternalKey,19) 
where taa.activated =1 and ta.ProviderId = 103

-- step #5
-- select the accounts from PTNR for provider id 102 (Tsys), ideally the count of this and count of step #3 should be same, if not, review the exception rows from the conversion log file.
select * from COPY_RC_PTNR..tAccounts ta
inner join tTSys_Account tsys on tsys.TSysAccountID = ta.CXNId
where ta.ProviderId = 102

--- Use the below queries to delete the records in the table, they can be used only until the transactions were made, 
--- once the transactions are made for a customer using gpr, you cannot delete these records for that customer

-- step #Del.1
-- Delete from ptnr accounts table for provider id 103 where the accounts were converted from tsys to visa
delete ta from COPY_RC_PTNR..tAccounts ta
inner join tVisa_Account tv on ta.CXNId = tv.VisaAccountID
inner join tTSys_Account taa on tv.ProxyId = RIGHT(REPLICATE('0',19)+taa.ExternalKey,19) 
where ta.ProviderId = 103

-- step #Del1.1 -- Run this if there are transactions conducted after conversion, and be careful,
-- this will delete your TRANSACTION records
delete tvt from tvisa_trx tvt 
inner join tvisa_account tv  on tvt.accountpk = tv.VisaAccountPK
inner join tTSys_Account ta on tv.ProxyId = RIGHT(REPLICATE('0', 19)+ta.ExternalKey,19)


-- step #Del.2
-- Delete from visa table, which was converted from tsys table
delete tv from  tVisa_Account tv inner join tTSys_Account ta on tv.ProxyId = RIGHT(REPLICATE('0',19)+ta.ExternalKey,19) 