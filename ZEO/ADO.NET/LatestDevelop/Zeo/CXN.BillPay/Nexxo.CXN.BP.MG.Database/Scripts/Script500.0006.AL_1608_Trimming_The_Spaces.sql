-- ================================================================================
-- Author:		<Anisha Abraham>
-- Create date: <09/01/2015>
-- Description:	<Alloy is not trimming spaces for customer fields>
-- Jira ID:		<AL-1068>
-- ================================================================================

UPDATE  
	tMGram_BillPay_Account
SET
	FirstName = LTRIM(RTRIM(FirstName)),
	LastName = LTRIM(RTRIM(LastName)),
	Address1 = LTRIM(RTRIM(Address1)),
	Address2 = LTRIM(RTRIM(Address2)),
	City = LTRIM(RTRIM(City)),
	Street = LTRIM(RTRIM(Street))
WHERE
	MGBillPayAccountPK IS NOT NULL
GO