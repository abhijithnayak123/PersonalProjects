-- ================================================================================
-- Author:		<Anisha Abraham>
-- Create date: <09/01/2015>
-- Description:	<Alloy is not trimming spaces for customer fields>
-- Jira ID:		<AL-1068>
-- ================================================================================

UPDATE  
	tWUnion_Account
SET 
	FirstName = LTRIM(RTRIM(FirstName)),
	LastName = LTRIM(RTRIM(LastName)),
	[Address] = LTRIM(RTRIM([Address])),
	City = LTRIM(RTRIM(City)),
	[State] = LTRIM(RTRIM([State])),
	MiddleName = LTRIM(RTRIM(MiddleName)),
	SecondLastName = LTRIM(RTRIM(SecondLastName))
WHERE
	WUAccountPK IS NOT NULL
GO
