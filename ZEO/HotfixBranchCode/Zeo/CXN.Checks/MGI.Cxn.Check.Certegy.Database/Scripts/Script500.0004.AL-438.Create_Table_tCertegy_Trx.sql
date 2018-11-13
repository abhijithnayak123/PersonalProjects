-- ============================================================
-- Author:		<Mohammed Khaja Firoz Molla>
-- Create date: <05/07/2015>
-- Description:	<DDL script to create tCertegy_Trx table>
-- Jira ID:	<AL-438>
-- ============================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tCertegy_Trx]') AND type in (N'U'))
DROP TABLE [dbo].[tCertegy_Trx]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tCertegy_Trx](
	[CertegyTrxPK] [uniqueidentifier] NOT NULL,
	[CertegyTrxID] [bigint] NOT NULL,
	[CheckAmount] [money] NOT NULL,
	[CertegyFee] [money] NULL,
	[CheckDate] [datetime] NOT NULL,
	[CheckNumber] [nvarchar](15)  NULL,
	[RoutingNumber] [nvarchar](20) NULL,
	[AccountNumber] [nvarchar](32) NULL,
	[Micr] [nvarchar](65) NOT NULL,
	[CertegyUID] [nvarchar] (25)  NULL,
	[ApprovalNumber] [nvarchar] (25) NULL,
	[SettlementID] [nvarchar] (25) NULL,	
	[ResponseCode] [int] NULL,
	[SiteID] [nvarchar](20) NOT NULL,
	[CertegyAccountPK] [uniqueidentifier] NOT NULL,	
	[AlloySubmitType] [int] NOT NULL,
	[AlloyReturnType] [int] NOT NULL,
	[CertegySubmitType] [char] (1) NOT NULL,
	[CertegyReturnType] [char] (1) NOT NULL,
	[TranType] [char] (1) NULL CONSTRAINT "df_Tran_Type" DEFAULT 'C',
	[FundsAvail] [char] (1) NULL CONSTRAINT "df_Funds_Avail" DEFAULT 'I',
	[Version] [nvarchar] (6) NOT NULL,
	[IdType] [char] (2) NOT NULL,
	[ExpansionType] [char] (4) NULL CONSTRAINT "df_Exansion_Type" DEFAULT 'TOAD',
	[MICREntryType] [char] (1) NOT NULL,
	[DeviceType] [char] (1) NOT NULL,
	[DeviceId] [nvarchar] (40) NOT NULL,
	[DeviceIP] [nvarchar] (15) NOT NULL,
	[ChannelPartnerID] [bigint] NOT NULL,	
	[IsCheckFranked] [bit] NOT NULL CONSTRAINT "df_Is_Check_Franked" DEFAULT ((0)),
	[DTTerminalCreate] [datetime] NOT NULL,
	[DTTerminalLastModified] [datetime] NULL,
	[DTServerCreate] [datetime] NOT NULL,
	[DTServerLastModified] [datetime] NULL,
 CONSTRAINT [PK_Certegy_Trx] PRIMARY KEY CLUSTERED 
(
	[CertegyTrxPK] ASC
)WITH (FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tCertegy_Trx]  WITH CHECK ADD  CONSTRAINT [FK_tCertegy_Trx_tCertegy_Account] FOREIGN KEY([CertegyAccountPK])
REFERENCES [dbo].[tCertegy_Account] ([CertegyAccountPK])
GO

ALTER TABLE [dbo].[tCertegy_Trx] CHECK CONSTRAINT [FK_tCertegy_Trx_tCertegy_Account]
GO


