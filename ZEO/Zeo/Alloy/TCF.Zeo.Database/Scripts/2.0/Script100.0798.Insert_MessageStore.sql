--- ===============================================================================
-- Author:		 Pushkal
-- Description:	 Adding the Error if the SSN is not provided.
-- Story : SSN / ITIN is required for US Citizens during registration - B-23417
-- ================================================================================


IF NOT EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey LIKE  '%1001.100.8608%')
BEGIN
	INSERT INTO dbo.tMessageStore
	(
		MessageKey,ChannelPartnerId,Language,Content,DTServerCreate,Processor,Type,AddlDetails,ContactType
	)
	VALUES
	(
	   N'1001.100.8608',
		1,
		N'0',
		N'Please enter SSN/ITIN to proceed further.',
		GETDATE(),
		N'ZEO',
		2,
		N'Please go to the Personal information page and provide the SSN/ITIN.',
		NULL
	)
END
GO