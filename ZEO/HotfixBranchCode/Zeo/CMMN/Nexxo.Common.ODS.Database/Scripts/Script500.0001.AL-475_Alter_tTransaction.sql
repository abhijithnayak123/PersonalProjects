-- ============================================================
-- Author:        <Ashok Kumar G>
-- Create date:   <06/03/2015>
-- Description:   <Added BusinessDate Columns in 'tTransaction' for ODS> 
-- Rally ID:      <AL-475>
-- ============================================================
IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'BusinessDate' AND Object_ID = Object_ID(N'tTransaction'))
	BEGIN 
		ALTER TABLE tTransaction
		ADD BusinessDate DATE NULL
	END
GO
