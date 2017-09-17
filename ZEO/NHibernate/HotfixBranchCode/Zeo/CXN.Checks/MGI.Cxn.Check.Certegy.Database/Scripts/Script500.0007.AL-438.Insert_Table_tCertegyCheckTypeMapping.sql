-- ============================================================
-- Author:		<Mohammed Khaja Firoz Molla>
-- Create date: <05/07/2015>
-- Description:	<DDL script to create tCertegy_Account table>
-- Jira ID:	<AL-438>
-- ============================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tCertegy_CheckTypeMapping]') AND type in (N'U'))

BEGIN

TRUNCATE TABLE [dbo].[tCertegy_CheckTypeMapping]

INSERT into [dbo].[tCertegy_CheckTypeMapping] values
            ('G', 3),  -- Government
			('Y', 6),  -- Handwritten Payroll
			('Y', 1),  -- Ins/Attorney/Cashiers
			('Y', 14), -- Loan/RAL
			('Y', 5),  -- Money Order
			('Y', 7),  -- Printed Payroll
			('P', 10), -- Two Party
			('G', 2)   -- US Treasury
END
GO