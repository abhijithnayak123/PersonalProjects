-- ============================================================
-- Author:		Manikandan Govindraj
-- Create date: <09/24/2015>
-- Description:	<Script for Altering trgTxn_Check_StageAudit trigger>
-- Jira ID:		<AL-2018>
-- ============================================================

IF EXISTS
( 
	SELECT 1
	FROM SYS.TRIGGERS
	WHERE NAME = 'trgTxn_Check_StageAudit'
)
BEGIN
	DROP TRIGGER [dbo].[trgTxn_Check_StageAudit]
END
GO


CREATE TRIGGER [dbo].[trgTxn_Check_StageAudit] 
	ON [dbo].[tTxn_Check_Stage] 
	AFTER INSERT, UPDATE, DELETE
AS
	SET NOCOUNT ON
	DECLARE @RevisionNo BIGINT
	DECLARE @AuditEvent SMALLINT

	SELECT 
		@RevisionNo = ISNULL(MAX(RevisionNo), 0) + 1 
	FROM 
		tTxn_Check_Stage_Aud
	WHERE 
		CheckID = (SELECT CheckID FROM INSERTED)
         
	
	IF ((SELECT COUNT(1) FROM INSERTED) > 0 AND (SELECT COUNT(1) FROM DELETED) > 0)
	BEGIN
		-- UPDATE
		SET @AuditEvent = 2
	END
	ELSE IF ((SELECT COUNT(*) FROM INSERTED) > 0)
	BEGIN
		-- INSERT
		SET @AuditEvent = 1
	END	 
	ELSE IF ((SELECT COUNT(*) FROM DELETED) > 0)
	BEGIN
		-- DELETE
		SET @AuditEvent = 3
	END     
	
	INSERT tTxn_Check_Stage_Aud
	(   LogId,
		CheckPK, 
		CheckID,
		Amount,
		Fee,
		AccountPK,
		MICR,
		CheckType,
		Status,
		IssueDate,
		DTServerCreate,
		DTServerLastModified,
		DTTerminalCreate,
		DTTerminalLastModified,
		RevisionNo,
		AuditEvent,
		DTAudit
	)
	SELECT
	    NEWID(),
		CheckPK, 
		CheckID,
		Amount,
		Fee,
		AccountPK,
		MICR,
		CheckType,
		Status,
		IssueDate,
		DTServerCreate,
		DTServerLastModified,
		DTTerminalCreate,
		DTTerminalLastModified,
		@RevisionNo,
		@AuditEvent,
		GETDATE() 
	FROM 
		INSERTED
GO


