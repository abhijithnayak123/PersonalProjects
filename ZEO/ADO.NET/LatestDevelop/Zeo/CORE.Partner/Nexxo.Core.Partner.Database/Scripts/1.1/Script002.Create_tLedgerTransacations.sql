
CREATE TABLE [dbo].[tLedgerTransactions](
    [rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL IDENTITY (1000000000, 1),
    [DTCreate] [datetime] NOT NULL,
CONSTRAINT [PK_tLedgerTransactions] PRIMARY KEY CLUSTERED 
(
    [rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
 
GO
ALTER TABLE [dbo].[tLedgerTransactions] ADD  CONSTRAINT [DF_tLedgerTransactions_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
