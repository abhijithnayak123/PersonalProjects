SET NOCOUNT ON

DECLARE @ListTypeID INT
DECLARE @ListItemID INT

-- TBL_ListType Insert Script
SET IDENTITY_INSERT TBL_ListType ON;

INSERT INTO TBL_ListType (
	ListTypeID
	,ListTypeName
	,IvanTableName
	,IvanFieldLength
	,Description
	,Keycode
	,IsMutable
	,ModifiedDate
	,ModifiedUserID
	,CreatedDate
	,CreatedUserID
	,DeletedUserID
	)
SELECT List_Type_ID
	,List_Type_Name
	,Ivan_Table_Name
	,Ivan_Field_Length
	,Description
	,Keycode
	,Is_Mutable
	,ModifiedDate
	,ModifiedUserID
	,CreatedDate
	,CreatedUserID
	,Deleted_User_ID
FROM $(ExcelsiorDB)..TBL_EIS_LIST_TYPE
WHERE LIST_TYPE_NAME IN (
		'Party Type'
		,'Institution Type'
		,'Non-Profit Type'
		,'Tax Conditions'
		,'TrustParticipantTaxCondition'
		,'Months'
		,'Frequency'
		,'Service Offering'
		,'Year Type'
		,'Annual Frequency'
		,'MVD Deliverable Type'
		,'ReportLevel'
		,'Options Type'
		,'State Situs'
		,'Third Party Type'
		,'Report Sort1'
		,'Report Output Type'
		,'Salutation'
		,'Country'
		,'Issue Type'
		,'Account Type'
		,'FASB AdventID'
		,'Employee Type'
		,'Authority Level'
		,'State'
		,'CGA Type'
		,'Trustee Status'
		,'Report Category'
		,'Report Schedule'
		,'Retirement/Perpetual'
		,'CTS Version'
		,'PV Rpt'
		,'Client Logo FileType'
		,'Custom label'
		,'TARP Production Cycle'
		,'TARP Calendar'
		,'CTS Client Versions'
		,'Send To'
		,'Display Payments on Report'
		,'Status'
		,'Payment Code'
		,'WFT Request View'
		,'Policy Options Type'
		,'Trading Tasks Status'
		,'Payment Condition'
		,'Entity'
		,'Logical Value'
		,'Deliverable Category List'
		,'Website Destination'
		,'WFT Flow Status'
		,'BeneReports Validation Engine Rule Category'
		,'QueueCreationMethod'
		,'Distribution Type'
		,'Calculation'
		,'Funding Type'
		,'Investment and Trading Strategy'
		,'Asset Category'
		,'MaturityDistributionPolicy'
		,'TerminationRule'
		,'FinalPayment'
		,'Reason'
		)

SET IDENTITY_INSERT TBL_ListType OFF;
-- TBL_ListItem Insert Script
SET IDENTITY_INSERT TBL_ListItem ON

INSERT INTO TBL_ListItem (
	ListItemID
	,ListTypeID
	,ListItemName
	,IvanValue
	,Abbrev
	,DisplaySequence
	,IsMutable
	,ModifiedDate
	,ModifiedUserID
	,CreatedDate
	,CreatedUserID
	,DeletedUserID
	,CustomFlag
	)
SELECT LIST_ITEM_ID
	,LIST_TYPE_ID
	,LIST_ITEM_NAME
	,IVAN_VALUE
	,ABBREV
	,DISPLAY_SEQUENCE
	,IS_MUTABLE
	,MODIFIEDDATE
	,MODIFIEDUSERID
	,CREATEDDATE
	,CREATEDUSERID
	,DELETED_USER_ID
	,Custom_Flag
FROM $(ExcelsiorDB)..TBL_EIS_LIST_ITEM
WHERE LIST_TYPE_ID IN (
		SELECT ListTypeID
		FROM TBL_ListType
		WHERE ListTypeName != 'Entity'
		)

SELECT @ListTypeID = LIST_TYPE_ID
FROM $(ExcelsiorDB)..TBL_EIS_LIST_TYPE
WHERE LIST_TYPE_NAME = 'Entity'

INSERT INTO TBL_ListItem (
	ListItemID
	,ListTypeID
	,ListItemName
	,IvanValue
	,Abbrev
	,DisplaySequence
	,IsMutable
	,ModifiedDate
	,ModifiedUserID
	,CreatedDate
	,CreatedUserID
	,DeletedUserID
	,CustomFlag
	)
