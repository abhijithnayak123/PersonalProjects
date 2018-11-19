IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetClientMailToClientTeam'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetClientMailToClientTeam;

	PRINT 'DROPPED USP_EX_GetClientMailToClientTeam';
END
GO

/**************************************************************************            
* New PROCEDURE NAME  : USP_EX_GetClientMailToClientTeam
* Old PROCEDURE NAME  : USP_EIS_EX_CLIENT_MailToClientTeam      
* DESCRIPTION     : RETRIEVE LIST OF CLIENTS ACCORDING TO THE CONDITION SPECIFIED            
* INPUT PARAMETER : @LOGIN_NAME, @CONDITION      
*      
* MODIFICATION LOG                                                         
*                                        
* DATE			MODIFIED			DESCRIPTION        
*----------------------------------------------------------------------------------------  
* 27-May-09		Abhishek			CREATED for SP09-446 
* 21-Mar-2014   Abhijith			EXCREQ 5.1
* 23-May-2014   Sanath               Sp name renamed as per Kaspick naming convention standard 
*****************************************************************************************/
CREATE PROCEDURE [dbo].USP_EX_GetClientMailToClientTeam (@CLIENTID VARCHAR(15))
AS
BEGIN
	SELECT AccMgrCds.ManagerCode
		,KsUsr.USERID
		,LTRIM(RTRIM(LOGINNAME)) + '@kaspick.com' AS EMAIL
		,
		--Commneted By Abhijith this has to be changed when IsPrimary is confirmed.
		--CASE WHEN SFROLE.IsPrimary = 1 THEN 'PRIMARY'  
		--  ELSE 'BACKUP'  
		--END AS RELATIONSHIP,
		'Please change this' AS RELATIONSHIP
		,KsUsr.USERID
	FROM TBL_KS_User AS KsUsr
	INNER JOIN SYN_IT_ContactMaster AS ConMstr ON ConMstr.ContactID = KsUsr.InnotrustContactID
	INNER JOIN SYN_IT_AccountManagerCodes AS AccMgrCds ON AccMgrCds.ManagerCode = ConMstr.ManagerCode
	--  INNER JOIN  TBL_KS_User AS KsUsr ON KsUsr.USERID = SFROLE.UserID   
	WHERE AccMgrCds.ManagerCode = @CLIENTID
		AND KsUsr.IsActive = 1
END
GO

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetClientMailToClientTeam'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_GetClientMailToClientTeam';
END