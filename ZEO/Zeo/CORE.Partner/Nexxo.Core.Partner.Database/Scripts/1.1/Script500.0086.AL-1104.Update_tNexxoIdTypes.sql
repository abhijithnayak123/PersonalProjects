--- ================================================================================
-- Author:		<Chinar Kulkarni>
-- Create date: <08/20/2015>
-- Description:	<Update the default Id Types to accept up to 20 characters (instead of 15)>
-- Jira ID:		<AL-1104>
-- ================================================================================

  UPDATE tNexxoIdTypes SET Mask = '^\w{4,20}$'
  WHERE Mask = '^\w{4,15}$'
    
  UPDATE tNexxoIdTypes SET Mask = '^[\w-*]{4,20}$'
  WHERE Mask = '^[\w-*]{4,15}$'

