--==========================================================================
-- Author: <Ashok Kumar G>
-- Date Created: <March 30 2015>
-- Description: <As a TCF user,
--				 I want to see updated Country names, 
--				 so that I can register a new customer>
-- User Story ID: <AL-108>
--===========================================================================

-- Remove foriegn key references
WHILE EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE='FOREIGN KEY' and TABLE_NAME= 'tNexxoIdTypes')
BEGIN
	DECLARE @Constarint NVARCHAR(2000)
	SELECT TOP 1 @Constarint =('ALTER TABLE ' + TABLE_SCHEMA + '.[' + TABLE_NAME
	+ '] DROP CONSTRAINT [' + CONSTRAINT_NAME + ']')
	FROM information_schema.table_constraints
	WHERE CONSTRAINT_TYPE = 'FOREIGN KEY' and TABLE_NAME= 'tNexxoIdTypes'
	EXEC (@Constarint)
END

GO
-- Update CountryId value to tMasterCountryId reference
UPDATE tNexxoIdTypes 
SET countryId = t.rowguid
FROM 
(SELECT mc.rowguid,mc.Name FROM tNexxoIdTypes nt
INNER JOIN tMasterCOuntries mc ON nt.country = mc.name) t
WHERE tNexxoIdTypes.Country = t.Name

GO
-- Add foriegn key references to tMasterCountries and tStates
ALTER TABLE tNexxoIdTypes WITH CHECK ADD Constraint FK_tNexxoIdTypes_tMasterCountries  FOREIGN KEY([CountryId])
REFERENCES tMasterCountries([rowguid])
GO

ALTER TABLE tNexxoIdTypes WITH CHECK ADD Constraint FK_tNexxoIdTypes_tStates FOREIGN KEY([StateId])
REFERENCES tStates ([id])
GO

-- Insert id type passport for new countries.
DECLARE @Id INT
SET @Id = 224
IF NOT EXISTS(SELECT Id FROM tNexxoIdTypes WHERE Id > @Id)
BEGIN
	INSERT INTO tNexxoIdTypes (rowguid, Id, Name, Mask, HasExpirationDate, Country, CountryId, IsActive) (
	SELECT NEWID(), @Id + ROW_NUMBER() OVER (ORDER BY mc.Name), 'PASSPORT', '^\w{4,15}$', 1, mc.Name, mc.rowguid, 1 FROM tMasterCountries mc 
	INNER JOIN tMasterCountries mcd ON mcd.Name = mc.Name
	WHERE mc.Name not in ('MEXICO', 'UNITED STATES'))
END

GO
-- Enable id type passport for new countries(for synovus we are not showing new countries).
INSERT INTO tChannelPartnerIDTypeMapping
(
	ChannelPartnerId, 
	NexxoIdTypeId,
	IsActive 
)
SELECT 
	tc.rowguid, 
	tn.rowguid, 
	tn.IsActive 
FROM tChannelPartners tc
	CROSS JOIN tNexxoIdTypes tn
	where tn.Id > 224 AND tc.Name <> 'Synovus'
GO