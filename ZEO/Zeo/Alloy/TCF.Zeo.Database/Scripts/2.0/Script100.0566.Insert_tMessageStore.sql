--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <06-27-2017>
-- Description:	Insert Record in the RCIF credentials table. 
-- Jira ID:		<>
-- ================================================================================

DELETE FROM tMessageStore WHERE MessageKey IN ('1001.100.2015')

INSERT INTO dbo.tMessageStore
(
    MessageKey,ChannelPartnerId,Language,Content,DTServerCreate,Processor,Type,AddlDetails
)
VALUES
(
    N'1001.100.2015',
	1,
	N'0',
	N'Certificate not found for RCIF', 
	GETDATE(),
	N'Zeo',
	2,
	N'Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.'
)


