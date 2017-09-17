--===========================================================================================
-- Auther:			Raviraj
-- Date Created:	1/27/2014
-- Description:		Script for insert NYCHA  to catalog
--===========================================================================================
--Insert NYCHA Biller to [tMasterCatalog]
IF NOT EXISTS(select * from [tMasterCatalog] 
where [ProviderCatalogId]=1 and [BillerName]='NYCHA' and [ChannelPartnerId]=28 and [ProviderId]=404)
INSERT INTO [dbo].[tMasterCatalog]
   ([rowguid],[ProviderCatalogId],[BillerName],[ChannelPartnerId],[ProviderId],[ProductType],
   [LogoURL],[IsActive],[DtCreate],[DtModified])
VALUES
   (newid(),1,'NYCHA',28,404,NULL,
   NULL,1,getdate(),NULL)
GO
