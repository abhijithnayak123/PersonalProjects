-- ============================================================
-- Author:		Abhijith
-- Create date: <11/17/2014>
-- Description:	<Script for Altering tProspectEmploymentDetails table>
-- Rally ID:	<US2169>
-- ============================================================

IF NOT EXISTS 
(
  SELECT 
	* 
  FROM   
	sys.columns 
  WHERE 
	name = N'OccupationDescription' 
	AND object_id = OBJECT_ID(N'[dbo].[tProspectEmploymentDetails]')      
)
BEGIN         
	ALTER TABLE tProspectEmploymentDetails 
	ADD OccupationDescription NVARCHAR(255) NULL 
END
GO
