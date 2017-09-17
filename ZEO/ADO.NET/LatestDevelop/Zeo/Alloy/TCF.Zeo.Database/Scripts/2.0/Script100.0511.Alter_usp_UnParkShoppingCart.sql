-- ================================================================================
-- Author     :	Manikandan Govindraj
-- Create date: 03/24/2017
-- Description: Update customersessionId when UnPark the transactions
-- ID         :	126
-- ================================================================================

IF OBJECT_ID(N'usp_UnParkShoppingCart', N'P') IS NOT NULL
BEGIN
    DROP PROCEDURE usp_UnParkShoppingCart   -- Drop the existing procedure.
END
GO


CREATE PROCEDURE usp_UnParkShoppingCart
(
    @customerSessionId BIGINT,
    @customerId        BIGINT
)
AS
BEGIN
    
BEGIN TRY
	 
	 -- Check the permission to un Park transactions
	 DECLARE @canUnPark BIT = dbo.ufn_CanUnParkTransactions( @customerSessionId )

	 IF(@canUnPark = 1)
	 BEGIN

       -- Get Parked shopping cart Id
	   DECLARE @cartId BIGINT =
	   (
	        SELECT 
               sc.CartID
            FROM 
	        	dbo.tShoppingCarts sc WITH (NOLOCK)
	        	INNER JOIN tCustomerSessions cs WITH (NOLOCK) 
	        		ON sc.CustomerSessionId = cs.CustomerSessionID
	        	INNER JOIN tCustomers tc WITH (NOLOCK) 
	        		ON	tc.CustomerID = cs.CustomerID
            WHERE 
	        	sc.State = 2 -- Parked
	        	AND
	        	cs.CustomerId = @customerId
	        	AND
	        	tc.ProfileStatus != 2 --Closed
        )


		-- If parked shopping cart available, Unpark shopping cart and update the CustomerSessionId for the transactions

		IF(@cartId IS NOT NULL) 
		BEGIN

			  UPDATE tShoppingCarts
			   SET
			     State = 1, -- Active
			     CustomerSessionId = @customerSessionId      
			  WHERE 
			     CartId = @cartId
        	  
			  
			  UPDATE tc
			     SET tc.CustomerSessionId = @customerSessionId
			   FROM
			       tShoppingCartTransactions sct WITH (NOLOCK)
			  INNER JOIN
			       tTxn_Check tc WITH (NOLOCK)
			     ON sct.TransactionId = tc.TransactionId 	
			  WHERE
			  	sct.CartId = @cartId
			  	AND
			  	sct.ProductId = 1 --Check
			  
			  
			  UPDATE bp
			     SET bp.CustomerSessionId = @customerSessionId
			  FROM tTxn_BillPay bp 
			  INNER JOIN
			       tShoppingCartTransactions sct 
			  	 ON sct.TransactionId = bp.TransactionId
			  WHERE 
			  	sct.CartId = @cartId 
			  	AND 
			  	sct.ProductId = 2 --BillPay
        	  
			  
			  UPDATE mo
			    SET mo.CustomerSessionId = @customerSessionId				
			  FROM
			      tTxn_MoneyOrder mo			
			  INNER JOIN 
			      tShoppingCartTransactions sct
			      ON mo.TransactionId = sct.TransactionId
			  WHERE 
			  	sct.CartId = @cartId 
			  	AND 
			  	sct.ProductId = 5 --MoneyOrder
			  
			  
			  UPDATE mt
			    SET mt.CustomerSessionId = @customerSessionId	
			  FROM 
			       tTxn_MoneyTransfer mt 	
			  INNER JOIN
			       tShoppingCartTransactions sct 
			  	 ON sct.TransactionId = mt.TransactionId		
			  WHERE 
			  	 sct.CartId = @cartId 
			  	 AND 
			  	 sct.ProductId = 3 -- Money Transfer

		END
	END

END TRY
BEGIN CATCH

   EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError

END CATCH

END   
GO

        