SELECT LIST_ITEM_ID
	,LIST_TYPE_ID
	,CASE LIST_ITEM_NAME
		WHEN 'Client'
			THEN 'Manager'
		WHEN 'Program'
			THEN 'Alliance'
		WHEN 'Beneficiary'
			THEN 'Contact'
		ELSE LIST_ITEM_NAME
		END
	,IVAN_VALUE
	,ABBREV
	,DISPLAY_SEQUENCE
	,IS_MUTABLE
	,MODIFIEDDATE
	,MODIFIEDUSERID
	,CREATEDDATE
	,CREATEDUSERID
	,DELETED_USER_ID
	,Custom_Flag
FROM $(ExcelsiorDB)..TBL_EIS_LIST_ITEM
WHERE LIST_TYPE_ID = @ListTypeID
	AND LIST_ITEM_NAME IN (
		'Client'
		,'Program'
		,'Account'
		,'Beneficiary'
		)

SET IDENTITY_INSERT TBL_ListItem OFF
-- TBL_ListType Insert Script
SET IDENTITY_INSERT TBL_ListType ON

INSERT INTO TBL_ListType (
	ListTypeID
	,ListTypeName
	,IvanTableName
	,IvanFieldLength
	,Description
	,Keycode
	,IsMutable
	,ModifiedDate
	,ModifiedUserID
	,CreatedDate
	,CreatedUserID
	,DeletedUserID
	)
SELECT List_Type_ID
	,'Manager Code View'
	,Ivan_Table_Name
	,Ivan_Field_Length
	,'Manager Code View'
	,'Mview'
	,Is_Mutable
	,ModifiedDate
	,ModifiedUserID
	,CreatedDate
	,CreatedUserID
	,Deleted_User_ID
FROM $(ExcelsiorDB)..TBL_EIS_LIST_TYPE
WHERE LIST_TYPE_NAME = 'Client View'

SET IDENTITY_INSERT TBL_ListType OFF

SELECT @ListTypeID = LIST_TYPE_ID
FROM $(ExcelsiorDB)..TBL_EIS_LIST_TYPE
WHERE LIST_TYPE_NAME = 'Client View'

SET IDENTITY_INSERT TBL_ListItem ON

INSERT INTO TBL_ListItem (
	ListItemID
	,ListTypeID
	,ListItemName
	,IvanValue
	,Abbrev
	,DisplaySequence
	,IsMutable
	,ModifiedDate
	,ModifiedUserID
	,CreatedDate
	,CreatedUserID
	,DeletedUserID
	,CustomFlag
	)
SELECT LIST_ITEM_ID
	,@ListTypeID
	,CASE LIST_ITEM_NAME
		WHEN 'My Clients'
			THEN 'My Manager Codes'
		WHEN 'My Back-ups'
			THEN 'My Backup Manager Codes'
		WHEN 'All Clients'
			THEN 'All Manager Codes'
		ELSE LIST_ITEM_NAME
		END
	,IVAN_VALUE
	,CASE LIST_ITEM_NAME
		WHEN 'My Clients'
			THEN 'MyMgr'
		WHEN 'My Back-ups'
			THEN 'MBkp'
		WHEN 'All Clients'
			THEN 'AllMgr'
		ELSE LIST_ITEM_NAME
		END
	,DISPLAY_SEQUENCE
	,IS_MUTABLE
	,MODIFIEDDATE
	,MODIFIEDUSERID
	,CREATEDDATE
	,CREATEDUSERID
	,DELETED_USER_ID
	,Custom_Flag
FROM $(ExcelsiorDB)..TBL_EIS_LIST_ITEM
WHERE LIST_TYPE_ID = @ListTypeID
	AND LIST_ITEM_NAME IN (
		'My Clients'
		,'My Back-ups'
		,'All Clients'
		)

SET IDENTITY_INSERT TBL_ListItem OFF
SET IDENTITY_INSERT TBL_ListType ON;

INSERT INTO TBL_ListType (
	LISTTYPEID
	,ListTypeName
	,IvanTableName
	,IvanFieldLength
	,Description
	,Keycode
	,IsMutable
	,ModifiedDate
	,ModifiedUserID
	,CreatedDate
	,CreatedUserID
	,DeletedUserID
	)
SELECT LIST_TYPE_ID
	,LIST_TYPE_NAME
	,IVAN_TABLE_NAME
	,IVAN_FIELD_LENGTH
	,DESCRIPTION
	,KEYCODE
	,IS_MUTABLE
	,MODIFIEDDATE
	,MODIFIEDUSERID
	,CREATEDDATE
	,CREATEDUSERID
	,DELETED_USER_ID
FROM $(ExcelsiorDB)..TBL_EIS_LIST_TYPE
WHERE LIST_TYPE_NAME IN (
		'Category'
		,'Business Year'
		,'Calculation Method'
		,'CGA Reserve Type'
		,'CheckACHMailRule'
		,'Check'
		,'Delivery Timing'
		,'FASB Instance Type'
		,'Filing Status'
		,'FSI option'
		,'Investment Type'
		,'Account Status'
		,'Investment Tax Status'
		,'Management'
		,'Lot Accounting'
		,'Discretionary Trade'
		,'Tranching Status'
		,'1098/9 Media'
		,'Mortality Table'
		,'Rate of Return Type'
		,'Report Type'
		,'Web Request Manager Code View'
		,'Submit Financial Statement'
		,'Tax Conditions Status'
		,'Trade'
		,'TX RTN MAILING RULE'
		,'Trust Participant View'
		)

SET IDENTITY_INSERT TBL_ListType OFF;
SET IDENTITY_INSERT TBL_ListItem ON;

INSERT INTO TBL_ListItem (
	ListTypeID
	,ListItemName
	,ListItemID
	,IvanValue
	,Abbrev
	,DisplaySequence
	,IsMutable
	,ModifiedDate
	,ModifiedUserID
	,CreatedDate
	,CreatedUserID
	,DeletedUserID
	,CustomFlag
	)
SELECT LIST_TYPE_ID
	,LIST_ITEM_NAME
	,LIST_ITEM_ID
	,IVAN_VALUE
	,ABBREV
	,DISPLAY_SEQUENCE
	,IS_MUTABLE
	,MODIFIEDDATE
	,MODIFIEDUSERID
	,CREATEDDATE
	,CREATEDUSERID
	,DELETED_USER_ID
	,Custom_Flag
FROM $(ExcelsiorDB)..TBL_EIS_LIST_ITEM
WHERE LIST_TYPE_ID IN (
		SELECT LIST_TYPE_ID
		FROM $(ExcelsiorDB)..TBL_EIS_LIST_TYPE
		WHERE LIST_TYPE_NAME IN (
				'Category'
				,'Business Year'
				,'Calculation Method'
				,'CGA Reserve Type'
				,'CheckACHMailRule'
				,'Check'
				,'Delivery Timing'
				,'FASB Instance Type'
				,'Filing Status'
				,'FSI option'
				,'Investment Type'
				,'Account Status'
				,'Investment Tax Status'
				,'Management'
				,'Lot Accounting'
				,'Discretionary Trade'
				,'Tranching Status'
				,'1098/9 Media'
				,'Mortality Table'
				,'Rate of Return Type'
				,'Report Type'
				,'Web Request Manager Code View'
				,'Submit Financial Statement'
				,'Tax Conditions Status'
				,'Trade'
				,'TX RTN MAILING RULE'
				,'Trust Participant View'
				)
			AND LIST_ITEM_NAME IN (
				'Check'
				,'Trade'
				,'CY or FY'
				,'FY'
				,'None'
				,'Specific Date'
				,'CGA'
				,'CLAT'
				,'CLU'
				,'CRAT'
				,'CRU'
				,'PIF'
				,'Exempt: None'
				,'Non-Segregated Reserve'
				,'Segregated Reserve'
				,'Client'
				,'Beneficiary'
				,'Cash Needs'
				,'Review'
				,'After as of Date'
				,'Other'
				,'Annual'
				,'Other'
				,'Applying'
				,'Considering Applying'
				,'Exempt'
				,'Not Applying'
				,'Registered'
				,'CC'
				,'CR'
				,'RR'
				,'NA'
				,'Custom'
				,'Multi-Asset'
				,'NA'
				,'Matured'
				,'Pending'
				,'Not Traded'
				,'Active'
				,'Taxable'
				,'Exempt'
				,'STGSensitive'
				,'DO NOT TRADE'
				,'Trade w/ Restrictions'
				,'Trade w/o Restrictions'
				,'NA'
				,'FIFO'
				,'LIFO'
				,'Lot'
				,'Full'
				,'None'
				,'AAChange'
				,'Addition'
				,'Complete'
				,'Tranching to Target'
				,'Not Applicable'
				,'Completed'
				,'Pending'
				,'Electronic'
				,'Paper'
				,'1937'
				,'1983 Basic'
				,'1983A'
				,'2000CM'
				,'80CNSMT'
				,'90CM'
				,'Ann2000'
				,'IRS Discount Rate'
				,'One Rate'
				,'Other'
				,'PIF Highest 3 Yrs'
				,'Client Note'
				,'Error'
				,'Mailing Insert'
				,'Special Instructions'
				,'Warning'
				,'My Requests'
				,'All Requests'
				,'My Primary Manager Codes'
				,'My Backup Manager Codes'
				,'All Manager Codes'
				,'No'
				,'Yes'
				,'Yes (Specialized)'
				,'Active'
				,'Inactive'
				,'All'
				,'Allocation Change'
				,'Buy T-Bills'
				,'Invest Excess Cash'
				,'Raise Cash'
				,'Rebalance'
				,'Reinvest Proceeds'
				,'Stock Sells'
				,'Tranche'
				,'Tranche Addition'
				,'Bonds Mature'
				,'Buy Schwab Value Advantage'
				,'Check Bids'
				,'GAP Reserve Review'
				,'Income Buy'
				,'Reverse Income Buy'
				,'Reverse Tax-Loss Sell'
				,'Stock Sells/Reinvest'
				,'Tax-Loss Sell'
				,'test'
				,'Trust Termination'
				,'To Client�Copy'
				,'To Tax Agency�Copy'
				,'To Client-NoCopy'
				,'To Tax Agency�NoCopy'
				,'Active'
				,'Active and Transition'
				,'All'
				,'Inactive'
				,'Transition'
				)
		)

