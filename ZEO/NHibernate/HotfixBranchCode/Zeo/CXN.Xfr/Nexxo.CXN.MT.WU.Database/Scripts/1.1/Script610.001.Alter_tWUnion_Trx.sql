-- ================================================================================
-- Author:		<Shwetha Mohan>
-- Create date: <01/19/2016>
-- Description:	<As WU, Populate the WU Message Area on receipts>
-- Jira ID:		<AL-3572>
-- ================================================================================

IF NOT EXISTS 
(
	SELECT 
		1 
	FROM   
		sys.columns 
	WHERE 
		name = N'MessageArea' 
		AND object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]')      
)
BEGIN         

	ALTER TABLE dbo.tWUnion_Trx
	ADD  MessageArea NVARCHAR(MAX)

END
GO