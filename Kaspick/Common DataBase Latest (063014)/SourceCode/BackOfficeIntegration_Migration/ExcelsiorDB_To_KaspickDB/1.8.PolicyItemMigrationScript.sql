SET NOCOUNT ON

DECLARE @PolicyDimensionID int
DECLARE @PolicyItemID int

SELECT @PolicyItemID = MAX(PolicyItemID)
FROM $(ExcelsiorDB)..PolicyItem

SET IDENTITY_INSERT TBL_PolicyItem ON

SET @PolicyItemID = @PolicyItemID + 100

-------------------------------------------------------------------------------------------------------------------

----'Logo on Payments'

INSERT INTO TBL_PolicyItem 
( PolicyItemID,PolicyDimensionID, PolicyLevel, OwnerID, NumericValue, TextValue, LogicalValue, DateValue, PolicyDropDownID,
Comment, ModifiedDate, ModifiedUserId, CreatedDate, CreatedUserId, DeletedUserId)
SELECT @PolicyItemID+ROW_NUMBER()over (order by PolicyItemID),PItem.PolicyDimensionID, PolicyLevel, clnt.BriefName,NULL,NULL,0,NULL,pdown.PolicyDropDownID,NULL,
GETDATE(),1,GETDATE(),1,null
from $(ExcelsiorDB)..PolicyItem PItem 
INNER JOIN  $(ExcelsiorDB).. policydimension PDim
ON PDim.PolicyDimensionID = PItem.PolicyDimensionID and fullname ='Logo on Payments'
inner JOIN $(ExcelsiorDB)..PolicyDropDown pdown ON pdown.PolicyDropDownID = PItem.PolicyDropDownID
inner JOIN $(ExcelsiorDB)..CLIENT clnt on clnt.ClientID = PItem.OwnerID AND PItem.PolicyLevel = 100

SELECT @PolicyItemID = MAX(PolicyItemID)
FROM TBL_PolicyItem

INSERT INTO TBL_PolicyItem 
( PolicyItemID,PolicyDimensionID, PolicyLevel, OwnerID, NumericValue, TextValue, LogicalValue, DateValue, PolicyDropDownID,
Comment, ModifiedDate, ModifiedUserId, CreatedDate, CreatedUserId, DeletedUserId)
SELECT @PolicyItemID+ROW_NUMBER()over (order by PolicyItemID),PItem.PolicyDimensionID, PolicyLevel, substring(pgm.BriefName,1,15),NULL,NULL,0,NULL,pdown.PolicyDropDownID,NULL,
GETDATE(),1,GETDATE(),1,null
from $(ExcelsiorDB)..PolicyItem PItem 
INNER JOIN  $(ExcelsiorDB).. policydimension PDim
ON PDim.PolicyDimensionID = PItem.PolicyDimensionID and fullname ='Logo on Payments'
inner JOIN $(ExcelsiorDB)..PolicyDropDown pdown ON pdown.PolicyDropDownID = PItem.PolicyDropDownID
inner JOIN $(ExcelsiorDB)..Program pgm on pgm.ProgramID = PItem.OwnerID AND PItem.PolicyLevel = 200

--INSERT INTO TBL_PolicyItem 
--( PolicyDimensionID, PolicyLevel, OwnerID, NumericValue, TextValue, LogicalValue, DateValue, PolicyDropDownID,
--Comment, ModifiedDate, ModifiedUserId, CreatedDate, CreatedUserId, DeletedUserId)
--SELECT PItem.PolicyDimensionID, PolicyLevel, acc.ADVENTID,NULL,NULL,0,NULL,pdown.PolicyDropDownID,NULL,
--GETDATE(),1,GETDATE(),1,null
--from $(ExcelsiorDB)..PolicyItem PItem 
--INNER JOIN  $(ExcelsiorDB).. policydimension PDim
--ON PDim.PolicyDimensionID = PItem.PolicyDimensionID and fullname ='Logo on Payments'
--inner JOIN $(ExcelsiorDB)..PolicyDropDown pdown ON pdown.PolicyDropDownID = PItem.PolicyDropDownID 
--AND pdown.PolicyDimensionID = PItem.PolicyDimensionID
--inner JOIN $(ExcelsiorDB)..V_EIS_EX_ACCOUNT acc on acc.ACCOUNTID = PItem.OwnerID AND PItem.PolicyLevel = 300
--ORDER BY PItem.OwnerID 

-------------------------------------------------------------------------------------------------------------------

--'Check signer'

SELECT @PolicyDimensionID = Policydimensionid from tbl_policydimension where fullname ='Check signer'

SELECT @PolicyItemID = MAX(PolicyItemID)
FROM TBL_PolicyItem

