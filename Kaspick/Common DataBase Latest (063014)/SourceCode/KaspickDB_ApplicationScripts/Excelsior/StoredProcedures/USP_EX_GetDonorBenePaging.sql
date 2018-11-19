IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetDonorBenePaging'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetDonorBenePaging;

	PRINT 'DROPPED USP_EX_GetDonorBenePaging';
END
GO

/******************************************************************************  
** Name: USP_EX_GetDonorBenePaging
** Old Name:     USP_EIS_EX_DonorBene_PAGING_SelProc  
** Short Desc: Put in Short Description  
**  
** Full Description  
**        More detailed description if necessary  
**  
** Sample Call  
   EXEC USP_EIS_EX_DonorBene_PAGING_SelProc  
 @USER_ID =1,  
 @VIEW_CONDITION ='ALL ACTIVE ACCOUNTS',  
 @ModuleId  =0,  
 @ClientId  =0,  
 @ProgramID =0,  
 @AccountID =0,  
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
** 04/20/2007 Chirag                Added block for 'Transition' view option  
** 14-Mar-2014 Mallikarjun          EXCREQ 8.4 Modified   
** 22-May-2014  Sanath              Sp name renamed as per Kaspick naming convention standard        
*******************************************************************************  
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved  
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION  
*******************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_GetDonorBenePaging] --100260,'All Customer Account Numbers', 0,0,0,0,1,18,'','Advent_ID',0,'pa','','','NAME',''  
	-- paremeters here  
	@USER_ID INT
	,@VIEW_CONDITION VARCHAR(50)
	,@ModuleId INT
	,-- from which module page is called 0-Home Page, 1-Client, 2-Program, 3-Account  
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
--  Initial Set statements  --  
SET NOCOUNT ON;
SET LOCK_TIMEOUT 30000;-- 30 seconds  

IF (
		(@VIEW_CONDITION = 'All Accounts')
		OR (@VIEW_CONDITION = 'ALL')
		)
BEGIN
	EXEC USP_EX_GetDonorBenePagingAllAccounts @ModuleId
		,@ManagerCode
		,@ProgramID
		,@AccountID
		,@startrow
		,@endrow
		,@SearchManagerCode
		,@SearchColumnName
		,@SearchCriteria
		,@strSearch
		,@Filtercolumnname
		,@strFilter
		,@sortcolumnname
		,@SortOrder
END
ELSE IF @VIEW_CONDITION = 'My Accounts'
BEGIN
	EXEC USP_EX_GetDonorBenePagingMyAccounts @USER_ID
		,@ModuleId
		,@ManagerCode
		,@ProgramID
		,@AccountID
		,@startrow
		,@endrow
		,@SearchManagerCode
		,@SearchColumnName
		,@SearchCriteria
		,@strSearch
		,@Filtercolumnname
		,@strFilter
		,@sortcolumnname
		,@SortOrder
END
ELSE IF @VIEW_CONDITION = 'Inactive'
BEGIN
	EXEC USP_EX_GetDonorBenePagingAllInactiveAccounts
		-- @USER_ID,  
		@ModuleId
		,@ManagerCode
		,@ProgramID
		,@AccountID
		,@startrow
		,@endrow
		,@SearchManagerCode
		,@SearchColumnName
		,@SearchCriteria
		,@strSearch
		,@Filtercolumnname
		,@strFilter
		,@sortcolumnname
		,@SortOrder
END
ELSE IF @VIEW_CONDITION = 'Active'
BEGIN
	EXEC USP_EX_GetDonorBenePagingAllactiveAccounts
		-- @USER_ID,  
		@ModuleId
		,@ManagerCode
		,@ProgramID
		,@AccountID
		,@startrow
		,@endrow
		,@SearchManagerCode
		,@SearchColumnName
		,@SearchCriteria
		,@strSearch
		,@Filtercolumnname
		,@strFilter
		,@sortcolumnname
		,@SortOrder
END
GO

SET NOCOUNT OFF;

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetDonorBenePaging'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_GetDonorBenePaging';
END