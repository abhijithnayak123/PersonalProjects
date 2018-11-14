--- ===============================================================================
-- Author     :	 Abhijith
-- Description:  Updating the Fraud Limit for the countries.
-- Creatd Date:  04-05-2018
-- Story Id   :  B-13688
-- ================================================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_CountryCurrencies' AND COLUMN_NAME = 'FraudLimit')
BEGIN
	ALTER TABLE tWUnion_CountryCurrencies
	ADD FraudLimit MONEY NULL
END
GO
