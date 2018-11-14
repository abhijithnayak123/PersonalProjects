-- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-20-2017>
-- Description:	Insert records a new table to store the Conditions for determining the Provider. 
-- Jira ID:		<B-08674>
-- ================================================================================

-- Insert records to table - 'tCheckClassifications' 15,16,17

DECLARE @onUsOCMOCheckTypeId INT = (SELECT CheckTypeId FROM tCheckTypes WHERE Name = 'OnUsOCMO')
DECLARE @onUsTRUECheckTypeId INT = (SELECT CheckTypeId FROM tCheckTypes WHERE Name = 'OnUsTRUE')
DECLARE @masterCountryId BIGINT = (SELECT MasterCountriesID FROM tMasterCountries WHERE Name = 'UNITED STATES')
DECLARE @stateCodeMN BIGINT = (SELECT StateId FROM tStates WHERE Abbr = 'MN' AND MasterCountriesID = @masterCountryId)
DECLARE @stateCodeCO BIGINT = (SELECT StateId FROM tStates WHERE Abbr = 'CO' AND MasterCountriesID = @masterCountryId)
DECLARE @stateCodeAZ BIGINT = (SELECT StateId FROM tStates WHERE Abbr = 'AZ' AND MasterCountriesID = @masterCountryId)
DECLARE @stateCodeIL BIGINT = (SELECT StateId FROM tStates WHERE Abbr = 'IL' AND MasterCountriesID = @masterCountryId)
DECLARE @stateCodeMI BIGINT = (SELECT StateId FROM tStates WHERE Abbr = 'MI' AND MasterCountriesID = @masterCountryId)
DECLARE @stateCodeWI BIGINT = (SELECT StateId FROM tStates WHERE Abbr = 'WI' AND MasterCountriesID = @masterCountryId)

BEGIN TRY
	BEGIN TRAN

		DELETE FROM tCheckClassifications

		INSERT INTO tCheckClassifications(CheckTypeId, StartRoutingNumber, EndRoutingNumber, StartAccountNumber, EndAccountNumber, AccountNumberLength, StateId, DTServerCreate) 
		VALUES
		(@onUsOCMOCheckTypeId, 291070001, 291070001, NULL, NULL, 13, @stateCodeMN, GETDATE())
		,(@onUsTRUECheckTypeId, 107006444, 107006444, NULL, NULL, 10,@stateCodeCO, GETDATE())
		,(@onUsTRUECheckTypeId, 122106183, 122106183, NULL, NULL, 10,@stateCodeAZ, GETDATE())
		,(@onUsTRUECheckTypeId, 271972572, 271972572, NULL, NULL, 10,@stateCodeIL, GETDATE())
		,(@onUsTRUECheckTypeId, 272471548, 272471548, NULL, NULL, 10,@stateCodeMI, GETDATE())
		,(@onUsTRUECheckTypeId, 275071385, 275071385, NULL, NULL, 10,@stateCodeWI, GETDATE())
		,(@onUsTRUECheckTypeId, 291070001, 291070001, NULL, NULL, 10,@stateCodeMN, GETDATE())

	 COMMIT TRAN
END TRY
BEGIN CATCH
	 IF(@@TRANCOUNT > 0)
        ROLLBACK TRAN;
END CATCH
GO
