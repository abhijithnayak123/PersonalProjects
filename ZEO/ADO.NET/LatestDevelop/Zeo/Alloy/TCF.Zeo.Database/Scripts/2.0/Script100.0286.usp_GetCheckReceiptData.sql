--- ===============================================================================
-- Author:		<Ashok Kumar>
-- Create date: <1-5-2017>
-- Description:	Get process check receipt data. 
-- Jira ID:		<>

--EXEC usp_GetProcessCheckReceiptData 1000000000
-- ================================================================================

IF OBJECT_ID(N'usp_GetProcessCheckReceiptData', N'P') IS NOT NULL
DROP PROC usp_GetProcessCheckReceiptData
GO

CREATE PROCEDURE usp_GetProcessCheckReceiptData
@transactionID BIGINT
AS
BEGIN
SELECT 
	(tTC.Amount - tTC.Fee)			AS NetAmount,
	tTC.ConfirmationNumber			AS ConfirmationNumber,
	tTC.DiscountApplied				AS Discount,
	tTC.DiscountName				AS DiscountName,
	tTC.BaseFee						AS Fee,
	tTC.Amount						AS Amount,
	tTC.TransactionID				AS TransactionId,
	tCTs.Name						AS ReturnType,
	ISNULL(tTC.DTTerminalLastModified, tTC.DTTerminalCreate) 
									AS ReceiptDate,
	-- Customer Details
	tCS.CustomerSessionID			AS SessionlID,
	tCS.DTStart						AS CustomerSessionDate,
	tC.FirstName + ' ' + ISNULL(tC.LastName, '') 
									AS CustomerName,
	-- Agent Details
	tAD.ClientAgentIdentifier		AS TellerNumber,
	SUBSTRING(tAD.UserName, 0, 6)	AS TellerName,
	
	-- Terminal Details
	tT.TerminalID					AS TerminalID,
	
	-- Location Details
	tL.Address1 + ' ' + ISNULL(tL.Address2,'') 
									AS LocationAddress,
	tL.City							AS LocationCity,
	tL.State						AS LocationState,
	tL.ZipCode						AS LocationZip,
	tL.BranchID						AS BranchId,
	tL.BankID						AS BankId,
	tL.PhoneNumber					AS LocationPhoneNumber,
	tL.LocationName					AS LocationName,
	tL.TimezoneID					AS Timezone,
	CASE	WHEN tL.TimezoneID = 'Pacific Standard Time' THEN 'PST'
			WHEN tL.TimezoneID = 'Mountain Standard Time' THEN 'MST'
			WHEN tL.TimezoneID = 'Eastern Standard Time' THEN 'EST'
			WHEN tL.TimezoneID = 'Central Standard Time' THEN 'CST'
			ELSE ''
			END						AS TimezoneId,
	
	-- Channel Partner Details
	tCP.Name						AS ClientName,
	tCP.LogoFileName				AS LogoUrl
FROM tTxn_Check tTC
INNER JOIN tChxr_Trx tCT			ON tTC.TransactionID = tCT.TransactionId
INNER JOIN tCheckTypes tCTs			ON tCTs.CheckTypeId = tTC.CheckType
INNER JOIN tCustomerSessions tCS	ON tCS.CustomerSessionID = tTC.CustomerSessionId
INNER JOIN tCustomers tC			ON tC.CustomerID = tCS.CustomerID
INNER JOIN tAgentSessions tAS		ON tAS.AgentSessionID = tCS.AgentSessionId
INNER JOIN tAgentDetails tAD		ON tAD.AgentID = tAS.AgentId
INNER JOIN tTerminals tT			ON tT.TerminalPK = tAS.TerminalPK
INNER JOIN tLocations tL			ON tL.LocationPK = tT.LocationPK
INNER JOIN tChannelPartners tCP		ON tL.ChannelPartnerId = tCP.ChannelPartnerId
WHERE tTC.TransactionID = @transactionID
END
