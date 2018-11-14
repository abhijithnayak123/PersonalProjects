

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tAgentDetails_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tAgentDetails] DROP CONSTRAINT [DF_tAgentDetails_rowguid]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tAgentDetails_DTCreate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tAgentDetails] DROP CONSTRAINT [DF_tAgentDetails_DTCreate]
END

GO


/****** Object:  Table [dbo].[tAgentDetails]    Script Date: 04/26/2013 15:04:58 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tAgentDetails]') AND type in (N'U'))
DROP TABLE [dbo].[tAgentDetails]
GO



CREATE TABLE [dbo].[tAgentDetails](
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](20) NULL,
	[FirstName] [nvarchar](20) NULL,
	[LastName] [nvarchar](20) NULL,
	[FullName] [nvarchar](50) NULL,
	[IsEnabled] [bit] NOT NULL,
	[ManagerId] [int] NULL,
	[PrimaryLocationId] [bigint] NOT NULL,
	[UserRoleId] [int] NOT NULL,
	[UserStatusId] [int] NOT NULL,
	[ChannelPartnerId] [int] NULL,
	[PhoneNumber] [nvarchar](20) NULL,
	[Email] [nvarchar](50) NULL,
	[Notes] [nvarchar](500) NULL,
	[DTLastLogin] [datetime] NULL,
	[DTCreate] [datetime] NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tAgentDetails] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tAgentDetails] ADD  CONSTRAINT [DF_tAgentDetails_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO

ALTER TABLE [dbo].[tAgentDetails] ADD  CONSTRAINT [DF_tAgentDetails_DTCreate]  DEFAULT (getdate()) FOR [DTCreate]
GO


