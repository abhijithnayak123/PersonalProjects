-- Author:		<Rogy Eapen>
-- Create date: <12/01/2014>
-- Description:	<Added AdditionalFee column in tTxn_Check>
-- Rally ID:	<DE3352>
-- ============================================================

IF NOT EXISTS(SELECT 1 FROM SYS.COLUMNS WHERE NAME = N'AdditionalFee' AND OBJECT_ID = OBJECT_ID(N'tTxn_Check'))
BEGIN
	ALTER TABLE tTxn_Check ADD
	AdditionalFee money NULL
END
GO