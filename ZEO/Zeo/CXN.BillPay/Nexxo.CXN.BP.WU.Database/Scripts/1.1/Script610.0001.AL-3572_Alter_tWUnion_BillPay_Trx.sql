-- ================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <02/16/2016>
-- Description:	<As WU, Populate the WU Message Area on receipts >
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
		AND object_id = OBJECT_ID(N'[dbo].[tWUnion_BillPay_Trx]')      
)
BEGIN         

	ALTER TABLE dbo.tWUnion_BillPay_Trx
	ADD  MessageArea NVARCHAR(MAX)

END
GO