SET NOCOUNT ON

-- TBL_IE_Status
INSERT INTO TBL_IE_Status (
	StatusID
	,STATUS
	,Description
	,CreatedDate
	)
SELECT StatusID
	,STATUS
	,Description
	,CreatedDate
FROM $(ExcelsiorDB)..TBL_EIS_IE_Status

-- TBL_IE_Group

SET IDENTITY_INSERT TBL_IE_Group ON

INSERT INTO TBL_IE_Group (
	GroupID
	,GroupName
	,TaxYear
	,IsOfficial
	,Description
	,IsDeleted
	,UserID
	,CreatedDate
	,ModifiedDate
	)
SELECT GroupID
	,GroupName
	,TaxYear
	,IsOfficial
	,Description
	,IsDeleted
	,UserID
	,CreatedDate
	,ModifiedDate
FROM $(ExcelsiorDB)..TBL_EIS_IE_Groups

SET IDENTITY_INSERT TBL_IE_Group OFF
-- TBL_IE_SecurityReclass

SET IDENTITY_INSERT TBL_IE_SecurityReclass ON

INSERT INTO TBL_IE_SecurityReclass (
	SecurityID
	,SecurityType
	,SecuritySymbol
	,CUSIP
	,ReclassPercentage
	,UserID
	,CreatedDate
	,ModifiedDate
	)
SELECT SecurityID
	,SecurityType
	,SecuritySymbol
	,'Cusip' AS CUSIP
	,ReclassPercentage
	,UserID
	,CreatedDate
	,ModifiedDate
FROM $(ExcelsiorDB)..TBL_EIS_IE_Security_Reclass SecRcls


SET IDENTITY_INSERT TBL_IE_SecurityReclass OFF
-- TBL_IE_ApplicationSetting
SET IDENTITY_INSERT TBL_IE_ApplicationSetting ON

INSERT INTO TBL_IE_ApplicationSetting (
	SettingID
	,SettingField
	,SettingValue
	,CreatedDate
	)
SELECT SettingID
	,SettingField
	,SettingValue
	,CreatedDate
FROM $(ExcelsiorDB)..TBL_EIS_IE_App_Settings

SET IDENTITY_INSERT TBL_IE_ApplicationSetting OFF
-- TBL_IE_STG_Estimate
SET IDENTITY_INSERT TBL_IE_STG_Estimate ON

INSERT INTO TBL_IE_STG_Estimate (
	StagingID
	,GroupID
	,TaxYear
	,CustomerAccountNumber
	,ManagerCode
	,AccountName
	,AccountType
	,CreationDate
	,MatureDate
	,RunDate
	,IsExcelsiorImported
	,StatusID
	,UnitrustPercentage
	,ObjectiveCode
	,IsDeleted
	,UserID
	,UserName
	,CreatedDate
	,ModifiedDate
	,ImportDate
	,DiagnosticError
	,IT_RequiresIncomeEstimate
	,IT_IsIncomeAccrual
	,IT_IsAmortization
	,IT_IsAccretion
	,IT_IsRentalIncomeExpenses
	,IT_IsMiscExpenses
	,IT_IsPropertyTaxIncome
	,IT_IsPromNoteAmort
	,IT_PayCapGainsAsIncome
	,EP_InvestPerPolicy
	,EP_InvestmentType
	,EP_FirstTradeDate
	,EP_TranchingStatus
	,KDB_HoldsSubstitutes
	,KDB_HoldsRestrictedAssets
	,IT_TerminationRule
	,IT_FlipProvision
	,IT_ExpectedFlipDate
	,Ca_TrustLifeExpectancy
	,IT_PayActualNI
	,IE_CurrentYearIncomeTarget
	,IE_PriorYearIncomeTarget
	,EP_InvestmentComment
	,IT_PaysSTGDAsIncome
	,IT_CalculationMethod
	,IT_ClientLegalName
	,PP_ValuationPriorYear1
	,PP_ValuationPriorYear2
	,PP_ValuationPriorYear3
	,PP_Valuation
	,Ex_ValuationImportDate
	,PP_OverPayment
	,PP_UnitrustAmount
	,PP_CumulativeDeficiency
	,PP_ValuationStatus
	,PP_InvestmentAllocationYield
	,IT_NewGiftsAndAdditions
	,PP_Q1_Q4PaymentsPriorYear1
	,PP_Q1_Q4PaymentsPriorYear2
	,PP_Q1_Q4PaymentsPriorYear3
	,PP_Q5PaymentsPriorYear1
	,PP_Q5PaymentsPriorYear2
	,PP_Q5PaymentsPriorYear3
	,PP_TotalPaymentsPriorYear1
	,PP_TotalPaymentsPriorYear2
	,PP_TotalPaymentsPriorYear3
	,IE_DisplayOrder
	,PP_TrustYieldPriorYear1
	,PP_TrustYieldPriorYear2
	,PP_TrustYieldPriorYear3
	,PP_PriorYearPayout
	,PP_PriorYearValuation
	,PP_PriorYearYield
	,EP_ClientIEComment
	,EP_MasterObjectiveName
	,PP_ScheduledPaymentAmount
	,Ex_FlipTriggerDate
	,Ax_EstimatedIncome
	,Ax_YTDOtherEx
	,Ax_PriorYearAccruedIncome
	,Ax_ReClassIncome
	,Ax_ActualSGD
	,Ax_ActIncStd
	,Ax_ActIncNonStd
	,Ax_EarnedIncome
	,Ax_FMVOnStartDate
	,Ax_FMVOnRunDate
	,Ax_GrossIncomeEstimate
	,Ax_AxysPortYield
	,Ax_AxysPortGrossYieldVar
	,Ax_GrossIncomeYieldOnStartDate
	,Ax_GrossIncomeYieldOnRunDate
	,Ax_NetIncomeYieldOnStartDate
	,IT_AnnIMFee_Income
	,IT_AnnTAFee_Income
	,IT_AnnTrusteeFee_Income
	,IT_YTDIMFee_Income
	,IT_YTDTAFee_Income
	,IT_YTDTrusteeFee_Income
	,IT_ProjectedIMFee_Income
	,IT_ProjectedTAFee_Income
	,IT_ProjectedTrusteeFee_Income
	,Projectedannualchg_income
	,Ax_TMCAnnualChg_Income
	,Ax_EstimatedSGD
	,Ca_InvAllocGrossYieldVar
	,Ca_InvAllocNetYieldVar
	,Ca_NewNetIncomeEstimate
	,Ca_NetIncomeEstimateUsedForPmt
	,Ca_NetIncomeProjectionOriginal
	,Ca_PmtYield
	,Ca_NetPriorYearQ1_Q4Var
	,Ca_NetPriorYearTotalVar
	,Ca_NetMidYearVar
	,EstimatedOtherExpenses
	,CurrentYearAccruedIncome
	,ActualSTGLTG
	,AssetAmortAccret
	,IPMIncomeOverride
	,RMIncomeOverride
	,CMIncomeOverride
	,ParentStagingID
	,IncomeOverrideStatus
	)
