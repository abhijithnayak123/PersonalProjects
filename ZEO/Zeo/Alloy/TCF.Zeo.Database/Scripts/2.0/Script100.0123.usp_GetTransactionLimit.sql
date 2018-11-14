--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <08-02-2016>
-- Description:	 Stored procedure to fetch customer fee adjustments
-- Jira ID:		<AL-7926>
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_GetTransactionLimit'
)
BEGIN
	DROP PROCEDURE usp_GetTransactionLimit
END
GO

CREATE PROCEDURE usp_GetTransactionLimit
	@channelPartnerId INT,
	@transactionType NVARCHAR(50)

AS
BEGIN
	BEGIN TRY
		 
		SELECT l.PerTransactionMaximum AS PerTransactionMaximum, l.PerTransactionMinimum AS PerTransactionMinimum, l.RollingLimits AS RollingLimits
			FROM tChannelPartners tcp
		INNER JOIN tCompliancePrograms cp ON tcp.ComplianceProgramName = cp.Name
		INNER JOIN tLimitTypes lt ON lt.ComplianceProgramID = cp.ComplianceProgramID
		INNER JOIN tLimits l ON l.LimitTypeID = lt.LimitTypeID
		WHERE tcp.ChannelPartnerId = @ChannelPartnerId AND lt.Name = @TransactionType
			
	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END