BEGIN TRY
	BEGIN TRAN;

		IF NOT EXISTS (
					SELECT 1 
					FROM INFORMATION_SCHEMA.COLUMNS 
					WHERE TABLE_NAME = 'tCustomerSessions' 
					AND COLUMN_NAME = 'CustomerID'
			)
			BEGIN
				ALTER TABLE 
					tCustomerSessions 
				ADD 
					CustomerID BIGINT NULL
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
GO

------------------------------Migration Script------------------------------
BEGIN TRY
	BEGIN TRAN;

		UPDATE 
			tCustomerSessions
		SET 
			CustomerID = C.CustomerID
		FROM 
			tCustomers C  
			INNER JOIN tPartnerCustomers PC ON PC.CustomerID = C.CustomerID 
			INNER JOIN tCustomerSessions CS ON CS.CustomerPK = PC.CustomerPK


		IF EXISTS(
				SELECT 1 
				FROM INFORMATION_SCHEMA.COLUMNS 
				WHERE TABLE_NAME = 'tCustomerSessions' 
				AND COLUMN_NAME IN ('AgentSessionId')
		)
		BEGIN
			UPDATE cs
			SET AgentSessionId = tas.agentsessionid
			FROM tCustomerSessions cs 
			INNER JOIN tAgentSessions tas ON tas.agentsessionpk = cs.agentsessionpk

		END

		UPDATE
			tCustomerSessionCounterIdDetails
		SET
			CustomerSessionID = C.CustomerSessionID
		FROM 
			tCustomerSessions C
			INNER JOIN tCustomerSessionCounterIdDetails CS ON CS.CustomerSessionPK = C.CustomerSessionPK
		WHERE 
			C.CustomerSessionPK = CS.CustomerSessionPK

	
		UPDATE
			tCustomerSessionShoppingCarts
		SET
			CustomerSessionID = C.CustomerSessionID
		FROM 
			tCustomerSessions C
			INNER JOIN tCustomerSessionShoppingCarts CSS ON CSS.CustomerSessionPK = C.CustomerSessionPK
		WHERE 
			C.CustomerSessionPK = CSS.CustomerSessionPK


		--------------------------Migrating the CIS related tales Data---------------------------

		UPDATE 
			tTCIS_Account
		SET		
			CustomerID = t.CID,
			CustomerSessionID = t.CSID
		FROM 
			(
			SELECT 
				C.CustomerID AS CID ,CS.CustomerSessionID as CSID, t.TCISAccountID as AccountID
			FROM
				tCustomers c 
				LEFT JOIN tPartnerCustomers pc on pc.CustomerID = c.CustomerID 
				LEFT JOIN tCustomerSessions cs on pc.CustomerPK = cs.CustomerPK 
					AND cs.CustomerSessionPK =
						(
							SELECT  TOP 1 cts.CustomerSessionPK
							FROM    tCustomerSessions cts
							WHERE   cts.CustomerPK  = cs.CustomerPK
							ORDER BY cts.DtServerCreate DESC
						)
				LEFT JOIN tAccounts a on a.CustomerPK= pc.CustomerPK 
				INNER JOIN tTCIS_Account t on t.TCISAccountID = a.CXNId
			WHERE 
				ProviderId = 602
		) t INNER JOIN tTCIS_Account ta ON ta.TCISAccountID = t.AccountID

		 
		--------------Update employment and govt. id details-------------------------------

		UPDATE cust 
		SET cust.[Occupation]=e.[Occupation]
			  ,cust.[Employer]=e.[Employer]
			  ,cust.[EmployerPhone]=e.[EmployerPhone]
			  ,cust.[OccupationDescription]=e.[OccupationDescription]
			  ,cust.GovtIdTypeId =  g.[IdTypeId]
			  ,cust.GovtIdentification  =g.[Identification]
			  ,cust.GovtIDExpirationDate=g.[ExpirationDate]
			  ,cust.GovtIdIssueDate=g.[IssueDate]
		FROM [dbo].[tCustomers] as cust
		  LEFT OUTER JOIN [dbo].tCustomerEmploymentDetails e on cust.customerpk=e.customerpk
		  LEFT OUTER JOIN [dbo].tCustomergovernmentidDetails g on cust.customerpk=g.customerpk

		--------------Update employment and govt. id details-----------------------------------
  

		-------------------Update details from tPartnerCustomers---------------------------------

		 ;With PCs as
		(
			SELECT [CXEID]    
			  ,[IsPartnerAccountHolder]
			  ,[ReferralCode]
			  ,ISNULL(ase.[AgentSessionID] , 0) AS AgentSessionID  
			FROM [dbo].[tPartnerCustomers] pc
			LEFT OUTER JOIN  [dbo].[tAgentSessions] ase on pc.agentsessionpk=ase.agentsessionpk
		)   

		UPDATE cust
		SET cust.[IsPartnerAccountHolder] =pcs.[IsPartnerAccountHolder] ,
			cust.[ReferralCode] = pcs.[ReferralCode],      
			cust.AgentSessionID =	 pcs.AgentSessionID
			,cust.LastUpdatedAgentSessionID = pcs.AgentSessionID
		FROM [dbo].[tCustomers] as cust
		  LEFT OUTER JOIN pcs on cust.customerid = pcs.cXEID


		-------------------Update details from tPartnerCustomers---------------------------------

		----------------------Update group 1 from tPartnerCustomerGroupSetting with the earliest row for the customer--------

		 ;With PCs as
		(  
			SELECT PartnerCustomerPK, [ChannelPartnerGroupId],a.DTTerminalCreate , b.cxeid      ,
					  ROW_NUMBER() OVER( PARTITION BY PartnerCustomerPK ORDER BY a.DTTerminalCreate) AS rownumm
			FROM [dbo].[tPartnerCustomerGroupSettings] a
				LEFT OUTER JOIN tPartnerCustomers b on  a.[PartnerCustomerPK]= b.[CustomerPK]
		) 

		UPDATE cust
		SET cust.[Group1]= pcs.[ChannelPartnerGroupId]       
		FROM [dbo].[tCustomers] as cust
		  LEFT OUTER JOIN pcs on cust.customerid = pcs.cXEID
		WHERE pcs.rownumm=1;

		----------------------Update group 1 from tPartnerCustomerGroupSetting with the earliest row for the customer--------

		 ----------------------Update group 2 from tPartnerCustomerGroupSetting with the second row for the customer --------

		 With PCs as
		(  
			SELECT PartnerCustomerPK, [ChannelPartnerGroupId],a.DTTerminalCreate , b.cxeid      ,
					  ROW_NUMBER() OVER( PARTITION BY PartnerCustomerPK ORDER BY a.DTTerminalCreate) AS rownumm
			FROM [dbo].[tPartnerCustomerGroupSettings] a
				LEFT OUTER JOIN tPartnerCustomers b on  a.[PartnerCustomerPK]= b.[CustomerPK]
		) 

		UPDATE cust
		SET cust.[Group2]= pcs.[ChannelPartnerGroupId]       
		FROM [dbo].[tCustomers] as cust
		  LEFT OUTER JOIN pcs on cust.customerid = pcs.cXEID
		WHERE pcs.rownumm=2;

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
GO
 ----------------------Update group 2 from tPartnerCustomerGroupSetting with the second row for the customer --------


