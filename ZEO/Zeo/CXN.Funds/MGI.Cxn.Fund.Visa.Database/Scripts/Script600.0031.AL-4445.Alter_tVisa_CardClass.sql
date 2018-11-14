-- ============================================================
-- Author:		<Nitish Biradar>
-- Create date: <01/25/2016>
-- Description:	<DDL script to insert and Update seed data tVisa_CardClass table>
-- Rally ID:	<AL-4445>
-- ============================================================

UPDATE tVisa_CardClass
   SET StateCode = 'IL'
 WHERE StateCode = 'ILIN'
GO

IF NOT EXISTS (SELECT 1 FROM tVisa_CardClass WHERE StateCode = 'IN')
BEGIN
	INSERT INTO tVisa_CardClass (VisaCardClassPK, StateCode, CardClass, DTServerCreate, DTServerLastModified) VALUES
	(NEWID(), 'IN',   11,  GETDATE(), NULL)
END
GO 

IF NOT EXISTS (SELECT 1 FROM tVisa_CardClass WHERE StateCode = 'SD')
BEGIN
	INSERT INTO tVisa_CardClass (VisaCardClassPK, StateCode, CardClass, DTServerCreate, DTServerLastModified) VALUES
	(NEWID(), 'SD',   13,  GETDATE(), NULL)
END
GO


