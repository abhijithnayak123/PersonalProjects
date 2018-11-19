SET NOCOUNT ON

DECLARE @ApplicationID INT

-- Paragon key migration script
SELECT @ApplicationID = ApplicationID
FROM TBL_KS_Application
WHERE ApplicationName = 'Payments'

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'TrustHasMaxPayoutRate'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'TrustHasMaxPayoutRate'
		,'^(CRUT|NICT|NIMU|GLUT|NLUT)$'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'AnnuityTrust'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'AnnuityTrust'
		,'^(CRAT|GLAT|NLAT)$'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'FlatTrust'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'FlatTrust'
		,'^(CRUT|GLUT|NLUT)$'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'PooledTrust'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'PooledTrust'
		,'^(PIF|GAP|GAPR|GAPP)$'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'OtherTrust'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'OtherTrust'
		,'^(CRUT|GLUT|NLUT|CRAT|GLAT|NLAT|PIF|GAP|GAPR|GAPP)$'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'PayNetIncomeTrust'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'PayNetIncomeTrust'
		,'^(FDN|NIMU|NICT|PR69|IRRV|GIRV|GREV|BRCT|ASST|TMCF)$'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'ProratedFinalPaymentTrusts'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'ProratedFinalPaymentTrusts'
		,'^(CRAT|GLAT|NLAT|CRUT|GLUT|NLUT)$'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'Obsolete'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'Obsolete'
		,'^(1041|D-D FUND|T1041)$'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'NonStandardExcluded'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'NonStandardExcluded'
		,'^(CORP|DAF|QPE|EST|CLR|GENL|END|RET|NQP|ENDQ|SIMP|ROTH|IRA|RET|SEP|PSP|PTA|PSHIP)$'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'NonStandardIncluded'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'NonStandardIncluded'
		,'^(TMCF|GAP|PR69|FDN|END|ENDQ|ENDT|GREV|PR69|ASST|BRCT|TMCF)$'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'NonTrustTrustTypes'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'NonTrustTrustTypes'
		,'^(IRRV|GIRV)$'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'FMVAccountTypes'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'FMVAccountTypes'
		,'^(FDN|CRAT|CRUT|NIMU|NICT|GREV|GLAT|NLAT|GLUT|NLUT|IRRV|GIRV|PR69|ASST|BRCT|TMCF)$'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'MutualFundSecurityTypeCode'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'MutualFundSecurityTypeCode'
		,'^(3200|3400|3500|3550|3600|3650|9100)$'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'PartialRelinquishmentTaxCode'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'PartialRelinquishmentTaxCode'
		,'^(633|634)$'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'PartialRelinquishmentTransCode'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'PartialRelinquishmentTransCode'
		,'^(ACHDSB|CASHDSP|CHECKDSB|DFP|JNLDSB|WIREDSB)$'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'GiftTransCode'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'GiftTransCode'
		,'^(ACHDEP|CASHDEP|CHECKDEP|RFP|WIREDEP)$'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'GiftTaxCode'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'GiftTaxCode'
		,'207,208,209,213,214,215,633,634'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'GrossIncome'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'GrossIncome'
		,'^(AISELL|DIV|DIVRE|INT|INTCD|INTOTHER|LOANINT|MBSINT|REINT)$'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'GrossIncomeAdjustments'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'GrossIncomeAdjustments'
		,'^(AIPURCH|DIVADJ|INTADJ)$'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'GrossIncomeExpenses'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'GrossIncomeExpenses'
		,'^(EXPENSE|FEEMISC|FEEPDACT)$'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'GrossIncomeExpAdj'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'GrossIncomeExpAdj'
		,'^(EXPNSAJ|FEEMADJ|FEEPDAJ)$'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'GrossIncomeDisbursements'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'GrossIncomeDisbursements'
		,'^(ACHDSB|CASHDSP|CHECKDSB|CSHDEPAJ|JNLDSB|WIREDSB|XFRFMI|XFRFMIA|XFRFMIAB|XFRFMIAM|XFRFMIB|XFRFMIM)$'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'GrossIncomeDeposits'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'GrossIncomeDeposits'
		,'^(ACHDEP|CASHDEP|CHECKDEP|CSHDSBAJ|WIREDEP|XFRTOI|XFRTOIA|XFRTOIAB|XFRTOIAM|XFRTOIB|XFRTOIM)$'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'GrossSGDDistributions'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'GrossSGDDistributions'
		,'^(STGAIN|STGAINRE)$'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'GrossSGDDistAdjustment'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'GrossSGDDistAdjustment'
		,'^(STGNADJ)$'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'GrossLGDDistributions'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'GrossLGDDistributions'
		,'^(LTGAIN|LTGAINRE)$'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'GrossLGDDistAdjustment'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'GrossLGDDistAdjustment'
		,'^(LTGNADJ)$'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'SecurityTypeCodeMF'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'SecurityTypeCodeMF'
		,'^(3200|3400|3500|3550|3600|3650|9100|3700|3750|3800|3850|3900)$'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'BalanceType'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'BalanceType'
		,'I,P'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'Status'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'Status'
		,'AUTH,SETT'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'TRUSTTYPE_FLAT'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'TRUSTTYPE_FLAT'
		,'CRUT'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'TRUSTTYPE_MAKEUP'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'TRUSTTYPE_MAKEUP'
		,'NIMU'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'GiftedPropertyFMVTrans'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'GiftedPropertyFMVTrans'
		,'^(DFP)$'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'GiftTaxCodeRegEx'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'GiftTaxCodeRegEx'
		,'^(207|208|209|213|214|215)$'
		)

SET @ApplicationID = 0

------------------------------------------------------------------------------
-- Peva key migration script
SELECT @ApplicationID = ApplicationID
FROM TBL_KS_Application
WHERE ApplicationName = 'Period End Valuation Automation'

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'SeveranceTaxCodeRegEx'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'SeveranceTaxCodeRegEx'
		,'^(631|632|633|634)$'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'SeveranceTransactionCodeRegEx'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'SeveranceTransactionCodeRegEx'
		,'^(ACHDEP|CASHDEP|CHECKDEP|RFP|WIREDEP)$'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'GiftTransactionCodeRegEx'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'GiftTransactionCodeRegEx'
		,'^(ACHDEP|CASHDEP|CHECKDEP|RFP|WIREDEP)$'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'GiftTaxCodeRegEx'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'GiftTaxCodeRegEx'
		,'^(207|208|209|213|214|215)$'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'SeveranceTaxCodeString'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'SeveranceTaxCodeString'
		,'631,632,633,634'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'SeveranceTransactionCodeString'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'SeveranceTransactionCodeString'
		,'ACHDEP,CASHDEP,CHECKDEP,RFP,WIREDEP'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'GiftTransactionCodeString'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'GiftTransactionCodeString'
		,'ACHDEP,CASHDEP,CHECKDEP,RFP,WIREDEP'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'GiftTaxCodeString'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'GiftTaxCodeString'
		,'207,208,209,213,214,215'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'BenePayDisTransactionCode'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'BenePayDisTransactionCode'
		,'ACHDSB,CHECKDSB,CSHDSBAJ,JNLDSB,WIREDSB'
		)

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_ApplicationInnoTrustKey
		WHERE [ApplicationID] = @ApplicationID
			AND [Key] = 'BenePayDisTaxCode'
		)
	INSERT INTO TBL_KS_ApplicationInnoTrustKey (
		[ApplicationID]
		,[Key]
		,[Value]
		)
	VALUES (
		@ApplicationID
		,'BenePayDisTaxCode'
		,'570,595,596,601,602,691,697,698'
		)
