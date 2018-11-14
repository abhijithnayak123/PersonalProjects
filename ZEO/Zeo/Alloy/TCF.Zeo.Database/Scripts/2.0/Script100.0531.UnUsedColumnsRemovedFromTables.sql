--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <05-10-2017>
-- Description:	Drop the PK columns/Unused columns from the table as part New DB design
-- Jira ID:		<>
-- ================================================================================

----- ReName ChannelPartnerId to ChannelPartnerPK
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tUserRolesPermissionsMapping' AND COLUMN_NAME = 'ChannelPartnerId' AND DATA_TYPE ='UNIQUEIDENTIFIER')
BEGIN
	 EXEC sp_RENAME '[tUserRolesPermissionsMapping].[ChannelPartnerId]' , 'ChannelPartnerPK' , 'COLUMN'	 
END
GO


IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tUserRolesPermissionsMapping' AND COLUMN_NAME = 'ChannelPartnerId')
BEGIN
    ALTER TABLE tUserRolesPermissionsMapping ADD ChannelPartnerId BIGINT NULL
END
GO 

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tUserRolesPermissionsMapping' AND COLUMN_NAME = 'ChannelPartnerPK' AND DATA_TYPE ='UNIQUEIDENTIFIER' )
BEGIN
    ALTER TABLE tUserRolesPermissionsMapping ADD ChannelPartnerPK UNIQUEIDENTIFIER NULL
END
GO 

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartners' AND COLUMN_NAME = 'ChannelPartnerPK' AND DATA_TYPE ='UNIQUEIDENTIFIER' )
BEGIN
    ALTER TABLE tChannelPartners ADD ChannelPartnerPK UNIQUEIDENTIFIER NULL
END
GO 


UPDATE pm
SET pm.ChannelPartnerId = cp.ChannelPartnerId
FROM
    tUserRolesPermissionsMapping pm
INNER JOIN 
    tChannelPartners cp
ON  pm.ChannelPartnerPK = cp.ChannelPartnerPK


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tUserRolesPermissionsMapping' AND COLUMN_NAME = 'ChannelPartnerId')
BEGIN
    ALTER TABLE tUserRolesPermissionsMapping ALTER COLUMN ChannelPartnerId BIGINT NOT NULL
END
GO

--================== Drop UnUsed columns from Ch transaction related tables =================================


IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tCheckImages_CheckPK' )
BEGIN
	ALTER TABLE tCheckImages DROP CONSTRAINT DF_tCheckImages_CheckPK 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCheckImages' AND COLUMN_NAME = 'CheckPK')
BEGIN
    ALTER TABLE tCheckImages DROP COLUMN CheckPK	
END
GO 

IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tChxr_Account_ChxrAccountPK' )
BEGIN
	ALTER TABLE tChxr_Account DROP CONSTRAINT DF_tChxr_Account_ChxrAccountPK 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account' AND COLUMN_NAME = 'ChxrAccountPK')
BEGIN
    ALTER TABLE tChxr_Account DROP COLUMN ChxrAccountPK	
END
GO 

IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tChxr_Account_Aud_ChxrAccountPK' )
BEGIN
	ALTER TABLE tChxr_Account_Aud DROP CONSTRAINT DF_tChxr_Account_Aud_ChxrAccountPK 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account_Aud' AND COLUMN_NAME = 'ChxrAccountPK')
BEGIN
    ALTER TABLE tChxr_Account_Aud DROP COLUMN ChxrAccountPK	
END
GO

IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tChxr_Partner_ChxrPartnerPK' )
BEGIN
	ALTER TABLE tChxr_Partner DROP CONSTRAINT DF_tChxr_Partner_ChxrPartnerPK 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Partner' AND COLUMN_NAME = 'ChxrPartnerPK')
BEGIN
    ALTER TABLE tChxr_Partner DROP COLUMN ChxrPartnerPK	
END
GO

IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tChxr_Session_ChxrSessionPK' )
BEGIN
	ALTER TABLE tChxr_Session DROP CONSTRAINT DF_tChxr_Session_ChxrSessionPK 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Session' AND COLUMN_NAME = 'ChxrSessionPK')
BEGIN
    ALTER TABLE tChxr_Session DROP COLUMN ChxrSessionPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Session' AND COLUMN_NAME = 'ChxrPartnerPK')
BEGIN
    ALTER TABLE tChxr_Session DROP COLUMN ChxrPartnerPK	
END
GO

IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tChxr_Trx_ChxrTrxPK' )
BEGIN
	ALTER TABLE tChxr_Trx DROP CONSTRAINT DF_tChxr_Trx_ChxrTrxPK 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Trx' AND COLUMN_NAME = 'ChxrTrxPK')
BEGIN
    ALTER TABLE tChxr_Trx DROP COLUMN ChxrTrxPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Trx' AND COLUMN_NAME = 'ChxrAccountPK')
BEGIN
    ALTER TABLE tChxr_Trx DROP COLUMN ChxrAccountPK	
END
GO

IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tChxr_Trx_Aud_ChxrTrxPK' )
BEGIN
	ALTER TABLE tChxr_Trx_Aud DROP CONSTRAINT DF_tChxr_Trx_Aud_ChxrTrxPK 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Trx_Aud' AND COLUMN_NAME = 'ChxrTrxPK')
BEGIN
    ALTER TABLE tChxr_Trx_Aud DROP COLUMN ChxrTrxPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Trx_Aud' AND COLUMN_NAME = 'ChxrAccountPK')
BEGIN
    ALTER TABLE tChxr_Trx_Aud DROP COLUMN ChxrAccountPK	
END
GO

IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tChxrSim_Account_ChxrSimAccountPK' )
BEGIN
	ALTER TABLE tChxrSim_Account DROP CONSTRAINT DF_tChxrSim_Account_ChxrSimAccountPK 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxrSim_Account' AND COLUMN_NAME = 'ChxrSimAccountPK')
BEGIN
    ALTER TABLE tChxrSim_Account DROP COLUMN ChxrSimAccountPK	
END
GO

IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tChxrSim_Invoice_ChxrSimInvoicePK' )
BEGIN
	ALTER TABLE tChxrSim_Invoice DROP CONSTRAINT DF_tChxrSim_Invoice_ChxrSimInvoicePK 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxrSim_Invoice' AND COLUMN_NAME = 'ChxrSimInvoicePK')
BEGIN
    ALTER TABLE tChxrSim_Invoice DROP COLUMN ChxrSimInvoicePK	
END
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxrSim_Invoice' AND COLUMN_NAME = 'ChxrSimAccountPK')
BEGIN
    ALTER TABLE tChxrSim_Invoice DROP COLUMN ChxrSimAccountPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxrSim_Invoice' AND COLUMN_NAME = 'ChxrSimAccountPK')
BEGIN
    ALTER TABLE tChxrSim_Invoice DROP COLUMN ChxrSimAccountPK	
END
GO

IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tTxn_Check_TxnPK' )
BEGIN
	ALTER TABLE tTxn_Check DROP CONSTRAINT DF_tTxn_Check_TxnPK 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_Check' AND COLUMN_NAME = 'TxnPK')
BEGIN
    ALTER TABLE tTxn_Check DROP COLUMN TxnPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_Check' AND COLUMN_NAME = 'CXEId')
BEGIN
    ALTER TABLE tTxn_Check DROP COLUMN CXEId	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_Check' AND COLUMN_NAME = 'CustomerSessionPK')
BEGIN
    ALTER TABLE tTxn_Check DROP COLUMN CustomerSessionPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_Check' AND COLUMN_NAME = 'AccountPK')
BEGIN
    ALTER TABLE tTxn_Check DROP COLUMN AccountPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_Check' AND COLUMN_NAME = 'CXNState')
BEGIN
    ALTER TABLE tTxn_Check DROP COLUMN CXNState	
END
GO

--================== Drop UnUsed columns from Bill Pay transaction related tables =================================

IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tTxn_Billpay_txnPK' )
BEGIN
	ALTER TABLE tTxn_BillPay DROP CONSTRAINT DF_tTxn_Billpay_txnPK 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_BillPay' AND COLUMN_NAME = 'TxnPK')
BEGIN
    ALTER TABLE tTxn_BillPay DROP COLUMN TxnPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_BillPay' AND COLUMN_NAME = 'CXEId')
BEGIN
    ALTER TABLE tTxn_BillPay DROP COLUMN CXEId	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_BillPay' AND COLUMN_NAME = 'CustomerSessionPK')
BEGIN
    ALTER TABLE tTxn_BillPay DROP COLUMN CustomerSessionPK	
END
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_BillPay' AND COLUMN_NAME = 'AccountPK')
BEGIN
    ALTER TABLE tTxn_BillPay DROP COLUMN AccountPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_BillPay' AND COLUMN_NAME = 'CXNState')
BEGIN
    ALTER TABLE tTxn_BillPay DROP COLUMN CXNState	
END
GO

IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tWUnion_BillPay_Account_PK' )
BEGIN
	ALTER TABLE tWUnion_BillPay_Account DROP CONSTRAINT DF_tWUnion_BillPay_Account_PK 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_BillPay_Account' AND COLUMN_NAME = 'WUBillPayAccountPK')
BEGIN
    ALTER TABLE tWUnion_BillPay_Account DROP COLUMN WUBillPayAccountPK	
END
GO

IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tWUnion_BillPay_Trx_WUtrxPK' )
BEGIN
	ALTER TABLE tWUnion_BillPay_Trx DROP CONSTRAINT DF_tWUnion_BillPay_Trx_WUtrxPK 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_BillPay_Trx' AND COLUMN_NAME = 'WUBillPayTrxPK')
BEGIN
    ALTER TABLE tWUnion_BillPay_Trx DROP COLUMN WUBillPayTrxPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_BillPay_Trx' AND COLUMN_NAME = 'WUBillPayAccountPK')
BEGIN
    ALTER TABLE tWUnion_BillPay_Trx DROP COLUMN WUBillPayAccountPK	
END
GO

IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tCustomerPreferedProducts_PK' )
BEGIN
	ALTER TABLE tCustomerPreferedProducts DROP CONSTRAINT DF_tCustomerPreferedProducts_PK 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomerPreferedProducts' AND COLUMN_NAME = 'CustProductPK')
BEGIN
    ALTER TABLE tCustomerPreferedProducts DROP COLUMN CustProductPK	
END
GO

IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tMasterCatalog_masterCatalogPK' )
BEGIN
	ALTER TABLE tMasterCatalog DROP CONSTRAINT DF_tMasterCatalog_masterCatalogPK 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tMasterCatalog' AND COLUMN_NAME = 'MasterCatalogPK')
BEGIN
    ALTER TABLE tMasterCatalog DROP COLUMN MasterCatalogPK	
END
GO

IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tPartnerCatalog_catalogPK' )
BEGIN
	ALTER TABLE tPartnerCatalog DROP CONSTRAINT DF_tPartnerCatalog_catalogPK 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tPartnerCatalog' AND COLUMN_NAME = 'tPartnerCatalogPK')
BEGIN
    ALTER TABLE tPartnerCatalog DROP COLUMN tPartnerCatalogPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tPartnerCatalog' AND COLUMN_NAME = 'MasterCatalogPK')
BEGIN
    ALTER TABLE tPartnerCatalog DROP COLUMN MasterCatalogPK	
END
GO


--================== Drop UnUsed columns from Money Order transaction related tables =================================

IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tMoneyOrderImage_MoneyOrderImagePK' )
BEGIN
	ALTER TABLE tMoneyOrderImage DROP CONSTRAINT DF_tMoneyOrderImage_MoneyOrderImagePK 
END
GO

IF EXISTS( SELECT 1 FROM sys.indexes WHERE name = 'IX_tMoneyOrderImage_TrxId' )
BEGIN
	DROP INDEX IX_tMoneyOrderImage_TrxId ON tMoneyOrderImage 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tMoneyOrderImage' AND COLUMN_NAME = 'MoneyOrderImagePK')
BEGIN
    ALTER TABLE tMoneyOrderImage DROP COLUMN MoneyOrderImagePK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tMoneyOrderImage' AND COLUMN_NAME = 'TrxId')
BEGIN
    ALTER TABLE tMoneyOrderImage DROP COLUMN TrxId	
END
GO

IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tTxn_MoneyOrder_txnPK' )
BEGIN
	ALTER TABLE tTxn_MoneyOrder DROP CONSTRAINT DF_tTxn_MoneyOrder_txnPK 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_MoneyOrder' AND COLUMN_NAME = 'TxnPK')
BEGIN
    ALTER TABLE tTxn_MoneyOrder DROP COLUMN TxnPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_MoneyOrder' AND COLUMN_NAME = 'CXEId')
BEGIN
    ALTER TABLE tTxn_MoneyOrder DROP COLUMN CXEId	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_MoneyOrder' AND COLUMN_NAME = 'CXNId')
BEGIN
    ALTER TABLE tTxn_MoneyOrder DROP COLUMN CXNId	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_MoneyOrder' AND COLUMN_NAME = 'CustomerSessionPK')
BEGIN
    ALTER TABLE tTxn_MoneyOrder DROP COLUMN CustomerSessionPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_MoneyOrder' AND COLUMN_NAME = 'AccountPK')
BEGIN
    ALTER TABLE tTxn_MoneyOrder DROP COLUMN AccountPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_MoneyOrder' AND COLUMN_NAME = 'CXNState')
BEGIN
    ALTER TABLE tTxn_MoneyOrder DROP COLUMN CXNState	
END
GO

--================== Drop UnUsed columns from Visa transaction related tables =================================

IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tVisa_Account_PK' )
BEGIN
	ALTER TABLE tVisa_Account DROP CONSTRAINT DF_tVisa_Account_PK 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_Account' AND COLUMN_NAME = 'VisaAccountPK')
BEGIN
    ALTER TABLE tVisa_Account DROP COLUMN VisaAccountPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_CardClass' AND COLUMN_NAME = 'VisaCardClassPK')
BEGIN
    ALTER TABLE tVisa_CardClass DROP COLUMN VisaCardClassPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_CardShippingTypes' AND COLUMN_NAME = 'ShippingTypePK')
BEGIN
    ALTER TABLE tVisa_CardShippingTypes DROP COLUMN ShippingTypePK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_ChannelPartnerFeeTypeMapping' AND COLUMN_NAME = 'ChannelPartnerFeeTypeMappingPK')
BEGIN
    ALTER TABLE tVisa_ChannelPartnerFeeTypeMapping DROP COLUMN ChannelPartnerFeeTypeMappingPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_ChannelPartnerFeeTypeMapping' AND COLUMN_NAME = 'VisaFeeTypePK')
BEGIN
    ALTER TABLE tVisa_ChannelPartnerFeeTypeMapping DROP COLUMN VisaFeeTypePK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_ChannelPartnerShippingTypeMapping' AND COLUMN_NAME = 'ChannelPartnerShippingTypePK')
BEGIN
    ALTER TABLE tVisa_ChannelPartnerShippingTypeMapping DROP COLUMN ChannelPartnerShippingTypePK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_ChannelPartnerShippingTypeMapping' AND COLUMN_NAME = 'ShippingTypePK')
BEGIN
    ALTER TABLE tVisa_ChannelPartnerShippingTypeMapping DROP COLUMN ShippingTypePK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_Credential' AND COLUMN_NAME = 'VisaCredentialPK')
BEGIN
    ALTER TABLE tVisa_Credential DROP COLUMN VisaCredentialPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_Fee' AND COLUMN_NAME = 'VisaFeePK')
BEGIN
    ALTER TABLE tVisa_Fee DROP COLUMN VisaFeePK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_Fee' AND COLUMN_NAME = 'ChannelPartnerFeeTypePK')
BEGIN
    ALTER TABLE tVisa_Fee DROP COLUMN ChannelPartnerFeeTypePK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_FeeTypes' AND COLUMN_NAME = 'VisaFeeTypePK')
BEGIN
    ALTER TABLE tVisa_FeeTypes DROP COLUMN VisaFeeTypePK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_ShippingFee' AND COLUMN_NAME = 'VisaShippingFeePK')
BEGIN
    ALTER TABLE tVisa_ShippingFee DROP COLUMN VisaShippingFeePK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_ShippingFee' AND COLUMN_NAME = 'ChannelPartnerShippingTypePK')
BEGIN
    ALTER TABLE tVisa_ShippingFee DROP COLUMN ChannelPartnerShippingTypePK	
END
GO

IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tVisa_Trx_PK' )
BEGIN
	ALTER TABLE tVisa_Trx DROP CONSTRAINT DF_tVisa_Trx_PK 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_Trx' AND COLUMN_NAME = 'VisaTrxPK')
BEGIN
    ALTER TABLE tVisa_Trx DROP COLUMN VisaTrxPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_Trx' AND COLUMN_NAME = 'AccountPK')
BEGIN
    ALTER TABLE tVisa_Trx DROP COLUMN AccountPK	
END
GO

IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tTxn_Funds_PK' )
BEGIN
	ALTER TABLE tTxn_Funds DROP CONSTRAINT DF_tTxn_Funds_PK 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_Funds' AND COLUMN_NAME = 'TxnPK')
BEGIN
    ALTER TABLE tTxn_Funds DROP COLUMN TxnPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_Funds' AND COLUMN_NAME = 'CXEId')
BEGIN
    ALTER TABLE tTxn_Funds DROP COLUMN CXEId	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_Funds' AND COLUMN_NAME = 'CustomerSessionPK')
BEGIN
    ALTER TABLE tTxn_Funds DROP COLUMN CustomerSessionPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_Funds' AND COLUMN_NAME = 'AccountPK')
BEGIN
    ALTER TABLE tTxn_Funds DROP COLUMN AccountPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_Funds' AND COLUMN_NAME = 'CXNState')
BEGIN
    ALTER TABLE tTxn_Funds DROP COLUMN CXNState	
END
GO

--================== Drop UnUsed columns from Money Transfer transaction related tables =================================


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_MoneyTransfer' AND COLUMN_NAME = 'TxnPK')
BEGIN
    ALTER TABLE tTxn_MoneyTransfer DROP COLUMN TxnPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_MoneyTransfer' AND COLUMN_NAME = 'CXEId')
BEGIN
    ALTER TABLE tTxn_MoneyTransfer DROP COLUMN CXEId	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_MoneyTransfer' AND COLUMN_NAME = 'CustomerSessionPK')
BEGIN
    ALTER TABLE tTxn_MoneyTransfer DROP COLUMN CustomerSessionPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_MoneyTransfer' AND COLUMN_NAME = 'AccountPK')
BEGIN
    ALTER TABLE tTxn_MoneyTransfer DROP COLUMN AccountPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_MoneyTransfer' AND COLUMN_NAME = 'CXNState')
BEGIN
    ALTER TABLE tTxn_MoneyTransfer DROP COLUMN CXNState	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Account' AND COLUMN_NAME = 'WUAccountPK')
BEGIN
    ALTER TABLE tWUnion_Account DROP COLUMN WUAccountPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Cities' AND COLUMN_NAME = 'WUCityPK')
BEGIN
    ALTER TABLE tWUnion_Cities DROP COLUMN WUCityPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Countries' AND COLUMN_NAME = 'WUCountryPK')
BEGIN
    ALTER TABLE tWUnion_Countries DROP COLUMN WUCountryPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_CountryCurrencies' AND COLUMN_NAME = 'WUCountryCurrencyPK')
BEGIN
    ALTER TABLE tWUnion_CountryCurrencies DROP COLUMN WUCountryCurrencyPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWunion_CountryTranslation' AND COLUMN_NAME = 'WUCountryTranslationPK')
BEGIN
    ALTER TABLE tWunion_CountryTranslation DROP COLUMN WUCountryTranslationPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Credential' AND COLUMN_NAME = 'WUCredentialPK')
BEGIN
    ALTER TABLE tWUnion_Credential DROP COLUMN WUCredentialPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWunion_DeliveryTranslations' AND COLUMN_NAME = 'WUDeliveryTranslationsPK')
BEGIN
    ALTER TABLE tWunion_DeliveryTranslations DROP COLUMN WUDeliveryTranslationsPK	
END
GO

IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tWUnion_ImportBillers_PK' )
BEGIN
	ALTER TABLE tWUnion_ImportBillers DROP CONSTRAINT DF_tWUnion_ImportBillers_PK 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_ImportBillers' AND COLUMN_NAME = 'WUBillersPK')
BEGIN
    ALTER TABLE tWUnion_ImportBillers DROP COLUMN WUBillersPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_ImportBillers' AND COLUMN_NAME = 'WUBillPayAccountPK')
BEGIN
    ALTER TABLE tWUnion_ImportBillers DROP COLUMN WUBillPayAccountPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Receiver' AND COLUMN_NAME = 'WUReceiverPK')
BEGIN
    ALTER TABLE tWUnion_Receiver DROP COLUMN WUReceiverPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_States' AND COLUMN_NAME = 'WUStatePK')
BEGIN
    ALTER TABLE tWUnion_States DROP COLUMN WUStatePK	
END
GO

IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tWUnion_Trx_WUTrxPK' )
BEGIN
	ALTER TABLE tWUnion_Trx DROP CONSTRAINT DF_tWUnion_Trx_WUTrxPK 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Trx' AND COLUMN_NAME = 'WUTrxPK')
BEGIN
    ALTER TABLE tWUnion_Trx DROP COLUMN WUTrxPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Trx' AND COLUMN_NAME = 'WUAccountPK')
BEGIN
    ALTER TABLE tWUnion_Trx DROP COLUMN WUAccountPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Trx' AND COLUMN_NAME = 'WUnionRecipientAccountPK')
BEGIN
    ALTER TABLE tWUnion_Trx DROP COLUMN WUnionRecipientAccountPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Trx_Aud' AND COLUMN_NAME = 'WUTrxPK')
BEGIN
    ALTER TABLE tWUnion_Trx_Aud DROP COLUMN WUTrxPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Trx_Aud' AND COLUMN_NAME = 'WUAccountPK')
BEGIN
    ALTER TABLE tWUnion_Trx_Aud DROP COLUMN WUAccountPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Trx_Aud' AND COLUMN_NAME = 'WUnionRecipientAccountPK')
BEGIN
    ALTER TABLE tWUnion_Trx_Aud DROP COLUMN WUnionRecipientAccountPK	
END
GO

--============================= Drop UnUsed columns in CAsh related tables ===================================

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_Cash' AND COLUMN_NAME = 'TxnPK')
BEGIN
    ALTER TABLE tTxn_Cash DROP COLUMN TxnPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_Cash' AND COLUMN_NAME = 'CXEId')
BEGIN
    ALTER TABLE tTxn_Cash DROP COLUMN CXEId	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_Cash' AND COLUMN_NAME = 'CustomerSessionPK')
BEGIN
    ALTER TABLE tTxn_Cash DROP COLUMN CustomerSessionPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_Cash' AND COLUMN_NAME = 'AccountPK')
BEGIN
    ALTER TABLE tTxn_Cash DROP COLUMN AccountPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_Cash' AND COLUMN_NAME = 'CXNState')
BEGIN
    ALTER TABLE tTxn_Cash DROP COLUMN CXNState	
END
GO

--============================= Drop UnUsed columns in CAsh related tables ===================================

IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tShoppingCarts_CartPK' )
BEGIN
	ALTER TABLE tShoppingCarts DROP CONSTRAINT DF_tShoppingCarts_CartPK 
END
GO

IF EXISTS( SELECT 1 FROM sys.indexes WHERE name = 'ix_tShoppingCartS_CustomerPK' )
BEGIN
	DROP INDEX ix_tShoppingCartS_CustomerPK ON tShoppingCarts 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCarts' AND COLUMN_NAME = 'CartPK')
BEGIN
    ALTER TABLE tShoppingCarts DROP COLUMN CartPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCarts' AND COLUMN_NAME = 'CustomerPK')
BEGIN
    ALTER TABLE tShoppingCarts DROP COLUMN CustomerPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCarts' AND COLUMN_NAME = 'Active')
BEGIN
    ALTER TABLE tShoppingCarts DROP COLUMN Active	
END
GO

IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tShoppingCarts_IsParked' )
BEGIN
	ALTER TABLE tShoppingCarts DROP CONSTRAINT DF_tShoppingCarts_IsParked 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCarts' AND COLUMN_NAME = 'IsParked')
BEGIN
    ALTER TABLE tShoppingCarts DROP COLUMN IsParked	
END
GO

IF EXISTS( SELECT 1 FROM sys.indexes WHERE name = 'Ix_tShoppingCarts_Aud_CartPk' )
BEGIN
	DROP INDEX Ix_tShoppingCarts_Aud_CartPk ON tShoppingCarts_Aud 
END
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCarts_Aud' AND COLUMN_NAME = 'CartPK')
BEGIN
    ALTER TABLE tShoppingCarts_Aud DROP COLUMN CartPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCarts_Aud' AND COLUMN_NAME = 'CustomerPK')
BEGIN
    ALTER TABLE tShoppingCarts_Aud DROP COLUMN CustomerPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCarts_Aud' AND COLUMN_NAME = 'Active')
BEGIN
    ALTER TABLE tShoppingCarts_Aud DROP COLUMN Active	
END
GO


DECLARE @constraint_Name NVARCHAR(50)

SELECT 
	 @constraint_Name = OBJECT_NAME(default_object_id) 
FROM 
	 SYS.COLUMNS
WHERE 
	 object_id = OBJECT_ID('tShoppingCarts_Aud') 
	 AND
	 name = 'IsParked'

IF @constraint_Name IS NOT NULL
BEGIN
	  EXEC('ALTER TABLE tShoppingCarts_Aud DROP '+ @constraint_Name)
END 

IF EXISTS( SELECT 1 FROM sys.indexes WHERE name = 'Ix_tShoppingCartTransactions_CartPk' )
BEGIN
	DROP INDEX Ix_tShoppingCartTransactions_CartPk ON tShoppingCartTransactions 
END
GO

IF EXISTS( SELECT 1 FROM sys.indexes WHERE name = 'Ix_tShoppingCartTransactions_Aud_txnPk' )
BEGIN
	DROP INDEX Ix_tShoppingCartTransactions_Aud_txnPk ON tShoppingCartTransactions_Aud 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCarts_Aud' AND COLUMN_NAME = 'IsParked')
BEGIN
    ALTER TABLE tShoppingCarts_Aud DROP COLUMN IsParked	
END
GO

IF EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' and Name = 'IX_TxnPK')
BEGIN
	DROP INDEX IX_TxnPK ON tShoppingCartTransactions
END


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCartTransactions' AND COLUMN_NAME = 'CartPK')
BEGIN
    ALTER TABLE tShoppingCartTransactions DROP COLUMN CartPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCartTransactions' AND COLUMN_NAME = 'TxnPK')
BEGIN
    ALTER TABLE tShoppingCartTransactions DROP COLUMN TxnPK	
END
GO

DECLARE @constraint_Name NVARCHAR(50)

SELECT 
	 @constraint_Name = OBJECT_NAME(default_object_id) 
FROM 
	 SYS.COLUMNS
WHERE 
	 object_id = OBJECT_ID('tShoppingCartTransactions') 
	 AND
	 name = 'CartTxnPK'

IF @constraint_Name IS NOT NULL
BEGIN
	  EXEC('ALTER TABLE tShoppingCartTransactions DROP '+ @constraint_Name)
END 
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCartTransactions' AND COLUMN_NAME = 'CartTxnPK')
BEGIN
    ALTER TABLE tShoppingCartTransactions DROP COLUMN CartTxnPK	
END
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCartTransactions_Aud' AND COLUMN_NAME = 'CartPK')
BEGIN
    ALTER TABLE tShoppingCartTransactions_Aud DROP COLUMN CartPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCartTransactions_Aud' AND COLUMN_NAME = 'TxnPK')
BEGIN
    ALTER TABLE tShoppingCartTransactions_Aud DROP COLUMN TxnPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tShoppingCartTransactions_Aud' AND COLUMN_NAME = 'CartTxnPK')
BEGIN
    ALTER TABLE tShoppingCartTransactions_Aud DROP COLUMN CartTxnPK	
END
GO

--============================= Drop UnUsed columns in Fee Adjustment related tables ===================================

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tFeeAdjustmentConditions' AND COLUMN_NAME = 'AdjConditionsPK')
BEGIN
    ALTER TABLE tFeeAdjustmentConditions DROP COLUMN AdjConditionsPK	
END
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tFeeAdjustmentConditions' AND COLUMN_NAME = 'FeeAdjustmentPK')
BEGIN
    ALTER TABLE tFeeAdjustmentConditions DROP COLUMN FeeAdjustmentPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tIdentificationConfirmation' AND COLUMN_NAME = 'IdConfirmPK')
BEGIN
    ALTER TABLE tIdentificationConfirmation DROP COLUMN IdConfirmPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tLegalCodes' AND COLUMN_NAME = 'LegalCodesPK')
BEGIN
    ALTER TABLE tLegalCodes DROP COLUMN LegalCodesPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tLimitFailures' AND COLUMN_NAME = 'LimitFailurePK')
BEGIN
    ALTER TABLE tLimitFailures DROP COLUMN LimitFailurePK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tLimitFailures' AND COLUMN_NAME = 'ComplianceProgramPK')
BEGIN
    ALTER TABLE tLimitFailures DROP COLUMN ComplianceProgramPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tLimits' AND COLUMN_NAME = 'LimitPK')
BEGIN
    ALTER TABLE tLimits DROP COLUMN LimitPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tLimits' AND COLUMN_NAME = 'LimitTypePK')
BEGIN
    ALTER TABLE tLimits DROP COLUMN LimitTypePK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tLimitTypes' AND COLUMN_NAME = 'LimitTypePK')
BEGIN
    ALTER TABLE tLimitTypes DROP COLUMN LimitTypePK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tLimitTypes' AND COLUMN_NAME = 'ComplianceProgramPK')
BEGIN
    ALTER TABLE tLimitTypes DROP COLUMN ComplianceProgramPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tMasterCountries' AND COLUMN_NAME = 'MasterCountriesPK')
BEGIN
    ALTER TABLE tMasterCountries DROP COLUMN MasterCountriesPK	
END
GO

IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tMessageCenter_MessageCenterPK' )
BEGIN
	ALTER TABLE tMessageCenter DROP CONSTRAINT DF_tMessageCenter_MessageCenterPK 
END
GO

IF EXISTS( SELECT 1 FROM sys.indexes WHERE name = 'IX_tMessageCenter_AgentId' )
BEGIN
	DROP INDEX IX_tMessageCenter_AgentId ON tMessageCenter 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tMessageCenter' AND COLUMN_NAME = 'MessageCenterPK')
BEGIN
    ALTER TABLE tMessageCenter DROP COLUMN MessageCenterPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tMessageCenter' AND COLUMN_NAME = 'AgentPK')
BEGIN
    ALTER TABLE tMessageCenter DROP COLUMN AgentPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tMessageCenter' AND COLUMN_NAME = 'TxnId')
BEGIN
    ALTER TABLE tMessageCenter DROP COLUMN TxnId	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tMessageStore' AND COLUMN_NAME = 'MessageStorePK')
BEGIN
    ALTER TABLE tMessageStore DROP COLUMN MessageStorePK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tOccupations' AND COLUMN_NAME = 'OccupationsPK')
BEGIN
    ALTER TABLE tOccupations DROP COLUMN OccupationsPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tPricing' AND COLUMN_NAME = 'PricingPK')
BEGIN
    ALTER TABLE tPricing DROP COLUMN PricingPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tPricing' AND COLUMN_NAME = 'PricingGroupPK')
BEGIN
    ALTER TABLE tPricing DROP COLUMN PricingGroupPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tPricingGroups' AND COLUMN_NAME = 'PricingGroupPK')
BEGIN
    ALTER TABLE tPricingGroups DROP COLUMN PricingGroupPK	
END
GO


DECLARE @constraint_Name NVARCHAR(50)

SELECT 
	 @constraint_Name = OBJECT_NAME(default_object_id) 
FROM 
	 SYS.COLUMNS
WHERE 
	 object_id = OBJECT_ID('tProcessors') 
	 AND
	 name = 'ProcessorsPK'

IF @constraint_Name IS NOT NULL
BEGIN
	  EXEC('ALTER TABLE tProcessors DROP '+ @constraint_Name)
END 
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tProcessors' AND COLUMN_NAME = 'ProcessorsPK')
BEGIN
    ALTER TABLE tProcessors DROP COLUMN ProcessorsPK	
END
GO

DECLARE @constraint_Name NVARCHAR(50)

SELECT 
	 @constraint_Name = OBJECT_NAME(default_object_id) 
FROM 
	 SYS.COLUMNS
WHERE 
	 object_id = OBJECT_ID('tProductProcessorsMapping') 
	 AND
	 name = 'ProductProcessorsMappingPK'

IF @constraint_Name IS NOT NULL
BEGIN
	  EXEC('ALTER TABLE tProductProcessorsMapping DROP '+ @constraint_Name)
END 
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tProductProcessorsMapping' AND COLUMN_NAME = 'ProductProcessorsMappingPK')
BEGIN
    ALTER TABLE tProductProcessorsMapping DROP COLUMN ProductProcessorsMappingPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tProductProcessorsMapping' AND COLUMN_NAME = 'ProductPK')
BEGIN
    ALTER TABLE tProductProcessorsMapping DROP COLUMN ProductPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tProductProcessorsMapping' AND COLUMN_NAME = 'ProcessorPK')
BEGIN
    ALTER TABLE tProductProcessorsMapping DROP COLUMN ProcessorPK	
END
GO

DECLARE @constraint_Name NVARCHAR(50)

SELECT 
	 @constraint_Name = OBJECT_NAME(default_object_id) 
FROM 
	 SYS.COLUMNS
WHERE 
	 object_id = OBJECT_ID('tProducts') 
	 AND
	 name = 'ProductsPK'

IF @constraint_Name IS NOT NULL
BEGIN
	  EXEC('ALTER TABLE tProducts DROP '+ @constraint_Name)
END 
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tProducts' AND COLUMN_NAME = 'ProductsPK')
BEGIN
    ALTER TABLE tProducts DROP COLUMN ProductsPK	
END
GO

IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tStates_id' )
BEGIN
	ALTER TABLE tStates DROP CONSTRAINT DF_tStates_id 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tStates' AND COLUMN_NAME = 'StatePK')
BEGIN
    ALTER TABLE tStates DROP COLUMN StatePK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tStates' AND COLUMN_NAME = 'CountryPK')
BEGIN
    ALTER TABLE tStates DROP COLUMN CountryPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTCIS_Account' AND COLUMN_NAME = 'TCISAccountPK')
BEGIN
    ALTER TABLE tTCIS_Account DROP COLUMN TCISAccountPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCompliancePrograms' AND COLUMN_NAME = 'ComplianceProgramPK')
BEGIN
    ALTER TABLE tCompliancePrograms DROP COLUMN ComplianceProgramPK	
END
GO

--============================tAgentDetails=========================================================

IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tAgentDetails_rowguid' )
BEGIN
	ALTER TABLE tAgentDetails DROP CONSTRAINT DF_tAgentDetails_rowguid 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tAgentDetails' AND COLUMN_NAME = 'AgentPK')
BEGIN
    ALTER TABLE tAgentDetails DROP COLUMN AgentPK	
END
GO

--============================tAgentSessions=========================================================

IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tAgentSessions_AgentSessionPK' )
BEGIN
	ALTER TABLE tAgentSessions DROP CONSTRAINT DF_tAgentSessions_AgentSessionPK 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tAgentSessions' AND COLUMN_NAME = 'AgentSessionPK')
BEGIN
    ALTER TABLE tAgentSessions DROP COLUMN AgentSessionPK
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tAgentSessions' AND COLUMN_NAME = 'TerminalPK')
BEGIN
    ALTER TABLE tAgentSessions DROP COLUMN TerminalPK
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tAgentSessions' AND COLUMN_NAME = 'AgentPK')
BEGIN
    ALTER TABLE tAgentSessions DROP COLUMN AgentPK
END
GO

--============================tPermissions=========================================================

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tPermissions' AND COLUMN_NAME = 'PermissionsPK')
BEGIN
    ALTER TABLE tPermissions DROP COLUMN PermissionsPK	
END
GO

--============================tUserRolesPermissionsMapping=========================================================

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tUserRolesPermissionsMapping' AND COLUMN_NAME = 'UserRolesPermissionsMappingPK')
BEGIN
    ALTER TABLE tUserRolesPermissionsMapping DROP COLUMN UserRolesPermissionsMappingPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tUserRolesPermissionsMapping' AND COLUMN_NAME = 'PermissionPK')
BEGIN
    ALTER TABLE tUserRolesPermissionsMapping DROP COLUMN PermissionPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tUserRolesPermissionsMapping' AND COLUMN_NAME = 'ChannelPartnerPK')
BEGIN
    ALTER TABLE tUserRolesPermissionsMapping DROP COLUMN ChannelPartnerPK	
END
GO

--============================tNpsTerminals=========================================================

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tNpsTerminals' AND COLUMN_NAME = 'NpsTerminalPK')
BEGIN
    ALTER TABLE tNpsTerminals DROP COLUMN NpsTerminalPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tNpsTerminals' AND COLUMN_NAME = 'LocationPK')
