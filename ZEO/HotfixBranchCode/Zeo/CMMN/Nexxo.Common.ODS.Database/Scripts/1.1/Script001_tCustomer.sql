/****** Object:  Table [dbo].[tCustomer]    Script Date: 08/14/2013 20:21:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tCustomer](
	[DTServer] [datetime] NOT NULL,
	[CustomerId] [bigint] NULL,
	[CardNumber] [nvarchar](34) NULL,
	[FirstName] [nvarchar](255) NULL,
	[MiddleName] [nvarchar](255) NULL,
	[LastName] [nvarchar](255) NULL,
	[SecondLastName] [nvarchar](255) NULL,
	[Gender] [nvarchar](6) NULL,
	[HomeAddress1] [nvarchar](255) NULL,
	[HomeAddress2] [nvarchar](255) NULL,
	[City] [nvarchar](255) NULL,
	[State] [nvarchar](255) NULL,
	[Zipcode] [nvarchar](255) NULL,
	[CustomerDOB] [datetime] NULL,
	[MothersMaidenName] [nvarchar](255) NULL,
	[SSN] [nvarchar](255) NULL,
	[CountryName] [nvarchar](70) NULL,
	[GovtIdType] [nvarchar](100) NULL,
	[IdIssuingEntity] [nvarchar](50) NULL,
	[IdIssuingDate] [datetime] NULL,
	[IdNumber] [nvarchar](255) NULL,
	[IdExpirationDate] [datetime] NULL,
	[EmailAddress] [nvarchar](320) NULL,
	[DoNotCall] [bit] NULL,
	[Occupation] [nvarchar](255) NULL,
	[NexxoPAN] [bigint] NULL,
	[AlternatePhone] [nvarchar](255) NULL,
	[PrimaryPhone] [nvarchar](255) NULL,
	[PhoneType] [nvarchar](255) NULL,
	[LastUpdate] [datetime] NULL
) ON [PRIMARY]

GO


