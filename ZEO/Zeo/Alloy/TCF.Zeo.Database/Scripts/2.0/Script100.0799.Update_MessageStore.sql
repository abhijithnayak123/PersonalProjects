--- ===============================================================================
-- Author:		 Abhijith
-- Description:	 Updating the Error if the SSN is not provided.
-- Story : SSN / ITIN is required for US Citizens during registration - B-23417
-- ================================================================================


IF EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey LIKE  '%1001.100.8608%')
BEGIN
	
	UPDATE dbo.tMessageStore
	SET Content = N'SSN/ ITIN is needed for US Citizens OR US Country of Birth.'
	WHERE MessageKey = '1001.100.8608'

END
GO