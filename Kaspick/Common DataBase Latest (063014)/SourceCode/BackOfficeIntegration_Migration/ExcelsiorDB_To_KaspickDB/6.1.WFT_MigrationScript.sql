SET NOCOUNT ON

--TBL_WFT_ETLRunTime Migration Script
INSERT INTO dbo.TBL_WFT_ETLRunTime (ETL_lastRunTime)
SELECT ETL_lastRunTime
FROM $(ExcelsiorDB)..WFT_ETL_RunTime

--TBL_WFT_Flow Migration Script
SET IDENTITY_INSERT TBL_WFT_Flow ON

INSERT INTO dbo.TBL_WFT_Flow (
	flow_id
	,flow_name
	)
SELECT flow_id
	,flow_name
FROM $(ExcelsiorDB)..WFT_Flow

SET IDENTITY_INSERT TBL_WFT_Flow OFF
--TBL_WFT_FlowStatus Migration Script
SET IDENTITY_INSERT TBL_WFT_FlowStatus ON

INSERT INTO dbo.TBL_WFT_FlowStatus (
	flow_status_id
	,flow_status_name
	,flow_status_abbrev
	,is_closed
	)
SELECT flow_status_id
	,flow_status_name
	,flow_status_abbrev
	,is_closed
FROM $(ExcelsiorDB)..WFT_Flow_Status

SET IDENTITY_INSERT TBL_WFT_FlowStatus OFF

-- TBL_WFT_ML_FrmCtrl script
INSERT INTO TBL_WFT_ML_FrmCtrl (
	FormID
	,Version
	,StratiTableName
	,WorkFlowTableName
	,FlowTitle
	,Active
	,WFTemplatesName
	,WFTemplatesPath
	,WFRunningPath
	,WFCompletedPath
	,form_guid
	)
SELECT FormID
	,Version
	,StratiTableName
	,WorkFlowTableName
	,FlowTitle
	,Active
	,WFTemplatesName
	,WFTemplatesPath
	,WFRunningPath
	,WFCompletedPath
	,form_guid
FROM $(ExcelsiorDB)..WFT_ML_FRMCTRL

-- TBL_WFT_ProcessingTeam script
SET IDENTITY_INSERT dbo.TBL_WFT_ProcessingTeam ON

INSERT INTO TBL_WFT_ProcessingTeam (
	processing_team_id
	,processing_team_name
	)
SELECT processing_team_id
	,processing_team_name
FROM $(ExcelsiorDB)..WFT_Processing_Team

SET IDENTITY_INSERT dbo.TBL_WFT_ProcessingTeam OFF

--TBL_WFT_AdditionToTrust Migration Script
INSERT INTO dbo.TBL_WFT_AdditionToTrust (
	addition_to_trust_id
	,GuID
	,form_id
	,notification_date
	,submitted_by
	,is_revised_submission
	,revision_date
	,ManagerCode
	,ManagerCodeName
	,CustomerAccountNumber
	,account_name
	,account_type
	,reason_for_submission
	,comments
	,asset_delivery_arrival
	,expected_asset_arrival_date
	,gift_date
	,gift_restriction
	,asset_type4
	,asset_type3
	,asset_type2
	,asset_type1
	,ticker_symbol4
	,ticker_symbol3
	,ticker_symbol2
	,ticker_symbol1
	,price_source4
	,price_source3
	,price_source2
	,price_source1
	,number_of_share4
	,number_of_share3
	,number_of_share2
	,number_of_share1
	,high_price4
	,high_price3
	,high_price2
	,high_price1
	,donors_date_of_acquisition4
	,donors_date_of_acquisition3
	,donors_date_of_acquisition2
	,donors_date_of_acquisition1
	,low_price4
	,low_price3
	,low_price2
	,low_price1
	,chkbx_long_term_holding4
	,chkbx_long_term_holding3
	,chkbx_long_term_holding2
	,chkbx_long_term_holding1
	,average_price4
	,average_price3
	,average_price2
	,average_price1
	,donors_cost_basis4
	,donors_cost_basis3
	,donors_cost_basis2
	,donors_cost_basis1
	,zero_cost_basis1
	,zero_cost_basis2
	,zero_cost_basis3
	,zero_cost_basis4
	,asset_value4
	,asset_value3
	,asset_value2
	,asset_value1
	,delivery_method4
	,delivery_method3
	,delivery_method2
	,delivery_method1
	,asset_description4
	,asset_description3
	,asset_description2
	,asset_description1
	,value_of_additional_gifted_assets
	,total_gift_value
	,total_cost_basis
	,investment_trading_strategy
	,current_investment_objective
	,trading_policy_additions_sell
	,trading_policy_additions_invest
	,pxy_userid
	,pxy_username
	,pxy_useremail
	,org_userid
	,org_useremail
	,org_username
	,client_manager
	,trust_administrator
	,modified
	,modified_by
	,ta_authorization_level
	,im_authorization_level
	,team_folder_name
	,hidden_asset_restore1
	,hidden_asset_restore2
	,hidden_asset_restore3
	,hidden_asset_restore4
	,hidden_asset_updates
	,fax_asset_appraisal
	,fax_investment_directive
	,fax_broker_confirm_trade
	)
SELECT addition_to_trust_id
	,guid
	,form_id
	,notification_date
	,submitted_by
	,is_revised_submission
	,revision_date
	,CLNT.BriefName
	,client_name
	,advent_id
	,account_name
	,AccMstr.AccountTypeCode
	,reason_for_submission
	,TRST.comments
	,asset_delivery_arrival
	,expected_asset_arrival_date
	,gift_date
	,gift_restriction
	,asset_type4
	,asset_type3
	,asset_type2
	,asset_type1
	,ticker_symbol4
	,ticker_symbol3
	,ticker_symbol2
	,ticker_symbol1
	,price_source4
	,price_source3
	,price_source2
	,price_source1
	,number_of_share4
	,number_of_share3
	,number_of_share2
	,number_of_share1
	,high_price4
	,high_price3
	,high_price2
	,high_price1
	,donors_date_of_acquisition4
	,donors_date_of_acquisition3
	,donors_date_of_acquisition2
	,donors_date_of_acquisition1
	,low_price4
	,low_price3
	,low_price2
	,low_price1
	,chkbx_long_term_holding4
	,chkbx_long_term_holding3
	,chkbx_long_term_holding2
	,chkbx_long_term_holding1
	,average_price4
	,average_price3
	,average_price2
	,average_price1
	,donors_cost_basis4
	,donors_cost_basis3
	,donors_cost_basis2
	,donors_cost_basis1
	,zero_cost_basis1
	,zero_cost_basis2
	,zero_cost_basis3
	,zero_cost_basis4
	,asset_value4
	,asset_value3
	,asset_value2
	,asset_value1
	,delivery_method4
	,delivery_method3
	,delivery_method2
	,delivery_method1
	,asset_description4
	,asset_description3
	,asset_description2
	,asset_description1
	,value_of_additional_gifted_assets
	,total_gift_value
	,total_cost_basis
	,investment_trading_strategy
	,current_investment_objective
	,trading_policy_additions_sell
	,trading_policy_additions_invest
	,pxy_userid
	,pxy_username
	,pxy_useremail
	,org_userid
	,org_useremail
	,org_username
	,client_manager
	,trust_administrator
	,modified
	,isnull(USR.UserID, 1)
	,ta_authorization_level
	,im_authorization_level
	,team_folder_name
	,hidden_asset_restore1
	,hidden_asset_restore2
	,hidden_asset_restore3
	,hidden_asset_restore4
	,hidden_asset_updates
	,fax_asset_appraisal
	,fax_investment_directive
	,fax_broker_confirm_trade
FROM $(ExcelsiorDB)..WFT_Addition_To_Trust TRST
INNER JOIN $(ExcelsiorDB)..CLIENT CLNT
	ON TRST.Client_ID = CLNT.ClientID
LEFT OUTER JOIN $(InnoTrustDB)..AccountMaster AccMstr
 ON TRST.ADVENT_ID = AccMstr.CustomerAccountNumber
LEFT OUTER JOIN TBL_KS_User USR
	ON TRST.modified_by = USR.UserID

--TBL_WFT_AdditionalDonatedAssset Migration Script
INSERT INTO dbo.TBL_WFT_AdditionalDonatedAssset (
	additional_donated_assets_id
	,form_id
	,GuID
	,is_revised_submission
	,revision_date
	,notification_date
	,ManagerCode
	,ManagerCodeName
	,CustomerAccountNumber
	,account_name
	,account_type
	,beneficiary_name
	,comments
	,asset_delivery_arrival
	,expected_asset_arrival_date
	,gift_date
	,asset_type4
	,asset_type3
	,asset_type2
	,asset_type1
	,ticker_symbol4
	,ticker_symbol3
	,ticker_symbol2
	,ticker_symbol1
	,price_source4
	,price_source3
	,price_source2
	,price_source1
	,number_of_share4
	,number_of_share3
	,number_of_share2
	,number_of_share1
	,high_price4
	,high_price3
	,high_price2
	,high_price1
	,donors_date_of_acquisition4
	,donors_date_of_acquisition3
	,donors_date_of_acquisition2
	,donors_date_of_acquisition1
	,low_price4
	,low_price3
	,low_price2
	,low_price1
	,long_term_holding4
	,long_term_holding3
	,long_term_holding2
	,long_term_holding1
	,average_price4
	,average_price3
	,average_price2
	,average_price1
	,donors_cost_basis4
	,donors_cost_basis3
	,donors_cost_basis2
	,donors_cost_basis1
	,zero_cost_basis1
	,zero_cost_basis2
	,zero_cost_basis3
	,zero_cost_basis4
	,asset_value4
	,asset_value3
	,asset_value2
	,asset_value1
	,delivery_method4
	,delivery_method3
	,delivery_method2
	,delivery_method1
	,asset_description4
	,asset_description3
	,asset_description2
	,asset_description1
	,hidden_asset_restore1
	,hidden_asset_restore2
	,hidden_asset_restore3
	,hidden_asset_restore4
	,total_asset_value
	,total_cost_basis
	,org_userid
	,org_useremail
	,org_username
	,pxy_userid
	,pxy_username
	,pxy_useremail
	,trust_administrator
	,client_manager
	,modified
	,modified_by
	,submitted_by
	,team_folder_name
	,fax_investment_directive
	,fax_broker_confirm_trade
	,fax_asset_appraisal
	)
SELECT additional_donated_assets_id
	,form_id
	,guid
	,is_revised_submission
	,revision_date
	,notification_date
	,CLNT.BriefName
	,client_name
	,advent_id
	,account_name
	,AccMstr.AccountTypeCode
	,beneficiary_name
	,ADA.comments
	,asset_delivery_arrival
	,expected_asset_arrival_date
	,gift_date
	,asset_type4
	,asset_type3
	,asset_type2
	,asset_type1
	,ticker_symbol4
	,ticker_symbol3
	,ticker_symbol2
	,ticker_symbol1
	,price_source4
	,price_source3
	,price_source2
	,price_source1
	,number_of_share4
	,number_of_share3
	,number_of_share2
	,number_of_share1
	,high_price4
	,high_price3
	,high_price2
	,high_price1
	,donors_date_of_acquisition4
	,donors_date_of_acquisition3
	,donors_date_of_acquisition2
	,donors_date_of_acquisition1
	,low_price4
	,low_price3
	,low_price2
	,low_price1
	,long_term_holding4
	,long_term_holding3
	,long_term_holding2
	,long_term_holding1
	,average_price4
	,average_price3
	,average_price2
	,average_price1
	,donors_cost_basis4
	,donors_cost_basis3
	,donors_cost_basis2
	,donors_cost_basis1
	,zero_cost_basis1
	,zero_cost_basis2
	,zero_cost_basis3
	,zero_cost_basis4
	,asset_value4
	,asset_value3
	,asset_value2
	,asset_value1
	,delivery_method4
	,delivery_method3
	,delivery_method2
	,delivery_method1
	,asset_description4
	,asset_description3
	,asset_description2
	,asset_description1
	,hidden_asset_restore1
	,hidden_asset_restore2
	,hidden_asset_restore3
	,hidden_asset_restore4
	,total_asset_value
	,total_cost_basis
	,org_userid
	,org_useremail
	,org_username
	,pxy_userid
	,pxy_username
	,pxy_useremail
	,trust_administrator
	,client_manager
	,modified
	,ISNULL(USR.userid, 1)
	,submitted_by
	,team_folder_name
	,fax_investment_directive
	,fax_broker_confirm_trade
	,fax_asset_appraisal
FROM $(ExcelsiorDB)..WFT_Additional_Donated_Asssets ADA
INNER JOIN $(ExcelsiorDB)..CLIENT CLNT
	ON ADA.Client_ID = CLNT.ClientID
INNER JOIN $(InnoTrustDB)..AccountMaster AccMstr
 ON ADA.ADVENT_ID = AccMstr.CustomerAccountNumber
LEFT OUTER JOIN TBL_KS_User USR
	ON ADA.modified_by = USR.UserID

--TBL_WFT_AddressChange Migration Script	
INSERT INTO dbo.TBL_WFT_AddressChange (
	address_change_id
	,form_id
	,GuID
	,ContactID
	,ManagerCode
	,ManagerCodeFullName
	,donor_bene_name
	,donor_bene_ssn_ein
	,is_all_or_some
	,accounts_affected
	,accounts_affected_count
	,accts_array
	,accts_total_count
	,legal_address1
	,legal_address2
	,legal_attn
	,legal_attn_name
	,legal_city
	,legal_country
	,legal_domicile
	,legal_effective_date
	,legal_state
	,legal_unit_number
	,legal_unit_type
	,legal_zip
	,alt_address1
	,alt_address2
	,alt_attn
	,alt_attn_name
	,alt_city
	,alt_country
	,alt_effective_date
	,alt_state
	,alt_unit_number
	,alt_unit_type
	,alt_zip
	,is_seasonal
	,seasonal_from_month
	,seasonal_to_month
	,comments
	,trust_admin
	,client_manager
	,notification_date
	,org_userid
	,org_username
	,org_useremail
	,pxy_userid
	,pxy_username
	,pxy_useremail
	,modified_by
	,modified_date
	,submitted_by
	,im_authorization_level
	,ta_authorization_level
	,team_folder_name
	)
SELECT address_change_id
	,form_id
	,GUID
	,PartLkup.CONTACTID
	,CLNT.BriefName
	,client_full_name
	,donor_bene_name
	,donor_bene_ssn_ein
	,is_all_or_some
	,accounts_affected
	,accounts_affected_count
	,accts_array
	,accts_total_count
	,legal_address1
	,legal_address2
	,legal_attn
	,legal_attn_name
	,legal_city
	,legal_country
	,legal_domicile
	,legal_effective_date
	,legal_state
	,legal_unit_number
	,legal_unit_type
	,legal_zip
	,alt_address1
	,alt_address2
	,alt_attn
	,alt_attn_name
	,alt_city
	,alt_country
	,alt_effective_date
	,alt_state
	,alt_unit_number
	,alt_unit_type
	,alt_zip
	,is_seasonal
	,seasonal_from_month
	,seasonal_to_month
	,ADRS.comments
	,trust_admin
	,client_manager
	,notification_date
	,org_userid
	,org_username
	,org_useremail
	,pxy_userid
	,pxy_username
	,pxy_useremail
	,ISNULL(USR.userid, 1)
	,modified_date
	,submitted_by
	,im_authorization_level
	,ta_authorization_level
	,team_folder_name
FROM $(ExcelsiorDB)..WFT_Address_Change ADRS
INNER JOIN $(ExcelsiorDB)..CLIENT CLNT
	ON ADRS.ClientID = CLNT.ClientID
INNER JOIN $(MappingDB)..TBL_TrustParticipantLookup PartLkup
	ON PartLkup.ParticipantID = ADRS.Participant_ID
	--and CLNT.BriefName=PartLkup.ClientBriefName -- Need to remove once migration issue is resolved
LEFT OUTER JOIN TBL_KS_User USR
	ON ADRS.modified_by = USR.UserID
--WHERE PartLkup.ParticipantID NOT IN ( -- Need to remove once migration issue is resolved
--		SELECT ParticipantID
--		FROM $(MappingDB)..TBL_ParticipantContactLookUp
--		GROUP BY ParticipantID
--		HAVING COUNT(ParticipantID) > 1
--		)

--TBL_WFT_BankPaymentChange Migration Script	
INSERT INTO dbo.TBL_WFT_BankPaymentChange (
	bank_payment_change_id
	,form_id
	,GuID
	,ManagerCode
	,ManagerCodeName
	,ContactID
	,donor_bene_name
	,donor_bene_ssn_ein
	,notification_date
	,effective_date
	,accts_affected
	,accts_total_count
	,accts_array
	,accts_affected_count
	,comments
	,payment_method
	,institution_split_check_name
	,institution_attn_name
	,institution_address
	,institution_city
	,institution_state
	,institution_zip_code
	,institution_country
	,institution_phone_number
	,institution_ach_name
	,account_holder_name
	,bank_account_number
	,aba_number
	,account_type
	,client_manager
	,trust_admininistrator
	,submitted_by
	,modified_date
	,modified_user_id
	,org_userid
	,org_username
	,org_useremail
	,pxy_userid
	,pxy_username
	,pxy_useremail
	,im_authorization_level
	,ta_authorization_level
	,team_folder_name
	)
SELECT bank_payment_change_id
	,form_id
	,GUID
	,CLNT.BriefName
	,client_name
	,PartLkup.CONTACTID
	,donor_bene_name
	,donor_bene_ssn_ein
	,notification_date
	,effective_date
	,accts_affected
	,accts_total_count
	,accts_array
	,accts_affected_count
	,BPC.comments
	,payment_method
	,institution_split_check_name
	,institution_attn_name
	,institution_address
	,institution_city
	,institution_state
	,institution_zip_code
	,institution_country
	,institution_phone_number
	,institution_ach_name
	,account_holder_name
	,bank_account_number
	,aba_number
	,account_type
	,client_manager
	,trust_admininistrator
	,submitted_by
	,modified_date
	,ISNULL(USR.userid, 1)
	,org_userid
	,org_username
	,org_useremail
	,pxy_userid
	,pxy_username
	,pxy_useremail
	,im_authorization_level
	,ta_authorization_level
	,team_folder_name
FROM $(ExcelsiorDB)..WFT_Bank_Payment_Change BPC
INNER JOIN $(ExcelsiorDB)..CLIENT CLNT
	ON BPC.ClientID = CLNT.ClientID
INNER JOIN $(MappingDB)..TBL_TrustParticipantLookup PartLkup
	ON PartLkup.ParticipantID = BPC.participant_id
	and CLNT.BriefName=PartLkup.ClientBriefName -- Need to remove once migration issue is resolved
LEFT OUTER JOIN TBL_KS_User USR
	ON BPC.modified_user_id = USR.UserID
--WHERE PartLkup.ParticipantID NOT IN ( -- Need to remove once migration issue is resolved
--		SELECT ParticipantID
--		FROM $(MappingDB)..TBL_ParticipantContactLookUp
--		GROUP BY ParticipantID
--		HAVING COUNT(ParticipantID) > 1
--		)


--TBL_WFT_CheckRequest Migration Script
INSERT INTO dbo.TBL_WFT_CheckRequest (
	check_request_id
	,form_id
	,GuID
	,LoginManagerCodeID
	,ManagerCodeName
	,CustomerAccountNumber
	,account_name
	,notification_date
	,account_number
	,has_ta2_limit
	,ta2_limit_amount
	,check_due
	,check_payable_to
	,vendor_tax_id
	,payment_amount
	,charge_percent_to_principal
	,charge_percent_to_income
	,payment_description
	,mail_check_to
	,mailing_address1
	,mailing_address2
	,comments
	,submitted_by
	,modified_date
	,modified_user_id
	,org_userid
	,org_username
	,org_useremail
	,pxy_userid
	,pxy_username
	,pxy_useremail
	,im_authorization_level
	,ta_authorization_level
	,fax_copy_of_invoice
	,team_folder_name
	,client_manager
	,trust_administrator
	)
SELECT check_request_id
	,form_id
	,guid
	,login_clientid
	,client_name
	,DGA.AdventID
	,account_name
	,notification_date
	,account_number
	,has_ta2_limit
	,ta2_limit_amount
	,check_due
	,check_payable_to
	,vendor_tax_id
	,payment_amount
	,charge_percent_to_principal
	,charge_percent_to_income
	,payment_description
	,mail_check_to
	,mailing_address1
	,mailing_address2
	,WCR.comments
	,submitted_by
	,modified_date
	,ISNULL(USR.userid, 1)
	,org_userid
	,org_username
	,org_useremail
	,pxy_userid
	,pxy_username
	,pxy_useremail
	,im_authorization_level
	,ta_authorization_level
	,fax_copy_of_invoice
	,team_folder_name
	,client_manager
	,trust_administrator
FROM $(ExcelsiorDB)..WFT_Check_Request WCR
INNER JOIN $(ExcelsiorDB)..DEFERREDGIFTACCOUNT DGA
	ON DGA.AccountID = WCR.account_id
LEFT OUTER JOIN TBL_KS_User USR
	ON WCR.modified_user_id = USR.UserID

-- TBL_WFT_MultipleBeneficiary script
INSERT INTO TBL_WFT_MultipleBeneficiary (
	multiple_beneficiaries_id
	,form_id
	,GUID
	,notification_date
	,submitted_by
	,ManagerCode
	,CustomerAccountNumber
	,ManagerCodeName
	,account_name
	,relationship_to_donor
	,beneficiary1_salutation
	,bene1_male_female
	,beneficiary1_name
	,beneficiary1_address1
	,beneficiary1_address2
	,beneficiary1_csz
	,beneficiary1_ssn
	,beneficiary1_dob
	,beneficiary2_salutation
	,bene2_male_female
	,beneficiary2_name
	,beneficiary2_address1
	,beneficiary2_address2
	,beneficiary2_csz
	,beneficiary2_ssn
	,beneficiary2_dob
	,bene1_auth_for_direct_deposit
	,bank1_pay_to_the_order_of
	,bank1_name
	,bank1_account_number
	,bank1_phone_number
	,bank1_address1
	,bank1_address2
	,bank1_csz
	,bene2_auth_for_direct_deposit
	,bank2_pay_to_the_order_of
	,bank2_name
	,bank2_account_number
	,bank2_phone_number
	,bank2_address1
	,bank2_address2
	,bank2_csz
	,radio_tax_preparation
	,under_one_ssn
	,org_userid
	,org_username
	,org_useremail
	,pxy_userid
	,pxy_username
	,pxy_useremail
	,modified_date
	,modified_user_id
	,team_folder_name
	,chkbx_revision
	,revision_date
	,chkbx_fax_auth_deposit
	,client_manager
	,trust_admininistrator
	,payment_method
	)
SELECT multiple_beneficiaries_id
	,form_id
	,GUID
	,notification_date
	,submitted_by
	,CLNT.BriefName
	,DGA.AdventID
	,client_name
	,account_name
	,relationship_to_donor
	,beneficiary1_salutation
	,bene1_male_female
	,beneficiary1_name
	,beneficiary1_address1
	,beneficiary1_address2
	,beneficiary1_csz
	,beneficiary1_ssn
	,beneficiary1_dob
	,beneficiary2_salutation
	,bene2_male_female
	,beneficiary2_name
	,beneficiary2_address1
	,beneficiary2_address2
	,beneficiary2_csz
	,beneficiary2_ssn
	,beneficiary2_dob
	,bene1_auth_for_direct_deposit
	,bank1_pay_to_the_order_of
	,bank1_name
	,bank1_account_number
	,bank1_phone_number
	,bank1_address1
	,bank1_address2
	,bank1_csz
	,bene2_auth_for_direct_deposit
	,bank2_pay_to_the_order_of
	,bank2_name
	,bank2_account_number
	,bank2_phone_number
	,bank2_address1
	,bank2_address2
	,bank2_csz
	,radio_tax_preparation
	,under_one_ssn
	,org_userid
	,org_username
	,org_useremail
	,pxy_userid
	,pxy_username
	,pxy_useremail
	,modified_date
	,ISNULL(USR.userid, 1)
	,team_folder_name
	,chkbx_revision
	,revision_date
	,chkbx_fax_auth_deposit
	,client_manager
	,trust_admininistrator
	,payment_method
FROM $(ExcelsiorDB)..WFT_Multiple_Beneficiaries WMulBen
INNER JOIN $(ExcelsiorDB)..CLIENT CLNT
	ON WMulBen.client_id = CLNT.ClientID
INNER JOIN $(ExcelsiorDB)..DEFERREDGIFTACCOUNT DGA
	ON DGA.AccountID = WMulBen.account_id
LEFT OUTER JOIN TBL_KS_User USR
	ON WMulBen.modified_user_id = USR.UserID

