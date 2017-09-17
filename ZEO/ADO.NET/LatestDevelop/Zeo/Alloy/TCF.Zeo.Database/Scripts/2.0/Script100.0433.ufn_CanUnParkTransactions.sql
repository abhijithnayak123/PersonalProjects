-- ================================================================================
-- Author:		Manikandan Govindraj
-- Create date: 03/24/2017
-- Description: Check the permission to un park the transactions
-- ID:		  : 126
-- ================================================================================

IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE = 'FN' AND NAME = 'ufn_CanUnParkTransactions')
BEGIN
    DROP FUNCTION dbo.ufn_CanUnParkTransactions
END
GO

CREATE FUNCTION dbo.ufn_CanUnParkTransactions
(
    @customerSessionId  BIGINT
)
RETURNS BIT
BEGIN
	
	DECLARE @canUnPark BIT = 0

	IF EXISTS
	    (
		   SELECT 1 FROM tUserRolesPermissionsMapping urp WITH(NOLOCK)
		
		   INNER JOIN tPermissions p WITH(NOLOCK) ON p.PermissionsID = urp.PermissionId
		   
		   INNER JOIN tUserRoles ur WITH(NOLOCK) ON ur.UserRolesID = urp.RoleId
		   
		   INNER JOIN tAgentDetails ad WITH(NOLOCK) ON ad.UserRoleId = ur.UserRolesID
		   
		   INNER JOIN tAgentSessions ags WITH(NOLOCK) ON ags.AgentId = ad.AgentId
		   
		   INNER JOIN tCustomerSessions cs WITH(NOLOCK) ON cs.AgentSessionId = ags.AgentSessionID
		   WHERE 
		   cs.CustomerSessionID = @customerSessionId
		   AND
		   p.Permission = 'CanUnparkTransactions'  -- Permission Name : Hard coded value in tPermissions table
		)

		BEGIN
		  SET @canUnPark = 1
		END

		RETURN @canUnPark
		
END
GO