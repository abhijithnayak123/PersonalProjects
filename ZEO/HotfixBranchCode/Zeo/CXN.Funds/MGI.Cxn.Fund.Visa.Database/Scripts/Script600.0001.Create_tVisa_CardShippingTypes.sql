/****** Object:  Table [dbo].[tVisa_CardShippingTypes]    Script Date: 11/23/2015 5:19:29 PM ******/
-- ============================================================
-- Author:		<Sunil Shetty>
-- Create date: <11/23/2015>
-- Description:	<DDL script to create tVisa_CardShippingTypes table>
-- Rally ID:	<AL-1641>
-- ============================================================

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tVisa_CardShippingTypes]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tVisa_CardShippingTypes](
	[ShippingTypePK] [uniqueidentifier] NOT NULL,
	[CardShippingTypeId] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[Code] [nvarchar](5) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[DTServerCreate] [datetime] NOT NULL,
	[DTServerLastModified] [datetime] NULL,
 CONSTRAINT [PK_tCardShippingType] PRIMARY KEY CLUSTERED 
(
	[ShippingTypePK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO






