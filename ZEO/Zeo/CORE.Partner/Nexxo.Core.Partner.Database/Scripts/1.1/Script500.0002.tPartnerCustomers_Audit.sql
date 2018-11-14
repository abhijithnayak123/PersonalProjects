--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to alter trigger tPartnerCustomers_Audit>           
-- Jira ID:	<AL-242>
--===========================================================================================

/****** Object:  Trigger [dbo].[tPartnerCustomers_Audit]    Script Date: 3/27/2015 2:30:01 PM ******/

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tPartnerCustomers_Aud]') AND type in (N'U'))
 BEGIN  
  IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'CustomerProfileStatus' AND [object_id] = OBJECT_ID(N'[dbo].[tPartnerCustomers_Aud]'))
  BEGIN    
   ALTER TABLE dbo.[tPartnerCustomers_Aud] ADD [CustomerProfileStatus] [SMALLINT] NULL
  END
 END

GO

IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[tPartnerCustomers_Audit]'))
	DROP TRIGGER [dbo].[tPartnerCustomers_Audit]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--- Update the  tPartnerCustomers_Audit table  trigger 
 
CREATE TRIGGER [dbo].[tPartnerCustomers_Audit] ON [dbo].[tPartnerCustomers] AFTER INSERT, UPDATE, DELETE
AS
       SET NOCOUNT ON
       Declare @RevisionNo bigint
       select @RevisionNo = isnull(MAX(RevisionNo),0) + 1 from tPartnerCustomers_Aud where CustomerID = (select CustomerID from inserted)
             
       IF ((SELECT COUNT(*) FROM inserted)<>0 AND (SELECT COUNT(*) FROM deleted)>0)
       begin
              insert into tPartnerCustomers_Aud(
					CustomerPK,
					CustomerID,
					CXEId,
					DTCreate,
					DTLastMod,
					IsPartnerAccountHolder,
					ReferralCode,
					ChannelPartnerPK,
					RevisionNo,
					AuditEvent,
					DTAudit,AgentSessionPK,CustomerProfileStatus
										 )
								  select CustomerPK,
					Inserted.CustomerID,
					CXEId,
					DTCreate,
					DTLastMod,
					IsPartnerAccountHolder,
					ReferralCode,
					ChannelPartnerPK, @RevisionNo,2 as AuditEvent,GETDATE(),
					AgentSessionPK,CustomerProfileStatus from inserted
       end
       ELSE IF(SELECT COUNT(*) FROM inserted)>0 AND (SELECT COUNT(*) FROM deleted)=0
       BEGIN
              INSERT INTO tPartnerCustomers_Aud(
					CustomerPK,
					CustomerID,
					CXEId,
					DTCreate,
					DTLastMod,
					IsPartnerAccountHolder,
					ReferralCode,
					ChannelPartnerPK,
					RevisionNo,
					AuditEvent,
					DTAudit,AgentSessionPK,CustomerProfileStatus)
								  SELECT CustomerPK,
					CustomerID,
					CXEId,
					DTCreate,
					DTLastMod,
					IsPartnerAccountHolder,
					ReferralCode,
					ChannelPartnerPK, @RevisionNo,1 AS AuditEvent,GETDATE(),
					AgentSessionPK,CustomerProfileStatus FROM inserted
       END
       ELSE IF(SELECT COUNT(*) FROM deleted)>0
       BEGIN
              INSERT INTO tPartnerCustomers_Aud(
					CustomerPK,
					CustomerID,
					CXEId,
					DTCreate,
					DTLastMod,
					IsPartnerAccountHolder,
					ReferralCode,
					ChannelPartnerPK,
					RevisionNo,
					AuditEvent,
					DTAudit,AgentSessionPK,CustomerProfileStatus)
								  SELECT CustomerPK,
					CustomerID,
					CXEId,
					DTCreate,
					DTLastMod,
					IsPartnerAccountHolder,
					ReferralCode,
					Deleted.ChannelPartnerPK, @RevisionNo,3 AS AuditEvent,GETDATE(),
					AgentSessionPK,CustomerProfileStatus FROM  deleted
       END
GO


