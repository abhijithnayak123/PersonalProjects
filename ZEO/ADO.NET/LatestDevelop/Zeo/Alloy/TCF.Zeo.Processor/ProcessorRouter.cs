using Chexar = TCF.Zeo.Cxn.Check.Chexar.Impl;
using TCF.Zeo.Cxn.Check.Contract;
using TCF.Zeo.Cxn.BillPay.Contract;
using TCF.Zeo.Cxn.Customer.Contract;
using TCFCustomer = TCF.Zeo.Cxn.Customer.TCF.Impl;
using static TCF.Zeo.Common.Util.Helper;
using CxnMTService = TCF.Zeo.Cxn.MoneyTransfer.WU.Impl;
using CxnMT = TCF.Zeo.Cxn.MoneyTransfer;
using WUGateway = TCF.Zeo.Cxn.BillPay.WU;
using TCF.Zeo.Cxn.Fund.Contract;
using visaProvider = TCF.Zeo.Cxn.Fund.Visa.Impl;

namespace TCF.Zeo.Processor
{
    public class ProcessorRouter
    {
        public IClientCustomerService GetCXNCustomerProcessor(string channelpartnerName)
        {
            switch (channelpartnerName.ToLower())
            {
                //case "synovus":
                //case "carver":
                case "tcf":
                    return new TCFCustomer.Gateway();
                default:
                    break;
            }
            return null;
        }

        public ICheckProcessor GetCXNCheckProcessor(string channelPartnerName)
        {
            switch (channelPartnerName.ToLower())
            {
                case "synovus":
                case "carver":
                case "tcf":
                    return new Chexar.ChexarGateway();
                default:
                    break;
            }
            return null;
        }

        public IBillPayProcessor GetBillPayCXNProcessor(string channelPartnerName)
        {
            switch (channelPartnerName.ToLower())
            {
                case "synovus":
                case "carver":
                case "tcf":
                    return new WUGateway.Impl.WesternUnionGateway();
                default:
                    break;
            }
            return null;
        }

        public ProviderId GetCheckProvider(string channelPartnerName)
        {
            switch (channelPartnerName.ToLower())
            {
                case "synovus":
                case "tcf":
                case "carver":
                    return ProviderId.Ingo;
                case "mgi":
                    return ProviderId.Certegy;
                default:
                    return ProviderId.Ingo;
            }
        }
        public CxnMT.Contract.IMoneyTransferService GetCXNMoneyTransferProcessor(string channelPartnerName)
        {
            switch (channelPartnerName.ToLower())
            {
                case "synovus":
                case "carver":
                case "tcf":
                    return new CxnMTService.WUGateway();
                default:
                    break;
            }
            return null;
        }

        public ProviderId GetBillPayProvider(string channelPartnerName)
        {
            switch (channelPartnerName.ToLower())
            {
                case "synovus":
                case "tcf":
                case "carver":
                    return ProviderId.WesternUnionBillPay;
                case "mgi":
                    return ProviderId.MoneyGramBillPay;
                default:
                    return ProviderId.WesternUnionBillPay;

            }
        }

        public IFundProcessor GetFundProvider(string channelPartnerName)
        {
            switch (channelPartnerName.ToLower())
            {
                case "synovus":
                case "tcf":
                    return new visaProvider.Gateway();
                default:
                    return new visaProvider.Gateway();
            }
        }

        public ProviderId GetFundProviders(string channelPartnerName)
        {
            switch (channelPartnerName.ToLower())
            {
                case "synovus":
                case "tcf":
                     return ProviderId.Visa;
                default:
                    return ProviderId.Visa;
            }
        }

        public int GetMoneyTransferProvider(string channelPartnerName)
        {
            switch (channelPartnerName.ToLower())
            {
                case "synovus":
                case "tcf":
                case "carver":
                    return (int)ProviderId.WesternUnion;
                case "mgi":
                    return (int)ProviderId.MoneyGram;
                default:
                    return (int)ProviderId.WesternUnion;

            }
        }
    }
}
