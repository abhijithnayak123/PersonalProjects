SET NOCOUNT ON
-- TBL_PV_ValuationStatus
SET IDENTITY_INSERT TBL_PV_ValuationStatus ON

INSERT INTO TBL_PV_ValuationStatus (
	StatusID
	,STATUS
	)
SELECT StatusID
	,STATUS
FROM $(ExcelsiorDB)..TBL_EIS_PEV_Valuation_Status

SET IDENTITY_INSERT TBL_PV_ValuationStatus OFF
-- TBL_PV_Valuation
SET IDENTITY_INSERT TBL_PV_Valuation ON

INSERT INTO dbo.TBL_PV_Valuation (
	ValuationID
	,ValuationDate
	,ReconFromDate
	,ReconToDate
	,CustomerAccountNumber
	,AccountType
	,ManagerCode
	,ManageCodeName
	,InitialFMV
	,PendingSeverence
	,PendingGiftAddition
	,EarlyTradedPayments
	,GiftwrapPaymentsAsOfDate
	,InnoTrustPaymentsAsOfDate
	,OtherAdjustment
	,FinalFMV
	,GiftwrapEquityUnit
	,NAV
	,ReconcilliationType
	,Comment
	,CurrentStatusID
	,ReconWithDW
	,GiftWrapCashtracInput
	,GiftWrapMVDARptGenerated
	,HasGiftExclusion
	,GiftExclusionComment
	,ValuationType
	,CreatedDate
	,CreatedBy
	,ModifiedDate
	,ModifiedBy
	)
SELECT ValuationID
	,ValuationDate
	,ReconFromDate
	,ReconToDate
	,AdventID
	,AccountTypeCode
	,ClientBriefName
	,PVal.ClientName
	,InitialFMV
	,PendingSeverences
	,PendingGiftAdditions
	,ACHPaymentsParagon
	,GiftwarpPayment
	,0 AS InnoTrustPaymentsAsOfDate
	,OtherAdjustments
	,FinalFMV
	,GiftwrapEquityUnits
	,NAV
	,ReconcilliationType
	,Comments
	,CurrentStatusID
	,ReconWithDW
	,GiftWrapCashtracInput
	,GiftWrapMVDARptsGenerated
	,HasGiftsExclusion
	,GiftsExclusionComments
	,ValuationType
	,CREATED_DATE
	,CREATED_USER_ID
	,MODIFIED_DATE
	,MODIFIED_USER_ID
FROM $(ExcelsiorDB)..TBL_EIS_PEV_Valuation PVal
INNER JOIN $(InnoTrustDB)..AccountMaster AccType
ON AccType.CustomerAccountNumber = adventid

SET IDENTITY_INSERT TBL_PV_Valuation OFF
-- TBL_PV_ManagerCodeParameter
SET IDENTITY_INSERT TBL_PV_ManagerCodeParameter ON

INSERT INTO dbo.TBL_PV_ManagerCodeParameter (
	ParameterID
	,CustomerAccountNumber
	,AccountType
	,ManagerCode
	,ManagerCodeName
	,GiftWrapAccount
	,GroupID
	,HasGiftExclusion
	,GiftExclusionComment
	,ExcludeAccruedInterest
	,AccountingRoutine
	,IsSubAccountingSystemFIMS
	,ExcludeIncomeCash
	)
SELECT ParameterID
	,AdventID
	,AccountTypeCode
	,ClientBriefName
	,ClParam.ClientName
	,GiftWrapAccount
	,NULL AS GroupID
	,HasGiftsExclusion
	,GiftsExclusionComments
	,ExcludeAccruedInterest
	,NULL AS AccountingRoutine
	,0 AS IsSubAccountingSystemFIMS
	,ExcludeIncomeCash
FROM $(ExcelsiorDB)..TBL_EIS_PEV_ClientParameters ClParam
INNER JOIN $(InnoTrustDB)..AccountMaster AccType 
	on ClParam.AdventID = AccType.CustomerAccountNumber
WHERE AccType.AccountTypeCode NOT IN (
		'GAPP'
		,'GAPR'
		)

SET IDENTITY_INSERT TBL_PV_ManagerCodeParameter OFF
-- TBL_PV_ValuationGiftwrapTransaction
SET IDENTITY_INSERT TBL_PV_ValuationGiftwrapTransaction ON

INSERT INTO dbo.TBL_PV_ValuationGiftwrapTransaction (
	TransactionID
	,ValuationID
	,MatchKey
	,MatchStatus
	,MatchComment
	,OrgID
	,OrgAccount1
	,GiftDate
	,GiftType
	,GiftAmount
	,MarketValue
	,MarketValueDate
	,GiftStatus
	,GiftKey
	,AccountClosedDate
	,Distributions
	,PersonGiftMapID
	,Association
	,PersonID
	,PersonName
	,LastName
	,FirstName
	,SSN
	,BirthDate
	,PersonStatus
	,DeathDate
	)
SELECT TransactionID
	,ValuationID
	,MatchKey
	,MatchStatus
	,MatchComments
	,org_pk
	,orgaccount
	,giftdate
	,gifttype
	,giftamt
	,mktvalue
	,mvdate
	,gstatus
	,gift_pk
	,acctclosed
	,distribamt
	,link1_pk
	,assoc
	,person_pk
	,personname
	,lname
	,fname
	,ssnum
	,dob
	,pstatus
	,dod
FROM $(ExcelsiorDB)..TBL_EIS_PEV_Valuation_Giftwrap_Transactions

SET IDENTITY_INSERT TBL_PV_ValuationGiftwrapTransaction OFF
-- TBL_PV_ValuationTracker
SET IDENTITY_INSERT TBL_PV_ValuationTracker ON

INSERT INTO dbo.TBL_PV_ValuationTracker (
	ValuationTrackerID
	,ValuationID
	,ModifiedDate
	,ModifiedBy
	,PreviousStatusID
	,PreviousStatus
	,CurrentStatusID
	,CurrentStatus
	)
SELECT ValuationTrackerID
	,PVTrack.ValuationID
	,MODIFIED_DATE
	,MODIFIED_USER_ID
	,PrvStatusID
	,PrvStatus
	,PVTrack.CurrentStatusID
	,CurrentStatus
FROM $(ExcelsiorDB)..TBL_EIS_PEV_Valuation_Tracker PVTrack
INNER JOIN TBL_PV_Valuation Val 
	ON PVTrack.ValuationID = Val.ValuationID

SET IDENTITY_INSERT TBL_PV_ValuationTracker OFF 
SET IDENTITY_INSERT TBL_PV_ValuationInnoTrustTransaction ON

INSERT INTO dbo.TBL_PV_ValuationInnoTrustTransaction (
	TransactionID
	,ValuationID
	,MatchKey
	,MatchStatus
	,MatchComment
	,GroupID
	--,TranCode
	,Security
	,TradeDate
	,Quantity
	,TradeAmount
	,Comment
	,TaxCode
	,TransactionNumber
	,TransactionCode
	,GiftID
	,ContactID
	)
SELECT TransactionID
	,ValuationID
	,MatchKey
	,MatchStatus
	,MatchComments
	,NULL AS InnoTrustGroupID
	--,TranCode
	,Security
	,TradeDate
	,Quantity
	,TradeAmount
	,Comments
	,NULL AS TaxCode
	,NULL AS InnoTrustTransactionNumber
	,NULL AS InnoTrustTransactionCode
	,0 AS GiftID
	,0 AS ContactID
FROM $(ExcelsiorDB)..TBL_EIS_PEV_Valuation_AXYS_Transactions

SET IDENTITY_INSERT TBL_PV_ValuationInnoTrustTransaction OFF


-- Script to update the AccountingRoutine 
UPDATE TBL_PV_ManagerCodeParameter
SET AccountingRoutine = CASE 
		WHEN UDFAMColumn044 = 'GW'
			THEN 'Giftwrap'
		WHEN UDFAMColumn044 = 'SA'
			THEN 'Sub-Accounting'
		ELSE UDFAMColumn044
		END
FROM TBL_PV_ManagerCodeParameter MgrCodeParam
INNER JOIN $(InnoTrustDB)..UDF_AccountMaster UDFAccMast
	ON MgrCodeParam.CustomerAccountNumber = UDFAccMast.CustomerAccountNumber_Key

-- Script to update the ManagerCodeName
UPDATE TBL_PV_ManagerCodeParameter
SET ManagerCodeName = ContMast.ContactName
FROM TBL_PV_ManagerCodeParameter MgrCodeParam
INNER JOIN $(InnoTrustDB)..AccountManagerCodes UDFAccMgr
	ON MgrCodeParam.ManagerCode = UDFAccMgr.ManagerCode
INNER JOIN $(InnoTrustDB)..ContactMaster ContMast
	ON ContMast.ContactID = UDFAccMgr.ContactID

