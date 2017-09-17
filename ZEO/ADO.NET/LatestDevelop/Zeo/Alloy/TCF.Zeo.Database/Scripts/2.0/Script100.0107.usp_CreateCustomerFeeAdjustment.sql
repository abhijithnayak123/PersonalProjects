--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <08-02-2016>
-- Description:	 Stored procedure to create customer fee adjustment
-- Jira ID:		<AL-7926>
-- ================================================================================

IF EXISTS (SELECT  1 FROM sys.objects WHERE NAME = 'usp_CreateCustomerFeeAdjustment')
BEGIN
	DROP PROCEDURE usp_CreateCustomerFeeAdjustment
END
GO

CREATE PROCEDURE usp_CreateCustomerFeeAdjustment
	@IsAvailed BIT,
	@DTTerminalCreate DATETIME,
	@DTServerCreate DATETIME,
	@CustomerSessionId BIGINT

AS
BEGIN
	
	BEGIN TRY

			DECLARE @customerId BIGINT
			DECLARE @feeAdjustmentId BIGINT 

			SELECT 
				 @customerId = c.CustomerID, @feeAdjustmentId = cf.FeeAdjustmentId
			FROM 
				tCustomers c 
			INNER JOIN 
				tCustomerSessions cs ON cs.CustomerID = c.CustomerID
			INNER JOIN 
				tChannelPartnerFeeAdjustments cf ON cf.ChannelPartnerId = c.ChannelPartnerId
			WHERE 
				cs.CustomerSessionID = @customerSessionId AND LOWER(cf.PromotionType) = 'referral'
		
		IF @feeAdjustmentId > 0
		BEGIN
			INSERT INTO tCustomerFeeAdjustments
				(CustomerId, IsAvailed, FeeAdjustmentId, DTServerCreate ,DTTerminalCreate ,DTTerminalLastModified ,DTServerLastModified)
			VALUES
				(@customerId, @IsAvailed, @feeAdjustmentId, @DTServerCreate, @DTTerminalCreate, NULL, NULL)
		END
	
	END TRY
	
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END
GO