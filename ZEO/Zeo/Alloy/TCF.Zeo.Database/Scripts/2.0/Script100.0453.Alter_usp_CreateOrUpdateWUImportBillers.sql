--- ===============================================================================
-- Author:		 Manikandan Govindraj
-- Description:	 Create or update all Western Union import billers
-- Jira ID:		<AL-8320>
-- ================================================================================



IF OBJECT_ID(N'usp_CreateOrUpdateWUImportBillers', N'P') IS NOT NULL
	DROP PROC usp_CreateOrUpdateWUImportBillers
GO

CREATE PROCEDURE usp_CreateOrUpdateWUImportBillers	
	@importBillers XML
AS
BEGIN
	BEGIN TRY
		
		SET NOCOUNT ON

		DECLARE @wUBillPayAcccountId BIGINT
		DECLARE @customerSessionId BIGINT

		IF OBJECT_ID('#TempImportBillers') IS NOT NULL
		BEGIN
			DROP TABLE #TempImportBillers
		END

		SELECT 
			[Table].[Column].value('BillerName[1]', 'VARCHAR(255)') AS 'BillerName',
			[Table].[Column].value('AccountNumber[1]', 'NVARCHAR(100)') AS 'AccountNumber',
			[Table].[Column].value('CardNumber[1]', 'NVARCHAR(100)') AS 'CardNumber',
			[Table].[Column].value('WUIndex[1]', 'NVARCHAR(100)') AS 'WUIndex',
			[Table].[Column].value('AgentSessionId[1]', 'BIGINT') AS 'AgentSessionId',
			[Table].[Column].value('CustomerSessionId[1]', 'BIGINT') AS 'CustomerSessionId',
			[Table].[Column].value('DTTerminalDate[1]', 'DATETIME') AS 'DTTerminalDate',
			[Table].[Column].value('DTServerDate[1]', 'DATETIME') AS 'DTServerDate'
		INTO 
			#TempImportBillers
		FROM 
			@importBillers.nodes('/DocumentElement/ImportBillers') AS [Table]([Column])

		--Getting the AccountId from the tWUnion_BillPayAccount

		SELECT TOP 1 @customerSessionId = CustomerSessionId  FROM #TempImportBillers

		SELECT
			@wUBillPayAcccountId = twa.WUBillPayAccountID
		FROM 
			dbo.tWUnion_BillPay_Account twa
			INNER JOIN dbo.tCustomerSessions tcs ON tcs.CustomerID = twa.CustomerId
		WHERE 
			tcs.CustomerSessionID = @customerSessionId
		
		-- Update Account number for existing billers
		UPDATE 
			wib
		SET
			wib.AccountNumber = tib.AccountNumber,
			wib.DTServerLastModified = tib.DTServerDate
		FROM 
			#TempImportBillers tib
			INNER JOIN tWUnion_ImportBillers wib ON tib.WUIndex = wib.WUIndex
				AND tib.CardNumber = wib.CardNumber
				AND tib.BillerName = wib.BillerName
	    WHERE
		     tib.AccountNumber != wib.AccountNumber
			 AND
			 wib.WUBillPayAccountId = @wUBillPayAcccountId
		 

		-- Insert Import billers which is not there in the table

		INSERT INTO tWUnion_ImportBillers
		(
			BillerName,
			AccountNumber,
			CardNumber,
			WUIndex,
			WUBillPayAccountId,
			AgentSessionId,
			CustomerSessionId,
			DTServerCreate
		)
		SELECT 
			tib.BillerName,
			tib.AccountNumber,
			tib.CardNumber,
			tib.WUIndex,
			@wUBillPayAcccountId,
			tib.AgentSessionId,
			tib.CustomerSessionId,
			tib.DTServerDate
		FROM 
			#TempImportBillers tib
			LEFT JOIN tWUnion_ImportBillers wib ON tib.WUIndex = wib.WUIndex AND wib.WUBillPayAccountId = @wUBillPayAcccountId
			AND tib.CardNumber = wib.CardNumber
			AND tib.BillerName = wib.BillerName
		WHERE 
			wib.WUIndex IS NULL
			AND wib.CardNumber IS NULL
			AND wib.BillerName IS NULL

		UPDATE 
			cpp
		SET
			cpp.ReceiverIndexNo = tib.WUIndex,
			cpp.DTServerLastModified = tib.DTServerDate,
			cpp.DTTerminalLastModified = tib.DTTerminalDate
		FROM 
			#TempImportBillers tib
			INNER JOIN tCustomerPreferedProducts cpp
			ON cpp.AccountNumber = tib.AccountNumber		
			INNER JOIN tCustomerSessions cs
			ON cs.CustomerId = cpp.CustomerId
			WHERE
			 cs.CustomerSessionId = @customerSessionId

		
		INSERT INTO dbo.tCustomerPreferedProducts
		(
			CustomerID,
			ProductId,
			AccountNumber,
			PhoneNumber,
			AccountDOB,
			Enabled,
			DTTerminalCreate,
			DTServerCreate
		)
		SELECT 
			tc.CustomerID,
			dbo.ufn_GetProductIdByBillerInfo(tib.BillerName, tc.ChannelPartnerId),
			tib.AccountNumber,
			tc.Phone1,
			tc.DOB,
			1, -- Enabled by default
			tib.DTTerminalDate,
			tib.DTServerDate
		FROM 
			dbo.tCustomers tc
			INNER JOIN dbo.tCustomerSessions tcs ON tcs.CustomerID = tc.CustomerID
			INNER JOIN #TempImportBillers tib ON tib.CustomerSessionId = tcs.CustomerSessionID
			LEFT JOIN tCustomerPreferedProducts tcpp ON tcpp.AccountNumber = tib.AccountNumber AND tcpp.CustomerId = tc.CustomerId 
		WHERE 
			tcpp.AccountNumber IS NULL

		
	END TRY

	BEGIN CATCH
		EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError
	END CATCH
END
GO
