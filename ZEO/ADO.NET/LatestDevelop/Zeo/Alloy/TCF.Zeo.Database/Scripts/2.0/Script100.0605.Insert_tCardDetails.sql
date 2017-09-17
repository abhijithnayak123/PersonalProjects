-- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <08-28-2017>
-- Description:	Insert BIN details to tCardDetails table.
-- Jira ID:		<B-06199>
-- ================================================================================



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
  


