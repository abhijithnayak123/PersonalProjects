-- ================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <07/27/2015>
-- Modified Date: <06/20/2017>
-- Description:	<Validating SSN against Active/Inactive customers and 
-- Jira ID:		<AL-7630>
-- ================================================================================
IF EXISTS(
	SELECT 1
	FROM SYS.objects
	WHERE NAME = 'usp_ValidateSSNByChannelPartnerId'
)

BEGIN
	DROP PROCEDURE usp_ValidateSSNByChannelPartnerId
END
GO

CREATE PROCEDURE usp_ValidateSSNByChannelPartnerId
	@channelPartnerId BIGINT,
	@SSN NVARCHAR(55),
	@customerId BIGINT = 0,
	@true BIT = 1,
	@false BIT = 0
AS
BEGIN
	BEGIN TRY
		SELECT 
			CASE WHEN COUNT(1) = 0
			THEN @true 
			ELSE @false
			END AS ValidSSN
		FROM
				tCustomers C INNER JOIN
				tChannelPartners cp on cp.ChannelPartnerId = c.ChannelPartnerId INNER JOIN
				tChannelPartnerConfig cpc on cpc.ChannelPartnerID = cp.ChannelPartnerId
		WHERE 
			C.ChannelPartnerId = @channelPartnerId 
			AND SSN = @SSN
			AND CustomerID <> @customerId
			AND ProfileStatus = 1 
			AND cpc.MasterSSN <> @SSN
			AND IsRCIFSuccess = 1
	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END
