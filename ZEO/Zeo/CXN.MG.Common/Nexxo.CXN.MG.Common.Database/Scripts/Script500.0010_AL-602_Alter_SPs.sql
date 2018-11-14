-- ================================================================================
-- Author:		<Ashok Kumar>
-- Create date: <07/01/2015>
-- Description:	<Alter SPs date columns to DTTerminalCreate, DTTerminalLastModified, DTServerCreate, DTServerLastModified. >
-- Jira ID:		<AL-602>
-- ================================================================================

-- usp_InsertMetadata
DROP PROCEDURE [dbo].[usp_InsertMetadata]
GO

CREATE PROCEDURE [dbo].[usp_InsertMetadata] 
(
	@countryTable dbo.CountryTableType READONLY,
	@stateTable dbo.StateTableType READONLY,
	@currencyTable dbo.CurrencyTableType READONLY,
	@countryCurrencyTable dbo.CountryCurrencyTableType READONLY,
	@deliveryOptionTable dbo.DeliveryOptionTableType READONLY,
	@stateRegulatorTable dbo.StateRegulatorTableType READONLY
)
AS
BEGIN
	SET NOCOUNT ON;	
	SET XACT_ABORT ON;
	
	BEGIN TRY
		BEGIN TRANSACTION
			--Swipes current data in meta data tables
			TRUNCATE TABLE tMGram_Countries
			TRUNCATE TABLE tMGram_States
			TRUNCATE TABLE tMGram_CountryCurrencies
			TRUNCATE TABLE tMGram_Currencies
			TRUNCATE TABLE tMGram_DeliveryOptions
			TRUNCATE TABLE tMGram_StateRegulators
			
			-- BULK INSERT to tMGram_Countries
			INSERT INTO tMGram_Countries
			(
				MGCountryPK, 
				Code, 
				Name, 
				LegacyCode, 
				SendActive, 
				ReceiveActive, 
				DirectedSendCountry, 
				MGDirectedSendCountry, 
				DTServerCreate
			)
			SELECT 
				NEWID(),
				CountryCode,
				CountryName, 
				CountryLegacyCode, 
				SendActive, 
				ReceiveActive, 
				DirectedSendCountry, 
				MGDirectedSendCountry,
				GETDATE()
			FROM
				@countryTable	
				
			-- BULK INSERT to tMGram_States
			INSERT INTO tMGram_States
			(
				MGStatePK, 
				Code, 
				Name, 
				CountryCode,
				DTServerCreate
			)
			SELECT 
				NEWID(),
				StateProvinceCode,
				StateProvinceName, 
				CountryCode,
				GETDATE()
			FROM
				@stateTable
				
			-- BULK INSERT to tMGram_Currencies
			INSERT INTO tMGram_Currencies
			(
				MGCurrencyPK, 
				Code, 
				Name, 
				CurrencyPrecision,
				DTServerCreate
			)
			SELECT 
				NEWID(),
				CurrencyCode,
				CurrencyName, 
				CurrencyPrecision,
				GETDATE()
			FROM
				@currencyTable
				
			-- BULK INSERT to tMGram_CountryCurrencies
			INSERT INTO tMGram_CountryCurrencies
			(
				MGCountryCurrencyPK, 
				CountryCode,
				BaseCurrency,
				LocalCurrency,
				ReceiveCurrency,
				IndicativeRateAvailable,
				DeliveryOption,
				DTServerCreate
			)
			SELECT 
				NEWID(),
				CountryCode,
				BaseCurrency,
				LocalCurrency,
				ReceiveCurrency,
				IndicativeRateAvailable,
				DeliveryOption,
				GETDATE()
			FROM
				@countryCurrencyTable
				
			-- BULK INSERT to tMGram_DeliveryOptions
			INSERT INTO tMGram_DeliveryOptions
			(
				MGDeliveryOptionPK, 
				OptionId,
				DeliveryOption,
				Name,
				DTServerCreate
			)
			SELECT 
				NEWID(),
				DeliveryOptionId,
				Delivery_Option,
				DeliveryOptionName,
				GETDATE()
			FROM
				@deliveryOptionTable						

			-- BULK INSERT to tMGram_StateRegulators
			INSERT INTO tMGram_StateRegulators
			(
				MGStateRegulatorPK, 
				DFJurisdiction,
				StateRegulatorURL,
				StateRegulatorPhone,
				LanguageCode,
				Translation,
				DTServerCreate
			)
			SELECT 
				NEWID(),
				DFJurisdiction,
				StateRegulatorURL,
				StateRegulatorPhone,
				LanguageCode,
				Translation,
				GETDATE()
			FROM
				@stateRegulatorTable
		COMMIT TRANSACTION;	
	END TRY
	
	BEGIN CATCH
		-- Execute error retrieval routine.
		EXECUTE usp_GetErrorInfo;
		IF ((XACT_STATE()) != 1)
		BEGIN
			ROLLBACK TRANSACTION;
		END	
	END CATCH	
END
GO

-- USP_PopulateCXNCatalog
DROP PROCEDURE [dbo].[USP_PopulateCXNCatalog]
GO

CREATE PROCEDURE [dbo].[USP_PopulateCXNCatalog]		
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
			  SET DTServerLastModified = GETDATE() , IsActive = 1 
			  FROM tWUnion_Catalog CTL  
			  INNER JOIN #TempBillers Temp ON 
			  Temp.CompanyName = CTL.CompanyName 
			
		--- insert new billers to the table. 
		INSERT INTO tWUnion_Catalog (WUCatalogPK,CompanyName,ISOCountryCode,Country, CurrencyCode,IsActive,ChannelPartnerId,DTServerCreate)
				 SELECT Distinct NEWID() AS WUCatalogPK, A.CompanyName, A.ISOCountryCode,A.Country,A.CurrencyCode, 
				 1 AS ISACTIVE,@ChannelPartnerId as ChannelPartnerId,  GETDATE() AS DTServerCreate
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

-- USP_PopulateMGramCatalog
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
			  SET DTServerLastModified = GETDATE() , IsActive = 1 
			  FROM tMGram_Catalog CTL  
			  INNER JOIN #TempBillers Temp ON 
			  Temp.ReceiveCode = CTL.ReceiveCode 
			--- insert new billers to the table. 
		INSERT INTO tMGram_Catalog (MGCatalogPK,ReceiveAgentId,ReceiveCode,BillerName, Poe_Svc_Msg_EN_Text,Poe_Svc_Msg_ES_Text,Keywords,IsActive,ChannelPartnerId,DTServerCreate)
				 SELECT Distinct NEWID() AS MGCatalogPK, A.ReceiveAgentId, A.ReceiveCode,A.BillerName,A.Poe_Svc_Msg_EN_Text,A.Poe_Svc_Msg_ES_Text, A.Keywords,
				 1 AS ISACTIVE,@ChannelPartnerId as ChannelPartnerId,  GETDATE() AS DTServerCREATE
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
