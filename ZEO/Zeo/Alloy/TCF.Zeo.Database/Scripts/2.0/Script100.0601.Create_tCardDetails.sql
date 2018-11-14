-- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <08-28-2017>
-- Description:	Create a new table to store Card BIN details
-- Jira ID:		<B-06199>
-- ================================================================================



-- Create new table as 'tCardDetails'

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE OBJECT_NAME(parent_object_id) = 'tCardDetails')
BEGIN
    CREATE TABLE tCardDetails
    ( 
		CardDetailId		 BIGINT IDENTITY(1,1) NOT NULL,
		CardBIN				 VARCHAR(10) NOT NULL,
		CardType			 VARCHAR(30) NOT NULL,
		DTServerCreate		 DATETIME NOT NULL,
		DTServerLastModified DATETIME
		CONSTRAINT PK_tCardDetails PRIMARY KEY CLUSTERED 
		(
			CardDetailId ASC
		)
     )
	
END
GO

-- Insert card BIN# in tCardDetails table

IF NOT EXISTS (SELECT 1 FROM tCardDetails WHERE CardBIN in('4761', '4479', '4855'))
BEGIN

	 INSERT INTO tCardDetails 
	 (
	 	CardBIN,
	 	CardType,
	 	DTServerCreate
	 )
	 VALUES 
	 ('4761', 'TCF', GETDATE()),
	 ('4479', 'TCF', GETDATE()),
	 ('4855', 'ZEO', GETDATE()),
	 ('4897', 'TCF', GETDATE()),
	 ('4389', 'TCF', GETDATE()),
	 ('572227', 'TCF', GETDATE()),
	 ('571076', 'TCF', GETDATE()),
	 ('574917', 'TCF', GETDATE()),
	 ('504530', 'TCF', GETDATE()),
	 ('572136', 'TCF', GETDATE()),
	 ('639541', 'TCF', GETDATE())

END
GO
  


