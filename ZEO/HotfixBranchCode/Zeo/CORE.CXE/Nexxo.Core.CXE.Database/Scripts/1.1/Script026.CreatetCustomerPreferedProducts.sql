/****** Object:  Table [dbo].[tCustomerPreferedProducts]    Script Date: 05/22/2013 01:49:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('tCustomerPreferedProducts'))
BEGIN
CREATE TABLE [dbo].[tCustomerPreferedProducts](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[PAN] [bigint] NOT NULL,
	[ProductId] [bigint] NOT NULL,
	[AccountNumber] [nvarchar](30) NULL,
	[PhoneNumber] [bigint] NULL,
	[AccountPIN] [nvarchar](20) NULL,
	[AccountDOB] [datetime] NULL,
	[AccountName] [nvarchar](50) NULL,
	[Enabled] [bit] NULL,
	[DTCreate] [datetime] NULL,
	[DTLastMod] [datetime] NULL,
	[Operator] [nvarchar](20) NULL,
	[AgentId] [int] NULL,
	[TenantId] [nvarchar](20) NULL,
 CONSTRAINT [PK_tCustomerPreferedProducts] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[tCustomerPreferedProducts] ADD  CONSTRAINT [DF_tCustomerPreferedProducts_Enabled]  DEFAULT ((1)) FOR [Enabled]

ALTER TABLE [dbo].[tCustomerPreferedProducts] ADD  CONSTRAINT [DF_tCustomerPreferedProducts_DTCreate]  DEFAULT (getdate()) FOR [DTCreate]

END

