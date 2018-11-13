-- ============================================================
-- Author:		<Bineesh Raghavan>
-- Create date: <01/10/2014>
-- Description:	<Corrected IF check in script 
--				Script200.0004.Alter_table_tWUnion_BillPay_Trx.sql>
-- ============================================================
IF NOT EXISTS(SELECT 1 FROM sys.columns 
            WHERE Name = N'Sender_ComplianceDetails_TemplateID' AND Object_ID = Object_ID(N'tWUnion_BillPay_Trx'))
BEGIN
	ALTER TABLE 
		tWUnion_BillPay_Trx
	ADD	
		Sender_ComplianceDetails_TemplateID 					VARCHAR(20),		
		Sender_ComplianceDetails_IdDetails_IdType				VARCHAR(10),
		Sender_ComplianceDetails_IdDetails_IdCountryOfIssue		VARCHAR(50),
		Sender_ComplianceDetails_IdDetails_IdPlaceOfIssue		VARCHAR(50),
		Sender_ComplianceDetails_IdDetails_IdNumber				VARCHAR(50),
		Sender_ComplianceDetails_SecondID_IdType				VARCHAR(10),
		Sender_ComplianceDetails_SecondID_IdCountryOfIssue		VARCHAR(50),
		Sender_ComplianceDetails_SecondID_IdNumber				VARCHAR(50),	
		Sender_ComplianceDetails_DateOfBirth					VARCHAR(20),
		Sender_ComplianceDetails_Occupation						VARCHAR(50),
		Sender_ComplianceDetails_CurrentAddress_AddrLine1		VARCHAR(255),
		Sender_ComplianceDetails_CurrentAddress_AddrLine2		VARCHAR(255),
		Sender_ComplianceDetails_CurrentAddress_City			VARCHAR(50),
		Sender_ComplianceDetails_CurrentAddress_StateCode		VARCHAR(20),
		Sender_ComplianceDetails_CurrentAddress_PostalCode		VARCHAR(20),
		Sender_ComplianceDetails_ContactPhone					VARCHAR(20),
		Sender_ComplianceDetails_I_ActOnMyBehalf				VARCHAR(10),
		Sender_ComplianceDetails_Ack_Flag						VARCHAR(10), 
		Sender_ComplianceDetails_ComplianceData_Buffer			VARCHAR(500)
END 
GO