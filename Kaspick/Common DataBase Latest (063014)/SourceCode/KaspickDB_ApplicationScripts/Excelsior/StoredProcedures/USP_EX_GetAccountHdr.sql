IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetAccountHdr'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetAccountHdr;

	PRINT 'DROPPED USP_EX_GetAccountHdr';
END
GO

SET QUOTED_IDENTIFIER ON
GO

/*********************************************************************************************************************                                                       
* New Procedure Name : USP_EX_GetAccountHdr
* Procedure Name : USP_EIS_EX_ACCOUNT_HDRSelProc              
* Description    : To retrieve ACCOUNT,CLIENT,PROGRAM Header Details.              
* Input          : @ACCOUNTID: Account Id, of which the details should be fetched.  
Sample call
  USP_EX_GetAccountHdr 'Test account',100260,'Test account','account'          
* Modification Log                                                           
*                                          
* Date   Modified By  Description                                                          
*--------------------------------------------------------------------------------------------------------------------                                                       
* 05-Dec-06  Tanuj   Created              
* 27-Dec-06  Saravanan      Modified        
* 21-May-07  Ganapati  Removed the delete flag      
* July-03-2007 Vshivhare modified for fetching Account Status
* 19-Feb-08		Saravanan PM	Added Entity Name Column To fix ET 7001 (Windows Title Name)
* 18-Jun-09		Saravanan PM	Added Spigot trust field in Select query (KCBR- Excelsior Enhancement - Profile Report Setting)
* 07-Apr-2014   Abhijith   EXCREQ 7.4 Modified 
* 23-May-2014  Sanath   Sp name renamed as per Kaspick naming convention standard 
*********************************************************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_GetAccountHdr] (
	@ACCOUNTID VARCHAR(14)
	,@USERID INT = NULL
	,@ENTITY_ID VARCHAR(50) = NULL
	,@ENTITY_TYPE VARCHAR(50) = NULL
	)
AS
BEGIN
	DECLARE @DATATYPE VARCHAR(50)
		,@VALUE VARCHAR(100)
		,@POLICYDIMENSIONID INT
		,@NEW_CONTR_PERMITTED_ID INT
		,@AC_STATUS VARCHAR(100)

	SELECT @AC_STATUS = ISNULL(ACCOUNT_STATUS, '')
	FROM VW_EX_Account
	WHERE ACCOUNTID = @ACCOUNTID

	DECLARE @ENTITY_NAME VARCHAR(150)

	SET @ENTITY_NAME = ''

	IF (
			@ENTITY_TYPE = 'TRUSTEE'
			AND @ENTITY_ID IS NOT NULL
			)
	BEGIN
		SELECT @ENTITY_NAME = ContactName
		FROM dbo.SYN_IT_ContactMaster AS ConMstr
		INNER JOIN dbo.SYN_IT_ContactAccountRoles AS ConAccRol ON ConAccRol.ContactId = ConMstr.ContactId
		INNER JOIN dbo.SYN_IT_ContactRoleCodes AS ConRolCds ON ConRolCds.Id = ConAccRol.ContactRoleCode
			AND ConRolCds.ID IN (16)
		INNER JOIN dbo.SYN_IT_AccountMaster AS AccMstr ON AccMstr.CustomerAccountNumber = ConAccRol.CustomerAccountNumber
			AND AccMstr.CustomerAccountNumber = @ENTITY_ID
	END

	IF (
			@ENTITY_TYPE = 'REMAINDERMAN'
			AND @ENTITY_ID IS NOT NULL
			)
	BEGIN
		SELECT @ENTITY_NAME = ContactName
		FROM dbo.SYN_IT_ContactMaster AS ConMstr
		INNER JOIN dbo.SYN_IT_ContactAccountRoles AS ConAccRol ON ConAccRol.ContactId = ConMstr.ContactId
		INNER JOIN dbo.SYN_IT_ContactRoleCodes AS ConRolCds ON ConRolCds.Id = ConAccRol.ContactRoleCode
			AND ConRolCds.ID IN (22)
		INNER JOIN dbo.SYN_IT_AccountMaster AS AccMstr ON AccMstr.CustomerAccountNumber = ConAccRol.CustomerAccountNumber
			AND AccMstr.CustomerAccountNumber = @ENTITY_ID
	END

	IF (@USERID IS NULL)
	BEGIN
		SELECT AccMstr.CustomerAccountNumber AS ACCOUNTID
			,ISNULL(AccMstr.CustomerAccountNumber, '') AC_ADVENTID
			,ISNULL(AccMstr.TrustAccountNumber, '') AC_BRIEFNAME
			,ISNULL(AccMstr.CustomerDescriptionLine1, '') AC_FULLNAME
			,AC_ACCOUNTTYPE = DBO.FN_GetListItemName(AccMstr.AccountTypeCode, 'Account Type')
			,AccMgrCds.ManagerCode AS CLIENTID
			,ISNULL(AccMgrCds.ManagerCode, '') CL_BRIEFNAME
			,ISNULL(ConMstrClient.ContactName, '') CL_FULLNAME
			,AlnsNmbr.AllianceNumber AS PROGRAMID
			,ISNULL(AlnsNmbr.AllianceNumber, '') PROG_BRIEFNAME
			,ISNULL(ConMstrPrg.ContactName, '') PROG_FULLNAME
			,@AC_STATUS AS AC_STATUS
			,ISNULL(@ENTITY_NAME, '') AS ENTITY_NAME
			,UDFAMColumn039 AS SPIGOTACCOUNT -- Added by Saravanan P Muthu dt:18-Jun-09  
		FROM SYN_IT_AccountMaster AccMstr
		INNER JOIN dbo.SYN_IT_AllianceNumbers AS AlnsNmbr ON AlnsNmbr.AllianceNumber = AccMstr.AllianceNumber
		INNER JOIN dbo.SYN_IT_AccountManagerCodes AS AccMgrCds ON AccMstr.ManagerCode = AccMgrCds.ManagerCode
		INNER JOIN dbo.SYN_IT_UDF_AccountMaster AS UdfAccMstr ON UdfAccMstr.CustomerAccountNumber_Key = AccMstr.CustomerAccountNumber
		INNER JOIN dbo.SYN_IT_ContactMaster AS ConMstrClient ON AccMgrCds.ContactId = ConMstrClient.ContactId
		INNER JOIN dbo.SYN_IT_ContactMaster AS ConMstrPrg  ON AlnsNmbr.ContactId = ConMstrPrg.ContactId
		WHERE AccMstr.CustomerAccountNumber = @ACCOUNTID
	END
	ELSE
	BEGIN
		SELECT AccMstr.CustomerAccountNumber AS ACCOUNTID
			,ISNULL(AccMstr.CustomerAccountNumber, '') AC_ADVENTID
			,ISNULL(AccMstr.TrustAccountNumber, '') AC_BRIEFNAME
			,ISNULL(AccMstr.CustomerDescriptionLine1, '') AC_FULLNAME
			,AC_ACCOUNTTYPE = DBO.FN_GetListItemName(AccMstr.AccountTypeCode, 'Account Type')
			,AccMgrCds.ManagerCode AS CLIENTID
			,ISNULL(AccMgrCds.ManagerCode, '') CL_BRIEFNAME
			,ISNULL(ConMstrClient.ContactName, '') CL_FULLNAME
			,AlnsNmbr.AllianceNumber AS PROGRAMID
			,ISNULL(AlnsNmbr.AllianceNumber, '') PROG_BRIEFNAME
			,ISNULL(ConMstrPrg.ContactName, '') PROG_FULLNAME
			,ASSOCIATION = CASE 
				WHEN (
						SELECT TOP 1 AccMgrCds.ManagerCode
						FROM SYN_IT_ContactMaster ConMstr
						INNER JOIN SYN_IT_AccountManagerCodes AccMgrCds ON ConMstr.ContactID = AccMgrCds.ContactID
						INNER JOIN SYN_IT_SubContactRoles subConRol ON subConRol.ContactID = ConMstr.ContactID
						INNER JOIN SYN_IT_ContactRoleCodes ConRolCds ON ConRolCds.Id = subConRol.ContactRoleCode
						INNER JOIN SYN_IT_ContactMaster KCoStfConMstr ON KCoStfConMstr.contactID = subConRol.subcontactID
						INNER JOIN TBL_KS_User KsUsr ON KsUsr.InnotrustContactID = KCoStfConMstr.contactID
						WHERE subConRol.ContactRoleCode IN (2) --,3,518,26,34,512,519,515,510
							AND KsUsr.USERID = @USERID
						) IS NULL
					THEN 0
				ELSE 1
				END
			,NEWCONTRIBUTIONS = (
				CASE 
					WHEN AccPrfl.AllowNewGift = 1
						THEN 'Yes'
					ELSE 'No'
					END
				)
			,@AC_STATUS AS AC_STATUS
			,ISNULL(@ENTITY_NAME, '') AS ENTITY_NAME
			,CASE UDFAMColumn039
				WHEN 'Y'
					THEN 1
				WHEN 'N'
					THEN 0
				END AS SPIGOTACCOUNT -- Added by Saravanan P Muthu dt:18-Jun-09     
		FROM SYN_IT_AccountMaster AccMstr
		INNER JOIN dbo.SYN_IT_AllianceNumbers AS AlnsNmbr ON AlnsNmbr.AllianceNumber = AccMstr.AllianceNumber
		--INNER JOIN dbo.SYN_IT_AccountManagerCodes AS AccMgrCds ON AccMgrCds.ManagerCode = AlnsNmbr.AllianceNumber
		INNER JOIN dbo.SYN_IT_AccountManagerCodes AS AccMgrCds ON AccMgrCds.ManagerCode=AccMstr.ManagerCode
		--INNER JOIN dbo.SYN_IT_AllianceNumbers AS AlnsNmbr ON AlnsNmbr.AllianceNumber=AccMst.AllianceNumber
		
		INNER JOIN dbo.SYN_IT_UDF_AccountMaster AS UdfAccMstr ON UdfAccMstr.CustomerAccountNumber_Key = AccMstr.CustomerAccountNumber
	    LEFT JOIN dbo.TBL_INV_AccountProfile AS AccPrfl ON AccPrfl.CustomerAccountNumber = AccMstr.CustomerAccountNumber
		INNER JOIN dbo.SYN_IT_ContactMaster AS ConMstrClient ON AccMgrCds.ContactId = ConMstrClient.ContactId
		INNER JOIN dbo.SYN_IT_ContactMaster AS ConMstrPrg ON AlnsNmbr.ContactId = ConMstrPrg.ContactId
		WHERE AccMstr.CustomerAccountNumber = @ACCOUNTID
	END
END
GO

SET NOCOUNT OFF;

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetAccountHdr'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_GetAccountHdr';
END