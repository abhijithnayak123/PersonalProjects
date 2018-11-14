--- ================================================================================
-- Author:		<Divya Boddu>
-- Create date: <05/24/2016>
-- Description:	<Implement Changes to Optimize the image size in PTNR DB Size.>
--              <Adding 2 more columns(FrontImagePath and BackImagePath) to tCheckImages table>
-- Jira ID:		<AL-6291>
-- ================================================================================
IF NOT EXISTS 
(
  SELECT 
      * 
  FROM   
      sys.columns 
  WHERE  
      object_id = OBJECT_ID(N'[dbo].[tCheckImages]') 
    AND name IN('FrontImagePath',  'BackImagePath')
)
BEGIN         
      ALTER TABLE tCheckImages
      ADD FrontImagePath varchar(1000) null,
	      BackImagePath  varchar(1000) null ;
END
GO