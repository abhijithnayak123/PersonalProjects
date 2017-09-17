--- ===============================================================================
-- Author:		<Abhijith Nayak>
-- Create date: <10-26-2016>
-- Description:	Migration script for MoneyTransfer related tables
-- Jira ID:		<AL-8324>
-- ================================================================================

--============ Migration script for Money Transfer transaction related tables=======================
--================ Starts Here =========================================================================

--DISABLE TRIGGER tWUnion_TrxAud ON tWUnion_Trx
--GO

BEGIN TRY
BEGIN TRAN

------------------------------------ tWUnion_Account --------------------------------------------
	  UPDATE wa
	  SET wa.CustomerId = pc.CXEId
	  FROM tWUnion_Account wa
		INNER JOIN tAccounts a ON wa.WUAccountID = a.cxnid
		INNER JOIN tpartnerCustomers pc ON pc.CustomerPK = a.CustomerPK
	  WHERE
		a.ProviderId IN (301,302) 


	;WITH wt as 
	(
	  SELECT CustomerId, CustomerSessionId,
			 ROW_NUMBER() OVER (PARTITION BY CustomerId ORDER BY DTServerCreate DESC) rowno
	  FROM tCustomerSessions
	)

	UPDATE wa
	 SET 
	  wa.CustomerRevisionNo = ISNULL(cr.revisionno,1),
	  wa.CustomerSessionId = wt.CustomerSessionId
	 FROM tWUnion_Account wa
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
		   ON cr.CustomerId = wa.CustomerId 
		 INNER JOIN wt ON wt.CustomerId = wa.CustomerId
	 WHERE
	   wt.rowno = 1;

------------------------------------ tWUnion_Account --------------------------------------------

-------------------------------------tTxn_MoneyTransfer------------------------------------------

	 UPDATE mt
	 SET
	  mt.providerAccountId = w.WUAccountID, 
	  mt.ProviderId = a.ProviderId
	 FROM 
	  tTxn_MoneyTransfer mt
		INNER JOIN tAccounts a ON mt.AccountPK = a.AccountPK
		INNER JOIN tWUnion_Account w ON w.WUAccountID = a.cxnId


	 UPDATE mt
	 SET
	  mt.customersessionid = cs.customersessionid
	 FROM 
	  tTxn_MoneyTransfer mt
		INNER JOIN tCustomerSessions cs ON mt.customersessionpk =cs.customersessionpk


	;WITH cr AS 
	(
	  SELECT CustomerId, Max(RevisionNo) as CustomerRevisionNo FROM tCustomers_Aud GROUP BY CustomerID 
	)

	UPDATE mt
	 SET
	  mt.CustomerRevisionNo = cr.CustomerRevisionNo
	 FROM 
	  tTxn_MoneyTransfer mt
		INNER JOIN tCustomerSessions cs ON mt.customersessionpk = cs.customersessionpk
		INNER JOIN tCustomers c ON cs.CustomerId = c.CustomerId
		INNER JOIN cr ON c.CustomerID = cr.CustomerID


	 UPDATE mt
	 SET
	  mt.Destination = mts.Destination
	 FROM 
	  tTxn_MoneyTransfer mt
		INNER JOIN tTxn_MoneyTransfer_Stage mts ON mt.CXEId = mts.MoneyTransferID


	 UPDATE mt
	 SET
	  mt.[State] = mts.[Status]
	 FROM 
	  tTxn_MoneyTransfer mt
		INNER JOIN tTxn_MoneyTransfer_Stage mts ON mt.CXEId = mts.MoneyTransferID
	 
-------------------------------------tTxn_MoneyTransfer------------------------------------------
-------------------------------------tWUnion_Trx------------------------------------------

	 UPDATE wt
	 SET
	   wt.WUAccountID = wa.WUAccountID
	 FROM 
	   tWUnion_Trx wt
		INNER JOIN tWUnion_Account wa ON wt.WUAccountPK = wa.WUAccountPK
   

	 -- UPDATE wt
	 -- SET
		--wt.TransactionId = mt.TransactionId
	 -- FROM
		--tWUnion_Trx wt
		--INNER JOIN tTxn_MoneyTransfer mt ON mt.CXNId = wt.WUTrxID


	  UPDATE wt
	  SET
		wt.WUReceiverID = wr.WUReceiverID
	  FROM
		tWUnion_Trx wt
		INNER JOIN tWUnion_Receiver wr ON wt.WUnionRecipientAccountPK = wr.WUReceiverPK

	-------------------------------------tWUnion_Trx------------------------------------------

	IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Account' AND COLUMN_NAME = 'CustomerId')
	BEGIN
		ALTER TABLE tWUnion_Account 
		ALTER COLUMN CustomerId BIGINT NOT NULL
	END

	IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Account' AND COLUMN_NAME = 'CustomerSessionId')
	BEGIN
		ALTER TABLE tWUnion_Account 
		ALTER COLUMN CustomerSessionId BIGINT NOT NULL
	END

	IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_MoneyTransfer' AND COLUMN_NAME = 'CustomerSessionId')
	BEGIN
		ALTER TABLE tTxn_MoneyTransfer 
		ALTER COLUMN CustomerSessionId BIGINT NOT NULL	
	END

	IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_MoneyTransfer' AND COLUMN_NAME = 'ProviderId')
	BEGIN
		ALTER TABLE tTxn_MoneyTransfer 
		ALTER COLUMN ProviderId BIGINT NOT NULL	
	END

	IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_MoneyTransfer' AND COLUMN_NAME = 'ProviderAccountId')
	BEGIN
		ALTER TABLE tTxn_MoneyTransfer 
		ALTER COLUMN ProviderAccountId BIGINT NOT NULL	
	END

	IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Trx' AND COLUMN_NAME = 'WUAccountID')
	BEGIN
		ALTER TABLE tWUnion_Trx 
		ALTER COLUMN WUAccountID BIGINT NOT NULL	
	END

	IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Trx' AND COLUMN_NAME = 'WUAccountPK')
	BEGIN
		ALTER TABLE tWUnion_Trx 
		ALTER COLUMN WUAccountPK UNIQUEIDENTIFIER NULL	
	END

	--IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Trx' AND COLUMN_NAME = 'TransactionID')
	--BEGIN
	--	ALTER TABLE tWUnion_Trx 
	--	ALTER COLUMN TransactionID BIGINT NOT NULL	
	--END

	IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Trx' AND COLUMN_NAME = 'WUReceiverID')
	BEGIN
		ALTER TABLE tWUnion_Trx 
		ALTER COLUMN WUReceiverID BIGINT NULL	
	END

	--IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'UNIQUE_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tWUnion_Trx' AND OBJECT_NAME(OBJECT_ID) = 'UC_tWUnion_Trx')
	--BEGIN
	--	ALTER TABLE [dbo].tWUnion_Trx 
	--	ADD  CONSTRAINT [UC_tWUnion_Trx] UNIQUE NONCLUSTERED 
	--	(
	--		[TransactionID] 
	--	)

	--END

	IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_PickupDetails' AND COLUMN_NAME = 'WUAccountID')
	BEGIN
		ALTER TABLE tWUnion_PickupDetails 
		ALTER COLUMN WUAccountID BIGINT NOT NULL	
	END

	COMMIT TRAN
END TRY

BEGIN CATCH
	 IF(@@TRANCOUNT > 0)
		SELECT
		ERROR_NUMBER() AS ErrorNumber
		,ERROR_SEVERITY() AS ErrorSeverity
		,ERROR_STATE() AS ErrorState
		,ERROR_PROCEDURE() AS ErrorProcedure
		,ERROR_LINE() AS ErrorLine
		,ERROR_MESSAGE() AS ErrorMessage,
		XACT_STATE()as state;
		ROLLBACK TRAN
END CATCH;

--ENABLE TRIGGER tWUnion_TrxAud ON tWUnion_Trx
--GO




--============================== Ends Here =========================================================================