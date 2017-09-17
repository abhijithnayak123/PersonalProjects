--- ===============================================================================
-- Author:		<Purna Pushkal>
-- Create date: <18-11-2016>
-- Description:	 pupulate the data to audit table
-- Jira ID:		<AL-8320>
-- ================================================================================

IF OBJECT_ID('tr_PopulateWUBillPayAccount_Aud') IS NOT NULL
	 BEGIN
		  DROP TRIGGER dbo.tr_PopulateWUBillPayAccount_Aud
	 END
GO

CREATE TRIGGER tr_PopulateWUBillPayAccount_Aud ON dbo.tWUnion_BillPay_Account
AFTER INSERT, UPDATE, DELETE
AS
BEGIN

	 SET NOCOUNT ON;
	 DECLARE @revisionNo BIGINT;
	 DECLARE @auditEvent SMALLINT;

	 IF(SELECT COUNT(1) FROM dbo.tWUnion_BillPay_Account_Aud wba INNER JOIN INSERTED i on i.WUBillPayAccountID = wba.WUBillPayAccountId) > 0
		  BEGIN
				SELECT @revisionNo = ISNULL(MAX(RevisionNo), 0) + 1
				FROM tWUnion_BillPay_Account_Aud wba
				INNER JOIN INSERTED i ON wba.WUBillPayAccountId = i.WUBillPayAccountID				
		  END
	 ELSE
		  BEGIN
				SELECT @revisionNo = 1
		  END

	 IF((SELECT COUNT(1) FROM INSERTED) > 0 AND (SELECT COUNT(1) FROM DELETED) > 0)
		  BEGIN
				-- UPDATE
				SET @auditEvent = 2
		  END
	 ELSE
	 IF((SELECT COUNT(1) FROM INSERTED) > 0)
		  BEGIN
				-- INSERT
				SET @auditEvent = 1
		  END
	 ELSE
	 IF((SELECT COUNT(1) FROM DELETED) > 0)
		  BEGIN
				-- DELETE
				SET @auditEvent = 3
		  END

     IF(@auditEvent != 3)
	 BEGIN
		 INSERT INTO dbo.tWUnion_BillPay_Account_Aud
		 (WUBillPayAccountId,
		  DTTerminalCreate,
		  DTTerminalLastModified,
		  CardNumber,
		  PreferredCustomerLevelCode,
		  SmsNotificationFlag,
		  DTServerCreate,
		  DTServerLastModified,
		  CustomerId,
		  CustomerSessionId,
		  DTAudit,
		  AuditEvent,
		  RevisionNo,
		  CustomerRevisionNo
		 )
				  SELECT WUBillPayAccountID,
							DTTerminalCreate,
							DTTerminalLastModified,
							CardNumber,
							PreferredCustomerLevelCode,
							SmsNotificationFlag,
							DTServerCreate,
							DTServerLastModified,
							CustomerId ,
							CustomerSessionId,
							GETDATE(),
							@auditEvent,
							@revisionNo,
							CustomerRevisionNo
				  FROM 
				    INSERTED
	END
	ELSE
	 BEGIN

			INSERT INTO dbo.tWUnion_BillPay_Account_Aud
				 (WUBillPayAccountId,
				  DTTerminalCreate,
				  DTTerminalLastModified,
				  CardNumber,
				  PreferredCustomerLevelCode,
				  SmsNotificationFlag,
				  DTServerCreate,
				  DTServerLastModified,
				  CustomerId,
				  CustomerSessionId,
				  DTAudit,
				  AuditEvent,
				  RevisionNo,
				  CustomerRevisionNo
				 )
						  SELECT WUBillPayAccountID,
									DTTerminalCreate,
									DTTerminalLastModified,
									CardNumber,
									PreferredCustomerLevelCode,
									SmsNotificationFlag,
									DTServerCreate,
									DTServerLastModified,
									CustomerId ,
									CustomerSessionId,
									GETDATE(),
									@auditEvent,
									@revisionNo,
									CustomerRevisionNo
						  FROM 
						     DELETED
    END
END
GO