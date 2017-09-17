-- ============================================================
-- Author:		<Rogy Eapen>
-- Create date: <02/04/2014>
-- Description:	<Added a New Column WUCard_TotalPointsEarned 
--				in 'tWUnion_Trx' to persist TotalPoints earned
--				by WU card Customer> 
-- Rally ID:	<US1707 - TA4125>
-- ============================================================

IF NOT EXISTS 
(
  SELECT 
	1 
  FROM   
	sys.columns 
  WHERE  
	object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]') 
    AND name = 'WUCard_TotalPointsEarned'
)
BEGIN         
	ALTER TABLE tWUnion_Trx 
	ADD WUCard_TotalPointsEarned VARCHAR(50) NULL
END
GO