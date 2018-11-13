

CREATE TABLE [dbo].[tCustomerSessions](
    [customerSessionRowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL IDENTITY (1000000000, 1),
	[AgentSessionPK] [uniqueidentifier] NOT NULL,
	[CustomerPK] [uniqueidentifier] NOT NULL,
    [DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
CONSTRAINT [PK_tCustomerSessions] PRIMARY KEY CLUSTERED 
(
    [customerSessionRowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
 
GO
 
ALTER TABLE [dbo].[tCustomerSessions]  WITH CHECK ADD CONSTRAINT [FK_tCustomerSessions_tAgentSessions] FOREIGN KEY([AgentSessionPK])
REFERENCES [dbo].[tAgentSessions] ([rowguid])
GO
 
ALTER TABLE [dbo].[tCustomerSessions] CHECK CONSTRAINT [FK_tCustomerSessions_tAgentSessions]
GO
 
ALTER TABLE [dbo].[tCustomerSessions]  WITH CHECK ADD CONSTRAINT [FK_tCustomerSessions_tCustomers] FOREIGN KEY([CustomerPK])
REFERENCES [dbo].[tCustomers] ([rowguid])
GO
 
ALTER TABLE [dbo].[tCustomerSessions] CHECK CONSTRAINT [FK_tCustomerSessions_tCustomers]
GO
