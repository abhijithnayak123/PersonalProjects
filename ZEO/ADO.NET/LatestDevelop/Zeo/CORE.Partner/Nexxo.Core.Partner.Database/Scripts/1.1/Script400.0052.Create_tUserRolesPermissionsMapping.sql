--===========================================================================================
-- Author:		<Mohammed Khaja Firoz Molla>
-- Create date: <Mar 3rd 2015>
-- Description:	<Script to create mapping table for tUserRoles and tPermissions table>           
-- Jira ID:	<AL-88>
--===========================================================================================

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tUserRolesPermissionsMapping]') AND type in (N'U'))
DROP TABLE [dbo].[tUserRolesPermissionsMapping]
GO

CREATE TABLE tUserRolesPermissionsMapping
(
	rowguid UNIQUEIDENTIFIER,
	Id BIGINT NOT NULL IDENTITY(1000000000,1),
	RoleId INT NOT NULL,
	PermissionId UNIQUEIDENTIFIER NOT NULL,
	ChannelPartnerId UNIQUEIDENTIFIER NOT NULL,
	IsEnabled BIT NOT NULL DEFAULT 0,
	DTCreate DATETIME NOT NULL,
	DTLastMod DATETIME NULL,
	CONSTRAINT PK_tUserRolesPermissionsMapping PRIMARY KEY NONCLUSTERED (rowguid),
	CONSTRAINT FK_tUserRolesPermissionsMapping_tuserRoles FOREIGN KEY (RoleId) REFERENCES tuserRoles(Id),
	CONSTRAINT FK_tUserRolesPermissionsMapping_tPermissions FOREIGN KEY (PermissionId) REFERENCES tPermissions(rowguid),
	CONSTRAINT FK_tUserRolesPermissionsMapping_tChannelPartners FOREIGN KEY (ChannelPartnerId) REFERENCES tChannelPartners(rowguid)	
)
GO