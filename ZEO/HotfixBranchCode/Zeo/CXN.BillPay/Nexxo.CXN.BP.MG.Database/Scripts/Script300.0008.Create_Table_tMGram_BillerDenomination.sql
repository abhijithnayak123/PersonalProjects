-- ============================================================
-- Author:		<Rahul K>
-- Create date: <09/09/2014>
-- Description:	<BillerDenomination table creation> 
-- Rally ID:	<NA>
-- ============================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tMGram_BillerDenomination]') AND type in (N'U'))
DROP TABLE [dbo].[tMGram_BillerDenomination]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tMGram_BillerDenomination](
	[rowguid] [uniqueidentifier] NOT NULL,
	[BillerLimitPK] [uniqueidentifier] NOT NULL,
	[DenominationAmount] decimal(18,2) NULL,
 CONSTRAINT [PK_tMGram_BillerDenomination] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[tMGram_BillerDenomination]  WITH CHECK ADD  CONSTRAINT [FK_tMGram_BillerDenomination_tMGram_BillerLimit] FOREIGN KEY([BillerLimitPK])
REFERENCES [dbo].[tMGram_BillerLimit] ([rowguid])
GO

ALTER TABLE [dbo].[tMGram_BillerDenomination] CHECK CONSTRAINT [FK_tMGram_BillerDenomination_tMGram_BillerLimit]
GO


