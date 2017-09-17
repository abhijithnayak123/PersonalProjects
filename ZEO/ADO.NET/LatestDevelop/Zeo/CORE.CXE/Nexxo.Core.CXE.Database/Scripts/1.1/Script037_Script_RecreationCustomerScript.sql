ALTER TABLE [dbo].[tCustomers]
ADD
	[FirstName] [nvarchar](255) NULL,
	[MiddleName] [nvarchar](255) NULL,
	[LastName] [nvarchar](255) NULL,
	[LastName2] [nvarchar](255) NULL,
	[MothersMaidenName] [nvarchar](255) NULL,
	[DOB] [datetime] NULL,
	[Address1] [nvarchar](255) NULL,
	[Address2] [nvarchar](255) NULL,
	[City] [nvarchar](255) NULL,
	[State] [nvarchar](255) NULL,
	[ZipCode] [nvarchar](255) NULL,
	[Phone1] [nvarchar](255) NULL,
	[Phone1Type] [nvarchar](255) NULL,
	[Phone1Provider] [nvarchar](255) NULL,
	[Phone2] [nvarchar](255) NULL,
	[Phone2Type] [nvarchar](255) NULL,
	[Phone2Provider] [nvarchar](255) NULL,
	[SSN] [nvarchar](255) NULL,
	[TaxpayerId] [nvarchar](255) NULL,
	[DoNotCall] [bit] NULL,
	[SMSEnabled] [bit] NULL,
	[MarketingSMSEnabled] [bit] NULL,
	[ChannelPartnerId] [bigint] NULL,
	[Gender] [nvarchar](6) NULL,
	[Email] [nvarchar](320) NULL,
	[PIN] [nvarchar](4) NULL,
	[IsMailingAddressDifferent] [bit] NULL,
	[MailingAddress1] [nvarchar](255) NULL,
	[MailingAddress2] [nvarchar](255) NULL,
	[MailingCity] [nvarchar](255) NULL,
	[MailingState] [nvarchar](255) NULL,
	[MailingZipCode] [nvarchar](255) NULL

	
GO

CREATE TABLE dbo.tCustomers_Aud
(
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL,
	[FirstName] [nvarchar](255) NULL,
	[MiddleName] [nvarchar](255) NULL,
	[LastName] [nvarchar](255) NULL,
	[LastName2] [nvarchar](255) NULL,
	[MothersMaidenName] [nvarchar](255) NULL,
	[DOB] [datetime] NULL,
	[Address1] [nvarchar](255) NULL,
	[Address2] [nvarchar](255) NULL,
	[City] [nvarchar](255) NULL,
	[State] [nvarchar](255) NULL,
	[ZipCode] [nvarchar](255) NULL,
	[Phone1] [nvarchar](255) NULL,
	[Phone1Type] [nvarchar](255) NULL,
	[Phone1Provider] [nvarchar](255) NULL,
	[Phone2] [nvarchar](255) NULL,
	[Phone2Type] [nvarchar](255) NULL,
	[Phone2Provider] [nvarchar](255) NULL,
	[SSN] [nvarchar](255) NULL,
	[TaxpayerId] [nvarchar](255) NULL,
	[DoNotCall] [bit] NULL,
	[SMSEnabled] [bit] NULL,
	[MarketingSMSEnabled] [bit] NULL,
	[ChannelPartnerId] [bigint] NULL,
	[DTCreate] [datetime] NULL,
	[DTLastMod] [datetime] NULL,
	[Gender] [nvarchar](6) NULL,
	[Email] [nvarchar](320) NULL,
	[PIN] [nvarchar](4) NULL,
	[IsMailingAddressDifferent] [bit] NULL,
	[MailingAddress1] [nvarchar](255) NULL,
	[MailingAddress2] [nvarchar](255) NULL,
	[MailingCity] [nvarchar](255) NULL,
	[MailingState] [nvarchar](255) NULL,
	[MailingZipCode] [nvarchar](255) NULL,
	RevisionNo Bigint null,
	AuditEvent smallint null,
	DTAudit datetime null
)
GO


--
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[trCustomersAudit]') AND type in (N'TR'))
DROP TRIGGER [dbo].[trCustomersAudit]
GO
create trigger trCustomersAudit on tCustomers AFTER Insert, Update, Delete
AS
       SET NOCOUNT ON
       Declare @RevisionNo bigint
       select @RevisionNo = isnull(MAX(RevisionNo),0) + 1 from tCustomers_Aud where Id = (select Id from inserted)
             
       if ((select COUNT(*) from inserted)<>0 and (select COUNT(*) from deleted)>0)
       begin
              insert into tCustomers_Aud(
					 rowguid,
                     Id,
                     FirstName,
                     MiddleName,
                     LastName,
                     LastName2,
                     MothersMaidenName,
                     DOB,
                     Address1,
                     Address2,
                     City,
                     [State],
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
                     DTAudit
                     )
              select rowguid, Id,  FirstName, MiddleName, LastName, LastName2, MothersMaidenName, DOB, Address1, Address2, City, [State], ZipCode, Phone1, Phone1Type, Phone1Provider, Phone2, Phone2Type, Phone2Provider, SSN, TaxpayerId, DoNotCall, SMSEnabled, MarketingSMSEnabled, ChannelPartnerId, DTCreate, DTLastMod, Gender, Email, PIN, IsMailingAddressDifferent, MailingAddress1, MailingAddress2, MailingCity, MailingState, MailingZipCode, @RevisionNo,2 as AuditEvent,GETDATE() from inserted
       end
       else if(select COUNT(*) from inserted)>0 and (select COUNT(*) from deleted)=0
       begin
              insert into tCustomers_Aud(
              rowguid,
                     Id,
                     FirstName,
                     MiddleName,
                     LastName,
                     LastName2,
                     MothersMaidenName,
                     DOB,
                     Address1,
                     Address2,
                     City,
                     [State],
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
                     DTAudit
              )
              select rowguid, Id,  FirstName, MiddleName, LastName, LastName2, MothersMaidenName, DOB, Address1, Address2, City, [State], ZipCode, Phone1, Phone1Type, Phone1Provider, Phone2, Phone2Type, Phone2Provider, SSN, TaxpayerId, DoNotCall, SMSEnabled, MarketingSMSEnabled, ChannelPartnerId, DTCreate, DTLastMod, Gender, Email, PIN, IsMailingAddressDifferent, MailingAddress1, MailingAddress2, MailingCity, MailingState, MailingZipCode,@RevisionNo,1 as AuditEvent,GETDATE() from inserted
       end
       else if(select COUNT(*) from deleted)>0
       begin
              insert into tCustomers_Aud(
              rowguid,
                     Id,
                     FirstName,
                     MiddleName,
                     LastName,
                     LastName2,
                     MothersMaidenName,
                     DOB,
                     Address1,
                     Address2,
                     City,
                     [State],
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
                     DTAudit
              )
              select rowguid, Id,  FirstName, MiddleName, LastName, LastName2, MothersMaidenName, DOB, Address1, Address2, City, [State], ZipCode, Phone1, Phone1Type, Phone1Provider, Phone2, Phone2Type, Phone2Provider, SSN, TaxpayerId, DoNotCall, SMSEnabled, MarketingSMSEnabled, ChannelPartnerId, DTCreate, DTLastMod, Gender, Email, PIN, IsMailingAddressDifferent, MailingAddress1, MailingAddress2, MailingCity, MailingState, MailingZipCode,@RevisionNo,3 as AuditEvent,GETDATE() from deleted
       end
 GO


