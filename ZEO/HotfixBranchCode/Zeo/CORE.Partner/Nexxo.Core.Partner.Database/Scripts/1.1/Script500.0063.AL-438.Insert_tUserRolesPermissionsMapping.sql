-- =============================================================================================
-- Author:		<Mohammed Khaja Firoz Molla>
-- Create date: <07/10/2015>
-- Description:	<Script for setting unpark permissions for a user roles for Redstone>
-- Jira ID:	<AL-438>
-- ==============================================================================================
DECLARE @RedStonePartnerId UNIQUEIDENTIFIER
SELECT @RedStonePartnerId = ChannelPartnerPK FROM tChannelPartners WHERE Name = 'Redstone'

DECLARE @canUnparkTransactionPermissionID UNIQUEIDENTIFIER
SELECT @canUnparkTransactionPermissionID = rowguid FROM tPermissions WHERE Permission = 'CanUnparkTransactions'

INSERT tUserRolesPermissionsMapping (rowguiD, RoleId, PermissionId, ChannelPartnerId, IsEnabled, DTServerCreate) VALUES
(NEWID(), 1, @canUnparkTransactionPermissionID, @RedStonePartnerId, 1, GETDATE()),
(NEWID(), 2, @canUnparkTransactionPermissionID, @RedStonePartnerId, 1, GETDATE()),
(NEWID(), 3, @canUnparkTransactionPermissionID, @RedStonePartnerId, 0, GETDATE()),
(NEWID(), 4, @canUnparkTransactionPermissionID, @RedStonePartnerId, 1, GETDATE())
GO