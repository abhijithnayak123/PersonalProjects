IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_FeeAdjustments' AND COLUMN_NAME = 'CustomerId')
BEGIN
   
   UPDATE fa
   SET fa.CustomerId = cs.CustomerID
   FROM tTxn_FeeAdjustments fa	
	INNER JOIN tChannelPartnerFeeAdjustments cfa ON fa.PromotionId = cfa.FeeAdjustmentId 
		AND cfa.TransactionType = 1
	INNER JOIN tTxn_Check tc ON fa.TransactionId = tc.TransactionID
	INNER JOIN tCustomerSessions cs ON tc.CustomerSessionId = cs.CustomerSessionID

   
   UPDATE fa
   SET fa.CustomerId = cs.CustomerID
   FROM tTxn_FeeAdjustments fa	
	INNER JOIN tChannelPartnerFeeAdjustments cfa ON fa.PromotionId = cfa.FeeAdjustmentId 
		AND cfa.TransactionType = 5
	INNER JOIN tTxn_MoneyOrder mo ON fa.TransactionId = mo.TransactionID
	INNER JOIN tCustomerSessions cs ON mo.CustomerSessionId = cs.CustomerSessionID

END
GO