-- TBL_WFT_NameChange script
INSERT INTO TBL_WFT_NameChange (
	name_change_id
	,form_id
	,GUID
	,notification_date
	,ManagerCode
	,ManagerCodeName
	,ContactID
	,donor_bene_name
	,donor_bene_ssn_ein
	,payment_name
	,accts_array
	,accts_affected_count
	,accts_affected
	,accts_total_count
	,donor_bene_salutation
	,donor_bene_first_name
	,donor_bene_middle_name
	,donor_bene_last_name
	,donor_bene_suffix
	,comments
	,client_manager
	,trust_administrator
	,submitted_by
	,modified_user_id
	,modified_date
	,org_userid
	,org_username
	,org_useremail
	,pxy_userid
	,pxy_username
	,pxy_useremail
	,im_authorization_level
	,ta_authorization_level
	,team_folder_name
	)
SELECT name_change_id
	,form_id
	,GUID
	,notification_date
	,CLNT.BriefName
	,client_name
	,PartLkup.CONTACTID
	,donor_bene_name
	,donor_bene_ssn_ein
	,payment_name
	,accts_array
	,accts_affected_count
	,accts_affected
	,accts_total_count
	,donor_bene_salutation
	,donor_bene_first_name
	,donor_bene_middle_name
	,donor_bene_last_name
	,donor_bene_suffix
	,WNameChng.comments
	,client_manager
	,trust_administrator
	,submitted_by
	,ISNULL(USR.userid, 1)
	,modified_date
	,org_userid
	,org_username
	,org_useremail
	,pxy_userid
	,pxy_username
	,pxy_useremail
	,im_authorization_level
	,ta_authorization_level
	,team_folder_name
FROM $(ExcelsiorDB)..WFT_Name_Change WNameChng
INNER JOIN $(ExcelsiorDB)..CLIENT CLNT
	ON WNameChng.clientid = CLNT.ClientID
INNER JOIN $(MappingDB)..TBL_TrustParticipantLookup PartLkup
	ON PartLkup.ParticipantID = WNameChng.participant_id
	--and CLNT.BriefName=PartLkup.ClientBriefName -- Need to remove once migration issue is resolved
LEFT OUTER JOIN TBL_KS_User USR
	ON WNameChng.modified_user_id = USR.UserID
--WHERE PartLkup.ParticipantID NOT IN ( -- Need to remove once migration issue is resolved
--		SELECT ParticipantID
--		FROM $(MappingDB)..TBL_ParticipantContactLookUp
--		GROUP BY ParticipantID
--		HAVING COUNT(ParticipantID) > 1
--		)

-- TBL_WFT_NewTrustInfo script
INSERT INTO TBL_WFT_NewTrustInfo (
	new_trust_info_id
	,guid
	,form_id
	,is_initial_revised
	,is_revision
	,is_initial
	,notification_date
	,submitted_by
	,LoginManagerCodeID
	,ManagerCodeName
	,is_co_trustee
	,account_name
	,trustee_code
	,trust_type
	,if_other
	,trust_payout_fixed_amount
	,trust_payout_percent
	,date_trust_signed
	,approx_trust_value
	,expected_asset_tranfer_date
	,comments
	,donor1_salutation
	,donor1_male_female
	,donor1_name
	,donor1_address1
	,donor1_address2
	,donor1_csz
	,donor1_ssn
	,donor1_dob
	,donor2_salutation
	,donor2_male_female
	,donor2_name
	,donor2_address1
	,donor2_address2
	,donor2_csz
	,donor2_dob
	,donor2_ssn
	,relationship_to_donor
	,beneficiary1_salutation
	,beneficiary1_male_female
	,beneficiary1_name
	,beneficiary1_address1
	,beneficiary1_address2
	,beneficiary1_csz
	,beneficiary1_ssn
	,beneficiary1_dob
	,beneficiary2_salutation
	,beneficiary2_male_female
	,beneficiary2_name
	,beneficiary2_address2
	,beneficiary2_address1
	,beneficiary2_csz
	,beneficiary2_ssn
	,beneficiary2_dob
	,auth_for_direct_deposit1
	,pay_to_the_order_of1
	,bank1_name
	,bank1_account_no
	,bank1_phone_number
	,bank1_address1
	,bank1_address2
	,bank1_csz
	,auth_for_direct_deposit2
	,pay_to_the_order_of2
	,bank2_name
	,bank2_account_number
	,bank2_phone_number
	,bank2_address1
	,bank2_address2
	,bank2_csz
	,tax_preparation
	,jointly_under_one_ssn
	,gift_purpose
	,remainderman_designation
	,fund_name
	,gift_restriction
	,is_multiple_remaindermen
	,irs_discount_rate
	,radio_asset_management
	,asset_na
	,asset_principal_percent
	,asset_principal_dollars
	,asset_dollars
	,asset_percent
	,trust_administration_fee
	,trust_na
	,trust_principal_dollars
	,trust_principal_percent
	,trust_percent
	,trust_dollars
	,radio_trustee
	,trustee_na
	,charge_to_trustee
	,trustee_principal_dollars
	,trustee_principal_percent
	,trustee_dollars
	,trustee_percent
	,investment_strategy
	,modified
	,modified_by
	,org_userid
	,org_useremail
	,org_username
	,pxy_userid
	,pxy_username
	,pxy_useremail
	,im_authorization_level
	,ta_authorization_level
	,client_manager
	,trust_administrator
	,team_folder_name
	,revision_date
	,ManagerCodeAccountCode2
	,general_comments
	)
SELECT new_trust_info_id
	,guid
	,form_id
	,is_initial_revised
	,is_revision
	,is_initial
	,notification_date
	,submitted_by
	,login_clientid
	,client_name
	,is_co_trustee
	,account_name
	,trustee_code
	,trust_type
	,if_other
	,trust_payout_fixed_amount
	,trust_payout_percent
	,date_trust_signed
	,approx_trust_value
	,expected_asset_tranfer_date
	,comments
	,donor1_salutation
	,donor1_male_female
	,donor1_name
	,donor1_address1
	,donor1_address2
	,donor1_csz
	,donor1_ssn
	,donor1_dob
	,donor2_salutation
	,donor2_male_female
	,donor2_name
	,donor2_address1
	,donor2_address2
	,donor2_csz
	,donor2_dob
	,donor2_ssn
	,relationship_to_donor
	,beneficiary1_salutation
	,beneficiary1_male_female
	,beneficiary1_name
	,beneficiary1_address1
	,beneficiary1_address2
	,beneficiary1_csz
	,beneficiary1_ssn
	,beneficiary1_dob
	,beneficiary2_salutation
	,beneficiary2_male_female
	,beneficiary2_name
	,beneficiary2_address2
	,beneficiary2_address1
	,beneficiary2_csz
	,beneficiary2_ssn
	,beneficiary2_dob
	,auth_for_direct_deposit1
	,pay_to_the_order_of1
	,bank1_name
	,bank1_account_no
	,bank1_phone_number
	,bank1_address1
	,bank1_address2
	,bank1_csz
	,auth_for_direct_deposit2
	,pay_to_the_order_of2
	,bank2_name
	,bank2_account_number
	,bank2_phone_number
	,bank2_address1
	,bank2_address2
	,bank2_csz
	,tax_preparation
	,jointly_under_one_ssn
	,gift_purpose
	,remainderman_designation
	,fund_name
	,gift_restriction
	,is_multiple_remaindermen
	,irs_discount_rate
	,radio_asset_management
	,asset_na
	,asset_principal_percent
	,asset_principal_dollars
	,asset_dollars
	,asset_percent
	,trust_administration_fee
	,trust_na
	,trust_principal_dollars
	,trust_principal_percent
	,trust_percent
	,trust_dollars
	,radio_trustee
	,trustee_na
	,charge_to_trustee
	,trustee_principal_dollars
	,trustee_principal_percent
	,trustee_dollars
	,trustee_percent
	,investment_strategy
	,modified
	,ISNULL(USR.userid, 1)
	,org_userid
	,org_useremail
	,org_username
	,pxy_userid
	,pxy_username
	,pxy_useremail
	,im_authorization_level
	,ta_authorization_level
	,client_manager
	,trust_administrator
	,team_folder_name
	,revision_date
	,CLIENT_ACCOUNT_CODE2
	,general_comments
FROM $(ExcelsiorDB)..WFT_New_Trust_Info TrInfo
LEFT OUTER JOIN TBL_KS_User USR
	ON TrInfo.modified_by = USR.UserID

