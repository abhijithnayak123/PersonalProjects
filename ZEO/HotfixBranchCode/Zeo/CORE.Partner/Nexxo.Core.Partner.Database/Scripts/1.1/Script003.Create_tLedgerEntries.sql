
CREATE TABLE [dbo].[tLedgerEntries](
    [rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL IDENTITY (1000000000, 1),
    [LedgerTransactionRowguid] [uniqueidentifier] NOT NULL,
    [AccountPK] [uniqueidentifier] NOT NULL,
    [Amount] [money] NULL,
    [DTCreate] [datetime] NOT NULL
CONSTRAINT [PK_tLedgerEntries] PRIMARY KEY CLUSTERED 
(
    [rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
 
GO
 
ALTER TABLE [dbo].[tLedgerEntries]  WITH CHECK ADD CONSTRAINT [FK_tLedgerEntries_tLedgerTransactions] FOREIGN KEY([LedgerTransactionRowguid])
REFERENCES [dbo].[tLedgerTransactions] ([rowguid])
GO
 
ALTER TABLE [dbo].[tLedgerEntries] CHECK CONSTRAINT [FK_tLedgerEntries_tLedgerTransactions]
GO

ALTER TABLE [dbo].[tLedgerEntries]  WITH CHECK ADD CONSTRAINT [FK_tLedgerEntries_tAccounts] FOREIGN KEY([AccountPK])
REFERENCES [dbo].[tAccounts] ([rowguid])
GO
 
ALTER TABLE [dbo].[tLedgerEntries] CHECK CONSTRAINT [FK_tLedgerEntries_tAccounts]
GO