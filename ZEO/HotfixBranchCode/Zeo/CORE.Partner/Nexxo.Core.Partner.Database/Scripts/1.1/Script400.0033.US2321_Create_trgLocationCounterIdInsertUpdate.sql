--===========================================================================================
-- Author:		<Abhijith>
-- Create date: <Jan 21 2015>
-- Description:	<Adding  trigger for inserting CounterID in tLocationCounterIDDetails>
-- Rally ID:	<US2321>
--===========================================================================================

IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[trgLocationCounterIdInsertUpdate]'))
DROP TRIGGER [dbo].[trgLocationCounterIdInsertUpdate]
GO

CREATE TRIGGER [dbo].[trgLocationCounterIdInsertUpdate] ON [dbo].[tLocationProcessorCredentials] 
AFTER INSERT,UPDATE
AS
BEGIN

	SET NOCOUNT ON

	DECLARE @LocationIdentifier nvarchar(50)
	DECLARE @LocationId uniqueidentifier 
	DECLARE @Increment int
	DECLARE @Count int
	DECLARE @SendMoneyProviderId int
	DECLARE @BillPayProviderId int
	DECLARE @CounterId nvarchar(50)
	SET @Increment = 1;
	SET @Count = 20;
	SET @SendMoneyProviderId = 301
	SET @BillPayProviderId = 401
	SET @CounterId=''
	
	SELECT @LocationId=LocationId, @LocationIdentifier=Identifier FROM inserted
	
	if(@LocationIdentifier  IS NOT NULL and @LocationIdentifier <> ''  and UPDATE(Identifier))
	BEGIN
		
		SELECT @Count = (SELECT NoOfCounterIds FROM tLocations WHERE rowguid = @LocationId)
		
		DELETE FROM tLocationCounterIdDetails where LocationId = @LocationId
	
		WHILE @Increment <= @Count
		BEGIN		
		
		IF(@Increment < 10)
			SET @CounterId = @LocationIdentifier + '0' + CAST(@Increment as varchar)
		ELSE
			SET @CounterId = @LocationIdentifier + CAST(@Increment as varchar)
		 
		INSERT INTO [dbo].[tLocationCounterIdDetails]
		([rowguid] ,[LocationId] ,[ProviderId]   ,[CounterId]  ,[IsAvailable]  ,[DTCreate]  ,[DTLastMod])
		VALUES (NEWID(),@LocationId,@SendMoneyProviderId,@CounterId,1,getdate(),getdate())		   
	  
	   INSERT INTO [dbo].[tLocationCounterIdDetails]
	   ([rowguid] ,[LocationId] ,[ProviderId]   ,[CounterId]  ,[IsAvailable]  ,[DTCreate]  ,[DTLastMod])
		VALUES (NEWID(),@LocationId,@BillPayProviderId,@CounterId,1,getdate(),getdate())

		SET @Increment = @Increment + 1;
		
		END;
	END	
END

GO
----------------------------------------
