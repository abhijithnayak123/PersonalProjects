SET NOCOUNT ON

SET IDENTITY_INSERT TBL_OP_Filter ON

INSERT INTO TBL_OP_Filter (
	FilterID
	,[FilterDescription]
	,[IsSigCash]
	,[IsError]
	,[IsReCon]
	,[IsActive]
	)
SELECT FilterID
	,[FilterDescription]
	,[IsSigCash]
	,[IsError]
	,[IsReCon]
	,[IsActive]
FROM $(ExcelsiorDB)..TBL_EIS_OP_Filter

SET IDENTITY_INSERT TBL_OP_Filter OFF
GO

SET IDENTITY_INSERT TBL_OP_FilterCriteriaAttribute ON;

INSERT INTO TBL_OP_FilterCriteriaAttribute (
	AttributeID
	,[FilterID]
	,[AttributeName]
	,[ExpectedValue]
	,[ExecutionOrder]
	,[IsActive]
	,[IgnoreCase]
	,[DataType]
	,[OperatorType]
	)
SELECT AttributeID
	,[FilterID]
	,[AttributeName]
	,[ExpectedValue]
	,[ExecutionOrder]
	,[IsActive]
	,[IgnoreCase]
	,[DataType]
	,[OperatorType]
FROM $(ExcelsiorDB)..TBL_EIS_OP_FilterCriteriaAttribute

SET IDENTITY_INSERT TBL_OP_FilterCriteriaAttribute OFF;
GO

SET IDENTITY_INSERT TBL_OP_FilterResultAttribute ON;

INSERT INTO TBL_OP_FilterResultAttribute (
	[FilterID]
	,[AttributeName]
	,[PassValue]
	,[IsActive]
	,[ResultAttributeID]
	,[CriteriaAttributeID]
	)
SELECT [FilterID]
	,[AttributeName]
	,[PassValue]
	,[IsActive]
	,[AttributeID]
	,[CriteriaAttributeID]
FROM $(ExcelsiorDB)..TBL_EIS_OP_FilterResultAttribute

SET IDENTITY_INSERT TBL_OP_FilterResultAttribute OFF;
GO

INSERT INTO TBL_OP_FilterType (
	[FilterTypeID]
	,[TypeName]
	)
SELECT FilterTypeID
	,TypeName
FROM $(ExcelsiorDB)..TBL_EIS_OP_FilterType
GO

INSERT INTO TBL_OP_FilterTypeLink (
	[FilterID]
	,[FilterTypeID]
	)
SELECT FilterID
	,FilterTypeID
FROM $(ExcelsiorDB)..TBL_EIS_OP_FilterTypeLink
GO

SET IDENTITY_INSERT TBL_OP_MissingSymbolStatus ON

INSERT INTO TBL_OP_MissingSymbolStatus (
	[StatusID]
	,[StatusName]
	)
SELECT StatusID
	,StatusName
FROM $(ExcelsiorDB)..TBL_EIS_OP_MissingSymbolsStatus
GO

SET IDENTITY_INSERT TBL_OP_MissingSymbolStatus OFF

INSERT INTO [TBL_OP_TransactionStatus] (
	[StatusID]
	,[StatusDescription]
	)
SELECT StatusID
	,StatusDescription
FROM $(ExcelsiorDB)..TBL_EIS_OP_TransactionStatus
GO

INSERT INTO TBL_OP_UserAuditFileFolder (
	[UserID]
	,[AuditFileFolder]
	,[IsActive]
	,[ExceptionFileFolder]
	)
SELECT [UserID]
	,[AuditFileFolder]
	,[IsActive]
	,[ExceptionFileFolder]
FROM $(ExcelsiorDB)..TBL_EIS_OP_UserAuditFileFolder
GO

SET IDENTITY_INSERT TBL_OP_ResolutionResponse ON

INSERT INTO TBL_OP_ResolutionResponse (
	[ResponseID]
	,[ResponseDescription]
	)
SELECT ResponseID
	,ResponseDescription
FROM $(ExcelsiorDB)..TBL_EIS_OP_ResolutionResponse
GO

SET IDENTITY_INSERT TBL_OP_ResolutionResponse OFF
SET IDENTITY_INSERT TBL_OP_TransactionDataImport ON;

INSERT INTO TBL_OP_TransactionDataImport (
	RunID
	,[RunStartDateTime]
	,[RunEndDateTime]
	,[NumberOfTransactions]
	,[RunUserID]
	)
SELECT RunID
	,[RunStartDateTime]
	,[RunEndDateTime]
	,[NumberOfTransactions]
	,[RunUser]
FROM $(ExcelsiorDB)..TBL_EIS_OP_TransactionDataImport

SET IDENTITY_INSERT TBL_OP_TransactionDataImport OFF;
GO

