CREATE TABLE [dbo].[tChxr_Trx_AUD](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NULL,
	[Amount] [money] NULL,
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
	[ChexarStatus] [nvarchar](100) NULL,
	[Type] [int] NULL,
	[ChexarType] [int] NULL,
	[DeclineCode] [int] NULL,
	[Message] [nvarchar](200) NULL,
	[Location] [nvarchar](50) NULL,
	[ChxrAccountPK] [uniqueidentifier] NULL,
	[DTCreate] [datetime] NULL,
	[DTLastMod] [datetime] NULL,
	[REV] [int] NOT NULL,
    [REVTYPE] [tinyint] NOT NULL,
 CONSTRAINT [PK_tChxr_Trx_AUD] PRIMARY KEY CLUSTERED 
(
	[rowguid],
	[REV]
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
