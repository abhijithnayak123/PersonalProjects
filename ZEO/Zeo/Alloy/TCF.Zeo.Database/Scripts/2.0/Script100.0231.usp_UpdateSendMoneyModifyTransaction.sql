--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-03-2016>
-- Description:	This SP is used to update the WU transaction.
-- Jira ID:		<AL-8324>
-- ================================================================================

IF OBJECT_ID(N'usp_UpdateSendMoneyModifyTransaction', N'P') IS NOT NULL
DROP PROC usp_UpdateSendMoneyModifyTransaction
GO


CREATE PROCEDURE usp_UpdateSendMoneyModifyTransaction
(
    @wuTrxId BIGINT
	,@principalAmount decimal(18,2) = 0
	,@receiver_unv_Buffer VARCHAR(300) = NULL
	,@sender_unv_Buffer VARCHAR(300) = NULL
	,@moneyTransferKey VARCHAR(100) = 0
	--Modify Trx
	,@mtcn VARCHAR(50) = NULL
	,@modifyType INT -- 1-ModifyTransaction, 2-ModifySearchTransaction
	,@dtTerminalDate DATETIME
	,@dtServerDate DATETIME
)
AS
BEGIN
BEGIN TRY
	
	IF @modifyType = 1
	BEGIN
		
		UPDATE tWUnion_Trx
		SET Mtcn = @mtcn
			,DTTerminalLastModified = @dtTerminalDate
			,DTServerLastModified = @dtServerDate
		WHERE WUTrxId = @wuTrxId

	END
	ELSE
	BEGIN

		UPDATE tWUnion_Trx
		SET PrincipalAmount = @principalAmount
			,Receiver_unv_Buffer = @receiver_unv_Buffer
			,Sender_unv_Buffer = @sender_unv_Buffer
			,MoneyTransferKey = @moneyTransferKey
			,DTTerminalLastModified = @dtTerminalDate
			,DTServerLastModified = @dtServerDate
		WHERE WUTrxId = @wuTrxId
	
	END

END TRY
BEGIN CATCH

    EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError

END CATCH
END
GO
