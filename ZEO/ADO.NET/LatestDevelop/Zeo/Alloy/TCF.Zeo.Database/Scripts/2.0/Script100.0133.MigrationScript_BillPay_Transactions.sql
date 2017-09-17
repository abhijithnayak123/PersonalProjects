--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <10-11-2016>
-- Description:	 Migration scripts for BillPay transaction related tables.
-- Jira ID:		<AL-8320>
-- ================================================================================


---------------------------tWUnion_BillPay_Account-----------------------------------
BEGIN TRY
	 BEGIN TRAN;

	 UPDATE ba
	 SET
		  ba.CustomerId = pc.CXEId
	 FROM tWUnion_BillPay_Account ba
			INNER JOIN tAccounts a ON ba.WUBillPayAccountID = a.cxnid
			INNER JOIN tpartnerCustomers pc ON pc.CustomerPK = a.CustomerPK
	 WHERE a.ProviderId IN(401, 405); -- 401-WU and 405 - MG

	 WITH ct
			AS (SELECT CustomerId,
						  CustomerSessionId,
						  ROW_NUMBER() OVER(PARTITION BY CustomerId ORDER BY DTServerCreate DESC) rowno
				 FROM tCustomerSessions)

			UPDATE ba
			SET
				 ba.CustomerRevisionNo = isnull(cr.revisionno, 1),
				 ba.CustomerSessionId = ct.CustomerSessionId
			FROM tWUnion_BillPay_Account ba
		    INNER JOIN
			(
				SELECT 
				  CustomerId,
				  MAX(RevisionNo) RevisionNo
				FROM tCustomers_Aud
				GROUP BY CustomerId
			) AS cr 
			
			ON cr.CustomerId = ba.CustomerId
				  INNER JOIN ct ON ct.CustomerId = ba.CustomerId
			WHERE ct.rowno = 1;

	 ------------------------------------------tTxn_BillPay--------------------------
	 UPDATE tb
	 SET
		  tb.providerAccountId = a.CXNId,
		  tb.ProviderId = a.ProviderId
	 FROM tTxn_BillPay tb
			INNER JOIN tAccounts a ON tb.AccountPK = a.AccountPK

	 UPDATE tb
	 SET
		  tb.customersessionid = cs.customersessionid
	 FROM tTxn_BillPay tb
			INNER JOIN tCustomerSessions cs ON tb.customersessionpk = cs.customersessionpk;

	 -- Update latest customer revisionNo for tTxn_BillPay table 
	 WITH cr
			AS (SELECT CustomerId,
						  MAX(RevisionNo) AS CustomerRevisionNo
				 FROM tCustomers_Aud
				 GROUP BY CustomerID)

			UPDATE tb
			SET
				 tb.CustomerRevisionNo = cr.CustomerRevisionNo
			FROM tTxn_BillPay tb
				  INNER JOIN tCustomerSessions cs ON tb.customersessionpk = cs.customersessionpk
				  INNER JOIN tCustomers AS c ON cs.CustomerId = c.CustomerId
				  INNER JOIN cr ON c.CustomerID = cr.CustomerID;

	 -----------------------update partnerCatalog table with the mastercatalogId-----------------------
	 UPDATE pc
	 SET
		  pc.MasterCatalogID = mc.MasterCatalogID
	 FROM tPartnerCatalog pc
			INNER JOIN tMasterCatalog mc ON mc.MasterCatalogPK = pc.MasterCatalogPK

	 -----------------------------tWUnion_BillPay_Trx-------------------------------

	 UPDATE wb
	 SET
		  wb.WUBillPayAccountID = ta.WUBillPayAccountID
	 FROM tWUnion_BillPay_Trx wb
		  INNER JOIN tWUnion_BillPay_Account ta ON wb.WUBillPayAccountPK = ta.WUBillPayAccountPK

	 --------------------------------tWUnion_ImportBillers--------------------------------------

	 UPDATE ib
	 SET
		  ib.WUBillPayAccountID = ba.WUBillPayAccountID
	 FROM dbo.tWUnion_ImportBillers ib
			INNER JOIN dbo.tWUnion_BillPay_Account ba ON ba.WUBillPayAccountPK = ib.WUBillPayAccountPK

		

--================================MIGRATION SCRIPTS FOR tNexxoIdTypes Table============================

