using System;
using System.Collections.Generic;

using TCF.Zeo.Cxn.Customer.Data;
using TCF.Zeo.Cxn.Customer.TCF.RCIFService;
using static TCF.Zeo.Common.Util.Helper;
using System.Globalization;
using TCF.Zeo.Common.Util;

namespace TCF.Zeo.Cxn.Customer.TCF.Data
{
    internal static class RCIFMapper
    {
        internal static List<CustomerProfile> Map(List<CustInfo> rcifCustomers)
        {
            List<CustomerProfile> customers = new List<CustomerProfile>();

            foreach (CustInfo rcifCustomer in rcifCustomers)
            {
                customers.Add(Map(rcifCustomer));
            }
            return customers;
        }

        internal static CustomerProfile Map(CustInfo rcifCustomer)
        {
            CustomerProfile customer = new CustomerProfile();
            if (rcifCustomer.PersonalInfo != null)
            {
                customer.FirstName = rcifCustomer.PersonalInfo.FName;
                customer.MiddleName = rcifCustomer.PersonalInfo.MName;
                customer.LastName = rcifCustomer.PersonalInfo.LName;
                customer.Address = new Address() { Address1 = rcifCustomer.PersonalInfo.Addr1, Address2 = rcifCustomer.PersonalInfo.Addr2, City = rcifCustomer.PersonalInfo.City, State = rcifCustomer.PersonalInfo.State, ZipCode = rcifCustomer.PersonalInfo.zip };
                //Do Not pass phone providers
                customer.Phone1 = new Phone() { Number = rcifCustomer.PersonalInfo.Ph1, Type = rcifCustomer.PersonalInfo.Ph1Type1 != null ? MapToAlloyPhoneTypes(rcifCustomer.PersonalInfo.Ph1Type1) : null };
                customer.Phone2 = new Phone() { Number = rcifCustomer.PersonalInfo.Ph2, Type = rcifCustomer.PersonalInfo.Ph2Type2 != null ? MapToAlloyPhoneTypes(rcifCustomer.PersonalInfo.Ph2Type2) : null };
                customer.Email = rcifCustomer.PersonalInfo.email;

                customer.IDCode = Helper.GetIDCode(rcifCustomer.PersonalInfo.ssn);

                customer.SSN = rcifCustomer.PersonalInfo.ssn;
                customer.ClientCustomerId = rcifCustomer.PersonalInfo.ClientCustId;
                customer.Gender = Helper.GetGender(rcifCustomer.PersonalInfo.gender);

            }
            if (rcifCustomer.Identification != null)
            {
                customer.MothersMaidenName = rcifCustomer.Identification.maiden;
                customer.IdType = MapToAlloyIdType(rcifCustomer.Identification.idType);
                if (rcifCustomer.Identification.idType == "D" || rcifCustomer.Identification.idType == "S")
                {
                    customer.IdIssuingState = rcifCustomer.Identification.idIssuer;
                }
                else
                {
                    customer.IdIssuingState = null;
                }
                customer.IdIssuingCountry = rcifCustomer.Identification.idIssuerCountry != null ? rcifCustomer.Identification.idIssuerCountry.Trim() : null;
                customer.IdNumber = rcifCustomer.Identification.idNbr;
                customer.LegalCode = rcifCustomer.Identification.legalCode;
                customer.PrimaryCountryCitizenShip = rcifCustomer.Identification.citizenshipCountry1;
                customer.SecondaryCountryCitizenShip = rcifCustomer.Identification.citizenshipCountry2;
                if (rcifCustomer.Identification.dob != null && Convert.ToInt32(rcifCustomer.Identification.dob) > 9999999)
                {
                    customer.DateOfBirth = DateTime.ParseExact(rcifCustomer.Identification.dob, "yyyyMMdd", CultureInfo.InvariantCulture);
                }
                if (rcifCustomer.Identification.idIssueDate != null && Convert.ToInt32(rcifCustomer.Identification.idIssueDate) != 0 && Convert.ToInt32(rcifCustomer.Identification.idIssueDate) > 9999999)
                {
                    customer.IdIssueDate = DateTime.ParseExact(rcifCustomer.Identification.idIssueDate, "yyyyMMdd", CultureInfo.InvariantCulture);
                }
                else
                {
                    customer.IdIssueDate = null;
                }
                if (rcifCustomer.Identification.idExpDate != null && Convert.ToInt32(rcifCustomer.Identification.idExpDate) != 0 && Convert.ToInt32(rcifCustomer.Identification.idExpDate) > 9999999)
                {
                    customer.IdExpirationDate = DateTime.ParseExact(rcifCustomer.Identification.idExpDate, "yyyyMMdd", CultureInfo.InvariantCulture);
                }
                else
                {
                    customer.IdExpirationDate = null;
                }
            }
            if (rcifCustomer.EmploymentInfo != null)
            {
                customer.Occupation = !string.IsNullOrEmpty(rcifCustomer.EmploymentInfo.Occupation) ? rcifCustomer.EmploymentInfo.Occupation : string.Empty;
                customer.OccupationDescription = rcifCustomer.EmploymentInfo.OccDesc;
                customer.EmployerName = rcifCustomer.EmploymentInfo.EmployerName;
                customer.EmployerPhone = rcifCustomer.EmploymentInfo.EmployerPhoneNum;
            }
            return customer;
        }

        private static string MapToAlloyPhoneTypes(string mapPhoneType)
        {
            string idType = string.Empty;
            Dictionary<string, string> phoneTypes = new Dictionary<string, string>()
            {
                {"H", "Home"},
                {"W", "Work"},
                {"M", "Cell"},
                {"O", "Other"},
            };

            if (phoneTypes.ContainsKey(mapPhoneType))
            {
                idType = phoneTypes[mapPhoneType];
            }
            return idType;
        }

        private static string MapToClientPhoneTypes(string mapPhoneType)
        {
            string idType = string.Empty;
            Dictionary<string, string> phoneTypes = new Dictionary<string, string>()
            {
                {"Home", "H"},
                {"Work", "W"},
                {"Cell", "M"},
                {"Other", "O"},
            };

            if (phoneTypes.ContainsKey(mapPhoneType))
            {
                idType = phoneTypes[mapPhoneType];
            }
            return idType;
        }

        private static string MapToAlloyIdType(string clientIdType)
        {
            string idType = string.Empty;
            Dictionary<string, string> idMappying = new Dictionary<string, string>()
            {
                {"D", "DRIVER'S LICENSE"},
                {"U", "MILITARY ID"},
                {"P", "PASSPORT"},
                {"S", "U.S. STATE IDENTITY CARD"},
                {"M", "MATRICULA CONSULAR"}
            };

            if (idMappying.ContainsKey(clientIdType))
            {
                idType = idMappying[clientIdType];
            }
            return idType;
        }

    }
}
