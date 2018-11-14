--- ===============================================================================
-- Author:		 Pushkal
-- Description:	 Adding the error for Not ZEO card
-- ================================================================================


IF NOT EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey LIKE  '%1000.100.8901%')
BEGIN
	INSERT INTO dbo.tMessageStore
	(
		MessageKey,ChannelPartnerId,Language,Content,DTServerCreate,Processor,Type,AddlDetails,ContactType
	)
	VALUES
	(
	   N'1000.100.8901',
		1,
		N'0',
		N'Please Swipe/Enter ZEO card number.',
		GETDATE(),
		N'ZEO',
		2,
		NULL,
		NULL
	)
END
GO



IF NOT EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey LIKE  '%1001.100.6019%')
BEGIN
	INSERT INTO dbo.tMessageStore
	(
		MessageKey,ChannelPartnerId,Language,Content,DTServerCreate,Processor,Type,AddlDetails,ContactType
	)
	VALUES
	(
	   N'1001.100.6019',
		1,
		N'0',
		N'Error occured while validating the ZEO card.',
		GETDATE(),
		N'ZEO',
		2,
		N'Transaction could not be completed. Please contact your technical support team for more information.',
		NULL
	)
END
GO

IF NOT EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey LIKE  '%1001.100.6020%')
BEGIN
	INSERT INTO dbo.tMessageStore
	(
		MessageKey,ChannelPartnerId,Language,Content,DTServerCreate,Processor,Type,AddlDetails,ContactType
	)
	VALUES
	(
	   N'1001.100.6020',
		1,
		N'0',
		N'Unable to complete transaction. The card provided does not belong to this customer. (or) No customer is associated with the card.',
		GETDATE(),
		N'ZEO',
		2,
		NULL,
		NULL
	)
END
GO