--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-03-2016>
-- Description:	This SP is used for Add/Edit receiver through UI of the application.
-- Jira ID:		<AL-8324>
-- ================================================================================

IF OBJECT_ID(N'usp_SaveReceiver', N'P') IS NOT NULL
DROP PROC usp_SaveReceiver
GO


CREATE PROCEDURE usp_SaveReceiver
(
    @receiverId BIGINT
	,@customerId BIGINT
	,@firstName VARCHAR(100) = NULL
	,@lastName VARCHAR(100) = NULL
	,@secondLastName VARCHAR(100) = NULL
	,@status VARCHAR(20) = NULL
	,@gender VARCHAR(10) = NULL
	,@country VARCHAR(200) = NULL
	,@address VARCHAR(250) = NULL
	,@city VARCHAR(200) = NULL
	,@stateProvince VARCHAR(200) = NULL
	,@zipCode VARCHAR(10) = NULL
	,@phoneNumber VARCHAR(20) = NULL
	,@pickupCountry VARCHAR(100) = NULL
	,@pickupStateProvince VARCHAR(100) = NULL
	,@pickupCity VARCHAR(100) = NULL
	,@deliveryMethod VARCHAR(100) = NULL
	,@deliveryOption VARCHAR(100) = NULL
	,@occupation VARCHAR(100) = NULL
	,@DOB DATETIME = NULL
	,@countryOfBirth VARCHAR(100) = NULL
	,@dtServerCreate DATETIME = NULL
	,@dtServerLastModified DATETIME = NULL
	,@receiverIndexNo VARCHAR(5) = NULL
	,@goldCardNumber VARCHAR(50) = NULL
	,@dtTerminalCreate DATETIME = NULL
	,@dtTerminalLastModified DATETIME = NULL
)
AS
BEGIN
	
BEGIN TRY
	
	DECLARE @isReceiverExists BIT = 0
	DECLARE @isOtherReceiverExists BIT = 0

	--Check whether the given receiver exists.
	SELECT 
			@isReceiverExists =
			(
				CASE  
					WHEN COUNT(1) > 0 THEN 1
					ELSE 0
				END
			)	
		FROM tWUnion_Receiver wr
		WHERE wr.WUReceiverID = @receiverId 


	--Check whether the other receiver exists for the given customerId and for given receiver details.
	SELECT 
			@isOtherReceiverExists =
			(
				CASE  
					WHEN COUNT(1) > 0 THEN 1
					ELSE 0
				END
			)	
	FROM tWUnion_Receiver wr
	WHERE (@isReceiverExists = 1 AND wr.WUReceiverID <> @receiverId) 
			AND wr.FirstName = ISNULL(@firstName, '')
			AND wr.LastName = ISNULL(@lastName, '')
			AND wr.CustomerId = @customerId
			AND UPPER(wr.[Status]) = 'ACTIVE'


	IF @isReceiverExists = 0 AND @isOtherReceiverExists = 0
	BEGIN
	
			INSERT INTO tWUnion_Receiver
			(
				[WUReceiverPK]
			   ,[FirstName]
			   ,[LastName]
			   ,[SecondLastName]
			   ,[Status]
			   ,[Gender]
			   ,[Country]
			   ,[Address]
			   ,[City]
			   ,[State/Province]
			   ,[ZipCode]
			   ,[PhoneNumber]
			   ,[PickupCountry]
			   ,[PickupState/Province]
			   ,[PickupCity]
			   ,[DeliveryMethod]
			   ,[DeliveryOption]
			   ,[Occupation]
			   ,[DOB]
			   ,[CountryOfBirth]
			   ,[CustomerId]
			   ,[DTServerCreate]
			   ,[DTServerLastModified]
			   ,[ReceiverIndexNo]
			   ,[GoldCardNumber]
			   ,[DTTerminalCreate]
			   ,[DTTerminalLastModified]
			)
			 VALUES
			 (
				NEWID()
			   ,@firstName
			   ,@lastName
			   ,@secondLastName
			   ,@status
			   ,@gender
			   ,@country
			   ,@address
			   ,@city
			   ,@stateProvince
			   ,@zipCode
			   ,@phoneNumber
			   ,@pickupCountry
			   ,@pickupStateProvince
			   ,@pickupCity
			   ,@deliveryMethod
			   ,@deliveryOption
			   ,@occupation
			   ,@DOB
			   ,@countryOfBirth
			   ,@customerId
			   ,@dtServerCreate
			   ,@dtServerLastModified
			   ,@receiverIndexNo
			   ,@goldCardNumber
			   ,@dtTerminalCreate
			   ,@dtTerminalLastModified 
			 )


			 SELECT CAST(SCOPE_IDENTITY() AS BIGINT) AS receiverID 
		END

	ELSE IF @isReceiverExists = 1 AND @isOtherReceiverExists = 0
	BEGIN

			UPDATE 
				tWUnion_Receiver
			SET
				DTTerminalLastModified = @dtTerminalLastModified
				, DTServerLastModified = @dtServerLastModified
				, ReceiverIndexNo = ISNULL(@receiverIndexNo, ReceiverIndexNo)
			WHERE 
				WUReceiverID = @receiverId 

		END
	
END TRY
BEGIN CATCH

    EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError
		
END CATCH
END
GO
