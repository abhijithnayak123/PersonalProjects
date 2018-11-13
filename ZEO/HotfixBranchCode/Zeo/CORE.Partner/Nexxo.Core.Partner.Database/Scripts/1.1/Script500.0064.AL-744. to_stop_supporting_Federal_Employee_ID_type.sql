--- ================================================================================
-- Author:		<Namit>
-- Create date: <07/152015>
-- Description:	<to stop supporting Federal Employee ID type>
-- Jira ID:		<AL-744>
-- ================================================================================
 
DELETE FROM	tChannelPartnerIDTypeMapping 
  
WHERE 
	 NexxoIdTypeId = '0F2CA0EC-7268-4A51-BFA1-19A0825FC4DA'
GO 