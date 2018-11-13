-- ============================================================
-- Author:		<Sunil Shetty>
-- Create date: <12/18/2015>
-- Description:	<DML script to insert CardShippingTypes data into tVisa_CardShippingTypes table>
-- Rally ID:	<AL-3758>
-- ============================================================

IF NOT EXISTS (SELECT 1 FROM tVisa_CardShippingTypes WHERE ShippingTypePK = '61B8303D-F67B-4EEF-B7C5-066844B5D329')
BEGIN
	INSERT INTO tVisa_CardShippingTypes(ShippingTypePK, Code, Name, DTServerCreate, DTServerLastModified, Active) VALUES
	('61B8303D-F67B-4EEF-B7C5-066844B5D329', -3, 'Instant Issue Replace\Lost', GETDATE(), NULL, 0)  
	END 
GO

