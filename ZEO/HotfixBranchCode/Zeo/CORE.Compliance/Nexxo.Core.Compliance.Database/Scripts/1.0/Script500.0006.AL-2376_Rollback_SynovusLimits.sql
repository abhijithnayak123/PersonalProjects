-- ================================================================
-- Author:		Karun
-- Create date: <01/Oct/2015>
-- Description:	<Update Limits for Synovus MO TAS-169, PROD-63, AL2376 >
-- JIRA ID:	<AL-2376>
-- =================================================================

DECLARE @MOLimitTypePK UNIQUEIDENTIFIER
DECLARE @BPLimitTypePK UNIQUEIDENTIFIER 
DECLARE @ComplianceProgramPK UNIQUEIDENTIFIER

SELECT @ComplianceProgramPK = ComplianceProgramPK FROM tcomplianceprograms where name = 'SynovusCompliance'
SELECT @MOLimitTypePK = LimitTypePK FROM tlimittypes WHERE NAME = 'MoneyOrder' AND ComplianceProgramPK = @ComplianceProgramPK
SELECT @BPLimitTypePK = LimitTypePK FROM tlimittypes WHERE NAME = 'BillPay' AND ComplianceProgramPK = @ComplianceProgramPK

UPDATE tlimits SET pertransactionminimum = 1.00 where limittypepk = @MOLimitTypePK
UPDATE tlimits SET pertransactionminimum = 10.00 where limittypepk = @BPLimitTypePK 

GO
