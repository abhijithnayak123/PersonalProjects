--- ===============================================================================
-- Author:		<Ashok Kumar>
-- Create date: <1-16-2017>
-- Description:	Get Funds receipt data. 
-- Jira ID:		<>

--EXEC usp_GetFundReceiptData 1000000000
-- ================================================================================

IF OBJECT_ID(N'usp_GetFundReceiptData', N'P') IS NOT NULL
BEGIN
     DROP PROCEDURE  usp_GetFundReceiptData
END
GO

CREATE PROCEDURE usp_GetFundReceiptData
(
@transactionID BIGINT
)
AS
  BEGIN
       BEGIN TRY
	         SET NOCOUNT ON;
				SELECT 
						tTF.DiscountApplied							AS DiscountApplied,
						tTF.Amount									AS Amount,
						tTF.Fee										AS Fee,
						tTF.BaseFee									AS BaseFee,
						tVT.ConfirmationId						    AS ConfirmationNo,
						tTF.FundType								AS FundType,
						tVT.Balance									AS PreviousCardBalance,
						tVA.CardNumber								AS CardNumber,
						tTF.DiscountName							AS DiscountName,
						tAC.FirstName + ' ' + tAC.LastName			AS CompanionName,
						tTF.TransactionID						    AS TransactionId,
						ISNULL(tTF.DTTerminalLastModified, tTF.DTTerminalCreate) 
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
						tC.State									AS SenderState,
						tC.ZipCode									AS SenderZip,
						tC.Phone1									AS SenderPhoneNumber,
				CASE	WHEN tC.Phone1Type = 'Cell' THEN tC.Phone1
						WHEN ISNULL(tC.Phone2Provider, '') = 'Cell' THEN tC.Phone2
						ELSE ''
						END									        AS SenderMobileNumber,
						-- Agent Details
						tAD.ClientAgentIdentifier					AS TellerNumber,
						SUBSTRING(tAD.UserName, 0, 6)				AS TellerName,

						-- Terminal Details
						tT.TerminalID								AS TerminalID,

						-- Location Details
						tL.Address1 + ' ' + ISNULL(tL.Address2,'') 
																	AS LocationAddress,
						tL.City										AS LocationCity,
						tL.State									AS LocationState,
						tL.ZipCode									AS LocationZip,
						tL.BranchID									AS BranchId,
						tL.BankID									AS BankId,
						tL.PhoneNumber								AS LocationPhoneNumber,
						tL.LocationName								AS LocationName,
						tL.TimezoneID								AS Timezone,
				CASE	WHEN tL.TimezoneID = 'Pacific Standard Time' THEN 'PST'
						WHEN tL.TimezoneID = 'Mountain Standard Time' THEN 'MST'
						WHEN tL.TimezoneID = 'Eastern Standard Time' THEN 'EST' 
						WHEN tL.TimezoneID = 'Central Standard Time' THEN 'CST'
						ELSE ''
						END									        AS TimezoneId,

						-- Channel Partner Details
						tCP.Name									AS ClientName,
						tCP.LogoFileName							AS LogoUrl
				FROM    tTxn_Funds tTF
						INNER JOIN tVisa_Trx tVT					ON tVT.VisaTrxID = tTF.CXNId
						INNER JOIN tVisa_Account tVA				ON tVA.VisaAccountID = tVT.VisaAccountId
						INNER JOIN tCustomerSessions tCS			ON tCS.CustomerSessionID = tTF.CustomerSessionId
						INNER JOIN tCustomers tC					ON tC.CustomerID = tCS.CustomerID
						LEFT JOIN tCustomers tAC					ON tTF.AddOnCustomerId = tAC.CustomerID
						INNER JOIN tAgentSessions tAS				ON tAS.AgentSessionID = tCS.AgentSessionId
						INNER JOIN tAgentDetails tAD				ON tAD.AgentID = tAS.AgentId
						INNER JOIN tTerminals tT					ON tT.TerminalID = tAS.TerminalId
						INNER JOIN tLocations tL					ON tL.LocationID = tT.LocationId
						INNER JOIN tChannelPartners tCP				ON tL.ChannelPartnerId = tCP.ChannelPartnerId
                WHERE   tTF.TransactionID=@transactionID
           END TRY
		   BEGIN CATCH
	          EXECUTE usp_CreateErrorInfo
           END CATCH   
     END