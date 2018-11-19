IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetDonorBenePagingMyAccounts'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetDonorBenePagingMyAccounts;

	PRINT 'DROPPED PROCEDURE USP_EX_GetDonorBenePagingMyAccounts';
END
GO

/******************************************************************************  
** Name : USP_EX_GetDonorBenePagingMyAccounts
** Old Name:     USP_EIS_EX_DONORBENE_PAGING_MyAccounts_SelProc  
** Short Desc: Lists Donor - Bene details for all accounts.  
**  
** Full Description  
**        Called from DONORBENE home page if view option selected in All Accounts.  
**  
** Sample Call  
   EXEC USP_EX_GetDonorBenePagingMyAccounts  
 @USER_ID =68,  
 @ModuleId  =0,  
 @ManagerCode ='',  
 @ProgramID =0,  
 @AccountID =0,  
 @startrow  =0,  
 @endrow    =19,  
 @SearchClientID   =0,  
 @SearchColumnName ='advent_id',  
 @SearchCriteria   =1,  
 @strSearch ='ad',  
 @Filtercolumnname ='',   
 @strFilter ='',    
  @sortcolumnname ='NAME',   
 @SortOrder ='DESC'  
**  
** Return values: NONE  
**  
**  
** Standard declarations  
**       SET NOCOUNT             ON  
**       SET LOCK_TIMEOUT         30000   -- 30 seconds  
**   
** Created By: Manjiri  
** Company   : Kaspick & Company  
** Project   : Excelsior  
** Created DT: 2/13/2007  
**              
*******************************************************************************  
**       Change History  
*******************************************************************************  
** Date:        Author:  Bug #     Description:                           Rvwd  
** --------     -------- ------    -------------------------------------- --------  
** 4/17/2007    Chirag   4788(CR) To filter on Last Name, and other CR  
**       4753(CR)   changes like adding more columns.  
** 6/19/2007 Ritu  4672  To set default sort order to Name instead  
         of Advent ID  
** 14-Mar-2014  Mallikarjun  EXCREQ 8.4 Modified
** 22-May-2014  Sanath   Sp name renamed as per Kaspick naming convention standard 
*******************************************************************************  
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved  
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION  
*******************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_GetDonorBenePagingMyAccounts]
	-- paremeters here  
	@USER_ID INT
	,-- logged in user id  
	@ModuleId INT
	,-- from which module page is called 1-Client, 2-Program, 3-Account  
	@ManagerCode VARCHAR(15)
	,-- when page is used inside client modules  
	@ProgramID CHAR(15)
	,-- when page is used inside program modules  
	@AccountID CHAR(14)
	,-- when page is used inside account modules  
	@startrow INT
	,-- start row for the page  
	@endrow INT
	,-- end row for the page  
	@SearchManagerCode CHAR(4)
	,-- when page is used inside other modules   
	@SearchColumnName VARCHAR(100)
	,-- this will be used when user wants to search a string within a column.  
	-- (Usually we send 'Advent_id' for this variable)  
	@SearchCriteria TINYINT
	,-- 0-Begins with, 1-Contains  
	@strSearch VARCHAR(255)
	,-- search string that will be located in column provided above.  
	@Filtercolumnname VARCHAR(100)
	,-- Column on which the default filtering will be done  
	@strFilter VARCHAR(255)
	,-- for filtering the char/varchar, which will work on filter control  
	@sortcolumnname VARCHAR(100)
	,-- column on which the sorting should work  
	@SortOrder VARCHAR(5) -- ASC or DESC   
AS
--  Initial Set statements  --  
SET NOCOUNT ON;
SET LOCK_TIMEOUT 30000;-- 30 seconds  
	--  Variable Declarations  --  

DECLARE @totalpagecount INT;


IF EXISTS (
		SELECT *
		FROM TEMPDB.DBO.SYSOBJECTS
		WHERE ID = OBJECT_ID(N'TEMPDB.[DBO].[#DONOR]')
		)
BEGIN
	DROP TABLE [DBO].[#DONOR];
END

--  Temp tables, Cursors, Table Variables  --  
CREATE TABLE [DBO].[#DONOR](
	TRUSTPARTICIPANT_TYPE_ID INT
	,ACCOUNT_ID CHAR(14)
	,ADVENT_ID CHAR(14)
	,PARTICIPANT_ID INT
	,DONOR_ID INT
	,BENEFICIARY_ID INT
	,DONOR_NAME VARCHAR(100)
	,FIRSTNAME VARCHAR(20)
	,LASTNAME VARCHAR(50)
	,ACCOUNT_STATUS VARCHAR(100)
	,DONOR_TYPE VARCHAR(100)
	,BIRTHDATE DATETIME
	,EXPIREDATE DATETIME
	,TaxIdType VARCHAR(4)
	,SSN VARCHAR(20)
	,MANAGERCODE CHAR(4)
	,PROGRAMID CHAR(15)
	)

	INSERT INTO [DBO].[#DONOR]
	SELECT 
		ConAccRol.ContactRoleCode
		,ConAccRol.CustomerAccountNumber
		,ConAccRol.CustomerAccountNumber
		,DonConMstr.ContactId
		,DonConMstr.ContactId
		,0
		,DonConMstr.ContactName
		,DonConMstr.PrimaryFirstName
		,DonConMstr.PrimaryLastName
		,CASE DonConMstr.ActiveFlag
			WHEN -1
				THEN 'Active'
			WHEN 0
				THEN 'Inactive'
			END AS ACCOUNT_STATUS
		,CASE 
			WHEN ISNULL(DonConMstr.DateOfDeath,'') = '' 
			THEN 'Active Donor'
			ELSE 'Inactive Donor'
		 END 
		 --ConRolCds.ContactRoleCodeDesc
		,DonConMstr.DateOfBirth
		,DonConMstr.DateOfDeath
		,DonConMstr.SSNFlag
		,DonConMstr.SSN
		,AccMgrCds.ManagerCode
		,AlnsNmbr.AllianceNumber
	FROM SYN_IT_ContactMaster DonConMstr
		INNER JOIN  SYN_IT_ContactAccountRoles AS ConAccRol ON DonConMstr.ContactID = ConAccRol.ContactID
		INNER JOIN SYN_IT_ContactRoleCodes AS ConRolCds ON ConRolCds.ID = ConAccRol.ContactRoleCode
		INNER JOIN SYN_IT_AccountMaster AS AccMstr ON AccMstr.CustomerAccountNumber = ConAccRol.CustomerAccountNumber
		INNER JOIN SYN_IT_AllianceNumbers AS AlnsNmbr ON AlnsNmbr.AllianceNumber = AccMstr.AllianceNumber
		INNER JOIN SYN_IT_AccountManagerCodes AS AccMgrCds ON AccMstr.ManagerCode = AccMgrCds.ManagerCode  
	WHERE ConRolCds.ID = 24 

	IF EXISTS (
			SELECT *
			FROM TEMPDB.DBO.SYSOBJECTS
			WHERE ID = OBJECT_ID(N'TEMPDB.[DBO].[#BENEFICIARIES]')
			)
	BEGIN
		DROP TABLE [DBO].[#BENEFICIARIES];
	END

	--  Temp tables, Cursors, Table Variables  --  
  CREATE TABLE [DBO].[#BENEFICIARIES](
	TRUSTPARTICIPANT_TYPE_ID INT
	,ACCOUNT_ID CHAR(14)
	,ADVENT_ID CHAR(14)
	,PARTICIPANT_ID INT
	,DONOR_ID INT
	,BENEFICIARY_ID INT
	,DONOR_NAME VARCHAR(100)
	,FIRSTNAME VARCHAR(20)
	,LASTNAME VARCHAR(50)
	,ACCOUNT_STATUS VARCHAR(100)
	,DONOR_TYPE VARCHAR(100)
	,BIRTHDATE DATETIME
	,EXPIREDATE DATETIME
	,TaxIdType VARCHAR(4)
	,SSN VARCHAR(20)
	,MANAGERCODE CHAR(4)
	,PROGRAMID CHAR(15)
	)

	INSERT INTO [DBO].[#BENEFICIARIES]
	SELECT 
		ConAccRol.ContactRoleCode
		,ConAccRol.CustomerAccountNumber
		,ConAccRol.CustomerAccountNumber
		,BenConMstr.ContactId
		,0
		,BenConMstr.ContactId
		,BenConMstr.ContactName
		,BenConMstr.PrimaryFirstName
		,BenConMstr.PrimaryLastName
		,CASE BenConMstr.ActiveFlag
			WHEN -1
				THEN 'Active'
			WHEN 0
				THEN 'Inactive'
			END AS ACCOUNT_STATUS
		,CASE
			WHEN ConRolCds.ID = 21 AND
				(
					SELECT TOP 1 ActiveFlag
					FROM SYN_IT_ContactAccountRoles car
						JOIN SYN_IT_RemittanceInstructions ri ON car.CustomerAccountNumber = ri.CustomerAccountNumber
						JOIN SYN_IT_BeneficiaryDistributions bd ON ri.InstructionID = bd.InstructionID AND car.ContactID =bd.PayeeID 
					WHERE car.ContactRoleCode=21 
					AND ri.CustomerAccountNumber=AccMstr.CustomerAccountNumber

				) = -1 AND ISNULL(BenConMstr.DateOfDeath,'') = ''
			THEN
				'Active Beneficiary'
			WHEN ConRolCds.ID = 21 AND
				((
					SELECT ActiveFlag
					FROM SYN_IT_ContactAccountRoles car
						JOIN SYN_IT_RemittanceInstructions ri ON car.CustomerAccountNumber = ri.CustomerAccountNumber
						JOIN SYN_IT_BeneficiaryDistributions bd ON ri.InstructionID = bd.InstructionID AND car.ContactID =bd.PayeeID 
					WHERE car.ContactRoleCode=21 
					AND ri.CustomerAccountNumber=AccMstr.CustomerAccountNumber

				) <> -1 OR ISNULL(BenConMstr.DateOfDeath,'') <> '')
			THEN	
				'Inactive Beneficiary'	
			--WHEN ConRolCds.ID = 37 OR ConRolCds.ID = 10 
			--THEN 'Inactive Beneficiary'		
			ELSE 'Inactive Beneficiary'		
		 END
		--ConRolCds.ContactRoleCodeDesc
		,BenConMstr.DateOfBirth
		,BenConMstr.DateOfDeath
		,BenConMstr.SSNFlag
		,BenConMstr.SSN
		,AccMgrCds.ManagerCode
		,AlnsNmbr.AllianceNumber
	FROM SYN_IT_ContactMaster BenConMstr
		INNER JOIN  SYN_IT_ContactAccountRoles AS ConAccRol ON BenConMstr.ContactID = ConAccRol.ContactID
		INNER JOIN SYN_IT_ContactRoleCodes AS ConRolCds ON ConRolCds.ID = ConAccRol.ContactRoleCode
		INNER JOIN SYN_IT_AccountMaster AS AccMstr ON AccMstr.CustomerAccountNumber = ConAccRol.CustomerAccountNumber
		INNER JOIN SYN_IT_AllianceNumbers AS AlnsNmbr ON AlnsNmbr.AllianceNumber = AccMstr.AllianceNumber
		INNER JOIN SYN_IT_AccountManagerCodes AS AccMgrCds ON AccMstr.ManagerCode = AccMgrCds.ManagerCode  
	WHERE ConRolCds.ID IN
	(
		10
		,21
		,37
	)
	
--  Temp tables, Cursors, Table Variables  --  
IF EXISTS (
		SELECT *
		FROM TEMPDB.DBO.SYSOBJECTS
		WHERE ID = OBJECT_ID(N'TEMPDB.[DBO].[#DONORBENE]')
		)
	DROP TABLE [DBO].[#DONORBENE];

--  Temp tables, Cursors, Table Variables  --  
CREATE TABLE #DONORBENE (
	TRUSTPARTICIPANT_TYPE_ID INT
	,ACCOUNT_ID CHAR(14)
	,ADVENT_ID CHAR(14)
	,PARTICIPANT_ID INT
	,DONOR_ID INT
	,BENEFICIARY_ID INT
	,DONOR_NAME VARCHAR(100)
	,FIRSTNAME VARCHAR(20)
	,LASTNAME VARCHAR(50)
	,ACCOUNT_STATUS VARCHAR(100)
	,DONOR_TYPE VARCHAR(100)
	,BIRTHDATE DATETIME
	,EXPIREDATE DATETIME
	,TaxIdType SMALLINT
	,SSN VARCHAR(20)
	,MANAGERCODE CHAR(4)
	,PROGRAMID CHAR(15)
	)

INSERT INTO #DONORBENE
SELECT 
	Ben.TRUSTPARTICIPANT_TYPE_ID
	,Dnr.ACCOUNT_ID
	,Dnr.ADVENT_ID
	,Dnr.PARTICIPANT_ID
	,Dnr.DONOR_ID
	,Ben.BENEFICIARY_ID
	,Dnr.DONOR_NAME
	,Dnr.FIRSTNAME
	,Dnr.LASTNAME
	,Dnr.ACCOUNT_STATUS
	,Dnr.DONOR_TYPE + '/' + Ben.DONOR_TYPE
	,Dnr.BIRTHDATE
	,Dnr.EXPIREDATE
	,Dnr.TaxIdType
	,Dnr.SSN
	,Dnr.ManagerCode
	,Dnr.PROGRAMID
FROM [DBO].[#DONOR] Dnr
	INNER JOIN [DBO].[#BENEFICIARIES] Ben ON Dnr.PARTICIPANT_ID = Ben.PARTICIPANT_ID
	
	
DELETE FROM [DBO].[#DONOR]
WHERE PARTICIPANT_ID IN
(
	SELECT PARTICIPANT_ID FROM [DBO].[#DONORBENE]
)

DELETE FROM [DBO].[#BENEFICIARIES]
WHERE PARTICIPANT_ID IN
(
	SELECT PARTICIPANT_ID FROM [DBO].[#DONORBENE]
)


IF EXISTS (
		SELECT *
		FROM TEMPDB.DBO.SYSOBJECTS
		WHERE ID = OBJECT_ID(N'TEMPDB.[DBO].[#MYDONORBENE]')
		)
BEGIN
	DROP TABLE [DBO].[#MYDONORBENE];
END

--  Temp tables, Cursors, Table Variables  --  
CREATE TABLE [DBO].[#MYDONORBENE](
	TRUSTPARTICIPANT_TYPE_ID INT
	,ACCOUNT_ID CHAR(14)
	,ADVENT_ID CHAR(14)
	,PARTICIPANT_ID INT
	,DONOR_ID INT
	,BENEFICIARY_ID INT
	,DONOR_NAME VARCHAR(100)
	,FIRSTNAME VARCHAR(20)
	,LASTNAME VARCHAR(50)
	,ACCOUNT_STATUS VARCHAR(100)
	,DONOR_TYPE VARCHAR(100)
	,BIRTHDATE DATETIME
	,EXPIREDATE DATETIME
	,TaxIdType VARCHAR(4)
	,SSN VARCHAR(20)
	,MANAGERCODE CHAR(4)
	,PROGRAMID CHAR(15)
	)

INSERT INTO [#MYDONORBENE]
SELECT DISTINCT
	TRUSTPARTICIPANT_TYPE_ID 
	,ACCOUNT_ID
	,ADVENT_ID
	,PARTICIPANT_ID 
	,DONOR_ID 
	,BENEFICIARY_ID
	,DONOR_NAME
	,FIRSTNAME
	,LASTNAME
	,ACCOUNT_STATUS
	,DONOR_TYPE
	,BIRTHDATE
	,EXPIREDATE
	,TaxIdType
	,SSN
	,MANAGERCODE
	,PROGRAMID
FROM [DBO].[#DONOR]
UNION ALL
SELECT DISTINCT
	TRUSTPARTICIPANT_TYPE_ID 
	,ACCOUNT_ID
	,ADVENT_ID
	,PARTICIPANT_ID 
	,DONOR_ID 
	,BENEFICIARY_ID
	,DONOR_NAME
	,FIRSTNAME
	,LASTNAME
	,ACCOUNT_STATUS
	,DONOR_TYPE
	,BIRTHDATE
	,EXPIREDATE
	,TaxIdType
	,SSN
	,MANAGERCODE
	,PROGRAMID
FROM [DBO].[#BENEFICIARIES]
UNION ALL
SELECT DISTINCT
	TRUSTPARTICIPANT_TYPE_ID 
	,ACCOUNT_ID
	,ADVENT_ID
	,PARTICIPANT_ID 
	,DONOR_ID 
	,BENEFICIARY_ID
	,DONOR_NAME
	,FIRSTNAME
	,LASTNAME
	,ACCOUNT_STATUS
	,DONOR_TYPE
	,BIRTHDATE
	,EXPIREDATE
	,TaxIdType
	,SSN
	,MANAGERCODE
	,PROGRAMID
FROM [DBO].[#DONORBENE]



	IF EXISTS (
		SELECT *
		FROM TEMPDB.DBO.SYSOBJECTS
		WHERE ID = OBJECT_ID(N'TEMPDB.[DBO].[#FILTEREDDONORBENE]')
		)
	BEGIN
		DROP TABLE [DBO].[#FILTEREDDONORBENE];
	END

--  Temp tables, Cursors, Table Variables  --  
CREATE TABLE [DBO].[#FILTEREDDONORBENE](
	TRUSTPARTICIPANT_TYPE_ID INT
	,ACCOUNT_ID CHAR(14)
	,ADVENT_ID CHAR(14)
	,PARTICIPANT_ID INT
	,DONOR_ID INT
	,BENEFICIARY_ID INT
	,DONOR_NAME VARCHAR(100)
	,FIRSTNAME VARCHAR(20)
	,LASTNAME VARCHAR(50)
	,ACCOUNT_STATUS VARCHAR(100)
	,DONOR_TYPE VARCHAR(100)
	,BIRTHDATE DATETIME
	,EXPIREDATE DATETIME
	,TaxIdType VARCHAR(4)
	,SSN VARCHAR(20)
	,MANAGERCODE CHAR(4)
	,PROGRAMID CHAR(15)
	)


	INSERT INTO [DBO].[#FILTEREDDONORBENE]
	SELECT 
		TRUSTPARTICIPANT_TYPE_ID
		,ACCOUNT_ID
		,ADVENT_ID
		,PARTICIPANT_ID
		,DONOR_ID
		,BENEFICIARY_ID
		,DONOR_NAME
		,FIRSTNAME
		,LASTNAME
		,ACCOUNT_STATUS
		,DONOR_TYPE
		,BIRTHDATE
		,EXPIREDATE
		,TaxIdType
		,SSN
		,MANAGERCODE
		,PROGRAMID
	FROM [#MYDONORBENE]
	WHERE 
	 CASE 
		WHEN @SearchColumnName = 'Advent_ID'
			THEN ADVENT_ID
		WHEN @SearchColumnName = ''
			THEN DONOR_NAME
		WHEN @SearchColumnName = 'ContactName'
			THEN DONOR_NAME
		END LIKE CASE 
		WHEN @SearchCriteria = 0
			THEN @strSearch + '%'
		WHEN @SearchCriteria = 1
			THEN '%' + @strSearch + '%'
		END
	AND MANAGERCODE IN (
		SELECT DISTINCT AccMgrcds.ManagerCode
		FROM SYN_IT_ContactMaster DonConMstr
		INNER JOIN SYN_IT_AccountManagerCodes AccMgrcds ON DonConMstr.ContactID = AccMgrcds.ContactID
		INNER JOIN SYN_IT_SubContactRoles subConRol ON subConRol.ContactID = DonConMstr.ContactID
		INNER JOIN SYN_IT_ContactRoleCodes ConRolCds ON ConRolCds.Id = subConRol.ContactRoleCode
		INNER JOIN SYN_IT_ContactMaster KCoStafConMstr ON KCoStafConMstr.contactID = subConRol.subcontactID
		INNER JOIN TBL_KS_User USR ON USR.InnotrustContactID = KCoStafConMstr.contactID
		WHERE USR.USERID = @User_Id
			AND AccMgrcds.ManagerCode = (
				CASE 
					WHEN LTrim(RTrim(@SearchManagerCode)) <> ''
					AND LTrim(RTrim(@SearchManagerCode)) <> '0'
					 	THEN LTrim(RTrim(@SearchManagerCode))
					ELSE AccMgrCds.ManagerCode
					END
				)
		)
	AND DONOR_NAME LIKE (
		CASE 
			WHEN @strFilter <> ''
				AND @strFilter <> '123'
				THEN @strFilter + '%'
			WHEN @strFilter = '123'
				THEN '[0-9]%'
			ELSE DONOR_NAME
			END
		)
	
DELETE FROM [#FILTEREDDONORBENE] WHERE ACCOUNT_STATUS='Inactive'

SELECT @totalpagecount = count(*)
FROM [#FILTEREDDONORBENE]

SELECT *
FROM (
	SELECT
		Row_number() OVER (
			ORDER BY 
			 CASE WHEN @sortcolumnname = 'NAME' AND @SortOrder = '' THEN DONOR_NAME END ASC
			,CASE WHEN @sortcolumnname = 'NAME' AND @SortOrder = 'ASC' THEN DONOR_NAME END ASC
			,CASE WHEN @sortcolumnname = 'NAME' AND @SortOrder = 'DESC' THEN DONOR_NAME END DESC
			,CASE WHEN @sortcolumnname = 'TYPE' AND @SortOrder = 'ASC' THEN DONOR_TYPE END ASC
			,CASE WHEN @sortcolumnname = 'TYPE' AND @SortOrder = 'DESC' THEN DONOR_TYPE END DESC
			,CASE WHEN @sortcolumnname = 'DATEOFBIRTH' AND @SortOrder = 'ASC' THEN BIRTHDATE END ASC
			,CASE WHEN @sortcolumnname = 'DATEOFBIRTH' AND @SortOrder = 'DESC' THEN BIRTHDATE END DESC
			,CASE WHEN @sortcolumnname = 'DATEOFDEATH' AND @SortOrder = 'ASC' THEN EXPIREDATE END ASC
			,CASE WHEN @sortcolumnname = 'DATEOFDEATH' AND @SortOrder = 'DESC' THEN EXPIREDATE END DESC
			,CASE WHEN @sortcolumnname = 'ADVENTID' AND @SortOrder = 'ASC' THEN ACCOUNT_ID END ASC
			,CASE WHEN @sortcolumnname = 'ADVENTID' AND @SortOrder = 'DESC' THEN ACCOUNT_ID END DESC
			,CASE WHEN @sortcolumnname = 'DONORID' AND @SortOrder = 'ASC' THEN DONOR_ID END ASC
			,CASE WHEN @sortcolumnname = 'DONORID' AND @SortOrder = 'DESC' THEN DONOR_ID END DESC
			,CASE WHEN @sortcolumnname = 'BENEFICIARYID' AND @SortOrder = 'ASC' THEN BENEFICIARY_ID END ASC
			,CASE WHEN @sortcolumnname = 'BENEFICIARYID' AND @SortOrder = 'DESC' THEN BENEFICIARY_ID END DESC
			,CASE WHEN @sortcolumnname = 'PARTICIPANTID' AND @SortOrder = 'ASC' THEN PARTICIPANT_ID END ASC
			,CASE WHEN @sortcolumnname = 'PARTICIPANTID' AND @SortOrder = 'DESC' THEN PARTICIPANT_ID END DESC
			) AS RowNumber 
		,TRUSTPARTICIPANT_TYPE_ID
		,ACCOUNT_ID
		,--ConAccRol.CustomerAccountNumber AS ACCOUNT_ID,       
		ADVENT_ID
		,PARTICIPANT_ID
		,DONOR_ID
		,BENEFICIARY_ID
		,DONOR_NAME
		,ACCOUNT_STATUS
		,DONOR_TYPE
		,BIRTHDATE
		,EXPIREDATE
		,SSN
		,TAXIDTYPE
		,isnull(@totalpagecount, 0) AS [TotalCount]
	FROM [#FILTEREDDONORBENE]
	) AS Client
WHERE RowNumber >= @startrow
	AND RowNumber <= @endrow


GO

SET NOCOUNT OFF;

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetDonorBenePagingMyAccounts'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_GetDonorBenePagingMyAccounts';
END