SELECT DISTINCT StagingID
	,GroupID
	,TaxYear
	,AdventID
	,BriefName
	,AccountName
	,AccMst.AccountTypeCode
	,CreationDate
	,MatureDate
	,RunDate
	,IsExcelsiorImported
	,StatusID
	,UnitrustPercentage
	,ObjectiveCode
	,IsDeleted
	,UserID
	,UserName
	,CreatedDate
	,ModifiedDate
	,ImportDate
	,DiagnosticError
	,Ex_RequiresIncomeEstimate
	,Ex_IsIncomeAccrual
	,Ex_IsAmortization
	,Ex_IsAccretion
	,Ex_IsRentalIncomeExpenses
	,Ex_IsMiscExpenses
	,Ex_IsPropertyTaxIncome
	,Ex_IsPromNoteAmort
	,Ex_PayCapGainsAsIncome
	,Ex_InvestPerPolicy
	,Ex_InvestmentType
	,Ex_FirstTradeDate
	,Ex_TranchingStatus
	,Ex_HoldsSubstitutes
	,Ex_HoldsRestrictedAssets
	,Ex_TerminationRule
	,Ex_FlipProvision
	,Ex_ExpectedFlipDate
	,Ex_TrustLifeExpectancy
	,Ex_PayActualNI
	,Ex_CurrentYearIncomeTarget
	,Ex_PriorYearIncomeTarget
	,Ex_InvestmentComment
	,Ex_PaysSTGDAsIncome
	,Ex_CalculationMethod
	,Ex_ClientLegalName
	,Ex_ValuationPriorYear1
	,Ex_ValuationPriorYear2
	,Ex_ValuationPriorYear3
	,Ex_Valuation
	,Ex_ValuationImportDate
	,Ex_OverPayment
	,Ex_UnitrustAmount
	,Ex_CumulativeDeficiency
	,Ex_ValuationStatus
	,Ex_InvestmentAllocationYield
	,Ex_NewGiftsAndAdditions
	,Ex_Q1_Q4PaymentsPriorYear1
	,Ex_Q1_Q4PaymentsPriorYear2
	,Ex_Q1_Q4PaymentsPriorYear3
	,Ex_Q5PaymentsPriorYear1
	,Ex_Q5PaymentsPriorYear2
	,Ex_Q5PaymentsPriorYear3
	,Ex_TotalPaymentsPriorYear1
	,Ex_TotalPaymentsPriorYear2
	,Ex_TotalPaymentsPriorYear3
	,Ex_DisplayOrder
	,Ex_TrustYieldPriorYear1
	,Ex_TrustYieldPriorYear2
	,Ex_TrustYieldPriorYear3
	,Ex_PriorYearPayout
	,Ex_PriorYearValuation
	,Ex_PriorYearYield
	,Ex_ClientIEComment
	,Ex_MasterObjectiveName
	,Ex_ScheduledPaymentAmount
	,Ex_FlipTriggerDate
	,Ax_EstimatedIncome
	,Ax_YTDOtherEx
	,Ax_PriorYearAccruedIncome
	,Ax_ReClassIncome
	,Ax_ActualSGD
	,Ax_ActIncStd
	,Ax_ActIncNonStd
	,Ax_EarnedIncome
	,Ax_FMVOnStartDate
	,Ax_FMVOnRunDate
	,Ax_GrossIncomeEstimate
	,Ax_AxysPortYield
	,Ax_AxysPortGrossYieldVar
	,Ax_GrossIncomeYieldOnStartDate
	,Ax_GrossIncomeYieldOnRunDate
	,Ax_NetIncomeYieldOnStartDate
	,Ax_AnnIMFee_Income
	,Ax_AnnTAFee_Income
	,Ax_AnnTrusteeFee_Income
	,NULL AS IT_YTDIMFee_Income
	,NULL AS IT_YTDTAFee_Income
	,NULL AS IT_YTDTrusteeFee_Income
	,NULL AS IT_ProjectedIMFee_Income
	,NULL AS IT_ProjectedTAFee_Income
	,NULL AS IT_ProjectedTrusteeFee_Income
	,NULL AS Projectedannualchg_income
	,Ax_TMCAnnualChg_Income
	,Ax_EstimatedSGD
	,Ca_InvAllocGrossYieldVar
	,Ca_InvAllocNetYieldVar
	,Ca_NewNetIncomeEstimate
	,Ca_NetIncomeEstimateUsedForPmt
	,Ca_NetIncomeProjectionOriginal
	,Ca_PmtYield
	,Ca_NetPriorYearQ1_Q4Var
	,Ca_NetPriorYearTotalVar
	,Ca_NetMidYearVar
	,EstimatedOtherExpenses
	,CurrentYearAccruedIncome
	,ActualSTGLTG
	,AssetAmortAccret
	,IPMIncomeOverride
	,RMIncomeOverride
	,CMIncomeOverride
	,ParentStagingID
	,IncomeOverrideStatus
FROM $(ExcelsiorDB)..TBL_EIS_IE_Estimate_Staging EstStg
INNER JOIN $(InnoTrustDB)..AccountMaster AccMst
	ON EstStg.AdventID = AccMst.CustomerAccountNumber
	
SET IDENTITY_INSERT TBL_IE_STG_Estimate OFF
-- TBL_IE_STG_EstimateDetail
SET IDENTITY_INSERT TBL_IE_STG_EstimateDetail ON

INSERT INTO TBL_IE_STG_EstimateDetail (
	DetailID
	,StagingID
	,GroupID
	,CustomerAccountNumber
	,RunDate
	,ImportDate
	,SecuritySymbol
	,CUSIP
	,Quantity
	,DividendRate
	,EstimatedIncome
	,CalcMethod
	,EstNoPymts
	,UserID
	,UserName
	)
SELECT DetailID
	,EstStgDet.StagingID
	,EstStgDet.GroupID
	,EstStgDet.AdventID
	,EstStgDet.RunDate
	,EstStgDet.ImportDate
	,SecuritySymbol
	,'Cusip' AS CUSIP
	,Quantity
	,DividendRate
	,EstimatedIncome
	,CalcMethod
	,EstNoPymts
	,EstStgDet.UserID
	,EstStgDet.UserName
FROM $(ExcelsiorDB)..TBL_EIS_IE_Estimate_Staging_Detail EstStgDet
INNER JOIN TBL_IE_STG_Estimate Est
	On Est.StagingID = EstStgDet.StagingID


SET IDENTITY_INSERT TBL_IE_STG_EstimateDetail OFF

-- TBL_IE_STG_Holding
SET IDENTITY_INSERT TBL_IE_STG_Holding ON

INSERT INTO TBL_IE_STG_Holding (
	HoldingID
	,StagingID
	,GroupID
	,RunDate
	,CustomerAccountNumber
	,SecuritySymbol
	,CUSIP
	,Quantity
	,Price
	,ImportDate
	,UserID
	,UserName
	)
SELECT HoldingID
	,StagingID
	,GroupID
	,RunDate
	,AdventID
	,SecuritySymbol
	,'Cusip' AS CUSIP
	,Quantity
	,Price
	,ImportDate
	,UserID
	,UserName
FROM $(ExcelsiorDB)..TBL_EIS_IE_Stg_Holdings StgHld


SET IDENTITY_INSERT TBL_IE_STG_Holding OFF
-- TBL_IE_STG_JanOneHolding
SET IDENTITY_INSERT TBL_IE_STG_JanOneHolding ON

INSERT INTO TBL_IE_STG_JanOneHolding (
	HoldingID
	,StagingID
	,GroupID
	,RunDate
	,CustomerAccountNumber
	,SecuritySymbol
	,CUSIP
	,Quantity
	,Price
	,ImportDate
	,UserID
	,UserName
	)
SELECT HoldingID
	,StagingID
	,GroupID
	,RunDate
	,AdventID
	,SecuritySymbol
	,'Cusip' AS CUSIP
	,Quantity
	,Price
	,ImportDate
	,UserID
	,UserName
FROM $(ExcelsiorDB)..TBL_EIS_IE_Stg_JanOneHoldings JanHld


SET IDENTITY_INSERT TBL_IE_STG_JanOneHolding OFF

-- TBL_IE_STG_Lock
INSERT INTO TBL_IE_STG_Lock (
	CustomerAccountNumber
	,UserID
	,UserName
	,LockDate
	,LockType
	)
SELECT AdventID
	,UserID
	,UserName
	,LockDate
	,LockType
