-- ============================================================
-- Author:		<Ratheesh PK>
-- Create date: <20/08/2014>
-- Description:	<Table to hold MoneyGram Billers imported from CSV file> 
-- Rally ID:	<NA>
-- ============================================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.tMGram_Catalog') AND type in (N'U'))
DROP TABLE dbo.tMGram_Catalog
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO	

CREATE TABLE [dbo].[tMGram_Catalog](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[ReceiveAgentId] varchar(8) NULL,
	[ReceiveCode] varchar(25) NOT NULL,
	[BillerName] varchar(100) NOT NULL,
	[Poe_Svc_Msg_EN_Text] varchar(100) NULL,
	[Poe_Svc_Msg_ES_Text] varchar(100) NULL,
	[Keywords] varchar(200) NULL,
	[IsActive] [bit] NOT NULL,
	[ChannelPartnerId] [int] NOT NULL,
	[DTCreate] [datetime] NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tMGram_Catalog] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


