-- ================================================================================
-- Author:		Manikandan Govindraj
-- Create date: 04/22/2017
-- Description: Modify the migration script as per the product enum changes.
-- Jira ID:		
-- ================================================================================

BEGIN TRY

    BEGIN TRAN
    
		--=================== Migration scripts for tShoppingCart's table ======================================

		 --ProductID  ProductName
		 --=========  =========== 
         --   1	      ProcessCheck
         --   2	      BillPayment
         --   3	      MoneyTransfer
         --   5	      MoneyOrder
         --   6	      GPR
		 --   7       Cash
		 -- ================================================================================

         UPDATE sct
         SET 
           sct.TransactionId = tc.TransactionId,
           sct.ProductId = 7
         FROM 
           tShoppingCartTransactions sct
         INNER JOIN 
           tTxn_Cash tc 
         ON
           sct.TxnPK = tc.TxnPK
         
         
         
         --============ Update the check processing transaction Id in tShoppingCartTransactions table ==================================
         
         UPDATE sct
         SET 
           sct.TransactionId = tc.TransactionId,
           sct.ProductId = 1
         FROM 
           tShoppingCartTransactions sct
         INNER JOIN 
           tTxn_Check tc 
         ON
           sct.TxnPK = tc.TxnPK
         
         
         --============ Update the Bill Pay transaction Id in tShoppingCartTransactions table ==================================
         
         UPDATE sct
         SET 
           sct.TransactionId = bp.TransactionId,
           sct.ProductId = 2
         FROM 
           tShoppingCartTransactions sct
         INNER JOIN 
           tTxn_BillPay bp
         ON
           sct.TxnPK = bp.TxnPK
         
         
         --============ Update the Money Transfer transaction Id in tShoppingCartTransactions table ==================================
         
         UPDATE sct
         SET 
           sct.TransactionId = mt.TransactionId,
           sct.ProductId = 3
         FROM 
           tShoppingCartTransactions sct
         INNER JOIN 
           tTxn_MoneyTransfer mt
         ON
           sct.TxnPK = mt.TxnPK
         
         
         --============ Update the Money Order transaction Id in tShoppingCartTransactions table ==================================
         
         UPDATE sct
         SET 
           sct.TransactionId = mo.TransactionId,
           sct.ProductId = 5
         FROM 
           tShoppingCartTransactions sct
         INNER JOIN 
           tTxn_MoneyOrder mo
         ON
           sct.TxnPK = mo.TxnPK
         
         
         --============ Update the GPR Card transaction Id in tShoppingCartTransactions table ==================================
         
         UPDATE sct
         SET 
           sct.TransactionId = tf.TransactionId,
           sct.ProductId = 6
         FROM 
           tShoppingCartTransactions sct
         INNER JOIN 
           tTxn_Funds tf
         ON
           sct.TxnPK = tf.TxnPK     
		

		COMMIT TRAN
END TRY

BEGIN CATCH
	 IF(@@TRANCOUNT > 0)
		SELECT
		ERROR_NUMBER() AS ErrorNumber
		,ERROR_SEVERITY() AS ErrorSeverity
		,ERROR_STATE() AS ErrorState
		,ERROR_PROCEDURE() AS ErrorProcedure
		,ERROR_LINE() AS ErrorLine
		,ERROR_MESSAGE() AS ErrorMessage,
		XACT_STATE()as state;
		ROLLBACK TRAN
END CATCH;

 