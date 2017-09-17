--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-03-2016>
-- Description:	This SP is used to get the receiver using receiver Id.
-- Jira ID:		<AL-8324>
-- ================================================================================

IF OBJECT_ID(N'usp_GetReceiverById', N'P') IS NOT NULL
DROP PROC usp_GetReceiverById
GO


CREATE PROCEDURE usp_GetReceiverById
(
    @receiverId BIGINT
)
AS
BEGIN
	
BEGIN TRY
	
	SELECT 
		wr.FirstName AS FirstName
		,wr.LastName AS LastName
		,wr.SecondLastName AS SecondLastName
		,wr.[Status] AS [Status]
		,wr.Gender AS Gender
		,wr.Country AS Country
		,wr.[Address] AS [Address]
		,wr.City AS City
		,wr.[State/Province] AS StateProvince
		,wr.ZipCode AS ZipCode
		,wr.PhoneNumber AS PhoneNumber
		,wr.PickupCountry AS PickupCountry
		,wr.[PickupState/Province] AS PickupStateProvince
		,wr.PickupCity AS PickupCity
		,wr.DeliveryMethod AS DeliveryMethod
		,wr.DeliveryOption AS DeliveryOption
		,wr.Occupation AS Occupation
		,wr.DOB AS DOB
		,wr.CountryOfBirth AS CountryOfBirth
		,wr.CustomerId AS CustomerId
		,wr.ReceiverIndexNo AS ReceiverIndexNo
		,wr.GoldCardNumber AS GoldCardNumber
	FROM tWUnion_Receiver wr
	WHERE WUReceiverID = @receiverId
	
END TRY
BEGIN CATCH

	EXECUTE usp_CreateErrorInfo
		
END CATCH
END
GO
