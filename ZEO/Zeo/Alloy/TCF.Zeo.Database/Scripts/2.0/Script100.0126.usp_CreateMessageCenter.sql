--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <08-04-2016>
-- Description:	 Create message center if check is Pending status
-- Jira ID:		<AL-7705>
-- ================================================================================

IF OBJECT_ID(N'usp_CreateMessageCenter', N'P') IS NOT NULL
DROP PROC usp_CreateMessageCenter
GO

CREATE PROCEDURE usp_CreateMessageCenter
	@transactionId BIGINT,		
	@dTTerminalCreate DATETIME,
	@dTServerServerCreate DATETIME
AS
BEGIN	
 
    BEGIN TRY

    DECLARE @agentId BIGINT 
    DECLARE @isParked BIT = 0
    DECLARE @IsActive BIT = 1    
    
    If NOT EXISTS (SELECT 1 FROM tMessageCenter WITH (NOLOCK) WHERE TransactionId = @transactionId)
    BEGIN
    
    	    SELECT @agentId = AgentID 
    		FROM 
    		  tAgentSessions a WITH (NOLOCK)
    		JOIN
    		  tCustomerSessions cs WITH (NOLOCK)
    		ON
    		  cs.AgentSessionId = a.AgentSessionID
    		JOIN 
    		  tTxn_Check tc WITH (NOLOCK)
    		ON
    		  tc.CustomerSessionId = cs.CustomerSessionID
    		WHERE 
    		  tc.TransactionID = @transactionId
    
    		INSERT INTO
    		   tMessageCenter
    		   (
       			 IsParked,
       			 IsActive,
       			 AgentId,
       			 TransactionId,
       			 DTTerminalCreate,
       			 DTServerCreate			
    		   )
    		VALUES
    		   (
    			 @isParked,
    			 @IsActive,
    			 @agentId,
    			 @transactionId,
    			 @dTTerminalCreate,
    			 @dTServerServerCreate		   
    		   )
    END

END TRY

BEGIN CATCH
    
	EXECUTE usp_CreateErrorInfo
		
END CATCH
END
GO



