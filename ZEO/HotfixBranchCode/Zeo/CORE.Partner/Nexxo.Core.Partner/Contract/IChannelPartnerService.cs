using System;
using System.Collections.Generic;

using MGI.Core.Partner.Data;

namespace MGI.Core.Partner.Contract
{
	public interface IChannelPartnerService
	{
		/// <summary>
		/// This method is to get the collection of locations by channel partner name
		/// </summary>
		/// <param name="channelPartner">This is channel partner name</param>
		/// <returns>Collection of locations</returns>
		List<string> Locations(string channelPartner);

		/// <summary>
		/// This method is to get channel partner details by channel partner name
		/// </summary>
		/// <param name="channelPartner">This is channel partner name</param>
		/// <returns>Channel partner details</returns>
		Partner.Data.ChannelPartner ChannelPartnerConfig(string channelPartner);

		/// <summary>
		/// This method is to get channel partner details by channel partner id
		/// </summary>
		/// <param name="channelPartnerId">This is channel partner id</param>
		/// <returns>Channel partner details</returns>
		Partner.Data.ChannelPartner ChannelPartnerConfig(long channelPartnerId);

		/// <summary>
		/// This method is to get channel partner details by channel partner Guid
		/// </summary>
		/// <param name="rowid">This is channel partner Guid</param>
		/// <returns>Channel partner details</returns>
		Partner.Data.ChannelPartner ChannelPartnerConfig(Guid rowid);

		/// <summary>
		/// This method is to get collection of channel partner product processors by channel partner id and product type id.
		/// </summary>
		/// <param name="channelPartnerId">This is channel partner id</param>
		/// <param name="productTypeId">This is channel partner product type id</param>
		/// <returns>Collection of channel partner product processors</returns>
		List<string> DBGetChannelPartnerProductProcessors(long channelPartnerId, int productTypeId);

		/// <summary>
		/// This method is to get collection of money transfer processors by channel partner id.
		/// </summary>
		/// <param name="channelPartnerId">This is channel partner id</param>		
		/// <returns>Collection of money transfer processors</returns>
		List<string> GetMoneyTransferProcessors(long channelPartnerId);

		/// <summary>
		/// This method is to get collection bill pay processors by channel partner id 
		/// </summary>
		/// <param name="channelPartnerId">This is channel partner id</param>
		/// <returns>Collection of bill pay processors</returns>
		List<string> GetBillPayProcessors(long channelPartnerId);

		/// <summary>
		/// This method is to get collection of check processors by channel partner id.
		/// </summary>
		/// <param name="channelPartnerId">This is channel partner id</param>
		/// <returns>Collection of check processors</returns>
		string GetCheckProcessor(long channelPartnerId);

		/// <summary>
		/// This method is to get collection of GPR processors by channel partner id.
		/// </summary>
		/// <param name="channelPartnerId">This is channel partner id</param>
		/// <returns>Collection of GPR processors</returns>
		string GetGPRProcessor(long channelPartnerId);

		/// <summary>
		/// This method is to get collection of top up processors by channel partner id.
		/// </summary>
		/// <param name="channelPartnerId">This is channel partner id</param>
		/// <returns>Collection of top up processors</returns>
		List<string> GetTopUpProcessors(long channelPartnerId);

		/// <summary>
		/// This method is to get collection of check types
		/// </summary>
		/// <returns>Collection check types</returns>
		List<string> GetCheckTypes();

		/// <summary>
		/// This method is to get check type by name
		/// </summary>
		/// <param name="name">This is check name</param>
		/// <returns>check type details</returns>
		CheckType GetCheckType(string name);

		/// <summary>
		/// This method is to get check type by id
		/// </summary>
		/// <param name="Id">Unique id</param>
		/// <returns>check type details</returns>
		CheckType GetCheckType(int Id);

		/// <summary>
		/// This method is to get collection of tips and offers
		/// </summary>
		/// <param name="channelPartner">This is channel partner name</param>
		/// <param name="language">This is language</param>
		/// <param name="viewName">This is view name</param>
		/// <returns>Collection of tips and offers details</returns>
		List<TipsAndOffers> GetTipsAndOffers(string channelPartner, string language, string viewName);

		/// <summary>
		/// This method is to get the channel partner certificate
		/// </summary>
		/// <param name="channelPartnerId">This is channel partner id</param>
		/// <param name="issuer">This is issuer</param>
		/// <returns>channel partner certificate</returns>
		ChannelPartnerCertificate GetChannelPartnerCertificateInfo(long channelPartnerId, string issuer);
	}
}