SET NOCOUNT ON
INSERT INTO TBL_INV_BeneReportComment (
	BRCommentID
	,PortfolioCode
	,PortfolioLabel
	,PortfolioDescription
	,ModifiedDate
	,ModifiedBy
	,CreatedDate
	,CreatedBy
	,DeletedBy
	)
SELECT BenRepCom.BRCommentID
	,PortfolioCode
	,PortfolioLabel
	,PortfolioDescription
	,isnull(MODIFIED_DATE, getdate())
	,isnull(MODIFIED_USER_ID, 1)
	,isnull(CREATED_DATE, getdate())
	,isnull(CREATED_USER_ID, 1)
	,DELETED_USER_ID
FROM $(ExcelsiorDB)..BENEREPORTCOMMENTS BenRepCom(NOLOCK)
LEFT OUTER JOIN $(ExcelsiorDB)..TBL_EIS_EX_BENEREPORTCOMMENTS_SUPPLEMENT BenRepComSupp(NOLOCK)
	ON BenRepCom.BRCommentID = BenRepComSupp.BRCommentID
GO

SET IDENTITY_INSERT TBL_INV_DecisionCommentType ON

INSERT INTO TBL_INV_DecisionCommentType (
	TypeID
	,TypeName
	)
SELECT T.TypeID
	,T.TypeName
FROM $(ExcelsiorDB)..TBL_EIS_ENF_DecisionCommentType T(NOLOCK)

SET IDENTITY_INSERT TBL_INV_DecisionCommentType OFF
GO

SET IDENTITY_INSERT TBL_INV_MasterObjective ON

INSERT INTO TBL_INV_MasterObjective (
	MasterObjectiveID
	,MasterObjectiveCode
	,MasterObjectiveDescription
	)
SELECT T.MasterObjectiveID
	,T.MasterObjectiveCode
	,T.MasterObjectiveDescription
FROM $(ExcelsiorDB)..TBL_EIS_ENF_MasterObjective T(NOLOCK)

SET IDENTITY_INSERT TBL_INV_MasterObjective OFF
GO

INSERT INTO TBL_INV_TargetFundAllocation (
	ObjectiveCode
	,SecuritySymbol
	,CashBalanceCode
	,FundWeight
	,FundRank
	,TargetFundPercent
	)
SELECT T.ObjectiveCode
	,T.SecuritySymbol
	,T.CashBalanceCode
	,T.FundWeight
	,T.FundRank
	,T.TargetFundPercent
FROM $(ExcelsiorDB)..TargetFundAllocation T(NOLOCK)
GO

INSERT INTO TBL_INV_TargetAllocation (
	ObjectiveCode
	,AssetClassName
	,CashBalanceCode
	,ClassRank
	,TargetPercent
	,DefaultAssetClass
	)
SELECT T.ObjectiveCode
	,T.AssetClassName
	,T.CashBalanceCode
	,T.ClassRank
	,T.TargetPercent
	,T.DefaultAssetClass
FROM $(ExcelsiorDB)..TargetAllocation T(NOLOCK)
GO

SET IDENTITY_INSERT TBL_INV_StrategicAllocation ON

INSERT INTO TBL_INV_StrategicAllocation (
	StrategicAllocationID
	,StrategicAllocationCode
	,StrategicAllocationName
	,Description
	,IsActive
	,ModifiedDate
	,ModifiedBy
	,BRCommentID
	,MasterObjectiveID
	)
SELECT T.StrategicAllocationID
	,T.StrategicAllocationCode
	,T.StrategicAllocationName
	,T.Description
	,T.IsActive
	,T.LastModifiedDate
	,T.LastModifiedUser
	,T.BRCommentID
	,T.MasterObjectiveID
FROM $(ExcelsiorDB)..TBL_EIS_ENF_StrategicAllocation T(NOLOCK)

SET IDENTITY_INSERT TBL_INV_StrategicAllocation OFF
GO

INSERT INTO TBL_INV_SecInfo (
	SecuritySymbol
	,SecurityDescription
	,SecurityTypeCode
	,AssetClass
	,IndustryGroup
	,UserDefinedCode
	,ModifiedDate
	,ModifiedBy
	)
SELECT T.SecuritySymbol
	,T.SecurityDescription
	,T.SecurityTypeCode
	,T.AssetClass
	,T.IndustryGroup
	,T.UserDefinedCode
	,T.LastModifiedDate
	,T.LastModifiedUser
