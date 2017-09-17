-- ================================================================================
-- Author:		Manikandan Govindraj
-- Create date: 12/07/2016
-- Description: Remove the shopping cart transaction
-- Jira ID:		AL-8047
-- ================================================================================



IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'usp_RemoveShoppingCartTransaction')
BEGIN
    DROP PROCEDURE usp_RemoveShoppingCartTransaction
END
GO


CREATE PROCEDURE usp_RemoveShoppingCartTransaction
(
	@transactionId  BIGINT,
	@productId      INT
)
AS
BEGIN
   BEGIN TRY

     if( @productId ! = 3 ) -- If Not MoneyTransfer
	 BEGIN

        UPDATE
	      tShoppingCartTransactions
	    SET
	      CartItemStatus = 1   -- Remove status	   
	    WHERE 
	      TransactionId = @transactionId
	      AND
	      ProductId = @productId 

     END
	 ELSE       --- Remove Money Transfer (Send Money, Receive Money, Refund, Modify Send Money)
	 BEGIN

	    IF EXISTS
		     ( 
			     SELECT 1
				 FROM 
				   tTxn_MoneyTransfer
			     WHERE 
			       TransactionID = @transactionId AND 
			       TransactionSubType IS NULL AND (TransferType = 1 OR TransferType = 2)
			 )
			 BEGIN
				 UPDATE
				   tShoppingCartTransactions
				 SET
				   CartItemStatus = 1   -- Remove status	   
				 WHERE 
				   TransactionId = @transactionId
				   AND
				   ProductId = @productId 
			END
			ELSE
			BEGIN
			     UPDATE sct
				 SET 
				   sct.CartItemStatus = 1   -- Remove status	
				 FROM  
				   tShoppingCartTransactions sct WITH (NOLOCK)
				   INNER JOIN
				   tTxn_MoneyTransfer mta WITH (NOLOCK)
				 ON 
				   sct.TransactionId = mta.TransactionID
                   INNER JOIN tTxn_MoneyTransfer mtb
				 ON
				   mta.OriginalTransactionId = mtb.OriginalTransactionId
				 WHERE
				   mtb.TransactionID = @transactionId  and sct.productid = @productId
			END
	 END

   END TRY

   BEGIN CATCH      
      EXECUTE usp_CreateErrorInfo
   END CATCH

END
GO


