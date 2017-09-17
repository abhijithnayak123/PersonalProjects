-- ================================================================================
-- Author:		<Shwetha Mohan>
-- Create date: <06/18/2015>
-- Description:	<Insert records in tChannelPartnerProductProcessorsMapping for TCF channel partner to map Money Order  >
-- Jira ID:		<AL-402>
-- ================================================================================
IF NOT EXISTS
	(
		SELECT 
			1 
		FROM 
			[tChannelPartnerProductProcessorsMapping] 
		WHERE 
			[ChannelPartnerId] = '6D7E785F-7BDD-42C8-BC49-44536A1885FC'
		AND [ProductProcessorId]= '1385D15F-B997-4A68-AF67-FE7DE0AE8B08'
		
	) 
BEGIN
INSERT INTO [tChannelPartnerProductProcessorsMapping]
           (
			   [rowguid]
			   ,[ChannelPartnerId]
			   ,[ProductProcessorId]
			   ,[Sequence]
			   ,[DTCreate]
			   ,[DTLastMod]
			   ,[IsTnCForcePrintRequired]
		   )
     VALUES
           (
			   NEWID(),
			   '6D7E785F-7BDD-42C8-BC49-44536A1885FC',
			   '1385D15F-B997-4A68-AF67-FE7DE0AE8B08',
			   6,
			   GETDATE(),
			   NULL,
			   0
		   )
END
GO
