--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <05-28-2018>
-- Description: Adding the Decline Code for Teller Inquiry Error Handling for OnUS.
-- ================================================================================


DELETE FROM tMessageStore WHERE MessageKey IN ('1002.202.0')

INSERT INTO dbo.tMessageStore
(
    MessageKey,ChannelPartnerId,Language,Content,DTServerCreate,Processor,Type,DisplayMessage,AddlDetails
)
VALUES
(
   N'1002.202.0',
	1,
	N'0',
    N'THE CHECK CANNOT BE APPROVED AT THIS TIME.',
	GETDATE(),
	N'TCFCheck',
	2,
	N'THE CHECK CANNOT BE APPROVED AT THIS TIME.',
	NULL
)
GO