--- ===============================================================================
-- Author:		<M.Purna Pushkal>
-- Create date: <09-07-2017>
-- Description: Adding the non-clustered index for the Partner Account Number column of tTcis_Account Table.
-- ================================================================================

IF NOT EXISTS(SELECT 1 FROM SYS.INDEXES WHERE NAME = 'IX_tTCIS_Account_PAN')
BEGIN
	 CREATE NONCLUSTERED INDEX IX_tTCIS_Account_PAN ON tTCIS_Account(PartnerAccountNumber ASC)
END
GO

