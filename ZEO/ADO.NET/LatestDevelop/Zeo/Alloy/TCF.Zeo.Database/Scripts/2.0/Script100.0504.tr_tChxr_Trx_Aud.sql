
--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <18-11-2016>
-- Description:	 pupulate the data to audit table
-- Jira ID:		<AL-8320>
-- ================================================================================

IF OBJECT_ID('tr_tChxr_Trx_Aud') IS NOT NULL
	 BEGIN
		  DROP TRIGGER dbo.tr_tChxr_Trx_Aud
	 END
GO

CREATE TRIGGER tr_tChxr_Trx_Aud ON tChxr_Trx
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
			  tChxr_Trx_Aud tca
			  INNER JOIN INSERTED i ON i.ChxrTrxID = tca.ChxrTrxID
		END
	ELSE
		BEGIN
		   SELECT 
			  @RevisionNo = ISNULL(MAX(RevisionNo), 0) + 1 
			FROM 
			  tChxr_Trx_Aud tca
			  INNER JOIN DELETED d ON d.ChxrTrxID = tca.ChxrTrxID
		END

	IF @AuditEvent != 3
		BEGIN
			
         
          INSERT INTO tChxr_Trx_Aud
           (
            ChxrTrxID
           ,Amount
           ,ChexarAmount
           ,ChexarFee
           ,CheckDate
           ,CheckNumber
           ,RoutingNumber
           ,AccountNumber
           ,Micr
           ,Latitude
           ,Longitude
           ,InvoiceId
           ,TicketId
           ,WaitTime
           ,Status
           ,ChexarStatus
           ,DeclineCode
           ,Message
           ,Location
           ,DTTerminalCreate
           ,DTTerminalLastModified
           ,AuditEvent
           ,DTAudit
           ,RevisionNo
           ,SubmitType
           ,ReturnType
           ,ChannelPartnerID
           ,DTServerCreate
           ,DTServerLastModified
           ,IsCheckFranked
           ,ChxrAccountId
		   )
       SELECT  
	        ChxrTrxID
           ,Amount
           ,ChexarAmount
           ,ChexarFee
           ,CheckDate
           ,CheckNumber
           ,RoutingNumber
           ,AccountNumber
           ,Micr
           ,Latitude
           ,Longitude
           ,InvoiceId
           ,TicketId
           ,WaitTime
           ,Status
           ,ChexarStatus
           ,DeclineCode
           ,Message
           ,Location
           ,DTTerminalCreate
           ,DTTerminalLastModified
           ,@auditEvent
           ,GETDATE()
           ,@RevisionNo
           ,SubmitType
           ,ReturnType
           ,ChannelPartnerID
           ,DTServerCreate
           ,DTServerLastModified
           ,IsCheckFranked
           ,ChxrAccountId

		  FROM 
		    INSERTED

		END
	ELSE
		BEGIN
		   INSERT INTO tChxr_Trx_Aud
           (
            ChxrTrxID
           ,Amount
           ,ChexarAmount
           ,ChexarFee
           ,CheckDate
           ,CheckNumber
           ,RoutingNumber
           ,AccountNumber
           ,Micr
           ,Latitude
           ,Longitude
           ,InvoiceId
           ,TicketId
           ,WaitTime
           ,Status
           ,ChexarStatus
           ,DeclineCode
           ,Message
           ,Location
           ,DTTerminalCreate
           ,DTTerminalLastModified
           ,AuditEvent
           ,DTAudit
           ,RevisionNo
           ,SubmitType
           ,ReturnType
           ,ChannelPartnerID
           ,DTServerCreate
           ,DTServerLastModified
           ,IsCheckFranked
           ,ChxrAccountId
		   )
       SELECT  
	        ChxrTrxID
           ,Amount
           ,ChexarAmount
           ,ChexarFee
           ,CheckDate
           ,CheckNumber
           ,RoutingNumber
           ,AccountNumber
           ,Micr
           ,Latitude
           ,Longitude
           ,InvoiceId
           ,TicketId
           ,WaitTime
           ,Status
           ,ChexarStatus
           ,DeclineCode
           ,Message
           ,Location
           ,DTTerminalCreate
           ,DTTerminalLastModified
           ,@auditEvent
           ,GETDATE()
           ,@RevisionNo
           ,SubmitType
           ,ReturnType
           ,ChannelPartnerID
           ,DTServerCreate
           ,DTServerLastModified
           ,IsCheckFranked
           ,ChxrAccountId

		  FROM 
		    DELETED
		END
END
GO
