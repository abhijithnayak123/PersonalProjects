using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;

using MGI.Core.Partner.Data;
using MGI.Core.Partner.Contract;
using MGI.Common.TransactionalLogging.Data;
using MGI.Common.DataAccess.Contract;
using MGI.Common.Util;

namespace MGI.Core.Partner.Impl
{
	public class ChannelPartnerServiceImpl : IChannelPartnerService, IChannelPartnerPrefundingService
	{
		#region Dependencies

		private IRepository<ChannelPartner> _channelPartnerServiceRepo;
		public IRepository<ChannelPartner> ChannelPartnerServiceRepo
		{
			set { _channelPartnerServiceRepo = value; }
		}

		private IRepository<Location> _locationsServiceRepo;
		public IRepository<Location> LocationsServiceRepo
		{
			set { _locationsServiceRepo = value; }
		}

		private IRepository<ChannelPartnerProductProcessors> _productProcessorServiceRepo;
		public IRepository<ChannelPartnerProductProcessors> ProductProcessorServiceRepo
		{
			set { _productProcessorServiceRepo = value; }
		}

		private IRepository<ChannelPartnerProductTypes> _productTypesServiceRepo;
		public IRepository<ChannelPartnerProductTypes> ProductTypesServiceRepo
		{
			set { _productTypesServiceRepo = value; }
		}

		private IRepository<TipsAndOffers> _tipsAndOffersServiceRepo;
		public IRepository<TipsAndOffers> TipsAndOffersServiceRepo
		{
			set { _tipsAndOffersServiceRepo = value; }
		}

		private IRepository<ChannelPartnerRemittanceTransaction> _remittanceTransactionServiceRepo;
		public IRepository<ChannelPartnerRemittanceTransaction> RemittanceTransactionServiceRepo
		{
			set { _remittanceTransactionServiceRepo = value; }
		}

		private IRepository<ChannelPartnerPreFunding> _preFundingServiceRepo;
		public IRepository<ChannelPartnerPreFunding> PreFundingServiceRepo
		{
			set { _preFundingServiceRepo = value; }
		}

		private IRepository<CheckType> _checkTypeRepo;
		public IRepository<CheckType> CheckTypeRepo { set { _checkTypeRepo = value; } }

		public IRepository<ChannelPartnerCertificate> ChannelPartnerCertificateRepo { private get; set; }
		public TLoggerCommon MongoDBLogger { private get; set; }
		#endregion


		public Data.ChannelPartner ChannelPartnerConfig(long channelPartnerId)
		{

			try
			{
				ChannelPartner channelPartner;
				channelPartner = _channelPartnerServiceRepo.FindBy(q => q.Id == channelPartnerId);
				return GetChannelPartner(channelPartner);
			}
			catch (Exception ex)
			{
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(channelPartnerId), "ChannelPartnerConfig", AlloyLayerName.CORE, ModuleName.Transaction,
				"Error in ChannelPartnerConfig -MGI.Core.Partner.Impl.ChannelPartnerServiceImpl", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new ChannelPartnerExceptions(ChannelPartnerExceptions.CHANNEL_PARTNER_GET_FAILED, ex);
			}

		}

