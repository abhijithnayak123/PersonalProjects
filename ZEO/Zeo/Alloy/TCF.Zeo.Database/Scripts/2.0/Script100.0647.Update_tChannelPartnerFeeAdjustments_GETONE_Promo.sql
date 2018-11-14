--- ===============================================================================
-- Author:		     <Manikandan Govindraj>
-- Create date:      <12-11-2017>
-- Description:	     Update promo Name and promo description
-- Version ONE ID:   <B-09843>
-- ================================================================================



IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerFeeAdjustments' AND COLUMN_NAME = 'Description')
BEGIN
   ALTER TABLE tChannelPartnerFeeAdjustments ALTER COLUMN Description NVARCHAR(500)
END
GO


UPDATE tChannelPartnerFeeAdjustments 
SET
  Name = 'GETONE',
  Description = 'Cash One, Get One: When you cash one check, your next check will be free (up to a check amount of $1,500).'
WHERE 
 Name = 'BOGO'

GO


UPDATE tFeeAdjustmentConditions 
SET 
  ConditionValue = 'GETONE'
WHERE 
  ConditionValue = 'BOGO'

GO