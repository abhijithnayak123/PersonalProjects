using System;
using System.Linq;
using System.Collections.Generic;

using AutoMapper;

using MGI.Core.Partner.Contract;

using PTNRContract = MGI.Core.Partner.Contract;
using ChannelPartner = MGI.Core.Partner.Data.ChannelPartner;
using TipsAndOffers = MGI.Core.Partner.Data.TipsAndOffers;
using ChannelPartnerDTO = MGI.Biz.Partner.Data.ChannelPartner;
using TipsAndOffersDTO = MGI.Biz.Partner.Data.TipsAndOffers;
using BizPartnerService = MGI.Biz.Partner.Contract.IChannelPartnerService;

using MGI.Biz.Partner.Data;
using MGI.Biz.Partner.Contract;
using MGI.Common.Util;

namespace MGI.Biz.Partner.Impl
{
	public class ChannelPartnerImpl : BizPartnerService
	{
		private PTNRContract.IChannelPartnerService _channelPartnerService;
		public PTNRContract.IChannelPartnerService ChannelPartnerServiceImpl { set { _channelPartnerService = value; } }

		private PTNRContract.IChannelPartnerGroupService _ptnrGroupService;
		public PTNRContract.IChannelPartnerGroupService PartnerGroupService { set { _ptnrGroupService = value; } }

		public ChannelPartnerImpl()
		{
			Mapper.CreateMap<MGI.Core.Partner.Data.ChannelPartnerProductProvider, ChannelPartnerProductProvider>()
					.ForMember(x => x.ProductName, o => o.MapFrom(x => x.ProductProcessor.Product.Name))
					.ForMember(x => x.ProcessorId, o => o.MapFrom(x => x.ProductProcessor.Code))
					.ForMember(x => x.ProcessorName, o => o.MapFrom(x => x.ProductProcessor.Processor.Name))
					.ForMember(x => x.IsSSNRequired, o => o.MapFrom(x => x.ProductProcessor.IsSSNRequired))
					.ForMember(x => x.IsSWBRequired, o => o.MapFrom(x => x.ProductProcessor.IsSWBRequired))
					.ForMember(x => x.IsTnCForcePrintRequired, o => o.MapFrom(x => x.IsTnCForcePrintRequired))
					.ForMember(x => x.ReceiptCopies, o => o.MapFrom(x => x.ProductProcessor.ReceiptCopies))
					.ForMember(x => x.ReceiptReprintCopies, o => o.MapFrom(x => x.ProductProcessor.ReceiptReprintCopies))
					.ForMember(x => x.CanParkReceiveMoney, o => o.MapFrom(x => x.ProductProcessor.CanParkReceiveMoney))
					.ForMember(x => x.CheckEntryType, o => o.MapFrom(x => x.CheckEntryType))
					.ForMember(x => x.MinimumTransactAge, o => o.MapFrom(x => x.MinimumTransactAge));
			Mapper.CreateMap<ChannelPartner, ChannelPartnerDTO>()
				  .ForMember(x => x.CashOverCounter, o => o.MapFrom(x => x.ChannelPartnerConfig.CashOverCounter))
				  .ForMember(x => x.DisableWithdrawCNP, o => o.MapFrom(x => x.ChannelPartnerConfig.DisableWithdrawCNP))
				  .ForMember(x => x.FrankData, o => o.MapFrom(x => x.ChannelPartnerConfig.FrankData))
				  .ForMember(x => x.IsCheckFrank, o => o.MapFrom(x => x.ChannelPartnerConfig.IsCheckFrank))
				  .ForMember(x => x.IsNotesEnable, o => o.MapFrom(x => x.ChannelPartnerConfig.IsNotesEnable))
				  .ForMember(x => x.IsReferralSectionEnable, o => o.MapFrom(x => x.ChannelPartnerConfig.IsReferralSectionEnable))
				  .ForMember(x => x.IsMGIAlloyLogoEnable, o => o.MapFrom(x => x.ChannelPartnerConfig.IsMGIAlloyLogoEnable))
				  .ForMember(x => x.MasterSSN, o => o.MapFrom(x => x.ChannelPartnerConfig.MasterSSN))
				  .ForMember(x => x.IsMailingAddressEnable, o => o.MapFrom(x => x.ChannelPartnerConfig.IsMailingAddressEnable))
				  .ForMember(x => x.CanEnableProfileStatus, o => o.MapFrom(x => x.ChannelPartnerConfig.CanEnableProfileStatus))
				  .ForMember(x => x.CustomerMinimumAge, o => o.MapFrom(x => x.ChannelPartnerConfig.CustomerMinimumAge));

			Mapper.CreateMap<ChannelPartnerDTO, ChannelPartner>();

			Mapper.CreateMap<TipsAndOffers, TipsAndOffersDTO>();

			Mapper.CreateMap<MGI.Core.Partner.Data.ChannelPartnerCertificate, Data.ChannelPartnerCertificate>();
		}

