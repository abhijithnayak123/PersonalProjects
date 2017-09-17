-- ============================================================
-- Author:		<Ashok Kumar G>
-- Create date: <11/4/2014>
-- Description:	<Alter Script to add IsSSNRequired column in tProductProcessorsmapping table.
--				Update script to achieve WU Send Money or WU Pay Bill or with GPR Activate/Issue calls ssn required>
-- Rally ID:	<US2156>
-- ============================================================

IF NOT EXISTS 
(
  SELECT * FROM  sys.columns WHERE name = N'IsSSNRequired' AND object_id = OBJECT_ID(N'[dbo].[tProductProcessorsMapping]')      
)
BEGIN
	ALTER TABLE tProductProcessorsMapping
	ADD IsSSNRequired BIT NOT NULL DEFAULT 0
END
GO

UPDATE tProductProcessorsMapping SET IsSSNRequired = 1 WHERE ProductId = '10D73929-52D5-4D5E-956F-0771526A3C42' and ProcessorId = 'AAB01C7F-1D62-422D-AC96-A855A5D0E1A1' 
UPDATE tProductProcessorsMapping SET IsSSNRequired = 1 WHERE ProductId = 'A3EB4C6E-F28C-4DB0-A2C6-DDDEC671D2DF' and ProcessorId = 'AAB01C7F-1D62-422D-AC96-A855A5D0E1A1' 
UPDATE tProductProcessorsMapping SET IsSSNRequired = 1 WHERE ProductId = '8F2FA74E-C135-4028-8F4D-36A57F46CF6E' and ProcessorId = 'C095332C-6754-422C-ACFD-4925E15F7449' 