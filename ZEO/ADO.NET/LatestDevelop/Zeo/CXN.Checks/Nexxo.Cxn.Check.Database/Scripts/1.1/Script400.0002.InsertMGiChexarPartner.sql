--===========================================================================================
-- Auther:			<Bineesh Raghavan>
-- Date Created:	03/27/2015
-- Description:		<Script for insert record into [tChxr_Partner] for MGI>
--===========================================================================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tChxr_Partner]') 
AND type in (N'U'))
BEGIN
	IF NOT EXISTS(SELECT * FROM [dbo].[tChxr_Partner] WHERE [Id] = 1)
	BEGIN
		INSERT tChxr_Partner 
		VALUES (NEWID(), 'MoneyGram', 'http://beta.chexar.net/webservice/', GETDATE(), NULL, 1)
	END
END
Go
--===========================================================================================