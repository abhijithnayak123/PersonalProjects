--- ================================================================================
-- Author:		<Namit>
-- Create date: <09/28/2015>
-- Description:	 Reverting the change made for MessageKey = 1005.2003
-- Jira ID:		<AL-1013>
-- ================================================================================
 Delete from tMessageStore 
 where MessageKey = '1005.2003' and Content = 'Error in Card lookup E9201 MISSING/INVALID PREFERRED CUSTOMER ACCOUNT NUMBER'
