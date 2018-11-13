--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to update column names and foreign key relationships>           
-- Jira ID:	<AL-242>
--===========================================================================================

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tStates'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tStates.id'
		,@newname = 'StatePK'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCountries'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	ALTER TABLE dbo.tCountries

	ALTER COLUMN ID

	DROP ROWGUIDCOL;

	EXEC sp_rename @objname = 'tCountries.ID'
		,@newname = 'CountryPK'
		,@objtype = 'COLUMN';

	ALTER TABLE dbo.tCountries

	ALTER COLUMN CountryPK ADD ROWGUIDCOL;
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tChannelPartners'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tChannelPartners.rowguid'
		,@newname = 'ChannelPartnerPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tChannelPartners'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tChannelPartners.id'
		,@newname = 'ChannelPartnerId'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tFeeAdjustmentConditions'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tFeeAdjustmentConditions.rowguid'
		,@newname = 'AdjConditionsPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tChannelPartnerFeeAdjustments'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tChannelPartnerFeeAdjustments.rowguid'
		,@newname = 'FeeAdjustmentPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTxn_FeeAdjustments'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tTxn_FeeAdjustments.rowguid'
		,@newname = 'TxnFeeAdjPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tChannelPartnerFees_MoneyOrder'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tChannelPartnerFees_MoneyOrder.rowguid'
		,@newname = 'FeeMoneyOrderPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tChannelPartnerFees_Check'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tChannelPartnerFees_Check.rowguid'
		,@newname = 'FeeCheckPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tChannelPartnerFees_Funds'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tChannelPartnerFees_Funds.rowguid'
		,@newname = 'FeeFundsPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tPartnerCustomerConfig'
			AND COLUMN_NAME = 'ChannelPartnerID'
		)
BEGIN
	EXEC sp_rename @objname = 'tPartnerCustomerConfig.ChannelPartnerID'
		,@newname = 'ChannelPartnerPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tProspects'
			AND COLUMN_NAME = 'ID'
		)
BEGIN
	EXEC sp_rename @objname = 'tProspects.id'
		,@newname = 'ProspectPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tProspects'
			AND COLUMN_NAME = 'ChannelPartnerId'
		)
BEGIN
	EXEC sp_rename @objname = 'tProspects.ChannelPartnerId'
		,@newname = 'ChannelPartnerPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tNexxoIdTypes'
			AND COLUMN_NAME = 'Rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tNexxoIdTypes.Rowguid'
		,@newname = 'NexxoIdTypePK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tProspectGovernmentIdDetails'
			AND COLUMN_NAME = 'IdTypePK'
		)
BEGIN
	EXEC sp_rename @objname = 'tProspectGovernmentIdDetails.IdTypePK'
		,@newname = 'NexxoIdTypePK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tNpsTerminals'
			AND COLUMN_NAME = 'Rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tNpsTerminals.Rowguid'
		,@newname = 'NpsTerminalPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTxn_Funds'
			AND COLUMN_NAME = 'txnRowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tTxn_Funds.txnRowguid'
		,@newname = 'TxnPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTxn_Funds'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tTxn_Funds.id'
		,@newname = 'TransactionID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTxn_MoneyOrder'
			AND COLUMN_NAME = 'txnRowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tTxn_MoneyOrder.txnRowguid'
		,@newname = 'TxnPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTxn_MoneyOrder'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tTxn_MoneyOrder.id'
		,@newname = 'TransactionID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTxn_MoneyTransfer'
			AND COLUMN_NAME = 'txnRowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tTxn_MoneyTransfer.txnRowguid'
		,@newname = 'TxnPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTxn_MoneyTransfer'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tTxn_MoneyTransfer.id'
		,@newname = 'TransactionID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTxn_Cash'
			AND COLUMN_NAME = 'txnRowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tTxn_Cash.txnRowguid'
		,@newname = 'TxnPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTxn_Cash'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tTxn_Cash.id'
		,@newname = 'TransactionID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTxn_Check'
			AND COLUMN_NAME = 'txnRowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tTxn_Check.txnRowguid'
		,@newname = 'TxnPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTxn_Check'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tTxn_Check.id'
		,@newname = 'TransactionID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTxn_BillPay'
			AND COLUMN_NAME = 'txnRowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tTxn_BillPay.txnRowguid'
		,@newname = 'TxnPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTxn_BillPay'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tTxn_BillPay.id'
		,@newname = 'TransactionID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tAccounts'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tAccounts.rowguid'
		,@newname = 'AccountPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tAccounts'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tAccounts.id'
		,@newname = 'AccountID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tShoppingCarts'
			AND COLUMN_NAME = 'CartRowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tShoppingCarts.CartRowguid'
		,@newname = 'CartPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tShoppingCarts_Aud'
			AND COLUMN_NAME = 'CartRowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tShoppingCarts_Aud.CartRowguid'
		,@newname = 'CartPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tPartnerCustomers'
			AND COLUMN_NAME = 'Rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tPartnerCustomers.Rowguid'
		,@newname = 'CustomerPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tPartnerCustomers_AUD'
			AND COLUMN_NAME = 'Rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tPartnerCustomers_AUD.Rowguid'
		,@newname = 'CustomerPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tPartnerCustomers'
			AND COLUMN_NAME = 'AgentSessionId'
		)
