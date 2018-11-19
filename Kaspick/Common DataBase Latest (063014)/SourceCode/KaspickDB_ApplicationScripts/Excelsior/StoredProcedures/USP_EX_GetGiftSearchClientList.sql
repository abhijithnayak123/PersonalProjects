IF EXISTS (SELECT *
           FROM   sysobjects 
           WHERE  type = 'P'
                  AND name = 'USP_EX_GetGiftSearchClientList')
    BEGIN
        DROP PROCEDURE USP_EX_GetGiftSearchClientList;
        PRINT 'DROPPED USP_EX_GetGiftSearchClientList';
    END
GO
  
  
/**************************************************************************    
* PROCEDURE NAME : USP_EX_GetGiftSearchClientList                
* OLD PROCEDURE NAME  : [USP_EIS_EX_Gift_SearchClientListSelProc]              
* DESCRIPTION     : RETRIEVE LIST OF CLIENT BRIEF NAMES FOR THE HOME-PAGE SEARCH                  
* INPUT PARAMETER :   @USER_ID INT    
*              
* MODIFICATION LOG                                                                 
*                                                
* DATE   MODIFIED  DESCRIPTION                
*----------------------------------------------------------------------------------------          
* 28-Feb-07  Chirag Parekh Created    
* 11-Apr-07     Saravanan PM Modified && Added Distinct Operator for getting unique client id    
* 18-May-07  Saravanan PM  Modifed && Removed Delete_flag     
* 13-Mar-2014 Mallikarjun  EXCREQ5.4 Modified  
* 23-MAY-2014 Mallikarjun EXCREQ 5.4    SP Name Renamed and Formatted  
*****************************************************************************************/    
CREATE PROCEDURE USP_EX_GetGiftSearchClientList  --999999  
(@USER_ID INT)    
AS    
BEGIN    
 IF(@USER_ID IS NULL)    
  SELECT ManagerCode As CLIENTID,ManagerCode As BRIEFNAME -- earlier it was clientid,briefname  
  FROM  dbo.SYN_IT_AccountManagerCodes  
  ORDER BY BRIEFNAME    
 ELSE    
SELECT DISTINCT AccMgrCds.ManagerCode  As CLIENTID, AccMgrCds.ManagerCode As BRIEFNAME
		FROM SYN_IT_ContactMaster ConMstr
		INNER JOIN SYN_IT_AccountManagerCodes AccMgrCds ON ConMstr.ContactID = AccMgrCds.ContactID
		INNER JOIN SYN_IT_SubContactRoles subConRol ON subConRol.ContactID = ConMstr.ContactID
		INNER JOIN SYN_IT_ContactRoleCodes ConRolCds ON ConRolCds.Id = subConRol.ContactRoleCode
		INNER JOIN SYN_IT_ContactMaster KCoStfConMstr ON KCoStfConMstr.contactID = subConRol.subcontactID
		INNER JOIN TBL_KS_User KsUsr ON KsUsr.InnotrustContactID = KCoStfConMstr.contactID
		WHERE KsUsr.USERID = @User_Id   
		 ORDER BY BRIEFNAME 
END    

GO
  IF EXISTS (	SELECT *
			FROM sysobjects
			WHERE type = 'P'
			AND name = 'USP_EX_GetGiftSearchClientList') 
	BEGIN
			PRINT 'CREATED PROCEDURE USP_EX_GetGiftSearchClientList';
	END  