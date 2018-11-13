--=========================================================
-- Author: <Kaushik S>
-- Date Created: <Feb 25 2015>
-- Description: <Adding new coloum CanParkReceiveMoney 
--				to tProductProcessorsMapping Table to disable
--				receive money park configurable>
-- User Story ID: <AL-89>
--==========================================================

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'CanParkReceiveMoney' AND Object_ID = Object_ID(N'tProductProcessorsMapping'))
BEGIN 
	ALTER TABLE tProductProcessorsMapping
	ADD CanParkReceiveMoney BIT NOT NULL CONSTRAINT DF_tProductProcessorsMapping_CanParkReceiveMoney DEFAULT (0)
END
GO

