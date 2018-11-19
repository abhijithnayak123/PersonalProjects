SET NOCOUNT ON
SET IDENTITY_INSERT TBL_TR_EventStatus ON

INSERT INTO [TBL_TR_EventStatus] (
	EventStatusID
	,EventStatus
	,StatusTargetID
	,LastModifiedUserID
	,LastModifiedDate
	)
SELECT EventStatusID
	,EventStatus
	,StatusTargetID
	,LastModifiedUser
	,LastModifiedDate
FROM $(ExcelsiorDB)..TBL_EIS_TR_EventStatus

SET IDENTITY_INSERT TBL_TR_EventStatus OFF
SET IDENTITY_INSERT TBL_TR_Event ON

INSERT INTO TBL_TR_Event (
	EventID
	,EventStatusID
	,EventName
	,EventSource
	,Comments
	,CancelComment
	,AXYSPositionDate
	,LoadPaymentsDataUpTo
	,ImposeBenePayment
	,ReviewAXYSPositionDate
	,ReviewLoadPaymentsDataUpTo
	,ReviewImposeBenePayment
	,Source
	,SourceFille
	,AssignedTraderID
	,CombinedAccount
	,EventTrancheRate
	,BlockRevokeApproval
	,TradeComment
	,CreatedUserID
	,CreatedDate
	,LastModifiedUserID
	,LastModifiedDate
	)
SELECT EventID
	,EventStatusID
	,EventName
	,EventSource
	,Comments
	,CancelComments
	,AXYSPositionDate
	,LoadPaymentsDataUpTo
	,ImposeBenePayments
	,ReviewAXYSPositionDate
	,ReviewLoadPaymentsDataUpTo
	,ReviewImposeBenePayments
	,Source
	,SourceFille
	,AssignedTrader
	,CombinedAccounts
	,EventTrancheRate
	,BlockRevokeApproval
	,TradeComment
	,CreatedUser
	,CreateDate
	,LastModifiedUser
	,LastModifiedDate
FROM $(ExcelsiorDB)..TBL_EIS_TR_Event

SET IDENTITY_INSERT TBL_TR_Event OFF

INSERT INTO TBL_TR_AuthorizedSignerSnapshot
SELECT EventID
	,EventAccountID
	,EventStatusID
	,Auth.EmployeeID
	,Account.AdventID
	,AuthorizationType
	,UserName
FROM $(ExcelsiorDB)..TBL_EIS_TR_AuthorizedSigner_Snapshot Auth
INNER JOIN $(ExcelsiorDB)..DEFERREDGIFTACCOUNT Account
	ON Auth.AccountId = Account.AccountID

SET IDENTITY_INSERT TBL_TR_EventAccount ON

INSERT INTO TBL_TR_EventAccount (
	EventAccountID
	,EventID
	,CustomerAccountNumber
	,AccountOrder
	,AccountName
	,AccountType
	,PayoutRate
	,AnnuityAmount
	,SLMasterAccount
	,Managed
	,Custodian
	,CustodianAccountNumber
	,InvestmentTypeCode
	,TaxStatus
	,TradeDiscretion
	,TradeStatusCode
	,FSIOption
	,InvestmentComment
	,TrancheStatusCode
	,ImposedBenePayment
	,ObjectiveCode
	,PoliByCli
	,LotAccountingCode
	,ScheduledAmount
	,AnnualPayment
	,PaymentDate
	,IMlevel2Count
	,IMlevel1Count
	,LastModifiedUserID
	,LastModifiedDate
	)
SELECT DISTINCT EventAccountID
	,EventID
	,AdventID
	,AccountOrder
	,AccountName
	,AccMstr.AccountTypeCode
	,PayoutRate
	,AnnuityAmount
	,SLMasterAccount
	,Managed
	,Custodian
	,CustodianAccountNumber
	,InvestmentTypeCode
	,TaxStatus
	,TradeDiscretion
	,TradeStatusCode
	,FSIOption
	,InvestmentComment
	,TrancheStatusCode
	,ImposedBenePayments
	,ObjectiveCode
	,PoliByCli
	,LotAccountingCode
	,ScheduledAmount
	,AnnualPayment
	,PaymentDate
	,IMlevel2Count
	,IMlevel1Count
	,LastModifiedUser
	,LastModifiedDate
FROM $(ExcelsiorDB)..TBL_EIS_TR_EventAccount EvntAcc
	INNER JOIN $(InnoTrustDB)..AccountMaster AccMstr
 ON EvntAcc.ADVENTID = AccMstr.CustomerAccountNumber

SET IDENTITY_INSERT TBL_TR_EventAccount OFF
SET IDENTITY_INSERT TBL_TR_EventTracker ON

INSERT INTO TBL_TR_EventTracker (
	EventTrackerID
	,EventStatusID
	,EventID
	,ModifiedDate
	,ModifiedUserID
	)
SELECT EventTrackerID
	,EventStatusID
	,EventID
	,ModifiedDate
	,ModifiedUser
FROM $(ExcelsiorDB)..TBL_EIS_TR_EventTracker

SET IDENTITY_INSERT TBL_TR_EventTracker OFF
SET IDENTITY_INSERT TBL_TR_EventTypeMaster ON

INSERT INTO TBL_TR_EventTypeMaster (
	EventTypeID
	,EventType
	,Action
	,EaseOfReview
	,EaseOfTrade
	,ImpactToKCo
	,RiskOfError
	,RiskLevel
	,LastModifiedUserID
	,LastModifiedDate
	)
SELECT EventTypeID
	,EventType
	,Action
	,EaseOfReview
	,EaseOfTrade
	,ImpactToKCo
	,RiskOfError
	,RiskLevel
	,LastModifiedUser
	,LastModifiedDate
FROM $(ExcelsiorDB)..TBL_EIS_TR_EventTypeMaster

SET IDENTITY_INSERT TBL_TR_EventTypeMaster OFF