SET IDENTITY_INSERT TBL_OP_Custodian ON

INSERT INTO TBL_OP_Custodian (
	[CustodianID]
	,[CustodianCode]
	,[CustodianName]
	,[TransactionFileFolder]
	,[IsActive]
	)
SELECT CustodianID
	,CustodianCode
	,CustodianName
	,TransactionFileFolder
	,IsActive
FROM $(ExcelsiorDB)..TBL_EIS_OP_Custodian
GO

SET IDENTITY_INSERT TBL_OP_Custodian OFF
SET IDENTITY_INSERT TBL_OP_Transaction ON;

INSERT INTO TBL_OP_Transaction (
	TransactionID
	,RunID
	,TransactionCode
	,TransactionDate
	,TradeDate
	,TransactionComment
	,TransactionDescription
	,AccountFMV
	,OrigninalCostDate
	,TradeAmount
	,SecurityType
	,SecuritySymbol
	,SourceType
	,SourceSymbol
	,TradeQuantity
	,LotLocation
	,Broker
	,Commission
	,LotID
	,CustomerAccountNumber
	,AXYSComment
	,AXYSPostDate
	,AXYSTransactionID
	,UserDefinedCode
	,IsError
	,IsRecon
	,IsSigCash
	,IsManagedAccount
	,CurrentErrorResponse
	,CurrentReconResponse
	,CurrentErrorStatus
	,CurrentReconStatus
	,AccountType
	,SecurityUserDefIndustryCode
	--,RecordVersion
	,LastModifiedUserID
	,LastModifiedDate
	)
SELECT TransactionID
	,RunID
	,TransactionCode
	,TransactionDate
	,TradeDate
	,TransactionComment
	,TransactionDescription
	,AccountFMV
	,OrigninalCostDate
	,TradeAmount
	,SecurityType
	,SecuritySymbol
	,SourceType
	,SourceSymbol
	,TradeQuantity
	,LotLocation
	,Broker
	,Commission
	,LotID
	,AdventID
	,AXYSComment
	,AXYSPostDate
	,AXYSTransactionID
	,UserDefinedCode
	,IsError
	,IsRecon
	,IsSigCash
	,IsManagedAccount
	,CurrentErrorResponse
	,CurrentReconResponse
	,CurrentErrorStatus
	,CurrentReconStatus
	,AccountType
	,SecurityUserDefIndustryCode
	--,RecordVersion
	,LastModifiedUser
	,LastModifiedDate
FROM $(ExcelsiorDB)..TBL_EIS_OP_Transaction

SET IDENTITY_INSERT TBL_OP_Transaction OFF;
GO

SET IDENTITY_INSERT TBL_OP_ReconciliationGroup ON;

INSERT INTO TBL_OP_ReconciliationGroup (
	ReconciliationGroupID
	,Comments
	)
SELECT ReconciliationGroupID
	,Comments
FROM $(ExcelsiorDB)..TBL_EIS_OP_ReconciliationGroup

SET IDENTITY_INSERT TBL_OP_ReconciliationGroup OFF;
GO

SET IDENTITY_INSERT TBL_OP_TransactionReconciliation ON;

INSERT INTO TBL_OP_TransactionReconciliation (
	ReconciliationID
	,ReConDateTime
	,TradeID
	,TransactionID
	,ReConUserID
	,ReconciliationGroupID
	)
SELECT ReconciliationID
	,ReConDateTime
	,TradeID
	,TransactionID
	,ReConUser
	,ReconciliationGroupID
FROM $(ExcelsiorDB)..TBL_EIS_OP_TransactionReconciliation

SET IDENTITY_INSERT TBL_OP_TransactionReconciliation OFF;
GO

SET IDENTITY_INSERT TBL_OP_TransactionTracker ON;

INSERT INTO TBL_OP_TransactionTracker (
	TransactionTrackerID
	,ModifiedDate
	,TransactionID
	,ErrorStatusID
	,ReconStatusID
	,ErrorResponseID
	,ReconResponseID
	,ModifiedUserID
	)
SELECT TransactionTrackerID
	,ModifiedDate
	,TransactionID
	,ErrorStatusID
	,ReconStatusID
	,ErrorResponseID
	,ReconResponseID
	,ModifiedUser
FROM $(ExcelsiorDB)..TBL_EIS_OP_TransactionTracker

SET IDENTITY_INSERT TBL_OP_TransactionTracker OFF;
GO

SET IDENTITY_INSERT TBL_OP_DataImport ON;

INSERT INTO TBL_OP_DataImport (
	RunID
	,RunDate
	,RunUserID
	,TradeBlottersGenerated
	)
SELECT RunID
	,RunDate
	,RunUser
	,TradeBlottersGenerated
FROM $(ExcelsiorDB)..TBL_EIS_OP_DataImport

