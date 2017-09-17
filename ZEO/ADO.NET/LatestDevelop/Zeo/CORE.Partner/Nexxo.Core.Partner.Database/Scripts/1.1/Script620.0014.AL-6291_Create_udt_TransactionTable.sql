--- ================================================================================
-- Author:		<Divya Boddu>
-- Create date: <05/23/2016>
-- Description:	<Implement Changes to Optimize the image size in PTNR DB Size.>
-- Jira ID:		<AL-6291>
-- ================================================================================

IF  EXISTS (SELECT 1 FROM sys.types WHERE is_user_defined = 1 AND name = 'TransactionTable')
DROP TYPE [dbo].[TransactionTable]
GO

CREATE TYPE [dbo].[TransactionTable] AS TABLE(
	TransactionId BIGINT NULL,
	FrontImagePath VARCHAR(1000) NULL,
	BackImagePath VARCHAR(1000) NULL
)
GO


