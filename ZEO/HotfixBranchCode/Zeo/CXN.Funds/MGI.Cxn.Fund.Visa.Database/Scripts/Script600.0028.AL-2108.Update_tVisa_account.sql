-- ==========================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <12/28/2015>
-- Description:	<As VisaDPS, update Primary Alias ID in Alloy for companion cardholder>
-- Rally ID:	<AL-2108>
-- ==========================================================================
IF EXISTS 
(
	SELECT 1 FROM tVisa_Account WHERE PrimaryCardAliasId IS NULL
)
BEGIN
	UPDATE tVisa_Account
	SET PrimaryCardAliasId = CardAliasId
END
GO