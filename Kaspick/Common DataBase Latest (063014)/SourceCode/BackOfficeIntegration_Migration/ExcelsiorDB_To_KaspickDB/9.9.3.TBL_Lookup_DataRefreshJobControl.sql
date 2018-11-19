IF NOT EXISTS (SELECT JOBNAME FROM TBL_Lookup_DataRefreshJobControl)
BEGIN
	INSERT INTO TBL_Lookup_DataRefreshJobControl(JobName,JobStatus,StartTime,EndTime,ParentJobRunning,CutoffStartTime,CutoffEndTime)
	SELECT 
			'Lookup_Table_DataRefresh'
			,'Ideal'
			,null
			,null
			,0
			,'1900-01-01 17:00:00.000'
			,'1900-01-01 18:00:00.000'
		
END		