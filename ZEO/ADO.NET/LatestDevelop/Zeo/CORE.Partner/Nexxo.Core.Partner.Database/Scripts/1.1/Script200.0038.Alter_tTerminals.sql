--===========================================================================================
-- Author:		<Bineesh Raghavan>
-- Create date: <05/23/2014>
-- Description:	<Made IPAddress column NULLable in tTerminals table>
-- Rally ID:	<DE2845>
--===========================================================================================
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tTerminals' AND COLUMN_NAME = 'IPAddress')
BEGIN
	ALTER TABLE tTerminals 
	ALTER COLUMN IPAddress VARCHAR(20) NULL
END
GO