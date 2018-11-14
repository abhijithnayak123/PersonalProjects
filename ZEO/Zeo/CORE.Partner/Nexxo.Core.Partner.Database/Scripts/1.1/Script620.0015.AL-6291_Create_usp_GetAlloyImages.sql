--- ================================================================================
-- Author:		<Divya Boddu>
-- Create date: <05/23/2016>
-- Description:	 Implement Changes to Optimize the image size in PTNR DB Size.
-- Jira ID:		<AL-6291>
-- ================================================================================

IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'usp_GetAlloyImages') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[usp_GetAlloyImages] 
GO

CREATE PROCEDURE [dbo].[usp_GetAlloyImages] 
	@transactionType SMALLINT,
	@archivePriorToDays INT
AS
BEGIN
	
	SET NOCOUNT ON;

	IF @transactionType = 1 -- MoneyOrder
	BEGIN
		PRINT 'Money Order - Work In Progress'
		-- Query in progress
		--SELECT 
		--	TrxId,
		--	CheckFrontImage,
		--	CheckBackImage 
		--FROM 
		--	tMoneyOrderImage 
		--WHERE 
		--	DTServerCreate < DATEADD(DAY,-@archivePriorToDays,GETDATE()) 
		--	AND CheckFrontImage IS NOT NULL 
		--	AND CheckBackImage IS NOT NULL
	END
	ELSE IF @transactionType = 2 -- Check
	BEGIN
		SELECT 
			CheckID,
			Front,
			Back,
			cp.Name As ChannelPartnerName
		FROM 
			tTxn_Check_Stage cs  
			INNER JOIN tCheckImages ci WITH (NOLOCK) ON cs.CheckPK = ci.CheckPK
			INNER JOIN tCustomerAccounts ca WITH (NOLOCK) ON ca.AccountPK = cs.AccountPK
			INNER JOIN tCustomers c WITH (NOLOCK) ON ca.CustomerPK = c.CustomerPK
			INNER JOIN tChannelPartners cp WITH (NOLOCK) ON cp.ChannelPartnerId = c.ChannelPartnerId
		WHERE 
			ci.DTServerCreate < DATEADD(DAY,-@archivePriorToDays, GETDATE()) 
			AND FrontImagePath IS NULL 
			AND BackImagePath IS NULL
	END
END
GO

 --EXEC [dbo].[usp_GetAlloyImages] 2, 10