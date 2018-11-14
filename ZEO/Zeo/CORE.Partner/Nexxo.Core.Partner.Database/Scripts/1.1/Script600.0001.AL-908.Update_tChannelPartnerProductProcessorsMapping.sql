-- ================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <23/08/2015>
-- Description:	<As Alloy, I need to configure products for Synovus>
-- Jira ID:		<AL-908>
-- ================================================================================
DECLARE @ChannelPartnerPK UNIQUEIDENTIFIER
DECLARE @VisaProcessorPK UNIQUEIDENTIFIER
DECLARE @TsysProcessorPK UNIQUEIDENTIFIER

SELECT @ChannelPartnerPK = ChannelPartnerPK
FROM tChannelPartners
WHERE Name= 'SYNOVUS'

SELECT @TsysProcessorPK = pp.rowguid
 FROM tProductProcessorsMapping pp
  inner join tProcessors pr on pr.rowguid= pp.ProcessorId
  inner join tProducts p on p.rowguid = pp.ProductId
WHERE pr.Name = 'TSys' and p.Name = 'ProductCredential'

SELECT @VisaProcessorPK = pp.rowguid
 FROM tProductProcessorsMapping pp
  inner join tProcessors pr on pr.rowguid= pp.ProcessorId
  inner join tProducts p on p.rowguid = pp.ProductId
WHERE pr.Name = 'VISA' and p.Name = 'ProductCredential'


UPDATE 
	tChannelPartnerProductProcessorsMapping
SET 
	ProductProcessorId = @VisaProcessorPK

WHERE 
	ProductProcessorId = @TsysProcessorPK
		
AND
	 ChannelPartnerId = @ChannelPartnerPK
GO