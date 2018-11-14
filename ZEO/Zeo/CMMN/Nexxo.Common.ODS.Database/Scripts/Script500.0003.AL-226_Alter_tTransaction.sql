-- ============================================================
-- Author:        <Divya Boddu>
-- Create date:   <06/25/2015>
-- Description:   <Added New DTLocal Column in 'tTransaction' for ODS> 
-- Rally ID:      <AL-226>
-- ============================================================
IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'DTLocal' AND Object_ID = Object_ID(N'tTransaction'))
	BEGIN 
		ALTER TABLE [tTransaction]
		ADD DTLocal DATETIME NULL
	END
GO
