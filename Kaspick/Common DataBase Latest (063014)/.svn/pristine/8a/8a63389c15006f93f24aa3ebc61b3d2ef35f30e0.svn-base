IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetDeliverableValidation'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetDeliverableValidation;

	PRINT 'DROPPED USP_EX_GetDeliverableValidation ';
END
GO

SET QUOTED_IDENTIFIER ON
GO

/******************************************************************************          
** New Name   :   USP_EX_GetDeliverableValidation
** Old Name   :   USP_EIS_DLV_Deliverable_Validation_Proc          
**    
** Short Desc : Validation for Deliverable Settings on edit     
**          
** Full Description          
**          
**  Validation for Deliverable Settings on edit     
**    
** Sample Call     
 EXEC USP_EX_GetDeliverableValidation'<ReportPackageCollection><InsertList><ServiceOffering ServiceOfferingID="1736"  /><YearType YearTypeID="1804"  OtherYearTypeDescription=""  /><Frequency FrequencyID="1722"  OtherFrequencyDescription=""  /><DeliverableMethod DeliverableMethodID="1"  /><PrimaryDeliverableMethod PrimaryDeliverableMethodID="1"  /></InsertList><UpdateList></UpdateList><DeleteList></DeleteList></ReportPackageCollection>',0,2,1800,'tt','tt','tt','tt',1,9,'C:\Users\shegde\Desktop\','',19,1,0,0,100038,0,0,0,0,0,0,0,0,0,'No',0,0,1835,'tt',10,100060,0,0,0,0,19   
 
  '<ReportPackageCollection><InsertList><ServiceOffering ServiceOfferingID=1736 /><YearType YearTypeID=1804 OtherYearTypeDescription='' /><Frequency FrequencyID=1722 OtherFrequencyDescription=''  /><DeliverableMethod DeliverableMethodID=1  /><PrimaryDeliverableMethod PrimaryDeliverableMethodID=1  /></InsertList><UpdateList></UpdateList><DeleteList></DeleteList></ReportPackageCollection>',0,2,1800,'tt','tt','tt','tt',1,9,'C:\Users\shegde\Desktop\','',19,1,0,0,100038,0,0,0,0,0,0,0,0,0,'No',0,0,1835,'tt',10,100060,0,0,0,0,19   
**          
** Return values: NONE          
**          
** Standard declarations          
**       SET LOCK_TIMEOUT         30000   -- 30 seconds          
** Created By :  Niveditha    
** Company  :  Kaspick & Company          
** Project  :  Deliverable Tool         
** Created DT :  May/05/2011           
**                      
*******************************************************************************          
**       Change History          
*******************************************************************************          
** Date:     Author:  Bug #  Description:        Rvwd          
** --------  -------- ------ ------------------------------------------ -------    
** Aug-08-2011 Chaithra Madappa   Changes for Website Phase 2 - Reportiore Queue Creation    
** Apr-05-2012 Ashvin   Adding Values for Owner, Approver 1 and Approver 2 for DT-Sprint Plan 2    
** Jun-26-2012 Anand   Adding Values for QueueCreationEnabled for DT-Sprint Plan 4    
** Apr-04-2014 Sanath   Req EXCREQ3.1 Excelsior Prime project
** 22-May-2014  Sanath   Sp name renamed as per Kaspick naming convention standard 
  
*******************************************************************************          
** Copyright (C) 2007 Kaspick & Company, All Rights Reserved          
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION          
*******************************************************************************/
CREATE PROCEDURE USP_EX_GetDeliverableValidation (
	@DeliverableXML XML
	,@intDeliverableID INT
	,@DeliverableTypeID INT
	,@DeliverableCategoryListID INT
	,@DeliverableName VARCHAR(4000)
	,@DeliverableDisplayName VARCHAR(4000)
	,@DeliverableClientDescription VARCHAR(4000)
	,@DeliverableInternalDescription VARCHAR(4000)
	,@IncludeClientReportMatrix BIT
	,@DeliverableProcessID INT
	,@RootFolderPath VARCHAR(4000)
	,@FileNameConvention VARCHAR(4000)
	,@UploadToWebsite INT
	,@UploadToWebsiteClientDescription VARCHAR(5)
	,@DeliverableLevel INT
	,@Parent_DeliverableID INT
	,@EmailGroupingID INT
	,@USER_ID INT
	,@AccountRequired INT
	,@ContactRequired INT
	,@ExpiryYears INT
	,@ExpiryMonths INT
	,@ExpiryDays INT
	,@AllowExpirationDateOverride INT
	,@MaximumFileSize INT
	,@UploadEmailTemplateID INT
	,@ReplaceEmailTemplateID INT
	,@MVDDeliverableID BIGINT
	,@IsActive BIT
	,@WebsiteDestinationID BIGINT
	,@DeliverableID INT OUTPUT
	,@client_param VARCHAR(1000) OUTPUT
	,@ReportLevelID INT
	,@QueueFilter VARCHAR(50)
	,@OwnerRole INT
	,@Owner INT
	,@Approver1Role INT
	,@Approver1 INT
	,@Approver2Role INT
	,@Approver2 INT
	,@QueueCreationEnabled INT
	)
