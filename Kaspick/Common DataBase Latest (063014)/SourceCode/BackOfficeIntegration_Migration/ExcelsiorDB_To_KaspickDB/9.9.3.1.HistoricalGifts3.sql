

SET IDENTITY_INSERT dbo.TBL_HG_GiftHistory ON
GO

INSERT INTO TBL_HG_GiftHistory (
			GiftAssetID,
			ManagerCode,
			CustomerAccountNumber,
			GiftDate,
			SecurityType,
			Shares_Original_Face,
			FUNDING_TYPE_ID,
			MarketValue,
			CostBasis,
			Description,
			AcquireDate,
			ISIN,
			TransactionNumber,
			Property_State_Location_Id,
			Is_RentRoyaltyIncomeGenerating,
			RegistrationCode,
			LocationCode,
			EffectiveDate,
			TaxYear,
			AssetTransactionSource,
			Created_Date,
			Created_User_Id,
			Modified_Date,
			Modified_User_Id,
			GiftID,
			Comments,
			IsInitialGift,
			IsTestamentary,
			Final_Funding_Date,
			Reserve_Amount,
			ClosingCost,
			DeliverMethod,
			DeliverDate,
			Zero_Cost_Per_Client_ID,
			Long_Term_Per_Client_ID,
			REAL_PROPERTY_ASSET_SALE_DATE,
			PRICE_SOURCE,
			HIGH_PRICE,
			Low_Price,
			Average_Price,
			INVESTMENT_TRADING_STRATEGY_ID,
			AssetType
			) 
select 		GFTAST.GiftAssetID
			,lookupdb.ManagerCode
			,lookupdb.CustomerAccountNumber
			,GFT.GiftDate
			,NULL AS SecurityType
			,NULL AS Shares_Original_Face
			,GFTSUP. FUNDING_TYPE_ID
			,GFTAST.MarketValue
			,GFTAST.CostBasis
			,GFTAST.Description
			,GFTAST.AcquireDate
			,NULL AS ISIN
			,NULL AS TransactionNumber
			,GFTASTSUP.Property_State_Location_Id
			,GFTASTSUP.Is_RentRoyaltyIncomeGenerating
			,NULL AS RegistrationCode
			,NULL AS LocationCode
			,NULL AS EffectiveDate
			,NULL AS TaxYear
			,'Historical Excelsior Data' AS AssetTransactionSource
			,GFTSUP.Created_Date
			,GFTSUP.Created_User_Id
			,GFTSUP.Modified_Date
			,GFTSUP.Modified_User_Id
			,GFT.GiftID
			,GFT.Comments
			,GFT.IsInitialGift
			,GFT.IsTestamentary
			,GFTSUP.Final_Funding_Date
			,GFTSUP.Reserve_Amount
			,GFTAST.ClosingCosts
			,GFTAST.DeliverMethod
			,GFTAST.DeliverDate
			,GFTASTSUP.Zero_Cost_Per_Client_ID
			,GFTASTSUP.Long_Term_Per_Client_ID
			,GFTASTSUP.REAL_PROPERTY_ASSET_SALE_DATE
			,GFTASTSUP.PRICE_SOURCE
			,GFTASTSUP.HIGH_PRICE
			,GFTASTSUP.Low_Price
			,GFTASTSUP.Average_Price
			,GFTASTSUP.INVESTMENT_TRADING_STRATEGY_ID
			,GFTAST.AssetType AS AssetType 
			FROM $(ExcelsiorDB)..Gift AS GFT 
			INNER JOIN $(ExcelsiorDB)..GiftAsset AS GFTAST ON GFT.GiftID=GFTAST.GiftID
			INNER JOIN $(ExcelsiorDB)..TBL_EIS_EX_GIFT_SUPPLEMENT AS GFTSUP ON GFTSUP.GiftID= GFT.GiftID
			INNER JOIN $(ExcelsiorDB)..TBL_EIS_EX_GIFT_ASSET_SUPPLEMENT AS GFTASTSUP ON GFTASTSUP.GiftAssetID= GFTAST.GiftAssetID
			INNER JOIN $(MappingDB)..TBL_AccountLookup  AS lookupdb ON lookupdb.accountid=GFT.accountid
  GO
  
  SET IDENTITY_INSERT dbo.TBL_HG_GiftHistory OFF
  GO
  
  
  
  