-- ============================================================
-- Author:		Manikandan Govindraj
-- Create date: <09/24/2015>
-- Description:	<Script for Altering trCustomerEmploymentDetailsAudit trigger>
-- Jira ID:		<AL-2018>
-- ============================================================

IF EXISTS
( 
	SELECT 1
	FROM SYS.TRIGGERS
	WHERE NAME = 'trCustomerEmploymentDetailsAudit'
)
BEGIN
	DROP TRIGGER [dbo].[trCustomerEmploymentDetailsAudit]
END
GO


CREATE TRIGGER [dbo].[trCustomerEmploymentDetailsAudit] ON [dbo].[tCustomerEmploymentDetails] AFTER INSERT, UPDATE, DELETE
AS
	   SET NOCOUNT ON;
       DECLARE @RevisionNo BIGINT
       SELECT @RevisionNo = isnull(MAX(RevisionNo),0) + 1 FROM tCustomerEmploymentDetails_Aud WHERE CustomerPK = (SELECT CustomerPK FROM inserted)
             
       if ((SELECT COUNT(*) FROM inserted)<>0 and (SELECT COUNT(*) FROM deleted)>0)
       BEGIN
              INSERT INTO tCustomerEmploymentDetails_Aud(
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
                     DTTerminalLastModified,
					 OccupationDescription)
              SELECT CustomerPK,Occupation,Employer,EmployerPhone,DTServerCreate,DTServerLastModified,@RevisionNo,2 as AuditEvent,GETDATE(),DTTerminalCreate,DTTerminalLastModified,OccupationDescription from inserted
       END
       ELSE IF(SELECT COUNT(*) FROM inserted)>0 and (SELECT COUNT(*) FROM deleted)=0
       BEGIN
              INSERT INTO tCustomerEmploymentDetails_Aud(
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
                     DTTerminalLastModified,
					 OccupationDescription)
              SELECT CustomerPK,Occupation,Employer,EmployerPhone,DTServerCreate,DTServerLastModified,@RevisionNo,1 AS AuditEvent,GETDATE(),DTTerminalCreate,DTTerminalLastModified,OccupationDescription FROM inserted
       END
       ELSE IF(SELECT COUNT(*) FROM deleted)>0
       BEGIN
              INSERT INTO tCustomerEmploymentDetails_Aud(
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
                     DTTerminalLastModified,
					 OccupationDescription)
              SELECT CustomerPK,Occupation,Employer,EmployerPhone,DTServerCreate,DTServerLastModified,@RevisionNo,3 AS AuditEvent,GETDATE(),DTTerminalCreate,DTTerminalLastModified,OccupationDescription FROM deleted
       END
GO


