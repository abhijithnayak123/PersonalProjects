-- ================================================================================
-- Author:		<Anisha Abraham>
-- Create date: <05/02/2016>
-- Description:	<Synovus Visa Card - On Card Maintenance screen when tried to request for a replacement card 
--				 that was in 'Lost Card' status,Getting error popup.>
-- Jira ID:		<AL-4781>
-- ================================================================================

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_NAME='IX_tVisa_CardClass_StateCode')
BEGIN
	ALTER TABLE tVisa_CardClass 
	DROP CONSTRAINT IX_tVisa_CardClass_StateCode
END
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_NAME='IX_tVisa_CardClass_StateCode_ChannelPartnerId')
BEGIN
	ALTER TABLE tVisa_CardClass 
	ADD CONSTRAINT IX_tVisa_CardClass_StateCode_ChannelPartnerId UNIQUE(StateCode, ChannelPartnerId)
END
GO
