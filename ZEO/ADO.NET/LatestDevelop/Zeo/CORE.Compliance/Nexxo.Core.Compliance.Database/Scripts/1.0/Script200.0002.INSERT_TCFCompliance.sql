--===========================================================================================
-- Auther:			Pamila Jose
-- Date Created:	07/02/2014
-- Description:		Script for inserting TCF Compliance details
-- Rally ID:		US1610
--===========================================================================================
INSERT INTO [dbo].[tCompliancePrograms]
	([rowguid],[Name],[RunOFAC],[DTCreate])
VALUES
	('F5748C1D-A79A-48CF-B554-B2E254408FA6', 'TCFCompliance', 0, GETDATE())

/*Assumptions*/
--TypeId in tLimitTypes from Core.Compliance.Data.LimitTypeIds.cs
--[Value]in tLimitTypes from Core.Compliance.Data.FundPaymentTypes.cs and Core.Partner.Contract.ProviderIds.cs
--Provider ID: TCFMoneyOrder - 504 added in Core.Partner.Contract.ProviderIds.cs & Server.Data.ProviderIds.cs
INSERT INTO [dbo].[tLimitTypes]
	([rowguid], [ComplianceProgramPK], [ClassId], [TypeId], [Name], [Value], [DTCreate])
VALUES
	('3E99D7CA-3A74-40E1-98AF-618254E1B6B6','F5748C1D-A79A-48CF-B554-B2E254408FA6', 'Product', 
	15, 'MoneyOrder', '4,5;504', GETDATE()),
	('A4F64E87-DAA0-4789-BF93-49EAFA4E82E0','F5748C1D-A79A-48CF-B554-B2E254408FA6', 'PaymentType', 
	13, 'PaymentTypeMoneyTransfer', '1,2,3;301', GETDATE()),
	('715F36CB-9156-44FC-8072-50270C1D1568','F5748C1D-A79A-48CF-B554-B2E254408FA6', 'PaymentType', 
	12, 'PaymentTypeBillPay', '4,5,9;401', GETDATE())
	

--Reference --> Nexxo.Core.Compliance.Database\Scripts\1.0\Script009.InsertScripts._SynovusLimits.sql
INSERT INTO [dbo].[tLimits]
	([rowguid],[LimitTypePK],[Name],[PerX],[PerDay],[PerNDays],[NDays],[IsDefault],[MultipleNDaysLimits],[DTCreate])
VALUES
	(NEWID(),'3E99D7CA-3A74-40E1-98AF-618254E1B6B6','TCF MoneyOrder',1000,5000,5000,30,1,'2:5000;7:5000',GETDATE()),
	(NEWID(),'A4F64E87-DAA0-4789-BF93-49EAFA4E82E0','TCF MoneyTransfer',5000,5000,10000,30,1,'4:5000;14:75000',GETDATE()),
	(NEWID(),'715F36CB-9156-44FC-8072-50270C1D1568','TCF BillPayment',2000,2000,5000,30,1,NULL,GETDATE())

INSERT INTO [dbo].[tTransactionMinimums]
	([rowguid],[ComplianceProgramPK],[TransactionType],[Minimum],[DTCreate])
VALUES
	(NEWID(),'F5748C1D-A79A-48CF-B554-B2E254408FA6',5,10,getdate()),/*MoneyOrder-5*/
	(NEWID(),'F5748C1D-A79A-48CF-B554-B2E254408FA6',6,50,getdate()),/*MoneyTransfer-6*/
	(NEWID(),'F5748C1D-A79A-48CF-B554-B2E254408FA6',2,3,getdate()),/*Check -2*/
	(NEWID(),'F5748C1D-A79A-48CF-B554-B2E254408FA6',4,10,getdate())/*BillPay -4*/

--===========================================================================================