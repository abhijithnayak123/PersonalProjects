CREATE TABLE [dbo].[tChxrSim_Invoice](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [int] IDENTITY(100000,1) NOT NULL,
	[TicketId] [int] NOT NULL,
	[Amount] [money] NOT NULL,
	[Fee] [money] NULL,
	[CheckType] [int] NULL,
	[Status] [nvarchar](50) NOT NULL,
	[WaitTime] [nvarchar](100) NULL,
	[DeclineId] [int] NULL,
	[DeclineReason] [nvarchar](300) NULL,
	[ChxrSimAccountPK] [uniqueidentifier] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tChxrSim_Invoice] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tChxrSim_Invoice]  WITH CHECK ADD  CONSTRAINT [FK_tChxrSim_Invoice_tChxrSim_Account] FOREIGN KEY([ChxrSimAccountPK])
REFERENCES [dbo].[tChxrSim_Account] ([rowguid])
GO

ALTER TABLE [dbo].[tChxrSim_Invoice] CHECK CONSTRAINT [FK_tChxrSim_Invoice_tChxrSim_Account]
