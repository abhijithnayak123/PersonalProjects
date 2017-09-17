--- ===============================================================================
-- Author:		 <M.Purna Pushkal>
-- Create date: <28/11/2016>
-- Description: Create WU transaction
-- Jira ID:		 <AL-8320>
-- ================================================================================
IF OBJECT_ID('usp_CreateOrUpdateWUBillPayTransaction') IS NOT NULL
BEGIN
	DROP PROCEDURE dbo.usp_CreateOrUpdateWUBillPayTransaction
END
GO
CREATE PROCEDURE usp_CreateOrUpdateWUBillPayTransaction 
(
	 @wuTrxID											BIGINT OUTPUT,	
	 @wuBillPayTrxID                                     BIGINT,
	 @wUBillPayAccountID                                 BIGINT,
	 @dTServerCreate                                     DATETIME,
	 @providerId                                         INT, 
	 @channelType                                        VARCHAR(10), 
	 @channelName                                        VARCHAR(20), 
	 @channelVersion                                     VARCHAR(10), 
	 @senderFirstName                                    VARCHAR(50), 
	 @senderLastname                                     VARCHAR(50), 
	 @senderAddressLine1                                 VARCHAR(50), 
	 @senderCity                                         VARCHAR(50), 
	 @senderState                                        VARCHAR(50), 
	 @senderPostalCode                                   VARCHAR(50), 
	 @senderAddressLine2                                 VARCHAR(50), 
	 @senderStreet                                       VARCHAR(50), 
	 @westernUnionCardNumber                             VARCHAR(15), 
	 @senderDateOfBirth                                  VARCHAR(50), 
	 @billerName                                         VARCHAR(50), 
	 @customerAccountNumber                              VARCHAR(50), 
	 @foreignRemoteSystemIdentifier                      VARCHAR(20),                                            
	 @foreignRemoteSystemReference_no                    VARCHAR(50),                                           
	 @foreignRemoteSystemCounterId                       VARCHAR(20),                                             
	 @dTTerminalCreate                                   DATETIME                                           
)                                     
AS
BEGIN
	BEGIN TRY
		IF EXISTS(SELECT 1 FROM dbo.tWUnion_BillPay_Trx WHERE WUBillPayTrxID = @wuBillPayTrxID)
		BEGIN
			UPDATE 
				tWUnion_BillPay_Trx
			SET
				BillerName = @billerName,
				Customer_AccountNumber = @customerAccountNumber
			WHERE 
				WUBillPayTrxID = @wuBillPayTrxID
		
			SET @wuTrxID = @wuBillPayTrxID
		END
		ELSE
		BEGIN
			INSERT INTO dbo.tWUnion_BillPay_Trx
			(
				ProviderId,
				Channel_Type,
				Channel_Name,
				Channel_Version,
				Sender_FirstName,
				Sender_Lastname,
				Sender_AddressLine1,
				Sender_City,
				Sender_State,
				Sender_PostalCode,
				Sender_AddressLine2,
				Sender_Street,
				WesternUnionCardNumber,
				Sender_DateOfBirth,
				BillerName,
				Customer_AccountNumber,
				ForeignRemoteSystem_Identifier,
				ForeignRemoteSystem_Reference_no,
				ForeignRemoteSystem_CounterId,
				DTTerminalCreate,
				DTServerCreate,
				WUBillPayAccountID
			)
			VALUES
			(
				@providerId,
				@channelType,
				@channelName,
				@channelVersion,
				@senderFirstName,
				@senderLastname,
				@senderAddressLine1,
				@senderCity,
				@senderState,
				@senderPostalCode,
				@senderAddressLine2,
				@senderStreet,
				@westernUnionCardNumber,
				@senderDateOfBirth,
				@billerName,
				@customerAccountNumber,
				@foreignRemoteSystemIdentifier,
				@foreignRemoteSystemReference_no,
				@foreignRemoteSystemCounterId,
				@dTTerminalCreate,
				@dTServerCreate,
				@wUBillPayAccountID
			)

			SELECT @wuTrxID = CAST(SCOPE_IDENTITY() AS BIGINT)
		END
	END TRY

	BEGIN CATCH
		 EXECUTE dbo.usp_CreateErrorInfo;
	END CATCH
END
GO
 