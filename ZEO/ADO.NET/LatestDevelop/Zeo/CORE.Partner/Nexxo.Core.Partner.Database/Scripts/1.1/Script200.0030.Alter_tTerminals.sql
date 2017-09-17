--===========================================================================================
-- Author:		SwarnaLakshmi S
-- Create date: <12/05/2014>
-- Description:	<Script for Adding ChannelPArtnerPK,Constraints for Location,Unique>
-- Rally ID:	<US1856>
--===========================================================================================


-- Modifying Allow Null for LocationPK in tTerminals
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tTerminals' AND COLUMN_NAME = 'LocationPK')
BEGIN
	ALTER TABLE tTerminals ALTER COLUMN LocationPK UniqueIdentifier NULL
END
GO

--- Dropping Unique Constraint for TerminalName in tTerminals
IF (OBJECT_ID('IX_tTerminals_Name', 'UQ') IS NOT NULL) 
BEGIN
    ALTER TABLE tTerminals DROP CONSTRAINT IX_tTerminals_Name
	ALTER TABLE tTerminals ADD CONSTRAINT IX_tTerminals_NameChannelPartner UNIQUE (Name,ChannelPartnerPK)
END
GO
