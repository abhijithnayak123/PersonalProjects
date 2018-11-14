--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <04-27-2017>
-- Description:	Modified the sender name for the receiver 
-- Jira ID:		<>

--EXEC usp_GetMoneyTransferReceiptData 1000000001
-- ================================================================================

IF OBJECT_ID(N'usp_GetMoneyTransferReceiptData', N'P') IS NOT NULL
BEGIN
     DROP PROCEDURE  usp_GetMoneyTransferReceiptData
END
GO

CREATE PROCEDURE [dbo].[usp_GetMoneyTransferReceiptData]
(
     @transactionID    BIGINT,
     @transactionType  VARCHAR(1000),
     @Provider         INT,
     @isReprint        BIT
)
AS
BEGIN
     BEGIN TRY
			DECLARE @trxid BIGINT
			DECLARE @transactionSubType INT
			DECLARE @originTransactionId BIGINT
			
			SELECT 
				@transactionSubType = TransactionSubType, @originTransactionId = OriginalTransactionID 
			FROM 
				tTxn_MoneyTransfer 
			WHERE 
				TransactionID = @transactionID	

			IF @transactionSubType = 3
				BEGIN
					SELECT 
						@trxid = TransactionID 
					FROM 
						tTxn_MoneyTransfer 
					WHERE 
						OriginalTransactionID = @originTransactionId AND TransactionSubType = 1 AND State != 6
				END
			ELSE
				BEGIN
				   SET @trxid = @transactionID
				END

              SELECT
                    ISNULL(tTM.DTTerminalLastModified, tTM.DTTerminalCreate)   AS TrxDateTime,
                    ISNULL(tTM.DTTerminalLastModified, tTM.DTTerminalCreate)   AS ReceiptDate,
                    tTM.TransactionId						                   AS TransactionId,
             CASE WHEN tWT.TranascationType = 2 THEN tWT.SenderName
			 ELSE
			 (
				CASE	WHEN tC.LastName2 IS NULL THEN 
				            CONCAT(tC.FirstName,' ', ISNULL(tC.MiddleName+' ', ''), ISNULL(tC.LastName, ''))
				       ELSE CONCAT(tC.FirstName,' ', ISNULL(tC.LastName+' ', ''), ISNULL(tC.LastName2, ''))
				       END		
			 ) 
			 END AS SenderName,					                           
             CASE	WHEN tC.LastName2 IS NULL THEN 
			             CONCAT(tC.FirstName,ISNULL(tC.MiddleName, ''),ISNULL(tC.LastName, ''))
		            ELSE CONCAT(tC.FirstName,ISNULL(tC.LastName, ''),ISNULL(tC.LastName2, ''))
		            END								                           AS CustomerName,
                         CONCAT(tC.Address1, ', ', ISNULL(tC.Address2, ''))    AS SenderAddress,
                    tC.City									                   AS SenderCity,
                    tC.State								                   AS SenderState,
                    tC.ZipCode								                   AS SenderZip,
                    tC.Phone1								                   AS SenderPhoneNumber,
              CASE	WHEN tC.Phone1Type = 'Cell' THEN tC.Phone1
		            WHEN ISNULL(tC.Phone2Provider, '') = 'Cell' THEN tC.Phone2
		            ELSE '' 
		      END								                               AS SenderMobileNumber,
                    tCS.DTStart								                   AS CustomerSessionDate,
                    CONCAT(ISNULL(tWT.RecieverFirstName, ''),' ',
					       ISNULL(tWT.RecieverLastName, ''), ' ',
						   ISNULL(tWT.RecieverSecondLastName, ''))
										                                       AS ReceiverName,
                    tWR.Address								                   AS ReceiverAddress,
                    tWR.City								                   AS ReceiverCity,
                    tWR.[State/Province]					                   AS ReceiverState,
                    tWR.ZipCode								                   AS ReceiverZip,
                    tWR.PhoneNumber							                   AS ReceiverPhoneNumber,
                    tWC.Name								                   AS ReceiverCountry,
                    CONCAT(tWC.Name, ' / ', t.TranslationName)			       AS PayoutCountry,
                    tWR.DOB									                   AS ReceiverDOB,
                    tWR.Occupation							                   AS ReceiverOccupation,
                    tWT.DestinationCountryCode				                   AS DestinationCountryCode,
                    tWT.DestinationState					                   AS PayoutState,
                    tWT.OriginatorsPrincipalAmount			                   AS TransferAmount,
                    tWT.OriginatingCountryCode				                   AS OriginatingCountry,
                    tWT.OriginatingCurrencyCode				                   AS CurrencyCode,
                    tWT.Charges + tWT.PromotionDiscount		                   AS TransferFee,
                    tWT.municipal_tax + tWT.state_tax + tWT.county_tax         AS TransferTaxes,
                    tWT.ExchangeRate						                   AS ExchangeRate,
                    tWT.DestinationPrincipalAmount			                   AS DstnTransferAmount,
                    tWT.DestinationCurrencyCode				                   AS DstnCurrencyCode,
                    tWT.Mtcn								                   AS MTCN,
                    tWT.GCNumber							                   AS GCNumber,
                    tWT.WUCard_TotalPointsEarned			                   AS CardPoints,
                    tWT.ExpectedPayoutCityName				                   AS PayoutCity,
                    tWT.DestinationPrincipalAmount			                   AS EstTransferAmount,
                    tWT.PaySideCharges						                   AS EstOtherFee,
                    tWT.AmountToReceiver					                   AS EstTotalToReceiver,
                    tWT.DeliveryOption						                   AS DeliveryOption,
                    tWT.DeliveryOptionDesc					                   AS DeliveryOptionDesc,
                    tWT.DeliveryServiceDesc					                   AS DeliveryServiceDesc,
                    tWT.TransalatedDeliveryServiceName		                   AS TransalatedDeliveryServiceName,
                    tWT.DTAvailableForPickup				                   AS DTAvailableForPickup,
                    tWT.OtherCharges						                   AS OtherCharges,
                    tWT.AmountToReceiver					                   AS TotalToReceiver,
                    tWT.AgencyName							                   AS AgencyName,
                    tWT.PhoneNumber							                   AS PhoneNumber,
                    tWT.Url									                   AS Url,
                    tWT.TestQuestion						                   AS TestQuestion,
                    tWT.TestAnswer							                   AS TestAnswer,
                    tWT.PersonalMessage						                   AS PersonalMessage,
                    tWT.MessageArea							                   AS MessageArea,
                    tWT.PaySideTax							                   AS PaySideTaxes,
                    tWT.TaxAmount							                   AS Taxes,
                    tWT.AdditionalCharges					                   AS AdditionalCharges,
                    tWT.message_charge						                   AS MessageCharge,
                    tWT.PaySideCharges						                   AS PaySideCharges,
                    tWT.IsFixedOnSend						                   AS IsFixOnSend,
                    tWT.plus_charges_amount					                   AS PlusChargesAmount,
                    tWT.FilingDate							                   AS FilingDate,
                    tWT.FilingTime							                   AS FilingTime,
                    tWT.PaidDateTime						                   AS PaidDateTime,
                    tWT.ExpectedPayoutCityName				                   AS ExpectedPayoutCity,
                    tWT.PromotionsCode						                   AS PromoCode,
                    tWT.PromotionDiscount					                   AS PromotionDiscount,
                    tWT.IsDomesticTransfer					                   AS IsDomesticTransfer,
                    tWT.TransactionSubType					                   AS TransactionSubType,
                    tWT.TranascationType					                   AS TranascationType,
                  -- Customer Details
                    tCS.CustomerSessionID						               AS SessionlID,
                  -- Agent Details
                    tAD.ClientAgentIdentifier				                   AS TellerNumber,
                    SUBSTRING(tAD.UserName, 0, 6)			                   AS TellerName,
                  -- Terminal Details
                    tT.TerminalID							                   AS TerminalID,
                  -- Location Details
                    tL.Address1 + ' ' + ISNULL(tL.Address2,'')                 AS LocationAddress,
                    tL.City									                   AS LocationCity,
                    tL.State								                   AS LocationState,
                    tL.ZipCode								                   AS LocationZip,
                    tL.BranchID								                   AS BranchId,
                    tL.BankID								                   AS BankId,
                    tL.PhoneNumber							                   AS LocationPhoneNumber,
                    tL.LocationName							                   AS LocationName,
                    tL.TimezoneID							                   AS Timezone,
             CASE	WHEN tL.TimezoneID = 'Pacific Standard Time' THEN 'PST'
		            WHEN tL.TimezoneID = 'Mountain Standard Time' THEN 'MST'
		            WHEN tL.TimezoneID = 'Eastern Standard Time' THEN 'EST'
		            WHEN tL.TimezoneID = 'Central Standard Time' THEN 'CST'
		            ELSE ''
		            END								                             AS TimezoneId,
                   -- Channel Partner Details
                    tCP.Name								                     AS ClientName,
                    tCP.LogoFileName						                     AS LogoUrl,
             CASE   WHEN @isReprint = 1 THEN  tPM.ReceiptReprintCopies
                    WHEN @isReprint = 0 THEN  tPM.ReceiptCopies
			        ELSE ''
		            END                                                          AS NoOfCopies
					
             FROM tTxn_MoneyTransfer tTM WITH (NOLOCK)
                  INNER JOIN tWUnion_Trx tWT WITH (NOLOCK)				                      ON tTM.CXNId = tWT.WUTrxID
                  LEFT  JOIN tWUnion_Receiver tWR WITH (NOLOCK)			                      ON tWR.WUReceiverID = tWT.WUReceiverId
                  INNER JOIN tCustomerSessions tCS WITH (NOLOCK)	                          ON tTM.CustomerSessionId = tCS.CustomerSessionID
                  INNER JOIN tCustomers tC WITH (NOLOCK)				                      ON tC.CustomerID = tCS.CustomerID
                  INNER JOIN tAgentSessions tAS	WITH (NOLOCK)		                          ON tAS.AgentSessionID = tCS.AgentSessionId
                  INNER JOIN tAgentDetails tAD WITH (NOLOCK)			                      ON tAD.AgentID = tAS.AgentId
                  INNER JOIN tTerminals tT WITH (NOLOCK)				                      ON tT.TerminalID = tAS.TerminalId
                  INNER JOIN tLocations tL WITH (NOLOCK)				                      ON tL.LocationID = tT.LocationId
                  INNER JOIN tChannelPartners tCP WITH (NOLOCK)			                      ON tL.ChannelPartnerId = tCP.ChannelPartnerId
                  INNER JOIN tWUnion_Countries tWC WITH (NOLOCK)	                          ON tWC.ISOCountryCode = tWT.DestinationCountryCode
                  INNER JOIN tWunion_CountryTranslation t WITH (NOLOCK)	                      ON t.ISOCountryCode = tWT.DestinationCountryCode AND t.LanguageCode = 'es'
                  INNER JOIN tChannelPartnerProductProcessorsMapping tPPM WITH (NOLOCK)       ON tCP.ChannelPartnerId = tPPM.ChannelPartnerId
                  INNER JOIN tProductProcessorsMapping tPM WITH (NOLOCK)                      ON tPM.ProductProcessorsMappingID = tPPM.ProductProcessorId
                  INNER JOIN tProducts P WITH (NOLOCK)                                        ON tPM.ProductId = P.ProductsID 
                  INNER JOIN tProcessors PS WITH (NOLOCK)                                     ON tPM.ProcessorId = PS.ProcessorsID 
             WHERE tTM.TransactionID = @trxid AND tPM.Code = @Provider AND LOWER(P.Name) = LOWER(@transactionType)  
       END TRY
	   BEGIN CATCH
	             EXECUTE  usp_CreateErrorInfo
	   END CATCH
END
GO


