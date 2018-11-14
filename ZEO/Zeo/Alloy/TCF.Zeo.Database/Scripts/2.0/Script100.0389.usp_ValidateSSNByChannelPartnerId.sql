-- ================================================================================
-- Author:		<Kaushik Sakal>
-- Create date: <07/27/2015>
-- Description:	<As an engineer, I want to implement ADO.Net for Customer module>
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
		SELECT CASE WHEN (
			SELECT 
				COUNT(1) 
			FROM
				 tCustomers C INNER JOIN
				 tChannelPartners cp on cp.ChannelPartnerId = c.ChannelPartnerId INNER JOIN
				 tChannelPartnerConfig cpc on cpc.ChannelPartnerID = cp.ChannelPartnerId
			WHERE 
				C.ChannelPartnerId = @channelPartnerId 
				AND SSN = @SSN
				AND CustomerID <> @customerId
				AND ProfileStatus <> 2
				AND cpc.MasterSSN <> @SSN
		) = 0 THEN @true ELSE @false END AS ValidSSN
		
	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END