INSERT INTO TBL_PolicyItem 
( PolicyItemID,PolicyDimensionID, PolicyLevel, OwnerID, NumericValue, TextValue, LogicalValue, DateValue, PolicyDropDownID,
Comment, ModifiedDate, ModifiedUserId, CreatedDate, CreatedUserId, DeletedUserId)
SELECT @PolicyItemID+ROW_NUMBER()over (order by PolicyItemID),@PolicyDimensionID, PolicyLevel, clnt.BriefName,NULL,NULL,0,NULL,pdownupd.PolicyDropDownID,NULL,
GETDATE(),1,GETDATE(),1,null
from $(ExcelsiorDB)..PolicyItem PItem 
INNER JOIN  $(ExcelsiorDB).. policydimension PDim
ON PDim.PolicyDimensionID = PItem.PolicyDimensionID and fullname ='Check signer'
inner JOIN $(ExcelsiorDB)..PolicyDropDown pdown ON pdown.PolicyDropDownID = PItem.PolicyDropDownID
inner JOIN TBL_PolicyDropDown pdownupd on pdownupd.PolicyDimensionID = @PolicyDimensionID 
AND pdown.dropdowntext = pdownupd.DropDownText
inner JOIN $(ExcelsiorDB)..CLIENT clnt on clnt.ClientID = PItem.OwnerID AND PItem.PolicyLevel = 100

SELECT @PolicyItemID = MAX(PolicyItemID)
FROM TBL_PolicyItem

INSERT INTO TBL_PolicyItem 
( PolicyItemID,PolicyDimensionID, PolicyLevel, OwnerID, NumericValue, TextValue, LogicalValue, DateValue, PolicyDropDownID,
Comment, ModifiedDate, ModifiedUserId, CreatedDate, CreatedUserId, DeletedUserId)
SELECT @PolicyItemID+ROW_NUMBER()over (order by PolicyItemID),@PolicyDimensionID, PolicyLevel, substring(pgm.BriefName,1,15),NULL,NULL,0,NULL,pdownupd.PolicyDropDownID,NULL,
GETDATE(),1,GETDATE(),1,null
from $(ExcelsiorDB)..PolicyItem PItem 
INNER JOIN  $(ExcelsiorDB).. policydimension PDim
ON PDim.PolicyDimensionID = PItem.PolicyDimensionID and fullname ='Check signer'
inner JOIN $(ExcelsiorDB)..PolicyDropDown pdown ON pdown.PolicyDropDownID = PItem.PolicyDropDownID
inner JOIN TBL_PolicyDropDown pdownupd on pdownupd.PolicyDimensionID = @PolicyDimensionID 
AND pdown.dropdowntext = pdownupd.DropDownText
inner JOIN $(ExcelsiorDB)..Program pgm on pgm.ProgramID = PItem.OwnerID AND PItem.PolicyLevel = 200

SELECT @PolicyItemID = MAX(PolicyItemID)
FROM TBL_PolicyItem

INSERT INTO TBL_PolicyItem 
( PolicyItemID,PolicyDimensionID, PolicyLevel, OwnerID, NumericValue, TextValue, LogicalValue, DateValue, PolicyDropDownID,
Comment, ModifiedDate, ModifiedUserId, CreatedDate, CreatedUserId, DeletedUserId)
SELECT @PolicyItemID+ROW_NUMBER()over (order by PolicyItemID),@PolicyDimensionID, PolicyLevel, acc.ADVENTID,NULL,NULL,0,NULL,pdownupd.PolicyDropDownID,NULL,
GETDATE(),1,GETDATE(),1,null
from $(ExcelsiorDB)..PolicyItem PItem 
INNER JOIN  $(ExcelsiorDB).. policydimension PDim
ON PDim.PolicyDimensionID = PItem.PolicyDimensionID and fullname ='Check signer'
inner JOIN $(ExcelsiorDB)..PolicyDropDown pdown ON pdown.PolicyDropDownID = PItem.PolicyDropDownID 
AND pdown.PolicyDimensionID = PItem.PolicyDimensionID
inner JOIN TBL_PolicyDropDown pdownupd on pdownupd.PolicyDimensionID = @PolicyDimensionID 
AND pdown.dropdowntext = pdownupd.DropDownText
inner JOIN $(ExcelsiorDB)..V_EIS_EX_ACCOUNT acc on acc.ACCOUNTID = PItem.OwnerID AND PItem.PolicyLevel = 300
ORDER BY PItem.OwnerID 

-------------------------------------------------------------------------------------------------------------------

--'Logo on Envelopes'

SELECT @PolicyItemID = MAX(PolicyItemID)
FROM TBL_PolicyItem

INSERT INTO TBL_PolicyItem 
( PolicyItemID,PolicyDimensionID, PolicyLevel, OwnerID, NumericValue, TextValue, LogicalValue, DateValue, PolicyDropDownID,
Comment, ModifiedDate, ModifiedUserId, CreatedDate, CreatedUserId, DeletedUserId)
SELECT @PolicyItemID+ROW_NUMBER()over (order by PolicyItemID),PItem.PolicyDimensionID, PolicyLevel, clnt.BriefName,NULL,NULL,0,NULL,pdown.PolicyDropDownID,NULL,
GETDATE(),1,GETDATE(),1,null
from $(ExcelsiorDB)..PolicyItem PItem 
INNER JOIN  $(ExcelsiorDB).. policydimension PDim
ON PDim.PolicyDimensionID = PItem.PolicyDimensionID and fullname ='Logo on Envelopes'
inner JOIN $(ExcelsiorDB)..PolicyDropDown pdown ON pdown.PolicyDropDownID = PItem.PolicyDropDownID
inner JOIN $(ExcelsiorDB)..CLIENT clnt on clnt.ClientID = PItem.OwnerID AND PItem.PolicyLevel = 100

