--===========================================================================================
-- Author:		<RogyEapen>
-- Create date: <12/10/2014>
-- Description:	<Carver Frank Data>
-- Rally ID:	<US2236-TA6427>
--===========================================================================================

----Carver 
DECLARE @PartnerPK uniqueidentifier
SELECT @PartnerPK = rowguid from tChannelPartners where id = 28
IF EXISTS 
(
  SELECT 
	* 
  FROM   
	sys.columns 
  WHERE 
  name = N'FrankData' 
	AND object_id = OBJECT_ID(N'[dbo].[tChannelPartnerConfig]')      
)
BEGIN         
UPDATE [dbo].[tChannelPartnerConfig]
SET 
	 FrankData = 'CAR-|LocationIdentifier',
	 IsCheckFrank = 1
WHERE 
	ChannelPartnerID = @PartnerPK

END
GO