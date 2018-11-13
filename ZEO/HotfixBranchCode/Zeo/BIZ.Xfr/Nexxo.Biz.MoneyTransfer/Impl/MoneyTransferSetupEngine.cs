using AutoMapper;
using MGI.Biz.MoneyTransfer.Contract;
using MGI.Biz.MoneyTransfer.Data;
using MGI.Common.TransactionalLogging.Data;
using MGI.Common.Util;
using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using MGI.Cxn.Common.Processor.Util;
using MGI.Cxn.MoneyTransfer.Contract;
using MGI.TimeStamp;
using System;
using System.Collections.Generic;
using bizMasterData = MGI.Biz.MoneyTransfer.Data.MasterData;
using cxnMasterData = MGI.Cxn.MoneyTransfer.Data.MasterData;

namespace MGI.Biz.MoneyTransfer.Impl
{
	public partial class MoneyTransferEngine : IMoneyTransferSetupEngine
	{
		// TODO Merge public ICustomerSessionService CustomerSessionService { private get; set; }
		//public IChannelPartnerService PTNRChannelPartnerService { private get; set; }
		//public ProcessorRouter MoneyTransferProcessorSvc { private get; set; }
		//public MGI.Core.Partner.Contract.ILocationCounterIdService LocationCounterIdService { private get; set; }

		public List<bizMasterData> GetCountries(long customerSessionId, MGIContext mgiContext)
		{
			try
			{
               #region AL-3370 Transactional Log User Story
                MongoDBLogger.Info<string>(customerSessionId, string.Empty, "GetCountries", AlloyLayerName.BIZ, ModuleName.SendMoney,
                                          "Begin GetCountries-MGI.Biz.MoneyTransfer.Impl.MoneyTransferSetupEngine", mgiContext);
			   #endregion
				IMoneyTransfer CXNMoneyTransfer = _GetMoneyTransferProcessor(mgiContext.ChannelPartnerName);
				List<bizMasterData> countries = Mapper.Map<List<cxnMasterData>, List<bizMasterData>>(CXNMoneyTransfer.GetCountries());

               #region AL-3370 Transactional Log User Story
                MongoDBLogger.ListInfo<bizMasterData>(customerSessionId, countries, "GetCountries", AlloyLayerName.BIZ, ModuleName.SendMoney,
                                          "End GetCountries-MGI.Biz.MoneyTransfer.Impl.MoneyTransferSetupEngine", mgiContext);
			   #endregion
				return countries;
			}
			catch (Exception ex)
			{
				throw new TransferException(TransferException.PROVIDER_ERROR, ex);
			}
		}

        public List<bizMasterData> GetStates(long customerSessionId, string countryCode, MGIContext mgiContext)
		{
			try
			{
               #region AL-3370 Transactional Log User Story
                MongoDBLogger.Info<string>(customerSessionId, countryCode, "GetStates", AlloyLayerName.BIZ, ModuleName.SendMoney,
                                          "Begin GetStates-MGI.Biz.MoneyTransfer.Impl.MoneyTransferSetupEngine", mgiContext);
			   #endregion
				IMoneyTransfer CXNMoneyTransfer = _GetMoneyTransferProcessor(mgiContext.ChannelPartnerName);
				List<bizMasterData> states = Mapper.Map<List<cxnMasterData>, List<bizMasterData>>(CXNMoneyTransfer.GetStates(countryCode));

               #region AL-3370 Transactional Log User Story
                MongoDBLogger.ListInfo<bizMasterData>(customerSessionId, states, "GetStates", AlloyLayerName.BIZ, ModuleName.SendMoney,
                                          "End GetStates-MGI.Biz.MoneyTransfer.Impl.MoneyTransferSetupEngine", mgiContext);
			   #endregion
				return states;
			}
			catch (Exception ex)
			{
				throw new TransferException(TransferException.PROVIDER_ERROR, ex);
			}
		}

        public List<bizMasterData> GetCities(long customerSessionId, string stateCode, MGIContext mgiContext)
		{
            try
            {
               #region AL-3370 Transactional Log User Story
                MongoDBLogger.Info<string>(customerSessionId, stateCode, "GetCities", AlloyLayerName.BIZ, ModuleName.SendMoney,
                                          "Begin GetCities-MGI.Biz.MoneyTransfer.Impl.MoneyTransferSetupEngine", mgiContext);
			   #endregion
				IMoneyTransfer CXNMoneyTransfer = _GetMoneyTransferProcessor(mgiContext.ChannelPartnerName);
                List<bizMasterData> cities = Mapper.Map<List<cxnMasterData>, List<bizMasterData>>(CXNMoneyTransfer.GetCities(stateCode));

               #region AL-3370 Transactional Log User Story
                MongoDBLogger.ListInfo<bizMasterData>(customerSessionId, cities, "GetCities", AlloyLayerName.BIZ, ModuleName.SendMoney,
                                          "End GetCities-MGI.Biz.MoneyTransfer.Impl.MoneyTransferSetupEngine", mgiContext);
			   #endregion
				return cities;
            }
            catch (Exception ex)
            {
                throw new TransferException(TransferException.PROVIDER_ERROR, ex);
            }
		}

        public List<bizMasterData> GetBannerMsgs(long agentSessionId, MGIContext mgiContext)
		{
				List<bizMasterData> bannermsgs = new List<bizMasterData>();

                string channerPartnerName = mgiContext.ChannelPartnerName;

                var provider = _GetMoneyTransferProvider(channerPartnerName);

                string counterId = string.Empty;

                if (mgiContext.LocationRowGuid!=null)
                    counterId = LocationCounterIdService.Get((Guid)mgiContext.LocationRowGuid, (int)Enum.Parse(typeof(ProviderIds), provider));
                mgiContext.WUCounterId = counterId;

            if (_GetMoneyTransferProcessor(mgiContext.ChannelPartnerName) != null)
                bannermsgs = Mapper.Map<List<cxnMasterData>, List<bizMasterData>>(_GetMoneyTransferProcessor(channerPartnerName).GetBannerMsgs(mgiContext));

				return bannermsgs;
		}

