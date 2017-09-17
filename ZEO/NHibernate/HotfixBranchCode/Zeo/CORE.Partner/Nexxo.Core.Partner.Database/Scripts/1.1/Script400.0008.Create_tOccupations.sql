-- ============================================================
-- Author:		Abhijith
-- Create date: <10/20/2014>
-- Description:	<Added table for Occupations>
-- Rally ID:	<US2157>
-- ============================================================

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tOccupations]') AND type in (N'U'))
DROP TABLE [dbo].[tOccupations]
GO

CREATE TABLE [dbo].[tOccupations](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[Code] [varchar](5) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[IsActive] bit NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tOccupation] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


ALTER TABLE [dbo].[tOccupations] ADD  CONSTRAINT [DF_tOccupations_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO