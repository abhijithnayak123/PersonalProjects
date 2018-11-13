-- ================================================================================
-- Author:		<Anisha Abraham>
-- Create date: <09/01/2015>
-- Description:	<Alloy is not trimming spaces for customer fields>
-- Jira ID:		<AL-1068>
-- ================================================================================


ALTER TABLE tChxr_Account DISABLE TRIGGER [trChxr_AccountAudit]
UPDATE
	tChxr_Account
SET
	FirstName = LTRIM(RTRIM(FirstName)),
	LastName = LTRIM(RTRIM(LastName)),
	Address1 = LTRIM(RTRIM(Address1)),
	Address2 = LTRIM(RTRIM(Address2)),
	City = LTRIM(RTRIM(City)),
	Occupation = LTRIM(RTRIM(Occupation)),
	Employer = LTRIM(RTRIM(Employer)),
	IDCardIssuedCountry = LTRIM(RTRIM(IDCardIssuedCountry))
WHERE 
	ChxrAccountPK IS NOT NULL
ALTER TABLE tChxr_Account ENABLE TRIGGER [trChxr_AccountAudit]
GO
