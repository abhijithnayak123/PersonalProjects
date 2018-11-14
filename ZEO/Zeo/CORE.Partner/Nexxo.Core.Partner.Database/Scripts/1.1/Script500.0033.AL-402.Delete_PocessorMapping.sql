-- ============================================================
-- Author:		<Shwetha Mohan>
-- Create date: <06/08/2015>
-- Description:	<To delete the incorrect record for TCF channel partner with 
--				MoneyGram was mapped as processor for MoneyOrder>
-- Jira ID:		<AL-402>
-- ============================================================

IF EXISTS
(
	SELECT 
		1
	FROM 
		tChannelPartnerProductProcessorsMapping 
	WHERE 
		ID = 11
)
BEGIN
	DELETE 
		tChannelPartnerProductProcessorsMapping 
	WHERE 
		ID = 11
END
GO 
