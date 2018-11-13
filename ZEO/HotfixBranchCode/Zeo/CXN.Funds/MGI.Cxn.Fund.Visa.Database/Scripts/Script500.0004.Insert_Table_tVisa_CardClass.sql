-- ============================================================
-- Author:		<Bineesh Raghavan>
-- Create date: <06/25/2014>
-- Description:	<DDL script to insert seed data tVisa_CardClass table>
-- Rally ID:	<AL-327>
-- ============================================================

IF NOT EXISTS (SELECT 1 FROM  tVisa_CardClass)
BEGIN
	INSERT INTO tVisa_CardClass (VisaCardClassPK, StateCode, CardClass, DTServerCreate, DTServerLastMod) VALUES
	(NEWID(), 'MI',   3,  GETDATE(), NULL), 
	(NEWID(), 'CO',   5,  GETDATE(), NULL), 
	(NEWID(), 'AZ',   7,  GETDATE(), NULL), 
	(NEWID(), 'WI',   9,  GETDATE(), NULL), 
	(NEWID(), 'ILIN', 11, GETDATE(), NULL), 
	(NEWID(), 'MN',   13, GETDATE(), NULL)
END
GO
