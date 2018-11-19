IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetClientEmployeeTax'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetClientEmployeeTax;

	PRINT 'DROPPED PROCEDURE USP_EX_GetClientEmployeeTax';
END
GO

/**************************************************************************          
* New Procedure Name  : USP_EX_GetClientEmployeeTax
* Old Procedure Name  : USP_EIS_EX_ClientEmployeeTaxSelProc --'AK'  
* Description     : Retrieve list of client employees for a particular a client.  
* Input Parameter :@CLIENTID    
*    
* Modification log: Query Indentation                                                     
*                                      
* Date   Modified by  Description      
*-------------------------------------------------------------------------          
* 21-Oct-06  Geetha P  Created      
* 30-Oct-06  MAnjiri C  Modified  
* 12/04/2007   Saravanan PM       Modifed Middle_Name to MiddleInitials && Change Request#4754    
*12/03/2014    Sanath  TAX tab EXCREQ 7.4
* 22-May-2014  Sanath   Sp name renamed as per Kaspick naming convention standard 

***********************************************************************************/
CREATE PROCEDURE [dbo].USP_EX_GetClientEmployeeTax --1000000        
	(@CLIENTID VARCHAR(15))
AS
BEGIN
	SELECT ConMstrEmp.ContactID AS EmployeeID
		,LTRIM(ISNULL(ConMstrEmp.PrimaryFirstName, '') + ' ' + (
				CASE 
					WHEN (
							ConMstrEmp.PrimaryMiddleInitial IS NULL
							OR LTRIM(RTRIM(ConMstrEmp.PrimaryMiddleInitial)) = ''
							)
						THEN ''
					ELSE (ConMstrEmp.PrimaryMiddleInitial + ' ')
					END
				) + ' ' + ISNULL(ConMstrEmp.PrimaryLastName, '')) AS EMPLOYEENAME
	FROM SYN_IT_ContactMaster ConMstr
	INNER JOIN SYN_IT_SubContactRoles subConRol ON subConRol.ContactID = ConMstr.ContactID
		AND subConRol.ContactRoleCode = (
			SELECT ID
			FROM SYN_IT_ContactRoleCodes
			WHERE ID IN (550)
			)
	INNER JOIN SYN_IT_ContactMaster ConMstrEmp ON ConMstrEmp.ContactID = subConRol.SubContactID
	WHERE ConMstr.ManagerCode = @CLIENTID
END
GO

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetClientEmployeeTax'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_GetClientEmployeeTax';
END