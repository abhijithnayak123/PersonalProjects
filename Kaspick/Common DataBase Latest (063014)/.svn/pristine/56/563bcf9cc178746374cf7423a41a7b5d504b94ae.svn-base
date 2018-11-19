SET NOCOUNT ON

-- Migration Script to migrate master tables data from ExcelsiorDB to KaspickDB for Middleware
DECLARE @Admin INT

SELECT @Admin = USER_ID
FROM $(ExcelsiorDB)..TBL_EIS_USER
WHERE LOGIN_NAME = 'Administrator'

-- TBL_PP_TemplateMaster migration script
SET IDENTITY_INSERT dbo.TBL_PP_TemplateMaster ON

INSERT INTO dbo.TBL_PP_TemplateMaster (
	TemplateCode
	,TemplateDescription
	--,CustomerAccountNumber
	,IsDeleted
	,ModifiedBy
	,ModifiedDate
	,CreatedBy
	,CreatedDate
	)
SELECT TemplateCode
	,TemplateDescription
	--,Custom1
	,DeleteFlag
	,1
	,GETDATE()
	,1
	,GETDATE()
FROM $(ExcelsiorDB)..TBL_PP_TEMPLATE_MASTER TEMPLATE

SET IDENTITY_INSERT dbo.TBL_PP_TemplateMaster OFF

-- TBL_PP_TemplateTypeRule migration script
INSERT INTO dbo.TBL_PP_TemplateTypeRule (
	ManagerCode
	,TemplateCode
	,TemplateDescription
	,ModifiedBy
	,ModifiedDate
	,CreatedBy
	,CreatedDate
	)
SELECT CLNT.BriefName
	,TemplateID
	,TemplateDescription
	,ISNULL(ModifiedUser.USER_ID, @Admin)
	,ModifiedDate
	,isnull(CreatedUser.USER_ID, @Admin)
	,CreatedDate
FROM $(ExcelsiorDB)..TBL_PP_TEMPLATETYPE_RULES RULES
LEFT OUTER JOIN $(ExcelsiorDB)..TBL_EIS_USER CreatedUser ON LTRIM(RTRIM(RULES.CREATEDBY)) = LTRIM(RTRIM(CreatedUser.LOGIN_NAME))
LEFT OUTER JOIN $(ExcelsiorDB)..TBL_EIS_USER ModifiedUser ON LTRIM(RTRIM(RULES.MODIFIEDBY)) = LTRIM(RTRIM(ModifiedUser.LOGIN_NAME))
INNER JOIN $(ExcelsiorDB)..CLIENT CLNT ON RULES.ClientID = CLNT.ClientID AND deleteflag = 0

-- TBL_PP_AccountRule migration script
INSERT INTO dbo.TBL_PP_AccountRule (
	CustomerAccountNumber
	,IsException
	,IsExclusion
	)
SELECT DISTINCT AdventID
	,0
	,0
FROM $(ExcelsiorDB)..TBL_PP_ACCOUNT_RULES RULES
WHERE DeleteFlag = 0
AND NOT ( ExceptionCode=0 and RuleID=2 )

UPDATE TBL_PP_AccountRule
SET IsException = 1
	,ExceptionCode = RULES.ExceptionCode
	,ExceptionDescription = Description
	,ModifiedBy = CASE 
		WHEN TMR.Modifieddate > isnull(RULES.Modifieddate, '01-01-1900')
			THEN TMR.ModifiedBy
		ELSE ISNULL(ModifiedUser.USER_ID, @Admin)
		END
	,Modifieddate = CASE 
		WHEN TMR.Modifieddate > isnull(RULES.Modifieddate, '01-01-1900')
			THEN TMR.Modifieddate
		ELSE RULES.Modifieddate
		END
	,CreatedBy = CASE 
		WHEN TMR.Createddate > isnull(RULES.Createddate, '01-01-1900')
			THEN TMR.CreatedBy
		ELSE isnull(CreatedUser.USER_ID, @Admin)
		END
	,Createddate = CASE 
		WHEN TMR.Createddate > isnull(RULES.Createddate, '01-01-1900')
			THEN TMR.Createddate
		ELSE RULES.Createddate
		END
