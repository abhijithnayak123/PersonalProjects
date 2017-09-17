-- ============================================================
-- Author:		<Sunil Shetty>
-- Create date: <07/03/2015>
-- Description:	<Added columns to avoid duplicate transaction>
-- Rally ID:	<AL-591>
-- ============================================================

IF NOT EXISTS 
(
  SELECT 
	1 
  FROM   
	sys.columns 
  WHERE 
  name = N'IsActive' 
	AND object_id = OBJECT_ID(N'[dbo].[tTxn_FeeAdjustments]')      
)
BEGIN         
	ALTER TABLE tTxn_FeeAdjustments
	ADD IsActive BIT NOT NULL
	CONSTRAINT tTxn_FeeAdjustments_IsActive DEFAULT 1
END
GO