SET IDENTITY_INSERT TBL_OP_DataImport OFF;
GO

SET IDENTITY_INSERT TBL_OP_CustodialDataImport ON

INSERT INTO TBL_OP_CustodialDataImport (
	CustodialDataImportID
	,RunID
	,CustodianID
	,ToPostTrnFile
	,ParsedTransactionsCount
	,LOTransactionsCount
	,AutoMatchedLOTransactionsCount
	,ManualMatchedLOTransactionsCount
	,ManualResolvedLOTransactionsCount
	,NonExceptionsTransactionsCount
	,ExceptionsTransactionsCount
	,ImportDate
	,ImportUserID
	)
SELECT CustodialDataImportID
	,RunID
	,CustodianID
	,ToPostTrnFile
	,ParsedTransactionsCount
	,LOTransactionsCount
	,AutoMatchedLOTransactionsCount
	,ManualMatchedLOTransactionsCount
	,ManualResolvedLOTransactionsCount
	,NonExceptionsTransactionsCount
	,ExceptionsTransactionsCount
	,ImportDate
	,ImportUser
FROM $(ExcelsiorDB)..TBL_EIS_OP_CustodialDataImport

SET IDENTITY_INSERT TBL_OP_CustodialDataImport OFF
GO

INSERT INTO TBL_OP_CustodialDataImportMissingSymbol (
	RunID
	,MissingSecuritySymbol
	,CorrectedSecuritySymbol
	,CorrectedUserID
	,MissingStatus
	)
SELECT RunID
	,MissingSecuritySymbol
	,CorrectedSecuritySymbol
	,CorrectedUser
	,MissingStatus
FROM $(ExcelsiorDB)..TBL_EIS_OP_CustodialDataImportMissingSymbols
GO

SET IDENTITY_INSERT TBL_OP_CustodialDataImportTransactionToResolve ON;

INSERT INTO TBL_OP_CustodialDataImportTransactionToResolve (
	CustomerAccountNumber
	,TranCode
	,CustodialDataImportID
	,TransactionID
	,TransactionComment
	,TransactionData
	,Amount
	,IsResolved
	,ResolutionComments
	,ResolutionUserID
	,CheckNumber
	,PaymentID
	,ExpenseCode
	,ChargeType
	,PaymentDate
	,PayeeName
	,TaxYear
	,TaxYearAware
	,ToReview
	--,RecordVersion
	)
SELECT AdventID
	,TranCode
	,CustodialDataImportID
	,TransactionID
	,TransactionComment
	,TransactionData
	,Amount
	,IsResolved
	,ResolutionComments
	,ResolutionUser
	,CheckNumber
	,PaymentID
	,ExpenseCode
	,ChargeType
	,PaymentDate
	,PayeeName
	,TaxYear
	,TaxYearAware
	,ToReview
	--,RecordVersion
FROM $(ExcelsiorDB)..TBL_EIS_OP_CustodialDataImportTransactionsToResolve

SET IDENTITY_INSERT TBL_OP_CustodialDataImportTransactionToResolve OFF;

-----------------------------------------------------------------------------------------------------------
INSERT INTO TBL_OP_ExpenseCode
SELECT 'benpmt'
	,'009'
	,'Beneficiary Income Payment'
	,0
	,0

INSERT INTO TBL_OP_ExpenseCode
SELECT 'benpmt'
	,'052'
	,'Beneficiary Income Payment'
	,0
	,0

INSERT INTO TBL_OP_ExpenseCode
SELECT 'benpmt'
	,'737'
	,'Beneficiary Income Payment'
	,0
	,0

INSERT INTO TBL_OP_ExpenseCode
SELECT 'benpmt'
	,'570'
	,'Beneficiary Income Payment'
	,0
	,0

INSERT INTO TBL_OP_ExpenseCode
SELECT 'fbentx'
	,'691'
	,'Foreign withholding from ben pmt'
	,1
	,0

INSERT INTO TBL_OP_ExpenseCode
SELECT 'bentx'
	,'697'
	,'State withholding from ben pmt'
	,1
	,0

INSERT INTO TBL_OP_ExpenseCode
SELECT 'taxfi'
	,'595'
	,'Balance Due on Federal Income Tax'
	,0
	,0

INSERT INTO TBL_OP_ExpenseCode
SELECT 'fitwh'
	,'596'
	,'Federal Income Tax Withholding'
	,0
	,0

INSERT INTO TBL_OP_ExpenseCode
SELECT 'taxsi'
	,'601'
	,'Balance Due on State Income Tax'
	,0
	,0

INSERT INTO TBL_OP_ExpenseCode
SELECT 'taxsi'
	,'602'
	,'Installment on State Income Tax'
	,0
	,0
	-----------------------------------------------------------------------------------------------------------------
