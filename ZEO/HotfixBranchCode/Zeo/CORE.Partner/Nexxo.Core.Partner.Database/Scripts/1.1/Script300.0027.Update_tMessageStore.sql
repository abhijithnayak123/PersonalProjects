 --===========================================================================================
-- Author:		<Sunil Shetty>
-- Create date: <10/27/2014>
-- Description:	<Update Decline Check Message for Message Key 1002.53>
-- Rally ID:	<US2077>
--===========================================================================================
 IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = N'tMessageStore')
	BEGIN
		update tMessageStore
		set Content = 'This is an On-Us check and cannot be cashed through this application. 
		If you are not sure how to process this check at your location; please contact your manager.'
		where MessageKey = '1002.53' and PartnerPK in (Select Id from tChannelPartners where Name = 'Synovus')
	END