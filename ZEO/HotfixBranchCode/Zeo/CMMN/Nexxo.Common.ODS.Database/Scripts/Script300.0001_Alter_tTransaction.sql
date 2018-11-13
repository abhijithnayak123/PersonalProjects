-- ============================================================
-- Author:        <Suresh Vedagiri>
-- Create date:   <05/05/2014>
-- Description:   <Added a New Column in 'tTransaction' 
--                      for ODS> 
-- Rally ID:      <US1819 – TA4940>
-- ============================================================

IF NOT EXISTS 
(
  SELECT 
      1 
  FROM   
      sys.columns 
  WHERE  
      object_id = OBJECT_ID(N'[dbo].[tTransaction]') 
    AND name = 'BaseFee'
)
BEGIN         
      ALTER TABLE tTransaction 
      ADD BaseFee MONEY  NULL
END
GO

IF NOT EXISTS 
(
  SELECT 
      1 
  FROM   
      sys.columns 
  WHERE  
      object_id = OBJECT_ID(N'[dbo].[tTransaction]') 
    AND name = 'DiscountApplied'
)
BEGIN         
      ALTER TABLE tTransaction 
      ADD   DiscountApplied MONEY NULL  
END
GO

IF NOT EXISTS 
(
  SELECT 
      1 
  FROM   
      sys.columns 
  WHERE  
      object_id = OBJECT_ID(N'[dbo].[tTransaction]') 
    AND name = 'NetFee'
)
BEGIN         
      ALTER TABLE tTransaction 
      ADD   NetFee MONEY NULL
END
GO

IF NOT EXISTS 
(
  SELECT 
      1 
  FROM   
      sys.columns 
  WHERE  
      object_id = OBJECT_ID(N'[dbo].[tTransaction]') 
      AND name = 'Promotion'
)
BEGIN         
      ALTER TABLE tTransaction 
      ADD Promotion NVARCHAR(50) NULL    
END
GO

IF NOT EXISTS 
(
  SELECT 
      1 
  FROM   
      sys.columns 
  WHERE  
      object_id = OBJECT_ID(N'[dbo].[tTransaction]') 
      AND name = 'PromotionDescription'
)
BEGIN         
      ALTER TABLE tTransaction 
      ADD PromotionDescription NVARCHAR(100) NULL    
END
GO

IF NOT EXISTS 
(
  SELECT 
      1 
  FROM   
      sys.columns 
  WHERE  
      object_id = OBJECT_ID(N'[dbo].[tTransaction]') 
      AND name = 'OrginatingTransactionId'
)
BEGIN         
      ALTER TABLE tTransaction 
      ADD OrginatingTransactionId BIGINT    
END
GO

IF NOT EXISTS 
(
  SELECT 
      1 
  FROM   
      sys.columns 
  WHERE  
      object_id = OBJECT_ID(N'[dbo].[tTransaction]') 
      AND name = 'TransactionSubType'
)
BEGIN         
      ALTER TABLE tTransaction 
      ADD TransactionSubType VARCHAR(20) NULL    
END
GO
