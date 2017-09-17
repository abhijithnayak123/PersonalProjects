-- ============================================================
-- Author:        <Chinar Kulkarni>
-- Create date:   <08/13/2015>
-- Description:   <Added New columns OccupationDDLCode and OccupationDDLDescription to the table tCustomer in ODS> 
-- Rally ID:      <AL-1051>
-- ============================================================

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'OccupationDDLCode' AND Object_ID = Object_ID(N'tCustomer'))
	BEGIN 
		ALTER TABLE tCustomer
		ADD OccupationDDLCode VARCHAR(5) NULL
	END
GO

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'OccupationDDLDescription' AND Object_ID = Object_ID(N'tCustomer'))
	BEGIN 
		ALTER TABLE tCustomer
		ADD OccupationDDLDescription VARCHAR(50) NULL
	END
GO