
CREATE TABLE [dbo].[tLimitTypes](
	[rowguid] [uniqueidentifier] NOT NULL,
	[id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[ComplianceProgramPK] [uniqueidentifier] NOT NULL,
	[ClassId] [nvarchar](50) NOT NULL,
	[TypeId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Value] [nvarchar](50) NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tLimitTypes] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tLimitTypes]  WITH CHECK ADD  CONSTRAINT [FK_tLimitTypes_tCompliancePrograms] FOREIGN KEY([ComplianceProgramPK])
REFERENCES [dbo].[tCompliancePrograms] ([rowguid])
GO

ALTER TABLE [dbo].[tLimitTypes] CHECK CONSTRAINT [FK_tLimitTypes_tCompliancePrograms]
GO

CREATE TABLE [dbo].[tLimits](
	[rowguid] [uniqueidentifier] NOT NULL,
	[id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[LimitTypePK] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[PerX] [money] NULL,
	[PerDay] [money] NULL,
	[PerNDays] [money] NULL,
	[NDays] [int] NULL,
	[IsDefault] [bit] NOT NULL,
	[CurrencyCode] [nvarchar](3) NULL,
	[MultipleNDaysLimits] [nvarchar](255) NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tLimits] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tLimits]  WITH CHECK ADD  CONSTRAINT [FK_tLimits_tLimitTypes] FOREIGN KEY([LimitTypePK])
REFERENCES [dbo].[tLimitTypes] ([rowguid])
GO

ALTER TABLE [dbo].[tLimits] CHECK CONSTRAINT [FK_tLimits_tLimitTypes]
GO


CREATE TABLE [dbo].[tLimitConditions](
	[rowguid] [uniqueidentifier] NOT NULL,
	[id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[LimitPK] [uniqueidentifier] NOT NULL,
	[TypeId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Value] [nvarchar](255) NULL,
	[AppliesToId] [int] NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tLimitConditions] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO