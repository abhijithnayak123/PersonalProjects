-- ============================================================
-- Author:		<Bineesh Raghavan>
-- Create date: <07/03/2014>
-- Description:	<SP to perform bulk insert of MG meta data> 
-- Rally ID:	<NA>
-- ============================================================
IF OBJECT_ID (N'usp_InsertMetadata', N'P') IS NOT NULL
    DROP PROCEDURE usp_InsertMetadata;
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
				rowguid, 
				Code, 
				Name, 
				LegacyCode, 
				SendActive, 
				ReceiveActive, 
				DirectedSendCountry, 
				MGDirectedSendCountry, 
				DTCreate
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
				rowguid, 
				Code, 
				Name, 
				CountryCode,
				DTCreate
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
				rowguid, 
				Code, 
				Name, 
				CurrencyPrecision,
				DTCreate
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
				rowguid, 
				CountryCode,
				BaseCurrency,
				LocalCurrency,
				ReceiveCurrency,
				IndicativeRateAvailable,
				DeliveryOption,
				DTCreate
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
				rowguid, 
				OptionId,
				DeliveryOption,
				Name,
				DTCreate
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
				rowguid, 
				DFJurisdiction,
				StateRegulatorURL,
				StateRegulatorPhone,
				LanguageCode,
				Translation,
				DTCreate
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
