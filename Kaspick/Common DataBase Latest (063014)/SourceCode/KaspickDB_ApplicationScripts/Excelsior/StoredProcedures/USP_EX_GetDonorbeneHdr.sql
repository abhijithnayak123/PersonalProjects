IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetDonorbeneHdr'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetDonorbeneHdr;

	PRINT 'DROPPED PROCEDURE USP_EX_GetDonorbeneHdr';
END
GO

/******************************************************************************              
** New Name:     USP_EX_GetDonorbeneHdr
** Old Name:     USP_EIS_EX_DONORBENE_HDRSelProc               
** Short Desc: Put in Short Description              
**              
** Full Description              
**        More detailed description if necessary              
**              
** Sample Call              
  EXEC USP_EIS_EX_DONORBENE_HDRSelProc   
   @TRUSTPARTICIPANT_TYPE_ID= 85214,  
   @USERID = 69,  
   @ENTITYID = null,  
   @ENTITYTYPE = null  
**              
** Return values: NONE              
**              
**              
** Standard declarations              
**       SET NOCOUNT             ON              
**       SET LOCK_TIMEOUT         30000   -- 30 seconds              
**               
** Created By: <Authorname>              
** Company   : Kaspick & Company              
** Project   : Excelsior              
** Created DT: 03/09/2007              
**                          
*******************************************************************************              
**       Change History              
*******************************************************************************              
** Date:        Author:  Bug #     Description:                           Rvwd              
** --------     -------- ------    -------------------------------------- --------              
** 12 Mar 08 Saravanan PM Modified && Added Tax Year Validation Flag   
** 02 Apr 08 Saravanan PM Modified && Added Account Type Condition to check Tax Year Validation Flag  
** 14 Apr 08 Saravanan PM Added First k1 tax year is not null then only Last k1 tax year is mandatory #5187.
** 19 Apr 14 Abhijith changed for Donor EXCRE #8.4 requirement.
** 22-May-2014  Sanath   Sp name renamed as per Kaspick naming convention standard 

EXEC USP_EX_GetDonorbeneHdr 29582,100023

*******************************************************************************                  
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved                  
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION                  
*******************************************************************************/
CREATE PROCEDURE USP_EX_GetDonorbeneHdr (
	@TRUSTPARTICIPANT_TYPE_ID INT
	,@TRUSTPARTICIPANTID INT
	,@ACCOUNTID CHAR(14)
	,@USERID INT
	,@ENTITYID INT = NULL
	,@ENTITYTYPE VARCHAR(50) = NULL
	)
AS
BEGIN
	DECLARE @ENTITYNAME VARCHAR(150)
		,@TAXYEARWARRNING INT

	SET @ENTITYNAME = ''
	SET @TAXYEARWARRNING = 0

	IF (
			@ENTITYTYPE = 'TRUSTADVISOR'
			AND @ENTITYID IS NOT NULL
			)
	BEGIN
		SELECT @ENTITYNAME = (
				PrimaryLastName + (
					CASE 
						WHEN PrimaryFirstName IS NULL
							OR PrimaryFirstName = ''
							THEN ''
						ELSE (' , ' + PrimaryFirstName)
						END
					) + (
					CASE 
						WHEN PrimaryMiddleInitial IS NULL
							THEN ''
						ELSE (' ' + PrimaryMiddleInitial)
						END
					)
				)
		FROM SYN_IT_ContactMaster ConMstr
		INNER JOIN SYN_IT_SubContactRoles subConRol ON subConRol.ContactId = ConMstr.ContactID
		INNER JOIN SYN_IT_ContactRoleCodes ConRolCds ON subConRol.ContactRoleCode = ConRolCds.ID
		WHERE ConRolCds.ID IN (8)
	END

	IF EXISTS (
			SELECT UDFCAColumn013
			FROM SYN_IT_ContactRoleCodes ConRolCds
			INNER JOIN SYN_IT_ContactAccountRoles ConAccRol ON ConRolCds.ID = ConAccRol.ContactRoleCode
				AND ConRolCds.ID IN (
					21
					,37
					,40
					,41
					,43
					)
			INNER JOIN SYN_IT_ContactMaster ConMstr ON ConAccRol.ContactID = ConMstr.ContactID
			INNER JOIN SYN_IT_UDF_ContactAccountRole UdfConAccRol ON UdfConAccRol.ContactID_Key = ConMstr.ContactID
			INNER JOIN VW_EX_TRUSTPARTICIPANTTYPE TrustPar ON UdfConAccRol.CONTACTID_KEY = TrustPar.ParticipantID
			WHERE TrustPar.ParticipantID = @TRUSTPARTICIPANTID
				AND TrustPar.TRUSTPARTICIPANT_TYPE_ID = @TRUSTPARTICIPANT_TYPE_ID
				AND TrustPar.CustomerAccountNumber = @ACCOUNTID
				AND UdfConAccRol.UDFCAColumn012 IS NOT NULL
				AND (
					UdfConAccRol.UDFCAColumn013 IS NULL
					OR UdfConAccRol.UDFCAColumn013 = 0
					)
				AND ConMstr.DateOfDeath IS NOT NULL
				AND TrustPar.AccountType <> 'CGA'
			)
	BEGIN
		SET @TAXYEARWARRNING = 1
	END

	SELECT PARTICIPANTID
		,PARTICIPANTNAME
		,PROGRAMID
		,TrustPar.CustomerAccountNumber AS ADVENTID
		,PARTICIPANT_TYPE
		,DONORID
		,BENEFICIARYID
		,ACCOUNT_FULLNAME
		,PROGRAM_BRIEFNAME
		,PROGRAM_FULLNAME
		,CLIENT_BRIEFNAME
		,CLIENT_FULLNAME
		,ACCOUNTTYPE
		,TrustPar.CustomerAccountNumber AS ACCOUNTID
		,CLIENTID
		,ACCOUNT_FULLNAME
		,(
			CASE 
				WHEN (
						SELECT TOP 1 acntMgr.ManagerCode
						FROM TBL_KS_User usr
						INNER JOIN SYN_IT_AccountManagerCodes acntMgr ON usr.InnotrustContactID = acntMgr.ContactID
						WHERE acntMgr.ManagerCode = TrustPar.CLIENTID
							AND usr.USERID = @USERID
						) IS NULL
					THEN 0
				ELSE 1
				END
			) AS ASSOCIATION
		,FIRSTNAME
		,LASTNAME
		,@ENTITYNAME AS ENTITY
		,@TAXYEARWARRNING AS TAXYEARWARRNING
	FROM VW_EX_TRUSTPARTICIPANTTYPE TrustPar
	WHERE TrustPar.CustomerAccountNumber = @ACCOUNTID
		AND ParticipantID = @TRUSTPARTICIPANTID
		AND TRUSTPARTICIPANT_TYPE_ID = @TRUSTPARTICIPANT_TYPE_ID
END
GO

SET NOCOUNT OFF;

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetDonorbeneHdr'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_GetDonorbeneHdr';
END