--===========================================================================================
-- Auther:			Ratheesh
-- Date Created:	10-Dec-2014
-- Description:		Script for inserting MGI Send Money limit

--===========================================================================================

DECLARE @mgiComplianceProgramPK uniqueidentifier
SET @mgiComplianceProgramPK = 'C375B0C7-06E1-419D-8F90-E2B26DF2859B'

/*Assumptions*/
--TypeId in tLimitTypes from Core.Compliance.Data.LimitTypeIds.cs
--[Value]in tLimitTypes from Core.Compliance.Data.FundPaymentTypes.cs and Core.Partner.Contract.ProviderIds.cs
--Provider ID: MoneyGram - 302 added in Core.Partner.Contract.ProviderIds.cs & Server.Data.ProviderIds.cs
INSERT INTO [dbo].[tLimitTypes]
	([rowguid], [ComplianceProgramPK], [ClassId], [TypeId], [Name], [Value], [DTCreate])
VALUES
	('DFA3AD73-BFF5-449D-B44A-7A8E55F16E3A',@mgiComplianceProgramPK, 'PaymentType', 
	13, 'PaymentTypeMoneyTransfer', '1;302', GETDATE())
	

INSERT INTO [dbo].[tLimits]
	([rowguid],[LimitTypePK],[Name],[PerX],[PerDay],[PerNDays],[NDays],[IsDefault],[MultipleNDaysLimits],[DTCreate])
VALUES
	(NEWID(),'DFA3AD73-BFF5-449D-B44A-7A8E55F16E3A','MGI MoneyTransfer',899.99,-1,-1,-1,1,NULL,GETDATE())


--===========================================================================================