-- ================================================================================
-- Author:		Manikandan Govindraj
-- Create date: 12/07/2016
-- Description: Cancel cashIn transactions
-- Jira ID:		AL-8047
-- ================================================================================

IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'usp_CancelCashIn')
BEGIN
	DROP PROCEDURE usp_CancelCashIn
END
GO


CREATE PROCEDURE usp_CancelCashIn
(
	@customerSessionId     BIGINT,
	@dTTerminalModified    DATETIME,
	@dTServerModified      DATETIME	
)
AS
BEGIN
	
	BEGIN TRY

	    SET NOCOUNT ON
	    

			     DECLARE @productId INT = 7   -- Cash


				 -- Cancel the cash and update the amount as '0'

				 UPDATE c
				     SET 
					    c.State = 6,   -- cancel
						c.Amount = 0,      -- Update Amount as '0'
						c.DTTerminalLastModified = @dTTerminalModified,
						c.DTServerLastModified = @dTServerModified
				 FROM 
				     tTxn_Cash c WITH (NOLOCK)
				 INNER JOIN 
					 tShoppingCartTransactions sct WITH (NOLOCK) 
						  ON c.TransactionID = sct.TransactionId
				 INNER JOIN 
				     tShoppingCarts sc WITH (NOLOCK) 
						  ON sc.CartID = sct.CartId
				 WHERE
				      sct.ProductId = @productId 
					  AND 
					  c.CustomerSessionId = @customerSessionId 
					  AND 
					  c.State = 2 --Authorised
					 
					 
			     --  remove the cash from shopping cart

				 UPDATE sct
				     SET 
					     sct.CartItemStatus = 1 -- Removed state
				 FROM 
				     tTxn_Cash c
				 INNER JOIN 
					 tShoppingCartTransactions sct WITH (NOLOCK) 
						  ON c.TransactionID = sct.TransactionId
				 INNER JOIN 
				     tShoppingCarts sc WITH (NOLOCK) 
						  ON sc.CartID = sct.CartId
				 WHERE
				      sct.ProductId = @productId 
					  AND 
					  c.CustomerSessionId = @customerSessionId 	
					  AND 
					  c.State = 6 --Cancelled

    END TRY

BEGIN CATCH

    EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError
		
END CATCH
END
GO

