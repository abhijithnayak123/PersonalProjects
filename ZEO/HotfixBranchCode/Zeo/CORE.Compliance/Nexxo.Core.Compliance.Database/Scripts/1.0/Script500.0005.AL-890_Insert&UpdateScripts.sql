--- ================================================================================
-- Author:		<Divya>
-- Create date: <07/30/2015>
-- Description:	<Send Money- Minimum limit ($1) to transfer amount is not working>
--               Minimum Transaction Amounts for TcfCompliance only
--               ==================================================
--                    MoneyOrder=$10.00 
--                    MoneyTransfer=$1.00
--                    Check=$3
--                    Billpay=$10
-- Jira ID:		<AL-890>
-- ================================================================================

DECLARE @TCFLimitTypePK UNIQUEIDENTIFIER
DECLARE @TCFLimitTypePK1 UNIQUEIDENTIFIER
DECLARE @TCFLimitTypePK2 UNIQUEIDENTIFIER
DECLARE @TCFLimitTypePK3 UNIQUEIDENTIFIER
DECLARE @TCF_COMP_PROG_PK UNIQUEIDENTIFIER

SELECT @TCF_COMP_PROG_PK = ComplianceProgramPK   FROM tCompliancePrograms WHERE Name ='TCFCompliance'

--Inserting MoneyTransfer record for TCF compliance in tLimits
SELECT @TCFLimitTypePK = LimitTypePK  FROM tLimitTypes WHERE ComplianceProgramPK = @TCF_COMP_PROG_PK and Name='MoneyTransfer'
  IF NOT EXISTS(SELECT 1 FROM tLimits WHERE LimitTypePK = @TCFLimitTypePK)
  BEGIN
		INSERT INTO tLimits(LimitPK,LimitTypePK,PerTransactionMaximum,PerTransactionMinimum,RollingLimits,DTServerCreate)
		VALUES
		(NEWID(),@TCFLimitTypePK,NULL,1,NULL,GETDATE())
  END

--Inserting Check record for TCF compliance in tLimits
SELECT @TCFLimitTypePK1 = LimitTypePK  FROM tLimitTypes WHERE ComplianceProgramPK = @TCF_COMP_PROG_PK and Name='Check'

IF NOT EXISTS(SELECT 1 FROM tLimits WHERE LimitTypePK = @TCFLimitTypePK1)
BEGIN
        INSERT INTO tLimits(LimitPK,LimitTypePK,PerTransactionMaximum,PerTransactionMinimum,RollingLimits,DTServerCreate)
        VALUES
        (NEWID(),@TCFLimitTypePK1,NULL,3,NULL,GETDATE())
END

--Inserting BillPay record for TCF compliance in tLimits

SELECT @TCFLimitTypePK2 = LimitTypePK  FROM tLimitTypes WHERE ComplianceProgramPK = @TCF_COMP_PROG_PK and Name='BillPay'
IF NOT EXISTS(SELECT 1 FROM tLimits WHERE LimitTypePK = @TCFLimitTypePK2)
BEGIN
		 INSERT INTO tLimits(LimitPK,LimitTypePK,PerTransactionMaximum,PerTransactionMinimum,RollingLimits,DTServerCreate)
		VALUES
		(NEWID(),@TCFLimitTypePK2,NULL,10,NULL,GETDATE())
END

--Update MoneyOrder Minlimit for TCFCompliance in tLimits
SELECT @TCFLimitTypePK3 = LimitTypePK  FROM tLimitTypes WHERE ComplianceProgramPK = @TCF_COMP_PROG_PK and Name='MoneyOrder'

IF NOT EXISTS(SELECT 1 FROM tLimits WHERE LimitTypePK = @TCFLimitTypePK3 and PerTransactionMinimum=10)
BEGIN 
		UPDATE tLimits SET PerTransactionMinimum=10 
		WHERE LimitTypePK=@TCFLimitTypePK3
END 