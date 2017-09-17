-- ================================================================================
-- Author:		<Anisha Abraham>
-- Create date: <09/01/2015>
-- Description:	<Alloy is not trimming spaces for customer fields>
-- Jira ID:		<AL-1068>
-- ================================================================================

UPDATE
	tCertegy_Account 
SET
	FirstName =LTRIM(RTRIM(FirstName)),
	LastName = LTRIM(RTRIM(LastName)),
	SecondLastName = LTRIM(RTRIM(SecondLastName)),
	Address1 = LTRIM(RTRIM(Address1)),
	Address2 = LTRIM(RTRIM(Address2)),
	City = LTRIM(RTRIM(City))
WHERE
	CertegyAccountPK IS NOT NULL
GO