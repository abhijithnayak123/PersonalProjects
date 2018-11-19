IF EXISTS (SELECT *
           FROM   sysobjects 
           WHERE  type = 'P'
                  AND name = 'USP_EX_GetInvestmentComment')
    BEGIN
        DROP PROCEDURE USP_EX_GetInvestmentComment;
        PRINT 'DROPPED USP_EX_GetInvestmentComment';
    END
GO
    
   
/******************************************************************************          
** Name : USP_EX_GetInvestmentComment  
** old Name   :   USP_EIS_EX_INVESTMENT_COMMENT_SelProc    
** Short Desc : Gets a list of Investment Comments for a given Advent Id     
**          
** Full Description          
**                  
**          
** Sample Call          
        EXEC USP_EX_GetInvestmentComment 'ADVENTID'    
   -- parameters          
**          
** Return values: NONE          
**          
**          
** Standard declarations          
**       SET LOCK_TIMEOUT         30000   -- 30 seconds          
**           
** Created By :  Prabin Varm     
** Company  :  Kaspick & Company          
** Project  :  Enfuego3B    
** Created DT :  Oct/13/2010          
**                      
*******************************************************************************          
**       Change History          
*******************************************************************************          
** Date:        Author:  Bug #     Description:                           Rvwd          
** --------     -------- ------    -------------------------------------- --------          
** 3-Apr-2014   Yugandhar  EXCREQ7.4 Modified   
  
** 22-MAY-2014 Mallikarjun EXCREQ 7.4    SP Name Renamed and Formatted  
******************************************************************************          
** Copyright (C) 2007 Kaspick & Company, All Rights Reserved          
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION          
*******************************************************************************/     
Create Procedure USP_EX_GetInvestmentComment    
(    
 @AdventId char(14)    
)    
As    
Begin    
 --  Initial Set statements  --        
 SET NOCOUNT ON;        
 SET LOCK_TIMEOUT                30000;   -- 30 seconds        
 --SET TRANSACTION ISOLATION LEVEL SNAPSHOT;    
    
 Select     
  InvstCmnt.[InvestmentCommentID],    
  InvstCmnt.CustomerAccountNumber,    
  InvstCmnt.[InvestmentComment],    
  InvstCmnt.CreatedBy,    
  InvstCmnt.ModifiedBy,    
  InvstCmnt.CreatedDate,    
  InvstCmnt.ModifiedDate,    
  KsUsr.LOGINNAME AS 'CreatedUserName',    
  KsUsrMod.LOGINNAME AS 'ModifiedUserName'    
 From TBL_INV_InvestmentComment InvstCmnt    
 INNER JOIN TBL_KS_USER KsUsr ON InvstCmnt.CreatedBy = KsUsr.[USERID]    
 LEFT OUTER JOIN TBL_KS_USER KsUsrMod ON InvstCmnt.ModifiedBy = KsUsrMod.[USERID]    
 Where CustomerAccountNumber = @AdventId    
 ORDER BY InvstCmnt.CreatedDate DESC    
End    
    
    
    GO
  IF EXISTS (	SELECT *
			FROM sysobjects
			WHERE type = 'P'
			AND name = 'USP_EX_GetInvestmentComment') 
	BEGIN
			PRINT 'CREATED PROCEDURE USP_EX_GetInvestmentComment';
	END  
    