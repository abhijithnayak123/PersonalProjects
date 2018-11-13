if exists (select * from dbo.sysobjects where name = 'tPartnerCustomers_Audit')
DROP TRIGGER [dbo].[tPartnerCustomers_Audit]
GO

CREATE TRIGGER [dbo].[tPartnerCustomers_Audit] on [dbo].[tPartnerCustomers] AFTER Insert, Update, Delete
AS
       SET NOCOUNT ON
       Declare @RevisionNo bigint
       select @RevisionNo = isnull(MAX(RevisionNo),0) + 1 from tPartnerCustomers_Aud where Id = (select Id from inserted)
             
       if ((select COUNT(*) from inserted)<>0 and (select COUNT(*) from deleted)>0)
       begin
              insert into tPartnerCustomers_Aud(
					rowguid,
Id,
CXEId,
DTCreate,
DTLastMod,
IsPartnerAccountHolder,
ReferralCode,
ChannelPartnerId,
RevisionNo,
AuditEvent,
DTAudit,AgentSessionId
                     )
              select rowguid,
Id,
CXEId,
DTCreate,
DTLastMod,
IsPartnerAccountHolder,
ReferralCode,
ChannelPartnerId, @RevisionNo,2 as AuditEvent,GETDATE(),
AgentSessionId from inserted
       end
       else if(select COUNT(*) from inserted)>0 and (select COUNT(*) from deleted)=0
       begin
              insert into tPartnerCustomers_Aud(
					rowguid,
Id,
CXEId,
DTCreate,
DTLastMod,
IsPartnerAccountHolder,
ReferralCode,
ChannelPartnerId,
RevisionNo,
AuditEvent,
DTAudit,AgentSessionId)
              select rowguid,
Id,
CXEId,
DTCreate,
DTLastMod,
IsPartnerAccountHolder,
ReferralCode,
ChannelPartnerId, @RevisionNo,1 as AuditEvent,GETDATE(),
AgentSessionId from inserted
       end
       else if(select COUNT(*) from deleted)>0
       begin
              insert into tPartnerCustomers_Aud(
					rowguid,
Id,
CXEId,
DTCreate,
DTLastMod,
IsPartnerAccountHolder,
ReferralCode,
ChannelPartnerId,
RevisionNo,
AuditEvent,
DTAudit,AgentSessionId)
			  select rowguid,
Id,
CXEId,
DTCreate,
DTLastMod,
IsPartnerAccountHolder,
ReferralCode,
ChannelPartnerId, @RevisionNo,3 as AuditEvent,GETDATE(),
AgentSessionId from  deleted
       end

GO


