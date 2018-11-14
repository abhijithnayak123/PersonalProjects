 ------------------------------Migration Script for Customer groups------------------------------
BEGIN TRY
	BEGIN TRAN;

	  ;With PCs as
			 (  
				 SELECT PartnerCustomerPK, tcpg.ChannelPartnerGroupId,a.DTTerminalCreate , b.cxeid      ,
							ROW_NUMBER() OVER( PARTITION BY PartnerCustomerPK ORDER BY a.DTTerminalCreate) AS rownumm
				 FROM [dbo].[tPartnerCustomerGroupSettings] a
					 INNER JOIN tPartnerCustomers b on  a.[PartnerCustomerPK]= b.[CustomerPK]
					 INNER JOIN tChannelPartnerGroups tcpg ON tcpg.ChannelPartnerGroupPK = a.ChannelPartnerGroupPK
			 ) 

			 UPDATE cust
			 SET cust.[Group1]= pcs.[ChannelPartnerGroupId]       
			 FROM [dbo].[tCustomers] as cust
				INNER JOIN pcs on cust.customerid = pcs.cXEID
			 WHERE pcs.rownumm = 1;

			 ----------------------Update group 1 from tPartnerCustomerGroupSetting with the earliest row for the customer--------

			  ----------------------Update group 2 from tPartnerCustomerGroupSetting with the second row for the customer --------

			  With PCs as
			 (  
				 SELECT PartnerCustomerPK, tcpg.ChannelPartnerGroupId,a.DTTerminalCreate , b.cxeid      ,
							ROW_NUMBER() OVER( PARTITION BY PartnerCustomerPK ORDER BY a.DTTerminalCreate) AS rownumm
				 FROM [dbo].[tPartnerCustomerGroupSettings] a
					 INNER JOIN tPartnerCustomers b on  a.[PartnerCustomerPK]= b.[CustomerPK]
					 INNER JOIN tChannelPartnerGroups tcpg ON tcpg.ChannelPartnerGroupPK = a.ChannelPartnerGroupPK
			 ) 

			 UPDATE cust
			 SET cust.[Group2]= pcs.[ChannelPartnerGroupId]       
			 FROM [dbo].[tCustomers] as cust
				INNER JOIN pcs on cust.customerid = pcs.cXEID
			 WHERE pcs.rownumm = 2;

	COMMIT TRAN
END TRY

BEGIN CATCH
		IF(@@TRANCOUNT > 0)
		SELECT
		ERROR_NUMBER() AS ErrorNumber
		,ERROR_SEVERITY() AS ErrorSeverity
		,ERROR_STATE() AS ErrorState
		,ERROR_PROCEDURE() AS ErrorProcedure
		,ERROR_LINE() AS ErrorLine
		,ERROR_MESSAGE() AS ErrorMessage,
		XACT_STATE()as state;
ROLLBACK TRAN
END CATCH