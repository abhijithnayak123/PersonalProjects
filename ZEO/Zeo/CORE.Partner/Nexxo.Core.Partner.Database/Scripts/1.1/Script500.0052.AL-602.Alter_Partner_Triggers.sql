-- ================================================================================
-- Author:		<Ashok Kumar>
-- Create date: <06/24/2015>
-- Description:	<Alter views date columns to DTTerminalCreate, DTTerminalLastModified, DTServerCreate, DTServerLastModified. >
-- Jira ID:		<AL-602>
-- ================================================================================


-- trgLocationCounterIdInsertUpdate
DROP TRIGGER [dbo].[trgLocationCounterIdInsertUpdate]
GO

CREATE TRIGGER [dbo].[trgLocationCounterIdInsertUpdate] ON [dbo].[tLocationProcessorCredentials] 
AFTER INSERT,UPDATE
AS
BEGIN

	SET NOCOUNT ON

	DECLARE @LocationIdentifier nvarchar(50)
	DECLARE @LocationId uniqueidentifier 
	DECLARE @Increment int
	DECLARE @Count int
	DECLARE @SendMoneyProviderId int
	DECLARE @BillPayProviderId int
	DECLARE @CounterId nvarchar(50)
	SET @Increment = 1;
	SET @Count = 20;
	SET @SendMoneyProviderId = 301
	SET @BillPayProviderId = 401
	SET @CounterId=''
	
	SELECT @LocationId=LocationId, @LocationIdentifier=Identifier FROM inserted
	
	if(@LocationIdentifier  IS NOT NULL and @LocationIdentifier <> ''  and UPDATE(Identifier))
	BEGIN
		
		SELECT @Count = (SELECT NoOfCounterIds FROM tLocations WHERE LocationPK = @LocationId)
		
		DELETE FROM tLocationCounterIdDetails where LocationId = @LocationId
	
		WHILE @Increment <= @Count
		BEGIN		
		
		IF(@Increment < 10)
			SET @CounterId = @LocationIdentifier + '0' + CAST(@Increment as varchar)
		ELSE
			SET @CounterId = @LocationIdentifier + CAST(@Increment as varchar)
		 
		INSERT INTO [dbo].[tLocationCounterIdDetails]
		([LocationCounterIdDetailPK] ,[LocationId] ,[ProviderId]   ,[CounterId]  ,[IsAvailable]  ,[DTServerCreate]  ,[DTServerLastModified])
		VALUES (NEWID(),@LocationId,@SendMoneyProviderId,@CounterId,1,getdate(),getdate())		   
	  
	   INSERT INTO [dbo].[tLocationCounterIdDetails]
	   ([LocationCounterIdDetailPK] ,[LocationId] ,[ProviderId]   ,[CounterId]  ,[IsAvailable]  ,[DTServerCreate]  ,[DTServerLastModified])
		VALUES (NEWID(),@LocationId,@BillPayProviderId,@CounterId,1,getdate(),getdate())

		SET @Increment = @Increment + 1;
		
		END;
	END	
END
GO


-- tPartnerCustomerGroupSettings_Delete
DROP TRIGGER [dbo].[tPartnerCustomerGroupSettings_Delete]
GO

CREATE TRIGGER [dbo].[tPartnerCustomerGroupSettings_Delete] ON [dbo].[tPartnerCustomerGroupSettings] AFTER DELETE
AS
BEGIN
	SET NOCOUNT ON
	insert tPartnerCustomerGroupSettings_Aud(PCGroupSettingPK, PartnerCustomerPK, ChannelPartnerGroupId, DTServerCreate, DTServerLastModified, RevisionNo, AuditEvent, DTAudit,ChannelPartnerGroupPK)
	select i.PCGroupSettingPK, PartnerCustomerPK, ChannelPartnerGroupId, DTServerCreate, DTServerLastModified, isnull(a.MaxRev,0) + 1, 3, GETDATE(),ChannelPartnerGroupPK
	from deleted i 
		left outer join (
			select PCGroupSettingPK, MAX(RevisionNo) as MaxRev
			from tPartnerCustomerGroupSettings_Aud
			group by PCGroupSettingPK
		) a on i.PCGroupSettingPK = a.PCGroupSettingPK
END
GO

-- tPartnerCustomerGroupSettings_Insert_Update
DROP TRIGGER [dbo].[tPartnerCustomerGroupSettings_Insert_Update]
GO

CREATE TRIGGER [dbo].[tPartnerCustomerGroupSettings_Insert_Update] ON [dbo].[tPartnerCustomerGroupSettings] AFTER INSERT, UPDATE
AS
BEGIN
	declare @auditEvent int

	if not exists (select * from deleted)
		set @auditEvent = 1
	else
		set @auditEvent = 2

	SET NOCOUNT ON

	insert tPartnerCustomerGroupSettings_Aud(PCGroupSettingPK, PartnerCustomerPK, ChannelPartnerGroupId, DTServerCreate, DTServerLastModified, RevisionNo, AuditEvent, DTAudit,ChannelPartnerGroupPK)
	select i.PCGroupSettingPK, PartnerCustomerPK, ChannelPartnerGroupId, DTServerCreate, DTServerLastModified, isnull(a.MaxRev,0) + 1, @auditEvent, GETDATE(),ChannelPartnerGroupPK
	from inserted i 
		left outer join (
			select PCGroupSettingPK, MAX(RevisionNo) as MaxRev
			from tPartnerCustomerGroupSettings_Aud
			group by PCGroupSettingPK
		) a on i.PCGroupSettingPK = a.PCGroupSettingPK
END
GO

-- tPartnerCustomers_Audit
DROP TRIGGER [dbo].[tPartnerCustomers_Audit]
GO

CREATE TRIGGER [dbo].[tPartnerCustomers_Audit] ON [dbo].[tPartnerCustomers] AFTER INSERT, UPDATE, DELETE
AS
       SET NOCOUNT ON
       Declare @RevisionNo bigint
       select @RevisionNo = isnull(MAX(RevisionNo),0) + 1 from tPartnerCustomers_Aud where CustomerID = (select CustomerID from inserted)
             
       IF ((SELECT COUNT(*) FROM inserted)<>0 AND (SELECT COUNT(*) FROM deleted)>0)
       begin
              insert into tPartnerCustomers_Aud(
					CustomerPK,
					CustomerID,
					CXEId,
					DTServerCreate,
					DTServerLastModified,
					IsPartnerAccountHolder,
					ReferralCode,
					ChannelPartnerPK,
					RevisionNo,
					AuditEvent,
					DTAudit,AgentSessionPK,CustomerProfileStatus
										 )
								  select CustomerPK,
					Inserted.CustomerID,
					CXEId,
					DTServerCreate,
					DTServerLastModified,
					IsPartnerAccountHolder,
					ReferralCode,
					ChannelPartnerPK, @RevisionNo,2 as AuditEvent,GETDATE(),
					AgentSessionPK,CustomerProfileStatus from inserted
       end
       ELSE IF(SELECT COUNT(*) FROM inserted)>0 AND (SELECT COUNT(*) FROM deleted)=0
       BEGIN
              INSERT INTO tPartnerCustomers_Aud(
					CustomerPK,
					CustomerID,
					CXEId,
					DTServerCreate,
					DTServerLastModified,
					IsPartnerAccountHolder,
					ReferralCode,
					ChannelPartnerPK,
					RevisionNo,
					AuditEvent,
					DTAudit,AgentSessionPK,CustomerProfileStatus)
								  SELECT CustomerPK,
					CustomerID,
					CXEId,
					DTServerCreate,
					DTServerLastModified,
					IsPartnerAccountHolder,
					ReferralCode,
					ChannelPartnerPK, @RevisionNo,1 AS AuditEvent,GETDATE(),
					AgentSessionPK,CustomerProfileStatus FROM inserted
       END
       ELSE IF(SELECT COUNT(*) FROM deleted)>0
       BEGIN
              INSERT INTO tPartnerCustomers_Aud(
					CustomerPK,
					CustomerID,
					CXEId,
					DTServerCreate,
					DTServerLastModified,
					IsPartnerAccountHolder,
					ReferralCode,
					ChannelPartnerPK,
					RevisionNo,
					AuditEvent,
					DTAudit,AgentSessionPK,CustomerProfileStatus)
								  SELECT CustomerPK,
					CustomerID,
					CXEId,
					DTServerCreate,
					DTServerLastModified,
					IsPartnerAccountHolder,
					ReferralCode,
					Deleted.ChannelPartnerPK, @RevisionNo,3 AS AuditEvent,GETDATE(),
					AgentSessionPK,CustomerProfileStatus FROM  deleted
       END
GO

-- trShoppingCartsAudit
DROP TRIGGER [dbo].[trShoppingCartsAudit]
GO

CREATE TRIGGER [dbo].[trShoppingCartsAudit] ON [dbo].[tShoppingCarts] AFTER INSERT, UPDATE, DELETE
AS
       SET NOCOUNT ON
       DECLARE @RevisionNo BIGINT
       SELECT @RevisionNo = ISNULL(MAX(RevisionNo),0) + 1 FROM tShoppingCarts_Aud WHERE cartPK = (SELECT cartPK FROM inserted)
             
       IF ((SELECT COUNT(*) FROM inserted)<>0 AND (SELECT COUNT(*) FROM deleted)>0)
       BEGIN
              INSERT INTO tShoppingCarts_Aud(
					 cartPK,
                     cartId,
                     Active,
                     DTTerminalCreate,
                     DTTerminalLastModified,
                     CustomerPK,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
              SELECT cartPK,cartId,Active,DTTerminalCreate,DTTerminalLastModified,CustomerPK,@RevisionNo,2 AS AuditEvent,GETDATE() FROM inserted
       END
       ELSE IF(SELECT COUNT(*) FROM inserted)>0 AND (SELECT COUNT(*) FROM deleted)=0
       BEGIN
              INSERT INTO tShoppingCarts_Aud(
					  cartPK,
                     cartId,
                     Active,
                     DTTerminalCreate,
                     DTTerminalLastModified,
                     CustomerPK,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
              SELECT cartPK,cartId,Active,DTTerminalCreate,DTTerminalLastModified,CustomerPK,@RevisionNo,1 AS AuditEvent,GETDATE() FROM inserted
       END
       ELSE IF(SELECT COUNT(*) FROM deleted)>0
       BEGIN
              INSERT INTO tShoppingCarts_Aud(
					  cartPK,
                     cartId,
                     Active,
                     DTTerminalCreate,
                     DTTerminalLastModified,
                     CustomerPK,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
              SELECT cartPK,cartId,Active,DTTerminalCreate,DTTerminalLastModified,CustomerPK,@RevisionNo,3 AS AuditEvent,GETDATE() FROM deleted
       END
GO