SET IDENTITY_INSERT TBL_ListItem OFF;

----------------------------------------------------------------------
-- Scripts for New List Types
SELECT @ListTypeID = MAX(LIST_TYPE_ID)
FROM $(ExcelsiorDB)..TBL_EIS_LIST_TYPE

SELECT @ListItemID = MAX(LIST_ITEM_ID)
FROM $(ExcelsiorDB)..TBL_EIS_LIST_ITEM

-- Validation Engine Error Type 
SET IDENTITY_INSERT TBL_ListType ON
SET @ListTypeID = @ListTypeID + 101

INSERT INTO TBL_ListType (
	ListTypeID
	,ListTypeName
	,IvanTableName
	,IvanFieldLength
	,Description
	,Keycode
	,IsMutable
	,ModifiedDate
	,ModifiedUserID
	,CreatedDate
	,CreatedUserID
	,DeletedUserID
	)
VALUES (
	@ListTypeID
	,'ValidationEngineErrorType'
	,NULL
	,NULL
	,'Validation Engine Error Type'
	,'VEET'
	,1
	,GETDATE()
	,1
	,GETDATE()
	,1
	,NULL
	)

SET IDENTITY_INSERT TBL_ListType OFF
SET IDENTITY_INSERT TBL_ListItem ON
SET @ListItemID = @ListItemID + 101

INSERT INTO TBL_ListItem (
	ListItemID
	,ListTypeID
	,ListItemName
	,IvanValue
	,Abbrev
	,DisplaySequence
	,IsMutable
	,ModifiedDate
	,ModifiedUserID
	,CreatedDate
	,CreatedUserID
	,DeletedUserID
	,CustomFlag
	)
VALUES (
	@ListItemID
	,@ListTypeID
	,'Error'
	,NULL
	,'ERR'
	,1
	,1
	,GETDATE()
	,1
	,GETDATE()
	,1
	,NULL
	,NULL
	)

SET @ListItemID = @ListItemID + 102

INSERT INTO TBL_ListItem (
	ListItemID
	,ListTypeID
	,ListItemName
	,IvanValue
	,Abbrev
	,DisplaySequence
	,IsMutable
	,ModifiedDate
	,ModifiedUserID
	,CreatedDate
	,CreatedUserID
	,DeletedUserID
	,CustomFlag
	)
VALUES (
	@ListItemID
	,@ListTypeID
	,'Warning'
	,NULL
	,'ERR'
	,1
	,1
	,GETDATE()
	,1
	,GETDATE()
	,1
	,NULL
	,NULL
	)

