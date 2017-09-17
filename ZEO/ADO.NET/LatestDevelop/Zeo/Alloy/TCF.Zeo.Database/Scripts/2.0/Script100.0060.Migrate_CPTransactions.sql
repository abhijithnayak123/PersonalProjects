--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <07-20-2016>
-- Description:	 Migration scripts for Check transaction related tables.
-- Jira ID:		<AL-7705>
-- ================================================================================

--========================================tChxr_Account========================================================

-- Enable the trigger.
DISABLE TRIGGER trChxr_AccountAudit ON tChxr_Account
GO
DISABLE TRIGGER trChxr_TrxAudit ON  tChxr_Trx
GO

BEGIN TRY
BEGIN TRANSACTION 

UPDATE ca
  SET
    ca.CustomerId = pc.CXEId
  FROM 
    tChxr_Account ca
  INNER JOIN
    tAccounts a 
  ON
    ca.chxraccountid = a.cxnid
  INNER JOIN 
    tpartnerCustomers pc
  ON
    pc.CustomerPK = a.CustomerPK
  WHERE
    a.ProviderId IN (200,201) 



;WITH ct as 
(
  SELECT CustomerId, CustomerSessionId,
         ROW_NUMBER() OVER (PARTITION BY CustomerId ORDER BY DTServerCreate DESC) rowno
  FROM tCustomerSessions
)

UPDATE ca
 SET 
  ca.CustomerRevisionNo = isnull(cr.revisionno,1),
  ca.CustomerSessionId = ct.CustomerSessionId
 FROM 
   tChxr_Account ca
 INNER JOIN 
   (    
	 SELECT 
	   CustomerId,
	   max(RevisionNo) RevisionNo
	 FROM
	   tCustomers_Aud 
	 GROUP BY
	   CustomerId	
   ) AS cr 
   
  ON 
    cr.CustomerId = ca.CustomerId 
 INNER JOIN 
  ct
 ON 
  ct.CustomerId = ca.CustomerId
 WHERE
   ct.rowno = 1;




------------------------ttxn_check---------------------------------------

UPDATE tc
 SET
  tc.providerAccountId = c.ChxrAccountID, 
  tc.ProviderId = a.ProviderId
 FROM 
  tTxn_Check tc
 INNER JOIN 
  tAccounts a 
 ON
  tc.AccountPK = a.AccountPK
 INNER JOIN
  tChxr_Account c 
 ON
  c.ChxrAccountID = a.cxnId
  
  
UPDATE tc
 SET
  tc.customersessionid = cs.customersessionid
 FROM 
  tTxn_check tc
 INNER JOIN 
  tCustomerSessions cs 
 ON 
  tc.customersessionpk =cs.customersessionpk

  
UPDATE tc 
 SET 
  tc.MICR = b.MICR,
  tc.CheckType = b.Checktype
 FROM
  tTxn_check tc
 INNER JOIN 
  tTxn_Check_Stage b
 ON 
  tc.CXEId = b.CheckId

	
-- Update latest customer revisionNo for tTxn_check table 

;WITH cr AS 
(
  SELECT CustomerId, Max(RevisionNo) as CustomerRevisionNo FROM tCustomers_Aud GROUP BY CustomerID 
)

UPDATE tc
 SET
  tc.CustomerRevisionNo = cr.CustomerRevisionNo
 FROM 
  tTxn_check tc
 INNER JOIN 
  tCustomerSessions cs 
 ON 
  tc.customersessionpk = cs.customersessionpk
 INNER JOIN
  tCustomers AS c
 ON
  cs.CustomerId = c.CustomerId
 INNER JOIN 
  cr
 ON
  c.CustomerID = cr.CustomerID



--=========================tChxr_Trx==============================================
 


UPDATE ct
 SET
   ct.ChxrAccountID = ca.ChxrAccountID
 FROM 
   tChxr_Trx ct
 INNER JOIN 
   tChxr_Account ca 
 ON
   ct.ChxrAccountPK = ca.ChxrAccountPK
   

------UPDATE c
------  SET
------    c.TransactionId = tc.TransactionId
------  FROM
------    tChxr_Trx c
------  INNER JOIN
------    tTxn_Check tc ON tc.CXNId = c.ChxrTrxID


	 
--=====================tCheckImages=================================================

UPDATE ci
 SET 
   ci.TransactionId = tc.TransactionId
 FROM 
   tCheckImages ci 
 INNER JOIN 
   tTxn_Check_Stage tcs
 ON
   ci.checkpk = tcs.checkpk
 INNER JOIN
   tTxn_Check tc 
 ON
   tcs.checkId = tc.CXEId


 --=====================tMessageCenter --============================================

UPDATE mc
 SET
  mc.TransactionId = tc.TransactionId
 FROM 
  tMessageCenter mc 
 INNER JOIN 
  tTxn_Check tc
 ON 
  mc.TxnId = tc.TxnPK

--=====================tChxr_Session=================================================

UPDATE cs
 SET
  cs.ChxrPartnerId = cp.ChxrPartnerId
 FROM 
  tChxr_Session cs
 INNER JOIN 
  tChxr_Partner cp
 ON
  cs.ChxrPartnerPK = cp.ChxrPartnerPK



UPDATE ci
  SET 
   ci.ChxrSimAccountId = ca.ChxrSimAccountId
  FROM 
   tChxrSim_Invoice ci
  INNER JOIN 
   tChxrSim_Account ca
  ON 
   ci.ChxrSimAccountPK = ca.ChxrSimAccountPK


UPDATE ca
  SET
    ca.CustomerId = pc.CXEId     
  FROM
    tChxrSim_Account ca
  INNER JOIN
    tAccounts a 
  ON
    ca.ChxrSimAccountId = a.CXNId
  INNER JOIN
    tPartnerCustomers pc 
  ON
    pc.CustomerPK = a.CustomerPK
  WHERE 
    a.ProviderId in (200,201) ;


;WITH cs AS 
(
   SELECT
    CustomerId,
	CustomerSessionId,
    ROW_NUMBER() OVER (PARTITION BY customerId ORDER BY DTServerCreate DESC) rowno
   FROM 
    tCustomerSessions
)

UPDATE ca
 SET 
   ca.CustomerSessionId = cs.CustomerSessionId
 FROM
   tChxrSim_Account ca
 INNER JOIN cs
 ON 
   cs.CustomerId = ca.CustomerId 
 WHERE
   cs.rowno = 1;




 COMMIT
END TRY

BEGIN CATCH
	ROLLBACK
END CATCH 
GO

 
 
 -- enable the trigger

 ENABLE TRIGGER trChxr_AccountAudit ON tChxr_Account
 GO
 ENABLE TRIGGER trChxr_TrxAudit ON  tChxr_Trx
 GO



 --==============================ALTER new column as NOT NULL ================================================

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCheckImages' AND COLUMN_NAME = 'TransactionId' )
BEGIN

	ALTER TABLE tCheckImages 
	ALTER COLUMN TransactionId BIGINT NOT NULL 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Session' AND COLUMN_NAME = 'ChxrPartnerId')
BEGIN
	
	ALTER TABLE tChxr_Session 
	ALTER COLUMN ChxrPartnerId BIGINT NOT NULL 
END
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account' AND COLUMN_NAME IN ('CustomerId', 'CustomerSessionId', 'CustomerRevisionNo'))
BEGIN
	ALTER TABLE tChxr_Account 
	ALTER COLUMN CustomerId BIGINT NOT NULL

	ALTER TABLE tChxr_Account 
	ALTER COLUMN CustomerRevisionNo BIGINT NOT NULL 
		
	ALTER TABLE tChxr_Account 
	ALTER COLUMN CustomerSessionId BIGINT NULL 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Trx' AND COLUMN_NAME IN ('ChxrAccountId'))
BEGIN
	ALTER TABLE tChxr_Trx 
	ALTER COLUMN ChxrAccountId BIGINT NOT NULL    
	
	--ALTER TABLE tChxr_Trx 
	--ALTER COLUMN TransactionId BIGINT NOT NULL 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxrSim_Account' AND COLUMN_NAME IN ('CustomerSessionId', 'CustomerId'))
BEGIN
	
	ALTER TABLE tChxrSim_Account 
	ALTER COLUMN CustomerId BIGINT NOT NULL	    
	
	ALTER TABLE tChxrSim_Account 
	ALTER COLUMN CustomerSessionId BIGINT NOT NULL

END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_Check' AND COLUMN_NAME ='providerAccountId')
BEGIN
	ALTER TABLE tTxn_Check 
	ALTER COLUMN providerAccountId BIGINT NOT NULL    
END

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_Check' AND COLUMN_NAME ='ProviderId')
BEGIN
	ALTER TABLE tTxn_Check 
	ALTER COLUMN ProviderId INT NOT NULL    
END

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_Check' AND COLUMN_NAME ='CustomerSessionId')
BEGIN
	ALTER TABLE tTxn_Check 
	ALTER COLUMN CustomerSessionId BIGINT NOT NULL    
END