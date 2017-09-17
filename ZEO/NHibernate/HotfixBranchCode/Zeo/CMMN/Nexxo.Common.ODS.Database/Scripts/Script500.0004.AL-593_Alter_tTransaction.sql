-- ============================================================
-- Author:        <Chinar Kulkarni>
-- Create date:   <06/25/2015>
-- Description:   <Added New DTLocal, FinalStatusDescription Columns in 'tTransaction' for ODS> 
-- Rally ID:      <AL-593>
-- ============================================================

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'BankId' AND Object_ID = Object_ID(N'tTransaction'))
	BEGIN 
		ALTER TABLE tTransaction
		ADD BankId VARCHAR(40) NULL
	END
GO

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'FinalStatusDescription' AND Object_ID = Object_ID(N'tTransaction'))
	BEGIN 
		ALTER TABLE tTransaction
		ADD FinalStatusDescription VARCHAR(50) NULL
	END
GO

