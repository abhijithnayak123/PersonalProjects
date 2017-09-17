-- ================================================================================
-- Author:		Manikandan Govindraj
-- Create date: 12/07/2017
-- Description: can close customer session
-- Jira ID:		AL-8047
--exec usp_CanCloseCustomerSession 1000000005,null,null
-- ================================================================================

IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'usp_CanCloseCustomerSession')
BEGIN
	DROP PROCEDURE usp_CanCloseCustomerSession
END
GO


CREATE PROCEDURE usp_CanCloseCustomerSession
(
	@customerSessionId     BIGINT,
	@dTTerminalModified    DATETIME,
	@dTServerModified      DATETIME	
)
AS
BEGIN
	
	BEGIN TRY

	    SET NOCOUNT ON

		DECLARE @isCartItemPresent BIT = 0	

		DECLARE @refundCount INT = 
			(
				SELECT COUNT(1)
				FROM 
	    			tShoppingCarts sc WITH (NOLOCK) 
					INNER JOIN tShoppingCartTransactions sct WITH (NOLOCK) 
	    			 ON sc.CartId = sct.CartId
					INNER JOIN ttxn_MoneyTransfer ttm WITH (NOLOCK) 
					 ON ttm.TransactionID = sct.TransactionId
				WHERE 
					sc.CustomerSessionId = @customerSessionId
					AND
					sc.State = 1        -- Active
					AND
					sct.CartItemStatus = 0  --added
					AND
					ttm.TransactionSubType = 3 --refund					
					AND 
					sct.ProductId = 3 --MoneyTransfer	
			) 
		
		  DECLARE @shoppingcartCount INT =
		   ( 
				SELECT COUNT(1)
				FROM 
	    			tShoppingCarts sc WITH (NOLOCK) 
					INNER JOIN tShoppingCartTransactions sct WITH (NOLOCK) 
	    			 ON sc.CartId = sct.CartId
				WHERE 
	    			sct.ProductId != 7  -- Cash
	    			AND
					sc.CustomerSessionId = @customerSessionId
					AND
					sc.State = 1        -- Active
					AND
					sct.CartItemStatus = 0  --added
			)

			--Doing this check to see if we have any transactions in the shoppingcart other than Refund transactions
			IF @shoppingcartCount > @refundCount
			BEGIN
				SET @isCartItemPresent = 1
			END		

		SELECT @isCartItemPresent as IsCartItemPresent

    END TRY

BEGIN CATCH

	EXECUTE usp_CreateErrorInfo
		
END CATCH
END
GO

