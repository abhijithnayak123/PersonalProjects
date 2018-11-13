-- ============================================================
-- Author:		<Sunil Shetty>
-- Create date: <11/23/2015>
-- Description:	<DML script to insert CardShippingTypes data into tVisa_CardShippingTypes table>
-- Rally ID:	<AL-1647>
-- ============================================================
IF  NOT EXISTS (SELECT 1 FROM tVisa_CardShippingTypes)
BEGIN
INSERT INTO tVisa_CardShippingTypes(ShippingTypePK, Code, Name, DTServerCreate, DTServerLastModified) VALUES 
('C3D7BFF7-6B63-45D2-A835-A9AD64F4EEB9', 0, 'Express Shipping',GETDATE(), NULL),
('95C6E11A-7D42-4936-A3E3-C7DF3B2B993F', 2, 'Standard Mail',GETDATE(), NULL),
('AFB0662A-FF57-445C-A628-D335A42F647E', -2, 'Instant Issue',GETDATE(), NULL)  
END 
GO



