-- ================================================================================
-- Author:		<Anisha Abraham>
-- Create date: <09/01/2015>
-- Description:	<Alloy is not trimming spaces for customer fields>
-- Jira ID:		<AL-1068>
-- ================================================================================

UPDATE  
	tMGram_Account
SET
	FirstName = LTRIM(RTRIM(FirstName)),
	LastName = LTRIM(RTRIM(LastName)),
	[Address] = LTRIM(RTRIM([Address])),
	City = LTRIM(RTRIM(City)) 
WHERE
	MGAccountPK IS NOT NULL
GO
