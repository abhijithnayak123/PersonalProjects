CREATE TABLE [dbo].[tTransactionMinimums](
	[rowguid] [uniqueidentifier] NOT NULL,
	[id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[ComplianceProgramPK] [uniqueidentifier] NOT NULL,
	[TransactionType] [int] NOT NULL,
	[Minimum] [money] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tTransactionMinimums] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tTransactionMinimums]  WITH CHECK ADD  CONSTRAINT [FK_tTransactionMinimums_tCompliancePrograms] FOREIGN KEY([ComplianceProgramPK])
REFERENCES [dbo].[tCompliancePrograms] ([rowguid])
GO

ALTER TABLE [dbo].[tTransactionMinimums] CHECK CONSTRAINT [FK_tTransactionMinimums_tCompliancePrograms]
GO
