-- ================================================================
-- Author:		Karun
-- Create date: <14/Dec/2015>
-- Description:	<Update Limits for Synovus GPR activate >
-- JIRA ID:	<AL-3635>
-- =================================================================

DECLARE @GPRActivateLimitTypePK UNIQUEIDENTIFIER
DECLARE @ComplianceProgramPK UNIQUEIDENTIFIER

SELECT @ComplianceProgramPK = ComplianceProgramPK FROM tcomplianceprograms where name = 'SynovusCompliance'
SELECT @GPRActivateLimitTypePK = LimitTypePK FROM tlimittypes WHERE NAME = 'ActivateGPR' AND ComplianceProgramPK = @ComplianceProgramPK

UPDATE tlimits SET pertransactionminimum = 0.00 where limittypepk = @GPRActivateLimitTypePK
GO
