IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[trCustomerGovernmentIdDetailsAudit]') AND type in (N'TR'))
DROP TRIGGER [dbo].[trCustomerGovernmentIdDetailsAudit]
GO
create trigger trCustomerGovernmentIdDetailsAudit on tCustomerGovernmentIdDetails AFTER Insert, Update, Delete
AS
       SET NOCOUNT ON  
	        
             
       if ((select COUNT(*) from inserted)<>0 and (select COUNT(*) from deleted)>0)
       begin
              insert into tCustomerGovernmentIdDetails_Aud(
					 CustomerPK,
                     IdTypeId,
                     Identification,
                     ExpirationDate,
                     DTCreate,
                     DTLastMod,
                     IssueDate,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)             
              select i.CustomerPK,i.IdTypeId,i.Identification,i.ExpirationDate,i.DTCreate,i.DTLastMod,i.IssueDate,
              isnull(aud.RevisionNo,1),2 as AuditEvent,GETDATE()from 
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
                     DTCreate,
                     DTLastMod,
                     IssueDate,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)             
              select i.CustomerPK,i.IdTypeId,i.Identification,i.ExpirationDate,i.DTCreate,i.DTLastMod,i.IssueDate,
              isnull(aud.RevisionNo,1),1 as AuditEvent,GETDATE() from 
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
                     DTCreate,
                     DTLastMod,
                     IssueDate,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
			  select i.CustomerPK,i.IdTypeId,i.Identification,i.ExpirationDate,i.DTCreate,i.DTLastMod,i.IssueDate,
              isnull(aud.RevisionNo,1),3 as AuditEvent,GETDATE() from 
              (select isnull(MAX(RevisionNo),0) + 1 as RevisionNo, CustomerPK from tCustomerGovernmentIdDetails_Aud 
              group by CustomerPK)aud right outer join inserted i on aud.CustomerPK = i.CustomerPK  
       end
 