FROM $(ExcelsiorDB)..TBL_EIS_IE_Stg_Locks

-- TBL_IE_Diagnostic
INSERT INTO TBL_IE_Diagnostic (
	DiagnosticID
	,DiagnosticType
	,DiagnosticLevel
	,DiagnosticField
	,DiagnosticCategory
	,DiagnosticCondition
	,CreatedDate
	)
SELECT DiagnosticID
	,DiagnosticType
	,DiagnosticLevel
	,DiagnosticField
	,'Income Estimate' AS DiagnosticCategory
	,DiagnosticCondition
	,CreatedDate
FROM $(ExcelsiorDB)..TBL_EIS_IE_Diagnostics

-- TBL_IE_DiagnosticResult
SET IDENTITY_INSERT TBL_IE_DiagnosticResult ON

INSERT INTO TBL_IE_DiagnosticResult (
	DiagnosticResultID
	,DiagnosticID
	,StagingID
	,GroupID
	,CustomerAccountNumber
	,DiagnosticResult
	,SecurityType
	,SecuritySymbol
	,CUSIP
	,UserID
	,UserName
	,ModifiedDate
	,CreatedDate
	)
SELECT DiagnosticResultsID
	,DiagnosticID
	,StagingID
	,GroupID
	,AdventID
	,DiagnosticResult
	,SecurityType
	,SecuritySymbol
	,'Cusip' AS CUSIP
	,UserID
	,UserName
	,ModifiedDate
	,CreatedDate
FROM $(ExcelsiorDB)..TBL_EIS_IE_Diagnostic_Results DiagRslt


SET IDENTITY_INSERT TBL_IE_DiagnosticResult OFF
-- TBL_IE_DisplayOrder
SET IDENTITY_INSERT TBL_IE_DisplayOrder ON

INSERT INTO TBL_IE_DisplayOrder (
	DisplayOrderID
	,DisplayOrder
	,CTSCat
	,MasterObjName
	,ITC
	)
SELECT DisplayOrderID
	,DisplayOrder
	,CTSCat
	,MasterObjName
	,ITC
FROM $(ExcelsiorDB)..TBL_EIS_IE_Display_Order

SET IDENTITY_INSERT TBL_IE_DisplayOrder OFF
-- TBL_IE_ManagerStepRateFeeHistory
SET IDENTITY_INSERT TBL_IE_ManagerStepRateFeeHistory ON

INSERT INTO TBL_IE_ManagerStepRateFeeHistory (
	FeeHistoryID
	,ManagerCode
	,IMStepRate
	,EffectiveDate
	,CreatedDate
	)
SELECT FeeHistoryID
	,Clnt.BriefName
	,IMStepRate
	,EffectiveDate
	,CreatedDate
FROM $(ExcelsiorDB)..TBL_EIS_IE_Client_StepRateFeeHistory StpRteHst
INNER JOIN $(ExcelsiorDB)..Client Clnt
	ON Clnt.ClientID = StpRteHst.ClientID

SET IDENTITY_INSERT TBL_IE_ManagerStepRateFeeHistory OFF
-- TBL_IE_History
SET IDENTITY_INSERT TBL_IE_History ON

INSERT INTO TBL_IE_History (
	HistoryID
	,GroupID
	,CustomerAccountNumber
	,RunDate
	,ChangedField
	,PreviousValue
	,CurrentValue
	,Comments
	,UserID
	,UserName
	,CreatedDate
	)
SELECT HistoryID
	,GroupID
	,AdventID
	,RunDate
	,ChangedField
	,PreviousValue
	,CurrentValue
	,Comments
	,UserID
	,UserName
	,CreatedDate
FROM $(ExcelsiorDB)..TBL_EIS_IE_History

SET IDENTITY_INSERT TBL_IE_History OFF

-- TBL_IE_AssetYield
INSERT INTO TBL_IE_AssetYield (
	PayDate
	,SecuritySymbol
	,CUSIP
	,EstimatedDividend
	,ActualDividend
	,RecordDate
	,XDate
	,EstimatedSTG
	,EstimatedLTG
	,ActualSTG
	,ActualLTG
	,QualifiedDividend
	,ConfirmedRecordDate
	)
SELECT AstYld.PayDate
	,SecuritySymbol
	,'Cusip' AS CUSIP
	,EstimatedDividend
	,ActualDividend
	,AstYld.RecordDate
	,XDate
	,EstimatedSTG
	,EstimatedLTG
	,ActualSTG
	,ActualLTG
	,QualifiedDividend
	,ConfirmedRecordDate
FROM $(ExcelsiorDB)..Assetyield AstYld


-- TBL_IE_Annuity2000
INSERT INTO TBL_IE_Annuity2000 (
	AnnuityId
	,Age
	,Alive
	,ModifiedBy
	,ModifiedDate
	,CreatedBy
	,CreatedDate
	)
SELECT AnnuityId
	,Age
	,Alive
	,ModifiedBy
	,ModifiedDate
	,CreatedBy
	,CreatedDate
FROM $(ExcelsiorDB)..TBL_ANNUITY2000

-- TBL_IE_PaymentStagingGroup
INSERT INTO TBL_IE_PaymentStagingGroup (
	PaymentStagingGroupID
	,PaymentStagingGroupName
	,TaxYear
	,RunDate
	,FromDate
	,ToDate
	,IsMidYear
	,IsOfficial
	,PaymentStagingGroupDesc
	)
SELECT PaymentStagingGroupID
	,PaymentStagingGroupName
	,TaxYear
	,RunDate
	,FromDate
	,ToDate
	,IsMidYear
	,IsOfficial
	,PaymentStagingGroupDesc
FROM $(ExcelsiorDB)..PaymentStagingGroup

-- TBL_IE_State
SET IDENTITY_INSERT TBL_IE_State ON

INSERT INTO TBL_IE_State(StateID,StateName) Values(1,'Data_Error_IPM')
INSERT INTO TBL_IE_State(StateID,StateName) Values(2,'Error')
INSERT INTO TBL_IE_State(StateID,StateName) Values(3,'Error_CM_Ops')
INSERT INTO TBL_IE_State(StateID,StateName) Values(4,'Error_CM_TA')
INSERT INTO TBL_IE_State(StateID,StateName) Values(5,'Error_CM_Valuation')
INSERT INTO TBL_IE_State(StateID,StateName) Values(6,'Error_Ops')
INSERT INTO TBL_IE_State(StateID,StateName) Values(7,'Mid_Year_Complete')
INSERT INTO TBL_IE_State(StateID,StateName) Values(8,'Mid_Year_Only')
INSERT INTO TBL_IE_State(StateID,StateName) Values(9,'New')
INSERT INTO TBL_IE_State(StateID,StateName) Values(10,'PDG_CM')
INSERT INTO TBL_IE_State(StateID,StateName) Values(11,'PDG_IPM')
INSERT INTO TBL_IE_State(StateID,StateName) Values(12,'PDG_RM')
INSERT INTO TBL_IE_State(StateID,StateName) Values(13,'Ready_For_Pmt')
INSERT INTO TBL_IE_State(StateID,StateName) Values(14,'Ready_For_ReImport')
INSERT INTO TBL_IE_State(StateID,StateName) Values(15,'Ready_To_Calc')
INSERT INTO TBL_IE_State(StateID,StateName) Values(16,'Review_IPM')
INSERT INTO TBL_IE_State(StateID,StateName) Values(17,'Scheduled')

SET IDENTITY_INSERT TBL_IE_State OFF

-- TBL_IE_StateFlow

INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(1,9,8)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(2,9,16)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(3,9,1)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(4,14,16)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(5,14,1)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(6,1,3)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(7,1,4)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(8,1,6)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(9,1,16)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(10,1,15)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(11,3,16)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(12,4,16)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(13,6,16)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(14,16,3)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(15,16,4)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(16,16,6)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(17,16,14)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(18,16,15)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(19,11,7)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(20,11,14)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(21,11,4)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(22,11,6)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(23,11,12)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(24,11,15)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(25,12,11)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(26,12,10)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(27,12,15)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(28,10,12)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(29,10,11)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(30,10,13)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(31,10,7)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(32,17,9)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(33,17,16)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(34,17,15)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(35,17,11)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(36,17,12)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(37,17,10)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(38,17,1)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(39,17,3)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(40,17,4)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(41,17,6)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(42,17,14)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(43,17,7)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(44,17,13)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(45,17,2)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(46,17,5)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(47,17,8)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(48,5,11)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(49,3,16)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(50,4,16)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(51,4,6)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(52,10,7)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(53,10,12)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(54,10,11)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(55,10,13)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(56,6,16)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(57,6,4)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(58,12,10)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(59,12,11)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(60,3,16)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(61,4,16)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(62,4,6)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(63,10,7)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(64,10,12)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(65,10,11)
INSERT INTO TBL_IE_StateFlow (StateFlowID,FromStateID,ToStateID) Values(66,10,13)


-- Modules and Privileges

DECLARE @ApplicationID INT
DECLARE @PrivilegeID INT
DECLARE @MDisplaySequence INT
DECLARE @PDisplaySequence INT
DECLARE @FunctionID INT
DECLARE @MainModuleID INT
DECLARE @ModuleID INT
DECLARE @ModuleIDDataImport INT
DECLARE @ModuleIDDataReview INT
DECLARE @ModuleIDEstReview INT
DECLaRE @RoleID INT

SELECT @ApplicationID = ApplicationID
FROM TBL_KS_Application
WHERE ApplicationName = 'Income Estimation'

SELECT @MainModuleID = MainModuleID
FROM TBL_KS_MainModule
WHERE MainModuleName = 'Income Estimation'

SELECT @FunctionID = FunctionID
FROM TBL_KS_Function
WHERE FunctionName = 'Non-Standard'

SELECT @ModuleIDDataImport = ModuleID
FROM TBL_KS_Module
WHERE ModuleName = 'ModuleDataImport'

SELECT @ModuleIDDataReview = ModuleID
FROM TBL_KS_Module
WHERE ModuleName = 'ModuleDataReview'

SELECT @ModuleIDEstReview = ModuleID
FROM TBL_KS_Module
WHERE ModuleName = 'ModuleEstimateReview'

SELECT @MDisplaySequence = MAX(DisplaySequence)
FROM TBL_KS_MainModule

-- Tbl_KS_Module
Insert Into TBL_KS_Module  (ApplicationID,ParentID,ModuleName,Abbrev,Description,DisplaySequence,IsActive,MainModuleID,ModuleURL,ImageURL) 
Values ( @ApplicationID,NULL,'IERolePrivilege','IE01','',@MDisplaySequence+10,1,@MainModuleID,NULL,NULL)

SELECT @PDisplaySequence = MAX(DisplaySequence)
FROM TBL_KS_Privilege


-- Privilege : ManagerIE

-- Get RoleID
SELECT @RoleID = RoleID
FROM TBL_KS_Role
WHERE RoleName = 'ManagerIE'

-- TBL_KS_Privilege

INSERT INTO TBL_KS_Privilege (PrivilegeName,Description,DisplaySequence,ModuleID,FunctionID,GroupID) 
Values ( 'PrvStateManagerIE','Income Estimates Manager',@PDisplaySequence+10,IDENT_CURRENT('TBL_KS_Module'),@FunctionID,NULL)

-- TBL_KS_RolePrivilege
INSERT INTO TBL_KS_RolePrivilege ( RoleID,PrivilegeID )
VALUES ( @RoleID ,IDENT_CURRENT('TBL_KS_Privilege')	)

-- TBL_IE_PrivilegeStateFlow

INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),1)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),2)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),3)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),4)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),5)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),6)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),7)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),8)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),9)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),10)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),11)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),12)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),13)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),14)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),15)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),16)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),17)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),18)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),19)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),20)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),21)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),22)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),23)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),24)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),25)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),26)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),27)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),28)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),29)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),30)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),31)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),32)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),33)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),34)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),35)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),36)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),37)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),38)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),39)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),40)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),41)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),42)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),43)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),44)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),45)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),46)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),47)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),48)

-- TBL_IE_DefaultSetting

INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataReview,'STATE','All')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataImport,'STATE','Data_Import')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDEstReview,'STATE','All')


INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataReview,'MANAGERGROUP','All')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataImport,'MANAGERGROUP','All')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDEstReview,'MANAGERGROUP','All')

-- Privilege : Client Accounts

-- Get RoleID
SELECT @RoleID = RoleID
FROM TBL_KS_Role
WHERE RoleName = 'Client Accounts'

-- TBL_KS_Privilege

INSERT INTO TBL_KS_Privilege (PrivilegeName,Description,DisplaySequence,ModuleID,FunctionID,GroupID) 
Values ( 'PrvStateClientAccounts','Client Accounts',@PDisplaySequence+20,IDENT_CURRENT('TBL_KS_Module'),@FunctionID,NULL)

-- TBL_KS_RolePrivilege
INSERT INTO TBL_KS_RolePrivilege ( RoleID,PrivilegeID )
VALUES ( @RoleID ,IDENT_CURRENT('TBL_KS_Privilege')	)

-- TBL_IE_PrivilegeStateFlow
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),49)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),50)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),51)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),52)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),53)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),54)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),55)

-- TBL_IE_DefaultSetting

INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataReview,'STATE','CM_Data_Review')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataImport,'STATE','All')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDEstReview,'STATE','CM_Estimate_Review')

INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataReview,'MANAGERGROUP','My_Clients')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataImport,'MANAGERGROUP','My_Clients')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDEstReview,'MANAGERGROUP','My_Clients')


-- Privilege : Accounting & Custody

-- Get RoleID
SELECT @RoleID = RoleID
FROM TBL_KS_Role
WHERE RoleName = 'Accounting & Custody'

-- TBL_KS_Privilege

INSERT INTO TBL_KS_Privilege (PrivilegeName,Description,DisplaySequence,ModuleID,FunctionID,GroupID) 
Values ( 'PrvStateAccounting&Custody','Accounting & Custody',@PDisplaySequence+30,IDENT_CURRENT('TBL_KS_Module'),@FunctionID,NULL)

-- TBL_KS_RolePrivilege
INSERT INTO TBL_KS_RolePrivilege ( RoleID,PrivilegeID )
VALUES ( @RoleID ,IDENT_CURRENT('TBL_KS_Privilege')	)

-- TBL_IE_PrivilegeStateFlow
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),56)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),57)

-- TBL_IE_DefaultSetting

INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataReview,'STATE','Error_Ops')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataImport,'STATE','All')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDEstReview,'STATE','All')

INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataReview,'MANAGERGROUP','My_Clients')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataImport,'MANAGERGROUP','My_Clients')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDEstReview,'MANAGERGROUP','My_Clients')


-- Privilege : RM

-- Get RoleID
SELECT @RoleID = RoleID
FROM TBL_KS_Role
WHERE RoleName = 'RM'

-- TBL_KS_Privilege

INSERT INTO TBL_KS_Privilege (PrivilegeName,Description,DisplaySequence,ModuleID,FunctionID,GroupID) 
Values ( 'PrvStateRM','Relationship Manager',@PDisplaySequence+40,IDENT_CURRENT('TBL_KS_Module'),@FunctionID,NULL)

-- TBL_KS_RolePrivilege
INSERT INTO TBL_KS_RolePrivilege ( RoleID,PrivilegeID )
VALUES ( @RoleID ,IDENT_CURRENT('TBL_KS_Privilege')	)

