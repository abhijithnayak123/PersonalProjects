--- ================================================================================
-- Author:		<Anisha Abraham>
-- Create date: <08/03/2015>
-- Description:	<EAD Card Text not spelled correctly in the ID Type dropdown field>
-- Jira ID:		<AL-905>
-- ================================================================================
IF EXISTS(SELECT 1 FROM [dbo].[tNexxoIdTypes] WHERE NexxoIdTypeID = 160)
BEGIN
	UPDATE [dbo].[tNexxoIdTypes]
	SET 
		[Name]='EMPLOYMENT AUTHORIZATION CARD (EAD)'
	WHERE 
		NexxoIdTypeID = 160
END
