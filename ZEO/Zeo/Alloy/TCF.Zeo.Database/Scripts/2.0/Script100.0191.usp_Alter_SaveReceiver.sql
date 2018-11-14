--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-03-2016>
-- Description:	This SP is used for Add/Edit receiver through UI of the application.
-- Jira ID:		<AL-8324>

/* 
EXEC usp_SaveReceiver 0, 1000000000000010, 'phil','chang',NULL, 'Active','jp nagar','bangalore','karnataka','560078','7899076396','XB',
	 NULL,NULL,'000',NULL,NULL,NULL, '12/7/2016 6:37:05 PM','12/7/2016 6:37:05 PM' 
*/
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
	,@receiverIndexNo VARCHAR(5) = NULL
	,@goldCardNumber VARCHAR(50) = NULL
	,@dtServerDate DATETIME = NULL
	,@dtTerminalDate DATETIME = NULL
)
AS
BEGIN
	
BEGIN TRY
	
	
	DECLARE @recId BIGINT = 0
	DECLARE @isReceiverExists BIT = 0
	DECLARE @isOtherReceiverExists BIT = 
	(
		SELECT dbo.ufn_IsOtherReceiverExists(@customerId, @firstName, @lastName)
	)

	--Check whether the given receiver exists. For updating the existing receiver.
	SELECT 
			@isReceiverExists =
			(
				CASE  
					WHEN COUNT(1) > 0 THEN 1
					ELSE 0
				END
			)	
		FROM tWUnion_Receiver
		WHERE WUReceiverID = @receiverId 

	-- For new receiver, when clicked on "Add Receiver". If there are no other receivers with the same firstname, lastname for 
	-- the given customerId.
	IF @isReceiverExists = 0 AND @isOtherReceiverExists = 0
	BEGIN
	
		-- This condition is added to check the receiver in our database. In case of Adding a new receiver
		-- we will get the receiverId - 0, in that case check the receivers based on firstname and lastname and active in case
		-- we are getting receiverId - 0
		IF NOT EXISTS 
		(
			SELECT 1
			FROM tWUnion_Receiver wr
			WHERE wr.FirstName = ISNULL(@firstName, '')
					AND wr.LastName = ISNULL(@lastName, '')
					AND wr.CustomerId = @customerId
					AND UPPER(wr.[Status]) = 'ACTIVE'
		)
		BEGIN

			INSERT INTO tWUnion_Receiver
			(
			    [FirstName]
			   ,[LastName]
			   ,[SecondLastName]
			   ,[Status]
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
			   ,[CustomerId]
			   ,[DTServerCreate]
			   ,[ReceiverIndexNo]
			   ,[GoldCardNumber]
			   ,[DTTerminalCreate]
			)
			 VALUES
			 (
			    @firstName
			   ,@lastName
			   ,@secondLastName
			   ,@status
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
			   ,@customerId
			   ,@dtServerDate
			   ,@receiverIndexNo
			   ,@goldCardNumber
			   ,@dtTerminalDate
			 )

			SET @recId = CAST (SCOPE_IDENTITY() AS BIGINT)
		
		END
		ELSE
		BEGIN
			-- Instead of creating new variable, making the existing variable as Receiver exists.
			SET @isOtherReceiverExists = 1
		END
	END
	ELSE IF @isReceiverExists = 1 AND @isOtherReceiverExists = 0
	BEGIN
			UPDATE 
				tWUnion_Receiver
			SET
			    FirstName=@firstName
				,LastName=@lastName
				,CustomerId=@customerId
				,SecondLastName=@secondLastName
				,DeliveryMethod=@deliveryMethod
				,DeliveryOption=@deliveryOption
				,Status=@status
				,GoldCardNumber=@goldCardNumber
				,PickupCity=@pickupCity
				,PickupCountry=@pickupCountry
				,[PickupState/Province]=@pickupStateProvince
				,DTTerminalLastModified = @dtTerminalDate
				,DTServerLastModified = @dtServerDate
				,ReceiverIndexNo = ISNULL(@receiverIndexNo, ReceiverIndexNo)
				,Address=@address
			    ,City=@city
			    ,[State/Province]=@stateProvince
			    ,ZipCode=@zipCode
			    ,PhoneNumber=@phoneNumber
			WHERE 
				WUReceiverID = @receiverId 
	END

	SELECT 
		@recId AS receiverID
		, @isOtherReceiverExists AS isReceiverExists
	
END TRY
BEGIN CATCH

    EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError
		
END CATCH
END
GO
