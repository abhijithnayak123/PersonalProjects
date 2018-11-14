-- ================================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <07/06/2015>
-- Description:	<Insert records to tMessageStore for Customer search synchIn call fails. >
-- Jira ID:		<AL-587>
-- ================================================================================
IF NOT EXISTS
(
	SELECT 1 FROM tMessageStore WHERE MessageKey =  '1011.2009'
) 
BEGIN
		
	INSERT INTO [dbo].[tMessageStore]
        ([MessageStorePK], [MessageKey], [PartnerPK], [Language], [Content], [DTCreate], [Processor])
    VALUES
		(NEWID(), '1011.2009', 34, '0', 'Customer sync failed:  Customer data not found', GETDATE(), 'RCIF')
END
GO
