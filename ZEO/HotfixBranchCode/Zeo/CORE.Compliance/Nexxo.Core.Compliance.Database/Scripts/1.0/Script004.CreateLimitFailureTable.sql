
CREATE TABLE [dbo].[tLimitFailures](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL IDENTITY(1000000000,1),
	CustomerSessionId [bigint] NOT NULL,
    TransactionType [int] NOT NULL,
    TransactionAmount [money] NOT NULL,
    LimitAmount [money] NOT NULL,
    ComplianceProgramName [nvarchar] (50) NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tLimitFailures] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

