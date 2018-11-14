-- ================================================================================
-- Author:		Nishad Varghese
-- Create date: 01/Oct/2016
-- Description:	Update MO status
-- Jira ID:		AL-7706
-- ================================================================================


IF OBJECT_ID(N'usp_CreateOrUpdateMoneyOrderImage', N'P') IS NOT NULL
    DROP PROCEDURE usp_CreateOrUpdateMoneyOrderImage   -- Drop the existing proc
GO

CREATE PROCEDURE usp_CreateOrUpdateMoneyOrderImage
(
	@transactionId BIGINT,
	@checkFrontImage VARBINARY(MAX),
	@checkBackImage VARBINARY(MAX),
	@dTServerDate DATETIME,
	@dTTerminalDate DATETIME
)
AS
BEGIN

  BEGIN TRY

     IF EXISTS
     (
         SELECT 1
         FROM tMoneyOrderImage
         WHERE TransactionId = @transactionId
     )
         BEGIN
             UPDATE tMoneyOrderImage
               SET
                   CheckFrontImage = @checkFrontImage,
                   CheckBackImage = @checkBackImage,
                   DTServerLastModified = @dTServerDate,
                   DTTerminalLastModified = @dTTerminalDate
             WHERE TransactionId = @transactionId
         END
     ELSE
     BEGIN
			  INSERT INTO tMoneyOrderImage
			  (
				CheckFrontImage,
				CheckBackImage,
				TransactionId,
				DTServerCreate,
				DTTerminalCreate
			  )
			  VALUES
			  (
			  @checkFrontImage,
			  @checkBackImage,
			  @transactionId,
			  @dTServerDate,
			  @dTTerminalDate
			  )
       END

    END TRY
	BEGIN CATCH
	
       EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError.

	END CATCH
END
GO