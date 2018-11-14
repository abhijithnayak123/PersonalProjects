-- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-21-2017>
-- Description:	Inserting TCF Provider for Check in Processor Credentials table. 
-- Jira ID:		<B-08674>
-- ================================================================================

INSERT INTO [dbo].[tProductProcessorsMapping]
           ([Code]
           ,[DTServerCreate]
           ,[DTServerLastModified]
           ,[IsSSNRequired]
           ,[IsSWBRequired]
           ,[CanParkReceiveMoney]
           ,[ReceiptCopies]
           ,[ReceiptReprintCopies]
           ,[ProductId]
           ,[ProcessorId])
     VALUES
           (202
		   ,GETDATE()
           ,NULL
           ,0
           ,0
           ,0
           ,1
           ,1
           ,1
           ,5)
GO