IF EXISTS (SELECT *
           FROM   sysobjects 
           WHERE  type = 'P'
                  AND name = 'USP_EX_GetListItem')
    BEGIN
        DROP PROCEDURE USP_EX_GetListItem;
        PRINT 'DROPPED USP_EX_GetListItem';
    END
GO
  
/*********************************************************************************************************************                                                   
* Procedure Name : USP_EX_GetListItem  
* Old Procedure Name  : USP_EIS_EX_LIST_ITEM_DDLSelProc           
* Description   : To retrieve the list items to populate the dropdown list.          
* Input           : @LIST_TYPE_NAME : List type name for what the list is to be fetched.          
*                                      
* Modification log: Indentation                     
     If condition removed;                                           
*                                      
* Date   Modified BY  Description                                                      
*--------------------------------------------------------------------------------------------------------------------                                                   
* 14-Jul-06  Chirag P  Created          
* 30-Oct-06  Saravanan P M  Modified         
* 31-Oct-06  Manjiri C  Modified       
* 27-Apr-07  Saravanan PM   Modified && Changed Order by condition       
*10-Jul-07   Vshivhare modified for one more if condition for State Situs field    
*11-Dec-2007 Saravanan PM Modified && Country list Order by List Item Name    
*15-FEB-2008 ASHWATH   MODIFIED && ET7131 (Issues Type Order by List Item Name)    
* 20-Feb-14  Sanath   EXCREQ 6.1  
* 22-MAY-2014 Mallikarjun EXCREQ  6.1  SP Name Renamed and Formatted  
*********************************************************************************************************************/          
  
  
CREATE PROCEDURE [dbo].[USP_EX_GetListItem] --'Submit Financial Statement'  
(              
 @LIST_TYPE_NAME VARCHAR(100)            
)              
AS              
BEGIN  
            
        
 IF(LOWER(@LIST_TYPE_NAME) in('salutation','country','Issue Type'))       
 BEGIN  
SELECT  
 ListItemID DDVALUE,  
 ListItemName DDTEXT  
FROM TBL_ListItem I  
INNER JOIN TBL_ListType T  
 ON I.ListTypeID = T.ListTypeID  
WHERE LOWER(ListTypeName) = LOWER(@LIST_TYPE_NAME)  
ORDER BY ListItemName  
END ELSE IF (LOWER(@LIST_TYPE_NAME) = 'state situs') BEGIN  
SELECT  
 ListItemID DDVALUE,  
 IvanValue DDTEXT  
FROM TBL_ListItem I  
INNER JOIN TBL_ListType T  
 ON I.ListTypeID = T.ListTypeID  
WHERE LOWER(ListTypeName) = LOWER(@LIST_TYPE_NAME)  
ORDER BY IvanValue  
END ELSE IF (LOWER(@LIST_TYPE_NAME) = 'AccountTypeNew') BEGIN  
SELECT  
 ListItemID DDVALUE,  
 IvanValue DDTEXT  
FROM TBL_ListItem I  
INNER JOIN TBL_ListType T  
 ON I.ListTypeID = T.ListTypeID  
WHERE LOWER(ListTypeName) = LOWER('Account Type')  
AND I.CustomFlag = 0  
ORDER BY IvanValue  
END   
ELSE IF (LOWER(@LIST_TYPE_NAME) = 'AccountTypeCustomFlag')   
BEGIN  
SELECT  
 ListItemID DDVALUE,  
 CONVERT(varchar, CustomFlag, 20) AS DDTEXT  
FROM TBL_ListItem I  
INNER JOIN TBL_ListType T  
 ON I.ListTypeID = T.ListTypeID  
WHERE LOWER(ListTypeName) = LOWER('Account Type')  
ORDER BY IvanValue  
END   
ELSE IF (LOWER(@LIST_TYPE_NAME) = 'Client View')   
 BEGIN  
   
  SELECT ListItemID DDVALUE, ListItemName DDTEXT  
  FROM TBL_ListItem I  
  INNER JOIN TBL_ListType T  
   ON I.ListTypeID = T.ListTypeID  
  WHERE LOWER(ListTypeName) = LOWER(@LIST_TYPE_NAME) 
  AND LOWER(ListItemName) <> 'All Active Clients' 
  AND LOWER(ListItemName) <> 'All Inactive Clients' 
   AND LOWER(ListItemName) <> 'My Back-ups' 
   AND LOWER(ListItemName) <> 'My Inactive Clients' 
   
  ORDER BY DisplaySequence  
 END    
ELSE IF (LOWER(@LIST_TYPE_NAME) = 'Web Request Manager Code View')     
 BEGIN    
   
  SELECT ListItemID DDVALUE, ListItemName DDTEXT    
  FROM TBL_ListItem I    
  INNER JOIN TBL_ListType T    
   ON I.ListTypeID = T.ListTypeID    
  WHERE LOWER(ListTypeName) = LOWER(@LIST_TYPE_NAME)   
  ORDER BY DisplaySequence    
 END     
ELSE IF (LOWER(@LIST_TYPE_NAME) = 'Dono Bene Account Group')   
 BEGIN  
   
  SELECT ListItemID DDVALUE, ListItemName DDTEXT  
  FROM TBL_ListItem I  
  INNER JOIN TBL_ListType T  
   ON I.ListTypeID = T.ListTypeID  
  WHERE LOWER(ListTypeName) = 'Account View' AND   
  LOWER(ListItemName) <> 'All Active Accounts' AND  
  LOWER(ListItemName) <> 'All Inactive Accounts' AND  
  LOWER(ListItemName) <> 'My Back-ups' AND  
  LOWER(ListItemName) <> 'My Inactive Accounts' 
  ORDER BY DisplaySequence  
 END     
ELSE IF (LOWER(@LIST_TYPE_NAME) = 'Account View')   
 BEGIN  
   
  SELECT ListItemID DDVALUE, ListItemName DDTEXT  
  FROM TBL_ListItem I  
  INNER JOIN TBL_ListType T  
   ON I.ListTypeID = T.ListTypeID  
  WHERE LOWER(ListTypeName) = LOWER(@LIST_TYPE_NAME) AND   
  LOWER(ListItemName) <> 'My Back-ups' AND  
  LOWER(ListItemName) <> 'All Inactive Accounts' AND
  LOWER(ListItemName) <> 'All Active Accounts'
  
    
  ORDER BY DisplaySequence  
 END   
ELSE IF (LOWER(@LIST_TYPE_NAME) = 'Web Request Account List')   
 BEGIN  
   
  SELECT AccountTypeCode DDVALUE, AccountTypeCode DDTEXT  
  FROM SYN_IT_AccountTypeCodes  
      
 END    
   
