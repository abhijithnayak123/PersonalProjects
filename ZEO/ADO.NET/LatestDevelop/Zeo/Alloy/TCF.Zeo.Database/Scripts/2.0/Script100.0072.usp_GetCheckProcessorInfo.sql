--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <08-04-2016>
-- Description:	 Get Check Processor info by location(format of {ChannelPartnerId}-{BranchUserName})
-- Jira ID:		<AL-7705>
-- ================================================================================

IF OBJECT_ID(N'usp_GetCheckProcessorInfo', N'P') IS NOT NULL
DROP PROC usp_GetCheckProcessorInfo
GO

CREATE PROCEDURE usp_GetCheckProcessorInfo 
(
    @agentSessionId BIGINT, 
  	@location NVARCHAR(50)
)
AS
BEGIN	
 BEGIN TRY

	SELECT 
	   cp.URL,
	   cs.EmployeeId,
	   cs.CompanyToken
	FROM 
	  tChxr_Partner AS cp WITH (NOLOCK)
	INNER JOIN  
	  tChxr_Session AS cs WITH (NOLOCK)
	ON 
	  cp.ChxrPartnerId = cs.ChxrPartnerId
	WHERE 
	  cs.Location = @location

 END TRY
 BEGIN CATCH

	 EXECUTE usp_CreateErrorInfo

 END CATCH
END
GO
