-- =======================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <02/26/2016>
-- Description:	<As Alloy we need a consolidated database>
-- Rally ID:	<AL-5219>
-- =====================================================================
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomerPreferedProducts' AND COLUMN_NAME = 'CustomerProductPK')
	BEGIN
		DROP TABLE tCustomerPreferedProducts;
	END
GO

IF EXISTS (SELECT 1 FROM dbo.tPartnerCustomers) AND 
EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_NAME = 'PK_tCustomers' AND TABLE_NAME ='tPartnerCustomers')
	BEGIN
		EXEC sp_rename 'dbo.tPartnerCustomers.PK_tCustomers', 'PK_tPartnerCustomers';
	END
GO