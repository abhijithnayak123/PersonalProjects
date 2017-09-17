
CREATE TABLE [dbo].[tShoppingCarts](
	[cartRowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Active] [bit] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tShoppingCarts] PRIMARY KEY CLUSTERED 
(
	[cartRowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO