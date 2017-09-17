--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <07-22-2016>
-- Description:	 Create table to log the erros
-- Jira ID:		<AL-7582>
-- ================================================================================

IF OBJECT_ID(N'usp_CreateErrorInfo', N'P') IS NOT NULL
BEGIN
	DROP PROCEDURE usp_CreateErrorInfo
END
GO


CREATE PROCEDURE usp_CreateErrorInfo
AS
BEGIN


	DECLARE @ErrorNumber    INT
	DECLARE @ErrorSeverity  INT
	DECLARE @ErrorState     INT
	DECLARE @ErrorLine      INT
	DECLARE @ErrorMessage   NVARCHAR(MAX)
	DECLARE @ErrorProcedure NVARCHAR(1000)


	SELECT   
	     @ErrorNumber = ERROR_NUMBER()  
	    ,@ErrorSeverity = ERROR_SEVERITY() 
	    ,@ErrorState = ERROR_STATE() 
	    ,@ErrorProcedure = ERROR_PROCEDURE()
	    ,@ErrorLine = ERROR_LINE () 		  
	    ,@ErrorMessage = ERROR_MESSAGE() 

	--  Raise the error Message
	RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState)

END
GO
