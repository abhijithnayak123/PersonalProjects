SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[vTransactionHistory]
AS

SELECT	tCT.rowguid AS rowguid, 
		tCT.[Id] AS CxnId,
		2 AS TxnType,
		tCTM.Name AS AddlInfo1,
		tca.Id AS CXNAccountId,
		200 as ProviderId,
		tCT.ChannelPartnerID AS ChannelPartnerId
FROM	tChxr_Trx tCT
		INNER JOIN tChxr_CheckTypeMapping tCTM ON tCT.ReturnType = tCTM.ChexarType
		INNER JOIN tChxr_Account tCA ON tca.rowguid = tct.ChxrAccountPK
WHERE	tCT.DTCreate > DATEADD(d, -90, GETDATE())

UNION ALL

SELECT	tFT.rowguid AS rowguid,
		tFT.[Id] AS CxnId,
		3 AS TxnType, 
		'**** ' + tFC.CardNumber AddlInfo1,
		tFC.Id AS CXNAccountId,
		101 as ProviderId,
		tFT.ChannelPartnerID AS ChannelPartnerId
FROM	tFView_Trx tFT
		JOIN tFView_Card tFC ON tFT.AccountPK = tFC.rowguid 
		WHERE	tFT.DTCreate > DATEADD(d, -90, GETDATE())
		
UNION ALL

SELECT  tFT.rowguid AS rowguid, 
		tFT.[Id] AS CxnId, 
		3 AS TxnType,
		'****' + substring(tFC.CardNumber,len(tFC.CardNumber)-3,len(ltrim(rtrim(tFC.CardNumber)))) AddlInfo1,
		tfc.Id AS CXNAccountId,
		600 as ProviderId,
		tFT.ChannelPartnerID AS ChannelPartnerId
FROM    tTSys_Trx tFT JOIN
        tTSys_Account tFC ON tFT.AccountPK = tFC.rowguid
		WHERE  tFT.DTCreate > DATEADD(d, - 90, GETDATE())
GO