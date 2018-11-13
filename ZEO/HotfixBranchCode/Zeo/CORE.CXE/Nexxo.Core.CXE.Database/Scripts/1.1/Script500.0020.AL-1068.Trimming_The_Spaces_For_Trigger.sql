-- Author:		<Anisha Abraham>
-- Create date: <09/01/2015>
-- Description:	<Alloy is not trimming spaces for customer fields>
-- Jira ID:		<AL-1068>
-- ================================================================================

UPDATE  
	DBO.tCustomers
SET
	FirstName = LTRIM(RTRIM(FirstName)),
	MiddleName = LTRIM(RTRIM(MiddleName)),
	LastName = LTRIM(RTRIM(LastName)),
	LastName2 = LTRIM(RTRIM(LastName2)),
	MothersMaidenName = LTRIM(RTRIM(MothersMaidenName)),
	Address1 = LTRIM(RTRIM(Address1)),
	Address2 = LTRIM(RTRIM(Address2)),
	City = LTRIM(RTRIM(City)),
	PIN = LTRIM(RTRIM(PIN)),
	MailingAddress1 = LTRIM(RTRIM(MailingAddress1)),
	MailingAddress2 = LTRIM(RTRIM(MailingAddress2)),
	MailingCity = LTRIM(RTRIM(MailingCity)),
	MailingState = LTRIM(RTRIM(MailingState)),
	LegalCode = LTRIM(RTRIM(LegalCode)),
	Notes = LTRIM(RTRIM(Notes))
WHERE
	 CustomerPK IS NOT NULL
GO
