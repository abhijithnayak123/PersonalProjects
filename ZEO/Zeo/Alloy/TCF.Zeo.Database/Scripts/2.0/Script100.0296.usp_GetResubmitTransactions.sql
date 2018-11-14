-- ================================================================================
-- Author:		M.Purna Pushkal
-- Create date: 02/02/2017
-- Description: To get the Check and money order transactions
-- Jira ID:		AL-8047
-- ================================================================================

IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'usp_GetResubmitTransactions')
BEGIN
	DROP PROCEDURE usp_GetResubmitTransactions
END
GO

CREATE PROCEDURE usp_GetResubmitTransactions 
(
	 @customerSessionId BIGINT,
	 @productId			  INT
)
AS
BEGIN

BEGIN TRY

	 SELECT 
		  sct.TransactionId
	 FROM 
		  tShoppingCartTransactions sct 
		  INNER JOIN 
		  tShoppingCarts sc ON sc.CartId = sct.CartID 
	 WHERE 
		  sc.State = 1 --Active
		  AND
		  sc.CustomerSessionId = @customerSessionId
		  AND
		  sct.ProductId = @productId
		  AND
		  sct.CartItemStatus = 0 --Added

END TRY

BEGIN CATCH
	 EXECUTE usp_CreateErrorInfo
END CATCH

END