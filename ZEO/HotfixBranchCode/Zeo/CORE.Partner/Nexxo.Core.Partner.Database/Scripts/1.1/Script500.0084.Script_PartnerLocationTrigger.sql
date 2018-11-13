-- ================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <08/04/2015>
-- Description:	<Alter Trigger date columns to DTTerminalCreate, DTTerminalLastModified>
-- Jira ID:		<AL-617>
-- ================================================================================

IF EXISTS( 
	SELECT 1
	FROM SYS.TRIGGERS
	WHERE NAME = 'trgLocationCounterIdInsertUpdate'
)
BEGIN
	DROP TRIGGER [dbo].[trgLocationCounterIdInsertUpdate]
	END
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
	DECLARE @DTTerminalCreate DATETIME
	SET @Increment = 1;
	SET @Count = 20;
	SET @SendMoneyProviderId = 301
	SET @BillPayProviderId = 401
	SET @CounterId=''
	
	SELECT @LocationId=LocationId, @LocationIdentifier=Identifier, @DTTerminalCreate = DTTerminalCreate FROM inserted
	
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
		([LocationCounterIdDetailPK] ,[LocationId] ,[ProviderId]   ,[CounterId]  ,[IsAvailable]  ,[DTServerCreate]  ,[DTServerLastModified],[DTTerminalCreate])
		VALUES (NEWID(),@LocationId,@SendMoneyProviderId,@CounterId,1,getdate(),getdate(),@DTTerminalCreate)		   
	  
	   INSERT INTO [dbo].[tLocationCounterIdDetails]
	   ([LocationCounterIdDetailPK] ,[LocationId] ,[ProviderId]   ,[CounterId]  ,[IsAvailable]  ,[DTServerCreate]  ,[DTServerLastModified],[DTTerminalCreate])
		VALUES (NEWID(),@LocationId,@BillPayProviderId,@CounterId,1,getdate(),getdate(),@DTTerminalCreate)

		SET @Increment = @Increment + 1;
		
		END;
	END	
END
GO


