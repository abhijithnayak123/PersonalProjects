-- ====================================================================
-- Author:		<Nitish Biradar>
-- Create date: <02/26/2018>
-- Description:	<Verify, the all qualifiers fields are present>
-- =====================================================================

-- select  dbo.ufn_VerifyQualifiers(100000001)

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'ufn_VerifyQualifiers') AND xtype IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION ufn_VerifyQualifiers
GO

CREATE FUNCTION [dbo].[ufn_VerifyQualifiers]
(    
     @promotionId BIGINT
)
RETURNS BIT
AS
BEGIN
	
	DECLARE @TempTable TABLE (isValid BIT)
	DECLARE @isValidQualifiers BIT = 1

	INSERT INTO 
		@TempTable
	SELECT 
		CASE 
			WHEN ProductId IS NOT NULL AND 
					(MinTransactionCount IS NOT NULL OR MaxTransactionCount IS NOT NULL OR Amount IS NOT NULL) AND
					TransactionStates IS NOT NULL THEN 1
			ELSE 0
		END
	FROM 
		tPromoQualifiers 
	WHERE 
		PromotionId = @promotionId
	
	SELECT @isValidQualifiers = isValid FROM @TempTable WHERE isValid = 0

	RETURN @isValidQualifiers

END
GO