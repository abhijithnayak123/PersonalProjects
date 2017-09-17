-- ============================================================
-- Author:        <Suresh Vedagiri>
-- Create date:   <05/05/2014>
-- Description:   <Added a New Column in 'tCustomer' 
--                      for ODS> 
-- Rally ID:      <US1819>
-- ============================================================

IF NOT EXISTS 
(
  SELECT 
      1 
  FROM   
      sys.columns 
  WHERE  
      object_id = OBJECT_ID(N'[dbo].[tCustomer]') 
    AND name = 'Group1'
)
BEGIN         
      ALTER TABLE tCustomer 
      ADD Group1 NVARCHAR(255)  NULL
END
GO

IF NOT EXISTS 
(
  SELECT 
      1 
  FROM   
      sys.columns 
  WHERE  
      object_id = OBJECT_ID(N'[dbo].[tCustomer]') 
    AND name = 'Group2'
)
BEGIN         
      ALTER TABLE tCustomer 
      ADD Group2 NVARCHAR(255) NULL  
END
GO