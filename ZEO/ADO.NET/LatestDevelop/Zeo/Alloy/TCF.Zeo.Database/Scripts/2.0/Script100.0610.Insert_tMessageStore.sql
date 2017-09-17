--- ===============================================================================
-- Author:		<M. Purna Pushkal>
-- Create date: <12-09-2017>
-- Description: inserting the error details when no customer found in Zeo.
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
	N'No Customer found', 
	GETDATE(),
	N'Zeo',
	2,
	N'Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.'
)