FROM $(ExcelsiorDB)..TBL_EIS_ENF_SecInfo T(NOLOCK)
GO

SET IDENTITY_INSERT TBL_INV_StrategicAllocationSet ON

INSERT INTO TBL_INV_StrategicAllocationSet (
	StrategicAllocationSetID
	,StrategicAllocationSetName
	,Description
	,IsActive
	,ModifiedDate
	,ModifiedBy
	)
SELECT T.StrategicAllocationSetID
	,T.StrategicAllocationSetName
	,T.Description
	,T.IsActive
	,T.LastModifiedDate
	,T.LastModifiedUser
FROM $(ExcelsiorDB)..TBL_EIS_ENF_StrategicAllocationSet T(NOLOCK)

SET IDENTITY_INSERT TBL_INV_StrategicAllocationSet OFF
GO

SET IDENTITY_INSERT TBL_INV_AUDIT_BeneReportComment ON

INSERT INTO TBL_INV_AUDIT_BeneReportComment (
	AuditSequence
	,AuditUserID
	,AuditDateTime
	,AuditFlag
	,AuditTable
	,AuditDetails
	,BRCommentID
	,PortfolioCode
	,PortfolioLabel
	,PortfolioDescription
	,ModifiedDate
	,ModifiedBy
	,CreatedDate
	,CreatedBy
	,DeletedBy
	)
SELECT T.AUDIT_SEQUENCE
	,T.AUDIT_USER_ID
	,T.AUDIT_DATETIME
	,T.AUDIT_FLAG
	,T.AUDIT_TABLE
	,T.AUDIT_DETAILS
	,T.BRCommentID
	,T.PortfolioCode
	,T.PortfolioLabel
	,T.PortfolioDescription
	,T.MODIFIED_DATE
	,T.MODIFIED_USER_ID
	,T.CREATED_DATE
	,T.CREATED_USER_ID
	,T.DELETED_USER_ID
FROM $(ExcelsiorDB)..TBL_EIS_EX_AUDIT_BENEREPORTCOMMENTS T(NOLOCK)

SET IDENTITY_INSERT TBL_INV_AUDIT_BeneReportComment OFF
GO

SET IDENTITY_INSERT TBL_INV_InvestmentComment ON

INSERT INTO TBL_INV_InvestmentComment (
	InvestmentCommentID
	,CustomerAccountNumber
	,InvestmentComment
	,CreatedDate
	,CreatedBy
	,ModifiedDate
	,ModifiedBy
	)
SELECT T.InvestmentCommentID
	,T.AdventID
	,T.InvestmentComment
	,T.CreateDate
	,T.CreatedUser
	,T.LastModifiedDate
	,T.LastModifiedUser
FROM $(ExcelsiorDB)..TBL_EIS_EX_INVESTMENT_COMMENT T(NOLOCK)

SET IDENTITY_INSERT TBL_INV_InvestmentComment OFF
GO

SET IDENTITY_INSERT TBL_INV_ClientInvestmentPolicyTrustType ON

INSERT INTO TBL_INV_ClientInvestmentPolicyTrustType (
	IPTrustTypeID
	,ManagerCode
	,TrustTypeID
	,TrustTypeIPName
	,MinHorizonValue
	,MinHorizonCond
	,MaxHorizonValue
	,MaxHorizonCond
	,Review
	,MinReviewValue
	,MinReviewCond
	,MaxReviewValue
	,MaxReviewCond
	,CreatedDate
	,CreatedBy
	)
SELECT T.IPTrustTypeID
	,C.BriefName
	,T.TrustTypeID
	,T.TrustTypeIPName
	,T.MinHorizonValue
	,T.MinHorizonCond
	,T.MaxHorizonValue
	,T.MaxHorizonCond
	,T.Review
	,T.MinReviewValue
	,T.MinReviewCond
	,T.MaxReviewValue
	,T.MaxReviewCond
	,T.CREATEDDATE
	,T.CREATEDUSERID
FROM $(ExcelsiorDB)..TBL_EIS_ENF_ClientInvestmentPolicy_TrustType T(NOLOCK)
INNER JOIN $(ExcelsiorDB)..CLIENT C(NOLOCK)
	ON T.ClientID = C.ClientID

