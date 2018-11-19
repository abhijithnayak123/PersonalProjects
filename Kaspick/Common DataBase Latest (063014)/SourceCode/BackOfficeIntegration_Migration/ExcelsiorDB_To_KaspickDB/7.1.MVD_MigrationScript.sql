SET NOCOUNT ON
-- CalendarParameter
INSERT INTO CalendarParameter (
	CalendarParameterID
	,FrequencyType
	,Recurrence
	,Occurrence
	,MonthOccurrence
	,DayOccurrence
	,LookAhead
	,StartDate
	,EndDate
	,Comments
	)
SELECT CalendarParameterID
	,FrequencyType
	,Recurrence
	,Occurrence
	,MonthOccurrence
	,DayOccurrence
	,LookAhead
	,StartDate
	,EndDate
	,Comments
FROM $(ExcelsiorDB)..CalendarParameter

--Deliverable
INSERT INTO Deliverable (
	DeliverableID
	,FullName
	,[Description]
	,SystemName
	,IsMVD
	,RootFolder
	,DeliverableDepartmentID
	,DeliverableCategoryID
	,DerivedFrequency
	,CalendarParameterID
	,ItemsInHistory
	)
SELECT DeliverableID
	,FullName
	,[Description]
	,SystemName
	,IsMVD
	,RootFolder
	,DeliverableDepartmentID
	,DeliverableCategoryID
	,DerivedFrequency
	,CalendarParameterID
	,ItemsInHistory
FROM $(ExcelsiorDB)..DELIVERABLE

--DeliverableCategory
INSERT INTO DeliverableCategory (
	DeliverableCategoryID
	,FullName
	,[Description]
	)
SELECT DeliverableCategoryID
	,FullName
	,[Description]
FROM $(ExcelsiorDB)..DeliverableCategory

-- DeliverableDepartment
INSERT INTO DeliverableDepartment (
	DeliverableDepartmentID
	,Fullname
	,[Description]
	)
SELECT DeliverableDepartmentID
	,Fullname
	,[Description]
FROM $(ExcelsiorDB)..DeliverableDepartment

-- DeliverableDetail
INSERT INTO DeliverableDetail (
	DeliverableDetailID
	,ClientID
	,DeliverableSetID
	,Comments
	,LagDays
	,DerivedFrequency
	,CalendarParameterID
	,IsClosed
	)
SELECT DeliverableDetailID
	,ClientID
	,DeliverableSetID
	,Comments
	,LagDays
	,DerivedFrequency
	,CalendarParameterID
	,IsClosed
FROM $(ExcelsiorDB)..DELIVERABLEDETAIL

-- DeliverableFormat
INSERT INTO DeliverableFormat (
	FormatID
	,FullName
	,[Description]
	)
SELECT FormatID
	,FullName
	,[Description]
FROM $(ExcelsiorDB)..DeliverableFormat
GO

-- DeliverableHistoryRule
INSERT INTO DeliverableHistoryRules (
	DerivedFrequency
	,FrequencyCode
	,ItemsInHistory
	,WebDisplayText
	,[Description]
	)
SELECT DerivedFrequency
	,FrequencyCode
	,ItemsInHistory
	,WebDisplayText
	,[Description]
FROM $(ExcelsiorDB)..DeliverableHistoryRules

-- DeliverableHorizon
INSERT INTO DeliverableHorizon (
	HorizonID
	,DeliverableDetailID
	,ProjectedDate
	,NextPlannedDate
	,ActualDate
	,DisplayOnWeb
	,FileLocation
	,[FileName]
	,LastModifyDate
	,ErrorCode
	,WebDisplayOrder
	,WebDisplayText
	,CalendarParameterID
	,Comment
	,ErrorText
	,Author_Name
	,Upload_Date
	,File_Size_In_KB
	)
SELECT HorizonID
	,DeliverableDetailID
	,ProjectedDate
	,NextPlannedDate
	,ActualDate
	,DisplayOnWeb
	,FileLocation
	,[FileName]
	,LastModifyDate
	,ErrorCode
	,WebDisplayOrder
	,WebDisplayText
	,CalendarParameterID
	,Comment
	,ErrorText
	,Author_Name
	,Upload_Date
	,File_Size_In_KB
FROM $(ExcelsiorDB)..DELIVERABLEHORIZON

-- DeliverableMedia
INSERT INTO DeliverableMedia (
	MediaID
	,FullName
	,[Description]
	)
SELECT MediaID
	,FullName
	,[Description]
FROM $(ExcelsiorDB)..DELIVERABLEMEDIA

-- DeliverableSet
INSERT INTO DeliverableSet (
	DeliverableSetID
	,DeliverableID
	,VersionID
	,MediaID
	,FormatID
	)
SELECT DeliverableSetID
	,DeliverableID
	,VersionID
	,MediaID
	,FormatID
FROM $(ExcelsiorDB)..DELIVERABLESET

-- DeliverableVersion
INSERT INTO DeliverableVersion (
	VersionID
	,FullName
	,[Description]
	,DeliverableID
	)
SELECT VersionID
	,FullName
	,[Description]
	,DeliverableID
FROM $(ExcelsiorDB)..DELIVERABLEVERSION

--UNIQUEID
INSERT INTO UNIQUEID (
	IDType
	,LastValue
	,DataTable
	)
SELECT IDType
	,LastValue
	,DataTable
FROM $(ExcelsiorDB)..UNIQUEID


SET NOCOUNT OFF