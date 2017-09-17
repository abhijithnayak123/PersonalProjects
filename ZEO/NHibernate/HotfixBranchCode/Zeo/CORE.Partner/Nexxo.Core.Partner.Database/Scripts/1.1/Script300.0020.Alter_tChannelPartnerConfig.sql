-- ============================================================
-- Author:		Swarnalakshmi
-- Create date: <04/15/2014>
-- Description:	<Added columns for CheckFranking>
-- Rally ID:	<US1421>
-- ============================================================

IF NOT EXISTS 
(
  SELECT 
	* 
  FROM   
	sys.columns 
  WHERE 
  name = N'FrankData' 
	AND object_id = OBJECT_ID(N'[dbo].[tChannelPartnerConfig]')      
)
BEGIN         
	ALTER TABLE tChannelPartnerConfig 
	ADD FrankData VARCHAR(200) NULL	,
	IsCheckFrank bit
END
GO