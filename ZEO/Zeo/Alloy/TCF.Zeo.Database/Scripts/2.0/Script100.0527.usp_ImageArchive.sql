--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <05-03-2017>
-- Description:	 Image Archival 
-- Jira ID:		<>
-- ================================================================================

--Dropping the existing SP for image archive and create with a new name.
IF OBJECT_ID('sp_archive_image_columns') IS NOT NULL
BEGIN
	DROP PROCEDURE sp_archive_image_columns
END
GO

IF OBJECT_ID('usp_ImageArchive') IS NOT NULL
BEGIN
	DROP PROCEDURE usp_ImageArchive
END
GO

CREATE PROCEDURE [dbo].[usp_ImageArchive]

	@Interval int = NULL

AS   

BEGIN 

DECLARE 

	@ErrorMessage    NVARCHAR(4000),
	@ErrorNumber     INT,
	@ErrorSeverity   INT,
	@ErrorState      INT,
	@ErrorLine       INT,
	@ErrorProcedure  NVARCHAR(200);

 BEGIN TRY

 BEGIN TRANSACTION


 -- SET NOCOUNT ON added to prevent extra result sets from  

 -- interfering with SELECT statements.

  SET NOCOUNT ON; 


IF @Interval IS NULL

	BEGIN

		SET @Interval = 95

	END

	BEGIN TRAN

		EXECUTE ('update tMoneyOrderImage

		set CheckFrontImage = NULL,

			CheckBackImage = NULL,

			DTServerLastModified = SYSDATETIME()

		from tMoneyOrderImage mo

		inner join tTxn_MoneyOrder m on m.TransactionID = mo.TransactionId

		where ISNULL(ISNULL(m.DTServerLastModified, m.DTServerCreate),m.DTTerminalCreate) <= dateadd(DD,-'+@Interval+',getdate()) 

		and (CheckFrontImage IS NOT NULL or CheckBackImage IS NOT NULL)')

	COMMIT TRAN


	BEGIN TRAN

		EXECUTE('update tCheckImages

			set Front = NULL,

				Back = NULL,

				DTServerLastModified = SYSDATETIME()

			from tCheckImages tc 

			inner join tTxn_Check tt on tc.TransactionId = tt.TransactionID

			where ISNULL(ISNULL(tt.DTServerLastModified,tt.DTServerCreate),tt.DTTerminalCreate) <= dateadd(DD,-'+@Interval+',getdate())

			and (Front IS NOT NULL or Back IS NOT NULL )')

	COMMIT TRAN


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