-- TBL_IE_PrivilegeStateFlow

INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),58)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),59)


-- TBL_IE_DefaultSetting

INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataReview,'STATE','All')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataImport,'STATE','All')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDEstReview,'STATE','PDG_RM')

INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataReview,'MANAGERGROUP','My_Clients')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataImport,'MANAGERGROUP','My_Clients')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDEstReview,'MANAGERGROUP','My_Clients')

-- Privilege : IT

-- Get RoleID
SELECT @RoleID = RoleID
FROM TBL_KS_Role
WHERE RoleName = 'IT'

-- TBL_KS_Privilege

INSERT INTO TBL_KS_Privilege (PrivilegeName,Description,DisplaySequence,ModuleID,FunctionID,GroupID) 
Values ( 'PrvStateIT','Information Technology',@PDisplaySequence+50,IDENT_CURRENT('TBL_KS_Module'),@FunctionID,NULL)

-- TBL_KS_RolePrivilege
INSERT INTO TBL_KS_RolePrivilege ( RoleID,PrivilegeID )
VALUES ( @RoleID ,IDENT_CURRENT('TBL_KS_Privilege')	)

-- TBL_IE_DefaultSetting

INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataReview,'STATE','All')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataImport,'STATE','All')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDEstReview,'STATE','All')

INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataReview,'MANAGERGROUP','My_Clients')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataImport,'MANAGERGROUP','My_Clients')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDEstReview,'MANAGERGROUP','My_Clients')


-- Privilege : TaxLegal
-- Get RoleID
SELECT @RoleID = RoleID
FROM TBL_KS_Role
WHERE RoleName = 'Tax Legal'

-- TBL_KS_Privilege

INSERT INTO TBL_KS_Privilege (PrivilegeName,Description,DisplaySequence,ModuleID,FunctionID,GroupID) 
Values ( 'PrvStateTaxLegal','Tax & Legal',@PDisplaySequence+60,IDENT_CURRENT('TBL_KS_Module'),@FunctionID,NULL)

-- TBL_KS_RolePrivilege
INSERT INTO TBL_KS_RolePrivilege ( RoleID,PrivilegeID )
VALUES ( @RoleID ,IDENT_CURRENT('TBL_KS_Privilege')	)

-- TBL_IE_DefaultSetting

INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataReview,'STATE','All')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataImport,'STATE','All')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDEstReview,'STATE','All')

INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataReview,'MANAGERGROUP','My_Clients')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataImport,'MANAGERGROUP','My_Clients')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDEstReview,'MANAGERGROUP','My_Clients')

-- Privilege : Gift Operations

-- Get RoleID
SELECT @RoleID = RoleID
FROM TBL_KS_Role
WHERE RoleName = 'Gift Operations'

-- TBL_KS_Privilege

INSERT INTO TBL_KS_Privilege (PrivilegeName,Description,DisplaySequence,ModuleID,FunctionID,GroupID) 
Values ( 'PrvStateGiftOperations','Gift Operations',@PDisplaySequence+70,IDENT_CURRENT('TBL_KS_Module'),@FunctionID,NULL)

-- TBL_KS_RolePrivilege
INSERT INTO TBL_KS_RolePrivilege ( RoleID,PrivilegeID )
VALUES ( @RoleID ,IDENT_CURRENT('TBL_KS_Privilege')	)

-- TBL_IE_PrivilegeStateFlow

INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),60)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),61)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),62)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),63)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),64)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),65)
INSERT INTO TBL_IE_PrivilegeStateFlow (PrivilegeID,StateFlowID)
Values ( IDENT_CURRENT('TBL_KS_Privilege'),66)

-- TBL_IE_DefaultSetting

INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataReview,'STATE','CM_Data_Review')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataImport,'STATE','All')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDEstReview,'STATE','CM_Estimate_Review')

INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataReview,'MANAGERGROUP','My_Clients')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataImport,'MANAGERGROUP','My_Clients')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDEstReview,'MANAGERGROUP','My_Clients')


-- Privilege : Trader

-- Get RoleID
SELECT @RoleID = RoleID
FROM TBL_KS_Role
WHERE RoleName = 'Trader'

-- TBL_KS_Privilege

INSERT INTO TBL_KS_Privilege (PrivilegeName,Description,DisplaySequence,ModuleID,FunctionID,GroupID) 
Values ( 'PrvStateTrader','Portfolio Management and Trading',@PDisplaySequence+80,IDENT_CURRENT('TBL_KS_Module'),@FunctionID,NULL)

-- TBL_KS_RolePrivilege
INSERT INTO TBL_KS_RolePrivilege ( RoleID,PrivilegeID )
VALUES ( @RoleID ,IDENT_CURRENT('TBL_KS_Privilege')	)

-- TBL_IE_DefaultSetting

INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataReview,'STATE','All')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataImport,'STATE','All')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDEstReview,'STATE','All')

INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataReview,'MANAGERGROUP','My_Clients')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataImport,'MANAGERGROUP','My_Clients')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDEstReview,'MANAGERGROUP','My_Clients')

-- Privilege : Production

-- Get RoleID
SELECT @RoleID = RoleID
FROM TBL_KS_Role
WHERE RoleName = 'Production'

-- TBL_KS_Privilege

INSERT INTO TBL_KS_Privilege (PrivilegeName,Description,DisplaySequence,ModuleID,FunctionID,GroupID) 
Values ( 'PrvStateProduction','Production Services',@PDisplaySequence+90,IDENT_CURRENT('TBL_KS_Module'),@FunctionID,NULL)

-- TBL_KS_RolePrivilege
INSERT INTO TBL_KS_RolePrivilege ( RoleID,PrivilegeID )
VALUES ( @RoleID ,IDENT_CURRENT('TBL_KS_Privilege')	)

-- TBL_IE_DefaultSetting

INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataReview,'STATE','All')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataImport,'STATE','All')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDEstReview,'STATE','All')

INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataReview,'MANAGERGROUP','My_Clients')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataImport,'MANAGERGROUP','My_Clients')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDEstReview,'MANAGERGROUP','My_Clients')

-- Privilege : PFA

-- Get RoleID
SELECT @RoleID = RoleID
FROM TBL_KS_Role
WHERE RoleName = 'PFA'

-- TBL_KS_Privilege

INSERT INTO TBL_KS_Privilege (PrivilegeName,Description,DisplaySequence,ModuleID,FunctionID,GroupID) 
Values ( 'PrvStatePFA','Pooled Fund Adminstrator',@PDisplaySequence+100,IDENT_CURRENT('TBL_KS_Module'),@FunctionID,NULL)

-- TBL_KS_RolePrivilege
INSERT INTO TBL_KS_RolePrivilege ( RoleID,PrivilegeID )
VALUES ( @RoleID ,IDENT_CURRENT('TBL_KS_Privilege')	)

-- TBL_IE_DefaultSetting

INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataReview,'STATE','All')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataImport,'STATE','All')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDEstReview,'STATE','All')

INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataReview,'MANAGERGROUP','My_Clients')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataImport,'MANAGERGROUP','My_Clients')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDEstReview,'MANAGERGROUP','My_Clients')

-- Privilege : Reporting

-- Get RoleID
SELECT @RoleID = RoleID
FROM TBL_KS_Role
WHERE RoleName = 'Reporting'

-- TBL_KS_Privilege

INSERT INTO TBL_KS_Privilege (PrivilegeName,Description,DisplaySequence,ModuleID,FunctionID,GroupID) 
Values ( 'PrvStateReporting','Reporting',@PDisplaySequence+110,IDENT_CURRENT('TBL_KS_Module'),@FunctionID,NULL)

