	--===========================================================================================
-- Auther:			Pamila Jose
-- Date Created:	27/11/2014
-- Description:		New columns related to Send Money Refund in tMGram_Transfer_Trx 
--===========================================================================================


	ALTER TABLE tMGram_Transfer_Trx 
	ADD 
	SendReversalType [varchar] (1) NULL,
	SendReversalReason [varchar] (25) NULL,
	FeeRefund [varchar](1) NULL,
	OperatorName [varchar](7) NULL,
	RefundTotalAmount [decimal](14,3) NULL,
	RefundFaceAmount [decimal](14,3) NULL,
	RefundFeeAmount [decimal](14,3) NULL
	GO
