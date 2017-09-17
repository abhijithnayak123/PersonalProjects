CREATE TABLE [dbo].[tChxr_Session](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Location] [nvarchar](50) NOT NULL,
	[CompanyToken] [nvarchar](50) NOT NULL,
	[BranchId] [int] NOT NULL,
	[EmployeeId] [int] NOT NULL,
	[ChxrPartnerPK] [uniqueidentifier] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tChxr_Session] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tChxr_Session]  WITH CHECK ADD  CONSTRAINT [FK_tChxr_Session_tChxr_Partner] FOREIGN KEY([ChxrPartnerPK])
REFERENCES [dbo].[tChxr_Partner] ([rowguid])
GO

ALTER TABLE [dbo].[tChxr_Session] CHECK CONSTRAINT [FK_tChxr_Session_tChxr_Partner]
GO

/****** Object:  Index [IX_tChxr_Session_Location]    Script Date: 05/07/2013 14:06:34 ******/
CREATE NONCLUSTERED INDEX [IX_tChxr_Session_Location] ON [dbo].[tChxr_Session] 
(
	[Location] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