SET IDENTITY_INSERT TBL_INV_ClientInvestmentPolicyTrustType OFF
GO

SET IDENTITY_INSERT TBL_INV_DecisionComment ON

INSERT INTO TBL_INV_DecisionComment (
	DecisionCommentID
	,Comment
	,DecisionDate
	,CommentUser
	,OriginType
	)
SELECT T.DecisionCommentsID
	,T.Comments
	,T.DecisionDate
	,T.CommentUser
	,T.OriginType
FROM $(ExcelsiorDB)..TBL_EIS_ENF_DecisionComments T(NOLOCK)

SET IDENTITY_INSERT TBL_INV_DecisionComment OFF
GO

SET IDENTITY_INSERT TBL_INV_BenchMarkType ON

INSERT INTO TBL_INV_BenchMarkType (
	BenchMarkTypeID
	,BenchMarkTypeName
	,ModifiedDate
	,ModifiedBy
	)
SELECT T.BenchMarkTypeID
	,T.BenchMarkTypeName
	,T.LastModifiedDate
	,T.LastModifiedUser
FROM $(ExcelsiorDB)..TBL_EIS_ENF_BenchMarkType T(NOLOCK)

SET IDENTITY_INSERT TBL_INV_BenchMarkType OFF
GO

SET IDENTITY_INSERT TBL_INV_BenchMark ON

INSERT INTO TBL_INV_BenchMark (
	BenchMarkID
	,BenchMarkName
	,HistoricBenchMarkID
	,BenchMarkTypeID
	,ModifiedDate
	,ModifiedBy
	)
SELECT T.BenchMarkID
	,T.BenchMarkName
	,T.HistoricBenchMarkID
	,T.BenchMarkTypeID
	,T.LastModifiedDate
	,T.LastModifiedUser
FROM $(ExcelsiorDB)..TBL_EIS_ENF_BenchMark T(NOLOCK)

SET IDENTITY_INSERT TBL_INV_BenchMark OFF
SET IDENTITY_INSERT TBL_INV_ClientStrategicAllocation ON

INSERT INTO TBL_INV_ClientStrategicAllocation (
	ClientStrategicAllocationID
	,ManagerCode
	,StrategicAllocationSetID
	,ModifiedBy
	,ModifiedDate
	)
SELECT T.ClientStrategicAllocationID
	,C.BriefName
	,T.StrategicAllocationSetID
	,T.LastModifiedUser
	,T.LastModifiedDate
FROM $(ExcelsiorDB)..TBL_EIS_ENF_ClientStrategicAllocation T(NOLOCK)
INNER JOIN $(ExcelsiorDB)..CLIENT C(NOLOCK)
	ON T.ClientID = C.ClientID

SET IDENTITY_INSERT TBL_INV_ClientStrategicAllocation OFF
GO

SET IDENTITY_INSERT TBL_INV_ClientInvestmentPolicyDetail ON

INSERT INTO TBL_INV_ClientInvestmentPolicyDetail (
	ClientInvestmentPolicyDetailID
	,IPTrustTypeID
	,StrategicAllocationID
	,TrustTypeID
	,MinPayoutValue
	,MinPayoutCond
	,MaxPayoutValue
	,MaxPayoutCond
	,CreatedDate
	,CreatedBy
	)
SELECT T.ClientInvestmentPolicyDetailsID
	,T.IPTrustTypeID
	,T.StrategicAllocationID
	,T.TrustTypeID
	,T.MinPayoutValue
	,T.MinPayoutCond
	,T.MaxPayoutValue
	,T.MaxPayoutCond
	,T.CREATEDDATE
	,T.CREATEDUSERID
FROM $(ExcelsiorDB)..TBL_EIS_ENF_ClientInvestmentPolicy_Details T(NOLOCK)

SET IDENTITY_INSERT TBL_INV_ClientInvestmentPolicyDetail OFF
GO

SET IDENTITY_INSERT TBL_INV_DecisionCommentTypeLink ON

INSERT INTO TBL_INV_DecisionCommentTypeLink (
	DecisionCommentID
	,DecisionCommentTypeLinkID
	,DecisionTypeValue
	,TypeID
	)
SELECT T.DecisionCommentsID
	,T.DecisionCommentsTypeLinkID
	,T.DecisionTypeValue
	,T.TypeID
FROM $(ExcelsiorDB)..TBL_EIS_ENF_DecisionCommentsTypeLink T(NOLOCK)

