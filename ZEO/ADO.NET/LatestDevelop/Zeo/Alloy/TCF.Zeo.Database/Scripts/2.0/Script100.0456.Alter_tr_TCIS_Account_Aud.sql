--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <04-03-2017>
-- Description:	 Alter Trigger for tTCIS_Account audit table
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
			FROM tTCIS_Account_Aud ta
				INNER JOIN INSERTED i ON ta.TCISAccountID = i.TCISAccountID	

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
			FROM tTCIS_Account_Aud ta
				INNER JOIN DELETED d ON ta.TCISAccountID = d.TCISAccountID	

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


