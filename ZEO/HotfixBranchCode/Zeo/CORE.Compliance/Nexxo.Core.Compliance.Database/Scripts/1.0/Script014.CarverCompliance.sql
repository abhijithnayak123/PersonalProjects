--===========================================================================================
-- Auther:			Raviraja
-- Date Created:	12/12/2013
-- Description:		Script for inserting Carver Compliance details
--===========================================================================================
INSERT INTO [dbo].[tCompliancePrograms]
	([rowguid],[Name],[RunOFAC],[DTCreate])
VALUES
	('E2965FBB-50D5-4D9F-ABBE-7576D5A28220', 'CarverCompliance', 0, GETDATE())

INSERT INTO [dbo].[tLimitTypes]
	([rowguid], [ComplianceProgramPK], [ClassId], [TypeId], [Name], [Value], [DTCreate])
VALUES
	('4C93FC36-14E5-4412-A745-CB062003FB85','E2965FBB-50D5-4D9F-ABBE-7576D5A28220', 'Product', 
	15, 'MoneyOrder', '4,5;503', GETDATE())

INSERT INTO [dbo].[tLimits]
	([rowguid],[LimitTypePK],[Name],[PerX],[PerDay],[PerNDays],[NDays],[IsDefault],[MultipleNDaysLimits],[DTCreate])
VALUES
	(NEWID(),'4C93FC36-14E5-4412-A745-CB062003FB85','Carver MoneyOrder',1000,-1,-1,-1,1,NULL,GETDATE())

INSERT INTO [dbo].[tTransactionMinimums]
	([rowguid],[ComplianceProgramPK],[TransactionType],[Minimum],[DTCreate])
VALUES
	(NEWID(),'E2965FBB-50D5-4D9F-ABBE-7576D5A28220',5,10,getdate()),/*MoneyOrder-5*/
	(NEWID(),'E2965FBB-50D5-4D9F-ABBE-7576D5A28220',6,50,getdate()),/*MoneyTransfer-6*/
	(NEWID(),'E2965FBB-50D5-4D9F-ABBE-7576D5A28220',2,3,getdate()),/*Check -2*/
	(NEWID(),'E2965FBB-50D5-4D9F-ABBE-7576D5A28220',4,10,getdate())/*BillPay -4*/

--===========================================================================================