-- =====================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <01/17/2018>
-- Description:	<Getting the transaction count based on the Product Id>
-- =====================================================================

-- select dbo.ufn_GetTransactionCount(100000001, 1, 200, 0,  0, 2540247527168537, '2018-01-01', '2018-02-02')

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'ufn_GetTransactionCount') AND xtype IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION ufn_GetTransactionCount
GO

CREATE FUNCTION ufn_GetTransactionCount
(
	 @customerSessionId BIGINT
	,@productId BIGINT
	,@providerId BIGINT
	,@transactionId BIGINT
	,@isNextCustomerSession BIT
	,@customerId BIGINT
	,@startDate  DATE
	,@endDate    DATE
	,@isPaidFee  BIT
	,@transactionStates NVARCHAR(50)
	,@isParked   BIT
)
RETURNS INT
AS
BEGIN
	
	DECLARE @trxCount INT = 0 

	
	IF( @productId = 1 )
	BEGIN
	        SELECT  @trxCount = COUNT(1) 
			FROM 
				tTxn_Check tc WITH (NOLOCK)		
			INNER JOIN 
			    tCustomerSessions cs on cs.CustomerID = @customerId AND tc.CustomerSessionId = cs.CustomerSessionID   	 
			INNER JOIN 
			    tShoppingCartTransactions sct WITH (NOLOCK) ON tc.CustomerSessionId = cs.CustomerSessionID AND sct.TransactionId = tc.TransactionID AND sct.ProductId = @productId
			INNER JOIN 
				tShoppingCarts sc WITH (NOLOCK) ON sc.CartID = sct.CartId   
			WHERE 
			   tc.State IN (SELECT CAST(item AS INT) FROM dbo.SplitString(@transactionStates,',')) -- transaction state should not be in failed, declined, cancelled 
			   AND (@isParked = 1 OR sc.State != 2)       -- When the business considers declined, cancelled, parked transactions, then we should not consider parked transactions as well.
			   AND tc.TransactionID != @transactionId     -- Exclude current transaction 
			   AND (@isNextCustomerSession = 0 OR tc.CustomerSessionId != @customerSessionId)   -- Exclude current customer session transaction count based on flag. 
			   AND CONVERT(DATE, tc.DTTerminalCreate) BETWEEN @startDate AND @endDate           -- Trx count between start date and end date
			   AND (@isPaidFee = 0 OR ISNULL(tc.DiscountApplied, 0) = 0)   -- Paid Fee transaction count based on flag

	 END
	 ELSE IF( @productId = 5 )
	 BEGIN
	      
		  
		   SELECT  @trxCount = COUNT(1) 
			FROM 
				tTxn_MoneyOrder mo WITH (NOLOCK)		
			INNER JOIN 
			    tCustomerSessions cs on cs.CustomerID = @customerId AND mo.CustomerSessionId = cs.CustomerSessionID   	 
			INNER JOIN 
			    tShoppingCartTransactions sct WITH (NOLOCK) ON mo.CustomerSessionId = cs.CustomerSessionID AND sct.TransactionId = mo.TransactionID AND sct.ProductId = @productId
			INNER JOIN 
				tShoppingCarts sc WITH (NOLOCK) ON sc.CartID = sct.CartId   
			WHERE 
			   mo.State IN (SELECT CAST(item AS INT) FROM dbo.SplitString(@transactionStates,','))  -- transaction state should not be in failed, declined, cancelled 
			   AND (@isParked = 1 OR sc.State != 2)       -- When the business considers declined,cancelled,parked transactions, then we should not consider parked transactions as well.
			   AND mo.TransactionID != @transactionId     -- Exclude current transaction 
			   AND (@isNextCustomerSession = 0 OR mo.CustomerSessionId != @customerSessionId)   -- Exclude current customer session transaction count based on flag. 
			   AND CONVERT(DATE, mo.DTTerminalCreate) BETWEEN @startDate AND @endDate           -- Trx count between start date and end date
			   AND (@isPaidFee = 0 OR ISNULL(mo.DiscountApplied, 0) = 0 )   -- Paid Fee transaction count based on flag


	 END
	 ELSE IF( @productId = 2 )
	 BEGIN
	      
		  
		    SELECT  @trxCount = COUNT(1) 
			FROM 
				tTxn_BillPay bp WITH (NOLOCK)		
			INNER JOIN 
			    tCustomerSessions cs on cs.CustomerID = @customerId AND bp.CustomerSessionId = cs.CustomerSessionID   	 
			INNER JOIN 
			    tShoppingCartTransactions sct WITH (NOLOCK) ON bp.CustomerSessionId = cs.CustomerSessionID AND sct.TransactionId = bp.TransactionID AND sct.ProductId = @productId
			INNER JOIN 
				tShoppingCarts sc WITH (NOLOCK) ON sc.CartID = sct.CartId   
			WHERE 
			   bp.State IN (SELECT CAST(item AS INT) FROM dbo.SplitString(@transactionStates,','))  -- transaction state should not be in failed, declined, cancelled 
			   AND (@isParked = 1 OR sc.State != 2)       --When the business considers declined,cancelled,parked transactions, then we should not consider parked transactions as well.
			   AND bp.TransactionID != @transactionId     -- Exclude current transaction 
			   AND (@isNextCustomerSession = 0 OR bp.CustomerSessionId != @customerSessionId)   -- Exclude current customer session transaction count based on flag. 
			   AND CONVERT(DATE, bp.DTTerminalCreate) BETWEEN @startDate AND @endDate           -- Trx count between start date and end date
			   AND (@isPaidFee = 0 OR ISNULL(bp.Fee, 0) > 0 )   -- Paid Fee transaction count based on flag


	 END
	 ELSE IF( @productId = 3 )
	 BEGIN
	      
		  
		    SELECT  @trxCount = COUNT(1) 
			FROM 
				tTxn_MoneyTransfer mt WITH (NOLOCK)		
			INNER JOIN 
			    tCustomerSessions cs on cs.CustomerID = @customerId AND mt.CustomerSessionId = cs.CustomerSessionID AND mt.TransferType = 1 -- Send money
			INNER JOIN 
			    tShoppingCartTransactions sct WITH (NOLOCK) ON mt.CustomerSessionId = cs.CustomerSessionID AND sct.TransactionId = mt.TransactionID AND sct.ProductId = @productId
			INNER JOIN 
				tShoppingCarts sc WITH (NOLOCK) ON sc.CartID = sct.CartId   
			WHERE 
			   mt.State IN (SELECT CAST(item AS INT) FROM dbo.SplitString(@transactionStates,','))  -- transaction state should not be in failed, declined, cancelled 
			   AND (@isParked = 1 OR sc.State != 2)       -- When the business considers declined,cancelled,parked transactions, then we should not consider parked transactions as well.
			   AND mt.TransactionID != @transactionId     -- Exclude current transaction 
			   AND (@isNextCustomerSession = 0 OR mt.CustomerSessionId != @customerSessionId)   -- Exclude current customer session transaction count based on flag. 
			   AND CONVERT(DATE, mt.DTTerminalCreate) BETWEEN @startDate AND @endDate           -- Trx count between start date and end date
			   AND (@isPaidFee = 0 OR ISNULL(mt.Fee, 0) > 0 )   -- Paid Fee transaction count based on flag


	 END
	 ELSE IF( @productId = 6 )
	 BEGIN
	      
		  
		    SELECT  @trxCount = COUNT(1) 
			FROM 
				tTxn_Funds f WITH (NOLOCK)		
			INNER JOIN 
			    tCustomerSessions cs on cs.CustomerID = @customerId AND f.CustomerSessionId = cs.CustomerSessionID   	 
			INNER JOIN 
			    tShoppingCartTransactions sct WITH (NOLOCK) ON f.CustomerSessionId = cs.CustomerSessionID AND sct.TransactionId = f.TransactionID AND sct.ProductId = @productId
			INNER JOIN 
				tShoppingCarts sc WITH (NOLOCK) ON sc.CartID = sct.CartId   
			WHERE 
			   f.State IN (SELECT CAST(item AS INT) FROM dbo.SplitString(@transactionStates,','))  -- transaction state should not be in failed, declined, cancelled 
			   AND (@isParked = 1 OR sc.State != 2)       --When the business considers declined,cancelled,parked transactions, then we should not consider parked transactions as well.
			   AND f.TransactionID != @transactionId     -- Exclude current transaction 
			   AND (@isNextCustomerSession = 0 OR f.CustomerSessionId != @customerSessionId)   -- Exclude current customer session transaction count based on flag. 
			   AND CONVERT(DATE, f.DTTerminalCreate) BETWEEN @startDate AND @endDate           -- Trx count between start date and end date


	 END
	-- Return the result of the function
	RETURN @trxCount

END
GO

