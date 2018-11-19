IF EXISTS (SELECT *
           FROM   sysobjects 
           WHERE  type = 'P'
                  AND name = 'USP_EX_GetGlobalContactPagingLov')
    BEGIN
        DROP PROCEDURE USP_EX_GetGlobalContactPagingLov;
        PRINT 'DROPPED USP_EX_GetGlobalContactPagingLov';
    END
GO
  
/*********************************************************************************************************************      
* Procedure Name: USP_EX_GetGlobalContactPagingLov  
* Old Procedure Name  : USP_EIS_EX_GLOBAL_CONTACT_PAGING_LOVSelProc    
* Description     : To retrieve record for TAX Screen LOOKUP    
* Input Parameter : @LOOK_FOR - Search string    
*     @CLIENT_ID - Client for whom the employee contacts to be returned    
*     @START_ROW - Start row number from where the recordset should start    
*     @END_ROW - End row number till where the records to be return    
*     @SORT_COLUMN_NAME - Sort column on which sorting to be performed    
*     @SORT_ORDER - Sort order [ASC/DESC]    
*    
* Modification Log    
*    
* Date   Modified By  Description    
*--------------------------------------------------------------------------------------------------------------------    
* 05-Oct-07  Chirag P  Created to fix the CR#5837  
* 15-Apr-2014 Yugandhar EXCREQ 7.4    
* 22-MAY-2014 Mallikarjun EXCREQ 7.4    SP Name Renamed and Formatted  
*-------------------------------------------------------------------------------------------------------------------*/  
CREATE PROCEDURE USP_EX_GetGlobalContactPagingLov -- '','mit',1,14,'Vanetta Birnie','asc'   
 (  
 @LOOK_FOR VARCHAR(100)  
 ,@CLIENT_ID VARCHAR(15)  
 ,@START_ROW INT  
 ,@END_ROW INT  
 ,@SORT_COLUMN_NAME VARCHAR(100)  
 ,@SORT_ORDER VARCHAR(4)  
 )  
