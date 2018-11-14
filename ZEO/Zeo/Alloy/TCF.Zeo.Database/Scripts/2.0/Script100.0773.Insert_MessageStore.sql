--- ===============================================================================
-- Author:		 Abhijith
-- Description:	 Adding the error for Blocked Countries.
-- ================================================================================


IF NOT EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey LIKE  '%1005.100.2090%')
BEGIN
	INSERT INTO dbo.tMessageStore
	(
		MessageKey,ChannelPartnerId,Language,Content,DTServerCreate,Processor,Type,AddlDetails,ContactType
	)
	VALUES
	(
	   N'1005.100.2090',
		1,
		N'0',
		N'Transaction Failed. Money has sent from the Blocked Country.',
		GETDATE(),
		N'ZEO',
		2,
		N'Transaction could not be completed. Please contact your technical support team for more information.',
		NULL
	)
END
GO