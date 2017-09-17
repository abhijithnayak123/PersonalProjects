--===========================================================================================
-- Author:		<SwarnaLakshmi>
-- Create date: <Dec 10 2014>
-- Description:	<Adding NoOfCounterIDs field to tLocations>
-- Rally ID:	<US2028>
--===========================================================================================

IF NOT EXISTS(SELECT 1 FROM SYS.COLUMNS WHERE NAME = N'NoOfCounterIDs' AND OBJECT_ID = OBJECT_ID(N'tLocations'))
BEGIN
	ALTER TABLE tLocations
	Add NoOfCounterIDs int
END
GO