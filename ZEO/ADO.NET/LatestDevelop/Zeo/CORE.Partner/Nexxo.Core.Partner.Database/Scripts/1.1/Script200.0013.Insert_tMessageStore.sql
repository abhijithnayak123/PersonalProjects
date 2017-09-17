--===========================================================================================
-- Auther:			Raviraja
-- Date Created:	12/17/2013
-- Description:		Script for insert new and refactoring MoneyOrder Exception 
--===========================================================================================
--Nexxo.Biz.Compliance.Contract.BizComplianceLimitException.cs
INSERT INTO [dbo].[TMEssageStore]
	([Rowguid],[MessageKey],[PArtnerPK],[Language],[Content],[DTCreate],[AddlDetails],[Processor]) 
VALUES
	(NewID(),'1008.6010',1,'0','MoneyOrder Amount Less Than Minimum Amount',getdate(),
	'MoneyOrder Amount Less Than Minimum Amount','')
Go

--Nexxo.Core.Partner.Contract.ChannelPartnerException.cs
INSERT INTO [dbo].[TMEssageStore]
	([Rowguid],[MessageKey],[PArtnerPK],[Language],[Content],[DTCreate],[AddlDetails],[Processor]) 
VALUES
	(NewID(),'1010.3009',1,'0','Channel Partner Money Order Fee Not Found',getdate(),
	'Channel Partner Money Order Fee Not Found','')
Go

--Refactoring Nexxo.Biz.MoneyOrderEngine.Contract.BizMoneyOrderEngineException.cs
IF EXISTS(SELECT * FROM [dbo].[TMEssageStore] WHERE [MessageKey] = '1006.6000')
BEGIN
	DELETE FROM [dbo].[TMEssageStore] WHERE [MessageKey] = '1006.6000'
	
	UPDATE [dbo].[tMessageStore] SET [MessageKey]='1006.6000'
	WHERE [MessageKey]='1006.6002'
END
ELSE
BEGIN
	UPDATE [dbo].[tMessageStore] SET [MessageKey]='1006.6000'
	WHERE [MessageKey]='1006.6002'
END
GO

IF EXISTS(SELECT * FROM [dbo].[TMEssageStore] WHERE [MessageKey] = '1006.6001')
BEGIN
	DELETE FROM [dbo].[TMEssageStore] WHERE [MessageKey] = '1006.6001'
	
	UPDATE [dbo].[tMessageStore] SET [MessageKey]='1006.6001'
	WHERE [MessageKey]='1006.6003'
END
ELSE
BEGIN
	UPDATE [dbo].[tMessageStore] SET [MessageKey]='1006.6001'
	WHERE [MessageKey]='1006.6003'
END
GO
--===========================================================================================
