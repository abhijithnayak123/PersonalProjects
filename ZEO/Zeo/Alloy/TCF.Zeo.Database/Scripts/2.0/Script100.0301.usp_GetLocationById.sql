-- =============================================
-- Author:		<Shwetha Mohan>
-- Create date: <12/12/2016>
-- Description: Store Proc to GET All Location using ID
-- Jira ID:		<AL-7582>
-- =============================================

IF EXISTS (SELECT 1 FROM SYS.PROCEDURES WHERE object_id = OBJECT_ID(N'[dbo].[usp_GetLocationById]') AND TYPE IN (N'P'))
DROP PROCEDURE [dbo].[usp_GetLocationById]
GO

CREATE PROCEDURE usp_GetLocationById	
(
	@locationid BIGINT 
)
AS
BEGIN
    
	SET NOCOUNT ON;
	BEGIN TRY

		SELECT 
			LocationID,
			LocationName,
			IsActive,
			Address1,
			Address2,
			City,
			[State],
			ZipCode,
			PhoneNumber,
			BankID,
			BranchID,
			TimezoneID,
			NoOfCounterIDs,
			LocationIdentifier
		FROM 
		  tLocations WITH (NOLOCK) 
		WHERE 
		  LocationID =  @locationid 

	END TRY	
	BEGIN CATCH      
		EXECUTE usp_CreateErrorInfo; 
		 
	END CATCH
END
GO