-- TBL_KS_RolePrivilege
INSERT INTO TBL_KS_RolePrivilege ( RoleID,PrivilegeID )
VALUES ( @RoleID ,IDENT_CURRENT('TBL_KS_Privilege')	)

-- TBL_IE_DefaultSetting

INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataReview,'STATE','All')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataImport,'STATE','All')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDEstReview,'STATE','All')

INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataReview,'MANAGERGROUP','My_Clients')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataImport,'MANAGERGROUP','My_Clients')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDEstReview,'MANAGERGROUP','My_Clients')

-- Privilege : Accounting

-- Get RoleID
SELECT @RoleID = RoleID
FROM TBL_KS_Role
WHERE RoleName = 'Accounting'

-- TBL_KS_Privilege

INSERT INTO TBL_KS_Privilege (PrivilegeName,Description,DisplaySequence,ModuleID,FunctionID,GroupID) 
Values ( 'PrvStateAccounting','Accounting',@PDisplaySequence+120,IDENT_CURRENT('TBL_KS_Module'),@FunctionID,NULL)

-- TBL_KS_RolePrivilege
INSERT INTO TBL_KS_RolePrivilege ( RoleID,PrivilegeID )
VALUES ( @RoleID ,IDENT_CURRENT('TBL_KS_Privilege')	)

-- TBL_IE_DefaultSetting

INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataReview,'STATE','All')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataImport,'STATE','All')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDEstReview,'STATE','All')

INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataReview,'MANAGERGROUP','My_Clients')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataImport,'MANAGERGROUP','My_Clients')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDEstReview,'MANAGERGROUP','My_Clients')

-- Privilege : IM

-- Get RoleID
SELECT @RoleID = RoleID
FROM TBL_KS_Role
WHERE RoleName = 'IM'

-- TBL_KS_Privilege

INSERT INTO TBL_KS_Privilege (PrivilegeName,Description,DisplaySequence,ModuleID,FunctionID,GroupID) 
Values ( 'PrvStateIM','Investment Management',@PDisplaySequence+130,IDENT_CURRENT('TBL_KS_Module'),@FunctionID,NULL)

-- TBL_KS_RolePrivilege
INSERT INTO TBL_KS_RolePrivilege ( RoleID,PrivilegeID )
VALUES ( @RoleID ,IDENT_CURRENT('TBL_KS_Privilege')	)

-- TBL_IE_DefaultSetting

INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataReview,'STATE','All')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataImport,'STATE','All')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDEstReview,'STATE','All')

INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataReview,'MANAGERGROUP','My_Clients')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataImport,'MANAGERGROUP','My_Clients')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDEstReview,'MANAGERGROUP','My_Clients')

-- Privilege : Client Pubs

-- Get RoleID
SELECT @RoleID = RoleID
FROM TBL_KS_Role
WHERE RoleName = 'Client Pubs'

-- TBL_KS_Privilege

INSERT INTO TBL_KS_Privilege (PrivilegeName,Description,DisplaySequence,ModuleID,FunctionID,GroupID) 
Values ( 'PrvStateClientPubs','Client Communication',@PDisplaySequence+140,IDENT_CURRENT('TBL_KS_Module'),@FunctionID,NULL)

-- TBL_KS_RolePrivilege
INSERT INTO TBL_KS_RolePrivilege ( RoleID,PrivilegeID )
VALUES ( @RoleID ,IDENT_CURRENT('TBL_KS_Privilege')	)

-- TBL_IE_DefaultSetting

INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataReview,'STATE','All')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataImport,'STATE','All')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDEstReview,'STATE','All')

INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataReview,'MANAGERGROUP','My_Clients')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataImport,'MANAGERGROUP','My_Clients')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDEstReview,'MANAGERGROUP','My_Clients')

-- Privilege : Admin
-- Get RoleID
SELECT @RoleID = RoleID
FROM TBL_KS_Role
WHERE RoleName = 'Admin'

-- TBL_KS_Privilege

INSERT INTO TBL_KS_Privilege (PrivilegeName,Description,DisplaySequence,ModuleID,FunctionID,GroupID) 
Values ( 'PrvStateAdmin','Admin',@PDisplaySequence+150,IDENT_CURRENT('TBL_KS_Module'),@FunctionID,NULL)

-- TBL_KS_RolePrivilege
INSERT INTO TBL_KS_RolePrivilege ( RoleID,PrivilegeID )
VALUES ( @RoleID ,IDENT_CURRENT('TBL_KS_Privilege')	)

-- TBL_IE_DefaultSetting

INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataReview,'STATE','All')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataImport,'STATE','All')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDEstReview,'STATE','All')

INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataReview,'MANAGERGROUP','My_Clients')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataImport,'MANAGERGROUP','My_Clients')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDEstReview,'MANAGERGROUP','My_Clients')

-- Privilege : Business Dev
-- Get RoleID
SELECT @RoleID = RoleID
FROM TBL_KS_Role
WHERE RoleName = 'Business Dev'

-- TBL_KS_Privilege

INSERT INTO TBL_KS_Privilege (PrivilegeName,Description,DisplaySequence,ModuleID,FunctionID,GroupID) 
Values ( 'PrvStateBusinessDev','Business Development',@PDisplaySequence+160,IDENT_CURRENT('TBL_KS_Module'),@FunctionID,NULL)

-- TBL_KS_RolePrivilege
INSERT INTO TBL_KS_RolePrivilege ( RoleID,PrivilegeID )
VALUES ( @RoleID ,IDENT_CURRENT('TBL_KS_Privilege')	)

-- TBL_IE_DefaultSetting

INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataReview,'STATE','All')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataImport,'STATE','All')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDEstReview,'STATE','All')

INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataReview,'MANAGERGROUP','My_Clients')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataImport,'MANAGERGROUP','My_Clients')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDEstReview,'MANAGERGROUP','My_Clients')

-- Privilege : WO Access to IE
-- Get RoleID
SELECT @RoleID = RoleID
FROM TBL_KS_Role
WHERE RoleName = 'WO Access to IE'

-- TBL_KS_Privilege

INSERT INTO TBL_KS_Privilege (PrivilegeName,Description,DisplaySequence,ModuleID,FunctionID,GroupID) 
Values ( 'PrvStateWOAccessToIE','WO Access to IE',@PDisplaySequence+170,IDENT_CURRENT('TBL_KS_Module'),@FunctionID,NULL)

-- TBL_KS_RolePrivilege
INSERT INTO TBL_KS_RolePrivilege ( RoleID,PrivilegeID )
VALUES ( @RoleID ,IDENT_CURRENT('TBL_KS_Privilege')	)

-- TBL_IE_DefaultSetting

INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataReview,'STATE','All')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataImport,'STATE','All')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDEstReview,'STATE','All')

INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataReview,'MANAGERGROUP','My_Clients')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDDataImport,'MANAGERGROUP','My_Clients')
INSERT INTO TBL_IE_DefaultSetting (PrivilegeID,ModuleID,SettingType,DefaultSetting) 
Values(IDENT_CURRENT('TBL_KS_Privilege'),@ModuleIDEstReview,'MANAGERGROUP','My_Clients')


-- Role : GA CM ; 
-- Get RoleID
SELECT @RoleID = RoleID
FROM TBL_KS_Role
WHERE RoleName = 'GA CM'

-- TBL_KS_Privilege

INSERT INTO TBL_KS_Privilege (PrivilegeName,Description,DisplaySequence,ModuleID,FunctionID,GroupID) 
Values ( 'PrvStateGACM','GA CM',@PDisplaySequence+180,IDENT_CURRENT('TBL_KS_Module'),@FunctionID,NULL)

