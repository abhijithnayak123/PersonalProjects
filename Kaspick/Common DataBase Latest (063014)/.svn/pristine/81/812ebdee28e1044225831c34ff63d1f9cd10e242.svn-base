IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetClientHdr'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetClientHdr;

	PRINT 'DROPPED USP_EX_GetClientHdr';
END
GO

/*********************************************************************************************************************                                                     
* New Procedure Name : USP_EX_GetClientHdr
* Old Procedure Name : USP_EIS_EX_CLIENT_HDRSelProc             
* Description    : To retrieve Client Header Details.            
* Input          : @CLIENTID: Client Id, of which the details should be fetched.            
* Modification Log                                                         
*                                        
* Date   Modified By  Description                                                        
*--------------------------------------------------------------------------------------------------------------------                                                     
* 08-Jan-07  Saravanan PM Created            
* 07-Mar-07  Geetha Priya Modified     
* July-03-2007 Vshivhare modified for fetching Account Status     
* 19-Feb-08  Saravanan PM Added Entity Name Column To fix ET 7001 (Windows Title Name)  
* 03-Mar-2014  Mallikarjun  EXCREQ 5.4 Modified
** 22-May-2014  Sanath   Sp name renamed as per Kaspick naming convention standard 
*********************************************************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_GetClientHdr] --'ACL',100260,NULL,NULL 
	(
	@ManagerCode VARCHAR(15)
	,@USER_ID INT
	,@ENTITY_ID INT = NULL
	,@ENTITY_TYPE VARCHAR(50) = NULL
	)
AS
BEGIN
	DECLARE @STATUS VARCHAR(100)
		,@STATUS_ID INT

	SELECT @STATUS_ID = ConMstr.ActiveFlag
	FROM SYN_IT_ContactMaster ConMstr
	WHERE ConMstr.ManagerCode = @ManagerCode

	DECLARE @ENTITY_NAME VARCHAR(150)

	SET @ENTITY_NAME = ''

	IF (
			@ENTITY_TYPE = 'CLIENTEMPLOYEE'
			AND @ENTITY_ID IS NOT NULL
			)
	BEGIN
		SELECT @ENTITY_NAME = (
				ConMstrEmp.PrimaryLastName + (
					CASE 
						WHEN ConMstrEmp.PrimaryFirstName IS NULL
							OR ConMstrEmp.PrimaryFirstName = ''
							THEN ''
						ELSE (' , ' + ConMstrEmp.PrimaryFirstName)
						END
					) + (
					CASE 
						WHEN ConMstrEmp.PrimaryMiddleInitial IS NULL
							THEN ''
						ELSE (' ' + ConMstrEmp.PrimaryMiddleInitial)
						END
					)
				)
		FROM SYN_IT_SubContactRoles subConRol
		INNER JOIN SYN_IT_ContactMaster ConMstrEmp ON subConRol.SubContactID = ConMstrEmp.ContactID
		INNER JOIN SYN_IT_ContactRoleCodes ConRolCds ON subConRol.ContactRoleCode = ConRolCds.ID
		INNER JOIN SYN_IT_AccountManagerCodes AccMgrCds ON AccMgrCds.CONTACTID = ConMstrEmp.CONTACTID
		WHERE ConRolCds.ID IN (550)
			AND subConRol.SubContactID = @ENTITY_ID
	END

	SELECT ConMstr.ManagerCode
		,ISNULL(ConMstr.ManagerCode, '') CL_BRIEFNAME
		,ISNULL(ConMstr.ContactName, '') CL_FULLNAME
		,ASSOCIATION = CASE 
			WHEN (
					SELECT TOP 1 AccMgrCds.ManagerCode
					FROM SYN_IT_ContactMaster ConMstr
					INNER JOIN SYN_IT_AccountManagerCodes AccMgrCds ON ConMstr.ContactID = AccMgrCds.ContactID
					INNER JOIN SYN_IT_SubContactRoles subConRol ON subConRol.ContactID = ConMstr.ContactID
					INNER JOIN SYN_IT_ContactRoleCodes ConRolCds ON ConRolCds.Id = subConRol.ContactRoleCode
					INNER JOIN SYN_IT_ContactMaster KcoStfConMstr ON KcoStfConMstr.contactID = subConRol.subcontactID
					INNER JOIN TBL_KS_User KsUsr ON KsUsr.InnotrustContactID = KcoStfConMstr.contactID
					WHERE subConRol.ContactRoleCode IN (2) --,3,518,26,34,512,519,515,510 -- 2 is Administrator
						AND KsUsr.USERID = @User_Id
					) IS NULL
				THEN 0
			ELSE 1
			END
		,ISNULL(@STATUS, '') AS CL_STATUS
		,ISNULL(@ENTITY_NAME, '') AS ENTITY_NAME
	FROM SYN_IT_ContactMaster ConMstr
	INNER JOIN SYN_IT_AccountManagerCodes AccMgrCodes ON AccMgrCodes.ContactId = ConMstr.ContactId
	WHERE AccMgrCodes.ManagerCode = @ManagerCode
END
GO

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetClientHdr'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_GetClientHdr';
END