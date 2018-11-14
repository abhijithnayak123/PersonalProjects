--- ===============================================================================
-- Author:		<Purna Pushkal>
-- Create date: <01-02-2018>
-- Description:	 Creating the trigger to populate data in the tPromoProvisions table
-- Jira ID:		<B-12321>
-- ================================================================================

IF OBJECT_ID('tr_tPromoProvisions_Aud') IS NOT NULL
	 BEGIN
		  DROP TRIGGER tr_tPromoProvisions_Aud
	 END
GO

CREATE TRIGGER tr_tPromoProvisions_Aud ON tPromoProvisions
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
			  tPromoProvisions_Aud tppa
			  INNER JOIN INSERTED i ON i.PromoProvisionId = tppa.PromoProvisionId
		END
	ELSE
		BEGIN
		   SELECT 
			  @RevisionNo = ISNULL(MAX(RevisionNo), 0) + 1 
			FROM 
			  tPromoProvisions_Aud tppa
			  INNER JOIN DELETED d ON d.PromoProvisionId = tppa.PromoProvisionId
		END

	IF @AuditEvent != 3
		BEGIN

		INSERT INTO tPromoProvisions_Aud
		(
		    PromoProvisionId,
		    PromotionId,
		    DiscountValue,
		    MinAmount,
		    MaxAmount,
		    CheckTypeIds,
		    locationIds,
		    Groups,
		    IsPercentage,
		    DTServerCreate,
		    DTServerLastModified,
		    DTTerminalCreate,
		    DTTerminalLastModified,
		    RevisionNo,
		    AuditEvent,
		    DTAudit
		)
		SELECT 
			 PromoProvisionId
			,PromotionId
			,DiscountValue
			,MinAmount
			,MaxAmount
			,CheckTypeIds
			,locationIds
			,Groups
			,IsPercentage
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
			INSERT INTO tPromoProvisions_Aud
			(
				PromoProvisionId,
				PromotionId,
				DiscountValue,
				MinAmount,
				MaxAmount,
				CheckTypeIds,
				locationIds,
				Groups,
				IsPercentage,
				DTServerCreate,
				DTServerLastModified,
				DTTerminalCreate,
				DTTerminalLastModified,
				RevisionNo,
				AuditEvent,
				DTAudit
			)
			SELECT 
				 PromoProvisionId
				,PromotionId
				,DiscountValue
				,MinAmount
				,MaxAmount
				,CheckTypeIds
				,locationIds
				,Groups
				,IsPercentage
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
