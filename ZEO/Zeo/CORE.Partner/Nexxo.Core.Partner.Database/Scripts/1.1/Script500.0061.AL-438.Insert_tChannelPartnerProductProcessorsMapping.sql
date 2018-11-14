-- =============================================================================================
-- Author:		<Mohammed Khaja Firoz Molla>
-- Create date: <07/03/2015>
-- Description:	<Script for insert data into to set up Certegy as Check Processor for RedStone>
-- Jira ID:	<AL-438>
-- ==============================================================================================

DECLARE @ProductProcessorMapping UNIQUEIDENTIFIER

SELECT @ProductProcessorMapping = PrdProcMapping.rowguid FROM [tProductProcessorsMapping] PrdProcMapping
JOIN [tProducts] products
ON  PrdProcMapping.ProductId = products.rowguid
JOIN [tProcessors] processors
ON processors.rowguid = PrdProcMapping.ProcessorId
AND processors.Name = 'Certegy'
AND products.Name = 'ProcessCheck'

DECLARE @PartnerPK UNIQUEIDENTIFIER
SELECT @PartnerPK = ChannelPartnerPK FROM tChannelPartners WHERE ChannelPartnerId = 35

IF NOT EXISTS (SELECT 1 FROM [dbo].[tChannelPartnerProductProcessorsMapping] WHERE [ProductProcessorId] = @ProductProcessorMapping AND [ChannelPartnerId] = @PartnerPK)
BEGIN
	INSERT INTO [tChannelPartnerProductProcessorsMapping] (rowguid, ChannelPartnerId, ProductProcessorId, Sequence, DTServerCreate, IsTnCForcePrintRequired)
		VALUES(NewID(), @PartnerPK, @ProductProcessorMapping, 1, GETDATE(), 0)
END
GO