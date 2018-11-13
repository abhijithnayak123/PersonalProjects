-- ================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <22/09/2015>
-- Description:	<Query to update the Occupation column values maitained 
--               of 5 digits with left side 0's if it is numbertype.>
-- Jira ID:		<AL-1884>
-- ================================================================================
DECLARE @cnt AS INT

SELECT @cnt = COUNT(1) FROM tProspectEmploymentDetails WHERE ISNUMERIC(Occupation) = 1

WHILE @cnt > 0
BEGIN
	DECLARE @ID AS UNIQUEIDENTIFIER;
	
	SELECT 
		top(1) @ID = ProspectPK 
	FROM 
		tProspectEmploymentDetails 
	WHERE 
		ISNUMERIC(Occupation) = 1 
	ORDER BY 
		Occupation DESC;
		
	UPDATE 
		ped
	SET 
		ped.occupation = RIGHT('00000' + CONVERT(VARCHAR,Occupation), 5)
	FROM 
		tProspectEmploymentDetails ped
	WHERE 
		ISNUMERIC(ped.Occupation) = 1 
		AND ped.ProspectPK = @ID;

	SET @cnt = @cnt - 1;
END
GO