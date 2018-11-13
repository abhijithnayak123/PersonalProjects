-- ================================================================
-- Author:		Kaushik Sakala
-- Create date: <24/06/2015>
-- Description:	<As a product owner, I should have the ability to 
--				configure multiple limits for each client.>
-- JIRA ID:	<AL-594>
-- =================================================================

--Adding the Table tLimitTypes as per AL-594

CREATE TABLE [tLimitTypes](
	[LimitTypePK] [uniqueidentifier] NOT NULL,
	[LimitTypeID] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[ComplianceProgramPK] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[DTServerCreate] [datetime] NOT NULL,
	[DTServerLastModified] [datetime] NULL,
 CONSTRAINT [PK_tLimitTypes] PRIMARY KEY CLUSTERED 
(
	[LimitTypePK] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [tLimitTypes]  WITH CHECK ADD  CONSTRAINT [FK_tLimitTypes_tCompliancePrograms] FOREIGN KEY([ComplianceProgramPK])
REFERENCES [tCompliancePrograms] ([ComplianceProgramPK])
GO

ALTER TABLE [tLimitTypes] CHECK CONSTRAINT [FK_tLimitTypes_tCompliancePrograms]
GO
