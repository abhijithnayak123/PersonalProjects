-- Author: suneel 
-- Date Created: 19 May 2014
-- Description: Creating the CustomerProfileStatus in tPartnerCustomers_Aud
-- and Update the tPartnerCustomers_Audit trigger
-- User Story ID: US1991

-- Add CustomerProfileStatus column in tPartnerCustomers_Aud
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tPartnerCustomers_Aud]') AND type in (N'U'))
 BEGIN  
  IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'CustomerProfileStatus' AND [object_id] = OBJECT_ID(N'[dbo].[tPartnerCustomers_Aud]'))
  BEGIN    
   ALTER TABLE dbo.[tPartnerCustomers_Aud] ADD [CustomerProfileStatus] [bit] NULL
  END
 END

GO

-- Drop the tPartnerCustomers_Audit trigger
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[tPartnerCustomers_Audit]'))
	DROP TRIGGER [dbo].[tPartnerCustomers_Audit]
GO


--Update the tPartnerCustomers table CustomerProfileStatus column
--Update based on CXE Status
UPDATE
    tPartnerCustomers
SET
    tPartnerCustomers.CustomerProfileStatus = sCustomer.profileStatus
FROM
    tPartnerCustomers
INNER JOIN
    sCustomer
ON
    tPartnerCustomers.CXEId = sCustomer.id
GO

--- Update the  tPartnerCustomers_Audit table  trigger 
 
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
					DTAudit,AgentSessionId,CustomerProfileStatus
										 )
								  select rowguid,
					Id,
					CXEId,
					DTCreate,
					DTLastMod,
					IsPartnerAccountHolder,
					ReferralCode,
					ChannelPartnerId, @RevisionNo,2 as AuditEvent,GETDATE(),
					AgentSessionId,CustomerProfileStatus from inserted
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
					DTAudit,AgentSessionId,CustomerProfileStatus)
								  select rowguid,
					Id,
					CXEId,
					DTCreate,
					DTLastMod,
					IsPartnerAccountHolder,
					ReferralCode,
					ChannelPartnerId, @RevisionNo,1 as AuditEvent,GETDATE(),
					AgentSessionId,CustomerProfileStatus from inserted
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
					DTAudit,AgentSessionId,CustomerProfileStatus)
								  select rowguid,
					Id,
					CXEId,
					DTCreate,
					DTLastMod,
					IsPartnerAccountHolder,
					ReferralCode,
					ChannelPartnerId, @RevisionNo,3 as AuditEvent,GETDATE(),
					AgentSessionId,CustomerProfileStatus from  deleted
       end
	 
Go

