IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetClientPaging'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetClientPaging;

	PRINT 'DROPPED USP_EX_GetClientPaging';
END
GO

/******************************************************************************
** New Name:	 USP_EX_GetClientPaging
** Old Name:     USP_EIS_EX_CLIENT_PAGING_SelProc
** Short Desc:	Put in Short Description
**
** Full Description
**        More detailed description if necessary
**
** Sample Call
   EXEC USP_EX_GetClientPaging
	@USER_ID = 100023,
	@VIEW_CONDITION ='All Manager Codes',
	@startrow  =0,
	@endrow    =19,
	@SearchColumnName ='BRIEF NAME',
	@SearchCriteria	  =0,
	@strSearch ='c',
	@Filtercolumnname ='BRIEF NAME', 
	@strFilter ='B',		
 	@sortcolumnname ='BRIEF NAME', 
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
** 21-Feb-2014  Mallikarjun EXCREQ5.4 Modified
** 22-May-2014  Sanath              Sp name renamed as per Kaspick naming convention standard 
*******************************************************************************
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION
*******************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_GetClientPaging]
	-- paremeters here  
	@USER_ID INT
	,@VIEW_CONDITION VARCHAR(50)
	,@startrow INT
	,-- start row for the page  
	@endrow INT
	,-- end row for the page  
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
IF (@VIEW_CONDITION = 'All Clients')
	EXEC USP_EX_GetClientPagingAllClients @startrow
		,@endrow
		,@SearchColumnName
		,@SearchCriteria
		,@strSearch
		,@Filtercolumnname
		,@strFilter
		,@sortcolumnname
		,@SortOrder
ELSE IF (@VIEW_CONDITION = 'My Clients')
	EXEC USP_EX_GetClientPagingMyClients @USER_ID
		,@startrow
		,@endrow
		,@SearchColumnName
		,@SearchCriteria
		,@strSearch
		,@Filtercolumnname
		,@strFilter
		,@sortcolumnname
		,@SortOrder
GO

SET NOCOUNT OFF;

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetClientPaging'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_GetClientPaging';
END