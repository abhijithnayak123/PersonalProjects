using MGI.Alloy.Common.Util;
using MGI.Alloy.CXN.Customer.Data;
using MGI.Alloy.CXN.Customer.FIS.Impl.FISService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Alloy.CXN.Customer.FIS.Data
{
    internal static class FISMapper
    {
        internal static List<CustomerProfile> Map(List<CICustTaxNbrSrchResDataCICustInfo> fiscustomers)
        {
            List<CustomerProfile> customers = new List<CustomerProfile>();
            foreach (CICustTaxNbrSrchResDataCICustInfo fiscustomerinfo in fiscustomers)
            {
                customers.Add(Map(fiscustomerinfo));
            }
            return customers;
        }

        internal static CustomerProfile Map(CICustTaxNbrSrchResDataCICustInfo fiscustomerinfo)
        {
            CustomerProfile customer = new CustomerProfile();

            customer.ClientCustomerId = fiscustomerinfo.E10033;
            customer.DateOfBirth = Convert.ToDateTime(fiscustomerinfo.E10036);
            customer.FirstName = fiscustomerinfo.E10102;
            customer.LastName = fiscustomerinfo.E10101;
            customer.MiddleName = fiscustomerinfo.E10103;
            customer.SSN = fiscustomerinfo.E10132;
            customer.IDCode = Helper.GetIDCode(fiscustomerinfo.E10134);
            customer.Gender = Helper.GetGender(fiscustomerinfo.E10153);

            customer.Phone1 = null;

            if (!string.IsNullOrEmpty(fiscustomerinfo.E10109))
            {
                customer.Phone1 = new Phone();
                customer.Phone1.Number = fiscustomerinfo.E10109;
                customer.Phone1.Type = "Home";
            }
            else if (!string.IsNullOrEmpty(fiscustomerinfo.E10113))
            {
                customer.Phone1 = new Phone();
                customer.Phone1.Number = fiscustomerinfo.E10113;
                customer.Phone1.Type = "Work";
            }
            else if (!string.IsNullOrEmpty(fiscustomerinfo.E10097))
            {
                customer.Phone1 = new Phone();
                customer.Phone1.Number = fiscustomerinfo.E10097;
                customer.Phone1.Type = "Other";
            }

            customer.Address = new Address();
            customer.Address.Address1 = fiscustomerinfo.E10042;
            customer.Address.City = fiscustomerinfo.E10094;
            customer.Address.State = fiscustomerinfo.E10114;
            customer.Address.ZipCode = fiscustomerinfo.E10122.Left(5);

            return customer;
        }
    }
}