AS  
BEGIN  
 BEGIN TRY  
  --BEGIN TRANSACTION  
  
  DECLARE @TOTALPAGECOUNT INT  
  DECLARE @CONTACTLIST TABLE (  
   PARTY_ID INT  
   ,PARTY_NAME VARCHAR(120)  
   )  
  
  IF (ISNULL(@LOOK_FOR, '') != '')  
  BEGIN  
   INSERT INTO @CONTACTLIST  
   SELECT subConRol.SubContactID AS PARTY_ID  
    ,ISNULL(EmpConMstr.PrimaryFirstName, '') + ' ' + ISNULL(EmpConMstr.PrimaryMiddleInitial, '') + ' ' + ISNULL(EmpConMstr.PrimaryLastName, '') AS CONTACT  
   FROM SYN_IT_SubContactRoles subConRol  
   INNER JOIN SYN_IT_ContactMaster EmpConMstr ON subConRol.SubContactID = EmpConMstr.ContactID  
   INNER JOIN SYN_IT_ContactRoleCodes ConRolCds ON subConRol.ContactRoleCode = ConRolCds.ID  
   INNER JOIN SYN_IT_ContactMaster ConMstr ON subConRol.ContactID = ConMstr.ContactID  
   INNER JOIN SYN_IT_AccountManagerCodes AccMgrCds ON AccMgrCds.CONTACTID = ConMstr.CONTACTID  
   WHERE ConRolCds.ID IN (  
     550 
     ,8
     )  
    AND ConRolCds.ID NOT IN (19)  
    AND EmpConMstr.ManagerCode = @CLIENT_ID  
    AND (  
     EmpConMstr.PrimaryFirstName LIKE @LOOK_FOR + '%'  
     OR EmpConMstr.PrimaryLastName LIKE @LOOK_FOR + '%'  
     )  
  END  
  ELSE  
  BEGIN  
   INSERT INTO @CONTACTLIST  
   SELECT subConRol.SubContactID AS PARTY_ID  
    ,ISNULL(EmpConMstr.PrimaryFirstName, '') + ' ' + ISNULL(EmpConMstr.PrimaryMiddleInitial, '') + ' ' + ISNULL(EmpConMstr.PrimaryLastName, '') AS CONTACT  
   FROM SYN_IT_SubContactRoles subConRol  
   INNER JOIN SYN_IT_ContactMaster EmpConMstr ON subConRol.SubContactID = EmpConMstr.ContactID  
   INNER JOIN SYN_IT_ContactRoleCodes ConRolCds ON subConRol.ContactRoleCode = ConRolCds.ID  
   INNER JOIN SYN_IT_ContactMaster ConMstr ON subConRol.ContactID = ConMstr.ContactID  
   INNER JOIN SYN_IT_AccountManagerCodes AccMgrCds ON AccMgrCds.CONTACTID = ConMstr.CONTACTID  
   WHERE ConRolCds.ID IN (  
     550  
     ,8
     )  
    AND ConRolCds.ID NOT IN (19)  
    AND EmpConMstr.ManagerCode = @CLIENT_ID  
  END  
  
  SELECT @TOTALPAGECOUNT = COUNT(*)  
  FROM @CONTACTLIST  
  
  IF @SORT_COLUMN_NAME = 'CONTACT NAME'  
  BEGIN  
   IF @SORT_ORDER = 'ASC'  
   BEGIN  
    SELECT *  
    FROM (  
     SELECT PARTY_ID  
      ,PARTY_NAME  
      ,ROW_NUMBER() OVER (  
       ORDER BY PARTY_NAME ASC  
       ) AS ROWNUMBER  
      ,ISNULL(@TOTALPAGECOUNT, 0) AS [TOTALCOUNT]  
     FROM @CONTACTLIST  
     ) AS UNIVERSE  
    WHERE ROWNUMBER >= @START_ROW  
     AND ROWNUMBER <= @END_ROW  
    ORDER BY PARTY_NAME ASC  
   END  
   ELSE  
   BEGIN  
    SELECT *  
    FROM (  
     SELECT PARTY_ID  
      ,PARTY_NAME  
      ,ROW_NUMBER() OVER (  
       ORDER BY PARTY_NAME DESC  
       ) AS ROWNUMBER  
      ,ISNULL(@TOTALPAGECOUNT, 0) AS [TOTALCOUNT]  
     FROM @CONTACTLIST  
     ) AS UNIVERSE  
    WHERE ROWNUMBER >= @START_ROW  
     AND ROWNUMBER <= @END_ROW  
    ORDER BY PARTY_NAME DESC  
   END  
  END  
  
  --COMMIT TRANSACTION  
 END TRY  
  
 BEGIN CATCH  
  --ROLLBACK TRANSACTION  
  
  DECLARE @procname VARCHAR(60);  
  DECLARE @ErrorMessage NVARCHAR(4000);  
  DECLARE @ErrorSeverity INT;  
  DECLARE @ErrorState INT;  
  
  SET @procname = 'USP_EX_GetGlobalContactPagingLov';  
  
  DECLARE @ErrorNumber INT;  
  
  SELECT @ErrorMessage = ERROR_MESSAGE()  
   ,@ErrorSeverity = ERROR_SEVERITY()  
   ,@ErrorState = ERROR_STATE()  
   ,@ErrorNumber = ERROR_NUMBER();  
  
  EXEC dbo.USP_EX_SYSErrorHandler @codename = @procname  
   ,@ErrorMessage = @ErrorMessage  
   ,@ErrorNumber = @ErrorNumber  
   ,@val1 = ''  
   ,@val1str = 'USP_EX_GetGlobalContactPagingLov: Cannot Select.'  
   ,@val2 = ''  
   ,@val2str = '';  
  
 END CATCH  
END  

GO
  IF EXISTS (	SELECT *
			FROM sysobjects
			WHERE type = 'P'
			AND name = 'USP_EX_GetGlobalContactPagingLov') 
	BEGIN
			PRINT 'CREATED PROCEDURE USP_EX_GetGlobalContactPagingLov';
	END  