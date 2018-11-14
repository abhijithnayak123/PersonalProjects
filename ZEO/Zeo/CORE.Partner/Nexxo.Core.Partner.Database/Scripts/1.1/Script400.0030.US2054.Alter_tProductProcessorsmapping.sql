-- ============================================================
-- Author:	<Rogy Eapen>
-- Create date: <29/01/2015>
-- Description:	<Alter Script to add IsSWBRequired column in tProductProcessorsmapping table>
-- Rally ID:	<US2054>
-- ============================================================

IF NOT EXISTS 
(
  SELECT * FROM  sys.columns WHERE name = N'IsSWBRequired' AND object_id = OBJECT_ID(N'[dbo].[tProductProcessorsMapping]')      
)
BEGIN
	ALTER TABLE tProductProcessorsMapping
	ADD IsSWBRequired BIT NOT NULL DEFAULT 0
END
GO