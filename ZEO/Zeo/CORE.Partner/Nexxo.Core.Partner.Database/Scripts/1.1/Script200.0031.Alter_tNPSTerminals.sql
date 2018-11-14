--===========================================================================================
-- Author:		SwarnaLakshmi S
-- Create date: <12/05/2014>
-- Description:	<Script for Adding ChannelPArtnerPK,Constraints for Location,Unique>
-- Rally ID:	<US1856>
--===========================================================================================


-- Modifying Allow Null for LocationPK in tNPSTerminals
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tNPSTerminals' AND COLUMN_NAME = 'LocationPK')
BEGIN
	ALTER TABLE tNPSTerminals ALTER COLUMN LocationPK UniqueIdentifier NULL
END
GO

-- Add New Column ChannelPartnerPK
IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tNPSTerminals' AND COLUMN_NAME = 'ChannelPartnerPK')
BEGIN
    ALTER TABLE [dbo].tNPSTerminals ADD ChannelPartnerPK UniqueIdentifier NULL
	CONSTRAINT [FK_tNpsTerminals_tChannelPartners] FOREIGN KEY(ChannelPartnerPK) REFERENCES tChannelPartners(rowguid); 
END
GO

-- Populate tNPSTerminal Table with ChannelPartnerPK values
BEGIN
	UPDATE NPS Set NPS.ChannelPartnerPK = CP.rowguid
	FROM tNpsTerminals NPS join tLocations L on L.rowguid = NPS.LocationPK 
	join tChannelPartners CP  on CP.Id = L.ChannelPartnerId 
END
GO


-- Modify Column ChannelPartnerPK Not Null
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tNPSTerminals' AND COLUMN_NAME = 'ChannelPartnerPK')
BEGIN
    ALTER TABLE [dbo].tNPSTerminals ALTER COLUMN ChannelPartnerPK UniqueIdentifier NOT NULL   
END
GO


------------ For NPS Service Changes

--- Dropping Unique Constraint for IPAddress in tTerminals
IF (OBJECT_ID('IX_tNpsTerminals_IPAddress', 'UQ') IS NOT NULL) 
BEGIN
    ALTER TABLE tNPSTerminals DROP CONSTRAINT [IX_tNpsTerminals_IPAddress]       
END
GO
-- Adding Unique Cosntraint for Channelpartner and Name in tNpsTerminals
--IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
--    WHERE TABLE_NAME = 'tNPSTerminals' AND COLUMN_NAME = 'ChannelPartnerPK')
--BEGIN
--	ALTER TABLE tNPSTerminals ADD CONSTRAINT IX_tNpsTerminals_Name UNIQUE (Name,ChannelPartnerPK)
--END
--GO

-- Modifying Allow Null for port in tNPSTerminals
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tNPSTerminals' AND COLUMN_NAME = 'Port')
BEGIN
	ALTER TABLE tNPSTerminals ALTER COLUMN Port varchar(10) NULL
	ALTER TABLE tNPSTerminals ALTER COLUMN [Status] varchar(50) NULL
	ALTER TABLE tNPSTerminals ALTER COLUMN IPAddress varchar(20) NULL
END
GO



