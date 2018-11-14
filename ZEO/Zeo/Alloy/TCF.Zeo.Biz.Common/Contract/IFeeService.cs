using System.Collections.Generic;
using TCF.Channel.Zeo.Data;
using commonData = TCF.Zeo.Common.Data;
using static TCF.Zeo.Common.Util.Helper;
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
        List<TransactionFee> GetFee(TCF.Zeo.Common.Util.Helper.TransactionType transactionType, decimal amount, int checkType, commonData.ZeoContext context, long transactionId = 0);


        /// <summary>
        /// This method is to get fee details
        /// </summary>
        /// <param name="transactionType"></param>
        /// <param name="amount">This is check amount</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>		
        /// <returns>Transaction fee details</returns>
        long ValidateProviderPromotion(Zeo.Common.Util.Helper.TransactionType transactionType, decimal amount, commonData.ZeoContext context);

        /// <summary>
        /// This method is to get fee details
        /// </summary>
        /// <param name="transactionType"></param>
        /// <param name="amount">This is check amount</param>
        /// <param name="checkType">This is check type</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>		
        /// <returns>Transaction fee details</returns>
        TransactionFee ReCalculateFee(TransactionType transactionType, decimal amount, int checkType, commonData.ZeoContext context, long transactionId = 0);

        /// <summary>
        /// Create or update transaction fee adjustments
        /// </summary>
        /// <param name="transactionType">product type</param>
        /// <param name="transactionId">transaction Id</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>		
        /// <returns></returns>
        bool CreateOrUpdateTransactionFeeAdjustment(long promotionId, TCF.Zeo.Common.Util.Helper.TransactionType transactionType, long transactionId, commonData.ZeoContext context, long provisionId = 0);

    }

}
