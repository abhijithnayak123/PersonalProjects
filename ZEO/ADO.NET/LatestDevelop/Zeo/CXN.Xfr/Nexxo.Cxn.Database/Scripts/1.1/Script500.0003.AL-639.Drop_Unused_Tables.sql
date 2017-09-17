--===========================================================================================
-- Author:		<Rogy Eapen>
-- Created date: <July 03 2015>
-- Description:	<Scripts for dropping unused tables>           
-- Jira ID:	<AL-639>
--===========================================================================================

IF EXISTS (SELECT name FROM sysobjects
   WHERE name = 'REVINFO')
BEGIN
   DROP TABLE REVINFO
END
GO

IF EXISTS (SELECT name FROM sysobjects
   WHERE name = 'tBillPayProcessorLogin')
BEGIN
   DROP TABLE tBillPayProcessorLogin
END
GO

IF EXISTS (SELECT name FROM sysobjects
   WHERE name = 'tChxr_Identity')
BEGIN
   DROP TABLE tChxr_Identity
END
GO

IF EXISTS (SELECT name FROM sysobjects
   WHERE name = 'tGPRCards')
BEGIN
   DROP TABLE tGPRCards
END
GO

IF EXISTS (SELECT name FROM sysobjects
   WHERE name = 'tRelationships')
BEGIN
   DROP TABLE tRelationships
END
GO

IF EXISTS (SELECT name FROM sysobjects
   WHERE name = 'tWUnion_AmountTypes')
BEGIN
   DROP TABLE tWUnion_AmountTypes
END
GO

IF EXISTS (SELECT name FROM sysobjects
   WHERE name = 'tWUnion_NameTypeMapping')
BEGIN
   DROP TABLE tWUnion_NameTypeMapping
END
GO

IF EXISTS (SELECT name FROM sysobjects
   WHERE name = 'tWUnion_Relationships')
BEGIN
   DROP TABLE tWUnion_Relationships
END
GO

-- IF EXISTS (SELECT name FROM sysobjects
   -- WHERE name = 'tNYCHA_BillPay_Trx')
-- BEGIN
   -- DROP TABLE tNYCHA_BillPay_Trx
-- END
-- GO

-- IF EXISTS (SELECT name FROM sysobjects
   -- WHERE name = 'tNYCHA_BillPay_Trx_Aud')
-- BEGIN
   -- DROP TABLE tNYCHA_BillPay_Trx_Aud
-- END
-- GO

-- IF EXISTS (SELECT name FROM sysobjects
   -- WHERE name = 'tNYCHA_BillPay_Account')
-- BEGIN
   -- DROP TABLE tNYCHA_BillPay_Account
-- END
-- GO


-- IF EXISTS (SELECT name FROM sysobjects
   -- WHERE name = 'tNYCHAPayments')
-- BEGIN
	-- DROP TABLE tNYCHAPayments
-- END
-- GO

-- IF EXISTS (SELECT name FROM sysobjects
   -- WHERE name = 'tNYCHATenant')
-- BEGIN
	-- DROP TABLE tNYCHATenant
-- END
-- GO

-- IF EXISTS (SELECT name FROM sysobjects
   -- WHERE name = 'tNYCHAFiles')
-- BEGIN
	-- DROP TABLE tNYCHAFiles
-- END
-- GO