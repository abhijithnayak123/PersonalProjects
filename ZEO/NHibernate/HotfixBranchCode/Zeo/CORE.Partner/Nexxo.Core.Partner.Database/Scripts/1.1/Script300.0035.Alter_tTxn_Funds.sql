-- Author:		<Rogy Eapen>
-- Create date: <12/03/2014>
-- Description:	<Added AdditionalFee column in tTxn_MoneyOrder>
-- Rally ID:	<DE3352>
-- ============================================================

IF NOT EXISTS(SELECT 1 FROM SYS.COLUMNS WHERE NAME = N'AdditionalFee' AND OBJECT_ID = OBJECT_ID(N'tTxn_MoneyOrder'))
BEGIN
	ALTER TABLE tTxn_MoneyOrder ADD
	AdditionalFee money NULL
END
GO