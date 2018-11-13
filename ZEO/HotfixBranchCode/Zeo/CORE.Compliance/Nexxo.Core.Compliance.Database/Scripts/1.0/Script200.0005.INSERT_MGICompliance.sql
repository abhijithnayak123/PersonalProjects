--===========================================================================================
-- Auther:			Ratheesh
-- Date Created:	07-Oct-2014
-- Description:		Script for inserting MGI Compliance details
-- Rally ID:		
--===========================================================================================
INSERT INTO [dbo].[tCompliancePrograms]
	([rowguid],[Name],[RunOFAC],[DTCreate])
VALUES
	('C375B0C7-06E1-419D-8F90-E2B26DF2859B', 'MGICompliance', 0, GETDATE())

/*Assumptions*/
--TypeId in tLimitTypes from Core.Compliance.Data.LimitTypeIds.cs
--[Value]in tLimitTypes from Core.Compliance.Data.FundPaymentTypes.cs and Core.Partner.Contract.ProviderIds.cs
--Provider ID: MGIMoneyOrder - 505 added in Core.Partner.Contract.ProviderIds.cs & Server.Data.ProviderIds.cs
INSERT INTO [dbo].[tLimitTypes]
	([rowguid], [ComplianceProgramPK], [ClassId], [TypeId], [Name], [Value], [DTCreate])
VALUES
	('072488E5-6563-4808-B9D8-2D3F3620EF48','C375B0C7-06E1-419D-8F90-E2B26DF2859B', 'Product', 
	15, 'MoneyOrder', '4,5;505', GETDATE())
	

--Reference --> Nexxo.Core.Compliance.Database\Scripts\1.0\Script009.InsertScripts._SynovusLimits.sql
INSERT INTO [dbo].[tLimits]
	([rowguid],[LimitTypePK],[Name],[PerX],[PerDay],[PerNDays],[NDays],[IsDefault],[MultipleNDaysLimits],[DTCreate])
VALUES
	(NEWID(),'072488E5-6563-4808-B9D8-2D3F3620EF48','MGI MoneyOrder',1000,5000,5000,30,1,'2:5000;7:5000',GETDATE())

INSERT INTO [dbo].[tTransactionMinimums]
	([rowguid],[ComplianceProgramPK],[TransactionType],[Minimum],[DTCreate])
VALUES
	(NEWID(),'C375B0C7-06E1-419D-8F90-E2B26DF2859B',5,10,getdate()),/*MoneyOrder-5*/
	(NEWID(),'C375B0C7-06E1-419D-8F90-E2B26DF2859B',2,3,getdate())/*Check -2*/

--===========================================================================================