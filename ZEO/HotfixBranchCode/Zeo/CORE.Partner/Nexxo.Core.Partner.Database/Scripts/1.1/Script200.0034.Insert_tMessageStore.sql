--===========================================================================================
-- Author:		<Bineesh Raghavan>
-- Create date: <05/23/2014>
-- Description:	<Error message for GPR withdraw error>
-- Rally ID:	<DE2845>
--===========================================================================================

INSERT tMessageStore 
(
	rowguid, 
	MessageKey, 
	PartnerPK, 
	Language, 
	Content, 
	DTCreate, 
	AddlDetails, 
	Processor
)
VALUES 
(
	NEWID(), 
	'1008.6012', 
	1, 
	0, 
	'GPR Withdraw Limit Exceeded', 
	GETDATE(), 
	'GPR Withdraw Limit Exceeded', 
	'Nexxo'
)
GO