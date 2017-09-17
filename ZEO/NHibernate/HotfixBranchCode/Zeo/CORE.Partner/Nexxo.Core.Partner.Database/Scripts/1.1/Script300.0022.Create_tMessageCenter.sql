--===========================================================================================
-- Author:		Sunil Shetty
-- Create date: <14/05/2014>
-- Description:	<Script for creating MessageCenter table>
-- Rally ID:	<US1988>
--===========================================================================================

IF  EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[FK_tMessageCenter_AgentId]') AND type = 'F')
BEGIN
ALTER TABLE [dbo].[tMessageCenter] DROP CONSTRAINT [FK_tMessageCenter_AgentId]
END
GO 

IF  EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[FK_tMessageCenter_TxnId]') AND type = 'F')
BEGIN
ALTER TABLE [dbo].[tMessageCenter] DROP CONSTRAINT [FK_tMessageCenter_TxnId]
END
GO

IF  EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tMessageCenter_IsParked]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tMessageCenter] DROP CONSTRAINT [DF_tMessageCenter_IsParked]
END
GO

IF  EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tMessageCenter_IsActive]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tMessageCenter] DROP CONSTRAINT [DF_tMessageCenter_IsActive]
END
GO


IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tMessageCenter]') AND type in (N'U'))
DROP TABLE [dbo].[tMessageCenter]
GO

CREATE TABLE [dbo].[tMessageCenter](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[AgentId] [uniqueidentifier] NOT NULL,
	[TxnId] [uniqueidentifier] NOT NULL,
	[IsParked] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
	[DTServerCreate] [datetime] NULL,
	[DTServerLastMod] [datetime] NULL
 CONSTRAINT [PK_tMessageCenter] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tMessageCenter]  WITH CHECK ADD  CONSTRAINT [FK_tMessageCenter_AgentId] FOREIGN KEY([AgentId])
REFERENCES [dbo].[tAgentDetails] ([rowguid])
GO

ALTER TABLE [dbo].[tMessageCenter] CHECK CONSTRAINT [FK_tMessageCenter_AgentId]
GO

ALTER TABLE [dbo].[tMessageCenter]  WITH CHECK ADD  CONSTRAINT [FK_tMessageCenter_TxnId] FOREIGN KEY([TxnId])
REFERENCES [dbo].[tTxn_Check] ([txnRowguid])
GO

ALTER TABLE [dbo].[tMessageCenter] CHECK CONSTRAINT [FK_tMessageCenter_TxnId]
GO

ALTER TABLE [dbo].[tMessageCenter] ADD  CONSTRAINT [DF_tMessageCenter_IsParked]  DEFAULT ((0)) FOR [IsParked]
GO

ALTER TABLE [dbo].[tMessageCenter] ADD  CONSTRAINT [DF_tMessageCenter_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO


