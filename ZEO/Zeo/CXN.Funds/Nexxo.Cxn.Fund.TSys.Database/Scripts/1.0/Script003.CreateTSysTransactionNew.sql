IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tTSys_Trx_tTSys_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[tTSys_Trx_New]'))
	ALTER TABLE [tTSys_Trx_New] DROP CONSTRAINT [FK_tTSys_Trx_New_tTSys_Account]
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tTSys_Trx_New]') AND type in (N'U'))
	DROP TABLE [dbo].[tTSys_Trx_New]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tTSys_Trx_New]
(
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[AccountPK] [uniqueidentifier] NOT NULL,
	[TransactionType] [int] NOT NULL,
	[Amount] [money] NOT NULL,
	[Fee] [money] NULL,
	[Description] [nvarchar](200) NULL,
	[DTLocalTransaction] [datetime] NULL,
	[DTTransmission] [datetime] NULL,
	[Status] [int] NOT NULL,
	[ErrorCode] [nvarchar](50) NULL,
	[ErrorMsg] [nvarchar](50) NULL,
	[ConfirmationId] [nvarchar](50) NULL,
	[Balance] [money] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tTSys_Trx_New] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tTSys_Trx_New]  WITH CHECK ADD CONSTRAINT [FK_tTSys_Trx_New_tTSys_Account] FOREIGN KEY([AccountPK])
	REFERENCES [dbo].[tTSys_Account] ([rowguid])
GO

ALTER TABLE [dbo].[tTSys_Trx_New] CHECK CONSTRAINT [FK_tTSys_Trx_New_tTSys_Account]
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_tTSys_Trx_New_Id] ON [dbo].[tTSys_Trx_New] 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

SET IDENTITY_INSERT tTSys_Trx OFF
GO

SET IDENTITY_INSERT tTSys_Trx_New ON
GO


INSERT INTO tTSys_Trx_New (rowguid,Id,AccountPK,TransactionType,Amount,Fee,[Description],
DTLocalTransaction,DTTransmission,[Status],ErrorCode,ErrorMsg,ConfirmationId,
Balance,DTCreate,DTLastMod) 
SELECT rowguid,Id,AccountPK,TransactionType,Amount,Fee,[Description],
DTLocalTransaction,DTTransmission,[Status],ErrorCode,ErrorMsg,ConfirmationId,
Balance,DTCreate,DTLastMod FROM dbo.tTSys_Trx

 
SET IDENTITY_INSERT tTSys_Trx_New OFF
GO


SP_RENAME 'tTSys_Trx','tTSys_Trx_Old'
GO

SP_RENAME 'tTSys_Trx_New','tTSys_Trx'
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[tTSys_Trx_Old]') AND type in (N'U'))
	DROP TABLE tTSys_Trx_Old
GO

EXEC sp_rename @objname = N'PK_tTSys_Trx_New', @newname = N'PK_tTSys_Trx';

EXEC sp_rename @objname = N'FK_tTSys_Trx_New_tTSys_Account', @newname = N'FK_tTSys_Trx_tTSys_Account';

