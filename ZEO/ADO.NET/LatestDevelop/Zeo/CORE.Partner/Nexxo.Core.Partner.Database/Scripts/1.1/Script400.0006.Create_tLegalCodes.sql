-- ============================================================
-- Author:		Abhijith
-- Create date: <10/20/2014>
-- Description:	<Added table for Legal Codes>
-- Rally ID:	<US2157>
-- ============================================================

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tLegalCodes]') AND type in (N'U'))
	DROP TABLE [dbo].[tLegalCodes]
GO

CREATE TABLE [dbo].[tLegalCodes](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[Code] [char](1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tLegalCodes] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
