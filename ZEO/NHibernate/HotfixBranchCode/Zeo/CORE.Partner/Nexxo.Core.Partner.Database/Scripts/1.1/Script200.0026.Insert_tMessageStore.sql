--===========================================================================================
-- Author:		Raviraja
-- Create date: <15/04/2014>
-- Description:	<Script for insert new FIS Validate customer exception  >
-- Rally ID:	<US1917>
--===========================================================================================
--Customer-FIS-ValidateCustomerError
INSERT INTO [dbo].[TMEssageStore]
	([Rowguid],[MessageKey],[PArtnerPK],[Language],[Content],
	[DTCreate],[AddlDetails],[Processor]) 
VALUES 
	(NewID(),'1011.2004.600.201',1,'0','Customer core account ID/number not associated with profile.Customer cannot transact',
	getdate(),'Retry updating customer profile or contact system administrator.','FIS')
Go

--Nexxo.Core.CXE.Contract.CXECustomerException.cs
INSERT INTO [dbo].[TMEssageStore]
	([Rowguid],[MessageKey],[PArtnerPK],[Language],[Content],[DTCreate],
	[AddlDetails],[Processor]) 
VALUES
	(NewID(),'1001.1007',1,'0','Customer accout is inactive. Customer cannot transact',getdate(),
	'Retry updating customer profile as active or contact system administrator.','')
Go

--===========================================================================================