BEGIN
    ALTER TABLE tNpsTerminals DROP COLUMN LocationPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tNpsTerminals' AND COLUMN_NAME = 'ChannelPartnerPK')
BEGIN
    ALTER TABLE tNpsTerminals DROP COLUMN ChannelPartnerPK	
END
GO

--============================tTerminals=========================================================

IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tTerminals_TerminalPK' )
BEGIN
	ALTER TABLE tTerminals DROP CONSTRAINT DF_tTerminals_TerminalPK 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTerminals' AND COLUMN_NAME = 'TerminalPK')
BEGIN
    ALTER TABLE tTerminals DROP COLUMN TerminalPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTerminals' AND COLUMN_NAME = 'LocationPK')
BEGIN
    ALTER TABLE tTerminals DROP COLUMN LocationPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTerminals' AND COLUMN_NAME = 'NpsTerminalPK')
BEGIN
    ALTER TABLE tTerminals DROP COLUMN NpsTerminalPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTerminals' AND COLUMN_NAME = 'ChannelPartnerPK')
BEGIN
    ALTER TABLE tTerminals DROP COLUMN ChannelPartnerPK	
END
GO

--============================tLocationCounterIdDetails=========================================================

IF EXISTS( SELECT 1 FROM sys.indexes WHERE name = 'IX_tLocationCounterIdDetails_locID_ProviderId_IsAvail' )
BEGIN
	DROP INDEX IX_tLocationCounterIdDetails_locID_ProviderId_IsAvail ON tLocationCounterIdDetails 
END
GO

IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_LocationCounterIdDetailPK_tLocationCounterIdDetails' )
BEGIN
	ALTER TABLE tLocationCounterIdDetails DROP CONSTRAINT DF_LocationCounterIdDetailPK_tLocationCounterIdDetails 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tLocationCounterIdDetails' AND COLUMN_NAME = 'LocationCounterIdDetailPK')
BEGIN
    ALTER TABLE tLocationCounterIdDetails DROP COLUMN LocationCounterIdDetailPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tLocationCounterIdDetails' AND COLUMN_NAME = 'LocationPK')
BEGIN
    ALTER TABLE tLocationCounterIdDetails DROP COLUMN LocationPK	
END
GO

--============================tLocationProcessorCredentials=========================================================

IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_LocationProcessorCredentialsPK_tLocationProcessorCredentials' )
BEGIN
	ALTER TABLE tLocationProcessorCredentials DROP CONSTRAINT DF_LocationProcessorCredentialsPK_tLocationProcessorCredentials 
END
GO

IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tLocationPK_tLocationProcessorCredentials' )
BEGIN
	ALTER TABLE tLocationProcessorCredentials DROP CONSTRAINT DF_tLocationPK_tLocationProcessorCredentials 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tLocationProcessorCredentials' AND COLUMN_NAME = 'LocationProcessorCredentialsPK')
BEGIN
    ALTER TABLE tLocationProcessorCredentials DROP COLUMN LocationProcessorCredentialsPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tLocationProcessorCredentials' AND COLUMN_NAME = 'LocationPK')
BEGIN
    ALTER TABLE tLocationProcessorCredentials DROP COLUMN LocationPK	
END
GO

--============================tLocations=========================================================

IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tLocationPK_tLocation' )
BEGIN
	ALTER TABLE tLocations DROP CONSTRAINT DF_tLocationPK_tLocation 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tLocations' AND COLUMN_NAME = 'LocationPK')
BEGIN
    ALTER TABLE tLocations DROP COLUMN LocationPK	
END
GO

--============================tCustomerFeeAdjustments=========================================================

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomerFeeAdjustments' AND COLUMN_NAME = 'CustomerFeeAdjustmentsPK')
BEGIN
    ALTER TABLE tCustomerFeeAdjustments DROP COLUMN CustomerFeeAdjustmentsPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomerFeeAdjustments' AND COLUMN_NAME = 'FeeAdjustmentPK')
BEGIN
    ALTER TABLE tCustomerFeeAdjustments DROP COLUMN FeeAdjustmentPK	
END
GO

--============================tCustomers=========================================================

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers' AND COLUMN_NAME = 'CustomerPK')
BEGIN
    ALTER TABLE tCustomers DROP COLUMN CustomerPK	
END
GO

--============================tCustomers_Aud=========================================================

IF EXISTS(SELECT 1 FROM SYS.INDEXES WHERE SYS.INDEXES.NAME = 'IX_tCustomers_Aud_rowguid')
BEGIN
	 DROP INDEX tCustomers_Aud.IX_tCustomers_Aud_rowguid
END

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers_Aud' AND COLUMN_NAME = 'CustomerPK')
BEGIN
    ALTER TABLE tCustomers_Aud DROP COLUMN CustomerPK	
END
GO

--============================tCustomerSessionCounterIdDetails=========================================================

IF EXISTS( SELECT 1 FROM sys.indexes WHERE name = 'IX_tCustomerSessionCounterIdDetails_CustomerSessionPk' )
BEGIN
	DROP INDEX IX_tCustomerSessionCounterIdDetails_CustomerSessionPk ON tCustomerSessionCounterIdDetails 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomerSessionCounterIdDetails' AND COLUMN_NAME = 'CustomerSessionPK')
BEGIN
    ALTER TABLE tCustomerSessionCounterIdDetails DROP COLUMN CustomerSessionPK	
END
GO

--============================tCustomerSessions=========================================================

IF EXISTS( SELECT 1 FROM SYS.INDEXES WHERE name = 'IX_tCustomerSessions_customerPK' )
BEGIN
	DROP INDEX IX_tCustomerSessions_customerPK ON tCustomerSessions 
END
GO

IF EXISTS(SELECT 1 FROM SYS.INDEXES WHERE SYS.INDEXES.NAME = 'IX_tCustomerSessions_ID')
BEGIN
	 DROP INDEX tCustomerSessions.IX_tCustomerSessions_ID
END

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomerSessions' AND COLUMN_NAME = 'CustomerSessionPK')
BEGIN
    ALTER TABLE tCustomerSessions DROP COLUMN CustomerSessionPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomerSessions' AND COLUMN_NAME = 'AgentSessionPK')
BEGIN
    ALTER TABLE tCustomerSessions DROP COLUMN AgentSessionPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomerSessions' AND COLUMN_NAME = 'CustomerPK')
BEGIN
    ALTER TABLE tCustomerSessions DROP COLUMN CustomerPK	
END
GO

--============================tChannelPartnerCertificate=========================================================

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerCertificate' AND COLUMN_NAME = 'ChannelPartnerCertificatePK')
BEGIN
    ALTER TABLE tChannelPartnerCertificate DROP COLUMN ChannelPartnerCertificatePK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerCertificate' AND COLUMN_NAME = 'ChannelPartnerPK')
BEGIN
    ALTER TABLE tChannelPartnerCertificate DROP COLUMN ChannelPartnerPK	
END
GO

--============================tChannelPartnerConfig=========================================================

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerConfig' AND COLUMN_NAME = 'ChannelPartnerPK')
BEGIN
    ALTER TABLE tChannelPartnerConfig DROP COLUMN ChannelPartnerPK	
END
GO

--============================tChannelPartnerFeeAdjustments=========================================================

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerFeeAdjustments' AND COLUMN_NAME = 'FeeAdjustmentPK')
BEGIN
    ALTER TABLE tChannelPartnerFeeAdjustments DROP COLUMN FeeAdjustmentPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerFeeAdjustments' AND COLUMN_NAME = 'ChannelPartnerPK')
BEGIN
    ALTER TABLE tChannelPartnerFeeAdjustments DROP COLUMN ChannelPartnerPK	
END
GO

--============================tChannelPartnerGroups=========================================================

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerGroups' AND COLUMN_NAME = 'ChannelPartnerPK')
BEGIN
    ALTER TABLE tChannelPartnerGroups DROP COLUMN ChannelPartnerPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerGroups' AND COLUMN_NAME = 'ChannelPartnerGroupPK')
BEGIN
    ALTER TABLE tChannelPartnerGroups DROP COLUMN ChannelPartnerGroupPK	
END
GO

--============================tChannelPartnerIDTypeMapping=========================================================

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerIDTypeMapping' AND COLUMN_NAME = 'ChannelPartnerPK')
BEGIN
    ALTER TABLE tChannelPartnerIDTypeMapping DROP COLUMN ChannelPartnerPK	
END
GO

DECLARE @constraintName NVARCHAR(200)

SELECT 
	 @constraintName = OBJECT_NAME(default_object_id) 
FROM 
	 SYS.COLUMNS
WHERE 
	 object_id = OBJECT_ID('tChannelPartnerIDTypeMapping') 
	 AND
	 name = 'ChannelPartnerIDTypeMappingPK'

IF @constraintName IS NOT NULL
BEGIN
	 EXEC('ALTER TABLE tChannelPartnerIDTypeMapping DROP '+ @constraintName)
END

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerIDTypeMapping' AND COLUMN_NAME = 'ChannelPartnerIDTypeMappingPK')
BEGIN
    ALTER TABLE tChannelPartnerIDTypeMapping DROP COLUMN ChannelPartnerIDTypeMappingPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerIDTypeMapping' AND COLUMN_NAME = 'NexxoIdTypePK')
BEGIN
    ALTER TABLE tChannelPartnerIDTypeMapping DROP COLUMN NexxoIdTypePK	
END
GO

--============================tChannelPartnerMasterCountryMapping=========================================================

DECLARE @constraint_Name NVARCHAR(50)

SELECT 
	 @constraint_Name = OBJECT_NAME(default_object_id) 
FROM 
	 SYS.COLUMNS
WHERE 
	 object_id = OBJECT_ID('tChannelPartnerMasterCountryMapping') 
	 AND
	 name = 'IsActive'

IF @constraint_Name IS NOT NULL
BEGIN
	  EXEC('ALTER TABLE tChannelPartnerMasterCountryMapping DROP '+ @constraint_Name)
END

SELECT 
	 @constraint_Name = OBJECT_NAME(default_object_id) 
FROM 
	 SYS.COLUMNS
WHERE 
	 object_id = OBJECT_ID('tChannelPartnerMasterCountryMapping') 
	 AND
	 name = 'ChannelPartnerMasterCountryMappingPK'

IF @constraint_Name IS NOT NULL
BEGIN
	  EXEC('ALTER TABLE tChannelPartnerMasterCountryMapping DROP '+ @constraint_Name)
END

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerMasterCountryMapping' AND COLUMN_NAME = 'ChannelPartnerMasterCountryMappingPK')
BEGIN
    ALTER TABLE tChannelPartnerMasterCountryMapping DROP COLUMN ChannelPartnerMasterCountryMappingPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerMasterCountryMapping' AND COLUMN_NAME = 'ChannelPartnerPK')
BEGIN
    ALTER TABLE tChannelPartnerMasterCountryMapping DROP COLUMN ChannelPartnerPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerMasterCountryMapping' AND COLUMN_NAME = 'MasterCountryId')
BEGIN
    ALTER TABLE tChannelPartnerMasterCountryMapping DROP COLUMN MasterCountryId	
END
GO

--============================tChannelPartnerPricing=========================================================

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerPricing' AND COLUMN_NAME = 'ChannelPartnerPricingPK')
BEGIN
    ALTER TABLE tChannelPartnerPricing DROP COLUMN ChannelPartnerPricingPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerPricing' AND COLUMN_NAME = 'PricingGroupPK')
BEGIN
    ALTER TABLE tChannelPartnerPricing DROP COLUMN PricingGroupPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerPricing' AND COLUMN_NAME = 'ChannelPartnerPK')
BEGIN
    ALTER TABLE tChannelPartnerPricing DROP COLUMN ChannelPartnerPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerPricing' AND COLUMN_NAME = 'LocationPK')
BEGIN
    ALTER TABLE tChannelPartnerPricing DROP COLUMN LocationPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerPricing' AND COLUMN_NAME = 'ProductPK')
BEGIN
    ALTER TABLE tChannelPartnerPricing DROP COLUMN ProductPK	
END
GO

--============================tChannelPartnerPricing_Aud=========================================================

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerPricing_Aud' AND COLUMN_NAME = 'ChannelPartnerPricingPK')
BEGIN
    ALTER TABLE tChannelPartnerPricing_Aud DROP COLUMN ChannelPartnerPricingPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerPricing_Aud' AND COLUMN_NAME = 'PricingGroupPK')
BEGIN
    ALTER TABLE tChannelPartnerPricing_Aud DROP COLUMN PricingGroupPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerPricing_Aud' AND COLUMN_NAME = 'ChannelPartnerPK')
BEGIN
    ALTER TABLE tChannelPartnerPricing_Aud DROP COLUMN ChannelPartnerPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerPricing_Aud' AND COLUMN_NAME = 'LocationPK')
BEGIN
    ALTER TABLE tChannelPartnerPricing_Aud DROP COLUMN LocationPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerPricing_Aud' AND COLUMN_NAME = 'ProductPK')
BEGIN
    ALTER TABLE tChannelPartnerPricing_Aud DROP COLUMN ProductPK	
END
GO

--============================tChannelPartnerProductProcessorsMapping=========================================================

DECLARE @constraint_Name NVARCHAR(50)

SELECT 
	 @constraint_Name = OBJECT_NAME(default_object_id) 
FROM 
	 SYS.COLUMNS
WHERE 
	 object_id = OBJECT_ID('tChannelPartnerProductProcessorsMapping') 
	 AND
	 name = 'ChannelPartnerProductProcessorsMappingPK'

IF @constraint_Name IS NOT NULL
BEGIN
	  EXEC('ALTER TABLE tChannelPartnerProductProcessorsMapping DROP '+ @constraint_Name)
END

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerProductProcessorsMapping' AND COLUMN_NAME = 'ChannelPartnerProductProcessorsMappingPK')
BEGIN
    ALTER TABLE tChannelPartnerProductProcessorsMapping DROP COLUMN ChannelPartnerProductProcessorsMappingPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerProductProcessorsMapping' AND COLUMN_NAME = 'ChannelPartnerPK')
BEGIN
    ALTER TABLE tChannelPartnerProductProcessorsMapping DROP COLUMN ChannelPartnerPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerProductProcessorsMapping' AND COLUMN_NAME = 'ProductProcessorPK')
BEGIN
    ALTER TABLE tChannelPartnerProductProcessorsMapping DROP COLUMN ProductProcessorPK	
END
GO

--============================tChannelPartners=========================================================

DECLARE @constraint_Name NVARCHAR(50)

SELECT 
	 @constraint_Name = OBJECT_NAME(default_object_id) 
FROM 
	 SYS.COLUMNS
WHERE 
	 object_id = OBJECT_ID('tChannelPartners') 
	 AND
	 name = 'ChannelPartnerPK'

IF @constraint_Name IS NOT NULL
BEGIN
	  EXEC('ALTER TABLE tChannelPartners DROP '+ @constraint_Name)
END

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartners' AND COLUMN_NAME = 'ChannelPartnerPK')
BEGIN
    ALTER TABLE tChannelPartners DROP COLUMN ChannelPartnerPK	
END
GO


--============================tChannelPartner_X9_Audit_Detail=========================================================

IF EXISTS( SELECT 1 FROM sys.indexes WHERE name = 'IX-tChannelPartner_X9_Audit_Detail_Itempk' )
BEGIN
	DROP INDEX [IX-tChannelPartner_X9_Audit_Detail_Itempk] ON tChannelPartner_X9_Audit_Detail 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartner_X9_Audit_Detail' AND COLUMN_NAME = 'ItemPK')
BEGIN
    ALTER TABLE tChannelPartner_X9_Audit_Detail DROP COLUMN ItemPK	
END
GO

--============================tChannelPartner_X9_Audit_Header=========================================================

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartner_X9_Audit_Header' AND COLUMN_NAME = 'ChannelPartnerPK')
BEGIN
    ALTER TABLE tChannelPartner_X9_Audit_Header DROP COLUMN ChannelPartnerPK	
END
GO

--============================tChannelPartner_X9_Parameters=========================================================

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartner_X9_Parameters' AND COLUMN_NAME = 'ChannelPartnerPK')
BEGIN
    ALTER TABLE tChannelPartner_X9_Parameters DROP COLUMN ChannelPartnerPK	
END
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tNexxoIdTypes' AND COLUMN_NAME = 'NexxoIdTypePK')
BEGIN
    ALTER TABLE tNexxoIdTypes DROP COLUMN NexxoIdTypePK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tNexxoIdTypes' AND COLUMN_NAME = 'CountryPK')
BEGIN
    ALTER TABLE tNexxoIdTypes DROP COLUMN CountryPK	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tNexxoIdTypes' AND COLUMN_NAME = 'StatePK')
BEGIN
    ALTER TABLE tNexxoIdTypes DROP COLUMN StatePK	
END
GO


IF EXISTS( SELECT 1 FROM sys.default_constraints WHERE name = 'DF_tWUnion_Catalog_catalogPK' )
BEGIN
	ALTER TABLE tWUnion_Catalog DROP CONSTRAINT DF_tWUnion_Catalog_catalogPK 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Catalog' AND COLUMN_NAME = 'WUCatalogPK')
BEGIN
    ALTER TABLE tWUnion_Catalog DROP COLUMN WUCatalogPK	
END
GO




IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account' AND COLUMN_NAME = 'FirstName')
BEGIN
    ALTER TABLE tChxr_Account DROP COLUMN FirstName	
END
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account' AND COLUMN_NAME = 'ITIN')
BEGIN
    ALTER TABLE tChxr_Account DROP COLUMN ITIN	
END
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account' AND COLUMN_NAME = 'LastName')
BEGIN
    ALTER TABLE tChxr_Account DROP COLUMN LastName	
END
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account' AND COLUMN_NAME = 'SSN')
BEGIN
    ALTER TABLE tChxr_Account DROP COLUMN SSN	
END
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account' AND COLUMN_NAME = 'DateOfBirth')
BEGIN
    ALTER TABLE tChxr_Account DROP COLUMN DateOfBirth	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account' AND COLUMN_NAME = 'Address1')
BEGIN
    ALTER TABLE tChxr_Account DROP COLUMN Address1	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account' AND COLUMN_NAME = 'Address2')
BEGIN
    ALTER TABLE tChxr_Account DROP COLUMN Address2	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account' AND COLUMN_NAME = 'City')
BEGIN
    ALTER TABLE tChxr_Account DROP COLUMN City	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account' AND COLUMN_NAME = 'State')
BEGIN
    ALTER TABLE tChxr_Account DROP COLUMN State	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account' AND COLUMN_NAME = 'Zip')
BEGIN
    ALTER TABLE tChxr_Account DROP COLUMN Zip	
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account' AND COLUMN_NAME = 'Phone')
BEGIN
    ALTER TABLE tChxr_Account DROP COLUMN Phone	
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account' AND COLUMN_NAME = 'Occupation')
BEGIN
    ALTER TABLE tChxr_Account DROP COLUMN Occupation	
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account' AND COLUMN_NAME = 'Employer')
BEGIN
    ALTER TABLE tChxr_Account DROP COLUMN Employer	
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account' AND COLUMN_NAME = 'EmployerPhone')
BEGIN
    ALTER TABLE tChxr_Account DROP COLUMN EmployerPhone	
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account' AND COLUMN_NAME = 'IDCardType')
BEGIN
    ALTER TABLE tChxr_Account DROP COLUMN IDCardType	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account' AND COLUMN_NAME = 'IDCardNumber')
BEGIN
    ALTER TABLE tChxr_Account DROP COLUMN IDCardNumber	
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account' AND COLUMN_NAME = 'IDCardIssuedCountry')
BEGIN
    ALTER TABLE tChxr_Account DROP COLUMN IDCardIssuedCountry	
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account' AND COLUMN_NAME = 'IDCardIssuedDate')
BEGIN
    ALTER TABLE tChxr_Account DROP COLUMN IDCardIssuedDate	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account' AND COLUMN_NAME = 'IDCardImage')
BEGIN
    ALTER TABLE tChxr_Account DROP COLUMN IDCardImage	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account' AND COLUMN_NAME = 'IDCardExpireDate')
BEGIN
    ALTER TABLE tChxr_Account DROP COLUMN IDCardExpireDate	
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account' AND COLUMN_NAME = 'IDCardExpireDate')
BEGIN
    ALTER TABLE tChxr_Account DROP COLUMN IDCardExpireDate	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account' AND COLUMN_NAME = 'CardNumber')
BEGIN
    ALTER TABLE tChxr_Account DROP COLUMN CardNumber	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account' AND COLUMN_NAME = 'CustomerScore')
BEGIN
    ALTER TABLE tChxr_Account DROP COLUMN CustomerScore	
END
GO



IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account_Aud' AND COLUMN_NAME = 'FirstName')
BEGIN
    ALTER TABLE tChxr_Account_Aud DROP COLUMN FirstName	
END
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account_Aud' AND COLUMN_NAME = 'ITIN')
BEGIN
    ALTER TABLE tChxr_Account_Aud DROP COLUMN ITIN	
END
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account_Aud' AND COLUMN_NAME = 'LastName')
BEGIN
    ALTER TABLE tChxr_Account_Aud DROP COLUMN LastName	
END
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account_Aud' AND COLUMN_NAME = 'SSN')
BEGIN
    ALTER TABLE tChxr_Account_Aud DROP COLUMN SSN	
END
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account_Aud' AND COLUMN_NAME = 'DateOfBirth')
BEGIN
    ALTER TABLE tChxr_Account_Aud DROP COLUMN DateOfBirth	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account_Aud' AND COLUMN_NAME = 'Address1')
BEGIN
    ALTER TABLE tChxr_Account_Aud DROP COLUMN Address1	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account_Aud' AND COLUMN_NAME = 'Address2')
BEGIN
    ALTER TABLE tChxr_Account_Aud DROP COLUMN Address2	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account_Aud' AND COLUMN_NAME = 'City')
BEGIN
    ALTER TABLE tChxr_Account_Aud DROP COLUMN City	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account_Aud' AND COLUMN_NAME = 'State')
BEGIN
    ALTER TABLE tChxr_Account_Aud DROP COLUMN State	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account_Aud' AND COLUMN_NAME = 'Zip')
BEGIN
    ALTER TABLE tChxr_Account_Aud DROP COLUMN Zip	
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account_Aud' AND COLUMN_NAME = 'Phone')
BEGIN
    ALTER TABLE tChxr_Account_Aud DROP COLUMN Phone	
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account_Aud' AND COLUMN_NAME = 'Occupation')
BEGIN
    ALTER TABLE tChxr_Account_Aud DROP COLUMN Occupation	
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account_Aud' AND COLUMN_NAME = 'Employer')
BEGIN
    ALTER TABLE tChxr_Account_Aud DROP COLUMN Employer	
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account_Aud' AND COLUMN_NAME = 'EmployerPhone')
BEGIN
    ALTER TABLE tChxr_Account_Aud DROP COLUMN EmployerPhone	
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account_Aud' AND COLUMN_NAME = 'IDCardType')
BEGIN
    ALTER TABLE tChxr_Account_Aud DROP COLUMN IDCardType	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account_Aud' AND COLUMN_NAME = 'IDCardNumber')
BEGIN
    ALTER TABLE tChxr_Account_Aud DROP COLUMN IDCardNumber	
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account_Aud' AND COLUMN_NAME = 'IDCardIssuedCountry')
BEGIN
    ALTER TABLE tChxr_Account_Aud DROP COLUMN IDCardIssuedCountry	
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account_Aud' AND COLUMN_NAME = 'IDCardIssuedDate')
BEGIN
    ALTER TABLE tChxr_Account_Aud DROP COLUMN IDCardIssuedDate	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account_Aud' AND COLUMN_NAME = 'IDCardImage')
BEGIN
    ALTER TABLE tChxr_Account_Aud DROP COLUMN IDCardImage	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account_Aud' AND COLUMN_NAME = 'IDCardExpireDate')
BEGIN
    ALTER TABLE tChxr_Account_Aud DROP COLUMN IDCardExpireDate	
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account_Aud' AND COLUMN_NAME = 'IDCardExpireDate')
BEGIN
    ALTER TABLE tChxr_Account_Aud DROP COLUMN IDCardExpireDate	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account_Aud' AND COLUMN_NAME = 'CardNumber')
BEGIN
    ALTER TABLE tChxr_Account_Aud DROP COLUMN CardNumber	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account_Aud' AND COLUMN_NAME = 'CustomerScore')
BEGIN
    ALTER TABLE tChxr_Account_Aud DROP COLUMN CustomerScore	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account_Aud' AND COLUMN_NAME = 'IDCode')
BEGIN
    ALTER TABLE tChxr_Account_Aud DROP COLUMN IDCode	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account' AND COLUMN_NAME = 'IDCode')
BEGIN
    ALTER TABLE tChxr_Account DROP COLUMN IDCode	
END
GO



IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxrSim_Account' AND COLUMN_NAME = 'FName')
BEGIN
    ALTER TABLE tChxrSim_Account DROP COLUMN FName	
END
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxrSim_Account' AND COLUMN_NAME = 'ITIN')
BEGIN
    ALTER TABLE tChxrSim_Account DROP COLUMN ITIN	
END
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxrSim_Account' AND COLUMN_NAME = 'LName')
BEGIN
    ALTER TABLE tChxrSim_Account DROP COLUMN LName	
END
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxrSim_Account' AND COLUMN_NAME = 'SSN')
BEGIN
    ALTER TABLE tChxrSim_Account DROP COLUMN SSN	
END
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxrSim_Account' AND COLUMN_NAME = 'DOB')
BEGIN
    ALTER TABLE tChxrSim_Account DROP COLUMN DOB	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxrSim_Account' AND COLUMN_NAME = 'Address1')
BEGIN
    ALTER TABLE tChxrSim_Account DROP COLUMN Address1	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxrSim_Account' AND COLUMN_NAME = 'Address2')
BEGIN
    ALTER TABLE tChxrSim_Account DROP COLUMN Address2	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxrSim_Account' AND COLUMN_NAME = 'City')
BEGIN
    ALTER TABLE tChxrSim_Account DROP COLUMN City	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxrSim_Account' AND COLUMN_NAME = 'State')
BEGIN
    ALTER TABLE tChxrSim_Account DROP COLUMN State	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxrSim_Account' AND COLUMN_NAME = 'Zip')
BEGIN
    ALTER TABLE tChxrSim_Account DROP COLUMN Zip	
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxrSim_Account' AND COLUMN_NAME = 'Phone')
BEGIN
    ALTER TABLE tChxrSim_Account DROP COLUMN Phone	
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxrSim_Account' AND COLUMN_NAME = 'Occupation')
BEGIN
    ALTER TABLE tChxrSim_Account DROP COLUMN Occupation	
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxrSim_Account' AND COLUMN_NAME = 'Employer')
BEGIN
    ALTER TABLE tChxrSim_Account DROP COLUMN Employer	
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxrSim_Account' AND COLUMN_NAME = 'EmployerPhone')
BEGIN
    ALTER TABLE tChxrSim_Account DROP COLUMN EmployerPhone	
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxrSim_Account' AND COLUMN_NAME = 'GovernmentId')
BEGIN
    ALTER TABLE tChxrSim_Account DROP COLUMN GovernmentId	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxrSim_Account' AND COLUMN_NAME = 'IDType')
BEGIN
    ALTER TABLE tChxrSim_Account DROP COLUMN IDType	
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxrSim_Account' AND COLUMN_NAME = 'IDCountry')
BEGIN
    ALTER TABLE tChxrSim_Account DROP COLUMN IDCountry	
END
GO
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxrSim_Account' AND COLUMN_NAME = 'IDExpDate')
BEGIN
    ALTER TABLE tChxrSim_Account DROP COLUMN IDExpDate	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxrSim_Account' AND COLUMN_NAME = 'IDImage')
BEGIN
    ALTER TABLE tChxrSim_Account DROP COLUMN IDImage	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxrSim_Account' AND COLUMN_NAME = 'CardNumber')
