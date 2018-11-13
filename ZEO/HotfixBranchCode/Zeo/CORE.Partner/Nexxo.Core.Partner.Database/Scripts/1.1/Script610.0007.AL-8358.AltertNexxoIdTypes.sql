-- ================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <17/10/2017>
-- Description:	<Production Issue: NexxoIdTypes mapping issue. tStates referring to
--				 invalid table tCountries instead of tCountries tMasterCountries table>
-- Jira ID:		<AL-8358>
-- ================================================================================

IF EXISTS (
	SELECT 1
	FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
	where TABLE_NAME = 'tStates' AND 
	CONSTRAINT_TYPE = 'FOREIGN KEY'
)

BEGIN
	DECLARE @CONSTRAINT VARCHAR(50)

	SELECT  @CONSTRAINT = CONSTRAINT_NAME
	FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
	where TABLE_NAME = 'tStates' AND 
	CONSTRAINT_TYPE = 'FOREIGN KEY'

	EXEC ('ALTER TABLE tStates 
	DROP CONSTRAINT ' + @CONSTRAINT)
END
GO


IF NOT EXISTS (
	SELECT *
	FROM INFORMATION_SCHEMA.COLUMNS
	where TABLE_NAME = 'tStates' AND 
	COLUMN_NAME = 'MasterCountriesPK'
)
BEGIN
	ALTER TABLE tStates 
	ADD MasterCountriesPk UNIQUEIDENTIFIER NULL
	CONSTRAINT FK_tStates_tMasterCountries FOREIGN KEY  (MasterCountriesPk)
	REFERENCES tMasterCountries (MasterCountriesPK)
END
GO

---Migrating the data where country names are equal
UPDATE tStates
SET MasterCountriesPk = m.MasterCountriesPK
FROM tCountries C 
INNER JOIN tMasterCountries M ON M.Name = C.Name
INNER JOIN tStates S ON C.CountryPK = S.CountryPK


IF EXISTS(
	SELECT 1
	FROM INFORMATION_SCHEMA.COLUMNS
	where TABLE_NAME = 'tStates' AND 
	COLUMN_NAME = 'CountryPK'
)
BEGIN
	ALTER TABLE tstates
	DROP COLUMN CountryPK
END
GO

IF EXISTS(
	SELECT 1
	FROM INFORMATION_SCHEMA.COLUMNS
	where TABLE_NAME = 'tStates' AND 
	COLUMN_NAME = 'MasterCountriesPK'
)
BEGIN
	EXEC sp_RENAME 'tStates.MasterCountriesPK' , 'CountryPK', 'COLUMN'
END
GO


ALTER TABLE tStates
ALTER COLUMN CountryCode CHAR(2)


UPDATE tStates
SET CountryCode = m.Abbr2
FROM tMasterCountries M 
INNER JOIN tStates S ON S.CountryPK = M.MasterCountriesPK

UPDATE 
	tMasterCountries
SET 
	NAME = 'CONGO REPUBLIC OF',
	Abbr2 = 'CF'
WHERE 
	NAME = 'CONGO-REPUBLIC OF'

UPDATE
	tMasterCountries
SET 
	NAME = 'CONGO DEMOCRATIC REPUB',
	Abbr2 = 'CG'
WHERE 
	NAME = 'CONGO-DEMOCRATIC REPUBLIC OF'


IF EXISTS(
	SELECT 1
	FROM INFORMATION_SCHEMA.COLUMNS
	where TABLE_NAME = 'tNexxoIdTypes' AND 
	COLUMN_NAME = 'Country'
)
BEGIN
	ALTER TABLE tNexxoIdTypes
	DROP COLUMN Country
END
GO

IF EXISTS(
	SELECT 1
	FROM INFORMATION_SCHEMA.COLUMNS
	where TABLE_NAME = 'tNexxoIdTypes' AND 
	COLUMN_NAME = 'State'
)
BEGIN
	ALTER TABLE tNexxoIdTypes
	DROP COLUMN State
END
GO