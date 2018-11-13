-- ============================================================
-- Author:		<Bineesh Raghavan>
-- Create date: <06/25/2014>
-- Description:	<DDL script to create tVisa_ShippingFee table>
-- Rally ID:	<AL-327>
-- ============================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tVisa_ShippingFee]') AND type in (N'U'))
DROP TABLE [dbo].[tVisa_ShippingFee]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tVisa_ShippingFee]
(
	[VisaShippingFeePK] [uniqueidentifier] NOT NULL,
	[VisaShippingFeeId] [bigint] IDENTITY(1000,1) NOT NULL,
	[ShippingType] INT NOT NULL ,
	[Fee] MONEY  NOT NULL,
	[DTServerCreate] [datetime] NOT NULL,
	[DTServerLastMod] [datetime] NULL,
	CONSTRAINT IX_tVisa_ShippingFee_ShippingType UNIQUE (ShippingType)
)
GO
