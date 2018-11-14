using TCF.Zeo.Core.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Data;
using P3Net.Data.Common;
using System.Data;
using P3Net.Data;

namespace TCF.Zeo.Core.Impl
{
    public class MessageStoreImpl : IMessageStore
    {
        public Message GetMessage(string messageKey, ZeoContext context)
        {
            Message message = null;

            StoredProcedure customerProcedure = new StoredProcedure("usp_GetErrorMessageByMessageKey");

            customerProcedure.WithParameters(InputParameter.Named("messageKey").WithValue(messageKey));
            customerProcedure.WithParameters(InputParameter.Named("ChannelPartnerId").WithValue(context.ChannelPartnerId));
            customerProcedure.WithParameters(InputParameter.Named("locationId").WithValue(context.LocationId));

            using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(customerProcedure))
            {
                while (datareader.Read())
                {
                    message = new Message();
                    message.MessageKey = datareader.GetStringOrDefault("MessageKey");
                    message.Content = datareader.GetStringOrDefault("Content");
                    message.AddlDetails = datareader.GetStringOrDefault("AddlDetails");
                    message.ErrorType = Convert.ToInt32(datareader.GetStringOrDefault("Type"));
                    message.Processor = datareader.GetStringOrDefault("Processor");
                }
            }
                
            return message;
        }

        public Message Lookup(MessageStoreSearch search, ZeoContext context)
        {
            Message message = null;
            StoredProcedure customerProcedure = new StoredProcedure("usp_GetErrorMessage");

            customerProcedure.WithParameters(InputParameter.Named("ChannelPartnerId").WithValue(context.ChannelPartnerId));
            customerProcedure.WithParameters(InputParameter.Named("errorCode").WithValue(search.ErrorCode));
            customerProcedure.WithParameters(InputParameter.Named("language").WithValue((int)search.Language));
            customerProcedure.WithParameters(InputParameter.Named("providerCode").WithValue(search.ProviderCode));
            customerProcedure.WithParameters(InputParameter.Named("productCode").WithValue(search.ProductCode));
            customerProcedure.WithParameters(InputParameter.Named("locationId").WithValue(context.LocationId));

            using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(customerProcedure))
            {
                while (datareader.Read())
                {
                    message = new Message();
                    message.MessageKey = datareader.GetStringOrDefault("MessageKey");
                    message.Content = datareader.GetStringOrDefault("Content");
                    message.AddlDetails = datareader.GetStringOrDefault("AddlDetails");
                    message.ErrorType = Convert.ToInt32(datareader.GetStringOrDefault("Type"));
                    message.Processor = datareader.GetStringOrDefault("Processor");
                }
            }
                
            return message;
        }
    }
}
