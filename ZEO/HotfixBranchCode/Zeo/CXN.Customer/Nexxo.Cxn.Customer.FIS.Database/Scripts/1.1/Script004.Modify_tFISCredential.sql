IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tFIS_Credential]') AND type in (N'U'))
DROP TABLE [dbo].[tFIS_Credential]
GO

CREATE TABLE [dbo].[tFIS_Credential]
(
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL,
	[UserName] [nvarchar](50) NULL,
	[Password] [nvarchar](50) NULL,
	[Applicationkey] [uniqueidentifier] NULL,
	[ChannelKey] [int] NULL,
	[ChannelPartnerId] [bigint] NULL,
	[BankId] [varchar](5) NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL
) ON [PRIMARY]
GO

INSERT [tFIS_Credential] 
(
[rowguid], 
[Id], 
[UserName], 
[Password], 
[Applicationkey], 
[ChannelKey], 
[ChannelPartnerId], 
[BankId], 
[DTCreate], 
[DTLastMod]
) VALUES 
(
N'127d5d0e-aded-4d47-8c9c-972c89d71f6a', 1, 
N'nexxouser', N'$yn1nex', 
N'a7da277e-3c52-431f-b53a-d6db1b4f0681', 5, 33, N'300', GETDATE(), NULL
)