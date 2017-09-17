--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <05-10-2017>
-- Description:	Drop unused tables from the DB as part New DB design
-- Jira ID:		<>
-- ================================================================================

IF OBJECT_ID(N'tAccounts', N'U') IS NOT NULL
BEGIN
    DROP TABLE tAccounts 
END
GO
IF OBJECT_ID(N'tCCISConnectsDb', N'U') IS NOT NULL
BEGIN
    DROP TABLE tCCISConnectsDb 
END
GO
IF OBJECT_ID(N'tCCIS_Account', N'U') IS NOT NULL
BEGIN
    DROP TABLE tCCIS_Account 
END
GO
IF OBJECT_ID(N'tCertegy_CheckImages', N'U') IS NOT NULL
BEGIN
    DROP TABLE tCertegy_CheckImages 
END
GO
IF OBJECT_ID(N'tCertegy_CheckTypeMapping', N'U') IS NOT NULL
BEGIN
    DROP TABLE tCertegy_CheckTypeMapping 
END
GO
IF OBJECT_ID(N'tCertegy_Credential', N'U') IS NOT NULL
BEGIN
    DROP TABLE tCertegy_Credential 
END
GO
IF OBJECT_ID(N'tCertegy_Trx', N'U') IS NOT NULL
BEGIN
    DROP TABLE tCertegy_Trx 
END
GO
IF OBJECT_ID(N'tCertegy_Trx_AUD', N'U') IS NOT NULL
BEGIN
    DROP TABLE tCertegy_Trx_AUD 
END
GO
IF OBJECT_ID(N'tCertegy_Account', N'U') IS NOT NULL
BEGIN
    DROP TABLE tCertegy_Account 
END
GO
IF OBJECT_ID(N'tCustomerEmploymentDetails', N'U') IS NOT NULL
BEGIN
    DROP TABLE tCustomerEmploymentDetails 
END
GO
IF OBJECT_ID(N'tCustomerEmploymentDetails_Aud', N'U') IS NOT NULL
BEGIN
    DROP TABLE tCustomerEmploymentDetails_Aud 
END
GO
IF OBJECT_ID(N'tCustomerGovernmentIdDetails', N'U') IS NOT NULL
BEGIN
    DROP TABLE tCustomerGovernmentIdDetails 
END
GO
IF OBJECT_ID(N'tCustomerGovernmentIdDetails_Aud', N'U') IS NOT NULL
BEGIN
    DROP TABLE tCustomerGovernmentIdDetails_Aud 
END
GO
IF OBJECT_ID(N'tCustomerSessionShoppingCarts', N'U') IS NOT NULL
BEGIN
    DROP TABLE tCustomerSessionShoppingCarts 
END
GO
IF OBJECT_ID(N'tErrorLog', N'U') IS NOT NULL
BEGIN
    DROP TABLE tErrorLog 
END
GO
IF OBJECT_ID(N'tFIS_Account', N'U') IS NOT NULL
BEGIN
    DROP TABLE tFIS_Account 
END
GO
IF OBJECT_ID(N'tFIS_Credential', N'U') IS NOT NULL
BEGIN
    DROP TABLE tFIS_Credential 
END
GO
IF OBJECT_ID(N'tFIS_Error', N'U') IS NOT NULL
BEGIN
    DROP TABLE tFIS_Error 
END
GO
IF OBJECT_ID(N'tFISConnectsDb', N'U') IS NOT NULL
BEGIN
    DROP TABLE tFISConnectsDb 
END
GO

IF OBJECT_ID(N'tFView_Card_Aud', N'U') IS NOT NULL
BEGIN
    DROP TABLE tFView_Card_Aud 
END
GO
IF OBJECT_ID(N'tFView_Credential', N'U') IS NOT NULL
BEGIN
    DROP TABLE tFView_Credential 
END
GO
IF OBJECT_ID(N'tFView_IdTypes', N'U') IS NOT NULL
BEGIN
    DROP TABLE tFView_IdTypes 
END
GO
IF OBJECT_ID(N'tFView_Trx', N'U') IS NOT NULL
BEGIN
    DROP TABLE tFView_Trx 
END
GO
IF OBJECT_ID(N'tFView_Trx_Aud', N'U') IS NOT NULL
BEGIN
    DROP TABLE tFView_Trx_Aud 
END
GO
IF OBJECT_ID(N'tFView_Card', N'U') IS NOT NULL
BEGIN
    DROP TABLE tFView_Card 
END
GO
IF OBJECT_ID(N'tMGram_Account', N'U') IS NOT NULL
BEGIN
    DROP TABLE tMGram_Account 
END
GO
IF OBJECT_ID(N'tMGram_BillerDenomination', N'U') IS NOT NULL
BEGIN
    DROP TABLE tMGram_BillerDenomination 
END
GO
IF OBJECT_ID(N'tMGram_BillerLimit', N'U') IS NOT NULL
BEGIN
    DROP TABLE tMGram_BillerLimit 
END
GO
IF OBJECT_ID(N'tMGram_BillPay_Trx', N'U') IS NOT NULL
BEGIN
    DROP TABLE tMGram_BillPay_Trx 
END
GO
IF OBJECT_ID(N'tMGram_BillPay_Trx_Aud', N'U') IS NOT NULL
BEGIN
    DROP TABLE tMGram_BillPay_Trx_Aud 
END
GO
IF OBJECT_ID(N'tMGram_BillPay_Account', N'U') IS NOT NULL
BEGIN
    DROP TABLE tMGram_BillPay_Account 
END
GO
IF OBJECT_ID(N'tMGram_Catalog', N'U') IS NOT NULL
BEGIN
    DROP TABLE tMGram_Catalog 
END
GO

IF OBJECT_ID(N'tMGram_Countries', N'U') IS NOT NULL
BEGIN
    DROP TABLE tMGram_Countries 
END
GO
IF OBJECT_ID(N'tMGram_CountryCurrencies', N'U') IS NOT NULL
BEGIN
    DROP TABLE tMGram_CountryCurrencies 
END
GO
IF OBJECT_ID(N'tMGram_Currencies', N'U') IS NOT NULL
BEGIN
    DROP TABLE tMGram_Currencies 
END
GO
IF OBJECT_ID(N'tMGram_DeliveryOptions', N'U') IS NOT NULL
BEGIN
    DROP TABLE tMGram_DeliveryOptions 
END
GO
IF OBJECT_ID(N'tMGram_Receiver', N'U') IS NOT NULL
BEGIN
    DROP TABLE tMGram_Receiver 
END
GO
IF OBJECT_ID(N'tMGram_StateRegulators', N'U') IS NOT NULL
BEGIN
    DROP TABLE tMGram_StateRegulators 
END
GO
IF OBJECT_ID(N'tMGram_States', N'U') IS NOT NULL
BEGIN
    DROP TABLE tMGram_States 
END
GO
IF OBJECT_ID(N'tMGram_Transfer_Trx', N'U') IS NOT NULL
BEGIN
    DROP TABLE tMGram_Transfer_Trx 
END
GO
IF OBJECT_ID(N'tMGram_Transfer_Trx_Aud', N'U') IS NOT NULL
BEGIN
    DROP TABLE tMGram_Transfer_Trx_Aud 
END
GO
IF OBJECT_ID(N'tNYCHA_BillPay_Trx', N'U') IS NOT NULL
BEGIN
    DROP TABLE tNYCHA_BillPay_Trx 
END
GO
IF OBJECT_ID(N'tNYCHA_BillPay_Trx_Aud', N'U') IS NOT NULL
BEGIN
    DROP TABLE tNYCHA_BillPay_Trx_Aud 
END
GO
IF OBJECT_ID(N'tNYCHAPayments', N'U') IS NOT NULL
BEGIN
    DROP TABLE tNYCHAPayments 
END
GO
IF OBJECT_ID(N'tNYCHA_BillPay_Account', N'U') IS NOT NULL
BEGIN
    DROP TABLE tNYCHA_BillPay_Account 
END
GO
IF OBJECT_ID(N'tNYCHATenant', N'U') IS NOT NULL
BEGIN
    DROP TABLE tNYCHATenant 
END
GO
IF OBJECT_ID(N'tNYCHAFiles', N'U') IS NOT NULL
BEGIN
    DROP TABLE tNYCHAFiles 
END
GO
IF OBJECT_ID(N'tPartnerCustomerGroupSettings', N'U') IS NOT NULL
BEGIN
    DROP TABLE tPartnerCustomerGroupSettings 
END
GO
IF OBJECT_ID(N'tPartnerCustomerGroupSettings_Aud', N'U') IS NOT NULL
BEGIN
    DROP TABLE tPartnerCustomerGroupSettings_Aud 
END
GO
IF OBJECT_ID(N'tPartnerCustomers', N'U') IS NOT NULL
BEGIN
    DROP TABLE tPartnerCustomers 
END
GO
IF OBJECT_ID(N'tPartnerCustomers_Aud', N'U') IS NOT NULL
BEGIN
    DROP TABLE tPartnerCustomers_Aud 
END
GO
IF OBJECT_ID(N'tProspectEmploymentDetails', N'U') IS NOT NULL
BEGIN
    DROP TABLE tProspectEmploymentDetails 
END
GO
IF OBJECT_ID(N'tProspectGovernmentIdDetails', N'U') IS NOT NULL
BEGIN
    DROP TABLE tProspectGovernmentIdDetails 
END
GO
IF OBJECT_ID(N'tProspectGroupSettings', N'U') IS NOT NULL
BEGIN
    DROP TABLE tProspectGroupSettings 
END
GO
IF OBJECT_ID(N'tProspects', N'U') IS NOT NULL
BEGIN
    DROP TABLE tProspects 
END
GO
IF OBJECT_ID(N'tTSys_Account_Aud', N'U') IS NOT NULL
BEGIN
    DROP TABLE tTSys_Account_Aud 
END
GO
IF OBJECT_ID(N'tTSys_Trx', N'U') IS NOT NULL
BEGIN
    DROP TABLE tTSys_Trx 
END
GO
IF OBJECT_ID(N'tTSys_Trx_Aud', N'U') IS NOT NULL
BEGIN
    DROP TABLE tTSys_Trx_Aud 
END
GO
IF OBJECT_ID(N'tTSys_Account', N'U') IS NOT NULL
BEGIN
    DROP TABLE tTSys_Account 
END
GO
IF OBJECT_ID(N'tTxn_BillPay_Commit', N'U') IS NOT NULL
BEGIN
    DROP TABLE tTxn_BillPay_Commit 
END
GO
IF OBJECT_ID(N'tTxn_BillPay_Stage', N'U') IS NOT NULL
BEGIN
    DROP TABLE tTxn_BillPay_Stage 
END
GO
IF OBJECT_ID(N'tTxn_BillPay_Stage_Aud', N'U') IS NOT NULL
BEGIN
    DROP TABLE tTxn_BillPay_Stage_Aud 
END
GO
IF OBJECT_ID(N'tTxn_Cash_Commit', N'U') IS NOT NULL
BEGIN
    DROP TABLE tTxn_Cash_Commit 
END
GO
IF OBJECT_ID(N'tTxn_Cash_Stage', N'U') IS NOT NULL
BEGIN
    DROP TABLE tTxn_Cash_Stage 
END
GO
IF OBJECT_ID(N'tTxn_Cash_Stage_Aud', N'U') IS NOT NULL
BEGIN
    DROP TABLE tTxn_Cash_Stage_Aud 
END
GO
IF OBJECT_ID(N'tTxn_Check_Commit', N'U') IS NOT NULL
BEGIN
    DROP TABLE tTxn_Check_Commit 
END
GO
IF OBJECT_ID(N'tTxn_Check_Stage', N'U') IS NOT NULL
BEGIN
    DROP TABLE tTxn_Check_Stage 
END
GO
IF OBJECT_ID(N'tTxn_Check_Stage_Aud', N'U') IS NOT NULL
BEGIN
    DROP TABLE tTxn_Check_Stage_Aud 
END
GO
IF OBJECT_ID(N'tTxn_Funds_Commit', N'U') IS NOT NULL
BEGIN
    DROP TABLE tTxn_Funds_Commit 
END
GO
IF OBJECT_ID(N'tTxn_Funds_Stage', N'U') IS NOT NULL
BEGIN
    DROP TABLE tTxn_Funds_Stage 
END
GO
IF OBJECT_ID(N'tTxn_Funds_Stage_Aud', N'U') IS NOT NULL
BEGIN
    DROP TABLE tTxn_Funds_Stage_Aud 
END
GO
IF OBJECT_ID(N'tTxn_MoneyOrder_Commit', N'U') IS NOT NULL
BEGIN
    DROP TABLE tTxn_MoneyOrder_Commit 
END
GO
IF OBJECT_ID(N'tTxn_MoneyOrder_Stage', N'U') IS NOT NULL
BEGIN
    DROP TABLE tTxn_MoneyOrder_Stage 
END
GO
IF OBJECT_ID(N'tTxn_MoneyOrder_Stage_Aud', N'U') IS NOT NULL
BEGIN
    DROP TABLE tTxn_MoneyOrder_Stage_Aud 
END
GO
IF OBJECT_ID(N'tTxn_MoneyTransfer_Commit', N'U') IS NOT NULL
BEGIN
    DROP TABLE tTxn_MoneyTransfer_Commit 
END
GO
IF OBJECT_ID(N'tTxn_MoneyTransfer_Commit_Aud', N'U') IS NOT NULL
BEGIN
    DROP TABLE tTxn_MoneyTransfer_Commit_Aud 
END
GO
IF OBJECT_ID(N'tTxn_MoneyTransfer_Stage', N'U') IS NOT NULL
BEGIN
    DROP TABLE tTxn_MoneyTransfer_Stage 
END
GO
IF OBJECT_ID(N'tTxn_MoneyTransfer_Stage_Aud', N'U') IS NOT NULL
BEGIN
    DROP TABLE tTxn_MoneyTransfer_Stage_Aud 
END
GO
IF OBJECT_ID(N'tCustomerAccounts', N'U') IS NOT NULL
BEGIN
    DROP TABLE tCustomerAccounts 
END
GO
IF OBJECT_ID(N'tWesternUnionCities', N'U') IS NOT NULL
BEGIN
    DROP TABLE tWesternUnionCities 
END
GO
IF OBJECT_ID(N'tWesternUnionCountries', N'U') IS NOT NULL
BEGIN
    DROP TABLE tWesternUnionCountries 
END
GO
IF OBJECT_ID(N'tWesternUnionCountryCurrencies', N'U') IS NOT NULL
BEGIN
    DROP TABLE tWesternUnionCountryCurrencies 
END
GO
IF OBJECT_ID(N'tWesternUnionCountryCurrencyDeliveryMethods', N'U') IS NOT NULL
BEGIN
    DROP TABLE tWesternUnionCountryCurrencyDeliveryMethods 
END
GO
IF OBJECT_ID(N'tWesternUnionDeliveryOptions', N'U') IS NOT NULL
BEGIN
    DROP TABLE tWesternUnionDeliveryOptions 
END
GO
IF OBJECT_ID(N'tWesternUnionLogs', N'U') IS NOT NULL
BEGIN
    DROP TABLE tWesternUnionLogs 
END
GO
IF OBJECT_ID(N'tWesternUnionPaymentMethods', N'U') IS NOT NULL
BEGIN
    DROP TABLE tWesternUnionPaymentMethods 
END
GO
IF OBJECT_ID(N'tWesternUnionPickupDetails', N'U') IS NOT NULL
BEGIN
    DROP TABLE tWesternUnionPickupDetails 
END
GO
IF OBJECT_ID(N'tWesternUnionPickupMethods', N'U') IS NOT NULL
BEGIN
    DROP TABLE tWesternUnionPickupMethods 
END
GO
IF OBJECT_ID(N'tWesternUnionReceiverProfiles', N'U') IS NOT NULL
BEGIN
    DROP TABLE tWesternUnionReceiverProfiles 
END
GO
IF OBJECT_ID(N'tWesternUnionStates', N'U') IS NOT NULL
BEGIN
    DROP TABLE tWesternUnionStates 
END
GO
IF OBJECT_ID(N'tWUnion_AmountTypes', N'U') IS NOT NULL
BEGIN
    DROP TABLE tWUnion_AmountTypes 
END
GO
IF OBJECT_ID(N'tWUnion_CountryCurrencyDeliveryMethods', N'U') IS NOT NULL
BEGIN
    DROP TABLE tWUnion_CountryCurrencyDeliveryMethods 
END
GO
IF OBJECT_ID(N'tWUnion_DeliveryOptions', N'U') IS NOT NULL
BEGIN
    DROP TABLE tWUnion_DeliveryOptions 
END
GO
IF OBJECT_ID(N'tWUnion_NameTypeMapping', N'U') IS NOT NULL
BEGIN
    DROP TABLE tWUnion_NameTypeMapping 
END
GO
IF OBJECT_ID(N'tWUnion_PaymentMethods', N'U') IS NOT NULL
BEGIN
    DROP TABLE tWUnion_PaymentMethods 
END
GO
IF OBJECT_ID(N'tWUnion_PickupDetails', N'U') IS NOT NULL
BEGIN
    DROP TABLE tWUnion_PickupDetails 
END
GO
IF OBJECT_ID(N'tWUnion_PickupMethods', N'U') IS NOT NULL
BEGIN
    DROP TABLE tWUnion_PickupMethods 
END
GO
IF OBJECT_ID(N'tWUnion_Recipient_Account', N'U') IS NOT NULL
BEGIN
    DROP TABLE tWUnion_Recipient_Account 
END
GO
IF OBJECT_ID(N'tWUnion_Relationships', N'U') IS NOT NULL
BEGIN
    DROP TABLE tWUnion_Relationships 
END
GO

IF OBJECT_ID(N'tChannelPartnerSMTPDetails', N'U') IS NOT NULL
BEGIN
    DROP TABLE tChannelPartnerSMTPDetails 
END
GO

IF OBJECT_ID(N'tAgentAuthentication', N'U') IS NOT NULL
BEGIN
    DROP TABLE tAgentAuthentication 
END
GO

IF OBJECT_ID(N'tAgentLocationMapping', N'U') IS NOT NULL
BEGIN
    DROP TABLE tAgentLocationMapping 
END
GO

IF OBJECT_ID(N'tUserStatuses', N'U') IS NOT NULL
BEGIN
    DROP TABLE tUserStatuses 
END
GO

IF OBJECT_ID(N'tCountries', N'U') IS NOT NULL
BEGIN
    DROP TABLE tCountries 
END
GO

IF OBJECT_ID(N'tXRecipientProfiles', N'U') IS NOT NULL
BEGIN
    DROP TABLE tXRecipientProfiles 
END
GO
