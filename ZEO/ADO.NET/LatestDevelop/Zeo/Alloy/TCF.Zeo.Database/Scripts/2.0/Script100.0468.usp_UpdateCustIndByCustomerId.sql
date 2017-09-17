	--- ===============================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <04-05-2017>
-- Description:	 Update TcfCustInd in tTcis_Account.
-- ====================================================================================

IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'usp_UpdateCustIndByCustomerId')
DROP PROCEDURE usp_UpdateCustIndByCustomerId
GO

CREATE PROCEDURE usp_UpdateCustIndByCustomerId
	@CustomerId BIGINT,
	@CustInd BIT
AS
BEGIN
	BEGIN TRY
		UPDATE 
			tTCIS_Account
		SET 
			TcfCustInd = @CustInd
		WHERE
			 CustomerID = @CustomerId
	END TRY
	BEGIN CATCH
	-- Execute error retrieval routine.  
	    EXECUTE usp_CreateErrorInfo; 
	END CATCH
END
GO

