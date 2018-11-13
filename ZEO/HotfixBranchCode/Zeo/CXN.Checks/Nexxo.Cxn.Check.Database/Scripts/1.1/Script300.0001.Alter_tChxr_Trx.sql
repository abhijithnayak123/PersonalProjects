--===========================================================================================
-- Auther:			SwarnaLakshmi
-- Date Created:	23rd june 2014
-- Description:		Script for alter tchxr_trx
--===========================================================================================
IF NOT EXISTS 
(
  SELECT 
      1 
  FROM   
      sys.columns 
  WHERE  
      object_id = OBJECT_ID(N'[dbo].[tChxr_Trx]') 
    AND name = 'IsCheckFranked'
)
BEGIN         
      ALTER TABLE tChxr_Trx 
      ADD IsCheckFranked BIT NOT NULL Default 0;
END
GO

IF NOT EXISTS 
(
  SELECT 
      1 
  FROM   
      sys.columns 
  WHERE  
      object_id = OBJECT_ID(N'[dbo].[tChxr_Trx_Aud]') 
    AND name = 'IsCheckFranked'
)
BEGIN         
      ALTER TABLE tChxr_Trx_Aud 
      ADD IsCheckFranked BIT NOT NULL Default 0;
END
GO
