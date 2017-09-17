

CREATE TABLE [dbo].[tTxn_MoneyTransfer](
    [rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL,
	[CXEId] [bigint] NOT NULL,
	[CXNId] [bigint] NULL,
    [LedgerEntryPK] [uniqueidentifier] NULL,
    [DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
CONSTRAINT [PK_tTxn_MoneyTransfer] PRIMARY KEY CLUSTERED 
(
    [rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
 
GO
 
ALTER TABLE [dbo].[tTxn_MoneyTransfer]  WITH CHECK ADD CONSTRAINT [FK_tTxn_MoneyTransfer_tLedgerEntries] FOREIGN KEY([LedgerEntryPK])
REFERENCES [dbo].[tLedgerEntries] ([rowguid])
GO
 
ALTER TABLE [dbo].[tTxn_MoneyTransfer] CHECK CONSTRAINT [FK_tTxn_MoneyTransfer_tLedgerEntries]
GO
