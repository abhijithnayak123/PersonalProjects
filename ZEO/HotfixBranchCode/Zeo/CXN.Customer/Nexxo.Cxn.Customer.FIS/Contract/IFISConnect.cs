using FISConnectData = MGI.Cxn.Customer.FIS.Data;
using System.Collections.Generic;

namespace MGI.Cxn.Customer.FIS.Contract
{
    /// <summary>
	/// Added this new interface to implement the SQL Injection. US#1789
    /// </summary>
    public interface IFISConnect
    {
        FISConnectData.FISConnect GetSSNForCustomer(string SSN);
        List<FISConnectData.FISConnect> FISConnectCustomerLookUp(Dictionary<string, object> customerLookUpCriteria);
    }
}
