USE [$DBPrifix$_PTNR]
update tChannelPartners set TIM=3
--Updating for Synovus carver and TCF

update tLocations set bankID=300 ,BranchID='0120', NoOfCounterIDs = 3
where LocationID = 1000000001 -- ('BC46F466-16D3-47B9-97CC-A9F95E2A2CCB')synovus

update tLocations set bankID=300 ,BranchID='0120', NoOfCounterIDs = 3
where LocationID =1000000002 

update tLocations set bankID=300 ,BranchID='0120', NoOfCounterIDs = 3
where LocationID =1000000003 

update tLocationProcessorCredentials set UserName = '13139925', Password = '13139925' 
where LocationID = 1000000001 and ProviderId = 200

update tLocationProcessorCredentials set UserName = '13139925', Password = '13139925' 
where LocationID = 1000000002 and ProviderId = 200

update tLocationProcessorCredentials set UserName = '13139925', Password = '13139925' 
where LocationID = 1000000003 and ProviderId = 200

update tLocationProcessorCredentials set Identifier = '13139925' 
where LocationID = 1000000001 and ProviderId = (401)

update tLocationProcessorCredentials set Identifier = '13139925' 
where LocationID = 1000000002 and ProviderId = (401)

update tLocationProcessorCredentials set Identifier = '13139925' 
where LocationID = 1000000003 and ProviderId in (401)

update tLocationProcessorCredentials set Identifier = '13139925' 
where LocationID = 1000000001 and ProviderId in (301)

update tLocationProcessorCredentials set Identifier = '13139925' 
where LocationID = 1000000002 and ProviderId in (301)

update tLocationProcessorCredentials set Identifier = '13139925' 
where LocationID = 1000000003 and ProviderId in (301)

update tLocationProcessorCredentials set Identifier = '13139925' 
where LocationID = 1000000001 and ProviderId = 102

update tLocationProcessorCredentials set Identifier = '13139925' 
where LocationID = 1000000002 and ProviderId = 102

update tLocationProcessorCredentials set Identifier = '13139925' 
where LocationID = 1000000003 and ProviderId = 102


-- Insert time Zone to synovus default location
update tLocations set [TimezoneID]='Eastern Standard Time'  
where LocationID = 1000000001


