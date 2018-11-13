-- ============================================================
-- Author     :	<RAJKUMAR M>
-- Create date: <27/02/2015>
-- Description:	<Updating ReceiptCopies,ReceiptReprintCopies for WU Send Money, WU Pay Bill and WU Receive Money in "tProductProcessorsMapping" Table>
-- JIRA ID    :	<AL-93>
-- ============================================================

UPDATE 
	tProductProcessorsMapping 
SET
	ReceiptCopies = 2, 
	ReceiptReprintCopies = 1
FROM 
	tProductProcessorsMapping tp
	INNER JOIN tProducts p on tp.ProductId = p.rowguid 
	INNER JOIN tProcessors t on t.rowguid = tp.processorid
WHERE 
	t.Name = 'WesternUnion' AND
	p.Name IN ('MoneyTransfer','BillPayment','ReceiveMoney')
GO