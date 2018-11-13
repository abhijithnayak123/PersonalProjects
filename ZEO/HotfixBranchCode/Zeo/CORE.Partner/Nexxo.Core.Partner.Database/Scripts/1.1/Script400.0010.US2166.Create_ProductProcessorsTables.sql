-- ============================================================
-- Author:		<Ashok Kumar G>
-- Create date: <10/30/2014>
-- Description:	<Script for Create tProducts, tProcessors, tProductProcessorsmapping, tChannelPartnerProductProcessorsMapping tables.>
-- Rally ID:	<US2166>
-- ============================================================


IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tChannelPartnerProductProcessorsMapping]') AND type in (N'U'))
DROP TABLE [dbo].[tChannelPartnerProductProcessorsMapping]
GO

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tProductProcessorsMapping]') AND type in (N'U'))
DROP TABLE [dbo].[tProductProcessorsMapping]
GO

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tProcessors]') AND type in (N'U'))
DROP TABLE [dbo].[tProcessors]
GO

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tProducts]') AND type in (N'U'))
DROP TABLE [dbo].[tProducts]
GO

CREATE TABLE tProducts
(
	rowguid UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	Id BIGINT NOT NULL IDENTITY(1,1),
	Name VARCHAR(1000) NOT NULL,
	DTCreate DATETIME NOT NULL,
	DTLastMod DATETIME NULL
)
GO

CREATE TABLE tProcessors
(
	rowguid UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	Id BIGINT NOT NULL IDENTITY(1,1),
	Name VARCHAR(1000) NOT NULL,
	DTCreate DATETIME NOT NULL,
	DTLastMod DATETIME NULL
)
GO

CREATE TABLE tProductProcessorsMapping
(
	rowguid UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	Id BIGINT NOT NULL IDENTITY(1,1),
	ProductId UNIQUEIDENTIFIER,
	ProcessorId UNIQUEIDENTIFIER,
	Code BIGINT,
	DTCreate DATETIME NOT NULL,
	DTLastMod DATETIME NULL,
	FOREIGN KEY (ProductId) REFERENCES tProducts(rowguid),
	FOREIGN KEY (ProcessorId) REFERENCES tProcessors(rowguid)
)
GO

CREATE TABLE tChannelPartnerProductProcessorsMapping
(
	rowguid UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	Id BIGINT NOT NULL IDENTITY(1,1),
	ChannelPartnerId UNIQUEIDENTIFIER,
	ProductProcessorId UNIQUEIDENTIFIER,
	Sequence INT NOT NULL,
	DTCreate DATETIME NOT NULL,
	DTLastMod DATETIME NULL,
	FOREIGN KEY (ChannelPartnerId) REFERENCES tChannelPartners(rowguid),
	FOREIGN KEY (ProductProcessorId) REFERENCES tProductProcessorsMapping(rowguid)
)
GO