INSERT INTO TBL_TR_EventType (
	EventID
	,EventTypeID
	,LastModifiedUserID
	,LastModifiedDate
	)
SELECT EventID
	,EventTypeID
	,LastModifiedUser
	,LastModifiedDate
FROM $(ExcelsiorDB)..TBL_EIS_TR_EventType

INSERT INTO TBL_TR_InvestmentCommentHistorySnapshot
SELECT EventID
	,EventAccountID
	,EventStatusID
	,InvestmentCommentId
	,adventid
	,InvestmentComments
	,CreatedUserID
	,ModifiedUserID
	,CreatedDate
	,ModifiedDate
FROM $(ExcelsiorDB)..TBL_EIS_TR_InvestmentCommentHistory_Snapshot

INSERT INTO TBL_TR_ReviewEventAccount (
	EventAccountID
	,AccountType
	,PayoutRate
	,AnnuityAmount
	,SLMasterAccount
	,Managed
	,Custodian
	,CustodianAccountNumber
	,InvestmentTypeCode
	,TaxStatus
	,TradeDiscretion
	,TradeStatusCode
	,FSIOption
	,InvestmentComment
	,TrancheStatusCode
	,ImposedBenePayment
	,ObjectiveCode
	,PoliByCli
	,ScheduledAmount
	,AnnualPayment
	,PaymentDate
	,IMlevel2Count
	,IMlevel1Count
	,LastModifiedUserID
	,LastModifiedDate
	)
SELECT DISTINCT RevEvntAcc.EventAccountID
	,AccMstr.AccountTypeCode
	,RevEvntAcc.PayoutRate
	,RevEvntAcc.AnnuityAmount
	,RevEvntAcc.SLMasterAccount
	,RevEvntAcc.Managed
	,RevEvntAcc.Custodian
	,RevEvntAcc.CustodianAccountNumber
	,RevEvntAcc.InvestmentTypeCode
	,RevEvntAcc.TaxStatus
	,RevEvntAcc.TradeDiscretion
	,RevEvntAcc.TradeStatusCode
	,RevEvntAcc.FSIOption
	,RevEvntAcc.InvestmentComment
	,RevEvntAcc.TrancheStatusCode
	,RevEvntAcc.ImposedBenePayments
	,RevEvntAcc.ObjectiveCode
	,RevEvntAcc.PoliByCli
	,RevEvntAcc.ScheduledAmount
	,RevEvntAcc.AnnualPayment
	,RevEvntAcc.PaymentDate
	,RevEvntAcc.IMlevel2Count
	,RevEvntAcc.IMlevel1Count
	,RevEvntAcc.LastModifiedUser
	,RevEvntAcc.LastModifiedDate
FROM $(ExcelsiorDB)..TBL_EIS_TR_ReviewEventAccount RevEvntAcc
	INNER JOIN $(ExcelsiorDB)..TBL_EIS_TR_EventAccount EvntAcc
		ON RevEvntAcc.EventAccountID = EvntAcc.EventAccountID 
	INNER JOIN $(InnoTrustDB)..AccountMaster AccMstr
		ON EvntAcc.ADVENTID = AccMstr.CustomerAccountNumber

--INSERT INTO TBL_TR_SecurityInfo (
--	SecuritySymbol
--	,SecurityTypeCode
--	,SecurityDescription
--	,IndustryGroup
--	,LastModifiedUserID
--	,LastModifiedDate
--	)
--SELECT SecuritySymbol
--	,SecurityTypeCode
--	,SecurityDescription
--	,IndustryGroup
--	,LastModifiedUser
--	,LastModifiedDate
--FROM $(ExcelsiorDB)..TBL_EIS_TR_SecurityInfo
--INSERT INTO TBL_TR_SecurityTypeInfo (
--	SecurityTypeCode
--	,SecurityTypeDescription
--	,AssetClass
--	,LastModifiedUserID
--	,LastModifiedDate
--	)
--SELECT SecurityTypeCode
--	,SecurityTypeDescription
--	,AssetClass
--	,LastModifiedUser
--	,LastModifiedDate
--FROM $(ExcelsiorDB)..TBL_EIS_TR_SecurityTypeInfo
SET IDENTITY_INSERT TBL_TR_TSheetApprovedTrade ON

INSERT INTO TBL_TR_TSheetApprovedTrade (
	TradeID
	,EventID
	,EventAccountID
	,SecurityTypeCode
	,SecuritySymbol
	,TradeQuantity
	,DollarAmount
	,TradeDate
	,TradeType
	,ArchiveTradeID
	,ActiveTradeID
	,SubmissionType
	,SplitTrade
	,SubmittedQuantityTillDate
	,LastModifiedUserID
	,LastModifiedDate
	)
SELECT TradeID
	,AppTrade.EventID
	,AppTrade.EventAccountID
	,SecurityTypeCode
	,SecuritySymbol
	,TradeQuantity
	,DollarAmount
	,TradeDate
	,TradeType
	,ArchiveTradeID
	,ActiveTradeID
	,SubmissionType
	,SplitTrade
	,SubmittedQuantityTillDate
	,LastModifiedUser
	,AppTrade.LastModifiedDate
FROM $(ExcelsiorDB)..TBL_EIS_TR_TSheetApprovedTrade AppTrade
INNER JOIN TBL_TR_EventAccount EvntAcc
	on EvntAcc.EventAccountID = AppTrade.EventAccountID

SET IDENTITY_INSERT TBL_TR_TSheetApprovedTrade OFF
SET IDENTITY_INSERT TBL_TR_TradeError ON

INSERT INTO TBL_TR_TradeError (
	TradeErrorID
	,ErrorComments
	,ErrorDate
	,TradeID
	,LastModifiedUserID
	,LastModifiedDate
	)
SELECT TradeErrorID
	,ErrorComments
	,ErrorDate
	,TradeID
	,LastModifiedUser
	,LastModifiedDate
FROM $(ExcelsiorDB)..TBL_EIS_TR_TradeError

SET IDENTITY_INSERT TBL_TR_TradeError OFF

INSERT INTO TBL_TR_TradeRestrictionSnapshot
SELECT EventID
	,EventAccountID
	,EventStatusID
	,TradeRestrictionID
	,AccountID
	,TradeRestrictionType
	,SecuritySymbol
	,TradeRes.Comments
	,CreatedBy
	,StartDate
	,EndDate
FROM $(ExcelsiorDB)..TBL_EIS_TR_TradeRestriction_Snapshot TradeRes

SET IDENTITY_INSERT TBL_TR_TradeSubmission ON

INSERT INTO TBL_TR_TradeSubmission (
	SubmissionID
	,SubmissionType
	,SubmissionFile
	,SubmissionDate
	,SubmissionUserID
	)
SELECT SubmissionID
	,SubmissionType
	,SubmissionFile
	,SubmissionDate
	,SubmissionUser
FROM $(ExcelsiorDB)..TBL_EIS_TR_TradeSubmission

SET IDENTITY_INSERT TBL_TR_TradeSubmission OFF

INSERT INTO TBL_TR_TSheetTradeProposal (
	EventID
	,EventAccountID
	,SecurityTypeCode
	,SecurityTypeOrder
	,SecurityTypeDescription
	,SecuritySymbol
	,SecurityDescription
	,TSheetTradeForReview
	,AssetClass
	,TargetPercentage
	,AdjustedPercentage
	,TotalDollarChange
	,TrancheDollarChange
	,TPlusOne
	,TPlusTwo
	,TPlusThree
	,SubmissionType
	,SellAll
	,TradeProposedDollarAmount
	,TradeProposedQuantity
	,IsAdd
	,IsSubstitute
	,IsExclude
	,IsAboveTheLine
	,LastModifiedUserID
	,LastModifiedDate
	)
SELECT TradeProp.EventID
	,TradeProp.EventAccountID
	,SecurityTypeCode
	,SecurityTypeOrder
	,SecurityTypeDescription
	,SecuritySymbol
	,SecurityDescription
	,TSheetTradeForReview
	,AssetClass
	,TargetPercentage
	,AdjustedPercentage
	,TotalDollarChange
	,TrancheDollarChange
	,TPlusOne
	,TPlusTwo
	,TPlusThree
	,SubmissionType
	,SellAll
	,TradeProposedDollarAmount
	,TradeProposedQuantity
	,IsAdd
	,IsSubstitute
	,IsExclude
	,IsAboveTheLine
	,LastModifiedUser
	,TradeProp.LastModifiedDate
FROM $(ExcelsiorDB)..TBL_EIS_TR_TSheetTradeProposal TradeProp
INNER JOIN TBL_TR_EventAccount EvntAcc
	ON EvntAcc.EventAccountID = TradeProp.EventAccountID

SET IDENTITY_INSERT TBL_TR_TSheetLot ON

INSERT INTO TBL_TR_TSheetLot (
	TSheetLotID
	,LotID
	,EventID
	,EventAccountID
	,SecurityTypeCode
	,SecuritySymbol
	,TSheetTradeForReview
	,Industry
	,CostBasis
	,PortfolioCode
	,UnitCost
	,AquisitionDate
	,SharePrice
	,Marketvalue
	,Quantity
	,UnrealizedGainLossDollars
	,UnrealizedGainLossPercentage
	,LastModifiedUserID
	,LastModifiedDate
	)
SELECT TSheetLotID
	,LotID
	,TSLot.EventID
	,TSLot.EventAccountID
	,TSLot.SecurityTypeCode
	,TSLot.SecuritySymbol
	,TSLot.TSheetTradeForReview
	,Industry
	,CostBasis
	,PortfolioCode
	,UnitCost
	,AquisitionDate
	,SharePrice
	,Marketvalue
	,Quantity
	,UnrealizedGainLossDollars
	,UnrealizedGainLossPercentage
	,LastModifiedUser
	,TSLot.LastModifiedDate
FROM $(ExcelsiorDB)..TBL_EIS_TR_TSheetLots TSLot
INNER JOIN TBL_TR_TSheetTradeProposal TSProp
	ON TSLot.EventAccountID = TSProp.EventAccountID
		AND TSLot.EventID = TSProp.EventID
		AND TSLot.SecurityTypeCode = TSProp.SecurityTypeCode
		AND TSLot.SecuritySymbol = TSProp.SecuritySymbol
		AND TSLot.TSheetTradeForReview = TSProp.TSheetTradeForReview

SET IDENTITY_INSERT TBL_TR_TSheetLot OFF

INSERT INTO TBL_TR_STG_AccountGroup
SELECT GroupName
	,GroupType
	,CreationDate
	,LastRefreshed
	,GroupDescription
	,Refreshable
	,QueryName
FROM $(ExcelsiorDB)..AccountGroup

INSERT INTO TBL_TR_GroupXAccount
SELECT GroupName
	,AdventID
FROM $(ExcelsiorDB)..GroupXAccount GroupX
INNER JOIN $(ExcelsiorDB)..DEFERREDGIFTACCOUNT Account
	ON GroupX.AccountID = Account.AccountID

INSERT INTO TBL_TR_TradeImport (
	TradeID
	,FSITypeCode
	,TradeIDEnfu
	,SchwabMasterID
	,CustodialAccount
	,CustomerAccountNumber
	,SecuritySymbol
	,TradeType
	,DollarTradeAmt
	,TradeQuantity
	,ApprovedDate
	,FileGenDate
	,ReconcileDate
	,TradeDate
	,SettleDate
	,UserID
	,Source
	,TradeStatus
	,SecurityType
	,TradeIDTRex
	,ExecutionPrice
	,Broker
	,SubmissionID
	)
