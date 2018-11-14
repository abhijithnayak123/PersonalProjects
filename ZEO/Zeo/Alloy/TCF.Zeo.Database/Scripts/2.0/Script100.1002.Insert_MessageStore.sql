--- ===============================================================================
-- Author:		 Abhijith
-- Description:	 Adding the Error for Enable/Disable features.
-- ================================================================================

IF NOT EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey LIKE  '%1000.100.4501%')
BEGIN
	INSERT INTO dbo.tMessageStore
	(
		MessageKey,ChannelPartnerId,Language,Content,DTServerCreate,Processor,Type,AddlDetails,ContactType
	)
	VALUES
	(
	   N'1000.100.4501',
		1,
		N'0',
		N'Error Occurred while fetching the features',
		GETDATE(),
		N'ZEO',
		2,
		N'Please try again. If problem persists, contact the IT Service Desk at {0}.',
		'ITServiceDesk'
	)
END
GO

IF NOT EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey LIKE  '%1000.100.4502%')
BEGIN
	INSERT INTO dbo.tMessageStore
	(
		MessageKey,ChannelPartnerId,Language,Content,DTServerCreate,Processor,Type,AddlDetails,ContactType
	)
	VALUES
	(
	   N'1000.100.4502',
		1,
		N'0',
		N'Error Occurred while updating the features',
		GETDATE(),
		N'ZEO',
		2,
		N'Please try again. If problem persists, contact the IT Service Desk at {0}.',
		'ITServiceDesk'
	)
END
GO


IF NOT EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey LIKE  '%1000.100.4911%')
BEGIN
	INSERT INTO dbo.tMessageStore
	(
		MessageKey,ChannelPartnerId,Language,Content,DTServerCreate,Processor,Type,AddlDetails,ContactType
	)
	VALUES
	(
	   N'1000.100.4911',
		1,
		N'0',
		N'Error Occurred while fetching the features',
		GETDATE(),
		N'ZEO',
		2,
		N'Please try again. If problem persists, contact the IT Service Desk at {0}.',
		'ITServiceDesk'
	)
END
GO


IF NOT EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey LIKE  '%1000.100.4912%')
BEGIN
	INSERT INTO dbo.tMessageStore
	(
		MessageKey,ChannelPartnerId,Language,Content,DTServerCreate,Processor,Type,AddlDetails,ContactType
	)
	VALUES
	(
	   N'1000.100.4912',
		1,
		N'0',
		N'Error Occurred while updating the features',
		GETDATE(),
		N'ZEO',
		2,
		N'Please try again. If problem persists, contact the IT Service Desk at {0}.',
		'ITServiceDesk'
	)
END
