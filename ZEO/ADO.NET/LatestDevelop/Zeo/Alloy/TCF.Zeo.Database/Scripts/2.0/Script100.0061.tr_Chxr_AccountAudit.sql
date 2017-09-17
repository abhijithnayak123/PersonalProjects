--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <07-20-2016>
-- Description:	 Alter Trigger for Chexar Account audit table
-- Jira ID:		<AL-7705>
-- ================================================================================


IF OBJECT_ID(N'trChxr_AccountAudit', N'TR') IS NOT NULL
DROP TRIGGER trChxr_AccountAudit   -- Drop the existing trigger.
GO


IF OBJECT_ID(N'tr_Chxr_Account_Aud', N'TR') IS NOT NULL
DROP TRIGGER tr_Chxr_Account_Aud
GO

CREATE TRIGGER tr_Chxr_Account_Aud ON tChxr_Account   
AFTER INSERT, UPDATE, DELETE
AS
	SET NOCOUNT ON
	DECLARE @RevisionNo BIGINT
	DECLARE @AuditEvent SMALLINT
		
	IF ((SELECT COUNT(1) FROM INSERTED) > 0 AND (SELECT COUNT(1) FROM DELETED) > 0)
	BEGIN
		-- UPDATE
		SET @AuditEvent = 2
	END
	ELSE IF ((SELECT COUNT(1) FROM INSERTED) > 0)
	BEGIN
		-- INSERT
		SET @AuditEvent = 1
	END	 
	ELSE IF ((SELECT COUNT(1) FROM DELETED) > 0)
	BEGIN
		-- DELETE
		SET @AuditEvent = 3
	END     
	
	IF(@AuditEvent != 3)
	BEGIN

		SELECT 
			@RevisionNo = ISNULL(MAX(RevisionNo), 0) + 1 
		FROM 
			tChxr_Account_Aud
		WHERE 
			ChxrAccountID = (SELECT ChxrAccountID FROM INSERTED)
         

		INSERT INTO 
		  tChxr_Account_Aud
		   (
			 ChxrAccountID,
			 Badge,
			 DTServerCreate,
			 DTServerLastModified,
			 RevisionNo,
			 AuditEvent,
			 DTAudit,
			 DTTerminalCreate,
			 DTTerminalLastModified,
			 CustomerID,
			 CustomerSessionID,
			 CustomerRevisionNo
		   )
		 SELECT 
			ChxrAccountID,
			Badge,
			DTServerCreate,
			DTServerLastModified,
			@revisionNo,
			@AuditEvent,
			GETDATE(), 
			DTTerminalCreate,
			DTTerminalLastModified,
			CustomerID,
			CustomerSessionID,
			CustomerRevisionNo
		FROM
		    INSERTED

     END
	 ELSE
	 BEGIN

	    SELECT 
			@RevisionNo = ISNULL(MAX(RevisionNo), 0) + 1 
		FROM 
			tChxr_Account_Aud
		WHERE 
			ChxrAccountID = (SELECT ChxrAccountID FROM DELETED)

 	    INSERT INTO 
		  tChxr_Account_Aud
		   (
			 ChxrAccountID,
			 Badge,
			 DTServerCreate,
			 DTServerLastModified,
			 RevisionNo,
			 AuditEvent,
			 DTAudit,
			 DTTerminalCreate,
			 DTTerminalLastModified,
			 CustomerID,
			 CustomerSessionID,
			 CustomerRevisionNo
		   )
		 SELECT 
			ChxrAccountID,
			Badge,
			DTServerCreate,
			DTServerLastModified,
			@revisionNo,
			@AuditEvent,
			GETDATE(), 
			DTTerminalCreate,
			DTTerminalLastModified,
			CustomerID,
			CustomerSessionID,
			CustomerRevisionNo
		FROM
		    DELETED
	END
GO	
	 