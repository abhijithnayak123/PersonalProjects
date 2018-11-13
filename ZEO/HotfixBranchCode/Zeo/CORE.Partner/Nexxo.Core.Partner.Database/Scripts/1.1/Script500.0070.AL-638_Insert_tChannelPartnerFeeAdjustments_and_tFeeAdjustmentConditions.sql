 --===========================================================================================
-- Auther:			<Chinar Kulkarni>
-- Date Created:	<22-July-2015>
-- Description:		<Script for Inserting new Promotion Codes for TCF>
-- Jira ID:			<AL-638>
--===========================================================================================

/*Promotion type          :	 Code
Client						 TCF
Group/Promotion code      :	 TCFOCMO
Promotion name            :	 Free TCF official check & money order cashing
Product					     Check
Promotion                 :	 100% off fee
Promotion constraints     :	 max $ off = n/a
Promotion criteria		  :  check types = Insurance/Attorney/Cashiers OR money order
Promotion priority		  :	 1
Start date 				  :   1-Jul-15
Offer earn end date 	  :   n/a
Offer processed end date  :  n/a
*/ 

IF NOT EXISTS(SELECT 1 FROM tChannelPartnerFeeAdjustments WHERE FeeAdjustmentPK = '452dcb2f-5bd9-4ab1-a2a2-2f66ef064a68')
BEGIN
	INSERT [dbo].[tChannelPartnerFeeAdjustments] ([FeeAdjustmentPK], [ChannelPartnerPK], [TransactionType], [Name], [Description], [DTStart], [SystemApplied], [AdjustmentRate], [AdjustmentAmount], [DTServerCreate], [PromotionType]) 
	VALUES (N'452dcb2f-5bd9-4ab1-a2a2-2f66ef064a68', N'6D7E785F-7BDD-42C8-BC49-44536A1885FC', 1, N'TCFOCMO', N'Free TCF official check & money order cashing', CAST(N'2015-07-01 00:00:00.000' AS DateTime), 0, -1.0000, 0.0000, GETDATE(), N'Code')
	
	/*To check for check type */
	INSERT [dbo].[tFeeAdjustmentConditions] ([AdjConditionsPK], [FeeAdjustmentPK], [ConditionTypePK], [CompareTypePK], [ConditionValue], [DTServerCreate]) 
	VALUES (NEWID(), N'452dcb2f-5bd9-4ab1-a2a2-2f66ef064a68', 5, 3, N'1,5', GETDATE())
	
	/*To check for code type */
	INSERT [dbo].[tFeeAdjustmentConditions] ([AdjConditionsPK], [FeeAdjustmentPK], [ConditionTypePK], [CompareTypePK], [ConditionValue], [DTServerCreate]) 
	VALUES (NEWID(), N'452dcb2f-5bd9-4ab1-a2a2-2f66ef064a68', 9, 1, 'TCFOCMO', GETDATE())
END
GO
---------------------------------------------------------------------------
/*
Promotion type			:	Code
Client						TCF
Group/Promotion code	:	CourtesyFree
Promotion name			:	Free check cashing for TCF Check Cash Customers
Product						Check
Promotion				:	100% off fee
Promotion constraints	:	max $ off = n/a
Promotion criteria		:	Check types = any type
Promotion priority		:	8
Start date				:	01-Jul-15
Offer earn end date		:	n/a
Offer processed end date:	End of full roll out
*/

IF NOT EXISTS(SELECT 1 FROM tChannelPartnerFeeAdjustments WHERE FeeAdjustmentPK = 'cfbe2904-4da2-4e2e-a766-4499cf181059')
BEGIN
	INSERT [dbo].[tChannelPartnerFeeAdjustments] ([FeeAdjustmentPK], [ChannelPartnerPK], [TransactionType], [Name], [Description], [DTStart], [SystemApplied], [AdjustmentRate], [AdjustmentAmount], [DTServerCreate], [PromotionType]) 
	VALUES (N'cfbe2904-4da2-4e2e-a766-4499cf181059', N'6d7e785f-7bdd-42c8-bc49-44536a1885fc', 1, N'CourtesyFree', N'Free check cashing for TCF Check Cash Customers', CAST(N'2015-07-01 00:00:00.000' AS DateTime), 0, -1.0000, 0.0000, GETDATE(), N'Code')
	
	/*To check for code type */
	INSERT [dbo].[tFeeAdjustmentConditions] ([AdjConditionsPK], [FeeAdjustmentPK], [ConditionTypePK], [CompareTypePK], [ConditionValue], [DTServerCreate]) 
	VALUES (NEWID(), N'cfbe2904-4da2-4e2e-a766-4499cf181059', 9, 1, 'CourtesyFree', GETDATE())
