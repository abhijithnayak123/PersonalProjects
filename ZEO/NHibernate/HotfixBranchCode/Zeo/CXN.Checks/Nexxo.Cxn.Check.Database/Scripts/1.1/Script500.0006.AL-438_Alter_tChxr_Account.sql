-- ============================================================================
-- Author:		<Mohammed Khaja Firoz Molla>
-- Create date: <07/15/2015>
-- Description:	<Script to Add Identity to create to ChxrAccountID Column>
-- Jira ID:	<AL-438>
-- =============================================================================

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT


BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_tChxr_Account
	(
	ChxrAccountPK uniqueidentifier NOT NULL,
	ChxrAccountID bigint NOT NULL IDENTITY (1000000000, 1),
	Badge int NOT NULL,
	FirstName nvarchar(50) NOT NULL,
	LastName nvarchar(50) NOT NULL,
	ITIN nvarchar(20) NULL,
	SSN nvarchar(20) NULL,
	DateOfBirth datetime NOT NULL,
	Address1 nvarchar(100) NOT NULL,
	Address2 nvarchar(50) NULL,
	City nvarchar(50) NOT NULL,
	State nvarchar(2) NOT NULL,
	Zip nvarchar(50) NOT NULL,
	Phone nvarchar(20) NOT NULL,
	Occupation nvarchar(100) NULL,
	Employer nvarchar(100) NULL,
	EmployerPhone nvarchar(100) NULL,
	IDCardType nvarchar(100) NULL,
	IDCardNumber nvarchar(50) NULL,
	IDCardIssuedCountry nvarchar(50) NULL,
	IDCardIssuedDate datetime NULL,
	IDCardImage varbinary(MAX) NULL,
	IDCardExpireDate datetime NULL,
	CardNumber nvarchar(50) NULL,
	CustomerScore int NOT NULL,
	DTServerCreate datetime NOT NULL,
	DTServerLastModified datetime NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE dbo.Tmp_tChxr_Account SET (LOCK_ESCALATION = TABLE)
GO

SET IDENTITY_INSERT dbo.Tmp_tChxr_Account ON
GO

IF EXISTS(SELECT * FROM dbo.tChxr_Account)
	 EXEC('INSERT INTO dbo.Tmp_tChxr_Account (ChxrAccountPK, ChxrAccountID, Badge, FirstName, LastName, ITIN, SSN, DateOfBirth, Address1, Address2, City, State, Zip, Phone, Occupation, Employer, EmployerPhone, IDCardType, IDCardNumber, IDCardIssuedCountry, IDCardIssuedDate, IDCardImage, IDCardExpireDate, CardNumber, CustomerScore, DTServerCreate, DTServerLastModified)
		SELECT ChxrAccountPK, ChxrAccountID, Badge, FirstName, LastName, ITIN, SSN, DateOfBirth, Address1, Address2, City, State, Zip, Phone, Occupation, Employer, EmployerPhone, IDCardType, IDCardNumber, IDCardIssuedCountry, IDCardIssuedDate, IDCardImage, IDCardExpireDate, CardNumber, CustomerScore, DTServerCreate, DTServerLastModified FROM dbo.tChxr_Account WITH (HOLDLOCK TABLOCKX)')
GO

SET IDENTITY_INSERT dbo.Tmp_tChxr_Account OFF
GO

ALTER TABLE dbo.tChxr_Trx
	DROP CONSTRAINT FK_tChxr_Trx_tChxr_Account
GO

DROP TABLE dbo.tChxr_Account
GO

EXECUTE sp_rename N'dbo.Tmp_tChxr_Account', N'tChxr_Account', 'OBJECT' 
GO

ALTER TABLE dbo.tChxr_Account ADD CONSTRAINT PK_tChxr_Account PRIMARY KEY CLUSTERED (ChxrAccountPK) 
WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO

CREATE UNIQUE NONCLUSTERED INDEX IX_tChxr_Account_Id ON dbo.tChxr_Account (ChxrAccountID) 
WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE trigger [dbo].[trChxr_AccountAudit] on dbo.tChxr_Account AFTER Insert, Update, Delete
AS
       SET NOCOUNT ON
       Declare @RevisionNo bigint
       select @RevisionNo = isnull(MAX(RevisionNo),0) + 1 from tChxr_Account_Aud where ChxrAccountID = (select ChxrAccountID from inserted)
             
       if ((select COUNT(*) from inserted)<>0 and (select COUNT(*) from deleted)>0)
       begin
              insert into tChxr_Account_Aud(
					 ChxrAccountPK,
                     ChxrAccountID,
                     Badge,
                     FirstName,
                     LastName,
                     ITIN,
                     SSN,
                     DateOfBirth,
                     Address1,
                     Address2,
                     City,
                     [State],
                     Zip,
                     Phone,
                     Occupation,
                     Employer,
                     EmployerPhone,
                     IDCardType,
                     IDCardNumber,
                     IDCardIssuedCountry,
                     IDCardIssuedDate,
                     IDCardImage,
                     IDCardExpireDate,
                     CardNumber,
                     CustomerScore,
                     DTServerCreate,
                     DTServerLastModified,
                     RevisionNo,
                     AuditEvent,
                     DTAudit
                     )
              select ChxrAccountPK, ChxrAccountID, Badge, FirstName, LastName, ITIN, SSN, DateOfBirth, Address1, Address2, City, [State], Zip, Phone, Occupation, Employer, EmployerPhone, IDCardType, IDCardNumber, IDCardIssuedCountry, IDCardIssuedDate, IDCardImage, IDCardExpireDate, CardNumber, CustomerScore, DTServerCreate, DTServerLastModified,@RevisionNo,2 as AuditEvent,GETDATE() from inserted
       end
       else if(select COUNT(*) from inserted)>0 and (select COUNT(*) from deleted)=0
       begin
              insert into tChxr_Account_Aud(
					ChxrAccountPK,
                     ChxrAccountID,
                     Badge,
                     FirstName,
                     LastName,
                     ITIN,
                     SSN,
                     DateOfBirth,
                     Address1,
                     Address2,
                     City,
                     [State],
                     Zip,
                     Phone,
                     Occupation,
                     Employer,
                     EmployerPhone,
                     IDCardType,
                     IDCardNumber,
                     IDCardIssuedCountry,
                     IDCardIssuedDate,
                     IDCardImage,
                     IDCardExpireDate,
                     CardNumber,
                     CustomerScore,
                     DTServerCreate,
                     DTServerLastModified,
                     RevisionNo,
                     AuditEvent,
                     DTAudit
                     )
              select ChxrAccountPK, ChxrAccountID, Badge, FirstName, LastName, ITIN, SSN, DateOfBirth, Address1, Address2, City, [State], Zip, Phone, Occupation, Employer, EmployerPhone, IDCardType, IDCardNumber, IDCardIssuedCountry, IDCardIssuedDate, IDCardImage, IDCardExpireDate, CardNumber, CustomerScore, DTServerCreate, DTServerLastModified,@RevisionNo,1 as AuditEvent,GETDATE() from inserted
       end
       else if(select COUNT(*) from deleted)>0
       begin
              insert into tChxr_Account_Aud(
					 ChxrAccountPK,
                    ChxrAccountID,
                     Badge,
                     FirstName,
                     LastName,
                     ITIN,
                     SSN,
                     DateOfBirth,
                     Address1,
                     Address2,
                     City,
                     [State],
                     Zip,
       Phone,
                     Occupation,
                     Employer,
                     EmployerPhone,
                     IDCardType,
                     IDCardNumber,
                     IDCardIssuedCountry,
                     IDCardIssuedDate,
                     IDCardImage,
                     IDCardExpireDate,
                     CardNumber,
                     CustomerScore,
                     DTServerCreate,
                     DTServerLastModified,
                     RevisionNo,
                     AuditEvent,
                     DTAudit
                     )
              select ChxrAccountPK, ChxrAccountID, Badge, FirstName, LastName, ITIN, SSN, DateOfBirth, Address1, Address2, City, [State], Zip, Phone, Occupation, Employer, EmployerPhone, IDCardType, IDCardNumber, IDCardIssuedCountry, IDCardIssuedDate, IDCardImage, IDCardExpireDate, CardNumber, CustomerScore, DTServerCreate, DTServerLastModified,@RevisionNo,3 as AuditEvent,GETDATE() from deleted
       end
GO
COMMIT

BEGIN TRANSACTION
GO

ALTER TABLE dbo.tChxr_Trx ADD CONSTRAINT FK_tChxr_Trx_tChxr_Account FOREIGN KEY (ChxrAccountPK) 
REFERENCES dbo.tChxr_Account(ChxrAccountPK) ON UPDATE  NO ACTION ON DELETE  NO ACTION 	
GO

ALTER TABLE dbo.tChxr_Trx SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
