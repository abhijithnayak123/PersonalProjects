--- ===============================================================================
-- Author:		<Ashok Kumar>
-- Create date: <02-14-2017>
-- Description:	Get billpay receipt data. 
-- Jira ID:		<>

--EXEC usp_GetBillpayReceiptData 1000000000
-- ================================================================================

IF OBJECT_ID(N'usp_GetBillpayReceiptData', N'P') IS NOT NULL
BEGIN
     DROP PROCEDURE usp_GetBillpayReceiptData
END
GO

CREATE PROCEDURE usp_GetBillpayReceiptData
(
    @transactionID    BIGINT,
    @transactionType  VARCHAR(1000),
    @Provider         INT,
    @isReprint        BIT
)
AS
BEGIN  
     BEGIN TRY
	         SET NOCOUNT ON;
             SELECT 
             -- Transaction Details
                   BillerName									AS ReceiverName,
                   Customer_AccountNumber						AS AccountNumber,
                   Financials_UndiscountedCharges				AS UnDiscountedFee,
                   Financials_TotalDiscount					    AS PrmDiscount,
                   MTCN										    AS MTCN,
                   tWBA.CardNumber								AS GCNumber,
             CASE  WHEN DeliveryCode = '000' THEN 'Urgent'
		           WHEN DeliveryCode = '100' THEN '2nd Business Day'
		           WHEN DeliveryCode = '200' THEN '3rd Business Day'
		           WHEN DeliveryCode = '300' THEN 'Next Business Day'
		           ELSE ''
		     END									            AS DeliveryService,
                   WUCard_TotalPointsEarned					    AS WuCardTotalPointsEarned,
                   MessageArea									AS MessageArea,
                   ISNULL(tTB.DTTerminalLastModified, tTB.DTTerminalCreate) 
											                    AS TxrDate,
                   tTB.Amount									AS TransferAmmount, 
                  (tTB.Amount + tTB.Fee)						AS NetAmount,
                   tTB.TransactionID						    AS TransactionId,
                   tTB.ConfirmationNumber						AS ConfirmationId,
                   ISNULL(tTB.DTTerminalLastModified, tTB.DTTerminalCreate) 
											                    AS ReceiptDate,

                  -- Customer Details
                   tCS.CustomerSessionID						AS SessionlID,
                   tCS.DTStart									AS CustomerSessionDate,
                   tC.FirstName + ' ' + ISNULL(tC.LastName, '') 
											                    AS CustomerName,
                   tC.FirstName + ' ' + ISNULL(tC.LastName, '') 
											                    AS SenderName,
                   tC.Address1 + ' ' + ISNULL(tC.Address2,'')
											                    AS SenderAddress,
                   tC.City										AS SenderCity,
                   tC.State									    AS SenderState,
                   tC.ZipCode									AS SenderZip,
                   tC.Phone1									AS SenderPhoneNumber,
            CASE   WHEN tC.Phone1Type = 'Cell' THEN tC.Phone1
		           WHEN ISNULL(tC.Phone2Provider, '') = 'Cell' THEN tC.Phone2
		           ELSE ''
		    END									                AS SenderMobileNumber,

               -- Agent Details
                  tAD.ClientAgentIdentifier					    AS TellerNumber,
                  SUBSTRING(tAD.UserName, 0, 6)				    AS TellerName,

               -- Terminal Details
                  tT.TerminalID								    AS TerminalID,

             -- Location Details
                  tL.Address1 + ' ' + ISNULL(tL.Address2,'') 
											                    AS LocationAddress,
                  tL.City										AS LocationCity,
                  tL.State									    AS LocationState,
                  tL.ZipCode									AS LocationZip,
                  tL.BranchID									AS BranchId,
                  tL.BankID									    AS BankId,
                  tL.PhoneNumber								AS LocationPhoneNumber,
                  tL.LocationName								AS LocationName,
                  tL.TimezoneID								    AS Timezone,
            CASE  WHEN tL.TimezoneID = 'Pacific Standard Time' THEN 'PST'
		          WHEN tL.TimezoneID = 'Mountain Standard Time'THEN 'MST'
		          WHEN tL.TimezoneID = 'Eastern Standard Time' THEN 'EST'
		          WHEN tL.TimezoneID = 'Central Standard Time' THEN 'CST'
		          ELSE ''
		    END									                AS TimezoneId,

            -- Channel Partner Details
                  tCP.Name									    AS ClientName,
                  tCP.LogoFileName							    AS LogoUrl,
            CASE  WHEN @isReprint = 1 THEN  tPM.ReceiptReprintCopies
                  WHEN @isReprint = 0 THEN  tPM.ReceiptCopies
			      ELSE ''
		    END                                                 AS NoOfCopies
            
			FROM tTxn_BillPay tTB	
				  INNER JOIN tWUnion_BillPay_Trx tWBT									   ON tTB.CXNID = tWBT.WUBillPayTrxID
                  INNER JOIN tWUnion_BillPay_Account tWBA WITH (NOLOCK)		               ON tWBT.WUBillPayAccountId = tWBA.WUBillPayAccountID
                  INNER JOIN tCustomerSessions tCS WITH (NOLOCK)			               ON tCS.CustomerSessionID = tTB.CustomerSessionId
                  INNER JOIN tCustomers tC	WITH (NOLOCK)				                   ON tC.CustomerID = tCS.CustomerID
                  INNER JOIN tAgentSessions tAS	WITH (NOLOCK)			                   ON tAS.AgentSessionID = tCS.AgentSessionId
                  INNER JOIN tAgentDetails tAD	WITH (NOLOCK)			                   ON tAD.AgentID = tAS.AgentId
                  INNER JOIN tTerminals tT	WITH (NOLOCK)				                   ON tT.TerminalId = tAS.TerminalId
                  INNER JOIN tLocations tL	WITH (NOLOCK)				                   ON tL.LocationID = tT.LocationId
                  INNER JOIN tChannelPartners tCP	WITH (NOLOCK)			               ON tL.ChannelPartnerId = tCP.ChannelPartnerId
                  INNER JOIN tChannelPartnerProductProcessorsMapping tPPM WITH (NOLOCK)    ON tCP.ChannelPartnerId = tPPM.ChannelPartnerId
                  INNER JOIN tProductProcessorsMapping tPM  WITH (NOLOCK)                  ON tPM.ProductProcessorsMappingID=tPPM.ProductProcessorId
                  INNER JOIN tProducts P WITH (NOLOCK)                                     ON tPM.ProductId = P.ProductsID 
                  INNER JOIN tProcessors PS WITH (NOLOCK)                                  ON tPM.ProcessorId = PS.ProcessorsID 
          WHERE         
		       tTB.TransactionID = @transactionID AND tPM.Code = @Provider AND LOWER(P.Name) = LOWER(@transactionType)
    END TRY
    BEGIN CATCH
	          EXECUTE usp_CreateErrorInfo
    END CATCH
END