SELECT @PolicyItemID = MAX(PolicyItemID)
FROM TBL_PolicyItem

INSERT INTO TBL_PolicyItem 
( PolicyItemID,PolicyDimensionID, PolicyLevel, OwnerID, NumericValue, TextValue, LogicalValue, DateValue, PolicyDropDownID,
Comment, ModifiedDate, ModifiedUserId, CreatedDate, CreatedUserId, DeletedUserId)
SELECT @PolicyItemID+ROW_NUMBER()over (order by PolicyItemID),PItem.PolicyDimensionID, PolicyLevel, substring(pgm.BriefName,1,15),NULL,NULL,0,NULL,pdown.PolicyDropDownID,NULL,
GETDATE(),1,GETDATE(),1,null
from $(ExcelsiorDB)..PolicyItem PItem 
INNER JOIN  $(ExcelsiorDB).. policydimension PDim
ON PDim.PolicyDimensionID = PItem.PolicyDimensionID and fullname ='Logo on Envelopes'
inner JOIN $(ExcelsiorDB)..PolicyDropDown pdown ON pdown.PolicyDropDownID = PItem.PolicyDropDownID
inner JOIN $(ExcelsiorDB)..Program pgm on pgm.ProgramID = PItem.OwnerID AND PItem.PolicyLevel = 200

--INSERT INTO TBL_PolicyItem 
--( PolicyDimensionID, PolicyLevel, OwnerID, NumericValue, TextValue, LogicalValue, DateValue, PolicyDropDownID,
--Comment, ModifiedDate, ModifiedUserId, CreatedDate, CreatedUserId, DeletedUserId)
--SELECT PItem.PolicyDimensionID, PolicyLevel, acc.ADVENTID,NULL,NULL,0,NULL,pdown.PolicyDropDownID,NULL,
--GETDATE(),1,GETDATE(),1,null
--from $(ExcelsiorDB)..PolicyItem PItem 
--INNER JOIN  $(ExcelsiorDB).. policydimension PDim
--ON PDim.PolicyDimensionID = PItem.PolicyDimensionID and fullname ='Logo on Envelopes'
--inner JOIN $(ExcelsiorDB)..PolicyDropDown pdown ON pdown.PolicyDropDownID = PItem.PolicyDropDownID 
--AND pdown.PolicyDimensionID = PItem.PolicyDimensionID
--inner JOIN $(ExcelsiorDB)..V_EIS_EX_ACCOUNT acc on acc.ACCOUNTID = PItem.OwnerID AND PItem.PolicyLevel = 300
--ORDER BY PItem.OwnerID 

-------------------------------------------------------------------------------------------------------------------

--'Envelopes'

SELECT @PolicyItemID = MAX(PolicyItemID)
FROM TBL_PolicyItem

INSERT INTO TBL_PolicyItem 
( PolicyItemID,PolicyDimensionID, PolicyLevel, OwnerID, NumericValue, TextValue, LogicalValue, DateValue, PolicyDropDownID,
Comment, ModifiedDate, ModifiedUserId, CreatedDate, CreatedUserId, DeletedUserId)
SELECT @PolicyItemID+ROW_NUMBER()over (order by PolicyItemID),PItem.PolicyDimensionID, PolicyLevel, clnt.BriefName,NULL,NULL,0,NULL,pdown.PolicyDropDownID,NULL,
GETDATE(),1,GETDATE(),1,null
from $(ExcelsiorDB)..PolicyItem PItem 
INNER JOIN  $(ExcelsiorDB).. policydimension PDim
ON PDim.PolicyDimensionID = PItem.PolicyDimensionID and fullname ='Envelopes'
inner JOIN $(ExcelsiorDB)..PolicyDropDown pdown ON pdown.PolicyDropDownID = PItem.PolicyDropDownID
inner JOIN $(ExcelsiorDB)..CLIENT clnt on clnt.ClientID = PItem.OwnerID AND PItem.PolicyLevel = 100

SELECT @PolicyItemID = MAX(PolicyItemID)
FROM TBL_PolicyItem

INSERT INTO TBL_PolicyItem 
( PolicyItemID,PolicyDimensionID, PolicyLevel, OwnerID, NumericValue, TextValue, LogicalValue, DateValue, PolicyDropDownID,
Comment, ModifiedDate, ModifiedUserId, CreatedDate, CreatedUserId, DeletedUserId)
SELECT @PolicyItemID+ROW_NUMBER()over (order by PolicyItemID),PItem.PolicyDimensionID, PolicyLevel, substring(pgm.BriefName,1,15),NULL,NULL,0,NULL,pdown.PolicyDropDownID,NULL,
GETDATE(),1,GETDATE(),1,null
from $(ExcelsiorDB)..PolicyItem PItem 
INNER JOIN  $(ExcelsiorDB).. policydimension PDim
ON PDim.PolicyDimensionID = PItem.PolicyDimensionID and fullname ='Envelopes'
inner JOIN $(ExcelsiorDB)..PolicyDropDown pdown ON pdown.PolicyDropDownID = PItem.PolicyDropDownID
inner JOIN $(ExcelsiorDB)..Program pgm on pgm.ProgramID = PItem.OwnerID AND PItem.PolicyLevel = 200