SET IDENTITY_INSERT TBL_INV_DecisionCommentTypeLink OFF
GO

INSERT INTO TBL_INV_InvestmentObjective (
	ObjectiveCode
	,ManagerID
	,ObjectiveName
	,ObjectiveDescription
	,ExpectedYield
	,ExpectedAppreciation
	,InvestmentNumber
	,MasterObjectiveName
	,MABenchmarkID
	,TradBenchmarkID
	,IsActive
	,StrategicAllocationID
	,BRCommentID
	)
SELECT T.ObjectiveCode
	,T.ManagerID
	,T.ObjectiveName
	,T.ObjectiveDescription
	,T.ExpectedYield
	,T.ExpectedAppreciation
	,T.InvestmentNumber
	,T.MasterObjectiveName
	,T.MABenchmarkID
	,T.TradBenchmarkID
	,T.IsActive
	,T.StrategicAllocationID
	,T.BRCommentID
FROM $(ExcelsiorDB)..InvestmentObjective T(NOLOCK)
GO

SET IDENTITY_INSERT TBL_INV_StrategicAllocationSetDetail ON

INSERT INTO TBL_INV_StrategicAllocationSetDetail (
	StrategicAllocationSetDetailKey
	,StrategicAllocationID
	,StrategicAllocationSetID
	)
SELECT T.StrategicAllocationSetDetailsKey
	,T.StrategicAllocationID
	,T.StrategicAllocationSetID
FROM $(ExcelsiorDB)..TBL_EIS_ENF_StrategicAllocationSetDetails T(NOLOCK)

SET IDENTITY_INSERT TBL_INV_StrategicAllocationSetDetail OFF
GO

INSERT INTO TBL_INV_StrategicAllocationDetail (
	StrategicAllocationID
	,AssetClassName
	,AssetClassType
	,AssetClassDescription
	,TargetPercentage
	,ModifiedDate
	,ModifiedBy
	)
SELECT T.StrategicAllocationID
	,T.AssetClassName
	,T.AssetClassType
	,T.AssetClassDescription
	,T.TargetPercentage
	,GETDATE()
	,T.LastModiefiedUser
FROM $(ExcelsiorDB)..TBL_EIS_ENF_StrategicAllocationDetails T(NOLOCK)
GO

SET IDENTITY_INSERT TBL_INV_StrategicAllocationBenchMark ON

INSERT INTO TBL_INV_StrategicAllocationBenchMark (
	StrategicAllocationBenchMarkID
	,StrategicAllocationID
	,BenchMarkID
	,ModifiedDate
	,ModifiedBy
	)
SELECT T.StrategicAllocationBenchMarkID
	,T.StrategicAllocationID
	,T.BenchMarkID
	,T.LastModifiedDate
	,T.LastModiedUser
FROM $(ExcelsiorDB)..TBL_EIS_ENF_StrategicAllocationBenchMark T(NOLOCK)

SET IDENTITY_INSERT TBL_INV_StrategicAllocationBenchMark OFF
GO

SET IDENTITY_INSERT TBL_INV_TacticalAllocationBenchMark ON

INSERT INTO TBL_INV_TacticalAllocationBenchMark (
	TacticalAllocationBenchMarkID
	,ObjectiveCode
	,BenchMarkID
	,ModifiedDate
	,ModifiedBy
	)
SELECT T.TacticalAllocationBenchMarkID
	,T.ObjectiveCode
	,T.BenchMarkID
	,T.LastModifiedDate
	,T.LastModiedUser
FROM $(ExcelsiorDB)..TBL_EIS_ENF_TacticalAllocationBenchMark T(NOLOCK)

SET IDENTITY_INSERT TBL_INV_TacticalAllocationBenchMark OFF
GO

INSERT INTO TBL_INV_InvestmentObjectivePolicy (
	InvestmentObjectivePolicyID
	,MaxLifeExpectancy
	,AccountType
	,ManagerCode
	,UnitrustPercentage
	,ObjectiveCode
	,MaxUTPct
	,ObjectiveCodeClientSpecific
	,MinLifeExpectancy
	,MinLifeExpectancyCond
	,MaxLifeExpectancyCond
	,MinUPCond
	,MaxUPCond
	,ClientInvestmentPolicyDetailID
	)