BEGIN TRY
	BEGIN TRAN;
		
		IF EXISTS (
				SELECT 1 
				FROM INFORMATION_SCHEMA.COLUMNS 
				WHERE TABLE_NAME = 'tCustomerSessions' 
				AND COLUMN_NAME = 'CustomerID'
		)
		BEGIN
			ALTER TABLE tCustomerSessions 
			ALTER COLUMN CustomerID BIGINT NOT NULL
		END

		
			IF NOT EXISTS (
				SELECT 1 
				FROM sys.foreign_keys 
				WHERE object_id = OBJECT_ID(N'[dbo].[FK_tCustomer_tCustomerSessions]') 
				AND parent_object_id = OBJECT_ID(N'[dbo].[tCustomerSessions]'))
		BEGIN
			ALTER TABLE 
				tCustomerSessions  
			WITH CHECK ADD CONSTRAINT 
				FK_tCustomer_tCustomerSessions 
			FOREIGN KEY
				(CustomerID)
			REFERENCES 
				tCustomers(CustomerID)	
		END
		
			IF NOT EXISTS (
				SELECT 1 
				FROM sys.foreign_keys 
				WHERE object_id = OBJECT_ID(N'[FK_tCustomerSessions_tCustomerSessionCounterIdDetails]') 
				AND parent_object_id = OBJECT_ID(N'[tCustomerSessionCounterIdDetails]'))
		BEGIN
			ALTER TABLE 
				tCustomerSessionCounterIdDetails  
			WITH CHECK ADD CONSTRAINT 
				FK_tCustomerSessions_tCustomerSessionCounterIdDetails
			FOREIGN KEY
				(CustomerSessionID)
			REFERENCES 
				tCustomerSessions(CustomerSessionID)	
		END
		
		
		
		IF EXISTS (
				SELECT 1 
				FROM INFORMATION_SCHEMA.COLUMNS 
				WHERE TABLE_NAME = 'tCustomerSessionShoppingCarts' 
				AND COLUMN_NAME = 'CustomerSessionID'
		)
		BEGIN
			ALTER TABLE tCustomerSessionShoppingCarts 
			ALTER COLUMN CustomerSessionID BIGINT NOT NULL
		END

		IF NOT EXISTS (
				SELECT 1 
				FROM sys.foreign_keys 
				WHERE object_id = OBJECT_ID(N'[FK_tCustomerSessions_tCustomerSessionShoppingCarts]') 
				AND parent_object_id = OBJECT_ID(N'[tCustomerSessionShoppingCarts]'))
		BEGIN
			ALTER TABLE 
				tCustomerSessionShoppingCarts  
			WITH CHECK ADD CONSTRAINT 
				FK_tCustomerSessions_tCustomerSessionShoppingCarts
			FOREIGN KEY
				(CustomerSessionID)
			REFERENCES 
				tCustomerSessions(CustomerSessionID)	
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
GO