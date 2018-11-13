
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tStates_tCountries]') AND parent_object_id = OBJECT_ID(N'[dbo].[tStates]'))
ALTER TABLE [dbo].[tStates] DROP CONSTRAINT [FK_tStates_tCountries]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tCountries]') AND type in (N'U'))
DROP TABLE [dbo].[tCountries]
GO

CREATE TABLE [dbo].[tCountries](
	[id] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Code] [int] NOT NULL,
	[Name] [nvarchar](70) NOT NULL,
	[Abbr2] [char](2) NOT NULL,
	[Abbr3] [char](3) NOT NULL,
	[xRate] [money] NULL,
	[DTRateAsOf] [datetime] NULL,
	[Currency_Name] [nvarchar](20) NULL,
	[Curr_CD_Alpha] [nvarchar](3) NULL,
	[Curr_CD_Num] [int] NULL,
	[Symbol] [nvarchar](5) NULL,
	[DTLastMod] [datetime] NULL,
	[DTCreate] [datetime] NULL,
	[CallingCode] [int] NULL,
	[IsCOB] [bit] NULL,
	[DebitAllowed] [bit] NULL,
 CONSTRAINT [PK_tCountries] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO

