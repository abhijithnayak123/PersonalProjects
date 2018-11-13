-- ============================================================
-- Author:		<Swarnalakshmi>
-- Create date: <07/03/2014>
-- Description:	<Table for MoneyGram Receiver> 
-- Rally ID:	<NA>
-- ============================================================
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tMGram_Receiver_IsActive]') AND type = 'D')
BEGIN
	ALTER TABLE [dbo].[tMGram_Receiver] DROP CONSTRAINT [DF_tMGram_Receiver_IsActive]
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tMGram_Receiver]') AND type in (N'U'))
	DROP TABLE [dbo].[tMGram_Receiver]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tMGram_Receiver](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[FirstName] [varchar](100) NULL,
	[LastName] [varchar](100) NULL,
	[SecondLastName] [varchar](100) NULL,
	[IsActive] [bit] NOT NULL,
	[Country] [varchar](200) NULL,
	[Address] [varchar](250) NULL,
	[City] [varchar](200) NULL,
	[State] [varchar](200) NULL,
	[ZipCode] [varchar](10) NULL,
	[PhoneNumber] [varchar](20) NULL,
	[PickupCountry] [varchar](100) NULL,
	[PickupState] [varchar](100) NULL,
	[CustomerId] [bigint] NOT NULL,
	[MiddleName] [nvarchar](255) NULL,
	[NickName] [nvarchar](255) NULL,
	[IsReceiverHasPhotoId] [bit] NULL,
	[SecurityQuestion] [varchar](100) NULL,
	[SecurityAnswer] [varchar](100) NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
	[DTServerCreate] [datetime] NOT NULL,
	[DTServerLastMod] [datetime] NULL
PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[tMGram_Receiver] ADD  CONSTRAINT [DF_tMGram_Receiver_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
