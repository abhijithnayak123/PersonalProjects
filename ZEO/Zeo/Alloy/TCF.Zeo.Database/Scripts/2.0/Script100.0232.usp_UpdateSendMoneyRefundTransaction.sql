--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-03-2016>
-- Description:	This SP is used to update the WU transaction.
-- Jira ID:		<AL-8324>
-- ================================================================================

IF OBJECT_ID(N'usp_UpdateSendMoneyRefundTransaction', N'P') IS NOT NULL
DROP PROC usp_UpdateSendMoneyRefundTransaction
GO


CREATE PROCEDURE usp_UpdateSendMoneyRefundTransaction
(
    @wuTrxId BIGINT
	,@originatorsPrincipalAmount DECIMAL(18,2)
	,@destinationPrincipalAmount DECIMAL(18,2)
	,@grossTotalAmount DECIMAL(18,2)
    ,@charges DECIMAL(18,2)
	,@recordingCountryCode NVARCHAR(20) = NULL
	,@recordingCurrencyCode NVARCHAR(20) = NULL
	,@moneyTransferKey VARCHAR(100) = NULL
	,@tempMTCN VARCHAR(100)
	,@refundType INT -- 1-RefundTransaction, 2-RefundSearchTransaction
	,@dtTerminalDate DATETIME
	,@dtServerDate DATETIME
)
AS
BEGIN
BEGIN TRY
		
		IF @refundType = 1
		BEGIN
			UPDATE tWUnion_Trx
			SET OriginatorsPrincipalAmount = @originatorsPrincipalAmount
				,DestinationPrincipalAmount = @destinationPrincipalAmount
				,GrossTotalAmount = @grossTotalAmount
				,Charges = @charges
				,DTTerminalLastModified = @dtTerminalDate
				,DTServerLastModified = @dtServerDate
			WHERE WUTrxId = @wuTrxId
		END
		ELSE
		BEGIN
			UPDATE tWUnion_Trx
			SET 
				RecordingCountryCode = @recordingCountryCode
				,RecordingCurrencyCode = @recordingCurrencyCode
				,OriginatorsPrincipalAmount = @originatorsPrincipalAmount
				,GrossTotalAmount = @grossTotalAmount
				,DestinationPrincipalAmount = @destinationPrincipalAmount
				,Charges = @charges
				,MoneyTransferKey = @moneyTransferKey
				,TempMTCN = @tempMTCN
				,DTTerminalLastModified = @dtTerminalDate
				,DTServerLastModified = @dtServerDate
			WHERE WUTrxId = @wuTrxId
		END

END TRY
BEGIN CATCH
	EXECUTE usp_CreateErrorInfo
END CATCH
END
GO
