--===========================================================================================
-- Author:		<RAJKUMAR M>
-- Created date: <12/15/2015>
-- Description:	<Script to add constraints to Column in NexxoIdTypeID>           
-- Jira ID:	<AL-3022>
--===========================================================================================

IF EXISTS (
		SELECT 1 
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tNexxoIdTypes' 
		AND COLUMN_NAME = 'NexxoIdTypeID'
		)
BEGIN
	ALTER TABLE dbo.[tNexxoIdTypes]
	ADD CONSTRAINT UX_NexxoIdTypeID UNIQUE (NexxoIdTypeID)
END
GO
