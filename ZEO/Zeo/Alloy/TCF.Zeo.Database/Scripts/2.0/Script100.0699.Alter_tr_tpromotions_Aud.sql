--- ===============================================================================
-- Author:		<Purna Pushkal>
-- Create date: <01-02-2018>
-- Description:	 Creating the trigger to populate data in the tpromotions table
-- Jira ID:		<B-12321>
-- ================================================================================

IF OBJECT_ID('tr_tpromotions_Aud') IS NOT NULL
	 BEGIN
		  DROP TRIGGER tr_tpromotions_Aud
	 END
GO

CREATE TRIGGER tr_tpromotions_Aud ON tPromotions
AFTER INSERT, UPDATE, DELETE
AS
BEGIN

	SET NOCOUNT ON
	DECLARE @revisionNo BIGINT
	DECLARE @auditEvent SMALLINT

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

	IF @AuditEvent != 3
		BEGIN
		   SELECT 
			  @RevisionNo = ISNULL(MAX(RevisionNo), 0) + 1 
			FROM 
			  tPromotions_Aud tpa
			  INNER JOIN INSERTED i ON i.PromotionId = tpa.PromotionId
		END
	ELSE
		BEGIN
		   SELECT 
			  @RevisionNo = ISNULL(MAX(RevisionNo), 0) + 1 
			FROM 
			  tPromotions_Aud tpa
			  INNER JOIN DELETED d ON d.PromotionId = tpa.PromotionId
		END

	IF @AuditEvent != 3
		BEGIN

		INSERT INTO tPromotions_Aud
		(
		    PromotionId,
		    Name,
		    Description,
		    ProductId,
		    ProviderId,
		    StartDate,
		    EndDate,
		    Priority,
		    IsSystemApplied,
		    IsOverridable,
		    IsNextCustomerSession,
		    IsPrintable,
		    DTServerCreate,
		    DTServerLastModified,
		    DTTerminalCreate,
		    DTTerminalLastModified,
			Status,
		    RevisionNo,
		    AuditEvent,
		    DTAudit
		)
		SELECT 
			 PromotionId
			,Name
			,Description
			,ProductId
			,ProviderId
			,StartDate
			,EndDate
			,Priority
			,IsSystemApplied
			,IsOverridable
			,IsNextCustomerSession
			,IsPrintable
			,DTServerCreate
			,DTServerLastModified
			,DTTerminalCreate
			,DTTerminalLastModified
			,Status
			,@revisionNo
			,@auditEvent
			,GETDATE()
		FROM 
			 INSERTED i
		END
	ELSE
		BEGIN
			INSERT INTO tPromotions_Aud
			(
				PromotionId,
				Name,
				Description,
				ProductId,
				ProviderId,
				StartDate,
				EndDate,
				Priority,
				IsSystemApplied,
				IsOverridable,
				IsNextCustomerSession,
				IsPrintable,
				DTServerCreate,
				DTServerLastModified,
				DTTerminalCreate,
				DTTerminalLastModified,
				Status,
				RevisionNo,
				AuditEvent,
				DTAudit
			)
			SELECT 
				 PromotionId
				,Name
				,Description
				,ProductId
				,ProviderId
				,StartDate
				,EndDate
				,Priority
				,IsSystemApplied
				,IsOverridable
				,IsNextCustomerSession
				,IsPrintable
				,DTServerCreate
				,DTServerLastModified
				,DTTerminalCreate
				,DTTerminalLastModified
				,Status
				,@revisionNo
				,@auditEvent
				,GETDATE()
			FROM 
				DELETED D
		END
END
GO
