--===========================================================================================
-- Author:		<Rita Patel>
-- Create date: <Mar 13 2015>
-- Description:	<Script to create MasterCountries table>
-- Jira ID:		<AL-139>
--===========================================================================================

IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tMasterCountries]') AND type in (N'U'))
DROP TABLE [dbo].[tMasterCountries]
GO

CREATE TABLE [dbo].[tMasterCountries](
	[rowguid] [uniqueidentifier] NOT NULL PRIMARY KEY,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Abbr2] [char](2) NOT NULL,
	[Abbr3] [char](3) NOT NULL,
	[DTCreate] [datetime]  NOT NULL DEFAULT GETDATE(),
	[DTLastMod] [datetime] NULL
	)
GO
