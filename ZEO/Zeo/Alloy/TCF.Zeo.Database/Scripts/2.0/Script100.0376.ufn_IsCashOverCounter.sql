-- ================================================================================
-- Author:		Manikandan Govindraj
-- Create date: 12/07/2016
-- Description: Create the function to get Cash over counter flag for shopping cart.
-- Jira ID:		AL-8047
-- ================================================================================

IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE = 'FN' AND NAME = 'ufn_IsCashOverCounter')
BEGIN
	DROP FUNCTION dbo.ufn_IsCashOverCounter
END
GO

CREATE FUNCTION dbo.ufn_IsCashOverCounter
(
    @channelPartnerId  INT
)
RETURNS BIT
BEGIN	

	DECLARE @cashOverCounter BIT =
	(
		SELECT 
		   CashOverCounter
		FROM 
		   tChannelPartnerConfig WITH (NOLOCK)
		WHERE 
		   ChannelPartnerId = @channelPartnerId -- ChannelPartnerId 
	)

	
	RETURN @cashOverCounter



END



