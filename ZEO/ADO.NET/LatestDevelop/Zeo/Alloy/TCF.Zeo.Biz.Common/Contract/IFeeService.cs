using TCF.Channel.Zeo.Data;
using commonData = TCF.Zeo.Common.Data;
namespace TCF.Zeo.Biz.Common.Contract
{
    public interface IFeeService
    {
        /// <summary>
        /// This method is to get fee details
        /// </summary>
        /// <param name="transactionType"></param>
        /// <param name="amount">This is check amount</param>
        /// <param name="checkType">This is check type</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>		
        /// <returns>Transaction fee details</returns>
        TransactionFee GetFee(TCF.Zeo.Common.Util.Helper.TransactionType transactionType, decimal amount, int checkType, commonData.ZeoContext context, long transactionId = 0);
    }

}
