IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tNpsTerminalMapping_tNpsTerminals_NpsTerminalPK]') AND parent_object_id = OBJECT_ID(N'[dbo].[tNpsTerminalMapping]'))
ALTER TABLE [dbo].[tNpsTerminalMapping] DROP CONSTRAINT [FK_tNpsTerminalMapping_tNpsTerminals_NpsTerminalPK]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tNpsTerminalMapping]') AND type in (N'U'))
DROP TABLE [dbo].[tNpsTerminalMapping]
GO
