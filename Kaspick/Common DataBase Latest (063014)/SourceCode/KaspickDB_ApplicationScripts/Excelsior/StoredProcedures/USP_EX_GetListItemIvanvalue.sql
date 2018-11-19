
IF EXISTS (SELECT *
           FROM   sysobjects 
           WHERE  type = 'P'
                  AND name = 'USP_EX_GetListItemIvanvalue')
    BEGIN
        DROP PROCEDURE USP_EX_GetListItemIvanvalue;
        PRINT 'DROPPED USP_EX_GetListItemIvanvalue';
    END
GO


  
   
  
/******************************************************************************      
** New Name : USP_EX_GetListItemIvanvalue  
** Old Name:     USP_EIS_EX_LISTITEM_IVANVALUE_SelProc     
** Short Desc: Put in Short Description      
**      
** Full Description      
**              
**      
** Sample Call      
        EXEC USP_EX_GetListItemIvanvalue  1060 ,''     
**      
** Return values: List item Ivan Value    
**      
**      
** Standard declarations      
**       SET LOCK_TIMEOUT         30000   -- 30 seconds      
**       
** Created By: Saravanan PM    
** Company   : Kaspick & Company      
** Project   : Excelsior      
** Created DT: 05/21/2007      
**                  
*******************************************************************************      
**       Change History      
*******************************************************************************      
** Date:        Author:   Bug #     Description:                           Rvwd      
** --------     -------------------------------------------------------------- --------      
   05/21/2007 Saravanan PM              
** <mm/dd/yyyy>     
** 04/03/2014   Sanath       Save option in trading task for req INVREQ3.1   
* 22-MAY-2014 Mallikarjun EXCREQ 7.4    SP Name Renamed and Formatted  
*******************************************************************************      
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved      
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION      
*******************************************************************************/     
  
CREATE PROCEDURE [dbo].[USP_EX_GetListItemIvanvalue]    
(      
 @LIST_ITEM_ID INT,    
 @IVAN_VALUE VARCHAR(100) OUTPUT    
)      
AS         
BEGIN      
  SET @IVAN_VALUE =NULL    
  IF (@LIST_ITEM_ID <>0)      
   SELECT @IVAN_VALUE =IvanValue   
   FROM TBL_ListItem   
   WHERE ListItemID=@LIST_ITEM_ID    
END      
    
    

GO
  IF EXISTS (	SELECT *
			FROM sysobjects
			WHERE type = 'P'
			AND name = 'USP_EX_GetListItemIvanvalue') 
	BEGIN
			PRINT 'CREATED PROCEDURE USP_EX_GetListItemIvanvalue';
	END