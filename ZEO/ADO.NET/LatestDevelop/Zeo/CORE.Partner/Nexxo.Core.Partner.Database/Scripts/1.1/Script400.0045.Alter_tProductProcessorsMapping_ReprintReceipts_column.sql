-- ============================================================
-- Author     :	<RAJKUMAR M>
-- Create date: <FEB 26 2015>
-- Description:	<Added columns "ReceiptCopies" and "ReprintReceiptCopies"
--				in 'tProductProcessorsMapping' to print number of copies 
--				at product and processor level>
-- JIRA ID    :	<AL-93>
-- ============================================================

IF NOT EXISTS 
(
	SELECT 
		1
	FROM sys.columns 
	WHERE name  in (N'ReceiptCopies', N'ReprintReceiptCopies') AND object_id = OBJECT_ID(N'[dbo].[tProductProcessorsMapping]')
)
BEGIN
	ALTER TABLE 
		tProductProcessorsMapping 
	ADD 
		ReceiptCopies INT NOT NULL CONSTRAINT DF_tProductProcessorsMapping_ReceiptCopies DEFAULT (1),
		ReceiptReprintCopies INT NOT NULL CONSTRAINT DF_tProductProcessorsMapping_ReceiptReprintCopies DEFAULT (1)
END
GO