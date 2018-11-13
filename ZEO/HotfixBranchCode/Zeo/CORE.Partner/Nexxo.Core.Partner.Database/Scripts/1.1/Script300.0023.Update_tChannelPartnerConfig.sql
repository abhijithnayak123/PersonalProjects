--===========================================================================================
-- Author:		<SwarnaLakshmi>
-- Create date: <06/19/2014>
-- Description:	<Synovus Frank Data>
-- Rally ID:	<US1421>
--===========================================================================================
DECLARE @PartnerPK uniqueidentifier

-- Synovus 
SELECT @PartnerPK = rowguid from tChannelPartners where id = 33
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
	 FrankData = 'SynCon |BankID|BranchID',
	 IsCheckFrank = 1
WHERE 
	ChannelPartnerID = @PartnerPK

END
GO

-- Carver 
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
	 FrankData = '',
	 IsCheckFrank = 0
WHERE 
	ChannelPartnerID = @PartnerPK

END
GO



