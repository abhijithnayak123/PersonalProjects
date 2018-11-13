--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to update column names and foreign key relationships>           
-- Jira ID:	<AL-243>
--===========================================================================================

/* table tTxn_Funds_Stage doesn't have a trigger(like other staging tables) to copy data to table tTxn_Funds_Stage_AUD,
AND the hence the latter has NO DATA IN production.*/
/**********************Renaming***************************/

EXEC sp_rename @objname='tCustomers.rowguid', @newname='CustomerPK', @objtype='COLUMN';
EXEC sp_rename @objname='tCustomers.id', @newname='CustomerID', @objtype='COLUMN';
GO
EXEC sp_rename @objname='tCustomers_AUD.rowguid', @newname='CustomerPK', @objtype='COLUMN';
EXEC sp_rename @objname='tCustomers_AUD.id', @newname='CustomerID', @objtype='COLUMN';
GO
EXEC sp_rename @objname='tCustomerAccounts.rowguid', @newname='AccountPK', @objtype='COLUMN';
EXEC sp_rename @objname='tCustomerAccounts.id', @newname='AccountID', @objtype='COLUMN';
GO
EXEC sp_rename @objname='tCustomerPreferedProducts.rowguid', @newname='CustProductPK', @objtype='COLUMN';
EXEC sp_rename @objname='tCustomerPreferedProducts.id', @newname='CustProductID', @objtype='COLUMN';
GO
EXEC sp_rename @objname='tTxn_BillPay_Stage.rowguid', @newname='BillPayPK', @objtype='COLUMN';
EXEC sp_rename @objname='tTxn_BillPay_Stage.id', @newname='BillPayID', @objtype='COLUMN';
EXEC sp_rename @objname='tTxn_BillPay_Stage_AUD.rowguid', @newname='BillPayPK', @objtype='COLUMN';
EXEC sp_rename @objname='tTxn_BillPay_Stage_AUD.id', @newname='BillPayID', @objtype='COLUMN';
EXEC sp_rename @objname='tTxn_BillPay_Commit.rowguid', @newname='BillPayPK', @objtype='COLUMN';
EXEC sp_rename @objname='tTxn_BillPay_Commit.id', @newname='BillPayID', @objtype='COLUMN';
GO
EXEC sp_rename @objname='tTxn_Cash_Stage.rowguid', @newname='CashPK', @objtype='COLUMN';
EXEC sp_rename @objname='tTxn_Cash_Stage.id', @newname='CashID', @objtype='COLUMN';
EXEC sp_rename @objname='tTxn_Cash_Stage_AUD.rowguid', @newname='CashPK', @objtype='COLUMN';
EXEC sp_rename @objname='tTxn_Cash_Stage_AUD.id', @newname='CashID', @objtype='COLUMN';
EXEC sp_rename @objname='tTxn_Cash_Commit.rowguid', @newname='CashPK', @objtype='COLUMN';
EXEC sp_rename @objname='tTxn_Cash_Commit.id', @newname='CashID', @objtype='COLUMN';
GO
EXEC sp_rename @objname='tTxn_Check_Stage.rowguid', @newname='CheckPK', @objtype='COLUMN';
EXEC sp_rename @objname='tTxn_Check_Stage.id', @newname='CheckID', @objtype='COLUMN';
EXEC sp_rename @objname='tTxn_Check_Stage_AUD.rowguid', @newname='CheckPK', @objtype='COLUMN';
EXEC sp_rename @objname='tTxn_Check_Stage_AUD.id', @newname='CheckID', @objtype='COLUMN';
EXEC sp_rename @objname='tTxn_Check_Commit.rowguid', @newname='CheckPK', @objtype='COLUMN';
EXEC sp_rename @objname='tTxn_Check_Commit.id', @newname='CheckID', @objtype='COLUMN';
GO
EXEC sp_rename @objname='tTxn_Funds_Stage.rowguid', @newname='FundsPK', @objtype='COLUMN';
EXEC sp_rename @objname='tTxn_Funds_Stage.id', @newname='FundsID', @objtype='COLUMN';
EXEC sp_rename @objname='tTxn_Funds_Stage_AUD.rowguid', @newname='FundsPK', @objtype='COLUMN';
EXEC sp_rename @objname='tTxn_Funds_Stage_AUD.id', @newname='FundsID', @objtype='COLUMN';
EXEC sp_rename @objname='tTxn_Funds_Commit.rowguid', @newname='FundsPK', @objtype='COLUMN';
EXEC sp_rename @objname='tTxn_Funds_Commit.id', @newname='FundsID', @objtype='COLUMN';
GO
EXEC sp_rename @objname='tTxn_MoneyOrder_Stage.rowguid', @newname='MoneyOrderPK', @objtype='COLUMN';
EXEC sp_rename @objname='tTxn_MoneyOrder_Stage.id', @newname='MoneyOrderID', @objtype='COLUMN';
EXEC sp_rename @objname='tTxn_MoneyOrder_Stage_AUD.rowguid', @newname='MoneyOrderPK', @objtype='COLUMN';
EXEC sp_rename @objname='tTxn_MoneyOrder_Stage_AUD.id', @newname='MoneyOrderID', @objtype='COLUMN';
EXEC sp_rename @objname='tTxn_MoneyOrder_Commit.rowguid', @newname='MoneyOrderPK', @objtype='COLUMN';
EXEC sp_rename @objname='tTxn_MoneyOrder_Commit.id', @newname='MoneyOrderID', @objtype='COLUMN';
GO
EXEC sp_rename @objname='tTxn_MoneyTransfer_Stage.rowguid', @newname='MoneyTransferPK', @objtype='COLUMN';
EXEC sp_rename @objname='tTxn_MoneyTransfer_Stage.id', @newname='MoneyTransferID', @objtype='COLUMN';
EXEC sp_rename @objname='tTxn_MoneyTransfer_Stage_AUD.rowguid', @newname='MoneyTransferPK', @objtype='COLUMN';
EXEC sp_rename @objname='tTxn_MoneyTransfer_Stage_AUD.id', @newname='MoneyTransferID', @objtype='COLUMN';
EXEC sp_rename @objname='tTxn_MoneyTransfer_Commit.rowguid', @newname='MoneyTransferPK', @objtype='COLUMN';
EXEC sp_rename @objname='tTxn_MoneyTransfer_Commit.id', @newname='MoneyTransferID', @objtype='COLUMN';
EXEC sp_rename @objname='tTxn_MoneyTransfer_Commit_AUD.rowguid', @newname='MoneyTransferPK', @objtype='COLUMN';
EXEC sp_rename @objname='tTxn_MoneyTransfer_Commit_AUD.id', @newname='MoneyTransferID', @objtype='COLUMN';
GO

