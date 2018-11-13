-- ================================================================
-- Author:		Kaushik Sakala
-- Create date: <24/06/2015>
-- Description:	<As a product owner, I should have the ability to 
--				configure multiple limits for each client.>
-- JIRA ID:	<AL-594>
-- =================================================================

--Adding the Table tLimits as per AL-594
CREATE TABLE [tLimits](
	[LimitPK] [uniqueidentifier] NOT NULL,
	[LimitID] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[LimitTypePK] [uniqueidentifier] NOT NULL,
	[PerTransactionMaximum] [money] NULL,
	[PerTransactionMinimum] [money] NULL,
	[RollingLimits] [nvarchar](255) NULL,
	[DTServerCreate] [datetime] NOT NULL,
	[DTServerLastModified] [datetime] NULL,
 CONSTRAINT [PK_tLimits] PRIMARY KEY CLUSTERED 
(
	[LimitPK] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [tLimits]  WITH CHECK ADD  CONSTRAINT [FK_tLimits_tLimitTypes] FOREIGN KEY([LimitTypePK])
REFERENCES [tLimitTypes] ([LimitTypePK])
GO

ALTER TABLE [tLimits] CHECK CONSTRAINT [FK_tLimits_tLimitTypes]
GO


