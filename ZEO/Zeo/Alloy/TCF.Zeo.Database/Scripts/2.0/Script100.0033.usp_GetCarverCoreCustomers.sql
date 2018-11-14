-- =============================================
-- Author:	Karun
-- Create date: 23-Jun-2016
-- Description:	SP to get CARVVER CORE CUSTOMES from database based on input parameters
-- =============================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_GetCarverCoreCustomers'
)
BEGIN
	DROP PROCEDURE usp_GetCarverCoreCustomers
END
GO

CREATE PROCEDURE usp_GetCarverCoreCustomers
(
	@Dateofbirth DATETIME = NULL,
	@Phonenumber NVARCHAR(50) = NULL,
	@Zipcode NVARCHAR(30) = NULL,
	@Lastname NVARCHAR(255) = NULL
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;    
	SELECT 
		[CustomerNumber]
		,[CustomerTaxNumber]
		,[PrimaryPhoneNumber]
		,[SecondaryPhone]
		,[LastName]
		,[FirstName]
		,[MiddleName]
		,[MiddleName2]
		,[AddressStreet]
		,[AddressCity]
		,[AddressState]
		,[ZipCode]
		,[DOB]
		,[MothersMaidenName]
		,[DriversLicenseNumber]
		,[ExternalKey]
		,[MetBankNumber]
		,[ProgramId]
		,[DTServerCreate]
		,[CCISConnectsID]
		,[CCISConnectsPK]
		,[Gender]
	FROM 
		tCCISConnectsDb  WITH (NOLOCK)
	WHERE 
		(@Dateofbirth IS NULL OR DOB = @Dateofbirth) 
		AND (@Phonenumber IS NULL OR PrimaryPhoneNumber = @Phonenumber) 
		AND (@Zipcode IS NULL OR ZipCode = @Zipcode) 
		AND (@lastname IS NULL OR LastName = @Lastname)
  END
GO