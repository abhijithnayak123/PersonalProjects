--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <01-31-2018>
-- Description:	 Insert record in tMessageStore
-- VersionId:	<B-13218>
-- ================================================================================

GO

INSERT INTO [dbo].[tMessageStore]
           ([MessageKey]
           ,[ChannelPartnerId]
           ,[Language]
           ,[Content]
           ,[DTServerCreate]
           ,[DTServerLastModified]
           ,[AddlDetails]
           ,[Processor]
           ,[Type]
           ,[DisplayMessage])
     VALUES
           ('1000.100.8900',
			34,
            0,
           'Promotion is not valid, please edit promotion with proper data and activate the promotion',
           GETDATE(),
		   NULL,
		   'Please contact technical support team for more information',
           'ZEO',
		   2,
		   NULL)
GO


