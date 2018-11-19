IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetDeliverable'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetDeliverable;

	PRINT 'DROPPED USP_EX_GetDeliverable';
END
GO

SET QUOTED_IDENTIFIER ON
GO

/******************************************************************************        
** New Name   :   USP_EX_GetDeliverable        
** Old Name   :   USP_EIS_DLV_Deliverable_SelProc        
**  
** Short Desc : Get details from TBL_EIS_DLV_Deliverable table.  
**        
** Full Description        
**        
**  Get details from TBL_EIS_DLV_Deliverable table.   
**  
** Sample Call   
 EXEC USP_EIS_DLV_Deliverable_SelProc  1799 , Reports  
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
** Date:     Author:  Bug #  Description:        Rvwd        
** --------  -------- ------ ------------------------------------------ -------  

** 2-Apr-2014 Yugandhar  EXCREQ 3.1 
** 22-May-2014  Sanath   Sp name renamed as per Kaspick naming convention standard 
*******************************************************************************        
** Copyright (C) 2007 Kaspick & Company, All Rights Reserved        
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION        
*******************************************************************************/
CREATE PROCEDURE USP_EX_GetDeliverable (
	@CategoryID INT
	,@Type VARCHAR(10)
	)
AS
BEGIN
	IF (@Type = 'Reports')
	BEGIN
		SELECT DlvDlvr.DeliverableID
			,DlvDlvr.DeliverableName
			,LstItm.LISTITEMNAME AS DeliverableCategory
			,CASE 
				WHEN (
						SELECT DeliverableTypeName
						FROM TBL_DLV_DeliverableType
						WHERE DeliverableTypeID = DlvDlvr.DeliverableTypeID
						) = 'Report Package'
					THEN 'Report Package'
				WHEN (
						SELECT DeliverableTypeName
						FROM TBL_DLV_DeliverableType
						WHERE DeliverableTypeID = DlvDlvr.DeliverableTypeID
						) = 'Report in a Package'
					THEN 'Report in a Package'
				WHEN (
						SELECT DeliverableTypeName
						FROM TBL_DLV_DeliverableType
						WHERE DeliverableTypeID = DlvDlvr.DeliverableTypeID
						) = 'Report'
					THEN 'Report'
				ELSE (
						SELECT DeliverableTypeName
						FROM TBL_DLV_DeliverableType
						WHERE DeliverableTypeID = DlvDlvr.DeliverableTypeID
						)
				END AS Type
			,DeilverableType = (
				SELECT DeliverableTypeName
				FROM TBL_DLV_DeliverableType
				WHERE DeliverableTypeID = DlvDlvr.DeliverableTypeID
				)
			,DlvDlvrRpt.DeliverableName AS ReportInPackage
			,CASE 
				WHEN DlvDlvr.Parent_DeliverableID IS NULL
					THEN DlvDlvr.DeliverableID
				ELSE DlvDlvr.Parent_DeliverableID
				END AS 'sortid'
			,CASE 
				WHEN (
						SELECT DeliverableTypeName
						FROM TBL_DLV_DeliverableType
						WHERE DeliverableTypeID = DlvDlvr.DeliverableTypeID
						) = 'Report Package'
					THEN 1
				WHEN (
						SELECT DeliverableTypeName
						FROM TBL_DLV_DeliverableType
						WHERE DeliverableTypeID = DlvDlvr.DeliverableTypeID
						) = 'Report in a Package'
					THEN 2
				WHEN (
						SELECT DeliverableTypeName
						FROM TBL_DLV_DeliverableType
						WHERE DeliverableTypeID = DlvDlvr.DeliverableTypeID
						) = 'Report'
					THEN 3
				ELSE (
						SELECT DeliverableTypeName
						FROM TBL_DLV_DeliverableType
						WHERE DeliverableTypeID = DlvDlvr.DeliverableTypeID
						)
				END AS 'sortname'
		FROM TBL_DLV_Deliverable DlvDlvr
		LEFT JOIN TBL_DLV_Deliverable DlvDlvrRpt ON DlvDlvrRpt.DeliverableID = DlvDlvr.Parent_DeliverableID
		LEFT JOIN TBL_ListItem LstItm ON LstItm.LISTITEMID = DlvDlvr.DeliverableCategoryListID
		WHERE @CategoryID = (
				CASE 
					WHEN isnull(@CategoryID, 0) = 0
						THEN 0
					ELSE DlvDlvr.DeliverableCategoryListID
					END
				)
			AND DlvDlvr.DeliverableTypeID IN (
				SELECT DeliverableTypeID
				FROM TBL_DLV_DeliverableType
				WHERE DeliverableTypeName IN (
						'Report'
						,'Report Package'
						,'Report in a Package'
						)
				)
		ORDER BY LstItm.LISTITEMNAME
			,sortid
			,sortname
			,DlvDlvr.DeliverableName
	END
	ELSE IF (@Type = 'Emails')
	BEGIN
		SELECT DlvDlvr.DeliverableID
			,DlvDlvr.DeliverableName
			,LstItm.LISTITEMNAME AS DeliverableCategory
			,DeilverableType = (
				SELECT DeliverableTypeName
				FROM TBL_DLV_DeliverableType
				WHERE DeliverableTypeID = DlvDlvr.DeliverableTypeID
				)
			,CASE 
				WHEN (
						SELECT DeliverableTypeName
						FROM TBL_DLV_DeliverableType
						WHERE DeliverableTypeID = DlvDlvr.DeliverableTypeID
						) = 'Email List'
					THEN 'Email List'
				END AS Type
			,DlvDlvrRpt.DeliverableName AS ReportInPackage
		FROM TBL_DLV_Deliverable DlvDlvr
		LEFT JOIN TBL_DLV_Deliverable DlvDlvrRpt ON DlvDlvrRpt.DeliverableID = DlvDlvr.Parent_DeliverableID
		LEFT JOIN TBL_ListItem LstItm ON LstItm.LISTITEMID = DlvDlvr.DeliverableCategoryListID
		WHERE @CategoryID = (
				CASE 
					WHEN isnull(@CategoryID, 0) = 0
						THEN 0
					ELSE DlvDlvr.DeliverableCategoryListID
					END
				)
			AND DlvDlvr.DeliverableTypeID IN (
				SELECT DeliverableTypeID
				FROM TBL_DLV_DeliverableType
				WHERE DeliverableTypeName IN ('Email List')
				)
		ORDER BY LstItm.LISTITEMNAME
			,DlvDlvrRpt.DeliverableName
	END
END
GO

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetDeliverable'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_GetDeliverable';
END