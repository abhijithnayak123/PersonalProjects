--===========================================================================================
-- Author:		<Prince Bajaj>
-- Create date: <April 21 2015>
-- Description:	<Script to Update ClientId in CXE Database tCustomers table and PTNR Database tProspect table>
-- Jira ID:	    <AL-392>
--===========================================================================================
DECLARE @carverProviderId INT = 601
 
UPDATE 
	cxeCust 
SET 
	cxeCust.ClientID = cxnAcnt.PartnerAccountNumber
FROM 
	tAccounts ptnrAcnt
	INNER JOIN stCCISAccount cxnAcnt ON cxnAcnt.Id = ptnrAcnt.CXNId
	AND ProviderId = @carverProviderId AND PartnerAccountNumber IS NOT NULL 
	INNER JOIN sCustomer cxeCust ON cxeCust.Id = ptnrAcnt.CXEId
GO


DECLARE @synovusProviderId INT = 600
UPDATE 
	cxeCust 
SET 
	cxeCust.ClientID = cxnAcnt.PartnerAccountNumber
FROM 
	tAccounts ptnrAcnt
	INNER JOIN stFISAccount cxnAcnt ON cxnAcnt.Id = ptnrAcnt.CXNId
	AND ProviderId = @synovusProviderId AND PartnerAccountNumber IS NOT NULL 
	INNER JOIN sCustomer cxeCust ON cxeCust.Id = ptnrAcnt.CXEId
GO


DECLARE @carverProviderId INT = 601
UPDATE 
	ptnrProspect  
SET 
	ptnrProspect.ClientID = cxnAcnt.PartnerAccountNumber
FROM 
	tAccounts ptnrAcnt
	INNER JOIN stCCISAccount cxnAcnt ON cxnAcnt.Id = ptnrAcnt.CXNId
	AND ProviderId = @carverProviderId AND PartnerAccountNumber IS NOT NULL 
	INNER JOIN tProspects ptnrProspect ON ptnrProspect.PAN = ptnrAcnt.CXEId
GO

DECLARE @synovusProviderId INT = 600
UPDATE 
	ptnrProspect  
SET 
	ptnrProspect.ClientID = cxnAcnt.PartnerAccountNumber
FROM 
	tAccounts ptnrAcnt
	INNER JOIN stFISAccount cxnAcnt ON cxnAcnt.Id = ptnrAcnt.CXNId
	AND ProviderId = @synovusProviderId AND PartnerAccountNumber IS NOT NULL 
	INNER JOIN tProspects ptnrProspect ON ptnrProspect.PAN = ptnrAcnt.CXEId
GO
