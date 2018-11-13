--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to alter trigger [trgTxn_BillPay_StageAudit]>           
-- Jira ID:	<AL-243>
--===========================================================================================

/****** Object:  Trigger [dbo].[trgTxn_BillPay_StageAudit]    Script Date: 3/30/2015 3:46:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TRIGGER [dbo].[trgTxn_BillPay_StageAudit] 
	ON [dbo].[tTxn_BillPay_Stage] 
	AFTER INSERT, UPDATE, DELETE
AS
	SET NOCOUNT ON
	DECLARE @RevisionNo BIGINT
	DECLARE @AuditEvent SMALLINT

	SELECT 
		@RevisionNo = ISNULL(MAX(RevisionNo), 0) + 1 
	FROM 
		tTxn_BillPay_Stage_Aud 
	WHERE 
		BillPayId = (SELECT BillPayId FROM INSERTED)
         
	
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
		BillPayPK, 
		BillPayId,
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
		BillPayPK, 
		BillPayId,
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


