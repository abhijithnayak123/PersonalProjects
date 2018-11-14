If '$RestoreDB$' = 'true'
BEGIN

--USE [$DBPrifix$_CXE]


--/****** Object:  Synonym [dbo].[sODS_tAccounts]    Script Date: 12/19/2015 9:50:13 PM ******/
--DROP SYNONYM [dbo].[sODS_AgentLocationInfo]


--/****** Object:  Synonym [dbo].[sODS_AgentLocationInfo]    Script Date: 12/19/2015 9:42:38 PM ******/
--CREATE SYNONYM [dbo].[sODS_AgentLocationInfo] FOR [$DBPrifix$_PTNR].[dbo].[vODS_AgentLocationInfo]


--/****** Object:  Synonym [dbo].[sODS_tAccounts]    Script Date: 12/19/2015 9:50:13 PM ******/
--DROP SYNONYM [dbo].[sODS_CustomerIdInfo]


--/****** Object:  Synonym [dbo].[sODS_CustomerIdInfo]    Script Date: 12/19/2015 9:42:38 PM ******/
--CREATE SYNONYM [dbo].[sODS_CustomerIdInfo] FOR [$DBPrifix$_PTNR].[dbo].[vODS_CustomerIdInfo]


--/****** Object:  Synonym [dbo].[sODS_tAccounts]    Script Date: 12/19/2015 9:50:13 PM ******/
--DROP SYNONYM [dbo].[sODS_tAccounts]


--/****** Object:  Synonym [dbo].[sODS_tAccounts]    Script Date: 12/19/2015 9:42:38 PM ******/
--CREATE SYNONYM [dbo].[sODS_tAccounts] FOR [$DBPrifix$_PTNR].[dbo].[tAccounts]



--USE [$DBPrifix$_CXN]


--/****** Object:  Synonym [dbo].[sODS_tAccounts]    Script Date: 12/19/2015 9:50:13 PM ******/
--DROP SYNONYM [dbo].[sODS_tAccounts]


--/****** Object:  Synonym [dbo].[sODS_tAccounts]    Script Date: 12/19/2015 9:44:41 PM ******/
--CREATE SYNONYM [dbo].[sODS_tAccounts] FOR [$DBPrifix$_PTNR].[dbo].[tAccounts]


USE [$DBPrifix$_PTNR]

/****** Object:  Synonym [dbo].[sWUnion_Trx]    Script Date: 12/19/2015 9:50:13 PM ******/
DROP SYNONYM [dbo].[sWUnion_Trx]

/****** Object:  Synonym [dbo].[sWUnion_Countries]    Script Date: 12/19/2015 9:50:13 PM ******/
DROP SYNONYM [dbo].[sWUnion_Countries]

/****** Object:  Synonym [dbo].[sWUnion_BillPay_Trx]    Script Date: 12/19/2015 9:50:13 PM ******/
DROP SYNONYM [dbo].[sWUnion_BillPay_Trx]

/****** Object:  Synonym [dbo].[sVisa_Trx]    Script Date: 12/19/2015 9:50:13 PM ******/
DROP SYNONYM [dbo].[sVisa_Trx]

/****** Object:  Synonym [dbo].[sVisa_Account]    Script Date: 12/19/2015 9:50:13 PM ******/
DROP SYNONYM [dbo].[sVisa_Account]

/****** Object:  Synonym [dbo].[stxn_Check_Stage]    Script Date: 12/19/2015 9:50:13 PM ******/
--DROP SYNONYM [dbo].[stxn_Check_Stage]

/****** Object:  Synonym [dbo].[sTSys_Trx]    Script Date: 12/19/2015 9:50:13 PM ******/
DROP SYNONYM [dbo].[sTSys_Trx]

/****** Object:  Synonym [dbo].[sTSys_Account]    Script Date: 12/19/2015 9:50:13 PM ******/
DROP SYNONYM [dbo].[sTSys_Account]

/****** Object:  Synonym [dbo].[sTransHistory]    Script Date: 12/19/2015 9:50:13 PM ******/
DROP SYNONYM [dbo].[sTransHistory]

/****** Object:  Synonym [dbo].[stFISAccount]    Script Date: 12/19/2015 9:50:13 PM ******/
DROP SYNONYM [dbo].[stFISAccount]

/****** Object:  Synonym [dbo].[stCCISAccount]    Script Date: 12/19/2015 9:50:13 PM ******/
DROP SYNONYM [dbo].[stCCISAccount]

/****** Object:  Synonym [dbo].[sPastTransactions]    Script Date: 12/19/2015 9:50:13 PM ******/
DROP SYNONYM [dbo].[sPastTransactions]

/****** Object:  Synonym [dbo].[sMGram_Transfer_Trx]    Script Date: 12/19/2015 9:50:13 PM ******/
DROP SYNONYM [dbo].[sMGram_Transfer_Trx]

/****** Object:  Synonym [dbo].[sMGram_Countries]    Script Date: 12/19/2015 9:50:13 PM ******/
DROP SYNONYM [dbo].[sMGram_Countries]

/****** Object:  Synonym [dbo].[sMGram_BillPay_Trx]    Script Date: 12/19/2015 9:50:13 PM ******/
DROP SYNONYM [dbo].[sMGram_BillPay_Trx]

/****** Object:  Synonym [dbo].[sCustomer]    Script Date: 12/19/2015 9:50:13 PM ******/
--DROP SYNONYM [dbo].[sCustomer]

/****** Object:  Synonym [dbo].[sChxr_Trx]    Script Date: 12/19/2015 9:50:13 PM ******/
DROP SYNONYM [dbo].[sChxr_Trx]

/****** Object:  Synonym [dbo].[sCertegy_Trx]    Script Date: 12/19/2015 9:50:13 PM ******/
DROP SYNONYM [dbo].[sCertegy_Trx]

/****** Object:  Synonym [dbo].[sCertegy_Trx]    Script Date: 12/19/2015 9:50:13 PM ******/
--CREATE SYNONYM [dbo].[sCertegy_Trx] FOR [$DBPrifix$_CXN].[dbo].[tCertegy_Trx]

--/****** Object:  Synonym [dbo].[sChxr_Trx]    Script Date: 12/19/2015 9:50:13 PM ******/
--CREATE SYNONYM [dbo].[sChxr_Trx] FOR [$DBPrifix$_CXN].[dbo].[tChxr_Trx]

--/****** Object:  Synonym [dbo].[sCustomer]    Script Date: 12/19/2015 9:50:13 PM ******/
--CREATE SYNONYM [dbo].[sCustomer] FOR [$DBPrifix$_CXE].[dbo].[tCustomers]

--/****** Object:  Synonym [dbo].[sMGram_BillPay_Trx]    Script Date: 12/19/2015 9:50:13 PM ******/
--CREATE SYNONYM [dbo].[sMGram_BillPay_Trx] FOR [$DBPrifix$_CXN].[dbo].[tMGram_BillPay_Trx]

--/****** Object:  Synonym [dbo].[sMGram_Countries]    Script Date: 12/19/2015 9:50:13 PM ******/
--CREATE SYNONYM [dbo].[sMGram_Countries] FOR [$DBPrifix$_CXN].[dbo].[tMGram_Countries]

--/****** Object:  Synonym [dbo].[sMGram_Transfer_Trx]    Script Date: 12/19/2015 9:50:13 PM ******/
--CREATE SYNONYM [dbo].[sMGram_Transfer_Trx] FOR [$DBPrifix$_CXN].[dbo].[tMGram_Transfer_Trx]

--/****** Object:  Synonym [dbo].[sPastTransactions]    Script Date: 12/19/2015 9:50:13 PM ******/
--CREATE SYNONYM [dbo].[sPastTransactions] FOR [$DBPrifix$_CXN].[dbo].[vPastTransaction]

--/****** Object:  Synonym [dbo].[stCCISAccount]    Script Date: 12/19/2015 9:50:13 PM ******/
--CREATE SYNONYM [dbo].[stCCISAccount] FOR [$DBPrifix$_CXN].[dbo].[tCCIS_Account]

--/****** Object:  Synonym [dbo].[stFISAccount]    Script Date: 12/19/2015 9:50:14 PM ******/
--CREATE SYNONYM [dbo].[stFISAccount] FOR [$DBPrifix$_CXN].[dbo].[tFIS_Account]

--/****** Object:  Synonym [dbo].[sTransHistory]    Script Date: 12/19/2015 9:50:14 PM ******/
--CREATE SYNONYM [dbo].[sTransHistory] FOR [$DBPrifix$_CXN].[dbo].[vTransactionHistory]

--/****** Object:  Synonym [dbo].[sTSys_Account]    Script Date: 12/19/2015 9:50:14 PM ******/
--CREATE SYNONYM [dbo].[sTSys_Account] FOR [$DBPrifix$_CXN].[dbo].[tTSys_Account]

--/****** Object:  Synonym [dbo].[sTSys_Trx]    Script Date: 12/19/2015 9:50:14 PM ******/
--CREATE SYNONYM [dbo].[sTSys_Trx] FOR [$DBPrifix$_CXN].[dbo].[tTSys_Trx]

--/****** Object:  Synonym [dbo].[stxn_Check_Stage]    Script Date: 12/19/2015 9:50:14 PM ******/
--CREATE SYNONYM [dbo].[stxn_Check_Stage] FOR [$DBPrifix$_CXE].[dbo].[tTxn_Check_Stage]

--/****** Object:  Synonym [dbo].[sVisa_Account]    Script Date: 12/19/2015 9:50:14 PM ******/
--CREATE SYNONYM [dbo].[sVisa_Account] FOR [$DBPrifix$_CXN].[dbo].[tVisa_Account]

--/****** Object:  Synonym [dbo].[sVisa_Trx]    Script Date: 12/19/2015 9:50:14 PM ******/
--CREATE SYNONYM [dbo].[sVisa_Trx] FOR [$DBPrifix$_CXN].[dbo].[tVisa_Trx]

--/****** Object:  Synonym [dbo].[sWUnion_BillPay_Trx]    Script Date: 12/19/2015 9:50:14 PM ******/
--CREATE SYNONYM [dbo].[sWUnion_BillPay_Trx] FOR [$DBPrifix$_CXN].[dbo].[tWUnion_BillPay_Trx]

--/****** Object:  Synonym [dbo].[sWUnion_Countries]    Script Date: 12/19/2015 9:50:14 PM ******/
--CREATE SYNONYM [dbo].[sWUnion_Countries] FOR [$DBPrifix$_CXN].[dbo].[tWUnion_Countries]

--/****** Object:  Synonym [dbo].[sWUnion_Trx]    Script Date: 12/19/2015 9:50:14 PM ******/
--CREATE SYNONYM [dbo].[sWUnion_Trx] FOR [$DBPrifix$_CXN].[dbo].[tWUnion_Trx]


END
