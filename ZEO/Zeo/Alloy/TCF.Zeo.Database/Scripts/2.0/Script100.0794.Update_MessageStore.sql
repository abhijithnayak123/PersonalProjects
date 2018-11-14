--- ================================================================================
-- Author:		<Abhijith>
-- Create date: <07/09/2018>
-- Description:	Update the Content as given in the story - "B-11866" for Card Swipe.
-- ================================================================================

UPDATE tMessageStore
SET Content = 'Unable to complete transaction.  The card provided does not belong to this customer.'
WHERE MessageKey = '1001.100.6020'


