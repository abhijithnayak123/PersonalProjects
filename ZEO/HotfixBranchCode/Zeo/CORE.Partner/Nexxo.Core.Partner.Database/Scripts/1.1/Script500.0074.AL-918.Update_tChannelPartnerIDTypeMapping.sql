--- ================================================================================
-- Author:		<Namit>
-- Create date: <07/31/2015>
-- Description:	<As Carver, I want to accept non-US passports and EA/green cards>
-- Jira ID:		<AL-918>
-- ================================================================================

DECLARE @ChannelPartnerPK UNIQUEIDENTIFIER
DECLARE @NexxoIdTypePK UNIQUEIDENTIFIER

SELECT @ChannelPartnerPK = ChannelPartnerPK from tchannelpartners where name = 'Carver'
SELECT @NexxoIdTypePK = NexxoIdTypePK from tNexxoIdTypes where NexxoIdTypeId = 158

IF EXISTS (SELECT * FROM tChannelPartnerIDTypeMapping WHERE NexxoIdTypeId  = @NexxoIdTypePK AND ChannelPartnerId = @ChannelPartnerPK)
BEGIN
	UPDATE tChannelPartnerIDTypeMapping SET IsActive = 1 WHERE NexxoIdTypeId  = @NexxoIdTypePK AND ChannelPartnerId = @ChannelPartnerPK
END
GO

DECLARE @ChannelPartnerPK UNIQUEIDENTIFIER
DECLARE @NexxoIdTypePK UNIQUEIDENTIFIER

SELECT @ChannelPartnerPK = ChannelPartnerPK from tchannelpartners where name = 'Carver'
SELECT @NexxoIdTypePK = NexxoIdTypePK from tNexxoIdTypes where NexxoIdTypeId = 160

IF EXISTS (SELECT * FROM tChannelPartnerIDTypeMapping WHERE NexxoIdTypeId  = @NexxoIdTypePK AND ChannelPartnerId = @ChannelPartnerPK)
BEGIN
	UPDATE tChannelPartnerIDTypeMapping SET IsActive = 1 WHERE NexxoIdTypeId  = @NexxoIdTypePK AND ChannelPartnerId = @ChannelPartnerPK
END
GO