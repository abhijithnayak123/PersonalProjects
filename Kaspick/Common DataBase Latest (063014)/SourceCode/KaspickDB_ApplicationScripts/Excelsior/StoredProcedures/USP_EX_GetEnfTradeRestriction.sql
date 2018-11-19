IF EXISTS (SELECT *
           FROM   sysobjects 
           WHERE  type = 'P'
                  AND name = 'USP_EX_GetEnfTradeRestriction')
    BEGIN
        DROP PROCEDURE USP_EX_GetEnfTradeRestriction;
        PRINT 'DROPPED USP_EX_GetEnfTradeRestriction';
    END
GO  
  
/******************************************************************************                            
** New Name:  USP_EX_GetEnfTradeRestriction  
** Old Name:     USP_EIX_EnfTradeRestrictionSELPROC                            
** Short Desc: To retrieve the Trade Restriction Details                   
**                            
** Full Description: To retrieve the Trade Restriction Details                   
**                                    
** Input Arguments: NONE          
**               
** Sample Call                            
**  EXEC USP_EX_GetEnfTradeRestriction 1, 'EDIT'         
             
**                   
** Return values: NONE          
**                            
**                            
** Standard declarations                            
**       SET LOCK_TIMEOUT         30000   -- 30 seconds                            
**                             
** Created By: Soorya                     
** Company   : Kaspick & Company                            
** Project   : Excelsior  - Enfuego 3B                            
** Created DT: 10/27/2010                            
**                                        
*******************************************************************************                      
**       Change History                            
*******************************************************************************                      
** Date:        Author:   Bug #  Description:         Rvwd                      
** --------  -------- ------  --------------------------------------   --------                      
** 20-Jun-2011 RaviKiran CR#776  Removed -- WHERE SecuritySymbol LIKE '[a-z]%'            
** 8-Apr-2014  Abhijith  EXCREQ 7.4       
** 22-MAY-2014 Mallikarjun EXCREQ 7.4    SP Name Renamed and Formatted        
*******************************************************************************                            
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved                            
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION                            
*******************************************************************************/                            
CREATE PROCEDURE [dbo].[USP_EX_GetEnfTradeRestriction]    
(    
 @TradeRestrictionID BIGINT,    
 @Mode VARCHAR(10)    
)                         
AS                      
BEGIN          
--  Variable Declarations  --                
DECLARE @procname    VARCHAR(60);                
DECLARE @ErrorMessage     VARCHAR(1000);                
DECLARE @ErrorNumber     INT;                
                
-- Variables used for error handling - uncomment if needed                
DECLARE @val1      VARCHAR(30);                
DECLARE @val2      VARCHAR(30);                
                
--  Variable Data Assignment  --                
SET @procname = 'USP_EX_GetEnfTradeRestriction';                
                
-- Body of procedure  --                
BEGIN TRY     
    
 SELECT RestrictionTypeID, RestrictionType FROM TBL_TR_TradeRestrictionTypeMaster    
    
  SELECT DISTINCT LTRIM(RTRIM(replace(SecuritySymbol,char(9),''))) AS SecuritySymbol FROM TBL_TR_Asset             
  ORDER BY SecuritySymbol ASC       
               
 SELECT RestrictionType, SecuritySymbol, RestrictionStartDate, RestrictionEndDate, RestrictionComment    
 FROM TBL_TR_TradeRestriction TradRestn     
 INNER JOIN TBL_TR_TradeRestrictionTypeMaster TradRestnTypMstr ON TradRestn.TradeRestrictionTypeID = TradRestnTypMstr.RestrictionTypeID    
 WHERE TradeRestrictionID = @TradeRestrictionID    
            
END TRY                
BEGIN CATCH                
    SET @ErrorMessage = ERROR_MESSAGE();                
 SET @ErrorNumber = ERROR_NUMBER();                
 SET @val1 = '';                
 SET @val2 = '';                
             
 EXEC dbo.USP_EX_SYSErrorHandler @codename = @procname,                
 @ErrorMessage = @ErrorMessage,                 
 @ErrorNumber = @ErrorNumber,                
 @val1 = '',                 
 @val1str = 'USP_EX_GetEnfTradeRestriction: Cannot Select.',                
 @val2 = '',                 
 @val2str = '';          
END CATCH                
-- End of procedure  --                
          
END     

GO
  IF EXISTS (	SELECT *
			FROM sysobjects
			WHERE type = 'P'
			AND name = 'USP_EX_GetEnfTradeRestriction') 
	BEGIN
			PRINT 'CREATED PROCEDURE USP_EX_GetEnfTradeRestriction';
	END  