--=========================================================
-- Author: <Kaushik S>
-- Date Created: <Feb 23 2015>
-- Description: <Adding new coloum IsTCForcePrintRequired 
--				to tChannelPartnerProductProcessorsMapping Table to make WU 
--				T&C condition Pop-up configurable>
-- User Story ID: <AL-59>
--==========================================================

DECLARE @ChannelPartnerId uniqueidentifier
DECLARE @ProcessorId uniqueidentifier
DECLARE @ProductProcessorId uniqueidentifier
DECLARE @ProductId uniqueidentifier
DECLARE @ChannelPartnerProductProcessorsId uniqueidentifier

SELECT @ChannelPartnerId = rowguid FROM dbo.tChannelPartners WHERE Name = 'Carver'
SELECT @ProcessorId = rowguid FROM dbo.tProcessors WHERE Name = 'WesternUnion'
SELECT @ProductId = rowguid FROM  dbo.tProducts WHERE Name = 'MoneyTransfer'
SELECT @ProductProcessorId = rowguid FROM dbo.tProductProcessorsMapping 
WHERE ProcessorId = @ProcessorId AND ProductId = @ProductId

SELECT 
	@ChannelPartnerProductProcessorsId = rowguid 
FROM tChannelPartnerProductProcessorsMapping 
WHERE 
	ChannelPartnerId = @ChannelPartnerId 
	AND ProductProcessorId = @ProductProcessorId

--
UPDATE 
	dbo.tChannelPartnerProductProcessorsMapping
SET 
	IsTnCForcePrintRequired = 1
WHERE 
	rowguid = @ChannelPartnerProductProcessorsId
GO