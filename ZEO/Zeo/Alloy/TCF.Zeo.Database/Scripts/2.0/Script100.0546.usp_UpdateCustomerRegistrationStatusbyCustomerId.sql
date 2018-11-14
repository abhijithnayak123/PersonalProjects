-- ================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <06/20/2017>
-- Description:	<SP to update Proflie status,cleintId and IsRCIFSuccess flag>
-- ================================================================================

IF EXISTS (SELECT  1 FROM sys.objects WHERE NAME = 'usp_UpdateCustomerRegistrationStatusbyCustomerId')
BEGIN
	DROP PROCEDURE usp_UpdateCustomerRegistrationStatusbyCustomerId
END
GO

CREATE PROCEDURE usp_UpdateCustomerRegistrationStatusbyCustomerId
@customerId BIGINT,
@clientId VARCHAR(15),
@errorReason varchar(max),
@status SMALLINT,
@isRCIFSuccess BIT,
@DTServerLastModified DATETIME,
@DTTerminalLastModified DATETIME
AS
BEGIN
	BEGIN TRY
	IF(@status = 1)
		UPDATE 
			tCustomers
		SET
		   IsRCIFSuccess = @isRCIFSuccess,
		   ProfileStatus = @status,
		   ClientID =  @clientId,
		   ErrorReason = @errorReason,
		   DTServerLastModified  = @DTServerLastModified,
		   DTTerminalLastModified = @DTTerminalLastModified
		WHERE 
			CustomerID = @customerId
	ELSE
		UPDATE 
			tCustomers
		SET
		   ErrorReason = @errorReason,
		   DTServerLastModified  = @DTServerLastModified,
		   DTTerminalLastModified = @DTTerminalLastModified
		WHERE 
			CustomerID = @customerId
	END TRY
	
	BEGIN CATCH	        
	 EXECUTE usp_CreateErrorInfo;  
	END CATCH
END