using System;
using System.Collections.Generic;
using MGI.Channel.Shared.Server.Contract;
using MGI.Common.Util;

namespace MGI.Channel.Shared.Server.Impl
{
    public partial class SharedEngine : ISharedService
    {
        #region Injected Services
        private ISharedService Self;
        #endregion

        public SharedEngine()
        {
            CustomerConverter();
            MoneyTransferConverter();
            FundEngineConverter();
            BillPayConverter();
            MoneyOrderConverter();
            CheckCashingConverter();
            ChannelPartnerConverter();
            ShoppingCartConverter();
            TransactionEngineConverter();
            SetSelf(this);
        }

        #region IConsumerService Impl

        public void SetSelf(ISharedService dts)
        {
            Self = dts;
        }

        #endregion

    }
}