SET IDENTITY_INSERT TBL_ListItem OFF
-- Account Group
SET IDENTITY_INSERT TBL_ListType ON
SET @ListTypeID = @ListTypeID + 102

INSERT INTO TBL_ListType (
	ListTypeID
	,ListTypeName
	,IvanTableName
	,IvanFieldLength
	,Description
	,Keycode
	,IsMutable
	,ModifiedDate
	,ModifiedUserID
	,CreatedDate
	,CreatedUserID
	,DeletedUserID
	)
VALUES (
	@ListTypeID
	,'Account Group'
	,NULL
	,NULL
	,'Account Group Filter'
	,'ActGrp'
	,1
	,GETDATE()
	,1
	,GETDATE()
	,1
	,NULL
	)

SET IDENTITY_INSERT TBL_ListType OFF
SET IDENTITY_INSERT TBL_ListItem ON
SET @ListItemID = @ListItemID + 103

INSERT INTO TBL_ListItem (
	ListItemID
	,ListTypeID
	,ListItemName
	,IvanValue
	,Abbrev
	,DisplaySequence
	,IsMutable
	,ModifiedDate
	,ModifiedUserID
	,CreatedDate
	,CreatedUserID
	,DeletedUserID
	,CustomFlag
	)
VALUES (
	@ListItemID
	,@ListTypeID
	,'My Customer Account Numbers'
	,NULL
	,'MyAc'
	,1
	,1
	,GETDATE()
	,1
	,GETDATE()
	,1
	,NULL
	,NULL
	)

SET @ListItemID = @ListItemID + 104

INSERT INTO TBL_ListItem (
	ListItemID
	,ListTypeID
	,ListItemName
	,IvanValue
	,Abbrev
	,DisplaySequence
	,IsMutable
	,ModifiedDate
	,ModifiedUserID
	,CreatedDate
	,CreatedUserID
	,DeletedUserID
	,CustomFlag
	)
VALUES (
	@ListItemID
	,@ListTypeID
	,'My Backup Customer Account Number'
	,NULL
	,'MyBkAc'
	,2
	,1
	,GETDATE()
	,1
	,GETDATE()
	,1
	,NULL
	,NULL
	)

SET @ListItemID = @ListItemID + 105

INSERT INTO TBL_ListItem (
	ListItemID
	,ListTypeID
	,ListItemName
	,IvanValue
	,Abbrev
	,DisplaySequence
	,IsMutable
	,ModifiedDate
	,ModifiedUserID
	,CreatedDate
	,CreatedUserID
	,DeletedUserID
	,CustomFlag
	)
VALUES (
	@ListItemID
	,@ListTypeID
	,'All Customer Account Numbers'
	,NULL
	,'AllAc'
	,3
	,1
	,GETDATE()
	,1
	,GETDATE()
	,1
	,NULL
	,NULL
	)

SET @ListItemID = @ListItemID + 106

INSERT INTO TBL_ListItem (
	ListItemID
	,ListTypeID
	,ListItemName
	,IvanValue
	,Abbrev
	,DisplaySequence
	,IsMutable
	,ModifiedDate
	,ModifiedUserID
	,CreatedDate
	,CreatedUserID
	,DeletedUserID
	,CustomFlag
	)
VALUES (
	@ListItemID
	,@ListTypeID
	,'My Inactive Customer Account Numbers'
	,NULL
	,'MyInAc'
	,4
	,1
	,GETDATE()
	,1
	,GETDATE()
	,1
	,NULL
	,NULL
	)

SET IDENTITY_INSERT TBL_ListItem OFF
-- Web Request Manager Code View
SET IDENTITY_INSERT TBL_ListType ON;
SET @ListTypeID = @ListTypeID + 103

INSERT INTO TBL_ListType (
	ListTypeID
	,ListTypeName
	,IvanTableName
	,IvanFieldLength
	,Description
	,Keycode
	,IsMutable
	,ModifiedDate
	,ModifiedUserID
	,CreatedDate
	,CreatedUserID
	,DeletedUserID
	)
VALUES (
	@ListTypeID
	,'Web Request Manager Code View'
	,NULL
	,NULL
	,'Web Request Manager Code View'
	,'WRMCV'
	,1
	,getdate()
	,1
	,getdate()
	,1
	,NULL
	)

SET IDENTITY_INSERT TBL_ListType OFF;
SET IDENTITY_INSERT TBL_ListItem ON;
SET @ListItemID = @ListItemID + 107

INSERT INTO TBL_ListItem (
	ListItemID
	,ListTypeID
	,ListItemName
	,IvanValue
	,Abbrev
	,DisplaySequence
	,IsMutable
	,ModifiedDate
	,ModifiedUserID
	,CreatedDate
	,CreatedUserID
	,DeletedUserID
	,CustomFlag
	)
VALUES (
	@ListItemID
	,@ListTypeID
	,'My Primary Manager Codes'
	,NULL
	,'MPMC'
	,1
	,1
	,GETDATE()
	,1
	,GETDATE()
	,1
	,NULL
	,NULL
	)

SET @ListItemID = @ListItemID + 108

INSERT INTO TBL_ListItem (
	ListItemID
	,ListTypeID
	,ListItemName
	,IvanValue
	,Abbrev
	,DisplaySequence
	,IsMutable
	,ModifiedDate
	,ModifiedUserID
	,CreatedDate
	,CreatedUserID
	,DeletedUserID
	,CustomFlag
	)
VALUES (
	@ListItemID
	,@ListTypeID
	,'My Backup Manager Codes'
	,NULL
	,'MBMC'
	,2
	,1
	,GETDATE()
	,1
	,GETDATE()
	,1
	,NULL
	,NULL
	)

SET @ListItemID = @ListItemID + 109

INSERT INTO TBL_ListItem (
	ListItemID
	,ListTypeID
	,ListItemName
	,IvanValue
	,Abbrev
	,DisplaySequence
	,IsMutable
	,ModifiedDate
	,ModifiedUserID
	,CreatedDate
	,CreatedUserID
	,DeletedUserID
	,CustomFlag
	)
VALUES (
	@ListItemID
	,@ListTypeID
	,'All Manager Codes'
	,NULL
	,'AMC'
	,3
	,1
	,GETDATE()
	,1
	,GETDATE()
	,1
	,NULL
	,NULL
	)

SET IDENTITY_INSERT TBL_ListItem OFF;

SET IDENTITY_INSERT TBL_ListType ON;

INSERT INTO TBL_ListType (
	LISTTYPEID
	,ListTypeName
	,IvanTableName
	,IvanFieldLength
	,Description
	,Keycode
	,IsMutable
	,ModifiedDate
	,ModifiedUserID
	,CreatedDate
	,CreatedUserID
	,DeletedUserID
	)
SELECT LIST_TYPE_ID
	,LIST_TYPE_NAME
	,IVAN_TABLE_NAME
	,IVAN_FIELD_LENGTH
	,DESCRIPTION
	,KEYCODE
	,IS_MUTABLE
	,MODIFIEDDATE
	,MODIFIEDUSERID
	,CREATEDDATE
	,CREATEDUSERID
	,DELETED_USER_ID
FROM $(ExcelsiorDB)..TBL_EIS_LIST_TYPE
WHERE LIST_TYPE_NAME IN (
		'Action on Account'
		,'Action on liquidate assets'
		,'Action on distribution'
		)

SET IDENTITY_INSERT TBL_ListType OFF;
SET IDENTITY_INSERT TBL_ListItem ON;

INSERT INTO TBL_ListItem (
	ListTypeID
	,ListItemName
	,ListItemID
	,IvanValue
	,Abbrev
	,DisplaySequence
	,IsMutable
	,ModifiedDate
	,ModifiedUserID
	,CreatedDate
	,CreatedUserID
	,DeletedUserID
	,CustomFlag
	)
SELECT LIST_TYPE_ID
	,LIST_ITEM_NAME
	,LIST_ITEM_ID
	,IVAN_VALUE
	,ABBREV
	,DISPLAY_SEQUENCE
	,IS_MUTABLE
	,MODIFIEDDATE
	,MODIFIEDUSERID
	,CREATEDDATE
	,CREATEDUSERID
	,DELETED_USER_ID
	,Custom_Flag
FROM $(ExcelsiorDB)..TBL_EIS_LIST_ITEM
WHERE LIST_TYPE_ID IN (
		SELECT LIST_TYPE_ID
		FROM $(ExcelsiorDB)..TBL_EIS_LIST_TYPE
		WHERE LIST_TYPE_NAME IN (
				'Action on Account'
				,'Action on liquidate assets'
				,'Action on distribution'
				)
			AND LIST_ITEM_NAME IN (
				'Account Closure'
				,'Shift of Interest'
				,'Yes'
				,'No'
				)
		)

SET IDENTITY_INSERT TBL_ListItem OFF;




--Client View
SET IDENTITY_INSERT TBL_ListType ON;
SET @ListTypeID = @ListTypeID + 104

INSERT INTO TBL_ListType(
	ListTypeID
	,ListTypeName
	,IvanTableName
	,IvanFieldLength
	,Description
	,Keycode
	,IsMutable
	,ModifiedDate
	,ModifiedUserID
	,CreatedDate
	,CreatedUserID
	,DeletedUserID
	)
SELECT @ListTypeID
	,LIST_TYPE_NAME
	,IVAN_TABLE_NAME
	,IVAN_FIELD_LENGTH
	,DESCRIPTION
	,KEYCODE
	,IS_MUTABLE
	,MODIFIEDDATE
	,MODIFIEDUSERID
	,CREATEDDATE
	,CREATEDUSERID
	,DELETED_USER_ID
FROM $(ExcelsiorDB)..TBL_EIS_LIST_TYPE
WHERE LIST_TYPE_NAME = 'Client View'

SET IDENTITY_INSERT TBL_ListType OFF;



SET IDENTITY_INSERT TBL_ListItem ON
SET @ListItemID = @ListItemID + 110
INSERT INTO TBL_ListItem(ListItemID,ListTypeID,ListItemName,IvanValue,Abbrev,DisplaySequence,IsMutable,ModifiedDate,ModifiedUserID,CreatedDate,CreatedUserID,DeletedUserID,CustomFlag) Values(@ListItemID,@ListTypeID,'All Active Clients',NULL,'AAC',5,1,'05-18-2007',1,'05-18-2007',1,NULL,NULL)
SET @ListItemID = @ListItemID + 111
INSERT INTO TBL_ListItem(ListItemID,ListTypeID,ListItemName,IvanValue,Abbrev,DisplaySequence,IsMutable,ModifiedDate,ModifiedUserID,CreatedDate,CreatedUserID,DeletedUserID,CustomFlag) Values(@ListItemID,@ListTypeID,'All Clients',NULL,'AClnt',4,1,'05-18-2007',1,'05-18-2007',1,NULL,NULL)
SET @ListItemID = @ListItemID + 112
INSERT INTO TBL_ListItem(ListItemID,ListTypeID,ListItemName,IvanValue,Abbrev,DisplaySequence,IsMutable,ModifiedDate,ModifiedUserID,CreatedDate,CreatedUserID,DeletedUserID,CustomFlag) Values(@ListItemID,@ListTypeID,'All Inactive Clients',NULL,'AIC',6,1,'05-18-2007',1,'05-18-2007',1,NULL,NULL)
SET @ListItemID = @ListItemID + 113
INSERT INTO TBL_ListItem(ListItemID,ListTypeID,ListItemName,IvanValue,Abbrev,DisplaySequence,IsMutable,ModifiedDate,ModifiedUserID,CreatedDate,CreatedUserID,DeletedUserID,CustomFlag) Values(@ListItemID,@ListTypeID,'My Back-ups',NULL,'MBkp',3,1,'05-18-2007',1,'05-18-2007',1,NULL,NULL)
SET @ListItemID = @ListItemID + 114
INSERT INTO TBL_ListItem(ListItemID,ListTypeID,ListItemName,IvanValue,Abbrev,DisplaySequence,IsMutable,ModifiedDate,ModifiedUserID,CreatedDate,CreatedUserID,DeletedUserID,CustomFlag) Values(@ListItemID,@ListTypeID,'My Clients',NULL,'MClnt',1,1,'05-18-2007',1,'05-18-2007',1,NULL,NULL)
SET @ListItemID = @ListItemID + 115
INSERT INTO TBL_ListItem(ListItemID,ListTypeID,ListItemName,IvanValue,Abbrev,DisplaySequence,IsMutable,ModifiedDate,ModifiedUserID,CreatedDate,CreatedUserID,DeletedUserID,CustomFlag) Values(@ListItemID,@ListTypeID,'My Inactive Clients',NULL,'MIC',2,1,'05-18-2007',1,'05-18-2007',1,NULL,NULL)
SET IDENTITY_INSERT TBL_ListItem OFF




--  Account View
SET IDENTITY_INSERT TBL_ListType ON;
SET @ListTypeID = @ListTypeID + 105

