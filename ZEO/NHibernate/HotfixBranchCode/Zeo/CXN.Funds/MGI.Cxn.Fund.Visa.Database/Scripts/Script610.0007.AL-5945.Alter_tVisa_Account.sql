-- Author:		<Kaushik Sakala>
-- Create date: <18/03/2016>
-- Description:	<As Synovus, the Visa location identifier should be passed in the API for card activation, 
--				 load and unload actions.>
-- Jira ID:		<AL-5945>
-- ================================================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_Account' AND COLUMN_NAME = 'ActivatedLocationNodeId')
BEGIN
ALTER TABLE tVisa_Account ADD ActivatedLocationNodeId [BIGINT] 

END
GO