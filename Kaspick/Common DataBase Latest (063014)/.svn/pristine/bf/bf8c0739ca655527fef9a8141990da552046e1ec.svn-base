IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetDonorbene'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetDonorbene;

	PRINT 'DROPPED USP_EX_GetDonorbene';
END
GO

/*********************************************************************************************************************                                                                     
* PROCEDURE NAME  :  USP_EX_GetDonorbene
* PROCEDURE NAME  :  USP_EIS_EX_DONORBENE_DTLSELPROC                     
* DESCRIPTION     : TO RETRIEVE RECORD FOR PARTICULAR CLIENT                            
* INPUT           :                       
*                   Exec  USP_EIS_EX_DONORBENE_DTLSELPROC     
      @TRUSTPARITCIPANT_TYPE_ID= 35582,        
      @USERID = 300009                                
* MODIFICATION LOG                                                                         
*                                                        
* DATE   MODIFIED BY  DESCRIPTION                                                                        
*--------------------------------------------------------------------------------------------------------------------                                                                     
* 13-FEB-07  VSHIVHARE  CREATED                         
* 14-FEB-07  VSHIVAHRE  MODIFIED            
* 17-Apr-07  Vshivhare  Modified for using donor id beneficiary id in where condition                         
* 02-May-07  Vshivhare  Modified for changed Donor Beneficiary Types       
* 08-Jun-07  Vshivhare Modified for domicile code inner join                       
* 08-Aug-07  Vshivhare  modified for Proxy Bene Id    
* 23-Nov-07  Vshivhare modified for CR#6536 columns added in Beneficiary supp.table and removed from trustparticipant supp.      
* 03-Dec-09  Tanuj  Added NRA_ID for Beneficiary Detail  
* 15-Dec-09  Tanuj  Added PROXY_REASON_ID for Beneficiary Detail
* 21-Apr-14  Yugandhar EXCREQ8.4 Modified  
* 22-May-2014  Sanath   Sp name renamed as per Kaspick naming convention standard 

EXEC USP_EX_GetDonorbene  21,1545,'CBSTE3',100023

*********************************************************************************************************************/
CREATE PROCEDURE USP_EX_GetDonorbene (
	@TRUSTPARITCIPANT_TYPE_ID INT = NULL
	,@PARITCIPANTID INT = NULL
	,@ACCOUNTID CHAR(14) = NULL
	,@USERID INT = NULL
	)
