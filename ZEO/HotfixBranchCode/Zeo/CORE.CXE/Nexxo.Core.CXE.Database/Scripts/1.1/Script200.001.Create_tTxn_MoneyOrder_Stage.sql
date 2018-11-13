/****** Object:  Table [dbo].[tTxn_MoneyOrder_Stage]    Script Date: 10/08/2013 11:57:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tTxn_MoneyOrder_Stage](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[Amount] [money] NOT NULL,
	[Fee] [money] NOT NULL,
	[PurchaseDate] [datetime] NOT NULL,
	[MoneyOrderCheckNumber] [nvarchar](50) NULL,
	[AccountPK] [uniqueidentifier] NOT NULL,
	[Status] [int] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tTxn_MoneyOrder_Stage] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tTxn_MoneyOrder_Stage]  WITH CHECK ADD  CONSTRAINT [FK_tTxn_MoneyOrder_Stage_tCustomerAccounts] FOREIGN KEY([AccountPK])
REFERENCES [dbo].[tCustomerAccounts] ([rowguid])
GO

ALTER TABLE [dbo].[tTxn_MoneyOrder_Stage] CHECK CONSTRAINT [FK_tTxn_MoneyOrder_Stage_tCustomerAccounts]
GO

/****** Object:  Table [dbo].[tTxn_MoneyOrder_Commit]    Script Date: 10/08/2013 12:28:01 ******/
CREATE TABLE [dbo].[tTxn_MoneyOrder_Commit](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL,
	[Amount] [money] NOT NULL,
	[Fee] [money] NOT NULL,
	[PurchaseDate] [datetime] NOT NULL,
	[MoneyOrderCheckNumber] [nvarchar](50) NOT NULL,
	[AccountPK] [uniqueidentifier] NOT NULL,
	[Status] [int] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tTxn_MoneyOrder_Commit] PRIMARY KEY NONCLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tTxn_MoneyOrder_Commit]  WITH CHECK ADD  CONSTRAINT [FK_tTxn_MoneyOrder_Commit_tCustomerAccounts] FOREIGN KEY([AccountPK])
REFERENCES [dbo].[tCustomerAccounts] ([rowguid])
GO

ALTER TABLE [dbo].[tTxn_MoneyOrder_Commit] CHECK CONSTRAINT [FK_tTxn_MoneyOrder_Commit_tCustomerAccounts]
GO

/****** Object:  Index [IX_tTxn_MoneyOrder_Commit_AccountPK]    Script Date: 10/08/2013 12:28:01 ******/
CREATE CLUSTERED INDEX [IX_tTxn_MoneyOrder_Commit_AccountPK] ON [dbo].[tTxn_MoneyOrder_Commit] 
(
	[AccountPK] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

/****** Object:  Index [IX_tTxn_MoneyOrder_Commit_Id]    Script Date: 10/08/2013 12:28:01 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_tTxn_MoneyOrder_Commit_Id] ON [dbo].[tTxn_MoneyOrder_Commit] 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

