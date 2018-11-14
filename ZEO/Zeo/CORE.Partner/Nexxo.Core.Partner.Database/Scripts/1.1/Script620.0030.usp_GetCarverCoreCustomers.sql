-- =============================================
-- Author:	Karun
-- Create date: 23-Jun-2016
-- Description:	SP to get CARVVER CORE CUSTOMES from database based on input parameters
-- =============================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_getCarverCoreCustomers'
)
BEGIN
	DROP PROCEDURE usp_getCarverCoreCustomers
END
GO

CREATE PROCEDURE [dbo].[usp_getCarverCoreCustomers]
(
	@Dateofbirth DATETIME,
	@Phonenumber nvarchar(20),
	@Zipcode nvarchar(20),
	@Lastname nvarchar(255)
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
		[AO_PTNR].[dbo].[tCCISConnectsDb] WITH (NOLOCK)
	WHERE 
		(@Dateofbirth IS NULL OR DOB = @Dateofbirth) 
		AND (@Phonenumber IS NULL OR PrimaryPhoneNumber = @Phonenumber) 
		AND (@Zipcode IS NULL OR ZipCode = @Zipcode) 
		AND (@lastname IS NULL OR LastName = @Lastname)
  END
GO