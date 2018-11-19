IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetDeliverableDetails'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetDeliverableDetails;

	PRINT 'DROPPED PROCEDURE USP_EX_GetDeliverableDetails';
END
GO

SET QUOTED_IDENTIFIER ON
GO

/******************************************************************************        
** New Name   :   USP_EX_GetDeliverableDetails
** Old Name   :   USP_EIS_DLV_DeliverableDetails_SelProc        
**  
** Short Desc : Procedure to retrieve Deliverable Settings   
**        
** Full Description        
**        
** Input Arguments:      
**  
** Sample Call   
 EXEC USP_EX_GetDeliverableDetails 1  
      
** Standard declarations        
**       SET LOCK_TIMEOUT         30000   -- 30 seconds        
** Created By :  Niveditha  
** Company  :  Kaspick & Company        
** Project  :  Deliverable Tool       
** Created DT :  25-Apr-11        
**                    
*******************************************************************************        
**       Change History        
*******************************************************************************        
** Date:     Author:  Bug #  Description:        Rvwd        
** --------  -------- ------ ------------------------------------------ -------  
** 25-Apr-11     Niveditha   Created   
** Aug-08-2011 Chaithra Madappa   Changes for Website Phase 2 - Reportiore Queue Creation  
** Jan-01-2012 Ashvin Mandowara   Changes for Adding Review Level in Deliverable Process  
** Mar-12-2012 Anand Kumar    Changes for Adding Comments as per Stored Procedure standard template.  
** Mar-14-2012 Anand Kumar    Changes for renaming column DeliverableTypeID to DeliverableProcessID in TBL_EIS_DT_Delivery_Type_Methods.  
** Jun-26-2012 Anand Kumar    Added QueueCreationEnabled Column in result set for Sprint 4  
** Feb-27-2014  Sanath        Report Publish setup link pointing to KaspickDB
** 22-May-2014  Sanath        Sp name renamed as per Kaspick naming convention standard 
  
*******************************************************************************        
** Copyright (C) 2007 Kaspick & Company, All Rights Reserved        
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION        
*******************************************************************************/
CREATE PROCEDURE USP_EX_GetDeliverableDetails (@DeliverableID INT)
AS
BEGIN
	DECLARE @AccountRequired INT
	DECLARE @MVDDeliverableID INT
	DECLARE @ContactRequired INT
	DECLARE @ExpiryYears INT
	DECLARE @ExpiryMonths INT
	DECLARE @ExpiryDays INT
	DECLARE @AllowExpirationDateOverride INT
	DECLARE @MaximumFileSize INT
	DECLARE @UploadEmailTemplateID INT
	DECLARE @ReplaceEmailTemplateID INT
	DECLARE @CreatedDate DATETIME
	DECLARE @CreatedUserID INT
	DECLARE @ModifiedDate DATETIME
	DECLARE @ModifiedUserID INT
	DECLARE @YearTypeID VARCHAR(4000)
	DECLARE @OtherYearDesc VARCHAR(4000)
	DECLARE @Freq VARCHAR(4000)
	DECLARE @OtherFreq VARCHAR(4000)
	DECLARE @WebsiteDestinationID BIGINT;
	DECLARE @DeliverableMethods VARCHAR(4000);
	DECLARE @PrimaryMethodID BIGINT;
	DECLARE @DeliverableProcessID INT;

	SELECT @YearTypeID = COALESCE(@YearTypeID + ', ', '') + CAST(YearTypeID AS VARCHAR(10))
	FROM TBL_DLV_DeliverableYearType
	WHERE DeliverableID = @DeliverableID

	SELECT @OtherYearDesc = CAST(OtherYearTypeDescription AS VARCHAR(1000))
	FROM TBL_DLV_DeliverableYearType
	WHERE DeliverableID = @DeliverableID
		AND OtherYearTypeDescription != ''

	SELECT @Freq = COALESCE(@Freq + ', ', '') + CAST(frequencyid AS VARCHAR(10))
	FROM TBL_DLV_DeliverableFrequency
	WHERE DeliverableID = @DeliverableID

	SELECT @OtherFreq = CAST(OtherFrequencyDescription AS VARCHAR(1000))
	FROM TBL_DLV_DeliverableFrequency
	WHERE DeliverableID = @DeliverableID
		AND OtherFrequencyDescription != ''

	-- Modification for column name change DeliverableTypeID to DeliverableProcessID in where clause  
	SELECT @DeliverableProcessID = DeliverableProcessID
	FROM TBL_DLV_Deliverable
	WHERE DeliverableID = @DeliverableID

	SELECT @DeliverableMethods = COALESCE(@DeliverableMethods + ', ', '') + CAST(DeliveryMethodID AS VARCHAR(10))
	FROM TBL_DLV_DeliverableMethod
	WHERE DeliverableID = @DeliverableID

	SELECT @PrimaryMethodID = DeliveryMethodID
	FROM TBL_DLV_DeliverableMethod
	WHERE DeliverableID = @DeliverableID
		AND IsPrimary = 1;

	-- End Modification  
	SELECT @AccountRequired = AccountRequired
		,@MVDDeliverableID = MVDDeliverableID
		,@ContactRequired = ContactRequired
		,@ExpiryYears = ExpiryYears
		,@ExpiryMonths = ExpiryMonths
		,@ExpiryDays = ExpiryDays
		,@AllowExpirationDateOverride = AllowExpirationDateOverride
		,@MaximumFileSize = MaximumFileSize
		,@UploadEmailTemplateID = UploadEmailTemplateID
		,@ReplaceEmailTemplateID = ReplaceEmailTemplateID
		,@WebsiteDestinationID = WebsiteDestinationID
		,@CreatedDate = CreatedDate
		,@CreatedUserID = CreatedUserID
		,@ModifiedDate = ModifiedDate
		,@ModifiedUserID = ModifiedUserID
	FROM TBL_DLV_DeliverableWebsiteOption
	WHERE DeliverableID = @DeliverableID

	DECLARE @Serv VARCHAR(4000)

	SELECT @Serv = COALESCE(@Serv + ', ', '') + CAST(ServiceOfferingID AS VARCHAR(10))
	FROM TBL_DLV_DeliverableServiceOffering
	WHERE DeliverableID = @DeliverableID

	SELECT DeliverableID
		,Delv.DeliverableTypeID
		,DeliverableName
		,DeliverableDisplayName
		,DeliverableManagerCodeDescription
		,DeliverableInternalDescription
		,DeliverableCategoryListID
		,IncludeManagerCodeReportMatrix
		,Delv.DeliverableProcessID
		,@Serv AS ServiceOfferingID
		,@YearTypeID AS YearTypeID
		,@OtherYearDesc AS OtherYearDesc
		,@Freq AS FrequencyID
		,@OtherFreq AS OtherFreq
		,RootFolderPath
		,Parent_DeliverableID
		,FileNameConvention
		,UploadToWebsite
		,DeliverableLevel
		,@AccountRequired AS AccountRequired
		,@MVDDeliverableID AS MVDDeliverableID
		,@ContactRequired AS ContactRequired
		,@ExpiryYears AS ExpiryYears
		,@ExpiryMonths AS ExpiryMonthsend
		,@ExpiryDays AS ExpiryDays
		,@AllowExpirationDateOverride AS AllowExpirationDateOverride
		,@MaximumFileSize AS MaximumFileSize
		,@UploadEmailTemplateID AS UploadEmailTemplateID
		,@ReplaceEmailTemplateID AS ReplaceEmailTemplateID
		,Delv.IsActive
		,@WebsiteDestinationID AS WebsiteDestinationID
		,@CreatedDate AS CreatedDate
		,@CreatedUserID AS CreatedUserID
		,@ModifiedDate AS ModifiedDate
		,@ModifiedUserID AS ModifiedUserID
		,@DeliverableMethods AS DeliverableMethodID
		,@PrimaryMethodID AS PrimaryDeliverableMethodID
		,ReportLevelID
		,QueueFilter
		,DelvProc.NoofApprovalLevels AS DeliverableReviewLevel
		,OwnerRoleId
		,OwnerId
		,Approver1RoleId
		,Approver1Id
		,Approver2RoleId
		,Approver2Id
		,QueueCreationEnabled
	FROM TBL_DLV_Deliverable Delv
	INNER JOIN TBL_DLV_DeliverableProcess DelvProc ON Delv.DeliverableProcessID = DelvProc.DeliverableProcessID
	WHERE DeliverableID = @DeliverableID

	SELECT DeliverableID
		,DeliverableName
		,FileNameConvention
	FROM TBL_DLV_Deliverable
	WHERE Parent_DeliverableID = @DeliverableID
	ORDER BY DeliverableName ASC
END
GO

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetDeliverableDetails'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_GetDeliverableDetails';
END