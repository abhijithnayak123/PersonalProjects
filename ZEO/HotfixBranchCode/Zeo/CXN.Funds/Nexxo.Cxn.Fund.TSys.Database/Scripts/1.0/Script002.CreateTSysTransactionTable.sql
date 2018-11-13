CREATE TABLE [dbo].[tTSys_Trx](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[AccountPK] [uniqueidentifier] NOT NULL,
	[TransactionType] [int] NOT NULL,
	[Amount] [money] NOT NULL,
	[Fee] [money] NULL,
	[Description] [nvarchar](200) NULL,
	[DTLocalTransaction] [datetime] NULL,
	[DTTransmission] [datetime] NULL,
	[Status] [int] NOT NULL,
	[ErrorCode] [nvarchar](50) NULL,
	[ErrorMsg] [nvarchar](50) NULL,
	[ConfirmationId] [nvarchar](50) NULL,
	[Balance] [money] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tTSys_Trx] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tTSys_Trx]  WITH CHECK ADD  CONSTRAINT [FK_tTSys_Trx_tTSys_Account] FOREIGN KEY([AccountPK])
REFERENCES [dbo].[tTSys_Account] ([rowguid])
GO

ALTER TABLE [dbo].[tTSys_Trx] CHECK CONSTRAINT [FK_tTSys_Trx_tTSys_Account]
GO

/****** Object:  Index [IX_tTSys_Trx_Id]    Script Date: 09/19/2013 20:40:41 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_tTSys_Trx_Id] ON [dbo].[tTSys_Trx] 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO



