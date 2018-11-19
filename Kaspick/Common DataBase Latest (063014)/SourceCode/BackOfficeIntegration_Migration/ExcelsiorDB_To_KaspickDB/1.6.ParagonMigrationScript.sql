SET NOCOUNT ON

-----------------------------------------
DECLARE @BeneID INT

IF OBJECT_ID(N'TEMPDB.[DBO].[#Tmp_BeneficiaryPayment]') IS NOT NULL
BEGIN
	DROP TABLE [DBO].[#Tmp_BeneficiaryPayment]
END

--IF OBJECT_ID(N'TEMPDB.[DBO].[#Tmp_PartBeneLookup]') IS NOT NULL
--BEGIN
--	DROP TABLE [DBO].[#Tmp_PartBeneLookup]
--END

CREATE TABLE #Tmp_BeneficiaryPayment (
	[PaymentID] INT NOT NULL
	,[CustomerAccountNumber] CHAR(14) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
	,[ContactID] INT NOT NULL
	,[ContactRoleCode] INT NOT NULL
	,[InstructionID] INT NOT NULL
	,[BeneficiaryDistributionID] INT NOT NULL
	,[TransactionNumber] CHAR(11) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
	,[DocumentNumber] VARCHAR(15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	,[AccountType] VARCHAR(20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	,[ManagerCode] CHAR(4) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
	,[PaymentDate] DATETIME NULL
	,[TaxYear] INT NULL
	,[PayeeName] VARCHAR(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	,[PayeeAddress] VARCHAR(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	,[PaymentAmount] MONEY NULL
	,[LastName] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
	,[SourceBankName] VARCHAR(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	,[SourceBankAddress] VARCHAR(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	,[SourceBankABA] VARCHAR(20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	,[FractionalRoutingCode] VARCHAR(20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	,[PayorName] VARCHAR(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	,[PayorAddress] VARCHAR(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	,[SourceAccount] VARCHAR(20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	,[DestBankName] VARCHAR(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	,[DestBankAddress] VARCHAR(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	,[DestBankABA] VARCHAR(20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	,[DestAccount] VARCHAR(20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	,[Memo] VARCHAR(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	,[VoidDate] DATETIME NULL
	,[ReissueOf] VARCHAR(15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	,[ReissueAs] VARCHAR(15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	,[DestAccountType] VARCHAR(10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	,[TaxCode] INT NULL
	,[ClearDate] DATETIME NULL
	,[ClearMethod] CHAR(1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	,[ChargeType] CHAR(1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	,[DirectDepositFlag] BIT NULL
	,[IsBackBuilt] BIT NULL
	,[Status] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
	,[PostDate] DATETIME NULL
	,[Comment] VARCHAR(1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	,[AcomPrintDate] DATETIME NULL
	,[ACHFileName] VARCHAR(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	,[Withholding] MONEY NULL
	,[WireSubmitted] DATETIME NULL
	,[WireSubmittedBy] VARCHAR(200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	,[MailingAddress] VARCHAR(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	,[SeparateCheck] BIT NOT NULL
	,[SeparateCheckAddress] VARCHAR(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	,[AllianceNumber] CHAR(15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
	,[GiftAccountName] VARCHAR(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	,[GSOInfo] VARCHAR(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	,[PrintAdvice] INT NOT NULL
	,[PaymentMethod] VARCHAR(20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
	,[PaymentFreq] INT NULL
	,[PaidforContactID] INT NULL
	,[DisplayVoidPaymentsInWeb] BIT NULL
	,[CreatedDate] DATETIME NOT NULL
	,[CreatedBy] INT NOT NULL
	,[ModifiedDate] DATETIME NOT NULL
	,[ModifiedBy] INT NOT NULL
	,[RecordVersion] TIMESTAMP NOT NULL
	,[WireAuthorizedDate] DATETIME NULL
	,[WireAuthorizedBy] VARCHAR(200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	,[WirePDFFileName] VARCHAR(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	,[WirePDFFileCreationDate] DATETIME NULL
	)

--SELECT BENEFICIARYID
--INTO #Tmp_PartBeneLookup
--FROM $(MappingDB)..TBL_PartBeneContactLookUp
--GROUP BY BENEFICIARYID
--HAVING COUNT(*) > 1

SELECT @BeneID = ID
FROM SYN_IT_ContactRoleCodes
WHERE contactrolecodedeSc = 'Beneficiary'

-------------------------------------------------

--TBL_PP_ValidationRule table Insert script
--Insert rules into TBL_PP_ValidationRule Table

SET IDENTITY_INSERT TBL_PP_ValidationRule ON

Insert into TBL_PP_ValidationRule(RuleID,RuleCategory,RuleName,ExecutionOrder,ResultType,DisplayMessage,RuleLeadTime,IsActive,StopValidationIfFails,PaymentStatus,InvokeMethod,SPName) Values(3,'Beneficiary payment','Account inactive',20,'Error','Beneficiary''s account is no longer active',14,1,0,'Scheduled','KaspickDB','USP_PP_GetValidationEngineBeneficiaryAccountStatus')
Insert into TBL_PP_ValidationRule(RuleID,RuleCategory,RuleName,ExecutionOrder,ResultType,DisplayMessage,RuleLeadTime,IsActive,StopValidationIfFails,PaymentStatus,InvokeMethod,SPName) Values(4,'Beneficiary payment','Beneficiary inactive',25,'Error','Beneficiary is no longer active',14,1,0,'Scheduled','KaspickDB','USP_PP_GetValidationEngineBeneficiaryStatus')
Insert into TBL_PP_ValidationRule(RuleID,RuleCategory,RuleName,ExecutionOrder,ResultType,DisplayMessage,RuleLeadTime,IsActive,StopValidationIfFails,PaymentStatus,InvokeMethod,SPName) Values(5,'Beneficiary payment','Proxy beneficiary linked to inactive beneficiary',30,'Error','Beneficiary is no longer active(not strictly same as dead bene)',14,1,0,'Scheduled','KaspickDB','USP_PP_GetValidationEngineBeneficiaryProxyStatus')
Insert into TBL_PP_ValidationRule(RuleID,RuleCategory,RuleName,ExecutionOrder,ResultType,DisplayMessage,RuleLeadTime,IsActive,StopValidationIfFails,PaymentStatus,InvokeMethod,SPName) Values(19,'Beneficiary payment','Beneficiary address missing',55,'Error','Missing beneficiary Alternate/Payment/Legal address',14,1,0,'Scheduled','KaspickDB','USP_PP_GetValidationEngineBeneficiaryAddressType')
Insert into TBL_PP_ValidationRule(RuleID,RuleCategory,RuleName,ExecutionOrder,ResultType,DisplayMessage,RuleLeadTime,IsActive,StopValidationIfFails,PaymentStatus,InvokeMethod,SPName) Values(23,'Beneficiary payment','$0 beneficiary payment',65,'Error','Outstanding beneficiary payment amount must be set to a valid amount to be processed',7,1,0,'Scheduled','KaspickDB','USP_PP_GetValidationEngineBeneficiaryPaymentAmount')
Insert into TBL_PP_ValidationRule(RuleID,RuleCategory,RuleName,ExecutionOrder,ResultType,DisplayMessage,RuleLeadTime,IsActive,StopValidationIfFails,PaymentStatus,InvokeMethod,SPName) Values(29,'Beneficiary payment','Possibly stale payment calculation',70,'Warning','Scheduled beneficiary payment may be incorrect due to recent gift addition',7,1,0,'Scheduled','KaspickDB','USP_PP_GetValidationEngineBeneficiaryGiftAdditionDetail')
Insert into TBL_PP_ValidationRule(RuleID,RuleCategory,RuleName,ExecutionOrder,ResultType,DisplayMessage,RuleLeadTime,IsActive,StopValidationIfFails,PaymentStatus,InvokeMethod,SPName) Values(30,'Beneficiary payment','Outstanding beneficiary payment',75,'Warning','Outstanding beneficiary payment OnHold - must be posted or deleted by the ExpectedPostingDate',8,1,0,'Scheduled','KaspickDB','USP_PP_GetValidationEngineBeneficiaryOnHoldPayment')
Insert into TBL_PP_ValidationRule(RuleID,RuleCategory,RuleName,ExecutionOrder,ResultType,DisplayMessage,RuleLeadTime,IsActive,StopValidationIfFails,PaymentStatus,InvokeMethod,SPName) Values(31,'Beneficiary payment','Late Beneficiary Payment',80,'Error','Past due beneficiary payment - must be updated or deleted',7,1,0,'Scheduled','KaspickDB','USP_PP_GetValidationEngineBeneficiaryOnHoldPaymentOnCurrentDate')
Insert into TBL_PP_ValidationRule(RuleID,RuleCategory,RuleName,ExecutionOrder,ResultType,DisplayMessage,RuleLeadTime,IsActive,StopValidationIfFails,PaymentStatus,InvokeMethod,SPName) Values(33,'Beneficiary payment','Beneficiary Journal Payment',85,'Error','Journal payments must be manually posted.',3,1,0,'Scheduled','KaspickDB','USP_PP_GetValidationEngineBeneficiaryPaymentMethodForJournal')
Insert into TBL_PP_ValidationRule(RuleID,RuleCategory,RuleName,ExecutionOrder,ResultType,DisplayMessage,RuleLeadTime,IsActive,StopValidationIfFails,PaymentStatus,InvokeMethod,SPName) Values(35,'Beneficiary payment','Back Built Payment',5,'Error','Unposted backbuilt payment is outstanding against this account',14,1,1,'Scheduled','KaspickDB','USP_PP_GetValidationEngineBackBuiltStatus')
Insert into TBL_PP_ValidationRule(RuleID,RuleCategory,RuleName,ExecutionOrder,ResultType,DisplayMessage,RuleLeadTime,IsActive,StopValidationIfFails,PaymentStatus,InvokeMethod,SPName) Values(50,'Account','Account payment missing for designated period',101,'Warning','Active or in transition account is missing a normally scheduled payment',14,1,0,'Scheduled','KaspickDB','USP_PP_GetValidationEngineMissingPaymentAccount')
Insert into TBL_PP_ValidationRule(RuleID,RuleCategory,RuleName,ExecutionOrder,ResultType,DisplayMessage,RuleLeadTime,IsActive,StopValidationIfFails,PaymentStatus,InvokeMethod,SPName) Values(54,'Beneficiary payment','Unsupported Beneficiary Account Type',23,'Error','Unsupported account type for beneficiary payment',7,1,0,'Scheduled','KaspickDB','USP_PP_GetValidationEngineBeneficiaryUnsupportedAccountType')
Insert into TBL_PP_ValidationRule(RuleID,RuleCategory,RuleName,ExecutionOrder,ResultType,DisplayMessage,RuleLeadTime,IsActive,StopValidationIfFails,PaymentStatus,InvokeMethod,SPName) Values(56,'Beneficiary payment','Missing MW client payment rule',12,'Error','Missing MW client payment rule for account associated with this payment',20,1,0,'Scheduled','KaspickDB','USP_PP_GetValidationEngineBeneficiaryPaymentManagerMWareRule')
Insert into TBL_PP_ValidationRule(RuleID,RuleCategory,RuleName,ExecutionOrder,ResultType,DisplayMessage,RuleLeadTime,IsActive,StopValidationIfFails,PaymentStatus,InvokeMethod,SPName) Values(57,'Beneficiary payment','Pays Actual Account',32,'Error','Pays Actual Account',2,1,0,'Scheduled','KaspickDB','USP_PP_GetValidationEngineBeneficiaryAccountPaymentActualFlag')
Insert into TBL_PP_ValidationRule(RuleID,RuleCategory,RuleName,ExecutionOrder,ResultType,DisplayMessage,RuleLeadTime,IsActive,StopValidationIfFails,PaymentStatus,InvokeMethod,SPName) Values(58,'Beneficiary payment','Client with Account Type Payment Condition',11,'Warning','Client with Payment Condition',14,1,0,'Scheduled','KaspickDB','USP_PP_GetValidationEngineBeneficiaryManagerPaymentCondition')
Insert into TBL_PP_ValidationRule(RuleID,RuleCategory,RuleName,ExecutionOrder,ResultType,DisplayMessage,RuleLeadTime,IsActive,StopValidationIfFails,PaymentStatus,InvokeMethod,SPName) Values(59,'Beneficiary payment','Account Payment Condition',20,'Warning','Account with Payment Condition',14,1,0,'Scheduled','KaspickDB','USP_PP_GetValidationEngineBeneficiaryAccountPaymentCondition')
Insert into TBL_PP_ValidationRule(RuleID,RuleCategory,RuleName,ExecutionOrder,ResultType,DisplayMessage,RuleLeadTime,IsActive,StopValidationIfFails,PaymentStatus,InvokeMethod,SPName) Values(60,'Beneficiary payment','Beneficiary Payment Condition',24,'Warning','Beneficiary with Payment Condition',14,1,0,'Scheduled','KaspickDB','USP_PP_GetValidationEngineBeneficiaryPaymentCondition')
Insert into TBL_PP_ValidationRule(RuleID,RuleCategory,RuleName,ExecutionOrder,ResultType,DisplayMessage,RuleLeadTime,IsActive,StopValidationIfFails,PaymentStatus,InvokeMethod,SPName) Values(61,'Account','Missing Payment Frequency',104,'Warning','Account Missing Payment Frequency',0,1,0,'Scheduled','KaspickDB','USP_PP_GetValidationEngineAccountPaymentFrequency')
--Insert into TBL_PP_ValidationRule(RuleID,RuleCategory,RuleName,ExecutionOrder,ResultType,DisplayMessage,RuleLeadTime,IsActive,StopValidationIfFails,PaymentStatus,InvokeMethod,SPName) Values(62,'Beneficiary payment','Beneficiary Payment Variance',155,'Warning','Beneficiary Payment Variance',14,0,0,'Scheduled','KaspickDB','USP_PP_GetValidationEngineBeneficiaryPaymentExpectedVariance')
Insert into TBL_PP_ValidationRule(RuleID,RuleCategory,RuleName,ExecutionOrder,ResultType,DisplayMessage,RuleLeadTime,IsActive,StopValidationIfFails,PaymentStatus,InvokeMethod,SPName) Values(62,'Beneficiary payment','Beneficiary Payment Variance',155,'Warning','Beneficiary Payment Variance',14,0,0,'Scheduled','KaspickDB','')
Insert into TBL_PP_ValidationRule(RuleID,RuleCategory,RuleName,ExecutionOrder,ResultType,DisplayMessage,RuleLeadTime,IsActive,StopValidationIfFails,PaymentStatus,InvokeMethod,SPName) Values(63,'Beneficiary payment','Missing DDA#',52,'Error','Account Missing DDA Number',14,1,0,'Scheduled','KaspickDB','USP_PP_GetValidationEngineBeneficiaryPaymentMissingDDANumber')
Insert into TBL_PP_ValidationRule(RuleID,RuleCategory,RuleName,ExecutionOrder,ResultType,DisplayMessage,RuleLeadTime,IsActive,StopValidationIfFails,PaymentStatus,InvokeMethod,SPName) Values(64,'Account','Beneficiary Payment Missing for Designated Period',110,'Warning','Beneficiary Payment Missing for Designated Period',14,1,0,'Scheduled','KaspickDB','')
Insert into TBL_PP_ValidationRule(RuleID,RuleCategory,RuleName,ExecutionOrder,ResultType,DisplayMessage,RuleLeadTime,IsActive,StopValidationIfFails,PaymentStatus,InvokeMethod,SPName) Values(75,'Beneficiary payment','Wire missing approval',185,'Warning','Beneficiary Wire Request requires authorization signature(s) - payment status must be updated to "Wire Authorized"',5,1,0,'Posted','KaspickDB','USP_PP_GetValidationEngineWireBeneficiaryAuthorized')
Insert into TBL_PP_ValidationRule(RuleID,RuleCategory,RuleName,ExecutionOrder,ResultType,DisplayMessage,RuleLeadTime,IsActive,StopValidationIfFails,PaymentStatus,InvokeMethod,SPName) Values(78,'Beneficiary payment','Wire request generation failed',205,'Warning','Errors in wire instruction - Beneficiary wire request has not been generated',5,1,0,'Posted','KaspickDB','USP_PP_GetValidationEngineWireBeneficiaryFileGeneration')
Insert into TBL_PP_ValidationRule(RuleID,RuleCategory,RuleName,ExecutionOrder,ResultType,DisplayMessage,RuleLeadTime,IsActive,StopValidationIfFails,PaymentStatus,InvokeMethod,SPName) Values(86,'Beneficiary payment','Manually matched PIF/GAP payments must be reviewed',241,'Error','Manually matched PIF/GAP payments needs to be reviewed.',5,1,0,'Scheduled','KaspickDB','USP_PP_GetValidationEngineManuallyMatchedPaymentReview')
Insert into TBL_PP_ValidationRule(RuleID,RuleCategory,RuleName,ExecutionOrder,ResultType,DisplayMessage,RuleLeadTime,IsActive,StopValidationIfFails,PaymentStatus,InvokeMethod,SPName) Values(87,'Beneficiary payment','Ready for Payments'' flag is unchecked',245,'Error','InnoTrust "Ready for Payments" field not checked.',5,1,0,'Scheduled','KaspickDB','USP_PP_GetValidationEngineBeneficiaryReadyForPaymentFlag')
Insert into TBL_PP_ValidationRule(RuleID,RuleCategory,RuleName,ExecutionOrder,ResultType,DisplayMessage,RuleLeadTime,IsActive,StopValidationIfFails,PaymentStatus,InvokeMethod,SPName) Values(88,'Beneficiary payment','Schwab Moneylink data not sent',250,'Error','Two business days must elapse between input of new ACH data and posting of payment; allows time for moneylink data to be sent to Schwab',2,1,0,'Scheduled','KaspickDB','USP_PP_GetValidationEngineBeneficiarySchwabMoneylinkData')
Insert into TBL_PP_ValidationRule(RuleID,RuleCategory,RuleName,ExecutionOrder,ResultType,DisplayMessage,RuleLeadTime,IsActive,StopValidationIfFails,PaymentStatus,InvokeMethod,SPName) Values(89,'Beneficiary payment','Missing Alliance address',255,'Error','Address for Alliance Contact in InnoTrust required; this is the payor address on payment',14,1,0,'Scheduled','KaspickDB','USP_PP_GetValidationEngineBeneficiaryAllianceAddress')
Insert into TBL_PP_ValidationRule(RuleID,RuleCategory,RuleName,ExecutionOrder,ResultType,DisplayMessage,RuleLeadTime,IsActive,StopValidationIfFails,PaymentStatus,InvokeMethod,SPName) Values(90,'Beneficiary payment','Exclusion rule exists for a check method payment',260,'Error','Exclusion rule exists for a check method payment',14,1,0,'Scheduled','KaspickDB','USP_PP_GetValidationEngineBeneficiaryExclusionRule')
Insert into TBL_PP_ValidationRule(RuleID,RuleCategory,RuleName,ExecutionOrder,ResultType,DisplayMessage,RuleLeadTime,IsActive,StopValidationIfFails,PaymentStatus,InvokeMethod,SPName) Values(91,'Beneficiary payment','Transaction ID not received',265,'Error','InnoTrust has not sent transaction ID for this payment to Paragon.',0,1,0,'Scheduled','DotNet','DotNet')

Insert into TBL_PP_ValidationRule(RuleID,RuleCategory,RuleName,ExecutionOrder,ResultType,DisplayMessage,RuleLeadTime,IsActive,StopValidationIfFails,PaymentStatus,InvokeMethod,SPName) Values(92,'Beneficiary payment','Invalid BeneficiaryDistributionID',8,'Error','Invalid BeneficiaryDistribution profile',20,1,1,'Scheduled','KaspickDB','USP_PP_GetValidationEngineInvalidBeneficiaryDistribution')

SET IDENTITY_INSERT TBL_PP_ValidationRule OFF

--- Insert rules into TBL_PP_ValidationRuleAttribute Table

SET IDENTITY_INSERT TBL_PP_ValidationRuleAttribute ON

Insert into TBL_PP_ValidationRuleAttribute(AttributeID,RuleID,Attribute,ExpectedValue,EvaluationOrder,OperatorType,Datatype) Values(1,35,'IsBackBuilt','^(0|False)','5.1','==','STRING')
Insert into TBL_PP_ValidationRuleAttribute(AttributeID,RuleID,Attribute,ExpectedValue,EvaluationOrder,OperatorType,Datatype) Values(4,3,'AccountStatus','^(Inactive)','20.1','!=','STRING')
Insert into TBL_PP_ValidationRuleAttribute(AttributeID,RuleID,Attribute,ExpectedValue,EvaluationOrder,OperatorType,Datatype) Values(5,4,'BeneStatus','^(Beneficiary Inactive|Donor Inactive/Beneficiary Inactive|Donor Active/Beneficiary Inactive)','25.1','!=','STRING')
Insert into TBL_PP_ValidationRuleAttribute(AttributeID,RuleID,Attribute,ExpectedValue,EvaluationOrder,OperatorType,Datatype) Values(6,5,'ProxyBeneStatus','^(Beneficiary Inactive|Donor Inactive/Beneficiary Inactive|Donor Active/Beneficiary Inactive)','30.1','!=','STRING')
Insert into TBL_PP_ValidationRuleAttribute(AttributeID,RuleID,Attribute,ExpectedValue,EvaluationOrder,OperatorType,Datatype) Values(16,19,'BeneAddressStatus','^(none|<none>)','55.1','!=','STRING')
Insert into TBL_PP_ValidationRuleAttribute(AttributeID,RuleID,Attribute,ExpectedValue,EvaluationOrder,OperatorType,Datatype) Values(19,23,'BenePaymentAmount','0','65.1','!=','NUMBER')
Insert into TBL_PP_ValidationRuleAttribute(AttributeID,RuleID,Attribute,ExpectedValue,EvaluationOrder,OperatorType,Datatype) Values(20,29,'GiftAdditionDateDiff','^(0|False)','70.1','==','STRING')
Insert into TBL_PP_ValidationRuleAttribute(AttributeID,RuleID,Attribute,ExpectedValue,EvaluationOrder,OperatorType,Datatype) Values(22,30,'DiffInPaymentDateFromCurrentDate','5','75.1','>=','NUMBER')
Insert into TBL_PP_ValidationRuleAttribute(AttributeID,RuleID,Attribute,ExpectedValue,EvaluationOrder,OperatorType,Datatype) Values(24,31,'DiffInPaymentDateFromCurrentDate','0','80.1','>','NUMBER')
Insert into TBL_PP_ValidationRuleAttribute(AttributeID,RuleID,Attribute,ExpectedValue,EvaluationOrder,OperatorType,Datatype) Values(25,33,'PaymentMethod','^(Journal)','85.1','!=','STRING')
Insert into TBL_PP_ValidationRuleAttribute(AttributeID,RuleID,Attribute,ExpectedValue,EvaluationOrder,OperatorType,Datatype) Values(36,54,'AccountType','^(CORP|PSHP|PTA|DAF|QPE|EST|CLR|NQP|DDF|IRA|PSP|RET|ROTH |SEP|SIMP)','23.1','!=','STRING')
Insert into TBL_PP_ValidationRuleAttribute(AttributeID,RuleID,Attribute,ExpectedValue,EvaluationOrder,OperatorType,Datatype) Values(41,50,'MissingPayment','^(0|False)','101.1','==','STRING')
Insert into TBL_PP_ValidationRuleAttribute(AttributeID,RuleID,Attribute,ExpectedValue,EvaluationOrder,OperatorType,Datatype) Values(45,56,'DaysValue','0','12.1','>=','NUMBER')
Insert into TBL_PP_ValidationRuleAttribute(AttributeID,RuleID,Attribute,ExpectedValue,EvaluationOrder,OperatorType,Datatype) Values(47,58,'ClientHasPaymentCondition','^(0|False)','11.1','==','STRING')
Insert into TBL_PP_ValidationRuleAttribute(AttributeID,RuleID,Attribute,ExpectedValue,EvaluationOrder,OperatorType,Datatype) Values(48,59,'AccountPaymentConditionStatus','^(Inactive)','20.1','==','STRING')
Insert into TBL_PP_ValidationRuleAttribute(AttributeID,RuleID,Attribute,ExpectedValue,EvaluationOrder,OperatorType,Datatype) Values(49,60,'BenePaymentConditionStatus','^(Inactive)','24.1','==','STRING')
Insert into TBL_PP_ValidationRuleAttribute(AttributeID,RuleID,Attribute,ExpectedValue,EvaluationOrder,OperatorType,Datatype) Values(50,63,'DDAccount','^(none|<none>)','52.1','!=','STRING')
Insert into TBL_PP_ValidationRuleAttribute(AttributeID,RuleID,Attribute,ExpectedValue,EvaluationOrder,OperatorType,Datatype) Values(51,57,'PayActualNI','^(0|False)','32.1','==','STRING')
Insert into TBL_PP_ValidationRuleAttribute(AttributeID,RuleID,Attribute,ExpectedValue,EvaluationOrder,OperatorType,Datatype) Values(52,62,'PaymentVariance','0.05','155.1','<=','NUMBER')
Insert into TBL_PP_ValidationRuleAttribute(AttributeID,RuleID,Attribute,ExpectedValue,EvaluationOrder,OperatorType,Datatype) Values(53,61,'AccountPaymentFrequencyFound','^(1|True)','104.1','==','STRING')
Insert into TBL_PP_ValidationRuleAttribute(AttributeID,RuleID,Attribute,ExpectedValue,EvaluationOrder,OperatorType,Datatype) Values(54,64,'BeneMissingPayment','^(0|False)','110.1','==','STRING')
Insert into TBL_PP_ValidationRuleAttribute(AttributeID,RuleID,Attribute,ExpectedValue,EvaluationOrder,OperatorType,Datatype) Values(67,75,'WireRequestAuthorized','^(none|<none>)','185.1','!=','STRING')
Insert into TBL_PP_ValidationRuleAttribute(AttributeID,RuleID,Attribute,ExpectedValue,EvaluationOrder,OperatorType,Datatype) Values(70,78,'WireRequestFileCreationDate','^(none|<none>)','205.1','!=','STRING')
Insert into TBL_PP_ValidationRuleAttribute(AttributeID,RuleID,Attribute,ExpectedValue,EvaluationOrder,OperatorType,Datatype) Values(86,86,'ManuallyMatchedPaymentReview','^(none|<none>)','241.1','!=','STRING')
Insert into TBL_PP_ValidationRuleAttribute(AttributeID,RuleID,Attribute,ExpectedValue,EvaluationOrder,OperatorType,Datatype) Values(87,87,'ReadyforPayments','^(1|True)','140.1','==','STRING')
Insert into TBL_PP_ValidationRuleAttribute(AttributeID,RuleID,Attribute,ExpectedValue,EvaluationOrder,OperatorType,Datatype) Values(88,88,'MoneyLinkSetUpCreated','^(1|True)','145.1','==','STRING')
Insert into TBL_PP_ValidationRuleAttribute(AttributeID,RuleID,Attribute,ExpectedValue,EvaluationOrder,OperatorType,Datatype) Values(89,89,'MissingAllianceAddress','^(none|<none>)','150.1','!=','STRING')
Insert into TBL_PP_ValidationRuleAttribute(AttributeID,RuleID,Attribute,ExpectedValue,EvaluationOrder,OperatorType,Datatype) Values(90,90,'IsExclusion','^(1|True)','155.1','==','STRING')
Insert into TBL_PP_ValidationRuleAttribute(AttributeID,RuleID,Attribute,ExpectedValue,EvaluationOrder,OperatorType,Datatype) Values(91,91,'TransactionNumber','^(none|<none>)','160.1','!=','STRING')

Insert into TBL_PP_ValidationRuleAttribute(AttributeID,RuleID,Attribute,ExpectedValue,EvaluationOrder,OperatorType,Datatype) Values(92,92,'ValidBenficiaryDistribtuionID','^(1|True)','8.1','==','STRING')

SET IDENTITY_INSERT TBL_PP_ValidationRuleAttribute OFF

-- TBL_PP_AccountPayoutSchedule migration script

SET IDENTITY_INSERT TBL_PP_AccountPayoutSchedule ON

INSERT INTO TBL_PP_AccountPayoutSchedule (
	APScheduleID
	,ManagerCode
	,CustomerAccountNumber
	,InstructionID
	,TaxYear
	,PaymentDate
	,ScheduledAmount
	,ModifiedDate
	,ModifiedBy
	,CreatedDate
	,CreatedBy
	)
SELECT aps.APScheduleID
	,CLIENT_BRIEFNAME
	,Acc.AdventID
	,ISNULL(Remit.InstructionID,0)
	,TaxYear
	,PaymentDate
	,ScheduledAmount
	,Modified_Date
	,Modified_user_id
	,Created_Date
	,Created_user_id
FROM $(ExcelsiorDB)..accountpayoutschedule aps
INNER JOIN $(ExcelsiorDB)..TBL_EIS_PP_AccountPayoutSchedule_Supplement aps_supp ON aps.APScheduleID = aps_supp.APScheduleID
INNER JOIN $(ExcelsiorDB)..V_EIS_EX_Account ACC ON ACC.AccountID = aps.AccountID
LEFT OUTER JOIN $(InnoTrustDB)..RemittanceInstructions Remit ON Acc.AdventID = Remit.CustomerAccountNumber 
AND Remit.CustomerAccountNumber NOT IN 
	(SELECT CustomerAccountNumber from $(InnoTrustDB)..RemittanceInstructions 
	group by CustomerAccountNumber having COUNT(*)>1 )
--INNER JOIN TBL_InstructionLookUp InstrLkUp ON InstrLkUp.CustomerAccountNumber = Acc.CustomerAccountNumber

SET IDENTITY_INSERT TBL_PP_AccountPayoutSchedule OFF


--UPDATE TBL_PP_AccountPayoutSchedule 
--SET InstructionID = Remit.InstructionID
--FROM TBL_PP_AccountPayoutSchedule APS
--INNER JOIN $(InnoTrustDB)..RemittanceInstructions Remit ON APS.CustomerAccountNumber = Remit.CustomerAccountNumber 
--inner join $(InnoTrustDB)..BeneficiaryDistributions BeneDist on Remit.InstructionID = BeneDist.InstructionID


-- TBL_PP_BeneficiaryPayoutSchedule migration script
SET IDENTITY_INSERT TBL_PP_BeneficiaryPayoutSchedule ON

INSERT INTO TBL_PP_BeneficiaryPayoutSchedule (
	BPScheduleID
	,APScheduleID
	,BeneficiaryDistributionID
	,ContactID
	,ContactRoleCode
	,PaymentDate
	,TaxYear
	,ScheduledAmount
	,PaymentID
	,IsBackBuilt
	,EPD
	,STATUS
	,Memo
	,Comments
	--,WirePaymentClientApproval
	--,WirePaymentClientApprovalBy
	--,WireAuthorized
	--,WireAuthorizedBy
	--,PDFFileName
	--,FileCreationDate
	,CreatedDate
	,CreatedBy
	,ModifiedDate
	,ModifiedBy
	)
SELECT DISTINCT bps.bpscheduleid
	,bps.apscheduleid
	,ISNULL(ContLookUP.BeneficiaryDistributionID,0)
	,ISNULL(ContLookUP.CONTACTID,0)  AS contactid
	,ISNULL(ContLookUP.ROLECODE,0)  AS contactrolecode
	,bps.paymentdate
	,bps.taxyear
	,bps.scheduledamount
	,PaymentID
	,IsBackBuilt
	,EPD
	,Statusid
	,Memo
	,Comments
	--,WirePaymentClientApproval
	--,WirePaymentClientApprovalBy
	--,WireAuthorized
	--,WireAuthorizedBy
	--,PDFFileName
	--,FileCreationDate
	,Created_Date
	,CREATED_USER_ID
	,Modified_Date
	,MODIFIED_USER_ID
FROM $(ExcelsiorDB)..benpayoutschedule bps
INNER JOIN $(ExcelsiorDB)..TBL_EIS_PP_BenPayoutSchedule_Supplement bps_supp ON bps.bpscheduleid = bps_supp.bpscheduleid
INNER JOIN TBL_PP_AccountPayoutSchedule APS ON APS.APScheduleID = bps.APScheduleID
LEFT OUTER JOIN $(MappingDB)..TBL_BeneficiaryLookup ContLookUP ON ContLookUP.BENEFICIARYID = bps.BENEFICIARYID
--LEFT OUTER JOIN $(InnoTrustDB)..RemittanceInstructions Remit ON APS.CustomerAccountNumber = Remit.CustomerAccountNumber 
--LEFT OUTER join 
--	(SELECT InstructionID
--		,ContactID
--		,PayeeID
--		,BeneficiaryDistributionID
--		,RANK() OVER (
--			ORDER BY BeneficiaryDistributionID
--			) AS rank
--	FROM $(InnoTrustDB)..BeneficiaryDistributions BeneDist
--	GROUP BY BeneDist.InstructionID
--		,BeneDist.ContactID
--		,BeneDist.PayeeID
--		,BeneDist.BeneficiaryDistributionID) BeneDist on Remit.InstructionID = BeneDist.InstructionID
--		AND ContLookUP.ContactID = BeneDist.PayeeID
--		AND ContLookUP.PaidForContactID = BeneDist.ContactID and BeneDist.rank = 1


--INNER JOIN TBL_BeneficiaryDistributionLookUp BenDistLkUp ON BenDistLkUp.BENEFICIARYID = bps.BENEFICIARYID
--WHERE ContLookUP.BENEFICIARYID NOT IN (		-- Need to remove once migration issue is resolved
--		SELECT BENEFICIARYID
--		FROM #Tmp_PartBeneLookup
--		)

SET IDENTITY_INSERT TBL_PP_BeneficiaryPayoutSchedule OFF

--UPDATE TBL_PP_BeneficiaryPayoutSchedule 
--SET BeneficiaryDistributionID = BeneDist.BeneficiaryDistributionID
--FROM TBL_PP_BeneficiaryPayoutSchedule BenPsCH
--INNER JOIN TBL_PP_AccountPayoutSchedule AccPSch ON AccPSch.APScheduleID = BenPsCH.APScheduleID
--INNER JOIN $(InnoTrustDB)..RemittanceInstructions Remit ON AccPSch.CustomerAccountNumber = Remit.CustomerAccountNumber 
--inner join $(InnoTrustDB)..BeneficiaryDistributions BeneDist on Remit.InstructionID = BeneDist.InstructionID
--AND BenPsCH.ContactID = BeneDist.PayeeID

--TBL_PP_SystemAdministration migration script
INSERT INTO TBL_PP_SystemAdministration
SELECT MinimumCheckPostingDays
	,MinimumNonCheckPostingDays
	,5
	,PaymentProcessStartTime
	,WireThresholdAmount
	--,WireDomestic
	--,WireInternational
	,Modified_Date
	,MODIFIED_USER_ID
	,Created_Date
	,CREATED_USER_ID
FROM $(ExcelsiorDB)..TBL_EIS_PP_SystemAdministration

--SET IDENTITY_INSERT TBL_PP_AccountTypeSupportForScheduling ON

---- TBL_PP_AccountTypeSupportForScheduling migration script 
--INSERT INTO TBL_PP_AccountTypeSupportForScheduling (
--	ID
--	,AccountType
--	,ImportFMV
--	,ScheduleBenePayment
--	,ScheduleVendorPayment
--	,UserID
--	,CreatedDate
--	,ModifiedDate
--	)
--SELECT ID
--	,InnotrustAccountTypeCode
--	,ImportFMV
--	,ScheduleBenePayment
--	,ScheduleVendorPayment
--	,UserID
--	,CreatedDate
--	,ModifiedDate
--FROM $(ExcelsiorDB)..TBL_EIS_PP_AccountType_Support_For_Scheduling SchAcctype
--INNER JOIN $(MappingDB)..TBL_AccountTypeMapping AccTypeMap ON SchAcctype.AccountType = AccTypeMap.ExcelsiorAccountTypeCode

--SET IDENTITY_INSERT TBL_PP_AccountTypeSupportForScheduling OFF

--TBL_PP_Reportlist paragon migration script
INSERT INTO dbo.TBL_PP_Reportlist (
	IsActive
	,ReportName
	,SourceType
	,ReportTitle
	,ReportDescription
	,ReportFields
	,ReportOrder
	,ReportParameters
	)
SELECT inproduction
	,reportname
	,'Paragon'
	,reporttitle
	,reportdescription
	,reportfields
	,reportorder
	,reportparameters
FROM $(ExcelsiorDB)..TBL_EIS_PP_REPLIST

-- TBL_PP_Reportlist middleware migration script


IF NOT EXISTS (
		SELECT *
		FROM TBL_PP_ReportList
		WHERE [ReportName] = 'Payment Processed Report'
			AND [SourceType] = 'Middleware'
		)
BEGIN
	INSERT INTO [TBL_PP_ReportList] (
		[IsActive]
		,[ReportName]
		,[SourceType]
		,[ReportTitle]
		,[ReportDescription]
		,[ReportFields]
		,[ReportOrder]
		,[ReportParameters]
		)
	VALUES (
		1
		,'Payment Processed Report'
		,'Middleware'
		,'Payment Processed Report'
		,'Displays payments processed for the selected Manager code(s) filtered on a given date range'
		,'ManagerCode,ManagerDescription,PaymentDate,PaymentAmount,NumberOfPaymentTransaction,NoOfExceptionPayment,NoOfStandardPayments'
		,'ManagerCode'
		,'UserID,FromDate,ToDate'
		)
END

IF NOT EXISTS (
		SELECT *
		FROM TBL_PP_ReportList
		WHERE [ReportName] = 'Payment Message Report'
			AND [SourceType] = 'Middleware'
		)
BEGIN
	INSERT INTO [TBL_PP_ReportList] (
		[IsActive]
		,[ReportName]
		,[SourceType]
		,[ReportTitle]
		,[ReportDescription]
		,[ReportFields]
		,[ReportOrder]
		,[ReportParameters]
		)
	VALUES (
		1
		,'Payment Message Report'
		,'Middleware'
		,'Payment Message Report'
		,'Displays Marketing Message and Reminder Message for selected Manager code(s)'
		,'ManagerDescription,ReminderMessage,MarketingMessage'
		,'MessageMgmtCode,MessageMgmtType'
		,'UserID'
		)
END

IF NOT EXISTS (
		SELECT *
		FROM TBL_PP_ReportList
		WHERE [ReportName] = 'Payment Consolidated Rules Report'
			AND [SourceType] = 'Middleware'
		)
BEGIN
	INSERT INTO [TBL_PP_ReportList] (
		[IsActive]
		,[ReportName]
		,[SourceType]
		,[ReportTitle]
		,[ReportDescription]
		,[ReportFields]
		,[ReportOrder]
		,[ReportParameters]
		)
	VALUES (
		1
		,'Payment Consolidated Rules Report'
		,'Middleware'
		,'Payment Consolidated Rules Report'
		,'Displays consolidated payment processing rules for selected Manager code(s)'
		,'ManagerCode,ManagerDescription,NoOfDaysBeforePayment,MarkedAsException,ExceptionCode,ExceptionDescription,ProcessPaymentThroughEZPay,ExclusionDescription'
		,'ManagerCode'
		,'UserID'
		)
END

IF NOT EXISTS (
		SELECT *
		FROM TBL_PP_ReportList
		WHERE [ReportName] = 'Envelope Summary Report'
			AND [SourceType] = 'Middleware'
		)
BEGIN
	INSERT INTO [TBL_PP_ReportList] (
		[IsActive]
		,[ReportName]
		,[SourceType]
		,[ReportTitle]
		,[ReportDescription]
		,[ReportFields]
		,[ReportOrder]
		,[ReportParameters]
		)
	VALUES (
		1
		,'Envelope Summary Report'
		,'Middleware'
		,'Envelope Summary Report'
		,'Displays quantity of envelopes needed against quantity of payments for selected Manager code(s)'
		,'ManagerCode,ManagerDescription,PaymentCount,EnvelopeCount,TotalPaymentAmount,ReviewedAndApproved'
		,'ManagerCode'
		,'UserID,FromDate,ToDate'
		)
END




--TBL_PP_PaymentCondition migration script

Declare @WarningID INT
SELECT @WarningID = ListItemID FROM TBL_ListItem ListItem
INNER JOIN TBL_ListType ListType ON ListItem.ListTypeID  = ListType.ListTypeID  and ListItem.ListItemName ='Warning'
and ListType.ListTypeName = 'ValidationEngineErrorType' 

SET IDENTITY_INSERT TBL_PP_PaymentCondition ON

INSERT INTO dbo.TBL_PP_PaymentCondition (
	ConditionID
	,EntityTypeId
	,ManagerCode
	,CustomerAccountNumber
	,ContactID
	,ContactRoleCode
	,AccountType
	,Comments
	,StatusID
	,ValidationEngineResultType
	,ModifiedDate
	,ModifiedBy
	,CreatedDate
	,CreatedBy
	)
SELECT distinct Condition_ID
	,Entity_Type_ID
	,ISNULL(ISNULL(clnt.BriefName,DGA.CLIENT_BRIEFNAME),BeneClnt.CLIENT_BRIEFNAME) as ClientBriefName
	,case when DGA.AdventID IS NOT NULL then DGA.AdventID else ContLookUP.CustomerAccountNumber END CustomerAccountNumber
	,ContLookUP.CONTACTID  AS ContactID
	,ContLookUP.CONTACTROLECODE  AS ContactRoleCode
	,NULL AS AccountType 
	,paycond.Comments
	,Status_ID
	,@WarningID AS ValidationEngineResultType  -- Default to Warning
	,MODIFIED_DATE
	,MODIFIED_USER_ID
	,CREATED_DATE
	,CREATED_USER_ID
FROM $(ExcelsiorDB)..TBL_EIS_EX_PAYMENT_CONDITIONS PayCond
LEFT OUTER JOIN $(ExcelsiorDB)..CLIENT Clnt ON clnt.ClientID = PayCond.ClientID
LEFT OUTER JOIN $(ExcelsiorDB)..V_EIS_EX_ACCOUNT DGA ON DGA.AccountID = PayCond.AccountID
LEFT OUTER JOIN $(MappingDB)..TBL_PartBeneContactLookUp ContLookUP ON ContLookUP.BENEFICIARYID = PayCond.BENEFICIARYID
LEFT OUTER JOIN $(ExcelsiorDB)..V_EIS_EX_ACCOUNT BeneClnt ON BeneClnt.ADVENTID = ContLookUP.CustomerAccountNumber
--WHERE ContLookUP.BENEFICIARYID NOT IN ( -- Need to remove once migration issue is resolved
--		SELECT BENEFICIARYID
--		FROM #Tmp_PartBeneLookup
--		)

SET IDENTITY_INSERT TBL_PP_PaymentCondition OFF

Update TBL_PP_PaymentCondition SET AccountType = 'GAP,CRAT,CRUT,NIMU,NICT,END,GREV,IRRV,PRE-69' where ConditionID = 365
Update TBL_PP_PaymentCondition SET AccountType = 'TMCF,GAP,CRAT,CRUT,NIMU,NICT,PIF,PR69' where ConditionID = 2003
Update TBL_PP_PaymentCondition SET AccountType = 'CRAT,CRUT,NIMU,PIF' where ConditionID = 8102
Update TBL_PP_PaymentCondition SET AccountType = 'CRAT,CRUT,NIMU,NICT' where ConditionID = 10611
Update TBL_PP_PaymentCondition SET AccountType = 'PIF' where ConditionID = 21275
Update TBL_PP_PaymentCondition SET AccountType = 'END' where ConditionID = 23145
Update TBL_PP_PaymentCondition SET AccountType = 'END' where ConditionID = 23146
Update TBL_PP_PaymentCondition SET AccountType = 'GAP,CRAT,CRUT,NIMU,NICT,GREV,IRRV,PIF,PR69' where ConditionID = 25994
Update TBL_PP_PaymentCondition SET AccountType = 'CRAT,CRUT,GAP,GAPP,GAPR,NICT,NIMU' where ConditionID = 26931


--TBL_PP_PGCalcPaymentData migration script 
INSERT INTO dbo.TBL_PP_PGCalcPaymentData (
	PGCalcPaymentID
	,PaymentBatch
	,CustomerAccountNumber
	,PaymentID
	,ContactID
	,ContactRoleCode
	,InstructionID
	,BeneficiaryDistributionID
	,PersonCode
	,PayeeName
	,PaymentAmount
	,PaymentDate
	,SeparateCheck
	,Account1
	,Account2
	,GiftDate
	,PIFUnits
	,PIFValue
	,PIFIncome
	,TaxWithholding
	,PeriodEndDate
	)
	SELECT distinct PGCalcPaymentID
	,PaymentBatch
	,adventid
	,PaymentID
	,ISNULL(ContLookUP.CONTACTID,0)  AS ContactID
	,ISNULL(ContLookUP.ROLECODE,0)  AS ContactRoleCode
	,ISNULL(BeneDist.InstructionID,0)InstructionID
	,ISNULL(ContLookUP.BeneficiaryDistributionID,0)BeneficiaryDistributionID
	,PersonCode
	,PayeeName
	,PaymentAmount
	,PaymentDate
	,SeparateCheck
	,Account1
	,Account2
	,GiftDate
	,PIFUnits
	,PIFValue
	,PIFIncome
	,TaxWithholding
	,PeriodEndDate
FROM $(ExcelsiorDB)..pgcalcpaymentdata pgcalc
LEFT OUTER JOIN $(MappingDB)..TBL_BeneficiaryLookup ContLookUP ON ContLookUP.BENEFICIARYID = pgcalc.BENEFICIARYID
LEFT OUTER JOIN $(InnoTrustDB)..BeneficiaryDistributions BeneDist ON BeneDist.BeneficiaryDistributionID=ContLookUP.BeneficiaryDistributionID
	
		
--UPDATE TBL_PP_PGCalcPaymentData 
--SET BeneficiaryDistributionID = BeneDist.BeneficiaryDistributionID
--	,InstructionID = BeneDist.InstructionID
--FROM TBL_PP_PGCalcPaymentData PGCalc
--INNER JOIN $(InnoTrustDB)..RemittanceInstructions Remit
--	ON PGCalc.CustomerAccountNumber = Remit.CustomerAccountNumber
--INNER JOIN $(InnoTrustDB)..BeneficiaryDistributions BeneDist
--	ON Remit.InstructionID = BeneDist.InstructionID
--		AND PGCalc.ContactID = BeneDist.PayeeID 
		
--TBL_PP_PaymentPostingProcessLog migration script
SET IDENTITY_INSERT TBL_PP_PaymentPostingProcessLog ON

INSERT INTO dbo.TBL_PP_PaymentPostingProcessLog (
	PaymentPostingProcessLogID
	,ProcessStartDateTime
	,ProcessEndDateTime
	,STATUS
	,FailureDescription
	,PaymentsPosted
	,PaymentsNotPosted
	,PaymentsValidated
	,PaymentsFailed
	)
SELECT PaymentProcessID
	,ProcessStartDateTime
	,ProcessEndDateTime
	,STATUS
	,FailureDescription
	,PaymentsPosted
	,PaymentsNotPosted
	,PaymentsValidated
	,PaymentsFailed
FROM $(ExcelsiorDB)..TBL_EIS_PP_PaymentProcess

SET IDENTITY_INSERT TBL_PP_PaymentPostingProcessLog OFF
--TBL_PP_PGCalcPaymentBatch Migration script
SET IDENTITY_INSERT TBL_PP_PGCalcPaymentBatch ON

INSERT INTO dbo.TBL_PP_PGCalcPaymentBatch (
	PaymentBatch
	,SourceFile
	,LoadDate
	,Description
	,ModifiedDate
	,ModifiedBy
	,CreatedDate
	,CreatedBy
	)
SELECT Batch.PaymentBatch
	,SourceFile
	,LoadDate
	,Description
	,GETDATE()
	,MODIFIED_USER_ID
	,GETDATE()
	,CREATED_USER_ID
FROM  $(ExcelsiorDB)..PGCalcPaymentBatch Batch
INNER JOIN  $(ExcelsiorDB)..TBL_EIS_PP_PGCalcPaymentBatch_Supplement BatchSupp 
	ON Batch.PaymentBatch = BatchSupp.PaymentBatch

SET IDENTITY_INSERT TBL_PP_PGCalcPaymentBatch OFF

-- TBL_PP_PGCalcPaymentDataMerged Migration script
INSERT INTO TBL_PP_PGCalcPaymentDataMerged (
	MergedPGCalcPaymentID
	,PaymentBatch
	,ManagerCode
	,CustomerAccountNumber
	,PersonCode
	,InstructionID
	,BeneficiaryDistributionID
	,PaymentID
	,ContactID
	,ContactRoleCode
	,PayeeName
	,PaymentAmount
	,PaymentDate
	,SeparateCheck
	,Account1
	,Account2
	,GiftDate
	,PIFUnits
	,PIFValue
	,PIFIncome
	,TaxWithholding
	,PeriodEndDate
	,EPD
	,TaxYear
	,STATUS
	,Memo
	,Comment
	--,WirePaymentClientApproval
	--,WirePaymentClientApprovalBy
	--,WireAuthorized
	--,WireAuthorizedBy
	--,PDFFileName
	--,FileCreationDate
	,ModifiedDate
	,ModifiedBy
	,CreatedDate
	,CreatedBy
	,MatchType
	,GiftWrapPaymentReviewDate
	,GiftWrapPaymentReviewedBy
	)
	SELECT DISTINCT PGCalcPaymentID
	,PaymentBatch
	,ACC.CLIENT_BRIEFNAME 
	,ACC.AdventID
	,PersonCode
	,ISNULL(BeneDist.InstructionID,0)
	,ISNULL(ContLookUP.BeneficiaryDistributionID,0)
	,PaymentID
	,ISNULL(ContLookUP.CONTACTID,0)  AS ContactID
	,ISNULL(ContLookUP.ROLECODE,0) AS ContactRoleCode
	,PayeeName
	,PaymentAmount
	,PaymentDate
	,SeparateCheck
	,Account1
	,Account2
	,GiftDate
	,PIFUnits
	,PIFValue
	,PIFIncome
	,TaxWithholding
	,PeriodEndDate
	,EPD
	,TaxYear
	,StatusId
	,Memo
	,Comment
	--,WirePaymentClientApproval
	--,WirePaymentClientApprovalBy
	--,WireAuthorized
	--,WireAuthorizedBy
	--,PDFFileName
	--,FileCreationDate
	,MODIFIED_DATE
	,MODIFIED_USER_ID
	,CREATED_DATE
	,CREATED_USER_ID
	,MatchType
	,GiftWrapPaymentReviewDate
	,GiftWrapPaymentReviewedBy
FROM $(ExcelsiorDB)..TBL_EIS_PP_PGCalcPaymentData_Merged pgcalc
INNER JOIN $(MappingDB)..TBL_BeneficiaryLookup ContLookUP ON ContLookUP.BENEFICIARYID = pgcalc.BENEFICIARYID
--INNER JOIN TBL_BeneficiaryDistributionLookUp BenDistLkUp ON BenDistLkUp.BENEFICIARYID = pgcalc.BENEFICIARYID
--LEFT OUTER JOIN $(InnoTrustDB)..RemittanceInstructions InstrLkUp ON InstrLkUp.CustomerAccountNumber = pgcalc.adventid
LEFT OUTER JOIN $(InnoTrustDB)..BeneficiaryDistributions BeneDist ON BeneDist.BeneficiaryDistributionID=ContLookUP.BeneficiaryDistributionID
	--ON InstrLkUp.InstructionID = BeneDist.InstructionID
	--AND ContLookUP.ContactID = BeneDist.PayeeID AND ContLookUP.PaidForContactID = BeneDist.ContactID
	--AND pgcalc.PayeeName = BeneDist.PaidToContactName
INNER JOIN $(ExcelsiorDB)..V_EIS_EX_ACCOUNT ACC on ACC.ADVENTID = pgcalc.adventid


--UPDATE TBL_PP_PGCalcPaymentDataMerged 
--SET BeneficiaryDistributionID = BeneDist.BeneficiaryDistributionID
--	,InstructionID = BeneDist.InstructionID
--FROM TBL_PP_PGCalcPaymentDataMerged PGCalc
--INNER JOIN $(InnoTrustDB)..RemittanceInstructions Remit
--	ON PGCalc.CustomerAccountNumber = Remit.CustomerAccountNumber
--INNER JOIN $(InnoTrustDB)..BeneficiaryDistributions BeneDist
--	ON Remit.InstructionID = BeneDist.InstructionID
--		AND PGCalc.ContactID = BeneDist.PayeeID		

		
-- TBL_PP_PGCalcPaymentDataRelation Migration Script
INSERT INTO TBL_PP_PGCalcPaymentDataRelation
SELECT PGCalcPaymentID
	,MergedPGCalcPaymentID
FROM $(ExcelsiorDB)..TBL_EIS_PP_PGCalcPaymentData_Relation
WHERE MergedPGCalcPaymentID -- Need to remove once migration issue is resolved
	IN (
		SELECT MergedPGCalcPaymentID
		FROM TBL_PP_PGCalcPaymentDataMerged
		)

-- TBL_PP_AnnualAccountPayoutInfo Migration script
INSERT INTO TBL_PP_AnnualAccountPayoutInfo (
	CustomerAccountNumber
	,PayoutYear
	,Valuation
	,ProratedGiftValuation
	,Yield
	,IncomeFees
	,EstAnnualPayout
	,EstMethod
	,YTDPayment
	,PrevOverPayment
	,Deficit
	,YTDNetIncome
	,FMV
	,OutstandingBenPayments
	,OtherValuationAdjustment
	,PaymentStagingID
	,ModifiedDate
	,ModifiedBy
	,CreatedDate
	,CreatedBy
	)
SELECT CustomerAccountNumber
	,PayoutYear
	,Valuation
	,ProratedGiftValuation
	,Yield
	,IncomeFees
	,EstAnnualPayout
	,EstMethod
	,YTDPayment
	,PrevOverPayment
	,Deficit
	,YTDNetIncome
	,FMV
	,OutstandingBenPayments
	,OtherValuationAdjustment
	,PaymentStagingID
	,GETDATE()
	,1
	,GETDATE()
	,1
FROM $(ExcelsiorDB)..AnnualAccountPayoutInfo AAPI
INNER JOIN $(MappingDB)..TBL_AccountLookup ACC ON ACC.AccountID = AAPI.AccountID


UPDATE TBL_PP_AnnualAccountPayoutInfo SET FMVAsOfDate =  '01-01-'+CAST(PayoutYear AS VARCHAR(4))
WHERE PayoutYear<>200


-- TBL_PP_BeneficiaryPayment Migration Script 
INSERT INTO #Tmp_BeneficiaryPayment (
	PaymentID
	,CustomerAccountNumber
	,ContactID
	,ContactRoleCode
	,InstructionID
	,BeneficiaryDistributionID
	,TransactionNumber
	,DocumentNumber
	,AccountType
	,ManagerCode
	,PaymentDate
	,TaxYear
	,PayeeName
	,PayeeAddress
	,PaymentAmount
	,LastName
	,SourceBankName
	,SourceBankAddress
	,SourceBankABA
	,FractionalRoutingCode
	,PayorName
	,PayorAddress
	,SourceAccount
	,DestBankName
	,DestBankAddress
	,DestBankABA
	,DestAccount
	,Memo
	,VoidDate
	,ReissueOf
	,ReissueAs
	,DestAccountType
	--,ExpenseCode
	,taxcode -- renaming expensecode as taxcode
	,ClearDate
	,ClearMethod
	,ChargeType
	,DirectDepositFlag
	,IsBackBuilt
	,STATUS
	,PostDate
	,Comment
	--,AcomStatus
	,AcomPrintDate
	,ACHFileName
	,Withholding
	,WireSubmitted
	,WireSubmittedBy
	,MailingAddress
	,SeparateCheck
	,SeparateCheckAddress
	,alliancenumber
	,GiftAccountName
	,GSOInfo
	,PrintAdvice
	,PaymentMethod
	,PaymentFreq
	,PaidforContactID
	,DisplayVoidPaymentsInWeb
	,CreatedDate
	,CreatedBy
	,ModifiedDate
	,ModifiedBy
	)
SELECT distinct p.PaymentID
	,bp.AdventID
	,ISNULL(lkup.CONTACTID,0)
	,ISNULL(lkup.RoleCode,0)
	,0
	,ISNULL(lkup.BeneficiaryDistributionID,0)
	,0 AS TransactionNumber ---------------NN
	,p.CheckNumber
	,NULL AS accounttype ---------------NN							---AccTypeLookUp.AccountTypeCode
	,CLIENT_BRIEFNAME
	,p.PaymentDate
	,p.TaxYear
	,p.PayeeName
	,p.PayeeAddress
	,p.PaymentAmount
	,TPart.LastName
	,p.SourceBankName
	,p.SourceBankAddress
	,p.SourceBankABA
	,p.FractionalRoutingCode
	,p.PayorName
	,p.PayorAddress
	,p.SourceAccount
	,p.DestBankName
	,p.DestBankAddress
	,p.DestBankABA
	,p.DestAccount
	,p.Memo
	,p.VoidDate
	,p.ReissueOf
	,p.ReissueAs
	,p.DestAccountType
	--,p.ExpenseCode
	,CASE 
		WHEN p.ExpenseCode = 'benpmt'
			THEN 570
		WHEN p.ExpenseCode = 'bentx'
			THEN 697
		WHEN p.ExpenseCode = 'fbentx'
			THEN 691
		ELSE NULL
		END -- defaulting value to taxcode based on innotrust changes
	,p.ClearDate
	,p.ClearMethod
	,p.ChargeType
	,p.DirectDepositFlag
	,ps.IsBackBuilt
	,ps.StatusID
	,ps.PostDate
	,ps.Comment
	--,ps.AcomStatus
	,ps.AcomPrintDate
	,ps.ACHFileName
	,ps.Withholding
	,ps.WireSubmitted
	,ps.WireSubmittedBy
	,bp.MailingAddress
	,bp.SeparateCheck
	,bp.SeparateCheckAddress
	,substring(acc.PROGRAM_BRIEFNAME, 1, 3) -- case when program briefname is large need to vreify  -- need to clean up this data 
	,bp.GiftAccountName
	,bp.GSOInfo
	,case when bp.PaymentMethod = 'SPLITCHECK' then -4 ELSE bp.PrintAdvice END
	,case when bp.PaymentMethod = 'SPLITCHECK' then 'CHECK' ELSE bp.PaymentMethod END
	,paymentfreq
	,lkup.PaidForContactID
	,Display_void_payments_in_web
	,bps.CREATED_DATE
	,bps.CREATED_USER_ID
	,bps.MODIFIED_DATE
	,bps.MODIFIED_USER_ID
FROM $(ExcelsiorDB)..payment p
INNER JOIN $(ExcelsiorDB)..TBL_EIS_PP_Payment_Supplement ps
	ON p.PaymentID = ps.PaymentID
INNER JOIN $(ExcelsiorDB)..BenPayment bp
	ON bp.PaymentID = p.PaymentID
INNER JOIN $(ExcelsiorDB)..TBL_EIS_PP_BenPayment_Supplement bps
	ON bps.paymentid = p.PaymentID
LEFT OUTER JOIN $(MappingDB)..TBL_BeneficiaryLookup lkup
	ON lkup.BeneficiaryID = bp.BeneficiaryID
		--AND CONTACTROLECODE = @BeneID		-- loads bene and proxy
		AND lkup.CUSTOMERACCOUNTNUMBER = bp.AdventID
--LEFT OUTER JOIN $(MappingDB)..TBL_BeneficiaryLookup K1Bene
--	ON K1Bene.BeneficiaryID = lkup.K1BeneficiaryID
INNER JOIN $(ExcelsiorDB)..V_EIS_EX_ACCOUNT acc
	ON acc.AdventID = bp.AdventID
INNER JOIN $(ExcelsiorDB)..Beneficiary Bene
	ON Bene.BeneficiaryID = bp.BeneficiaryID
INNER JOIN $(ExcelsiorDB)..TRUSTPARTICIPANT TPart
	ON TPart.ParticipantID = Bene.ParticipantID
WHERE bp.PaymentMethod IS NOT NULL -- Need to cleanup data
	--AND lkup.BENEFICIARYID NOT IN (
	--	-- -- Need to remove once migration issue is resolved
	--	SELECT BENEFICIARYID
	--	FROM #Tmp_PartBeneLookup
	--	)

UPDATE #Tmp_BeneficiaryPayment
SET TransactionNumber = tranlookup.TransactionNumber
FROM #Tmp_BeneficiaryPayment BenPymt
INNER JOIN TBL_TransactionLookUp tranlookup
	ON tranlookup.PaymentID = BenPymt.PaymentID


UPDATE #Tmp_BeneficiaryPayment
SET AccountType = ISNULL(AccountTypeCode,'')
FROM #Tmp_BeneficiaryPayment BenPymt
LEFT OUTER JOIN SYN_IT_AccountMaster AccMstr
	ON BenPymt.CustomerAccountNumber = AccMstr.CustomerAccountNumber


UPDATE #Tmp_BeneficiaryPayment
SET AcomPrintDate = ProcessedDate_MiddleWare
FROM #Tmp_BeneficiaryPayment BenPymnt
INNER JOIN $(ExcelsiorDB)..TBL_PP_ARCH_PAYMENTEXPORT PymntExport
	ON BenPymnt.PaymentID = PymntExport.PaymentID


UPDATE #Tmp_BeneficiaryPayment
SET [WireAuthorizedDate] = bps_supp.WireAuthorized
	,[WireAuthorizedBy] = bps_supp.WireAuthorizedBy
	,[WirePDFFileName] = bps_supp.PDFFileName
	,[WirePDFFileCreationDate] = bps_supp.FileCreationDate
FROM #Tmp_BeneficiaryPayment BenPymnt
INNER JOIN $(ExcelsiorDB)..BenPayoutSchedule BPS
	ON BenPymnt.PaymentID = BPS.PaymentID
INNER JOIN $(ExcelsiorDB)..TBL_EIS_PP_BenPayoutSchedule_Supplement bps_supp
	ON bps.bpscheduleid = bps_supp.bpscheduleid


UPDATE #Tmp_BeneficiaryPayment
SET [WireAuthorizedDate] = PGCalc.WireAuthorized
	,[WireAuthorizedBy] = PGCalc.WireAuthorizedBy
	,[WirePDFFileName] = PGCalc.PDFFileName
	,[WirePDFFileCreationDate] = PGCalc.FileCreationDate
FROM #Tmp_BeneficiaryPayment BenPymnt
INNER JOIN $(ExcelsiorDB)..TBL_EIS_PP_PGCalcPaymentData_Merged PGCalc
	ON BenPymnt.PaymentID = PGCalc.PaymentID


UPDATE #Tmp_BeneficiaryPayment
--SET BeneficiaryDistributionID = BeneDist.BeneficiaryDistributionID
	SET InstructionID = Remit.InstructionID
FROM #Tmp_BeneficiaryPayment BenPymnt
INNER JOIN $(InnoTrustDB)..RemittanceInstructions Remit
	ON BenPymnt.CustomerAccountNumber = Remit.CustomerAccountNumber
--INNER JOIN $(InnoTrustDB)..BeneficiaryDistributions BeneDist
--	ON Remit.InstructionID = BeneDist.InstructionID
--		AND BenPymnt.ContactID = BeneDist.PayeeID AND BenPymnt.PaidforContactID = BeneDist.ContactID

set IDENTITY_INSERT TBL_PP_BeneficiaryPayment ON 

INSERT INTO TBL_PP_BeneficiaryPayment (
	PaymentID
	,CustomerAccountNumber
	,ContactID
	,ContactRoleCode
	,InstructionID
	,BeneficiaryDistributionID
	,TransactionNumber
	,DocumentNumber
	,AccountType
	,ManagerCode
	,PaymentDate
	,TaxYear
	,PayeeName
	,PayeeAddress
	,PaymentAmount
	,LastName
	,SourceBankName
	,SourceBankAddress
	,SourceBankABA
	,FractionalRoutingCode
	,PayorName
	,PayorAddress
	,SourceAccount
	,DestBankName
	,DestBankAddress
	,DestBankABA
	,DestAccount
	,Memo
	,VoidDate
	,ReissueOf
	,ReissueAs
	,DestAccountType
	--,ExpenseCode
	,taxcode -- renaming expensecode as taxcode
	,ClearDate
	,ClearMethod
	,ChargeType
	,DirectDepositFlag
	,IsBackBuilt
	,STATUS
	,PostDate
	,Comment
	--,AcomStatus
	,AcomPrintDate
	,ACHFileName
	,Withholding
	,WireSubmitted
	,WireSubmittedBy
	,MailingAddress
	,SeparateCheck
	,SeparateCheckAddress
	,alliancenumber
	,GiftAccountName
	,GSOInfo
	,PrintAdvice
	,PaymentMethod
	,PaymentFreq
	,PaidforContactID
	,DisplayVoidPaymentsInWeb
	,CreatedDate
	,CreatedBy
	,ModifiedDate
	,ModifiedBy
	,WireAuthorizedDate
	,WireAuthorizedBy
	,WirePDFFileName
	,WirePDFFileCreationDate


	)
SELECT 
	PaymentID
	,CustomerAccountNumber
	,ContactID
	,ContactRoleCode
	,InstructionID
	,BeneficiaryDistributionID
	,TransactionNumber
	,DocumentNumber
	,AccountType
	,ManagerCode
	,PaymentDate
	,TaxYear
	,PayeeName
	,PayeeAddress
	,PaymentAmount
	,LastName
	,SourceBankName
	,SourceBankAddress
	,SourceBankABA
	,FractionalRoutingCode
	,PayorName
	,PayorAddress
	,SourceAccount
	,DestBankName
	,DestBankAddress
	,DestBankABA
	,DestAccount
	,Memo
	,VoidDate
	,ReissueOf
	,ReissueAs
	,DestAccountType
	,taxcode 
	,ClearDate
	,ClearMethod
	,ChargeType
	,DirectDepositFlag
	,IsBackBuilt
	,STATUS
	,PostDate
	,Comment
	,AcomPrintDate
	,ACHFileName
	,Withholding
	,WireSubmitted
	,WireSubmittedBy
	,MailingAddress
	,SeparateCheck
	,SeparateCheckAddress
	,alliancenumber
	,GiftAccountName
	,GSOInfo
	,PrintAdvice
	,PaymentMethod
	,PaymentFreq
	,PaidforContactID
	,DisplayVoidPaymentsInWeb
	,CreatedDate
	,CreatedBy
	,ModifiedDate
	,ModifiedBy
	,WireAuthorizedDate
	,WireAuthorizedBy
	,WirePDFFileName
	,WirePDFFileCreationDate
FROM #Tmp_BeneficiaryPayment

	
set IDENTITY_INSERT TBL_PP_BeneficiaryPayment OFF 

--PRINT 'Due to Innotrust data issue some of the beneficiaries are filtered during migration in tables dbo.TBL_PP_BeneficiaryPayoutSchedule, TBL_PP_PaymentCondition, TBL_PP_PGCalcPaymentData, TBL_PP_PGCalcPaymentDataMerged, TBL_PP_BeneficiaryPayment and TBL_PP_PGCalcPaymentDataRelation'

UPDATE TBL_PP_ValidationRuleAttribute SET ExpectedValue='0', OperatorType='!=', Datatype='NUMBER' WHERE RuleID=4
UPDATE TBL_PP_ValidationRuleAttribute SET ExpectedValue='^(1|True)', OperatorType='==', Datatype='STRING' WHERE RuleID=5

