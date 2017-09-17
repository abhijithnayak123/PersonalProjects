--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <08-28-2017>
-- Description:	Insert Record in the RCIF credentials table. 
-- Jira ID:		<B-08245 - Move the RCIF service url configuration to database layer>
-- ================================================================================

INSERT INTO [dbo].[tRCIF_Credential]
          (ServiceUrl
		  ,CertificateName
		  ,ThumbPrint
          ,ChannelPartnerId
          ,DTServerCreate
          ,DTServerLastModified)
VALUES
           (N'https://proxy.ic.local/tcf/mb/ZeoCustomerService',
		   N'alloy-dev.tcfbank.com_client',
		   'e9f07bd446116876834377c6d885dae06f705233',
		   34,
		   GETDATE(),
           NULL)
GO