END
GO
--------------------------------------------------------------------------------
/*
Promotion type			:	Code
Client						TCF
Group/Promotion code	:	TCFDiv
Promotion name			:	Free TCF dividend check cashing
Product						Check
Promotion				:	100% off fee
Promotion constraints	:	max $ off = n/a
Promotion criteria		:	check type = Two Party
Promotion priority		:	2
Start date				:	01-Jul-15
Offer earn end date		:	n/a
Offer processed end date:	n/a
*/
IF NOT EXISTS(SELECT 1 FROM tChannelPartnerFeeAdjustments WHERE FeeAdjustmentPK = '70c83946-9e80-4abf-acc2-b97000b30b75')
BEGIN
	INSERT [dbo].[tChannelPartnerFeeAdjustments] ([FeeAdjustmentPK], [ChannelPartnerPK], [TransactionType], [Name], [Description], [DTStart], [SystemApplied], [AdjustmentRate], [AdjustmentAmount], [DTServerCreate], [PromotionType]) 
	VALUES (N'70c83946-9e80-4abf-acc2-b97000b30b75', N'6d7e785f-7bdd-42c8-bc49-44536a1885fc', 1, N'TCFDiv', N'Free TCF dividend check cashing', CAST(N'2015-07-01 00:00:00.000' AS DateTime), 0, -1.0000, 0.0000, GETDATE(), N'Code')
	
	/*To check for check type */
	INSERT [dbo].[tFeeAdjustmentConditions] ([AdjConditionsPK], [FeeAdjustmentPK], [ConditionTypePK], [CompareTypePK], [ConditionValue], [DTServerCreate]) 
	VALUES (NEWID(), N'70c83946-9e80-4abf-acc2-b97000b30b75', 5, 1, N'10', GETDATE())
	
	/*To check for code type */
	INSERT [dbo].[tFeeAdjustmentConditions] ([AdjConditionsPK], [FeeAdjustmentPK], [ConditionTypePK], [CompareTypePK], [ConditionValue], [DTServerCreate]) 
	VALUES (NEWID(), N'70c83946-9e80-4abf-acc2-b97000b30b75', 9, 1, 'TCFDiv', GETDATE())
END
GO
-----------------------------------------------------------------------------
/*
Promotion type			:	Code
Client						TCF
Group/Promotion code	:	FreeCommand
Promotion name			:	Free command credit line check cashing
Product						Check
Promotion				:	100% off fee
Promotion constraints	:	max $ off = n/a
Promotion criteria		:	check type = Two Party
Promotion priority		:	3
Start date				:	01-Jul-15
Offer earn end date		:	n/a
Offer processed end date:	n/a
*/
IF NOT EXISTS(SELECT 1 FROM tChannelPartnerFeeAdjustments WHERE FeeAdjustmentPK = '50b73c32-7d29-46b1-9917-b76f3f4abf34')
BEGIN
	INSERT [dbo].[tChannelPartnerFeeAdjustments] ([FeeAdjustmentPK], [ChannelPartnerPK], [TransactionType], [Name], [Description], [DTStart], [SystemApplied], [AdjustmentRate], [AdjustmentAmount], [DTServerCreate], [PromotionType]) 
	VALUES (N'50b73c32-7d29-46b1-9917-b76f3f4abf34', N'6d7e785f-7bdd-42c8-bc49-44536a1885fc', 1, N'FreeCommand', N'Free command credit line check cashing', CAST(N'2015-07-01 00:00:00.000' AS DateTime), 0, -1.0000, 0.0000, GETDATE(), N'Code')
	
	/*To check for check type */
	INSERT [dbo].[tFeeAdjustmentConditions] ([AdjConditionsPK], [FeeAdjustmentPK], [ConditionTypePK], [CompareTypePK], [ConditionValue], [DTServerCreate]) 
	VALUES (NEWID(), N'50b73c32-7d29-46b1-9917-b76f3f4abf34', 5, 1, N'10', GETDATE())
	
	/*To check for code type */
	INSERT [dbo].[tFeeAdjustmentConditions] ([AdjConditionsPK], [FeeAdjustmentPK], [ConditionTypePK], [CompareTypePK], [ConditionValue], [DTServerCreate]) 
	VALUES (NEWID(), N'50b73c32-7d29-46b1-9917-b76f3f4abf34', 9, 1,'FreeCommand', GETDATE())
