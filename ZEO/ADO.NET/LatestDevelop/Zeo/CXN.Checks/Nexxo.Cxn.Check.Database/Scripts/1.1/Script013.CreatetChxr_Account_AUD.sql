CREATE TABLE [dbo].[tChxr_Account_AUD](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL,
	[Badge] [int] NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[ITIN] [nvarchar](20) NULL,
	[SSN] [nvarchar](20) NULL,
	[DateOfBirth] [datetime] NOT NULL,
	[Address1] [nvarchar](100) NOT NULL,
	[Address2] [nvarchar](50) NULL,
	[City] [nvarchar](50) NOT NULL,
	[State] [nvarchar](2) NOT NULL,
	[Zip] [nvarchar](50) NOT NULL,
	[Phone] [nvarchar](20) NOT NULL,
	[Occupation] [nvarchar](100) NULL,
	[Employer] [nvarchar](100) NULL,
	[EmployerPhone] [nvarchar](100) NULL,
	[IDCardType] [nvarchar](100) NULL,
	[IDCardNumber] [nvarchar](50) NULL,
	[IDCardIssuedCountry] [nvarchar](50) NULL,
	[IDCardIssuedDate] [datetime] NULL,
	[IDCardImage] [varbinary](max) NULL,
	[IDCardExpireDate] [datetime] NULL,
	[CardNumber] [nvarchar](50) NULL,
	[CustomerScore] [int] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
	[REV] [int] NOT NULL,
	[REVTYPE] [tinyint] NOT NULL,
 CONSTRAINT [PK_tChxr_Account_AUD] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC,
	[REV] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO
