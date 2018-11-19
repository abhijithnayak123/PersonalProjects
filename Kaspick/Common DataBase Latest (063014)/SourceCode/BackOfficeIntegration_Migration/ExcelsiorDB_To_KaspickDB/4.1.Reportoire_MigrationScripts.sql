SET NOCOUNT ON


IF OBJECT_ID(N'TEMPDB.[DBO].[#Tmp_GlobalContacts]') IS NOT NULL
BEGIN
	DROP TABLE [DBO].[#Tmp_GlobalContacts]
END

SELECT ContactID, PartyID, ClientBriefName 
INTO #Tmp_GlobalContacts
FROM $(MappingDB)..TBL_GlobalContactsLookup
WHERE PartyID NOT IN (
		SELECT PartyID
		FROM $(MappingDB)..TBL_GlobalContactsLookup
		GROUP BY PartyID
		HAVING COUNT(ContactID) > 1
		)
		
		
-- Migration Script to migrate master tables data from ExcelsiorDB to KaspickDB for Reportoire
--DECLARE @Admin INT
--SELECT @Admin = USER_ID
--FROM $(ExcelsiorDB)..TBL_EIS_USER
--WHERE LOGIN_NAME = 'Administrator'
--TBL_DLV_DeliverableProcess Migration Script
ALTER TABLE TBL_DLV_StatusFlow NOCHECK CONSTRAINT FK_TBL_DLV_StatusFlow_TBL_DLV_DeliverableProcess_DeliverableProcessID

--TBL_DLV_StatusFlow Migration Script
INSERT INTO dbo.TBL_DLV_StatusFlow (
	StatusFlowID
	,DeliverableProcessID
	,StatusFlowDefinition
	,Start_DeliverableStatusID
	,End_DeliverableStatusID
	,FlowType
	,CreatedDate
	,CreatedUserID
	,IsDeliverableDefined
	,DeliverableDefinedRole
	,RequireFileDetection
	)
SELECT StatusFlowID
	,DeliverableProcessID
	,StatusFlowDefinition
	,Start_DeliverableStatusID
	,End_DeliverableStatusID
	,FlowType
	,Createddate
	,Createduserid
	,IsDeliverableDefined
	,DeliverableDefinedRole
	,RequireFileDetection
FROM $(ExcelsiorDB)..TBL_EIS_DT_StatusFlow

ALTER TABLE TBL_DLV_StatusFlow CHECK CONSTRAINT FK_TBL_DLV_StatusFlow_TBL_DLV_DeliverableProcess_DeliverableProcessID

--TBL_DLV_DeliveryMethod Migration Script
SET IDENTITY_INSERT dbo.TBL_DLV_DeliveryMethod ON

INSERT INTO dbo.TBL_DLV_DeliveryMethod (
	DeliveryMethodID
	,DeliveryMethodName
	,DeliverySequence
	,CreatedDate
	,CreatedUserID
	)
SELECT DeliveryMethodID
	,DeliveryMethodName
	,DeliverySequence
	,CreatedDate
	,CreatedUserID
FROM $(ExcelsiorDB)..TBL_EIS_DT_Delivery_Method

SET IDENTITY_INSERT dbo.TBL_DLV_DeliveryMethod OFF

ALTER TABLE TBL_DLV_DeliverableProcessStatus NOCHECK CONSTRAINT FK_TBL_DLV_DeliverableProcessStatus_TBL_DLV_DeliverableProcess_DeliverableProcessID

--TBL_DLV_DeliverableProcessStatus Migration Script
SET IDENTITY_INSERT dbo.TBL_DLV_DeliverableProcessStatus ON

INSERT INTO dbo.TBL_DLV_DeliverableProcessStatus (
	DeliverableItemStatusID
	,DeliverableProcessID
	,DeliveryMethodID
	,StatusName
	,DisplaySequence
	,Isclosed
	,IsError
	,CreatedDate
	,CreatedUserID
	)
SELECT DeliverableItemStatusID
	,DeliverableProcessID
	,DeliveryMethodID
	,StatusName
	,DisplaySequence
	,Isclosed
	,IsError
	,CreatedDate
	,CreatedUserID
FROM $(ExcelsiorDB)..TBL_EIS_DT_DeliverableProcess_Statuses

SET IDENTITY_INSERT dbo.TBL_DLV_DeliverableProcessStatus OFF
SET IDENTITY_INSERT dbo.TBL_DLV_DeliverableProcess ON

INSERT INTO dbo.TBL_DLV_DeliverableProcess (
	DeliverableProcessID
	,DeliverableProcessName
	,DeliverableProcessDescription
	,QueueCreationMethodID
	,CustomKeyword
	,CreatedDate
	,CreatedUserID
	,NoofApprovalLevels
	,IsActive
	,FileDetectionStatusFlowID
	,PublishToWebStatusFlowID
	,MissingFileErrorStatusID
	)
SELECT DeliverableProcessID
	,DeliverableProcessName
	,DeliverableProcessDescription
	,QueueCreationMethodID
	,CustomKeyword
	,CreatedDate
	,CreatedUserID
	,NoofApprovalLevels
	,IsActive
	,FileDetectionStatusFlowID
	,PublishToWebStatusFlowID
	,MissingFileErrorStatusID
FROM $(ExcelsiorDB)..DLV_DeliverableProcess

SET IDENTITY_INSERT dbo.TBL_DLV_DeliverableProcess OFF

ALTER TABLE TBL_DLV_DeliverableProcessStatus NOCHECK CONSTRAINT FK_TBL_DLV_DeliverableProcessStatus_TBL_DLV_DeliverableProcess_DeliverableProcessID

--TBL_DLV_DeliverableServiceOffering Migration Script
SET IDENTITY_INSERT dbo.TBL_DLV_DeliverableType ON

INSERT INTO dbo.TBL_DLV_DeliverableType (
	DeliverableTypeID
	,DeliverableTypeName
	)
SELECT DeliverableTypeID
	,DeliverableTypeName
FROM $(ExcelsiorDB)..DLV_DeliverableType

SET IDENTITY_INSERT dbo.TBL_DLV_DeliverableType OFF
--TBL_DLV_Deliverable Migration Script
SET IDENTITY_INSERT dbo.TBL_DLV_Deliverable ON

INSERT INTO dbo.TBL_DLV_Deliverable (
	DeliverableID
	,DeliverableTypeID
	,DeliverableCategoryListID
	,DeliverableName
	,DeliverableDisplayName
	,DeliverableManagerCodeDescription
	,DeliverableInternalDescription
	,IncludeManagerCodeReportMatrix
	,DeliverableProcessID
	,RootFolderPath
	,FileNameConvention
	,UploadToWebsite
	,DeliverableLevel
	,Parent_DeliverableID
	,EmailGroupingID
	,SettingsLinkCode
	,IsActive
	,ReportLevelID
	,QueueFilter
	,CreatedDate
	,CreatedUserID
	,ModifiedDate
	,ModifiedUserID
	,DeletedUserID
	,OwnerID
	,Approver1RoleID
	,Approver1ID
	,Approver2RoleID
	,Approver2ID
	,OwnerRoleID
	,QueueCreationEnabled
	)
SELECT DeliverableID
	,DeliverableTypeID
	,DeliverableCategoryListID
	,DeliverableName
	,DeliverableDisplayName
	,DeliverableClientDescription
	,DeliverableInternalDescription
	,IncludeClientReportMatrix
	,DeliverableProcessID
	,RootFolderPath
	,FileNameConvention
	,UploadToWebsite
	,DeliverableLevel
	,Parent_DeliverableID
	,EmailGroupingID
	,SettingsLinkCode
	,IsActive
	,ReportLevelID
	,QueueFilter
	,CreatedDate
	,CreatedUserID
	,ModifiedDate
	,ModifiedUserID
	,DeletedUserID
	,OwnerID
	,Approver1RoleID
	,Approver1ID
	,Approver2RoleID
	,Approver2ID
	,CASE WHEN OwnerRoleID = 7 THEN 519 ELSE OwnerRoleID END AS OwnerRoleID
	,QueueCreationEnabled
FROM $(ExcelsiorDB)..DLV_Deliverable

SET IDENTITY_INSERT dbo.TBL_DLV_Deliverable OFF
--TBL_BR_FootNote Migration Script
SET IDENTITY_INSERT dbo.TBL_BR_FootNote ON

INSERT INTO dbo.TBL_BR_FootNote (
	FootnoteID
	,DeliverableID
	,FootnoteCodeID
	,FootnoteTagID
	,Footnote
	,DisplaySequence
	,FootNoteType
	,CreatedDate
	,CREATEDUSERID
	,ModifiedDate
	,ModifiedUserID
	)
SELECT FootnoteID
	,DeliverableID
	,FootnoteCodeID
	,FootnoteTagID
	,Footnote
	,DisplaySequence
	,FootNoteType
	,CreatedDate
	,CreatedUserID
	,ModifiedDate
	,ModifiedUserID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_BR_Footnotes

SET IDENTITY_INSERT dbo.TBL_BR_FootNote OFF
-- TBL_BR_ReportLabel script
SET IDENTITY_INSERT dbo.TBL_BR_ReportLabel ON

INSERT INTO TBL_BR_ReportLabel (
	ReportLabelID
	,ReportLabelTagID
	,ReportLabelCode
	,ReportLabelValue
	,DisplaySequence
	,ReportLabelDescription
	,CreatedDate
	,CREATEDUSERID
	)
SELECT ReportLabelID
	,ReportLabelTagID
	,ReportLabelCode
	,ReportLabelValue
	,DisplaySequence
	,ReportLabelDescription
	,CreatedDate
	,CreatedUserID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_BR_ReportLabel

SET IDENTITY_INSERT dbo.TBL_BR_ReportLabel OFF
--TBL_BR_SectionBlock Migration Script
SET IDENTITY_INSERT dbo.TBL_BR_SectionBlock ON

INSERT INTO dbo.TBL_BR_SectionBlock (
	SectionBlockId
	,SectionBlockTagID
	,SectionBlockName
	,SectionBlockTitle
	,SectionBlockDescription
	,CreatedDate
	,CREATEDUSERID
	)
SELECT SectionBlockId
	,SectionBlockTagID
	,SectionBlockName
	,SectionBlockTitle
	,SectionBlockDescription
	,CreatedDate
	,CreatedUserID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_BR_SECTIONBLOCK

SET IDENTITY_INSERT dbo.TBL_BR_SectionBlock OFF
--TBL_DLV_DocumentType Migration Script
SET IDENTITY_INSERT dbo.TBL_DLV_DocumentType ON

INSERT INTO dbo.TBL_DLV_DocumentType (
	DocumentTypeID
	,DeliverableID
	,DocumentTypeName
	,IsMandatory
	,SequenceNo
	,DocumentFileType
	,CreatedDate
	,CreatedUserID
	)
SELECT DocumentTypeID
	,DeliverableID
	,DocumentTypeName
	,IsMandatory
	,SequenceNo
	,DocumentFileType
	,CreatedDate
	,CreatedUserID
FROM $(ExcelsiorDB)..TBL_EIS_DT_DocumentTypes

SET IDENTITY_INSERT dbo.TBL_DLV_DocumentType OFF
--TBL_BR_ReportSectionBlockDefinition Migration Script
SET IDENTITY_INSERT dbo.TBL_BR_ReportSectionBlockDefinition ON

INSERT INTO dbo.TBL_BR_ReportSectionBlockDefinition (
	SectionBlockDefinitionId
	,SectionBlockID
	,DocumentTypeID
	,SequenceNo
	,IsEnabled
	,DisplaySectionBlock
	,CreatedDate
	,CREATEDUSERID
	)
SELECT SectionBlockDefinitionId
	,SectionBlockId
	,DocumentTypeID
	,SequenceNo
	,IsEnabled
	,DisplaySectionBlock
	,CreatedDate
	,CreatedUserID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_BR_REPORT_SECTIONBLOCK_DEFINITION

SET IDENTITY_INSERT dbo.TBL_BR_ReportSectionBlockDefinition OFF
--TBL_BR_Section Migration Script
SET IDENTITY_INSERT dbo.TBL_BR_Section ON

INSERT INTO dbo.TBL_BR_Section (
	SectionId
	,SectionTagID
	,SectionName
	,SectionTitle
	,SectionDescription
	,CreatedDate
	,CREATEDUSERID
	)
SELECT SectionId
	,SectionTagID
	,SectionName
	,SectionTitle
	,SectionDescription
	,CreatedDate
	,CreatedUserID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_BR_SECTION

SET IDENTITY_INSERT dbo.TBL_BR_Section OFF
--TBL_DLV_DeliverableFrequency Migration Script
SET IDENTITY_INSERT dbo.TBL_DLV_DeliverableFrequency ON

INSERT INTO dbo.TBL_DLV_DeliverableFrequency (
	DeliverableFrequencyID
	,FrequencyID
	,DeliverableID
	,OtherFrequencyDescription
	,CreatedDate
	,CreatedUserID
	)
SELECT DeliverableFrequencyID
	,FrequencyID
	,DeliverableID
	,OtherFrequencyDescription
	,CreatedDate
	,CreatedUserID
FROM $(ExcelsiorDB)..DLV_DeliverableFrequency

SET IDENTITY_INSERT dbo.TBL_DLV_DeliverableFrequency OFF
--TBL_DLV_QueueStatus Migration Script
SET IDENTITY_INSERT dbo.TBL_DLV_QueueStatus ON

INSERT INTO dbo.TBL_DLV_QueueStatus (
	QueueStatusID
	,DeliverableID
	,StatusName
	,Isclosed
	,RunQueueBuildJob
	,CreatedDate
	,CreatedUserID
	)
SELECT QueueStatusID
	,DeliverableID
	,StatusName
	,Isclosed
	,RunQueueBuildJob
	,CreatedDate
	,CreatedUserID
FROM $(ExcelsiorDB)..TBL_EIS_DT_Queue_Statuses

SET IDENTITY_INSERT dbo.TBL_DLV_QueueStatus OFF
SET IDENTITY_INSERT TBL_DLV_DeliverableQueue ON

--TBL_DLV_DeliverableQueue Migration Script
INSERT INTO dbo.TBL_DLV_DeliverableQueue (
	DeliverableQueueID
	,DeliverableID
	,QueueStatusID
	,GenerationMethod
	,DeliverableQueueBriefName
	,DeliverableQueueName
	,DeliverableStartDate
	,DeliverableEndDate
	,DeliverableQueueYear
	,ModifiedDate
	,ModifiedUserID
	,CreatedDate
	,CreatedUserID
	,FrequencyID
	)
SELECT DeliverableQueueID
	,DeliverableID
	,QueueStatusID
	,GenerationMethod
	,DeliverableQueueBriefName
	,DeliverableQueueName
	,DeliverableStartDate
	,DeliverableEndDate
	,DeliverableQueueYear
	,ModifiedDate
	,ModifiedUserID
	,CreatedDate
	,CreatedUserID
	,FrequencyID
FROM $(ExcelsiorDB)..TBL_EIS_DT_Deliverable_Queue

SET IDENTITY_INSERT TBL_DLV_DeliverableQueue OFF
--TBL_DLV_DeliverableItem Migration Script
SET IDENTITY_INSERT dbo.TBL_DLV_DeliverableItem ON

INSERT INTO TBL_DLV_DeliverableItem (
	DeliverableItemID
	,DeliverableQueueID
	,EmployeeContactID --ClientEmployeeID	
	,DonorBeneContactID --ContactID	
	,Beneficiarycontactrolecode --ContactRoleCode
	,Donorcontactrolecode
	,GiftKey
	,ReportType
	,ModifiedDate
	,ModifiedUserID
	,CreatedDate
	,CreatedUserID
	,AccountType
	,CustomerAccountNumber
	,ManagerCode
	,ReportDate
	,Frequency
	,FolderName
	,FILENAME
	,IsReportInaPackage
	,Approver1RoleID
	,Approver1ID
	,Approver2RoleID
	,Approver2ID
	,WaitingForNextActionRole
	,OwnerRoleID
	,OwnerID
	)
SELECT DLVITM.DeliverableItemID
	,DLVITM.DeliverableQueueID
	,CLIEMP.SUBCONTACTID --DLVITM.ClientEmployeeID 
	,ISNULL(CASE 
			WHEN DLVITM.ReportType = 'D'
				THEN PPTLKP.CONTACTID
			ELSE (
					SELECT ISNULL(CONTACTID, 0)
					FROM $(MappingDB)..TBL_BeneficiaryLookup BENLKP
					WHERE BENLKP.BENEFICIARYID = DLVITM.BENEFICIARYID
						AND BENLKP.ACCOUNTID = DLVITM.ACCOUNTID
					)
			END, 0)
	,ISNULL(CASE 
			WHEN ISNULL(DLVITM.BeneficiaryID, 0) = 0
				THEN 0
			ELSE (
					SELECT ISNULL(ROLECODE, 0)
					FROM $(MappingDB)..TBL_BeneficiaryLookup BENLKP
					WHERE BENLKP.BENEFICIARYID = DLVITM.BENEFICIARYID
						AND BENLKP.ACCOUNTID = DLVITM.ACCOUNTID
					)
			END, 0)
	,ISNULL(CASE 
			WHEN ISNULL(DLVITM.DonorID, 0) = 0
				THEN 0
			ELSE (
					SELECT ROLECODE
					FROM $(MappingDB)..TBL_DONORLookup DNRLKP
					WHERE DNRLKP.DONORID = DLVITM.DONORID
						AND DNRLKP.CustomerAccountNumber = ACCLKP.CustomerAccountNumber
					)
			END, 0)
	,DLVITM.GiftKey
	,DLVITM.ReportType
	,DLVITM.ModifiedDate
	,DLVITM.ModifiedUserID
	,DLVITM.CreatedDate
	,DLVITM.CreatedUserID
	,DLVITM.AccountType
	,ISNULL(ACCLKP.customerAccountNumber,'')
	,ISNULL(CLILKP.AccountManagerCode, '')
	,DLVITM.ReportDate
	,DLVITM.Frequency
	,DLVITM.FolderName
	,DLVITM.FILENAME
	,DLVITM.IsReportInaPackage
	,DLVITM.Approver1RoleID
	,DLVITM.Approver1ID
	,DLVITM.Approver2RoleID
	,DLVITM.Approver2ID
	,DLVITM.WaitingForNextActionRole
	,CASE WHEN DLVITM.OwnerRoleID = 7 THEN 519 ELSE DLVITM.OwnerRoleID END
	,DLVITM.OwnerID
FROM $(ExcelsiorDB)..TBL_EIS_DT_Deliverable_Items DLVITM
LEFT JOIN $(MappingDB)..TBL_ClientEmployeeLookup CLIEMP
	ON CLIEMP.employeeid = DLVITM.ClientEmployeeID
LEFT JOIN $(MappingDB)..TBL_DONORLOOKUP PPTLKP
	ON PPTLKP.DONORID = DLVITM.DONORID
LEFT JOIN $(MappingDB)..TBL_AccountLookup ACCLKP
	ON DLVITM.AccountID = ACCLKP.AccountID
LEFT JOIN $(MappingDB)..TBL_ClientLookup CLILKP
	ON CLILKP.ClientId = DLVITM.Clientid

SET IDENTITY_INSERT dbo.TBL_DLV_DeliverableItem OFF
--TBL_DLV_DeliverableTypeAttribute Migration Script
SET IDENTITY_INSERT TBL_DLV_DeliverableTypeAttribute ON

INSERT INTO dbo.TBL_DLV_DeliverableTypeAttribute (
	DeliverableTypeAttributeID
	,DeliverableID
	,AttributeSequenceNo
	,AttributePromptName
	,DataType
	,AttibuteLongName
	,Description
	,CreatedDate
	,CreatedUserID
	)
SELECT DeliverableTypeAttributeID
	,DeliverableID
	,AttributeSequenceNo
	,AttributePromptName
	,DataType
	,AttibuteLongName
	,Description
	,CreatedDate
	,CreatedUserID
FROM $(ExcelsiorDB)..TBL_EIS_DT_Deliverable_Type_Attributes

SET IDENTITY_INSERT TBL_DLV_DeliverableTypeAttribute OFF
--TBL_DLV_DeliverableYearType Migration Script
SET IDENTITY_INSERT dbo.TBL_DLV_DeliverableYearType ON

INSERT INTO dbo.TBL_DLV_DeliverableYearType (
	DeliverableYearTypeID
	,YearTypeID
	,DeliverableID
	,OtherYearTypeDescription
	,CreatedDate
	,CreatedUserID
	)
SELECT DeliverableYearTypeID
	,YearTypeID
	,DeliverableID
	,OtherYearTypeDescription
	,CreatedDate
	,CreatedUserID
FROM $(ExcelsiorDB)..DLV_DeliverableYearType

SET IDENTITY_INSERT dbo.TBL_DLV_DeliverableYearType OFF
--TBL_DLV_ItemMethodStatus Migration Script
SET IDENTITY_INSERT dbo.TBL_DLV_ItemMethodStatus ON

INSERT INTO TBL_DLV_ItemMethodStatus (
	ItemMethodStatusID
	,DeliverableItemID
	,DeliveryMethodID
	,DeliverableItemStatusID
	,Comments
	,CreatedDate
	,CreatedUserID
	,ModifiedDate
	,ModifiedUserID
	)
SELECT ItemMethodStatusID
	,DeliverableItemID
	,DeliveryMethodID
	,DeliverableItemStatusID
	,Comments
	,CreatedDate
	,CreatedUserID
	,ModifiedDate
	,ModifiedUserID
FROM $(ExcelsiorDB)..TBL_EIS_DT_ItemMethodStatus
WHERE modifieduserid IN (
		SELECT DISTINCT (userid)
		FROM tbl_ks_user
		)
	AND DeliverableItemID IN (
		SELECT DeliverableItemID
		FROM TBL_DLV_DeliverableItem
		)

SET IDENTITY_INSERT dbo.TBL_DLV_ItemMethodStatus OFF
--TBL_DLV_ManagerCodeDeliverable Migration Script
SET IDENTITY_INSERT dbo.TBL_DLV_ManagerCodeDeliverable ON

INSERT INTO dbo.TBL_DLV_ManagerCodeDeliverable (
	ManagerCodeDeliverableID
	,ManagerCode
	,DeliverableID
	,CreatedDate
	,CreatedUserID
	,ModifiedDate
	,ModifiedUserID
	,DeletedUserID
	)
SELECT ClientDeliverablesID
	,CLNT.BriefName
	,DeliverableID
	,CreatedDate
	,CreatedUserID
	,ModifiedDate
	,ModifiedUserID
	,DeletedUserID
FROM $(ExcelsiorDB)..DLV_ClientDeliverables DLV
INNER JOIN $(ExcelsiorDB)..CLIENT CLNT
	ON DLV.ClientID = CLNT.ClientID

SET IDENTITY_INSERT dbo.TBL_DLV_ManagerCodeDeliverable OFF
--TBL_DLV_Menu Migration Script
SET IDENTITY_INSERT dbo.TBL_DLV_Menu ON

INSERT INTO dbo.TBL_DLV_Menu (
	MenuID
	,DeliverableProcessID
	,NAME
	,Description
	,ModuleName
	,ImageResourceConstant
	,GroupSequence
	,DisplaySequence
	,ActionMethod
	,CreatedDate
	,CreatedUserID
	)
SELECT MenuID
	,DeliverableProcessID
	,NAME
	,Description
	,ModuleName
	,ImageResourceConstant
	,GroupSequence
	,DisplaySequence
	,ActionMethod
	,CreatedDate
	,CreatedUserID
FROM $(ExcelsiorDB)..TBL_EIS_DT_Menus

SET IDENTITY_INSERT dbo.TBL_DLV_Menu OFF
--TBL_DLV_ValidationRule Migration Script
SET IDENTITY_INSERT dbo.TBL_DLV_ValidationRule ON

INSERT INTO dbo.TBL_DLV_ValidationRule (
	ValidationRuleID
	,DeliverableID
	,RuleCategoryID
	,ValidationProcessNo
	,RuleName
	,ExecutionOrder
	,ResultType
	,DisplayMessage
	,RuleLeadTime
	,IsActive
	,StopValidationIfFails
	,DatabaseName
	,SPName
	,CreatedDate
	,CreatedUserID
	)
SELECT ValidationRuleID
	,DeliverableID
	,RuleCategoryID
	,ValidationProcessNo
	,RuleName
	,ExecutionOrder
	,ResultType
	,DisplayMessage
	,RuleLeadTime
	,IsActive
	,StopValidationIfFails
	,DatabaseName
	,SPName
	,CreatedDate
	,CreatedUserID
FROM $(ExcelsiorDB)..TBL_EIS_DT_Validation_Rules

SET IDENTITY_INSERT dbo.TBL_DLV_ValidationRule OFF
--TBL_BR_AssetTypeAssetClass Migration Script
SET IDENTITY_INSERT dbo.TBL_BR_AssetTypeAssetClass ON

INSERT INTO dbo.TBL_BR_AssetTypeAssetClass (
	SecurityTypeID
	,AssetClassID
	,SecurityTypeDescription
	,SecurityTypeDisplayOrder
	,SecurityType
	,ColorSchemeDetailsID
	,CreatedDate
	,CREATEDUSERID
	)
SELECT SecurityTypeID
	,AssetClassID
	,SecurityTypeDescription
	,SecurityTypeDisplayOrder
	,SecurityType
	,ColorSchemeDetailsID
	,CREATEDDATE
	,CREATEDUSERID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_BR_AssetType_AssetClass

SET IDENTITY_INSERT dbo.TBL_BR_AssetTypeAssetClass OFF

----TBL_DLV_DeliverableItemDeliverableTypeAttribute Migration Script
INSERT INTO TBL_DLV_DeliverableItemDeliverableTypeAttribute (
	DeliverableItemID
	,DeliverableTypeAttributeID
	,AttributeValue
	,CreatedDate
	,CreatedUserID
	,ModifiedDate
	,ModifiedUserID
	)
SELECT DeliverableItemID
	,DeliverableTypeAttributeID
	,AttributeValue
	,CreatedDate
	,CreatedUserID
	,ModifiedDate
	,ModifiedUserID
FROM $(ExcelsiorDB)..TBL_EIS_DT_Deliverable_Item_Deliverable_Type_Attributes
WHERE DeliverableItemID IN (
		SELECT DeliverableItemID
		FROM TBL_DLV_DeliverableItem
		)

--TBL_DLV_DeliverableItemDocument Migration Script
SET IDENTITY_INSERT dbo.TBL_DLV_DeliverableItemDocument ON

INSERT INTO dbo.TBL_DLV_DeliverableItemDocument (
	DeliverableDocumentsID
	,DeliverableItemID
	,DocumentTypeID
	,FilePath
	,CreatedDate
	,CreatedUserID
	)
SELECT DeliverableDocumentsID
	,DeliverableItemID
	,DocumentTypeID
	,FilePath
	,CreatedDate
	,CreatedUserID
FROM $(ExcelsiorDB)..TBL_EIS_DT_Deliverable_Item_Documents
WHERE DeliverableItemID IN (
		SELECT DeliverableItemID
		FROM TBL_DLV_DeliverableItem
		)

SET IDENTITY_INSERT dbo.TBL_DLV_DeliverableItemDocument OFF

--TBL_DLV_DeliverableMethod Migration Script
INSERT INTO dbo.TBL_DLV_DeliverableMethod (
	DeliverableID
	,DeliveryMethodID
	,SequenceNo
	,IsPrimary
	,CreatedDate
	,CreatedUserID
	)
SELECT DeliverableID
	,DeliveryMethodID
	,SequenceNo
	,IsPrimary
	,CreatedDate
	,CreatedUserID
FROM $(ExcelsiorDB)..TBL_EIS_DT_Deliverable_Methods

--TBL_DLV_DeliverableQueueStatusChangeLog Migration Script
SET IDENTITY_INSERT dbo.TBL_DLV_DeliverableQueueStatusChangeLog ON

INSERT INTO dbo.TBL_DLV_DeliverableQueueStatusChangeLog (
	DeliverableQueueStatusChangeLogID
	,DeliverableQueueID
	,New_StatusID
	,Old_StatusID
	,Comment
	,StatusChangeUserID
	,StatusChangeDT
	)
SELECT DeliverableQueueStatusChangeLogID
	,DeliverableQueueID
	,New_StatusID
	,Old_StatusID
	,Comment
	,StatusChangeUserID
	,StatusChangeDT
FROM $(ExcelsiorDB)..TBL_EIS_DT_DeliverableQueue_StatusChangeLog

SET IDENTITY_INSERT dbo.TBL_DLV_DeliverableQueueStatusChangeLog OFF
--TBL_DLV_MethodStatusChangeLog Migration Script
SET IDENTITY_INSERT dbo.TBL_DLV_MethodStatusChangeLog ON

INSERT INTO dbo.TBL_DLV_MethodStatusChangeLog (
	MethodStatusChangeLogID
	,New_StatusID
	,Old_StatusID
	,Comment
	,StatusChangeUserID
	,StatusChangeDT
	,ItemMethodStatusID
	)
SELECT MethodStatusChangeLogID
	,New_StatusID
	,Old_StatusID
	,Comment
	,StatusChangeUserID
	,StatusChangeDT
	,ItemMethodStatusID
FROM $(ExcelsiorDB)..TBL_EIS_DT_Methods_StatusChangeLog
WHERE ItemMethodStatusID IN (
		SELECT ItemMethodStatusID
		FROM TBL_DLV_ItemMethodStatus
		)

SET IDENTITY_INSERT dbo.TBL_DLV_MethodStatusChangeLog OFF
--TBL_DLV_ReportXMLEditLog Migration Script
SET IDENTITY_INSERT dbo.TBL_DLV_ReportXMLEditLog ON

INSERT INTO dbo.TBL_DLV_ReportXMLEditLog (
	ReportXMLEditLogID
	,DeliverableItemID
	,ReportXMLDate
	,Comments
	,Type
	,CreatedUserID
	,CreatedDate
	)
SELECT ReportXMLEditLogID
	,DeliverableItemID
	,ReportXMLDate
	,Comments
	,Type
	,CreatedUserID
	,CreatedDate
FROM $(ExcelsiorDB)..TBL_EIS_DT_ReportXML_Edit_Log

SET IDENTITY_INSERT dbo.TBL_DLV_ReportXMLEditLog OFF

--TBL_DLV_StatusFlowRole Migration Script
INSERT INTO dbo.TBL_DLV_StatusFlowRole (
	StatusFlowID
	,ROLE_ID
	,CreatedDate
	,CreatedUserID
	)
SELECT StatusFlowID
	,ROLE_ID
	,CreatedDate
	,CreatedUserID
FROM $(ExcelsiorDB)..TBL_EIS_DT_StatusFlowRoles

--TBL_DLV_ValidationResult Migration Script
SET IDENTITY_INSERT dbo.TBL_DLV_ValidationResult ON

INSERT INTO TBL_DLV_ValidationResult (
	ValidationResultID
	,ValidationRuleID
	,DeliverableItemID
	,RunDate
	,ErrorDetails
	,IsAcknowledged
	,UserID
	)
SELECT ValidationResultID
	,ValidationRuleID
	,DeliverableItemID
	,RunDate
	,ErrorDetails
	,IsAcknowledged
	,UserID
FROM $(ExcelsiorDB)..TBL_EIS_DT_Validation_Results

SET IDENTITY_INSERT dbo.TBL_DLV_ValidationResult OFF
--TBL_DLV_ValidationRuleAttribute Migration Script
SET IDENTITY_INSERT dbo.TBL_DLV_ValidationRuleAttribute ON

INSERT INTO dbo.TBL_DLV_ValidationRuleAttribute (
	ValidationRuleAttributeID
	,ValidationRuleID
	,Attribute
	,ExpectedValue
	,EvaluationOrder
	,OperatorType
	,DataType
	,CreatedDate
	,CreatedUserID
	)
SELECT ValidationRuleAttributeID
	,ValidationRuleID
	,Attribute
	,ExpectedValue
	,EvaluationOrder
	,OperatorType
	,Datatype
	,CreatedDate
	,CreatedUserID
FROM $(ExcelsiorDB)..TBL_EIS_DT_Validation_Rule_Attribute

SET IDENTITY_INSERT dbo.TBL_DLV_ValidationRuleAttribute OFF

--TBL_DLV_ManagerCodeDeliverableFrequency Migration Script
INSERT INTO dbo.TBL_DLV_ManagerCodeDeliverableFrequency (
	ManagerCodeDeliverableID
	,DeliverableFrequencyID
	,CreatedDate
	,CreatedUserID
	,ModifiedDate
	,ModifiedUserID
	)
SELECT ClientDeliverablesID
	,DeliverableFrequencyID
	,CreatedDate
	,CreatedUserID
	,ModifiedDate
	,ModifiedUserID
FROM $(ExcelsiorDB)..DLV_ClientDeliverableFrequency

--TBL_DLV_ManagerCodeDeliverableReportInPackage Migration Script
SET IDENTITY_INSERT dbo.TBL_DLV_ManagerCodeDeliverableReportInPackage ON

INSERT INTO dbo.TBL_DLV_ManagerCodeDeliverableReportInPackage (
	ManagerCodeDeliverableLevel2
	,ManagerCodeDeliverableID
	,DeliverableID
	,CreatedDate
	,CreatedUserID
	,ModifiedDate
	,ModifiedUserID
	)
SELECT ClientDeliverablesLevel2
	,ClientDeliverablesID
	,DeliverableID
	,CreatedDate
	,CreatedUserID
	,ModifiedDate
	,ModifiedUserID
FROM $(ExcelsiorDB)..DLV_ClientDeliverableReportInPackage

SET IDENTITY_INSERT dbo.TBL_DLV_ManagerCodeDeliverableReportInPackage OFF
SET IDENTITY_INSERT TBL_DLV_ManagerCodeDeliverableEmployee ON

--TBL_DLV_ManagerCodeDeliverableEmployee Migration Script
INSERT INTO dbo.TBL_DLV_ManagerCodeDeliverableEmployee (
	ManagerCodeDeliverableID
	,EmployeeID
	,ManagerCodeDeliverableEmployeeID
	,EmailListRecipientID
	,CreatedDate
	,CreatedUserID
	,ModifiedDate
	,ModifiedUserID
	)
SELECT ClientDeliverablesID
	,EmployeeID
	,ClientDeliverablesEmployeeID
	,EmailListRecipientID
	,CreatedDate
	,CreatedUserId
	,ModifiedDate
	,ModifiedUserId
FROM $(ExcelsiorDB)..DLV_ClientDeliverablesEmployee

SET IDENTITY_INSERT TBL_DLV_ManagerCodeDeliverableEmployee OFF

--TBL_DLV_ManagerCodeDeliverableYearType Migration Script
INSERT INTO dbo.TBL_DLV_ManagerCodeDeliverableYearType (
	ManagerCodeDeliverableID
	,DeliverableYearTypeID
	,OtherMonth
	,OtherDayofMonth
	,CreatedDate
	,CreatedUserID
	,ModifiedDate
	,ModifiedUserID
	)
SELECT ClientDeliverablesID
	,DeliverableYearTypeID
	,OtherMonth
	,OtherDayofMonth
	,CreatedDate
	,CreatedUserID
	,ModifiedDate
	,ModifiedUserID
FROM $(ExcelsiorDB)..DLV_ClientDeliverableYearType

--TBL_DLV_DeliverableServiceOffering Migration Script
INSERT INTO dbo.TBL_DLV_DeliverableServiceOffering (
	DeliverableID
	,ServiceOfferingID
	)
SELECT DeliverableID
	,ServiceOfferingID
FROM $(ExcelsiorDB)..DLV_DeliverableServiceOffering

--TBL_DLV_DeliverableWebsiteOption Migration Script
INSERT INTO dbo.TBL_DLV_DeliverableWebsiteOption (
	DeliverableID
	,AccountRequired
	,MVDDeliverableID
	,WebsiteDestinationID
	,ContactRequired
	,ExpiryYears
	,ExpiryMonths
	,ExpiryDays
	,AllowExpirationDateOverride
	,MaximumFileSize
	,UploadEmailTemplateID
	,ReplaceEmailTemplateID
	,CreatedDate
	,CreatedUserID
	,ModifiedDate
	,ModifiedUserID
	,DeletedUserID
	)
SELECT DeliverableID
	,AccountRequired
	,MVDDeliverableID
	,WebsiteDestinationID
	,ParticipantRequired
	,ExpiryYears
	,ExpiryMonths
	,ExpiryDays
	,AllowExpirationDateOverride
	,MaximumFileSize
	,UploadEmailTemplateID
	,ReplaceEmailTemplateID
	,CreatedDate
	,CreatedUserID
	,ModifiedDate
	,ModifiedUserID
	,DeletedUserID
FROM $(ExcelsiorDB)..DLV_DeliverableWebsiteOption

--TBL_DLV_MenuStatusFlow Migration Script
SET IDENTITY_INSERT dbo.TBL_DLV_MenuStatusFlow ON

INSERT INTO dbo.TBL_DLV_MenuStatusFlow (
	MenuStatusFlowID
	,MenuID
	,StatusFlowID
	)
SELECT MenuStatusFlowID
	,MenuID
	,StatusFlowID
FROM $(ExcelsiorDB)..DLV_Menu_StatusFlow

SET IDENTITY_INSERT dbo.TBL_DLV_MenuStatusFlow OFF
--TBL_DLV_Role Migration Script
SET IDENTITY_INSERT dbo.TBL_DLV_Role ON

INSERT INTO dbo.TBL_DLV_Role (
	RoleID
	,BriefName
	,FullName
	,Description
	,CreatedDate
	,CreatedUserID
	)
SELECT RoleID
	,BriefName
	,FullName
	,Description
	,CreatedDate
	,CreatedUserID
FROM $(ExcelsiorDB)..DLV_ROLE

SET IDENTITY_INSERT dbo.TBL_DLV_Role OFF
--TBL_DLV_STAFFROLE Migration Script
SET IDENTITY_INSERT dbo.TBL_DLV_STAFFROLE ON

INSERT INTO dbo.TBL_DLV_STAFFROLE (
	StaffRoleID
	,RoleID
	,UserID
	,ManagerCode
	)
SELECT StaffRoleID
	,RoleID
	,UserID
	,CLNT.BriefName
FROM $(ExcelsiorDB)..DLV_STAFFROLE STAFF
INNER JOIN $(ExcelsiorDB)..CLIENT CLNT
	ON STAFF.ClientID = CLNT.ClientID

SET IDENTITY_INSERT dbo.TBL_DLV_STAFFROLE OFF
--TBL_BR_AssetClass Migration Script
SET IDENTITY_INSERT dbo.TBL_BR_AssetClass ON

INSERT INTO dbo.TBL_BR_AssetClass (
	AssetClassID
	,AssetClassDescription
	,AssetClassDisplayOrder
	,AssetClassCode
	,ColorSchemeDetailsID
	,CreatedDate
	,CREATEDUSERID
	)
SELECT AssetClassID
	,AssetClassDescription
	,AssetClassDisplayOrder
	,AssetClassCode
	,ColorSchemeDetailsID
	,CREATEDDATE
	,CREATEDUSERID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_BR_AssetClass

SET IDENTITY_INSERT dbo.TBL_BR_AssetClass OFF
--TBL_BR_Audit_ReportGeneration Migration Script
SET IDENTITY_INSERT dbo.TBL_BR_Audit_ReportGeneration ON

INSERT INTO dbo.TBL_BR_Audit_ReportGeneration (
	ReportGenerationId
	,DeliverableItemID
	,DeliverableDocumentsID
	,SessionID
	,XMLGenerationStartDateTime
	,InDesignRequestDateTime
	,InDesignResponseDateTime
	,ReportGenerationUserID
	,ReportGenerationStatus
	,InDesignRequestData
	,ReportGenerationFailureReason
	,IndesignErrorCode
	,Comments
	)
SELECT ReportGenerationId
	,DeliverableItemID
	,DeliverableDocumentsID
	,SessionID
	,XMLGenerationStartDateTime
	,InDesignRequestDateTime
	,InDesignResponseDateTime
	,ReportGenerationUserId
	,ReportGenerationStatus
	,InDesignRequestData
	,ReportGenerationFailureReason
	,IndesignErrorCode
	,Comments
FROM $(ExcelsiorDB)..TBL_EIS_RPT_BR_AUDIT_REPORT_GENERATION

SET IDENTITY_INSERT dbo.TBL_BR_Audit_ReportGeneration OFF
--TBL_BR_FootNoteSection Migration Script
SET IDENTITY_INSERT dbo.TBL_BR_FootNoteSection ON

INSERT INTO dbo.TBL_BR_FootNoteSection (
	FootNotesSectionID
	,FootnoteID
	,SectionID
	,CreatedDate
	,CREATEDUSERID
	)
SELECT FootNotesSectionID
	,FootnoteID
	,SectionId
	,CreatedDate
	,CreatedUserID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_BR_FootNotes_Section

SET IDENTITY_INSERT dbo.TBL_BR_FootNoteSection OFF
----TBL_BR_ReportColorScheme Migration Script
--SET IDENTITY_INSERT dbo.TBL_BR_ReportColorScheme ON
--INSERT INTO dbo.TBL_BR_ReportColorScheme (
--	ColorSchemeID
--	,ColorSchemeBriefName
--	,ColorSchemeDescription
--	,CreatedDate
--	,CREATEDUSERID
--	)
--SELECT ColorSchemeID
--	,ColorSchemeBriefName
--	,ColorSchemeDescription
--	,CreatedDate
--	,CreatedUserID
--FROM $(ExcelsiorDB)..TBL_EIS_RPT_BR_REPORT_COLOR_SCHEME
--SET IDENTITY_INSERT dbo.TBL_BR_ReportColorScheme OFF
----TBL_BR_ReportColorSchemeDetail Migration Script
--SET IDENTITY_INSERT dbo.TBL_BR_ReportColorSchemeDetail ON
--INSERT INTO dbo.TBL_BR_ReportColorSchemeDetail (
--	ColorSchemeDetailsID
--	,ColorSchemeID
--	,ColorSequenceNo
--	,ColorSchemeTagID
--	,ColorSchemeDetailsName
--	,ColorSchemeDetailsValue
--	,ColorSchemeDetailsRGBValue
--	,ColorSchemeDetails
--	,CreatedDate
--	,CREATEDUSERID
--	)
--SELECT ColorSchemeDetailsID
--	,ColorSchemeID
--	,ColorSequenceNo
--	,ColorSchemeTagID
--	,ColorSchemeDetailsName
--	,ColorSchemeDetailsValue
--	,ColorSchemeDetailsRGBValue
--	,ColorSchemeDetails
--	,CreatedDate
--	,CreatedUserID
--FROM $(ExcelsiorDB)..TBL_EIS_RPT_BR_REPORT_COLOR_SCHEME_DETAILS
--SET IDENTITY_INSERT dbo.TBL_BR_ReportColorSchemeDetail OFF
----TBL_BR_ReportColorSchemeTag Migration Script
--SET IDENTITY_INSERT dbo.TBL_BR_ReportColorSchemeTag ON
--INSERT INTO dbo.TBL_BR_ReportColorSchemeTag (
--	ColorSchemeTagID
--	,ColorSchemeTag
--	,ColorSchemeDetails
--	,CreatedDate
--	,CREATEDUSERID
--	)
--SELECT ColorSchemeTagID
--	,ColorSchemeTag
--	,ColorSchemeDetails
--	,CreatedDate
--	,CreatedUserID
--FROM $(ExcelsiorDB)..TBL_EIS_RPT_BR_REPORT_COLOR_SCHEME_tags
--SET IDENTITY_INSERT dbo.TBL_BR_ReportColorSchemeTag OFF
--TBL_BR_ReportSectionDefinition Migration Script
SET IDENTITY_INSERT dbo.TBL_BR_ReportSectionDefinition ON

INSERT INTO dbo.TBL_BR_ReportSectionDefinition (
	SectionDefinitionId
	,SectionBlockDefinitionID
	,SectionId
	,SequenceNo
	,SP_Name
	,ParameterList
	,IsEnabled
	,DisplaySection
	,Description
	,CreatedDate
	,CREATEDUSERID
	)
SELECT SectionDefinitionId
	,SectionBlockDefinitionId
	,SectionId
	,SequenceNo
	,SP_Name
	,ParameterList
	,IsEnabled
	,DisplaySection
	,Description
	,CreatedDate
	,CreatedUserID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_BR_REPORT_SECTION_DEFINITION

SET IDENTITY_INSERT dbo.TBL_BR_ReportSectionDefinition OFF
--TBL_BR_Audit_Session Migration Script
SET IDENTITY_INSERT dbo.TBL_BR_Audit_Session ON

INSERT INTO dbo.TBL_BR_Audit_Session (
	SessionID
	,SessionType
	,StartDateTime
	,EndDateTime
	,SessionStatus
	,SessionResult
	,CREATEDUSERID
	)
SELECT SessionID
	,SessionType
	,StartDateTime
	,EndDateTime
	,SessionStatus
	,SessionResult
	,ISNULL(USR.UserID, 1)
FROM $(ExcelsiorDB)..TBL_EIS_RPT_BR_AUDIT_SESSION AudSession
LEFT OUTER JOIN tbl_ks_user USR
	ON USR.UserID = AudSession.CreatedUserID

SET IDENTITY_INSERT dbo.TBL_BR_Audit_Session OFF
--TBL_DLV_AuditDeliverableQueue Migration Script
SET IDENTITY_INSERT dbo.TBL_DLV_Audit_DeliverableQueue ON

INSERT INTO dbo.TBL_DLV_Audit_DeliverableQueue (
	AUDIT_SEQUENCE
	,AUDIT_USER_ID
	,AUDIT_DATETIME
	,AUDIT_FLAG
	,AUDIT_TABLE
	,AUDIT_DETAILS
	,DeliverableQueueID
	,DeliverableTypeID
	,QueueStatusID
	,GenerationMethod
	,DeliverableQueueBriefName
	,DeliverableQueueName
	,DeliverableStartDate
	,DeliverableEndDate
	,DeliverableQueueYear
	,ModifiedDate
	,ModifiedUserID
	,CreatedDate
	,CREATEDUSERID
	,DeletedUserID
	)
SELECT AUDIT_SEQUENCE
	,AUDIT_USER_ID
	,AUDIT_DATETIME
	,AUDIT_FLAG
	,AUDIT_TABLE
	,AUDIT_DETAILS
	,DeliverableQueueID
	,DeliverableTypeID
	,QueueStatusID
	,GenerationMethod
	,DeliverableQueueBriefName
	,DeliverableQueueName
	,DeliverableStartDate
	,DeliverableEndDate
	,DeliverableQueueYear
	,ModifiedDate
	,ModifiedUserID
	,CreatedDate
	,CreatedUserID
	,DeletedUserID
FROM $(ExcelsiorDB)..TBL_EIS_DT_AUDIT_DELIVERABLE_QUEUE

SET IDENTITY_INSERT dbo.TBL_DLV_Audit_DeliverableQueue OFF
-- TBL_DLV_Audit_DeliverableItem migration script
--SET IDENTITY_INSERT dbo.TBL_DLV_Audit_DeliverableItem ON
--INSERT INTO TBL_DLV_Audit_DeliverableItem (
--	AUDIT_SEQUENCE
--	,AUDIT_USER_ID
--	,AUDIT_DATETIME
--	,AUDIT_FLAG
--	,AUDIT_TABLE
--	,AUDIT_DETAILS
--	,DeliverableItemID
--	,DeliverableQueueID
--	,ManagerCode
--	,ClientEmployeeID
--	,CustomerAccountNumber
--	,ContactID
--	,ContactRoleCode
--	,GiftKey
--	,ModifiedDate
--	,ModifiedUserID
--	,CreatedDate
--	,CREATEDUSERID
--	,DeletedUserID
--	)
--SELECT AUDIT_SEQUENCE
--	,AUDIT_USER_ID
--	,AUDIT_DATETIME
--	,AUDIT_FLAG
--	,AUDIT_TABLE
--	,AUDIT_DETAILS
--	,DeliverableItemID
--	,DeliverableQueueID
--	,CLNT.BriefName
--	,ClientEmployeeID
--	,DGA.AdventID
--	,TrustparticipantID
--	,BeneficiaryID
--	,DonorID
--	,GiftKey
--	,ModifiedDate
--	,ModifiedUserID
--	,CreatedDate
--	,CreatedUserID
--	,DeletedUserID
--FROM $(ExcelsiorDB)..TBL_EIS_DT_AUDIT_DELIVERABLE_ITEMS DLVITM
--INNER JOIN $(ExcelsiorDB)..CLIENT CLNT
--	ON DLVITM.ClientID = CLNT.ClientID
--INNER JOIN $(ExcelsiorDB)..DEFERREDGIFTACCOUNT DGA
--	ON DLVITM.AccountID = DGA.AccountID
--SET IDENTITY_INSERT dbo.TBL_DLV_AuditDeliverableItem OFF
-- TBL_DLV_AuditDeliverableItemDeliverableTypeAttribute script
SET IDENTITY_INSERT dbo.TBL_DLV_Audit_DeliverableItemDeliverableTypeAttribute ON

INSERT INTO TBL_DLV_Audit_DeliverableItemDeliverableTypeAttribute (
	AUDIT_SEQUENCE
	,AUDIT_USER_ID
	,AUDIT_DATETIME
	,AUDIT_FLAG
	,AUDIT_TABLE
	,AUDIT_DETAILS
	,DeliverableItemID
	,DeliverableTypeAttributeID
	,AttributeValue
	,CreatedDate
	,CreatedUserID
	,ModifiedDate
	,ModifiedUserID
	)
SELECT AUDIT_SEQUENCE
	,AUDIT_USER_ID
	,AUDIT_DATETIME
	,AUDIT_FLAG
	,AUDIT_TABLE
	,AUDIT_DETAILS
	,DeliverableItemID
	,DeliverableTypeAttributeID
	,AttributeValue
	,CreatedDate
	,CreatedUserID
	,ModifiedDate
	,ModifiedUserID
FROM $(ExcelsiorDB)..TBL_EIS_DT_AUDIT_DELIVERABLE_ITEM_DELIVERABLE_TYPE_ATTRIBUTES

SET IDENTITY_INSERT dbo.TBL_DLV_Audit_DeliverableItemDeliverableTypeAttribute OFF

-- TBL_DLV_STG_FMVAccountClosed script
INSERT INTO TBL_DLV_STG_FMVAccountClosed (
	CustomerAccountNumber
	,GiftKey
	,DQEndDate
	,DQueueID
	,FMV
	,AcctClosed
	)
SELECT adventid
	,Giftkey
	,DQEndDate
	,DQueueID
	,FMV
	,acctclosed
FROM $(ExcelsiorDB)..TBL_EIS_DT_STG_FMV_AcctClosed

-- TBL_EIS_DT_STG_EndingMarketValue script
INSERT INTO TBL_EIS_DT_STG_EndingMarketValue (
	DQEndDate
	,DQueueID
	,AccountID
	,EndingMarketValue
	,PortfolioObjectiveDescription
	)
SELECT DQEndDate
	,DQueueID
	,AccountID
	,EndingMarketValue
	,PortfolioObjectiveDescription
FROM $(ExcelsiorDB)..TBL_EIS_DT_STG_EndingMarketValue

---- TBL_DLV_STG_EndingMarketValue script 
--INSERT INTO TBL_DLV_STG_EndingMarketValue (
--	DQEndDate
--	,DQueueID
--	,CustomerAccountNumber
--	,EndingMarketValue
--	,PortfolioObjectiveDescription
--	)
--SELECT DQEndDate
--	,DQueueID
--	,DGA.AdventID
--	,EndingMarketValue
--	,PortfolioObjectiveDescription
--FROM $(ExcelsiorDB)..TBL_EIS_DT_STG_EndingMarketValue EMV
--INNER JOIN $(ExcelsiorDB)..DEFERREDGIFTACCOUNT DGA
--	ON EMV.AccountID = DGA.AccountID
-- TBL_EIS_DT_STG_Detect_PostDataLoad script
INSERT INTO TBL_EIS_DT_STG_Detect_PostDataLoad (
	DQEndDate
	,DQueueID
	,AccountID
	,TLAsofDate_Date
	,DtStartDate
	)
SELECT DQEndDate
	,DQueueID
	,AccountID
	,TLAsofDate_Date
	,dtStartDate
FROM $(ExcelsiorDB)..TBL_EIS_DT_STG_Detect_PostDataLoad

---- TBL_DLV_STG_DetectPostDataLoad script
--INSERT INTO TBL_DLV_STG_DetectPostDataLoad (
--	DQEndDate
--	,DQueueID
--	,CustomerAccountNumber
--	,TLAsofDate_Date
--	,DtStartDate
--	)
--SELECT DQEndDate
--	,DQueueID
--	,DGA.AdventID
--	,TLAsofDate_Date
--	,dtStartDate
--FROM $(ExcelsiorDB)..TBL_EIS_DT_STG_Detect_PostDataLoad POST
--INNER JOIN $(ExcelsiorDB)..DEFERREDGIFTACCOUNT DGA
--	ON POST.AccountID = DGA.AccountID
-- TBL_BR_ReportLabelSectionBlock script
SET IDENTITY_INSERT dbo.TBL_BR_ReportLabelSectionBlock ON

INSERT INTO TBL_BR_ReportLabelSectionBlock (
	ReportLabelSectionBlockID
	,ReportLabelID
	,SectionBlockID
	,CreatedDate
	,CREATEDUSERID
	)
SELECT ReportLabelSectionBlockID
	,ReportLabelID
	,SectionBlockId
	,CreatedDate
	,CreatedUserID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_BR_ReportLabel_SectionBlock

SET IDENTITY_INSERT dbo.TBL_BR_ReportLabelSectionBlock OFF
-- TBL_BR_ReportLabelSection script
SET IDENTITY_INSERT dbo.TBL_BR_ReportLabelSection ON

INSERT INTO TBL_BR_ReportLabelSection (
	ReportLabelSectionID
	,ReportLabelID
	,SectionID
	,CreatedDate
	,CREATEDUSERID
	)
SELECT ReportLabelSectionID
	,ReportLabelID
	,SectionId
	,CreatedDate
	,CreatedUserID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_BR_ReportLabel_Section

SET IDENTITY_INSERT dbo.TBL_BR_ReportLabelSection OFF

-- TBL_BR_ManagerCodeInvestmentObjectiveMapping script
INSERT INTO TBL_BR_ManagerCodeInvestmentObjectiveMapping (
	ManagerCode
	,ObjectiveCode
	,VersionNo
	)
SELECT ClientBriefName
	,ObjectiveCode
	,VersionNo
FROM $(ExcelsiorDB)..TBL_EIS_RPT_BR_ClientInvestmentObjectiveMapping

-- TBL_BR_AssetClassPieChart script
SET IDENTITY_INSERT dbo.TBL_BR_AssetClassPieChart ON

INSERT INTO TBL_BR_AssetClassPieChart (
	AssetClassPieChartID
	,AssetClass
	,Type
	,ChartDescription
	,CreatedDate
	,CREATEDUSERID
	)
SELECT AssetClassPieChartID
	,AssetClass
	,Type
	,ChartDescription
	,CreatedDate
	,CreatedUserID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_BR_AssetClassPieChart

SET IDENTITY_INSERT dbo.TBL_BR_AssetClassPieChart OFF
-- TBL_BR_AssetSecurityMapping script
SET IDENTITY_INSERT dbo.TBL_BR_AssetSecurityMapping ON

INSERT INTO TBL_BR_AssetSecurityMapping (
	AssetSecurityMappingID
	,SecurityTypeID
	,Symbol
	,CustomAssetClassDescription
	,CustomSecurityTypeDescription
	,CreatedDate
	,CreatedBy
	)
SELECT AssetSecurityMappingID
	,SecurityTypeID
	,Symbol
	,CustomAssetClassDescription
	,CustomSecurityTypeDescription
	,CreatedDate
	,CreatedUserID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_BR_AssetSecurityMapping

SET IDENTITY_INSERT dbo.TBL_BR_AssetSecurityMapping OFF
---- TBL_BR_IndiceManagementVersionMasterObjective script(dup below)
--SET IDENTITY_INSERT dbo.TBL_BR_IndiceManagementVersionMasterObjective ON
--INSERT INTO TBL_BR_IndiceManagementVersionMasterObjective (
--	IndexVersionMgmtId
--	,VersionNo
--	,ObjectiveCode
--	)
--SELECT IndexVersionMgmtId
--	,VersionNo
--	,ObjectiveCode
--FROM $(ExcelsiorDB)..TBL_EIS_RPT_BR_INDICES_MGMT_VERSION_MASTEROBJECTIVE
SET IDENTITY_INSERT dbo.TBL_BR_IndiceManagementVersionMasterObjective OFF

-- TBL_BR_ETLParameter script
INSERT INTO TBL_BR_ETLParameter (
	CurrentDate
	,QueueID
	,ReportAsOfDate
	,ReportPeriodStartDate
	,DeliverableFrequency
	,ReportTemplate
	,ReportYear
	,UserID
	,UserName
	)
SELECT CurrentDate
	,QueueID
	,ReportAsofDate
	,ReportPeriodStartDate
	,DeliverableFrequency
	,ReportTemplate
	,ReportYear
	,UserID
	,UserName
FROM $(ExcelsiorDB)..TBL_EIS_RPT_BR_ETL_Parameters

-- TBL_BR_IndesignErrorCode script
INSERT INTO TBL_BR_IndesignErrorCode (
	Error_CodeID
	,Error_Type
	,Error_Description
	)
SELECT Error_CodeID
	,Error_Type
	,Error_Description
FROM $(ExcelsiorDB)..TBL_EIS_RPT_BR_INDESIGN_ERROR_CODE

-- TBL_BR_TrustType script
SET IDENTITY_INSERT dbo.TBL_BR_TrustType ON

INSERT INTO TBL_BR_TrustType (
	TrustTypeID
	,AccountType
	,FilpProvision1
	,FilpProvision0
	,WithdrawalLabel
	,CreatedDate
	,CREATEDUSERID
	,VerifiedByBusiness
	)
SELECT TrustTypeID
	,AccountType
	,FilpProvision1
	,FilpProvision0
	,WithdrawalLabel
	,CreatedDate
	,CreatedUserID
	,VerifiedbyBusiness
FROM $(ExcelsiorDB)..TBL_EIS_RPT_BR_TrustType

SET IDENTITY_INSERT dbo.TBL_BR_TrustType OFF
-- TBL_BR_TransMapping script
SET IDENTITY_INSERT dbo.TBL_BR_TransMapping ON

INSERT INTO TBL_BR_TransMapping (
	CategoryID
	,CategoryCode
	,CategoryDescription
	,TransactionCode
	,SecurityType
	,SecuritySymbol
	,SourceDestType
	,SourceDestAccount
	,TaxYear
	,UserDef2
	,CreatedDate
	,CREATEDUSERID
	)
SELECT CategoryID
	,CategoryCode
	,CategoryDescription
	,TransactionCode
	,SecurityType
	,SecuritySymbol
	,SourceDestType
	,SourceDestAccount
	,TaxYear
	,UserDef2
	,CreatedDate
	,CreatedUserID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_BR_Transmapping

SET IDENTITY_INSERT dbo.TBL_BR_TransMapping OFF
-- TBL_DLV_STG_WebPublishedItem script
SET IDENTITY_INSERT dbo.TBL_DLV_STG_WebPublishedItem ON

INSERT INTO TBL_DLV_STG_WebPublishedItem (
	UID
	,DeliverableItemID
	,ManagerCode
	,CustomerAccountNumber
	,ContactID
	,FileName
	,DeliverableID
	,DeliverableName
	,UploadEmailTemplateID
	,ReplaceEmailTemplateID
	,EmailRecipientsIDs
	,WebReportID
	,UploadStatus
	)
SELECT UID
	,DeliverableItemID
	,CLNT.BriefName
	,DGA.AdventID
	,PartLkup.CONTACTID AS CONTACTID
	,FileName
	,DeliverableID
	,DeliverableName
	,UploadEmailTemplateID
	,ReplaceEmailTemplateID
	,EmailRecipientsIDs
	,WebReportID
	,UploadStatus
FROM $(ExcelsiorDB)..DLV_STG_WebPublished_Items WEBITEM
INNER JOIN $(ExcelsiorDB)..CLIENT CLNT
	ON WEBITEM.ClientID = CLNT.ClientID
INNER JOIN $(ExcelsiorDB)..DEFERREDGIFTACCOUNT DGA
	ON WEBITEM.AccountID = DGA.AccountID
INNER JOIN $(MappingDB)..TBL_ParticipantContactLookUp PartLkup
	ON PartLkup.ParticipantID = WEBITEM.ParticipantID

SET IDENTITY_INSERT dbo.TBL_DLV_STG_WebPublishedItem OFF
-- TBL_DLV_ManagerCodeDeliverableEmployeeAudit script
SET IDENTITY_INSERT dbo.TBL_DLV_ManagerCodeDeliverableEmployeeAudit ON

INSERT INTO TBL_DLV_ManagerCodeDeliverableEmployeeAudit (
	AUDIT_SEQUENCE
	,AUDIT_USER_ID
	,AUDIT_DATETIME
	,AUDIT_FLAG
	,AUDIT_TABLE
	,AUDIT_DETAILS
	,ManagerCodeDeliverableID
	,EmployeeID
	,EmailListRecipientID
	)
SELECT AUDIT_SEQUENCE
	,AUDIT_USER_ID
	,AUDIT_DATETIME
	,AUDIT_FLAG
	,AUDIT_TABLE
	,AUDIT_DETAILS
	,ClientDeliverablesID
	,EmployeeID
	,EmailListRecipientID
FROM $(ExcelsiorDB)..DLV_ClientDeliverablesEmployeeAudit

SET IDENTITY_INSERT dbo.TBL_DLV_ManagerCodeDeliverableEmployeeAudit OFF
-- TBL_DLV_AutoCompleteExtenderList script
SET IDENTITY_INSERT dbo.TBL_DLV_AutoCompleteExtenderList ON

INSERT INTO TBL_DLV_AutoCompleteExtenderList (
	AutoCompleteExtenderID
	,AutoCompleteExtenderName
	,AutoCompleteExtenderType
	,Description
	,DisplaySequence
	)
SELECT AutoCompleteExtenderID
	,AutoCompleteExtenderName
	,AutoCompleteExtenderType
	,Description
	,DisplaySequence
FROM $(ExcelsiorDB)..DLV_AutoCompleteExtenderList

SET IDENTITY_INSERT dbo.TBL_DLV_AutoCompleteExtenderList OFF

SET NOCOUNT ON
-- TBL_BR_AmountToDistribute migration script
SET IDENTITY_INSERT TBL_BR_AmountToDistribute ON

INSERT INTO TBL_BR_AmountToDistribute (
	AmountToDistributeID
	,CustomerAccountNumber
	,ReportDate
	,AmountToDistributeValue
	,CreatedDate
	,CreatedUserID
	)
SELECT AmountToDistributeID
	,DGA.AdventID
	,ReportDate
	,AmountToDistributeValue
	,CreatedDate
	,CreatedUserID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_AmountToDistribute ATD
INNER JOIN $(ExcelsiorDB)..DEFERREDGIFTACCOUNT DGA
	ON DGA.AccountID = ATD.AccountID

SET IDENTITY_INSERT TBL_BR_AmountToDistribute OFF

-- TBL_BR_CompanyLogo migration script
INSERT INTO TBL_BR_CompanyLogo (
	Service_Offering_ID
	,CompanyLogo
	,CreatedDate
	,CreatedUserID
	)
SELECT Service_Offering_ID
	,CompanyLogo
	,CreatedDate
	,CreatedUserID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_CompanyLogo

-- TBL_BR_DAFDistribution
SET IDENTITY_INSERT TBL_BR_DAFDistribution ON

INSERT INTO TBL_BR_DAFDistribution (
	DAFDistributionID
	,CustomerAccountNumber
	,ReportDate
	,DistributionDate
	,Recipient
	,DistributionAmount
	,RecipientSubTotal
	,SubTotalDisplaySeq
	,FootNote
	,CREATEDDATE
	,CREATEDUSERID
	)
SELECT DAFDistributionID
	,DGA.AdventID
	,ReportDate
	,DistributionDate
	,Recipient
	,DistributionAmount
	,RecipientSubTotal
	,SubTotalDisplaySeq
	,FootNote
	,CREATEDDATE
	,CREATEDUSERID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_DAFDistributions DAFD
INNER JOIN $(ExcelsiorDB)..DEFERREDGIFTACCOUNT DGA
	ON DGA.AccountID = DAFD.AccountID

SET IDENTITY_INSERT TBL_BR_DAFDistribution OFF

-- TBL_BR_PIFHistory
INSERT INTO TBL_BR_PIFHistory (
	CustomerAccountNumber
	,Year
	,INCOME_UNIT
	,MARKETVALUE_UNIT
	,MODIFIED_DATE
	,MODIFIED_USER_ID
	,CREATED_DATE
	,CREATED_USER_ID
	,DELETED_USER_ID
	)
SELECT DGA.AdventID
	,Year
	,INCOME_UNIT
	,MARKETVALUE_UNIT
	,MODIFIED_DATE
	,MODIFIED_USER_ID
	,CREATED_DATE
	,CREATED_USER_ID
	,DELETED_USER_ID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_PIF_HISTORY PIFH
INNER JOIN $(ExcelsiorDB)..DEFERREDGIFTACCOUNT DGA
	ON DGA.AccountID = PIFH.AccountID

-- TBL_BR_PostContributionGainLoss
SET IDENTITY_INSERT TBL_BR_PostContributionGainLoss ON

INSERT INTO TBL_BR_PostContributionGainLoss (
	AnnuityContractID
	,CustomerAccountNumber
	,ReportDate
	,AnnuityName
	,YearEndMarketValue
	,DeferredAnnuityContractInvestment
	,CREATEDDATE
	,CREATEDUSERID
	)
SELECT AnnuityContractID
	,DGA.AdventID
	,ReportDate
	,AnnuityName
	,YearEndMarketValue
	,DeferredAnnuityContractInvestment
	,CREATEDDATE
	,CREATEDUSERID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_PostContribution_GainLoss PCG
INNER JOIN $(ExcelsiorDB)..DEFERREDGIFTACCOUNT DGA
	ON DGA.AccountID = PCG.AccountID

SET IDENTITY_INSERT TBL_BR_PostContributionGainLoss OFF
-- TBL_BR_ReportCondition
SET IDENTITY_INSERT TBL_BR_ReportCondition ON

INSERT INTO TBL_BR_ReportCondition (
	Condition_ID
	,Entity_Type_ID
	,Report_Type_ID
	,ManagerCode
	,CustomerAccountNumber
	,AllianceNumber
	,ContactID
	,ContactRoleCode
	,AccountType
	,Comments
	,Status_ID
	,MODIFIED_DATE
	,MODIFIED_USER_ID
	,CREATED_DATE
	,CREATED_USER_ID
	,DELETED_USER_ID
	,FREQUENCYID
	)
SELECT DISTINCT Condition_ID
	,Entity_Type_ID
	,Report_Type_ID
	,Clnt.AccountManagerCode
	,ACC.CustomerAccountNumber
	,Pgm.AllianceNumber
	,Bene.ContactID 
	,Bene.RoleCode 
	,AccMstr.AccountTypeCode
	,RPTC.Comments
	,Status_ID
	,MODIFIED_DATE
	,MODIFIED_USER_ID
	,CREATED_DATE
	,CREATED_USER_ID
	,DELETED_USER_ID
	,FREQUENCYID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_REPORT_CONDITIONS RPTC
LEFT OUTER JOIN $(MappingDB)..TBL_AccountLookup ACC
	ON RPTC.AccountID = ACC.AccountID
LEFT OUTER JOIN $(MappingDB)..TBL_ProgramLookup Pgm
	on RPTC.ProgramID = Pgm.PROGRAMID	
LEFT OUTER JOIN $(MappingDB)..TBL_ClientLookup Clnt
	on RPTC.ClientId = Clnt.CLIENTID
LEFT OUTER JOIN $(MappingDB)..TBL_BeneficiaryLookup Bene
	on RPTC.BeneficiaryID  = Bene.BeneficiaryID 
LEFT OUTER JOIN $(InnoTrustDB)..AccountMaster AccMstr
	on ACC.CustomerAccountNumber = AccMstr.CustomerAccountNumber

SET IDENTITY_INSERT TBL_BR_ReportCondition OFF

-- TBL_BR_UnitrustDeficitAccountInfo
INSERT INTO TBL_BR_UnitrustDeficitAccountInfo (
	CustomerAccountNumber
	,ReportDate
	,StartingDeficitAccountBalance
	,UnitrustAmount
	,Distributions
	,CREATEDDATE
	,CREATEDUSERID
	)
SELECT DGA.AdventID
	,ReportDate
	,StartingDeficitAccountBalance
	,UnitrustAmount
	,Distributions
	,CREATEDDATE
	,CREATEDUSERID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_UnitrustDeficitAccountInfo UDAI
INNER JOIN $(ExcelsiorDB)..DEFERREDGIFTACCOUNT DGA
	ON DGA.AccountID = UDAI.AccountID

-- TBL_BR_ReportColorSchemeTag
SET IDENTITY_INSERT TBL_BR_ReportColorSchemeTag ON

INSERT INTO TBL_BR_ReportColorSchemeTag (
	ColorSchemeTagID
	,ColorSchemeTag
	,ColorSchemeDetails
	,MODIFIED_DATE
	,MODIFIED_USER_ID
	,CREATED_DATE
	,CREATED_USER_ID
	,DELETED_USER_ID
	)
SELECT ColorSchemeTagID
	,ColorSchemeTag
	,ColorSchemeDetails
	,MODIFIED_DATE
	,MODIFIED_USER_ID
	,CREATED_DATE
	,CREATED_USER_ID
	,DELETED_USER_ID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_REPORT_COLOR_SCHEME_TAGS

SET IDENTITY_INSERT TBL_BR_ReportColorSchemeTag OFF
-- TBL_BR_ReportColorScheme
SET IDENTITY_INSERT TBL_BR_ReportColorScheme ON

INSERT INTO TBL_BR_ReportColorScheme (
	ColorSchemeID
	,ColorSchemeBriefName
	,ColorSchemeDescription
	,MODIFIED_DATE
	,MODIFIED_USER_ID
	,CREATED_DATE
	,CREATED_USER_ID
	,DELETED_USER_ID
	)
SELECT ColorSchemeID
	,ColorSchemeBriefName
	,ColorSchemeDescription
	,MODIFIED_DATE
	,MODIFIED_USER_ID
	,CREATED_DATE
	,CREATED_USER_ID
	,DELETED_USER_ID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_REPORT_COLOR_SCHEME

SET IDENTITY_INSERT TBL_BR_ReportColorScheme OFF
-- TBL_BR_ReportColorSchemeDetail
SET IDENTITY_INSERT TBL_BR_ReportColorSchemeDetail ON

INSERT INTO TBL_BR_ReportColorSchemeDetail (
	ColorSchemeDetailsID
	,ColorSchemeID
	,ColorSequenceNo
	,ColorSchemeTagID
	,ColorSchemeDetailsName
	,ColorSchemeDetailsValue
	,ColorSchemeDetailsRGBValue
	,ColorSchemeDetails
	,MODIFIED_DATE
	,MODIFIED_USER_ID
	,CREATED_DATE
	,CREATED_USER_ID
	,DELETED_USER_ID
	)
SELECT ColorSchemeDetailsID
	,ColorSchemeID
	,ColorSequenceNo
	,ColorSchemeTagID
	,ColorSchemeDetailsName
	,ColorSchemeDetailsValue
	,ColorSchemeDetailsRGBValue
	,ColorSchemeDetails
	,MODIFIED_DATE
	,MODIFIED_USER_ID
	,CREATED_DATE
	,CREATED_USER_ID
	,DELETED_USER_ID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_REPORT_COLOR_SCHEME_DETAILS

SET IDENTITY_INSERT TBL_BR_ReportColorSchemeDetail OFF
SET NOCOUNT ON
-- TBL_BR_ReportIndex
SET IDENTITY_INSERT TBL_BR_ReportIndex ON

INSERT INTO TBL_BR_ReportIndex (
	IndexID
	,IndexSequenceNo
	,IndexBriefName
	,IndexFullName
	,MODIFIED_DATE
	,MODIFIED_USER_ID
	,CREATED_DATE
	,CREATED_USER_ID
	,DELETED_USER_ID
	)
SELECT IndexID
	,IndexSequenceNo
	,IndexBriefName
	,IndexFullName
	,MODIFIED_DATE
	,MODIFIED_USER_ID
	,CREATED_DATE
	,CREATED_USER_ID
	,DELETED_USER_ID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_REPORT_INDEX

SET IDENTITY_INSERT TBL_BR_ReportIndex OFF
-- TBL_BR_ReportIndiceManagementVersion
SET IDENTITY_INSERT TBL_BR_ReportIndiceManagementVersion ON

INSERT INTO TBL_BR_ReportIndiceManagementVersion (
	ReportIndexMgmtVersionID
	,ManagerCode
	,VersionNo
	,MultiAssetBM
	,MODIFIED_DATE
	,MODIFIED_USER_ID
	,CREATED_DATE
	,CREATED_USER_ID
	,DELETED_USER_ID
	)
SELECT ReportIndexMgmtVersionID
	,CLNT.BriefName
	,VersionNo
	,MultiAssetBM
	,MODIFIED_DATE
	,MODIFIED_USER_ID
	,CREATED_DATE
	,CREATED_USER_ID
	,DELETED_USER_ID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_REPORT_INDICES_MANAGEMENT_VERSION IMV
INNER JOIN $(ExcelsiorDB)..CLIENT CLNT
	ON IMV.ClientID = CLNT.ClientID

SET IDENTITY_INSERT TBL_BR_ReportIndiceManagementVersion OFF



		
-- TBL_BR_ProfileTax
SET IDENTITY_INSERT TBL_IRS_ProfileTax ON 
INSERT INTO TBL_IRS_ProfileTax (
	TAX_ID
	,ENTITY_TYPE_ID
	,ManagerCode
	,AllianceNumber
	,CustomerAccountNumber
	,BeneContactID
	,ContactRoleCode
	,EmployeeContactID
	,BENEFICIARY_TAX_FORM_ID
	,TAX_RETURN_ID
	,TAX_10989_ID
	,TAX_1099R_ID
	,HIGH_PRIORITY
	,K_1_PRIORITY
	,MODIFIED_DATE
	,MODIFIED_USER_ID
	,CREATED_DATE
	,CREATED_USER_ID
	,DELETED_USER_ID
	,CT_12S_ID
	,OR_REGISTRATION_NO
	,FASCIMILE_SIGNER_FLAG
	)
SELECT TAX_ID
	,ENTITY_TYPE_ID
	,Clnt.AccountManagerCode
	,Pgm.AllianceNumber
	,Acc.CustomerAccountNumber
	,Bene.ContactID
	,Bene.RoleCode
	,ClntEmp.SubContactId
	,GlbllContBenTaxFrm.ContactID
	,GlbllContTaxRtrn.ContactID 
	,GlbllContTax10989.ContactID 
	,GlbllContTax1099R.ContactID
	,Tax.HIGH_PRIORITY
	,Tax.K_1_PRIORITY
	,Tax.MODIFIED_DATE
	,Tax.MODIFIED_USER_ID
	,Tax.CREATED_DATE
	,Tax.CREATED_USER_ID
	,Tax.DELETED_USER_ID
	,Tax.CT_12S_ID
	,Tax.OR_REGISTRATION_NO
	,Tax.FASCIMILE_SIGNER_FLAG
FROM $(ExcelsiorDB)..TBL_EIS_EX_PROFILE_TAX Tax
LEFT OUTER JOIN $(MappingDB)..TBL_ClientLookup Clnt
	ON Clnt.ClientID = Tax.CLIENTID
LEFT OUTER JOIN $(MappingDB)..TBL_ProgramLookup Pgm
	ON Pgm.ProgramID = Tax.PROGRAMID
LEFT OUTER JOIN $(MappingDB)..TBL_AccountLookup Acc
	ON Acc.AccountID = Tax.ACCOUNTID
LEFT OUTER JOIN $(MappingDB)..TBL_BeneficiaryLookup Bene
	ON Bene.BeneficiaryID = Tax.BENEFICIARYID
LEFT OUTER JOIN $(MappingDB)..TBL_ClientEmployeeLookup ClntEmp
	ON ClntEmp.EmployeeID = Tax.FASCIMILE_SIGNER_EMPLOYEEID
LEFT OUTER JOIN #Tmp_GlobalContacts GlbllContBenTaxFrm
	ON Tax.BENEFICIARY_TAX_FORM_ID = GlbllContBenTaxFrm.PartyID
LEFT OUTER JOIN #Tmp_GlobalContacts GlbllContTaxRtrn
	ON Tax.TAX_RETURN_ID = GlbllContTaxRtrn.PartyID
LEFT OUTER JOIN #Tmp_GlobalContacts GlbllContTax10989
	ON Tax.TAX_10989_ID = GlbllContTax10989.PartyID
LEFT OUTER JOIN #Tmp_GlobalContacts GlbllContTax1099R
	ON Tax.TAX_1099R_ID = GlbllContTax1099R.PartyID
SET IDENTITY_INSERT TBL_IRS_ProfileTax OFF

UPDATE TBL_IRS_PROFILETAX
SET ENTITY_TYPE_ID = 1021
WHERE ENTITY_TYPE_ID = 5

drop TABLE #Tmp_GlobalContacts

-- TBL_BR_ProfileTaxCondition
SET IDENTITY_INSERT TBL_IRS_ProfileTaxCondition ON
INSERT INTO TBL_IRS_ProfileTaxCondition (
	TAX_ID
	,CONDITION_ID
	,STATUS_ID
	,START_YEAR
	,END_YEAR
	,MODIFIED_DATE
	,MODIFIED_USER_ID
	,CREATED_DATE
	,CREATED_USER_ID
	,DELETED_USER_ID
	,COMMENTS
	,TAX_CONDITION_ID
	)
SELECT TAX_ID
	,CONDITION_ID
	,STATUS_ID
	,START_YEAR
	,END_YEAR
	,MODIFIED_DATE
	,MODIFIED_USER_ID
	,CREATED_DATE
	,CREATED_USER_ID
	,DELETED_USER_ID
	,COMMENTS
	,TAX_CONDITION_ID
FROM $(ExcelsiorDB)..TBL_EIS_EX_PROFILE_TAX_CONDITIONS
SET IDENTITY_INSERT TBL_IRS_ProfileTaxCondition OFF



	-- TBL_IRS_Audit_ProfileTax
	SET IDENTITY_INSERT TBL_IRS_Audit_ProfileTax ON
	INSERT INTO TBL_IRS_Audit_ProfileTax (
		AUDIT_SEQUENCE
		,AUDIT_USER_ID
		,AUDIT_DATETIME
		,AUDIT_FLAG
		,AUDIT_TABLE
		,AUDIT_DETAILS
		,TAX_ID
		,Entity_Type_ID
		,ManagerCode
		,AllianceNumber
		,CustomerAccountNumber
		,BeneContactID
		,ContactRoleCode
		,EmployeeContactID
		,BENEFICIARY_TAX_FORM_ID
		,TAX_RETURN_ID
		,TAX_10989_ID
		,TAX_1099R_ID
		,HIGH_PRIORITY
		,K_1_PRIORITY
		,MODIFIED_DATE
		,MODIFIED_USER_ID
		,CREATED_DATE
		,CREATED_USER_ID
		,DELETED_USER_ID
		,CT_12S_ID
		,OR_REGISTRATION_NO
		,FASCIMILE_SIGNER_FLAG
		)
	SELECT AUDIT_SEQUENCE
		,AUDIT_USER_ID
		,AUDIT_DATETIME
		,AUDIT_FLAG
		,AUDIT_TABLE
		,AUDIT_DETAILS
		,TAX_ID
		,ENTITY_TYPE_ID
		,Clnt.AccountManagerCode
		,Pgm.AllianceNumber
		,Acc.CustomerAccountNumber
		,Bene.ContactID
		,Bene.RoleCode
		,ClntEmp.SubContactId
		,Tax.BENEFICIARY_TAX_FORM_ID
		,Tax.TAX_RETURN_ID
		,Tax.TAX_10989_ID
		,Tax.TAX_1099R_ID
		,Tax.HIGH_PRIORITY
		,Tax.K_1_PRIORITY
		,Tax.MODIFIED_DATE
		,Tax.MODIFIED_USER_ID
		,Tax.CREATED_DATE
		,Tax.CREATED_USER_ID
		,Tax.DELETED_USER_ID
		,Tax.CT_12S_ID
		,Tax.OR_REGISTRATION_NO
		,Tax.FASCIMILE_SIGNER_FLAG
	FROM $(ExcelsiorDB)..TBL_EIS_EX_AUDIT_PROFILE_TAX Tax
	LEFT OUTER JOIN $(MappingDB)..TBL_ClientLookup Clnt
		ON Clnt.ClientID = Tax.CLIENTID
	LEFT OUTER JOIN $(MappingDB)..TBL_ProgramLookup Pgm
		ON Pgm.ProgramID = Tax.PROGRAMID
	LEFT OUTER JOIN $(MappingDB)..TBL_AccountLookup Acc
		ON Acc.AccountID = Tax.ACCOUNTID
	LEFT OUTER JOIN $(MappingDB)..TBL_BeneficiaryLookup Bene
		ON Bene.BeneficiaryID = Tax.BENEFICIARYID
	LEFT OUTER JOIN $(MappingDB)..TBL_ClientEmployeeLookup ClntEmp
		ON ClntEmp.EmployeeID = Tax.FASCIMILE_SIGNER_EMPLOYEEID
	SET IDENTITY_INSERT TBL_IRS_Audit_ProfileTax OFF

--TBL_IRS_Audit_ProfileTaxCondition
SET IDENTITY_INSERT TBL_IRS_Audit_ProfileTaxCondition ON
INSERT INTO TBL_IRS_Audit_ProfileTaxCondition (
	AUDIT_SEQUENCE
	,AUDIT_USER_ID
	,AUDIT_DATETIME
	,AUDIT_FLAG
	,AUDIT_TABLE
	,AUDIT_DETAILS
	,TAX_ID
	,CONDITION_ID
	,STATUS_ID
	,START_YEAR
	,END_YEAR
	,MODIFIED_DATE
	,MODIFIED_USER_ID
	,CREATED_DATE
	,CREATED_USER_ID
	,DELETED_USER_ID
	,COMMENTS
	,TAX_CONDITION_ID
	)
SELECT AUDIT_SEQUENCE
	,AUDIT_USER_ID
	,AUDIT_DATETIME
	,AUDIT_FLAG
	,AUDIT_TABLE	
	,AUDIT_DETAILS
	,TAX_ID
	,CONDITION_ID
	,STATUS_ID
	,START_YEAR
	,END_YEAR
	,MODIFIED_DATE
	,MODIFIED_USER_ID
	,CREATED_DATE
	,CREATED_USER_ID
	,DELETED_USER_ID
	,COMMENTS
	,TAX_CONDITION_ID
FROM $(ExcelsiorDB)..TBL_EIS_EX_AUDIT_PROFILE_TAX_CONDITIONS
SET IDENTITY_INSERT TBL_IRS_Audit_ProfileTaxCondition OFF


-- TBL_BR_ErrorLog
SET IDENTITY_INSERT TBL_BR_ErrorLog ON

INSERT INTO TBL_BR_ErrorLog (
	ERRLOG_ID
	,Err_Module_Name
	,Err_Date_Time
	,Err_Number
	,Err_Source
	,Err_Description
	,Err_Login_Name
	,Err_PhysicalMachineName
	,Err_CurrentObject_MethodCall
	,Err_DLL_Version_No
	,Err_Application_Name
	,Err_Stacktrace_Info
	,Err_Severity
	,Err_Line_No
	)
SELECT ERRLOG_ID
	,Err_Module_Name
	,Err_Date_Time
	,Err_Number
	,Err_Source
	,Err_Description
	,Err_Login_Name
	,Err_PhysicalMachineName
	,Err_CurrentObject_MethodCall
	,Err_DLL_Version_No
	,Err_Application_Name
	,Err_Stacktrace_Info
	,Err_Severity
	,Err_Line_No
FROM $(ExcelsiorDB)..TBL_EIS_ERRORLOG

SET IDENTITY_INSERT TBL_BR_ErrorLog OFF

-- TBL_BR_ErrorMessage
INSERT INTO TBL_BR_ErrorMessage (
	ERROR_NUMBER
	,DESCRIPTION
	,ERROR_TYPE
	,SEVERITY
	,CATEGORY
	)
SELECT ERROR_NUMBER
	,DESCRIPTION
	,ERROR_TYPE
	,SEVERITY
	,CATEGORY
FROM $(ExcelsiorDB)..TBL_EIS_ERRORMESSAGES

-- TBL_BR_CommentLinkage
INSERT INTO TBL_BR_CommentLinkage (
	CustomerAccountNumber
	,ComLinkTypeID
	,CommentID
	)
SELECT DGA.AdventID
	,ComLinkTypeID
	,CommentID
FROM $(ExcelsiorDB)..CommentLinkage CL
LEFT OUTER JOIN $(ExcelsiorDB)..DEFERREDGIFTACCOUNT DGA
	ON CL.AccountID = DGA.AccountID
WHERE DGA.AdventID IS NOT NULL

-- TBL_BR_CommentLinkType
INSERT INTO TBL_BR_CommentLinkType (
	ComLinkTypeID
	,ComDescription
	)
SELECT ComLinkTypeID
	,ComDescription
FROM $(ExcelsiorDB)..CommentLinkType

-- TBL_BR_AlertEvent
INSERT INTO TBL_BR_AlertEvent (
	AlertID
	,AlertStatus
	,AlertCause
	,CustomerAccountNumber
	,AlertDetail
	,AlertComment
	,DefinitionDate
	,AlertDate
	,CompletionDate
	,AssignedTo
	,DefinedBy
	,CompletedBy
	,CashNeeds
	,AlertLinkTable
	,AlertLinkID
	,ALERTCATEGORY_ID
	,MODIFIED_DATE
	,MODIFIED_USER_ID
	,CREATED_DATE
	,CREATED_USER_ID
	,DELETED_USER_ID
	)
SELECT AE.AlertID
	,AlertStatus
	,AlertCause
	,DGA.AdventID
	,AlertDetail
	,AlertComment
	,DefinitionDate
	,AlertDate
	,CompletionDate
	,AssignedTo
	,DefinedBy
	,CompletedBy
	,CashNeeds
	,AlertLinkTable
	,AlertLinkID
	,AES.ALERTCATEGORY_ID
	,AES.MODIFIED_DATE
	,AES.MODIFIED_USER_ID
	,AES.CREATED_DATE
	,AES.CREATED_USER_ID
	,AES.DELETED_USER_ID
FROM $(ExcelsiorDB)..AlertEvent AE
INNER JOIN $(ExcelsiorDB)..TBL_EIS_EX_ALERTEVENT_SUPPLEMENT AES
	ON AE.AlertID = AES.AlertID
LEFT OUTER JOIN $(ExcelsiorDB)..DEFERREDGIFTACCOUNT DGA
	ON AE.AccountID = DGA.AccountID
WHERE DGA.AdventID IS NOT NULL

-- TBL_BR_IndiceManagementVersionMasterObjective Migration Script
SET IDENTITY_INSERT dbo.TBL_BR_IndiceManagementVersionMasterObjective ON

INSERT INTO dbo.TBL_BR_IndiceManagementVersionMasterObjective (
	IndexVersionMgmtId
	,VersionNo
	,ObjectiveCode
	)
SELECT IndexVersionMgmtId
	,VersionNo
	,ObjectiveCode
FROM $(ExcelsiorDB)..TBL_EIS_RPT_BR_Indices_MGMT_Version_MasterObjective

SET IDENTITY_INSERT dbo.TBL_BR_IndiceManagementVersionMasterObjective OFF

--TBL_BR_DAFAccountInformation Migration Script
INSERT INTO dbo.TBL_BR_DAFAccountInformation (
	CustomerAccountNumber
	,ReportDate
	,OutstandingDistribution
	,CharityName
	,MinAnnualDistributionPerc
	,MinCharityShareofAnnualPerc
	,MinimumDistribution
	,MinimumtoCharity
	,MinimumOutstandingDistribution
	,MinimumOutstandingDistributiontoCharity
	,CREATEDDATE
	,CREATEDUSERID
	)
SELECT DGA.AdventID
	,ReportDate
	,OutstandingDistribution
	,CharityName
	,MinAnnualDistributionPerc
	,MinCharityShareofAnnualPerc
	,MinimumDistribution
	,MinimumtoCharity
	,MinimumOutstandingDistribution
	,MinimumOutstandingDistributiontoCharity
	,CREATEDDATE
	,CREATEDUSERID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_DAFAccountInfo DAI
INNER JOIN $(ExcelsiorDB)..DEFERREDGIFTACCOUNT DGA
	ON DAI.AccountID = DGA.AccountID

-- TBL_EIS_RPT_FMV_History Migration Script
SET IDENTITY_INSERT dbo.TBL_BR_FMVHistory ON

INSERT INTO dbo.TBL_BR_FMVHistory (
	FMVHistoryID
	,CustomerAccountNumber
	,ReportFrequency
	,ReportDate
	,FMV
	,ReportRunDate
	,CreatedDate
	,CreatedUserID
	,LASTUPDATEDDT
	)
SELECT FMVHistoryID
	,DGA.AdventID
	,ReportFrequency
	,ReportDate
	,FMV
	,ReportRunDate
	,CreatedDate
	,CreatedUserID
	,LASTUPDATEDDT
FROM $(ExcelsiorDB)..TBL_EIS_RPT_FMV_History FMVH
INNER JOIN $(ExcelsiorDB)..DEFERREDGIFTACCOUNT DGA
	ON FMVH.AccountID = DGA.AccountID

SET IDENTITY_INSERT dbo.TBL_BR_FMVHistory OFF
-- TBL_EIS_RPT_PIF_PROJECTION Migration Script
SET IDENTITY_INSERT dbo.TBL_BR_PIFProjection ON

INSERT INTO dbo.TBL_BR_PIFProjection (
	ProjectionID
	,CustomerAccountNumber
	,Year
	,ExpectedIncome
	,MODIFIED_DATE
	,MODIFIED_USER_ID
	,CREATED_DATE
	,CREATED_USER_ID
	,DELETED_USER_ID
	)
SELECT ProjectionID
	,DGA.AdventID
	,Year
	,ExpectedIncome
	,MODIFIED_DATE
	,MODIFIED_USER_ID
	,CREATED_DATE
	,CREATED_USER_ID
	,DELETED_USER_ID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_PIF_PROJECTION PPRoj
INNER JOIN $(ExcelsiorDB)..DEFERREDGIFTACCOUNT DGA
	ON PPRoj.AccountID = DGA.AccountID

SET IDENTITY_INSERT dbo.TBL_BR_PIFProjection OFF

-- TBL_BR_ReportManagerMarketPaymentData Migration Script
INSERT INTO dbo.TBL_BR_ReportManagerMarketPaymentData (
	CustomerAccountNumber
	,TaxYear
	,FMV
	,Payment
	,MODIFIED_DATE
	,MODIFIED_USER_ID
	,CREATED_DATE
	,CREATED_USER_ID
	,DELETED_USER_ID
	)
SELECT DGA.AdventID
	,TaxYear
	,FMV
	,Payment
	,MODIFIED_DATE
	,MODIFIED_USER_ID
	,CREATED_DATE
	,CREATED_USER_ID
	,DELETED_USER_ID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_REPORT_CLIENT_MKT_PMT_DATA CPD
INNER JOIN $(ExcelsiorDB)..DEFERREDGIFTACCOUNT DGA
	ON CPD.AccountID = DGA.AccountID

-- TBL_BR_ReportIndiceManagement Migration Script
SET IDENTITY_INSERT dbo.TBL_BR_ReportIndiceManagement ON

INSERT INTO dbo.TBL_BR_ReportIndiceManagement (
	ReportIndexMgmtID
	,ReportIndexMgmtVersionID
	,IndexNo
	,IndexID
	,MODIFIED_DATE
	,MODIFIED_USER_ID
	,CREATED_DATE
	,CREATED_USER_ID
	,DELETED_USER_ID
	)
SELECT ReportIndexMgmtID
	,ReportIndexMgmtVersionID
	,IndexNo
	,IndexID
	,MODIFIED_DATE
	,MODIFIED_USER_ID
	,CREATED_DATE
	,CREATED_USER_ID
	,DELETED_USER_ID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_Report_Indices_Management

SET IDENTITY_INSERT dbo.TBL_BR_ReportIndiceManagement OFF
-- TBL_BR_FASBProfileInformation
SET IDENTITY_INSERT dbo.TBL_BR_FASBProfileInformation ON

INSERT INTO TBL_BR_FASBProfileInformation (
	FASB_InstanceID
	,ManagerCode
	,InstanceTypeID
	,ReportingMonth
	,MarketValueMonth
	,ReportingDate
	,InstanceLabel
	,DeliveryDays
	,DeliveryTiming
	,FixedDate
	,ExcludedGiftKeys
	,CustomRequirements
	,CreatedDate
	,CreatedUserID
	,ModifiedDate
	,ModifiedUserID
	)
SELECT FASB_InstanceID
	,CLNT.BriefName
	,InstanceTypeID
	,ReportingMonth
	,MarketValueMonth
	,ReportingDate
	,InstanceLabel
	,DeliveryDays
	,DeliveryTiming
	,FixedDate
	,ExcludedGiftKeys
	,CustomRequirements
	,CreatedDate
	,CreatedUserID
	,ModifiedDate
	,ModifiedUserID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_FASB_ProfileInfo FPI
INNER JOIN $(ExcelsiorDB)..CLIENT CLNT
	ON FPI.ClientID = CLNT.ClientID

SET IDENTITY_INSERT dbo.TBL_BR_FASBProfileInformation OFF
-- TBL_BR_CGASetup
SET IDENTITY_INSERT TBL_BR_CGASetup ON

INSERT INTO TBL_BR_CGASetup (
	SETUP_ID
	,STATE_TYPE_ID
	,CGATYPE_TYPE_ID
	,RESERVE_TYPE_ID
	,ANNUAL_FORM_TYPE_ID
	,DAYS_FORM_DUE_AFTER_FY_OR_CY
	,ANNUAL_REPORTING_FORM_RULE
	,SUBMIT_FINANCIAL_STATEMENT_TYPE_ID
	,ANNUAL_FINANCIAL_STATEMENT_TYPE_ID
	,DAYS_STATEMENT_DUE_AFTER_FY_OR_CY
	,ANNUAL_FINANCIAL_STATEMENT_RULE
	,WEBSITE_ADDRESS
	,MODIFIED_DATE
	,MODIFIED_USER_ID
	,CREATED_DATE
	,CREATED_USER_ID
	,DELETED_USER_ID
	)
SELECT SETUP_ID
	,STATE_TYPE_ID
	,CGATYPE_TYPE_ID
	,RESERVE_TYPE_ID
	,ANNUAL_FORM_TYPE_ID
	,DAYS_FORM_DUE_AFTER_FY_OR_CY
	,ANNUAL_REPORTING_FORM_RULE
	,SUBMIT_FINANCIAL_STATEMENT_TYPE_ID
	,ANNUAL_FINANCIAL_STATEMENT_TYPE_ID
	,DAYS_STATEMENT_DUE_AFTER_FY_OR_CY
	,ANNUAL_FINANCIAL_STATEMENT_RULE
	,WEBSITE_ADDRESS
	,MODIFIED_DATE
	,MODIFIED_USER_ID
	,CREATED_DATE
	,CREATED_USER_ID
	,DELETED_USER_ID
FROM $(ExcelsiorDB)..TBL_EIS_EX_CGA_SETUP

SET IDENTITY_INSERT TBL_BR_CGASetup OFF

-- TBL_BR_FASBAccountExclusionConditionList
INSERT INTO TBL_BR_FASBAccountExclusionConditionList (
	CustomerAccountNumber
	,FASB_InstanceID
	,ListType
	,ConditionComments
	,CreatedDate
	,CreatedUserID
	)
SELECT DGA.AdventID
	,FASB_InstanceID
	,ListType
	,ConditionComments
	,CreatedDate
	,CreatedUserID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_FASB_AdventID_Exclusion_Condition_List FECL
INNER JOIN $(ExcelsiorDB)..DEFERREDGIFTACCOUNT DGA
	ON FECL.AccountID = DGA.AccountID

-- TBL_BR_FASBInputParam
SET IDENTITY_INSERT dbo.TBL_BR_FASBInputParam ON

INSERT INTO TBL_BR_FASBInputParam (
	Input_ParamID
	,FASB_InstanceID
	,DisplaySequence
	,TrustTypeLabel
	,IsReporting_Req
	,CalculationMethod
	,RateOf_ReturnType
	,MortalityTable
	,IsRun_ByRestriction
	,RateOfReturn
	,Comment
	,CreatedUserID
	,CreatedDate
	,ModifiedUserID
	,ModifiedDate
	)
SELECT Input_ParamID
	,FASB_InstanceID
	,DisplaySequence
	,TrustTypeLabel
	,IsReporting_Req
	,CalculationMethod
	,RateOf_ReturnType
	,MortalityTable
	,IsRun_ByRestriction
	,RateOfReturn
	,Comment
	,CreatedUserID
	,CreatedDate
	,ModifiedUserID
	,ModifiedDate
FROM $(ExcelsiorDB)..TBL_EIS_RPT_FASB_Input_Param

SET IDENTITY_INSERT dbo.TBL_BR_FASBInputParam OFF
-- TBL_BR_FASBOutputParam
SET IDENTITY_INSERT dbo.TBL_BR_FASBOutputParam ON

INSERT INTO TBL_BR_FASBOutputParam (
	Output_ParamID
	,FASB_InstanceID
	,DisplaySequence
	,ReportOutputType
	,IsOutputRequired
	,Sort1
	,Sort2
	,CreatedDate
	,CreatedUserID
	,ModifiedDate
	,ModifiedUserID
	)
SELECT Output_ParamID
	,FASB_InstanceID
	,DisplaySequence
	,ReportOutputType
	,IsOutputRequired
	,Sort1
	,Sort2
	,CreatedDate
	,CreatedUserID
	,ModifiedDate
	,ModifiedUserID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_FASB_Output_Param

SET IDENTITY_INSERT dbo.TBL_BR_FASBOutputParam OFF
-- TBL_BR_FASBTrustTypeLabelMap
SET IDENTITY_INSERT TBL_BR_FASBTrustTypeLabelMap ON

INSERT INTO TBL_BR_FASBTrustTypeLabelMap (
	RecordID
	,LabelID
	,List_Item_ID
	)
SELECT RecordID
	,LabelID
	,List_Item_ID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_FASB_Trust_Type_Label_Map

SET IDENTITY_INSERT TBL_BR_FASBTrustTypeLabelMap OFF

-- TBL_BR_FASBTrustTypeRateofReturnTypeMap
INSERT INTO TBL_BR_FASBTrustTypeRateofReturnTypeMap (
	LabelID
	,RateofReturnType
	)
SELECT LabelID
	,RateofReturnType
FROM $(ExcelsiorDB)..TBL_EIS_RPT_FASB_TrustType_RateofReturnType_Map

---- TBL_BR_FASBTrustTypeTranslation
SET IDENTITY_INSERT TBL_BR_FASBTrustTypeTranslation ON

INSERT INTO TBL_BR_FASBTrustTypeTranslation (
	TranAcctTypeRefKey
	,TrustTypeCalcMethod
	,InnotrustAccountType
	,GiftTypeID
	,RepPurpose
	)
VALUES (1	,'CRU'	,'CRUT'	,6	,'FASB'	)
	,(	2	,'CRU'	,'NIMU'	,6	,'FASB'	)
	,(	3	,'CRU'	,'NICT'	,6	,'FASB'	)
	,(	4	,'CRAT'	,'CRAT'	,5	,'FASB'	)
	,(	5	,'CLAT'	,'GLAT'	,3	,'FASB'	)
	,(	6	,'CLU'	,'GLUT'	,4	,'FASB'	)
	,(	7	,'PIF'	,'IRRV'	,9	,'FASB'	)
	,(	8	,'CRU'	,'IRRV'	,6	,'FASB'	)
	,(	9	,'CRAT'	,'IRRV'	,5	,'FASB'	)
	,(	10	,'PIF'	,'PR69'	,9	,'FASB'	)
	,(  11	,'CRU'	,'PR69'	,6	,'FASB'	)
	,(	12	,'CRAT'	,'PR69'	,5	,'FASB'	)
	,(	13	,'PIF'	,'FDN'	,9	,'FASB'	)
	,(	14	,'CRU'	,'FDN'	,6	,'FASB'	)
	,(	15	,'CRAT'	,'FDN'	,5	,'FASB'	)
	,(	19	,'N/A'	,'CRUT'	,6	,'990'	)
	,(	20	,'N/A'	,'NIMU'	,6	,'990'	)
	,(	21	,'N/A'	,'NICT'	,6	,'990'	)
	,(	22	,'N/A'	,'CRAT'	,5	,'990'	)
	,(	23	,'N/A'	,'GLAT'	,3	,'990'	)
	,(	24	,'N/A'	,'GLUT'	,4	,'990'	)
	,(	25	,'N/A'	,'PR69'	,9	,'990'	)
	,(	26	,'PIF'	,'PIF'	,9	,'FASB'	)
	,(	27	,'CGA'	,'GAP'	,2	,'FASB'	)
	,(	28	,'CLAT'	,'NLAT'	,3	,'FASB'	)
	,(	29	,'CLU'	,'NLUT'	,4	,'FASB'	)
	,(	30	,'PIF'	,'GIRV'	,9	,'FASB'	)
	,(	31	,'CRU'	,'GIRV'	,6	,'FASB'	)
	,(	32	,'CRAT'	,'GIRV'	,5	,'FASB'	)
	,(	33	,'N/A'	,'NLAT'	,3	,'990'	)
	,(	34	,'N/A'	,'NLUT'	,4	,'990'	)

SET IDENTITY_INSERT TBL_BR_FASBTrustTypeTranslation OFF
-- TBL_BR_StaffRoleNonClient
SET IDENTITY_INSERT TBL_BR_StaffRoleNonClient ON

INSERT INTO TBL_BR_StaffRoleNonClient (
	Staff_Role_non_client_ID
	,Master_Role_ID
	,User_ID
	)
SELECT Staff_Role_non_client_ID
	,Master_Role_ID
	,User_ID
FROM $(ExcelsiorDB)..TBL_EIS_STAFFROLENONCLIENT

SET IDENTITY_INSERT TBL_BR_StaffRoleNonClient OFF

-- TBL_EX_ProfilePFA
INSERT INTO TBL_EX_ProfilePFA (
	PFA_ID
	,CGA_SETUP_ID
	,ENTITY_TYPE_ID
	,ManagerCode
	,AllianceNumber
	,CustomerAccountNumber
	,FORM_DUE_DATE
	,FILING_STATUS_TYPE_ID
	,MODIFIED_DATE
	,MODIFIED_USER_ID
	,CREATED_DATE
	,CREATED_USER_ID
	,DELETED_USER_ID
	)
SELECT PFA_ID
	,CGA_SETUP_ID
	,ENTITY_TYPE_ID
	,Clnt.AccountManagerCode 
	,Pgm.AllianceNumber 
	,acc.CustomerAccountNumber 
	,FORM_DUE_DATE
	,FILING_STATUS_TYPE_ID
	,MODIFIED_DATE
	,MODIFIED_USER_ID
	,CREATED_DATE
	,CREATED_USER_ID
	,DELETED_USER_ID
FROM $(ExcelsiorDB)..TBL_EIS_EX_PROFILE_PFA PPFA
LEFT OUTER JOIN $(MappingDB)..TBL_AccountLookup ACC
	ON ACC.AccountID = PPFA.AccountID
LEFT OUTER JOIN $(MappingDB)..TBL_ProgramLookup Pgm
	on Pgm.ProgramID = PPFA.PROGRAMID	
LEFT OUTER JOIN $(MappingDB)..TBL_ClientLookup Clnt
	on Clnt.ClientId = PPFA.CLIENTID

-- TBL_BR_FASBAccountTypeList
-- This table has no data
INSERT INTO TBL_BR_FASBAccountTypeList (
	Input_ParamID
	,AccountTypeID
	,FASB_InstanceID
	,DisplaySequence
	,CreatedDate
	,CreatedUserID
	)
SELECT Input_ParamID
	,AccountTypeID
	,FASB_InstanceID
	,DisplaySequence
	,CreatedDate
	,CreatedUserID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_FASB_AccountType_List

-- TBL_BR_AUDIT_FASB
-- This Table has no data
SET IDENTITY_INSERT TBL_BR_AUDIT_FASB ON

INSERT INTO TBL_BR_AUDIT_FASB (
	RecordID
	,UserID
	,StartDateTime
	,EndDateTime
	,STATUS
	,Comments
	)
SELECT RecordID
	,UserID
	,StartDateTime
	,EndDateTime
	,STATUS
	,Comments
FROM $(ExcelsiorDB)..TBL_EIS_RPT_FASB_AUDIT

SET IDENTITY_INSERT TBL_BR_AUDIT_FASB OFF

-- TBL_BR_FASBAXYSMarketValue
INSERT INTO TBL_BR_FASBAXYSMarketValue (
	MarketValue
	,CustomerAccountNumber
	,MVDate
	,CreatedDate
	,CreatedUser
	)
SELECT MarketValue
	,AdventID
	,MVDate
	,CreatedDate
	,CreatedUser
FROM $(ExcelsiorDB)..TBL_EIS_RPT_FASB_AXYS_MARKETVALUE

-- TBL_BR_FASBProcessLog
SET IDENTITY_INSERT TBL_BR_FASBProcessLog ON

INSERT INTO TBL_BR_FASBProcessLog (
	RecordID
	,ProcessID
	,LevelDesc
	,ErrorDesc
	,StartTime
	,EndTime
	,STATUS
	)
SELECT RecordID
	,ProcessID
	,LevelDesc
	,ErrorDesc
	,StartTime
	,EndTime
	,STATUS
FROM $(ExcelsiorDB)..TBL_EIS_RPT_FASB_PROCESS_LOG

SET IDENTITY_INSERT TBL_BR_FASBProcessLog OFF
-- TBL_BR_FASBReportFile
SET IDENTITY_INSERT TBL_BR_FASBReportFile ON

INSERT INTO TBL_BR_FASBReportFile (
	FASB_FileID
	,FASB_File_Name
	,IS_Required
	,SP_Name
	,DB_Name
	,CreatedDate
	,Created_UserID
	,ModifiedDate
	,Modified_UserID
	)
SELECT FASB_FileID
	,FASB_File_Name
	,IS_Required
	,SP_Name
	,DB_Name
	,CreatedDate
	,Created_UserID
	,ModifiedDate
	,Modified_UserID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_FASB_Report_Files

SET IDENTITY_INSERT TBL_BR_FASBReportFile OFF
-- TBL_BR_FASBReportSetting
SET IDENTITY_INSERT TBL_BR_FASBReportSetting ON

INSERT INTO TBL_BR_FASBReportSetting (
	Report_ColumnID
	,FASB_FileID
	,IS_Required
	,Column_Sequence
	,Table_Name
	,Column_Name
	,CSV_Column_Name
	,Data_Definition
	,Default_Value
	,CreatedDate
	,Created_UserID
	,ModifiedDate
	,Modified_UserID
	)
SELECT Report_ColumnID
	,FASB_FileID
	,IS_Required
	,Column_Sequence
	,Table_Name
	,Column_Name
	,CSV_Column_Name
	,Data_Definition
	,Default_Value
	,CreatedDate
	,Created_UserID
	,ModifiedDate
	,Modified_UserID
FROM $(ExcelsiorDB)..TBL_EIS_RPT_FASB_Report_Settings

SET IDENTITY_INSERT TBL_BR_FASBReportSetting OFF
-- TBL_BR_FASBSubTypeTranslation
SET IDENTITY_INSERT TBL_BR_FASBSubTypeTranslation ON

INSERT INTO TBL_BR_FASBSubTypeTranslation (
	SubTypeRefKey
	,TrustTypeCalcMethod
	,AccountType
	,Lead_Trust_Taxation_ID
	,SubTypeID
	,RepPurpose
	)
SELECT SubTypeRefKey
	,TrustTypeCalcMethod
	,EX_AccountType
	,Lead_Trust_Taxation_ID
	,SubTypeID
	,RepPurpose
FROM $(ExcelsiorDB)..TBL_EIS_RPT_FASB_SubTypeTranslation

SET IDENTITY_INSERT TBL_BR_FASBSubTypeTranslation OFF

-- TBL_BR_FASBValidationResult
INSERT INTO TBL_BR_FASBValidationResult (
	ManagerCode
	,ManagerName
	,RecordIdentifier
	,ValidationMessage
	,ValidationType
	)
SELECT Clnt.Briefname
	,ClientName
	,RecordIdentifier
	,ValidationMessage
	,ValidationType
FROM $(ExcelsiorDB)..TBL_EIS_RPT_FASB_VALIDATION_RESULTS Rslt
LEFT OUTER JOIN $(ExcelsiorDB)..CLIENT Clnt
	ON Clnt.ClientID = Rslt.ClientID

INSERT INTO TBL_EIS_DT_STG_Detect_PostDataLoad (
	DQEndDate
	,DQueueID
	,AccountID
	,TLAsofDate_Date
	,dtStartDate
	)
SELECT DQEndDate
	,DQueueID
	,AccountID
	,TLAsofDate_Date
	,dtStartDate
FROM $(ExcelsiorDB)..TBL_EIS_DT_STG_Detect_PostDataLoad


INSERT INTO TBL_EIS_DT_STG_EndingMarketValue (
	DQEndDate
	,DQueueID
	,AccountID
	,EndingMarketValue
	,PortfolioObjectiveDescription
	)
SELECT DQEndDate
	,DQueueID
	,AccountID
	,EndingMarketValue
	,PortfolioObjectiveDescription
FROM $(ExcelsiorDB)..TBL_EIS_DT_STG_EndingMarketValue


-------------TBL_BR_AUDIT_PIFProjection
SET IDENTITY_INSERT TBL_BR_AUDIT_PIFProjection ON 
GO
INSERT INTO TBL_BR_AUDIT_PIFProjection (AUDIT_SEQUENCE,
		AUDIT_USER_ID,
		AUDIT_DATETIME,
		AUDIT_FLAG,
		AUDIT_TABLE,
		AUDIT_DETAILS,
		ProjectionID,
		CustomerAccountNumber,
		Year,
		ExpectedIncome,
		MODIFIED_DATE,
		MODIFIED_USER_ID,
		CREATED_DATE,
		CREATED_USER_ID,
		DELETED_USER_ID
		)
SELECT AUDIT_SEQUENCE,
		AUDIT_USER_ID,
		AUDIT_DATETIME,
		AUDIT_FLAG,
		AUDIT_TABLE,
		AUDIT_DETAILS,
		ProjectionID,
		ACCLKP.CustomerAccountNumber,
		Year,
		ExpectedIncome,
		MODIFIED_DATE,
		MODIFIED_USER_ID,
		CREATED_DATE,
		CREATED_USER_ID,
		DELETED_USER_ID
		FROM $(ExcelsiorDB)..TBL_EIS_RPT_AUDIT_PIF_PROJECTION PIFPROJ 
		INNER JOIN TBL_Lookup_Account ACCLKP ON ACCLKP.AccountID=PIFPROJ.Accountid

SET IDENTITY_INSERT TBL_BR_AUDIT_PIFProjection OFF 
GO

------------TBL_BR_FASBTrustTypeTranslation
SET IDENTITY_INSERT TBL_BR_FASBTrustTypeTranslation ON 
GO

INSERT INTO TBL_BR_FASBTrustTypeTranslation(TranAcctTypeRefKey,
		TrustTypeCalcMethod,
		InnotrustAccountType,
		GiftTypeID,
		RepPurpose)
SELECT  TranAcctTypeRefKey,
		TrustTypeCalcMethod,
		ACC.SourceAccountType,--EX_AccountType,
		GiftTypeID,
		RepPurpose
		FROM $(ExcelsiorDB)..TBL_EIS_RPT_FASB_TrustTypeTranslation  TRUST
		INNER JOIN TBL_Lookup_AccountType ACC ON ACC.TargetAccountType=TRUST.EX_AccountType

SET IDENTITY_INSERT TBL_BR_FASBTrustTypeTranslation OFF 
GO


IF OBJECT_ID(N'TEMPDB.[DBO].[#Tmp_GlobalContacts]') IS NOT NULL
BEGIN
	DROP TABLE [DBO].[#Tmp_GlobalContacts]
END


PRINT 'Due to innotrust data issue, for TBL_IRS_ProfileTax table - BENEFICIARY_TAX_FORM_ID, TAX_RETURN_ID, TAX_10989_ID, TAX_1099R_ID columns - few of the partyids are not migrated'