using AutoMapper;
using MGI.Common.Util;
using MGI.CXN.MG.Common.AgentConnectService;
using MGI.CXN.MG.Common.Data;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using MGI.CXN.MG.Common.Contract;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.CXN.MG.Common.Impl
{
	public class IO : IMGCommonIO
	{
		public TLoggerCommon MongoDBLogger { private get; set; }
		#region Private Members

		private string _serviceURL = "https://extws.moneygram.com/extws/services/AgentConnect1305";

		#endregion

		public IO()
		{
			Mapper.CreateMap<BaseRequest, CodeTableRequest>();

			Mapper.CreateMap<CountryInfo, Country>();
			Mapper.CreateMap<StateProvinceInfo, StateProvince>();
			Mapper.CreateMap<CurrencyInfo, Currency>();
			Mapper.CreateMap<CountryCurrencyInfo, CountryCurrency>();
			Mapper.CreateMap<DeliveryOptionInfo, DeliveryOption>();

			Mapper.CreateMap<StateRegulatorRequest, DoddFrankStateRegulatorInfoRequest>();
			Mapper.CreateMap<stateRegulatorInfo, StateRegulator>()
				.ForMember(d => d.Translation, s => s.MapFrom(c => c.stateRegulatorName[0].textTranslation))
				.ForMember(d => d.LanguageCode, s => s.MapFrom(c => c.stateRegulatorName[0].longLanguageCode));

			Mapper.CreateMap<Data.TranslationsRequest, AgentConnectService.TranslationsRequest>();
			Mapper.CreateMap<Data.CountryTranslation, AgentConnectService.CountryTranslation>();
			Mapper.CreateMap<AgentConnectService.CountryTranslation, Data.CountryTranslation>();
			Mapper.CreateMap<AgentConnectService.DeliveryOptionTranslation, Data.DeliveryOptionTranslation>();
			Mapper.CreateMap<AgentConnectService.IndustryTranslation, Data.IndustryTranslation>();
			Mapper.CreateMap<AgentConnectService.CurrencyTranslation, Data.CurrencyTranslation>();
			Mapper.CreateMap<AgentConnectService.FQDOTextTranslation, Data.FQDOTextTranslation>();
		}

		public CTResponse GetMetaData(BaseRequest request, MGIContext mgiContext)
		{
			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<BaseRequest>(mgiContext.CustomerSessionId, request, "GetMetaData", AlloyLayerName.CXN,
				ModuleName.Transaction, "GetMetaData -MGI.Cxn.MG.Common.Impl.IO", "REQUEST", mgiContext);
			#endregion
			AgentConnectClient client = GetAgentClient();

			CodeTableRequest codeTableRequest = Mapper.Map<CodeTableRequest>(request);

			CodeTableResponse ctResponse = client.codeTable(codeTableRequest);

			CTResponse response = ConvertCodeTableResponse(ctResponse);

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<CTResponse>(mgiContext.CustomerSessionId, response, "GetMetaData", AlloyLayerName.CXN,
				ModuleName.Transaction, "GetMetaData -MGI.Cxn.MG.Common.Impl.IO", "RESPONSE", mgiContext);
			#endregion
			return response;
		}


		public StateRegulatorResponse GetDoddFrankStateRegulatorInfo(StateRegulatorRequest request, MGIContext mgiContext)
		{
			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<StateRegulatorRequest>(mgiContext.CustomerSessionId, request, "GetDoddFrankStateRegulatorInfo", AlloyLayerName.CXN,
				ModuleName.Transaction, "GetDoddFrankStateRegulatorInfo -MGI.Cxn.MG.Common.Impl.IO", "REQUEST", mgiContext);
			#endregion
			AgentConnectClient client = GetAgentClient();

			DoddFrankStateRegulatorInfoRequest doddFrankStateRegulatorInfoRequest = Mapper.Map<DoddFrankStateRegulatorInfoRequest>(request);

			DoddFrankStateRegulatorInfoResponse response = client.doddFrankStateRegulatorInfo(doddFrankStateRegulatorInfoRequest);

			StateRegulatorResponse srResponse = ConvertStateRegulatorResponse(response);

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<StateRegulatorResponse>(mgiContext.CustomerSessionId, srResponse, "GetDoddFrankStateRegulatorInfo", AlloyLayerName.CXN,
				ModuleName.Transaction, "GetDoddFrankStateRegulatorInfo -MGI.Cxn.MG.Common.Impl.IO", "RESPONSE", mgiContext);
			#endregion
			return srResponse;
		}

		public Data.TranslationsResponse GetTransalations(Data.TranslationsRequest request, MGIContext context)
		{
			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<Data.TranslationsRequest>(context.CustomerSessionId, request, "GetTransalations", AlloyLayerName.CXN,
				ModuleName.Transaction, "GetTransalations -MGI.Cxn.MG.Common.Impl.IO", "REQUEST", context);
			#endregion
			AgentConnectClient client = GetAgentClient();

			var translationsRequest = Mapper.Map<AgentConnectService.TranslationsRequest>(request);

			AgentConnectService.TranslationsResponse response = client.translations(translationsRequest);

			Data.TranslationsResponse translationResponse = ConvertTranslationsResponse(response);

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<Data.TranslationsResponse>(context.CustomerSessionId, translationResponse, "GetTransalations", AlloyLayerName.CXN,
				ModuleName.Transaction, "GetTransalations -MGI.Cxn.MG.Common.Impl.IO", "RESPONSE", context);
			#endregion
			return translationResponse;
		}

		public FeeLookupResponse GetFee(FeeLookupRequest feeLookupRequest, MGIContext mgiContext)
		{
			try
			{
				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<FeeLookupRequest>(mgiContext.CustomerSessionId, feeLookupRequest, "GetFee", AlloyLayerName.CXN,
					ModuleName.Transaction, "GetFee -MGI.Cxn.MG.Common.Impl.IO", "REQUEST", mgiContext);
				#endregion
				AgentConnectClient client = GetAgentClient();

				FeeLookupResponse response = client.feeLookup(feeLookupRequest);

				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<FeeLookupResponse>(mgiContext.CustomerSessionId, response, "GetFee", AlloyLayerName.CXN,
					ModuleName.Transaction, "GetFee -MGI.Cxn.MG.Common.Impl.IO", "RESPONSE", mgiContext);
				#endregion
				return response;
			}
			catch (FaultException ex)
			{
				var errorDetail = ((FaultException<Error>)(ex)).Detail;
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<FeeLookupRequest>(feeLookupRequest, "GetFee", AlloyLayerName.CXN, ModuleName.Transaction,
					"Error in GetFee -MGI.CXN.MG.Common.Impl.IO", errorDetail.errorString, ex.StackTrace);
				
				throw new MGramProviderException(errorDetail.errorCode, errorDetail.errorString);
			}
		}

		public GetFieldsForProductResponse GetFieldsForProduct(GetFieldsForProductRequest request, MGIContext mgiContext)
		{
			try
			{
				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<GetFieldsForProductRequest>(mgiContext.CustomerSessionId, request, "GetFieldsForProduct", AlloyLayerName.CXN,
					ModuleName.Transaction, "GetFieldsForProduct -MGI.Cxn.MG.Common.Impl.IO", "REQUEST", mgiContext);
				#endregion
				AgentConnectClient client = GetAgentClient();

				GetFieldsForProductResponse response = client.getFieldsForProduct(request);

				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<GetFieldsForProductResponse>(mgiContext.CustomerSessionId, response, "GetFieldsForProduct", AlloyLayerName.CXN,
					ModuleName.Transaction, "GetFieldsForProduct -MGI.Cxn.MG.Common.Impl.IO", "RESPONSE", mgiContext);
				#endregion
				return response;
			}
			catch (FaultException ex)
			{
				var errorDetail = ((FaultException<Error>)(ex)).Detail;
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<GetFieldsForProductRequest>(request, "GetFieldsForProduct", AlloyLayerName.CXN, ModuleName.Transaction,
					"Error in GetFieldsForProduct -MGI.CXN.MG.Common.Impl.IO", errorDetail.errorString, ex.StackTrace);
				
				throw new MGramProviderException(errorDetail.errorCode, errorDetail.errorString);
			}
		}


		public CommitTransactionResponse CommitTransaction(CommitTransactionRequest commitRequest, MGIContext mgiContext)
        {
            try
            {
				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<CommitTransactionRequest>(mgiContext.CustomerSessionId, commitRequest, "CommitTransaction", AlloyLayerName.CXN,
					ModuleName.Transaction, "CommitTransaction -MGI.Cxn.MG.Common.Impl.IO", "REQUEST", mgiContext);
				#endregion
                AgentConnectClient client = GetAgentClient();

				CommitTransactionResponse commitResponse = client.commitTransaction(commitRequest);

				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<CommitTransactionResponse>(mgiContext.CustomerSessionId, commitResponse, "CommitTransaction", AlloyLayerName.CXN,
					ModuleName.Transaction, "CommitTransaction -MGI.Cxn.MG.Common.Impl.IO", "RESPONSE", mgiContext);
				#endregion
				return commitResponse;
			}
			catch (FaultException ex)
			{
				var errorDetail = ((FaultException<Error>)(ex)).Detail;
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<CommitTransactionRequest>(commitRequest, "CommitTransaction", AlloyLayerName.CXN, ModuleName.Transaction,
					"Error in CommitTransaction -MGI.CXN.MG.Common.Impl.IO", errorDetail.errorString, ex.StackTrace);
				
				throw new MGramProviderException(errorDetail.errorCode, errorDetail.errorString);
			}
		}


		#region Private Methods

		private AgentConnectClient GetAgentClient()
		{
			var client = new AgentConnectClient();
			client.Endpoint.Address = new EndpointAddress(_serviceURL);
			return client;
		}

		private CTResponse ConvertCodeTableResponse(CodeTableResponse ctResponse)
		{
			var codeTableResponse = new CTResponse()
			{
				Version = ctResponse.version,
				TimeStamp = Convert.ToDateTime(ctResponse.timeStamp),
				StateProvinces = Mapper.Map<List<StateProvince>>(ctResponse.stateProvinceInfo),
				Countries = Mapper.Map<List<Country>>(ctResponse.countryInfo),
				CountryCurrencies = Mapper.Map<List<CountryCurrency>>(ctResponse.countryCurrencyInfo),
				Currencies = Mapper.Map<List<Currency>>(ctResponse.currencyInfo),
				DeliveryOptions = Mapper.Map<List<DeliveryOption>>(ctResponse.deliveryOptionInfo),
			};
			return codeTableResponse;
		}

		private StateRegulatorResponse ConvertStateRegulatorResponse(DoddFrankStateRegulatorInfoResponse dFSRResponse)
		{
			var srResponse = new StateRegulatorResponse()
			{
				Version = dFSRResponse.version,
				TimeStamp = Convert.ToDateTime(dFSRResponse.timeStamp),
				StateRegulators = Mapper.Map<List<StateRegulator>>(dFSRResponse.stateRegulatorInfo),
			};

			return srResponse;
		}

		private Data.TranslationsResponse ConvertTranslationsResponse(AgentConnectService.TranslationsResponse response)
		{
			var translationsRes = new Data.TranslationsResponse()
			{
				Version = response.translationsVersion,
				TimeStamp = response.timeStamp,
				Countries = Mapper.Map<List<Data.CountryTranslation>>(response.countryTranslations),
				DeliveryOptions = Mapper.Map<List<Data.DeliveryOptionTranslation>>(response.deliveryOptionTranslations),
				Industries = Mapper.Map<List<Data.IndustryTranslation>>(response.industryTranslations),
				Currencies = Mapper.Map<List<Data.CurrencyTranslation>>(response.currencyTranslations),
				FQDOTexts = Mapper.Map<List<Data.FQDOTextTranslation>>(response.fqdoTextTranslations)
			};

			return translationsRes;
		}
		#endregion


		public PhotoIDType GetPhotoIdType(string idType)
		{
			var photoIdType = PhotoIDType.GOV;

			switch (idType)
			{
				case "DRIVER'S LICENSE":
					photoIdType = PhotoIDType.DRV;
					break;
				case "EMPLOYMENT AUTHORIZATION CARD (EAD)":
					photoIdType = PhotoIDType.GOV;
					break;
				case "GREEN CARD / PERMANENT RESIDENT CARD":
					photoIdType = PhotoIDType.ALN;
					break;
				case "MILITARY ID":
					photoIdType = PhotoIDType.GOV;
					break;
				case "PASSPORT":
					photoIdType = PhotoIDType.PAS;
					break;
				case "U.S. STATE IDENTITY CARD":
					photoIdType = PhotoIDType.STA;
					break;
				case "INSTITUTO FEDERAL ELECTORAL":
					photoIdType = PhotoIDType.GOV;
					break;
				case "LICENCIA DE CONDUCIR":
					photoIdType = PhotoIDType.DRV;
					break;
				case "MATRICULA CONSULAR":
					photoIdType = PhotoIDType.GOV;
					break;
			}

			return photoIdType;
		}
	}
}