BEGIN
	EXEC sp_rename @objname = 'tPartnerCustomers.AgentSessionId'
		,@newname = 'AgentSessionPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tShoppingCartTransactions'
			AND COLUMN_NAME = 'CartRowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tShoppingCartTransactions.CartRowguid'
		,@newname = 'CartPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tShoppingCartTransactions'
			AND COLUMN_NAME = 'TxnRowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tShoppingCartTransactions.TxnRowguid'
		,@newname = 'TxnPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tShoppingCartTransactions'
			AND COLUMN_NAME = 'cartTxnRowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tShoppingCartTransactions.cartTxnRowguid'
		,@newname = 'CartTxnPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tShoppingCartTransactions_AUD'
			AND COLUMN_NAME = 'CartRowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tShoppingCartTransactions_AUD.CartRowguid'
		,@newname = 'CartPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tShoppingCartTransactions_AUD'
			AND COLUMN_NAME = 'TxnRowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tShoppingCartTransactions_AUD.TxnRowguid'
		,@newname = 'TxnPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTerminals'
			AND COLUMN_NAME = 'Rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tTerminals.Rowguid'
		,@newname = 'TerminalPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTerminals'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tTerminals.ID'
		,@newname = 'TerminalID'
		,@objtype = 'COLUMN' -- used in vCustomerSessions
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tAgentDetails'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	ALTER TABLE dbo.tAgentDetails

	ALTER COLUMN rowguid

	DROP ROWGUIDCOL;

	EXEC sp_rename @objname = 'tAgentDetails.rowguid'
		,@newname = 'AgentPK'
		,@objtype = 'COLUMN';-- used in vTransactionHistory

	ALTER TABLE dbo.tAgentDetails

	ALTER COLUMN AgentPK ADD ROWGUIDCOL;
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tAgentDetails'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tAgentDetails.Id'
		,@newname = 'AgentID'
		,@objtype = 'COLUMN' -- used in vTransactionHistory
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tAgentSessions'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tAgentSessions.Id'
		,@newname = 'AgentSessionID'
		,@objtype = 'COLUMN';-- used in vTransactionHistory
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tAgentSessions'
			AND COLUMN_NAME = 'Rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tAgentSessions.Rowguid'
		,@newname = 'AgentSessionPK'
		,@objtype = 'COLUMN';-- used in vTransactionHistory
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerSessions'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tCustomerSessions.Id'
		,@newname = 'CustomerSessionID'
		,@objtype = 'COLUMN';-- used in vTransactionHistory
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerSessions'
			AND COLUMN_NAME = 'CustomerSessionRowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tCustomerSessions.CustomerSessionRowguid'
		,@newname = 'CustomerSessionPK'
		,@objtype = 'COLUMN';-- used in vTransactionHistory
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerSessionShoppingCarts'
			AND COLUMN_NAME = 'CustomerSessionRowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tCustomerSessionShoppingCarts.CustomerSessionRowguid'
		,@newname = 'CustomerSessionPK'
		,@objtype = 'COLUMN';-- used in vTransactionHistory
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerSessionShoppingCarts'
			AND COLUMN_NAME = 'cartRowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tCustomerSessionShoppingCarts.cartRowguid'
		,@newname = 'CartPK'
		,@objtype = 'COLUMN';-- used in vTransactionHistory
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tIdentificationConfirmation'
			AND COLUMN_NAME = 'Rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tIdentificationConfirmation.Rowguid'
		,@newname = 'IdConfirmPK'
		,@objtype = 'COLUMN';-- used in vTransactionHistory
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tLocations'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tLocations.id'
		,@newname = 'LocationID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tLocations'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tLocations.rowguid'
		,@newname = 'LocationPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tStates'
			AND COLUMN_NAME = 'Countryid'
		)
