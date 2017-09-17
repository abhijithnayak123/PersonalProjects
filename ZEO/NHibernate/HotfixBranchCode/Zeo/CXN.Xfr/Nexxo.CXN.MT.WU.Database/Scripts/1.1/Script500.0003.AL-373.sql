--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <May 5th 2015>
-- Description:	<Script to update column names>           
-- Jira ID:	<AL-373>
--===========================================================================================

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_CountryCurrencies'
			AND COLUMN_NAME = 'WUCurrencyPK'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_CountryCurrencies.WUCurrencyPK'
		,@newname = 'WUCountryCurrencyPK'
		,@objtype = 'COLUMN';
END
GO
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_CountryCurrencies'
			AND COLUMN_NAME = 'WUCurrencyID'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_CountryCurrencies.WUCurrencyID'
		,@newname = 'WUCountryCurrencyID'
		,@objtype = 'COLUMN';
END
GO