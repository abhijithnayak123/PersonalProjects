-- ====================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <01/17/2018>
-- Description:	<Verify, the promotion is already applied or not>
-- =====================================================================

-- select  dbo.ufn_IsPromotionApplied(10000001, 2540247527168537, 1, 1)

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'ufn_IsPromotionApplied') AND xtype IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION ufn_IsPromotionApplied
GO

CREATE FUNCTION [dbo].[ufn_IsPromotionApplied]
(    
     @promotionId   BIGINT
	,@transactionId BIGINT
	,@customerId    BIGINT
	,@freeTrxCount  INT
)
RETURNS BIT
AS
BEGIN
	
		 DECLARE @trxCount INT = 0, @IsPromotionApplied BIT = 0
		

		        SELECT @trxCount = COUNT(1) FROM tTxn_FeeAdjustments WHERE CustomerId = @customerId AND PromotionId = @promotionId AND TransactionId != @transactionId AND IsActive = 1

				-- eg. Consider a promotion - ThreeThenTwofree, In this case for 4th and 5th transaction we need to give the Promotion and these values (i.e, 4,5) are configured
				-- in Min and Max trx count. So here '@IsPromotionApplied' is used to check whether the Promo need to be applied for the current trx or not.  
				
				IF(@freeTrxCount <= @trxCount)  
				BEGIN
				    SET @IsPromotionApplied = 1
				END
		         
		
		-- Return the result of the function
		RETURN @IsPromotionApplied

END
GO
