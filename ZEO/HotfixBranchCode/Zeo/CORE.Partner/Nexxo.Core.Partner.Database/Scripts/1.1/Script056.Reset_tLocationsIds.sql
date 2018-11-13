-- Resetting the Id column values for tLocations table

ALTER TABLE tLocations
  DROP COLUMN Id  
GO    
 
ALTER TABLE tLocations
	ADD [Id] [bigint] IDENTITY(1000000000,1) NOT NULL	
GO