FROM TBL_PP_AccountRule TMR
INNER JOIN $(ExcelsiorDB)..TBL_PP_ACCOUNT_RULES RULES ON TMR.CustomerAccountNumber = RULES.AdventID
LEFT OUTER JOIN $(ExcelsiorDB)..TBL_EIS_USER CreatedUser ON LTRIM(RTRIM(RULES.CREATEDBY)) = LTRIM(RTRIM(CreatedUser.LOGIN_NAME))
LEFT OUTER JOIN $(ExcelsiorDB)..TBL_EIS_USER ModifiedUser ON LTRIM(RTRIM(RULES.MODIFIEDBY)) = LTRIM(RTRIM(ModifiedUser.LOGIN_NAME))
WHERE RULES.RuleID = 2
	AND DeleteFlag = 0

UPDATE TBL_PP_AccountRule
SET IsExclusion = 1
	,ExclusionDescription = Description
	,ModifiedBy = CASE 
		WHEN TMR.Modifieddate > isnull(RULES.Modifieddate, '01-01-1900')
			THEN TMR.ModifiedBy
		ELSE ISNULL(ModifiedUser.USER_ID, @Admin)
		END
	,Modifieddate = CASE 
		WHEN TMR.Modifieddate > isnull(RULES.Modifieddate, '01-01-1900')
			THEN TMR.Modifieddate
		ELSE RULES.Modifieddate
		END
	,CreatedBy = CASE 
		WHEN TMR.Createddate > isnull(RULES.Createddate, '01-01-1900')
			THEN TMR.CreatedBy
		ELSE isnull(CreatedUser.USER_ID, @Admin)
		END
	,Createddate = CASE 
		WHEN TMR.Createddate > isnull(RULES.Createddate, '01-01-1900')
			THEN TMR.Createddate
		ELSE RULES.Createddate
		END
FROM TBL_PP_AccountRule TMR
INNER JOIN $(ExcelsiorDB)..TBL_PP_ACCOUNT_RULES RULES ON TMR.CustomerAccountNumber = RULES.AdventID
LEFT OUTER JOIN $(ExcelsiorDB)..TBL_EIS_USER CreatedUser ON LTRIM(RTRIM(RULES.CREATEDBY)) = LTRIM(RTRIM(CreatedUser.LOGIN_NAME))
LEFT OUTER JOIN $(ExcelsiorDB)..TBL_EIS_USER ModifiedUser ON LTRIM(RTRIM(RULES.MODIFIEDBY)) = LTRIM(RTRIM(ModifiedUser.LOGIN_NAME))
WHERE RULES.RuleID = 1
	AND DeleteFlag = 0

UPDATE TBL_PP_AccountRule
SET DaysValue = ISNULL(RULES.Days_Value,MRules.Days_Value) 
	,ModifiedBy = CASE 
		WHEN TMR.Modifieddate > isnull(RULES.Modifieddate, '01-01-1900')
			THEN TMR.ModifiedBy
		ELSE ISNULL(ModifiedUser.USER_ID, @Admin)
		END
	,Modifieddate = CASE 
		WHEN TMR.Modifieddate > isnull(RULES.Modifieddate, '01-01-1900')
			THEN TMR.Modifieddate
		ELSE RULES.Modifieddate
		END
	,CreatedBy = CASE 
		WHEN TMR.Createddate > isnull(RULES.Createddate, '01-01-1900')
			THEN TMR.CreatedBy
		ELSE isnull(CreatedUser.USER_ID, @Admin)
		END
	,Createddate = CASE 
		WHEN TMR.Createddate > isnull(RULES.Createddate, '01-01-1900')
			THEN TMR.Createddate
		ELSE RULES.Createddate
		END
FROM TBL_PP_AccountRule TMR
LEFT OUTER JOIN $(ExcelsiorDB)..TBL_PP_ACCOUNT_RULES RULES ON TMR.CustomerAccountNumber = RULES.AdventID
			AND RULES.RuleID = 3 AND DeleteFlag = 0
LEFT OUTER JOIN $(ExcelsiorDB)..V_EIS_EX_ACCOUNT Accnt ON Accnt.AdventID=TMR.CustomerAccountNumber
LEFT OUTER JOIN $(ExcelsiorDB)..TBL_PP_CLIENT_RULES MRules ON Accnt.ClientID = MRules.ClientID
			AND MRules.RuleID = 3 AND MRules.DeleteFlag = 0
LEFT OUTER JOIN $(ExcelsiorDB)..TBL_EIS_USER CreatedUser ON LTRIM(RTRIM(RULES.CREATEDBY)) = LTRIM(RTRIM(CreatedUser.LOGIN_NAME))
LEFT OUTER JOIN $(ExcelsiorDB)..TBL_EIS_USER ModifiedUser ON LTRIM(RTRIM(RULES.MODIFIEDBY)) = LTRIM(RTRIM(ModifiedUser.LOGIN_NAME))


-- TBL_PP_ManagerRule migration script
INSERT INTO dbo.TBL_PP_ManagerRule (
	ManagerCode
	,IsException
	,IsExclusion
	)
SELECT DISTINCT CLNT.BriefName
	,0
	,0
FROM $(ExcelsiorDB)..TBL_PP_CLIENT_RULES RULES
INNER JOIN $(ExcelsiorDB)..CLIENT CLNT ON RULES.ClientID = CLNT.ClientID
WHERE Rules.DeleteFlag = 0

UPDATE TBL_PP_ManagerRule
SET IsException = 1
	,ExceptionCode = RULES.ExceptionCode
	,ExceptionDescription = Description
	,ModifiedBy = ISNULL(ModifiedUser.USER_ID, @Admin)
	,ModifiedDate = RULES.ModifiedDate
	,CreatedBy = isnull(CreatedUser.USER_ID, @Admin)
	,CreatedDate = RULES.CreatedDate
FROM TBL_PP_ManagerRule TMR
INNER JOIN $(ExcelsiorDB)..CLIENT CLNT ON TMR.ManagerCode = CLNT.BriefName
INNER JOIN $(ExcelsiorDB)..TBL_PP_CLIENT_RULES RULES ON RULES.ClientID = CLNT.ClientID
LEFT OUTER JOIN $(ExcelsiorDB)..TBL_EIS_USER CreatedUser ON LTRIM(RTRIM(RULES.CREATEDBY)) = LTRIM(RTRIM(CreatedUser.LOGIN_NAME))
LEFT OUTER JOIN $(ExcelsiorDB)..TBL_EIS_USER ModifiedUser ON LTRIM(RTRIM(RULES.MODIFIEDBY)) = LTRIM(RTRIM(ModifiedUser.LOGIN_NAME))
WHERE RULES.RuleID = 2
	AND DeleteFlag = 0

UPDATE TBL_PP_ManagerRule
SET IsExclusion = 1
	,ExclusionDescription = Description
	,ModifiedBy = CASE 
		WHEN TMR.Modifieddate > isnull(RULES.Modifieddate, '01-01-1900')
			THEN TMR.ModifiedBy
		ELSE ISNULL(ModifiedUser.USER_ID, @Admin)
		END
	,Modifieddate = CASE 
		WHEN TMR.Modifieddate > isnull(RULES.Modifieddate, '01-01-1900')
			THEN TMR.Modifieddate
		ELSE RULES.Modifieddate
		END
	,CreatedBy = CASE 
		WHEN TMR.Createddate > isnull(RULES.Createddate, '01-01-1900')
			THEN TMR.CreatedBy
		ELSE isnull(CreatedUser.USER_ID, @Admin)
		END
	,Createddate = CASE 
		WHEN TMR.Createddate > isnull(RULES.Createddate, '01-01-1900')
			THEN TMR.Createddate
		ELSE RULES.Createddate
		END
FROM TBL_PP_ManagerRule TMR
INNER JOIN $(ExcelsiorDB)..CLIENT CLNT ON TMR.ManagerCode = CLNT.BriefName
INNER JOIN $(ExcelsiorDB)..TBL_PP_CLIENT_RULES RULES ON RULES.ClientID = CLNT.ClientID
LEFT OUTER JOIN $(ExcelsiorDB)..TBL_EIS_USER CreatedUser ON LTRIM(RTRIM(RULES.CREATEDBY)) = LTRIM(RTRIM(CreatedUser.LOGIN_NAME))
LEFT OUTER JOIN $(ExcelsiorDB)..TBL_EIS_USER ModifiedUser ON LTRIM(RTRIM(RULES.MODIFIEDBY)) = LTRIM(RTRIM(ModifiedUser.LOGIN_NAME))
WHERE RULES.RuleID = 1
	AND DeleteFlag = 0

