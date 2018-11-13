-- ================================================================================
-- Author:		<Ashok Kumar G>
-- Create date: <27/01/2016>
-- Description:	<TCF: Duplicate records in ODS>
-- Jira ID:		<AL-4604>
-- ================================================================================

;WITH cte AS
(
SELECT 
	txnPK, 
	DTServerCreate,
	IsActive, 
	ROW_Number() OVER(PARTITION BY txnPK ORDER BY DTServerCreate DESC) AS CountNumber
FROM 
	tTxn_FeeAdjustments
)

UPDATE cte SET IsActive = 0 WHERE CountNumber > 1
