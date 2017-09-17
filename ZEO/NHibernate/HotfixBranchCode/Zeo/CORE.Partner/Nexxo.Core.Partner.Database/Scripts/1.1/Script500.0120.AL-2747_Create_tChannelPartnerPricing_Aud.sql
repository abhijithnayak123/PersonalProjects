--- ================================================================================
-- Author:		<Abhijith>
-- Create date: <11/04/2015>
-- Description:	<As Alloy, I want to have an audit log for pricing cluster changes>
-- Jira ID:		<AL-2747>
-- ================================================================================

IF NOT EXISTS
(	
	SELECT 1 
	FROM dbo.sysobjects
	WHERE name = N'tChannelPartnerPricing_Aud'
)
BEGIN

CREATE TABLE [dbo].[tChannelPartnerPricing_Aud](
    [ChannelPartnerPricingPK] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL,
	[PricingGroupPK] [uniqueidentifier] NOT NULL,
	[ChannelPartnerPK] [uniqueidentifier] NOT NULL,
	[LocationPK] [uniqueidentifier] NULL,
	[ProductPK] [uniqueidentifier] NOT NULL,
	[ProductType] [int] NULL,
	[RevisionNo] [bigint] NOT NULL,
	[AuditEvent] [smallint] NOT NULL,
	[DTAudit] datetime NOT NULL,
    [DTServerCreate] [datetime] NOT NULL,
	[DTServerLastModified] [datetime] NULL,
	[DTTerminalCreate] [datetime] NOT NULL,
	[DTTerminalLastModified] [datetime] NULL
)

END
GO
