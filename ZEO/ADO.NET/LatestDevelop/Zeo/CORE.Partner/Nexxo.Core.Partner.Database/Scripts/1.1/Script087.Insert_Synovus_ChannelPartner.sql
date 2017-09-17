INSERT	[tChannelPartners] 
(
	[rowguid],
	[id],
	[Name],
	[FeesFollowCustomer],
	[CashFeeDescriptionEN],
	[CashFeeDescriptionES],
	[DebitFeeDescriptionEN],
	[DebitFeeDescriptionES],
	[ConvenienceFeeCash],
	[ConvenienceFeeDebit],
	[ConvenienceFeeDescriptionEN],
	[ConvenienceFeeDescriptionES],
	[CanCashCheckWOGovtId],
	[LogoFileName],
	[IsEFSPartner],
	[EFSClientId],
	[UsePINForNonGPR],
	[IsCUPartner],
	[HasNonGPRCard],
	[ManagesCash],
	[AllowPhoneNumberAuthentication],
	[ComplianceProgramName]
)		
VALUES 
(
	'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17',
	33,
	N'Synovus',
	0,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	0,
	N'synovusLogo.png',
	NULL,
	3,
	1,
	1,
	1,
	0,
	NULL,
	'SynovusTest'
 )
GO

INSERT [dbo].[tSkins] 
(
	[Id],
	[ChannelPartnerId],
	[ValueType],
	[Name],
	[ValueEn],
	[ValueEs],
	[DTCreate],
	[DTLastMod]
) 
VALUES 
(
	NEWID(),
	33,
	1,
	N'PhoneNumber',
	N'866-340-6392',
	N'866-340-6392',
	CAST(0x0000A15600DA95D7 AS DateTime),
	NULL
)
GO

INSERT [tTipsAndOffers] 
(
	[Id],
	[ViewName],
	[ChannelPartnerName],
	[TipsAndOffersEn],
	[TipsAndOffersEs],
	[OptionalFilter],
	[DTCreate],
	[DTLastMod]
) 
VALUES 
(
	13,
	N'TransactionSummary',
	N'Synovus',
	N'<p>Load and withdraw amounts can be adjusted during the check-out process</p>',
	N'<p>Load and withdraw amounts can be adjusted during the check-out process</p>',
	N'Prepaid Card',
	NULL,
	NULL
),
(
	14,
	N'CardTransactionSummary',
	N'Synovus',
	N'<p>Load and withdraw amounts can be adjusted during the check-out process</p>',
	N'<p>Load and withdraw amounts can be adjusted during the check-out process</p>',
	NULL,
	NULL,
	NULL
 )
GO

--script to insert record into tlocations
INSERT	tLocations 
(
	[rowguid],
	[LocationName],
	[IsActive],
	[Address1],
	[Address2],
	[City],
	[State],
	[ZipCode],
	[DTCreate],
	[DTLastMod],
	[ChannelPartnerId]
)
VALUES	
(
	'BC46F466-16D3-47B9-97CC-A9F95E2A2CCB',
	'Synovus',
	1,
	'test',
	'test',
	'test',
	'CA',
	'12345',
	'2013-05-20 22:37:21.000',
	'2013-05-21 01:09:34.000',
	33
)
GO

--script to insert record into tterminals
INSERT	tTerminals 
(
	[rowguid],
	[Name],
	[MacAddress],
	[IpAddress],
	[LocationPK],
	[NpsTerminalPK],
	[DTCreate],
	[DTLastMod]
)
VALUES	
(
	NEWID(),
	'Synovus',
	'',
	'10.111.109.16',
	'BC46F466-16D3-47B9-97CC-A9F95E2A2CCB',
	null,
	GETDATE(),
	GETDATE()
)
GO

--script for inserting sysadmin user
SET IDENTITY_INSERT tAgentDetails ON
GO

INSERT tAgentDetails
(
	rowguid,
	Id,
	UserName,
	FirstName,
	LastName,
	FullName,
	IsEnabled,
	ManagerId,
	PrimaryLocationId,
	UserRoleId,
	UserStatusId,
	ChannelPartnerId,
	PhoneNumber,
	Email,
	Notes,
	DTLastLogin,
	DTCreate,
	DTLastMod
) 
VALUES	
(
	NEWID(),
	200000,
	'SysAdmin',
	'System',
	'Administrator',
	'System Administrator',
	1,
	200000,
	33,
	4,
	1,
	33,
	NULL,
	'',
	NULL,
	NULL,
	GETDATE(),
	GETDATE()
)
GO
SET IDENTITY_INSERT tAgentDetails OFF
GO

INSERT tAgentAuthentication 
(
	AgentId,
	UserName,
	PasswordHash,
	Salt,
	AuthenticationFailures,
	TemporaryPassword,
	DTLastPasswordUpdate,
	LastPasswordUpdateBy
)
VALUES
(
	200000,
	'SysAdmin',
	'cBCHMLnHbeU5aBy4',
	'2SwFOic=',
	0,
	1,
	GETDATE(),
	GETDATE()
)
GO

INSERT tAgentLocationMapping 
(
	AgentId,
	LocationId
)
VALUES
(
	200000,
	33
)
GO
