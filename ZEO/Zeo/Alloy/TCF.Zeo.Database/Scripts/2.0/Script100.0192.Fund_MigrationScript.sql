--- ===============================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <10-19-2016>
-- Description:	As an engineer, I want to implement ADO.Net for GPR - Database changes
-- Jira ID:		<AL-8323>
-- ================================================================================

BEGIN TRY
	BEGIN TRAN;
	
	UPDATE 
		tvt
	SET 
		VisaAccountId = tva.VisaAccountID
	FROM 
		dbo.tVisa_Trx tvt
		INNER JOIN dbo.tVisa_Account tva ON  tva.VisaAccountPK = tvt.AccountPK


	ALTER TABLE tVisa_Trx 
	ALTER COLUMN VisaAccountId BIGINT NOT NULL

	IF NOT EXISTS (
			SELECT 1 
			FROM sys.foreign_keys 
			WHERE object_id = OBJECT_ID(N'[dbo].[FK_tVisa_Trx_tVisa_Account]') 
			AND parent_object_id = OBJECT_ID(N'[dbo].[tVisa_Account]'))
	BEGIN
		ALTER TABLE 
			tVisa_Trx  
		ADD CONSTRAINT 
			FK_tVisa_Trx_tVisa_Account 
		FOREIGN KEY
			(VisaAccountId)
		REFERENCES 
			tVisa_Account(VisaAccountID)	
	END

		UPDATE 
			tVisa_Fee
		SET 
			ChannelPartnerFeeTypeId = VCF.ChannelPartnerFeeTypeId
		FROM 
			tVisa_Fee VF
			INNER JOIN tVisa_ChannelPartnerFeeTypeMapping VCF ON VCF.ChannelPartnerFeeTypeMappingPK = VF.ChannelPartnerFeeTypePK


		ALTER TABLE tVisa_Fee 
		ALTER COLUMN ChannelPartnerFeeTypeId BIGINT NOT NULL

		IF NOT EXISTS (
				SELECT 1 
				FROM sys.foreign_keys 
				WHERE object_id = OBJECT_ID(N'[dbo].[FK_tVisa_Fee_tVisa_ChannelPartnerFeeTypeMapping]') 
				AND parent_object_id = OBJECT_ID(N'[dbo].[tVisa_Fee]'))
		BEGIN
			ALTER TABLE 
				tVisa_Fee  
			WITH CHECK ADD CONSTRAINT 
				FK_tVisa_Fee_tVisa_ChannelPartnerFeeTypeMapping 
			FOREIGN KEY
				(ChannelPartnerFeeTypeId)
			REFERENCES 
				tVisa_ChannelPartnerFeeTypeMapping(ChannelPartnerFeeTypeId)	
		END

		UPDATE 
			tVisa_ChannelPartnerFeeTypeMapping
		SET 
			VisaFeeTypeId = VF.VisaFeeTypeId
		FROM 
			tVisa_ChannelPartnerFeeTypeMapping VFM
			INNER JOIN tVisa_FeeTypes VF ON VF.VisaFeeTypePK = VFM.VisaFeeTypePK


		ALTER TABLE tVisa_ChannelPartnerFeeTypeMapping 
		ALTER COLUMN VisaFeeTypeId BIGINT NOT NULL

		IF NOT EXISTS (
				SELECT 1 
				FROM sys.foreign_keys 
				WHERE object_id = OBJECT_ID(N'[dbo].[FK_tVisa_ChannelPartnerFeeTypeMapping_tVisa_FeeTypes]') 
				AND parent_object_id = OBJECT_ID(N'[dbo].[tVisa_ChannelPartnerFeeTypeMapping]'))
		BEGIN
			ALTER TABLE 
				tVisa_ChannelPartnerFeeTypeMapping  
			WITH CHECK ADD CONSTRAINT 
				FK_tVisa_ChannelPartnerFeeTypeMapping_tVisa_FeeTypes 
			FOREIGN KEY
				(VisaFeeTypeId)
			REFERENCES 
				tVisa_FeeTypes(VisaFeeTypeId)	
		END

		UPDATE 
			tVisa_ChannelPartnerShippingTypeMapping
		SET 
			CardShippingTypeId = CST.CardShippingTypeId
		FROM 
			tVisa_ChannelPartnerShippingTypeMapping CSTM
			INNER JOIN tVisa_CardShippingTypes CST ON CST.ShippingTypePK = CSTM.ShippingTypePK


		ALTER TABLE tVisa_ChannelPartnerShippingTypeMapping 
		ALTER COLUMN CardShippingTypeId BIGINT NOT NULL

		IF NOT EXISTS (
				SELECT 1 
				FROM sys.foreign_keys 
				WHERE object_id = OBJECT_ID(N'[dbo].[FK_tVisa_ChannelPartnerShippingTypeMapping_tVisa_CardShippingTypes]') 
				AND parent_object_id = OBJECT_ID(N'[dbo].[tVisa_ChannelPartnerShippingTypeMapping]'))
		BEGIN
			ALTER TABLE 
				tVisa_ChannelPartnerShippingTypeMapping  
			WITH CHECK ADD CONSTRAINT 
				FK_tVisa_ChannelPartnerShippingTypeMapping_tVisa_CardShippingTypes 
			FOREIGN KEY
				(CardShippingTypeId)
			REFERENCES 
				tVisa_CardShippingTypes(CardShippingTypeId)	
		END

		UPDATE 
			tVisa_ShippingFee
		SET 
			ChannelPartnerShippingTypeId = VCP.ChannelPartnerShippingTypeId
		FROM 
			tVisa_ShippingFee VS
			INNER JOIN tVisa_ChannelPartnerShippingTypeMapping VCP ON VCP.ChannelPartnerShippingTypePK = VS.ChannelPartnerShippingTypePK

	
		ALTER TABLE tVisa_ShippingFee 
		ALTER COLUMN ChannelPartnerShippingTypeId BIGINT NOT NULL

		IF NOT EXISTS (
				SELECT 1 
				FROM sys.foreign_keys 
				WHERE object_id = OBJECT_ID(N'[dbo].[FK_tVisa_ShippingFee_tVisa_ChannelPartnerShippingTypeMapping]') 
				AND parent_object_id = OBJECT_ID(N'[dbo].[tVisa_ShippingFee]'))
		BEGIN
			ALTER TABLE 
				tVisa_ShippingFee  
			WITH CHECK ADD CONSTRAINT 
				FK_tVisa_ShippingFee_tVisa_ChannelPartnerShippingTypeMapping 
			FOREIGN KEY
				(ChannelPartnerShippingTypeId)
			REFERENCES 
				tVisa_ChannelPartnerShippingTypeMapping(ChannelPartnerShippingTypeId)	
		END

		;WITH ct AS 
		(
		  SELECT CustomerId, CustomerSessionId,
				 ROW_NUMBER() OVER (PARTITION BY CustomerId ORDER BY DTServerCreate DESC) rowno
		  FROM tCustomerSessions
		)


		UPDATE 
			tVisa_Account
		SET 
			CustomerID = C.CustomerID,
			CustomerRevisionNo = CR.RevisionNo,
			CustomerSessionId = CT.CustomerSessionID
		FROM 
			tVisa_Account VA
			INNER JOIN tAccounts A ON VA.VisaAccountID = A.CXNId
			INNER JOIN tCustomerAccounts PC ON PC.AccountID = A.CXEId
			INNER JOIN tCustomers C ON C.CustomerPK = PC.CustomerPK
			INNER JOIN ( SELECT CustomerId, MAX(RevisionNo) RevisionNo FROM  tCustomers_Aud GROUP BY CustomerId) AS cr ON CR.CustomerID = C.CustomerID
			INNER JOIN tPartnerCustomers p on a.CustomerPK = p.CustomerPK
			INNER JOIN tCustomerSessions cs on cs.CustomerPK = p.CustomerPK
			INNER JOIN CT ON ct.CustomerID = c.CustomerID
		WHERE 
			A.ProviderId = 103 AND ct.rowno = 1;


		ALTER TABLE tVisa_Account 
		ALTER COLUMN CustomerID BIGINT NOT NULL 

		ALTER TABLE tVisa_Account 
		ALTER COLUMN CustomerSessionId BIGINT NOT NULL 

		IF NOT EXISTS (
				SELECT 1 
				FROM sys.foreign_keys 
				WHERE object_id = OBJECT_ID(N'[dbo].[FK_tVisa_Account_tCustomers]') 
				AND parent_object_id = OBJECT_ID(N'[dbo].[tVisa_Account]'))
		BEGIN
			ALTER TABLE 
				tVisa_Account  
			WITH CHECK ADD CONSTRAINT 
				FK_tVisa_Account_tCustomers 
			FOREIGN KEY
				(CustomerID)
			REFERENCES 
				tCustomers(CustomerID)	
		END

		IF NOT EXISTS (
				SELECT 1 
				FROM sys.foreign_keys 
				WHERE object_id = OBJECT_ID(N'[dbo].[FK_tVisa_Account_tCustomerSessions]') 
				AND parent_object_id = OBJECT_ID(N'[dbo].[tVisa_Account]'))
		BEGIN
			ALTER TABLE 
				tVisa_Account  
			WITH CHECK ADD CONSTRAINT 
				FK_tVisa_Account_tCustomerSessions 
			FOREIGN KEY
				(CustomerSessionId)
			REFERENCES 
				tCustomerSessions(CustomerSessionId)	
		END

		UPDATE tf
		 SET
		  tf.ProviderAccountId = v.VisaAccountID, 
		  tf.ProviderId = a.ProviderId
		 FROM 
		  tTxn_Funds tf
		 INNER JOIN 
		  tAccounts a 
		 ON
		  tf.AccountPK = a.AccountPK
		 INNER JOIN
		  tVisa_Account v 
		 ON
		  v.VisaAccountID = a.cxnId


		UPDATE tf
		SET
			tf.CustomerSessionId = cs.CustomerSessionID
		FROM 
			tTxn_Funds tf
		INNER JOIN 
			tCustomerSessions cs ON tf.CustomerSessionPK = cs.CustomerSessionPK


		;WITH cr AS 
		(
		  SELECT CustomerId, Max(RevisionNo) as CustomerRevisionNo FROM tCustomers_Aud GROUP BY CustomerID 
		)

		UPDATE tf
		 SET
		  tf.CustomerRevisionNo = cr.CustomerRevisionNo
		 FROM 
		  tTxn_Funds tf
		 INNER JOIN 
		  tCustomerSessions cs 
		 ON 
		  tf.customersessionpk = cs.customersessionpk
		 INNER JOIN
		  tCustomers AS c
		 ON
		  cs.CustomerId = c.CustomerId
		 INNER JOIN 
		  cr
		 ON
		  c.CustomerID = cr.CustomerID


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