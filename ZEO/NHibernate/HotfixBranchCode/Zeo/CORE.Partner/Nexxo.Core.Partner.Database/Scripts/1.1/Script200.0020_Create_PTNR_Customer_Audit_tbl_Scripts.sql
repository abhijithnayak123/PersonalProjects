-- ============================================================
-- Author:		<Sunil Shetty>
-- Create date: <21/03/2014>
-- Description:	<Create new table 'tPartnerCustomers_Aud'>
-- Rally ID:	<US1919 - TA4567: Add Teller/User Username and ID to ODS>
-- ============================================================
IF NOT EXISTS
(
	SELECT 
		1 
	FROM 
		dbo.sysobjects
	WHERE 
		name = N'tPartnerCustomers_Aud'
)
BEGIN
CREATE TABLE [dbo].[tPartnerCustomers_Aud](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL,
	[CXEId] [bigint] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
	[IsPartnerAccountHolder] [bit] NULL,
	[ReferralCode] [nvarchar](16) NULL,
	[ChannelPartnerId] [uniqueidentifier] NULL,
	[AuditEvent] [smallint] NOT NULL,
	[DTAudit] [datetime] NOT NULL,
	[RevisionNo] [bigint] NULL,
	[AgentSessionId] [uniqueidentifier] NULL
) ON [PRIMARY]
END
GO


