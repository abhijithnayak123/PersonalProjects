--===========================================================================================
-- Auther:			Raviraja
-- Date Created:	1/22/2014
-- Description:		Script for insert in to [TMEssageStore]  
--===========================================================================================
--Nexxo.Cxn.BillPay.Contract.BillPayException.cs
INSERT INTO [dbo].[TMEssageStore]
	([Rowguid],[MessageKey],[PArtnerPK],[Language],[Content],[DTCreate],
	[AddlDetails],[Processor]) 
VALUES
	(NewID(),'1004.2407',1,'0','NYCHA BillPay Create Failed',getdate(),
	'BillPay Create Failed',''),
	(NewID(),'1004.2408',1,'0','NYCHA BillPay Details Not Found',getdate(),
	'BillPay Not Found',''),
	(NewID(),'1004.2409',1,'0','Create Account Failed',getdate(),
	'Create Account Failed',''),
	(NewID(),'1004.2410',1,'0','Account Not Found',getdate(),
	'Account Not Found',''),
	(NewID(),'1004.2411',1,'0','NYCHA Payment Create Failed',getdate(),
	'NYCHA Payment Create Failed',''),
	(NewID(),'1004.2412',1,'0','Not valid Account/Tenant ID combination',getdate(),
	'Not valid Account/Tenant ID combination',''),
	(NewID(),'1004.2413',1,'0','NYCHA BillPay Commit Failed',getdate(),
	'NYCHA BillPay Commit Failed','')
Go

--Nexxo.Biz.Customer.Contract.BizCustomerException.cs
INSERT INTO [dbo].[TMEssageStore]
	([Rowguid],[MessageKey],[PArtnerPK],[Language],[Content],[DTCreate],
	[AddlDetails],[Processor]) 
VALUES
	(NewID(),'1001.6043',1,'0','Anonymous Customer Not Created',getdate(),
	'Anonymous Customer Not Created','')
Go

--Nexxo.Biz.Compliance.Contract.BizComplianceLimitException.cs
INSERT INTO [dbo].[TMEssageStore]
	([Rowguid],[MessageKey],[PArtnerPK],[Language],[Content],[DTCreate],
	[AddlDetails],[Processor]) 
VALUES
	(NewID(),'1008.6011',1,'0','Payment Amount is less than Minimum Amount',getdate(),
	'Payment Amount is less than Minimum Amount','')
Go
--===========================================================================================



