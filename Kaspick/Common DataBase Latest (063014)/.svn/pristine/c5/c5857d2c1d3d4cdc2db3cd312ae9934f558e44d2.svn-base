IF EXISTS (SELECT *
           FROM   sysobjects 
           WHERE  type = 'P'
                  AND name = 'USP_EX_GetListItemAbbrevDDL')
    BEGIN
        DROP PROCEDURE USP_EX_GetListItemAbbrevDDL;
        PRINT 'DROPPED USP_EX_GetListItemAbbrevDDL';
    END
GO
    
      
/*********************************************************************************************************************                                             
* Procedure Name: USP_EX_GetListItemAbbrevDDL  
* Old Procedure Name  : USP_EIS_EX_LIST_ITEM_ABBREV_DDLSelProc    
* Description   : To retrieve the Abbrevations of list items to populate the dropdown list.    
* Input           : @LIST_TYPE_NAME : List type name for what the list is to be fetched.    
*                                
* Modification log                                                 
*                                
* Date   Modified BY  Description                                                
*--------------------------------------------------------------------------------------------------------------------                                             
* 24-Oct-06  Venugopal B  Created   
* 3-Apr-2014 Yugandhar EXCREQ7.4 Modified   
* 23-MAY-2014 Mallikarjun EXCREQ 7.4    SP Name Renamed and Formatted  
*********************************************************************************************************************/    
    
CREATE PROCEDURE USP_EX_GetListItemAbbrevDDL     
(      
 @LIST_TYPE_NAME VARCHAR(50)    
)      
AS      
    
BEGIN      
    
SELECT       
    LISTITEMID DDVALUE,      
    ABBREV DDTEXT      
    FROM       
    TBL_ListItem LstItm     
    INNER JOIN TBL_ListType LstTyp ON LstItm.LISTTYPEID = LstTyp.LISTTYPEID      
    WHERE     
    LOWER(LISTTYPENAME) = LOWER(@LIST_TYPE_NAME)      
    ORDER BY     
    ABBREV    
    
END     
    
    GO
  IF EXISTS (	SELECT *
			FROM sysobjects
			WHERE type = 'P'
			AND name = 'USP_EX_GetListItemAbbrevDDL') 
	BEGIN
			PRINT 'CREATED PROCEDURE USP_EX_GetListItemAbbrevDDL';
	END