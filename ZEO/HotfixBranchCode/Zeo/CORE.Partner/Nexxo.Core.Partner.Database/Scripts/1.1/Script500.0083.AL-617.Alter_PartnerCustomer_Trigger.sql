-- ================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <08/04/2015>
-- Description:	<Alter Trigger date columns to DTTerminalCreate, DTTerminalLastModified>
-- Jira ID:		<AL-617>
-- ================================================================================

IF EXISTS( 
	SELECT 1
	FROM SYS.TRIGGERS
	WHERE NAME = 'tPartnerCustomers_Audit'
)
BEGIN
	DROP TRIGGER [dbo].[tPartnerCustomers_Audit]
	END
GO


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
					DTServerCreate,
					DTServerLastModified,
					IsPartnerAccountHolder,
					ReferralCode,
					ChannelPartnerPK,
					RevisionNo,
					AuditEvent,
					DTAudit,AgentSessionPK,CustomerProfileStatus,
					DTTerminalCreate,DTTerminalLastModified)
							SELECT CustomerPK,
					Inserted.CustomerID,
					CXEId,
					DTServerCreate,
					DTServerLastModified,
					IsPartnerAccountHolder,
					ReferralCode,
					ChannelPartnerPK, @RevisionNo,2 as AuditEvent,GETDATE(),
					AgentSessionPK,CustomerProfileStatus,
					DTTerminalCreate,DTTerminalLastModified from inserted
       end
       ELSE IF(SELECT COUNT(*) FROM inserted)>0 AND (SELECT COUNT(*) FROM deleted)=0
       BEGIN
              INSERT INTO tPartnerCustomers_Aud(
					CustomerPK,
					CustomerID,
					CXEId,
					DTServerCreate,
					DTServerLastModified,
					IsPartnerAccountHolder,
					ReferralCode,
					ChannelPartnerPK,
					RevisionNo,
					AuditEvent,
					DTAudit,AgentSessionPK,CustomerProfileStatus,
					DTTerminalCreate,DTTerminalLastModified)
								  SELECT CustomerPK,
					CustomerID,
					CXEId,
					DTServerCreate,
					DTServerLastModified,
					IsPartnerAccountHolder,
					ReferralCode,
					ChannelPartnerPK, @RevisionNo,1 AS AuditEvent,GETDATE(),
					AgentSessionPK,CustomerProfileStatus, 
					DTTerminalCreate,DTTerminalLastModified FROM inserted
       END
       ELSE IF(SELECT COUNT(*) FROM deleted)>0
       BEGIN
              INSERT INTO tPartnerCustomers_Aud(
					CustomerPK,
					CustomerID,
					CXEId,
					DTServerCreate,
					DTServerLastModified,
					IsPartnerAccountHolder,
					ReferralCode,
					ChannelPartnerPK,
					RevisionNo,
					AuditEvent,
					DTAudit,AgentSessionPK,CustomerProfileStatus,
					DTTerminalCreate,DTTerminalLastModified)
								  SELECT CustomerPK,
					CustomerID,
					CXEId,
					DTServerCreate,
					DTServerLastModified,
					IsPartnerAccountHolder,
					ReferralCode,
					Deleted.ChannelPartnerPK, @RevisionNo,3 AS AuditEvent,GETDATE(),
					AgentSessionPK,CustomerProfileStatus,
					DTTerminalCreate,DTTerminalLastModified FROM deleted
       END
GO


