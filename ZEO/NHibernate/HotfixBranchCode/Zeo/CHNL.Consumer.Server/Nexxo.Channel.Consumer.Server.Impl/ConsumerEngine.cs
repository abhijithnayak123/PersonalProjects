using System;
using System.Collections.Generic;
using System.Linq;
using MGI.Channel.Consumer.Server.Contract;
using MGI.Channel.Shared.Server.Contract;
using MGI.Channel.Shared.Server.Impl;
using bizPartnerSessionContext = MGI.Biz.Partner.Data.SessionContext;
using bizCustomerSessionContext = MGI.Biz.Customer.Data.SessionContext;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;

namespace MGI.Channel.Consumer.Server.Impl
{
    public partial class ConsumerEngine : IConsumerService
    {
        #region Injected Services
        private IConsumerService Self;
        public Dictionary<string, string> TimeZones { get; set; }
        public ISharedService SharedEngine { get; set; }
        #endregion

        public ConsumerEngine()
        {
            CustomerConverter();
            MoneyTransferConverter();
            BillPayConverter();
            FundEngineConverter();
            ShoppingCartConverter();
            SetSelf(this);
        }

        #region IConsumerService Impl

        public void SetSelf(IConsumerService dts)
        {
            Self = dts;
        }

        #endregion

        #region Private methods

		private SessionContext GetSharedSessionContext(MGIContext mgiContext)
        {
            return new SessionContext
            {
                AgentId = 0,
                AgentName = null,
                LocationId = Guid.Empty,
                ChannelPartnerId = int.Parse(mgiContext.ChannelPartnerId.ToString()),
                AppName = mgiContext.ApplicationName,
                //CustomerSessionId = Convert.ToString(GetDictionaryValue(context, "CustomerSessionId")),
                //DTKiosk = ,
                //LocationAgentId = ,
                //SelectedLanguage = 
            };
        }

        /// <summary>
        /// US2364 : Append timeZone abbreviation of transaction to the transaction Date 
        /// </summary>
        /// <param name="metaData"></param>
        /// <param name="dateField"></param>
        /// <returns></returns>
        private Dictionary<string, object> AppendTimeZoneAbbr(Dictionary<string, object> metaData,string dateField)
        {
            if (metaData == null)
                return null;

            if ( metaData.ContainsKey("TransactionTimeZone") && TimeZones.Where(y => y.Key == metaData["TransactionTimeZone"].ToString()).Count() > 0)
            {
                string timeZonesAbbr = TimeZones.Where(y => y.Key == metaData["TransactionTimeZone"].ToString()).FirstOrDefault().Value;

                if (metaData.ContainsKey(dateField))
                {
                    metaData[dateField] += " " + timeZonesAbbr;
                }
            }

            return metaData;
        }

        #endregion



    }
}
