-- ================================================================================
-- Author:		Manikandan Govindraj
-- Create date: 03/24/2017
-- Description: check the permission to unpark the shopping cart transactions
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
	 
	 DECLARE @canUnPark BIT = dbo.ufn_CanUnParkTransactions( @customerSessionId )

	 IF(@canUnPark = 1)
	 BEGIN

        UPDATE sc
	     SET
	       sc.State = 1,
	    	sc.CustomerSessionId = @customerSessionId
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
	END

END TRY
BEGIN CATCH

   EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError

END CATCH

END   
GO