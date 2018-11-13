-- ============================================================
-- Author:		<Mohammed Khaja Firoz Molla>
-- Create date: <05/07/2015>
-- Description:	<DDL script to create tCertegy_CheckTypeMapping table>
-- Jira ID:	<AL-438>
-- ============================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tCertegy_CheckTypeMapping]') AND type in (N'U'))
DROP TABLE [dbo].[tCertegy_CheckTypeMapping]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tCertegy_CheckTypeMapping](
	[CertegyTypePK] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[Name] [char](1) NOT NULL,
	[CheckType] [int] NOT NULL,
 CONSTRAINT [PK_Certegy_CheckTypeMapping] PRIMARY KEY CLUSTERED 
(
	[CertegyTypePK] ASC
)WITH (FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO


