IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE = 'FN' AND NAME = 'ufn_IsOtherReceiverExists')
DROP FUNCTION ufn_IsOtherReceiverExists
GO

CREATE FUNCTION ufn_IsOtherReceiverExists
(
	@customerId BIGINT
	,@receiverFname VARCHAR(100)
	,@receiverLname VARCHAR(100)
	,@receiversecondlastname VARCHAR(100)
	,@pickupcountry VARCHAR(100)
	,@receiverId BIGINT
)
RETURNS BIT
AS
BEGIN
	DECLARE @isOtherReceiverExists BIT = 0 
	 
	DECLARE @isReceiverExists BIT

	SELECT 
		@isReceiverExists =
		(
			CASE  
				WHEN COUNT(1) > 0 THEN 1
				ELSE 0
			END
		)	
	FROM 
		tWUnion_Receiver
	WHERE 
		WUReceiverID = @receiverId 
	
	IF @isReceiverExists = 0
	BEGIN		
	 ---First we are validating with considering receiver secondlastname and checking if there is any record match with this receiver details
			SELECT 
				@isOtherReceiverExists = 
				(
					CASE  
						WHEN COUNT(1) > 0 THEN 1
						ELSE 0
					END
				)	
			 FROM 
				tWUnion_Receiver wr
			 WHERE 
				wr.FirstName = ISNULL(@receiverFname, '')
				AND wr.LastName = ISNULL(@receiverLname, '')
				AND wr.SecondLastName = ISNULL(@receiversecondlastname, '')
				AND wr.PickupCountry = ISNULL(@pickupcountry, '')
				AND wr.CustomerId = @customerId
				AND UPPER(wr.[Status]) = 'ACTIVE'
				AND @receiversecondlastname IS NOT NULL
			
			--- After considering receiver secondLastName 
			IF @isOtherReceiverExists = 0
				BEGIN
					SELECT 
						@isOtherReceiverExists = 
						(
							CASE  
								WHEN COUNT(1) > 0 THEN 1
								ELSE 0
							END
						)	
					FROM 
						tWUnion_Receiver wr
					WHERE 
						wr.FirstName = ISNULL(@receiverFname, '')
						AND wr.LastName = ISNULL(@receiverLname, '')
						AND wr.PickupCountry = ISNULL(@pickupcountry, '')
						AND wr.CustomerId = @customerId
						AND UPPER(wr.[Status]) = 'ACTIVE'
						AND @receiversecondlastname IS NULL
				END

	END
	ELSE
		BEGIN
			SELECT 
				@isOtherReceiverExists = 
				(
					CASE  
						WHEN COUNT(1) > 0 THEN 1
						ELSE 0
					END
				)	
			 FROM 
				tWUnion_Receiver wr
			 WHERE 
				wr.FirstName = ISNULL(@receiverFname, '')
				AND wr.LastName = ISNULL(@receiverLname, '')
				AND wr.SecondLastName = ISNULL(@receiversecondlastname, '')
				AND wr.PickupCountry = ISNULL(@pickupcountry, '')
				AND wr.CustomerId = @customerId
				AND UPPER(wr.[Status]) = 'ACTIVE'
				AND @receiversecondlastname IS NOT NULL
				AND wr.WUReceiverID != @receiverId
			
			--- After considering receiver secondLastName 
			IF @isOtherReceiverExists = 0
				BEGIN
					SELECT 
						@isOtherReceiverExists = 
						(
							CASE  
								WHEN COUNT(1) > 0 THEN 1
								ELSE 0
							END
						)	
					FROM 
						tWUnion_Receiver wr
					WHERE 
						wr.FirstName = ISNULL(@receiverFname, '')
						AND wr.LastName = ISNULL(@receiverLname, '')
						AND wr.PickupCountry = ISNULL(@pickupcountry, '')
						AND wr.CustomerId = @customerId
						AND UPPER(wr.[Status]) = 'ACTIVE'
						AND @receiversecondlastname IS NULL
						AND wr.WUReceiverID != @receiverId
				END
		END

		RETURN @isOtherReceiverExists;
END
GO

