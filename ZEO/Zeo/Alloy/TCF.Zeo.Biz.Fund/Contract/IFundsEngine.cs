using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Util;
using TCF.Channel.Zeo.Data;
using commonData = TCF.Zeo.Common.Data;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Zeo.Biz.Fund.Contract
{
    public interface IFundsEngine
    {

        /// <summary>
        /// Used for adding a GPR account to the customer in CXE, PTNR and CXN layer
        /// </summary>
        /// <param name="fundsAccount">Customer profile information</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Unique Id associated with the account</returns>
        long Add(FundsAccount fundsAccount, commonData.ZeoContext context);

        /// <summary>
        /// Validates the request and stages the withdraw transaction
        /// </summary>
        /// <param name="funds">Contains information related to fund transaction</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Transaction Id used for making withdraw transaction</returns>
        long Withdraw(Funds funds, commonData.ZeoContext context);

        /// <summary>
        /// Validates the request and stages the load transaction
        /// </summary>
        /// <param name="funds">Contains information related to fund transaction</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Transaction Id used for making load transaction</returns>
        long Load(Funds funds, commonData.ZeoContext context);

        /// <summary>
        /// Commit the Withdraw Or Load transaction.
        /// </summary>
        /// <param name="cxeTrxId">Unique Id associated with fund transaction</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <param name="cardNumber">GPR card has a unique number used for transactions</param>
        /// <returns>Provides the state of transaction</returns>
        int Commit(long cxeTrxId, commonData.ZeoContext context);

        /// <summary>
        /// A transaction staged for activation
        /// </summary>
        /// <param name="funds">Contains information related to fund transaction</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Transaction Id used for making card activation</returns>
        long Activate(Funds funds, commonData.ZeoContext context);

        /// <summary>
        /// Get the balance amount on the card.
        /// </summary>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Card Balance</returns>
        CardBalanceInfo GetBalance(commonData.ZeoContext context);

        /// <summary>
        /// Used for fetching customer profile information.
        /// </summary>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Customer profile information</returns>
        FundsAccount GetAccount(commonData.ZeoContext context);

        /// <summary>
        /// Gets transaction and customer details
        /// </summary>
        /// <param name="Id">Unique Id associated with the transaction</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Transaction and account details</returns>
        Funds Get(long Id, bool isEditTransaction, commonData.ZeoContext context);

        /// <summary>
        /// Get Fee for a particular type of funds transaction
        /// </summary>
        /// <param name="amount">Load/Withdraw amount in fund transaction</param>
        /// <param name="fundsType">Type of fund transaction</param> 
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Partner-configured fee object for given fundsType</returns>
        TransactionFee GetFee(decimal amount, Helper.FundType fundsType, commonData.ZeoContext context);

        /// <summary>
        /// Update the transaction amount for the funds transaction
        /// </summary>
        /// <param name="cxeFundTrxId">Unique Id associated with fund transaction</param>
        /// <param name="amount">Updated amount for the fund transaction</param>
        /// <param name="fundType">Type of fund transaction</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Unique transactionId associated with each transaction</returns>
        long UpdateAmount(long cxnFundTrxId, decimal amount, FundType fundType, string gprPromoCode, commonData.ZeoContext context);

        /// <summary>
        /// Get the minimum load amount based on load type ie initial or successive load.
        /// </summary>
        /// <param name="initialLoad">Flag to determine the load is during activation or transaction</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Minimum amount required for a particular load transaction</returns>
        decimal GetMinimumLoadAmount(bool initialLoad, commonData.ZeoContext context);

        /// <summary>
        /// Deletes the card account associated with the customer
        /// </summary>
        /// <param name="fundsId">Unique Id associated with the card account</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
		void Cancel(long fundsId, bool hasFundsAccount, commonData.ZeoContext context);

        /// <summary>
        /// Getting transaction history of visa card transactions
        /// </summary>
        /// <param name="request">Transaction history selection type</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>All trnsactions</returns>
        List<VisaTransactionHistory> GetTransactionHistory(TransactionHistoryRequest request, commonData.ZeoContext context);

        /// <summary>
        ///  Closing existing account(card)
        /// </summary>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns></returns>
        bool CloseAccount(commonData.ZeoContext context);

        /// <summary>
        /// updating the card status
        /// </summary>
        /// <param name="cardMaintenanceInfo">shipping type and updated card status</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns></returns>
        bool UpdateCardStatus(CardMaintenanceInfo cardMaintenanceInfo, commonData.ZeoContext context);

        /// <summary>
        /// Replace the existing card
        /// </summary>
        /// <param name="cardMaintenanceInfo">shipping type and updated card status</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns></returns>
        bool ReplaceCard(CardMaintenanceInfo cardMaintenanceInfo, commonData.ZeoContext context);


        /// <summary>
        /// Get ChannelPartner Shipping Type for Visa Card
        /// </summary>			
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns></returns>
        List<ShippingTypes> GetShippingTypes(commonData.ZeoContext context);

        /// <summary>
        /// Get Shipping Type Fee
        /// </summary>
        /// <param name="cardMaintenanceInfo"></param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns></returns>
        double GetShippingFee(CardMaintenanceInfo cardMaintenanceInfo, commonData.ZeoContext context);

        /// <summary>
        /// Associating the card with Alloy Customer
        /// </summary>
        /// <param name="fundsAccount">Customer profile information</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Unique Id associated with the account</returns>
        long AssociateCard(FundsAccount fundsAccount, commonData.ZeoContext context);

        /// <summary>
        /// This method is to get the Visa Fee
        /// </summary>
        /// <param name="cardMaintenanceInfo">Card maintenance details</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns></returns>
        double GetFundFee(CardMaintenanceInfo cardMaintenanceInfo, commonData.ZeoContext context);


        /// <summary>
        /// A transaction staged for AddOnCard order
        /// </summary>
        /// <param name="funds">Contains information related to fund transaction</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Transaction Id used for making card activation</returns>
        long IssueAddOnCard(Funds funds, commonData.ZeoContext context);

        /// <summary>
        /// This method is to get the card number
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <returns></returns>
        string GetPrepaidCardNumber(commonData.ZeoContext context);


        long UpdateAccount(FundsAccount fundsAccount, commonData.ZeoContext context);
    }
}
