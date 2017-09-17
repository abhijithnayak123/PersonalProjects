
drop table tTxn_Cash

CREATE TABLE [dbo].[tTxn_Cash](
    [txnRowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL,
	[CXEId] [bigint] NOT NULL,
	[CXNId] [bigint] NULL,
    [LedgerEntryPK] [uniqueidentifier] NULL,
	CustomerSessionPK [uniqueidentifier] NOT NULL,
	AccountPK [uniqueidentifier] NOT NULL,
	Amount [money] NULL,
	Fee [money] NULL,
	[Description] [nvarchar](255) NULL,
	CXEState [int] NULL,
	CXNState [int] NULL,
    [DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
CONSTRAINT [PK_tTxn_Cash] PRIMARY KEY CLUSTERED 
(
    [txnRowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
 
GO
 
ALTER TABLE [dbo].[tTxn_Cash]  WITH CHECK ADD CONSTRAINT [FK_tTxn_Cash_tLedgerEntries] FOREIGN KEY([LedgerEntryPK])
REFERENCES [dbo].[tLedgerEntries] ([rowguid])
GO
 
ALTER TABLE [dbo].[tTxn_Cash] CHECK CONSTRAINT [FK_tTxn_Cash_tLedgerEntries]
GO

ALTER TABLE [dbo].[tTxn_Cash]  WITH CHECK ADD CONSTRAINT [FK_tTxn_Cash_tCustomerSessions] FOREIGN KEY([CustomerSessionPK])
REFERENCES [dbo].[tCustomerSessions] ([CustomerSessionRowguid])
GO
 
ALTER TABLE [dbo].[tTxn_Cash] CHECK CONSTRAINT [FK_tTxn_Cash_tCustomerSessions]
GO

ALTER TABLE [dbo].[tTxn_Cash]  WITH CHECK ADD CONSTRAINT [FK_tTxn_Cash_tAccounts] FOREIGN KEY([AccountPK])
REFERENCES [dbo].[tAccounts] ([rowguid])
GO
 
ALTER TABLE [dbo].[tTxn_Cash] CHECK CONSTRAINT [FK_tTxn_Cash_tAccounts]
GO
 
---------------

drop table tTxn_Check

CREATE TABLE [dbo].[tTxn_Check](
    [txnRowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL,
	[CXEId] [bigint] NOT NULL,
	[CXNId] [bigint] NULL,
    [LedgerEntryPK] [uniqueidentifier] NULL,
	CustomerSessionPK [uniqueidentifier] NOT NULL,
	AccountPK [uniqueidentifier] NOT NULL,
	Amount [money] NULL,
	Fee [money] NULL,
	[Description] [nvarchar](255) NULL,
	CXEState [int] NULL,
	CXNState [int] NULL,
    [DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
CONSTRAINT [PK_tTxn_Check] PRIMARY KEY CLUSTERED 
(
    [txnRowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
 
GO
 
ALTER TABLE [dbo].[tTxn_Check]  WITH CHECK ADD CONSTRAINT [FK_tTxn_Check_tLedgerEntries] FOREIGN KEY([LedgerEntryPK])
REFERENCES [dbo].[tLedgerEntries] ([rowguid])
GO
 
ALTER TABLE [dbo].[tTxn_Check] CHECK CONSTRAINT [FK_tTxn_Check_tLedgerEntries]
GO

ALTER TABLE [dbo].[tTxn_Check]  WITH CHECK ADD CONSTRAINT [FK_tTxn_Check_tCustomerSessions] FOREIGN KEY([CustomerSessionPK])
REFERENCES [dbo].[tCustomerSessions] ([CustomerSessionRowguid])
GO
 
ALTER TABLE [dbo].[tTxn_Check] CHECK CONSTRAINT [FK_tTxn_Check_tCustomerSessions]
GO

ALTER TABLE [dbo].[tTxn_Check]  WITH CHECK ADD CONSTRAINT [FK_tTxn_Check_tAccounts] FOREIGN KEY([AccountPK])
REFERENCES [dbo].[tAccounts] ([rowguid])
GO
 
ALTER TABLE [dbo].[tTxn_Check] CHECK CONSTRAINT [FK_tTxn_Check_tAccounts]
GO
 
---------------

drop table tTxn_BillPay

CREATE TABLE [dbo].[tTxn_BillPay](
    [txnRowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL,
	[CXEId] [bigint] NOT NULL,
	[CXNId] [bigint] NULL,
    [LedgerEntryPK] [uniqueidentifier] NULL,
	CustomerSessionPK [uniqueidentifier] NOT NULL,
	AccountPK [uniqueidentifier] NOT NULL,
	Amount [money] NULL,
	Fee [money] NULL,
	[Description] [nvarchar](255) NULL,
	CXEState [int] NULL,
	CXNState [int] NULL,
    [DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
CONSTRAINT [PK_tTxn_BillPay] PRIMARY KEY CLUSTERED 
(
    [txnRowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
 
GO
 
ALTER TABLE [dbo].[tTxn_BillPay]  WITH CHECK ADD CONSTRAINT [FK_tTxn_BillPay_tLedgerEntries] FOREIGN KEY([LedgerEntryPK])
REFERENCES [dbo].[tLedgerEntries] ([rowguid])
GO
 
ALTER TABLE [dbo].[tTxn_BillPay] CHECK CONSTRAINT [FK_tTxn_BillPay_tLedgerEntries]
GO

ALTER TABLE [dbo].[tTxn_BillPay]  WITH CHECK ADD CONSTRAINT [FK_tTxn_BillPay_tCustomerSessions] FOREIGN KEY([CustomerSessionPK])
REFERENCES [dbo].[tCustomerSessions] ([CustomerSessionRowguid])
GO
 
ALTER TABLE [dbo].[tTxn_BillPay] CHECK CONSTRAINT [FK_tTxn_BillPay_tCustomerSessions]
GO

ALTER TABLE [dbo].[tTxn_BillPay]  WITH CHECK ADD CONSTRAINT [FK_tTxn_BillPay_tAccounts] FOREIGN KEY([AccountPK])
REFERENCES [dbo].[tAccounts] ([rowguid])
GO
 
ALTER TABLE [dbo].[tTxn_BillPay] CHECK CONSTRAINT [FK_tTxn_BillPay_tAccounts]
GO
 
---------------

drop table tTxn_Funds

CREATE TABLE [dbo].[tTxn_Funds](
    [txnRowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL,
	[CXEId] [bigint] NOT NULL,
	[CXNId] [bigint] NULL,
    [LedgerEntryPK] [uniqueidentifier] NULL,
	CustomerSessionPK [uniqueidentifier] NOT NULL,
	AccountPK [uniqueidentifier] NOT NULL,
	Amount [money] NULL,
	Fee [money] NULL,
	[Description] [nvarchar](255) NULL,
	CXEState [int] NULL,
	CXNState [int] NULL,
    [DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
CONSTRAINT [PK_tTxn_Funds] PRIMARY KEY CLUSTERED 
(
    [txnRowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
 
GO
 
ALTER TABLE [dbo].[tTxn_Funds]  WITH CHECK ADD CONSTRAINT [FK_tTxn_Funds_tLedgerEntries] FOREIGN KEY([LedgerEntryPK])
REFERENCES [dbo].[tLedgerEntries] ([rowguid])
GO
 
ALTER TABLE [dbo].[tTxn_Funds] CHECK CONSTRAINT [FK_tTxn_Funds_tLedgerEntries]
GO

ALTER TABLE [dbo].[tTxn_Funds]  WITH CHECK ADD CONSTRAINT [FK_tTxn_Funds_tCustomerSessions] FOREIGN KEY([CustomerSessionPK])
REFERENCES [dbo].[tCustomerSessions] ([CustomerSessionRowguid])
GO
 
ALTER TABLE [dbo].[tTxn_Funds] CHECK CONSTRAINT [FK_tTxn_Funds_tCustomerSessions]
GO

ALTER TABLE [dbo].[tTxn_Funds]  WITH CHECK ADD CONSTRAINT [FK_tTxn_Funds_tAccounts] FOREIGN KEY([AccountPK])
REFERENCES [dbo].[tAccounts] ([rowguid])
GO
 
ALTER TABLE [dbo].[tTxn_Funds] CHECK CONSTRAINT [FK_tTxn_Funds_tAccounts]
GO
 
---------------

drop table tTxn_MoneyOrder

CREATE TABLE [dbo].[tTxn_MoneyOrder](
    [txnRowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL,
	[CXEId] [bigint] NOT NULL,
	[CXNId] [bigint] NULL,
    [LedgerEntryPK] [uniqueidentifier] NULL,
	CustomerSessionPK [uniqueidentifier] NOT NULL,
	AccountPK [uniqueidentifier] NOT NULL,
	Amount [money] NULL,
	Fee [money] NULL,
	[Description] [nvarchar](255) NULL,
	CXEState [int] NULL,
	CXNState [int] NULL,
    [DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
CONSTRAINT [PK_tTxn_MoneyOrder] PRIMARY KEY CLUSTERED 
(
    [txnRowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
 
GO
 
ALTER TABLE [dbo].[tTxn_MoneyOrder]  WITH CHECK ADD CONSTRAINT [FK_tTxn_MoneyOrder_tLedgerEntries] FOREIGN KEY([LedgerEntryPK])
REFERENCES [dbo].[tLedgerEntries] ([rowguid])
GO
 
ALTER TABLE [dbo].[tTxn_MoneyOrder] CHECK CONSTRAINT [FK_tTxn_MoneyOrder_tLedgerEntries]
GO

ALTER TABLE [dbo].[tTxn_MoneyOrder]  WITH CHECK ADD CONSTRAINT [FK_tTxn_MoneyOrder_tCustomerSessions] FOREIGN KEY([CustomerSessionPK])
REFERENCES [dbo].[tCustomerSessions] ([CustomerSessionRowguid])
GO
 
ALTER TABLE [dbo].[tTxn_MoneyOrder] CHECK CONSTRAINT [FK_tTxn_MoneyOrder_tCustomerSessions]
GO

ALTER TABLE [dbo].[tTxn_MoneyOrder]  WITH CHECK ADD CONSTRAINT [FK_tTxn_MoneyOrder_tAccounts] FOREIGN KEY([AccountPK])
REFERENCES [dbo].[tAccounts] ([rowguid])
GO
 
ALTER TABLE [dbo].[tTxn_MoneyOrder] CHECK CONSTRAINT [FK_tTxn_MoneyOrder_tAccounts]
GO
 
---------------

drop table tTxn_MoneyTransfer

CREATE TABLE [dbo].[tTxn_MoneyTransfer](
    [txnRowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL,
	[CXEId] [bigint] NOT NULL,
	[CXNId] [bigint] NULL,
    [LedgerEntryPK] [uniqueidentifier] NULL,
	CustomerSessionPK [uniqueidentifier] NOT NULL,
	AccountPK [uniqueidentifier] NOT NULL,
	Amount [money] NULL,
	Fee [money] NULL,
	[Description] [nvarchar](255) NULL,
	CXEState [int] NULL,
	CXNState [int] NULL,
    [DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
CONSTRAINT [PK_tTxn_MoneyTransfer] PRIMARY KEY CLUSTERED 
(
    [txnRowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
 
GO
 
ALTER TABLE [dbo].[tTxn_MoneyTransfer]  WITH CHECK ADD CONSTRAINT [FK_tTxn_MoneyTransfer_tLedgerEntries] FOREIGN KEY([LedgerEntryPK])
REFERENCES [dbo].[tLedgerEntries] ([rowguid])
GO
 
ALTER TABLE [dbo].[tTxn_MoneyTransfer] CHECK CONSTRAINT [FK_tTxn_MoneyTransfer_tLedgerEntries]
GO

ALTER TABLE [dbo].[tTxn_MoneyTransfer]  WITH CHECK ADD CONSTRAINT [FK_tTxn_MoneyTransfer_tCustomerSessions] FOREIGN KEY([CustomerSessionPK])
REFERENCES [dbo].[tCustomerSessions] ([CustomerSessionRowguid])
GO
 
ALTER TABLE [dbo].[tTxn_MoneyTransfer] CHECK CONSTRAINT [FK_tTxn_MoneyTransfer_tCustomerSessions]
GO

ALTER TABLE [dbo].[tTxn_MoneyTransfer]  WITH CHECK ADD CONSTRAINT [FK_tTxn_MoneyTransfer_tAccounts] FOREIGN KEY([AccountPK])
REFERENCES [dbo].[tAccounts] ([rowguid])
GO
 
ALTER TABLE [dbo].[tTxn_MoneyTransfer] CHECK CONSTRAINT [FK_tTxn_MoneyTransfer_tAccounts]
GO
 
---------------
