using TCF.Channel.Zeo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using commonData = TCF.Zeo.Common.Data;
namespace TCF.Zeo.Biz.Contract
{
    public interface IMessageStoreService
    {
        /// <summary>
        /// To retrieve error message from Database
        /// </summary>
        /// <param name="partnerId">ChannelPartnerId</param>
        /// <param name="errorKey">Error Code</param>
        /// <returns>Error Message</returns>
        Message Lookup(MessageStoreSearch search, commonData.ZeoContext context);

        /// <summary>
        /// To Retrive error messages for web layer.
        /// </summary>
        /// <param name="messageKey">Error Code</param>
        /// <param name="context">Common Paramater</param>
        /// <returns></returns>
        Message GetMessage(string messageKey, commonData.ZeoContext context);
    }
}
