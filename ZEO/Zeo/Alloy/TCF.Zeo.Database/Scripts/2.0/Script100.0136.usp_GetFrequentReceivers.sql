--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-02-2016>
-- Description:	Get the frequent receivers. 
-- Jira ID:		<AL-8324>
-- ================================================================================

IF OBJECT_ID(N'usp_GetFrequentReceivers', N'P') IS NOT NULL
DROP PROC usp_GetFrequentReceivers
GO


CREATE PROCEDURE usp_GetFrequentReceivers
(
    @customerId BIGINT
)
AS
BEGIN
	
BEGIN TRY
	
	SELECT 
		wr.FirstName AS FirstName,
		wr.LastName AS LastName,
		wr.SecondLastName AS SecondLastName,
		wr.Status AS Status,
		wr.Gender AS Gender,
		wr.WUReceiverID AS ReceiverID,
		wr.PickupCountry AS PickupCountry,
		wr.[State/Province] AS  StateProvince,
		wr.City AS City
	FROM tWUnion_Receiver wr
	WHERE wr.CustomerId = @customerId 
		AND UPPER(wr.Status) = 'ACTIVE'
    
END TRY
BEGIN CATCH

	EXECUTE usp_CreateErrorInfo
		
END CATCH
END
GO
