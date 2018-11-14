--===========================================================================================
-- Author:		Shwetha Mohan
-- Created date: 12/08/2016
-- Description:	< Made changes from LocationPK(UNIQUEIDENTIFIER) to LocationId(BIGINT)>           
-- Jira ID:	<AL-7582>

--After ADO.NEt changes we have Alter the trigger as per LocationID column changes
--===========================================================================================
IF OBJECT_ID(N'[trgLocationCounterIdInsertUpdate]', N'TR') IS NOT NULL
DROP TRIGGER [trgLocationCounterIdInsertUpdate]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trgLocationCounterIdInsertUpdate] ON [dbo].[tLocationProcessorCredentials] 
AFTER INSERT,UPDATE
AS
BEGIN

	SET NOCOUNT ON

	DECLARE @locationIdentifier nvarchar(50)
	DECLARE @locationId BIGINT 
	DECLARE @increment int
	DECLARE @count int
	DECLARE @sendMoneyProviderId int
	DECLARE @billPayProviderId int
	DECLARE @counterId nvarchar(50)
	SET @increment = 1;
	SET @count = 20;
	SET @sendMoneyProviderId = 301
	SET @billPayProviderId = 401
	SET @counterId=''
	DECLARE @provider int
	
	SELECT @locationId=LocationId, @locationIdentifier=Identifier,@provider = ProviderId FROM INSERTED
	
	if(@locationIdentifier IS NOT NULL AND @locationIdentifier <> '' AND UPDATE(Identifier) AND @provider = 401)
	BEGIN
		SELECT @count = (SELECT NoOfCounterIds FROM tLocations WHERE LocationID = @locationId)
		
		DELETE FROM tLocationCounterIdDetails where LocationId = @locationId
	
		WHILE @increment <= @count
		BEGIN		
		
			IF(@increment < 10)
				SET @counterId = @locationIdentifier + '0' + CAST(@increment AS VARCHAR)
			ELSE
				SET @counterId = @locationIdentifier + CAST(@increment AS VARCHAR)
		 
		   INSERT INTO [dbo].[tLocationCounterIdDetails]
		   ([LocationId] ,[ProviderId],[CounterId],[DTServerCreate] ,[DTTerminalCreate])
		   VALUES (@locationId,@sendMoneyProviderId,@counterId,GETDATE(),GETDATE())		   
	  
		   INSERT INTO [dbo].[tLocationCounterIdDetails]
		   ([LocationId] ,[ProviderId]  ,[CounterId] ,[DTServerCreate] ,[DTTerminalCreate])
			VALUES (@locationId,@billPayProviderId,@counterId,GETDATE(),GETDATE())

			SET @increment = @increment + 1;
		
		END;
	END	
END