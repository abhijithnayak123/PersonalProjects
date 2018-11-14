--- ===============================================================================
-- Author:		 Abhijith
-- Description:	 Adding the error for WU related to Fraud Limit.
-- Story Id   :  B-17069
-- ================================================================================


IF NOT EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey LIKE  '%1005.301.R1109%')
BEGIN
	INSERT INTO dbo.tMessageStore
	(
		MessageKey,ChannelPartnerId,Language,Content,DTServerCreate,Processor,Type,AddlDetails,ContactType
	)
	VALUES
	(
	   N'1005.301.R1109',
		1,
		N'0',
		N'',
		GETDATE(),
		N'Western Union',
		2,
		N'If your customer has questions, direct them to contact Western Union customer service at {0}.',
		'WU'
	)
END
GO