UPDATE TBL_PP_ManagerRule
SET DaysValue = RULES.Days_Value
	,ModifiedBy = CASE 
		WHEN TMR.Modifieddate > isnull(RULES.Modifieddate, '01-01-1900')
			THEN TMR.ModifiedBy
		ELSE ISNULL(ModifiedUser.USER_ID, @Admin)
		END
	,Modifieddate = CASE 
		WHEN TMR.Modifieddate > isnull(RULES.Modifieddate, '01-01-1900')
			THEN TMR.Modifieddate
		ELSE RULES.Modifieddate
		END
	,CreatedBy = CASE 
		WHEN TMR.Createddate > isnull(RULES.Createddate, '01-01-1900')
			THEN TMR.CreatedBy
		ELSE isnull(CreatedUser.USER_ID, @Admin)
		END
	,Createddate = CASE 
		WHEN TMR.Createddate > isnull(RULES.Createddate, '01-01-1900')
			THEN TMR.Createddate
		ELSE RULES.Createddate
		END
FROM TBL_PP_ManagerRule TMR
INNER JOIN $(ExcelsiorDB)..CLIENT CLNT ON TMR.ManagerCode = CLNT.BriefName
INNER JOIN $(ExcelsiorDB)..TBL_PP_CLIENT_RULES RULES ON RULES.ClientID = CLNT.ClientID
LEFT OUTER JOIN $(ExcelsiorDB)..TBL_EIS_USER CreatedUser ON LTRIM(RTRIM(RULES.CREATEDBY)) = LTRIM(RTRIM(CreatedUser.LOGIN_NAME))
LEFT OUTER JOIN $(ExcelsiorDB)..TBL_EIS_USER ModifiedUser ON LTRIM(RTRIM(RULES.MODIFIEDBY)) = LTRIM(RTRIM(ModifiedUser.LOGIN_NAME))
WHERE RULES.RuleID = 3
	AND DeleteFlag = 0

-- TBL_PP_ExceptionMaster migration script
SET IDENTITY_INSERT dbo.TBL_PP_ExceptionMaster ON

INSERT INTO dbo.TBL_PP_ExceptionMaster (
	ExceptionCode
	,ExceptionDescription
	,IsDeleted
	,ModifiedBy
	,ModifiedDate
	,CreatedBy
	,CreatedDate
	)
SELECT ExceptionCode
	,ExceptionDescription
	,DeleteFlag
	,ISNULL(ModifiedUser.USER_ID, @Admin)
	,ModifiedDate
	,isnull(CreatedUser.USER_ID, @Admin)
	,CreatedDate
FROM $(ExcelsiorDB)..TBL_PP_EXCEPTION_MASTER EXCEPTION
LEFT OUTER JOIN $(ExcelsiorDB)..TBL_EIS_USER CreatedUser ON LTRIM(RTRIM(EXCEPTION.CREATEDBY)) = LTRIM(RTRIM(CreatedUser.LOGIN_NAME))
LEFT OUTER JOIN $(ExcelsiorDB)..TBL_EIS_USER ModifiedUser ON LTRIM(RTRIM(EXCEPTION.MODIFIEDBY)) = LTRIM(RTRIM(ModifiedUser.LOGIN_NAME))

SET IDENTITY_INSERT dbo.TBL_PP_ExceptionMaster OFF



-- TBL_PP_PaymentPublishProcessLog script
-- Dummy entry to support flat file migration
SET IDENTITY_INSERT [TBL_PP_PaymentPublishProcessLog] ON

INSERT INTO [TBL_PP_PaymentPublishProcessLog](ProcessID,
					ProcessStatus,
					NoOfFilesProcessed,
					StartTime,
					EndTime,
					CreatedBy) 
			VALUES(1, 
					'Success',
					0,
					GETDATE(),
					GETDATE(),
					1)

SET IDENTITY_INSERT [TBL_PP_PaymentPublishProcessLog] OFF 

-- [TBL_PP_PaymentPublishProcessLogDetail] script
SET IDENTITY_INSERT TBL_PP_PaymentPublishProcessLogDetail ON  
INSERT INTO TBL_PP_PaymentPublishProcessLogDetail(ProcessDetailID,
												ProcessID,
												Description,
												Error,
												StartTime,
												EndTime) 
										 VALUES(1, 
											    1,
												'Dummy entry to support flat file migration',
												NULL,
												GETDATE(),
												GETDATE())
