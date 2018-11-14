--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <04-04-2017>
-- Description:	This SP is used to update the Money Transfer transaction.
-- Jira ID:		<AL-8324>
-- ================================================================================

IF OBJECT_ID(N'usp_UpdateReceiverDetails', N'P') IS NOT NULL
DROP PROC usp_UpdateReceiverDetails
GO


CREATE PROCEDURE usp_UpdateReceiverDetails
(
	@wuTrxId BIGINT
	,@customerSessionId BIGINT
	,@dtTerminalDate DATETIME
	,@dtServerDate DATETIME
)
AS
BEGIN
BEGIN TRY
	
		DECLARE @customerId BIGINT =
		(
			SELECT CustomerId
			FROM tCustomerSessions
			WHERE CustomerSessionId = @customerSessionId
		)

		UPDATE wr
		SET 
		  wr.FirstName = ISNULL(wt.RecieverFirstName,wr.FirstName)
		  ,wr.LastName = ISNULL(wt.RecieverLastName,wr.LastName) 
		  ,wr.SecondLastName = ISNULL(wt.RecieverSecondLastName,wr.SecondLastName) 
		  ,wr.PickupCountry = ISNULL(wt.DestinationCountryCode,wr.PickupCountry) 
		  ,wr.[PickupState/Province] = ISNULL(wt.ExpectedPayoutStateCode,NULL) 
		  ,wr.PickupCity = ISNULL(wt.ExpectedPayoutCityName,NULL) 
		  ,wr.DeliveryMethod = ISNULL(wt.DeliveryOption,wr.DeliveryMethod) 
		  ,wr.CustomerId = @customerId
		  ,wr.DTTerminalLastModified = @dtTerminalDate
		  ,wr.DTServerLastModified = @dtServerDate
		FROM tWUnion_Trx wt
			INNER JOIN tWUnion_Receiver wr ON wt.WUReceiverID = wr.WUReceiverID
		WHERE wt.WUTrxID = @wuTrxId
	
END TRY
BEGIN CATCH

	EXECUTE usp_CreateErrorInfo
		
END CATCH
END
GO
