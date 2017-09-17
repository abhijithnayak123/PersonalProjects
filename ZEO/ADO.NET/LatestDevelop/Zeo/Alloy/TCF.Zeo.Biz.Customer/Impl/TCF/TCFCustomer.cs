#region Using References
using System.Collections.Generic;
using System.Linq;

#region Zeo References
using TCF.Zeo.Biz.Customer.Contract;
using TCF.Zeo.Processor;
using TCF.Channel.Zeo.Data;
using CXNDATA = TCF.Zeo.Cxn.Customer.Data;
using COREDATA = TCF.Zeo.Core.Data;
using commonData = TCF.Zeo.Common.Data;
using CORECONTRACT = TCF.Zeo.Core.Contract;
using CoreImpl = TCF.Zeo.Core.Impl;
using TCF.Zeo.Cxn.Customer.Contract;
using static TCF.Zeo.Common.Util.Helper;
using CXNFundsException = TCF.Zeo.Cxn.Fund.Data.Exceptions;
#endregion

#region External References
using AutoMapper;
using System;
using System.Text.RegularExpressions;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Biz.Common.Data.Exceptions;
using System.IO;
using System.Data;
using TCF.Zeo.Cxn.Fund.Contract;
using TCF.Zeo.Common.DataProtection.Contract;
using System.Configuration;
using TCF.Zeo.Common.DataProtection.Impl;
using TCF.Zeo.Cxn.Fund.Data;
#endregion
#endregion

namespace TCF.Zeo.Biz.Customer.Impl.TCF
{
    public class TCFCustomer : ICustomerRepository
    {
        IMapper Mapper;
        IClientCustomerService ClientCustomerService;
        ProcessorRouter ProcessorRouter;
        CORECONTRACT.ICustomerService CoreCustomerService;
        CORECONTRACT.IDataStructuresService dataStructuresService;

        private const int FirstNameMaxLength = 20;
        private const int LastNameMaxLength = 20;
        private const int MiddleNameMaxLength = 15;
        private const int FullNameLength = 40;

