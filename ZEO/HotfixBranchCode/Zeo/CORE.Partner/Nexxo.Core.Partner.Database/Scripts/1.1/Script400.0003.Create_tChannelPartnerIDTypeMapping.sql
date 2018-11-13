--===========================================================================================
-- Auther:			<Ashok Kumar>
-- Date Created:	<22-Oct-2014>
-- Description:		<Script for create tChannelPartnerIDTypeMapping table>
-- Rally ID:		<TA5684>
--===========================================================================================

IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tChannelPartnerIDTypeMapping]') AND type in (N'U'))
BEGIN
	DROP TABLE tChannelPartnerIDTypeMapping
END
GO
CREATE TABLE tChannelPartnerIDTypeMapping
(
	rowguid UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	ChannelPartnerId UNIQUEIDENTIFIER NOT NULL,
	NexxoIdTypeId UNIQUEIDENTIFIER NOT NULL,
	IsActive BIT
)
GO