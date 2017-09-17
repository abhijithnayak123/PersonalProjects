-- ============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <09/07/2015>
-- Description:	<Script to create synonyms to get the transaction details.>
-- Jira ID:	<AL-798>
-- =============================================================================

-- CXE database synonyms
IF EXISTS (SELECT * FROM sys.synonyms WHERE name = N'stxn_Check_Stage')
	DROP SYNONYM [dbo].[stxn_Check_Stage]
GO

CREATE SYNONYM [dbo].[stxn_Check_Stage] FOR [$PTNRDATABASE$].[dbo].[tTxn_Check_Stage]
GO

-- CXN database synonyms

IF EXISTS (SELECT * FROM sys.synonyms WHERE name = N'sChxr_Trx')
	DROP SYNONYM [dbo].[sChxr_Trx]
GO

CREATE SYNONYM [dbo].[sChxr_Trx] FOR [$CXNDATABASE$].[dbo].[tChxr_Trx]
GO

IF EXISTS (SELECT * FROM sys.synonyms WHERE name = N'sCertegy_Trx')
	DROP SYNONYM [dbo].[sCertegy_Trx]
GO

CREATE SYNONYM [dbo].[sCertegy_Trx] FOR [$CXNDATABASE$].[dbo].[tCertegy_Trx]
GO

IF EXISTS (SELECT * FROM sys.synonyms WHERE name = N'sMGram_BillPay_Trx')
	DROP SYNONYM [dbo].[sMGram_BillPay_Trx]
GO

CREATE SYNONYM [dbo].[sMGram_BillPay_Trx] FOR [$CXNDATABASE$].[dbo].[tMGram_BillPay_Trx]
GO

IF EXISTS (SELECT * FROM sys.synonyms WHERE name = N'sMGram_Countries')
	DROP SYNONYM [dbo].[sMGram_Countries]
GO

CREATE SYNONYM [dbo].[sMGram_Countries] FOR [$CXNDATABASE$].[dbo].[tMGram_Countries]
GO
 
IF EXISTS (SELECT * FROM sys.synonyms WHERE name = N'sMGram_Transfer_Trx')
	DROP SYNONYM [dbo].[sMGram_Transfer_Trx]
GO

CREATE SYNONYM [dbo].[sMGram_Transfer_Trx] FOR [$CXNDATABASE$].[dbo].[tMGram_Transfer_Trx]
GO

IF EXISTS (SELECT * FROM sys.synonyms WHERE name = N'sTSys_Account')
	DROP SYNONYM [dbo].[sTSys_Account]
GO

CREATE SYNONYM [dbo].[sTSys_Account] FOR [$CXNDATABASE$].[dbo].[tTSys_Account]
GO

IF EXISTS (SELECT * FROM sys.synonyms WHERE name = N'sTSys_Trx')
	DROP SYNONYM [dbo].[sTSys_Trx]
GO

CREATE SYNONYM [dbo].[sTSys_Trx] FOR [$CXNDATABASE$].[dbo].[tTSys_Trx]
GO



IF EXISTS (SELECT * FROM sys.synonyms WHERE name = N'sVisa_Account')
	DROP SYNONYM [dbo].[sVisa_Account]
GO

CREATE SYNONYM [dbo].[sVisa_Account] FOR [$CXNDATABASE$].[dbo].[tVisa_Account]
GO

IF EXISTS (SELECT * FROM sys.synonyms WHERE name = N'sVisa_Trx')
	DROP SYNONYM [dbo].[sVisa_Trx]
GO

CREATE SYNONYM [dbo].[sVisa_Trx] FOR [$CXNDATABASE$].[dbo].[tVisa_Trx]
GO

IF EXISTS (SELECT * FROM sys.synonyms WHERE name = N'sWUnion_BillPay_Trx')
	DROP SYNONYM [dbo].[sWUnion_BillPay_Trx]
GO

CREATE SYNONYM [dbo].[sWUnion_BillPay_Trx] FOR [$CXNDATABASE$].[dbo].[tWUnion_BillPay_Trx]
GO
 
IF EXISTS (SELECT * FROM sys.synonyms WHERE name = N'sWUnion_Countries')
	DROP SYNONYM [dbo].[sWUnion_Countries]
GO

CREATE SYNONYM [dbo].[sWUnion_Countries] FOR [$CXNDATABASE$].[dbo].[tWUnion_Countries]
GO

IF EXISTS (SELECT * FROM sys.synonyms WHERE name = N'sWUnion_Trx')
	DROP SYNONYM [dbo].[sWUnion_Trx]
GO

CREATE SYNONYM [dbo].[sWUnion_Trx] FOR [$CXNDATABASE$].[dbo].[tWUnion_Trx]
GO