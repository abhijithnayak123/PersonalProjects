-- =============================================
-- Author:	Karun
-- Create date: 06-Jun-2016
-- Description:	SP to get fis credential from database based on channel partnerid and bank id
-- =============================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_GetFISCredential'
)
BEGIN
	DROP PROCEDURE usp_GetFISCredential
END
GO

CREATE PROCEDURE usp_GetFISCredential
(
	@ChannelPartnerId BIGINT,
	@BankId VARCHAR(5)
)
AS
BEGIN

    BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;    
	SELECT 
		UserName,
		Password,
		ApplicationKey,
		ChannelKey,
		ChannelPartnerId,
		BankId,
		MetBankNumber 
	FROM 
		tFIS_Credential WITH (NOLOCK) 
	WHERE 
		ChannelPartnerId = @ChannelPartnerId 
		AND BankId = @BankId
	END TRY
	BEGIN CATCH

	   IF @@TRANCOUNT > 0 
       BEGIN   	  
           ROLLBACK TRANSACTION 		
       END
	   
       EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError

	END CATCH
END
GO