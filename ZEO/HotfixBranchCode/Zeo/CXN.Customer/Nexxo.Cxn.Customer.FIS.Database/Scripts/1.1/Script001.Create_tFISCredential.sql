CREATE TABLE [dbo].[tFIS_Credential](
      [rowguid] [uniqueidentifier] NOT NULL,
      [Id] [bigint] NOT NULL,      
      [User] [nvarchar](50) NULL,
      [Password] [nvarchar](50) NULL,
      [Applky] [uniqueidentifier] NULL,      
      [ChannelKy] [int] NULL,
      [ChannelPartnerId] [bigint] NULL,
      [DTCreate] [datetime] NULL,
      [DTLastMod] [datetime] NULL
) ON [PRIMARY]
