--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <08-3-2016>
-- Description:	 Add new column in tChannelPartnerGroups table
-- ================================================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerGroups' AND COLUMN_NAME = 'DTStart')
BEGIN
    ALTER TABLE tChannelPartnerGroups
	ADD DTStart DATETIME NULL
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerGroups' AND COLUMN_NAME = 'DTEnd')
BEGIN
    ALTER TABLE tChannelPartnerGroups
	ADD DTEnd DATETIME NULL
END
GO


-- Udpate the date for the promo THREETHENFREE

UPDATE tChannelPartnerGroups 
SET 
  DTStart = '2017-08-16 00:00:00.000',
  DTEnd = '2017-09-30 00:00:00.000'
WHERE Name = 'THREETHENFREE'


-- Udpate the date for the group promo CommercialPayroll

UPDATE tChannelPartnerGroups 
SET 
  DTStart = '2000-01-01 00:00:00.000', -- Min Date
  DTEnd = '9999-12-31 00:00:00.000'    -- Max Date
WHERE Name = 'CommercialPayroll'

-- Udpate the date for the group promo InstorePayroll

UPDATE tChannelPartnerGroups 
SET 
  DTStart = '2000-01-01 00:00:00.000', -- Min Date
  DTEnd = '9999-12-31 00:00:00.000'    -- Max Date
WHERE Name = 'InstorePayroll'