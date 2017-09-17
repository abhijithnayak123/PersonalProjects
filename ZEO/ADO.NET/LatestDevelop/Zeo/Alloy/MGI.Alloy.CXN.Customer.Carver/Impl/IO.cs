using System.Collections.Generic;

using MGI.Alloy.CXN.Customer.Data;
using System;
using P3Net.Data.Common;
using P3Net.Data.Sql;
using P3Net.Data;
using System.Data;
using MGI.Alloy.Common.Data;
using MGI.Alloy.Common.Util;

namespace MGI.Alloy.CXN.Customer.Carver.Impl
{
    internal class IO
    {
        internal List<CustomerProfile> SearchCoreCustomers(CustomerSearchCriteria searchCriteria)
        {
            StoredProcedure coreCustomerProcedure = new StoredProcedure("usp_getCarverCoreCustomers");

            coreCustomerProcedure.WithParameters(InputParameter.Named("DateOfBirth").WithValue(searchCriteria.DateOfBirth));
            coreCustomerProcedure.WithParameters(InputParameter.Named("PhoneNumber").WithValue(searchCriteria.Phonenumber));
            coreCustomerProcedure.WithParameters(InputParameter.Named("ZipCode").WithValue(searchCriteria.Zipcode));
            coreCustomerProcedure.WithParameters(InputParameter.Named("LastName").WithValue(searchCriteria.Lastname));

            IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(coreCustomerProcedure);

            List<CustomerProfile> customers = new List<CustomerProfile>();
            CustomerProfile customer;
            while (datareader.Read())
            {
                customer = new CustomerProfile();
                customer.Phone1 = new Phone() { Number = datareader.GetStringOrDefault("PrimaryPhoneNumber") };
                customer.Phone2 = new Phone() { Number = datareader.GetStringOrDefault("SecondaryPhone") };
                customer.SSN = datareader.GetStringOrDefault("CustomerTaxNumber");
                customer.IDCode = Helper.GetIDCode(customer.SSN);
                customer.LastName = datareader.GetStringOrDefault("LastName");
                customer.FirstName = datareader.GetStringOrDefault("FirstName");
                customer.MiddleName = datareader.GetStringOrDefault("MiddleName");
                customer.Address = new Address() { Address1 = datareader.GetStringOrDefault("AddressStreet"), City = datareader.GetStringOrDefault("AddressCity"), State = datareader.GetStringOrDefault("AddressState"), ZipCode = datareader.GetStringOrDefault("ZipCode") };
                customer.DateOfBirth = datareader.GetDateTimeOrDefault("DOB");
                customer.MothersMaidenName = datareader.GetStringOrDefault("MothersMaidenName");
                customer.Gender = Helper.GetGender(datareader.GetStringOrDefault("Gender"));
                customer.IdNumber = datareader.GetStringOrDefault("DriversLicenseNumber");
                customer.CustomerBankId = datareader.GetStringOrDefault("MetBankNumber");
                customer.ClientCustomerId = datareader.GetStringOrDefault("CustomerNumber");
                customer.ProgramId = datareader.GetStringOrDefault("ProgramId");
                customers.Add(customer);
            }
            return customers;
        }

    }
}