        public TCFCustomer()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CXNDATA.CustomerProfile, CustomerProfile>();
                cfg.CreateMap<CustomerProfile, CXNDATA.CustomerProfile>();
                cfg.CreateMap<COREDATA.CustomerProfile, CXNDATA.CustomerProfile>();
                cfg.CreateMap<COREDATA.Phone, CXNDATA.Phone>();
                cfg.CreateMap<COREDATA.Address, CXNDATA.Address>();
                cfg.CreateMap<CXNDATA.CustomerProfile, COREDATA.CustomerProfile>();
                cfg.CreateMap<CXNDATA.Phone, Phone>();
                cfg.CreateMap<Phone, CXNDATA.Phone>();
                cfg.CreateMap<CXNDATA.Address, Address>();
                cfg.CreateMap<Address, CXNDATA.Address>();
                cfg.CreateMap<CustomerSearchCriteria, CXNDATA.CustomerSearchCriteria>();
                cfg.CreateMap<CXNDATA.Phone, COREDATA.Phone>();
                cfg.CreateMap<CXNDATA.Address, COREDATA.Address>();
                cfg.CreateMap<CustomerSearchCriteria, COREDATA.CustomerSearchCriteria>();
                cfg.CreateMap<COREDATA.CustomerProfile, CustomerProfile>();
                cfg.CreateMap<COREDATA.Phone, Phone>();
                cfg.CreateMap<COREDATA.Address, Address>();
            });

            Mapper = config.CreateMapper();
        }

        #region Public methods

        /// <summary>
        /// This method is used to search the customers at the provider level.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        List<CustomerProfile> ICustomerRepository.SearchCoreCustomers(CustomerSearchCriteria criteria, commonData.ZeoContext context)
        {
            try
            {
                ProcessorRouter = new ProcessorRouter();

                ClientCustomerService = ProcessorRouter.GetCXNCustomerProcessor(criteria.ChannelPartnerName);
                CXNDATA.CustomerSearchCriteria cxnCriteria = Mapper.Map<CustomerSearchCriteria, CXNDATA.CustomerSearchCriteria>(criteria);

                List<CXNDATA.CustomerProfile> coreCustomerProfiles = ClientCustomerService.SearchCoreCustomers(cxnCriteria, context);
                List<CustomerProfile> customerProfiles = Mapper.Map<List<CXNDATA.CustomerProfile>, List<CustomerProfile>>(coreCustomerProfiles);

                return customerProfiles;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.CUSTOMER_SEARCH_FAILED, ex);
            }
        }

        /// <summary>
        /// This method is used to search the customer from the card number.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public List<CustomerProfile> SearchCardCustomer(CustomerSearchCriteria criteria, commonData.ZeoContext context)
        {
            try
            {
                ProcessorRouter = new ProcessorRouter();
                ClientCustomerService = ProcessorRouter.GetCXNCustomerProcessor(context.ChannelPartnerName);

                List<CXNDATA.CustomerProfile> coreCustomerProfiles = ClientCustomerService.SearchCustomerWithCardNumber(criteria.CardNumber, context);
                List<CustomerProfile> customerProfiles = Mapper.Map<List<CXNDATA.CustomerProfile>, List<CustomerProfile>>(coreCustomerProfiles);

                return customerProfiles;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.CUSTOMER_SEARCH_FAILED, ex);
            }
        }

        /// <summary>
        /// This method is used to push the customer data to client.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public long RegisterToClient(commonData.ZeoContext context)
        {
            ICustomerCommonService svc = new CustomerService();
            CustomerProfile profile = svc.GetCustomer(context);  // this is core customer

            context.SSN = profile.SSN;
            ProcessorRouter = new ProcessorRouter();

            CXNDATA.CustomerProfile cxnCustomer = Mapper.Map<CXNDATA.CustomerProfile>(profile);

            //cxnCustomer.EmployerName = profile.EmployerName;
            CoretoCxnMapper(context, profile, cxnCustomer);
            addSSOAttributes(context);

            long cxnAccountId;

            //If the customer details are not populated using Customer Look up
            ClientCustomerService = ProcessorRouter.GetCXNCustomerProcessor(context.ChannelPartnerName);

            //Insert into TCIS
            cxnAccountId = ClientCustomerService.AddCXNAccount(cxnCustomer, context);

            try
            {
                context.CXNAccountId = cxnAccountId;
                string clientId = ClientCustomerService.Add(cxnCustomer, context);
                svc.UpdateCustomerRegistrationStatus(ProfileStatus.Active, clientId, string.Empty, true, context);
                return cxnAccountId;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.CUSTOMER_REGISTRATION_FAILED, ex);
            }
        }

        /// <summary>
        /// This method will update the customer data to client.
        /// </summary>
        /// <param name="context"></param>
        public void UpdateCustomerToClient(commonData.ZeoContext context)
        {
            ProcessorRouter = new ProcessorRouter();

            ICustomerCommonService svc = new CustomerService();
            CustomerProfile profile = svc.GetCustomer(context); // This is core customer
            ClientCustomerService = ProcessorRouter.GetCXNCustomerProcessor(context.ChannelPartnerName);

            context.SSN = profile.SSN;

            CXNDATA.CustomerProfile tcfAccount = ClientCustomerService.GetAccountByCustomerId(context);

            CXNDATA.CustomerProfile customer = Mapper.Map<CXNDATA.CustomerProfile>(profile);

            long cxnAccountId = 0;

            if (tcfAccount != null)
            {
                cxnAccountId = Convert.ToInt64(tcfAccount.Id);

                ProfileStatus cxnProfileStatus = tcfAccount.ProfileStatus;

                if (cxnProfileStatus == ProfileStatus.Inactive && profile.ProfileStatus != ProfileStatus.Closed)
                {
                    try
                    {
                        CoretoCxnMapper(context, profile, customer);
                        addSSOAttributes(context);
                        //Insert into TCIS
                        context.CXNAccountId = cxnAccountId;
                        string clientId = ClientCustomerService.Add(customer, context);

                        svc.UpdateCustomerRegistrationStatus(ProfileStatus.Active, clientId, string.Empty, true, context);
                    }
                    catch (Exception ex)
                    {
                        if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                        throw new CustomerException(CustomerException.CUSTOMER_UPDATE_FAILED, ex);
                    }
                }
            }
            else
            {
                RegisterToClient(context);
            }
        }

        /// <summary>
        /// This method is used to sync in the customer details with TCF.
        /// </summary>
        /// <param name="context"></param>
        public void CustomerSyncInFromClient(commonData.ZeoContext context)
        {
            try
            {
                ProcessorRouter = new ProcessorRouter();

                CXNDATA.CustomerProfile cxnTCISCustomerProfile = null;
                ClientCustomerService = ProcessorRouter.GetCXNCustomerProcessor(context.ChannelPartnerName);

                //Check is it a right way to use Core here. Else we will end up having it in 2 places - Gateway and Core.
                COREDATA.CustomerProfile customerProfile = new COREDATA.CustomerProfile();

                using (CoreCustomerService = new CoreImpl.ZeoCoreImpl())
                {
                    customerProfile = CoreCustomerService.GetCustomer(context);
                }

                CXNDATA.CustomerProfile cxnCustomerProfile = Mapper.Map<COREDATA.CustomerProfile, CXNDATA.CustomerProfile>(customerProfile);

                //RCIF customer details.
                cxnTCISCustomerProfile = ClientCustomerService.CustomerSyncInFromClient(cxnCustomerProfile, context);

                if (cxnTCISCustomerProfile != null)
                {
                    customerProfile = MapToCxeCustomer(customerProfile, cxnTCISCustomerProfile, context);

                    using (CoreCustomerService = new CoreImpl.ZeoCoreImpl())
                    {
                        CoreCustomerService.UpdateCustomer(customerProfile, context);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.CUSTOMER_SYNC_IN_FAILED, ex);
            }
        }

        /// <summary>
        /// For validating the customer.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool ValidateCustomer(CustomerProfile customer, commonData.ZeoContext context)
        {
            try
            {
                Regex nameRgx = new Regex("[^A-Za-z\\-' ]");

                if (string.IsNullOrEmpty(customer.FirstName) || nameRgx.IsMatch(customer.FirstName ?? ""))
                    return false;
                if (string.IsNullOrEmpty(customer.LastName) || nameRgx.IsMatch(customer.LastName ?? ""))
                    return false;
                if (string.IsNullOrEmpty(customer.MothersMaidenName) || nameRgx.IsMatch(customer.MothersMaidenName ?? ""))
                    return false;
                if ((customer.Gender) == null)
                    return false;
                if (customer.Phone1 != null)
                {

                    if (string.IsNullOrEmpty(customer.Phone1.Number) && customer.Phone1.Number.Length < 10)
                        return false;
                    if (string.IsNullOrEmpty(customer.Phone1.Type))
                        return false;
                    else if (customer.Phone1.Type.ToUpper() == "CELL" && !string.IsNullOrEmpty(customer.Phone1.Provider))
                        return false;
                }
                else
                    return false;
                if (customer.Phone2 != null)
                {
                    if (!string.IsNullOrEmpty(customer.Phone2.Number) && customer.Phone2.Number.Length < 10)
                        return false;
                    else if (!string.IsNullOrEmpty(customer.Phone2.Type) && customer.Phone2.Type.ToUpper() == "CELL" && string.IsNullOrEmpty(customer.Phone2.Provider))
                        return false;
                }
                else
                    return false;

                Regex cityRgx = new Regex("[^A-Za-z ]");
                Regex zipRgx = new Regex("\\d{5}");

                if (customer.Address != null)
                {
                    if (string.IsNullOrEmpty(customer.Address.Address1))
                        return false;
                    if (string.IsNullOrEmpty(customer.Address.City) || cityRgx.IsMatch(customer.Address.City.ToUpper() ?? ""))
                        return false;
                    if (string.IsNullOrEmpty(customer.Address.ZipCode) || !zipRgx.IsMatch(customer.Address.ZipCode ?? ""))
                        return false;
                }
                else
                    return false;

                if (customer.MailingAddressDifferent == true && customer.MailingAddress != null)
                {
                    if (string.IsNullOrEmpty(customer.MailingAddress.Address1))
                        return false;
                    if (string.IsNullOrEmpty(customer.MailingAddress.State))
                        return false;
                    if (string.IsNullOrEmpty(customer.MailingAddress.City) || nameRgx.IsMatch(customer.MailingAddress.City.ToUpper() ?? ""))
                        return false;
                    if (string.IsNullOrEmpty(customer.MailingAddress.ZipCode) || !zipRgx.IsMatch(customer.MailingAddress.ZipCode ?? ""))
                        return false;
                }

                if (string.IsNullOrEmpty(customer.CountryOfBirth))
                    return false;

                if (customer.DateOfBirth == null)
                    return false;

                if (string.IsNullOrEmpty(customer.IdIssuingCountry))
                    return false;

                if (string.IsNullOrEmpty(customer.IdType))
                    return false;
                else if (customer.IdType.ToUpper() == "DRIVER'S LICENSE" && (string.IsNullOrEmpty(customer.IdIssuingState) || customer.IdIssueDate == null))
                    return false;
                else if (customer.IdType.ToUpper() == "U.S. STATE IDENTITY CARD" && string.IsNullOrEmpty(customer.IdIssuingState))
                    return false;

                if (string.IsNullOrEmpty(customer.IdNumber))
                    return false;

                if (customer.IdExpirationDate == null)
                    return false;

                if (string.IsNullOrEmpty(customer.LegalCode))
                    return false;

                if (string.IsNullOrEmpty(customer.PrimaryCountryCitizenShip))
                    return false;

                if (string.IsNullOrEmpty(customer.Occupation))
                    return false;

                if (string.IsNullOrEmpty(customer.PIN))
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.CUSTOMER_VALIDATION_FAILED, ex);
            }
        }

        /// <summary>
        /// Used to search the customers either from the card number or the customer details
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public List<CustomerProfile> SearchCustomers(CustomerSearchCriteria criteria, commonData.ZeoContext context)
        {
            List<CustomerProfile> customers = null;

            try
            {
                if (IsRCIFSearch(criteria))
                {
                    customers = GetRcifCustomers(criteria, context);  // RCIF search and Zeo search
                }
                else if (!string.IsNullOrEmpty(criteria.CardNumber))
                {
                    customers = GetCardCustomerDetails(criteria, context); // card search either in Zeo or RCIF based on the card type
                }
                else
                {
                    customers = GetZeoCustomers(criteria, context); // only in Zeo DB
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;

                throw new CustomerException(CustomerException.CUSTOMER_FETCH_FAILED, ex);
            }

            return customers;
        }

        #endregion

        #region Private methods

        private Dictionary<string, string> TruncateFullName(string firstName, string middleName, string lastName, string secondLastName)
        {
            firstName = firstName.Substring(0, firstName.Length >= FirstNameMaxLength ? FirstNameMaxLength : firstName.Length);
            middleName = middleName.Substring(0, middleName.Length >= MiddleNameMaxLength ? MiddleNameMaxLength : middleName.Length);

            int lastNameLength;
            if (firstName.Length == FirstNameMaxLength)
            {
                lastName = lastName.Substring(0, lastName.Length >= LastNameMaxLength - 1 ? LastNameMaxLength - 1 : lastName.Length);

                lastNameLength = FullNameLength - (FirstNameMaxLength + lastName.Length + 2);
            }
            else
            {
                lastName = lastName.Substring(0, lastName.Length >= LastNameMaxLength ? LastNameMaxLength : lastName.Length);

                lastNameLength = LastNameMaxLength - (lastName.Length + 1);
            }

            if (!string.IsNullOrWhiteSpace(secondLastName) && lastNameLength > 0)
            {
                secondLastName = secondLastName.Substring(0, secondLastName.Length >= lastNameLength ? lastNameLength : secondLastName.Length);
            }
            else
            {
                secondLastName = string.Empty;
            }

            string lastNamePlus = lastName;
            if (!string.IsNullOrWhiteSpace(secondLastName))
            {
                lastNamePlus = string.Format("{0} {1}", lastName, secondLastName);
            }

            Dictionary<string, string> fullName = new Dictionary<string, string>();

            //Full name should be 40 incliding spaces
            if ((firstName + middleName + lastNamePlus).Length <= FullNameLength - 2)
            {
                //Do not Truncate
            }
            else if ((firstName + lastNamePlus).Length <= FullNameLength - 3)
            {
                middleName = string.IsNullOrWhiteSpace(middleName) ? string.Empty : middleName.Substring(0, 1);
            }
            else
            {
                middleName = string.Empty;
            }

            fullName.Add("FirstName", firstName);
            fullName.Add("MiddleName", middleName);
            fullName.Add("LastName", lastName);
            fullName.Add("SecondLastName", secondLastName);

            return fullName;
        }

        private string GetMasterCountryName(string countryCode, commonData.ZeoContext context)
        {
            CoreImpl.DataStructureServiceImpl dataStructureService = new CoreImpl.DataStructureServiceImpl();
            COREDATA.MasterCountry masterCountry = dataStructureService.GetMasterCountryByCode(countryCode, context);
            string countryName = masterCountry != null ? masterCountry.Name : null;
            return countryName;
        }

        private void CoretoCxnMapper(commonData.ZeoContext context, CustomerProfile profile, CXNDATA.CustomerProfile cxnCustomer)
        {
            cxnCustomer.IDIssuingStateCode = profile.IdIssuingStateAbbr;

            if (profile.IdType.ToUpper() == "PASSPORT")
            {
                cxnCustomer.IDIssuingStateCode = profile.IdIssuingCountry;
            }

            cxnCustomer.PrimaryCountryCitizenShip = GetMasterCountryName(profile.PrimaryCountryCitizenShip, context);
            cxnCustomer.SecondaryCountryCitizenShip = GetMasterCountryName(profile.SecondaryCountryCitizenShip, context);
            cxnCustomer.CustomerId = profile.CustomerId;

            Dictionary<string, string> fullName = TruncateFullName(cxnCustomer.FirstName, cxnCustomer.MiddleName, cxnCustomer.LastName, cxnCustomer.LastName2);

            cxnCustomer.FirstName = fullName["FirstName"];
            cxnCustomer.MiddleName = fullName["MiddleName"];
            cxnCustomer.LastName = fullName["LastName"];
            cxnCustomer.LastName2 = fullName["SecondLastName"];

            //For new customer registration TCF expect ClientCustID as 0
            if (string.IsNullOrWhiteSpace(cxnCustomer.ClientCustomerId))
            {
                cxnCustomer.ClientCustomerId = "0";
            }
        }

        private static void addSSOAttributes(commonData.ZeoContext context)
        {
            if (context.Context == null)
            {
                context.Context = new Dictionary<string, object>();
            }
            context.Context.Add("SSOAttributes", context.SSOAttributes);
        }

        private COREDATA.CustomerProfile MapToCxeCustomer(COREDATA.CustomerProfile cxeCustomer, CXNDATA.CustomerProfile cxnCustomerProfiles, commonData.ZeoContext context)
        {

            if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.FirstName))
            {
                cxeCustomer.FirstName = cxnCustomerProfiles.FirstName;
            }
            if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.MiddleName))
            {
                cxeCustomer.MiddleName = cxnCustomerProfiles.MiddleName;
            }
            if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.LastName))
            {
                cxeCustomer.LastName = cxnCustomerProfiles.LastName;
            }
            if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.LastName2))
            {
                cxeCustomer.LastName2 = cxnCustomerProfiles.LastName2;
            }
            if (cxnCustomerProfiles.Address != null)
            {
                if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.Address.Address1))
                {
                    cxeCustomer.Address.Address1 = cxnCustomerProfiles.Address.Address1;
                }
                if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.Address.Address2))
                {
                    cxeCustomer.Address.Address2 = cxnCustomerProfiles.Address.Address2;
                }
                if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.Address.City))
                {
                    cxeCustomer.Address.City = cxnCustomerProfiles.Address.City;
                }
                if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.Address.State))
                {
                    cxeCustomer.Address.State = cxnCustomerProfiles.Address.State;
                }
                if (IsValidZipCode(cxnCustomerProfiles.Address.ZipCode))
                {
                    cxeCustomer.Address.ZipCode = cxnCustomerProfiles.Address.ZipCode;
                }
            }
            if (cxnCustomerProfiles.Phone1 != null)
            {
                if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.Phone1.Number) && cxnCustomerProfiles.Phone1.Number.Length == 10 && PhoneNumberCheck(cxnCustomerProfiles.Phone1.Number) > 1)
                {
                    cxeCustomer.Phone1.Number = cxnCustomerProfiles.Phone1.Number;
                }
                if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.Phone1.Provider))
                {
                    cxeCustomer.Phone1.Provider = cxnCustomerProfiles.Phone1.Provider;
                }
                if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.Phone1.Type))
                {
                    cxeCustomer.Phone1.Type = cxnCustomerProfiles.Phone1.Type;
                }
            }

            if (cxnCustomerProfiles.Phone2 != null)
            {
                if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.Phone2.Provider))
                {
                    cxeCustomer.Phone2.Provider = cxnCustomerProfiles.Phone2.Provider;
                }
                if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.Phone2.Number) && cxnCustomerProfiles.Phone2.Number.Length == 10 && PhoneNumberCheck(cxnCustomerProfiles.Phone2.Number) > 1)
                {
                    cxeCustomer.Phone2.Number = cxnCustomerProfiles.Phone2.Number;
                }
                if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.Phone2.Type))
                {
                    cxeCustomer.Phone2.Type = cxnCustomerProfiles.Phone2.Type;
                }
            }
            if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.SSN) && cxnCustomerProfiles.SSN.Length == 9)
            {
                cxeCustomer.SSN = cxnCustomerProfiles.SSN;
                cxeCustomer.IDCode = GetIDCode(cxnCustomerProfiles.SSN);
            }

            if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.CountryOfBirth))
            {
                cxeCustomer.CountryOfBirth = cxnCustomerProfiles.CountryOfBirth;
            }
            if (cxnCustomerProfiles.DateOfBirth != null && cxnCustomerProfiles.DateOfBirth != DateTime.MinValue)
            {
                cxeCustomer.DateOfBirth = cxnCustomerProfiles.DateOfBirth;
            }
            if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.Email))
            {
                cxeCustomer.Email = cxnCustomerProfiles.Email;
            }
            if (cxnCustomerProfiles.Gender != null)
            {
                cxeCustomer.Gender = cxnCustomerProfiles.Gender;
            }
            if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.MothersMaidenName))
            {
                cxeCustomer.MothersMaidenName = cxnCustomerProfiles.MothersMaidenName;
            }
            if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.PrimaryCountryCitizenShip))
            {
                cxeCustomer.PrimaryCountryCitizenShip = cxnCustomerProfiles.PrimaryCountryCitizenShip;
            }
            if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.SecondaryCountryCitizenShip))
            {
                cxeCustomer.SecondaryCountryCitizenShip = cxnCustomerProfiles.SecondaryCountryCitizenShip;
            }
            if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.IdIssuingState))
            {
                cxeCustomer.IdIssuingState = cxnCustomerProfiles.IdIssuingState;
            }
            if (cxnCustomerProfiles.IdExpirationDate != null)
            {
                cxeCustomer.IdExpirationDate = cxnCustomerProfiles.IdExpirationDate;
            }
            if (cxnCustomerProfiles.IdIssueDate != null)
            {
                cxeCustomer.IdIssueDate = cxnCustomerProfiles.IdIssueDate;
            }
            if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.IDTypeName))
            {
                cxeCustomer.IDTypeName = cxnCustomerProfiles.IDTypeName;
            }
            if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.LegalCode))
            {
                cxeCustomer.LegalCode = cxnCustomerProfiles.LegalCode;
            }
            if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.IdIssuingCountry))
            {
                cxeCustomer.IdIssuingCountry = cxnCustomerProfiles.IdIssuingCountry;
            }
            if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.IdType) && !string.IsNullOrWhiteSpace(cxnCustomerProfiles.IdIssuingCountry))
            {
                cxeCustomer.IdType = cxnCustomerProfiles.IdType;

                if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.IdNumber) && cxnCustomerProfiles.IdNumber != "0")
                {
                    cxeCustomer.IdNumber = cxnCustomerProfiles.IdNumber;
                }
            }

            if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.EmployerName))
            {
                cxeCustomer.EmployerName = cxnCustomerProfiles.EmployerName;
            }
            if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.EmployerPhone))
            {
                cxeCustomer.EmployerPhone = cxnCustomerProfiles.EmployerPhone;
            }
            if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.Occupation))
            {
                cxeCustomer.Occupation = cxnCustomerProfiles.Occupation;
            }
            if (!string.IsNullOrWhiteSpace(cxnCustomerProfiles.OccupationDescription))
            {
                cxeCustomer.OccupationDescription = cxnCustomerProfiles.OccupationDescription;
            }
            if (cxnCustomerProfiles.PIN != null)
            {
                cxeCustomer.PIN = cxnCustomerProfiles.PIN;
            }

            return cxeCustomer;
        }

        public static bool IsValidZipCode(string zipCode)
        {
            bool isValid = false;

            if (!string.IsNullOrWhiteSpace(zipCode))
            {
                string x = zipCode.Replace("0", "");
                isValid = !string.IsNullOrWhiteSpace(x);
            }

            return isValid;
        }

        public int PhoneNumberCheck(string phoneNumber)
        {
            int count = 0;
            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                count = Regex.Matches(phoneNumber, @"[1-9]").Count;
            }
            return count;
        }

        private bool IsRCIFSearch(CustomerSearchCriteria criteria)
        {
            if (((criteria.AccountNumber ?? criteria.SSN) != null) && criteria.DateOfBirth != null)
            {
                return true;
            }
            return false;
        }

        private List<CustomerProfile> GetRcifCustomers(CustomerSearchCriteria criteria, commonData.ZeoContext context)
        {
            List<CXNDATA.CustomerProfile> customers = new List<Cxn.Customer.Data.CustomerProfile>();

            ProcessorRouter ProcessorRouter = new ProcessorRouter();

            IClientCustomerService clientCustomerService = ProcessorRouter.GetCXNCustomerProcessor(context.ChannelPartnerName);

            CXNDATA.CustomerSearchCriteria cxnCriteria = Mapper.Map<CustomerSearchCriteria, CXNDATA.CustomerSearchCriteria>(criteria);

            List<CXNDATA.CustomerProfile> cxnCustomerProfiles = clientCustomerService.SearchCoreCustomers(cxnCriteria, context);

            if (cxnCustomerProfiles.Count > 0)
            {
                customers = clientCustomerService.GetRcifMatchedCustomersFromZeo(cxnCustomerProfiles, context);

                customers = customers.Count > 0 ? customers.Concat(cxnCustomerProfiles.Where(x => customers.Any(y => y.ClientCustomerId != x.ClientCustomerId))).ToList() : cxnCustomerProfiles;
            }

            return Mapper.Map<List<CXNDATA.CustomerProfile>, List<CustomerProfile>>(customers);
        }

        private List<CustomerProfile> GetCardCustomerDetails(CustomerSearchCriteria criteria, commonData.ZeoContext context)
        {
            List<CustomerProfile> customers = null;

            switch (criteria.CardType)
            {
                case CardType.TCF:
                    if (criteria.DateOfBirth != null)
                        customers = GetRcifCustomers(criteria, context);
                    break;
                case CardType.ZEO:
                    customers = GetZeoCustomersByCardNumber(criteria, context);
                    break;
            }
            return customers;
        }

        private List<CustomerProfile> GetZeoCustomersByCardNumber(CustomerSearchCriteria criteria, commonData.ZeoContext context)
        {
            List<CustomerProfile> customers = new List<CustomerProfile>();

            string plainCardNumber = criteria.CardNumber;

            criteria.ChannelPartnerId = context.ChannelPartnerId;

            criteria.CardNumber = EncryptCardNumber(criteria.CardNumber); 

            customers = SearchCardCustomer(criteria, context);

            if (customers == null || customers.Count <= 0)
            {
                IFundProcessor fundProcessor = _GetProcessor(context.ChannelPartnerName);

                int providerId = (int)(_GetFundProvider(context.ChannelPartnerName));

                CardAccount cardAccount = new CardAccount()
                {
                    CardNumber = plainCardNumber
                };
                bool isNewCard = true;

                CustomerCard card = fundProcessor.AssociateCard(cardAccount, null, context, isNewCard);

                if (card == null || (card.CustomerId == 0 && card.CustomerId != -1))
                    throw new CXNFundsException.FundException(CXNFundsException.FundException.CARD_MAPPING_ERROR);

                criteria.CardNumber = card.CardNumber;

                customers = SearchCardCustomer(criteria, context);
            }

            return customers;
        }

        private List<CustomerProfile> GetZeoCustomers(CustomerSearchCriteria criteria, commonData.ZeoContext context)
        {
            List<COREDATA.CustomerProfile> zeoCustomers = null;

            COREDATA.CustomerSearchCriteria searchCriteria = Mapper.Map<CustomerSearchCriteria, COREDATA.CustomerSearchCriteria>(criteria);

            using (CoreCustomerService = new CoreImpl.ZeoCoreImpl())
            {
                zeoCustomers = CoreCustomerService.SearchCustomer(searchCriteria, context);
            }

            if(zeoCustomers.Count == 0)
                throw new CustomerException(CustomerException.NO_CUSTOMERS_FOUND_IN_ZEO);

            return Mapper.Map<List<COREDATA.CustomerProfile>, List<CustomerProfile>>(zeoCustomers);
        }

        private string EncryptCardNumber(string cardNumber)
        {
            IDataProtectionService dataProtectionService = null;

            string type = (ConfigurationManager.AppSettings["DataProtectionService"]);

            if (type.ToLower() == "simulator")
            {
                dataProtectionService = new DataProtectionSimulator();
            }
            else
            {
                dataProtectionService = new DataProtectionService();
            }

            return dataProtectionService.Encrypt(cardNumber, 0);
        }

        private IFundProcessor _GetProcessor(string channelPartner)
        {
            ProcessorRouter processorRouter = new ProcessorRouter();
            // get the fund processor for the channel partner.
            return (IFundProcessor)processorRouter.GetFundProvider(channelPartner);
        }

        private ProviderId _GetFundProvider(string channelPartner)
        {
            ProcessorRouter processorRouter = new ProcessorRouter();
            // get the fund provider for the channel partner.
            return processorRouter.GetFundProviders(channelPartner);
        }
    }
    #endregion

}
