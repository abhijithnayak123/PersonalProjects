
IF OBJECT_ID('sp_get_tcf_profileextract_details', 'P') IS NOT NULL
DROP PROC sp_get_tcf_profileextract_details
GO


CREATE PROCEDURE sp_get_tcf_profileextract_details
@tran_start_date DATETIME = NULL,
@tran_end_date DATETIME = NULL
AS   
BEGIN --{   
/*==============================================================================  
 *  
 *  NAME:  sp_get_tcf_profileextract_details  
 *  
 *  DESC:  1.	The below stored procedure retreives all the profile extract details for TCF from ODS DB
 *		   2.	By default the data is extracted for the current business day covering previous day transactions 	
 *		   3.	The SP can also be passed with any custome date as reqired	
 *  
 * ==============================================================================  
 *  
 *  RETS:  0 on success, -1 on failure  
 *  
 * ==============================================================================  
 *  
 *  HIST:  
 *  
 *    Date		Programmer             Description  
 *  --------	------------           -------------  ----------------------------------------------------  
 *  08/30/2016  MGI-Onsite 			  * Initial Version 
*==============================================================================*/  
 
-- Declare Variables
DECLARE 
	@ErrorMessage    NVARCHAR(4000),
	@ErrorNumber     INT,
	@ErrorSeverity   INT,
	@ErrorState      INT,
	@ErrorLine       INT,
	@ErrorProcedure  NVARCHAR(200);

-- Set Tran date for daily default run unless the dates have been passed by the user for a different date range
IF (@tran_start_date is null or @tran_end_date is null)
BEGIN

	SET @tran_start_date = cast(dateadd(dd,-1,getdate()) as date)
	SET @tran_end_date = cast(getdate() as Date) 


END

 BEGIN TRY

 BEGIN TRANSACTION

 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.
  SET NOCOUNT ON; 
 
 
 -- Begin data extract SQL query
 -- If the transaction date is null tran_date should be set to previous business date

 SELECT
 tc.ClientId as ClientID,
 tc.NexxoPAN as MGI_PAN,
 tc.FirstName as FirstName,
 tc.MiddleName as MiddleName,
 tc.LastName as LastName,
 tc.SecondLastName as SecondLastName,
 tc.Gender as Gender,
 tc.PrimaryPhone as PrimaryPhone,
 tc.PhoneType as PrimaryPhoneType,
 tc.AlternatePhone as AlternatePhone,
 tc.EmailAddress as EmailAddress,
 tc.MailingAddress1 as MailingAddress1,
 tc.MailingAddress2 as MailingAddress2,
 tc.MailingCity as MailingCity,
 tc.MailingState as MailingState,
 tc.MailingZipcode as MailingZipCode,
 tc.HomeAddress1 as HomeAddress1,
 tc.HomeAddress2 as HomeAddress2,
 tc.City as HomeCity,
 tc.State as HomeState,
 cast(left(tc.Zipcode,5) as char(5)) as ZipCode,
 CONVERT(varchar(10),tc.[CustomerDOB], 101) as CustomerDOB,
 tc.MothersMaidenName as MothersMaidenName,
 tc.SSN as SSN_ITIN,
 tc.GovtIdType as GovtIdType,
 tc.CountryName as CountryName,
 tc.IdIssuingEntity as IdIssuingEntity,
 convert(varchar(10),tc.[IdIssuingDate], 101) + right(convert(varchar(32),tc.[IdIssuingDate],100),8) as IdIssuingDate,
 tc.IdNumber as IdNumber,
 convert(varchar(10),tc.[IdExpirationDate], 101) + right(convert(varchar(32),tc.[IdExpirationDate],100),8) as IdExpirationDate,
 cast(tc.DoNotCall as int) as DoNotCall,
 tc.Occupation as Occupation,
 tc.ClientCustomerId as ClientCustomerId,
 convert(varchar(10),tc.[DTServer], 101) + right(convert(varchar(32),tc.[DTServer],100),8) as CreateDateTime,
 convert(varchar(10),tc.[LastUpdate], 101) + right(convert(varchar(32),tc.[LastUpdate],100),8) as LastUpdate,
 tc.LocationName as LocationName,
 tc.LocationId as LocationID,
 tc.TellerUserId as UserID,
 tc.TellerUserName as UserName,
 tl.BankID as BankID,
 (RTRIM(tc.FirstName) + ' ' + RTRIM(tc.LastName)+ ' ' + RTRIM(tc.SecondLastName)) as CustomerName,
 tc.CustomerStatus as ProfileStatus,
 tl.BranchID as BranchID,
 tc.EmployerName as EmployerName,
 tc.CountryOfCitizenship1 as CountryofCitizenship1,
 tc.CountryOfCitizenship2 as CountryofCitizenship2,
 tc.CountryOfBirth as CountryofBirth,
 tc.LegalCode as LegalCode,
 tc.OccupationDDLCode as OccupationDDLCode
 FROM 
 (select * from tCustomer tc1 where ClientId = 34 and tc1.Revision = (select max(Revision) from tCustomer tc2 where tc1.NexxoPAN = tc2.NexxoPAN)) tc 
 left join 
 (select * from tLocation tl1 where ClientID = 34 and tl1.Revision = (select max(Revision) from tLocation tl2 where tl1.LocationID = tl2.LocationID)) tl
 on tc.LocationId = tl.LocationID
 where tc.ClientId = 34
 and 
 (cast(tc.DTServer  as varchar(12))>= @tran_start_date and cast(tc.DTServer  as varchar(12))< @tran_end_date)
 or 
 (cast(tc.DTServerLastModified  as varchar(12)) >= @tran_start_date and cast(tc.DTServerLastModified  as varchar(12))< @tran_end_date)
 ORDER by tc.NexxoPAN

 
 COMMIT TRANSACTION

 END TRY


 BEGIN CATCH

 IF @@ERROR > 0  
    BEGIN  

    -- Assign variables to error-handling functions that 
    -- capture information for RAISERROR.
    SELECT 
        @ErrorNumber = ERROR_NUMBER(),
        @ErrorSeverity = ERROR_SEVERITY(),
        @ErrorState = ERROR_STATE(),
        @ErrorLine = ERROR_LINE(),
        @ErrorProcedure = ISNULL(ERROR_PROCEDURE(), '-'),
		@ErrorMessage = ERROR_MESSAGE();

    RAISERROR 
        (
        @ErrorMessage, 
        @ErrorSeverity, 
        1,               
        @ErrorNumber,    -- parameter: original error number.
        @ErrorSeverity,  -- parameter: original error severity.
        @ErrorState,     -- parameter: original error state.
        @ErrorProcedure, -- parameter: original error procedure name.
        @ErrorLine       -- parameter: original error line number.
        );

        ROLLBACK TRANSACTION
			
    END  
	
 END CATCH 

 END
 GO	