--INSERT INTO TBL_PolicyItem 
--( PolicyDimensionID, PolicyLevel, OwnerID, NumericValue, TextValue, LogicalValue, DateValue, PolicyDropDownID,
--Comment, ModifiedDate, ModifiedUserId, CreatedDate, CreatedUserId, DeletedUserId)
--SELECT PItem.PolicyDimensionID, PolicyLevel, acc.ADVENTID,NULL,NULL,0,NULL,pdown.PolicyDropDownID,NULL,
--GETDATE(),1,GETDATE(),1,null
--from $(ExcelsiorDB)..PolicyItem PItem 
--INNER JOIN  $(ExcelsiorDB).. policydimension PDim
--ON PDim.PolicyDimensionID = PItem.PolicyDimensionID and fullname ='Envelopes'
--inner JOIN $(ExcelsiorDB)..PolicyDropDown pdown ON pdown.PolicyDropDownID = PItem.PolicyDropDownID 
--AND pdown.PolicyDimensionID = PItem.PolicyDimensionID
--inner JOIN $(ExcelsiorDB)..V_EIS_EX_ACCOUNT acc on acc.ACCOUNTID = PItem.OwnerID AND PItem.PolicyLevel = 300
--ORDER BY PItem.OwnerID 

-------------------------------------------------------------------------------------------------------------------

--'Tax Pmt Mail Inst.'

SELECT @PolicyItemID = MAX(PolicyItemID)
FROM TBL_PolicyItem

INSERT INTO TBL_PolicyItem 
( PolicyItemID,PolicyDimensionID, PolicyLevel, OwnerID, NumericValue, TextValue, LogicalValue, DateValue, PolicyDropDownID,
Comment, ModifiedDate, ModifiedUserId, CreatedDate, CreatedUserId, DeletedUserId)
SELECT @PolicyItemID+ROW_NUMBER()over (order by PolicyItemID),PItem.PolicyDimensionID, PolicyLevel, clnt.BriefName,NULL,NULL,0,NULL,pdown.PolicyDropDownID,NULL,
GETDATE(),1,GETDATE(),1,null
from $(ExcelsiorDB)..PolicyItem PItem 
INNER JOIN  $(ExcelsiorDB).. policydimension PDim
ON PDim.PolicyDimensionID = PItem.PolicyDimensionID and fullname ='Tax Pmt Mail Inst.'
inner JOIN $(ExcelsiorDB)..PolicyDropDown pdown ON pdown.PolicyDropDownID = PItem.PolicyDropDownID
inner JOIN $(ExcelsiorDB)..CLIENT clnt on clnt.ClientID = PItem.OwnerID AND PItem.PolicyLevel = 100

SELECT @PolicyItemID = MAX(PolicyItemID)
FROM TBL_PolicyItem

INSERT INTO TBL_PolicyItem 
( PolicyItemID,PolicyDimensionID, PolicyLevel, OwnerID, NumericValue, TextValue, LogicalValue, DateValue, PolicyDropDownID,
Comment, ModifiedDate, ModifiedUserId, CreatedDate, CreatedUserId, DeletedUserId)
SELECT @PolicyItemID+ROW_NUMBER()over (order by PolicyItemID),PItem.PolicyDimensionID, PolicyLevel, substring(pgm.BriefName,1,15),NULL,NULL,0,NULL,pdown.PolicyDropDownID,NULL,
GETDATE(),1,GETDATE(),1,null
from $(ExcelsiorDB)..PolicyItem PItem 
INNER JOIN  $(ExcelsiorDB).. policydimension PDim
ON PDim.PolicyDimensionID = PItem.PolicyDimensionID and fullname ='Tax Pmt Mail Inst.'
inner JOIN $(ExcelsiorDB)..PolicyDropDown pdown ON pdown.PolicyDropDownID = PItem.PolicyDropDownID
inner JOIN $(ExcelsiorDB)..Program pgm on pgm.ProgramID = PItem.OwnerID AND PItem.PolicyLevel = 200

--INSERT INTO TBL_PolicyItem 
--( PolicyDimensionID, PolicyLevel, OwnerID, NumericValue, TextValue, LogicalValue, DateValue, PolicyDropDownID,
--Comment, ModifiedDate, ModifiedUserId, CreatedDate, CreatedUserId, DeletedUserId)
--SELECT PItem.PolicyDimensionID, PolicyLevel, acc.ADVENTID,NULL,NULL,0,NULL,pdown.PolicyDropDownID,NULL,
--GETDATE(),1,GETDATE(),1,null
--from $(ExcelsiorDB)..PolicyItem PItem 
--INNER JOIN  $(ExcelsiorDB).. policydimension PDim
--ON PDim.PolicyDimensionID = PItem.PolicyDimensionID and fullname ='Tax Pmt Mail Inst.'
--inner JOIN $(ExcelsiorDB)..PolicyDropDown pdown ON pdown.PolicyDropDownID = PItem.PolicyDropDownID 
--AND pdown.PolicyDimensionID = PItem.PolicyDimensionID
--inner JOIN $(ExcelsiorDB)..V_EIS_EX_ACCOUNT acc on acc.ACCOUNTID = PItem.OwnerID AND PItem.PolicyLevel = 300
--ORDER BY PItem.OwnerID 