SELECT T.InvestmentObjectivePolicyID
	,T.MaxLifeExpectancy
	,T.AccountType
	,C.BriefName
	,T.UnitrustPercentage
	,T.ObjectiveCode
	,T.MaxUTPct
	,T.ObjectiveCodeClientSpecific
	,T.MinLifeExpectancy
	,T.MinLifeExpectancyCond
	,T.MaxLifeExpectancyCond
	,T.MinUPCond
	,T.MaxUPCond
	,T.ClientInvestmentPolicyDetailsID
FROM $(ExcelsiorDB)..InvestmentObjectivePolicy T(NOLOCK)
INNER JOIN $(ExcelsiorDB)..CLIENT C(NOLOCK)
	ON T.ClientID = C.ClientID
GO

INSERT INTO TBL_INV_ClientInvestmentObjective (
	ManagerCode
	,ObjectiveCode
	,ObjectivePolicyAssigned
	,OrderBy
	)
SELECT C.BriefName
	,T.ObjectiveCode
	,T.ObjectivePolicyAssigned
	,T.OrderBy
FROM $(ExcelsiorDB)..ClientInvestmentObjective T(NOLOCK)
INNER JOIN $(ExcelsiorDB)..CLIENT C(NOLOCK)
	ON T.ClientID = C.ClientID
GO

INSERT INTO TBL_INV_AccountProfile (
	CustomerAccountNumber
	,AllowNewGift
	,AssetsUnderMgmt
	,AssetsUnderTA
	,Authority
	,AxysFMV
	,CashBalanceCode
	,DiscretionaryTrade
	,FirstTradeDate
	,FSITypeCode
	,HoldsSubstitutes
	,HorizonAsOfDate
	,HorizonComment
	,HorizonOmit
	,HorizonYears
	,HorizonYoungestAge
	,IncomeSensitiveYN
	,InvestmentComment
	,InvestmentTaxStatusCode
	,InvestmentTypeCode
	,LotAccountingCode
	,ObjectiveCode
	,PortfolioTypeCode
	,RestrictedAssetComment
	,RestrictedAssets
	,RestrictedAssetWeight
	,SchwabMasterID
	,SubstituteAssetComment
	,SubstituteAssetWeight
	,TradeStatusCode
	,TrancheID
	,TranchesRemaining
	,TrancheStatusCode
	,TranchesTotal
	,KCOTranserTo
	,AccountStatusCodeNA
	,FSITypeCodeNA
	,InvestmentTaxStatusCodeNA
	,InvestmentTypeCodeNA
	,TradeStatusCodeNA
	,ChangeInInvestmentObjective
	,ByPolicy
	,AccountInvestmentStatusCode
	,ExceptionDoc
	)
SELECT AdventID
	,AllowNewGifts
	,AssetsUnderMgmt
	,AssetsUnderTA
	,Authority
	,AxysFMV
	,CashBalanceCode
	,DiscretionaryTrade
	,FirstTradeDate
	,FSITypeCode
	,HoldsSubstitutes
	,HorizonAsOfDate
	,HorizonComment
	,HorizonOmit
	,HorizonYears
	,HorizonYoungestAge
	,IncomeSensitiveYN
	,InvestmentComment
	,InvestmentTaxStatusCode
	,InvestmentTypeCode
	,LotAccountingCode
	,ObjectiveCode
	,PortfolioTypeCode
	,RestrictedAssetComment
	,RestrictedAssets
	,RestrictedAssetWeight
	,SchwabMasterID
	,SubstituteAssetComment
	,SubstituteAssetWeight
	,TradeStatusCode
	,TrancheID
	,TranchesRemaining
	,TrancheStatusCode
	,TranchesTotal
	,KCO_TRANSFER_TO
	,ACCOUNTSTATUSCODE_NA
	,FSITYPECODE_NA
	,INVESTMENTTAXSTATUSCODE_NA
	,INVESTMENTTYPECODE_NA
	,TRADESTATUSCODE_NA
	,ChangeInInvestmentObjective
	,ByPolicy
	,AccountStatusCode
	,ExceptionDoc
FROM $(ExcelsiorDB)..DeferredGiftAccount DGA(NOLOCK)
INNER JOIN $(ExcelsiorDB)..TBL_EIS_EX_DEFERREDGIFTACCOUNT_SUPPLEMENT DGASUP
	ON DGA.AccountID = DGASUP.AccountID