-- TBL_WFT_PifGapcontactDataAssetValution script
INSERT INTO TBL_WFT_PifGapcontactDataAssetValution (
	Pif_Gap_Contact_Data_Asset_Valution_ID
	,form_id
	,guid
	,revision_date
	,notification_date
	,submitted_by
	,is_revised_submission
	,LoginManagerCodeID
	,ManagerCodeName
	,CustomerAccountNumber
	,account_name
	,account_type
	,comments
	,asset_delivery_arrival
	,expected_asset_arrival_date
	,gift_date
	,asset_type4
	,asset_type3
	,asset_type2
	,asset_type1
	,ticker_symbol4
	,ticker_symbol3
	,ticker_symbol2
	,ticker_symbol1
	,price_source4
	,price_source3
	,price_source2
	,price_source1
	,number_of_share4
	,number_of_share3
	,number_of_share2
	,number_of_share1
	,high_price4
	,high_price3
	,high_price2
	,high_price1
	,donors_date_of_acquisition4
	,donors_date_of_acquisition3
	,donors_date_of_acquisition2
	,donors_date_of_acquisition1
	,low_price4
	,low_price3
	,low_price2
	,low_price1
	,long_term_holding4
	,long_term_holding3
	,long_term_holding2
	,long_term_holding1
	,average_price4
	,average_price3
	,average_price2
	,average_price1
	,donors_cost_basis4
	,donors_cost_basis3
	,donors_cost_basis2
	,donors_cost_basis1
	,chkbx_zero_cost_1
	,chkbx_zero_cost_2
	,chkbx_zero_cost_3
	,chkbx_zero_cost_4
	,asset_value4
	,asset_value3
	,asset_value2
	,asset_value1
	,delivery_method4
	,delivery_method3
	,delivery_method2
	,delivery_method1
	,asset_description4
	,asset_description3
	,asset_description2
	,asset_description1
	,value_of_additional_gifted_assets
	,total_gift_value
	,total_cost_basis
	,irs_discount_rate
	,charitable_deduction_amount
	,gift_purpose
	,gift_restriction
	,contract_number
	,assets_owned_by
	,asset_joint_owned_by
	,annuity_percent
	,annuity_amount
	,date_of_first_payment
	,payment_frequency
	,prorated_first_payment_amount
	,regular_payment_amount
	,bene1_salutation
	,bene1_male_female
	,bene1_first_name
	,bene1_middle_initial
	,bene1_last_name
	,bene1_address3
	,bene1_address2
	,bene1_address1
	,bene1_city
	,bene1_state
	,bene1_zip
	,bene1_country
	,bene1_ssn
	,bene1_birthdate
	,bene1_relationship_to_gift
	,bene2_salutation
	,bene2_male_female
	,bene2_first_name
	,bene2_middle_initial
	,bene2_last_name
	,bene2_address3
	,bene2_address2
	,bene2_address1
	,bene2_city
	,bene2_state
	,bene2_zip
	,bene2_country
	,bene2_ssn
	,bene2_birthdate
	,bene2_relationship_to_gift
	,bene2_relationship_to_bene1
	,donor_first_name
	,donor_last_name
	,use_same_payment_method
	,auth_for_direct_deposit1
	,pay_to_the_order_of1
	,bank1_name
	,bank1_account_number
	,bank1_phone_number
	,bank1_address1
	,bank1_address2
	,bank1_city
	,bank1_state
	,bank1_zip
	,auth_for_direct_deposit2
	,pay_to_the_order_of2
	,bank2_name
	,bank2_account_number
	,bank2_phone_number
	,bank2_address2
	,bank2_address1
	,bank2_city
	,bank2_state
	,bank2_zip
	,if_married_payments_will_be
	,if_jointly_ssn
	,from_month
	,to_month
	,completed_by
	,modified
	,modified_by
	,org_userid
	,org_username
	,org_useremail
	,pxy_userid
	,pxy_username
	,pxy_useremail
	,client_manager
	,trust_administrator
	,ta_authorization_level
	,im_authorization_level
	,team_folder_name
	,this_is_for
	)
SELECT pif_gap_participant_data_asset_valution_id
	,form_id
	,guid
	,revision_date
	,notification_date
	,submitted_by
	,is_revised_submission
	,login_clientid
	,client_name
	,advent_id
	,account_name
	,AccMstr.AccountTypeCode
	,comments
	,asset_delivery_arrival
	,expected_asset_arrival_date
	,gift_date
	,asset_type4
	,asset_type3
	,asset_type2
	,asset_type1
	,ticker_symbol4
	,ticker_symbol3
	,ticker_symbol2
	,ticker_symbol1
	,price_source4
	,price_source3
	,price_source2
	,price_source1
	,number_of_share4
	,number_of_share3
	,number_of_share2
	,number_of_share1
	,high_price4
	,high_price3
	,high_price2
	,high_price1
	,donors_date_of_acquisition4
	,donors_date_of_acquisition3
	,donors_date_of_acquisition2
	,donors_date_of_acquisition1
	,low_price4
	,low_price3
	,low_price2
	,low_price1
	,long_term_holding4
	,long_term_holding3
	,long_term_holding2
	,long_term_holding1
	,average_price4
	,average_price3
	,average_price2
	,average_price1
	,donors_cost_basis4
	,donors_cost_basis3
	,donors_cost_basis2
	,donors_cost_basis1
	,chkbx_zero_cost_1
	,chkbx_zero_cost_2
	,chkbx_zero_cost_3
	,chkbx_zero_cost_4
	,asset_value4
	,asset_value3
	,asset_value2
	,asset_value1
	,delivery_method4
	,delivery_method3
	,delivery_method2
	,delivery_method1
	,asset_description4
	,asset_description3
	,asset_description2
	,asset_description1
	,value_of_additional_gifted_assets
	,total_gift_value
	,total_cost_basis
	,irs_discount_rate
	,charitable_deduction_amount
	,gift_purpose
	,gift_restriction
	,contract_number
	,assets_owned_by
	,asset_joint_owned_by
	,annuity_percent
	,annuity_amount
	,date_of_first_payment
	,payment_frequency
	,prorated_first_payment_amount
	,regular_payment_amount
	,bene1_salutation
	,bene1_male_female
	,bene1_first_name
	,bene1_middle_initial
	,bene1_last_name
	,bene1_address3
	,bene1_address2
	,bene1_address1
	,bene1_city
	,bene1_state
	,bene1_zip
	,bene1_country
	,bene1_ssn
	,bene1_birthdate
	,bene1_relationship_to_gift
	,bene2_salutation
	,bene2_male_female
	,bene2_first_name
	,bene2_middle_initial
	,bene2_last_name
	,bene2_address3
	,bene2_address2
	,bene2_address1
	,bene2_city
	,bene2_state
	,bene2_zip
	,bene2_country
	,bene2_ssn
	,bene2_birthdate
	,bene2_relationship_to_gift
	,bene2_relationship_to_bene1
	,donor_first_name
	,donor_last_name
	,use_same_payment_method
	,auth_for_direct_deposit1
	,pay_to_the_order_of1
	,bank1_name
	,bank1_account_number
	,bank1_phone_number
	,bank1_address1
	,bank1_address2
	,bank1_city
	,bank1_state
	,bank1_zip
	,auth_for_direct_deposit2
	,pay_to_the_order_of2
	,bank2_name
	,bank2_account_number
	,bank2_phone_number
	,bank2_address2
	,bank2_address1
	,bank2_city
	,bank2_state
	,bank2_zip
	,if_married_payments_will_be
	,if_jointly_ssn
	,from_month
	,to_month
	,completed_by
	,modified
	,ISNULL(USR.userid, 1)
	,org_userid
	,org_username
	,org_useremail
	,pxy_userid
	,pxy_username
	,pxy_useremail
	,client_manager
	,trust_administrator
	,ta_authorization_level
	,im_authorization_level
	,team_folder_name
	,this_is_for
FROM $(ExcelsiorDB)..WFT_Pif_Gap_Participant_Data_Asset_Valution Val
LEFT OUTER JOIN $(InnoTrustDB)..AccountMaster AccMstr
 ON Val.ADVENT_ID = AccMstr.CustomerAccountNumber
LEFT OUTER JOIN TBL_KS_User USR
	ON Val.modified_by = USR.UserID

