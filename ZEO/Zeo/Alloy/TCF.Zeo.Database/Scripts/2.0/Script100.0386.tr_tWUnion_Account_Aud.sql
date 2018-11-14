--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-09-2016>
-- Description:	 Alter Trigger for tWUnion_Account audit table
-- Jira ID:		<AL-8324>
-- ================================================================================

IF OBJECT_ID(N'tr_tWUnion_Account_Aud', N'TR') IS NOT NULL
DROP TRIGGER tr_tWUnion_Account_Aud
GO


CREATE TRIGGER [dbo].[tr_tWUnion_Account_Aud] ON tWUnion_Account
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
			  tWUnion_Account_Aud
			WHERE 
			  WUAccountID = (SELECT WUAccountID FROM INSERTED)

             INSERT INTO [dbo].[tWUnion_Account_Aud]
			   ([WUAccountID]
			   ,[NameType]
			   ,[CustomerId]
			   ,[CustomerSessionId]
			   ,[CustomerRevisionNo]
			   ,[PreferredCustomerAccountNumber]
			   ,[PreferredCustomerLevelCode]
			   ,[SmsNotificationFlag]
			   ,[DTAudit]
			   ,[AuditEvent]
			   ,[RevisionNo]
			   ,[DTTerminalCreate]
			   ,[DTTerminalLastModified]
			   ,[DTServerCreate]
			   ,[DTServerLastModified])	
             SELECT 
				WUAccountID
				,NameType
				,CustomerId
				,CustomerSessionId
				,CustomerRevisionNo
				,PreferredCustomerAccountNumber
				,PreferredCustomerLevelCode
				,SmsNotificationFlag
				,GETDATE() 
				,@AuditEvent
				,@RevisionNo
				,DTTerminalCreate
				,DTTerminalLastModified
				,DTServerCreate
				,DTServerLastModified
			FROM INSERTED

       END
		ELSE
		BEGIN

			SELECT 
			  @RevisionNo = ISNULL(MAX(RevisionNo), 0) + 1 
			FROM 
			  tWUnion_Account_Aud
			WHERE 
			  WUAccountID = (SELECT WUAccountID FROM DELETED)

			 INSERT INTO [dbo].[tWUnion_Account_Aud]
			   ([WUAccountID]
			   ,[NameType]
			   ,[CustomerId]
			   ,[CustomerSessionId]
			   ,[CustomerRevisionNo]
			   ,[PreferredCustomerAccountNumber]
			   ,[PreferredCustomerLevelCode]
			   ,[SmsNotificationFlag]
			   ,[DTAudit]
			   ,[AuditEvent]
			   ,[RevisionNo]
			   ,[DTTerminalCreate]
			   ,[DTTerminalLastModified]
			   ,[DTServerCreate]
			   ,[DTServerLastModified])	
             SELECT 
				WUAccountID
				,NameType
				,CustomerId
				,CustomerSessionId
				,CustomerRevisionNo
				,PreferredCustomerAccountNumber
				,PreferredCustomerLevelCode
				,SmsNotificationFlag
				,GETDATE() 
				,@AuditEvent
				,@RevisionNo
				,DTTerminalCreate
				,DTTerminalLastModified
				,DTServerCreate
				,DTServerLastModified
			FROM DELETED

       END
GO


