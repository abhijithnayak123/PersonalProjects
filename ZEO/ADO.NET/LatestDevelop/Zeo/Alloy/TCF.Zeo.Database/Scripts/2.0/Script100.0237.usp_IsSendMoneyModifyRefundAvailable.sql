--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <12-14-2016>
-- Description:	This SP is used to update the Money Transfer transaction.
-- Jira ID:		<AL-8324>
--exec usp_IsSendMoneyModifiedOrRefunded 1000000015,1000000000000020
-- ================================================================================

IF OBJECT_ID(N'usp_IsSendMoneyModifiedOrRefunded', N'P') IS NOT NULL
DROP PROC usp_IsSendMoneyModifiedOrRefunded
GO


CREATE PROCEDURE usp_IsSendMoneyModifiedOrRefunded
(
	@mtTransactionId BIGINT
	,@customerId BIGINT
)
AS
BEGIN
BEGIN TRY
		
	   SELECT 
			CAST(CASE 
				 WHEN COUNT(1) > 0 
				 THEN 1
				 ELSE 0
			 END AS BIT) AS isAvailable
		FROM 
			 tWunion_Account wa 
			 INNER JOIN tTxn_MoneyTransfer mt ON mt.ProviderAccountId = wa.WUAccountID
		WHERE 
			wa.CustomerId = @customerId AND mt.OriginalTransactionId = @mtTransactionId
				AND (mt.State = 4 OR mt.State = 2)
	
END TRY
BEGIN CATCH

	EXECUTE usp_CreateErrorInfo
		
END CATCH
END
GO