BEGIN
    ALTER TABLE tChxrSim_Account DROP COLUMN CardNumber	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxrSim_Account' AND COLUMN_NAME = 'CustomerScore')
BEGIN
    ALTER TABLE tChxrSim_Account DROP COLUMN CustomerScore	
END
GO

--============================tWUnion_BillPay_Account=========================================================

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_BillPay_Account' AND COLUMN_NAME = 'FirstName')
BEGIN
    ALTER TABLE tWUnion_BillPay_Account DROP COLUMN FirstName	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_BillPay_Account' AND COLUMN_NAME = 'LastName')
BEGIN
    ALTER TABLE tWUnion_BillPay_Account DROP COLUMN LastName	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_BillPay_Account' AND COLUMN_NAME = 'DateOfBirth')
BEGIN
    ALTER TABLE tWUnion_BillPay_Account DROP COLUMN DateOfBirth	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_BillPay_Account' AND COLUMN_NAME = 'Address1')
BEGIN
    ALTER TABLE tWUnion_BillPay_Account DROP COLUMN Address1	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_BillPay_Account' AND COLUMN_NAME = 'Address2')
BEGIN
    ALTER TABLE tWUnion_BillPay_Account DROP COLUMN Address2	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_BillPay_Account' AND COLUMN_NAME = 'City')
BEGIN
    ALTER TABLE tWUnion_BillPay_Account DROP COLUMN City	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_BillPay_Account' AND COLUMN_NAME = 'Street')
BEGIN
    ALTER TABLE tWUnion_BillPay_Account DROP COLUMN Street	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_BillPay_Account' AND COLUMN_NAME = 'State')
BEGIN
    ALTER TABLE tWUnion_BillPay_Account DROP COLUMN State	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_BillPay_Account' AND COLUMN_NAME = 'PostalCode')
BEGIN
    ALTER TABLE tWUnion_BillPay_Account DROP COLUMN PostalCode	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_BillPay_Account' AND COLUMN_NAME = 'Email')
BEGIN
    ALTER TABLE tWUnion_BillPay_Account DROP COLUMN Email	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_BillPay_Account' AND COLUMN_NAME = 'ContactPhone')
BEGIN
    ALTER TABLE tWUnion_BillPay_Account DROP COLUMN ContactPhone	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_BillPay_Account' AND COLUMN_NAME = 'MobilePhone')
BEGIN
    ALTER TABLE tWUnion_BillPay_Account DROP COLUMN MobilePhone	
END
GO


--============================tVisa_Account=========================================================

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_Account' AND COLUMN_NAME = 'FirstName')
BEGIN
    ALTER TABLE tVisa_Account DROP COLUMN FirstName	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_Account' AND COLUMN_NAME = 'MiddleName')
BEGIN
    ALTER TABLE tVisa_Account DROP COLUMN MiddleName	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_Account' AND COLUMN_NAME = 'LastName')
BEGIN
    ALTER TABLE tVisa_Account DROP COLUMN LastName	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_Account' AND COLUMN_NAME = 'DateOfBirth')
BEGIN
    ALTER TABLE tVisa_Account DROP COLUMN DateOfBirth	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_Account' AND COLUMN_NAME = 'SSN')
BEGIN
    ALTER TABLE tVisa_Account DROP COLUMN SSN	
END
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_Account' AND COLUMN_NAME = 'Phone')
BEGIN
    ALTER TABLE tVisa_Account DROP COLUMN Phone	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_Account' AND COLUMN_NAME = 'Address1')
BEGIN
    ALTER TABLE tVisa_Account DROP COLUMN Address1	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_Account' AND COLUMN_NAME = 'Address2')
BEGIN
    ALTER TABLE tVisa_Account DROP COLUMN Address2	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_Account' AND COLUMN_NAME = 'City')
BEGIN
    ALTER TABLE tVisa_Account DROP COLUMN City	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_Account' AND COLUMN_NAME = 'State')
BEGIN
    ALTER TABLE tVisa_Account DROP COLUMN State	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_Account' AND COLUMN_NAME = 'Country')
BEGIN
    ALTER TABLE tVisa_Account DROP COLUMN Country	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_Account' AND COLUMN_NAME = 'MothersMaidenName')
BEGIN
    ALTER TABLE tVisa_Account DROP COLUMN MothersMaidenName	
END
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_Account' AND COLUMN_NAME = 'Email')
BEGIN
    ALTER TABLE tVisa_Account DROP COLUMN Email	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_Account' AND COLUMN_NAME = 'IDCode')
BEGIN
    ALTER TABLE tVisa_Account DROP COLUMN IDCode	
END
GO

--============================tWUnion_Account=========================================================


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Account' AND COLUMN_NAME = 'FirstName')
BEGIN
    ALTER TABLE tWUnion_Account DROP COLUMN FirstName	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Account' AND COLUMN_NAME = 'LastName')
BEGIN
    ALTER TABLE tWUnion_Account DROP COLUMN LastName	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Account' AND COLUMN_NAME = 'Address')
BEGIN
    ALTER TABLE tWUnion_Account DROP COLUMN Address	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Account' AND COLUMN_NAME = 'City')
BEGIN
    ALTER TABLE tWUnion_Account DROP COLUMN City	
END
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Account' AND COLUMN_NAME = 'State')
BEGIN
    ALTER TABLE tWUnion_Account DROP COLUMN State	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Account' AND COLUMN_NAME = 'PostalCode')
BEGIN
    ALTER TABLE tWUnion_Account DROP COLUMN PostalCode	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Account' AND COLUMN_NAME = 'Email')
BEGIN
    ALTER TABLE tWUnion_Account DROP COLUMN Email	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Account' AND COLUMN_NAME = 'ContactPhone')
BEGIN
    ALTER TABLE tWUnion_Account DROP COLUMN ContactPhone	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Account' AND COLUMN_NAME = 'MobilePhone')
BEGIN
    ALTER TABLE tWUnion_Account DROP COLUMN MobilePhone	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Account' AND COLUMN_NAME = 'MiddleName')
BEGIN
    ALTER TABLE tWUnion_Account DROP COLUMN MiddleName	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Account' AND COLUMN_NAME = 'SecondLastName')
BEGIN
    ALTER TABLE tWUnion_Account DROP COLUMN SecondLastName	
END
GO


--================================== tTCIS_Account =================================================

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTCIS_Account' AND COLUMN_NAME = 'FirstName')
BEGIN
    ALTER TABLE tTCIS_Account DROP COLUMN FirstName	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTCIS_Account' AND COLUMN_NAME = 'MiddleName')
BEGIN
    ALTER TABLE tTCIS_Account DROP COLUMN MiddleName	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTCIS_Account' AND COLUMN_NAME = 'LastName')
BEGIN
    ALTER TABLE tTCIS_Account DROP COLUMN LastName	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTCIS_Account' AND COLUMN_NAME = 'LastName2')
BEGIN
    ALTER TABLE tTCIS_Account DROP COLUMN LastName2	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTCIS_Account' AND COLUMN_NAME = 'MothersMaidenName')
BEGIN
    ALTER TABLE tTCIS_Account DROP COLUMN MothersMaidenName	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTCIS_Account' AND COLUMN_NAME = 'DOB')
BEGIN
    ALTER TABLE tTCIS_Account DROP COLUMN DOB	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTCIS_Account' AND COLUMN_NAME = 'Address1')
BEGIN
    ALTER TABLE tTCIS_Account DROP COLUMN Address1	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTCIS_Account' AND COLUMN_NAME = 'Address2')
BEGIN
    ALTER TABLE tTCIS_Account DROP COLUMN Address2	
END
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTCIS_Account' AND COLUMN_NAME = 'City')
BEGIN
    ALTER TABLE tTCIS_Account DROP COLUMN City	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTCIS_Account' AND COLUMN_NAME = 'State')
BEGIN
    ALTER TABLE tTCIS_Account DROP COLUMN State	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTCIS_Account' AND COLUMN_NAME = 'ZipCode')
BEGIN
    ALTER TABLE tTCIS_Account DROP COLUMN ZipCode	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTCIS_Account' AND COLUMN_NAME = 'Phone1')
BEGIN
    ALTER TABLE tTCIS_Account DROP COLUMN Phone1	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTCIS_Account' AND COLUMN_NAME = 'Phone2')
BEGIN
    ALTER TABLE tTCIS_Account DROP COLUMN Phone2	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTCIS_Account' AND COLUMN_NAME = 'SSN')
BEGIN
    ALTER TABLE tTCIS_Account DROP COLUMN SSN	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTCIS_Account' AND COLUMN_NAME = 'Gender')
BEGIN
    ALTER TABLE tTCIS_Account DROP COLUMN Gender	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTCIS_Account' AND COLUMN_NAME = 'Gender')
BEGIN
    ALTER TABLE tTCIS_Account DROP COLUMN Gender	
END
GO

IF EXISTS( SELECT 1 FROM sys.indexes WHERE name = 'IX_tTxn_Cash_CXNId' )
BEGIN
	DROP INDEX IX_tTxn_Cash_CXNId ON tTxn_Cash 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_Cash' AND COLUMN_NAME = 'CXNId')
BEGIN
    ALTER TABLE tTxn_Cash DROP COLUMN CXNId	
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_ErrorMessages' AND COLUMN_NAME = 'WUErrorMessagePK')
BEGIN
    ALTER TABLE tWUnion_ErrorMessages DROP COLUMN WUErrorMessagePK	
END
GO


