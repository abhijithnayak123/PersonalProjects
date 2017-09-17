--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <May 5th 2015>
-- Description:	<Script to update column names>           
-- Jira ID:	<AL-373>
--===========================================================================================

/****** Object:  Trigger [dbo].[tPartnerCustomerGroupSettings_Delete]    Script Date: 5/4/2015 10:27:58 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER TRIGGER [dbo].[tPartnerCustomerGroupSettings_Delete] ON [dbo].[tPartnerCustomerGroupSettings] AFTER DELETE
AS
BEGIN
	SET NOCOUNT ON
	insert tPartnerCustomerGroupSettings_Aud(PCGroupSettingPK, PartnerCustomerPK, ChannelPartnerGroupId, DTCreate, DTLastMod, RevisionNo, AuditEvent, DTAudit,ChannelPartnerGroupPK)
	select i.PCGroupSettingPK, PartnerCustomerPK, ChannelPartnerGroupId, DTCreate, DTLastMod, isnull(a.MaxRev,0) + 1, 3, GETDATE(),ChannelPartnerGroupPK
	from deleted i 
		left outer join (
			select PCGroupSettingPK, MAX(RevisionNo) as MaxRev
			from tPartnerCustomerGroupSettings_Aud
			group by PCGroupSettingPK
		) a on i.PCGroupSettingPK = a.PCGroupSettingPK
END

GO


