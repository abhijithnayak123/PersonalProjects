IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetClientPagingMyClients'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetClientPagingMyClients;

	PRINT 'DROPPED USP_EX_GetClientPagingMyClients';
END
GO

/******************************************************************************
** New Name :    USP_EX_GetClientPagingMyClients
** Old Name:     USP_EIS_EX_CLIENT_PAGING_MyClients_SelProc
** Short Desc:	Put in Short Description
**
** Full Description
**        More detailed description if necessary
**
** Sample Call
   EXEC USP_EX_GetClientPagingMyClients
	@User_Id   = 100278, --30,
	@startrow  =-1,
	@endrow    =19,
	@SearchColumnName ='',
	@SearchCriteria	  =0,
	@strSearch ='',
	@Filtercolumnname ='', 
	@strFilter ='',		
 	@sortcolumnname ='STATUS', 
	@SortOrder ='ASC'

**
** Return values: NONE
**
**
** Standard declarations
**       SET NOCOUNT             ON
**       SET LOCK_TIMEOUT         30000   -- 30 seconds
**	
**	Created By: <Authorname>
**	Company	  :	Kaspick & Company
**	Project	  :	Excelsior
**	Created DT:	<mm/dd/yyyy>
**            
*******************************************************************************
**       Change History
*******************************************************************************
** Date:        Author:  Bug #     Description:                           Rvwd
** --------     -------- ------    -------------------------------------- --------
** <mm/dd/yyyy>
** <06/21/2007>	Tanuj	5280		Added new parameter 'PARTYTYPE'
** 21-Feb-2014  Mallikarjun  Modified   EXCREQ 5.1
** 22-May-2014  Sanath   Sp name renamed as per Kaspick naming convention standard 
*******************************************************************************
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION
*******************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_GetClientPagingMyClients]
	-- paremeters here  
	@User_Id INT
	,-- logged in user id  
	@startrow INT
	,-- start row for the page  
	@endrow INT
	,-- end row for the page  
	@SearchColumnName VARCHAR(100)
	,-- this will be used when user wants to search a string within a column.  
	-- 'Program Brief Name'/'Program Name'  
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
--  Variable Declarations  --  
DECLARE @totalpagecount INT;

--  Temp tables, Cursors, Table Variables  --  
IF EXISTS (
		SELECT *
		FROM TEMPDB.DBO.SYSOBJECTS
		WHERE ID = OBJECT_ID(N'TEMPDB.[DBO].[#CLIENT ]')
		)
	DROP TABLE [DBO].[#CLIENT];

CREATE TABLE #CLIENT (
	ManagerCode VARCHAR(4)
	,ManagerName VARCHAR(100)
	,STATUS VARCHAR(100)
	)

INSERT INTO #CLIENT
SELECT AccMgrCds.ManagerCode AS ManagerCode
	,ConMstr.ContactName AS ManagerName
	,CASE AccMgrCds.ActiveFlag
		WHEN - 1
			THEN 'Active'
		WHEN 0
			THEN 'Inactive'
		END AS STATUS
FROM SYN_IT_AccountManagerCodes AccMgrCds
INNER JOIN SYN_IT_ContactMaster ConMstr ON AccMgrCds.CONTACTID = ConMstr.CONTACTID
WHERE CASE 
		WHEN @SearchColumnName = 'BRIEF NAME'
			THEN AccMgrCds.ManagerCode
		WHEN @SearchColumnName = ''
			THEN AccMgrCds.ManagerCode
		WHEN @SearchColumnName = 'FULL NAME'
			THEN ConMstr.ContactName
		END LIKE CASE 
		WHEN @SearchCriteria = 0
			THEN @strSearch + '%'
		WHEN @SearchCriteria = 1
			THEN '%' + @strSearch + '%'
		END
	AND AccMgrCds.ManagerCode IN (
		SELECT DISTINCT AccMgrCds.ManagerCode
		FROM SYN_IT_ContactMaster ConMstr
		INNER JOIN SYN_IT_AccountManagerCodes AccMgrCds ON ConMstr.ContactID = AccMgrCds.ContactID
		INNER JOIN SYN_IT_SubContactRoles subConRol ON subConRol.ContactID = ConMstr.ContactID
		INNER JOIN SYN_IT_ContactRoleCodes ConRolCds ON ConRolCds.Id = subConRol.ContactRoleCode
		INNER JOIN SYN_IT_ContactMaster KCoStfConMstr ON KCoStfConMstr.contactID = subConRol.subcontactID
		INNER JOIN TBL_KS_User KsUsr ON KsUsr.InnotrustContactID = KCoStfConMstr.contactID
		WHERE KsUsr.USERID = @User_Id
			AND AccMgrCds.ManagerCode LIKE (
				CASE 
					WHEN @strFilter <> ''
						AND @strFilter <> '123'
						THEN @strFilter + '%'
					WHEN @strFilter = '123'
						THEN '[0-9]%'
					ELSE AccMgrCds.ManagerCode
					END
				)
		)


DELETE FROM #CLIENT WHERE STATUS='Inactive'

SELECT @totalpagecount = count(*)
FROM #CLIENT

SELECT *
FROM (
	SELECT ManagerCode AS ManagerCode
		,ManagerName AS ManagerName
		,STATUS
		,Row_number() OVER (
			ORDER BY 
					CASE WHEN @sortcolumnname = 'BRIEF NAME' AND @SortOrder = 'ASC' THEN ManagerCode END ASC,
					CASE WHEN @sortcolumnname = 'BRIEF NAME' AND @SortOrder = 'DESC' THEN ManagerCode END DESC,
					CASE WHEN @sortcolumnname = 'LEGAL NAME' AND @SortOrder = 'ASC' THEN ManagerName END ASC,
					CASE WHEN @sortcolumnname = 'LEGAL NAME' AND @SortOrder = 'DESC' THEN ManagerName END DESC,
					CASE WHEN @sortcolumnname = 'STATUS' AND @SortOrder = 'ASC' THEN STATUS END ASC,
					CASE WHEN @sortcolumnname = 'STATUS' AND @SortOrder = 'DESC' THEN STATUS END DESC
			) AS RowNumber
		,isnull(@totalpagecount, 0) AS [TotalCount]
	FROM #CLIENT
	) AS Client
WHERE RowNumber >= @startrow
	AND RowNumber <= @endrow
GO

SET NOCOUNT OFF;

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetClientPagingMyClients'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_GetClientPagingMyClients';
END