
IF EXISTS (SELECT *
           FROM   sysobjects 
           WHERE  type = 'P'
                  AND name = 'USP_EX_GetListItemName')
    BEGIN
        DROP PROCEDURE USP_EX_GetListItemName;
        PRINT 'DROPPED USP_EX_GetListItemName';
    END
GO




  
/*********************************************************************************************************************                                               
* New Procedure Name : USP_EX_GetListItemName  
* Old Procedure Name  : USP_EIS_EX_LIST_ITEM_NAME_SelProc  
* Description   : To retrieve the list item name      
* Input           : @LIST_TYPE_ID : List type ID.      
*                                  
* Modification log                                                   
*                                  
* Date   Modified BY Description                                                  
*--------------------------------------------------------------------------------------------------------------------                                               
* 29-Dec-06  Vshivhare  Created      
* 20-Mar-2014 Abhijith      EXCREQ 5.4 Modified  
* 22-MAY-2014 Mallikarjun EXCREQ     SP Name Renamed and Formatted  
*********************************************************************************************************************/      
  
  
       
CREATE PROCEDURE [dbo].USP_EX_GetListItemName       
(          
 @LIST_ITEM_ID INT,    
 @LIST_ITEM_NAME varchar(100) output        
)          
AS          
        
BEGIN          
 SELECT @LIST_ITEM_NAME= ListItemName           
 FROM TBL_ListItem           
 --INNER JOIN TBL_EIS_LIST_TYPE T ON I.LIST_TYPE_ID = T.LIST_TYPE_ID          
 WHERE         
 ListItemID=@LIST_ITEM_ID    
END   



GO
  IF EXISTS (	SELECT *
			FROM sysobjects
			WHERE type = 'P'
			AND name = 'USP_EX_GetListItemName') 
	BEGIN
			PRINT 'CREATED PROCEDURE USP_EX_GetListItemName';
	END 