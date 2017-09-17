--- ================================================================================
-- Author:		<Divya Boddu>
-- Create date: <05/24/2016>
-- Description:	<Implement Changes to Optimize the image size in PTNR DB Size.>
--              <Dropping tChxr_CheckImages table as it has redundent data of tCheckImages table>
-- Jira ID:		<AL-6291>
-- ================================================================================

IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tChxr_CheckImages]') AND type in (N'U'))
DROP TABLE [dbo].[tChxr_CheckImages]
GO


