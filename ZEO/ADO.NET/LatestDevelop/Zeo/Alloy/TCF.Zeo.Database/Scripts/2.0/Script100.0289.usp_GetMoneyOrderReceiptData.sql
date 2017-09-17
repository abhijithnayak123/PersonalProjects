--- ===============================================================================
-- Author:		<Ashok Kumar>
-- Create date: <1-5-2017>
-- Description:	Get money order receipt data. 
-- Jira ID:		<>

--EXEC usp_GetMoneyOrderReceiptData 1000000000
-- ================================================================================

IF OBJECT_ID(N'usp_GetMoneyOrderReceiptData', N'P') IS NOT NULL
BEGIN
     DROP PROC usp_GetMoneyOrderReceiptData
END
GO

CREATE PROCEDURE usp_GetMoneyOrderReceiptData
(
     @transactionID BIGINT
)
AS
BEGIN
     BEGIN TRY
              SELECT 
                    tTM.CheckNumber								AS MONumber,
                    tTM.Amount									AS Amount,
                    tTM.DiscountApplied							AS Discount,
                    tTM.DiscountName							AS DiscountName,
                    tTM.BaseFee									AS Fee,
                    tTM.Amount + tTM.Fee						AS NetAmount,
                    ISNULL(tTM.DTTerminalLastModified, tTM.DTTerminalCreate) 
											                    AS ReceiptDate,
                    tTM.TransactionId							AS TransactionId,
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
		       END									            AS SenderMobileNumber,
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
		            END									         AS TimezoneId,
                -- Channel Partner Details
                   tCP.Name									     AS ClientName,
                   tCP.LogoFileName							     AS LogoUrl
              FROM tTxn_MoneyOrder tTM WITH (NOLOCK)
                   INNER JOIN tCustomerSessions tCS WITH (NOLOCK)			ON tCS.CustomerSessionID = tTM.CustomerSessionId
                   INNER JOIN tCustomers tC	WITH (NOLOCK)					ON tC.CustomerID = tCS.CustomerID
                   INNER JOIN tAgentSessions tAS WITH (NOLOCK)				ON tAS.AgentSessionID = tCS.AgentSessionId
                   INNER JOIN tAgentDetails tAD WITH (NOLOCK)				ON tAD.AgentID = tAS.AgentId
                   INNER JOIN tTerminals tT	WITH (NOLOCK)					ON tT.TerminalPK = tAS.TerminalPK
                   INNER JOIN tLocations tL	WITH (NOLOCK)					ON tL.LocationPK = tT.LocationPK
                   INNER JOIN tChannelPartners tCP WITH (NOLOCK)			ON tL.ChannelPartnerId = tCP.ChannelPartnerId 
              WHERE tTM.TransactionID=@transactionID
       END TRY
	   BEGIN CATCH
	              EXECUTE usp_CreateErrorInfo
	   END CATCH
END
