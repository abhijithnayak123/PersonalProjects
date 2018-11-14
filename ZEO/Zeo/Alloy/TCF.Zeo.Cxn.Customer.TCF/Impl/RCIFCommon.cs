using P3Net.Data.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Cxn.Common;
using TCF.Zeo.Cxn.Customer.TCF.Data;
using P3Net.Data;

namespace TCF.Zeo.Cxn.Customer.TCF.Impl
{
    internal class RCIFCommon
    {
        public static RCIFCredential GetCredential(long channelPartnerId)
        {
            RCIFCredential credentials = new RCIFCredential();

            StoredProcedure coreCustomerProcedure = new StoredProcedure("usp_GetRCIFCredentials");

            coreCustomerProcedure.WithParameters(InputParameter.Named("channelPartnerId").WithValue(channelPartnerId));

            using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(coreCustomerProcedure))
            {
                while (datareader.Read())
                {
                    credentials.ServiceUrl = datareader.GetStringOrDefault("ServiceUrl");
                    credentials.ChannelPartnerId = datareader.GetInt32OrDefault("ChannelPartnerId");
                    credentials.CertificateName = datareader.GetStringOrDefault("CertificateName");
                    credentials.ThumbPrint = datareader.GetStringOrDefault("ThumbPrint");
                    credentials.TellerInquiryUrl = datareader.GetStringOrDefault("TellerInquiryURL");
                    credentials.RCIFFinalCommitURL = datareader.GetStringOrDefault("RCIFFinalCommitURL");
                    credentials.EWSPreScanURL = datareader.GetStringOrDefault("EWSPreScanURL");
                    credentials.RCIFCustomerRegURL = datareader.GetStringOrDefault("RCIFCustomerRegURL");
                }
            }

            return credentials;
        }
    }
}
