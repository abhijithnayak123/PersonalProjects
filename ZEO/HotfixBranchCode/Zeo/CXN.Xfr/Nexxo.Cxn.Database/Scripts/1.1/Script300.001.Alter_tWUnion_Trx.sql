--This script is added for User Story #US1684
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