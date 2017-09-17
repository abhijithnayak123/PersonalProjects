--- ===============================================================================
-- Author:		<Rizwana Shaik>
-- Create date: <11-29-2016>
-- Description:	Create procedure for update WU points earned
-- Jira ID:		<AL-8423>
-- ================================================================================


IF OBJECT_ID(N'usp_UpdateWUCardPointEarned', N'P') IS NOT NULL
	DROP PROCEDURE usp_UpdateWUCardPointEarned   -- Drop the existing procedure.
GO

CREATE PROCEDURE usp_UpdateWUCardPointEarned
(
	@wuBillPayTrxID          BIGINT,
	@totalPointsEarned      NVARCHAR(50),
	@dtTerminalLastModified DATETIME,
	@dtServerLastModified   DATETIME
)
AS
BEGIN
	BEGIN TRY
		UPDATE 
			dbo.tWUnion_BillPay_Trx
		SET
			WUCard_TotalPointsEarned = @totalPointsEarned,
			DTTerminalLastModified = @dtTerminalLastModified,
			DTServerLastModified = @dtServerLastModified
		WHERE 
			WUBillPayTrxID = @wuBillPayTrxID
	END TRY 
	
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo
	END CATCH
END
GO
