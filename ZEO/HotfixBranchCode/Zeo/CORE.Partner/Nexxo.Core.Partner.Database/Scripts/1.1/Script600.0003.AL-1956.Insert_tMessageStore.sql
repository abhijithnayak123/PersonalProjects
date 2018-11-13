-- ================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <14/12/2015>
-- Description:	<As Synovus, Associate GPR card issued outside of Alloy>
-- Jira ID:		<AL-1956>
-- ================================================================================
IF NOT EXISTS 
(
	SELECT 1 FROM tMessageStore WHERE MessageKey = '1003.2110'
)
BEGIN
	INSERT INTO tMessageStore ([MessageStorePK], [MessageKey], [PartnerPK], [Language], [Content], [DTServerCreate], [Processor])
	VALUES (NEWID(), '1003.2110', 1, 0, 'The SSN/ITIN and/or name on the card does not match that on the account. Please verify card belongs to customer', GETDATE(), 'VisaDPS')
END
GO


