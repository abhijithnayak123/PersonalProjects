--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <08-28-2017>
-- Description:	Create the RCIF credentials table. 
-- Jira ID:		<B-08245 - Move the RCIF service url configuration to database layer>
-- ================================================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tRCIF_Credential]') AND type in (N'U'))
	DROP TABLE [dbo].[tRCIF_Credential]
GO 


CREATE TABLE tRCIF_Credential (
	CredentialId BIGINT IDENTITY(1000000000,1) NOT NULL PRIMARY KEY,
	ServiceUrl NVARCHAR(MAX) NULL,
	CertificateName NVARCHAR(500) NULL,
	ThumbPrint NVARCHAR(500) NULL,
	ChannelPartnerId SMALLINT NOT NULL,
	DTServerCreate DATETIME NOT NULL,
	DTServerLastModified DATETIME NULL
)
