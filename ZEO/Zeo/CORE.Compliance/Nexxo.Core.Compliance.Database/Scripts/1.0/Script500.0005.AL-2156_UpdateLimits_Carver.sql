-- ================================================================
-- Author:		Karun
-- Create date: <01/Oct/2015>
-- Description:	<As Carver, reduce the minimum Money Order to $1 from $10.>
-- JIRA ID:	<AL-2156>
-- =================================================================

DECLARE @CarverComplianceId UNIQUEIDENTIFIER
DECLARE @LimitTypePK UNIQUEIDENTIFIER

SELECT @CarverComplianceId= ComplianceProgramPK FROM tCompliancePrograms where Name ='CarverCompliance'
SELECT @LimitTypePK = LimitTypePK from tLimitTypes WHERE ComplianceProgramPK = @CarverComplianceId AND Name = 'MoneyOrder'

UPDATE tLimits SET PerTransactionMinimum = 1.00, DTServerLastModified = GETDATE() WHERE LimitTypePK = @LimitTypePK
GO