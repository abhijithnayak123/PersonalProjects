IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE = 'FN' AND NAME = 'ufn_IsOtherReceiverExists')
DROP FUNCTION ufn_IsOtherReceiverExists
GO

CREATE FUNCTION ufn_IsOtherReceiverExists
(
	@customerId BIGINT
	,@receiverFname VARCHAR(100)
	,@receiverLname VARCHAR(100)
)
RETURNS BIT
AS
BEGIN
	 DECLARE @isOtherReceiverExists BIT = 0 	

	 SELECT 
		@isOtherReceiverExists = 
		(
			CASE  
				WHEN COUNT(1) > 1 THEN 1
				ELSE 0
			END
		)	
	 FROM tWUnion_Receiver wr
	 WHERE wr.FirstName = ISNULL(@receiverFname, '')
				AND wr.LastName = ISNULL(@receiverLname, '')
				AND wr.CustomerId = @customerId
				AND UPPER(wr.[Status]) = 'ACTIVE'

	RETURN @isOtherReceiverExists;
END
GO