SET IDENTITY_INSERT TBL_PP_PaymentPublishProcessLogDetail OFF 

-- TBL_PP_FlatFile migration script
SET IDENTITY_INSERT dbo.TBL_PP_FlatFile ON

INSERT INTO dbo.TBL_PP_FlatFile (
	FlatFileNo
	,ProcessID
	,ReportSLNo
	,FlatFileName
	,TemplateCode
	)
SELECT FlatFileNo
	,1			-- Dummy entry to support flat file migration
	,ReportSlNo
	,FlatFileName
	,TemplateCode
FROM $(ExcelsiorDB)..TBL_PP_FLATFILES FILES
LEFT OUTER JOIN $(ExcelsiorDB)..TBL_EIS_USER CreatedUser ON LTRIM(RTRIM(FILES.CREATEDBY)) = LTRIM(RTRIM(CreatedUser.LOGIN_NAME))
LEFT OUTER JOIN $(ExcelsiorDB)..TBL_EIS_USER ModifiedUser ON LTRIM(RTRIM(FILES.MODIFIEDBY)) = LTRIM(RTRIM(ModifiedUser.LOGIN_NAME))

SET IDENTITY_INSERT dbo.TBL_PP_FlatFile OFF
-- TBL_PP_Holiday migration script
SET IDENTITY_INSERT dbo.TBL_PP_Holiday ON

INSERT INTO dbo.TBL_PP_Holiday (
	HolidayID
	,HolidayDate
	,HolidayDescription
	,IsHoliday
	,IsDeleted
	,ModifiedBy
	,ModifiedDate
	,CreatedBy
	,CreatedDate
	)
SELECT HOLIDAYID
	,HOLIDAYDATE
	,HOLIDAYDESCRIPTION
	,HOLIDAYFLAG
	,DELETEFLAG
	,ISNULL(ModifiedUser.USER_ID, @Admin)
	,ModifiedDate
	,isnull(CreatedUser.USER_ID, @Admin)
	,CREATEDDATE
FROM $(ExcelsiorDB)..TBL_PP_HOLIDAYS HOLIDAYS
LEFT OUTER JOIN $(ExcelsiorDB)..TBL_EIS_USER CreatedUser ON LTRIM(RTRIM(HOLIDAYS.CREATEDBY)) = LTRIM(RTRIM(CreatedUser.LOGIN_NAME))
LEFT OUTER JOIN $(ExcelsiorDB)..TBL_EIS_USER ModifiedUser ON LTRIM(RTRIM(HOLIDAYS.MODIFIEDBY)) = LTRIM(RTRIM(ModifiedUser.LOGIN_NAME))

SET IDENTITY_INSERT dbo.TBL_PP_Holiday OFF

-- TBL_PP_MessageMgmt migration script
INSERT INTO dbo.TBL_PP_MessageMgmt (
	MessageMgmtCode
	,MessageMgmtType
	,Description
	,ReminderText
	,MarketingText
	,IsDeleted
	,ModifiedBy
	,ModifiedDate
	,CreatedBy
	,CreatedDate
	)
SELECT CLNT.BriefName
	,'Manager'
	,ClientDescription
	,Reminder
	,MarketingText
	,DeleteFlag
	,ISNULL(ModifiedUser.USER_ID, 1)
	,ModifiedDate
	,isnull(CreatedUser.USER_ID, 1)
	,CreatedDate
FROM $(ExcelsiorDB)..TBL_PP_MESSAGEMGMT MESSAGE
LEFT OUTER JOIN $(ExcelsiorDB)..TBL_EIS_USER CreatedUser ON LTRIM(RTRIM(MESSAGE.CREATEDBY)) = LTRIM(RTRIM(CreatedUser.LOGIN_NAME))
LEFT OUTER JOIN $(ExcelsiorDB)..TBL_EIS_USER ModifiedUser ON LTRIM(RTRIM(MESSAGE.MODIFIEDBY)) = LTRIM(RTRIM(ModifiedUser.LOGIN_NAME))
INNER JOIN $(ExcelsiorDB)..CLIENT CLNT ON MESSAGE.ClientID = CLNT.ClientID

