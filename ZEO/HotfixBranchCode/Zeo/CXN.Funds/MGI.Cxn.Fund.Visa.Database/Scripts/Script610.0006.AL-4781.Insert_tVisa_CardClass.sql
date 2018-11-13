-- ================================================================================
-- Author:		<Anisha Abraham>
-- Create date: <05/02/2016>
-- Description:	<Synovus Visa Card - On Card Maintenance screen when tried to request for a replacement card 
--				 that was in 'Lost Card' status, Getting error popup.>
-- Jira ID:		<AL-4781>
-- ================================================================================

DECLARE @channelPartnerId INT = 33 -- Synovus channel partner 

IF NOT EXISTS(SELECT 1 FROM tVisa_CardClass WHERE ChannelPartnerId = @channelPartnerId)
BEGIN
	INSERT tVisa_CardClass ([VisaCardClassPK], [StateCode], [CardClass], [DTServerCreate], [ChannelPartnerId]) VALUES 
	(NEWID(), 'AA', 1, GETDATE(), 33), 
	(NEWID(), 'AE', 1, GETDATE(), 33), 
	(NEWID(), 'AK', 1, GETDATE(), 33), 
	(NEWID(), 'AL', 1, GETDATE(), 33), 
	(NEWID(), 'AP', 1, GETDATE(), 33), 
	(NEWID(), 'AR', 1, GETDATE(), 33), 
	(NEWID(), 'AS', 1, GETDATE(), 33), 
	(NEWID(), 'AZ', 1, GETDATE(), 33), 
	(NEWID(), 'CA', 1, GETDATE(), 33), 
	(NEWID(), 'CO', 1, GETDATE(), 33), 
	(NEWID(), 'CT', 1, GETDATE(), 33), 
	(NEWID(), 'DC', 1, GETDATE(), 33), 
	(NEWID(), 'DE', 1, GETDATE(), 33), 
	(NEWID(), 'FL', 1, GETDATE(), 33), 
	(NEWID(), 'FM', 1, GETDATE(), 33), 
	(NEWID(), 'GA', 1, GETDATE(), 33), 
	(NEWID(), 'GU', 1, GETDATE(), 33), 
	(NEWID(), 'HI', 1, GETDATE(), 33), 
	(NEWID(), 'IA', 1, GETDATE(), 33), 
	(NEWID(), 'ID', 1, GETDATE(), 33), 
	(NEWID(), 'IL', 1, GETDATE(), 33), 
	(NEWID(), 'IN', 1, GETDATE(), 33), 
	(NEWID(), 'KS', 1, GETDATE(), 33), 
	(NEWID(), 'KY', 1, GETDATE(), 33), 
	(NEWID(), 'LA', 1, GETDATE(), 33), 
	(NEWID(), 'MA', 1, GETDATE(), 33), 
	(NEWID(), 'MD', 1, GETDATE(), 33), 
	(NEWID(), 'ME', 1, GETDATE(), 33), 
	(NEWID(), 'MH', 1, GETDATE(), 33), 
	(NEWID(), 'MI', 1, GETDATE(), 33), 
	(NEWID(), 'MN', 1, GETDATE(), 33), 
	(NEWID(), 'MO', 1, GETDATE(), 33), 
	(NEWID(), 'MP', 1, GETDATE(), 33), 
	(NEWID(), 'MS', 1, GETDATE(), 33), 
	(NEWID(), 'MT', 1, GETDATE(), 33), 
	(NEWID(), 'NC', 1, GETDATE(), 33), 
	(NEWID(), 'ND', 1, GETDATE(), 33), 
	(NEWID(), 'NE', 1, GETDATE(), 33), 
	(NEWID(), 'NH', 1, GETDATE(), 33), 
	(NEWID(), 'NJ', 1, GETDATE(), 33), 
	(NEWID(), 'NM', 1, GETDATE(), 33), 
	(NEWID(), 'NV', 1, GETDATE(), 33), 
	(NEWID(), 'NY', 1, GETDATE(), 33), 
	(NEWID(), 'OH', 1, GETDATE(), 33), 
	(NEWID(), 'OK', 1, GETDATE(), 33), 
	(NEWID(), 'OR', 1, GETDATE(), 33), 
	(NEWID(), 'PA', 1, GETDATE(), 33), 
	(NEWID(), 'PR', 1, GETDATE(), 33), 
	(NEWID(), 'PW', 1, GETDATE(), 33), 
	(NEWID(), 'RI', 1, GETDATE(), 33), 
	(NEWID(), 'SC', 1, GETDATE(), 33), 
	(NEWID(), 'SD', 1, GETDATE(), 33), 
	(NEWID(), 'TN', 1, GETDATE(), 33), 
	(NEWID(), 'TX', 1, GETDATE(), 33), 
	(NEWID(), 'UT', 1, GETDATE(), 33), 
	(NEWID(), 'VA', 1, GETDATE(), 33), 
	(NEWID(), 'VI', 1, GETDATE(), 33), 
	(NEWID(), 'VT', 1, GETDATE(), 33), 
	(NEWID(), 'WA', 1, GETDATE(), 33), 
	(NEWID(), 'WI', 1, GETDATE(), 33), 
	(NEWID(), 'WV', 1, GETDATE(), 33), 
	(NEWID(), 'WY', 1, GETDATE(), 33)
END
GO
