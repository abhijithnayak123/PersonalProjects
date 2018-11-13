-- ================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <12/09/2015>
-- Description:	<As Synovus, Configure limits and fees for VISA DPS Prepaid cards>
-- Jira ID:		<AL-1632>
-- ================================================================================

UPDATE l
SET PerTransactionMinimum = 20.00
FROM tLimits l 
	INNER JOIN tLimitTypes lt ON l.LimitTypePK = lt.LimitTypePK
	INNER JOIN tCompliancePrograms cp ON lt.ComplianceProgramPK = cp.ComplianceProgramPK
WHERE lt.NAME IN ('LoadToGPR') AND cp.Name = 'SynovusCompliance'


UPDATE l
SET PerTransactionMinimum = 0.00
FROM tLimits l 
	INNER JOIN tLimitTypes lt ON l.LimitTypePK = lt.LimitTypePK
	INNER JOIN tCompliancePrograms cp ON lt.ComplianceProgramPK = cp.ComplianceProgramPK
WHERE lt.NAME IN ('ActivateGPR') AND cp.Name = 'SynovusCompliance'
