using TCF.Zeo.Common.Data;
using TCF.Channel.Zeo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Channel.Zeo.Service.Contract
{
    [ServiceContract]
    public interface IChannelPartnerService
    {
        /// <summary>
        /// This method is to get the collection of channel partner group details by channel partner name
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ZeoSoapFault))]
        Response GetPartnerGroups(long channelPartnerId, Data.ZeoContext context);

        /// <summary>
        /// This method is to get the ChannelPartnerConfig details by channel partner id
        /// </summary>
        /// <param name="channelPartner"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ZeoSoapFault))]
        Response ChannelPartnerConfig(Data.ZeoContext context);

        /// <summary>
        /// This method is to get the ChannelPartnerConfig details by channel partner name
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ZeoSoapFault))]
        Response ChannelPartnerConfigByName(string channelPartnerName, Data.ZeoContext context);



        /// <summary>
        /// This method  to get List of Tips and Offers by channel partner name and view name
        /// </summary>
        /// <param name="context"></param>
        /// <param name="language"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ZeoSoapFault))]
        Response GetTipsAndOffers(long channelPartnerId, string language, string viewName, string optionalFilter, Data.ZeoContext context);

        /// <summary>
        /// This method  to get channel partner certificate info by channel partner id and issuer  
        /// </summary>
        /// <param name="context"></param>
        /// <param name="issuer"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ZeoSoapFault))]
        Response GetChannelPartnerCertificateInfo(long channelpartnerId, string issuer, Data.ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(ZeoSoapFault))]
        Response GetProvidersbyChannelPartnerName(string channelPartnerName, Data.ZeoContext context);
    }
}
