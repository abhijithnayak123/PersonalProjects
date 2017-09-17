-- ============================================================
-- Author:		<Mohammed Khaja Firoz Molla>
-- Create date: <05/07/2015>
-- Description:	<DDL script to create tCertegy_Trx_AUD table>
-- Jira ID:	<AL-438>
-- ============================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tCertegy_Trx_AUD]') AND type in (N'U'))
DROP TABLE [dbo].[tCertegy_Trx_AUD]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tCertegy_Trx_AUD](
	[CertegyTrxPK] [uniqueidentifier] NULL,
	[CertegyTrxID] [bigint] NULL,
	[CheckAmount] [money] NULL,
	[CertegyFee] [money] NULL,
	[CheckDate] [datetime] NULL,
	[CheckNumber] [nvarchar](15)  NULL,
	[RoutingNumber] [nvarchar](20) NULL,
	[AccountNumber] [nvarchar](32) NULL,
	[Micr] [nvarchar](65) NULL,
	[CertegyUID] [nvarchar] (25)  NULL,
	[ApprovalNumber] [nvarchar] (25) NULL,
	[SettlementID] [nvarchar] (25) NULL,	
	[ResponseCode] [int] NULL,
	[SiteID] [nvarchar](20) NULL,
	[CertegyAccountPK] [uniqueidentifier] NULL,	
	[AlloySubmitType] [int] NULL,
	[AlloyReturnType] [int] NULL,
	[CertegySubmitType] [char] (1) NULL,
	[CertegyReturnType] [char] (1) NULL,
	[TranType] [char] (1) NULL,
	[FundsAvail] [char] (1) NULL,
	[Version] [nvarchar] (6) NULL,
	[IdType] [char] (2) NULL,
	[ExpansionType] [char] (4) NULL,
	[MICREntryType] [char] (1) NULL,
	[DeviceType] [char] (1) NULL,
	[DeviceId] [nvarchar] (40) NULL,
	[DeviceIP] [nvarchar] (15) NULL,
	[ChannelPartnerID] [bigint] NULL,	
	[IsCheckFranked] [bit] NULL,
	[DTTerminalCreate] [datetime] NULL,
	[DTTerminalLastModified] [datetime] NULL,
	[DTServerCreate] [datetime] NULL,
	[DTServerLastModified] [datetime] NULL,
	[AuditEvent] [smallint] NULL,
	[DTAudit] [datetime] NULL,
	[RevisionNo] [bigint] NULL,
    ) 

GO