INSERT INTO TBL_ListType(
	ListTypeID
	,ListTypeName
	,IvanTableName
	,IvanFieldLength
	,Description
	,Keycode
	,IsMutable
	,ModifiedDate
	,ModifiedUserID
	,CreatedDate
	,CreatedUserID
	,DeletedUserID
	)
SELECT @ListTypeID
	,LIST_TYPE_NAME
	,IVAN_TABLE_NAME
	,IVAN_FIELD_LENGTH
	,DESCRIPTION
	,KEYCODE
	,IS_MUTABLE
	,MODIFIEDDATE
	,MODIFIEDUSERID
	,CREATEDDATE
	,CREATEDUSERID
	,DELETED_USER_ID
FROM $(ExcelsiorDB)..TBL_EIS_LIST_TYPE
WHERE LIST_TYPE_NAME = 'Account View'

SET IDENTITY_INSERT TBL_ListType OFF;


SET IDENTITY_INSERT TBL_ListItem ON
SET @ListItemID = @ListItemID + 116
INSERT INTO TBL_ListItem(ListItemID,ListTypeID,ListItemName,IvanValue,Abbrev,DisplaySequence,IsMutable,ModifiedDate,ModifiedUserID,CreatedDate,CreatedUserID,DeletedUserID,CustomFlag) Values(@ListItemID,@ListTypeID,'All Accounts',NULL,'AAcc',4,1,'05-18-2007',1,'05-18-2007',1,NULL,NULL)
SET @ListItemID = @ListItemID + 117
INSERT INTO TBL_ListItem(ListItemID,ListTypeID,ListItemName,IvanValue,Abbrev,DisplaySequence,IsMutable,ModifiedDate,ModifiedUserID,CreatedDate,CreatedUserID,DeletedUserID,CustomFlag) Values(@ListItemID,@ListTypeID,'All Active Accounts',NULL,'AAA',5,1,'05-18-2007',1,'05-18-2007',1,NULL,NULL)
SET @ListItemID = @ListItemID + 118
INSERT INTO TBL_ListItem(ListItemID,ListTypeID,ListItemName,IvanValue,Abbrev,DisplaySequence,IsMutable,ModifiedDate,ModifiedUserID,CreatedDate,CreatedUserID,DeletedUserID,CustomFlag) Values(@ListItemID,@ListTypeID,'All Inactive Accounts',NULL,'AIA',6,1,'05-18-2007',1,'05-18-2007',1,NULL,NULL)
SET @ListItemID = @ListItemID + 119
INSERT INTO TBL_ListItem(ListItemID,ListTypeID,ListItemName,IvanValue,Abbrev,DisplaySequence,IsMutable,ModifiedDate,ModifiedUserID,CreatedDate,CreatedUserID,DeletedUserID,CustomFlag) Values(@ListItemID,@ListTypeID,'My Accounts',NULL,'MAcc',1,1,'05-18-2007',1,'05-18-2007',1,NULL,NULL)
SET @ListItemID = @ListItemID + 120
INSERT INTO TBL_ListItem(ListItemID,ListTypeID,ListItemName,IvanValue,Abbrev,DisplaySequence,IsMutable,ModifiedDate,ModifiedUserID,CreatedDate,CreatedUserID,DeletedUserID,CustomFlag) Values(@ListItemID,@ListTypeID,'My Back-ups',NULL,'MBkp',3,1,'05-18-2007',1,'05-18-2007',1,NULL,NULL)
SET @ListItemID = @ListItemID + 121
INSERT INTO TBL_ListItem(ListItemID,ListTypeID,ListItemName,IvanValue,Abbrev,DisplaySequence,IsMutable,ModifiedDate,ModifiedUserID,CreatedDate,CreatedUserID,DeletedUserID,CustomFlag) Values(@ListItemID,@ListTypeID,'My Inactive Accounts',NULL,'MIA',2,1,'05-18-2007',1,'05-18-2007',1,NULL,NULL)
SET IDENTITY_INSERT TBL_ListItem OFF

update TBL_ListItem set ListItemName='My Primary Clients' where ListItemName='My Primary Manager Codes' and ListTypeID=544;
update TBL_ListItem set ListItemName='My Backup Clients' where ListItemName='My Backup Manager Codes' and ListTypeID=544;
update TBL_ListItem set ListItemName='All Clients' where ListItemName='All Manager Codes' and ListTypeID=544;

