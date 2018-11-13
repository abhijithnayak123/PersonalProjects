IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tTxn_Cash_Stage_tCustomerAccounts]') AND parent_object_id = OBJECT_ID(N'[dbo].[tTxn_Cash_Stage]'))
ALTER TABLE [dbo].[tTxn_Cash_Stage] DROP CONSTRAINT [FK_tTxn_Cash_Stage_tCustomerAccounts]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tTxn_Cash_Stage]') AND type in (N'U'))
DROP TABLE [dbo].[tTxn_Cash_Stage]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tTxn_Cash_Stage](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[Amount] [money] NOT NULL,
	[Fee] [money] NOT NULL,
	[AccountPK] [uniqueidentifier] NOT NULL,
	[CashTrxType] [int] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tTxn_Cash_Stage] PRIMARY KEY NONCLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tTxn_Cash_Stage]  WITH CHECK ADD  CONSTRAINT [FK_tTxn_Cash_Stage_tCustomerAccounts] FOREIGN KEY([AccountPK])
REFERENCES [dbo].[tCustomerAccounts] ([rowguid])
GO

ALTER TABLE [dbo].[tTxn_Cash_Stage] CHECK CONSTRAINT [FK_tTxn_Cash_Stage_tCustomerAccounts]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tTxn_Cash_Commit_tCustomerAccounts]') AND parent_object_id = OBJECT_ID(N'[dbo].[tTxn_Cash_Commit]'))
ALTER TABLE [dbo].[tTxn_Cash_Commit] DROP CONSTRAINT [FK_tTxn_Cash_Commit_tCustomerAccounts]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tTxn_Cash_Commit]') AND type in (N'U'))
DROP TABLE [dbo].[tTxn_Cash_Commit]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tTxn_Cash_Commit](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL,
	[Amount] [money] NOT NULL,
	[Fee] [money] NOT NULL,
	[AccountPK] [uniqueidentifier] NOT NULL,
	[CashTrxType] [int] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tTxn_Cash_Commit] PRIMARY KEY NONCLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tTxn_Cash_Commit]  WITH CHECK ADD  CONSTRAINT [FK_tTxn_Cash_Commit_tCustomerAccounts] FOREIGN KEY([AccountPK])
REFERENCES [dbo].[tCustomerAccounts] ([rowguid])
GO

ALTER TABLE [dbo].[tTxn_Cash_Commit] CHECK CONSTRAINT [FK_tTxn_Cash_Commit_tCustomerAccounts]
GO

