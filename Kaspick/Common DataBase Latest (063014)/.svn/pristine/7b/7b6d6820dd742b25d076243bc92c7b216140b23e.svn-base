
IF EXISTS (SELECT *
           FROM   sysobjects 
           WHERE  type = 'P'
                  AND name = 'USP_EX_GetEnfAllocationDetails')
    BEGIN
        DROP PROCEDURE USP_EX_GetEnfAllocationDetails;
        PRINT 'DROPPED USP_EX_GetEnfAllocationDetails';
    END
GO
    
/******************************************************************************                      
** New Name:	 USP_EX_GetEnfAllocationDetails
** Old Name:     USP_EIX_EnfAllocationDetailsSelProc                      
** Short Desc: To retrieve the Investment Allocation Management details             
**                      
** Full Description: To retrieve the Investment Allocation Management details             
**                              
** Input Arguments: NONE    
**         
** Sample Call                      
**  EXEC USP_EX_GetEnfAllocationDetails    
   @ObjectiveCode = 'Test_Tanuj'    
       
**             
** Return values: NONE    
**                      
**                      
** Standard declarations                      
**       SET LOCK_TIMEOUT         30000   -- 30 seconds                      
**                       
** Created By: Saravanan P Muthu               
** Company   : Kaspick & Company                      
** Project   : Excelsior  - Enfuego 4                      
** Created DT: 10/12/2009                      
**                                  
*******************************************************************************                
**       Change History                      
*******************************************************************************                
** Date:        Author:  Bug #     Description:                           Rvwd                
** --------  -------- ------    -------------------------------------- --------                
** <MM/DD/YYYY>  
**03/22/2014  Sanath Requirement INVERQ 3.1
  
*******************************************************************************                      
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved                      
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION                      
*******************************************************************************/  
                    
CREATE PROCEDURE [dbo].[USP_EX_GetEnfAllocationDetails]                     
(    
 @ObjectiveCode  varchar(100)    
)    
AS                
BEGIN    
--  Variable Declarations  --          
Declare @procname    varchar(60);          
Declare @ErrorMessage     varchar(1000);          
Declare @ErrorNumber     int;          
          
-- Variables used for error handling - uncomment if needed          
Declare @val1      varchar(30);          
Declare @val2      varchar(30);          
          
--  Variable Data Assignment  --          
Set @procname = 'USP_EX_GetEnfAllocationDetails';          
          
-- Body of procedure  --          
BEGIN TRY          
    
 DECLARE @DecisionComments varchar(max),    
   @DecisionDate datetime,    
   @CommentUser int    
    
 --Get Decision Comments for the selected Objective Code    
 SET @DecisionComments = ''    
 SELECT @DecisionComments = @DecisionComments + '('    
        + left(DATENAME(dw, DecsnCmnt.DecisionDate),3) + ' '     
        + left(DATENAME(mm, DecsnCmnt.DecisionDate),3) + ' '     
        + CAST(DATEPART(dd, DecsnCmnt.DecisionDate) AS VARCHAR(2)) + ' '     
        + CAST(DATEPART(hh, DecsnCmnt.DecisionDate) AS VARCHAR(2)) + ':'     
        + CAST(DATEPART(mi, DecsnCmnt.DecisionDate) AS VARCHAR(2)) + ' '     
        + CAST(DATEPART(year, DecsnCmnt.DecisionDate) AS VARCHAR(4)) + ', ' + KsUsr.LoginName + ') '     
        + DecsnCmnt.Comment + CHAR(13) + CHAR(10)     
 from TBL_INV_DecisionComment DecsnCmnt    
   inner join TBL_INV_DecisionCommentTypeLink DecsnCmntTypLnk ON DecsnCmnt.DecisionCommentID=DecsnCmntTypLnk.DecisionCommentID    
   INNER JOIN TBL_INV_DecisionCommentType DecsnCmntTyp ON DecsnCmntTyp.TypeID=DecsnCmntTypLnk.TypeID AND DecsnCmntTyp.TypeName='Objective'    
   INNER JOIN TBL_KS_User KsUsr ON KsUsr.UserID = DecsnCmnt.CommentUser     
 WHERE DecsnCmntTypLnk.decisionTypeValue= @ObjectiveCode    
 ORDER BY DecsnCmnt.DecisionCommentID DESC    
   
---- Enfuego4 - Strategic Allocation - Cascading Enhancement ----  
DECLARE @StrategicAllocationID BIGINT  
DECLARE @TA_MABenchMarkID BIGINT  
DECLARE @TA_TradeBenchMarkID BIGINT  
DECLARE @TA_BeneCommentID BIGINT  
DECLARE @SA_MABenchMarkID BIGINT  
DECLARE @SA_TradeBenchMarkID BIGINT  
DECLARE @SA_BeneCommentID BIGINT  
DECLARE @Is_MABenchMarkID_Inherited BIT  
DECLARE @Is_TradeBenchMark_Inherited BIT  
DECLARE @Is_BeneComment_Inherited BIT  
  
SET @Is_MABenchMarkID_Inherited = 0  
SET @Is_TradeBenchMark_Inherited = 0  
SET @Is_BeneComment_Inherited = 0  
  
SELECT @StrategicAllocationID = StrategicAllocationID FROM TBL_INV_InvestmentObjective WHERE ObjectiveCode = @ObjectiveCode  
  
SELECT @TA_MABenchMarkID = BnchMrk.BenchMarkID  
FROM TBL_INV_BenchMark BnchMrk   
INNER JOIN TBL_INV_InvestmentObjective InvstObjct ON InvstObjct.MABenchmarkID=HistoricBenchMarkID  
where BnchMrk.BenchMarkTypeID=1 and InvstObjct.ObjectiveCode = @ObjectiveCode  
  
SELECT @TA_TradeBenchMarkID  = BnchMrk.BenchmarkID  
FROM TBL_INV_BenchMark BnchMrk   
INNER JOIN TBL_INV_InvestmentObjective InvstObjct ON InvstObjct.TradBenchmarkID=HistoricBenchMarkID  
where BnchMrk.BenchMarkTypeID=2 and InvstObjct.ObjectiveCode = @ObjectiveCode  
  
SELECT @TA_BeneCommentID =BRCommentID FROM TBL_INV_InvestmentObjective WHERE ObjectiveCode = @ObjectiveCode  
  
  
  
SET @SA_MABenchMarkID = (select StgcAlcBnckMrk.BenchMarkID from TBL_INV_StrategicAllocationBenchMark StgcAlcBnckMrk   
         inner join TBL_INV_BenchMark BnchMrk on BnchMrk.BenchMarkID = StgcAlcBnckMrk.BenchMarkID   
         inner join TBL_INV_BenchMarkType BnchMrkTyp on BnchMrk.BenchMarkTypeID=BnchMrkTyp.BenchMarkTypeID  
         where BnchMrkTyp.BenchMarkTypeName='MultiAsset' and StgcAlcBnckMrk.StrategicAllocationID = @StrategicAllocationID)  
  
SET @SA_TradeBenchMarkID = (select StgcAlcBnckMrk.BenchMarkID from TBL_INV_StrategicAllocationBenchMark StgcAlcBnckMrk   
       inner join TBL_INV_BenchMark BnchMrk on BnchMrk.BenchMarkID = StgcAlcBnckMrk.BenchMarkID   
       inner join TBL_INV_BenchMarkType BnchMrkTyp on BnchMrk.BenchMarkTypeID=BnchMrkTyp.BenchMarkTypeID  
       where BnchMrkTyp.BenchMarkTypeName='Traditional' and StgcAlcBnckMrk.StrategicAllocationID = @StrategicAllocationID)  
  
SELECT @SA_BeneCommentID = BRCommentID FROM TBL_INV_StrategicAllocation WHERE StrategicAllocationID = @StrategicAllocationID  
  
 IF(@TA_MABenchMarkID = @SA_MABenchMarkID)  
 BEGIN  
  SET @Is_MABenchMarkID_Inherited = 1  
 END  
  
 IF(@TA_TradeBenchMarkID = @SA_TradeBenchMarkID)  
 BEGIN  
  SET @Is_TradeBenchMark_Inherited = 1  
 END  
  
 IF(@TA_BeneCommentID = @SA_BeneCommentID)  
 BEGIN  
  SET @Is_BeneComment_Inherited = 1  
 END  
