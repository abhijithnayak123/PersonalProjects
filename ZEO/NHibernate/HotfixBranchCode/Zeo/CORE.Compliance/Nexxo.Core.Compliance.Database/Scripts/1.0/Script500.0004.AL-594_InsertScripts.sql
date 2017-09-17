-- ================================================================
-- Author:		Kaushik Sakala
-- Create date: <24/06/2015>
-- Description:	<As a product owner, I should have the ability to 
--				configure multiple limits for each client.>
-- JIRA ID:	<AL-594>
-- =================================================================

--INSERTING VALUES INTO tLimitTypes

DECLARE @COMPLIANCEPROGRAM UNIQUEIDENTIFIER

--Compliance Data for Synovus channlepartner
SELECT @COMPLIANCEPROGRAM=ComplianceProgramPK FROM tCompliancePrograms WHERE NAME = 'SynovusCompliance'

INSERT INTO tLimitTypes(LimitTypePK,ComplianceProgramPK,Name,DTServerCreate,DTServerLastModified)
     VALUES (NEWID(),@COMPLIANCEPROGRAM,'Check',GETDATE(),NULL),
			(NEWID(),@COMPLIANCEPROGRAM,'Funds',GETDATE(),NULL),
			(NEWID(),@COMPLIANCEPROGRAM,'BillPay',GETDATE(),NULL),
			(NEWID(),@COMPLIANCEPROGRAM,'MoneyOrder',GETDATE(),NULL),
			(NEWID(),@COMPLIANCEPROGRAM,'MoneyTransfer',GETDATE(),NULL),
			(NEWID(),@COMPLIANCEPROGRAM,'CashWithdrawal',GETDATE(),NULL),
			(NEWID(),@COMPLIANCEPROGRAM,'LoadToGPR',GETDATE(),NULL),
			(NEWID(),@COMPLIANCEPROGRAM,'ActivateGPR',GETDATE(),NULL),
			(NEWID(),@COMPLIANCEPROGRAM,'DebitGPR',GETDATE(),NULL)
GO
	
--Compliance Data for Carver channlepartner		

DECLARE @COMPLIANCEPROGRAM UNIQUEIDENTIFIER

SELECT @COMPLIANCEPROGRAM=ComplianceProgramPK FROM tCompliancePrograms WHERE NAME = 'CarverCompliance'

INSERT INTO tLimitTypes(LimitTypePK,ComplianceProgramPK,Name,DTServerCreate,DTServerLastModified)
     VALUES (NEWID(),@COMPLIANCEPROGRAM,'Check',GETDATE(),NULL),
			(NEWID(),@COMPLIANCEPROGRAM,'Funds',GETDATE(),NULL),
			(NEWID(),@COMPLIANCEPROGRAM,'BillPay',GETDATE(),NULL),
			(NEWID(),@COMPLIANCEPROGRAM,'MoneyOrder',GETDATE(),NULL),
			(NEWID(),@COMPLIANCEPROGRAM,'MoneyTransfer',GETDATE(),NULL)
GO
--Compliance Data for TCF channlepartner	

DECLARE @COMPLIANCEPROGRAM UNIQUEIDENTIFIER
			
SELECT @COMPLIANCEPROGRAM=ComplianceProgramPK FROM tCompliancePrograms WHERE NAME = 'TCFCompliance'

INSERT INTO tLimitTypes(LimitTypePK,ComplianceProgramPK,Name,DTServerCreate,DTServerLastModified)
      VALUES(NEWID(),@COMPLIANCEPROGRAM,'Check',GETDATE(),NULL),
			(NEWID(),@COMPLIANCEPROGRAM,'Funds',GETDATE(),NULL),
			(NEWID(),@COMPLIANCEPROGRAM,'BillPay',GETDATE(),NULL),
			(NEWID(),@COMPLIANCEPROGRAM,'MoneyOrder',GETDATE(),NULL),
			(NEWID(),@COMPLIANCEPROGRAM,'MoneyTransfer',GETDATE(),NULL),
			(NEWID(),@COMPLIANCEPROGRAM,'CashWithdrawal',GETDATE(),NULL),
			(NEWID(),@COMPLIANCEPROGRAM,'LoadToGPR',GETDATE(),NULL),
			(NEWID(),@COMPLIANCEPROGRAM,'ActivateGPR',GETDATE(),NULL),
			(NEWID(),@COMPLIANCEPROGRAM,'DebitGPR',GETDATE(),NULL)
GO

--Compliance Data for MGI channlepartner	

DECLARE @COMPLIANCEPROGRAM UNIQUEIDENTIFIER

SELECT @COMPLIANCEPROGRAM=ComplianceProgramPK FROM tCompliancePrograms WHERE NAME = 'MGICompliance'

INSERT INTO tLimitTypes(LimitTypePK,ComplianceProgramPK,Name,DTServerCreate,DTServerLastModified)
     VALUES (NEWID(),@COMPLIANCEPROGRAM,'Check',GETDATE(),NULL),
			(NEWID(),@COMPLIANCEPROGRAM,'Funds',GETDATE(),NULL),
			(NEWID(),@COMPLIANCEPROGRAM,'BillPay',GETDATE(),NULL),
			(NEWID(),@COMPLIANCEPROGRAM,'MoneyOrder',GETDATE(),NULL),
			(NEWID(),@COMPLIANCEPROGRAM,'MoneyTransfer',GETDATE(),NULL)
GO

--Compliance Data for Redstone channlepartner	

DECLARE @COMPLIANCEPROGRAM UNIQUEIDENTIFIER

SELECT @COMPLIANCEPROGRAM=ComplianceProgramPK FROM tCompliancePrograms WHERE NAME = 'RedstoneCompliance'

INSERT INTO tLimitTypes(LimitTypePK,ComplianceProgramPK,Name,DTServerCreate,DTServerLastModified)
      VALUES(NEWID(),@COMPLIANCEPROGRAM,'Funds',GETDATE(),NULL),
			(NEWID(),@COMPLIANCEPROGRAM,'Check',GETDATE(),NULL),
			(NEWID(),@COMPLIANCEPROGRAM,'BillPay',GETDATE(),NULL),
			(NEWID(),@COMPLIANCEPROGRAM,'MoneyTransfer',GETDATE(),NULL)

GO
--Limits data for Synovus and Carver

DECLARE @SynovusComplianceId UNIQUEIDENTIFIER

SELECT @SynovusComplianceId= ComplianceProgramPK FROM tCompliancePrograms where Name ='SynovusCompliance'

--Adding Data for synovus compliance
INSERT INTO tLimits(LimitPK,LimitTypePK,PerTransactionMaximum,PerTransactionMinimum,RollingLimits,DTServerCreate)
VALUES
(NEWID(),(select LimitTypePK from tLimitTypes where ComplianceProgramPK = @SynovusComplianceId and Name = 'Check'),NULL,3,NULL,GETDATE()),
(NEWID(),(select LimitTypePK from tLimitTypes where ComplianceProgramPK = @SynovusComplianceId and Name = 'BillPay'),NULL,1,NULL,GETDATE()),
(NEWID(),(select LimitTypePK from tLimitTypes where ComplianceProgramPK = @SynovusComplianceId and Name = 'MoneyTransfer'),NULL,1,NULL,GETDATE()),
(NEWID(),(select LimitTypePK from tLimitTypes where ComplianceProgramPK = @SynovusComplianceId and Name = 'MoneyOrder'),1000,10,NULL,GETDATE()),
(NEWID(),(select LimitTypePK from tLimitTypes where ComplianceProgramPK = @SynovusComplianceId and Name = 'LoadToGPR'),NULL,20,NULL,GETDATE()),
(NEWID(),(select LimitTypePK from tLimitTypes where ComplianceProgramPK = @SynovusComplianceId and Name = 'DebitGPR'),2500,NULL,'1:2500',GETDATE()),
(NEWID(),(select LimitTypePK from tLimitTypes where ComplianceProgramPK = @SynovusComplianceId and Name = 'ActivateGPR'),NULL,20,'1:2500',GETDATE())

GO

DECLARE @TCFComplianceId UNIQUEIDENTIFIER

SELECT @TCFComplianceId= ComplianceProgramPK FROM tCompliancePrograms where Name ='TCFCompliance'

--Adding Data for TCF compliance
INSERT INTO tLimits(LimitPK,LimitTypePK,PerTransactionMaximum,PerTransactionMinimum,RollingLimits,DTServerCreate)
VALUES
(NEWID(),(select LimitTypePK from tLimitTypes where ComplianceProgramPK = @TCFComplianceId and Name = 'MoneyOrder'),1000,NULL,NULL,GETDATE())
GO

DECLARE @CarverComplianceId UNIQUEIDENTIFIER

SELECT @CarverComplianceId= ComplianceProgramPK FROM tCompliancePrograms where Name ='CarverCompliance'

--Adding Data for Carver compliance
INSERT INTO tLimits(LimitPK,LimitTypePK,PerTransactionMaximum,PerTransactionMinimum,RollingLimits,DTServerCreate)
VALUES
(NEWID(),(select LimitTypePK from tLimitTypes where ComplianceProgramPK = @CarverComplianceId and Name = 'Check'),NULL,3,NULL,GETDATE()),
(NEWID(),(select LimitTypePK from tLimitTypes where ComplianceProgramPK = @CarverComplianceId and Name = 'BillPay'),NULL,10,NULL,GETDATE()),
(NEWID(),(select LimitTypePK from tLimitTypes where ComplianceProgramPK = @CarverComplianceId and Name = 'MoneyTransfer'),NULL,1,NULL,GETDATE()),
(NEWID(),(select LimitTypePK from tLimitTypes where ComplianceProgramPK = @CarverComplianceId and Name = 'MoneyOrder'),1000,10,NULL,GETDATE())
GO

