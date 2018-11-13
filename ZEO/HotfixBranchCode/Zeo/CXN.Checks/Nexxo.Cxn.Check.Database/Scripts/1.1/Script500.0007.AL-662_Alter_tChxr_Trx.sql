-- ============================================================================
-- Author:		<Sunil Shetty>
-- Create date: <07/15/2015>
-- Description:	<Script to alter message column width as decline message for decline code 2 was found having length greater then 200>
-- Jira ID:	<AL-662>
-- =============================================================================
IF EXISTS 
(
  SELECT 
      1 
  FROM   
      sys.columns 
  WHERE  
      object_id = OBJECT_ID(N'[dbo].[tChxr_Trx]') 
    AND name = 'Message'
)
BEGIN  
ALTER TABLE [dbo].[tChxr_Trx] ALTER COLUMN Message nvarchar(255) NULL
END
GO