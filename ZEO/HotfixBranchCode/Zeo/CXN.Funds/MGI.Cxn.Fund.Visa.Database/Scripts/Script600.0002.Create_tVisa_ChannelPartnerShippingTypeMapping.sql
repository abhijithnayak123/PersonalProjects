-- ============================================================
-- Author:		<Sunil Shetty>
-- Create date: <11/23/2015>
-- Description:	<DDL script to create tVisa_ChannelPartnerShippingTypeMapping table>
-- Rally ID:	<AL-1641>
-- ============================================================
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tVisa_ChannelPartnerShippingTypeMapping]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tVisa_ChannelPartnerShippingTypeMapping](
	[ChannelPartnerShippingTypePK] [uniqueidentifier] NOT NULL,
	[ShippingTypePK] [uniqueidentifier] NOT NULL,
	[ChannelPartnerId] [bigint] NOT NULL,
 CONSTRAINT [PK_tVisa_ChannelPartnerShippingTypeMapping] PRIMARY KEY CLUSTERED 
(
	[ChannelPartnerShippingTypePK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (Select * from sys.foreign_keys where Name = 'FK_tVisa_ChannelPartnerShippingTypeMapping_tVisa_CardShippingTypes' AND type in (N'F'))
BEGIN
ALTER TABLE [dbo].[tVisa_ChannelPartnerShippingTypeMapping]  WITH CHECK ADD  CONSTRAINT [FK_tVisa_ChannelPartnerShippingTypeMapping_tVisa_CardShippingTypes] FOREIGN KEY(ShippingTypePK)
REFERENCES [dbo].[tVisa_CardShippingTypes] ([ShippingTypePK])

ALTER TABLE [dbo].[tVisa_ChannelPartnerShippingTypeMapping] CHECK CONSTRAINT [FK_tVisa_ChannelPartnerShippingTypeMapping_tVisa_CardShippingTypes]
END
GO
