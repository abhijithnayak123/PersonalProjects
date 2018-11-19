SET NOCOUNT ON

-- [TBL_PolicyLevel] Migration script
INSERT INTO TBL_PolicyLevel
SELECT PolicyLevel
	,CASE 
		WHEN LevelName = 'Client'
			THEN 'Manager'
		WHEN LevelName = 'Program'
			THEN 'Alliance'
		ELSE LevelName
		END AS LevelName
FROM $(ExcelsiorDB)..PolicyLevel

-- TBL_PolicyCategory Migration Script 
INSERT INTO TBL_PolicyCategory
VALUES (
	1
	,'Paragon'
	,'Paragon - Payment Profile Settings'
	)

INSERT INTO TBL_PolicyCategory (
	PolicyCategoryID
	,FullName
	,Description
	)
SELECT PolicyCategoryID
	,FullName
	,Description
FROM $(ExcelsiorDB)..PolicyCategory
WHERE PolicyCategoryID IN (
		SELECT DISTINCT PolicyCategoryID
		FROM $(ExcelsiorDB)..PolicyDimension
WHERE FULLNAME IN (
		'Client Name'
		,'TARP FCB Rollover'
		,'TARP Mkt Value Roll.'
		,'TARP Data Extract'
		,'Rpt Schedule'
		,'Mon Bene Rpt Prn set'
		,'Qtrly report copies'
		,'SAnn Ben Rpt Prnset'
		,'Ann Bene Rpt Copies'
		,'Ann Don Rpt Prn set'
		,'Mon Publish to Web'
		,'Qtr Publish to Web'
		,'SAnn Publish to Web'
		,'Ann Publish to Web'
		,'PIF Key Statistics'
		,'PIF Account History'
		,'PIF Unit Dec Prec'
		,'Payment Summary'
		,'Ann Acct History'
		,'Ann Alloc Summary'
		,'Ann Perf Summary'
		,'Per Perf Summary'
		,'Ann Portfolio App'
		,'Per Portfolio App'
		,'Cli Acct # on Rpts'
		,'Spigot Special Page'
		,'Min Req Dist (DAF)'
		,'Donor Advised Fund'
		,'Client Color Scheme'
		,'Client Logo'
		,'Client Contact'
		,'Mon DVD copies'
		,'Qtr DVD copies'
		,'SAnn DVD copies'
		,'No. of CD - Ann Rpt'
		,'Prior Period Adj'
		,'Custom label'
		,'TARP Prod. Cycle'
		,'TARP Calendar'
		,'CTS Production'
		,'Perf Net of Fees'
		,'Comp. Statistics Rpt'
		,'Exc from Rpt Medians'
		,'CTS Client Policy'
		,'PV Rpt by Acct Type'
		,'PV Rpt by Designatn'
		,'Retirement/Perpetual'
		,'Srt dt for perform.'
		,'Display Pmt Report'
		,'K-1 MAILING RULE'
		,'Trustee Change Date'
		,'Tax Return Ext. Date'
		,'Account Transfer Dt'
		,'Address Change Date'
		,'1099R Media'
		,'TX RTN MAILING RULE'
		,'MN State ID'
		,'Depre. Schedule'
		,'Mon Send To'
		,'Qtrly Send To'
		,'SAnn Send To'
		,'Ann Send To'
		,'Mon Client Recipient'
		,'Qtr Client Recipient'
		,'SA Client Recipient'
		,'Ann Client Recipient'
		,'Label Logo'
		,'Mon Perf Summary'
		,'SA Perf Summary'
		,'Mon Portfolio App'
		,'SA Portfolio App'
		,'PO Start Date'
		,'PO End Date'
		,'QAS Start Date'
		,'Fiscal Year End'
		,'High Priority'
		,'High Priority K-1'
		,'Institution Type'
		,'Maturity Dist Policy'
		) )

-- [TBL_PolicyDimension] Migration Script
INSERT INTO [TBL_PolicyDimension]
SELECT PolicyDimensionID
	,replace(FullName, 'Logo on Payments', 'Logo on Beneficiary Payments') AS FullName
	,DataType
	,MaxPolicyLevel
	,CASE 
		WHEN FullName IN (
				 'Logo on Envelopes'
				,'Envelopes'
				,'Logo on Payments'
				,'Tax Pmt Mail Inst.'
				,'Address on Payments'
				)
			THEN 1
		ELSE PolicyCategoryID
		END
	,replace(Description, 'Logo on Payments', 'Logo on Beneficiary Payments') AS Description
FROM $(ExcelsiorDB)..PolicyDimension
WHERE FullName IN (
		'Logo on Envelopes'
		,'Envelopes'
		,'Logo on Payments'
		,'Tax Pmt Mail Inst.'
		,'Address on Payments'
		,'Client Name'
		,'TARP FCB Rollover'
		,'TARP Mkt Value Roll.'
		,'TARP Data Extract'
		,'Rpt Schedule'
		,'Mon Bene Rpt Prn set'
		,'Qtrly report copies'
		,'SAnn Ben Rpt Prnset'
		,'Ann Bene Rpt Copies'
		,'Ann Don Rpt Prn set'
		,'Mon Publish to Web'
		,'Qtr Publish to Web'
		,'SAnn Publish to Web'
		,'Ann Publish to Web'
		,'PIF Key Statistics'
		,'PIF Account History'
		,'PIF Unit Dec Prec'
		,'Payment Summary'
		,'Ann Acct History'
		,'Ann Alloc Summary'
		,'Ann Perf Summary'
		,'Per Perf Summary'
		,'Ann Portfolio App'
		,'Per Portfolio App'
		,'Cli Acct # on Rpts'
		,'Spigot Special Page'
		,'Min Req Dist (DAF)'
		,'Donor Advised Fund'
		,'Client Color Scheme'
		,'Client Logo'
		,'Client Contact'
		,'Mon DVD copies'
		,'Qtr DVD copies'
		,'SAnn DVD copies'
		,'No. of CD - Ann Rpt'
		,'Prior Period Adj'
		,'Custom label'
		,'TARP Prod. Cycle'
		,'TARP Calendar'
		,'CTS Production'
		,'Perf Net of Fees'
		,'Comp. Statistics Rpt'
		,'Exc from Rpt Medians'
		,'CTS Client Policy'
		,'PV Rpt by Acct Type'
		,'PV Rpt by Designatn'
		,'Retirement/Perpetual'
		,'Srt dt for perform.'
		,'Display Pmt Report'
		,'K-1 MAILING RULE'
		,'Trustee Change Date'
		,'Tax Return Ext. Date'
		,'Account Transfer Dt'
		,'Address Change Date'
		,'1099R Media'
		,'TX RTN MAILING RULE'
		,'MN State ID'
		,'Depre. Schedule'
		,'Mon Send To'
		,'Qtrly Send To'
		,'SAnn Send To'
		,'Ann Send To'
		,'Mon Client Recipient'
		,'Qtr Client Recipient'
		,'SA Client Recipient'
		,'Ann Client Recipient'
		,'Label Logo'
		,'Mon Perf Summary'
		,'SA Perf Summary'
		,'Mon Portfolio App'
		,'SA Portfolio App'
		,'PO Start Date'
		,'PO End Date'
		,'QAS Start Date'
		,'Fiscal Year End'
		,'High Priority'
		,'High Priority K-1'
		,'Institution Type'
		,'Maturity Dist Policy'
		)

-- [TBL_PolicyDropDown] Migration Script
INSERT INTO [TBL_PolicyDropDown]
SELECT PolicyDropDownID
	,PolicyDimensionID
	,Replace(DropDownText, 'KCO Envelopes', 'KCo Envelopes')
	,Replace(Description, 'KCO Envelopes', 'KCo Envelopes')
FROM $(ExcelsiorDB)..PolicyDropDown
WHERE policydimensionid IN (
		SELECT PolicyDimensionID
		FROM TBL_PolicyDimension
		)

INSERT INTO [TBL_PolicyDimension]
SELECT MAX(PolicyDimensionID) + 100
	,'Payment Logo/Marketing Message'
	,'List'
	,100
	,1
	,'Payment Logo/Marketing Message'
FROM $(ExcelsiorDB)..PolicyDimension

INSERT INTO [TBL_PolicyDimension]
SELECT MAX(PolicyDimensionID) + 101
	,'Templates with withholding info'
	,'List'
	,100
	,1
	,'Templates with withholding info'
FROM $(ExcelsiorDB)..PolicyDimension

INSERT INTO [TBL_PolicyDimension]
SELECT MAX(PolicyDimensionID) + 102
	,'Check Signer'
	,'List'
	,300
	,1
	,'Whether the client or K&Co will sign beneficiary and vendor payments.'
FROM $(ExcelsiorDB)..PolicyDimension


DECLARE @PolicyDimensionID INT

SELECT @PolicyDimensionID = PolicyDimensionID
FROM [TBL_PolicyDimension]
WHERE FullName = 'Payment Logo/Marketing Message'

INSERT INTO [TBL_PolicyDropDown]
SELECT MAX(PolicyDropDownID) + 100
	,@PolicyDimensionID
	,'Manager Level'
	,'Manager Level'
FROM $(ExcelsiorDB)..PolicyDropDown

INSERT INTO [TBL_PolicyDropDown]
SELECT MAX(PolicyDropDownID) + 101
	,@PolicyDimensionID
	,'Alliance Level'
	,'Alliance Level'
FROM $(ExcelsiorDB)..PolicyDropDown

SELECT @PolicyDimensionID = PolicyDimensionID
FROM [TBL_PolicyDimension]
WHERE FullName = 'Templates with withholding info'

INSERT INTO [TBL_PolicyDropDown]
SELECT MAX(PolicyDropDownID) + 102
	,@PolicyDimensionID
	,'Yes'
	,'Yes'
FROM $(ExcelsiorDB)..PolicyDropDown

INSERT INTO [TBL_PolicyDropDown]
SELECT MAX(PolicyDropDownID) + 103
	,@PolicyDimensionID
	,'No'
	,'No'
FROM $(ExcelsiorDB)..PolicyDropDown

SELECT @PolicyDimensionID = PolicyDimensionID
FROM [TBL_PolicyDimension]
WHERE FullName = 'Check Signer'

INSERT INTO [TBL_PolicyDropDown]
SELECT MAX(PolicyDropDownID) + 104
	,@PolicyDimensionID
	,'Client'
	,'Client'
FROM $(ExcelsiorDB)..PolicyDropDown

INSERT INTO [TBL_PolicyDropDown]
SELECT MAX(PolicyDropDownID) + 105
	,@PolicyDimensionID
	,'Kaspick'
	,'Kaspick'
FROM $(ExcelsiorDB)..PolicyDropDown