BEGIN
	EXEC sp_rename @objname = 'tStates.Countryid'
		,@newname = 'CountryPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tNpsTerminals'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tNpsTerminals.id'
		,@newname = 'NpsTerminalID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tTipsAndOffers'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tTipsAndOffers.id'
		,@newname = 'TipAndOfferPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tNexxoIdTypes'
			AND COLUMN_NAME = 'StateID'
		)
BEGIN
	EXEC sp_rename @objname = 'tNexxoIdTypes.StateID'
		,@newname = 'StatePK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tNexxoIdTypes'
			AND COLUMN_NAME = 'Countryid'
		)
BEGIN
	EXEC sp_rename @objname = 'tNexxoIdTypes.Countryid'
		,@newname = 'CountryPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tNexxoIdTypes'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tNexxoIdTypes.id'
		,@newname = 'NexxoIdTypeID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tProspectEmploymentDetails'
			AND COLUMN_NAME = 'Prospectid'
		)
BEGIN
	EXEC sp_rename @objname = 'tProspectEmploymentDetails.Prospectid'
		,@newname = 'ProspectPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tProspectGovernmentIdDetails'
			AND COLUMN_NAME = 'Prospectid'
		)
BEGIN
	EXEC sp_rename @objname = 'tProspectGovernmentIdDetails.Prospectid'
		,@newname = 'ProspectPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tProspectGroupSettings'
			AND COLUMN_NAME = 'Prospectid'
		)
BEGIN
	EXEC sp_rename @objname = 'tProspectGroupSettings.Prospectid'
		,@newname = 'ProspectPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tPartnerCustomers_AUD'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tPartnerCustomers_AUD.id'
		,@newname = 'CustomerID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tPartnerCustomers'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tPartnerCustomers.id'
		,@newname = 'CustomerID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tShoppingCarts_AUD'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tShoppingCarts_AUD.id'
		,@newname = 'CartID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tShoppingCarts'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tShoppingCarts.id'
		,@newname = 'CartID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCheckTypes'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tCheckTypes.id'
		,@newname = 'CheckTypePK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tChannelPartnerFees_Check'
			AND COLUMN_NAME = 'CheckType'
		)
BEGIN
	EXEC sp_rename @objname = 'tChannelPartnerFees_Check.CheckType'
		,@newname = 'CheckTypePK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tIdentificationConfirmation'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tIdentificationConfirmation.id'
		,@newname = 'IdConfirmID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tContactTypes'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tContactTypes.id'
		,@newname = 'ContactTypePK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tFeeAdjustmentCompareTypes'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tFeeAdjustmentCompareTypes.id'
		,@newname = 'CompareTypePK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tFeeAdjustmentConditionTypes'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tFeeAdjustmentConditionTypes.id'
		,@newname = 'ConditionTypePK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tFeeAdjustmentConditions'
			AND COLUMN_NAME = 'CompareType'
		)
BEGIN
	EXEC sp_rename @objname = 'tFeeAdjustmentConditions.CompareType'
		,@newname = 'CompareTypePK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tFeeAdjustmentConditions'
			AND COLUMN_NAME = 'ConditionType'
		)
BEGIN
	EXEC sp_rename @objname = 'tFeeAdjustmentConditions.ConditionType'
		,@newname = 'ConditionTypePK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMessageCenter'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tMessageCenter.id'
		,@newname = 'MessageCenterID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMessageCenter'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tMessageCenter.rowguid'
		,@newname = 'MessageCenterPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMessageStore'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tMessageStore.id'
		,@newname = 'MessageStoreID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tPartnerCustomers_AUD'
			AND COLUMN_NAME = 'AgentSessionId'
		)
