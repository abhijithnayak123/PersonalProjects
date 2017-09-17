

CREATE TABLE [dbo].[tCustomers](
    [rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL,
	[CXEId] [bigint] NOT NULL,
    [DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
CONSTRAINT [PK_tCustomers] PRIMARY KEY CLUSTERED 
(
    [rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
 
GO
