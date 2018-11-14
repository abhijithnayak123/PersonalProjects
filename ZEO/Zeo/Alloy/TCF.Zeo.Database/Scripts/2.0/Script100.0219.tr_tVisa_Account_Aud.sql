--- ===============================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <11-22-2016>
-- Description:	 Trigger for tVisa_Account audit table
-- Jira ID:		<AL-8323>
-- ================================================================================

IF OBJECT_ID(N'tr_tVisa_Account_Aud', N'TR') IS NOT NULL
DROP TRIGGER tr_tVisa_Account_Aud
GO


CREATE TRIGGER tr_tVisa_Account_Aud ON tVisa_Account   
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
		tVisa_Account_Aud
	WHERE 
		VisaAccountID = (SELECT VisaAccountID FROM INSERTED)
					
			INSERT INTO [dbo].[tVisa_Account_Aud]
					   (
					    [VisaAccountID]
					   ,[ProxyId]
					   ,[PseudoDDA]
					   ,[CardNumber]
					   ,[CardAliasId]
					   ,[Activated]
					   ,[ExpirationMonth]
					   ,[ExpirationYear]
					   ,[SubClientNodeId]
					   ,[DTTerminalCreate]
					   ,[DTTerminalLastModified]
					   ,[DTServerCreate]
					   ,[DTServerLastModified]
					   ,[DTAccountClosed]
					   ,[PrimaryCardAliasId]
					   ,[ActivatedLocationNodeId]
					   ,[CustomerID]
					   ,[CustomerSessionID]
					   ,[CustomerRevisionNo]
					   ,[RevisionNo]
					   ,[AuditEvent]
					   ,[DTAudit]
					   )
				 SELECT  VisaAccountID
						,ProxyId
						,PseudoDDA
						,CardNumber
						,CardAliasId
						,Activated
						,ExpirationMonth
						,ExpirationYear
						,SubClientNodeId
						,DTTerminalCreate
						,DTTerminalLastModified
						,DTServerCreate
						,DTServerLastModified
						,DTAccountClosed
						,PrimaryCardAliasId
						,ActivatedLocationNodeId
						,CustomerId
						,CustomerSessionId
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
		  tVisa_Account_Aud
		WHERE 
		  VisaAccountID = (SELECT VisaAccountID FROM DELETED)
           
		   INSERT INTO [dbo].[tVisa_Account_Aud]
					   (
					    [VisaAccountID]
					   ,[ProxyId]
					   ,[PseudoDDA]
					   ,[CardNumber]
					   ,[CardAliasId]
					   ,[Activated]
					   ,[ExpirationMonth]
					   ,[ExpirationYear]
					   ,[SubClientNodeId]
					   ,[DTTerminalCreate]
					   ,[DTTerminalLastModified]
					   ,[DTServerCreate]
					   ,[DTServerLastModified]
					   ,[DTAccountClosed]
					   ,[PrimaryCardAliasId]
					   ,[ActivatedLocationNodeId]
					   ,[CustomerID]
					   ,[CustomerSessionID]
					   ,[CustomerRevisionNo]
					   ,[RevisionNo]
					   ,[AuditEvent]
					   ,[DTAudit]
					   )
				 SELECT  VisaAccountID
						,ProxyId
						,PseudoDDA
						,CardNumber
						,CardAliasId
						,Activated
						,ExpirationMonth
						,ExpirationYear
						,SubClientNodeId
						,DTTerminalCreate
						,DTTerminalLastModified
						,DTServerCreate
						,DTServerLastModified
						,DTAccountClosed
						,PrimaryCardAliasId
						,ActivatedLocationNodeId
						,CustomerId
						,CustomerSessionId
						,CustomerRevisionNo
						,@RevisionNo
						,@AuditEvent
						,GETDATE()
				 FROM DELETED
				 END
GO


