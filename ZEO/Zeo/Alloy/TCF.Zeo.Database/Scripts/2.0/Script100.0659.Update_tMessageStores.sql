--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <02-13-2018>
-- Description:	 Update script to update VISADPS to Visa.
-- Version One:		<B-12629>
-- ================================================================================
	
	UPDATE 
		tMessageStore
	SET 
		Processor = 'Visa'
	WHERE 
		Processor = 'VISADPS'
	

	UPDATE 
		tMessageStore
	SET
		Content = 'Visa is down at this time'
	WHERE
		MessageKey = '1003.103.5002'
GO