ELSE IF (LOWER(@LIST_TYPE_NAME) = 'WFT Flow Status') or (LOWER(@LIST_TYPE_NAME) = 'RequestType')  
 BEGIN      
       
  SELECT ListItemID DDVALUE, ListItemName DDTEXT      
  FROM TBL_ListItem I      
  INNER JOIN TBL_ListType T      
   ON I.ListTypeID = T.ListTypeID      
  WHERE LOWER(ListTypeName) = LOWER(@LIST_TYPE_NAME)   
 END        
  ELSE IF (LOWER(@LIST_TYPE_NAME) = 'WFT Request view') or  (LOWER(@LIST_TYPE_NAME) = 'GetRequestFlowStatus')   
 BEGIN      
       
  SELECT ListItemID DDVALUE, ListItemName DDTEXT      
  FROM TBL_ListItem I      
  INNER JOIN TBL_ListType T      
   ON I.ListTypeID = T.ListTypeID      
  WHERE LOWER(ListTypeName) = LOWER(@LIST_TYPE_NAME)   
 END   
   
 ELSE IF (LOWER(@LIST_TYPE_NAME) = 'Trading Tasks Status')   
 BEGIN      
       
  SELECT ListItemID DDVALUE, ListItemName DDTEXT      
  FROM TBL_ListItem I      
  INNER JOIN TBL_ListType T      
   ON I.ListTypeID = T.ListTypeID      
  WHERE LOWER(ListTypeName) = LOWER(@LIST_TYPE_NAME)   
 END   
   
 ELSE IF (LOWER(@LIST_TYPE_NAME) = 'Category')   
 BEGIN      
       
  SELECT ListItemID DDVALUE, ListItemName DDTEXT      
  FROM TBL_ListItem I      
  INNER JOIN TBL_ListType T      
   ON I.ListTypeID = T.ListTypeID      
  WHERE LOWER(ListTypeName) = LOWER(@LIST_TYPE_NAME)   
 END   
   
  ELSE IF (LOWER(@LIST_TYPE_NAME) = 'Check')   
 BEGIN      
       
  SELECT ListItemID DDVALUE, ListItemName DDTEXT      
  FROM TBL_ListItem I      
  INNER JOIN TBL_ListType T      
   ON I.ListTypeID = T.ListTypeID      
  WHERE LOWER(ListTypeName) = LOWER(@LIST_TYPE_NAME)   
 END   
    
  ELSE IF (LOWER(@LIST_TYPE_NAME) = 'Trade')   
 BEGIN      
       
  SELECT ListItemID DDVALUE, ListItemName DDTEXT      
  FROM TBL_ListItem I      
  INNER JOIN TBL_ListType T      
   ON I.ListTypeID = T.ListTypeID      
  WHERE LOWER(ListTypeName) = LOWER(@LIST_TYPE_NAME)   
 END   
   
 ELSE IF (LOWER(@LIST_TYPE_NAME) = 'State')   
 BEGIN      
       
  SELECT ListItemID DDVALUE, ListItemName DDTEXT      
  FROM TBL_ListItem I      
  INNER JOIN TBL_ListType T      
   ON I.ListTypeID = T.ListTypeID      
  WHERE LOWER(ListTypeName) = LOWER(@LIST_TYPE_NAME)   
 END   
   
 ELSE IF (LOWER(@LIST_TYPE_NAME) = 'CGA TYPE')   
 BEGIN      
       
  SELECT ListItemID DDVALUE, ListItemName DDTEXT      
  FROM TBL_ListItem I      
  INNER JOIN TBL_ListType T      
   ON I.ListTypeID = T.ListTypeID      
  WHERE LOWER(ListTypeName) = LOWER(@LIST_TYPE_NAME)   
 END   
   
  ELSE IF (LOWER(@LIST_TYPE_NAME) = 'CGA Reserve Type')   
 BEGIN      
       
  SELECT ListItemID DDVALUE, ListItemName DDTEXT      
  FROM TBL_ListItem I      
  INNER JOIN TBL_ListType T      
   ON I.ListTypeID = T.ListTypeID      
  WHERE LOWER(ListTypeName) = LOWER(@LIST_TYPE_NAME)   
 END   
    
  ELSE IF (LOWER(@LIST_TYPE_NAME) = 'Business Year')   
 BEGIN      
       
  SELECT ListItemID DDVALUE, ListItemName DDTEXT      
  FROM TBL_ListItem I      
  INNER JOIN TBL_ListType T      
   ON I.ListTypeID = T.ListTypeID      
  WHERE LOWER(ListTypeName) = LOWER(@LIST_TYPE_NAME)   
 END   
    
    
   ELSE IF (LOWER(@LIST_TYPE_NAME) = 'Submit Financial Statement')   
 BEGIN      
       
  SELECT ListItemID DDVALUE, ListItemName DDTEXT      
  FROM TBL_ListItem I      
  INNER JOIN TBL_ListType T      
   ON I.ListTypeID = T.ListTypeID      
  WHERE LOWER(ListTypeName) = LOWER(@LIST_TYPE_NAME)   
 END   
   
 ELSE IF (LOWER(@LIST_TYPE_NAME) = 'Trust Participant View')   
 BEGIN      
       
  SELECT ListItemID DDVALUE, ListItemName DDTEXT      
  FROM TBL_ListItem I      
  INNER JOIN TBL_ListType T      
   ON I.ListTypeID = T.ListTypeID      
  WHERE LOWER(ListTypeName) = LOWER(@LIST_TYPE_NAME) AND   
  LOWER(ListItemName) <> 'Transition' AND  
  LOWER(ListItemName) <> 'Active and Transition'   
  ORDER BY DisplaySequence  
 END   
   
   
   
   
      
ELSE  
BEGIN  
SELECT  
 ListItemID DDVALUE,  
 ListItemName DDTEXT  
FROM TBL_ListItem I  
INNER JOIN TBL_ListType T  
 ON I.ListTypeID = T.ListTypeID  
WHERE LOWER(ListTypeName) = LOWER(@LIST_TYPE_NAME)  
ORDER BY DisplaySequence  
END  
  
END  


GO
  IF EXISTS (	SELECT *
			FROM sysobjects
			WHERE type = 'P'
			AND name = 'USP_EX_GetListItem') 
	BEGIN
			PRINT 'CREATED PROCEDURE USP_EX_GetListItem';
	END  