BEGIN
	EXEC sp_rename @objname = 'tPartnerCustomers_AUD.AgentSessionId'
		,@newname = 'AgentSessionPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tPartnerCustomers'
			AND COLUMN_NAME = 'ChannelPartnerID'
		)
BEGIN
	EXEC sp_rename @objname = 'tPartnerCustomers.ChannelPartnerID'
		,@newname = 'ChannelPartnerPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tPartnerCustomers_AUD'
			AND COLUMN_NAME = 'ChannelPartnerID'
		)
BEGIN
	EXEC sp_rename @objname = 'tPartnerCustomers_AUD.ChannelPartnerID'
		,@newname = 'ChannelPartnerPK'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tMessageStore'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	ALTER TABLE dbo.tMessageStore

	ALTER COLUMN rowguid

	DROP ROWGUIDCOL;

	EXEC sp_rename @objname = 'tMessageStore.rowguid'
		,@newname = 'MessageStorePK'
		,@objtype = 'COLUMN';

	ALTER TABLE dbo.tMessageStore

	ALTER COLUMN MessageStorePK ADD ROWGUIDCOL
END
GO

--tChannelPartnerSMTPDetails
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tChannelPartnerSMTPDetails'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tChannelPartnerSMTPDetails.id'
		,@newname = 'ChannelPartnerSMTPId'
		,@objtype = 'COLUMN'
END
GO

--tChannelPartnerGroups
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tChannelPartnerGroups'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tChannelPartnerGroups.id'
		,@newname = 'ChannelPartnerGroupIdPK'
		,@objtype = 'COLUMN'
END
GO

--tChannelPartnerGroupSettings
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tPartnerCustomerGroupSettings'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tPartnerCustomerGroupSettings.rowguid'
		,@newname = 'PCGroupSettingPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tPartnerCustomerGroupSettings_AUD'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tPartnerCustomerGroupSettings_AUD.rowguid'
		,@newname = 'PCGroupSettingPK'
		,@objtype = 'COLUMN'
END
GO

--tProspectGroupSettings
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tProspectGroupSettings'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tProspectGroupSettings.id'
		,@newname = 'ProspectGroupId'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tProspectGroupSettings'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tProspectGroupSettings.rowguid'
		,@newname = 'ProspectGroupPK'
		,@objtype = 'COLUMN'
END
GO

--ALTER TABLE tNexxoIdTypes ALTER COLUMN StatePK UNIQUEIDENTIFIER NOT NULL;   -has records with null statePKs
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tNexxoIdTypes'
			AND COLUMN_NAME = 'CountryPK'
		)
BEGIN
	ALTER TABLE tNexxoIdTypes

	ALTER COLUMN CountryPK UNIQUEIDENTIFIER NOT NULL;
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerSessions'
			AND COLUMN_NAME = 'AgentSessionPK'
		)
BEGIN
	ALTER TABLE tCustomerSessions

	ALTER COLUMN AgentSessionPK UNIQUEIDENTIFIER NULL;
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tChannelPartnerConfig'
			AND COLUMN_NAME = 'ChannelPartnerID'
		)
BEGIN
	EXEC sp_rename @objname = 'tChannelPartnerConfig.ChannelPartnerID'
		,@newname = 'ChannelPartnerPK'
		,@objtype = 'COLUMN'
END
GO

--FOREIGN KEYS
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tChannelPartners'
			AND COLUMN_NAME = 'ChannelPartnerPK'
		)
