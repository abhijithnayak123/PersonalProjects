-- ============================================================
-- Author:		<Mohammed Khaja Firoz Molla>
-- Create date: <07/15/2015>
-- Description:	<DDL script to create tCertegy_Account table>
-- Jira ID:	<AL-438>
-- ============================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tCertegy_Account]') AND type in (N'U'))
	DBCC CHECKIDENT (tCertegy_Account, RESEED, 2000000000)
GO

