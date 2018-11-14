--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <15-11-2017>
-- Description: Add new column as 'IsOverridable' in 'tChannelPartnerFeeAdjustments' table.
-- ================================================================================


IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerFeeAdjustments' AND COLUMN_NAME = 'Priority')
BEGIN
    ALTER TABLE tChannelPartnerFeeAdjustments
	ADD Priority INT NOT NULL DEFAULT(0)
END
GO


IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerFeeAdjustments' AND COLUMN_NAME = 'IsOverridable')
BEGIN
    ALTER TABLE tChannelPartnerFeeAdjustments
	ADD IsOverridable BIT NOT NULL DEFAULT(1)
END
GO

UPDATE
  tChannelPartnerFeeAdjustments 
SET 
  IsOverridable = 0
WHERE
  Name IN ('CommercialPayroll', 'InstorePayroll')

GO

UPDATE
  tChannelPartnerFeeAdjustments 
SET 
  Priority = 9999
WHERE
  Name IN ('CommercialPayroll', 'InstorePayroll')

GO
 