-- TBL_WFT_ThirdPartyChange script
INSERT INTO TBL_WFT_ThirdPartyChange (
	advisor_id
	,ManagerCode
	,org_userid
	,form_id
	,advisor1_address2
	,pxy_useremail
	,advisor2_middle_initial
	,advisor1_last_name
	,donor_bene_name
	,advisor1_city
	,comments
	,advisor2_address1
	,advisor2_phone
	,advisor1_address1
	,advisor2_change_type
	,trust_administrator
	,advisor2_name
	,pxy_userid
	,advisor2_first_name
	,org_useremail
	,advisor2_unit_type
	,submitted_by
	,advisor_country
	,advisor2_attn_careof
	,advisor1_phone
	,advisor1_comments
	,advisor1_unit_number
	,advisor1_first_name
	,ManagerCodeName
	,notification_date
	,donor_bene_ssn_ein
	,advisor1_salutation
	,advisor1_zipcode
	,advisor1_unit_type
	,advisor1_middle_initial
	,advisor1_country2
	,team_folder_name
	,advisor2_unit_number
	,advisor1_state
	,advisor1_name
	,advisor2_city
	,advisor2_institution_name
	,advisor2_address2
	,advisor1_attn_careof
	,advisor2_comments
	,client_manager
	,advisor2_salutation
	,advisor2_last_name
	,advisor1_institution_name
	,pxy_username
	,advisor2_state
	,advisor1_change_type
	,advisor2_zipcode
	,advisor1_role
	,org_username
	,advisor2_role
	,GUID
	,modified_date
	,modified_user_id
	,ContactID
	,im_authorization_level
	,ta_authorization_level
	)
SELECT advisor_id
	,CLNT.BriefName
	,org_userid
	,form_id
	,advisor1_address2
	,pxy_useremail
	,advisor2_middle_initial
	,advisor1_last_name
	,donor_bene_name
	,advisor1_city
	,W3rdPChange.comments
	,advisor2_address1
	,advisor2_phone
	,advisor1_address1
	,advisor2_change_type
	,trust_administrator
	,advisor2_name
	,pxy_userid
	,advisor2_first_name
	,org_useremail
	,advisor2_unit_type
	,submitted_by
	,advisor_country
	,advisor2_attn_careof
	,advisor1_phone
	,advisor1_comments
	,advisor1_unit_number
	,advisor1_first_name
	,client_name
	,notification_date
	,donor_bene_ssn_ein
	,advisor1_salutation
	,advisor1_zipcode
	,advisor1_unit_type
	,advisor1_middle_initial
	,advisor1_country2
	,team_folder_name
	,advisor2_unit_number
	,advisor1_state
	,advisor1_name
	,advisor2_city
	,advisor2_institution_name
	,advisor2_address2
	,advisor1_attn_careof
	,advisor2_comments
	,client_manager
	,advisor2_salutation
	,advisor2_last_name
	,advisor1_institution_name
	,pxy_username
	,advisor2_state
	,advisor1_change_type
	,advisor2_zipcode
	,advisor1_role
	,org_username
	,advisor2_role
	,GUID
	,modified_date
	,ISNULL(USR.userid, 1)
	,PartLkup.CONTACTID
	,im_authorization_level
	,ta_authorization_level
FROM $(ExcelsiorDB)..WFT_Third_Party_Change W3rdPChange
INNER JOIN $(ExcelsiorDB)..CLIENT CLNT
	ON W3rdPChange.clientid = CLNT.ClientID
INNER JOIN $(MappingDB)..TBL_TrustParticipantLookup PartLkup
	ON PartLkup.ParticipantID = W3rdPChange.participant_id
	--and CLNT.BriefName=PartLkup.ClientBriefName -- Need to remove once migration issue is resolved
LEFT OUTER JOIN TBL_KS_User USR
	ON W3rdPChange.modified_user_id = USR.UserID
--WHERE PartLkup.ParticipantID NOT IN ( -- Need to remove once migration issue is resolved
--		SELECT ParticipantID
--		FROM $(MappingDB)..TBL_ParticipantContactLookUp
--		GROUP BY ParticipantID
--		HAVING COUNT(ParticipantID) > 1
--		)
		
-- TBL_WFT_TrustAssetValuation script
INSERT INTO TBL_WFT_TrustAssetValuation (
	trust_asset_valuation_id
	,form_id
	,guid
	,is_revised_submission
	,revision_date
	,submitted_by
	,notification_date
	,ManagerCode
	,ManagerCodeName
	,CustomerAccountNumber
	,account_name
	,account_type
	,reason_for_submission
	,comments
	,gift_restriction
	,asset_delivery_arrival
	,expected_asset_arrival_date
	,gift_date
	,asset_type4
	,asset_type3
	,asset_type2
	,asset_type1
	,ticker_symbol4
	,ticker_symbol3
	,ticker_symbol2
	,ticker_symbol1
	,price_source4
	,price_source3
	,price_source2
	,price_source1
	,number_of_share4
	,number_of_share3
	,number_of_share2
	,number_of_share1
	,high_price4
	,high_price3
	,high_price2
	,high_price1
	,donors_date_of_acquisition4
	,donors_date_of_acquisition3
	,donors_date_of_acquisition2
	,donors_date_of_acquisition1
	,low_price4
	,low_price3
	,low_price2
	,low_price1
	,long_term_holding4
	,long_term_holding3
	,long_term_holding2
	,long_term_holding1
	,average_price4
	,average_price3
	,average_price2
	,average_price1
	,donors_cost_basis4
	,donors_cost_basis3
	,donors_cost_basis2
	,donors_cost_basis1
	,asset_value4
	,asset_value3
	,asset_value2
	,asset_value1
	,delivery_method4
	,delivery_method3
	,delivery_method2
	,delivery_method1
	,asset_description4
	,asset_description3
	,asset_description2
	,asset_description1
	,hidden_asset_restore1
	,hidden_asset_restore2
	,hidden_asset_restore3
	,hidden_asset_restore4
	,total_gift_value
	,total_cost_basis
	,value_of_additional_gifted_assets
	,trust_administrator
	,client_manager
	,pxy_useremail
	,org_useremail
	,org_username
	,pxy_username
	,org_userid
	,pxy_userid
	,modified
	,modified_by
	,team_folder_name
	,fax_broker_confirm_trade
	,fax_asset_appraisal
	,fax_investment_directive
	)
SELECT trust_asset_valuation_id
	,form_id
	,guid
	,is_revised_submission
	,revision_date
	,submitted_by
	,notification_date
	,CLNT.BriefName
	,client_name
	,advent_id
	,account_name
	,account_type
	,reason_for_submission
	,WAssetVal.comments
	,gift_restriction
	,asset_delivery_arrival
	,expected_asset_arrival_date
	,gift_date
	,asset_type4
	,asset_type3
	,asset_type2
	,asset_type1
	,ticker_symbol4
	,ticker_symbol3
	,ticker_symbol2
	,ticker_symbol1
	,price_source4
	,price_source3
	,price_source2
	,price_source1
	,number_of_share4
	,number_of_share3
	,number_of_share2
	,number_of_share1
	,high_price4
	,high_price3
	,high_price2
	,high_price1
	,donors_date_of_acquisition4
	,donors_date_of_acquisition3
	,donors_date_of_acquisition2
	,donors_date_of_acquisition1
	,low_price4
	,low_price3
	,low_price2
	,low_price1
	,long_term_holding4
	,long_term_holding3
	,long_term_holding2
	,long_term_holding1
	,average_price4
	,average_price3
	,average_price2
	,average_price1
	,donors_cost_basis4
	,donors_cost_basis3
	,donors_cost_basis2
	,donors_cost_basis1
	,asset_value4
	,asset_value3
	,asset_value2
	,asset_value1
	,delivery_method4
	,delivery_method3
	,delivery_method2
	,delivery_method1
	,asset_description4
	,asset_description3
	,asset_description2
	,asset_description1
	,hidden_asset_restore1
	,hidden_asset_restore2
	,hidden_asset_restore3
	,hidden_asset_restore4
	,total_gift_value
	,total_cost_basis
	,value_of_additional_gifted_assets
	,trust_administrator
	,client_manager
	,pxy_useremail
	,org_useremail
	,org_username
	,pxy_username
	,org_userid
	,pxy_userid
	,modified
	,ISNULL(USR.userid, 1)
	,team_folder_name
	,fax_broker_confirm_trade
	,fax_asset_appraisal
	,fax_investment_directive
FROM $(ExcelsiorDB)..WFT_Trust_Asset_Valuation WAssetVal
INNER JOIN $(ExcelsiorDB)..CLIENT CLNT
	ON WAssetVal.client_id = CLNT.ClientID
LEFT OUTER JOIN TBL_KS_User USR
	ON WAssetVal.modified_by = USR.UserID

