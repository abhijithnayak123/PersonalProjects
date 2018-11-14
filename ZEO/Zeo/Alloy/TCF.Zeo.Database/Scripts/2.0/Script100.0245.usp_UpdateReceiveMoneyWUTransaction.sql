IF OBJECT_ID(N'usp_UpdateReceiveMoneyWUTransaction', N'P') IS NOT NULL
DROP PROC usp_UpdateReceiveMoneyWUTransaction
GO

CREATE PROCEDURE usp_UpdateReceiveMoneyWUTransaction
(	
	@exchangerate DECIMAL(18,4),
	@charges DECIMAL(18,2),
	@destinationprincipalamount DECIMAL,
	@grosstotamt DECIMAL(18,2),
	@amounttoreceiver DECIMAL(18,2),
	@moneytransferkey VARCHAR(50),
	@dtterminallastmodified DATETIME,
	@dtserverlastmodified DATETIME,
	@tempMtcn VARCHAR(100),
	@mtcn NVARCHAR(50),
	@originatingcurrencycode VARCHAR(20),
	@destinationcurrencycode VARCHAR(20),
	@sendername VARCHAR(50),
	@originatingcountrycode VARCHAR(20),
	@destinationcountrycode VARCHAR(20),
	@testQuestion VARCHAR(100),
	@testAnswer VARCHAR(100),
	@personalMessage NVARCHAR(1000),
	@expectedPayoutCityName VARCHAR(100),
	@expectedPayoutStateCode VARCHAR(100),
	@originaldestinationcountrycode VARCHAR(20),
	@originaldestinationcurrencycode VARCHAR(20),
	@originating_city NVARCHAR(100),
	@recieverFirstName VARCHAR(100),
	@recieverLastName VARCHAR(100),
	@receiversecondlastname VARCHAR(100),
	@wuTrxId BIGINT
)
AS
BEGIN
	
BEGIN TRY

	UPDATE [dbo].[tWUnion_Trx]
		SET 
			ExchangeRate = @exchangerate,
			Charges = @charges,
			DestinationPrincipalAmount = @destinationprincipalamount,
			GrossTotalAmount = @grosstotamt,
			AmountToReceiver = @amounttoreceiver,
			MoneyTransferKey = @moneytransferkey,
			DTTerminalLastModified = @dtterminallastmodified,
			DTServerLastModified = @dtserverlastmodified,
			TempMTCN = @tempMtcn,
			Mtcn = @mtcn,
			OriginatingCurrencyCode = @originatingcurrencycode,
			DestinationCurrencyCode = @destinationcurrencycode,
			SenderName = @sendername,
			OriginatingCountryCode = @originatingcountrycode,
			DestinationCountryCode = @destinationcountrycode,
			TestQuestion = @testQuestion,
			TestAnswer = @testAnswer,
			PersonalMessage = @personalMessage,
			ExpectedPayoutCityName = @expectedPayoutCityName,
			ExpectedPayoutStateCode = @expectedPayoutStateCode,
			OriginalDestinationCountryCode = @originaldestinationcountrycode,
			OriginalDestinationCurrencyCode = @originaldestinationcurrencycode,
			originating_city = @originating_city,
			RecieverFirstName = @recieverFirstName,
			RecieverLastName = @recieverLastName,
			RecieverSecondLastName = @receiversecondlastname
	   WHERE 
			WUTrxId = @wuTrxId
END TRY
BEGIN CATCH

	EXECUTE usp_CreateErrorInfo
		
END CATCH
END
GO
