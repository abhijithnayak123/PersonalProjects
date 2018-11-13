/****** Object:  Table [dbo].[tTransaction]    Script Date: 08/14/2013 20:21:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tTransaction](
	[DTServer] [datetime] NOT NULL,
	[ClientId] [smallint] NULL,
	[LocationName] [nvarchar](100) NULL,
	[SessionId] [bigint] NULL,
	[TellerUserName] [nvarchar](20) NULL,
	[TellerId] [nvarchar](50) NULL,
	[TransactionId] [bigint] NULL,
	[Channel] [nvarchar](50) NULL,
	[ProviderId] [nvarchar](20) NULL,
	[ProviderTxnId] [varchar](20) NULL,
	[NexxoPan] [bigint] NULL,
	[TransactionType] [varchar](20) NULL,
	[TransactionAmount] [money] NULL,
	[CustomerFees] [money] NULL,
	[NetTransactionAmount] [money] NULL,
	[FinalStatus] [int] NULL,
	[ProcessorMessage] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