-------------------------------------------------------------------------------------------------------------------

--'Address on Payments'

SELECT @PolicyItemID = MAX(PolicyItemID)
FROM TBL_PolicyItem

INSERT INTO TBL_PolicyItem 
( PolicyItemID,PolicyDimensionID, PolicyLevel, OwnerID, NumericValue, TextValue, LogicalValue, DateValue, PolicyDropDownID,
Comment, ModifiedDate, ModifiedUserId, CreatedDate, CreatedUserId, DeletedUserId)
SELECT @PolicyItemID+ROW_NUMBER()over (order by PolicyItemID),PItem.PolicyDimensionID, PolicyLevel, clnt.BriefName,NULL,NULL,0,NULL,pdown.PolicyDropDownID,NULL,
GETDATE(),1,GETDATE(),1,null
from $(ExcelsiorDB)..PolicyItem PItem 
INNER JOIN  $(ExcelsiorDB).. policydimension PDim
ON PDim.PolicyDimensionID = PItem.PolicyDimensionID and fullname ='Address on Payments'
inner JOIN $(ExcelsiorDB)..PolicyDropDown pdown ON pdown.PolicyDropDownID = PItem.PolicyDropDownID
inner JOIN $(ExcelsiorDB)..CLIENT clnt on clnt.ClientID = PItem.OwnerID AND PItem.PolicyLevel = 100

SELECT @PolicyItemID = MAX(PolicyItemID)
FROM TBL_PolicyItem

INSERT INTO TBL_PolicyItem 
( PolicyItemID,PolicyDimensionID, PolicyLevel, OwnerID, NumericValue, TextValue, LogicalValue, DateValue, PolicyDropDownID,
Comment, ModifiedDate, ModifiedUserId, CreatedDate, CreatedUserId, DeletedUserId)
SELECT @PolicyItemID+ROW_NUMBER()over (order by PolicyItemID),PItem.PolicyDimensionID, PolicyLevel, substring(pgm.BriefName,1,15),NULL,NULL,0,NULL,pdown.PolicyDropDownID,NULL,
GETDATE(),1,GETDATE(),1,null
from $(ExcelsiorDB)..PolicyItem PItem 
INNER JOIN  $(ExcelsiorDB).. policydimension PDim
ON PDim.PolicyDimensionID = PItem.PolicyDimensionID and fullname ='Address on Payments'
inner JOIN $(ExcelsiorDB)..PolicyDropDown pdown ON pdown.PolicyDropDownID = PItem.PolicyDropDownID
inner JOIN $(ExcelsiorDB)..Program pgm on pgm.ProgramID = PItem.OwnerID AND PItem.PolicyLevel = 200

SELECT @PolicyItemID = MAX(PolicyItemID)
FROM TBL_PolicyItem

INSERT INTO TBL_PolicyItem 
( PolicyItemID,PolicyDimensionID, PolicyLevel, OwnerID, NumericValue, TextValue, LogicalValue, DateValue, PolicyDropDownID,
Comment, ModifiedDate, ModifiedUserId, CreatedDate, CreatedUserId, DeletedUserId)
SELECT @PolicyItemID+ROW_NUMBER()over (order by PolicyItemID),PItem.PolicyDimensionID, PolicyLevel, acc.ADVENTID,NULL,NULL,0,NULL,pdown.PolicyDropDownID,NULL,
GETDATE(),1,GETDATE(),1,null
from $(ExcelsiorDB)..PolicyItem PItem 
INNER JOIN  $(ExcelsiorDB).. policydimension PDim
ON PDim.PolicyDimensionID = PItem.PolicyDimensionID and fullname ='Address on Payments'
inner JOIN $(ExcelsiorDB)..PolicyDropDown pdown ON pdown.PolicyDropDownID = PItem.PolicyDropDownID 
AND pdown.PolicyDimensionID = PItem.PolicyDimensionID
inner JOIN $(ExcelsiorDB)..V_EIS_EX_ACCOUNT acc on acc.ACCOUNTID = PItem.OwnerID AND PItem.PolicyLevel = 300
ORDER BY PItem.OwnerID 

-------------------------------------------------------------------------------------------------------------------

-- Payment Logo/Marketing Message

SELECT @PolicyDimensionID = Policydimensionid from tbl_policydimension where fullname ='Payment Logo/Marketing Message'

SELECT @PolicyItemID = MAX(PolicyItemID)
FROM TBL_PolicyItem

