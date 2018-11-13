CREATE TABLE [dbo].[tPartnerCustomerGroupSettings_Aud](
	[rowguid] [uniqueidentifier] NOT NULL,
	[PartnerCustomerPK] [uniqueidentifier] NOT NULL,
	[ChannelPartnerGroupId] [int] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
	[AuditEvent] [smallint] NOT NULL,
	[DTAudit] [datetime] NOT NULL,
	[RevisionNo] [bigint] NULL
) ON [PRIMARY]
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

	insert tPartnerCustomerGroupSettings_Aud(rowguid, PartnerCustomerPK, ChannelPartnerGroupId, DTCreate, DTLastMod, RevisionNo, AuditEvent, DTAudit)
	select i.rowguid, PartnerCustomerPK, ChannelPartnerGroupId, DTCreate, DTLastMod, isnull(a.MaxRev,0) + 1, @auditEvent, GETDATE()
	from inserted i 
		left outer join (
			select rowguid, MAX(RevisionNo) as MaxRev
			from tPartnerCustomerGroupSettings_Aud
			group by rowguid
		) a on i.rowguid = a.rowguid
END
GO

CREATE TRIGGER [dbo].[tPartnerCustomerGroupSettings_Delete] ON [dbo].[tPartnerCustomerGroupSettings] AFTER DELETE
AS
BEGIN
	SET NOCOUNT ON
	insert tPartnerCustomerGroupSettings_Aud(rowguid, PartnerCustomerPK, ChannelPartnerGroupId, DTCreate, DTLastMod, RevisionNo, AuditEvent, DTAudit)
	select i.rowguid, PartnerCustomerPK, ChannelPartnerGroupId, DTCreate, DTLastMod, isnull(a.MaxRev,0) + 1, 3, GETDATE()
	from deleted i 
		left outer join (
			select rowguid, MAX(RevisionNo) as MaxRev
			from tPartnerCustomerGroupSettings_Aud
			group by rowguid
		) a on i.rowguid = a.rowguid
END
GO