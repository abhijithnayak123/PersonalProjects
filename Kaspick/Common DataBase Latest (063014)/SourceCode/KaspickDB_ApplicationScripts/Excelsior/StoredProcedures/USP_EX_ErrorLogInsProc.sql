IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_ErrorLogInsProc'
		)
BEGIN
	DROP PROCEDURE USP_EX_ErrorLogInsProc;

	PRINT 'DROPPED USP_EX_ErrorLogInsProc';
END
GO
/******************************************************************************  
** Procedure Name:  USP_EX_ErrorLogInsProc  
** Old Procedure Name:  TBL_EIS_ERRORLOGInsProc
**
**  
** Description:  Writes data to the errors table  
**  
** Created By:  
** Company   : Kaspick & Company  
** Project   : Excelsior  
** Created DT:   
**  
*******************************************************************************  
**       Change History  
*******************************************************************************  
** Date/Version  Author:  Bug #  Description:  
** --------   -------- -------------------------------------------  
** 7/25/2014 Modified  Mallikarjun  Modified as per kaspick db
*******************************************************************************  
** Copyright (C) 2007 Kaspick & Company, All Rights Reserved  
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION  
*******************************************************************************/
  
CREATE PROCEDURE [dbo].[USP_EX_ErrorLogInsProc]  
(  
  @Err_Module_Name varchar(50),  
  @Err_Date_Time Datetime,  
  @Err_Number  Decimal(18,0),  
  @Err_Source varchar(100),  
  @Err_Description varchar(4000),  
  @Err_Login_Name  varchar(50),  
  @Err_Line_No int,
  @Err_PhysicalMachineName char(20),  
  @Err_CurrentObject_MethodCall varchar(100),  
  @Err_DLL_Version_No varchar(50),  
  @Err_Application_Name  varchar(100),  
  @Err_Stacktrace_Info varchar(1000),  
  @Err_Severity int=NULL  
       )   
AS  
BEGIN  
    --BEGIN TRAN 
    -- Variables used for error handling -     
    DECLARE @procname VARCHAR(60);
    DECLARE @ErrorMessage VARCHAR(1000);
	DECLARE @ErrorNumber INT; 
	DECLARE @val1 VARCHAR(Max);
  	
  	BEGIN TRY
	
    INSERT INTO TBL_BR_ErrorLog(  
           Err_Module_Name,  
		   Err_Date_Time,  
           Err_Number,  
           Err_Source,  
           Err_Description,  
           Err_Login_Name,  
           Err_PhysicalMachineName,  
           Err_CurrentObject_MethodCall,  
           Err_DLL_Version_No,  
           Err_Application_Name,  
           Err_Stacktrace_Info,  
           Err_Severity,  
           Err_Line_No )  
    VALUES(@Err_Module_Name,  
     GETDATE(),  
           @Err_Number,  
           @Err_Source,  
           @Err_Description,  
           @Err_Login_Name,  
           @Err_PhysicalMachineName,  
           @Err_CurrentObject_MethodCall,  
           @Err_DLL_Version_No,  
           @Err_Application_Name,  
           @Err_Stacktrace_Info,  
           @Err_Severity,  
           @Err_Line_No  )  
  
 
       END TRY

	BEGIN CATCH
		SET @ErrorMessage = ERROR_MESSAGE();
		SET @ErrorNumber = ERROR_NUMBER();
		--SET @val1 = @ROLE;

		EXEC USP_EX_SYSErrorHandler @codename = @procname
			,@ErrorMessage = @ErrorMessage
			,@ErrorNumber = @ErrorNumber
			,@val1 = @val1;
			--,@val1str = 'Role';
	END CATCH;
  
END  
 GO

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_ErrorLogInsProc'
		)
BEGIN
	PRINT 'CREATED USP_EX_ErrorLogInsProc';
END 
  