--- ================================================================================
-- Author:		<Abhijith>
-- Create date: <06/19/2018>
-- Description:	Inserting a new record in .
-- ================================================================================

BEGIN TRY
BEGIN TRAN

	DECLARE @productId BIGINT =
	(
		SELECT ProductsID
		FROM tProducts
		WHERE Name = 'ProcessCheck'
	)

	DECLARE @processorId BIGINT =
	(
		SELECT ProcessorsId
		FROM tprocessors
		WHERE Name = 'TCF'
	)

	DECLARE @productProcessorId BIGINT =
	(
		SELECT ProductProcessorsMappingID
		FROM tProductProcessorsMapping
		WHERE ProductId = @productId AND ProcessorId = @processorId
	)
	
	
	----------------------------  For OnUS ---------------------------------------
	IF NOT EXISTS(SELECT 1 FROM tChannelPartnerProductProcessorsMapping WHERE ProductProcessorId=@productProcessorId)
	BEGIN

		INSERT INTO [dbo].[tChannelPartnerProductProcessorsMapping]
			([Sequence]
			,[DTServerCreate]
			,[DTServerLastModified]
			,[IsTnCForcePrintRequired]
			,[CheckEntryType]
			,[MinimumTransactAge]
			,[CardExpiryPeriod]
			,[ProductProcessorId]
			,[ChannelPartnerID])
		VALUES
		   (6
		   ,GETDATE()
		   ,NULL
		   ,0
		   ,1
		   ,18
		   ,0
		   ,@productProcessorId
		   ,34)

	END
	COMMIT TRAN
END TRY
BEGIN CATCH
    ROLLBACK TRAN
END CATCH
----------------------------  For OnUS ---------------------------------------


