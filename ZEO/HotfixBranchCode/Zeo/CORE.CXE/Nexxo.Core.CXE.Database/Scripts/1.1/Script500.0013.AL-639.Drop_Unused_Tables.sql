--===========================================================================================
-- Author:		<Rogy Eapen>
-- Created date: <July 02 2015>
-- Description:	<Scripts for dropping unused tables>           
-- Jira ID:	<AL-639>
--===========================================================================================

IF EXISTS (SELECT name FROM sysobjects
   WHERE name = 'REVINFO')
   DROP TABLE REVINFO
GO

IF EXISTS (SELECT name FROM sysobjects
   WHERE name = 'tCustomerProfiles')
BEGIN
   DROP TABLE tCustomerProfiles
END
GO

IF EXISTS (SELECT name FROM sysobjects
   WHERE name = 'tCustomerProfiles_Aud')
BEGIN
   DROP TABLE tCustomerProfiles_Aud
END
GO