INSERT INTO TBL_PolicyItem 
( PolicyItemID,PolicyDimensionID, PolicyLevel, OwnerID, NumericValue, TextValue, LogicalValue, DateValue, PolicyDropDownID,
Comment, ModifiedDate, ModifiedUserId, CreatedDate, CreatedUserId, DeletedUserId)
SELECT @PolicyItemID+ROW_NUMBER()over (order by PolicyDropDownID),@PolicyDimensionID, 100,Clnt.BriefName,NULL,NULL,0,NULL,PolicyDropDownID,
NULL,GETDATE(),1,GETDATE(),1,null
FROM $(ExcelsiorDB)..tbl_eis_ex_client_supplement ClntSupp
INNER JOIN $(ExcelsiorDB)..CLIENT Clnt ON Clnt.ClientID = ClntSupp.CLIENTID 
INNER JOIN $(ExcelsiorDB)..TBL_EIS_LIST_ITEM LItem ON LItem.LIST_ITEM_ID = ClntSupp.PmtLogo_MktMsg_ID
INNER JOIN TBL_PolicyDropDown PDDown on PDDown.PolicyDimensionID = @PolicyDimensionID AND 
	CASE WHEN LItem.LIST_ITEM_NAME ='Client Level' THEN 'Manager Level'
	when LItem.LIST_ITEM_NAME ='Program Level' THEN 'Alliance Level' END
	 = DropDownText 


-------------------------------------------------------------------------------------------------------------------

-- Templates with withholding info


SELECT @PolicyDimensionID = Policydimensionid from tbl_policydimension where fullname ='Templates with withholding info'

SELECT @PolicyItemID = MAX(PolicyItemID)
FROM TBL_PolicyItem

INSERT INTO TBL_PolicyItem 
( PolicyItemID,PolicyDimensionID, PolicyLevel, OwnerID, NumericValue, TextValue, LogicalValue, DateValue, PolicyDropDownID,
Comment, ModifiedDate, ModifiedUserId, CreatedDate, CreatedUserId, DeletedUserId)
SELECT @PolicyItemID+ROW_NUMBER()over (order by PolicyDropDownID),@PolicyDimensionID, 100,Clnt.BriefName,NULL,NULL,0,NULL,PolicyDropDownID,
NULL,GETDATE(),1,GETDATE(),1,null
FROM $(ExcelsiorDB)..tbl_eis_ex_client_supplement ClntSupp
INNER JOIN $(ExcelsiorDB)..CLIENT Clnt ON Clnt.ClientID = ClntSupp.CLIENTID 
INNER JOIN $(ExcelsiorDB)..TBL_EIS_LIST_ITEM LItem ON LItem.LIST_ITEM_ID = ClntSupp.TmplWithHold_ID
INNER JOIN TBL_PolicyDropDown PDDown on PDDown.PolicyDimensionID = @PolicyDimensionID AND 
	LItem.LIST_ITEM_NAME = DropDownText 


-- Delete data from Account which has same data as Alliance(program)
DELETE
FROM TBL_PolicyItem
WHERE PolicyItemID IN (
		SELECT Child.PolicyItemID
		FROM (
			SELECT PolicyDimensionID
				,PolicyDropDownID
				,OwnerID
				,PolicyItemID
			FROM TBL_PolicyItem
			WHERE PolicyLevel = 200
			) Parent
		INNER JOIN (
			SELECT PolicyDimensionID
				,PolicyDropDownID
				,AllianceNumber AS OwnerID
				,PolicyItemID
			FROM TBL_PolicyItem PItem
			INNER JOIN $(InnoTrustDB)..AccountMaster AccMstr
				ON PItem.OwnerID = AccMstr.CustomerAccountNumber
			WHERE PolicyLevel = 300
			) Child
			ON Parent.PolicyDimensionID = Child.PolicyDimensionID
				AND Parent.PolicyDropDownID = Child.PolicyDropDownID
				AND Child.OwnerID = Parent.OwnerID
		)


-- Delete data from Alliance(program) which has same data as Client
DELETE
FROM TBL_PolicyItem
WHERE PolicyItemID IN (
		SELECT Child.PolicyItemID
		FROM (
			SELECT PolicyDimensionID
				,PolicyDropDownID
				,OwnerID
				,PolicyItemID
			FROM TBL_PolicyItem
			WHERE PolicyLevel = 100
			) Parent
		INNER JOIN (
			SELECT DISTINCT PolicyDimensionID
				,PolicyDropDownID
				,ManagerCode AS OwnerID
				,PolicyItemID
			FROM TBL_PolicyItem PItem
			INNER JOIN $(InnoTrustDB)..AccountMaster AccMstr
				ON PItem.OwnerID = AccMstr.AllianceNumber
			WHERE PolicyLevel = 200
			) Child
			ON Parent.PolicyDimensionID = Child.PolicyDimensionID
				AND Parent.PolicyDropDownID = Child.PolicyDropDownID
				AND Child.OwnerID = Parent.OwnerID
		)


