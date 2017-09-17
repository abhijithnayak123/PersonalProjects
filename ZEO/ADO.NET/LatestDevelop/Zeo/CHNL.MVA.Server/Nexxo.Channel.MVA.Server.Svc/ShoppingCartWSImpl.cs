using MGI.Channel.MVA.Server.Contract;
using SharedData = MGI.Channel.Shared.Server.Data;
namespace MGI.Channel.MVA.Server.Svc
{
    public partial class MVAWSImpl : IShoppingCartService
    {
        #region IShoppingCartService Impl

        public SharedData.Receipts Checkout(long customerSessionId)
        {
            return MVAEngine.Checkout(customerSessionId);
        }

        public void RemoveMoneyTransfer(long customerSessionId, long moneyTransferId)
        {
            MVAEngine.RemoveMoneyTransfer(customerSessionId, moneyTransferId);
        }

        public void RemoveBillPay(long customerSessionId, long billPayId)
        {
            MVAEngine.RemoveBillPay(customerSessionId, billPayId );
        }

        #endregion
    }
}