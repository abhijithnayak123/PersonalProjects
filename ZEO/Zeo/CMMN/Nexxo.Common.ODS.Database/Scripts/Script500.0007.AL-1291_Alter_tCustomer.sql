-- ============================================================
-- Author:        <Chinar Kulkarni>
-- Create date:   <08/14/2015>
-- Description:   <Added New columns IDIssuingStateAbbr and IDIssuingCountryAbbr to the table tCustomer in ODS> 
-- Rally ID:      <AL-1051>
-- ============================================================

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'IDIssuingStateAbbr' AND Object_ID = Object_ID(N'tCustomer'))
	BEGIN 
		ALTER TABLE tCustomer
		ADD IDIssuingStateAbbr NVARCHAR(10) NULL
	END
GO

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'IDIssuingCountryAbbr' AND Object_ID = Object_ID(N'tCustomer'))
	BEGIN 
		ALTER TABLE tCustomer
		ADD IDIssuingCountryAbbr CHAR(2) NULL
	END
GO