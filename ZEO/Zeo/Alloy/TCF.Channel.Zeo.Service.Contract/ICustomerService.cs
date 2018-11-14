
using System.Collections.Generic;
using System.ServiceModel;


#region Zeo References
using TCF.Channel.Zeo.Data;
using TCF.Zeo.Common.Data;
#endregion

namespace TCF.Channel.Zeo.Service.Contract
{
    [ServiceContract]
    public interface ICustomerService
    {       
        /// <summary>
        /// This method is used to validate the SSN.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract]
        Response ValidateSSN(Data.ZeoContext context);

        /// <summary>
        /// This method is used to insert the customer.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract]
        Response InsertCustomer(CustomerProfile customer, Data.ZeoContext context);

        /// <summary>
        /// This method is used to update the customer.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract]
        Response UpdateCustomer(CustomerProfile customer, Data.ZeoContext context);


        /// <summary>
        /// This method is create customer at client side.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract]
        Response RegisterToClient(Data.ZeoContext context);

        /// <summary>
        /// This method is used to get customer by customer Id.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract]
        Response GetCustomer(Data.ZeoContext context);

        /// <summary>
        /// This method is used to sync in the customer details with TCF.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract]
        Response CustomerSyncInFromClient(Data.ZeoContext context);

        /// <summary>
        /// This method is used to initialize the customer session.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract]
        Response InitiateCustomerSession(int cardSearchType, Data.ZeoContext context);

        /// <summary>
        /// This method is updates customer at client side.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract]
        Response UpdateCustomerToClient(Data.ZeoContext context);

        /// <summary>
        /// This method is used to validate all required fields of core customer.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract]
        Response ValidateCustomer(CustomerProfile customer, Data.ZeoContext context);

        /// <summary>
        /// This method is used to whether the agent has a permission to close the customer.
        /// </summary>
        /// <param name="profileStatus"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract]
        Response CanChangeProfileStatus(string profileStatus, Data.ZeoContext context);

        /// <summary>
        /// This method is used to search the customers either by card number or customer details.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract]
        Response SearchCustomers(CustomerSearchCriteria criteria, Data.ZeoContext context);

        /// <summary>
        /// To get the customer details by card Number
        /// </summary>
        /// <param name="cardnumber"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract]
        Response IsCardValidForCustomer(string cardnumber, Data.ZeoContext context);

    }
}
