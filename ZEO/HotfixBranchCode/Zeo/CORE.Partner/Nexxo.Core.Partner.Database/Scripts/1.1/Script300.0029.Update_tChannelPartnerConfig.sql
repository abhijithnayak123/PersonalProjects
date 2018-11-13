--===========================================================================================
-- Author:		<Sunil Shetty>
-- Create date: <11/24/2014>
-- Description:	<Notes to be Enabled on for Synovus and disabled to Carver and TCF>
-- Rally ID:	<US2260>
--===========================================================================================
DECLARE @PartnerPK uniqueidentifier

-- Synovus 
SELECT @PartnerPK = rowguid from tChannelPartners where Name = 'Synovus'

IF EXISTS 
(
  SELECT 
	* 
  FROM   
	sys.columns 
  WHERE 
  name = N'IsNotesEnable' 
	AND object_id = OBJECT_ID(N'[dbo].[tChannelPartnerConfig]')      
)
BEGIN         
	Update tChannelPartnerConfig
	set IsNotesEnable = 1
	where ChannelPartnerID = @PartnerPK
END
GO
-- Carver and TCF

IF EXISTS 
(
  SELECT 
	* 
  FROM   
	sys.columns 
  WHERE 
  name = N'IsNotesEnable' 
	AND object_id = OBJECT_ID(N'[dbo].[tChannelPartnerConfig]')      
)
BEGIN         
	Update tChannelPartnerConfig
	set IsNotesEnable = 0
	where ChannelPartnerID in 
	(Select rowguid from tChannelPartners where Name in ('Carver','TCF'))
END
GO
