--- ===============================================================================
-- Author:		 Abhijith
-- Description:	 Adding the error for Fraud Limit.
-- Story Id   :  B-13688
-- ================================================================================


IF NOT EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey LIKE  '%1005.100.2088%')
BEGIN
	INSERT INTO dbo.tMessageStore
	(
		MessageKey,ChannelPartnerId,Language,Content,DTServerCreate,Processor,Type,AddlDetails
	)
	VALUES
	(
	   N'1005.100.2088',
		1,
		N'0',
		N'Error while fetching Fraud Limit',
		GETDATE(),
		N'ZEO',
		2,
		N'Transaction could not be completed. Please contact your technical support team for more information'
	)
END
GO

IF NOT EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey LIKE  '%1005.100.2089%')
BEGIN
	INSERT INTO dbo.tMessageStore
	(
		MessageKey,ChannelPartnerId,Language,Content,DTServerCreate,Processor,Type,AddlDetails
	)
	VALUES
	(
	   N'1005.100.2089',
		1,
		N'0',
		N'Error while fetching destination amount',
		GETDATE(),
		N'ZEO',
		2,
		N'Transaction could not be completed. Please contact your technical support team for more information'
	)
END
GO

IF NOT EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey LIKE  '%1005.100.6043%')
BEGIN
	INSERT INTO dbo.tMessageStore
	(
		MessageKey,ChannelPartnerId,Language,Content,DTServerCreate,Processor,Type,AddlDetails
	)
	VALUES
	(
	   N'1005.100.6043',
		1,
		N'0',
		N'Error while fetching Fraud Limit',
		GETDATE(),
		N'ZEO',
		2,
		N'Transaction could not be completed. Please contact your technical support team for more information'
	)
END
GO



IF NOT EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey LIKE  '%1005.100.6044%')
BEGIN
	INSERT INTO dbo.tMessageStore
	(
		MessageKey,ChannelPartnerId,Language,Content,DTServerCreate,Processor,Type,AddlDetails
	)
	VALUES
	(
	   N'1005.100.6044',
		1,
		N'0',
		N'Error while fetching destination amount',
		GETDATE(),
		N'ZEO',
		2,
		N'Transaction could not be completed. Please contact your technical support team for more information'
	)
END
GO