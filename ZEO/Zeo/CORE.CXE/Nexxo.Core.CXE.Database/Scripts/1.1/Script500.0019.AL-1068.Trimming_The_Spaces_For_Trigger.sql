-- ================================================================================
-- Author:		<Anisha Abraham>
-- Create date: <09/01/2015>
-- Description:	<Alloy is not trimming spaces for customer fields>
-- Jira ID:		<AL-1068>
-- ================================================================================

--Table :- [tCustomerEmploymentDetails]
ALTER TABLE DBO.tCustomerEmploymentDetails DISABLE TRIGGER [trCustomerEmploymentDetailsAudit] 
UPDATE  
	tCustomerEmploymentDetails 
SET
	Occupation = LTRIM(RTRIM(Occupation)), 
	Employer = LTRIM(RTRIM(Employer)),
	OccupationDescription = LTRIM(RTRIM(OccupationDescription))
WHERE
	CustomerPK IS NOT NULL
GO
ALTER TABLE DBO.tCustomerEmploymentDetails ENABLE TRIGGER [trCustomerEmploymentDetailsAudit]