-- TBL_KS_RolePrivilege
INSERT INTO TBL_KS_RolePrivilege ( RoleID,PrivilegeID )
VALUES ( @RoleID ,IDENT_CURRENT('TBL_KS_Privilege')	)

-- Role : GA TA
-- Get RoleID
SELECT @RoleID = RoleID
FROM TBL_KS_Role
WHERE RoleName = 'GA TA'

-- TBL_KS_Privilege

INSERT INTO TBL_KS_Privilege (PrivilegeName,Description,DisplaySequence,ModuleID,FunctionID,GroupID) 
Values ( 'PrvStateGATA','GA TA',@PDisplaySequence+190,IDENT_CURRENT('TBL_KS_Module'),@FunctionID,NULL)

-- TBL_KS_RolePrivilege
INSERT INTO TBL_KS_RolePrivilege ( RoleID,PrivilegeID )
VALUES ( @RoleID ,IDENT_CURRENT('TBL_KS_Privilege')	)


-- TBL_IE_AXYSSetting

-- Actual Income on Standard Assets

INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Actual Income on Standard Assets','AIPURCH','agus, isus, emus, lcus, eius, ieus, reus, rius, mhus, gbus, gaus, mgus, cbus, mius, ibus, mjus, jbus, mfus, mbus, bfus, fmus, gmus, fhus, cmus, pmus, stus, tbus, peus, pbus, mcus, wtus, psus, anus, utus, cdus, hfus, vcus, cvus, cous, gtus, csus, cpus, clus, ptus, rtus, dpus, caus, mmus')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Actual Income on Standard Assets','AISELL','agus, isus, emus, lcus, eius, ieus, reus, rius, mhus, gbus, gaus, mgus, cbus, mius, ibus, mjus, jbus, mfus, mbus, bfus, fmus, gmus, fhus, cmus, pmus, stus, tbus, peus, pbus, mcus, wtus, psus, anus, utus, cdus, hfus, vcus, cvus, cous, gtus, csus, cpus, clus, ptus, rtus, dpus, caus, mmus')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Actual Income on Standard Assets','DIV','agus, isus, emus, lcus, eius, ieus, reus, rius, mhus, gbus, gaus, mgus, cbus, mius, ibus, mjus, jbus, mfus, mbus, bfus, fmus, gmus, fhus, cmus, pmus, stus, tbus, peus, pbus, mcus, wtus, psus, anus, utus, cdus, hfus, vcus, cvus, cous, gtus, csus, cpus, clus, ptus, rtus, dpus, caus, mmus')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Actual Income on Standard Assets','DIVADJ','agus, isus, emus, lcus, eius, ieus, reus, rius, mhus, gbus, gaus, mgus, cbus, mius, ibus, mjus, jbus, mfus, mbus, bfus, fmus, gmus, fhus, cmus, pmus, stus, tbus, peus, pbus, mcus, wtus, psus, anus, utus, cdus, hfus, vcus, cvus, cous, gtus, csus, cpus, clus, ptus, rtus, dpus, caus, mmus')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Actual Income on Standard Assets','DIVRE','agus, isus, emus, lcus, eius, ieus, reus, rius, mhus, gbus, gaus, mgus, cbus, mius, ibus, mjus, jbus, mfus, mbus, bfus, fmus, gmus, fhus, cmus, pmus, stus, tbus, peus, pbus, mcus, wtus, psus, anus, utus, cdus, hfus, vcus, cvus, cous, gtus, csus, cpus, clus, ptus, rtus, dpus, caus, mmus')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Actual Income on Standard Assets','INT','agus, isus, emus, lcus, eius, ieus, reus, rius, mhus, gbus, gaus, mgus, cbus, mius, ibus, mjus, jbus, mfus, mbus, bfus, fmus, gmus, fhus, cmus, pmus, stus, tbus, peus, pbus, mcus, wtus, psus, anus, utus, cdus, hfus, vcus, cvus, cous, gtus, csus, cpus, clus, ptus, rtus, dpus, caus, mmus')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Actual Income on Standard Assets','INTADJ','agus, isus, emus, lcus, eius, ieus, reus, rius, mhus, gbus, gaus, mgus, cbus, mius, ibus, mjus, jbus, mfus, mbus, bfus, fmus, gmus, fhus, cmus, pmus, stus, tbus, peus, pbus, mcus, wtus, psus, anus, utus, cdus, hfus, vcus, cvus, cous, gtus, csus, cpus, clus, ptus, rtus, dpus, caus, mmus')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Actual Income on Standard Assets','INTCD','agus, isus, emus, lcus, eius, ieus, reus, rius, mhus, gbus, gaus, mgus, cbus, mius, ibus, mjus, jbus, mfus, mbus, bfus, fmus, gmus, fhus, cmus, pmus, stus, tbus, peus, pbus, mcus, wtus, psus, anus, utus, cdus, hfus, vcus, cvus, cous, gtus, csus, cpus, clus, ptus, rtus, dpus, caus, mmus')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Actual Income on Standard Assets','INTOTHER','agus, isus, emus, lcus, eius, ieus, reus, rius, mhus, gbus, gaus, mgus, cbus, mius, ibus, mjus, jbus, mfus, mbus, bfus, fmus, gmus, fhus, cmus, pmus, stus, tbus, peus, pbus, mcus, wtus, psus, anus, utus, cdus, hfus, vcus, cvus, cous, gtus, csus, cpus, clus, ptus, rtus, dpus, caus, mmus')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Actual Income on Standard Assets','MBSINT','agus, isus, emus, lcus, eius, ieus, reus, rius, mhus, gbus, gaus, mgus, cbus, mius, ibus, mjus, jbus, mfus, mbus, bfus, fmus, gmus, fhus, cmus, pmus, stus, tbus, peus, pbus, mcus, wtus, psus, anus, utus, cdus, hfus, vcus, cvus, cous, gtus, csus, cpus, clus, ptus, rtus, dpus, caus, mmus')

-- Estimated Income on Standard Assets

INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Estimated Income on Standard Assets',NULL,'agus, isus, emus, lcus, eius, ieus, csus, peus, pbus, bfus, hfus, reus, rius, pmus, gtus, cous, mhus, mgus, mius, mjus, mfus, mcus, vcus, oaus')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Estimated Income on Standard Assets',NULL,'agus, isus, emus, lcus, eius, ieus, csus, peus, pbus, wtus, psus, hfus, reus, rius, cous, vcus, utus, anus, pmus, cpus, clus, ptus, rtus, gtus, mhus, mgus, mius, mjus, mfus, oius, lpus, teus, mcus, hfus, oaus')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Estimated Income on Standard Assets',NULL,'gbus, gaus, cbus, ibus, jbus, mbus, cmus, stus, gtus, cdus, cvus, anus, cfus, teus, cpus')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Estimated Income on Standard Assets',NULL,'gbus, gaus, cbus, ibus, jbus, mbus, cmus, stus, gtus, cdus, cvus, anus, cfus, teus, cpus')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Estimated Income on Standard Assets',NULL,'fmus, gmus, fhus.')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Estimated Income on Standard Assets',NULL,'fmus, gmus, fhus.')

-- Actual Income on Non-Standard Assets

INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Actual Income on Non-Standard Assets','AIPURCH','rpus, oaus, pnus, vaus')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Actual Income on Non-Standard Assets','AISELL','rpus, oaus, pnus, vaus')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Actual Income on Non-Standard Assets','DIV','rpus, oaus, pnus, vaus')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Actual Income on Non-Standard Assets','DIVADJ','rpus, oaus, pnus, vaus')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Actual Income on Non-Standard Assets','DIVRE','rpus, oaus, pnus, vaus')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Actual Income on Non-Standard Assets','INT','rpus, oaus, pnus, vaus')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Actual Income on Non-Standard Assets','INTADJ','rpus, oaus, pnus, vaus')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Actual Income on Non-Standard Assets','INTCD','rpus, oaus, pnus, vaus')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Actual Income on Non-Standard Assets','INTOTHER','rpus, oaus, pnus, vaus')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Actual Income on Non-Standard Assets','LOANINT','rpus, oaus, pnus, vaus')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Actual Income on Non-Standard Assets','MBSINT','rpus, oaus, pnus, vaus')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Actual Income on Non-Standard Assets','REINT','rpus, oaus, pnus, vaus')

