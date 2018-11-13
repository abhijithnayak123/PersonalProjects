--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to update vCashDrawerReport view>           
-- Jira ID:	<AL-242>
--===========================================================================================

/****** Object:  View [dbo].[vCashDrawerReport]    Script Date: 3/27/2015 10:41:32 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
DROP VIEW [dbo].[vCashDrawerReport]
GO
CREATE VIEW [dbo].[vCashDrawerReport]
AS
SELECT	NEWID() AS rowguid, tAS.AgentId AgentId, tCA.CashType CashType, 
		tL.LocationID LocationId, tL.LocationName Location, SUM(Amount) Amount
FROM	tTxn_Cash tCA
		INNER JOIN tAccounts tA ON tCA.AccountPK = tA.AccountPK
		INNER JOIN tPartnerCustomers tPC ON tPC.CustomerPK = tA.CustomerPK 
		INNER JOIN tCustomerSessions tCS ON tCS.CustomerSessionPK = tCA.CustomerSessionPK
		INNER JOIN tAgentSessions tAS ON tAS.AgentSessionPK = tCS.AgentSessionPK
		INNER JOIN tTerminals tT ON tAS.TerminalPK = tT.TerminalPK
		INNER JOIN tLocations tL ON tT.LocationPK = tL.LocationPK
WHERE	tCA.DTCreate >= DATEADD(dd, DATEDIFF(dd, 0, GETDATE()) - 0, 0)
		AND tCA.DTCreate < DATEADD(dd, DATEDIFF(dd, 0, GETDATE())+1, 0)
		AND tCA.CXEState = 4 
GROUP BY tAS.AgentId, tCA.CashType, tL.LocationID,tL.LocationName
GO


