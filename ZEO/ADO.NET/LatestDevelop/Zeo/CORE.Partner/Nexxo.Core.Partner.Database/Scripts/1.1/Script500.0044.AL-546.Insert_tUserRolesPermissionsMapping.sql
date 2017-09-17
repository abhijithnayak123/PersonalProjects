--===========================================================================================
-- Author:		<Rogy Eapen>
-- Create date: <June 26 2015>
-- Description:	<Script to create rows for mapping table for tUserRoles and tPermissions table
--               for mapping the unpark transaction permissions to the Tech user role.>            
-- Jira ID:	<AL-546>
--===========================================================================================

DECLARE @techRole INT
SELECT @TechRole = id FROM tUserRoles WHERE Role = 'Tech'
DECLARE @CarverPartnerId UNIQUEIDENTIFIER
DECLARE @MGIPartnerId UNIQUEIDENTIFIER
DECLARE @RedStonePartnerId UNIQUEIDENTIFIER
DECLARE @SynovusPartnerId UNIQUEIDENTIFIER
DECLARE @TCFPartnerId UNIQUEIDENTIFIER

SELECT @CarverPartnerId = ChannelPartnerPK FROM tChannelPartners WHERE Name = 'Carver' 
SELECT @MGIPartnerId = ChannelPartnerPK FROM tChannelPartners WHERE Name = 'MGI'
SELECT @RedStonePartnerId = ChannelPartnerPK FROM tChannelPartners WHERE Name = 'Redstone'
SELECT @SynovusPartnerId = ChannelPartnerPK FROM tChannelPartners WHERE Name = 'Synovus'
SELECT @TCFPartnerId = ChannelPartnerPK FROM tChannelPartners WHERE Name = 'TCF'

DECLARE @canUnparkTransactionPermissionID UNIQUEIDENTIFIER
SELECT @canUnparkTransactionPermissionID = rowguid FROM tPermissions WHERE Permission = 'CanUnparkTransactions'

IF NOT EXISTS(SELECT 1 FROM tUserRolesPermissionsMapping WHERE [RoleId] = 5)
BEGIN
	INSERT INTO [dbo].[tUserRolesPermissionsMapping] ([rowguid], [RoleId], [PermissionId], [ChannelPartnerId], [IsEnabled], [DTCreate]) VALUES 
	(NEWID(), @TechRole, @canUnparkTransactionPermissionID, @CarverPartnerId, 0, GETDATE()),
    (NEWID(), @TechRole, @canUnparkTransactionPermissionID, @MGIPartnerId, 0, GETDATE()),
	(NEWID(), @TechRole, @canUnparkTransactionPermissionID, @RedStonePartnerId, 0, GETDATE()),
	(NEWID(), @TechRole, @canUnparkTransactionPermissionID, @SynovusPartnerId, 0, GETDATE()),
	(NEWID(), @TechRole, @canUnparkTransactionPermissionID, @TCFPartnerId, 0, GETDATE())
END
GO