-- Estimated Income on Non-Standard Assets

INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Estimated Income on Non-Standard Assets','AIPURCH','Debit')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Estimated Income on Non-Standard Assets','AISELL','Credit')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Estimated Income on Non-Standard Assets','DIV','Credit')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Estimated Income on Non-Standard Assets','DIVADJ','Debit')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Estimated Income on Non-Standard Assets','DIVRE','Credit')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Estimated Income on Non-Standard Assets','INT','Credit')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Estimated Income on Non-Standard Assets','INTADJ','Debit')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Estimated Income on Non-Standard Assets','INTCD','Credit')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Estimated Income on Non-Standard Assets','INTOTHER','Credit')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Estimated Income on Non-Standard Assets','LOANINT','Credit')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Estimated Income on Non-Standard Assets','MBSINT','Credit')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Estimated Income on Non-Standard Assets','REINT','Credit')

INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Estimated Income on Non-Standard Assets','INT','Credit')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Estimated Income on Non-Standard Assets','INTADJ','Debit')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Estimated Income on Non-Standard Assets','INTCD','Credit')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Estimated Income on Non-Standard Assets','INTOTHER','Credit')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Estimated Income on Non-Standard Assets','LOANINT','Credit')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Estimated Income on Non-Standard Assets','MBSINT','Credit')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Estimated Income on Non-Standard Assets','REINT','Credit')

INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Estimated Income on Non-Standard Assets','INT','Credit')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Estimated Income on Non-Standard Assets','INTADJ','Debit')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Estimated Income on Non-Standard Assets','INTCD','Credit')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Estimated Income on Non-Standard Assets','INTOTHER','Credit')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Estimated Income on Non-Standard Assets','LOANINT','Credit')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Estimated Income on Non-Standard Assets','MBSINT','Credit')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Estimated Income on Non-Standard Assets','REINT','Credit')

--Reclassified Income

INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Reclassified Income','AIPURCH','All')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Reclassified Income','AISELL','All')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Reclassified Income','DIV','All')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Reclassified Income','DIVADJ','All')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Reclassified Income','DIVRE','All')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Reclassified Income','INT','All')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Reclassified Income','INTADJ','All')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Reclassified Income','INTCD','All')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Reclassified Income','INTOTHER','All')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Reclassified Income','MBSINT','All')

-- Annual IM Fee
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Annual IM Fee','FEEMISC ','NA')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Annual IM Fee','FEEMADJ ','NA')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Annual IM Fee','FEEPDACT','NA')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Annual IM Fee','FEEPDAJ ','NA')

-- Annual TA Fee
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Annual TA Fee','FEEMISC ','NA')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Annual TA Fee','FEEMADJ ','NA')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Annual TA Fee','FEEPDACT','NA')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Annual TA Fee','FEEPDAJ ','NA')

-- Annual Trustee Fee (HOLDINGS BASED)

INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Annual Trustee Fee (HOLDINGS BASED)','FEEMISC ','NA')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Annual Trustee Fee (HOLDINGS BASED)','FEEMADJ ','NA')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Annual Trustee Fee (HOLDINGS BASED)','FEEPDACT','NA')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Annual Trustee Fee (HOLDINGS BASED)','FEEPDAJ ','NA')

-- Annual Trustee Fee (INCOME BASED)
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Annual Trustee Fee (INCOME BASED)','NULL','NA')

-- YTD Other Expenses aka YTD Vendor Payments
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Other Expenses aka YTD Vendor Payments','NULL','ACHDSB')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Other Expenses aka YTD Vendor Payments','NULL','CASHDSP ')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Other Expenses aka YTD Vendor Payments','NULL','CHECKDSB')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Other Expenses aka YTD Vendor Payments','NULL','CSHDSBAJ')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Other Expenses aka YTD Vendor Payments','NULL','JNLDSB')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Other Expenses aka YTD Vendor Payments','NULL','WIREDSB')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Other Expenses aka YTD Vendor Payments','NULL','ACHDEP')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Other Expenses aka YTD Vendor Payments','NULL','CASHDEP ')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Other Expenses aka YTD Vendor Payments','NULL','CHECKDEP')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Other Expenses aka YTD Vendor Payments','NULL','CSHDEPAJ')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Other Expenses aka YTD Vendor Payments','NULL','EXPENSE')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Other Expenses aka YTD Vendor Payments','NULL','EXPNSAJ')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Other Expenses aka YTD Vendor Payments','NULL','WIREDEP')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Other Expenses aka YTD Vendor Payments','NULL','XFRFMIA')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Other Expenses aka YTD Vendor Payments','NULL','XFRFMIAB')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Other Expenses aka YTD Vendor Payments','NULL','XFRFMIAM')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Other Expenses aka YTD Vendor Payments','NULL','XFRTOIA')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Other Expenses aka YTD Vendor Payments','NULL','XFRTOIAB')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Other Expenses aka YTD Vendor Payments','NULL','XFRTOIAM')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Other Expenses aka YTD Vendor Payments','NULL','XFRFMI')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Other Expenses aka YTD Vendor Payments','NULL','XFRFMIB')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Other Expenses aka YTD Vendor Payments','NULL','XFRFMIM')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Other Expenses aka YTD Vendor Payments','NULL','XFRTOI')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Other Expenses aka YTD Vendor Payments','NULL','XFRTOIB')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Other Expenses aka YTD Vendor Payments','NULL','XFRTOIM')

-- Asset Amortization
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Asset Amortization','NULL','NA')


-- YTD Actual Short-term Gain Distributions (SGD)

INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Actual Short-term Gain Distributions (SGD)','NULL','3200')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Actual Short-term Gain Distributions (SGD)','NULL','3400')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Actual Short-term Gain Distributions (SGD)','NULL','3500')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Actual Short-term Gain Distributions (SGD)','NULL','3550')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Actual Short-term Gain Distributions (SGD)','NULL','3600')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Actual Short-term Gain Distributions (SGD)','NULL','3650')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Actual Short-term Gain Distributions (SGD)','NULL','9100')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Actual Short-term Gain Distributions (SGD)','NULL','3700')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Actual Short-term Gain Distributions (SGD)','NULL','3750')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Actual Short-term Gain Distributions (SGD)','NULL','3800')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Actual Short-term Gain Distributions (SGD)','NULL','3850')
INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('YTD Actual Short-term Gain Distributions (SGD)','NULL','3900')


-- Estimated Short-term Gain Distributions (SGD)

INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Estimated Short-term Gain Distributions (SGD)','NULL','agus, isus, emus, lcus, eius, ieus, csus, peus, pbus, bfus, hfus, reus, rius, pmus, gtus, cous, mhus, mgus, mius, mjus, mfus')

-- Actual STG/LTG

INSERT INTO TBL_IE_AXYSSetting (AssetType,TransactionCode,SecurityType) 
Values('Actual STG/LTG','NULL','NA')

SET NOCOUNT OFF

PRINT 'CUSIP column migration pending for TBL_IE_SecurityReclass, TBL_IE_STG_EstimateDetail, TBL_IE_STG_Holding, TBL_IE_STG_JanOneHolding, TBL_IE_DiagnosticResult, TBL_IE_AssetYield'
