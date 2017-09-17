-- ============================================================
-- Author:		<RAJKUMAR M>
-- Create date: <12/18/2015>
-- Description:	<To delete the incorrect record for Product level with 
--				INGO decline code was mapped as processor for Check Process>
-- Jira ID:		<AL-1409>
-- ============================================================

DELETE FROM 
	[dbo].[tMessageStore]
WHERE 
	PartnerPK in (28, 33, 34) 
	AND MessageKey LIKE '1002.%'
GO
