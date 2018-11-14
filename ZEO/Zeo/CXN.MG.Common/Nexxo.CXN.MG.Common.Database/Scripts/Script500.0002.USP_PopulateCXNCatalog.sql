--===========================================================================================
-- Modified By: Mohammed Firoz
-- Created date: <Apr 21st 2015>
-- Description:	<Script to alter stored procedure USP_PopulateCXNCatalog>           
-- Jira ID:	<AL-244>
--===========================================================================================

/****** Object:  StoredProcedure [dbo].[USP_PopulateCXNCatalog]    Script Date: 4/7/2015 3:18:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[USP_PopulateCXNCatalog]		
		@billers xml,	
		@ChannelPartnerId int,	
		@result int out
	AS
	BEGIN
		BEGIN TRANSACTION;
	  
		SET NOCOUNT ON;
		
		IF EXISTS (select * from sys.objects where name = '#TempBillers' and type = 'u')
		drop table #TempBillers	
		
		SELECT
		[Table].[Column].value('Name[1]', 'nvarchar(max)') as 'CompanyName',
		[Table].[Column].value('ISOCountryCode[1]', 'nvarchar(max)') as 'ISOCountryCode',
		[Table].[Column].value('Country[1]', 'nvarchar(max)') as 'Country',
		[Table].[Column].value('CurrencyCode[1]', 'nvarchar(50)') as 'CurrencyCode'
		into #TempBillers
		FROM @billers.nodes('/ Billers/Biller') as [Table]([Column])

		----- Update all the billers in tWUnion_Catalog to false
		UPDATE tWUnion_Catalog  set IsActive = 0 
		
		----- Update only the existing billers to true 
		UPDATE tWUnion_Catalog
			  SET DTLastMod = GETDATE() , IsActive = 1 
			  FROM tWUnion_Catalog CTL  
			  INNER JOIN #TempBillers Temp ON 
			  Temp.CompanyName = CTL.CompanyName 
			
		--- insert new billers to the table. 
		INSERT INTO tWUnion_Catalog (WUCatalogPK,CompanyName,ISOCountryCode,Country, CurrencyCode,IsActive,ChannelPartnerId,DTCreate)
				 SELECT Distinct NEWID() AS WUCatalogPK, A.CompanyName, A.ISOCountryCode,A.Country,A.CurrencyCode, 
				 1 AS ISACTIVE,@ChannelPartnerId as ChannelPartnerId,  GETDATE() AS DTCREATE
				 FROM 
				 (SELECT DISTINCT Temp.CompanyName, Temp.ISOCountryCode,Temp.Country,Temp.CurrencyCode 
				   from #TempBillers Temp 
				   WHERE NOT EXISTS (SELECT CompanyName FROM tWUnion_Catalog  CTL WHERE Temp.CompanyName = CTL.CompanyName)				   
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



