-- ================================================================================
-- Author:		<Anisha Abraham>
-- Create date: <09/01/2015>
-- Description:	<Alloy is not trimming spaces for customer fields>
-- Jira ID:		<AL-1068>
-- ================================================================================

--Table :- tTSys_Account_Aud
UPDATE  
	tTSys_Account_Aud
SET 
	FirstName = LTRIM(RTRIM(FirstName)),
	MiddleName = LTRIM(RTRIM(MiddleName)),
	LastName = LTRIM(RTRIM(LastName)),
	Address1 = LTRIM(RTRIM(Address1)),
	Address2 = LTRIM(RTRIM(Address2)),
	City = LTRIM(RTRIM(City)),
	[State] = LTRIM(RTRIM([State])),
	Country = LTRIM(RTRIM(Country))
WHERE
	TSysAccountPK IS NOT NULL
GO