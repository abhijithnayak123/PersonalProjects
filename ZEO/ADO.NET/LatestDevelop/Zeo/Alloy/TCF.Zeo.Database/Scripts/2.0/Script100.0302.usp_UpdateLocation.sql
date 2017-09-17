-- =============================================
-- Author:		<Shwetha Mohan>
-- Create date: <12/12/2016>	
-- Description:	Store Proc to Update Location
-- Jira ID:		<AL-7582>
-- =============================================

IF EXISTS (SELECT 1 FROM SYS.PROCEDURES WHERE object_id = OBJECT_ID(N'[dbo].[usp_UpdateLocation]') AND TYPE IN (N'P'))
DROP PROCEDURE [dbo].[usp_UpdateLocation]
GO

CREATE PROCEDURE usp_UpdateLocation 
(
	@isActive BIT,
	@locationName NVARCHAR(100),
	@address1 nvarchar(100),
	@address2 nvarchar(100),
	@city nvarchar(50),
	@state nvarchar(2),
	@zipCode nvarchar(10),
	@dTServerLastModified DATETIME,
	@channelPartnerId SMALLINT,
	@phoneNumber NVARCHAR(20),
	@bankId VARCHAR(40),
	@branchId VARCHAR(40),
	@timeZoneId VARCHAR(100),
	@locationID VARCHAR(50),
	@numberOfCounterId INT,
	@locationIdentifier VARCHAR(50),
	@dTTerminatalLastModified DATETIME
)
AS
BEGIN
	
	SET NOCOUNT ON;
	BEGIN TRY

		UPDATE tLocations
		SET
 			IsActive = @iSActive,
			LocationName=@locationName ,
			Address1 = @address1,
			Address2= @address2,
			City = @city,
			[State] = @state,
			ZipCode= @zipCode,
			DTServerLastModified = @dTServerLastModified,
			ChannelPartnerId = @channelPartnerId,
			PhoneNumber = @phoneNumber,
			BankID = @bankId,
			BranchID = @branchId,
			TimezoneID = @timeZoneId,
			NoOfCounterIDs=	@numberOfCounterId,
			LocationIdentifier= @locationIdentifier,
			DTTerMinalLastModified	= @dTTerminatalLastModified

		WHERE 
			LocationId = @locationID AND ChannelPartnerId = @channelPartnerId		
		
	END TRY
	BEGIN CATCH	        
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END
GO
