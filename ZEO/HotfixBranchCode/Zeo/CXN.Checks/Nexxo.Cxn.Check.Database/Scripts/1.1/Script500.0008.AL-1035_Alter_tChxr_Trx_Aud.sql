-- ============================================================================
-- Author:		<Sunil Shetty>
-- Create date: <08/07/2015>
-- Description:	<Script to alter message column width as decline message for decline code 2 was found having length greater then 200>
-- Jira ID:	<AL-1035>
-- =============================================================================
IF EXISTS 
(
  SELECT 
      1 
  FROM   
      sys.columns 
  WHERE  
      object_id = OBJECT_ID(N'[dbo].[tChxr_Trx_aud]')
    AND name = 'Message'
)
	BEGIN  
		ALTER TABLE [dbo].[tChxr_Trx_aud] 
		ALTER COLUMN Message nvarchar(255) NULL
	END
GO