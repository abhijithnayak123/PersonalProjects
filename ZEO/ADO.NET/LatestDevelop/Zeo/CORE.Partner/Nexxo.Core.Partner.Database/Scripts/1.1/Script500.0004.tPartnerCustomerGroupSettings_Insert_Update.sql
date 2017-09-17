--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to alter tPartnerCustomerGroupSettings_Insert_Update>           
-- Jira ID:	<AL-242>
--===========================================================================================

/****** Object:  Trigger [dbo].[tPartnerCustomerGroupSettings_Insert_Update]    Script Date: 3/27/2015 2:26:28 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TRIGGER [dbo].[tPartnerCustomerGroupSettings_Insert_Update] ON [dbo].[tPartnerCustomerGroupSettings] AFTER INSERT, UPDATE
AS
BEGIN
	DECLARE @auditEvent INT

	IF NOT EXISTS (SELECT * FROM deleted)
		SET @auditEvent = 1
	ELSE
		SET @auditEvent = 2

	SET NOCOUNT ON

	INSERT tPartnerCustomerGroupSettings_Aud(PCGroupSettingPK, PartnerCustomerPK, ChannelPartnerGroupId, DTCreate, DTLastMod, RevisionNo, AuditEvent, DTAudit)
	SELECT i.PCGroupSettingPK, PartnerCustomerPK, ChannelPartnerGroupId, DTCreate, DTLastMod, ISNULL(a.MaxRev,0) + 1, @auditEvent, GETDATE()
	FROM inserted i 
		LEFT OUTER JOIN (
			SELECT PCGroupSettingPK, MAX(RevisionNo) AS MaxRev
			FROM tPartnerCustomerGroupSettings_Aud
			GROUP BY PCGroupSettingPK
		) a ON i.PCGroupSettingPK = a.PCGroupSettingPK
END
GO