-----------------   
     
 --Get Master Objective details for the selected objective_code    
 SELECT InvstObjct.ObjectiveCode    
 ,InvstObjct.ObjectiveName    
 ,InvstObjct.ObjectiveDescription    
 ,InvstObjct.ExpectedYield    
 ,InvstObjct.ExpectedAppreciation    
 ,InvstObjct.InvestmentNumber    
 ,MasterObjectiveName    
 ,@TA_MABenchMarkID as MABenchmarkID  
 ,@TA_TradeBenchMarkID as TradBenchmarkID  
 ,@DecisionComments as DecisionComments  
 ,InvstObjct.StrategicAllocationID  
 ,InvstObjct.BRCommentID  
 ,@Is_MABenchMarkID_Inherited  
 ,@Is_TradeBenchMark_Inherited  
 ,@Is_BeneComment_Inherited     
 ,isnull(StgcAlc.StrategicAllocationCode,'') as StrategicAllocationCode  
 FROM  TBL_INV_InvestmentObjective   InvstObjct  
LEFT OUTER JOIN TBL_INV_StrategicAllocation  StgcAlc on InvstObjct.StrategicAllocationID =StgcAlc.StrategicAllocationID   
 WHERE ObjectiveCode = @ObjectiveCode    
  
    
 --Target Allocation    
 SELECT ObjectiveCode    
   ,AssetClassName    
   ,CashBalanceCode    
   ,ClassRank    
   ,TargetPercent    
   ,DefaultAssetClass    
 FROM TBL_INV_TargetAllocation TrgtAlcn    
 WHERE TrgtAlcn.objectivecode = @ObjectiveCode     
   AND TrgtAlcn.cashbalancecode = 'High'    
 ORDER BY TrgtAlcn.ClassRank, TrgtAlcn.AssetClassName    
    
 --Target Fund Allocation    
 SELECT TrgtFndAlcn.ObjectiveCode    
   ,TrgtFndAlcn.SecuritySymbol    
   ,TrgtFndAlcn.CashBalanceCode    
   ,TrgtFndAlcn.FundWeight    
   ,TrgtFndAlcn.FundRank    
   ,TrgtFndAlcn.TargetFundPercent    
   ,TrAset.AssetClassName    
 FROM TBL_INV_TargetFundAllocation TrgtFndAlcn     
   INNER JOIN TBL_TR_Asset TrAset on TrgtFndAlcn.SecuritySymbol=TrAset.SecuritySymbol    
 WHERE TrgtFndAlcn.objectivecode = @ObjectiveCode     
   AND TrgtFndAlcn.cashbalancecode = 'High'    
 ORDER BY TrAset.AssetClassName, TrgtFndAlcn.SecuritySymbol, TrgtFndAlcn.FundRank    
  
  
 --Get Cascading Detail    
 SELECT InvstObjct.ObjectiveCode,  
  Is_PortfolioCode = (case when InvstObjct.BRCommentID = (select CommentID from TBL_BR_CommentLinkage where CustomerAccountNumber=AcntPrfl.CustomerAccountNumber 
       and ComLinkTypeID =(select ComLinkTypeID  From TBL_BR_CommentLinkType Where comdescription = 'BeneComment')) then 'True' else 'False' end),  
  AcntPrfl.CustomerAccountNumber,  
  InvstObjct.IsActive as Is_AssociatedToTactical  
 FROM  TBL_INV_InvestmentObjective InvstObjct   
    inner join TBL_INV_AccountProfile AcntPrfl on InvstObjct.ObjectiveCode = AcntPrfl.ObjectiveCode         
 WHERE InvstObjct.ObjectiveCode = @ObjectiveCode   
    
    
END TRY          
BEGIN CATCH          
 Set @ErrorMessage = ERROR_MESSAGE();          
 Set @ErrorNumber = ERROR_NUMBER();          
 Set @val1 = '';          
 Set @val2 = '';          
       
 exec USP_EX_SYSErrorHandler @CodeName = @ProcName,          
 @ErrorMessage = @ErrorMessage,           
 @ErrorNumber = @ErrorNumber,          
 @val1 = '',           
 @val1str = 'USP_EX_GetEnfAllocationDetails: Cannot Select.',           
 @val2 = '',           
 @val2str = '';          
END CATCH          
    
END 

GO

IF EXISTS (SELECT *
           FROM   sysobjects 
           WHERE  type = 'P'
                  AND name = 'USP_EX_GetEnfAllocationDetails')
    BEGIN
       
        PRINT 'CREATED USP_EX_GetEnfAllocationDetails';
    END
