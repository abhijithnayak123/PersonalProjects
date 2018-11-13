--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to alter tPartnerCustomerGroupSettings_Delete>           
-- Jira ID:	<AL-242>
--===========================================================================================

/****** Object:  Trigger [dbo].[tPartnerCustomerGroupSettings_Delete]    Script Date: 3/27/2015 2:22:25 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TRIGGER [dbo].[tPartnerCustomerGroupSettings_Delete] ON [dbo].[tPartnerCustomerGroupSettings] AFTER DELETE
AS
BEGIN
	SET NOCOUNT ON
	INSERT tPartnerCustomerGroupSettings_Aud(PCGroupSettingPK, PartnerCustomerPK, ChannelPartnerGroupId, DTCreate, DTLastMod, RevisionNo, AuditEvent, DTAudit)
	SELECT i.PCGroupSettingPK, PartnerCustomerPK, ChannelPartnerGroupId, DTCreate, DTLastMod, ISNULL(a.MaxRev,0) + 1, 3, GETDATE()
	FROM deleted i 
		LEFT OUTER JOIN (
			SELECT PCGroupSettingPK, MAX(RevisionNo) AS MaxRev
			FROM tPartnerCustomerGroupSettings_Aud
			GROUP BY PCGroupSettingPK
		) a ON i.PCGroupSettingPK = a.PCGroupSettingPK
END
GO


