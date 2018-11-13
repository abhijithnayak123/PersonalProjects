--===========================================================================================
-- Author:		<Abhijith>
-- Create date: <Jan 23 2015>
-- Description:	<Migration script for moving the location details to new table>
-- Rally ID:	<US2321>
--===========================================================================================

INSERT INTO tLocationProcessorCredentials
(
	rowguid
	,LocationId
	,ProviderId
	,UserName
	,Password
	,Identifier
	,DTCreate
)
SELECT 
	rowguid
	,LocationId
	,ProviderId
	,UserName
	,Password
	,Identifier
	,DTCreate
	FROM
	(
		SELECT 
			NEWID() as rowguid
			,loc.rowguid as LocationId
			,prodProc.Code as ProviderId
			,loc.LocationIdentifier as UserName
			,loc.LocationIdentifier as Password
			,loc.LocationIdentifier as Identifier
			,ROW_NUMBER() over (PARTITION by loc.LocationIdentifier,prodProc.Code order by prodProc.Code) as rownumber
			,GETDATE() as DTCreate
		FROM tChannelPartners chpart
		INNER JOIN tLocations loc ON chpart.id = loc.ChannelPartnerId
		INNER JOIN tChannelPartnerProductProcessorsMapping partProc ON partProc.ChannelPartnerId = chpart.rowguid
		INNER JOIN tProductProcessorsMapping prodProc ON prodProc.rowguid = partProc.ProductProcessorId
		WHERE prodProc.Code <> 1000 -- Remove all TransactionHisory product
     )  temp
  where rownumber = 1

GO
--===========================================================================================
