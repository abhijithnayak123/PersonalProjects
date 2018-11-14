--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <10-11-2016>
-- Description:	 Alter PK and FK constraints for User Roles related tables
-- ================================================================================


--==============================================================================================================================
--Drop foreign key constraints from tUserRolesPermissionsMapping table
--==============================================================================================================================

IF EXISTS( SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tUserRolesPermissionsMapping_tPermissions]') AND PARENT_OBJECT_ID = OBJECT_ID(N'[dbo].[tUserRolesPermissionsMapping]'))
BEGIN
    ALTER TABLE [dbo].[tUserRolesPermissionsMapping] DROP CONSTRAINT FK_tUserRolesPermissionsMapping_tPermissions;
END

--==============================================================================================================================
--Drop Primary key constraints fromt UserRolesPermissionsMapping table
--==============================================================================================================================

IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(PARENT_OBJECT_ID) = 'tUserRolesPermissionsMapping' AND OBJECT_NAME(OBJECT_ID) = 'PK_tUserRolesPermissionsMapping')
BEGIN
	ALTER TABLE [dbo].[tUserRolesPermissionsMapping] DROP CONSTRAINT PK_tUserRolesPermissionsMapping
END


IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(PARENT_OBJECT_ID) = 'tPermissions' AND OBJECT_NAME(OBJECT_ID) = 'PK_tPermissions')
BEGIN
	ALTER TABLE [dbo].[tPermissions] DROP CONSTRAINT PK_tPermissions
END


-- Rename the Id column to PK

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tUserRolesPermissionsMapping' AND COLUMN_NAME = 'PermissionId'  AND DATA_TYPE = 'uniqueidentifier' )
BEGIN
	 EXEC sp_RENAME '[tUserRolesPermissionsMapping].[PermissionId]' , 'PermissionPK' , 'COLUMN'	 
END

--=========================================================================================================
-- Add new columns in tUserRolesPermissionsMapping table
--=========================================================================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tPermissions' AND COLUMN_NAME = 'PermissionId')
BEGIN
	ALTER TABLE tUserRolesPermissionsMapping 
	ADD PermissionId  BIGINT NULL 
END


--=========================================================================================================
--Adding PK constraints to the tUserRolesPermissionsMapping table
--=========================================================================================================

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tUserRolesPermissionsMapping' AND OBJECT_NAME(OBJECT_ID) = 'PK_tUserRolesPermissionsMapping')
BEGIN
	ALTER TABLE [dbo].[tUserRolesPermissionsMapping] ADD CONSTRAINT [PK_tUserRolesPermissionsMapping] PRIMARY KEY CLUSTERED (UserRolesPermissionsMappingID)
END

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tPermissions' AND OBJECT_NAME(OBJECT_ID) = 'PK_tPermissions')
BEGIN
	ALTER TABLE [dbo].[tPermissions] ADD CONSTRAINT [PK_tPermissions] PRIMARY KEY CLUSTERED (PermissionsID)
END


--=========================================================================================================
--Adding FK constraints to the tUserRolesPermissionsMapping table
--=========================================================================================================

IF NOT EXISTS  (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[dbo].[FK_tUserRolesPermissionsMapping_tPermissions]') AND parent_object_id = OBJECT_ID(N'[dbo].[tUserRolesPermissionsMapping]'))
BEGIN
    ALTER TABLE [dbo].[tUserRolesPermissionsMapping]  WITH CHECK ADD  CONSTRAINT [FK_tUserRolesPermissionsMapping_tPermissions] FOREIGN KEY(PermissionId)
	REFERENCES [dbo].[tPermissions] (PermissionsID)
END



--=========================================================================================================
--Alter PK column for tUserRolesPermissionsMapping table
--=========================================================================================================

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tUserRolesPermissionsMapping' AND COLUMN_NAME = 'UserRolesPermissionsMappingPK' )
BEGIN
	ALTER TABLE tUserRolesPermissionsMapping 
	ALTER COLUMN UserRolesPermissionsMappingPK UNIQUEIDENTIFIER NULL 
END
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tPermissions' AND COLUMN_NAME = 'PermissionsPK' )
BEGIN
	ALTER TABLE tPermissions 
	ALTER COLUMN PermissionsPK UNIQUEIDENTIFIER NULL 
END
GO
