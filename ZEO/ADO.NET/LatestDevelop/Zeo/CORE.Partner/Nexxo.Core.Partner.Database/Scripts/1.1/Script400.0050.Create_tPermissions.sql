--===========================================================================================
-- Author:		<Mohammed Khaja Firoz Molla>
-- Create date: <Mar 3rd 2015>
-- Description:	<Script to create permission table to store permission for a role>
-- Jira ID:	<AL-88>
--===========================================================================================

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tPermissions]') AND type in (N'U'))
DROP TABLE [dbo].[tPermissions]
GO

CREATE TABLE tPermissions
(
	rowguid UNIQUEIDENTIFIER,
	Id BIGINT NOT NULL IDENTITY(1000000000,1),
	Permission NVARCHAR(255) NOT NULL,
	DTCreate DATETIME NOT NULL,
	DTLastMod DATETIME NULL,
	CONSTRAINT PK_tPermissions PRIMARY KEY (rowguid)
)
GO