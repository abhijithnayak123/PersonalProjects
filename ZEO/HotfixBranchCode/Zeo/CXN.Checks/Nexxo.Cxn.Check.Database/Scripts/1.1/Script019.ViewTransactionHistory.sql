IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vTransactionHistory]'))
DROP VIEW [dbo].[vTransactionHistory]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[vTransactionHistory]
AS

SELECT	tCT.rowguid AS rowguid, 
		tCT.[Id] AS CxnId,
		2 AS TxnType,
		tCTM.Name AS AddlInfo1
FROM	tChxr_Trx tCT
		JOIN tChxr_CheckTypeMapping tCTM ON tCT.ReturnType = tCTM.ChexarType
WHERE	tCT.DTCreate > DATEADD(d, -90, GETDATE())

UNION ALL

SELECT	tFT.rowguid AS rowguid,
		tFT.[Id] AS CxnId,
		3 AS TxnType, 
		'**** ' + tFC.CardNumber AddlInfo1
FROM	tFView_Trx tFT
		JOIN tFView_Card tFC ON tFT.AccountPK = tFC.rowguid 
		WHERE	tFT.DTCreate > DATEADD(d, -90, GETDATE())
		
GO


