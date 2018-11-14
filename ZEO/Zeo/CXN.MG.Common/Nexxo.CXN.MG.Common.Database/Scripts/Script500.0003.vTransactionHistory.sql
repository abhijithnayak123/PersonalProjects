--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to alter view vTransactionHistory>           
-- Jira ID:	<AL-244>
--===========================================================================================

/****** Object:  View [dbo].[vTransactionHistory]    Script Date: 4/7/2015 3:06:49 PM ******/

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vTransactionHistory]'))
DROP VIEW [dbo].[vTransactionHistory]
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vTransactionHistory]
AS

SELECT	tCT.ChxrTrxPK AS rowguid, 
		tCT.ChxrTrxID AS CxnId,
		2 AS TxnType,
		tCTM.Name AS AddlInfo1,
		tca.ChxrAccountID AS CXNAccountId,
		200 as ProviderId,
		tCT.ChannelPartnerID AS ChannelPartnerId
FROM	tChxr_Trx tCT
		INNER JOIN tChxr_CheckTypeMapping tCTM ON tCT.ReturnType = tCTM.ChexarTypePK
		INNER JOIN tChxr_Account tCA ON tca.ChxrAccountPK = tct.ChxrAccountPK
WHERE	tCT.DTCreate > DATEADD(d, -90, GETDATE())

UNION ALL

SELECT	tFT.FViewTrxPK AS rowguid,
		tFT.FViewTrxID AS CxnId,
		3 AS TxnType, 
		'**** ' + tFC.CardNumber AddlInfo1,
		tFC.FViewCardID AS CXNAccountId,
		101 as ProviderId,
		tFT.ChannelPartnerID AS ChannelPartnerId
FROM	tFView_Trx tFT
		JOIN tFView_Card tFC ON tFT.FViewCardPK = tFC.FViewCardPK 
		WHERE	tFT.DTCreate > DATEADD(d, -90, GETDATE())
		
UNION ALL

SELECT  tFT.TSysTrxPK AS rowguid, 
		tFT.TSysTrxID AS CxnId, 
		3 AS TxnType,
		'****' + substring(tFC.CardNumber,len(tFC.CardNumber)-3,len(ltrim(rtrim(tFC.CardNumber)))) AddlInfo1,
		tfc.TSysAccountID AS CXNAccountId,
		600 as ProviderId,
		tFT.ChannelPartnerID AS ChannelPartnerId
FROM    tTSys_Trx tFT JOIN
        tTSys_Account tFC ON tFT.TSysAccountPK = tFC.TSysAccountPK
		WHERE  tFT.DTCreate > DATEADD(d, - 90, GETDATE())
GO


