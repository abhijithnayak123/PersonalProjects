-- ============================================================
-- Author:		<Bineesh Raghavan>
-- Create date: <11/11/2014>
-- Description:	<script for creating tVisa_Credential table.>
-- Rally ID:	<US2154>
-- ============================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tVisa_Credential]') AND type in (N'U'))
DROP TABLE [dbo].[tVisa_Credential]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE tVisa_Credential
(
	rowguid UNIQUEIDENTIFIER NOT NULL,
	Id BIGINT IDENTITY(1000000000,1) NOT NULL,
	ServiceUrl VARCHAR(500) NOT NULL,
	CertificateName VARCHAR(255) NOT NULL,
	UserName VARCHAR(50) NOT NULL,
	[Password] VARCHAR(50) NOT NULL,
	ClientNodeId BIGINT NOT NULL,
	CardProgramNodeId BIGINT NOT NULL,
	SubClientNodeId BIGINT NOT NULL,
	StockId VARCHAR(50) NOT NULL,
	ChannelPartnerId BIGINT NOT NULL,
	DTCreate DATETIME NOT NULL,
	DTLastMod DATETIME NULL,
	 CONSTRAINT [PK_tVisa_Credential] PRIMARY KEY CLUSTERED 
	(
		[rowguid] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE tVisa_Credential
ADD CONSTRAINT IX_tVisa_Credential_ChannelPartnerId UNIQUE (ChannelPartnerId)
WITH (IGNORE_DUP_KEY = ON)
GO