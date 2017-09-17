-- =============================================
-- Author:		<Shwetha Mohan>
-- Create date: <12/12/2016>
-- Description:Store Proc to Create New Locatiom
-- Jira ID:		<AL-7582>
-- =============================================
-- EXEC  usp_CreateLocation 10000000,true,Test,Test,Null,Ca,CA,95404,34,9658741230,12345,12345,12345,'2016-12-12','2016-12-12'
--Print @locationId
IF EXISTS (SELECT 1 FROM SYS.PROCEDURES WHERE object_id = OBJECT_ID(N'[dbo].[usp_CreateLocation]') AND TYPE IN (N'P'))
DROP PROCEDURE [dbo].[usp_CreateLocation]
GO
CREATE PROCEDURE usp_CreateLocation	
	@locationId BIGINT OUTPUT,
	@isActive BIT,
	@locationName NVARCHAR(100),
	@address1 NVARCHAR(100),
	@address2 NVARCHAR(100),
	@city NVARCHAR(50),
	@state NVARCHAR(2),
	@zipCode NVARCHAR(10),
	@channelPartnerId SMALLINT,
	@phoneNumber NVARCHAR(20),
	@bankId VARCHAR(40),
	@branchId VARCHAR(40),
	@noOfCounterId INT,
	@locationIdentifier VARCHAR(50),
	@timezone VARCHAR(100),
	@dTTerminatalCreate DATETIME,
	@dTServercreate DATETIME
	AS
BEGIN
    SET NOCOUNT ON;
		BEGIN TRY	 
			
		INSERT INTO
		 tLocations
		  (	
				IsActive,
				LocationName,
				Address1,
				Address2,
				City,
				[State],
				ZipCode,
				ChannelPartnerId,
				PhoneNumber,
				BankID,
				BranchId,
				LocationIdentifier,
				NoOfCounterIDs,
				TimezoneID,
				DTTerminalCreate,
				DTServerCreate
		  )
		 VALUES
		  (
				@isActive,
				@locationName,
				@address1,
				@address2, 
				@city, 
				@state,
				@zipCode, 
				@channelPartnerId,
				@phoneNumber, 
				@bankId,
				@branchId,
				@locationIdentifier,
				@noOfCounterId,
				@timezone,
				@dTTerminatalCreate,
				@dTServercreate
			)
		 SET @locationId = CAST(SCOPE_IDENTITY() AS BIGINT)	
	END TRY
	
	BEGIN CATCH	        
			EXECUTE usp_CreateErrorInfo;  
	END CATCH
END
GO

