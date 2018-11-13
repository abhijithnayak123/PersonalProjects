-- ============================================================
-- Author:	   <Manikandan Govindraj>
-- Create date: <07/27/2015>
-- Description:	<Alter tWunion_trx table to add new columns>
-- ============================================================
 
IF NOT EXISTS 
(
SELECT 	1 FROM sys.columns
  WHERE  
	object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]') 
    AND name = 'PrincipalAmount'
) 
BEGIN 
  ALTER TABLE [dbo].[tWUnion_Trx] 
  ADD PrincipalAmount DECIMAL(18,2) NULL 
END

IF NOT EXISTS 
(
SELECT 	1 FROM sys.columns
WHERE  
object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]') 
AND name = 'Receiver_unv_Buffer'
) 
BEGIN
  ALTER TABLE [dbo].[tWUnion_Trx] 
  ADD Receiver_unv_Buffer VARCHAR(300) NULL  
END

IF NOT EXISTS 
(
SELECT 	1 FROM sys.columns
WHERE  
object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]') 
AND name = 'Sender_unv_Buffer'
) 
BEGIN
  ALTER TABLE [dbo].[tWUnion_Trx] ADD Sender_unv_Buffer VARCHAR(300) NULL
END
 
GO
 


