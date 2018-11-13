--===========================================================================================
-- Author:		Raviraja
-- Create date: <22/05/2014>
-- Description:	<Script for update error message >
-- Rally ID:	<US1991>
--===========================================================================================
--Nexxo.Core.CXE.Contract.CXECustomerException.cs
IF EXISTS(SELECT * FROM [dbo].[TMEssageStore] WHERE [MessageKey] = '1001.1004')
BEGIN
	UPDATE [dbo].[tMessageStore] 
	SET 
		[Content]='Customer registration in Nexxo DMS has failed.',
		[AddlDetails]='Please retry registering customer. If unsuccessful, please contact system administrator.',
		[DTLastMod]=getdate()
	WHERE 
		[MessageKey]='1001.1004'
END
ELSE
BEGIN
	INSERT INTO [dbo].[TMEssageStore]
		([Rowguid],[MessageKey],[PArtnerPK],[Language],[Content],[DTCreate],[AddlDetails],[Processor]) 
	VALUES
	(NewID(),'1001.1004',1,'0','Customer registration in Nexxo DMS has failed.',getdate(),
	'Please retry registering customer. If unsuccessful, please contact system administrator.','')
END
GO

--Nexxo.Core.CXE.Contract.CXECustomerException.cs
IF EXISTS(SELECT * FROM [dbo].[TMEssageStore] WHERE [MessageKey] = '1001.1007')
BEGIN
	UPDATE [dbo].[tMessageStore] 
	SET 
		[Content]='Customer Cannot transact.',
		[AddlDetails]='Customer profile status would need to be updated to active status to enable transaction.',
		[DTLastMod]=getdate()
	WHERE 
		[MessageKey]='1001.1007'
END
ELSE
BEGIN
	INSERT INTO [dbo].[TMEssageStore]
		([Rowguid],[MessageKey],[PArtnerPK],[Language],[Content],[DTCreate],[AddlDetails],[Processor]) 
	VALUES
	(NewID(),'1001.1007',1,'0','Customer Cannot transact.',getdate(),
	'Customer profile status would need to be updated to active status to enable transaction.','')
END
GO
--===========================================================================================