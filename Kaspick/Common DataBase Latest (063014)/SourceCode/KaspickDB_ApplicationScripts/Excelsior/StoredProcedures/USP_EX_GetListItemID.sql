IF EXISTS (SELECT
	*
FROM sysobjects
WHERE type = 'P'
AND name = 'USP_EX_GetListItemID') 
BEGIN
DROP PROCEDURE USP_EX_GetListItemID;
PRINT 'DROPPED USP_EX_GetListItemID';
END
GO     

/*********************************************************************************************************************                                         
* Procedure Name : USP_EX_GetListItemID
* Old Procedure Name  : USP_EIS_EX_LIST_ITEM_ID_SelProc
* Description     : To retrieve List_Item_Id from table TBL_EIS_LIST_ITEM 
* Input           : @LIST_TYPE_NAME             
*                 : @LIST_ITEM_NAME
* Output		  : @LIST_ITEM_ID
*          
* Modification Log:                                             
*                            
* Date			Modified By			Description                                            
*--------------------------------------------------------------------------------------------------------------------                                         
* 31-Oct-06	    Saravanan P M		Created
* 20-Mar-14     Sanath   EXCREQ5.4 Modified      
*********************************************************************************************************************/          
  

CREATE PROCEDURE [dbo].[USP_EX_GetListItemID] --'Manager Code View','My Manager Codes',0  
(     
 @LIST_TYPE_NAME VARCHAR(100),    
 @LIST_ITEM_NAME VARCHAR(100),    
 @LIST_ITEM_ID INT OUTPUT    
)    
AS    
BEGIN    
 SELECT  
  @LIST_ITEM_ID=LstItm.ListItemID    
 FROM     
  TBL_ListItem LstItm     
  INNER JOIN TBL_ListType LstTyp ON LstTyp.ListTypeID = LstItm.ListTypeID   
 WHERE     
  UPPER(LstTyp.ListTypeName) = UPPER(@LIST_TYPE_NAME )    
  AND UPPER(LstItm.ListItemName) = UPPER(@LIST_ITEM_NAME)    
 
END 

GO   
 IF EXISTS (SELECT
	*
FROM sysobjects
WHERE type = 'P'
AND name = 'USP_EX_GetListItemID') 
BEGIN

PRINT 'CREATED USP_EX_GetListItemID';
END
GO    