-- Delete data from Account which has same data as Manager(client)
DELETE
FROM TBL_PolicyItem
WHERE PolicyItemID IN (
		SELECT Child.PolicyItemID
		FROM (
			SELECT PolicyDimensionID
				,PolicyDropDownID
				,OwnerID
				,PolicyItemID
			FROM TBL_PolicyItem
			WHERE PolicyLevel = 100
			) Parent
		INNER JOIN (
			SELECT PolicyDimensionID
				,PolicyDropDownID
				,ManagerCode AS OwnerID
				,PolicyItemID
			FROM TBL_PolicyItem PItem
			INNER JOIN $(InnoTrustDB)..AccountMaster AccMstr
				ON PItem.OwnerID = AccMstr.CustomerAccountNumber
			WHERE PolicyLevel = 300
			) Child
			ON Parent.PolicyDimensionID = Child.PolicyDimensionID
				AND Parent.PolicyDropDownID = Child.PolicyDropDownID
				AND Child.OwnerID = Parent.OwnerID
		)
-------------------------------------------------------------------------------------------------

INSERT INTO TBL_PolicyItem (
	PolicyItemId
	,PolicyDimensionId
	,PolicyLevel
	,OwnerId
	,NumericValue
	,TextValue
	,LogicalValue
	,DateValue
	,PolicyDropdownId
	,Comment
	,ModifiedDate
	,ModifiedUserId
	,CreatedDate
	,CreatedUserId
	,DeletedUserId
	)
SELECT PolicyItemId
	,PolicyDimensionId
	,PolicyLevel
	,clnt.BriefName
	,NumericValue
	,TextValue
	,LogicalValue
	,DateValue
	,PolicyDropdownId
	,Comment
	,getdate()
	,1
	,getdate()
	,1
	,NULL
FROM $(ExcelsiorDB)..PolicyItem PItem
INNER JOIN $(ExcelsiorDB)..CLIENT clnt on clnt.ClientId = PItem.OwnerID AND PItem.PolicyLevel = 100
WHERE POLICYDIMENSIONID IN (
		SELECT DISTINCT policydimensionid
		FROM TBL_PolicyDimension
		WHERE Fullname IN (
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
				,'Maturity Dist Policy'
			)
		)
	AND PolicyItemId NOT IN (
		SELECT DISTINCT policyitemid
		FROM tbl_policyitem
		)
		
INSERT INTO TBL_PolicyItem (
	PolicyItemId
	,PolicyDimensionId
	,PolicyLevel
	,OwnerID
	,NumericValue
	,TextValue
	,LogicalValue
	,DateValue
	,PolicyDropdownId
	,Comment
	,ModifiedDate
	,ModifiedUserId
	,CreatedDate
	,CreatedUserId
	,DeletedUserId
	)
SELECT PolicyItemId
	,PolicyDimensionId
	,PolicyLevel
	,substring(pgm.BriefName,1,15)
	,NumericValue
	,TextValue
	,LogicalValue
	,DateValue
	,PolicyDropdownId
	,Comment
	,getdate()
	,1
	,getdate()
	,1
	,NULL
FROM $(ExcelsiorDB)..PolicyItem PItem
INNER JOIN $(ExcelsiorDB)..Program pgm on pgm.ProgramID = PItem.OwnerID AND PItem.PolicyLevel = 200
WHERE POLICYDIMENSIONID IN (
		SELECT DISTINCT policydimensionid
		FROM TBL_PolicyDimension
		WHERE Fullname IN (
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
				,'Maturity Dist Policy'
				)
		)
	AND PolicyItemId NOT IN (
		SELECT DISTINCT policyitemid
		FROM tbl_policyitem
		)
		
INSERT INTO TBL_PolicyItem (
	PolicyItemId
	,PolicyDimensionId
	,PolicyLevel
	,OwnerID
	,NumericValue
	,TextValue
	,LogicalValue
	,DateValue
	,PolicyDropdownId
	,Comment
	,ModifiedDate
	,ModifiedUserId
	,CreatedDate
	,CreatedUserId
	,DeletedUserId
	)
SELECT PolicyItemId
	,PolicyDimensionId
	,PolicyLevel
	,acc.ADVENTID
	,NumericValue
	,TextValue
	,LogicalValue
	,DateValue
	,PolicyDropdownId
	,Comment
	,getdate()
	,1
	,getdate()
	,1
	,NULL
FROM $(ExcelsiorDB)..PolicyItem PItem
INNER JOIN $(ExcelsiorDB)..V_EIS_EX_ACCOUNT acc on acc.ACCOUNTID = PItem.OwnerID AND PItem.PolicyLevel = 300
WHERE POLICYDIMENSIONID IN (
		SELECT DISTINCT policydimensionid
		FROM TBL_PolicyDimension
		WHERE Fullname IN (
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
				,'Maturity Dist Policy'
				)
		)
	AND PolicyItemId NOT IN (
		SELECT DISTINCT policyitemid
		FROM tbl_policyitem
		)
		
INSERT INTO TBL_PolicyItem (
	PolicyItemId
	,PolicyDimensionId
	,PolicyLevel
	,OwnerID
	,NumericValue
	,TextValue
	,LogicalValue
	,DateValue
	,PolicyDropdownId
	,Comment
	,ModifiedDate
	,ModifiedUserId
	,CreatedDate
	,CreatedUserId
	,DeletedUserId
	)
