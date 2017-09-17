IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tBillPayProcessorLogin]') AND type in (N'U'))
 DROP TABLE [dbo].[tBillPayProcessorLogin]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE tBillPayProcessorLogin
(
 Id UNIQUEIDENTIFIER NOT NULL,
 ClerkID VARCHAR(20) NULL,
 SecureKey VARCHAR(100) NULL, 
 TerminalID VARCHAR(20) NULL,
 ServiceURL VARCHAR(500) NOT NULL,
 ServiceNamespace VARCHAR(100) NULL,
 AgentID INT NOT NULL,
 ProcessorID INT NOT NULL,
 ServicePartnerId INT NULL,
 UserName VARCHAR(100) NULL,
 Password VARCHAR(100) NULL,
 CONSTRAINT PK_tBillPayProcessorLogin_Id PRIMARY KEY (Id)
)ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_tBillPayProcessorLogin_AgentID_ProcessorID]
ON tBillPayProcessorLogin (AgentID, ProcessorID)
GO