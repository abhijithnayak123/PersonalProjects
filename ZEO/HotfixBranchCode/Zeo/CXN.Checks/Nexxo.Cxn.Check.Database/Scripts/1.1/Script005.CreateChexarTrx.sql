CREATE TABLE [dbo].[tChxr_Trx](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL,
	[Amount] [money] NOT NULL,
	[ChexarAmount] [money] NULL,
	[ChexarFee] [money] NULL,
	[CheckDate] [datetime] NULL,
	[CheckNumber] [nvarchar](20) NULL,
	[RoutingNumber] [nvarchar](20) NULL,
	[AccountNumber] [nvarchar](20) NULL,
	[Micr] [nvarchar](50) NULL,
	[Latitude] [float] NULL,
	[Longitude] [float] NULL,
	[InvoiceId] [int] NULL,
	[TicketId] [int] NULL,
	[WaitTime] [nvarchar](50) NULL,
	[Status] [int] NOT NULL,
	[ChexarStatus] [nvarchar](50) NOT NULL,
	[Type] [int] NULL,
	[ChexarType] [int] NULL,
	[DeclineCode] [int] NULL,
	[Message] [nvarchar](200) NULL,
	[Location] [nvarchar](50) NOT NULL,
	[ChxrAccountPK] [uniqueidentifier] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tChxr_Trx] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tChxr_Trx]  WITH CHECK ADD  CONSTRAINT [FK_tChxr_Trx_tChxr_Account] FOREIGN KEY([ChxrAccountPK])
REFERENCES [dbo].[tChxr_Account] ([rowguid])
GO

ALTER TABLE [dbo].[tChxr_Trx] CHECK CONSTRAINT [FK_tChxr_Trx_tChxr_Account]
GO

/****** Object:  Index [IX_tChxr_Trx_Id]    Script Date: 05/07/2013 14:11:47 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_tChxr_Trx_Id] ON [dbo].[tChxr_Trx] 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

