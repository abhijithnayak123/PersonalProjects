-- ============================================================
-- Author:	<Rogy Eapen>
-- Create date: <29/01/2015>
-- Description:	<Updating IsSWBRequired for WU Send Money, WU Pay Bill and WU Receive Money>
-- Rally ID:	<US2054>
-- ============================================================
Declare @ProcessorId uniqueidentifier
Declare @BillPayProductId uniqueidentifier
Declare @MoneyTransferProductId uniqueidentifier
Declare @ReceiveMoneyProductId uniqueidentifier

select @ProcessorId = rowguid from tProcessors where Name='WesternUnion'
select @ReceiveMoneyProductId = rowguid from tProducts where Name='ReceiveMoney'
select @MoneyTransferProductId = rowguid from tProducts where Name='MoneyTransfer'
select @BillPayProductId = rowguid from tProducts where Name='BillPayment'

UPDATE tProductProcessorsMapping SET IsSWBRequired = 1 WHERE ProductId = @BillPayProductId and ProcessorId = @ProcessorId 
UPDATE tProductProcessorsMapping SET IsSWBRequired = 1 WHERE ProductId = @MoneyTransferProductId and ProcessorId = @ProcessorId
UPDATE tProductProcessorsMapping SET IsSWBRequired = 1 WHERE ProductId = @ReceiveMoneyProductId and ProcessorId = @ProcessorId 