--===========================================================================================
-- Author:		Raviraja
-- Create date: <25/04/2014>
-- Description:	<Script for insert CCIS Lookup all Error  >
-- Rally ID:	<U3S198>
--===========================================================================================
--Customer-CCIS-Lookup
INSERT INTO [dbo].[TMEssageStore]
	([Rowguid],[MessageKey],[PArtnerPK],[Language],[Content],
	[DTCreate],[AddlDetails],[Processor]) 
VALUES 
	(NewID(),'1011.2006',1,'0','CCIS Customer Lookup Failed.',
	getdate(),'CCIS Customer Lookup Failed.','')
Go

--===========================================================================================