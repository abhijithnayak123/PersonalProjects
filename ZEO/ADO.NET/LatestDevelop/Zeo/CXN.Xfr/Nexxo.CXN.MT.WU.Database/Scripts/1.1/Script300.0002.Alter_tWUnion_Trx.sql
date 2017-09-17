-- ============================================================
-- Author:		<Sudhir Baregar>
-- Create date: <02/04/2014>
-- Description:	<Added a New Column PersonalMessage 
--				in 'tWUnion_Trx' to persist PersonalMessage entered 
--				by the Teller in the Send Money Screens.>
-- Rally ID:	<US1684 - TA4160>
-- ============================================================

IF NOT EXISTS 
(
  SELECT 
	1 
  FROM   
	sys.columns 
  WHERE  
	object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]') 
    AND name = 'PersonalMessage'
)
BEGIN         
	ALTER TABLE tWUnion_Trx 
	ADD PersonalMessage NVARCHAR(1000) NULL
END
GO