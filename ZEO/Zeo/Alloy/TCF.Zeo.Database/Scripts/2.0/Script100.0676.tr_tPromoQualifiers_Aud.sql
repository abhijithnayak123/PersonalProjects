--- ===============================================================================
-- Author:		<Purna Pushkal>
-- Create date: <01-02-2018>
-- Description:	 Creating the trigger to populate data in the tPromoQualifiers table
-- Jira ID:		<B-12321>
-- ================================================================================

IF OBJECT_ID('tr_tPromoQualifiers_Aud') IS NOT NULL
	 BEGIN
		  DROP TRIGGER tr_tPromoQualifiers_Aud
	 END
GO

CREATE TRIGGER tr_tPromoQualifiers_Aud ON tPromoQualifiers
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
			  tPromoQualifiers_Aud tpqa 
			  INNER JOIN INSERTED i ON i.PromoQualifierId = tpqa.PromoQualifierId
		END
	ELSE
		BEGIN
		   SELECT 
			  @RevisionNo = ISNULL(MAX(RevisionNo), 0) + 1 
			FROM 
			  tPromoQualifiers_Aud tpqa
			  INNER JOIN DELETED d ON d.PromoQualifierId = tpqa.PromoQualifierId
		END

	IF @AuditEvent != 3
		BEGIN

		INSERT INTO tPromoQualifiers_Aud
		(
		    PromoQualifierId,
		    PromotionId,
		    StartDate,
		    EndDate,
		    Amount,
		    MinTransactionCount,
		    MaxTransactionCount,
		    ProductId,
		    IsPaidFee,
		    TransactionStates,
		    IsParked,
		    DTServerCreate,
		    DTServerLastModified,
		    DTTerminalCreate,
		    DTTerminalLastModified,
		    RevisionNo,
		    AuditEvent,
		    DTAudit
		)
				
		SELECT 
			 PromoQualifierId
			,PromotionId
			,StartDate
			,EndDate
			,Amount
			,MinTransactionCount
			,MaxTransactionCount
			,ProductId
			,IsPaidFee
			,TransactionStates
			,IsParked
			,DTServerCreate
			,DTServerLastModified
			,DTTerminalCreate
			,DTTerminalLastModified
			,@revisionNo
			,@auditEvent
			,GETDATE()
		FROM 
			 INSERTED i
		END
	ELSE
		BEGIN
			INSERT INTO tPromoQualifiers_Aud
			(
				PromoQualifierId,
				PromotionId,
				StartDate,
				EndDate,
				Amount,
				MinTransactionCount,
				MaxTransactionCount,
				ProductId,
				IsPaidFee,
				TransactionStates,
				IsParked,
				DTServerCreate,
				DTServerLastModified,
				DTTerminalCreate,
				DTTerminalLastModified,
				RevisionNo,
				AuditEvent,
				DTAudit
			)
				
			SELECT 
				 PromoQualifierId
				,PromotionId
				,StartDate
				,EndDate
				,Amount
				,MinTransactionCount
				,MaxTransactionCount
				,ProductId
				,IsPaidFee
				,TransactionStates
				,IsParked
				,DTServerCreate
				,DTServerLastModified
				,DTTerminalCreate
				,DTTerminalLastModified
				,@revisionNo
				,@auditEvent
				,GETDATE()
				FROM 
					DELETED D
		END
END
GO
