IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetClientEmployeeName'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetClientEmployeeName;

	PRINT 'DROPPED USP_EX_GetClientEmployeeName';
END
GO

/******************************************************************************            
** New Name : USP_EX_GetClientEmployeeName
** Old Name   :   USP_EIS_EX_CLIENTEMPLOYEENAMES_DTLSelProc            
**      
** Short Desc :       
**            
** Full Description            
**      
**      
** Sample Call       
 EXEC USP_EX_GetClientEmployeeName 100260,'MANAGER'  
  EXEC USP_EX_GetClientEmployeeName 'ACADW','ACCOUNT' 
 
      USP_EIS_EX_CLIENTEMPLOYEENAMES_DTLSelProc 300901,'ACCOUNT'     
**            
** Return values: NONE            
**            
** Standard declarations            
**       SET LOCK_TIMEOUT         30000   -- 30 seconds            
** Created By :        
** Company  :  Kaspick & Company            
** Project  :  Perid End valuation           
** Created DT :  October/21/2009            
**                        
*******************************************************************************            
**       Change History            
*******************************************************************************            
** Date:     Author:  Bug #  Description:        Rvwd            
** --------  -------- ------ ------------------------------------------ -------      
** 3-Apr-2014 Mallikarjun  EXCREQ 5.4  
** 22-May-2014  Sanath   Sp name renamed as per Kaspick naming convention standard  
*******************************************************************************            
** Copyright (C) 2007 Kaspick & Company, All Rights Reserved            
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION            
*******************************************************************************/
CREATE PROCEDURE USP_EX_GetClientEmployeeName (
	@ENTITYID CHAR(14)
	,-- MANAAGERCODE IS ASSIGNED FOR ENTITYID in client profile reporting                    
	@ENTITYTYPE VARCHAR(20)
	)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @ClientID VARCHAR(15)

	IF (@ENTITYTYPE = 'ACCOUNT')
	BEGIN
		SELECT @ClientID = CLIENTID
		FROM VW_EX_Account
		WHERE ACCOUNTID = @ENTITYID

		SELECT subConRol.SubContactID AS EMPLOYEEID
			,ISNULL(ConMstr.PrimaryFirstName, ' ') + ' ' + ISNULL(ConMstr.PrimaryLastName, ' ') AS EMPLOYEENAME
		FROM SYN_IT_SubContactRoles subConRol
		INNER JOIN SYN_IT_ContactMaster ConMstr ON subConRol.SubContactID = ConMstr.ContactID
		INNER JOIN SYN_IT_ContactRoleCodes ConRolCds ON subConRol.ContactRoleCode = ConRolCds.ID
	--	INNER JOIN SYN_IT_AccountManagerCodes AccMgrCds ON AccMgrCds.CONTACTID = ConMstr.CONTACTID
		WHERE ConRolCds.ID IN (550)
			AND ConMstr.ManagerCode = @CLIENTID
	END
	ELSE
		SELECT subConRol.SubContactID AS EMPLOYEEID
			,ISNULL(ConMstr.PrimaryFirstName, ' ') + ' ' + ISNULL(ConMstr.PrimaryLastName, ' ') AS EMPLOYEENAME
		FROM SYN_IT_SubContactRoles subConRol
		INNER JOIN SYN_IT_ContactMaster ConMstr ON subConRol.SubContactID = ConMstr.ContactID
		INNER JOIN SYN_IT_ContactRoleCodes ConRolCds ON subConRol.ContactRoleCode = ConRolCds.ID
		--INNER JOIN SYN_IT_AccountManagerCodes AccMgrCds ON AccMgrCds.CONTACTID = ConMstr.CONTACTID
		WHERE ConRolCds.ID IN (550)
			AND ConMstr.ManagerCode = @ENTITYID
END
GO

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetClientEmployeeName'
		)
BEGIN
	PRINT 'CREATED USP_EX_GetClientEmployeeName';
END
GO

