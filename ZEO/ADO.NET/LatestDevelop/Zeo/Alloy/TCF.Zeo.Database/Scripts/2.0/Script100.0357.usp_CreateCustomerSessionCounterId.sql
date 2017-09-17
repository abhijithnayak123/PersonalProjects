--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <18-11-2016>
-- Description:	Create customer Session counter id 
-- Jira ID:		<AL-8320>
-- ================================================================================

IF OBJECT_ID('usp_CreateCustomerSessionCounterId') IS NOT NULL
BEGIN
	DROP PROCEDURE dbo.usp_CreateCustomerSessionCounterId
END
GO

CREATE PROCEDURE usp_CreateCustomerSessionCounterId
(     @counterIdValue    BIGINT OUTPUT,
	  @providerId        INT,																	
	  @customerSessionId BIGINT,																	
	  @locationId        BIGINT,																	
	  @dtServerDate      DATETIME,																	
	  @dtTerminalDate    DATETIME
 )
AS
BEGIN
	BEGIN TRY
		
			DECLARE @counterId BIGINT
			DECLARE @customerSessionPk UNIQUEIDENTIFIER

		
		  --Getting the CustomerSessionPk from tCustomerSessions Table
			SELECT 
				@customerSessionPk = CustomerSessionPK 
			FROM 
				dbo.tCustomerSessions 
			WHERE 
				CustomerSessionID = @customerSessionId

		  --Getting the counter Id from the tCustomerSessionCounterIdDetails with the customerSessionId
			SELECT 
				@counterId = tl.CounterId
			FROM 
				dbo.tCustomerSessionCounterIdDetails tcl WITH (NOLOCK)
				INNER JOIN dbo.tLocationCounterIdDetails tl ON tl.CounterId = tcl.CounterId
			WHERE 
				tcl.CustomerSessionID = @customerSessionId
				AND tl.ProviderId = @providerId
				AND tl.LocationId = @locationId
				AND tl.IsAvailable = 0

			IF @counterId IS NULL
			BEGIN
				--Getting the available counter ID from tLocationCounterIdDetails
				SELECT TOP 1 
					@counterId = tlc.CounterId
				FROM 
					dbo.tLocationCounterIdDetails tlc WITH (NOLOCK)
				WHERE 
					ProviderId = @providerId
					AND LocationId = @locationId
					AND tlc.IsAvailable = 1
				ORDER BY 
					tlc.DTServerLastModified DESC

				--Update the IsAvailable status in tLocationCounterIdDetails to 0
				EXECUTE usp_UpdateLocationCounterIdStatus
						@locationId,
						@providerId,
						@counterId,
						0,
						@dtTerminalDate

				--Insert into tCustomerSessionCounterIdDetails table
				INSERT INTO dbo.tCustomerSessionCounterIdDetails
				(
					CounterId,
					DTServerCreate,
					DTTerminalCreate,
					CustomerSessionID,
					CustomerSessionPk
				)
				VALUES
				(
					@counterId,
					@dtServerDate,
					@dtTerminalDate,
					@customerSessionId,
					@customerSessionPK
				)
			END

				SELECT @counterIdValue =@counterId

		  
	 END TRY

	 BEGIN CATCH

		EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError
	 END CATCH

END
GO
