ALTER TABLE tChannelPartners
DROP CONSTRAINT PK_tChannelPartners

ALTER TABLE tChannelPartners
ADD rowguid uniqueidentifier NOT NULL PRIMARY KEY
DEFAULT NEWID()


ALTER TABLE dbo.tProspects
DROP COLUMN ChannelPartnerId

ALTER TABLE dbo.tProspects
ADD ChannelPartnerId UNIQUEIDENTIFIER

ALTER TABLE tNexxoIdTypes
ADD CountryId UNIQUEIDENTIFIER NULL,
StateId UNIQUEIDENTIFIER NULL

GO

UPDATE tNexxoIdTypes
SET		countryid = b.id 
FROM	tNexxoIdTypes a JOIN tCountries b
		ON a.Country = b.Name 

UPDATE tNexxoIdTypes
SET		StateId = b.id 
FROM	tNexxoIdTypes a JOIN tStates b
		ON a.State = b.Name 

GO

AlTER TABLE tNexxoIdTypes
ADD FOREIGN KEY (StateId)
REFERENCES tStates(id)

ALTER TABLE dbo.tStates
DROP CONSTRAINT FK_tStates_tCountries

ALTER TABLE dbo.tStates
ADD CountryId uniqueidentifier

ALTER TABLE tCountries
DROP CONSTRAINT PK_tCountries

ALTER TABLE tCountries
ADD PRIMARY KEY (id)

ALTER TABLE dbo.tStates
ADD FOREIGN KEY(CountryId)
REFERENCES tCountries(id)

GO
update tStates 
set CountryId = c.id
from tStates s
join tCountries c on c.Code = s.CountryCode

GO

ALTER TABLE tNexxoIdTypes
ADD FOREIGN KEY (CountryId)
REFERENCES tCountries(id)