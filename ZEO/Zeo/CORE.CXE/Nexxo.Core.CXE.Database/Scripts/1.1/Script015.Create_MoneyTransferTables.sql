/****** Object:  Table [dbo].[tTxn_MoneyTransfer_Stage]    Script Date: 04/25/2013 13:28:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tTxn_MoneyTransfer_Stage](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[Amount] [money] NOT NULL,
	[Fee] [money] NOT NULL,
	[AccountPK] [uniqueidentifier] NOT NULL,
	[Status] [int] NOT NULL,
	[ConfirmationNumber] [nvarchar](50) NULL,
    [ReceiverName] [nvarchar](100) NULL,
    [Destination] [nvarchar](200) NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tTxn_MoneyTransfer_Stage] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tTxn_MoneyTransfer_Stage]  WITH CHECK ADD CONSTRAINT [FK_tTxn_MoneyTransfer_Stage_tCustomerAccounts] FOREIGN KEY([AccountPK])
REFERENCES [dbo].[tCustomerAccounts] ([rowguid])
GO

ALTER TABLE [dbo].[tTxn_MoneyTransfer_Stage] CHECK CONSTRAINT [FK_tTxn_MoneyTransfer_Stage_tCustomerAccounts]
GO

/****** Object:  Index [IX_tTxn_MoneyTransfer_Stage_Id]    Script Date: 04/25/2013 13:28:22 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_tTxn_MoneyTransfer_Stage_Id] ON [dbo].[tTxn_MoneyTransfer_Stage] 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[tTxn_MoneyTransfer_Commit]    Script Date: 04/25/2013 13:28:39 ******/
CREATE TABLE [dbo].[tTxn_MoneyTransfer_Commit](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL,
	[Amount] [money] NOT NULL,
	[Fee] [money] NOT NULL,
	[AccountPK] [uniqueidentifier] NOT NULL,
	[Status] [int] NOT NULL,
	[ConfirmationNumber] [nvarchar](50) NULL,
    [ReceiverName] [nvarchar](100) NULL,
    [Destination] [nvarchar](200) NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tTxn_MoneyTransfer_Commit] PRIMARY KEY NONCLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tTxn_MoneyTransfer_Commit]  WITH CHECK ADD CONSTRAINT [FK_tTxn_MoneyTransfer_Commit_tCustomerAccounts] FOREIGN KEY([AccountPK])
REFERENCES [dbo].[tCustomerAccounts] ([rowguid])
GO

ALTER TABLE [dbo].[tTxn_MoneyTransfer_Commit] CHECK CONSTRAINT [FK_tTxn_MoneyTransfer_Commit_tCustomerAccounts]
GO

/****** Object:  Index [IX_tTxn_MoneyTransfer_Commit_AccountPK]    Script Date: 04/25/2013 15:04:30 ******/
CREATE CLUSTERED INDEX [IX_tTxn_MoneyTransfer_Commit_AccountPK] ON [dbo].[tTxn_MoneyTransfer_Commit] 
(
	[AccountPK] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

/****** Object:  Index [IX_tTxn_MoneyTransfer_Stage_Id]    Script Date: 04/25/2013 13:28:22 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_tTxn_MoneyTransfer_Commit_Id] ON [dbo].[tTxn_MoneyTransfer_Commit] 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO