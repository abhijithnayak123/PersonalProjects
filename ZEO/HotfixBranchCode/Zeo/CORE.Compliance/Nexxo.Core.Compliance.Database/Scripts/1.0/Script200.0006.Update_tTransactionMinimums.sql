--===========================================================================================
-- Auther:			Sunil Shetty
-- Date Created:	10/07/2014
-- Description:		Script for Updating tTransactionMinimums minimum to 0
--===========================================================================================

DECLARE @CompliancePK UNIQUEIDENTIFIER

SELECT 
	@CompliancePK = rowguid 
FROM 
	tCompliancePrograms 
WHERE
	Name = 'SynovusCompliance'

UPDATE 
	tTransactionMinimums 
SET 
	Minimum = '0.00', 
	DTLastMod = GETDATE() 
WHERE 
	ComplianceProgramPK = @CompliancePK 
	AND TransactionType = 9

GO
