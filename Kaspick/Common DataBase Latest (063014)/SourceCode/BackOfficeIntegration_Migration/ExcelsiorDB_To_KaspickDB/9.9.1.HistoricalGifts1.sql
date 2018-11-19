IF NOT EXISTS
      (
            SELECT * 
            FROM TBL_KS_Application 
            WHERE ApplicationName='Historical Gifts'
      )
      BEGIN

            DECLARE @DISPLAY_SEQUENCE_APP INT
            
            SELECT @DISPLAY_SEQUENCE_APP = MAX(DisplaySequence) 
            FROM TBL_KS_Application
            
            INSERT INTO TBL_KS_Application(ApplicationName, Abbrev, Description, DisplaySequence, IsActive)
            VALUES('Historical Gifts','HG','HG',@DISPLAY_SEQUENCE_APP + 10,1)

      END
      GO

--Adding 'RunReports' under Application 'Report Runner'
      IF NOT EXISTS 
      (
            SELECT * 
            FROM TBL_KS_Module 
            WHERE ModuleName = 'Historical Gifts'
      )
      BEGIN
            
            DECLARE @APPLICATION_ID INT
            DECLARE @DISPLAY_SEQUENCE INT
            
            SELECT @APPLICATION_ID = ApplicationID 
            FROM TBL_KS_Application 
            WHERE ApplicationName='Historical Gifts'
            
            SELECT @DISPLAY_SEQUENCE = MAX(DisplaySequence) from TBL_KS_MainModule

            INSERT INTO TBL_KS_Module(ApplicationID, ParentID, ModuleName, Abbrev, Description, DisplaySequence, IsActive, MainModuleID, ModuleURL, ImageURL)
            VALUES(@APPLICATION_ID,Null,'Historical Gifts','Gifts','HGModule',@DISPLAY_SEQUENCE + 10,1,4,null,null)
            
      END
      GO
      