IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetAccountPagingMyInactiveAccounts'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetAccountPagingMyInactiveAccounts;

	PRINT 'DROPPED USP_EX_GetAccountPagingMyInactiveAccounts';
END
GO

/*************************************************************************** 
** New Name:     USP_EX_GetAccountPagingMyInactiveAccounts  
** Old Name:     USP_EIX_AccountPagingMyInactiveAccountsSelProc 
** Short Desc: Put in Short Description  
**  
** Full Description  
**        More detailed description if necessary  
**  
** Sample Call  
   EXEC USP_EX_GetAccountPagingMyInactiveAccounts  
  @ModuleId  =0,  
 @ClientId  =0,  
 @ProgramID =0,  
 @startrow  =1,  
 @endrow    =19,  
 @SearchClientID   =0,  
 @SearchColumnName ='',  
 @SearchCriteria   =0,  
 @strSearch ='',  
 @Filtercolumnname ='',   
 @strFilter ='',    
  @sortcolumnname ='PROGRAMBRIEFNAME',   
 @SortOrder ='desc' 
 
 **       Change History  
*******************************************************************************  
** Date:        Author:  Bug #     Description:                           Rvwd  
** --------     -------- ------    -------------------------------------- --------  
** 03/13/2014    Sanath  EXCREQ 7.4  Modified        Account module  
** 23-May-2014  Sanath   Sp name renamed as per Kaspick naming convention standard 
******************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_GetAccountPagingMyInactiveAccounts] -- 100038,0,0,0, 1,18,0,'Advent_ID',0,'','','','',''   
	@USER_ID INT
	,@ModuleId INT
	,-- from which module page is called 1-Client, 2-Program, 3-Account  
	@ClientId VARCHAR(15)
	,-- when page is used inside client modules  
	@ProgramID CHAR(15)
	,-- when page is used inside program modules  
	@startrow INT
	,-- start row for the page  
	@endrow INT
	,-- end row for the page  
	@SearchClientID CHAR(4)
	,-- when page is used inside other modules   
	@SearchColumnName VARCHAR(100)
	,-- this will be used when user wants to search a string within a column.  
	-- (Usually we send 'Advent_id' for this variable)  
	@SearchCriteria TINYINT
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
--  Variable Declarations  --    
DECLARE @totalpagecount INT;

--  Temp tables, Cursors, Table Variables  --    
IF EXISTS (
		SELECT *
		FROM TEMPDB.DBO.SYSOBJECTS
		WHERE ID = OBJECT_ID(N'TEMPDB.[DBO].[#ACCOUNT ]')
		)
	DROP TABLE [DBO].[#ACCOUNT];

CREATE TABLE #ACCOUNT (
	ADVENT_ID CHAR(14)
	,ACCOUNTID CHAR(14)
	,CREATED_DATE DATETIME
	,MATURE_DATE DATETIME
	,TAXID VARCHAR(12)
	,ACCOUNT_NAME VARCHAR(40)
	,STATUS VARCHAR(10)
	,ACCOUNT_TYPE CHAR(10)
	,PROGRAM_BRIEF_NAME CHAR(15)
	,CLIENT_ACCOUNT_CODE VARCHAR(20)
	)

INSERT INTO #ACCOUNT
SELECT AccMstr.CustomerAccountNumber AS ADVENT_ID
	,AccMstr.CustomerAccountNumber AS ACCOUNTID
	,AccMstr.AccountFundedDate AS CREATED_DATE
	,UdfAccMstr.UDFAMColumn030 AS MATURE_DATE
	,AccMstr.TaxIDNumber AS TAXID
	,AccMstr.CustomerDescriptionLine1 AS ACCOUNT_NAME
	,CASE  
		WHEN AccMstr.ActiveFlag = -1 AND AccMstr.ClosedFlag =0
			THEN 'Active'
		ELSE
			 'Inactive'
		END AS STATUS
	,AccMstr.AccountTypeCode AS ACCOUNT_TYPE
	,AccMstr.AllianceNumber AS PROGRAM_BRIEF_NAME
	,UdfAccMstr.UDFAMColumn042 AS CLIENT_ACCOUNT_CODE
FROM SYN_IT_AccountMaster AccMstr
INNER JOIN SYN_IT_UDF_AccountMaster UdfAccMstr ON AccMstr.CustomerAccountNumber = UdfAccMstr.CustomerAccountNumber_Key
INNER JOIN SYN_IT_AllianceNumbers AlnsNmbr ON AccMstr.AllianceNumber = AlnsNmbr.AllianceNumber
WHERE CASE 
		WHEN @SearchColumnName = 'Advent_ID'
			THEN AccMstr.CustomerAccountNumber
		WHEN @SearchColumnName = ''
			THEN AccMstr.CustomerAccountNumber
		WHEN @SearchColumnName = 'AccountName'
			THEN AccMstr.CustomerDescriptionLine1
		END LIKE CASE 
		WHEN @SearchCriteria = 0
			THEN @strSearch + '%'
		WHEN @SearchCriteria = 1
			THEN '%' + @strSearch + '%'
		END
	AND AccMstr.CustomerAccountNumber IN (
		SELECT DISTINCT AccMstr.CustomerAccountNumber
		FROM SYN_IT_ContactMaster ConMstr
		INNER JOIN SYN_IT_AccountManagerCodes AccMgrCds ON ConMstr.ContactID = AccMgrCds.ContactID
		INNER JOIN SYN_IT_AccountMaster AccMstr ON AccMgrCds.ManagerCode = AccMstr.ManagerCode
		INNER JOIN SYN_IT_SubContactRoles subConRol ON subConRol.ContactID = ConMstr.ContactID
		INNER JOIN SYN_IT_ContactRoleCodes ConRolCds ON ConRolCds.Id = subConRol.ContactRoleCode
		INNER JOIN SYN_IT_ContactMaster KCoStafConMstr ON KCoStafConMstr.contactID = subConRol.subcontactID
		INNER JOIN TBL_KS_User Usr ON Usr.InnotrustContactID = KCoStafConMstr.contactID
		WHERE Usr.USERID = @User_Id
		)
	AND NOT (AccMstr.ActiveFlag = -1 AND AccMstr.ClosedFlag = 0)


SELECT @totalpagecount = count(*)
FROM #ACCOUNT

SELECT *
FROM (
	SELECT ADVENT_ID
		,ACCOUNTID
		,CREATED_DATE
		,MATURE_DATE
		,TAXID
		,ACCOUNT_NAME
		,STATUS
		,ACCOUNT_TYPE
		,PROGRAM_BRIEF_NAME
		,CLIENT_ACCOUNT_CODE
		,Row_number() OVER (
			ORDER BY 
			        CASE WHEN @sortcolumnname = 'ADVENTID' AND @SortOrder = '' THEN ADVENT_ID END ASC,
					CASE WHEN @sortcolumnname = 'ADVENTID' AND @SortOrder = 'ASC' THEN ADVENT_ID END ASC,
					CASE WHEN @sortcolumnname = 'ADVENTID' AND @SortOrder = 'DESC' THEN ADVENT_ID END DESC,
					CASE WHEN @sortcolumnname = 'AccountName' AND @SortOrder = 'ASC' THEN ACCOUNT_NAME END ASC,
					CASE WHEN @sortcolumnname = 'AccountName' AND @SortOrder = 'DESC' THEN ACCOUNT_NAME END DESC,
					CASE WHEN @sortcolumnname = 'Status' AND @SortOrder = 'ASC' THEN STATUS END ASC,
					CASE WHEN @sortcolumnname = 'Status' AND @SortOrder = 'DESC' THEN STATUS END DESC,
					CASE WHEN @sortcolumnname = 'Type' AND @SortOrder = 'ASC' THEN ACCOUNT_TYPE END ASC,
					CASE WHEN @sortcolumnname = 'Type' AND @SortOrder = 'DESC' THEN ACCOUNT_TYPE END DESC,
					CASE WHEN @sortcolumnname = 'ProgramBriefName' AND @SortOrder = 'ASC' THEN PROGRAM_BRIEF_NAME END ASC,
					CASE WHEN @sortcolumnname = 'ProgramBriefName' AND @SortOrder = 'DESC' THEN PROGRAM_BRIEF_NAME END DESC
			) AS RowNumber
		,isnull(@totalpagecount, 0) AS [TotalCount]
	FROM #ACCOUNT
	) AS Account
WHERE RowNumber >= @startrow
	AND RowNumber <= @endrow
GO

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetAccountPagingMyInactiveAccounts'
		)
BEGIN
	PRINT 'CREATED USP_EX_GetAccountPagingMyInactiveAccounts';
END