--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <09-15-2016>
-- Description:	 Alter Trigger for tTCIS_Account audit table
-- Jira ID:		<AL-7630>
-- ================================================================================

IF OBJECT_ID(N'tr_TCIS_Account_Aud', N'TR') IS NOT NULL
DROP TRIGGER tr_TCIS_Account_Aud
GO


CREATE TRIGGER [dbo].[tr_TCIS_Account_Aud] ON tTCIS_Account
AFTER INSERT, UPDATE, DELETE
AS
	   SET NOCOUNT ON;
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
			  tTCIS_Account_Aud
			WHERE 
			  TCISAccountID = (SELECT TCISAccountID FROM INSERTED)

			INSERT INTO tTCIS_Account_Aud
			(
				[TCISAccountID]
			   ,[PartnerAccountNumber]
			   ,[RelationshipAccountNumber]
			   ,[ProfileStatus]
			   ,[DTTerminalCreate]
			   ,[DTTerminalLastModified]
			   ,[BankId]
			   ,[BranchId]
			   ,[TcfCustInd]
			   ,[DTServerCreate]
			   ,[DTServerLastModified]
			   ,[CustomerID]
			   ,[CustomerSessionID]
			   ,[CustomerRevisionNo]
			   ,[RevisionNo]
			   ,[AuditEvent]
			   ,[DTAudit]
			)
			SELECT 
				TCISAccountID
				,PartnerAccountNumber
				,RelationshipAccountNumber
				,ProfileStatus
				,DTTerminalCreate
				,DTTerminalLastModified
				,BankId
				,BranchId
				,TcfCustInd
				,DTServerCreate
				,DTServerLastModified
				,CustomerID
				,CustomerSessionID
				,CustomerRevisionNo
				,@RevisionNo
				,@AuditEvent
				,GETDATE() 
			FROM INSERTED

       END
       ELSE 
       BEGIN
            SELECT 
			  @RevisionNo = ISNULL(MAX(RevisionNo), 0) + 1 
			FROM 
			  tTCIS_Account_Aud
			WHERE 
			  TCISAccountID = (SELECT TCISAccountID FROM DELETED)

			INSERT INTO tTCIS_Account_Aud
			(
				[TCISAccountID]
			   ,[PartnerAccountNumber]
			   ,[RelationshipAccountNumber]
			   ,[ProfileStatus]
			   ,[DTTerminalCreate]
			   ,[DTTerminalLastModified]
			   ,[BankId]
			   ,[BranchId]
			   ,[TcfCustInd]
			   ,[DTServerCreate]
			   ,[DTServerLastModified]
			   ,[CustomerID]
			   ,[CustomerSessionID]
			   ,[CustomerRevisionNo]
			   ,[RevisionNo]
			   ,[AuditEvent]
			   ,[DTAudit]
			)
			SELECT 
				TCISAccountID
				,PartnerAccountNumber
				,RelationshipAccountNumber
				,ProfileStatus
				,DTTerminalCreate
				,DTTerminalLastModified
				,BankId
				,BranchId
				,TcfCustInd
				,DTServerCreate
				,DTServerLastModified
				,CustomerID
				,CustomerSessionID
				,CustomerRevisionNo
				,@RevisionNo
				,@AuditEvent
				,GETDATE() 
			FROM DELETED

       END
GO