        public string GetCurrencyCode(long customerSessionId, string countryCode, MGIContext mgiContext)
		{
			try
			{
                IMoneyTransfer CXNMoneyTransfer = _GetMoneyTransferProcessor(mgiContext.ChannelPartnerName);
				return CXNMoneyTransfer.GetCurrencyCode(countryCode);
			}
			catch (Exception ex)
			{
				throw new TransferException(TransferException.UNKNOWN, ex);
			}
		}

        public List<bizMasterData> GetCurrencyCodeList(long customerSessionId, string countryCode, MGIContext mgiContext)
		{
			try
			{
               #region AL-3370 Transactional Log User Story
                MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(countryCode), "GetCurrencyCodeList", AlloyLayerName.BIZ, ModuleName.SendMoney,
                                          "Begin GetCurrencyCodeList-MGI.Biz.MoneyTransfer.Impl.MoneyTransferSetupEngine", mgiContext);
			   #endregion
				IMoneyTransfer CXNMoneyTransfer = _GetMoneyTransferProcessor(mgiContext.ChannelPartnerName);
				List<bizMasterData> currencyList = Mapper.Map<List<cxnMasterData>, List<bizMasterData>>(CXNMoneyTransfer.GetCurrencyCodeList(countryCode));

               #region AL-3370 Transactional Log User Story
                MongoDBLogger.ListInfo<bizMasterData>(customerSessionId, currencyList, "GetCurrencyCodeList", AlloyLayerName.BIZ, ModuleName.SendMoney,
                                          "End GetCurrencyCodeList-MGI.Biz.MoneyTransfer.Impl.MoneyTransferSetupEngine", mgiContext);
			   #endregion
				return currencyList;
			}
			catch (Exception ex)
			{
				throw new TransferException(TransferException.UNKNOWN, ex);
			}
		}

		public List<Reason> GetRefundReasons(long customerSessionId, ReasonRequest request, MGIContext mgiContext)
		{
           #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<ReasonRequest>(customerSessionId, request, "GetRefundReasons", AlloyLayerName.BIZ, ModuleName.SendMoney,
                                      "Begin GetRefundReasons-MGI.Biz.MoneyTransfer.Impl.MoneyTransferSetupEngine", mgiContext);
		   #endregion
			List<Reason> reasonlists = new List<Reason>();
			CustomerSession customerSession = GetCustomerSession(customerSessionId);

            GetCustomerSessionCounterId(customerSession, ref mgiContext);
			if (_GetMoneyTransferProcessor(mgiContext.ChannelPartnerName) != null)
			{
				MGI.Cxn.MoneyTransfer.Data.ReasonRequest cxnReasonRequest = Mapper.Map<MGI.Cxn.MoneyTransfer.Data.ReasonRequest>(request);
                reasonlists = Mapper.Map<List<MGI.Cxn.MoneyTransfer.Data.Reason>, List<Reason>>(_GetMoneyTransferProcessor(mgiContext.ChannelPartnerName).GetRefundReasons(cxnReasonRequest, mgiContext));
			}
           #region AL-3370 Transactional Log User Story
            MongoDBLogger.ListInfo<Reason>(customerSessionId, reasonlists, "GetRefundReasons", AlloyLayerName.BIZ, ModuleName.SendMoney,
                                      "End GetRefundReasons-MGI.Biz.MoneyTransfer.Impl.MoneyTransferSetupEngine", mgiContext);
		   #endregion
			return reasonlists;

		}
        
		private void GetCustomerSessionCounterId(CustomerSession session, ref MGIContext mgiContext)
		{
            if (!IsHardCodedCounterId)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(mgiContext.LocationRowGuid)))
                {
                    string counterId = string.Empty;

                    if (session.CustomerSessionCounter == null)
                    {
                        //Update Customersession with CounterId 
                        ChannelPartner channelPartner = PTNRChannelPartnerService.ChannelPartnerConfig(session.Customer.ChannelPartnerId);

                        var provider = _GetMoneyTransferProvider(channelPartner.Name);

                        int providerId = (int)Enum.Parse(typeof(ProviderIds), provider);

                        Guid locationRowGuid = new Guid(Convert.ToString(mgiContext.LocationRowGuid));

                        counterId = LocationCounterIdService.Get(locationRowGuid, providerId);

                        if (!string.IsNullOrEmpty(counterId))
                        {
                            UpdateLocationCounterIdStatus(locationRowGuid, counterId, providerId, false, mgiContext);

                            CreateCustomerSessionCounterId(session, counterId, mgiContext);
                        }
                    }
                    else
                    {
                        counterId = session.CustomerSessionCounter.CounterId;
                    }

                    mgiContext.WUCounterId = counterId;
                }
            }
		}

		private void CreateCustomerSessionCounterId(CustomerSession session, string counterId, MGIContext mgiContext)
		{
			CustomerSessionCounter customerSessionCounterId = new CustomerSessionCounter()
			{
				CustomerSession = session,
				CounterId = counterId,
				DTServerCreate = DateTime.Now,
				DTServerLastModified = DateTime.Now,
				DTTerminalCreate = Clock.DateTimeWithTimeZone(mgiContext.TimeZone),
                DTTerminalLastModified = Clock.DateTimeWithTimeZone(mgiContext.TimeZone)
			};

			session.CustomerSessionCounter = customerSessionCounterId;

			CustomerSessionService.Save(session);

		}
	}
}
