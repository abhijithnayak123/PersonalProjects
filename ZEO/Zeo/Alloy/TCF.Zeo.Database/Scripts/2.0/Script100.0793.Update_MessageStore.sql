--- ================================================================================
-- Author:		<Abhijith>
-- Create date: <07/09/2018>
-- Description:	Update the Content to Empty instead of Unknown Error.
-- ================================================================================

UPDATE tMessageStore
SET Content = ''
WHERE MessageKey IN ('1001.602.1','1001.602.2') 


