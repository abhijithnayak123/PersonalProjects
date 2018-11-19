
/****** Object:  StoredProcedure [dbo].[USP_AV_GetValuationData]    Script Date: 06/12/2014 17:43:14 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_AV_GetValuationData]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_AV_GetValuationData]
GO

/****** Object:  StoredProcedure [dbo].[USP_AV_GetValuationData]    Script Date: 06/12/2014 17:43:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/******************************************************************************          
** Name   :   USP_AV_GetValuationData          
** Short Desc : Put in Short Description          
**          
** Full Description          
**                  
**          
** Sample Call          
        EXEC USP_AV_GetValuationData '<DataSetValuation>
  <DataTableValuation>
    <Ticker>Cash / Other</Ticker>
    <SecurityName>Cash / Other</SecurityName>
    <Shares>3,343.0000</Shares>
    <HighPrice />
    <LowPrice />
    <MeanPrice>1</MeanPrice>
    <Total>3,343.00</Total>
    <IsBond />
  </DataTableValuation>
</DataSetValuation>'      
   -- parameters          
**          
** Return values: NONE          
**          
**          
** Standard declarations          
**       SET LOCK_TIMEOUT         30000   -- 30 seconds          
**           
** Created By : Tanuj     
** Company  :  Kaspick & Company          
** Project  :  BOI - Katana          
** Created DT :  05/15/2014          
**                      
*******************************************************************************          
**       Change History          
*******************************************************************************          
** Date:        Author:  Bug #     Description:                           Rvwd          
** --------     -------- ------    -------------------------------------- --------          
** 05/15/2014   Tanuj             Created 
******************************************************************************          
** Copyright (C) 2007 Kaspick & Company, All Rights Reserved          
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION          
*******************************************************************************/
CREATE PROCEDURE [dbo].[USP_AV_GetValuationData]
  @XMLInput xml
AS
BEGIN
SELECT 
 x.item.value('Ticker[1]','varchar(50)') AS Ticker
,x.item.value('SecurityName[1]','varchar(50)') AS SecurityName
,x.item.value('Shares[1]','varchar(50)') AS Shares
,x.item.value('HighPrice[1]','varchar(50)') AS HighPrice
,x.item.value('LowPrice[1]','varchar(50)') AS LowPrice
,x.item.value('MeanPrice[1]','varchar(50)') AS MeanPrice
,x.item.value('Total[1]','varchar(50)') AS Total
,x.item.value('IsBond[1]','varchar(50)') AS IsBond
FROM @XMLInput.nodes('/DataSetValuation/DataTableValuation') x(item)
END
      
     

