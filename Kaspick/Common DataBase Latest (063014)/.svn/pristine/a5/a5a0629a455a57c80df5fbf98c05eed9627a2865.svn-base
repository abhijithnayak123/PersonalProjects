IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetDeliverableMonth'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetDeliverableMonth;

	PRINT 'DROPPED USP_EX_GetDeliverableMonth';
END
GO

SET QUOTED_IDENTIFIER ON
GO

/******************************************************************************                      
** New Name :    USP_EX_GetDeliverableMonth
** Old Name:     USP_EIS_DLV_GetMonth_SelProc                      
** Short Desc: To retrieve the month for Client module                     
**                      
** Full Description                      
**        To retrieve the month for Client module for reports screen                     
**                      
** Sample Call                      
+**                      
** Return values: NONE                      
           USP_EIS_DLV_GetMonth_SelProc      1 
                  
**                      
** Standard declarations                      
**       SET NOCOUNT             ON                      
**       SET LOCK_TIMEOUT         30000   -- 30 seconds                      
**                       
** Created By: Tanuj Gupta                      
** Company   : Kaspick & Company                      
** Project   : Excelsior --                     
** Created DT: 04/11/2011                      
**                                  
*******************************************************************************                      
**       Change History                      
*******************************************************************************                      
** Date:        Author:  Bug #     Description:                           Rvwd                      
** --------     -------- ------    -------------------------------------- --------                      
** 04-Mar-2014  Mallikarjun        EXCREQ 5.4 
** 22-May-2014  Sanath             Sp name renamed as per Kaspick naming convention standard 
*******************************************************************************                      
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved                      
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION                      
*******************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_GetDeliverableMonth] (@ListTypeName VARCHAR(75))
AS
BEGIN
	SELECT LstItm.DisplaySequence
		,LstItm.ListItemName
	FROM TBL_ListType LstTyp
	INNER JOIN TBL_ListItem LstItm ON LstItm.ListTypeID = LstTyp.ListTypeID
	WHERE LstTyp.ListTypeName = @ListTypeName
	ORDER BY LstItm.DisplaySequence
END
GO

SET NOCOUNT OFF;

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetDeliverableMonth'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_GetDeliverableMonth';
END