BEGIN
	IF EXISTS (
			SELECT 1
			FROM INFORMATION_SCHEMA.COLUMNS
			WHERE TABLE_NAME = 'tProspects'
				AND COLUMN_NAME = 'ChannelPartnerPK'
			)
	BEGIN
		ALTER TABLE tProspects

		ALTER COLUMN ChannelPartnerPK UNIQUEIDENTIFIER NOT NULL;

		IF NOT EXISTS (
				SELECT 1
				FROM sys.foreign_keys AS f
				INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id
				INNER JOIN sys.objects AS o ON o.OBJECT_ID = fc.referenced_object_id
				WHERE OBJECT_NAME(f.parent_object_id) = 'tProspects'
					AND COL_NAME(fc.parent_object_id, fc.parent_column_id) = 'ChannelPartnerPK'
					AND OBJECT_NAME(f.referenced_object_id) = 'tChannelPartners'
					AND COL_NAME(fc.referenced_object_id, fc.referenced_column_id) = 'ChannelPartnerPK'
				)
		BEGIN
			ALTER TABLE [dbo].[tProspects]
				WITH CHECK ADD CONSTRAINT [FK_tProspects_tChannelPartners] FOREIGN KEY ([ChannelPartnerPK]) REFERENCES [dbo].[tChannelPartners]([ChannelPartnerPK]);
		END
	END

	IF NOT EXISTS (
			SELECT 1
			FROM sys.foreign_keys AS f
			INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id
			INNER JOIN sys.objects AS o ON o.OBJECT_ID = fc.referenced_object_id
			WHERE OBJECT_NAME(f.parent_object_id) = 'tChannelPartnerConfig'
				AND COL_NAME(fc.parent_object_id, fc.parent_column_id) = 'ChannelPartnerPK'
				AND OBJECT_NAME(f.referenced_object_id) = 'tChannelPartners'
				AND COL_NAME(fc.referenced_object_id, fc.referenced_column_id) = 'ChannelPartnerPK'
			)
	BEGIN
		ALTER TABLE [dbo].[tChannelPartnerConfig]
			WITH CHECK ADD CONSTRAINT [FK_tChannelPartnerConfig_tChannelPartners] FOREIGN KEY ([ChannelPartnerPK]) REFERENCES [dbo].[tChannelPartners]([ChannelPartnerPK]);
	END

	IF EXISTS (
			SELECT 1
			FROM INFORMATION_SCHEMA.COLUMNS
			WHERE TABLE_NAME = 'tPartnerCustomers'
				AND COLUMN_NAME = 'ChannelPartnerPK'
			)
	BEGIN
		ALTER TABLE tPartnerCustomers

		ALTER COLUMN ChannelPartnerPK UNIQUEIDENTIFIER NOT NULL;

		IF NOT EXISTS (
				SELECT 1
				FROM sys.foreign_keys AS f
				INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id
				INNER JOIN sys.objects AS o ON o.OBJECT_ID = fc.referenced_object_id
				WHERE OBJECT_NAME(f.parent_object_id) = 'tPartnerCustomers'
					AND COL_NAME(fc.parent_object_id, fc.parent_column_id) = 'ChannelPartnerPK'
					AND OBJECT_NAME(f.referenced_object_id) = 'tChannelPartners'
					AND COL_NAME(fc.referenced_object_id, fc.referenced_column_id) = 'ChannelPartnerPK'
				)
		BEGIN
			ALTER TABLE [dbo].[tPartnerCustomers]
				WITH CHECK ADD CONSTRAINT [FK_tPartnerCustomers_tChannelPartners] FOREIGN KEY ([ChannelPartnerPK]) REFERENCES [dbo].[tChannelPartners]([ChannelPartnerPK]);
		END
	END
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tFeeAdjustmentConditions'
			AND COLUMN_NAME = 'CompareType'
		)
BEGIN
	ALTER TABLE [dbo].[tFeeAdjustmentConditions]

	ALTER COLUMN CompareType TINYINT NOT NULL;

	IF NOT EXISTS (
			SELECT 1
			FROM sys.foreign_keys AS f
			INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id
			INNER JOIN sys.objects AS o ON o.OBJECT_ID = fc.referenced_object_id
			WHERE OBJECT_NAME(f.parent_object_id) = 'tFeeAdjustmentConditions'
				AND COL_NAME(fc.parent_object_id, fc.parent_column_id) = 'CompareTypePK'
				AND OBJECT_NAME(f.referenced_object_id) = 'tFeeAdjustmentCompareTypes'
				AND COL_NAME(fc.referenced_object_id, fc.referenced_column_id) = 'CompareTypePK'
			)
	BEGIN
		ALTER TABLE [dbo].[tFeeAdjustmentConditions]
			WITH CHECK ADD CONSTRAINT [FK_tFeeAdjConditions_tFeeAdjCompareTypes] FOREIGN KEY ([CompareTypePK]) REFERENCES [dbo].[tFeeAdjustmentCompareTypes]([CompareTypePK]);
	END
END
GO