SELECT PolicyItemId
      ,PolicyDimensionId
      ,PolicyLevel
      ,clnt.BriefName
      ,CliEmp.SubContactId
      ,TextValue
      ,LogicalValue
      ,DateValue
      ,PolicyDropdownId
      ,Comment
      ,getdate()
      ,1
      ,getdate()
      ,1
      ,NULL
from $(ExcelsiorDB)..PolicyItem PItem 
INNER JOIN $(ExcelsiorDB)..CLIENT clnt on clnt.clientid = PItem.OwnerID AND PItem.PolicyLevel = 100
INNER JOIN $(MappingDB)..TBL_ClientEmployeeLookup CliEmp ON CliEmp.employeeid = PItem.numericvalue and clnt.clientid = CliEmp.clientid
where POLICYDIMENSIONID in 
(select POLICYDIMENSIONID from $(ExcelsiorDB)..PolicyDimension
where FullName in ('Mon Client Recipient'
                        ,'Qtr Client Recipient'
                        ,'SA Client Recipient'
                        ,'Ann Client Recipient'))
                        
INSERT INTO TBL_PolicyItem (
	PolicyItemId
	,PolicyDimensionId
	,PolicyLevel
	,OwnerID
	,NumericValue
	,TextValue
	,LogicalValue
	,DateValue
	,PolicyDropdownId
	,Comment
	,ModifiedDate
	,ModifiedUserId
	,CreatedDate
	,CreatedUserId
	,DeletedUserId
	)
SELECT PolicyItemId
      ,PolicyDimensionId
      ,PolicyLevel
      ,substring(pgm.BriefName,1,15)
      ,CliEmp.SubContactId
      ,TextValue
      ,LogicalValue
      ,DateValue
      ,PolicyDropdownId
      ,Comment
      ,getdate()
      ,1
      ,getdate()
      ,1
      ,NULL
from $(ExcelsiorDB)..PolicyItem PItem 
INNER JOIN $(ExcelsiorDB)..Program pgm on pgm.ProgramID = PItem.OwnerID AND PItem.PolicyLevel = 200
INNER JOIN $(MappingDB)..TBL_ClientEmployeeLookup CliEmp ON CliEmp.employeeid = PItem.numericvalue and pgm.clientid = CliEmp.clientid
where POLICYDIMENSIONID in 
(select POLICYDIMENSIONID from $(ExcelsiorDB)..PolicyDimension
where FullName in ('Mon Client Recipient'
                        ,'Qtr Client Recipient'
                        ,'SA Client Recipient'
                        ,'Ann Client Recipient'))
                        
INSERT INTO TBL_PolicyItem (
	PolicyItemId
	,PolicyDimensionId
	,PolicyLevel
	,OwnerID
	,NumericValue
	,TextValue
	,LogicalValue
	,DateValue
	,PolicyDropdownId
	,Comment
	,ModifiedDate
	,ModifiedUserId
	,CreatedDate
	,CreatedUserId
	,DeletedUserId
	)
SELECT PolicyItemId
      ,PolicyDimensionId
      ,PolicyLevel
      ,acc.ADVENTID
      ,CliEmp.SubContactId
      ,TextValue
      ,LogicalValue
      ,DateValue
      ,PolicyDropdownId
      ,Comment
      ,getdate()
      ,1
      ,getdate()
      ,1
      ,NULL
from $(ExcelsiorDB)..PolicyItem PItem 
INNER JOIN $(ExcelsiorDB)..V_EIS_EX_ACCOUNT acc on acc.ACCOUNTID = PItem.OwnerID AND PItem.PolicyLevel = 300
INNER JOIN $(MappingDB)..TBL_ClientEmployeeLookup CliEmp ON CliEmp.employeeid = PItem.numericvalue and acc.clientid = CliEmp.clientid
where POLICYDIMENSIONID in 
(select POLICYDIMENSIONID from $(ExcelsiorDB)..PolicyDimension
where FullName in ('Mon Client Recipient'
                        ,'Qtr Client Recipient'
                        ,'SA Client Recipient'
                        ,'Ann Client Recipient'))
                        

INSERT INTO TBL_PolicyItem (
	PolicyItemId
	,PolicyDimensionId
	,PolicyLevel
	,OwnerID
	,NumericValue
	,TextValue
	,LogicalValue
	,DateValue
	,PolicyDropdownId
	,Comment
	,ModifiedDate
	,ModifiedUserId
	,CreatedDate
	,CreatedUserId
	,DeletedUserId
	)
SELECT PolicyItemId
	,PolicyDimensionId
	,PolicyLevel
	,Clnt.BriefName
	,NumericValue
	,TextValue
	,LogicalValue
	,DateValue
	,PolicyDropdownId
	,Comment
	,getdate()
	,1
	,getdate()
	,1
	,NULL
FROM $(ExcelsiorDB)..PolicyItem PItem
INNER JOIN $(ExcelsiorDB)..Client Clnt on Clnt.ClientID = PItem.OwnerID AND PItem.PolicyLevel = 100
WHERE POLICYDIMENSIONID = 118

SET IDENTITY_INSERT TBL_PolicyItem OFF
		
SET NOCOUNT OFF

