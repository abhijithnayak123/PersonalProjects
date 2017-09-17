--===========================================================================================
-- Author:		<SwarnaLakshmi>
-- Create date: <10/27/2014>
-- Description:	<Synovus Location Counter Id Details>
-- Rally ID:	<US2028>
--===========================================================================================

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tLocationCounterIdDetails_LocationId]') AND parent_object_id = OBJECT_ID(N'[dbo].[tLocationCounterIdDetails]'))
ALTER TABLE [dbo].[tLocationCounterIdDetails] DROP CONSTRAINT [FK_tLocationCounterIdDetails_LocationId]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tLocationCounterIdDetails_IsAvailable]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tLocationCounterIdDetails] DROP CONSTRAINT [DF_tLocationCounterIdDetails_IsAvailable]
END

GO



/****** Object:  Table [dbo].[tLocationCounterIdDetails]    Script Date: 11/12/2014 02:27:26 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tLocationCounterIdDetails]') AND type in (N'U'))
DROP TABLE [dbo].[tLocationCounterIdDetails]
GO



CREATE TABLE [dbo].[tLocationCounterIdDetails](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[LocationId] [uniqueidentifier] NOT NULL,
	[ProviderId] [int] NOT NULL,
	[CounterId] [varchar](50) NOT NULL,
	[IsAvailable] [bit] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tLocationCounterIdDetails] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


ALTER TABLE [dbo].[tLocationCounterIdDetails]  WITH CHECK ADD  CONSTRAINT [FK_tLocationCounterIdDetails_LocationId] FOREIGN KEY([LocationId])
REFERENCES [dbo].[tLocations] ([rowguid])
GO

ALTER TABLE [dbo].[tLocationCounterIdDetails] CHECK CONSTRAINT [FK_tLocationCounterIdDetails_LocationId]
GO

ALTER TABLE [dbo].[tLocationCounterIdDetails] ADD  CONSTRAINT [DF_tLocationCounterIdDetails_IsAvailable]  DEFAULT ((1)) FOR [IsAvailable]
GO