-- TBL_WFT_TrustTermination script
INSERT INTO TBL_WFT_TrustTermination (
	trust_termination_id
	,GUID
	,form_id
	,ManagerCode
	,ManagerCodeName
	,CustomerAccountNumber
	,account_name
	,account_type
	,reason_for_notification
	,action_on_account
	,relinquished
	,partial_distribution
	,liquidate_assets
	,comments
	,ContactID
	,beneficiary_name
	,beneficiary_ssn
	,date_of_death
	,documentation_of_death
	,successor_beneficiaries
	,multiple_account
	,use_different_address
	,mfc_add1
	,mfc_add2
	,mfc_add3
	,mfc_city
	,mfc_state
	,mfc_zip
	,mfc_country
	,percent_of_liquidation
	,fixed_distribution_amount
	,distribution_method
	,check_mail_add1
	,check_mail_add3
	,check_mail_add2
	,check_mail_city
	,check_mailing_state
	,check_mail_zip
	,check_mail_country
	,custodial_account_number
	,journal_account_number
	,asset_description
	,use_standing_wire_instruction
	,bank_name
	,account_registration_name
	,aba_number
	,account_number
	,wire_mail_address_line1
	,wire_mail_address_line2
	,wire_mail_city
	,wire_mail_state
	,wire_mail_zip
	,wire_mail_country
	,others_instruction
	,client_manager
	,trust_administrator
	,signature
	,pxy_userid
	,pxy_username
	,pxy_useremail
	,org_userid
	,org_useremail
	,org_username
	,ta_authorization_level
	,im_authorization_level
	,notification_date
	,effective_date
	,modified
	,modified_by
	,submitted_by
	,team_folder_name
	,action_on_account_modified_by
	,action_on_account_modified_date
	,action_on_distribution
	,action_on_liquidate_assets
	,OldTerminationRequest
	)
SELECT trust_termination_id
	,GUID
	,form_id
	,CLNT.BriefName
	,client_name
	,adventid
	,account_name
	,AccMstr.AccountTypeCode
	,reason_for_notification
	,action_on_account
	,relinquished
	,partial_distribution
	,liquidate_assets
	,WTrustTerm.comments
	,PartLkup.CONTACTID
	,beneficiary_name
	,beneficiary_ssn
	,date_of_death
	,documentation_of_death
	,successor_beneficiaries
	,multiple_account
	,use_different_address
	,mfc_add1
	,mfc_add2
	,mfc_add3
	,mfc_city
	,mfc_state
	,mfc_zip
	,mfc_country
	,percent_of_liquidation
	,fixed_distribution_amount
	,distribution_method
	,check_mail_add1
	,check_mail_add3
	,check_mail_add2
	,check_mail_city
	,check_mailing_state
	,check_mail_zip
	,check_mail_country
	,custodial_account_number
	,journal_account_number
	,asset_description
	,use_standing_wire_instruction
	,bank_name
	,account_registration_name
	,aba_number
	,account_number
	,wire_mail_address_line1
	,wire_mail_address_line2
	,wire_mail_city
	,wire_mail_state
	,wire_mail_zip
	,wire_mail_country
	,others_instruction
	,client_manager
	,trust_administrator
	,signature
	,pxy_userid
	,pxy_username
	,pxy_useremail
	,org_userid
	,org_useremail
	,org_username
	,ta_authorization_level
	,im_authorization_level
	,notification_date
	,effective_date
	,modified
	,ISNULL(USR.UserID, 1)
	,submitted_by
	,team_folder_name
	,action_on_account_modified_by
	,action_on_account_modified_date
	,action_on_distribution
	,action_on_liquidate_assets
	,OldTerminationRequest
FROM $(ExcelsiorDB)..WFT_Trust_Termination WTrustTerm
INNER JOIN $(ExcelsiorDB)..CLIENT CLNT
	ON WTrustTerm.ClientID = CLNT.ClientID
INNER JOIN $(MappingDB)..TBL_TrustParticipantLookup PartLkup
	ON PartLkup.ParticipantID = WTrustTerm.participantid
	--and CLNT.BriefName=PartLkup.ClientBriefName -- Need to remove once migration issue is resolved
LEFT OUTER JOIN $(InnoTrustDB)..AccountMaster AccMstr
 ON WTrustTerm.ADVENTID = AccMstr.CustomerAccountNumber
LEFT OUTER JOIN TBL_KS_User USR
	ON WTrustTerm.modified_by = USR.UserID
--WHERE PartLkup.ParticipantID NOT IN ( -- Need to remove once migration issue is resolved
--		SELECT ParticipantID
--		FROM $(MappingDB)..TBL_ParticipantContactLookUp
--		GROUP BY ParticipantID
--		HAVING COUNT(ParticipantID) > 1
--		)

-- TBL_WFT_InvestmentDirective script
INSERT INTO TBL_WFT_InvestmentDirective (
	investment_directive_id
	,form_id
	,GUID
	,LoginManagerCodeID
	,submitted_date
	,submitted_by
	,ta_authorization_level
	,im_authorization_level
	,is_second_signature_required
	,trustee_name
	,account_name
	,CustomerAccountNumber
	,account_type
	,payout_rate_annuity_amount
	,reason_for_submission
	,comments
	,current_investment_objective
	,is_investment_per_policy
	,per_policy_investment_objective
	,is_trade_per_policy
	,trading_policy_new_trust_sell
	,trading_policy_new_trust_invest
	,trading_policy_additions_invest
	,trading_policy_additions_sell
	,is_invest_or_trade_by_exception
	,is_non_policy_investment_objective
	,non_policy_investment_objective_name
	,is_non_policy_trading_instructions
	,hold_donated_assets
	,has_other_special_instructions
	,special_instructions
	,modified_date
	,modified_user_id
	,org_userid
	,org_username
	,org_useremail
	,pxy_userid
	,pxy_username
	,pxy_useremail
	,team_folder_name
	,relationship_manager
	,client_manager
	,trader
	,dplst_authorized_signers_name
	,completed_by
	,request_date
	,first_required_signature
	,hidden_signed_investment_policies
	,second_required_signature
	)
SELECT investment_directive_id
	,form_id
	,GUID
	,login_clientid
	,submitted_date
	,submitted_by
	,ta_authorization_level
	,im_authorization_level
	,is_second_signature_required
	,trustee_name
	,account_name
	,advent_id
	,AccMstr.AccountTypeCode
	,payout_rate_annuity_amount
	,reason_for_submission
	,comments
	,current_investment_objective
	,is_investment_per_policy
	,per_policy_investment_objective
	,is_trade_per_policy
	,trading_policy_new_trust_sell
	,trading_policy_new_trust_invest
	,trading_policy_additions_invest
	,trading_policy_additions_sell
	,is_invest_or_trade_by_exception
	,is_non_policy_investment_objective
	,non_policy_investment_objective_name
	,is_non_policy_trading_instructions
	,hold_donated_assets
	,has_other_special_instructions
	,special_instructions
	,modified_date
	,ISNULL(USR.UserID, 1)
	,org_userid
	,org_username
	,org_useremail
	,pxy_userid
	,pxy_username
	,pxy_useremail
	,team_folder_name
	,relationship_manager
	,client_manager
	,trader
	,dplst_authorized_signers_name
	,completed_by
	,request_date
	,first_required_signature
	,hidden_signed_investment_policies
	,second_required_signature
FROM $(ExcelsiorDB)..WFT_Investment_Directive InvDir
LEFT OUTER JOIN $(InnoTrustDB)..AccountMaster AccMstr
 ON InvDir.ADVENT_ID = AccMstr.CustomerAccountNumber
LEFT OUTER JOIN TBL_KS_User USR
	ON InvDir.modified_user_id = USR.UserID

--TBL_WFT_EmailRule Migration Script
SET IDENTITY_INSERT TBL_WFT_EmailRule ON

INSERT INTO dbo.TBL_WFT_EmailRule (
	email_rule_id
	,email_rule_name
	,email_rule_description
	,FormID
	,new_or_modified
	,execution_order
	,database_name
	,stored_procedure_name
	,email_template_rule_id
	,send_to
	,from_email_address
	,reply_to_email_address
	,bounce_back_email_address
	,to_email_address
	,cc_email_address
	,bcc_email_address
	,is_active
	)
SELECT email_rule_id
	,email_rule_name
	,email_rule_description
	,FormID
	,new_or_modified
	,execution_order
	,database_name
	,stored_procedure_name
	,email_template_rule_id
	,send_to
	,from_email_address
	,reply_to_email_address
	,bounce_back_email_address
	,to_email_address
	,cc_email_address
	,bcc_email_address
	,is_active
