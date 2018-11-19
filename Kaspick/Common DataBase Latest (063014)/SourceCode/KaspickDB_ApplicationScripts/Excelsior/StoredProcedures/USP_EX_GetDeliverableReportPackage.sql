IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetDeliverableReportPackage'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetDeliverableReportPackage;

	PRINT 'DROPPED USP_EX_GetDeliverableReportPackage';
END
GO

SET QUOTED_IDENTIFIER ON
GO

/******************************************************************************        
** New Name   :   USP_EX_GetDeliverableReportPackage
** Old Name   :   USP_EIS_DLV_ReportPackage_DDLSelProc         
** Short Desc : Get details from TBL_EIS_DLV_DeliverableType, TBL_EIS_LIST_ITEM, TBL_EIS_DLV_DeliverableProcess, TBL_EIS_DLV_DeliverableWebsiteOption table.   
**        
** Full Description        
  
** Sample Call   
 EXEC USP_EIS_DLV_ReportPackage_DDLSelProc    
**        
** Return values: NONE        
**        
** Standard declarations        
**       SET LOCK_TIMEOUT         30000   -- 30 seconds        
** Created By :  Jenney Alexandria  
** Company  :  Kaspick & Company        
** Project  :  Deliverable Tool       
** Created DT :  April/11/2011        
**                    
*******************************************************************************        
**       Change History        
*******************************************************************************        
** Date:       Author:           Description:           Rvwd        
** --------  -------- ------ ------------------------------------------ -------  
** Aug-08-2011 Chaithra Madappa   Changes for Website Phase 2 - Reportiore Queue Creation  
** Jan-01-2012 Ashvin Mandowara   Changes for Adding Review Level in Deliverable Process  
** Mar-12-2012 Anand Kumar    Changes for Adding Comments as per Stored Procedure standard template.  
** Mar-14-2012   Anand Kumar    ERD changes implemetation.  
** Apr-05-2012 Ashvin   Adding Values for NoofApprovalLevels for DT-Sprint Plan 2  
** Apr-13-2012 Anand   Modification regarding User Role View - DT-Sprint Plan 2  
** Apr-16-2012 Anand   Added joins for replacing the sub query and used v_eis_list_items instead tables  
** Apr-04-2014 Sanath  Req EXCREQ3.1 Report publish setup  
** 22-May-2014  Sanath   Sp name renamed as per Kaspick naming convention standard 
  
*******************************************************************************        
** Copyright (C) 2007 Kaspick & Company, All Rights Reserved        
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION        
*******************************************************************************/
CREATE PROCEDURE USP_EX_GetDeliverableReportPackage
AS
BEGIN
	SELECT DeliverableTypeID
		,DeliverableTypeName
	FROM TBL_DLV_DeliverableType

	SELECT ListItemID
		,ListItemName
	FROM VW_EX_ListItem
	WHERE ListTypeName = 'Deliverable Category List'

	SELECT ListItemID
		,ListItemName
	FROM VW_EX_ListItem
	WHERE ListTypeName = 'Logical Value'
	ORDER BY ListItemID ASC

	SELECT NoofApprovalLevels AS DeliverableReviewLevel
		,DeliverableProcessID
		,DeliverableProcessName
		,DeliverableProcessDescription
	FROM TBL_DLV_DeliverableProcess
	WHERE IsActive = 1

	SELECT ListItemID
		,ListItemName
	FROM VW_EX_ListItem
	WHERE ListTypeName = 'Service Offering'
	ORDER BY ListItemID

	SELECT ListItemID
		,ListItemName
	FROM VW_EX_ListItem
	WHERE ListTypeName = 'Year Type'
	ORDER BY ListItemID

	SELECT ListItemID
		,ListItemName
	FROM VW_EX_ListItem
	WHERE ListTypeName = 'Annual Frequency'
	ORDER BY ListItemID

	-- Remove 'Standard Kaspick Email Footer' from the List of Email Template.      
	SELECT email_template_id
		,email_template_name
	FROM TBL_EML_EmailTemplate
	WHERE email_template_id != 0
	ORDER BY email_template_name ASC

	SELECT DlvDelv.DeliverableID
		,DlvDelv.DeliverableName
	FROM TBL_DLV_Deliverable DlvDelv
	INNER JOIN TBL_DLV_DeliverableType DlvDelvTyp ON DlvDelv.DeliverableTypeID = DlvDelvTyp.DeliverableTypeID
	WHERE DlvDelvTyp.DeliverableTypeName = 'Report Package'
	ORDER BY DlvDelv.DeliverableName

	SELECT ListItemID
		,ListItemName
	FROM VW_EX_ListItem
	WHERE ListTypeName = 'Website Destination'

	SELECT ListItemID
		,ListItemName
	FROM VW_EX_ListItem
	WHERE ListTypeName = 'MVD Deliverable Type'

	SELECT DlvDelv.DeliverableID
		,(CAST(DlvDelv.DeliverableTypeID AS VARCHAR(10)) + ',' + DlvDelv.DeliverableName) AS DeliverableName
	FROM TBL_DLV_Deliverable DlvDelv
	INNER JOIN TBL_DLV_DeliverableType DlvDelvTyp ON DlvDelv.DeliverableTypeID = DlvDelvTyp.DeliverableTypeID
	WHERE DlvDelvTyp.DeliverableTypeName IN (
			'Report'
			,'Report Package'
			,'Report in a Package'
			)
	ORDER BY DlvDelv.DeliverableName

	SELECT AutoCompleteExtenderID
		,AutoCompleteExtenderName
	FROM TBL_DLV_AutoCompleteExtenderList
	WHERE AutoCompleteExtenderType = 'Folder and File Name'

	SELECT DeliveryMethodID
		,DeliveryMethodName
	FROM TBL_DLV_DeliveryMethod

	SELECT DlvDelv.DeliverableID
		,DlvDelv.DeliverableName
	FROM TBL_DLV_Deliverable DlvDelv
	INNER JOIN TBL_DLV_ManagerCodeDeliverableReportInPackage MgrCodDelvRptPkg ON DlvDelv.DeliverableID = MgrCodDelvRptPkg.DeliverableID
	INNER JOIN TBL_DLV_DeliverableType DlvDelvTyp ON DlvDelvTyp.DeliverableTypeID = DlvDelv.DeliverableTypeID
	WHERE DlvDelvTyp.DeliverableTypeName IN (
			'Report'
			,'Report Package'
			,'Report in a Package'
			)

	SELECT AutoCompleteExtenderID
		,(AutoCompleteExtenderName + ',' + Description) AS AutoCompleteExtenderName
	FROM TBL_DLV_AutoCompleteExtenderList
	WHERE AutoCompleteExtenderType = 'Folder and File Name'

	SELECT ListItemID
		,ListItemName
	FROM VW_EX_ListItem
	WHERE ListTypeName = 'ReportLevel'
	ORDER BY DisplaySequence ASC

	SELECT DISTINCT MasterRoleID
		,RoleFullName
	FROM VW_EX_UserRole
	ORDER BY RoleFullName

	--Selecting all distinct active Users from V_EIS_USER_ROLE view      
	SELECT DISTINCT VwUsrRol.MasterRoleID
		,VwUsrRol.UserID
		,VwUsrRol.USER_NAME
	FROM VW_EX_UserRole VwUsrRol
	INNER JOIN TBL_KS_User KsUsr ON KsUsr.UserID = VwUsrRol.UserID
	WHERE KsUsr.IsActive = 1
	ORDER BY VwUsrRol.USER_NAME
END
GO

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetDeliverableReportPackage'
		)
BEGIN
	PRINT 'CREATED USP_EX_GetDeliverableReportPackage';
END