
--- ================================================================================
-- Author:		<Nitish Biradar>
-- Create date: <11/25/2015>
-- Description:	<Bill Pay: WU T0415 error message is not proper>
-- Jira ID:		<AL-3175>
-- ================================================================================

--BillPay
IF NOT EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey='1004.415'and PartnerPK=1)
BEGIN
INSERT INTO tMessageStore
(	[MessageStorePK],
	[MessageKey],
	[PartnerPK],
	[Language],
	[Content],
	[DTServerCreate],
	[AddlDetails],
	[Processor])
VALUES(NEWID(),
	'1004.415',
	1,
	0,
	'This transaction requires additional customer information',GETDATE(),
  	'Please add ID, SSN/ITIN, Date of Birth and/or Occupation to customer''s profile and then resubmit transaction','Western Union')
END
GO

--SendMoney
IF NOT EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey='1005.415'and PartnerPK=1)
BEGIN
INSERT INTO tMessageStore(
	[MessageStorePK],
	[MessageKey],
	[PartnerPK],
	[Language],
	[Content],
	[DTServerCreate],
	[AddlDetails],
	[Processor])
VALUES(
	NEWID(),
	'1005.415',
	1,
	0,
	'This transaction requires additional customer information',
	GETDATE(),
  	'Please add ID, SSN/ITIN, Date of Birth and/or Occupation to customer''s profile and then resubmit transaction','Western Union')
END
GO