-- TBL_PP_ContactRule migration script
INSERT INTO dbo.TBL_PP_ContactRule (
	CustomerAccountNumber
	,ContactID
	,IsException
	,IsExclusion
	)
SELECT DISTINCT pr.CustomData1
	,PartLkup.CONTACTID  AS CONTACTID 
	,0
	,0
FROM $(ExcelsiorDB)..TBL_PP_PARTICIPANT_RULES pr
INNER JOIN $(MappingDB)..TBL_ParticipantContactLookUp PartLkup ON PartLkup.ParticipantID = pr.ParticipantID
	--AND PartLkup.CUSTOMERACCOUNTNUMBER = pr.CustomData1
WHERE DeleteFlag = 0

UPDATE TBL_PP_ContactRule
SET IsException = 1
	,ExceptionCode = RULES.ExceptionCode
	,ExceptionDescription = Description
	,ModifiedBy = ISNULL(ModifiedUser.USER_ID, @Admin)
	,ModifiedDate = RULES.ModifiedDate
	,CreatedBy = isnull(CreatedUser.USER_ID, @Admin)
	,CreatedDate = RULES.CreatedDate
FROM TBL_PP_ContactRule TMR
INNER JOIN $(ExcelsiorDB)..TBL_PP_PARTICIPANT_RULES RULES ON TMR.CustomerAccountNumber = RULES.CustomData1
LEFT JOIN $(MappingDB)..TBL_ParticipantContactLookUp PartLkup ON PartLkup.ParticipantID = RULES.ParticipantID
	-- AND PartLkup.CUSTOMERACCOUNTNUMBER = RULES.CustomData1
LEFT OUTER JOIN $(ExcelsiorDB)..TBL_EIS_USER CreatedUser ON LTRIM(RTRIM(RULES.CREATEDBY)) = LTRIM(RTRIM(CreatedUser.LOGIN_NAME))
LEFT OUTER JOIN $(ExcelsiorDB)..TBL_EIS_USER ModifiedUser ON LTRIM(RTRIM(RULES.MODIFIEDBY)) = LTRIM(RTRIM(ModifiedUser.LOGIN_NAME))
WHERE RULES.RuleID = 2
	AND Rules.DeleteFlag = 0

UPDATE TBL_PP_ContactRule
SET IsExclusion = 1
	,ExclusionDescription = Description
	,ModifiedBy = CASE 
		WHEN TMR.Modifieddate > isnull(RULES.Modifieddate, '01-01-1900')
			THEN TMR.ModifiedBy
		ELSE ISNULL(ModifiedUser.USER_ID, @Admin)
		END
	,Modifieddate = CASE 
		WHEN TMR.Modifieddate > isnull(RULES.Modifieddate, '01-01-1900')
			THEN TMR.Modifieddate
		ELSE RULES.Modifieddate
		END
	,CreatedBy = CASE 
		WHEN TMR.Createddate > isnull(RULES.Createddate, '01-01-1900')
			THEN TMR.CreatedBy
		ELSE isnull(CreatedUser.USER_ID, @Admin)
		END
	,Createddate = CASE 
		WHEN TMR.Createddate > isnull(RULES.Createddate, '01-01-1900')
			THEN TMR.Createddate
		ELSE RULES.Createddate
		END
FROM TBL_PP_ContactRule TMR
INNER JOIN $(ExcelsiorDB)..TBL_PP_PARTICIPANT_RULES RULES ON TMR.CustomerAccountNumber = RULES.CustomData1
LEFT JOIN $(MappingDB)..TBL_ParticipantContactLookUp PartLkup ON PartLkup.ParticipantID = Rules.ParticipantID
	--AND PartLkup.CUSTOMERACCOUNTNUMBER = Rules.CustomData1
LEFT OUTER JOIN $(ExcelsiorDB)..TBL_EIS_USER CreatedUser ON LTRIM(RTRIM(RULES.CREATEDBY)) = LTRIM(RTRIM(CreatedUser.LOGIN_NAME))
LEFT OUTER JOIN $(ExcelsiorDB)..TBL_EIS_USER ModifiedUser ON LTRIM(RTRIM(RULES.MODIFIEDBY)) = LTRIM(RTRIM(ModifiedUser.LOGIN_NAME))
WHERE RULES.RuleID = 1
	AND RULES.DeleteFlag = 0

UPDATE TBL_PP_ContactRule
SET DaysValue = COALESCE(RULES.Days_Value,Arule.Days_Value,Mrule.Days_Value)
	,ModifiedBy = CASE 
		WHEN TMR.Modifieddate > isnull(RULES.Modifieddate, '01-01-1900')
			THEN TMR.ModifiedBy
		ELSE ISNULL(ModifiedUser.USER_ID, @Admin)
		END
	,Modifieddate = CASE 
		WHEN TMR.Modifieddate > isnull(RULES.Modifieddate, '01-01-1900')
			THEN TMR.Modifieddate
		ELSE RULES.Modifieddate
		END
	,CreatedBy = CASE 
		WHEN TMR.Createddate > isnull(RULES.Createddate, '01-01-1900')
			THEN TMR.CreatedBy
		ELSE isnull(CreatedUser.USER_ID, @Admin)
		END
	,Createddate = CASE 
		WHEN TMR.Createddate > isnull(RULES.Createddate, '01-01-1900')
			THEN TMR.Createddate
		ELSE RULES.Createddate
		END
FROM TBL_PP_ContactRule TMR
LEFT OUTER JOIN $(ExcelsiorDB)..TBL_PP_PARTICIPANT_RULES RULES ON TMR.CustomerAccountNumber = RULES.CustomData1
	AND RULES.RuleID = 3 AND RULES.DeleteFlag = 0
LEFT OUTER JOIN $(MappingDB)..TBL_ParticipantContactLookUp PartLkup ON PartLkup.ParticipantID = Rules.ParticipantID
	--AND PartLkup.CUSTOMERACCOUNTNUMBER = Rules.CustomData1
LEFT OUTER JOIN $(ExcelsiorDB)..TBL_PP_ACCOUNT_RULES Arule ON Arule.AdventID =  TMR.CustomerAccountNumber
	AND Arule.RuleID = 3 AND Arule.DeleteFlag = 0
LEFT OUTER JOIN $(ExcelsiorDB)..V_EIS_EX_ACCOUNT Acc ON Acc.AdventID=TMR.CustomerAccountNumber
LEFT OUTER JOIN $(ExcelsiorDB)..TBL_PP_CLIENT_RULES Mrule ON Acc.clientid = Mrule.clientid
	AND Mrule.RuleID = 3 AND Mrule.DeleteFlag = 0
LEFT OUTER JOIN $(ExcelsiorDB)..TBL_EIS_USER CreatedUser ON LTRIM(RTRIM(RULES.CREATEDBY)) = LTRIM(RTRIM(CreatedUser.LOGIN_NAME))
LEFT OUTER JOIN $(ExcelsiorDB)..TBL_EIS_USER ModifiedUser ON LTRIM(RTRIM(RULES.MODIFIEDBY)) = LTRIM(RTRIM(ModifiedUser.LOGIN_NAME))


-- TBL_PP_PaymentProcDetail migration script
INSERT INTO TBL_PP_PaymentProcDetail VALUES('Fetch Data For Payment Process','USP_PP_InsPendingPayment',1,'OK',NULL,1,1,getdate(),1,getdate())
INSERT INTO TBL_PP_PaymentProcDetail VALUES('Fetch Beneficiary Payment Data','USP_PP_InsStagingBeneficiaryPayment',1,'OK',NULL,3,1,getdate(),1,getdate())
INSERT INTO TBL_PP_PaymentProcDetail VALUES('Fetch PIF Payment Data','USP_PP_InsStagingPGCalcPaymentData',1,'OK',NULL,4,1,getdate(),1,getdate())
INSERT INTO TBL_PP_PaymentProcDetail VALUES('Fetch CGA Payment Data','USP_PP_InsStagingCGAPaymentDetail',1,'OK',NULL,5,1,getdate(),1,getdate())
INSERT INTO TBL_PP_PaymentProcDetail VALUES('Fetch Withholding Data','USP_PP_InsStagingYTDPaymentData',1,'OK',NULL,6,1,getdate(),1,getdate())
INSERT INTO TBL_PP_PaymentProcDetail VALUES('Start Payment Data Publishing Process','USP_PP_InsStagingPaymentExport',1,'OK',NULL,7,1,getdate(),1,getdate())

