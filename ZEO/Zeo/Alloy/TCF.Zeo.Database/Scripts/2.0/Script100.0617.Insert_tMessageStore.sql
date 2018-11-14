--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <22-09-2017>
-- Description: Change more than 2 search options error to a pop up
-- ================================================================================


DELETE FROM tMessageStore WHERE MessageKey IN ('1001.100.8607')

INSERT INTO dbo.tMessageStore
(
    MessageKey,ChannelPartnerId,Language,Content,DTServerCreate,Processor,Type,AddlDetails
)
VALUES
(
   N'1001.100.8607',
	1,
	N'0',
    N'<span>Can only search by Date of Birth and ONE other search option. Click &quot;Ok&quot; to try your search again.</span>',
	GETDATE(),
	N'ZEO',
	2,
	N''
)
