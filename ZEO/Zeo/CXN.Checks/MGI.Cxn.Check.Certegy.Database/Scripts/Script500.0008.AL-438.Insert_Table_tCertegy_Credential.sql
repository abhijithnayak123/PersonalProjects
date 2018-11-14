-- ============================================================
-- Author:		<Mohammed Khaja Firoz Molla>
-- Create date: <05/07/2015>
-- Description:	<DDL script to create tCertegy_Account table>
-- Jira ID:	<AL-438>
-- ============================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tCertegy_Credential]') AND type in (N'U'))

BEGIN

INSERT INTO [dbo].[tCertegy_Credential]
           ([CertegyCredentialPK]
		   ,[ServiceUrl]
           ,[CertificateName]
           ,[Version]
           ,[ChannelPartnerId]
           ,[DeviceType]         
           ,[DeviceIP]
		   ,[DTServerCreate])		                     
     VALUES
           (NEWID()
		    ,'https://transtest2.certegy.com/mepca2/PCAService'
            ,'Todd Bowersox'
            ,'1.2'
            , 35
            ,'P'		
			,'12.227.206.70'
			,GETDATE())		    
END
GO


