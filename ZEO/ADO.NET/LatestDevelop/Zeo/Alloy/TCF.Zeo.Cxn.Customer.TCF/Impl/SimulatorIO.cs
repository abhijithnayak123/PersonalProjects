using TCF.Zeo.Cxn.Customer.TCF.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Cxn.Customer.Data;
using TCF.Zeo.Cxn.Customer.TCF.Data;
using TCF.Zeo.Common.Util;

namespace TCF.Zeo.Cxn.Customer.TCF.Impl
{
    public class SimulatorIO : IIO
    {
        public List<CustomerProfile> SearchCoreCustomers(CustomerSearchCriteria criteria, RCIFCredential credential, ZeoContext context)
        {

            return new List<CustomerProfile>() {
                new CustomerProfile()
                {
                    FirstName = "FName",
                    LastName = "Lname",
                    Address = new Address()
                    {
                        Address1 = "746 COTTAGE LN",
                        City = "BOULDER",
                        State = "CO",
                        ZipCode = "80304"
                    },
                    Phone1 = new Phone()
                    {
                        Number = Helper.GenerateRandomNumber(10),
                        Type = "Home"
                    },
                    Phone2 = new Phone(),
                    Email = "TCFZEO@YAHOO.COM",
                    SSN = criteria.SSN,
                    ClientCustomerId = Helper.GenerateRandomNumber(13),
                    Gender = Helper.Gender.MALE,
                    DateOfBirth = criteria.DateOfBirth,
                    MothersMaidenName = "CLARK",
                    IdNumber = string.Concat("d",Helper.GenerateRandomNumber(7)),
                    Occupation = "00013",
                    EmployerName = "BREMSETH LAW FIRM",
                    IDCode = Helper.GetIDCode(criteria.SSN),
                    PrimaryCountryCitizenShip = "US",
                    SecondaryCountryCitizenShip = "US",
                    IdType = "DRIVER'S LICENSE",
                    IdIssuingState = "CALIFORNIA",
                    LegalCode = "U"
                } };

        }

        public string CreateCustomer(CustomerProfile customer, RCIFCredential credential, ZeoContext context)
        {
            return Helper.GenerateRandomNumber(13);
        }

        public bool PreFlush(CustomerTransactionDetails cart, RCIFCredential credential, ZeoContext context)
        {
            return true;
        }

        public void PostFlush(CustomerTransactionDetails cart, RCIFCredential credential, ZeoContext context)
        {
        }

    }
}
