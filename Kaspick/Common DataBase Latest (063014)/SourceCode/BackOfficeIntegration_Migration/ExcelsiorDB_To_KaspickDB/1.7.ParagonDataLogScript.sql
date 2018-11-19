SET NOCOUNT ON

-- Data log for TBL_PP_AccountPayoutSchedule
INSERT INTO $(MappingDB)..TBL_DataLog (
	TableName
	,ColumnName
	,ColumnID
	,Comment
	)
SELECT 'TBL_PP_AccountPayoutSchedule'
	,'APScheduleID'
	,EAccPyot.APScheduleID
	,'Missing/not available accounts'
FROM $(ExcelsiorDB)..AccountPayoutSchedule EAccPyot
LEFT OUTER JOIN TBL_PP_AccountPayoutSchedule KAccPyot
	ON EAccPyot.APScheduleID = KAccPyot.APScheduleID
WHERE KAccPyot.APScheduleID IS NULL

-- Data log for TBL_PP_BeneficiaryPayoutSchedule
INSERT INTO $(MappingDB)..TBL_DataLog (
	TableName
	,ColumnName
	,ColumnID
	,Comment
	)
SELECT 'TBL_PP_BeneficiaryPayoutSchedule'
	,'BPScheduleID'
	,EBenPyot.BPScheduleID
	,'Missing/not available BeneficiaryID mapping'
FROM $(ExcelsiorDB)..BenPayoutSchedule EBenPyot
LEFT OUTER JOIN TBL_PP_BeneficiaryPayoutSchedule KBenPyot
	ON EBenPyot.BPScheduleID = KBenPyot.BPScheduleID
WHERE KBenPyot.BPScheduleID IS NULL

-- Data log for TBL_PP_PaymentCondition
INSERT INTO $(MappingDB)..TBL_DataLog (
	TableName
	,ColumnName
	,ColumnID
	,Comment
	)
SELECT 'TBL_PP_PaymentCondition'
	,'ConditionID'
	,EPymtCond.Condition_ID
	,'Missing/not available Beneficiary mapping'
FROM $(ExcelsiorDB)..TBL_EIS_EX_PAYMENT_CONDITIONS EPymtCond
LEFT OUTER JOIN TBL_PP_PaymentCondition KPymtCond
	ON EPymtCond.Condition_ID = KPymtCond.ConditionID
WHERE KPymtCond.ConditionID IS NULL

-- Data log for TBL_PP_PGCalcPaymentData
INSERT INTO $(MappingDB)..TBL_DataLog (
	TableName
	,ColumnName
	,ColumnID
	,Comment
	)
SELECT 'TBL_PP_PGCalcPaymentData'
	,'PGCalcPaymentID'
	,EPgcalc.PGCalcPaymentID
	,'Missing/not available BeneficiaryID mapping'
FROM $(ExcelsiorDB)..PGCalcPaymentData EPgcalc
LEFT OUTER JOIN TBL_PP_PGCalcPaymentData KPgcalc
	ON EPgcalc.PGCalcPaymentID = KPgcalc.PGCalcPaymentID
WHERE KPgcalc.PGCalcPaymentID IS NULL

-- Data log for TBL_PP_PGCalcPaymentData
INSERT INTO $(MappingDB)..TBL_DataLog (
	TableName
	,ColumnName
	,ColumnID
	,Comment
	)
SELECT 'TBL_PP_PGCalcPaymentDataMerged'
	,'MergedPGCalcPaymentID'
	,EPgcalcMrg.PGCalcPaymentID
	,'Missing/not available BeneficiaryID mapping'
FROM $(ExcelsiorDB)..TBL_EIS_PP_PGCalcPaymentData_Merged EPgcalcMrg
LEFT OUTER JOIN TBL_PP_PGCalcPaymentDataMerged KPgcalcMrg
	ON EPgcalcMrg.PGCalcPaymentID = KPgcalcMrg.MergedPGCalcPaymentID
WHERE KPgcalcMrg.MergedPGCalcPaymentID IS NULL

-- Data log for TBL_PP_PGCalcPaymentDataRelation
INSERT INTO $(MappingDB)..TBL_DataLog (
	TableName
	,ColumnName
	,ColumnID
	,Comment
	)
SELECT 'TBL_PP_PGCalcPaymentDataRelation'
	,'PGCalcPaymentID, MergedPGCalcPaymentID'
	,cast(EPgcalcRlt.PGCalcPaymentID AS VARCHAR(10)) + ',' + cast(EPgcalcRlt.MergedPGCalcPaymentID AS VARCHAR(10))
	,'Missing/not available BeneficiaryID mapping'
FROM $(ExcelsiorDB)..TBL_EIS_PP_PGCalcPaymentData_Relation EPgcalcRlt
LEFT OUTER JOIN TBL_PP_PGCalcPaymentDataRelation KPgcalcRlt
	ON EPgcalcRlt.PGCalcPaymentID = KPgcalcRlt.PGCalcPaymentID
		AND EPgcalcRlt.MergedPGCalcPaymentID = KPgcalcRlt.MergedPGCalcPaymentID
WHERE KPgcalcRlt.PGCalcPaymentID IS NULL
	AND KPgcalcRlt.MergedPGCalcPaymentID IS NULL

-- Data log for TBL_PP_AnnualAccountPayoutInfo
INSERT INTO $(MappingDB)..TBL_DataLog (
	TableName
	,ColumnName
	,ColumnID
	,Comment
	)
SELECT 'TBL_PP_AnnualAccountPayoutInfo'
	,'CustomerAccountNumber, PayoutYear'
	,cast(EAccPyot.AccountID AS VARCHAR(10)) + ',' + cast(EAccPyot.PayoutYear AS VARCHAR(6))
	,'Captured AccountID,PayoutYear'
FROM $(ExcelsiorDB)..AnnualAccountPayoutInfo EAccPyot
LEFT OUTER JOIN $(ExcelsiorDB)..DEFERREDGIFTACCOUNT DGA
	ON DGA.AccountID = EAccPyot.AccountID
LEFT OUTER JOIN TBL_PP_AnnualAccountPayoutInfo KAccPyot
	ON DGA.AdventID = KAccPyot.CustomerAccountNUmber
		AND EAccPyot.PayoutYear = KAccPyot.PayoutYear
WHERE KAccPyot.CustomerAccountNumber IS NULL
	AND KAccPyot.PayoutYear IS NULL

-- Data log for TBL_PP_BeneficiaryPayment
INSERT INTO $(MappingDB)..TBL_DataLog (
	TableName
	,ColumnName
	,ColumnID
	,Comment
	)
SELECT 'TBL_PP_BeneficiaryPayment'
	,'PaymentID'
	,EBenPymt.PaymentID
	,'Missing/not available BeneficiaryID mapping'
FROM $(ExcelsiorDB)..BenPayment EBenPymt
LEFT OUTER JOIN TBL_PP_BeneficiaryPayment KBenPymt
	ON EBenPymt.PaymentID = KBenPymt.PaymentID
WHERE KBenPymt.PaymentID IS NULL



