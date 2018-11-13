-- ================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <03/08/2015>
-- Description:	<Alter Trigger to Add DTTerminalCreate, DTTerminalLastModified, 
--					DTServerCreate, DTServerLastModified.>
-- Jira ID:		<AL-617>
-- ================================================================================
IF EXISTS( 
	SELECT 1
	FROM SYS.TRIGGERS
	WHERE NAME = 'trCustomerGovernmentIdDetailsAudit'
)
BEGIN
	DROP trigger [dbo].[trCustomerGovernmentIdDetailsAudit]
	END
GO

create trigger [dbo].[trCustomerGovernmentIdDetailsAudit] on [dbo].[tCustomerGovernmentIdDetails] AFTER Insert, Update, Delete
AS
       SET NOCOUNT ON;  
       if ((select COUNT(*) from inserted)<>0 and (select COUNT(*) from deleted)>0)
       begin
              insert into tCustomerGovernmentIdDetails_Aud(
					 CustomerPK,
                     IdTypeId,
                     Identification,
                     ExpirationDate,
                     DTServerCreate,
                     DTServerLastModified,
                     IssueDate,
                     RevisionNo,
                     AuditEvent,
                     DTAudit,
                     DTTerminalCreate,
                     DTTerminalLastModified)             
              select i.CustomerPK,i.IdTypeId,i.Identification,i.ExpirationDate,i.DTServerCreate,i.DTServerLastModified,i.IssueDate,
              isnull(aud.RevisionNo,1),2 as AuditEvent,GETDATE(),DTTerminalCreate,DTTerminalLastModified from 
              (select isnull(MAX(RevisionNo),0) + 1 as RevisionNo, CustomerPK from tCustomerGovernmentIdDetails_Aud 
              group by CustomerPK)aud right outer join inserted i on aud.CustomerPK = i.CustomerPK  
       end
       else if(select COUNT(*) from inserted)>0 and (select COUNT(*) from deleted)=0
       begin
              insert into tCustomerGovernmentIdDetails_Aud(
					 CustomerPK,
                     IdTypeId,
                     Identification,
                     ExpirationDate,
                     DTServerCreate,
                     DTServerLastModified,
                     IssueDate,
                     RevisionNo,
                     AuditEvent,
                     DTAudit,
                     DTTerminalCreate,
                     DTTerminalLastModified)             
              select i.CustomerPK,i.IdTypeId,i.Identification,i.ExpirationDate,i.DTServerCreate,i.DTServerLastModified,i.IssueDate,
              isnull(aud.RevisionNo,1),1 as AuditEvent,GETDATE(),DTTerminalCreate,DTTerminalLastModified from 
              (select isnull(MAX(RevisionNo),0) + 1 as RevisionNo, CustomerPK from tCustomerGovernmentIdDetails_Aud 
              group by CustomerPK)aud right outer join inserted i on aud.CustomerPK = i.CustomerPK  
              
       end
       else if(select COUNT(*) from deleted)>0
       begin
              insert into tCustomerGovernmentIdDetails_Aud(
					 CustomerPK,
                     IdTypeId,
                     Identification,
                     ExpirationDate,
                     DTServerCreate,
                     DTServerLastModified,
                     IssueDate,
                     RevisionNo,
                     AuditEvent,
                     DTAudit,
                     DTTerminalCreate,
                     DTTerminalLastModified)
			  select i.CustomerPK,i.IdTypeId,i.Identification,i.ExpirationDate,i.DTServerCreate,i.DTServerLastModified,i.IssueDate,
              isnull(aud.RevisionNo,1),3 as AuditEvent,GETDATE(),DTTerminalCreate,DTTerminalLastModified from 
              (select isnull(MAX(RevisionNo),0) + 1 as RevisionNo, CustomerPK from tCustomerGovernmentIdDetails_Aud 
              group by CustomerPK)aud right outer join inserted i on aud.CustomerPK = i.CustomerPK  
       end
GO


