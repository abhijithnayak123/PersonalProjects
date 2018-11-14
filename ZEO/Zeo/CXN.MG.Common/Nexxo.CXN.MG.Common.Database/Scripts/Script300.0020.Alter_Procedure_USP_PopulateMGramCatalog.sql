--===========================================================================================
-- Auther:			Rahul K
-- Date Created:	8/10/2014
-- Description:		Script for Creating PopulateMGramCatalog 
--===========================================================================================


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_PopulateMGramCatalog]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_PopulateMGramCatalog]
GO

CREATE PROCEDURE [dbo].[USP_PopulateMGramCatalog] (
@billers xml,	
@ChannelPartnerId int,	
@result int out
)
AS
BEGIN
BEGIN TRANSACTION;

IF EXISTS (select * from sys.objects where name = '#TempBillers' and type = 'u')
drop table #TempBillers	
SELECT
		[Table].[Column].value('ReceiveAgentId[1]', 'nvarchar(100)') as 'ReceiveAgentId',
		[Table].[Column].value('ReceiveCode[1]', 'nvarchar(100)') as 'ReceiveCode',
		[Table].[Column].value('BillerName[1]', 'nvarchar(100)') as 'BillerName',
		[Table].[Column].value('PoeSvcMsgENText[1]', 'nvarchar(max)') as 'Poe_Svc_Msg_EN_Text',
		[Table].[Column].value('PoeSvcMsgESText[1]', 'nvarchar(max)') as 'Poe_Svc_Msg_ES_Text',
		[Table].[Column].value('Keywords[1]', 'nvarchar(max)') as 'Keywords'
		into #TempBillers
		FROM @billers.nodes('/ Billers/Biller') as [Table]([Column])
		
		----- Update all the billers in tMgram_Catalog to false
		UPDATE tMGram_Catalog  set IsActive = 0 
		
		----- Update only the existing billers to true 
		UPDATE tMGram_Catalog
			  SET DTLastMod = GETDATE() , IsActive = 1 
			  FROM tMGram_Catalog CTL  
			  INNER JOIN #TempBillers Temp ON 
			  Temp.ReceiveCode = CTL.ReceiveCode 
			--- insert new billers to the table. 
		INSERT INTO tMGram_Catalog (ROWGUID,ReceiveAgentId,ReceiveCode,BillerName, Poe_Svc_Msg_EN_Text,Poe_Svc_Msg_ES_Text,Keywords,IsActive,ChannelPartnerId,DTCreate)
				 SELECT Distinct NEWID() AS ROWGUID, A.ReceiveAgentId, A.ReceiveCode,A.BillerName,A.Poe_Svc_Msg_EN_Text,A.Poe_Svc_Msg_ES_Text, A.Keywords,
				 1 AS ISACTIVE,@ChannelPartnerId as ChannelPartnerId,  GETDATE() AS DTCREATE
				 FROM 
				 (SELECT DISTINCT Temp.ReceiveAgentId, Temp.ReceiveCode,Temp.BillerName,Temp.Poe_Svc_Msg_EN_Text, Temp.Poe_Svc_Msg_ES_Text,Temp.Keywords 
				   from #TempBillers Temp 
				   WHERE NOT EXISTS (SELECT ReceiveCode FROM tMGram_Catalog  CTL WHERE Temp.ReceiveCode = CTL.ReceiveCode)				   
				 )A
				 IF @@ERROR > 0
		Begin
			ROLLBACK TRANSACTION;
			set @result = 0
		End 
		ELSE 
		Begin 
		 If @@TRANCOUNT > 0
			BEGIN
			COMMIT TRANSACTION;
			set @result = 1
			END
		END 
				  				
		
END
GO
