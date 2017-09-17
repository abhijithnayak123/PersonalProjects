-- ================================================================================
-- Author:		Manikandan Govindraj
-- Create date: 12/07/2016
-- Description: Migration scripts for tShopping cart
-- Jira ID:		AL-8047
-- ================================================================================

--=================== Migration scripts for tShoppingCart's table ======================================


DISABLE TRIGGER trShoppingCartsAudit
ON tShoppingCarts
GO

BEGIN TRY
    BEGIN TRAN

        -- Update the CustomerSessionId in tShoppingCart table
        
		UPDATE sc 
        SET 
          sc.CustomerSessionId = sct.CustomerSessionId
        FROM
          tShoppingCarts sc
       	INNER JOIN
			(
				SELECT 
				    CartPK,
				    MAX(CustomerSessionId) CustomerSessionId
				FROM 
				    tCustomerSessionShoppingCarts
				GROUP BY
				    CartPK
			) AS sct
        ON 
          sct.CartPK = sc.CartPK    
      
        
        -- below are Shopping cart state and Update the shopping cart state
        
        -- 1 : Active
        -- 2 : Parked
        -- 3 : Closed
        
        
        UPDATE tShoppingCarts
        SET State = 
		  (
             CASE  
                IsParked WHEN 1 THEN 2 
             ELSE 
                 CASE 
             	   Active WHEN 0 THEN 3
                 ELSE 1
                 END
             END
		  )
		 
		 
		 UPDATE tc
		 SET
		    tc.CustomerSessionId = cs.CustomerSessionId
		 FROM
		    tTxn_Cash tc 
		 INNER JOIN
		    tCustomerSessions cs
		 ON 
		    tc.CustomerSessionPK = cs.CustomerSessionPK


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

ENABLE TRIGGER trShoppingCartsAudit
ON tShoppingCarts
GO



--=================== Migration scripts for tShoppingCartTransaction's table ======================================

BEGIN TRY

    BEGIN TRAN

         UPDATE sct
         SET 
          sct.CartId = sc.CartId
         FROM 
          tShoppingCartTransactions sct
         INNER JOIN
          tShoppingCarts sc
         ON
          sct.CartPK = sc.CartPK         
         
        
		 --ProductID  ProductName
		 --=========  ===========
         --   1       Cash 
         --   2	      ProcessCheck
         --   3	      BillPayment
         --   4	      MoneyTransfer
         --   5	      MoneyOrder
         --   6	      GPR

         --============ Update the cash transaction Id in tShoppingCartTransactions table ==================================

         UPDATE sct
         SET 
           sct.TransactionId = tc.TransactionId,
           sct.ProductId = 1
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
           sct.ProductId = 2
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
           sct.ProductId = 3
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
           sct.ProductId = 4
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
--=================== Alter column not nullable ===================================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCarts' AND COLUMN_NAME = 'CustomerSessionId')
BEGIN
	ALTER TABLE tShoppingCarts
	ALTER COLUMN CustomerSessionId BIGINT NOT NULL
END

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCarts' AND COLUMN_NAME = 'State')
BEGIN
	ALTER TABLE tShoppingCarts
	ALTER COLUMN State INT NOT NULL
END

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_Cash' AND COLUMN_NAME = 'CustomerSessionId')
BEGIN
	ALTER TABLE tTxn_Cash
	ALTER COLUMN CustomerSessionId BIGINT NOT NULL
END

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCartTransactions' AND COLUMN_NAME = 'CartId')
BEGIN
	ALTER TABLE tShoppingCartTransactions
	ALTER COLUMN CartId BIGINT NOT NULL
END

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCartTransactions' AND COLUMN_NAME = 'TransactionId')
BEGIN
	ALTER TABLE tShoppingCartTransactions
	ALTER COLUMN TransactionId BIGINT NOT NULL
END

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCartTransactions' AND COLUMN_NAME = 'ProductId')
BEGIN
	ALTER TABLE tShoppingCartTransactions
	ALTER COLUMN ProductId BIGINT NOT NULL
END
GO
