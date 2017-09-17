--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <04-06-2017>
-- Description:	Get the state name from state code.

--EXEC usp_GetStateNameByCode 'WI'
-- ================================================================================

IF OBJECT_ID(N'usp_GetStateNameByCode', N'P') IS NOT NULL
BEGIN
     DROP PROCEDURE usp_GetStateNameByCode
END
GO

CREATE PROCEDURE usp_GetStateNameByCode
(
     @stateCode  NVARCHAR(10)
)
AS
BEGIN
     BEGIN TRY
              
			  SELECT Name 
			  FROM tStates (NOLOCK)
			  WHERE Abbr = @stateCode OR Name = @stateCode

       END TRY
	   BEGIN CATCH
	             EXECUTE  usp_CreateErrorInfo
	   END CATCH
END
