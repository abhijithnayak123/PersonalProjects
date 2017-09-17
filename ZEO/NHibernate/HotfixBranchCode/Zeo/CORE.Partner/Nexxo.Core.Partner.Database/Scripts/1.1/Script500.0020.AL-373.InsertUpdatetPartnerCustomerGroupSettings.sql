--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <May 5th 2015>
-- Description:	<Script to update column names>           
-- Jira ID:	<AL-373>
--===========================================================================================

/****** Object:  Trigger [dbo].[tPartnerCustomerGroupSettings_Insert_Update]    Script Date: 5/4/2015 10:29:16 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER TRIGGER [dbo].[tPartnerCustomerGroupSettings_Insert_Update] ON [dbo].[tPartnerCustomerGroupSettings] AFTER INSERT, UPDATE
AS
BEGIN
	declare @auditEvent int

	if not exists (select * from deleted)
		set @auditEvent = 1
	else
		set @auditEvent = 2

	SET NOCOUNT ON

	insert tPartnerCustomerGroupSettings_Aud(PCGroupSettingPK, PartnerCustomerPK, ChannelPartnerGroupId, DTCreate, DTLastMod, RevisionNo, AuditEvent, DTAudit,ChannelPartnerGroupPK)
	select i.PCGroupSettingPK, PartnerCustomerPK, ChannelPartnerGroupId, DTCreate, DTLastMod, isnull(a.MaxRev,0) + 1, @auditEvent, GETDATE(),ChannelPartnerGroupPK
	from inserted i 
		left outer join (
			select PCGroupSettingPK, MAX(RevisionNo) as MaxRev
			from tPartnerCustomerGroupSettings_Aud
			group by PCGroupSettingPK
		) a on i.PCGroupSettingPK = a.PCGroupSettingPK
END

GO


