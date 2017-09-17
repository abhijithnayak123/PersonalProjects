
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Alloy Reference

using System.ServiceModel;
using CommonData = TCF.Zeo.Common.Data;
using TCF.Channel.Zeo.Data;

#endregion

namespace TCF.Channel.Zeo.Service.Contract
{
    [ServiceContract]
    public interface IBillPayService
    {
        /// <summary>
        /// This method is used to Get Bill Pay locations
        /// </summary>
        /// <param name="billerName">This field is Biller Name</param>
        /// <param name="accountNumber">This is unique identifier for account number</param>
        /// <param name="amount">Amount</param>
        /// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Bill Pay Locations</returns>
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetLocations(long transactionId, string billerName, string accountNumber, decimal amount, ZeoContext context);

        /// <summary>
        /// This methd is used to Get Bill Pay fee
        /// </summary>
        /// <param name="billerNameOrCode">BillerName or Code</param>
        /// <param name="accountNumber">This is unique identifier for account number</param>
        /// <param name="amount">Bill Pay Amount</param>
        /// <param name="location">This field is locataion based on the fee</param>
        /// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Bill Pay Fee</returns>
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetBillPayFee(long transactionId, string billerNameOrCode, string accountNumber, decimal amount, BillerLocation location, ZeoContext context);

        /// <summary>
        /// This method is used to Get biller message
        /// </summary>
        /// <param name="billerNameOrCode">Biller Name or Code</param>
        /// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Biller Info</returns>
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetBillerInfo(string billerNameOrCode, ZeoContext context);

        /// <summary>
        /// This method is used to Get BP provider attributes
        /// </summary>
        /// <param name="billerNameOrCode">Biller Name or Code</param>
        /// <param name="location">Location Name</param>
        /// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Bill Pay Provider Attributes</returns>
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetProviderAttributes(string billerNameOrCode, string location, ZeoContext context);

        /// <summary>
        /// This method is used to Get favorite biller
        /// </summary>
        /// <param name="billerNameOrCode">Biller Name or Code</param>
        /// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Favorite Biller Details</returns>
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetBillerDetails(string billerNameOrCode, ZeoContext context);

        /// <summary>
        /// This method is used Begin BP transaction
        /// </summary>
        /// <param name="transactionID">This is unique identifier for transaction id</param>
        /// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response StageBillPayment(long transactionID, ZeoContext context);

        /// <summary>
        /// This method is used to get Validate BP request
        /// </summary>
        /// <param name="billPayment">Bill Payment</param>
        /// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Bill Pay Transaction ID</returns>
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response ValidateBillPayment(long transactionId, BillPayment billPayment, ZeoContext context);

        /// <summary>
        /// This method skeleton is added to add the "Past Billers into DB" and in the favorite billers in Bill Pay Screens for User Story # US1646.
        /// </summary>
        /// <param name="cardNumber">Card Number</param>
        /// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response AddPastBillers(string cardNumber, ZeoContext context);

        /// <summary>
        /// This method is used to Cancel the Bill Payment Transaction
        /// </summary>
        /// <param name="transactionId">This is unique identifier for Transaction ID</param>
        /// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response CancelBillPayment(long transactionId, ZeoContext context);

        /// <summary>
        /// This method is used to delete a specific biller from the Favorite Billers list
        /// </summary>
        /// <param name="billerId">This is unique identifier for Bill ID</param>
        /// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response DeleteFavoriteBiller(long billerId, ZeoContext context);

        /// <summary>
        /// This methods is used to get the List of Billers Information 
        /// </summary>
        /// <param name="searchTerm">This field is used to serarch the term of billers name</param>
        /// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>List of Billers Information</returns>
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        [OperationContract]
        Response GetBillers(string searchTerm, ZeoContext context);
        
        /// <summary>
        /// This method is used to Get frequent billers for customer
        /// </summary>
        /// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>List of Frequently used Billers</returns>
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetFrequentBillers(ZeoContext context);

        /// <summary>
		/// This method is used to Get card info
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Card Info</returns>
		[OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetCardInfo( ZeoContext context);

        /// <summary>
        /// This method to get billpay transaction 
        /// </summary>
        /// <param name="transactionId">Unique transactionId</param>
        /// <param name="context">This is common parameter to pass supplimental information</param>
        /// <returns>BillPay Transaction</returns>
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetBillPayTransaction(long transactionId, ZeoContext context);
    }
}