SELECT TradeID
	,TrImport.FSITypeCode
	,TradeID_Enfu
	,TrImport.SchwabMasterID
	,TrImport.CustodialAccount
	,Account.AdventID
	,SecuritySymbol
	,TradeType
	,DollarTradeAmt
	,TradeQuantity
	,ApprovedDate
	,FileGenDate
	,ReconcileDate
	,TradeDate
	,SettleDate
	,UserID
	,Source
	,TradeStatus
	,SecurityType
	,TradeID_TRex
	,ExecutionPrice
	,Broker
	,SubmissionID
FROM $(ExcelsiorDB)..TradeImport TrImport
INNER JOIN $(ExcelsiorDB)..DEFERREDGIFTACCOUNT Account
	ON Account.AccountID = TrImport.AccountID

INSERT INTO TBL_TR_TradingAssetType
SELECT FundingAssetTypeID
	,FundingAssetTypeName
	,InvestmentActionID
FROM $(ExcelsiorDB)..TradingAssetType

INSERT INTO TBL_TR_TradingInvestmentAction
SELECT InvestmentActionID
	,InvestmentName
FROM $(ExcelsiorDB)..TradingInvestmentAction

INSERT INTO TBL_TR_TradingInvestmentPolicy
SELECT InvestmentPolicyID
	,InvestmentPolicyName
	,InvestmentActionID
FROM $(ExcelsiorDB)..TradingInvestmentPolicy

INSERT INTO TBL_TR_TradingPolicy
SELECT TradingPolicyID
	,TriggerActionID
	,InvestmentActionID
	,InvestmentPolicyID
	,FundingAssetTypeID
	,FullyInvestInFunding
	,FullyInvesttToRisk
	,UseNewtrustRule
	,NumberOfTranches
	,TranchesInDays
	,TranchesTimeFrame
	,FMVIsInDollar
	,FMVLowCondition
	,FMVLow
	,FMVHighCondition
	,FMVHigh
	,Groupid
FROM $(ExcelsiorDB)..TradingPolicy

INSERT INTO TBL_TR_TradingTriggerAction
SELECT TriggerActionID
	,TriggerName
FROM $(ExcelsiorDB)..TradingTriggerAction

--INSERT INTO TBL_TR_Asset
--SELECT SecuritySymbol
--	,FSITypeCode
--	,AssetType
--	,BondMatureDate
--	,IndustryID
--	,DelayTradeDate
--	,IsFund
--	,Dividend
--	,PriceStatusCode
--	,AssetClassName
--	,AssetName
--	,CurrentPrice
--	,LastUpdate
--	,SwapSecuritySymbol
--	,IsKCOAsset
--	,LastPaymentDate
--	,CouponDelay
--FROM $(ExcelsiorDB)..Asset
--INSERT INTO TBL_TR_AssetClassList
--SELECT AssetClassName
--	,AssetClassCategory
--	,AssetClassType
--	,ClassDisplayOrder
--	,AssetClassDescription
--	,AssetClassScaler
--	,AssetClassEquivalent
--FROM $(ExcelsiorDB)..AssetClassList
INSERT INTO TBL_TR_AXYSInfoImportLog
SELECT ImportLogID
	,DestinationTable
	,LastUpdated
	,FileDescription
	,FileName
	,RowsLoaded
FROM $(ExcelsiorDB)..ImportLog

SET IDENTITY_INSERT TBL_TR_AUDIT_Event ON

INSERT INTO TBL_TR_AUDIT_Event (
	AuditSequence
	,AuditUserId
	,AuditDatetime
	,AuditFlag
	,AuditTable
	,AuditDetail
	,EventID
	,EventStatusID
	,EventName
	,EventSource
	,Comments
	,CancelComment
	,AXYSPositionDate
	,LoadPaymentsDataUpTo
	,ImposeBenePayment
	,ReviewAXYSPositionDate
	,ReviewLoadPaymentsDataUpTo
	,ReviewImposeBenePayment
	,Source
	,SourceFille
	,AssignedTraderID
	,CombinedAccount
	,CreatedUserID
	,CreatedDate
	,LastModifiedUserID
	,LastModifiedDate
	,DeletedUserID
	,TradeComment
	)
SELECT AUDIT_SEQUENCE
	,AUDIT_USER_ID
	,AUDIT_DATETIME
	,AUDIT_FLAG
	,AUDIT_TABLE
	,AUDIT_DETAILS
	,EventID
	,EventStatusID
	,EventName
	,EventSource
	,Comments
	,CancelComments
	,AXYSPositionDate
	,LoadPaymentsDataUpTo
	,ImposeBenePayments
	,ReviewAXYSPositionDate
	,ReviewLoadPaymentsDataUpTo
	,ReviewImposeBenePayments
	,Source
	,SourceFille
	,AssignedTrader
	,CombinedAccounts
	,CreatedUser
	,CreateDate
	,LastModifiedUser
	,LastModifiedDate
	,DELETED_USER_ID
	,TradeComment
FROM $(ExcelsiorDB)..TBL_EIS_TR_AUDIT_Event

SET IDENTITY_INSERT TBL_TR_AUDIT_Event OFF
-------------------------------------
-- TBL_TR_AUDIT_EventTracker insert script 
SET IDENTITY_INSERT TBL_TR_AUDIT_EventTracker ON

INSERT INTO TBL_TR_AUDIT_EventTracker (
	AuditSequence
	,AuditUserID
	,AuditDatetime
	,AuditFlag
	,AuditTable
	,AuditDetail
	,EventTrackerID
	,EventStatusID
	,EventID
	,ModifiedDate
	,ModifiedUserID
	,DeletedUserID
	)
SELECT AUDIT_SEQUENCE
	,AUDIT_USER_ID
	,AUDIT_DATETIME
	,AUDIT_FLAG
	,AUDIT_TABLE
	,AUDIT_DETAILS
	,EventTrackerID
	,EventStatusID
	,EventID
	,ModifiedDate
	,ModifiedUser
	,DELETED_USER_ID
FROM $(ExcelsiorDB)..TBL_EIS_TR_AUDIT_EventTracker

SET IDENTITY_INSERT TBL_TR_AUDIT_EventTracker OFF
-- TBL_TR_AUDIT_EventAccount insert script
SET IDENTITY_INSERT TBL_TR_AUDIT_EventAccount ON

INSERT INTO TBL_TR_AUDIT_EventAccount (
	AuditSequence
	,AuditUserID
	,AuditDatetime
	,AuditFlag
	,AuditTable
	,AuditDetail
	,EventAccountID
	,EventID
	,CustomerAccountNumber
	,AccountOrder
	,AccountName
	,AccountType
	,PayoutRate
	,AnnuityAmount
	,SLMasterAccount
	,Managed
	,Custodian
	,CustodianAccountNumber
	,InvestmentTypeCode
	,TaxStatus
	,TradeDiscretion
	,TradeStatusCode
	,FSIOption
	,InvestmentComment
	,TrancheStatusCode
	,ImposedBenePayment
	,ObjectiveCode
	,PoliByCli
	,LotAccountingCode
	,ScheduledAmount
	,AnnualPayment
	,PaymentDate
	,IMlevel2Count
	,IMlevel1Count
	,LastModifiedUserID
	,LastModifiedDate
	,DeletedUserID
	)
SELECT DISTINCT AUDIT_SEQUENCE
	,AUDIT_USER_ID
	,AUDIT_DATETIME
	,AUDIT_FLAG
	,AUDIT_TABLE
	,AUDIT_DETAILS
	,EventAccountID
	,EventID
	,AdventID
	,AccountOrder
	,AccountName
	,AccMstr.AccountTypeCode
	,PayoutRate
	,AnnuityAmount
	,SLMasterAccount
	,Managed
	,Custodian
	,CustodianAccountNumber
	,InvestmentTypeCode
	,TaxStatus
	,TradeDiscretion
	,TradeStatusCode
	,FSIOption
	,InvestmentComment
	,TrancheStatusCode
	,ImposedBenePayments
	,ObjectiveCode
	,PoliByCli
	,LotAccountingCode
	,ScheduledAmount
	,AnnualPayment
	,PaymentDate
	,IMlevel2Count
	,IMlevel1Count
	,LastModifiedUser
	,LastModifiedDate
	,DELETED_USER_ID
FROM $(ExcelsiorDB)..TBL_EIS_TR_AUDIT_EventAccount EvntAcc
	INNER JOIN $(InnoTrustDB)..AccountMaster AccMstr
 ON EvntAcc.ADVENTID = AccMstr.CustomerAccountNumber

SET IDENTITY_INSERT TBL_TR_AUDIT_EventAccount OFF
-- TBL_TR_AUDIT_ReviewEventAccount Insert Script
SET IDENTITY_INSERT TBL_TR_AUDIT_ReviewEventAccount ON

INSERT INTO TBL_TR_AUDIT_ReviewEventAccount (
	AuditSequence
	,AuditUserID
	,AuditDatetime
	,AuditFlag
	,AuditTable
	,AuditDetail
	,EventAccountID
	,AccountType
	,PayoutRate
	,AnnuityAmount
	,SLMasterAccount
	,Managed
	,Custodian
	,CustodianAccountNumber
	,InvestmentTypeCode
	,TaxStatus
	,TradeDiscretion
	,TradeStatusCode
	,FSIOption
	,InvestmentComment
	,TrancheStatusCode
	,ImposedBenePayment
	,ObjectiveCode
	,PoliByCli
	,LotAccountingCode
	,ScheduledAmount
	,AnnualPayment
	,PaymentDate
	,IMlevel2Count
	,IMlevel1Count
	,LastModifiedUserID
	,LastModifiedDate
	,DeletedUserID
	)
SELECT DISTINCT AudRevEvntAcc.AUDIT_SEQUENCE
	,AudRevEvntAcc.AUDIT_USER_ID
	,AudRevEvntAcc.AUDIT_DATETIME
	,AudRevEvntAcc.AUDIT_FLAG
	,AudRevEvntAcc.AUDIT_TABLE
	,AudRevEvntAcc.AUDIT_DETAILS
	,AudRevEvntAcc.EventAccountID
	,AccMstr.AccountTypeCode
	,AudRevEvntAcc.PayoutRate
	,AudRevEvntAcc.AnnuityAmount
	,AudRevEvntAcc.SLMasterAccount
	,AudRevEvntAcc.Managed
	,AudRevEvntAcc.Custodian
	,AudRevEvntAcc.CustodianAccountNumber
	,AudRevEvntAcc.InvestmentTypeCode
	,AudRevEvntAcc.TaxStatus
	,AudRevEvntAcc.TradeDiscretion
	,AudRevEvntAcc.TradeStatusCode
	,AudRevEvntAcc.FSIOption
	,AudRevEvntAcc.InvestmentComment
	,AudRevEvntAcc.TrancheStatusCode
	,AudRevEvntAcc.ImposedBenePayments
	,AudRevEvntAcc.ObjectiveCode
	,AudRevEvntAcc.PoliByCli
	,AudRevEvntAcc.LotAccountingCode
	,AudRevEvntAcc.ScheduledAmount
	,AudRevEvntAcc.AnnualPayment
	,AudRevEvntAcc.PaymentDate
	,AudRevEvntAcc.IMlevel2Count
	,AudRevEvntAcc.IMlevel1Count
	,AudRevEvntAcc.LastModifiedUser
	,AudRevEvntAcc.LastModifiedDate
	,AudRevEvntAcc.DELETED_USER_ID
FROM $(ExcelsiorDB)..TBL_EIS_TR_AUDIT_ReviewEventAccount AudRevEvntAcc
	INNER JOIN $(ExcelsiorDB)..TBL_EIS_TR_EventAccount EvntAcc
		ON AudRevEvntAcc.EventAccountID = EvntAcc.EventAccountID 
	INNER JOIN $(InnoTrustDB)..AccountMaster AccMstr
		ON EvntAcc.ADVENTID = AccMstr.CustomerAccountNumber

