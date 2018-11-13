-- ================================================================================
-- Author:		<Praveen SR>
-- Create date: <04/09/2015>
-- Description:	<Query to update the Occupation column values maitained of 5 digits with left side 0's if it is numbertype.>
-- Jira ID:		<AL-1597>
-- ================================================================================
DECLARE @cnt AS INT

SELECT @cnt = COUNT(1) FROM tCustomerEmploymentDetails WHERE ISNUMERIC(occupation) = 1

WHILE @cnt > 0
BEGIN
	DECLARE @ID AS UNIQUEIDENTIFIER;
	
	SELECT 
		top(1) @ID = CustomerPK 
	FROM 
		tCustomerEmploymentDetails 
	WHERE 
		ISNUMERIC(occupation) = 1 
	ORDER BY 
		Occupation DESC;
		
	UPDATE 
		ced
	SET 
		ced.occupation = RIGHT('00000' + CONVERT(VARCHAR,Occupation), 5)
	FROM 
		tCustomerEmploymentDetails ced
	WHERE 
		ISNUMERIC(ced.occupation) = 1 
		AND ced.CustomerPK = @ID;

	SET @cnt = @cnt - 1;
END
GO