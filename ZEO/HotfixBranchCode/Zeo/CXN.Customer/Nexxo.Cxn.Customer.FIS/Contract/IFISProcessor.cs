using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using MGI.Cxn.Customer.FIS.Data; 

namespace MGI.Cxn.Customer.FIS.Contract
{
    public interface IFISProcessor
    {
        //Used to retrieve RACF credentials by Bank ID.
       // AppInfoResponse  GetAppKey(GetAppKeyInfoRequest credentials);
        //Used to retrieve customer profile elements for an existing Synovus customer based on SSN (search limited within RACF / Bank ID)
        CustTaxNbrSrchRes SearchCustomerBySSN(CustTaxNbrSrchReq custdetails);
        //Used to create a new customer in FIS and add their profile info (will return the FIS customer ID).
        CustOpenIndvCustRes CreateFISCustomer(CustOpenIndvCustReq custdetails);
        //Used to update Name, phone, address on FIS customer record
        CustNameAddrMaintRes UpdateCustomerProfile(CustNameAddrMaintReq custprofile);
        //Used to associate  account relationship in FIS for Connections and for Connects customers with their FIS customer ID
        CustOpenMiscAcctReq CreateMiscAccount(CustOpenMiscAcctReq cusmiscdtls);
        //Nexxo created service to retrieve Connections card customer info to prepopulate in Nexxo customer tables (references data expected)
        ConnectsDBTaxNbrSrchRes GetCustomerProfile(ConnectsDBTaxNbrSrchReq custProfile); 
    }
}
