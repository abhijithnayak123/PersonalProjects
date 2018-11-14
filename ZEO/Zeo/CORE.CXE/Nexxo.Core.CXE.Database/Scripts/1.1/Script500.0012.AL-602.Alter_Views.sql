-- ================================================================================
-- Author:		<Ashok Kumar>
-- Create date: <06/24/2015>
-- Description:	<Alter table date columns to DTTerminalCreate, DTTerminalLastModified, DTServerCreate, DTServerLastModified. >
-- Jira ID:		<AL-602>
-- ================================================================================

-- trCustomerEmploymentDetailsAudit
DROP TRIGGER [dbo].[trCustomerEmploymentDetailsAudit]
GO

create trigger [dbo].[trCustomerEmploymentDetailsAudit] on [dbo].[tCustomerEmploymentDetails] AFTER Insert, Update, Delete
AS
       SET NOCOUNT ON
       Declare @RevisionNo bigint
       select @RevisionNo = isnull(MAX(RevisionNo),0) + 1 from tCustomerEmploymentDetails_Aud where CustomerPK = (select CustomerPK from inserted)
             
       if ((select COUNT(*) from inserted)<>0 and (select COUNT(*) from deleted)>0)
       begin
              insert into tCustomerEmploymentDetails_Aud(
					 CustomerPK,
                     Occupation,
                     Employer,
                     EmployerPhone,
                     DTServerCreate,
                     DTServerLastModified,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
              select CustomerPK,Occupation,Employer,EmployerPhone,DTServerCreate,DTServerLastModified,@RevisionNo,2 as AuditEvent,GETDATE() from inserted
       end
       else if(select COUNT(*) from inserted)>0 and (select COUNT(*) from deleted)=0
       begin
              insert into tCustomerEmploymentDetails_Aud(
					 CustomerPK,
                     Occupation,
                     Employer,
                     EmployerPhone,
                     DTServerCreate,
                     DTServerLastModified,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
              select CustomerPK,Occupation,Employer,EmployerPhone,DTServerCreate,DTServerLastModified,@RevisionNo,1 as AuditEvent,GETDATE() from inserted
       end
       else if(select COUNT(*) from deleted)>0
       begin
              insert into tCustomerEmploymentDetails_Aud(
					 CustomerPK,
                     Occupation,
                     Employer,
                     EmployerPhone,
                     DTServerCreate,
                     DTServerLastModified,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
              select CustomerPK,Occupation,Employer,EmployerPhone,DTServerCreate,DTServerLastModified,@RevisionNo,3 as AuditEvent,GETDATE() from deleted
       end
GO

-- trCustomerGovernmentIdDetailsAudit
DROP TRIGGER [dbo].[trCustomerGovernmentIdDetailsAudit]
GO

create trigger [dbo].[trCustomerGovernmentIdDetailsAudit] on [dbo].[tCustomerGovernmentIdDetails] AFTER Insert, Update, Delete
AS
       SET NOCOUNT ON  
	        
             
       if ((select COUNT(*) from inserted)<>0 and (select COUNT(*) from deleted)>0)
       begin
              insert into tCustomerGovernmentIdDetails_Aud(
					 CustomerPK,
                     IdTypeId,
                     Identification,
                     ExpirationDate,
                     DTServerCreate,
                     DTServerLastModified,
                     IssueDate,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)             
              select i.CustomerPK,i.IdTypeId,i.Identification,i.ExpirationDate,i.DTServerCreate,i.DTServerLastModified,i.IssueDate,
              isnull(aud.RevisionNo,1),2 as AuditEvent,GETDATE()from 
              (select isnull(MAX(RevisionNo),0) + 1 as RevisionNo, CustomerPK from tCustomerGovernmentIdDetails_Aud 
              group by CustomerPK)aud right outer join inserted i on aud.CustomerPK = i.CustomerPK  
       end
       else if(select COUNT(*) from inserted)>0 and (select COUNT(*) from deleted)=0
       begin
              insert into tCustomerGovernmentIdDetails_Aud(
					 CustomerPK,
                     IdTypeId,
                     Identification,
                     ExpirationDate,
                     DTServerCreate,
                     DTServerLastModified,
                     IssueDate,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)             
              select i.CustomerPK,i.IdTypeId,i.Identification,i.ExpirationDate,i.DTServerCreate,i.DTServerLastModified,i.IssueDate,
              isnull(aud.RevisionNo,1),1 as AuditEvent,GETDATE() from 
              (select isnull(MAX(RevisionNo),0) + 1 as RevisionNo, CustomerPK from tCustomerGovernmentIdDetails_Aud 
              group by CustomerPK)aud right outer join inserted i on aud.CustomerPK = i.CustomerPK  
              
       end
       else if(select COUNT(*) from deleted)>0
       begin
              insert into tCustomerGovernmentIdDetails_Aud(
					 CustomerPK,
                     IdTypeId,
                     Identification,
                     ExpirationDate,
                     DTServerCreate,
                     DTServerLastModified,
                     IssueDate,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
			  select i.CustomerPK,i.IdTypeId,i.Identification,i.ExpirationDate,i.DTServerCreate,i.DTServerLastModified,i.IssueDate,
              isnull(aud.RevisionNo,1),3 as AuditEvent,GETDATE() from 
              (select isnull(MAX(RevisionNo),0) + 1 as RevisionNo, CustomerPK from tCustomerGovernmentIdDetails_Aud 
              group by CustomerPK)aud right outer join inserted i on aud.CustomerPK = i.CustomerPK  
       end
GO

-- tCustomers_delete
DROP TRIGGER [dbo].[tCustomers_delete]
GO

CREATE TRIGGER [dbo].[tCustomers_delete] ON [dbo].[tCustomers] AFTER DELETE
AS
	SET NOCOUNT ON	
	INSERT INTO tCustomers_Aud(CustomerPK,CustomerID, FirstName, MiddleName, LastName, LastName2, MothersMaidenName, DOB, Address1, Address2, City, [State], ZipCode, Phone1, Phone1Type, Phone1Provider, Phone2, Phone2Type, Phone2Provider, SSN, TaxpayerId, DoNotCall, SMSEnabled, MarketingSMSEnabled, ChannelPartnerId, DTTerminalCreate, DTTerminalLastModified, Gender, Email, PIN, IsMailingAddressDifferent, MailingAddress1, MailingAddress2, MailingCity, MailingState, MailingZipCode, RevisionNo, AuditEvent, DTAudit,DTServerCreate,DTServerLastModified)
	SELECT d.CustomerPK, d.CustomerID, d.FirstName, d.MiddleName, d.LastName, d.LastName2, d.MothersMaidenName, d.DOB, d.Address1, d.Address2, d.City, d.State, d.ZipCode, d.Phone1, d.Phone1Type, d.Phone1Provider, d.Phone2, d.Phone2Type, d.Phone2Provider, d.SSN, d.TaxpayerId, d.DoNotCall, d.SMSEnabled, d.MarketingSMSEnabled, d.ChannelPartnerId, d.DTTerminalCreate, d.DTTerminalLastModified, d.Gender, d.Email, d.PIN, d.IsMailingAddressDifferent, d.MailingAddress1, d.MailingAddress2, d.MailingCity, d.MailingState, d.MailingZipCode, ISNULL(a.MaxRev,0) + 1, 3, GETDATE(),d.DTServerCreate,d.DTServerLastModified
	FROM deleted d
	LEFT OUTER JOIN (
		SELECT CustomerPK, MAX(RevisionNo) AS MaxRev
		FROM tCustomers_Aud
		GROUP BY CustomerPK
	) a ON d.CustomerPK = a.CustomerPK
GO

DROP TRIGGER [dbo].[tCustomers_insert]
GO

CREATE TRIGGER [dbo].[tCustomers_insert] ON [dbo].[tCustomers] AFTER INSERT
AS
	SET NOCOUNT ON           
	INSERT INTO tCustomers_Aud(CustomerPK, CustomerId, FirstName, MiddleName, LastName, LastName2, MothersMaidenName, DOB, Address1, Address2, City, State, ZipCode, Phone1, Phone1Type, Phone1Provider, Phone2, Phone2Type, Phone2Provider, SSN, TaxpayerId, DoNotCall, SMSEnabled, MarketingSMSEnabled, ChannelPartnerId, DTTerminalCreate, DTTerminalLastModified, Gender, Email, PIN, IsMailingAddressDifferent, MailingAddress1, MailingAddress2, MailingCity, MailingState, MailingZipCode, RevisionNo, AuditEvent, DTAudit,DTServerCreate,DTServerLastModified)
	SELECT i.CustomerPK, i.CustomerId, i.FirstName, i.MiddleName, i.LastName, i.LastName2, i.MothersMaidenName, i.DOB, i.Address1, i.Address2, i.City, i.State, i.ZipCode, i.Phone1, i.Phone1Type, i.Phone1Provider, i.Phone2, i.Phone2Type, i.Phone2Provider, i.SSN, i.TaxpayerId, i.DoNotCall, i.SMSEnabled, i.MarketingSMSEnabled, i.ChannelPartnerId, i.DTTerminalCreate, i.DTTerminalLastModified, i.Gender, i.Email, i.PIN, i.IsMailingAddressDifferent, i.MailingAddress1, i.MailingAddress2, i.MailingCity, i.MailingState, i.MailingZipCode, ISNULL(a.MaxRev,0) + 1, 1, GETDATE(),i.DTServerCreate,i.DTServerLastModified
	FROM inserted i 
	LEFT OUTER JOIN (
		SELECT CustomerPK, MAX(RevisionNo) AS MaxRev
		FROM tCustomers_Aud
		GROUP BY CustomerPK
	) a ON i.CustomerPK = a.CustomerPK
GO

DROP TRIGGER [dbo].[tCustomers_update]
GO

CREATE TRIGGER [dbo].[tCustomers_update] ON [dbo].[tCustomers] AFTER UPDATE
AS
	SET NOCOUNT ON	
	INSERT INTO tCustomers_Aud(CustomerPK, CustomerId, FirstName, MiddleName, LastName, LastName2, MothersMaidenName, DOB, Address1, Address2, City, [State], ZipCode, Phone1, Phone1Type, Phone1Provider, Phone2, Phone2Type, Phone2Provider, SSN, TaxpayerId, DoNotCall, SMSEnabled, MarketingSMSEnabled, ChannelPartnerId, DTTerminalCreate, DTTerminalLastModified, Gender, Email, PIN, IsMailingAddressDifferent, MailingAddress1, MailingAddress2, MailingCity, MailingState, MailingZipCode, RevisionNo, AuditEvent, DTAudit,DTServerCreate,DTServerLastModified)
	SELECT i.CustomerPK, i.CustomerId, i.FirstName, i.MiddleName, i.LastName, i.LastName2, i.MothersMaidenName, i.DOB, i.Address1, i.Address2, i.City, i.State, i.ZipCode, i.Phone1, i.Phone1Type, i.Phone1Provider, i.Phone2, i.Phone2Type, i.Phone2Provider, i.SSN, i.TaxpayerId, i.DoNotCall, i.SMSEnabled, i.MarketingSMSEnabled, i.ChannelPartnerId, i.DTTerminalCreate, i.DTTerminalLastModified, i.Gender, i.Email, i.PIN, i.IsMailingAddressDifferent, i.MailingAddress1, i.MailingAddress2, i.MailingCity, i.MailingState, i.MailingZipCode, ISNULL(a.MaxRev,0) + 1, 2, GETDATE(),i.DTServerCreate,i.DTServerLastModified
	FROM inserted i
	LEFT OUTER JOIN (
		SELECT CustomerPK, MAX(RevisionNo) AS MaxRev
		FROM tCustomers_Aud
		GROUP BY CustomerPK
	) a ON i.CustomerPK = a.CustomerPK
GO

-- trgTxn_BillPay_StageAudit
DROP TRIGGER [dbo].[trgTxn_BillPay_StageAudit]
GO

CREATE TRIGGER [dbo].[trgTxn_BillPay_StageAudit] 
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
		DTServerCreate,
		DTServerLastModified,
		DTTerminalCreate,
		DTTerminalLastModified,
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
		DTServerCreate,
		DTServerLastModified,
		DTTerminalCreate,
		DTTerminalLastModified,
		@RevisionNo,
		@AuditEvent,
		GETDATE() 
	FROM 
		INSERTED
GO

-- trTxn_Cash_StageAudit
DROP TRIGGER [dbo].[trTxn_Cash_StageAudit]
GO

CREATE TRIGGER [dbo].[trTxn_Cash_StageAudit] ON [dbo].[tTxn_Cash_Stage] AFTER INSERT, UPDATE, DELETE
AS
       SET NOCOUNT ON
       Declare @RevisionNo bigint
       select @RevisionNo = isnull(MAX(RevisionNo),0) + 1 from tTxn_Cash_Stage_Aud where CashId = (select CashId from inserted)
             
       IF ((SELECT COUNT(*) FROM inserted)<>0 AND (SELECT COUNT(*) FROM deleted)>0)
       BEGIN
              INSERT INTO tTxn_Cash_Stage_Aud(
					 CashPK,
                     CashId,
                     Amount,
                     Fee,
                     AccountPK,
                     CashTrxType,
                     DTTerminalCreate,
                     DTTerminalLastModified,
                     RevisionNo,
                     AuditEvent,
                     DTAudit,
                     DTServerCreate,
                     DTServerLastModified
                     )
              SELECT CashPK, CashId,  Amount, Fee, AccountPK, CashTrxType, DTTerminalCreate, DTTerminalLastModified, @RevisionNo,2 AS AuditEvent,GETDATE(),DTServerCreate,DTServerLastModified FROM inserted
       END
       ELSE IF(SELECT COUNT(*) FROM inserted)>0 AND (SELECT COUNT(*) FROM deleted)=0
       BEGIN
              INSERT INTO tTxn_Cash_Stage_Aud(
					 CashPK,
                     CashId,
                     Amount,
                     Fee,
                     AccountPK,
                     CashTrxType,
                     DTTerminalCreate,
                     DTTerminalLastModified,
                     RevisionNo,
                     AuditEvent,
                     DTAudit,
                     DTServerCreate,
                     DTServerLastModified
              )
              SELECT CashPK, CashId,  Amount, Fee, AccountPK, CashTrxType, DTTerminalCreate, DTTerminalLastModified,@RevisionNo,1 AS AuditEvent,GETDATE(),DTServerCreate,DTServerLastModified FROM inserted
       END
       ELSE IF(SELECT COUNT(*) FROM deleted)>0
       BEGIN
              INSERT INTO tTxn_Cash_Stage_Aud(
					 CashPK,
                     CashId,
                     Amount,
                     Fee,
                     AccountPK,
                     CashTrxType,
                     DTTerminalCreate,
                     DTTerminalLastModified,
                     RevisionNo,
                     AuditEvent,
                     DTAudit,
                     DTServerCreate,
                     DTServerLastModified
              )
              SELECT CashPK, CashId,  Amount, Fee, AccountPK, CashTrxType, DTTerminalCreate, DTTerminalLastModified, @RevisionNo,3 AS AuditEvent,GETDATE(),DTServerCreate,DTServerLastModified FROM deleted
       END
GO

-- trFundsStageAudit
DROP TRIGGER [dbo].[trFundsStageAudit]
GO

CREATE TRIGGER [dbo].[trFundsStageAudit] ON [dbo].[tTxn_Funds_Stage] AFTER INSERT, UPDATE, DELETE
AS
       SET NOCOUNT ON
       Declare @RevisionNo bigint
       select @RevisionNo = isnull(MAX(RevisionNo),0) + 1 from tTxn_Funds_Stage_Aud where FundsId = (select FundsId from inserted)
             
       IF ((SELECT COUNT(*) FROM inserted)<>0 AND (SELECT COUNT(*) FROM deleted)>0)
       BEGIN
              INSERT INTO tTxn_Funds_Stage_Aud(FundsPK,
                     FundsId,
                     Amount,
                     Fee,
                     AccountPK,
                     [Type],
                     [Status],
                     DTTerminalCreate,
                     DTTerminalLastModified,
                     RevisionNo,
                     AuditEvent,
                     DTAudit,
                     DTServerCreate,
                     DTServerLastModified
                     )
              SELECT FundsPK,FundsId,Amount,Fee,AccountPK,[TYPE],[Status],DTTerminalCreate,DTTerminalLastModified,@RevisionNo,2 AS AuditEvent,GETDATE(),DTServerCreate,DTServerLastModified FROM inserted
       END
       ELSE IF(SELECT COUNT(*) FROM inserted)>0 AND (SELECT COUNT(*) FROM deleted)=0
       BEGIN
              INSERT INTO tTxn_Funds_Stage_Aud(FundsPK,
                     FundsId,
                     Amount,
                     Fee,
                     AccountPK,
                     [Type],
                     [Status],
                     DTTerminalCreate,
                     DTTerminalLastModified,
                     RevisionNo,
                     AuditEvent,
                     DTAudit,
                     DTServerCreate,
                     DTServerLastModified
                     )
              SELECT FundsPK,FundsId,Amount,Fee,AccountPK,[TYPE],[Status],DTTerminalCreate,DTTerminalLastModified,@RevisionNo,1 AS AuditEvent,GETDATE(),DTServerCreate,DTServerLastModified FROM inserted
       END
       ELSE IF(SELECT COUNT(*) FROM deleted)>0
       BEGIN
              INSERT INTO tTxn_Funds_Stage_Aud(FundsPK,
                     FundsId,
                     Amount,
                     Fee,
                     AccountPK,
                     [Type],
                     [Status],
                     DTTerminalCreate,
                     DTTerminalLastModified,
                     RevisionNo,
                     AuditEvent,
                     DTAudit,
                     DTServerCreate,
                     DTServerLastModified
                     )
              SELECT FundsPK,FundsId,Amount,Fee,AccountPK,[TYPE],[Status],DTTerminalCreate,DTTerminalLastModified,@RevisionNo,3 AS AuditEvent,GETDATE(),DTServerCreate,DTServerLastModified FROM deleted
       END
GO


-- trTxn_MoneyOrder_StageAudit
DROP TRIGGER [dbo].[trTxn_MoneyOrder_StageAudit]
GO

CREATE TRIGGER [dbo].[trTxn_MoneyOrder_StageAudit] ON [dbo].[tTxn_MoneyOrder_Stage] AFTER INSERT, UPDATE, DELETE
AS
       SET NOCOUNT ON
             
       IF ((SELECT COUNT(*) FROM inserted)<>0 AND (SELECT COUNT(*) FROM deleted)>0)
       BEGIN
              INSERT INTO tTxn_MoneyOrder_Stage_Aud(
					 MoneyOrderPK,
                     MoneyOrderId,
                     Amount,
                     Fee,
                     PurchaseDate,
                     MoneyOrderCheckNumber,
                     AccountPK,
                     [Status],
                     DTTerminalCreate,
                     DTTerminalLastModified,
                     AuditEvent,
                     DTAudit,
                     DTServerCreate,
                     DTServerLastModified
                     )
              SELECT MoneyOrderPK, MoneyOrderId,  Amount, Fee, PurchaseDate, MoneyOrderCheckNumber, AccountPK, [Status], DTTerminalCreate, DTTerminalLastModified, 2 AS AuditEvent,GETDATE(),DTServerCreate,DTServerLastModified FROM inserted
       END
       ELSE IF(SELECT COUNT(*) FROM inserted)>0 AND (SELECT COUNT(*) FROM deleted)=0
       BEGIN
              INSERT INTO tTxn_MoneyOrder_Stage_Aud(
					 MoneyOrderPK,
                     MoneyOrderId,
                     Amount,
                     Fee,
                     PurchaseDate,
                     MoneyOrderCheckNumber,
                     AccountPK,
                     [Status],
                     DTTerminalCreate,
                     DTTerminalLastModified,
                     AuditEvent,
                     DTAudit,
                     DTServerCreate,
                     DTServerLastModified
              )
              SELECT MoneyOrderPK, MoneyOrderId,  Amount, Fee, PurchaseDate, MoneyOrderCheckNumber, AccountPK, [Status], DTTerminalCreate, DTTerminalLastModified, 1 AS AuditEvent,GETDATE(),DTServerCreate,DTServerLastModified FROM inserted
       END
       ELSE IF(SELECT COUNT(*) FROM deleted)>0
       BEGIN
              INSERT INTO tTxn_MoneyOrder_Stage_Aud(
					 MoneyOrderPK,
                     MoneyOrderId,
                     Amount,
                     Fee,
                     PurchaseDate,
                     MoneyOrderCheckNumber,
                     AccountPK,
                     [Status],
                     DTTerminalCreate,
                     DTTerminalLastModified,
                     AuditEvent,
                     DTAudit,
                     DTServerCreate,
                     DTServerLastModified
              )
              SELECT MoneyOrderPK, MoneyOrderId,  Amount, Fee, PurchaseDate, MoneyOrderCheckNumber, AccountPK, [Status], DTTerminalCreate, DTTerminalLastModified, 3 AS AuditEvent,GETDATE(),DTServerCreate,DTServerLastModified FROM deleted
       END
GO

-- tTxn_MoneyTransfer_CommitAudit
DROP TRIGGER [dbo].[tTxn_MoneyTransfer_CommitAudit]
GO

CREATE TRIGGER [dbo].[tTxn_MoneyTransfer_CommitAudit] ON [dbo].[tTxn_MoneyTransfer_Commit] AFTER INSERT, UPDATE, DELETE
AS
       SET NOCOUNT ON
       DECLARE @RevisionNo BIGINT
       SELECT @RevisionNo = ISNULL(MAX(RevisionNo),0) + 1 FROM tTxn_MoneyTransfer_Commit_Aud
       WHERE MoneyTransferId = (SELECT MoneyTransferId FROM inserted)
             
       IF ((SELECT COUNT(*) FROM inserted)<>0 AND (SELECT COUNT(*) FROM deleted)>0)
       BEGIN
              INSERT INTO tTxn_MoneyTransfer_Commit_Aud(
					[MoneyTransferPK],[MoneyTransferId],[Amount],[Fee],[AccountPK],	[Status] , [ConfirmationNumber],[ReceiverName],
					[Destination],[DTTerminalCreate],[DTTerminalLastModified],[DTServerCreate],[DTServerLastModified],
                     RevisionNo, AuditEvent,DTAudit)
              SELECT  [MoneyTransferPK],[MoneyTransferId],[Amount],[Fee],[AccountPK], [Status] , [ConfirmationNumber],[ReceiverName],
					[Destination],[DTTerminalCreate],[DTTerminalLastModified],[DTServerCreate],[DTServerLastModified],
					 @RevisionNo,2 AS AuditEvent,GETDATE() FROM inserted
       END
       ELSE IF(SELECT COUNT(*) FROM inserted)>0 AND (SELECT COUNT(*) FROM deleted)=0
       BEGIN
               INSERT INTO tTxn_MoneyTransfer_Commit_Aud(
					 [MoneyTransferPK],[MoneyTransferId],[Amount],[Fee],[AccountPK], [Status] , [ConfirmationNumber],[ReceiverName],
					[Destination],[DTTerminalCreate],[DTTerminalLastModified],[DTServerCreate],[DTServerLastModified],
                     RevisionNo, AuditEvent,DTAudit)
              SELECT [MoneyTransferPK],[MoneyTransferId],[Amount],[Fee],[AccountPK], [Status] , [ConfirmationNumber],[ReceiverName],
					[Destination],[DTTerminalCreate],[DTTerminalLastModified],[DTServerCreate],[DTServerLastModified],
					 @RevisionNo,1 AS AuditEvent,GETDATE() FROM inserted
       END
       ELSE IF(SELECT COUNT(*) FROM deleted)>0
       BEGIN
              INSERT INTO tTxn_MoneyTransfer_Commit_Aud(
					 [MoneyTransferPK], [MoneyTransferId], [Amount], [Fee],[AccountPK],[Status],
					 [ConfirmationNumber],[ReceiverName],[Destination],
					 [DTTerminalCreate],[DTTerminalLastModified],[DTServerCreate],[DTServerLastModified],
                     RevisionNo, AuditEvent,DTAudit)
              SELECT [MoneyTransferPK],[MoneyTransferId],[Amount],[Fee],[AccountPK], [Status] , [ConfirmationNumber],[ReceiverName],
					[Destination],[DTTerminalCreate],[DTTerminalLastModified],[DTServerCreate],[DTTerminalLastModified],
					 @RevisionNo,3 AS AuditEvent,GETDATE() FROM deleted
       END
GO

-- tTxn_MoneyTransfer_StageAudit
DROP TRIGGER [dbo].[tTxn_MoneyTransfer_StageAudit]
GO

CREATE TRIGGER [dbo].[tTxn_MoneyTransfer_StageAudit] ON [dbo].[tTxn_MoneyTransfer_Stage] AFTER INSERT, UPDATE, DELETE
AS
       SET NOCOUNT ON
       DECLARE @RevisionNo BIGINT
       SELECT @RevisionNo = ISNULL(MAX(RevisionNo),0) + 1 FROM tTxn_MoneyTransfer_Stage_Aud WHERE MoneyTransferId = (SELECT MoneyTransferId FROM inserted)
             
       IF ((SELECT COUNT(*) FROM inserted)<>0 AND (SELECT COUNT(*) FROM deleted)>0)
       BEGIN
              INSERT INTO tTxn_MoneyTransfer_Stage_Aud(
					 [MoneyTransferPK], [MoneyTransferId], [Amount], [Fee],[AccountPK],[Status],
					 [ConfirmationNumber],[ReceiverName],[Destination],
					 [DTTerminalCreate],[DTTerminalLastModified],[DTServerCreate],[DTServerLastModified],
                     RevisionNo, AuditEvent,DTAudit)
              SELECT  [MoneyTransferPK], [MoneyTransferId], [Amount], [Fee],[AccountPK],[Status],
					 [ConfirmationNumber],[ReceiverName],[Destination],
					 [DTTerminalCreate],[DTTerminalLastModified],[DTServerCreate],[DTServerLastModified],
					 @RevisionNo,2 AS AuditEvent,GETDATE() FROM inserted
       END
       ELSE IF(SELECT COUNT(*) FROM inserted)>0 AND (SELECT COUNT(*) FROM deleted)=0
       BEGIN
               INSERT INTO tTxn_MoneyTransfer_Stage_Aud(
					 [MoneyTransferPK], [MoneyTransferId], [Amount], [Fee],[AccountPK],[Status],
					 [ConfirmationNumber],[ReceiverName],[Destination],
					 [DTTerminalCreate],[DTTerminalLastModified],[DTServerCreate],[DTServerLastModified],
                     RevisionNo, AuditEvent,DTAudit)
              SELECT [MoneyTransferPK], [MoneyTransferId], [Amount], [Fee],[AccountPK],[Status],
					 [ConfirmationNumber],[ReceiverName],[Destination],
					 [DTTerminalCreate],[DTTerminalLastModified],[DTServerCreate],[DTServerLastModified],
					 @RevisionNo,1 AS AuditEvent,GETDATE() FROM inserted
       END
       ELSE IF(SELECT COUNT(*) FROM deleted)>0
       BEGIN
              INSERT INTO tTxn_MoneyTransfer_Stage_Aud(
					 [MoneyTransferPK], [MoneyTransferId], [Amount], [Fee],[AccountPK],[Status],
					 [ConfirmationNumber],[ReceiverName],[Destination],
					 [DTTerminalCreate],[DTTerminalLastModified],[DTServerCreate],[DTServerLastModified],
                     RevisionNo, AuditEvent,DTAudit)
              SELECT [MoneyTransferPK], [MoneyTransferId], [Amount], [Fee],[AccountPK],[Status],
					 [ConfirmationNumber],[ReceiverName],[Destination],
					 [DTTerminalCreate],[DTTerminalLastModified],[DTServerCreate],[DTServerLastModified],
					 @RevisionNo,3 AS AuditEvent,GETDATE() FROM deleted
       END
GO