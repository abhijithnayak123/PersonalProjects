-- ============================================================
-- Author:		Sunil Shetty
-- Create date: <05/25/2015>
-- Description:	<Changed Message Key value for Check declined reasons>
-- Rally ID:	<AL-517>
-- ============================================================

UPDATE tMessageStore SET MessageKey = '1002.1' WHERE MessageKey = '1002.01'			
UPDATE tMessageStore SET MessageKey = '1002.6' WHERE MessageKey = '1002.06'
GO			