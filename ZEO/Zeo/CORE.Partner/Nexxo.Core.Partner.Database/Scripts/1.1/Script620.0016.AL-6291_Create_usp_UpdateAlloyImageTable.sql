--- ================================================================================
-- Author:		<Divya Boddu>
-- Create date: <05/23/2016>
-- Description:	<Implement Changes to Optimize the image size in PTNR DB Size.>
-- Jira ID:		<AL-6291>
-- ================================================================================

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'usp_UpdateAlloyImageTable') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_UpdateAlloyImageTable]
GO

CREATE PROCEDURE [dbo].[usp_UpdateAlloyImageTable] 
	@transactionType SMALLINT,
	@transactionTable TransactionTable READONLY
AS
BEGIN
	  IF @transactionType = 2 --for check images
	--begin
	--update tMoneyOrderImage set CheckFrontImage=null, CheckBackImage=null  where TrxId=@PKid;
	--end
	--ELSE IF @transactionType = 2 -- Check
		 BEGIN
			  UPDATE  t1
           SET 
             front = NULL,
             back = NULL,
		     FrontImagePath = t3.FrontImagePath,
		     BackImagePath = t3.BackImagePath
		   FROM tCheckImages AS t1
		     INNER JOIN tTxn_Check_Stage AS t2 ON t1.CheckPK = t2.CheckPK
		     INNER JOIN @transactionTable AS t3 ON t2.CheckID = t3.TransactionId
		 END
END
GO


