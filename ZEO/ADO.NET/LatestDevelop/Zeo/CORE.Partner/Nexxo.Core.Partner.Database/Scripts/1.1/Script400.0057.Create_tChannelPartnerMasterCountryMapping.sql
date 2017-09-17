--===========================================================================================
-- Author:		<Rita Patel>
-- Create date: <Mar 13 2015>
-- Description:	<Script to create ChannelPartnerMasterCountryMapping table>
-- Jira ID:	    <AL-139>
--===========================================================================================

IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tChannelPartnerMasterCountryMapping]') AND type in (N'U'))
DROP TABLE [dbo].[tChannelPartnerMasterCountryMapping]
GO

CREATE TABLE [dbo].[tChannelPartnerMasterCountryMapping](
	[rowguid] [uniqueidentifier] NOT NULL PRIMARY KEY DEFAULT NEWID(),
	[ChannelPartnerId] [uniqueidentifier] NOT NULL,
	[MasterCountryId] [uniqueidentifier] NOT NULL,
	[IsActive] [bit] NULL DEFAULT 1,
	CONSTRAINT [FK_ChannelPartner_tChannelPartnerMasterCountryMapping] FOREIGN KEY([ChannelPartnerId])
     REFERENCES [dbo].[tChannelPartners] (rowguid),
    CONSTRAINT [FK_MasterCountry_tChannelPartnerMasterCountryMapping] FOREIGN KEY([MasterCountryId])
    REFERENCES [dbo].[tMasterCountries] (rowguid)
	)
GO
