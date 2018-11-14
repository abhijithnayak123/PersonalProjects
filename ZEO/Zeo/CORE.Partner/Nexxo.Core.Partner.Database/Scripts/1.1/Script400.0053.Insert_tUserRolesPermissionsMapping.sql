--===========================================================================================
-- Author:		<Mohammed Khaja Firoz Molla>
-- Create date: <Mar 3rd 2015>
-- Description:	<Script to create rows for mapping table for tUserRoles and tPermissions table
--               for mapping the unpark transaction permissions to the user roles.>            
-- Jira ID:	<AL-88>
--===========================================================================================

DECLARE @tellerRole INT
DECLARE @managerRole INT
DECLARE @complianceManagerRole INT
DECLARE @systemAdminRole INT

SELECT @tellerRole = id FROM tUserRoles WHERE Role = 'Teller'
SELECT @managerRole = id FROM tUserRoles WHERE Role = 'Manager'
SELECT @complianceManagerRole = id  FROM tUserRoles WHERE Role = 'Compliance Manager'
SELECT @systemAdminRole =id FROM tUserRoles WHERE Role = 'System Admin'

DECLARE @TCFId UNIQUEIDENTIFIER
DECLARE @SynovusId UNIQUEIDENTIFIER
DECLARE @CarverId UNIQUEIDENTIFIER
DECLARE @MGIId UNIQUEIDENTIFIER

SELECT @TCFId = rowguid FROM tChannelPartners WHERE Name = 'TCF'
SELECT @SynovusId = rowguid FROM tChannelPartners WHERE Name = 'Synovus'
SELECT @CarverId = rowguid FROM tChannelPartners WHERE Name = 'Carver' 
SELECT @MGIId = rowguid FROM tChannelPartners WHERE Name = 'MGI'

DECLARE @canUnparktransactionPermissionID UNIQUEIDENTIFIER
SELECT @canUnparktransactionPermissionID = rowguid FROM tPermissions WHERE Permission = 'CanUnparkTransactions'

INSERT INTO [dbo].[tUserRolesPermissionsMapping] ([rowguid], [RoleId], [PermissionId], [ChannelPartnerId], [IsEnabled], [DTCreate])
     VALUES (NEWID(), @tellerRole , @canUnparktransactionPermissionID, @TCFId, 1, GETDATE())
INSERT INTO [dbo].[tUserRolesPermissionsMapping] ([rowguid], [RoleId], [PermissionId], [ChannelPartnerId], [IsEnabled], [DTCreate])
     VALUES (NEWID(), @managerRole , @canUnparktransactionPermissionID, @TCFId, 1, GETDATE())
INSERT INTO [dbo].[tUserRolesPermissionsMapping] ([rowguid], [RoleId], [PermissionId], [ChannelPartnerId], [IsEnabled], [DTCreate])
     VALUES (NEWID(), @complianceManagerRole, @canUnparktransactionPermissionID, @TCFId, 0, GETDATE())
INSERT INTO [dbo].[tUserRolesPermissionsMapping] ([rowguid], [RoleId], [PermissionId], [ChannelPartnerId], [IsEnabled], [DTCreate])
     VALUES (NEWID(), @systemAdminRole, @canUnparktransactionPermissionID, @TCFId, 1, GETDATE())

	 
INSERT INTO [dbo].[tUserRolesPermissionsMapping] ([rowguid], [RoleId], [PermissionId], [ChannelPartnerId], [IsEnabled], [DTCreate])
     VALUES (NEWID(), @tellerRole , @canUnparktransactionPermissionID, @CarverId, 1, GETDATE())
INSERT INTO [dbo].[tUserRolesPermissionsMapping] ([rowguid], [RoleId], [PermissionId], [ChannelPartnerId], [IsEnabled], [DTCreate])
     VALUES (NEWID(), @managerRole , @canUnparktransactionPermissionID, @CarverId, 1, GETDATE())
INSERT INTO [dbo].[tUserRolesPermissionsMapping] ([rowguid], [RoleId], [PermissionId], [ChannelPartnerId], [IsEnabled], [DTCreate])
     VALUES (NEWID(), @complianceManagerRole, @canUnparktransactionPermissionID, @CarverId, 0, GETDATE())
INSERT INTO [dbo].[tUserRolesPermissionsMapping] ([rowguid], [RoleId], [PermissionId], [ChannelPartnerId], [IsEnabled], [DTCreate])
     VALUES (NEWID(), @systemAdminRole, @canUnparktransactionPermissionID, @CarverId, 1, GETDATE())


INSERT INTO [dbo].[tUserRolesPermissionsMapping] ([rowguid], [RoleId], [PermissionId], [ChannelPartnerId], [IsEnabled], [DTCreate])
     VALUES (NEWID(), @tellerRole , @canUnparktransactionPermissionID, @SynovusId, 1, GETDATE())
INSERT INTO [dbo].[tUserRolesPermissionsMapping] ([rowguid], [RoleId], [PermissionId], [ChannelPartnerId], [IsEnabled], [DTCreate])
     VALUES (NEWID(), @managerRole , @canUnparktransactionPermissionID, @SynovusId, 1, GETDATE())
INSERT INTO [dbo].[tUserRolesPermissionsMapping] ([rowguid], [RoleId], [PermissionId], [ChannelPartnerId], [IsEnabled], [DTCreate])
     VALUES (NEWID(), @complianceManagerRole, @canUnparktransactionPermissionID, @SynovusId, 0, GETDATE())
INSERT INTO [dbo].[tUserRolesPermissionsMapping] ([rowguid], [RoleId], [PermissionId], [ChannelPartnerId], [IsEnabled], [DTCreate])
     VALUES (NEWID(), @systemAdminRole, @canUnparktransactionPermissionID, @SynovusId, 1, GETDATE())

INSERT INTO [dbo].[tUserRolesPermissionsMapping] ([rowguid], [RoleId], [PermissionId], [ChannelPartnerId], [IsEnabled], [DTCreate])
     VALUES (NEWID(), @tellerRole , @canUnparktransactionPermissionID, @MGIId, 1, GETDATE())
INSERT INTO [dbo].[tUserRolesPermissionsMapping] ([rowguid], [RoleId], [PermissionId], [ChannelPartnerId], [IsEnabled], [DTCreate])
     VALUES (NEWID(), @managerRole , @canUnparktransactionPermissionID, @MGIId, 1, GETDATE())
INSERT INTO [dbo].[tUserRolesPermissionsMapping] ([rowguid], [RoleId], [PermissionId], [ChannelPartnerId], [IsEnabled], [DTCreate])
     VALUES (NEWID(), @complianceManagerRole, @canUnparktransactionPermissionID, @MGIId, 0, GETDATE())
INSERT INTO [dbo].[tUserRolesPermissionsMapping] ([rowguid], [RoleId], [PermissionId], [ChannelPartnerId], [IsEnabled], [DTCreate])
     VALUES (NEWID(), @systemAdminRole, @canUnparktransactionPermissionID, @MGIId, 1, GETDATE())

GO