		public System.Collections.Generic.List<string> Locations(long agentSessionId, string channelPartner, MGIContext mgiContext)
		{
			try
			{
				return _channelPartnerService.Locations(channelPartner);
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizChannelPartnerException(BizChannelPartnerException.CHANNEL_PARTNER_LOCATIONS_GET_FAILED, ex);
			}
		}

		public ChannelPartnerDTO ChannelPartnerConfig(string channelPartner, MGIContext mgiContext)
		{
			try
			{
				ChannelPartner cp = _channelPartnerService.ChannelPartnerConfig(channelPartner);
				return Mapper.Map<ChannelPartner, ChannelPartnerDTO>(cp);
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizChannelPartnerException(BizChannelPartnerException.CHANNEL_PARTNER_GET_FAILED, ex);
			}
		}

		public ChannelPartnerDTO ChannelPartnerConfig(int channelPartnerId, MGIContext mgiContext)
		{
			try
			{
				ChannelPartner cp = _channelPartnerService.ChannelPartnerConfig(channelPartnerId);
				return Mapper.Map<ChannelPartner, ChannelPartnerDTO>(cp);
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizChannelPartnerException(BizChannelPartnerException.CHANNEL_PARTNER_GET_FAILED, ex);
			}
		}

		public ChannelPartnerDTO ChannelPartnerConfig(Guid rowid, MGIContext mgiContext)
		{
			try
			{
				ChannelPartner cp = _channelPartnerService.ChannelPartnerConfig(rowid);
				return Mapper.Map<ChannelPartner, ChannelPartnerDTO>(cp);
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizChannelPartnerException(BizChannelPartnerException.CHANNEL_PARTNER_GET_FAILED, ex);
			}
		}
		public List<TipsAndOffersDTO> GetTipsAndOffers(long agentSessionId, string channelPartner, string language, string viewName, MGIContext mgiContext)
		{
			try
			{
				List<TipsAndOffers> coreTipsAndOffers = _channelPartnerService.GetTipsAndOffers(channelPartner, language, viewName);
				List<TipsAndOffersDTO> bizTipsAndOffers = Mapper.Map<List<TipsAndOffersDTO>>(coreTipsAndOffers);
				return bizTipsAndOffers;
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizChannelPartnerException(BizChannelPartnerException.TIPS_AND_OFFERS_GET_FAILED, ex);
			}
		}

		public List<string> GetGroups(string channelPartner, MGIContext mgiContext)
		{
			try
			{
				return _ptnrGroupService.GetAll(channelPartner).Select(g => g.Name).ToList();
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizChannelPartnerException(BizChannelPartnerException.CHANNEL_PARTNER_GROUP_GET_FAILED, ex);
			}
		}

		public ChannelPartnerCertificate GetChannelPartnerCertificateInfo(long channelPartnerId, string issuer, MGIContext mgiContext)
		{
			try
			{
				Core.Partner.Data.ChannelPartnerCertificate certificateInfo = _channelPartnerService.GetChannelPartnerCertificateInfo(channelPartnerId, issuer);
				return Mapper.Map<ChannelPartnerCertificate>(certificateInfo);
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizChannelPartnerException(BizChannelPartnerException.CHANNEL_PARTNER_CERTIFICATE_INFO_GET_FAILED, ex);
			}
		}
	}
}
