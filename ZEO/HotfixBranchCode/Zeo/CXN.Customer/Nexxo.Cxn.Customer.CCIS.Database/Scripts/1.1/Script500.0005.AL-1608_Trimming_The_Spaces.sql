-- ================================================================================
-- Author:		<Anisha Abraham>
-- Create date: <09/01/2015>
-- Description:	<Alloy is not trimming spaces for customer fields>
-- Jira ID:		<AL-1068>
-- ================================================================================

UPDATE 
	tCCIS_Account
SET 
	FirstName = LTRIM(RTRIM(FirstName)),
	MiddleName = LTRIM(RTRIM(MiddleName)),
	LastName =LTRIM(RTRIM(LastName)),
	LastName2 = LTRIM(RTRIM(LastName2)),
	MothersMaidenName = LTRIM(RTRIM(MothersMaidenName)),
	Address1 = LTRIM(RTRIM(Address1)),
	Address2 = LTRIM(RTRIM(Address2)),
	City = LTRIM(RTRIM(City)),
	[State] = LTRIM(RTRIM([State]))
WHERE 
	[CCISAccountPK] IS NOT NULL
GO