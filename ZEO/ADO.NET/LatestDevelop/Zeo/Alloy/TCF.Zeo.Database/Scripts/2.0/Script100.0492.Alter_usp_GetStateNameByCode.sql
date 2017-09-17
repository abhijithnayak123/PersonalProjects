--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <04-06-2017>
-- Description:	Get the state name from state code.

--EXEC usp_GetStateNameByCode 'MINNESOTA','US'
-- ================================================================================

IF OBJECT_ID(N'usp_GetStateNameByCode', N'P') IS NOT NULL
BEGIN
     DROP PROCEDURE usp_GetStateNameByCode
END
GO

CREATE PROCEDURE usp_GetStateNameByCode
(
     @stateCode  NVARCHAR(50)
	 ,@countryCode NVARCHAR(100)
)
AS
BEGIN
     BEGIN TRY
              
			  SELECT s.Name 
			  FROM tStates s(NOLOCK)
					INNER JOIN tMasterCountries mc (NOLOCK) ON s.MasterCountriesId = mc.MasterCountriesID 
			  WHERE (s.Abbr = @stateCode OR s.Name = @stateCode) AND 
					(mc.Abbr2 = @countryCode OR mc.Name = @countryCode) 

       END TRY
	   BEGIN CATCH
	             EXECUTE  usp_CreateErrorInfo
	   END CATCH
END
