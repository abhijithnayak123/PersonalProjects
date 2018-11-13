ALTER TABLE dbo.tTxn_Funds_Stage_Aud
DROP COLUMN LogId

GO
ALTER TABLE dbo.tTxn_Funds_Stage_Aud
ADD RevisionNo BIGINT NULL

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[trFundsStageAudit]') AND type in (N'TR'))
DROP TRIGGER [dbo].[trFundsStageAudit]
GO
create trigger trFundsStageAudit on tTxn_Funds_Stage AFTER Insert, Update, Delete
AS
       SET NOCOUNT ON
       Declare @RevisionNo bigint
       select @RevisionNo = isnull(MAX(RevisionNo),0) + 1 from tTxn_Funds_Stage_Aud where Id = (select Id from inserted)
             
       if ((select COUNT(*) from inserted)<>0 and (select COUNT(*) from deleted)>0)
       begin
              insert into tTxn_Funds_Stage_Aud(rowguid,
                     Id,
                     Amount,
                     Fee,
                     AccountPK,
                     [Type],
                     [Status],
                     DTCreate,
                     DTLastMod,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
              select rowguid,Id,Amount,Fee,AccountPK,[TYPE],[Status],DTCreate,DTLastMod,@RevisionNo,2 as AuditEvent,GETDATE() from inserted
       end
       else if(select COUNT(*) from inserted)>0 and (select COUNT(*) from deleted)=0
       begin
              insert into tTxn_Funds_Stage_Aud(rowguid,
                     Id,
                     Amount,
                     Fee,
                     AccountPK,
                     [Type],
                     [Status],
                     DTCreate,
                     DTLastMod,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
              select rowguid,Id,Amount,Fee,AccountPK,[TYPE],[Status],DTCreate,DTLastMod,@RevisionNo,1 as AuditEvent,GETDATE() from inserted
       end
       else if(select COUNT(*) from deleted)>0
       begin
              insert into tTxn_Funds_Stage_Aud(rowguid,
                     Id,
                     Amount,
                     Fee,
                     AccountPK,
                     [Type],
                     [Status],
                     DTCreate,
                     DTLastMod,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
              select rowguid,Id,Amount,Fee,AccountPK,[TYPE],[Status],DTCreate,DTLastMod,@RevisionNo,3 as AuditEvent,GETDATE() from deleted
       end
 GO
--========================================================================================

ALTER TABLE dbo.tCustomerProfiles_Aud
ADD RevisionNo BIGINT NULL

GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[trCustomerProfilesAudit]') AND type in (N'TR'))
DROP TRIGGER [dbo].[trCustomerProfilesAudit]
GO
create trigger trCustomerProfilesAudit on tCustomerProfiles AFTER Insert, Update, Delete
AS
       SET NOCOUNT ON
       Declare @RevisionNo bigint
       select @RevisionNo = isnull(MAX(RevisionNo),0) + 1 from tCustomerProfiles_Aud where CustomerPK = (select CustomerPK from inserted)
             
       if ((select COUNT(*) from inserted)<>0 and (select COUNT(*) from deleted)>0)
       begin
              insert into tCustomerProfiles_Aud(CustomerPK,
                     FirstName,
                     MiddleName,
                     LastName,
                     LastName2,
                     MothersMaidenName,
                     DOB,
                     Address1,
                     Address2,
                     City,
                     State,
                     ZipCode,
                     Phone1,
                     Phone1Type,
                     Phone1Provider,
                     Phone2,
                     Phone2Type,
                     Phone2Provider,
                     SSN,
                     TaxpayerId,
                     DoNotCall,
                     SMSEnabled,
                     MarketingSMSEnabled,
                     ChannelPartnerId,
                     DTCreate,
                     DTLastMod,
                     Gender,
                     Email,
                     PIN,
                     IsMailingAddressDifferent,
                     MailingAddress1,
                     MailingAddress2,
                     MailingCity,
                     MailingState,
                     MailingZipCode,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
              select CustomerPK, FirstName,MiddleName,LastName, LastName2, MothersMaidenName, DOB, Address1, Address2, City, State, ZipCode, Phone1, Phone1Type, Phone1Provider, Phone2, Phone2Type, Phone2Provider, SSN, TaxpayerId, DoNotCall, SMSEnabled,MarketingSMSEnabled,ChannelPartnerId, DTCreate, DTLastMod, Gender, Email, PIN, IsMailingAddressDifferent, MailingAddress1, MailingAddress2, MailingCity, MailingState, MailingZipCode,@RevisionNo,2 as AuditEvent,GETDATE() from inserted
       end
       else if(select COUNT(*) from inserted)>0 and (select COUNT(*) from deleted)=0
       begin
              insert into tCustomerProfiles_Aud(CustomerPK,
                     FirstName,
                     MiddleName,
                     LastName,
                     LastName2,
                     MothersMaidenName,
                     DOB,
                     Address1,
                     Address2,
                     City,
                     State,
                     ZipCode,
                     Phone1,
                     Phone1Type,
                     Phone1Provider,
                     Phone2,
                     Phone2Type,
                     Phone2Provider,
                     SSN,
                     TaxpayerId,
                     DoNotCall,
                     SMSEnabled,
                     MarketingSMSEnabled,
                     ChannelPartnerId,
                     DTCreate,
                     DTLastMod,
                     Gender,
                     Email,
                     PIN,
                     IsMailingAddressDifferent,
                     MailingAddress1,
                     MailingAddress2,
                     MailingCity,
                     MailingState,
                     MailingZipCode,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
              select CustomerPK, FirstName,MiddleName,LastName, LastName2, MothersMaidenName, DOB, Address1, Address2, City, State, ZipCode, Phone1, Phone1Type, Phone1Provider, Phone2, Phone2Type, Phone2Provider, SSN, TaxpayerId, DoNotCall, SMSEnabled,MarketingSMSEnabled,ChannelPartnerId, DTCreate, DTLastMod, Gender, Email, PIN, IsMailingAddressDifferent, MailingAddress1, MailingAddress2, MailingCity, MailingState, MailingZipCode,@RevisionNo,1 as AuditEvent,GETDATE() from inserted
       end
       else if(select COUNT(*) from deleted)>0
       begin
              insert into tCustomerProfiles_Aud(CustomerPK,
                     FirstName,
                     MiddleName,
                     LastName,
                     LastName2,
                     MothersMaidenName,
                     DOB,
                     Address1,
                     Address2,
                     City,
                     State,
                     ZipCode,
                     Phone1,
                     Phone1Type,
                     Phone1Provider,
                     Phone2,
                     Phone2Type,
                     Phone2Provider,
                     SSN,
                     TaxpayerId,
                     DoNotCall,
                     SMSEnabled,
                     MarketingSMSEnabled,
                     ChannelPartnerId,
                     DTCreate,
                     DTLastMod,
                     Gender,
                     Email,
                     PIN,
                     IsMailingAddressDifferent,
                     MailingAddress1,
                     MailingAddress2,
                     MailingCity,
                     MailingState,
                     MailingZipCode,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
              select CustomerPK, FirstName,MiddleName,LastName, LastName2, MothersMaidenName, DOB, Address1, Address2, City, State, ZipCode, Phone1, Phone1Type, Phone1Provider, Phone2, Phone2Type, Phone2Provider, SSN, TaxpayerId, DoNotCall, SMSEnabled,MarketingSMSEnabled,ChannelPartnerId, DTCreate, DTLastMod, Gender, Email, PIN, IsMailingAddressDifferent, MailingAddress1, MailingAddress2, MailingCity, MailingState, MailingZipCode,@RevisionNo,3 as AuditEvent,GETDATE() from deleted
       end
GO
 --=======================================================================================
ALTER TABLE dbo.tCustomerEmploymentDetails_Aud
ADD RevisionNo BIGINT NULL
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[trCustomerEmploymentDetailsAudit]') AND type in (N'TR'))
DROP TRIGGER [dbo].[trCustomerEmploymentDetailsAudit]
GO
create trigger trCustomerEmploymentDetailsAudit on tCustomerEmploymentDetails AFTER Insert, Update, Delete
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
                     DTCreate,
                     DTLastMod,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
              select CustomerPK,Occupation,Employer,EmployerPhone,DTCreate,DTLastMod,@RevisionNo,2 as AuditEvent,GETDATE() from inserted
       end
       else if(select COUNT(*) from inserted)>0 and (select COUNT(*) from deleted)=0
       begin
              insert into tCustomerEmploymentDetails_Aud(
					 CustomerPK,
                     Occupation,
                     Employer,
                     EmployerPhone,
                     DTCreate,
                     DTLastMod,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
              select CustomerPK,Occupation,Employer,EmployerPhone,DTCreate,DTLastMod,@RevisionNo,1 as AuditEvent,GETDATE() from inserted
       end
       else if(select COUNT(*) from deleted)>0
       begin
              insert into tCustomerEmploymentDetails_Aud(
					 CustomerPK,
                     Occupation,
                     Employer,
                     EmployerPhone,
                     DTCreate,
                     DTLastMod,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
              select CustomerPK,Occupation,Employer,EmployerPhone,DTCreate,DTLastMod,@RevisionNo,3 as AuditEvent,GETDATE() from deleted
       end
GO
 --=======================================================================================

ALTER TABLE tCustomerGovernmentIdDetails_Aud
ADD RevisionNo BIGINT NULL

GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[trCustomerGovernmentIdDetailsAudit]') AND type in (N'TR'))
DROP TRIGGER [dbo].[trCustomerGovernmentIdDetailsAudit]
GO
create trigger trCustomerGovernmentIdDetailsAudit on tCustomerGovernmentIdDetails AFTER Insert, Update, Delete
AS
       SET NOCOUNT ON
       Declare @RevisionNo bigint
       select @RevisionNo = isnull(MAX(RevisionNo),0) + 1 from tCustomerEmploymentDetails_Aud where CustomerPK = (select CustomerPK from inserted)
             
       if ((select COUNT(*) from inserted)<>0 and (select COUNT(*) from deleted)>0)
       begin
              insert into tCustomerGovernmentIdDetails_Aud(
					 CustomerPK,
                     IdTypeId,
                     Identification,
                     ExpirationDate,
                     DTCreate,
                     DTLastMod,
                     IssueDate,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
              select CustomerPK,IdTypeId,Identification,ExpirationDate,DTCreate,DTLastMod,IssueDate,@RevisionNo,2 as AuditEvent,GETDATE() from inserted
       end
       else if(select COUNT(*) from inserted)>0 and (select COUNT(*) from deleted)=0
       begin
              insert into tCustomerGovernmentIdDetails_Aud(
					 CustomerPK,
                     IdTypeId,
                     Identification,
                     ExpirationDate,
                     DTCreate,
                     DTLastMod,
                     IssueDate,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
              select CustomerPK,IdTypeId,Identification,ExpirationDate,DTCreate,DTLastMod,IssueDate,@RevisionNo,1 as AuditEvent,GETDATE() from inserted
       end
       else if(select COUNT(*) from deleted)>0
       begin
              insert into tCustomerGovernmentIdDetails_Aud(
					 CustomerPK,
                     IdTypeId,
                     Identification,
                     ExpirationDate,
                     DTCreate,
                     DTLastMod,
                     IssueDate,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
              select CustomerPK,IdTypeId,Identification,ExpirationDate,DTCreate,DTLastMod,IssueDate,@RevisionNo,3 as AuditEvent,GETDATE() from deleted
       end
 