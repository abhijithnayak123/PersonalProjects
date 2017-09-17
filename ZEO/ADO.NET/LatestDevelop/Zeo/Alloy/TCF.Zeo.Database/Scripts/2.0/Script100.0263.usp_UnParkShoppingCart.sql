-- ================================================================================
-- Author:		M.Purna Pushkal
-- Create date: 12/19/2016
-- Description: Unpark shopping cart
-- Jira ID:		AL-8952
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
	 -- create function to check the user's permissions once agent module implemented.

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
		sc.State = 2
		AND
		cs.CustomerId = @customerId
		AND
		tc.ProfileStatus != 2 --Closed

END TRY
BEGIN CATCH

   EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError

END CATCH

END   
GO