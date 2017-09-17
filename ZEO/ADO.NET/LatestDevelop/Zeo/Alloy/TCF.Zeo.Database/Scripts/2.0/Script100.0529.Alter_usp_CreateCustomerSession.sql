-- ================================================================================
-- Author:		Manikandan Govindraj
-- Create date: 05/04/2017
-- Description:	Modified the SP to add Identification informations
-- Jira ID:		<>
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_CreateCustomerSession'
)
BEGIN
	DROP PROCEDURE usp_CreateCustomerSession
END
GO

CREATE PROCEDURE [dbo].[usp_CreateCustomerSession]
	@agentSessionId BIGINT,
	@customerId BIGINT,
	@timeZone VARCHAR(200),
	@dtServerCreate DATETIME,
	@dtTerminalCreate DATETIME,
	@cardSearchType INT
AS
BEGIN
	BEGIN TRY

	DECLARE @customerSessionId BIGINT 
	


	DECLARE @profileStatus BIT
	DECLARE @channelPartnerId BIGINT
	
		SELECT @profileStatus = ProfileStatus,
			   @channelPartnerId = ChannelPartnerId
		FROM tCustomers
		WHERE CustomerID = @customerId
	 
	DECLARE @cardPresent BIT = 
	(
		SELECT 
			CASE WHEN 
				(CardPresenceVerificationConfig = 1 OR CardPresenceVerificationConfig = 2) AND 
				(@cardSearchType = 1 OR @cardSearchType = 2) THEN 1 ELSE 0 END 
		FROM tChannelPartners
		WHERE ChannelPartnerId = @channelPartnerId
	)



	DECLARE @cardNumber VARCHAR(50)
	DECLARE @isGPRCustomer INT
	SELECT 
		@cardNumber = CardNumber,
		@isGPRCustomer = CASE WHEN ((CardNumber) IS NULL )THEN 0 ELSE 1 END
	FROM 
		tVisa_Account
	WHERE 
		CustomerId = @customerId AND Activated = 1

	INSERT INTO tCustomerSessions
           (
		    CustomerID
           ,DTServerCreate
           ,DTStart
           ,CardPresent
           ,TimezoneID
		   ,AgentSessionId
           ,DTTerminalCreate)
     VALUES(
			 @customerId
			,@dtServerCreate
			,GETDATE()
			,@cardPresent
			,@timeZone
			,@agentSessionId
			,@dtTerminalCreate
		)

	SELECT @customerSessionId = SCOPE_IDENTITY()

	SELECT 
	   CAST(@customerSessionId AS BIGINT) AS CustomerSessionID
	  ,CAST(@isGPRCustomer AS BIT) AS IsGPRCustomer
	  ,CAST(@cardPresent AS BIT) AS CardPresent
	  ,@cardNumber AS CardNumber
	  ,CAST(@profileStatus AS VARCHAR(50)) AS ProfileStatus

	EXEC usp_UnParkShoppingCart @customerSessionId, @customerId


	--===================================================== Add Identification Information ========================

	DECLARE @agentId BIGINT = 
	(
		SELECT AgentId FROM tAgentSessions WHERE AgentSessionId = @agentSessionId			
	)
	 
	INSERT INTO tIdentificationConfirmation
										(
										AgentID,
										CustomerSessionID,
										DateIdentified,
										ConfirmStatus,
										DTServerCreate
										)
									VALUES
									    (
										 @agentId,
										 @customerSessionId,
										 @dtServerCreate,
										 1,   -- true
										 @dtServerCreate
										)

    --===================================================== End ================================================

END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END