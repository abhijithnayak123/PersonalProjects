
--===========================================================================================
-- Author:		<Prince Bajaj>
-- Create date: <Mar 31 2015>
-- Description:	<Script to Update ClientId in CXE Database tCustomers table>
-- Jira ID:	    <AL-265>
--===========================================================================================
 
UPDATE cxeCust 
SET cxeCust.ClientID = cxnAcnt.PartnerAccountNumber
FROM tAccounts ptnrAcnt
INNER JOIN stCCISAccount cxnAcnt ON cxnAcnt.Id = ptnrAcnt.CXNId
INNER JOIN sCustomer cxeCust ON cxeCust.Id = ptnrAcnt.CXEId
GO

UPDATE cxeCust 
SET cxeCust.ClientID = cxnAcnt.PartnerAccountNumber
FROM tAccounts ptnrAcnt
INNER JOIN stFISAccount cxnAcnt ON cxnAcnt.Id = ptnrAcnt.CXNId
INNER JOIN sCustomer cxeCust ON cxeCust.Id = ptnrAcnt.CXEId
GO