SET IDENTITY_INSERT TBL_TR_AUDIT_ReviewEventAccount OFF
-- TBL_TR_AUDIT_TradeSubmission Insert Script 
SET IDENTITY_INSERT TBL_TR_AUDIT_TradeSubmission ON

INSERT INTO TBL_TR_AUDIT_TradeSubmission (
	AuditSequence
	,AuditUserID
	,AuditDatetime
	,AuditFlag
	,AuditTable
	,AuditDetail
	,SubmissionID
	,SubmissionType
	,SubmissionFile
	,SubmissionDate
	,SubmissionUserID
	,DeletedUserID
	)
SELECT AUDIT_SEQUENCE
	,AUDIT_USER_ID
	,AUDIT_DATETIME
	,AUDIT_FLAG
	,AUDIT_TABLE
	,AUDIT_DETAILS
	,SubmissionID
	,SubmissionType
	,SubmissionFile
	,SubmissionDate
	,SubmissionUser
	,DELETED_USER_ID
FROM $(ExcelsiorDB)..TBL_EIS_TR_AUDIT_TradeSubmission

SET IDENTITY_INSERT TBL_TR_AUDIT_TradeSubmission OFF
-- TBL_TR_AUDIT_TSheetApprovedTrade Insert Script
SET IDENTITY_INSERT TBL_TR_AUDIT_TSheetApprovedTrade ON

INSERT INTO TBL_TR_AUDIT_TSheetApprovedTrade (
	AuditSequence
	,AuditUserID
	,AuditDatetime
	,AuditFlag
	,AuditTable
	,AuditDetail
	,TradeID
	,EventID
	,EventAccountID
	,SecurityTypeCode
	,SecuritySymbol
	,TradeQuantity
	,DollarAmount
	,TradeDate
	,TradeType
	,ArchiveTradeID
	,ActiveTradeID
	,SubmissionType
	,SplitTrade
	,SubmittedQuantityTillDate
	,LastModifiedUserID
	,LastModifiedDate
	,DeletedUserID
	)
SELECT AUDIT_SEQUENCE
	,AUDIT_USER_ID
	,AUDIT_DATETIME
	,AUDIT_FLAG
	,AUDIT_TABLE
	,AUDIT_DETAILS
	,TradeID
	,EventID
	,EventAccountID
	,SecurityTypeCode
	,SecuritySymbol
	,TradeQuantity
	,DollarAmount
	,TradeDate
	,TradeType
	,ArchiveTradeID
	,ActiveTradeID
	,SubmissionType
	,SplitTrade
	,SubmittedQuantityTillDate
	,LastModifiedUser
	,LastModifiedDate
	,DELETED_USER_ID
FROM $(ExcelsiorDB)..TBL_EIS_TR_AUDIT_TSheetApprovedTrade

SET IDENTITY_INSERT TBL_TR_AUDIT_TSheetApprovedTrade OFF
--TBL_TR_AUDIT_TSheetLot Insert Script
SET IDENTITY_INSERT TBL_TR_AUDIT_TSheetLot ON

INSERT INTO TBL_TR_AUDIT_TSheetLot (
	AuditSequence
	,AuditUserID
	,AuditDatetime
	,AuditFlag
	,AuditTable
	,AuditDetail
	,TSheetLotID
	,LotID
	,EventID
	,EventAccountID
	,SecurityTypeCode
	,SecuritySymbol
	,TSheetTradeForReview
	,Industry
	,CostBasis
	,PortfolioCode
	,UnitCost
	,AquisitionDate
	,SharePrice
	,Marketvalue
	,Quantity
	,UnrealizedGainLossDollars
	,UnrealizedGainLossPercentage
	,LastModifiedUserID
	,LastModifiedDate
	,DeletedUserID
	)
SELECT AUDIT_SEQUENCE
	,AUDIT_USER_ID
	,AUDIT_DATETIME
	,AUDIT_FLAG
	,AUDIT_TABLE
	,AUDIT_DETAILS
	,TSheetLotID
	,LotID
	,EventID
	,EventAccountID
	,SecurityTypeCode
	,SecuritySymbol
	,TSheetTradeForReview
	,Industry
	,CostBasis
	,PortfolioCode
	,UnitCost
	,AquisitionDate
	,SharePrice
	,Marketvalue
	,Quantity
	,UnrealizedGainLossDollars
	,UnrealizedGainLossPercentage
	,LastModifiedUser
	,LastModifiedDate
	,DELETED_USER_ID
FROM $(ExcelsiorDB)..TBL_EIS_TR_AUDIT_TSheetLots

SET IDENTITY_INSERT TBL_TR_AUDIT_TSheetLot OFF
-- TBL_TR_AUDIT_TSheetTradeProposal Insert Script
SET IDENTITY_INSERT TBL_TR_AUDIT_TSheetTradeProposal ON

INSERT INTO TBL_TR_AUDIT_TSheetTradeProposal (
	AuditSequence
	,AuditUserID
	,AuditDatetime
	,AuditFlag
	,AuditTable
	,AuditDetail
	,EventID
	,EventAccountID
	,SecurityTypeCode
	,SecuritySymbol
	,TSheetTradeForReview
	,AssetClass
	,TargetPercentage
	,AdjustedPercentage
	,TotalDollarChange
	,TrancheDollarChange
	,TPlusOne
	,TPlusTwo
	,TPlusThree
	,SubmissionType
	,SellAll
	,TradeProposedDollarAmount
	,TradeProposedQuantity
	,IsAdd
	,IsSubstitute
	,IsExclude
	,IsAboveTheLine
	,LastModifiedUserID
	,LastModifiedDate
	,DeletedUserID
	)
SELECT AUDIT_SEQUENCE
	,AUDIT_USER_ID
	,AUDIT_DATETIME
	,AUDIT_FLAG
	,AUDIT_TABLE
	,AUDIT_DETAILS
	,EventID
	,EventAccountID
	,SecurityTypeCode
	,SecuritySymbol
	,TSheetTradeForReview
	,AssetClass
	,TargetPercentage
	,AdjustedPercentage
	,TotalDollarChange
	,TrancheDollarChange
	,TPlusOne
	,TPlusTwo
	,TPlusThree
	,SubmissionType
	,SellAll
	,TradeProposedDollarAmount
	,TradeProposedQuantity
	,IsAdd
	,IsSubstitute
	,IsExclude
	,IsAboveTheLine
	,LastModifiedUser
	,LastModifiedDate
	,DELETED_USER_ID
FROM $(ExcelsiorDB)..TBL_EIS_TR_AUDIT_TSheetTradeProposal

SET IDENTITY_INSERT TBL_TR_AUDIT_TSheetTradeProposal OFF

-- TBL_TR_AUDIT_TradeImport Insert Script
INSERT INTO TBL_TR_AUDIT_TradeImport (
	TradeID
	,FSITypeCode
	,TradeIDEnfu
	,SchwabMasterID
	,CustodialAccount
	,CustomerAccountNumber
	,SecuritySymbol
	,TradeType
	,DollarTradeAmt
	,TradeQuantity
	,ApprovedDate
	,FileGenDate
	,ReconcileDate
	,TradeDate
	,SettleDate
	,UserID
	,Source
	,TradeStatus
	,SecurityType
	,TradeIDTRex
	,ExecutionPrice
	,Broker
	,SubmissionID
	)
SELECT TradeID
	,AudTradeImp.FSITypeCode
	,TradeID_Enfu
	,AudTradeImp.SchwabMasterID
	,AudTradeImp.CustodialAccount
	,Account.AdventID
	,SecuritySymbol
	,TradeType
	,DollarTradeAmt
	,TradeQuantity
	,ApprovedDate
	,FileGenDate
	,ReconcileDate
	,TradeDate
	,SettleDate
	,UserID
	,Source
	,TradeStatus
	,SecurityType
	,TradeID_TRex
	,ExecutionPrice
	,Broker
	,SubmissionID
FROM $(ExcelsiorDB)..TBL_EIS_TR_AUDIT_TradeImport AudTradeImp
INNER JOIN $(ExcelsiorDB)..DEFERREDGIFTACCOUNT Account
	ON Account.AccountID = AudTradeImp.AccountID

-- TBL_TR_Asset table creation script
INSERT INTO TBL_TR_Asset (
	SecuritySymbol
	,FSITypeCode
	,AssetType
	,BondMatureDate
	,IndustryID
	,DelayTradeDate
	,IsFund
	,Dividend
	,PriceStatusCode
	,AssetClassName
	,AssetName
	,CurrentPrice
	,LastUpdate
	,SwapSecuritySymbol
	,IsKCOAsset
	,LastPaymentDate
	,CouponDelay
	)
SELECT SecuritySymbol
	,FSITypeCode
	,AssetType
	,BondMatureDate
	,IndustryID
	,DelayTradeDate
	,IsFund
	,Dividend
	,PriceStatusCode
	,AssetClassName
	,AssetName
	,CurrentPrice
	,LastUpdate
	,SwapSecuritySymbol
	,IsKCOAsset
	,LastPaymentDate
	,CouponDelay
FROM $(ExcelsiorDB)..Asset


-- TBL_TR_AssetClassList table creation script
INSERT INTO TBL_TR_AssetClassList (
	AssetClassName
	,AssetClassCategory
	,AssetClassType
	,ClassDisplayOrder
	,AssetClassDescription
	,AssetClassScaler
	,AssetClassEquivalent
	)
SELECT AssetClassName
	,AssetClassCategory
	,AssetClassType
	,ClassDisplayOrder
	,AssetClassDescription
	,AssetClassScaler
	,AssetClassEquivalent
FROM $(ExcelsiorDB)..AssetClassList


-- TBL_TR_BROKER Insert Script
SET IDENTITY_INSERT TBL_TR_BROKER ON

INSERT INTO TBL_TR_BROKER (BrokerID,BrokerName) VALUES (1,'Schwab')
INSERT INTO TBL_TR_BROKER (BrokerID,BrokerName) VALUES (2,'State Street')
INSERT INTO TBL_TR_BROKER (BrokerID,BrokerName) VALUES (3,'Mellon')
INSERT INTO TBL_TR_BROKER (BrokerID,BrokerName) VALUES (4,'AAM')
INSERT INTO TBL_TR_BROKER (BrokerID,BrokerName) VALUES (5,'Wells Fargo')
INSERT INTO TBL_TR_BROKER (BrokerID,BrokerName) VALUES (6,'BB T')
INSERT INTO TBL_TR_BROKER (BrokerID,BrokerName) VALUES (7,'MuniCenter')
INSERT INTO TBL_TR_BROKER (BrokerID,BrokerName) VALUES (8,'DuncanW')
INSERT INTO TBL_TR_BROKER (BrokerID,BrokerName) VALUES (9,'Fidelity')
INSERT INTO TBL_TR_BROKER (BrokerID,BrokerName) VALUES (10,'Seattle NW')
INSERT INTO TBL_TR_BROKER (BrokerID,BrokerName) VALUES (11,'UBS')
INSERT INTO TBL_TR_BROKER (BrokerID,BrokerName) VALUES (12,'HSEC')

SET IDENTITY_INSERT TBL_TR_BROKER OFF

-- TBL_TR_TSheetApprovedTrade Update Script

UPDATE TBL_TR_TSheetApprovedTrade
SET SubmissionType = 'Manual Orders'
WHERE SubmissionType = 'Fax'

UPDATE TBL_TR_TSheetApprovedTrade
SET SubmissionType = 'Schwab File'
WHERE SubmissionType = 'File'

UPDATE TBL_TR_TSheetApprovedTrade
SET SubmissionType = 'Executed Trades'
WHERE SubmissionType = 'Phone'

-- TBL_TR_TradeSubmission Update Script

UPDATE TBL_TR_TradeSubmission
SET SubmissionType = 'Manual Orders'
WHERE SubmissionType = 'Fax'

UPDATE TBL_TR_TradeSubmission
SET SubmissionType = 'Schwab File'
WHERE SubmissionType = 'File'

UPDATE TBL_TR_TradeSubmission
SET SubmissionType = 'Executed Trades'
WHERE SubmissionType = 'Phone'


-- TBL_TR_TSheetTradeProposal Update Script

UPDATE TBL_TR_TSheetTradeProposal
SET SubmissionType = 'Manual Orders'
WHERE SubmissionType = 'Fax'

UPDATE TBL_TR_TSheetTradeProposal
SET SubmissionType = 'Schwab File'
WHERE SubmissionType = 'File'

UPDATE TBL_TR_TSheetTradeProposal
SET SubmissionType = 'Executed Trades'
WHERE SubmissionType = 'Phone'

-- TBL_TR_TradeGroup Insert Script
SET IDENTITY_INSERT TBL_TR_TradeGroup ON

INSERT INTO TBL_TR_TradeGroup (
	GroupID
	,GroupName
	,CreatedUserID
	,CreateDate
	,LastModifiedUserID
	,LastModifiedDate
	)
SELECT GroupID
	,GroupName
	,CreatedUser
	,CreateDate
	,LastModifiedUser
	,LastModifiedDate
FROM $(ExcelsiorDB)..TBL_EIS_EX_TRADEGROUP

SET IDENTITY_INSERT TBL_TR_TradeGroup OFF
-- TBL_TR_TradeGroup Insert Script
SET IDENTITY_INSERT TBL_TR_TradeGroupAccount ON

INSERT INTO TBL_TR_TradeGroupAccount (
	GroupAccountID
	,GroupID
	,CustomerAccountNumber
	,CreatedUserID
	,CreateDate
	,LastModifiedUserID
	,LastModifiedDate
	)
SELECT GroupAccountID
	,GroupID
	,AdventID
	,CreatedUser
	,CreateDate
	,LastModifiedUser
	,LastModifiedDate
FROM $(ExcelsiorDB)..TBL_EIS_EX_TRADEGROUPACCOUNT

SET IDENTITY_INSERT TBL_TR_TradeGroupAccount OFF

-- TBL_TR_TradingPolicyGroup Insert Script
INSERT INTO TBL_TR_TradingPolicyGroup (
	GroupID
	,Groupname
	)
SELECT GroupID
	,Groupname
FROM $(ExcelsiorDB)..TradingPolicyGroup

-- TBL_TR_TradeRestrictionTypeMaster Insert Script
SET IDENTITY_INSERT TBL_TR_TradeRestrictionTypeMaster ON

INSERT INTO TBL_TR_TradeRestrictionTypeMaster (
	RestrictionTypeID
	,RestrictionType
	,LastModifiedUserID
	,LastModifiedDate
	,QCType
	)
SELECT RestrictionTypeID
	,RestrictionType
	,LastModifiedUser
	,LastModifiedDate
	,QCType
FROM $(ExcelsiorDB)..TBL_EIS_EX_TRADE_RESTRICTION_TYPE_MASTER

SET IDENTITY_INSERT TBL_TR_TradeRestrictionTypeMaster OFF
-- TBL_TR_TradeRestriction Insert Script
SET IDENTITY_INSERT TBL_TR_TradeRestriction ON

INSERT INTO TBL_TR_TradeRestriction (
	TradeRestrictionID
	,TradeRestrictionTypeID
	,CustomerAccountNumber
	,SecuritySymbol
	,RestrictionStartDate
	,RestrictionEndDate
	,RestrictionComment
	,CreatedUserID
	,CreateDate
	,LastModifiedUserID
	,LastModifiedDate
	,DeletedUserID
	,IsActive
	)
SELECT TradeRestrictionID
	,TradeRestrictionTypeID
	,Account.AdventID
	,SecuritySymbol
	,RestrictionStartDate
	,RestrictionEndDate
	,RestrictionComment
	,CreatedUser
	,CreateDate
	,ISNULL(LastModifiedUser,CreatedUser)
	,ISNULL(LastModifiedDate,CreateDate)
	,DeletedUser
	,IsActive
FROM $(ExcelsiorDB)..TBL_EIS_EX_TRADE_RESTRICTION TRRestr
INNER JOIN $(ExcelsiorDB)..DEFERREDGIFTACCOUNT Account
	ON Account.AccountID = TRRestr.AccountID

SET IDENTITY_INSERT TBL_TR_TradeRestriction OFF

-- TBL_TR_TradingPolicyLinkage Insert Script
INSERT INTO TBL_TR_TradingPolicyLinkage (
	LinkID
	,TradingPolicyID
	,TPLinkTypeID
	)
SELECT Clnt.BriefName
	,TradingPolicyID
	,TPLinkTypeID
FROM $(ExcelsiorDB)..TradingPolicyLinkage PolcyLink
INNER JOIN $(ExcelsiorDB)..CLIENT Clnt
	ON Clnt.ClientID = PolcyLink.LinkID


UPDATE TBL_TR_Event
SET source = 'Tr-Ex'
WHERE source = 'TRex'

UPDATE TBL_TR_TradeImport
SET source = 'Tr-Ex'
WHERE source = 'TRex'

UPDATE TBL_TR_AUDIT_Event
SET source = 'Tr-Ex'
WHERE source = 'TRex'

UPDATE TBL_TR_AUDIT_TradeImport
SET source = 'Tr-Ex'
WHERE source = 'TRex'

