﻿/****** Object:  Table [dbo].[tTxn_Funds_Stage]    Script Date: 04/25/2013 10:10:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tTxn_Funds_Stage](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[Amount] [money] NOT NULL,
	[Fee] [money] NOT NULL,
	[AccountPK] [uniqueidentifier] NOT NULL,
	[Type] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tTxn_Funds_Stage] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tTxn_Funds_Stage]  WITH CHECK ADD  CONSTRAINT [FK_tTxn_Funds_Stage_tCustomerAccounts] FOREIGN KEY([AccountPK])
REFERENCES [dbo].[tCustomerAccounts] ([rowguid])
GO

ALTER TABLE [dbo].[tTxn_Funds_Stage] CHECK CONSTRAINT [FK_tTxn_Funds_Stage_tCustomerAccounts]
GO

/****** Object:  Index [IX_tTxn_Funds_Stage_Id]    Script Date: 04/25/2013 08:58:39 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_tTxn_Funds_Stage_Id] ON [dbo].[tTxn_Funds_Stage] 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[tTxn_Funds_Commit]    Script Date: 04/25/2013 11:17:28 ******/
CREATE TABLE [dbo].[tTxn_Funds_Commit](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL,
	[Amount] [money] NOT NULL,
	[Fee] [money] NOT NULL,
	[AccountPK] [uniqueidentifier] NOT NULL,
	[Type] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tTxn_Funds_Commit] PRIMARY KEY NONCLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tTxn_Funds_Commit]  WITH CHECK ADD  CONSTRAINT [FK_tTxn_Funds_Commit_tCustomerAccounts] FOREIGN KEY([AccountPK])
REFERENCES [dbo].[tCustomerAccounts] ([rowguid])
GO

ALTER TABLE [dbo].[tTxn_Funds_Commit] CHECK CONSTRAINT [FK_tTxn_Funds_Commit_tCustomerAccounts]
GO

/****** Object:  Index [IX_tTxn_Funds_Commit_AccountPK]    Script Date: 04/25/2013 11:16:43 ******/
CREATE CLUSTERED INDEX [IX_tTxn_Funds_Commit_AccountPK] ON [dbo].[tTxn_Funds_Commit] 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

/****** Object:  Index [tTxn_Funds_Commit_Id]    Script Date: 04/25/2013 11:18:27 ******/
CREATE UNIQUE NONCLUSTERED INDEX [tTxn_Funds_Commit_Id] ON [dbo].[tTxn_Funds_Commit] 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