---------------------- Updating the table tMasterCountries as we have did in Hotfix branch--------------
		UPDATE 
			tMasterCountries
		SET 
			NAME = 'CONGO REPUBLIC OF',
			Abbr2 = 'CF'
		WHERE 
			NAME = 'CONGO-REPUBLIC OF'

		UPDATE
			tMasterCountries
		SET 
			NAME = 'CONGO DEMOCRATIC REPUB',
			Abbr2 = 'CG'
		WHERE 
			NAME = 'CONGO-DEMOCRATIC REPUBLIC OF'
	
	-------------------- Migrating the Mastercountry and state id's in tNexxoIdTypes----------------------------
		UPDATE tnx
		SET
		 tnx.MasterCountriesID = tmc.MasterCountriesID
		 FROM dbo.tNexxoIdTypes tnx
		 INNER JOIN dbo.tMasterCountries tmc ON tmc.MasterCountriesPK = tnx.CountryPK
	 
		UPDATE tnx
		SET
			tnx.StateId = ts.StateId
			 FROM dbo.tNexxoIdTypes tnx
			 INNER JOIN dbo.tStates ts ON ts.StatePK = tnx.StatePK;  

	 
		--------------------Migrating the Mastercountry Id tChannelPartnerMasterCountryMapping----------------------------	 
		UPDATE tcpm
		SET
			 tcpm.MasterCountriesId = tmc.MasterCountriesID
		FROM dbo.tChannelPartnerMasterCountryMapping tcpm
			  INNER JOIN dbo.tMasterCountries tmc ON tmc.MasterCountriesPK = tcpm.MasterCountryId;	

		---Migrating the data into tStates where country names are equal from tMasterCountries table-----------------------
		UPDATE tStates
		SET
			 MasterCountriesID = m.MasterCountriesID
		FROM tCountries C
			  INNER JOIN tMasterCountries M ON M.Name = C.Name
			  INNER JOIN tStates S ON C.CountryPK = S.CountryPK 

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

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_BillPay_Account' AND COLUMN_NAME IN ('CustomerId', 'CustomerSessionId', 'CustomerRevisionNo'))
BEGIN
	ALTER TABLE tWUnion_BillPay_Account 
	ALTER COLUMN CustomerId BIGINT NOT NULL

	ALTER TABLE tWUnion_BillPay_Account 
	ALTER COLUMN CustomerRevisionNo BIGINT NOT NULL 

	ALTER TABLE tWUnion_BillPay_Account 
	ALTER COLUMN CustomerSessionId BIGINT NOT NULL 
END

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_BillPay_Trx' AND COLUMN_NAME = 'WUBillPayAccountID')
BEGIN
ALTER TABLE tWUnion_BillPay_Trx 
	ALTER COLUMN WUBillPayAccountID BIGINT NOT NULL    
END

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tPartnerCatalog' AND COLUMN_NAME ='MasterCatalogID')
BEGIN
	ALTER TABLE tPartnerCatalog 
	ALTER COLUMN MasterCatalogID BIGINT NOT NULL    
END

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_BillPay' AND COLUMN_NAME ='CustomerSessionId')
BEGIN
	ALTER TABLE tTxn_BillPay 
	ALTER COLUMN CustomerSessionId BIGINT NOT NULL    
END

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_BillPay' AND COLUMN_NAME ='providerAccountId')
BEGIN
	ALTER TABLE tTxn_BillPay 
	ALTER COLUMN providerAccountId BIGINT NOT NULL    
END

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_BillPay' AND COLUMN_NAME ='ProviderId')
BEGIN
	ALTER TABLE tTxn_BillPay 
	ALTER COLUMN ProviderId INT NOT NULL    
END

-----Making the required columns not nullable-------------------

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tNexxoIdTypes' AND COLUMN_NAME ='MasterCountriesID')
BEGIN
ALTER TABLE tNexxoIdTypes 
	ALTER COLUMN MasterCountriesID BIGINT NOT NULL    
END

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tStates' AND COLUMN_NAME ='MasterCountriesID')
BEGIN
	ALTER TABLE tStates 
	ALTER COLUMN MasterCountriesID BIGINT NOT NULL    
END

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerMasterCountryMapping' AND COLUMN_NAME ='MasterCountriesId')
BEGIN
	ALTER TABLE tChannelPartnerMasterCountryMapping 
	ALTER COLUMN MasterCountriesID BIGINT NOT NULL    
END
