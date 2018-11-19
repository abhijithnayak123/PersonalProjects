IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetDonorBenePagingAllactiveAccounts'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetDonorBenePagingAllactiveAccounts;

	PRINT 'DROPPED PROCEDURE USP_EX_GetDonorBenePagingAllactiveAccounts';
END
GO

/******************************************************************************  
** New Name:     USP_EX_GetDonorBenePagingAllactiveAccounts
** Old Name:     USP_EIS_EX_DONORBENE_PAGING_AllActiveAccounts_SelProc  
** Short Desc: Lists Donor - Bene details for all accounts.  
**  
** Full Description  
**        Called from DONORBENE home page if view option selected in All Accounts.  
**  
** Sample Call  
 
 EXEC USP_EX_GetDonorBenePagingAllactiveAccounts  
 @ModuleId  =3,  
 @ManagerCode ='0',  
 @ProgramID = '0',  
 @AccountID ='ACADW',  
 @startrow  =1,  
 @endrow    =18,  
 @SearchManagerCode = '',  
 @SearchColumnName ='',  
 @SearchCriteria   =0,  
 @strSearch ='',  
 @Filtercolumnname ='NAME',   
 @strFilter ='K',    
 @sortcolumnname ='ADVENTID',   
 @SortOrder ='ASC'  
 
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
** 6/19/2007 Ritu  4672  To set the sort order to Name column   
         instead of Advent_ID column
** 5/12/2014 Sanath   client donor/Beneficiary paging[Inner paging] 
** 22-May-2014  Sanath   Sp name renamed as per Kaspick naming convention standard         
*******************************************************************************  
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved  
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION  
*******************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_GetDonorBenePagingAllactiveAccounts]
	-- paremeters here  
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
	@SearchCriteria INT
	,-- 0-Begins with, 1-Contains  
	@strSearch VARCHAR(255)
	,-- search string that will be located in column provided above.  
	@Filtercolumnname VARCHAR(100)
	,-- Column on which the default filtering will be done (ADVENT_ID)  
	@strFilter VARCHAR(255)
	,-- for filtering the char/varchar, which will work on filter control  
	@sortcolumnname VARCHAR(100)
	,-- column on which the sorting should work (NAME / TYPE/ DATEOFBIRTH / ADVENT_ID)  
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
	,BENEFICIARY_ID CHAR(14)
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
		,'0'
		,DonConMstr.ContactName
		,DonConMstr.PrimaryFirstName
		,DonConMstr.PrimaryLastName
		,CASE AccMstr.ActiveFlag
			WHEN - 1
				THEN 'Yes'
			WHEN 0
				THEN 'No'
			END AS ACCOUNT_STATUS
		,ConRolCds.ContactRoleCodeDesc
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
	AND CASE 
		WHEN @SearchColumnName = 'Advent_ID'
			THEN ConAccRol.CustomerAccountNumber
		WHEN @SearchColumnName = ''
			THEN DonConMstr.ContactName
		WHEN @SearchColumnName = 'ContactName'
			THEN DonConMstr.ContactName
		END LIKE CASE 
		WHEN @SearchCriteria = 0
			THEN @strSearch + '%'
		WHEN @SearchCriteria = 1
			THEN '%' + @strSearch + '%'
		END
	AND AccMgrCds.ManagerCode = (
		CASE 
			WHEN LTrim(RTrim(@SearchManagerCode)) <> ''
				AND LTrim(RTrim(@SearchManagerCode)) <> '0'
				THEN LTrim(RTrim(@SearchManagerCode))
			ELSE AccMgrCds.ManagerCode
			END
		)
	AND DonConMstr.ContactName LIKE (
		CASE 
			WHEN @strFilter <> ''
				AND @strFilter <> '123'
				THEN @strFilter + '%'
			WHEN @strFilter = '123'
				THEN '[0-9]%'
			ELSE DonConMstr.ContactName
			END
		)
	AND AccMstr.ManagerCode = (
		CASE 
			WHEN @ModuleId=1 and @ManagerCode <> '0'
		THEN @ManagerCode
		ELSE AccMstr.ManagerCode	
		END
		)	
	AND AccMstr.AllianceNumber = (
		CASE 
			WHEN @ModuleId=2 and @ProgramID <> '0'
		THEN @ProgramID	
		ELSE AccMstr.AllianceNumber
		END
		)
	AND AccMstr.CustomerAccountNumber = (
		CASE 
			WHEN @ModuleId=3 and @AccountID <> '0'
		THEN @AccountID	
		ELSE AccMstr.CustomerAccountNumber
		END
		)
	AND AccMstr.CustomerAccountNumber LIKE (
			CASE 
				WHEN @strFilter <> '' 
					AND @strFilter <> '123'
					THEN @strFilter + '%'
				WHEN @strFilter = '123'
					THEN '[0-9]%'
				WHEN @ModuleId=3 and @AccountID <> '0' THEN @AccountID	
				ELSE AccMstr.CustomerAccountNumber
				END
			)
	AND AccMstr.ActiveFlag = - 1
	ORDER BY DonConMstr.ContactName


	
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
	,BENEFICIARY_ID CHAR(14)
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
		,'0'
		,BenConMstr.ContactId
		,BenConMstr.ContactName
		,BenConMstr.PrimaryFirstName
		,BenConMstr.PrimaryLastName
		,CASE AccMstr.ActiveFlag
			WHEN - 1
				THEN 'Yes'
			WHEN 0
				THEN 'No'
			END AS ACCOUNT_STATUS
		,ConRolCds.ContactRoleCodeDesc
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
	AND CASE 
		WHEN @SearchColumnName = 'Advent_ID'
			THEN ConAccRol.CustomerAccountNumber
		WHEN @SearchColumnName = ''
			THEN BenConMstr.ContactName
		WHEN @SearchColumnName = 'ContactName'
			THEN BenConMstr.ContactName
		END LIKE CASE 
		WHEN @SearchCriteria = 0
			THEN @strSearch + '%'
		WHEN @SearchCriteria = 1
			THEN '%' + @strSearch + '%'
		END
	AND AccMgrCds.ManagerCode = (
		CASE 
			WHEN LTrim(RTrim(@SearchManagerCode)) <> ''
				AND LTrim(RTrim(@SearchManagerCode)) <> '0'
				THEN LTrim(RTrim(@SearchManagerCode))
			ELSE AccMgrCds.ManagerCode
			END
		)
	AND BenConMstr.ContactName LIKE (
		CASE 
			WHEN @strFilter <> ''
				AND @strFilter <> '123'
				THEN @strFilter + '%'
			WHEN @strFilter = '123'
				THEN '[0-9]%'
			ELSE BenConMstr.ContactName
			END
		)
	AND AccMstr.ManagerCode = (
		CASE 
			WHEN @ModuleId=1 and @ManagerCode <> '0'
		THEN @ManagerCode
		ELSE AccMstr.ManagerCode	
		END
		)	
	AND AccMstr.AllianceNumber = (
		CASE 
			WHEN @ModuleId=2 and @ProgramID <> '0'
		THEN @ProgramID	
		ELSE AccMstr.AllianceNumber
		END
		)
	AND AccMstr.CustomerAccountNumber LIKE (
			CASE 
				WHEN @strFilter <> '' 
					AND @strFilter <> '123'
					THEN @strFilter + '%'
				WHEN @strFilter = '123'
					THEN '[0-9]%'
				WHEN @ModuleId=3 and @AccountID <> '0' THEN @AccountID	
				ELSE AccMstr.CustomerAccountNumber
				END
			)
	AND AccMstr.ActiveFlag = - 1
	ORDER BY BenConMstr.ContactName

	
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
	,BENEFICIARY_ID CHAR(14)
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
		,'Donor/Bene'
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
			WHERE ID = OBJECT_ID(N'TEMPDB.[DBO].[#ALLDONORBENE]')
			)
	BEGIN
		DROP TABLE [DBO].[#ALLDONORBENE];
	END

	
--  Temp tables, Cursors, Table Variables  --  
CREATE TABLE [DBO].[#ALLDONORBENE](
	TRUSTPARTICIPANT_TYPE_ID INT
	,ACCOUNT_ID CHAR(14)
	,ADVENT_ID CHAR(14)
	,PARTICIPANT_ID INT
	,DONOR_ID INT
	,BENEFICIARY_ID CHAR(14)
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

	INSERT INTO [#ALLDONORBENE]
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
	FROM [DBO].[#DONOR]
	UNION
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
	FROM [DBO].[#BENEFICIARIES]
	UNION
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
	FROM [DBO].[#DONORBENE]


	SELECT @totalpagecount = count(*)
	FROM [#ALLDONORBENE]


	SELECT *
	FROM (
		SELECT Row_number() OVER (
				ORDER BY DONOR_NAME ASC
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
		FROM [#ALLDONORBENE]
		) AS Client
	WHERE RowNumber >= @startrow
		AND RowNumber <= @endrow
	ORDER BY
		CASE
		WHEN @SortOrder = 'ASC'
		THEN
			CASE @sortcolumnname
				WHEN 'NAME' THEN DONOR_NAME
				WHEN 'TYPE' THEN CAST (TRUSTPARTICIPANT_TYPE_ID AS VARCHAR(50)) --ABS(TRUSTPARTICIPANT_TYPE_ID)
				WHEN 'DATEOFBIRTH' THEN CONVERT(VARCHAR(50), BIRTHDATE, 120)
				WHEN 'DATEOFDEATH' THEN CONVERT(VARCHAR(50), EXPIREDATE, 120) 
				WHEN 'ADVENTID' THEN ADVENT_ID
				WHEN 'DONORID' THEN CAST (DONOR_ID AS VARCHAR(50))
				WHEN 'BENEFICIARYID' THEN CAST (BENEFICIARY_ID AS VARCHAR(50)) --ABS(BENEFICIARY_ID)
				WHEN 'PARTICIPANTID' THEN CAST (PARTICIPANT_ID AS VARCHAR(50)) --ABS(PARTICIPANT_ID)
			END
		END ASC,
		CASE
			WHEN @SortOrder = 'DESC'
			THEN
				CASE @sortcolumnname
					WHEN 'NAME' THEN DONOR_NAME
					WHEN 'TYPE' THEN CAST (TRUSTPARTICIPANT_TYPE_ID AS VARCHAR(50)) --ABS(TRUSTPARTICIPANT_TYPE_ID)
					WHEN 'DATEOFBIRTH' THEN CONVERT(VARCHAR(50), BIRTHDATE, 120)
					WHEN 'DATEOFDEATH' THEN CONVERT(VARCHAR(50), EXPIREDATE, 120) 
					WHEN 'ADVENTID' THEN ADVENT_ID
					WHEN 'DONORID' THEN CAST (DONOR_ID AS VARCHAR(50))
					WHEN 'BENEFICIARYID' THEN CAST (BENEFICIARY_ID AS VARCHAR(50)) --ABS(BENEFICIARY_ID)
					WHEN 'PARTICIPANTID' THEN CAST (PARTICIPANT_ID AS VARCHAR(50)) --ABS(PARTICIPANT_ID)
				END
		END DESC	
GO

SET NOCOUNT OFF;

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetDonorBenePagingAllactiveAccounts'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_GetDonorBenePagingAllactiveAccounts';
END