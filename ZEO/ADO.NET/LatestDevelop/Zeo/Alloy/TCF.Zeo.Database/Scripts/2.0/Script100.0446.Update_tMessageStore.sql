-- ===================================================
-- Author:		<Abhijith>
-- Create date: <03/29/2017>
-- Description:	 Updating the Message content of the send money.
-- ===================================================

UPDATE tms
SET Content = REPLACE(Content,'Error occure', 'Error occurred')
FROM dbo.tMessageStore tms
WHERE MessageKey LIKE '%1005.100%' AND Content LIKE '%Error occure%'