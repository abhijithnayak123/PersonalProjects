--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <05-28-2018>
-- Description: Adding Error Messages.
-- ================================================================================


DELETE FROM tMessageStore WHERE MessageKey IN ('1010.100.2102','1010.100.2103','1010.100.2104','1010.602.3703','1010.602.3704')

INSERT INTO dbo.tMessageStore
(
    MessageKey,ChannelPartnerId,Language,Content,DTServerCreate,Processor,Type,DisplayMessage,AddlDetails
)
VALUES
(
   N'1010.100.2102',
	1,
	N'0',
    N'Teller MainFrame Commit Failed.',
	GETDATE(),
	N'ZEO',
	2,
	N'Transaction could not be completed. Please contact your technical support team for more information.',
	NULL
)

INSERT INTO dbo.tMessageStore
(
    MessageKey,ChannelPartnerId,Language,Content,DTServerCreate,Processor,Type,DisplayMessage,AddlDetails
)
VALUES
(
   N'1010.100.2103',
	1,
	N'0',
    N'Teller MiddleTier Commit Failed.',
	GETDATE(),
	N'ZEO',
	2,
	N'Transaction could not be completed. Please contact your technical support team for more information.',
	NULL
)
GO

INSERT INTO dbo.tMessageStore
(
    MessageKey,ChannelPartnerId,Language,Content,DTServerCreate,Processor,Type,DisplayMessage,AddlDetails
)
VALUES
(
   N'1010.100.2104',
	1,
	N'0',
    N'RCIF Commit Failed.',
	GETDATE(),
	N'ZEO',
	2,
	N'Transaction could not be completed. Please contact your technical support team for more information.',
	NULL
)
GO

INSERT INTO dbo.tMessageStore
(
    MessageKey,ChannelPartnerId,Language,Content,DTServerCreate,Processor,Type,DisplayMessage,AddlDetails
)
VALUES
(
   N'1010.602.3703',
	1,
	N'0',
    N'No Response From Teller Middle Tier.',
	GETDATE(),
	N'ZEO',
	2,
	N'Transaction could not be completed. Please contact your technical support team for more information.',
	NULL
)
GO

INSERT INTO dbo.tMessageStore
(
    MessageKey,ChannelPartnerId,Language,Content,DTServerCreate,Processor,Type,DisplayMessage,AddlDetails
)
VALUES
(
   N'1010.602.3704',
	1,
	N'0',
    N'Settlement failing - Action - Banker would need to notify IT that there is an issue AND offset the GL manually.',
	GETDATE(),
	N'ZEO',
	2,
	N'Transaction could not be completed. Please contact your technical support team for more information.',
	NULL
)
GO