--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to update trgLocationCounterIdInsertUpdate trigger>           
-- Jira ID:	<AL-242>
--===========================================================================================

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TRIGGER [dbo].[trgLocationCounterIdInsertUpdate] ON [dbo].[tLocationProcessorCredentials] 
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
		
		SELECT @Count = (SELECT NoOfCounterIds FROM tLocations WHERE LocationPK = @LocationId)
		
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