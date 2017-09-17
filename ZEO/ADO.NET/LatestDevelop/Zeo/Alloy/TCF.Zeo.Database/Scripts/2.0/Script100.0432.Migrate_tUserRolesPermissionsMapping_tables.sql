--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <07-20-2016>
-- Description:	 Migration scripts for tUserRolesPermissionsMapping table.
-- Jira ID:		<AL-7705>
-- ================================================================================

--========================================tUserRolesPermissionsMapping========================================================

BEGIN TRY
 BEGIN TRANSACTION 

  UPDATE urp
  SET
    urp.PermissionId = p.PermissionsID
  FROM 
    tUserRolesPermissionsMapping urp
  INNER JOIN
    tPermissions p 
  ON
    urp.PermissionPK = p.PermissionsPK
 
 
 COMMIT
END TRY

BEGIN CATCH
	ROLLBACK
END CATCH 
GO


 --============================== ALTER new column as NOT NULL ================================================


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tUserRolesPermissionsMapping' AND COLUMN_NAME = 'PermissionId')
BEGIN	
	ALTER TABLE tUserRolesPermissionsMapping 
	ALTER COLUMN PermissionId BIGINT NOT NULL 
END
GO
