-- ============================================================
-- Author:		<Ashok Kumar G>
-- Create date: <05/08/2014>
-- Description:	<Added a New Column in 'tWUnion_Receiver' 
--				for Receiver GoldCardNumber> 
-- Rally ID:	<N/A>
-- ============================================================

IF NOT EXISTS 
(
  SELECT 
	1 
  FROM   
	sys.columns 
  WHERE  
	object_id = OBJECT_ID(N'[dbo].[tWUnion_Receiver]') 
    AND name = 'GoldCardNumber'
)
BEGIN         
	ALTER TABLE dbo.tWUnion_Receiver
	ADD GoldCardNumber VARCHAR(50) NULL
END
GO