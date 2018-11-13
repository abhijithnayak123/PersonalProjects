CREATE TABLE [dbo].[tChxr_Identity](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[Location] [nvarchar](50) NOT NULL,
	[BranchUsername] [nvarchar](50) NOT NULL,
	[BranchPassword] [nvarchar](50) NOT NULL,
	[EmpUsername] [nvarchar](50) NOT NULL,
	[EmpPassword] [nvarchar](50) NOT NULL,
	[ChxrPartnerPK] [uniqueidentifier] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tChxr_Identity] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tChxr_Identity]  WITH CHECK ADD  CONSTRAINT [FK_tChxr_Identity_tChxr_Partner] FOREIGN KEY([ChxrPartnerPK])
REFERENCES [dbo].[tChxr_Partner] ([rowguid])
GO

ALTER TABLE [dbo].[tChxr_Identity] CHECK CONSTRAINT [FK_tChxr_Identity_tChxr_Partner]
GO

/****** Object:  Index [IX_tChxr_Identity_Location]    Script Date: 05/07/2013 14:09:27 ******/
CREATE NONCLUSTERED INDEX [IX_tChxr_Identity_Location] ON [dbo].[tChxr_Identity] 
(
	[Location] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

