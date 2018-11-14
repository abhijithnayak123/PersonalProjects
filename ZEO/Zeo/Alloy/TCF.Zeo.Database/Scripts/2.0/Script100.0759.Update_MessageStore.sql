--- ===============================================================================
-- Author:		 Nitish Biradar
-- Description:	 Adding the error for WU related to Fraud Limit.
-- Story Id   :  B-17069
-- ================================================================================


IF EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey LIKE  '%1005.301.R1109%')
BEGIN
	
	UPDATE tMessageStore
	SET 
		Content = 'R1109 DECLINED IT''S ILLEGAL TO SEND MONEY FOR A TELEMARKETING PURCHASE',
		DTServerLastModified = GETDATE()
	WHERE
		MessageKey = '1005.301.R1109'
END
GO