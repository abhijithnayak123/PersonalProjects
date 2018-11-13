--- ================================================================================
-- Author:		<Abhijith>
-- Create date: <10/28/2015>
-- Description:	<As Alloy, a configurable pricing engine should be built to apply specific pricing at the right level for each product and apply to that level>
-- Jira ID:		<AL-1759>
-- ================================================================================

IF NOT EXISTS(SELECT 1 FROM tFeeAdjustmentCompareTypes WHERE Name='between')
BEGIN

  DECLARE @compareType TINYINT = 
  (
	 SELECT MAX(CompareTypePK) + 1
	 FROM tFeeAdjustmentCompareTypes
  )

  INSERT INTO tFeeAdjustmentCompareTypes
  ([CompareTypePK],[Name])
  VALUES
  (@compareType, 'between')
 
END
GO