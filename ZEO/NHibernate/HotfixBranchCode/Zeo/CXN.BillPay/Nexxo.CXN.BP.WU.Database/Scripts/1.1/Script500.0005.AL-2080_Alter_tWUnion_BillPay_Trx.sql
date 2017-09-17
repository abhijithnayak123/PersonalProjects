-- ================================================================================
-- Author:		<Sunil Shetty>
-- Create date: <10/08/2015>
-- Description:	<Alter DOB to nullable As Carver, I'd like to reduce the number of required registration fields>
-- Jira ID:		<AL-2080>
-- ================================================================================

IF EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'Sender_DateOfBirth' 
AND Object_ID = Object_ID(N'tWUnion_BillPay_Trx'))
BEGIN
ALTER TABLE tWUnion_BillPay_Trx ALTER COLUMN Sender_DateOfBirth Datetime NULL
END