AS
BEGIN
	DECLARE @ENTITY_TYPE_ID INT
		,@ENABLE_CLIENTBENEFICIARY_ID INT
		,@CLBENE_TYPE VARCHAR(20)
		,@ENABLE_CLBENE BIT
		,@CLIENTBENEFICIARY_ID VARCHAR(8)
		,@DONORID INT
		,@BENEFICIARYID INT
		,@PARTICIPANTID INT
		,@TYPE_NAME VARCHAR(50)
		,@PROXYBENENAME VARCHAR(100)
		,@SPOUSENAME VARCHAR(100)
		,@SALUTATION_ID INT
		,@SALUTATION_NAME VARCHAR(100)
		,@PARTICIPANT_TYPE_ID INT
		,@ACCOUNT_STATUS INT
	DECLARE @BENE_COUNT INT
		,@TAXREPORTING_VAL VARCHAR(100)

	SELECT @BENE_COUNT = COUNT(TRUSTPARTICIPANT_TYPE_ID)
	FROM VW_EX_TRUSTPARTICIPANTTYPE
	WHERE CustomerAccountNumber = @ACCOUNTID
		AND ParticipantID = @PARITCIPANTID
		AND TRUSTPARTICIPANT_TYPE_ID = @TRUSTPARITCIPANT_TYPE_ID
		AND PARTICIPANT_TYPE IN (
			'Beneficiary'
			,'Contingent Beneficiary'
			,'Donor'
			) --and TRUSTPARTICIPANT_TYPE_ID<>@TRUSTPARITCIPANT_TYPE_ID       

	SELECT @DONORID = DONORID
		,@BENEFICIARYID = BENEFICIARYID
		,@PARTICIPANTID = PARTICIPANTID
		,@TYPE_NAME = PARTICIPANT_TYPE
		,@PARTICIPANT_TYPE_ID = PARTICIPANT_TYPE_ID
		,@ACCOUNT_STATUS = Account_Status
	FROM VW_EX_TRUSTPARTICIPANTTYPE
	WHERE TRUSTPARTICIPANT_TYPE_ID = @TRUSTPARITCIPANT_TYPE_ID
		AND CustomerAccountNumber = @ACCOUNTID
		AND ParticipantID = @PARITCIPANTID

	SELECT @TAXREPORTING_VAL = LISTITEMNAME
	FROM VW_EX_ListItem
	WHERE LISTTYPENAME = 'TAX REPORTING'

	EXEC USP_EX_GetListItemID @LIST_TYPE_NAME = 'ENTITY'
		,@LIST_ITEM_NAME = 'Contact'
		,@LIST_ITEM_ID = @ENTITY_TYPE_ID OUTPUT

	SET @ENABLE_CLBENE = 0

	SELECT @SALUTATION_NAME = ConMstr.PrimaryPrefix
	FROM SYN_IT_ContactMaster ConMstr
	INNER JOIN VW_EX_TRUSTPARTICIPANTTYPE TrustPar ON TrustPar.ParticipantID = ConMstr.ContactID
	WHERE TrustPar.TRUSTPARTICIPANT_TYPE_ID = @TRUSTPARITCIPANT_TYPE_ID
		AND TrustPar.CustomerAccountNumber = @ACCOUNTID
		AND TrustPar.ParticipantID = @PARITCIPANTID

	SET @SALUTATION_ID = dbo.FN_GetListItemID(@SALUTATION_NAME, 'Salutation')

	SELECT @PROXYBENENAME = (
			SELECT TOP 1 PARTICIPANTNAME + ' (' + CAST(TrustPar.BENEFICIARYID AS VARCHAR) + ')'
			FROM VW_EX_TRUSTPARTICIPANTTYPE TrustPar
			WHERE TrustPar.BENEFICIARYID = (
					SELECT TOP 1 bd.PayeeID 
					from SYN_IT_ContactAccountRoles car
					JOIN SYN_IT_RemittanceInstructions ri ON car.CustomerAccountNumber = ri.CustomerAccountNumber
					JOIN SYN_IT_BeneficiaryDistributions bd ON ri.InstructionID = bd.InstructionID AND car.ContactID =bd.PayeeID 
					WHERE car.ContactRoleCode=10 AND ActiveFlag=-1
					AND ri.CustomerAccountNumber=@ACCOUNTID
					)
			)

	SELECT @SPOUSENAME = LTRIM(RTRIM((
					CASE 
						WHEN (ISNULL(LTRIM(RTRIM(ConMstr.PrimaryLastName)), '') = '')
							THEN ''
						ELSE (ConMstr.PrimaryLastName + ' ')
						END
					) + (
					CASE 
						WHEN (ISNULL(LTRIM(RTRIM(ConMstr.PrimaryFirstName)), '') = '')
							THEN ''
						ELSE (ConMstr.PrimaryFirstName + ' ')
						END
					) + ISNULL(ConMstr.PrimaryMiddleInitial, ''))) + ' (' + CAST(ConMstr.ContactID AS VARCHAR) + ')'
	FROM SYN_IT_ContactMaster ConMstr
	WHERE ConMstr.ContactID = (
			SELECT DISTINCT SPOUSE_ID
			FROM VW_EX_TRUSTPARTICIPANTTYPE
			WHERE PARTICIPANTID = @PARTICIPANTID
				AND TRUSTPARTICIPANT_TYPE_ID = @TRUSTPARITCIPANT_TYPE_ID
				AND CustomerAccountNumber = @ACCOUNTID
			)

	--FETCH REQ LATER VALUES FOR DOB                  
	DECLARE @REQUIRED_LATER_ID_DOB INT
	DECLARE @REQUIRED_LATER_DATE_DOB DATETIME
	DECLARE @REQUIRED_LATER_ID_TAXID INT
	DECLARE @REQUIRED_LATER_DATE_TAXID DATETIME

	IF (
			@TYPE_NAME = 'BENEFICIARY'
			OR @TYPE_NAME = 'Contingent Beneficiary'
			)
	BEGIN
		--FETCH DETAILS FROM BENE TABLE                            
		SELECT TrustPar.PARTICIPANTID
			,ConMstr.PrimaryGENDER
			,TrustPar.SPOUSE_ID
			,@SPOUSENAME AS SPOUSENAME
			,NULL AS TAX_REPORTING_ID
			,NULL AS PARTICIPANT_HIERARCHY_ID
			,NULL AS RIGHT_TO_REVOKE_ID
			,TrustPar.PROXY_STATUS_ID
			,TrustPar.PARTICIPANTID
			,SALUTATION = dbo.FN_GetListItemName(ConMstr.PrimaryPrefix, 'Salutation')
			,ConMstr.PrimaryFIRSTNAME
			,ConMstr.PrimaryLASTNAME
			,ConMstr.PrimaryMIDDLEINITIAL
			,ConMstr.TAXNAMELine1
			,ConMstr.DateofBIRTH
			,ConMstr.DATEofDeath
			,TrustPar.TAXID
			,TAXIDTYPE = dbo.FN_GetListItemName(TrustPar.TAXIDTYPE, 'Tax ID Type')
			,NULL DONORID
			,TrustPar.CustomerAccountNumber
			,TrustPar.BENEFICIARYID
			,TrustPar.PROGRAMID
			,NULL AS EXCLUDEFROMLIFEOFTRUST
			,@SALUTATION_ID AS SALUTATIONID
			,TrustPar.PROGRAM_FULLNAME
			,0 AS PROXYBENEID
			,@PROXYBENENAME AS PROXYBENENAME
			,@PARTICIPANT_TYPE_ID AS PARTICIPANT_TYPE_ID
			,NULL AS DOBBELLDATE
			,@ENABLE_CLBENE AS ENABLE_CLBENE
			,UdfConAccRol.UDFCAColumn011
			,VLstItm.LISTITEMNAME AS DOMICILECODE
			,VLstItm.LISTITEMID AS DOMICILE_ID
			,
			--Additional fields            
			TrustPar.PARTICIPANTNAME
			,TrustPar.ACCOUNT_FULLNAME
			,TrustPar.PROGRAM_BRIEFNAME
			,TrustPar.CLIENT_BRIEFNAME
			,TrustPar.CLIENT_FULLNAME
			,TrustPar.ACCOUNTTYPE
			,TrustPar.ACCOUNT_FULLNAME
			,TrustPar.CLIENTID
			,0 AS ASSOCIATION
			,TrustPar.CustomerAccountNumber
			,@ACCOUNT_STATUS AS ACCOUNTSTATUS
			,ISNULL(@BENE_COUNT, 0) AS BENE_COUNT
			,@TAXREPORTING_VAL AS TAXREPORTING_VAL
			,NULL AS NRA_ID
			,NULL AS TAXIDBELLDATE
			,NULL AS PROXY_REASON_ID
		FROM SYN_IT_ContactMaster ConMstr
		INNER JOIN SYN_IT_UDF_ContactAccountRole UdfConAccRol ON ConMstr.ContactID = UdfConAccRol.ContactID_Key
		INNER JOIN VW_EX_TRUSTPARTICIPANTTYPE TrustPar ON TrustPar.ParticipantID = ConMstr.ContactID
			AND TrustPar.TRUSTPARTICIPANT_TYPE_ID = @TRUSTPARITCIPANT_TYPE_ID
		LEFT JOIN VW_EX_ListItem VLstItm ON VLstItm.IVANVALUE = ConMstr.DOMICILEStateCode
			AND VLstItm.ListTypeName IN (
				'Domicile Country'
				,'State'
				)
		WHERE TrustPar.BENEFICIARYID = @BENEFICIARYID
			AND TrustPar.PARTICIPANTID = @PARTICIPANTID
	END
	ELSE IF (@TYPE_NAME = 'DONOR')
	BEGIN
		--FETCH DETAILS FROM DONOR TABLE                            
		SELECT TrustPar.PARTICIPANTID
			,ConMstr.PrimaryGENDER
			,TrustPar.SPOUSE_ID
			,@SPOUSENAME AS SPOUSENAME
			,NULL AS TAX_REPORTING_ID
			,NULL AS PARTICIPANT_HIERARCHY_ID
			,NULL AS RIGHT_TO_REVOKE_ID
			,TrustPar.PROXY_STATUS_ID
			,TrustPar.PARTICIPANTID
			,SALUTATION = DBO.FN_GetListItemName(ConMstr.PrimaryPrefix, 'Salutation')
			,ConMstr.PrimaryFIRSTNAME
			,ConMstr.PrimaryLASTNAME
			,ConMstr.PrimaryMIDDLEINITIAL
			,ConMstr.TAXNAMELine1
			,ConMstr.DateofBIRTH
			,ConMstr.DATEofDeath
			,TrustPar.TAXID
			,TAXIDTYPE = dbo.FN_GetListItemName(TrustPar.TAXIDTYPE, 'Tax ID Type')
			,--(CASE WHEN    ConMstr.TAXIDTYPE=0 THEN 'SSN' when ConMstr.TAXIDTYPE=1 then 'EIN' WHEN ConMstr.TAXIDTYPE=2    THEN 'ALPHA' ELSE NULL END) ,                                  
			ConRolCds.ID AS DONORID
			,TrustPar.CustomerAccountNumber
			,NULL BENEFICIARYID
			,TrustPar.PROGRAMID
			,NULL EXCLUDEFROMLIFEOFTRUST
			,@SALUTATION_ID AS SALUTATIONID
			,TrustPar.PROGRAM_FULLNAME
			,0 AS PROXYBENEID
			,@PROXYBENENAME AS PROXYBENENAME
			,@PARTICIPANT_TYPE_ID AS PARTICIPANT_TYPE_ID
			,NULL AS DOBBELLDATE
			,@ENABLE_CLBENE AS ENABLE_CLBENE
			,UdfConAccRol.UDFCAColumn011
			,VLstItm.LISTITEMNAME AS DOMICILECODE
			,VLstItm.LISTITEMID AS DOMICILE_ID
			,
			--Additional fields            
			TrustPar.PARTICIPANTNAME
			,TrustPar.ACCOUNT_FULLNAME
			,TrustPar.PROGRAM_BRIEFNAME
			,TrustPar.CLIENT_BRIEFNAME
			,TrustPar.CLIENT_FULLNAME
			,TrustPar.ACCOUNTTYPE
			,TrustPar.ACCOUNT_FULLNAME
			,TrustPar.CLIENTID
			,0 AS ASSOCIATION
			,TrustPar.CustomerAccountNumber
			,@ACCOUNT_STATUS AS ACCOUNTSTATUS
			,ISNULL(@BENE_COUNT, 0) AS BENE_COUNT
			,@TAXREPORTING_VAL AS TAXREPORTING_VAL
			,NULL AS NRA_ID
			,NULL AS TAXIDBELLDATE
			,NULL AS PROXY_REASON_ID
		FROM SYN_IT_ContactMaster ConMstr
		INNER JOIN SYN_IT_UDF_ContactAccountRole UdfConAccRol ON ConMstr.ContactID = UdfConAccRol.ContactID_Key
		INNER JOIN VW_EX_TRUSTPARTICIPANTTYPE TrustPar ON TrustPar.ParticipantID = ConMstr.ContactID
			AND TrustPar.TRUSTPARTICIPANT_TYPE_ID = @TRUSTPARITCIPANT_TYPE_ID
		LEFT JOIN VW_EX_ListItem VLstItm ON VLstItm.IVANVALUE = ConMstr.DOMICILEStateCode
			AND VLstItm.ListTypeName IN (
				'Domicile Country'
				,'State'
				)
		INNER JOIN SYN_IT_ContactAccountRoles ConAccRol ON ConAccRol.ContactID = ConMstr.ContactID
		INNER JOIN SYN_IT_AccountMaster AccMstr ON AccMstr.CustomerAccountNumber = ConAccRol.CustomerAccountNumber
		INNER JOIN SYN_IT_ContactRoleCodes ConRolCds ON ConAccRol.ContactRoleCode = ConRolCds.ID
		INNER JOIN SYN_IT_AccountManagerCodes AccMgrCds ON AccMgrCds.ManagerCode = AccMstr.ManagerCode
		WHERE ConRolCds.ID IN (24)
			AND TrustPar.PARTICIPANTID = @PARTICIPANTID
	END
END
GO

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetDonorbene'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_GetDonorbene';
END