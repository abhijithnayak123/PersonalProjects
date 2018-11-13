-- ================================================================================
-- Author:		<Anisha Abraham>
-- Create date: <09/01/2015>
-- Description:	<Alloy is not trimming spaces for customer fields>
-- Jira ID:		<AL-1068>
-- ================================================================================

UPDATE  
	tChxr_Account_Aud 
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
	[ChxrAccountPK] IS NOT NULL
GO

UPDATE
	 tChxrSim_Account 
SET
	FName =LTRIM(RTRIM(FName)),
	LName = LTRIM(RTRIM(LName)),
	Address1 = LTRIM(RTRIM(Address1)),
	Address2 = LTRIM(RTRIM(Address2)),
	City = LTRIM(RTRIM(City)),
	Occupation = LTRIM(RTRIM(Occupation)),
	Employer = LTRIM(RTRIM(Employer))
WHERE
	[ChxrSimAccountPK] IS NOT NULL
GO