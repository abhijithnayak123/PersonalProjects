--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <09-22-2017>
-- Description: inserting the error details for no customer found.
-- ================================================================================


DELETE FROM tMessageStore WHERE MessageKey IN ('1001.100.6018')

INSERT INTO dbo.tMessageStore
(
    MessageKey,ChannelPartnerId,Language,Content,DTServerCreate,Processor,Type,AddlDetails
)
VALUES
(
   N'1001.100.6018',
	1,
	N'0',
	N'<span>A customer was not found with your current search. Click &quot;Cancel&quot; to retry your search with different search information.</span><br /><center>OR</center><span> To register a New Customer, click &quot;Continue to Registration&quot;.</span>',
	GETDATE(),
	N'Zeo',
	2,
	''
)
