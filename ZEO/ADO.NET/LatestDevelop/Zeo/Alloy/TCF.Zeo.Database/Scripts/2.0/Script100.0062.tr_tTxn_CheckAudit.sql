--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <07-20-2016>
-- Description:	 Alter Trigger for tTxn_Check audit table
-- Jira ID:		<AL-7705>
-- ================================================================================

IF OBJECT_ID(N'tr_tTxn_Check_Aud', N'TR') IS NOT NULL
DROP TRIGGER tr_tTxn_Check_Aud
GO



CREATE TRIGGER tr_tTxn_Check_Aud ON tTxn_Check   
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
		  tTxn_Check_Aud
		WHERE 
		  TransactionId = (SELECT TransactionId FROM INSERTED)


		INSERT INTO tTxn_Check_Aud
          (
		   TransactionID,
           Amount,
           Fee,
           Description,
           State,
           DTTerminalCreate,
           DTTerminalLastModified,
           ConfirmationNumber,
           DTServerCreate,
           DTServerLastModified,
           BaseFee,
           DiscountApplied,
           AdditionalFee,
           DiscountName,
           DiscountDescription,
           IsSystemApplied,
           CustomerSessionId,
           CustomerRevisionNo,
           ProviderId,
           ProviderAccountId,
           CheckType,
           MICR,
           DTAudit,
           AuditEvent,
           RevisionNo
		  )
       SELECT
		   TransactionID,
           Amount,
           Fee,
           Description,
           State,
           DTTerminalCreate,
           DTTerminalLastModified,
           ConfirmationNumber,
           DTServerCreate,
           DTServerLastModified,
           BaseFee,
           DiscountApplied,
           AdditionalFee,
           DiscountName,
           DiscountDescription,
           IsSystemApplied,
           CustomerSessionId,
           CustomerRevisionNo,
           ProviderId,
           ProviderAccountId,
           CheckType,
           MICR,
           GETDATE(),
           @AuditEvent,
           @RevisionNo
		 FROM
		   INSERTED
     END
	 ELSE
	 BEGIN

	    SELECT 
		  @RevisionNo = ISNULL(MAX(RevisionNo), 0) + 1 
		FROM 
		  tTxn_Check_Aud
		WHERE 
		  TransactionId = (SELECT TransactionId FROM DELETED)

	    INSERT INTO tTxn_Check_Aud
          (
		   TransactionID,
           Amount,
           Fee,
           Description,
           State,
           DTTerminalCreate,
           DTTerminalLastModified,
           ConfirmationNumber,
           DTServerCreate,
           DTServerLastModified,
           BaseFee,
           DiscountApplied,
           AdditionalFee,
           DiscountName,
           DiscountDescription,
           IsSystemApplied,
           CustomerSessionId,
           CustomerRevisionNo,
           ProviderId,
           ProviderAccountId,
           CheckType,
           MICR,
           DTAudit,
           AuditEvent,
           RevisionNo
		  )
       SELECT
		   TransactionID,
           Amount,
           Fee,
           Description,
           State,
           DTTerminalCreate,
           DTTerminalLastModified,
           ConfirmationNumber,
           DTServerCreate,
           DTServerLastModified,
           BaseFee,
           DiscountApplied,
           AdditionalFee,
           DiscountName,
           DiscountDescription,
           IsSystemApplied,
           CustomerSessionId,
           CustomerRevisionNo,
           ProviderId,
           ProviderAccountId,
           CheckType,
           MICR,
           GETDATE(),
           @AuditEvent,
           @RevisionNo
		 FROM
		   DELETED
      END
GO 