END
GO
----------------------------------------------------------------------------------
/*
Promotion type:	Code
Client						TCF
Group/Promotion code	:	LoanCust
Promotion nam			:	Free check cashing when making a TCF loan payment
Product						Check
Promotion				:	100% off fee
Promotion constraints	:	max $ off = n/a
Promotion criteria		:	Check types = any type
Promotion priority		:	6
Start date				:	01-Jul-15
Offer earn end date		:	n/a
Offer processed end date:	n/a
*/
IF NOT EXISTS(SELECT 1 FROM tChannelPartnerFeeAdjustments WHERE FeeAdjustmentPK = '1e693f4f-3239-498f-ba94-668607db2b00')
BEGIN
	INSERT [dbo].[tChannelPartnerFeeAdjustments] ([FeeAdjustmentPK], [ChannelPartnerPK], [TransactionType], [Name], [Description], [DTStart], [SystemApplied], [AdjustmentRate], [AdjustmentAmount], [DTServerCreate], [PromotionType]) 
	VALUES (N'1e693f4f-3239-498f-ba94-668607db2b00', N'6d7e785f-7bdd-42c8-bc49-44536a1885fc', 1, N'LoanCust', N'Free check cashing when making a TCF loan payment', CAST(N'2015-07-01 00:00:00.000' AS DateTime), 0, -1.0000, 0.0000, GETDATE(), N'Code')
	
	/*To check for code type */
	INSERT [dbo].[tFeeAdjustmentConditions] ([AdjConditionsPK], [FeeAdjustmentPK], [ConditionTypePK], [CompareTypePK], [ConditionValue], [DTServerCreate]) 
	VALUES (NEWID(), N'1e693f4f-3239-498f-ba94-668607db2b00', 9, 1, 'LoanCust', GETDATE())
END
GO
--------------------------------------------------------------------------------
/*
Promotion type			 :	Code
Client						TCF
Group/Promotion code	 :	FreeCorporate
Promotion name			 :	Free check cashing for TCF Commercial accounts
Product						Check
Promotion				 :	100% off fee
Promotion constraints	 :	max $ off = n/a
Promotion criteria		 :	check types = Preprinted OR Handwritten Payroll OR Two Party
Promotion priority		 :	7
Start date				 :	01-Jul-15
Offer earn end date		 :	n/a
Offer processed end date :	n/a
*/
IF NOT EXISTS(SELECT 1 FROM tChannelPartnerFeeAdjustments WHERE FeeAdjustmentPK = '19c36147-0211-40ee-8e47-e5889789d1e4')
BEGIN
	INSERT [dbo].[tChannelPartnerFeeAdjustments] ([FeeAdjustmentPK], [ChannelPartnerPK], [TransactionType], [Name], [Description], [DTStart], [SystemApplied], [AdjustmentRate], [AdjustmentAmount], [DTServerCreate], [PromotionType]) 
	VALUES (N'19c36147-0211-40ee-8e47-e5889789d1e4', N'6d7e785f-7bdd-42c8-bc49-44536a1885fc', 1, N'FreeCorporate', N'Free check cashing for TCF Commercial accounts', CAST(N'2015-07-01 00:00:00.000' AS DateTime), 0, -1.0000, 0.0000, GETDATE(), N'Code')
	
	/*To check for check type */
	INSERT [dbo].[tFeeAdjustmentConditions] ([AdjConditionsPK], [FeeAdjustmentPK], [ConditionTypePK], [CompareTypePK], [ConditionValue], [DTServerCreate]) 
	VALUES (NEWID(), N'19c36147-0211-40ee-8e47-e5889789d1e4', 5, 3, N'6,7,10', GETDATE())
	
	/*To check for code type */
	INSERT [dbo].[tFeeAdjustmentConditions] ([AdjConditionsPK], [FeeAdjustmentPK], [ConditionTypePK], [CompareTypePK], [ConditionValue], [DTServerCreate]) 
	VALUES (NEWID(), N'19c36147-0211-40ee-8e47-e5889789d1e4', 9, 1,'FreeCorporate', GETDATE())
END
GO
--------------------------------------------------------------------------------
/*
Promotion type			 :	Group
Client						TCF
Group/Promotion code	 :	CommercialPayroll
Promotion name			 :	Free TCF payroll check cashing for commercial accounts
Product						Check
Promotion				 :	100% off fee
Promotion constraints	 :	max $ off = n/a
Promotion criteria		 :	check types = Preprinted OR Handwritten Payroll
Promotion priority		 :	4
Start date				 :	01-Jul-15
Offer earn end date		 :	n/a
Offer processed end date :	n/a
*/
IF NOT EXISTS(SELECT 1 FROM tChannelPartnerFeeAdjustments WHERE FeeAdjustmentPK = '505B3EC5-42F3-43F1-BEDE-EE7B39B0A4F9')
BEGIN
	INSERT [dbo].[tChannelPartnerFeeAdjustments] ([FeeAdjustmentPK], [ChannelPartnerPK], [TransactionType], [Name], [Description], [DTStart], [SystemApplied], [AdjustmentRate], [AdjustmentAmount], [DTServerCreate], [PromotionType]) 
	VALUES (N'505B3EC5-42F3-43F1-BEDE-EE7B39B0A4F9', N'6d7e785f-7bdd-42c8-bc49-44536a1885fc', 1, N'CommercialPayroll', N'Free TCF payroll check cashing for commercial accounts', CAST(N'2015-07-01 00:00:00.000' AS DateTime), 0, -1.0000, 0.0000, GETDATE(), N'Code')
	
	/*To check for check type */
	INSERT [dbo].[tFeeAdjustmentConditions] ([AdjConditionsPK], [FeeAdjustmentPK], [ConditionTypePK], [CompareTypePK], [ConditionValue], [DTServerCreate]) 
	VALUES (NEWID(), N'505B3EC5-42F3-43F1-BEDE-EE7B39B0A4F9', 5, 3, N'6,7', GETDATE())
	
	/*To check for code type */
	INSERT [dbo].[tFeeAdjustmentConditions] ([AdjConditionsPK], [FeeAdjustmentPK], [ConditionTypePK], [CompareTypePK], [ConditionValue], [DTServerCreate]) 
	VALUES (NEWID(), N'505B3EC5-42F3-43F1-BEDE-EE7B39B0A4F9', 1, 1,'CommercialPayroll', GETDATE())
END
GO
--------------------------------------------------------------------------------
/*
Promotion type			 :	Group
Client						TCF
Group/Promotion code	 :	InstorePayroll
Promotion name			 :	Free payroll check cashing for Jewel/Osco and Cub Foods employees
Product						Check
Promotion				 :	100% off fee
Promotion constraints	 :	max $ off = n/a
Promotion criteria		 :	check types = Preprinted OR Handwritten Payroll
Promotion priority		 :	5
Start date				 :	01-Jul-15
Offer earn end date		 :	n/a
Offer processed end date :	n/a
*/

IF NOT EXISTS(SELECT 1 FROM tChannelPartnerFeeAdjustments WHERE FeeAdjustmentPK = 'F10FB156-31DF-44CA-B8CF-43AC52745839')
BEGIN
	INSERT [dbo].[tChannelPartnerFeeAdjustments] ([FeeAdjustmentPK], [ChannelPartnerPK], [TransactionType], [Name], [Description], [DTStart], [SystemApplied], [AdjustmentRate], [AdjustmentAmount], [DTServerCreate], [PromotionType]) 
	VALUES (N'F10FB156-31DF-44CA-B8CF-43AC52745839', N'6d7e785f-7bdd-42c8-bc49-44536a1885fc', 1, N'InstorePayroll', N'Free payroll check cashing for Jewel/Osco and Cub Foods employees', CAST(N'2015-07-01 00:00:00.000' AS DateTime), 0, -1.0000, 0.0000, GETDATE(), N'Code')
	
	/*To check for check type */
	INSERT [dbo].[tFeeAdjustmentConditions] ([AdjConditionsPK], [FeeAdjustmentPK], [ConditionTypePK], [CompareTypePK], [ConditionValue], [DTServerCreate]) 
	VALUES (NEWID(), N'F10FB156-31DF-44CA-B8CF-43AC52745839', 5, 3, N'6,7', GETDATE())
	
	/*To check for code type */
	INSERT [dbo].[tFeeAdjustmentConditions] ([AdjConditionsPK], [FeeAdjustmentPK], [ConditionTypePK], [CompareTypePK], [ConditionValue], [DTServerCreate]) 
	VALUES (NEWID(), N'F10FB156-31DF-44CA-B8CF-43AC52745839', 1, 1, 'InstorePayroll', GETDATE())	
END
GO
--------------------------------------------------------------------------------