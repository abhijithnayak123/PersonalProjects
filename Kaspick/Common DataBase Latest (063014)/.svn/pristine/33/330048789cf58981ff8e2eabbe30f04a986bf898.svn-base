IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetAccountPaging'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetAccountPaging;

	PRINT 'DROPPED USP_EX_GetAccountPaging';
END
GO

/******************************************************************************  
** New Name:     USP_EX_GetAccountPaging  
** Old Name:     USP_EIS_EX_ACCOUNT_PAGING_SelProc  
** Short Desc: Put in Short Description  
**  
** Full Description  
**        More detailed description if necessary  
**  
** Sample Call  
   EXEC USP_EIS_EX_ACCOUNT_PAGING_SelProc  
 @USER_ID =32,  
 @VIEW_CONDITION ='TRANSITION',  
 @ModuleId  =0,  
 @ClientId  =0,  
 @ProgramID =0,  
 @startrow  =0,  
 @endrow    =19,  
 @SearchClientID   =0,  
 @SearchColumnName ='',  
 @SearchCriteria   =0,  
 @strSearch ='',  
 @Filtercolumnname ='',   
 @strFilter ='',    
  @sortcolumnname ='TYPE',   
 @SortOrder ='ASC'  
  
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
** Created DT: <mm/dd/yyyy>  
**              
*******************************************************************************  
**       Change History  
*******************************************************************************  
** Date:        Author:  Bug #     Description:                           Rvwd  
** --------     -------- ------    -------------------------------------- --------  
** 03/12/2014    Sanath   excreq 7.4  Modified       Account module 
** 22-May-2014  Sanath   Sp name renamed as per Kaspick naming convention standard 
*******************************************************************************  
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved  
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION  
*******************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_GetAccountPaging] --100038,'All Customer Account Numbers',0,0,0,1,18,0,'Advent_ID',0,'','','','Advent_ID',''  
	-- paremeters here  
	@USER_ID INT
	,@VIEW_CONDITION VARCHAR(60)
	,@ModuleId INT
	,-- from which module page is called 0-Home Page, 1-Client, 2-Program, 3-Account  
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
	,-- this will be used when user wants to search a string within a column.(Usually we send Adventid from this variable)  
	@SearchCriteria TINYINT
	,-- 0-Begins with, 1-Contains, 2-on date, 3-on or after, 4-on or before  
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
-- Body of procedure  --    
IF (
		(@VIEW_CONDITION = 'All Accounts')
		OR (@VIEW_CONDITION = 'ALL')
		)
	EXEC USP_EX_GetAccountPagingAllAccounts @ModuleId
		,@ClientId
		,@ProgramID
		,@startrow
		,@endrow
		,@SearchClientID
		,@SearchColumnName
		,@SearchCriteria
		,@strSearch
		,@Filtercolumnname
		,@strFilter
		,@sortcolumnname
		,@SortOrder
ELSE IF (@VIEW_CONDITION = 'My Accounts')
	EXEC USP_EX_GetAccountPagingMyAccounts @USER_ID
		,@ModuleId
		,@ClientId
		,@ProgramID
		,@startrow
		,@endrow
		,@SearchClientID
		,@SearchColumnName
		,@SearchCriteria
		,@strSearch
		,@Filtercolumnname
		,@strFilter
		,@sortcolumnname
		,@SortOrder
ELSE IF (@VIEW_CONDITION = 'My Inactive Accounts')
	EXEC USP_EX_GetAccountPagingMyInactiveAccounts @USER_ID
		,@ModuleId
		,@ClientId
		,@ProgramID
		,@startrow
		,@endrow
		,@SearchClientID
		,@SearchColumnName
		,@SearchCriteria
		,@strSearch
		,@Filtercolumnname
		,@strFilter
		,@sortcolumnname
		,@SortOrder
ELSE IF (@VIEW_CONDITION = 'Inactive')
	EXEC USP_EX_GetAccountPagingAllInactiveAccounts
		-- @USER_ID,  
		@ModuleId
		,@ClientId
		,@ProgramID
		,@startrow
		,@endrow
		,@SearchClientID
		,@SearchColumnName
		,@SearchCriteria
		,@strSearch
		,@Filtercolumnname
		,@strFilter
		,@sortcolumnname
		,@SortOrder
ELSE IF (@VIEW_CONDITION = 'Active')
	EXEC USP_EX_GetAccountPagingAllActiveAccounts
		-- @USER_ID,  
		@ModuleId
		,@ClientId
		,@ProgramID
		,@startrow
		,@endrow
		,@SearchClientID
		,@SearchColumnName
		,@SearchCriteria
		,@strSearch
		,@Filtercolumnname
		,@strFilter
		,@sortcolumnname
		,@SortOrder
GO

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetAccountPaging'
		)
BEGIN
	PRINT 'CREATED USP_EX_GetAccountPaging';
END