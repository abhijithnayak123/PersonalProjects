IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vCashDrawerReport]'))
DROP VIEW [dbo].[vCashDrawerReport]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vCashDrawerReport]
AS
SELECT	NewID() AS rowguid, tAS.AgentId AgentId, tCA.CashType CashType, 
		tL.Id LocationId, tL.LocationName Location, SUM(Amount) Amount
FROM	tTxn_Cash tCA
		INNER JOIN tAccounts tA ON tCA.AccountPK = tA.rowguid
		INNER JOIN tPartnerCustomers tPC ON tPC.rowguid = tA.CustomerPK 
		INNER JOIN tCustomerSessions tCS ON tCS.customerSessionRowguid = tCA.CustomerSessionPK
		INNER JOIN tAgentSessions tAS ON tAS.rowguid = tCS.AgentSessionPK
		INNER JOIN tTerminals tT ON tAS.TerminalPK = tT.rowguid
		INNER JOIN tLocations tL ON tT.LocationPK = tL.rowguid
WHERE	tCA.DTCreate >= DATEADD(dd, DATEDIFF(dd, 0, GETDATE()) - 0, 0)
		AND tCA.DTCreate < DATEADD(dd, DATEDIFF(dd, 0, GETDATE())+1, 0)
		AND tCA.CXEState = 4 
GROUP BY tAS.AgentId, tCA.CashType, tL.Id,tL.LocationName

GO


