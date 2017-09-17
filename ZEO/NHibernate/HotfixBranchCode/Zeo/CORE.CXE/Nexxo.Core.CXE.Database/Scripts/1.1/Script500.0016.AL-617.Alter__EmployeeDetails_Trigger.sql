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
	WHERE NAME = 'trCustomerEmploymentDetailsAudit'
)
BEGIN
	DROP trigger [dbo].[trCustomerEmploymentDetailsAudit]
	END
GO

create trigger [dbo].[trCustomerEmploymentDetailsAudit] on [dbo].[tCustomerEmploymentDetails] AFTER Insert, Update, Delete
AS
	   SET NOCOUNT ON;
       Declare @RevisionNo bigint
       select @RevisionNo = isnull(MAX(RevisionNo),0) + 1 from tCustomerEmploymentDetails_Aud where CustomerPK = (select CustomerPK from inserted)
             
       if ((select COUNT(*) from inserted)<>0 and (select COUNT(*) from deleted)>0)
       begin
              insert into tCustomerEmploymentDetails_Aud(
					 CustomerPK,
                     Occupation,
                     Employer,
                     EmployerPhone,
                     DTServerCreate,
                     DTServerLastModified,
                     RevisionNo,
                     AuditEvent,
                     DTAudit,
                     DTTerminalCreate,
                     DTTerminalLastModified)
              select CustomerPK,Occupation,Employer,EmployerPhone,DTServerCreate,DTServerLastModified,@RevisionNo,2 as AuditEvent,GETDATE(),DTTerminalCreate,DTTerminalLastModified from inserted
       end
       else if(select COUNT(*) from inserted)>0 and (select COUNT(*) from deleted)=0
       begin
              insert into tCustomerEmploymentDetails_Aud(
					 CustomerPK,
                     Occupation,
                     Employer,
                     EmployerPhone,
                     DTServerCreate,
                     DTServerLastModified,
                     RevisionNo,
                     AuditEvent,
                     DTAudit,
                     DTTerminalCreate,
                     DTTerminalLastModified)
              select CustomerPK,Occupation,Employer,EmployerPhone,DTServerCreate,DTServerLastModified,@RevisionNo,1 as AuditEvent,GETDATE(),DTTerminalCreate,DTTerminalLastModified from inserted
       end
       else if(select COUNT(*) from deleted)>0
       begin
              insert into tCustomerEmploymentDetails_Aud(
					 CustomerPK,
                     Occupation,
                     Employer,
                     EmployerPhone,
                     DTServerCreate,
                     DTServerLastModified,
                     RevisionNo,
                     AuditEvent,
                     DTAudit,
                     DTTerminalCreate,
                     DTTerminalLastModified)
              select CustomerPK,Occupation,Employer,EmployerPhone,DTServerCreate,DTServerLastModified,@RevisionNo,3 as AuditEvent,GETDATE(),DTTerminalCreate,DTTerminalLastModified from deleted
       end
GO


