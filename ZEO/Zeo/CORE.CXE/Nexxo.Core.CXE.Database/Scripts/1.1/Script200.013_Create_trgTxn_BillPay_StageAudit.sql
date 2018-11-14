IF EXISTS (SELECT 1 FROM   sys.objects WHERE  object_id = OBJECT_ID(N'trgTxn_BillPay_StageAudit') AND type = 'TR')
BEGIN
	DROP TRIGGER [dbo].[trgTxn_BillPay_StageAudit]
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[trgTxn_BillPay_StageAudit] 
	ON [dbo].[tTxn_BillPay_Stage] 
	AFTER Insert, Update, Delete
AS
	SET NOCOUNT ON
	DECLARE @RevisionNo BIGINT
	DECLARE @AuditEvent SMALLINT

	SELECT 
		@RevisionNo = ISNULL(MAX(RevisionNo), 0) + 1 
	FROM 
		tTxn_BillPay_Stage_Aud 
	WHERE 
		Id = (SELECT Id FROM INSERTED)
         
	
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
	
	INSERT tTxn_BillPay_Stage_Aud
	(
		rowguid, 
		Id,
		Amount,
		Fee,
		AccountPK,
		Status,
		ProductId,
		AccountNumber,
		ConfirmationNumber,
		DTCreate,
		DTLastMod,
		DTServerCreate,
		DTServerLastMod,
		RevisionNo,
		AuditEvent,
		DTAudit
	)
	SELECT
		rowguid, 
		Id,
		Amount,
		Fee,
		AccountPK,
		Status,
		ProductId,
		AccountNumber,
		ConfirmationNumber,
		DTCreate,
		DTLastMod,
		DTServerCreate,
		DTServerLastMod,
		@RevisionNo,
		@AuditEvent,
		GETDATE() 
	FROM 
		INSERTED
GO
