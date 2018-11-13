CREATE TABLE [dbo].[tChxrSim_Account](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Badge] [int] IDENTITY(10000000,1) NOT NULL,
	[FName] [nvarchar](50) NULL,
	[LName] [nvarchar](50) NULL,
	[ITIN] [nvarchar](50) NULL,
	[SSN] [nvarchar](50) NULL,
	[DOB] [datetime] NULL,
	[Address1] [nvarchar](200) NULL,
	[Address2] [nvarchar](200) NULL,
	[City] [nvarchar](100) NULL,
	[State] [nvarchar](100) NULL,
	[Zip] [nvarchar](50) NULL,
	[Phone] [nvarchar](50) NULL,
	[Occupation] [nvarchar](200) NULL,
	[Employer] [nvarchar](200) NULL,
	[EmployerPhone] [nvarchar](100) NULL,
	[ID] [nvarchar](50) NULL,
	[IDType] [int] NULL,
	[IDCountry] [nvarchar](50) NULL,
	[IDExpDate] [datetime] NULL,
	[IDImage] [varbinary](max) NULL,
	[CardNumber] [nvarchar](50) NULL,
	[CustomerScore] [int] NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tChxrSim_Account] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