FROM $(ExcelsiorDB)..WFT_EmailRule

SET IDENTITY_INSERT TBL_WFT_EmailRule OFF
--TBL_WFT_EmailRuleAttribute Migration Script
SET IDENTITY_INSERT TBL_WFT_EmailRuleAttribute ON

INSERT INTO dbo.TBL_WFT_EmailRuleAttribute (
	email_rule_attribute_id
	,email_rule_id
	,attribute_name
	,expected_value
	,evaluation_order
	,operation_type
	,DataType
	)
SELECT email_rule_attribute_id
	,email_rule_id
	,attribute_name
	,expected_value
	,evaluation_order
	,operation_type
	,datatype
FROM $(ExcelsiorDB)..WFT_EmailRuleAttribute

SET IDENTITY_INSERT TBL_WFT_EmailRuleAttribute OFF

--TBL_WFT_FormAuthorizationType Migration Script	
INSERT INTO dbo.TBL_WFT_FormAuthorizationType (
	FormID
	,AuthorizationType
	)
SELECT FormID
	,AuthorizationType
FROM $(ExcelsiorDB)..WFT_Form_Authorization_Type

--TBL_WFT_FormFlow Migration Script
SET IDENTITY_INSERT TBL_WFT_FormFlow ON

INSERT INTO dbo.TBL_WFT_FormFlow (
	form_flow_id
	,FormID
	,flow_id
	,flow_number
	)
SELECT form_flow_id
	,FormID
	,flow_id
	,flow_number
FROM $(ExcelsiorDB)..WFT_Form_Flow

SET IDENTITY_INSERT TBL_WFT_FormFlow OFF
--TBL_WFT_FormFlowStage Migration Script
SET IDENTITY_INSERT TBL_WFT_FormFlowStage ON

INSERT INTO dbo.TBL_WFT_FormFlowStage (
	form_flow_stage_id
	,form_flow_id
	,flow_status_id
	,processing_team_id
	,status_display_sequence
	)
SELECT form_flow_stage_id
	,form_flow_id
	,flow_status_id
	,processing_team_id
	,status_display_sequence
FROM $(ExcelsiorDB)..WFT_Form_Flow_Stages

SET IDENTITY_INSERT TBL_WFT_FormFlowStage OFF

--TBL_WFT_FormFlowStageNextStage Migration Script	
INSERT INTO dbo.TBL_WFT_FormFlowStageNextStage (
	form_flow_stage_id
	,flow_status_id
	)
SELECT form_flow_stage_id
	,flow_status_id
FROM $(ExcelsiorDB)..WFT_Form_Flow_Stages_Next_Stage

--TBL_WFT_FormFlowStageUserRestriction Migration Script
INSERT INTO dbo.TBL_WFT_FormFlowStageUserRestriction (
	form_flow_stage_id
	,restricted_flow_status_id
	)
SELECT form_flow_stage_id
	,restricted_flow_status_id
FROM $(ExcelsiorDB)..WFT_Form_Flow_Stages_User_Restrictions

-- TBL_WFT_ML_FrmCtrlExtn script
INSERT INTO TBL_WFT_ML_FrmCtrlExtn (
	FormID
	,form_abbrev
	,number_of_flows
	,initial_status_id
	,initial_status_flow2_id
	,initial_status_flow3_id
	)
SELECT FormID
	,form_abbrev
	,number_of_flows
	,initial_status_id
	,initial_status_flow2_id
	,initial_status_flow3_id
FROM $(ExcelsiorDB)..WFT_ML_FRMCTRL_Extn

-- TBL_WFT_ProcessingTeamUser script
INSERT INTO TBL_WFT_ProcessingTeamUser (
	processing_team_id
	,[USER_ID]
	)
SELECT processing_team_id
	,[USER_ID]
FROM $(ExcelsiorDB)..WFT_Processing_Team_Users

-- TBL_WFT_Request script
SET IDENTITY_INSERT dbo.TBL_WFT_Request ON

INSERT INTO TBL_WFT_Request (
	request_id
	,GUID
	,FormID
	,ManagerCode
	,CustomerAccountNumber
	,ContactID
	,ContactName
	,submitted_employee_id
	,submitted_datetime
	,request_status_id
	,status_datetime
	,status_user_id
	,im_authorization_type
	,ta_authorization_type
	,is_authorized
	,workflow_form_url
	,priority_flag
	,Auto_Populate_Excelsior
	,SubmittedBy_UserName
	,SubmittedBy_email
	,Auto_Populate_date
	,TagFolderLocation
	)
SELECT request_id
	,GUID
	,FormID
	,CLNT.BriefName
	,ACC.AdventID
	,PartLkup.CONTACTID
	,participant_name
	,submitted_employee_id
	,submitted_datetime
	,request_status_id
	,status_datetime
	,status_user_id
	,im_authorization_type
	,ta_authorization_type
	,is_authorized
	,workflow_form_url
	,priority_flag
	,Auto_Populate_Excelsior
	,SubmittedBy_UserName
	,SubmittedBy_email
	,Auto_Populate_date
	,TagFolderLocation
FROM $(ExcelsiorDB)..WFT_Request WRequest
INNER JOIN $(ExcelsiorDB)..CLIENT CLNT
	ON WRequest.clientid = CLNT.ClientID
LEFT OUTER JOIN $(ExcelsiorDB)..DeferredGiftAccount ACC
	ON WRequest.AccountID = ACC.AccountID
LEFT OUTER JOIN $(MappingDB)..TBL_TrustParticipantLookup PartLkup
	ON PartLkup.ParticipantID = WRequest.ParticipantID
	and CLNT.BriefName=PartLkup.ClientBriefName -- Need to remove once migration issue is resolved
--WHERE PartLkup.ParticipantID NOT IN ( -- Need to remove once migration issue is resolved
--		SELECT ParticipantID
--		FROM $(MappingDB)..TBL_ParticipantContactLookUp
--		GROUP BY ParticipantID
--		HAVING COUNT(ParticipantID) > 1
--		)
		
		
SET IDENTITY_INSERT dbo.TBL_WFT_Request OFF
-- TBL_WFT_RequestFlow script
SET IDENTITY_INSERT dbo.TBL_WFT_RequestFlow ON

INSERT INTO TBL_WFT_RequestFlow (
	request_flow_id
	,request_id
	,form_flow_id
	,flow_status_id
	,flow_status_datetime
	,flow_status_user_id
	,flow_status_comment
	)
SELECT request_flow_id
	,RqstFlw.request_id
	,form_flow_id
	,flow_status_id
	,flow_status_datetime
	,flow_status_user_id
	,flow_status_comment
FROM $(ExcelsiorDB)..WFT_Request_Flow RqstFlw
INNER JOIN TBL_WFT_Request WReq
	On WReq.request_id=RqstFlw.request_id

SET IDENTITY_INSERT dbo.TBL_WFT_RequestFlow OFF
-- TBL_WFT_RequestFlowAudit script
SET IDENTITY_INSERT dbo.TBL_WFT_RequestFlowAudit ON

INSERT INTO TBL_WFT_RequestFlowAudit (
	request_flow_audit_id
	,request_flow_id
	,from_flow_status_id
	,to_flow_status_id
	,audit_datetime
	,audit_user_id
	,flow_status_comment
	)
SELECT request_flow_audit_id
	,WFlwAdt.request_flow_id
	,from_flow_status_id
	,to_flow_status_id
	,audit_datetime
	,audit_user_id
	,WFlwAdt.flow_status_comment
FROM $(ExcelsiorDB)..WFT_Request_Flow_Audit WFlwAdt
INNER JOIN TBL_WFT_RequestFlow WReqFlw
	On WReqFlw.request_flow_id=WFlwAdt.request_flow_id

SET IDENTITY_INSERT dbo.TBL_WFT_RequestFlowAudit OFF



Declare @DBName Varchar(50)

SELECT @DBName = DB_NAME()

UPDATE TBL_WFT_EmailRule SET DATABASE_NAME = @DBName, STORED_PROCEDURE_NAME = 'USP_WFT_GetEmailRuleEngineAttributeDetails'
UPDATE TBL_EML_EmailTemplateRule SET DATABASE_NAME = @DBName, STORED_PROCEDURE_NAME = 'USP_WFT_GetEmailRuleEngineTemplateAttributeDetails'


 PRINT 'Due to multiple client association some of the participants are filtered during migration in tables  WFT_Bank_Payment_Change and WFT_Request'
