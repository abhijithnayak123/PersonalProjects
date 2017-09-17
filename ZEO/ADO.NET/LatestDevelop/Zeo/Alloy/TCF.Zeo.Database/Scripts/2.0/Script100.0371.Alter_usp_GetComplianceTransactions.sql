--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <09-21-2016>
-- Description:	 Stored procedure to fetch compliance transaction
-- Jira ID:		<AL-8195>
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_GetComplianceTransactions'
)
BEGIN
	DROP PROCEDURE usp_GetComplianceTransactions
END
GO

CREATE PROCEDURE usp_GetComplianceTransactions
	@TransactionType INT,
	@Period INT,
	@CustomerSessionId BIGINT,
	@ShouldIncludeShoppingCartItems BIT

AS
BEGIN
	BEGIN TRY
		
		DECLARE @xTotalAmount MONEY
		
		DECLARE @xActiveTrx MONEY
		
		SELECT @xTotalAmount = SUM(Amount)   
			FROM vComplianceTransactions ct
		INNER JOIN tCustomers c ON c.CustomerID = ct.CustomerId
		INNER JOIN tCustomerSessions cs ON cs.CustomerId = c.CustomerID
		WHERE cs.CustomerSessionID = @CustomerSessionId AND TransactionType = @TransactionType AND ct.DTTerminalCreate >= DATEADD(DAY, -@Period, GETDATE())	
		
		IF @ShouldIncludeShoppingCartItems = 1
			BEGIN

				DECLARE @CartId BIGINT = dbo.ufn_GetShoppingCartId(@CustomerSessionId,1)

				--IF @TransactionType = 1
				--	BEGIN
				--		SELECT @xActiveTrx = SUM(tcash.Amount) FROM tShoppingCartTransactions sct
				--		JOIN tTxn_Cash tcash ON sct.TxnId = tcash.TransactionID WHERE sct.CartId = @CartId AND sct.CartItemStatus = 1 AND tcash.CashType = @TransactionType
				--	END

				IF @TransactionType = 2
					BEGIN
						SELECT @xActiveTrx = SUM(tcheck.Amount) FROM tShoppingCartTransactions sct
						JOIN tTxn_Check tcheck ON sct.TransactionId = tcheck.TransactionID WHERE sct.CartId = @CartId AND sct.CartItemStatus = 1
					END
				
				--ELSE IF @TransactionType = 3
				--	BEGIN
				--		SELECT @xActiveTrx = SUM(tfund.Amount) FROM tShoppingCartTransactions sct
				--		JOIN tTxn_Funds tfund ON sct.TxnId = tfund.TransactionID WHERE sct.CartId = @CartId AND sct.CartItemStatus = 1 AND tfund.FundType = 0
				--	END
				
				ELSE IF @TransactionType = 4
					BEGIN
						SELECT @xActiveTrx = SUM(tbp.Amount) FROM tShoppingCartTransactions sct
						JOIN tTxn_BillPay tbp ON sct.TransactionId = tbp.TransactionID WHERE sct.CartId = @CartId AND sct.CartItemStatus = 1
					END

				ELSE IF @TransactionType = 5
					BEGIN
						SELECT @xActiveTrx = SUM(tmo.Amount) FROM tShoppingCartTransactions sct
						JOIN tTxn_MoneyOrder tmo ON sct.TransactionId = tmo.TransactionID WHERE sct.CartId = @CartId AND sct.CartItemStatus = 1
					END
				
				ELSE IF @TransactionType = 6
					BEGIN
						SELECT @xActiveTrx = SUM(tmt.Amount) FROM tShoppingCartTransactions sct
						JOIN tTxn_MoneyTransfer tmt ON sct.TransactionId = tmt.TransactionID WHERE TransferType = 1 AND sct.CartId = @CartId AND sct.CartItemStatus = 1
					END
				
				--ELSE IF @TransactionType = 7
				--	BEGIN
				--		SELECT @xActiveTrx = SUM(tfund.Amount) FROM tShoppingCartTransactions sct
				--		JOIN tTxn_Funds tfund ON sct.TxnId = tfund.TransactionID WHERE sct.CartId = @CartId AND sct.CartItemStatus = 1 AND tfund.FundType = @TransactionType
				--	END
				
				ELSE IF @TransactionType = 8
					BEGIN
						SELECT @xActiveTrx = SUM(tfund.Amount) FROM tShoppingCartTransactions sct
						JOIN tTxn_Funds tfund ON sct.TransactionId = tfund.TransactionID WHERE sct.CartId = @CartId AND sct.CartItemStatus = 1 AND tfund.FundType = 1
					END
				
				--ELSE IF @TransactionType = 9
				--	BEGIN
				--		SELECT @xActiveTrx = SUM(tfund.Amount) FROM tShoppingCartTransactions sct
				--		JOIN tTxn_Funds tfund ON sct.TxnId = tfund.TransactionID WHERE sct.CartId = @CartId AND sct.CartItemStatus = 1 AND tfund.FundType = 2
				--	END
				
				ELSE IF @TransactionType = 10
					BEGIN
						SELECT @xActiveTrx = SUM(tfund.Amount) FROM tShoppingCartTransactions sct
						JOIN tTxn_Funds tfund ON sct.TransactionId = tfund.TransactionID WHERE sct.CartId = @CartId AND sct.CartItemStatus = 1 AND tfund.FundType = 0
					END
			END

		
		SELECT CAST(@xTotalAmount + @xActiveTrx AS decimal) AS 'xDayTrxsTotalAmount'

	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END