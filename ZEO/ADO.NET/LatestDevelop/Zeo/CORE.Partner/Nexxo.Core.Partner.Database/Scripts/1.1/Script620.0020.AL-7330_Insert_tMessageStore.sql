--- ================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <06/09/2016>
-- Description:	 Add Error/Exception handling Framework in Receipts
-- Jira ID:		<AL-7330>
-- =================================================================================
DELETE FROM tMessageStore WHERE MessageKey in 
('1000.100.4100','1000.100.4103','1000.100.4104','1000.100.4105','1000.100.4106','1000.100.4107','1000.100.4108','1000.100.4109','1000.100.4110','1000.100.4111','1000.100.8022','1000.100.8023','1000.100.8024','1000.100.8025','1000.100.9999')
GO

INSERT INTO tMessageStore (MessageStorePK, MessageKey, PartnerPK, Language, Content, AddlDetails, DTServerCreate, Processor, Type)
VALUES
(NEWID(),'1000.100.4100',1,0,'Receipt Templates Not Found','Please contact technical support team for further assistance',	GETDATE(),'	MGiAlloy',2),
(NEWID(),'1000.100.4103',1,0,'Error while retrieving check receipt template','Please contact technical support team for further assistance',GETDATE(),'MGiAlloy',2),
(NEWID(),'1000.100.4104',1,0,'Error while retrieving money transfer receipt template','Please contact technical support team for further assistance',GETDATE(),'MGiAlloy',2),
(NEWID(),'1000.100.4105',1,0,'Error while retrieving funds receipt template','Please contact technical support team for further assistance',GETDATE(),'MGiAlloy',	2),
(NEWID(),'1000.100.4106',1,0,'Error while retrieving billpay receipt template','Please contact technical support team for further assistance',GETDATE(),'MGiAlloy',2),
(NEWID(),'1000.100.4107',1,0,'Error while retrieving money order receipt template','Please contact technical support team for further assistance',GETDATE(),'MGiAlloy',2),
(NEWID(),'1000.100.4108',1,0,'Error while retrieving summary receipt template','Please contact technical support team for further assistance',GETDATE(),'MGiAlloy',2),
(NEWID(),'1000.100.4109',1,0,'Error while retrieving check declined receipt template','Please contact technical support team for further assistance',GETDATE(),'MGiAlloy',2),
(NEWID(),'1000.100.4110',1,0,'Error while retrieving coupon receipt template','Please contact technical support team for further assistance',GETDATE(),'MGiAlloy',2),
(NEWID(),'1000.100.4111',1,0,'Error while retrieving dodfrank receipt template','Please contact technical support team for further assistance',GETDATE(),'MGiAlloy',2),
(NEWID(),'1000.100.8022',1,0,'Error while retrieving check declined receipt template','Please contact technical support team for further assistance',GETDATE(),'MGiAlloy',2),
(NEWID(),'1000.100.8023',1,0,'Error while retrieving dodfrank receipt template','Please contact technical support team for further assistance',GETDATE(),'MGiAlloy',2),
(NEWID(),'1000.100.8024',1,0,'PS Service is not running','Please contact technical support team for further assistance',GETDATE(),'MGiAlloy',2),
(NEWID(),'1000.100.8025',1,0,'Receipt Templates Not Found','Please contact technical support team for further assistance',GETDATE(),'MGiAlloy',2),
(NEWID(),'1000.100.9999',1,0,'This operation could not be completed','Please contact your technical support team for more information',GETDATE(),'MGiAlloy',2)
GO
