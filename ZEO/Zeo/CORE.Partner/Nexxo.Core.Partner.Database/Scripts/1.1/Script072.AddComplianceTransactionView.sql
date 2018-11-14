

CREATE VIEW [dbo].[vComplianceTransactions]
AS
SELECT	tBP.txnRowguid rowguid, tBP.Id AS TransactionId, tPC.Id CustomerId, tBP.DTCreate, 4 AS TransactionType,
		tBP.Amount, tBP.Fee, tBP.CXEState AS [State], tA.ProviderId, lt.Id AS LedgerTransactionId, null as xRecipientId, null as xRate, 
		tBP.ProductId AS bpProductId, tBP.AccountNumber AS bpAccountNumber
FROM	tTxn_BillPay tBP
		INNER JOIN tAccounts tA ON tBP.AccountPK = tA.rowguid
		INNER JOIN tPartnerCustomers tPC ON tPC.rowguid = tA.CustomerPK 
		LEFT OUTER JOIN tLedgerEntries le ON tBP.LedgerEntryPK=le.rowguid
		LEFT OUTER JOIN tLedgerTransactions lt ON le.LedgerTransactionRowguid=lt.rowguid

UNION ALL

SELECT	tC.txnRowguid rowguid, tC.Id AS TransactionId, tPC.Id CustomerId, tC.DTCreate, 2 AS TransactionType,
		tC.Amount, tC.Fee, tC.CXEState AS [State], tA.ProviderId, lt.Id AS LedgerTransactionId, null as xRecipientId, null as xRate, 
		null AS bpProductId, null AS bpAccountNumber
FROM	tTxn_Check tC
		INNER JOIN tAccounts tA ON tC.AccountPK = tA.rowguid
		INNER JOIN tPartnerCustomers tPC ON tPC.rowguid = tA.CustomerPK 
		LEFT OUTER JOIN tLedgerEntries le ON tC.LedgerEntryPK=le.rowguid
		LEFT OUTER JOIN tLedgerTransactions lt ON le.LedgerTransactionRowguid=lt.rowguid

UNION ALL

SELECT	tMO.txnRowguid rowguid, tMO.Id AS TransactionId, tPC.Id CustomerId, tMO.DTCreate, 5 AS TransactionType,
		tMO.Amount, tMO.Fee, tMO.CXEState AS [State], tA.ProviderId, lt.Id AS LedgerTransactionId, null as xRecipientId, null as xRate, 
		null AS bpProductId, null AS bpAccountNumber
FROM	tTxn_MoneyOrder tMO
		INNER JOIN tAccounts tA ON tMO.AccountPK = tA.rowguid
		INNER JOIN tPartnerCustomers tPC ON tPC.rowguid = tA.CustomerPK 
		LEFT OUTER JOIN tLedgerEntries le ON tMO.LedgerEntryPK=le.rowguid
		LEFT OUTER JOIN tLedgerTransactions lt ON le.LedgerTransactionRowguid=lt.rowguid

UNION ALL

SELECT	tF.txnRowguid rowguid, tF.Id AS TransactionId, tPC.Id CustomerId, tF.DTCreate, 
		CASE tF.FundType WHEN 0 THEN 3 
						 WHEN 1 THEN 8 
						 ELSE 9 END AS TransactionType,
		tF.Amount, tF.Fee, tF.CXEState AS [State], tA.ProviderId, lt.Id AS LedgerTransactionId, null as xRecipientId, null as xRate, 
		null AS bpProductId, null AS bpAccountNumber
FROM	tTxn_Funds tF
		INNER JOIN tAccounts tA ON tF.AccountPK = tA.rowguid
		INNER JOIN tPartnerCustomers tPC ON tPC.rowguid = tA.CustomerPK 
		LEFT OUTER JOIN tLedgerEntries le ON tF.LedgerEntryPK=le.rowguid
		LEFT OUTER JOIN tLedgerTransactions lt ON le.LedgerTransactionRowguid=lt.rowguid

UNION ALL

SELECT	tCSH.txnRowguid rowguid, tCSH.Id AS TransactionId, tPC.Id CustomerId, tCSH.DTCreate, 
		CASE tCSH.CashType WHEN 1 THEN 1
							ELSE 7 END AS TransactionType,
		tCSH.Amount, tCSH.Fee, tCSH.CXEState AS [State], tA.ProviderId, lt.Id AS LedgerTransactionId, null as xRecipientId, null as xRate, 
		null AS bpProductId, null AS bpAccountNumber
FROM	tTxn_Cash tCSH
		INNER JOIN tAccounts tA ON tCSH.AccountPK = tA.rowguid
		INNER JOIN tPartnerCustomers tPC ON tPC.rowguid = tA.CustomerPK 
		LEFT OUTER JOIN tLedgerEntries le ON tCSH.LedgerEntryPK=le.rowguid
		LEFT OUTER JOIN tLedgerTransactions lt ON le.LedgerTransactionRowguid=lt.rowguid

UNION ALL

SELECT	tMT.txnRowguid rowguid, tMT.Id AS TransactionId, tPC.Id CustomerId, tMT.DTCreate, 6 AS TransactionType,
		tMT.Amount, tMT.Fee, tMT.CXEState AS [State], tA.ProviderId, lt.Id AS LedgerTransactionId, tMT.RecipientId as xRecipientId, 
		tMT.ExchangeRate as xRate, null AS bpProductId, null AS bpAccountNumber
FROM	tTxn_MoneyTransfer tMT
		INNER JOIN tAccounts tA ON tMT.AccountPK = tA.rowguid
		INNER JOIN tPartnerCustomers tPC ON tPC.rowguid = tA.CustomerPK 
		LEFT OUTER JOIN tLedgerEntries le ON tMT.LedgerEntryPK=le.rowguid
		LEFT OUTER JOIN tLedgerTransactions lt ON le.LedgerTransactionRowguid=lt.rowguid

GO