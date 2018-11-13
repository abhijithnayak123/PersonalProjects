-- ============================================================
-- Author:		<Pamila Jose>
-- Create date: <11/05/2014>
-- Description:	<Script for insert data into tProducts, tProcessors, tProductProcessorsmapping, tChannelPartnerProductProcessorsMapping tables.>
-- Rally ID:	<US2127>
-- ============================================================

-- Insert Script for tChannelPartnerProductProcessorsMapping For ReceiveMoney to MGI
INSERT INTO tChannelPartnerProductProcessorsMapping (ChannelPartnerId, ProductProcessorId, Sequence, DTCreate) 
	VALUES('10F2865B-DBC5-4A0B-983C-62E0A0574354', 'F070307D-6F52-4C38-80D7-EA8C1F6620F5', 4, GETDATE())

UPDATE tChannelPartnerProductProcessorsMapping
SET		Sequence = 5
WHERE ChannelPartnerId = '10F2865B-DBC5-4A0B-983C-62E0A0574354'
AND ProductProcessorId = 'B2CD730B-95EC-41C6-B8C4-5427E8DB21CF'

UPDATE tChannelPartnerProductProcessorsMapping
SET		Sequence = 6
WHERE ChannelPartnerId = '10F2865B-DBC5-4A0B-983C-62E0A0574354'
AND ProductProcessorId = '75401337-7718-40F3-AF8F-D177D25E4DDF'

GO
