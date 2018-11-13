-- ================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <08/04/2015>
-- Description:	<Alter Trigger date columns to DTTerminalCreate, DTTerminalLastModified,
--					 DTServerCreate, DTServerLastModified. >
-- Jira ID:		<AL-617>
-- ================================================================================
IF EXISTS( 
	SELECT 1
	FROM SYS.TRIGGERS
	WHERE NAME in ('tPartnerCustomerGroupSettings_Insert_Update','tPartnerCustomerGroupSettings_Delete')
)
BEGIN
	DROP TRIGGER [dbo].[tPartnerCustomerGroupSettings_Insert_Update]


	DROP TRIGGER [dbo].[tPartnerCustomerGroupSettings_Delete]

END
GO

CREATE TRIGGER [dbo].[tPartnerCustomerGroupSettings_Insert_Update] ON [dbo].[tPartnerCustomerGroupSettings] AFTER INSERT, UPDATE
AS
BEGIN
	declare @auditEvent int

	if not exists (select * from deleted)
		set @auditEvent = 1
	else
		set @auditEvent = 2

	SET NOCOUNT ON

	insert tPartnerCustomerGroupSettings_Aud(PCGroupSettingPK, PartnerCustomerPK, ChannelPartnerGroupId, DTServerCreate, DTServerLastModified, RevisionNo, AuditEvent, DTAudit,ChannelPartnerGroupPK,DTTerminalCreate,DTTerminalLastModified)
	select i.PCGroupSettingPK, PartnerCustomerPK, ChannelPartnerGroupId, DTServerCreate, DTServerLastModified, isnull(a.MaxRev,0) + 1, @auditEvent, GETDATE(),ChannelPartnerGroupPK,DTTerminalCreate,DTTerminalLastModified 
	from inserted i 
		left outer join (
			select PCGroupSettingPK, MAX(RevisionNo) as MaxRev
			from tPartnerCustomerGroupSettings_Aud
			group by PCGroupSettingPK
		) a on i.PCGroupSettingPK = a.PCGroupSettingPK
END
GO

CREATE TRIGGER [dbo].[tPartnerCustomerGroupSettings_Delete] ON [dbo].[tPartnerCustomerGroupSettings] AFTER DELETE
AS
BEGIN
	SET NOCOUNT ON
	insert tPartnerCustomerGroupSettings_Aud(PCGroupSettingPK, PartnerCustomerPK, ChannelPartnerGroupId, DTServerCreate, DTServerLastModified, RevisionNo, AuditEvent, DTAudit,ChannelPartnerGroupPK,DTTerminalCreate,DTTerminalLastModified)
	select i.PCGroupSettingPK, PartnerCustomerPK, ChannelPartnerGroupId, DTServerCreate, DTServerLastModified, isnull(a.MaxRev,0) + 1, 3, GETDATE(),ChannelPartnerGroupPK,DTTerminalCreate,DTTerminalLastModified
	from deleted i 
		left outer join (
			select PCGroupSettingPK, MAX(RevisionNo) as MaxRev
			from tPartnerCustomerGroupSettings_Aud
			group by PCGroupSettingPK
		) a on i.PCGroupSettingPK = a.PCGroupSettingPK
END
GO

