--- ===============================================================================
-- Author:		 Abhijith
-- Description:	 Adding a new column to store the Consumer FraudPrompt Question from the Customer
-- Story Id   :  B-13688
-- ================================================================================

IF NOT EXISTS 
(
	SELECT 1
    FROM INFORMATION_SCHEMA.COLUMNS
	WHERE TABLE_NAME = 'tWUnion_Trx' AND COLUMN_NAME = 'ConsumerFraudPromptQuestion'      
)
BEGIN         

	ALTER TABLE tWUnion_Trx
	ADD ConsumerFraudPromptQuestion VARCHAR(1) NULL

END
GO



