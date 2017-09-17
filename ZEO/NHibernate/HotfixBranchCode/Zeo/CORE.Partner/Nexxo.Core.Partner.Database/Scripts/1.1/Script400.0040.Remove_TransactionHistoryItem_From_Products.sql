-- ============================================================
-- Author:		Ashok Kumar
-- Create date: <02/12/2015>
-- Description:	<Removed transaction history item from tProducts table>
-- Rally ID:	<US2166>
-- ============================================================

DECLARE @ProductRowGuid UNIQUEIDENTIFIER;

DECLARE @ProductProcessorRowGuid UNIQUEIDENTIFIER;

SET @ProductRowGuid = ( SELECT rowguid FROM tProducts WHERE Name = 'TransactionHistory' )

SET @ProductProcessorRowGuid = ( SELECT rowguid FROM tProductProcessorsMapping WHERE ProductId = @ProductRowGuid )

DELETE FROM tChannelPartnerProductProcessorsMapping WHERE ProductProcessorId = @ProductProcessorRowGuid

DELETE FROM tProductProcessorsMapping WHERE ProductId = @ProductRowGuid

DELETE FROM tProducts WHERE Name = 'TransactionHistory'