		public Data.ChannelPartner ChannelPartnerConfig(string channelPartner)
		{

			try
			{
				ChannelPartner channelPartners;
				channelPartners = _channelPartnerServiceRepo.FindBy(q => q.Name.ToLower() == channelPartner.ToLower());
				return GetChannelPartner(channelPartners);
			}
			catch (Exception ex)
			{

				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<string>(channelPartner, "ChannelPartnerConfig", AlloyLayerName.CORE, ModuleName.ShoppingCart,
				"Error in ChannelPartnerConfig -MGI.Core.Partner.Impl.ChannelPartnerServiceImpl", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new ChannelPartnerExceptions(ChannelPartnerExceptions.CHANNEL_PARTNER_GET_FAILED, ex);
			}

		}

		public Data.ChannelPartner ChannelPartnerConfig(Guid rowid)
		{

			try
			{
				ChannelPartner channelPartner;
				channelPartner = _channelPartnerServiceRepo.FindBy(q => q.rowguid == rowid);
				return GetChannelPartner(channelPartner);
			}
			catch (Exception ex)
			{
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<Guid>(rowid, "ChannelPartnerConfig", AlloyLayerName.CORE, ModuleName.ShoppingCart,
					"Error in ChannelPartnerConfig -MGI.Core.Partner.Impl.ChannelPartnerServiceImpl", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new ChannelPartnerExceptions(ChannelPartnerExceptions.CHANNEL_PARTNER_GET_FAILED, ex);
			}

		}

		private Data.ChannelPartner GetChannelPartner(ChannelPartner channelPartner)
		{
			string authType;

			if (channelPartner.HasNonGPRCard && channelPartner.AllowPhoneNumberAuthentication)
				authType = "MembershipCardAndPhone";
			else if (channelPartner.HasNonGPRCard)
				authType = "MembershipCard";
			else if (channelPartner.AllowPhoneNumberAuthentication)
				authType = "Phone";
			else
				authType = "GPRCard";

			channelPartner.AuthenticationType = authType;
			return channelPartner;
		}

		public List<string> Locations(string channelPartner)
		{
			try
			{
				var chnlpartner = ChannelPartnerConfig(channelPartner);
				var locationsList = _locationsServiceRepo.FilterBy(q => q.ChannelPartnerId == chnlpartner.Id);
				locationsList = locationsList.Where(c => c.IsActive == true);
				List<string> locations = new List<string>();
				if (locationsList != null)
					locations = locationsList.Select(y => y.LocationName).ToList();
				return locations;
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new ChannelPartnerExceptions(ChannelPartnerExceptions.CHANNEL_PARTNER_LOCATIONS_GET_FAILED, ex);
			}
		}

		private string safeSQLString(string s)
		{
			return s.Replace("\\", "").Replace("'", "''");
		}

		public List<string> DBGetChannelPartnerProductProcessors(long channelPartnerId, int productTypeId)
		{
			var results = (from x in _productTypesServiceRepo.All()
						   join y in _productProcessorServiceRepo.All() on x.Id equals y.ChannelPartnerProductTypeId into xy
						   where x.ChannelPartnerId == channelPartnerId && x.ProductTypeId == productTypeId
						   from z in xy
						   select z.Processor).ToList();

			return results;
		}

		public List<string> GetBillPayProcessors(long channelPartnerId)
		{
			return this.DBGetChannelPartnerProductProcessors(channelPartnerId, (int)ProductType.BillPay);
		}

		public string GetCheckProcessor(long channelPartnerId)
		{
			return GetSingleProcessor(channelPartnerId, (int)ProductType.CheckCashing);
		}

		public string GetGPRProcessor(long channelPartnerId)
		{
			return GetSingleProcessor(channelPartnerId, (int)ProductType.ExoNexxoPurseDebit);
		}

		private string GetSingleProcessor(long channelPartnerId, int productTypeId)
		{
			return this.DBGetChannelPartnerProductProcessors(channelPartnerId, productTypeId).FirstOrDefault();
		}

		public List<string> GetMoneyTransferProcessors(long channelPartnerId)
		{
			return this.DBGetChannelPartnerProductProcessors(channelPartnerId, (int)ProductType.NexxoPromotion);
		}

		public List<string> GetTopUpProcessors(long channelPartnerId)
		{
			List<string> procs = new List<string>();
			procs = this.DBGetChannelPartnerProductProcessors(channelPartnerId, (int)ProductType.DomesticTopUp);
			procs.AddRange(this.DBGetChannelPartnerProductProcessors(channelPartnerId, (int)ProductType.IntlTopUp));
			return procs;
		}

		////tips and tools
		public List<TipsAndOffers> GetTipsAndOffers(string channelPartner, string lang, string viewName)
		{
			try
			{
				List<TipsAndOffers> tipsAndOffers;

				switch (lang.ToLower())
				{
					case "es-us":
						tipsAndOffers = (from recs in _tipsAndOffersServiceRepo.FilterBy(a => a.ViewName == viewName && a.ChannelPartnerName == channelPartner)
										 select new TipsAndOffers { TipsAndOffersValue = recs.TipsAndOffersEs, OptionalFilter = recs.OptionalFilter }).ToList();
						break;
					case "en-us":
						tipsAndOffers = (from recs in _tipsAndOffersServiceRepo.FilterBy(a => a.ViewName == viewName && a.ChannelPartnerName == channelPartner)
										 select new TipsAndOffers { TipsAndOffersValue = recs.TipsAndOffersEn, OptionalFilter = recs.OptionalFilter }).ToList();
						break;
					default:
						tipsAndOffers = (from recs in _tipsAndOffersServiceRepo.FilterBy(a => a.ViewName == viewName && a.ChannelPartnerName == channelPartner)
										 select new TipsAndOffers { TipsAndOffersValue = recs.TipsAndOffersEn, OptionalFilter = recs.OptionalFilter }).ToList();
						break;
				}
				return tipsAndOffers;
			}
			catch (Exception ex)
			{
				throw new ChannelPartnerExceptions(ChannelPartnerExceptions.TIPS_AND_OFFERS_GET_FAILED, ex);
			}
		}

		public List<string> GetCheckTypes()
		{

			List<CheckType> checkTypes = _checkTypeRepo.All().ToList();

			if (checkTypes == null)
				throw new TransactionServiceException(TransactionServiceException.CHECKTYPE_NOT_FOUND);

			return new List<string>(checkTypes.OrderBy(x => x.Name).Select(cc => cc.Name));
		}

		public CheckType GetCheckType(string name)
		{
			CheckType checkType = _checkTypeRepo.FindBy(t => t.Name == name);
			if (checkType == null)
				throw new TransactionServiceException(TransactionServiceException.CHECKTYPE_NOT_FOUND);
			return checkType;
		}

		public CheckType GetCheckType(int Id)
		{
			CheckType checkType = _checkTypeRepo.FindBy(t => t.Id == Id);
			if (checkType == null)
				throw new TransactionServiceException(TransactionServiceException.CHECKTYPE_NOT_FOUND);
			return checkType;
		}

		public ChannelPartnerCertificate GetChannelPartnerCertificateInfo(long channelPartnerId, string issuer)
		{
			ChannelPartnerCertificate certificateInfo = null;

			try
			{
				certificateInfo = ChannelPartnerCertificateRepo.FindBy(cpc => cpc.ChannelPartner.Id == channelPartnerId && cpc.Issuer.ToLower() == issuer.ToLower());
			}
			catch (Exception ex)
			{
				throw new ChannelPartnerExceptions(ChannelPartnerExceptions.CHANNEL_PARTNER_CERTIFICATE_INFO_GET_FAILED, ex);
			}

			return certificateInfo;
		}
	}
}
