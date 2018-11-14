-- ============================================================
-- Author:		<Abhijith>
-- Create date: <05/23/2018>
-- Description:	<script for creating tTCFOnusAccount table>
-- ============================================================

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tTCFOnus_Account]') AND type in (N'U'))
DROP TABLE [dbo].[tTCFOnus_Account]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tTCFOnus_Account](
	[TCFOnusAccountID] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[DTServerCreate] [datetime] NOT NULL,
	[DTServerLastModified] [datetime] NULL,
	[DTTerminalCreate] [datetime] NOT NULL,
	[DTTerminalLastModified] [datetime] NULL,
	[CustomerId] [bigint] NOT NULL,
	[CustomerRevisionNo] [bigint] NOT NULL,
	[CustomerSessionId] [bigint] NULL,
 CONSTRAINT [PK_tTCFOnus_Account] PRIMARY KEY CLUSTERED 
(
	[TCFOnusAccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tTCFOnus_Account]  WITH CHECK ADD  CONSTRAINT [FK_tTCFOnus_Account_tCustomers] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[tCustomers] ([CustomerID])
GO

ALTER TABLE [dbo].[tTCFOnus_Account]  WITH CHECK ADD  CONSTRAINT [FK_tTCFOnus_Account_tCustomerSessions] FOREIGN KEY([CustomerSessionId])
REFERENCES [dbo].[tCustomerSessions] ([CustomerSessionID])
GO