AS
BEGIN
	IF @intDeliverableID > 0
	BEGIN
		SELECT @client_param = COALESCE(@client_param + ', ', '') + a.listname
		FROM (
			SELECT DISTINCT LstItm.LISTITEMNAME AS listname
			FROM TBL_DLV_ManagerCodeDeliverableYearType MgrCodDelvYerTyp
			INNER JOIN TBL_DLV_DeliverableYearType DelvYerTyp ON MgrCodDelvYerTyp.DeliverableYearTypeID = DelvYerTyp.DeliverableYearTypeID
			INNER JOIN TBL_LISTITEM LstItm ON LstItm.LISTITEMID = DelvYerTyp.YearTypeID
			WHERE DelvYerTyp.DeliverableID = @intDeliverableID
				AND LstItm.LISTITEMID NOT IN (
					SELECT TempTable.YearTypeColumns.value('@YearTypeID[1]', 'bigint')
					FROM @DeliverableXML.nodes('/ReportPackageCollection/InsertList/YearType') AS TempTable(YearTypeColumns)
					)
			) a

		SELECT @client_param = COALESCE(@client_param + ', ', '') + b.listname
		FROM (
			SELECT DISTINCT LstItm.LISTITEMNAME AS listname
			FROM TBL_DLV_ManagerCodeDeliverableFrequency MgrCodDelvFreq
			INNER JOIN TBL_DLV_DeliverableFrequency DelvFreq ON MgrCodDelvFreq.DeliverableFrequencyID = DelvFreq.DeliverableFrequencyID
			INNER JOIN TBL_LISTITEM LstItm ON LstItm.ListItemID = DelvFreq.FrequencyID
			WHERE DelvFreq.DeliverableID = @intDeliverableID
				AND LstItm.LISTITEMID NOT IN (
					SELECT TempTable.YearTypeColumns.value('@FrequencyID[1]', 'bigint')
					FROM @DeliverableXML.nodes('/ReportPackageCollection/InsertList/Frequency') AS TempTable(YearTypeColumns)
					)
			) b
	END

	IF @intDeliverableID = 0
		OR (
			@intDeliverableID > 0
			AND @client_param IS NULL
			)
		EXEC USP_EX_SaveDeliverable @DeliverableXML
			,@intDeliverableID
			,@DeliverableTypeID
			,@DeliverableCategoryListID
			,@DeliverableName
			,@DeliverableDisplayName
			,@DeliverableClientDescription
			,@DeliverableInternalDescription
			,@IncludeClientReportMatrix
			,@DeliverableProcessID
			,@RootFolderPath
			,@FileNameConvention
			,@UploadToWebsite
			,@UploadToWebsiteClientDescription
			,@DeliverableLevel
			,@Parent_DeliverableID
			,@EmailGroupingID
			,@USER_ID
			,@AccountRequired
			,@ContactRequired
			,@ExpiryYears
			,@ExpiryMonths
			,@ExpiryDays
			,@AllowExpirationDateOverride
			,@MaximumFileSize
			,@UploadEmailTemplateID
			,@ReplaceEmailTemplateID
			,@MVDDeliverableID
			,@IsActive
			,@WebsiteDestinationID
			,@DeliverableID OUTPUT
			,@client_param OUTPUT
			,@ReportLevelID
			,@QueueFilter
			,@OwnerRole
			,@Owner
			,@Approver1Role
			,@Approver1
			,@Approver2Role
			,@Approver2
			,@QueueCreationEnabled
	ELSE
		SET @DeliverableID = @intDeliverableID
END
GO

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetDeliverableValidation'
		)
BEGIN
	PRINT 'CREATED USP_EX_GetDeliverableValidation ';
END