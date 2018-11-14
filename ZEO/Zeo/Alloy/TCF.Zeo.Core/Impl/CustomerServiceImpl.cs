using System;
using System.Collections.Generic;
using System.Data;
using P3Net.Data.Common;
using P3Net.Data;
using static TCF.Zeo.Common.Util.Helper;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Core.Contract;
using TCF.Zeo.Core.Data;
using TCF.Zeo.Common.Data;
using System.Configuration;
using TCF.Zeo.Core.Data.Exceptions;
using System.IO;

namespace TCF.Zeo.Core.Impl
{
    public partial class ZeoCoreImpl : ICustomerService
    {
        #region public methods

        /// <summary>
        /// This method is used to get the Validate SSN Number.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool ValidateSSN(ZeoContext context)
        {
            bool isValid = false;

            try
            {
                StoredProcedure coreCustomerProcedure = new StoredProcedure("usp_ValidateSSNByChannelPartnerId");

                coreCustomerProcedure.WithParameters(InputParameter.Named("SSN").WithValue(context.SSN));
                coreCustomerProcedure.WithParameters(InputParameter.Named("channelPartnerId").WithValue(context.ChannelPartnerId));
                coreCustomerProcedure.WithParameters(InputParameter.Named("customerId").WithValue(context.CustomerId));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(coreCustomerProcedure))
                {
                    while (datareader.Read())
                    {
                        isValid = datareader.GetBooleanOrDefault("ValidSSN");
                    }
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.SSN_VALIDATE_FAILED, ex);
            }

            return isValid;
        }

        /// <summary>
        /// This method is used to create a new customer in Alloy.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public long InsertCustomer(CustomerProfile customer, ZeoContext context)
        {
            long customerId = 0;

            try
            {
                long nextPan = NextPAN();

                customer.MailingAddress = customer.MailingAddressDifferent == false ? customer.Address : customer.MailingAddress;

                StoredProcedure coreCustomerProcedure = new StoredProcedure("usp_CreateCustomer");
                //This needs to be passed from code as it is not an Identity column.
                coreCustomerProcedure.WithParameters(InputParameter.Named("CustomerID").WithValue(nextPan)); //This needs to be passed from code as it is not an Identity column.
                coreCustomerProcedure.WithParameters(InputParameter.Named("FirstName").WithValue(customer.FirstName));
                coreCustomerProcedure.WithParameters(InputParameter.Named("MiddleName").WithValue(customer.MiddleName));
                coreCustomerProcedure.WithParameters(InputParameter.Named("LastName").WithValue(customer.LastName));
                coreCustomerProcedure.WithParameters(InputParameter.Named("LastName2").WithValue(customer.LastName2));
                coreCustomerProcedure.WithParameters(InputParameter.Named("MothersMaidenName").WithValue(customer.MothersMaidenName));
                coreCustomerProcedure.WithParameters(InputParameter.Named("DOB").WithValue(customer.DateOfBirth));
                if (customer.Address != null)
                {
                    coreCustomerProcedure.WithParameters(InputParameter.Named("Address1").WithValue(customer.Address.Address1));
                    coreCustomerProcedure.WithParameters(InputParameter.Named("Address2").WithValue(customer.Address.Address2));
                    coreCustomerProcedure.WithParameters(InputParameter.Named("City").WithValue(customer.Address.City));
                    coreCustomerProcedure.WithParameters(InputParameter.Named("State").WithValue(customer.Address.State));
                    coreCustomerProcedure.WithParameters(InputParameter.Named("ZipCode").WithValue(customer.Address.ZipCode));
                }
                if (customer.Phone1 != null)
                {
                    coreCustomerProcedure.WithParameters(InputParameter.Named("Phone1").WithValue(customer.Phone1.Number));
                    coreCustomerProcedure.WithParameters(InputParameter.Named("Phone1Type").WithValue(customer.Phone1.Type));
                    coreCustomerProcedure.WithParameters(InputParameter.Named("Phone1Provider").WithValue(customer.Phone1.Provider));
                }
                if (customer.Phone2 != null)
                {
                    coreCustomerProcedure.WithParameters(InputParameter.Named("Phone2").WithValue(customer.Phone2.Number));
                    coreCustomerProcedure.WithParameters(InputParameter.Named("Phone2Type").WithValue(customer.Phone2.Type));
                    coreCustomerProcedure.WithParameters(InputParameter.Named("Phone2Provider").WithValue(customer.Phone2.Provider));
                }
                coreCustomerProcedure.WithParameters(InputParameter.Named("SSN").WithValue(customer.SSN));
                coreCustomerProcedure.WithParameters(InputParameter.Named("DoNotCall").WithValue(customer.DoNotCall));
                coreCustomerProcedure.WithParameters(InputParameter.Named("SMSEnabled").WithValue(customer.SMSEnabled));
                coreCustomerProcedure.WithParameters(InputParameter.Named("ChannelPartnerId").WithValue(context.ChannelPartnerId));
                coreCustomerProcedure.WithParameters(InputParameter.Named("Gender").WithValue(customer.Gender.ToString()));
                coreCustomerProcedure.WithParameters(InputParameter.Named("Email").WithValue(customer.Email));
                coreCustomerProcedure.WithParameters(InputParameter.Named("PIN").WithValue(customer.PIN));
                coreCustomerProcedure.WithParameters(InputParameter.Named("IsMailingAddressDifferent").WithValue(customer.MailingAddressDifferent));
                if (customer.MailingAddress != null)
                {
                    coreCustomerProcedure.WithParameters(InputParameter.Named("MailingAddress1").WithValue(customer.MailingAddress.Address1));
                    coreCustomerProcedure.WithParameters(InputParameter.Named("MailingAddress2").WithValue(customer.MailingAddress.Address2));
                    coreCustomerProcedure.WithParameters(InputParameter.Named("MailingCity").WithValue(customer.MailingAddress.City));
                    coreCustomerProcedure.WithParameters(InputParameter.Named("MailingState").WithValue(customer.MailingAddress.State));
                    coreCustomerProcedure.WithParameters(InputParameter.Named("MailingZipCode").WithValue(customer.MailingAddress.ZipCode));
                }
                coreCustomerProcedure.WithParameters(InputParameter.Named("ReceiptLanguage").WithValue(customer.ReceiptLanguage));
                coreCustomerProcedure.WithParameters(InputParameter.Named("ProfileStatus").WithValue((int)customer.ProfileStatus));
                coreCustomerProcedure.WithParameters(InputParameter.Named("CountryOfBirth").WithValue(customer.CountryOfBirth));
                coreCustomerProcedure.WithParameters(InputParameter.Named("Notes").WithValue(customer.Notes));
                coreCustomerProcedure.WithParameters(InputParameter.Named("clientid").WithValue(customer.ClientCustomerId));
                coreCustomerProcedure.WithParameters(InputParameter.Named("LegalCode").WithValue(customer.LegalCode));
                coreCustomerProcedure.WithParameters(InputParameter.Named("PrimaryCountryCitizenShip").WithValue(customer.PrimaryCountryCitizenShip));
                coreCustomerProcedure.WithParameters(InputParameter.Named("SecondaryCountryCitizenShip").WithValue(customer.SecondaryCountryCitizenShip));
                coreCustomerProcedure.WithParameters(InputParameter.Named("IDCode").WithValue(customer.IDCode.ToString()));
                coreCustomerProcedure.WithParameters(InputParameter.Named("Occupation").WithValue(customer.Occupation));
                coreCustomerProcedure.WithParameters(InputParameter.Named("Employer").WithValue(customer.EmployerName));
                coreCustomerProcedure.WithParameters(InputParameter.Named("EmployerPhone").WithValue(customer.EmployerPhone));
                coreCustomerProcedure.WithParameters(InputParameter.Named("OccupationDescription").WithValue(customer.OccupationDescription));
                coreCustomerProcedure.WithParameters(InputParameter.Named("GovtIdTypeId").WithValue(customer.IdType));
                //coreCustomerProcedure.WithParameters(InputParameter.Named("GovtIdentification").WithValue(customer.IDTypeName));
                coreCustomerProcedure.WithParameters(InputParameter.Named("GovtIdentification").WithValue(customer.IdNumber));
                coreCustomerProcedure.WithParameters(InputParameter.Named("GovtIDExpirationDate").WithValue(customer.IdExpirationDate));
                coreCustomerProcedure.WithParameters(InputParameter.Named("GovtIdIssueDate").WithValue(customer.IdIssueDate));
                coreCustomerProcedure.WithParameters(InputParameter.Named("Group1").WithValue(customer.Group1));
                coreCustomerProcedure.WithParameters(InputParameter.Named("Group2").WithValue(customer.Group2));
                coreCustomerProcedure.WithParameters(InputParameter.Named("IsPartnerAccountHolder").WithValue(customer.IsAccountHolder));
                coreCustomerProcedure.WithParameters(InputParameter.Named("ReferralCode").WithValue(customer.ReferralCode));
                coreCustomerProcedure.WithParameters(InputParameter.Named("AgentSessionID").WithValue(context.AgentSessionId));
                coreCustomerProcedure.WithParameters(InputParameter.Named("LastUpdatedAgentSessionID").WithValue(context.AgentSessionId));
                coreCustomerProcedure.WithParameters(InputParameter.Named("IdIssueCountry").WithValue(customer.IdIssuingCountry));
                coreCustomerProcedure.WithParameters(InputParameter.Named("IdIssueState").WithValue(customer.IdIssuingState));
                coreCustomerProcedure.WithParameters(InputParameter.Named("DTServerCreate").WithValue(customer.DTServerCreate));
                coreCustomerProcedure.WithParameters(InputParameter.Named("DTTerminalCreate").WithValue(customer.DTTerminalCreate));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(coreCustomerProcedure))
                {
                    while (datareader.Read())
                    {
                        customerId = datareader.GetInt64OrDefault("CustomerID");
                    }
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.CUSTOMER_CREATE_FAILED, ex);
            }

            return customerId;
        }

        /// <summary>
        /// This method is used to update the existing Alloy customer.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool UpdateCustomer(CustomerProfile customer, ZeoContext context)
        {
            bool isSuccess = false;

            try
            {
                StoredProcedure coreCustomerProcedure = new StoredProcedure("usp_UpdateCustomer");

                customer.MailingAddress = customer.MailingAddressDifferent == false ? customer.Address : customer.MailingAddress;

                coreCustomerProcedure.WithParameters(InputParameter.Named("CustomerID").WithValue(customer.CustomerId)); //This needs to be passed from code as it is not an Identity column.
                coreCustomerProcedure.WithParameters(InputParameter.Named("FirstName").WithValue(customer.FirstName));
                coreCustomerProcedure.WithParameters(InputParameter.Named("MiddleName").WithValue(customer.MiddleName));
                coreCustomerProcedure.WithParameters(InputParameter.Named("LastName").WithValue(customer.LastName));
                coreCustomerProcedure.WithParameters(InputParameter.Named("LastName2").WithValue(customer.LastName2));
                coreCustomerProcedure.WithParameters(InputParameter.Named("MothersMaidenName").WithValue(customer.MothersMaidenName));
                coreCustomerProcedure.WithParameters(InputParameter.Named("DOB").WithValue(customer.DateOfBirth));
                if (customer.Address != null)
                {
                    coreCustomerProcedure.WithParameters(InputParameter.Named("Address1").WithValue(customer.Address.Address1));
                    coreCustomerProcedure.WithParameters(InputParameter.Named("Address2").WithValue(customer.Address.Address2));
                    coreCustomerProcedure.WithParameters(InputParameter.Named("City").WithValue(customer.Address.City));
                    coreCustomerProcedure.WithParameters(InputParameter.Named("State").WithValue(customer.Address.State));
                    coreCustomerProcedure.WithParameters(InputParameter.Named("ZipCode").WithValue(customer.Address.ZipCode));
                }
                if (customer.Phone1 != null)
                {
                    coreCustomerProcedure.WithParameters(InputParameter.Named("Phone1").WithValue(customer.Phone1.Number));
                    coreCustomerProcedure.WithParameters(InputParameter.Named("Phone1Type").WithValue(customer.Phone1.Type));
                    coreCustomerProcedure.WithParameters(InputParameter.Named("Phone1Provider").WithValue(customer.Phone1.Provider));
                }
                if (customer.Phone2 != null)
                {
                    if (customer.Phone2.Number != null)
                    {
                        coreCustomerProcedure.WithParameters(InputParameter.Named("Phone2").WithValue(customer.Phone2.Number));
                        coreCustomerProcedure.WithParameters(InputParameter.Named("Phone2Type").WithValue(customer.Phone2.Type));
                        coreCustomerProcedure.WithParameters(InputParameter.Named("Phone2Provider").WithValue(customer.Phone2.Provider));
                    }
                }
                coreCustomerProcedure.WithParameters(InputParameter.Named("SSN").WithValue(customer.SSN));
                //  coreCustomerProcedure.WithParameters(InputParameter.Named("TaxpayerId").WithValue(customer.taxPayerID));
                coreCustomerProcedure.WithParameters(InputParameter.Named("DoNotCall").WithValue(customer.DoNotCall));
                coreCustomerProcedure.WithParameters(InputParameter.Named("SMSEnabled").WithValue(customer.SMSEnabled));
                //coreCustomerProcedure.WithParameters(InputParameter.Named("MarketingSMSEnabled").WithValue(customer.Marke));
                coreCustomerProcedure.WithParameters(InputParameter.Named("ChannelPartnerId").WithValue(context.ChannelPartnerId));
                coreCustomerProcedure.WithParameters(InputParameter.Named("Gender").WithValue(customer.Gender.ToString()));
                coreCustomerProcedure.WithParameters(InputParameter.Named("Email").WithValue(customer.Email));
                coreCustomerProcedure.WithParameters(InputParameter.Named("PIN").WithValue(customer.PIN));
                coreCustomerProcedure.WithParameters(InputParameter.Named("IsMailingAddressDifferent").WithValue(customer.MailingAddressDifferent));
                if (customer.MailingAddress != null)
                {
                    coreCustomerProcedure.WithParameters(InputParameter.Named("MailingAddress1").WithValue(customer.MailingAddress.Address1));
                    coreCustomerProcedure.WithParameters(InputParameter.Named("MailingAddress2").WithValue(customer.MailingAddress.Address2));
                    coreCustomerProcedure.WithParameters(InputParameter.Named("MailingCity").WithValue(customer.MailingAddress.City));
                    coreCustomerProcedure.WithParameters(InputParameter.Named("MailingState").WithValue(customer.MailingAddress.State));
                    coreCustomerProcedure.WithParameters(InputParameter.Named("MailingZipCode").WithValue(customer.MailingAddress.ZipCode));
                }
                coreCustomerProcedure.WithParameters(InputParameter.Named("ReceiptLanguage").WithValue(customer.ReceiptLanguage));
                coreCustomerProcedure.WithParameters(InputParameter.Named("ProfileStatus").WithValue((int)(customer.ProfileStatus)));
                coreCustomerProcedure.WithParameters(InputParameter.Named("CountryOfBirth").WithValue(customer.CountryOfBirth));
                coreCustomerProcedure.WithParameters(InputParameter.Named("Notes").WithValue(customer.Notes));
                coreCustomerProcedure.WithParameters(InputParameter.Named("ClientID").WithValue(customer.ClientCustomerId));
                coreCustomerProcedure.WithParameters(InputParameter.Named("LegalCode").WithValue(customer.LegalCode));
                coreCustomerProcedure.WithParameters(InputParameter.Named("PrimaryCountryCitizenShip").WithValue(customer.PrimaryCountryCitizenShip));
                coreCustomerProcedure.WithParameters(InputParameter.Named("SecondaryCountryCitizenShip").WithValue(customer.SecondaryCountryCitizenShip));
                coreCustomerProcedure.WithParameters(InputParameter.Named("IDCode").WithValue((Helper.GetIDCode(customer.SSN)).ToString()));
                coreCustomerProcedure.WithParameters(InputParameter.Named("Occupation").WithValue(customer.Occupation));
                coreCustomerProcedure.WithParameters(InputParameter.Named("Employer").WithValue(customer.EmployerName));
                coreCustomerProcedure.WithParameters(InputParameter.Named("EmployerPhone").WithValue(customer.EmployerPhone));
                coreCustomerProcedure.WithParameters(InputParameter.Named("OccupationDescription").WithValue(customer.OccupationDescription));
                coreCustomerProcedure.WithParameters(InputParameter.Named("GovtIdTypeId").WithValue(customer.IdType));
                //coreCustomerProcedure.WithParameters(InputParameter.Named("GovtIdentification").WithValue(customer.IDTypeName));
                coreCustomerProcedure.WithParameters(InputParameter.Named("GovtIdentification").WithValue(customer.IdNumber));
                coreCustomerProcedure.WithParameters(InputParameter.Named("GovtIDExpirationDate").WithValue(customer.IdExpirationDate == DateTime.MinValue ? null : customer.IdExpirationDate));
                coreCustomerProcedure.WithParameters(InputParameter.Named("GovtIdIssueDate").WithValue(customer.IdIssueDate == DateTime.MinValue ? null : customer.IdIssueDate));
                coreCustomerProcedure.WithParameters(InputParameter.Named("IdIssueCountry").WithValue(customer.IdIssuingCountry));
                coreCustomerProcedure.WithParameters(InputParameter.Named("IdIssueState").WithValue(customer.IdIssuingState));
                coreCustomerProcedure.WithParameters(InputParameter.Named("Group1").WithValue(customer.Group1));
                coreCustomerProcedure.WithParameters(InputParameter.Named("Group2").WithValue(customer.Group2));
                coreCustomerProcedure.WithParameters(InputParameter.Named("IsPartnerAccountHolder").WithValue(customer.IsAccountHolder));
                coreCustomerProcedure.WithParameters(InputParameter.Named("ReferralCode").WithValue(customer.ReferralCode));
                coreCustomerProcedure.WithParameters(InputParameter.Named("AgentSessionID").WithValue(context.AgentSessionId));
                coreCustomerProcedure.WithParameters(InputParameter.Named("LastUpdatedAgentSessionID").WithValue(context.AgentSessionId));
                coreCustomerProcedure.WithParameters(InputParameter.Named("DTServerLastModified").WithValue(DateTime.Now));
                coreCustomerProcedure.WithParameters(InputParameter.Named("DTTerminalLastModified").WithValue(GetTimeZoneTime(context.TimeZone)));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(coreCustomerProcedure))
                {
                    isSuccess = datareader.RecordsAffected > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.CUSTOMER_UPDATE_FAILED, ex);
            }

            return isSuccess;
        }

        /// <summary>
        /// This method is used to get the customer.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public CustomerProfile GetCustomer(ZeoContext context)
        {
            CustomerProfile customerProfile = null;

            try
            {
                StoredProcedure customerProcedure = new StoredProcedure("usp_GetCustomerByCustomerId");

                customerProcedure.WithParameters(InputParameter.Named("CustomerID").WithValue(context.CustomerId));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(customerProcedure))
                {
                    while (datareader.Read())
                    {
                        customerProfile = new CustomerProfile();
                        customerProfile.CustomerId = datareader.GetInt64OrDefault("CustomerID");
                        customerProfile.SSN = datareader.GetStringOrDefault("SSN");
                        customerProfile.FirstName = datareader.GetStringOrDefault("FirstName");
                        customerProfile.MiddleName = datareader.GetStringOrDefault("MiddleName");
                        customerProfile.LastName = datareader.GetStringOrDefault("LastName");
                        customerProfile.LastName2 = datareader.GetStringOrDefault("LastName2");
                        customerProfile.MothersMaidenName = datareader.GetStringOrDefault("MothersMaidenName");
                        customerProfile.DateOfBirth = datareader.GetDateTimeOrDefault("DOB");
                        customerProfile.Address = new Address()
                        {
                            Address1 = datareader.GetStringOrDefault("Address1"),
                            Address2 = datareader.GetStringOrDefault("Address2"),
                            City = datareader.GetStringOrDefault("City"),
                            State = datareader.GetStringOrDefault("State"),
                            ZipCode = datareader.GetStringOrDefault("ZipCode")
                        };
                        customerProfile.Phone1 = new Phone()
                        {
                            Number = datareader.GetStringOrDefault("Phone1"),
                            Type = datareader.GetStringOrDefault("Phone1Type"),
                            Provider = datareader.GetStringOrDefault("Phone1Provider")
                        };
                        customerProfile.Phone2 = new Phone()
                        {
                            Number = datareader.GetStringOrDefault("Phone2"),
                            Type = datareader.GetStringOrDefault("Phone2Type"),
                            Provider = datareader.GetStringOrDefault("Phone2Provider")
                        };
                        customerProfile.DoNotCall = datareader.GetBooleanOrDefault("DoNotCall");
                        customerProfile.SMSEnabled = datareader.GetBooleanOrDefault("SMSEnabled");
                        customerProfile.CardNumber = datareader.GetStringOrDefault("CardNumber");
                        //customerProfile.ChannelPartnerId = datareader.GetInt64OrDefault("ChannelPartnerId");
                        customerProfile.Gender = GetGender(datareader.GetStringOrDefault("Gender"));
                        customerProfile.Email = datareader.GetStringOrDefault("Email");
                        customerProfile.PIN = datareader.GetStringOrDefault("PIN");
                        customerProfile.MailingAddressDifferent = datareader.GetBooleanOrDefault("IsMailingAddressDifferent");
                        customerProfile.ClientProfileStatus = GetProfileStatus(datareader.GetStringOrDefault("ClientProfileStatus"));
                        customerProfile.MailingAddress = new Address()
                        {
                            Address1 = datareader.GetStringOrDefault("MailingAddress1"),
                            Address2 = datareader.GetStringOrDefault("MailingAddress2"),
                            City = datareader.GetStringOrDefault("MailingCity"),
                            State = datareader.GetStringOrDefault("MailingState"),
                            ZipCode = datareader.GetStringOrDefault("MailingZipCode")
                        };
                        customerProfile.ReceiptLanguage = datareader.GetStringOrDefault("ReceiptLanguage");
                        customerProfile.ProfileStatus = GetProfileStatus(datareader.GetStringOrDefault("ProfileStatus"));
                        customerProfile.CountryOfBirth = datareader.GetStringOrDefault("CountryOfBirth");
                        customerProfile.Notes = datareader.GetStringOrDefault("Notes");
                        customerProfile.ClientCustomerId = datareader.GetStringOrDefault("ClientID");
                        customerProfile.LegalCode = datareader.GetStringOrDefault("LegalCode");
                        customerProfile.PrimaryCountryCitizenShip = datareader.GetStringOrDefault("PrimaryCountryCitizenShip");
                        customerProfile.SecondaryCountryCitizenShip = datareader.GetStringOrDefault("SecondaryCountryCitizenShip");

                        string idcode = datareader.GetStringOrDefault("IDCode");
                        customerProfile.IDCode = string.IsNullOrWhiteSpace(idcode) ? (TaxIDCode?)null : (TaxIDCode)Enum.Parse(typeof(TaxIDCode), idcode);

                        customerProfile.Occupation = datareader.GetStringOrDefault("Occupation");
                        customerProfile.EmployerName = datareader.GetStringOrDefault("Employer");
                        customerProfile.EmployerPhone = datareader.GetStringOrDefault("EmployerPhone");
                        customerProfile.OccupationDescription = datareader.GetStringOrDefault("OccupationDescription");
                        customerProfile.IdType = datareader.GetStringOrDefault("IdName");
                        customerProfile.IdIssuingCountry = datareader.GetStringOrDefault("IdCountry");
                        customerProfile.IdIssuingState = datareader.GetStringOrDefault("IdState");
                        customerProfile.IdIssuingStateAbbr = datareader.GetStringOrDefault("IdStateAbbr");
                        customerProfile.IdExpirationDate = datareader.GetDateTimeOrDefault("GovtIDExpirationDate");
                        customerProfile.IdIssueDate = datareader.GetDateTimeOrDefault("GovtIdIssueDate");
                        //customerProfile.IDTypeName = datareader.GetStringOrDefault("GovtIdentification");
                        customerProfile.IdNumber = datareader.GetStringOrDefault("GovtIdentification");
                        customerProfile.Group1 = datareader.GetStringOrDefault("Group1");
                        customerProfile.Group2 = datareader.GetStringOrDefault("Group2");
                        customerProfile.IsAccountHolder = datareader.GetBooleanOrDefault("IsPartnerAccountHolder");
                        customerProfile.ReferralCode = datareader.GetStringOrDefault("ReferralCode");
                    }

                }

            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.CUSTOMER_NOT_FOUND, ex);
            }

            return customerProfile;
        }

        /// <summary>
        /// This method is used to search the customer from search criteria.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public List<CustomerProfile> SearchCustomer(CustomerSearchCriteria criteria, ZeoContext context)
        {
            List<Core.Data.CustomerProfile> customerProfiles = new List<CustomerProfile>();
            try
            {
                StoredProcedure customerProcedure = new StoredProcedure("usp_SearchCustomerBySearchCriteria");

                customerProcedure.WithParameters(InputParameter.Named("channelPartnerId").WithValue(context.ChannelPartnerId));
                customerProcedure.WithParameters(InputParameter.Named("ssn").WithValue(criteria.SSN));
                customerProcedure.WithParameters(InputParameter.Named("lastName").WithValue(criteria.LastName));
                customerProcedure.WithParameters(InputParameter.Named("dob").WithValue(criteria.DateOfBirth));
                customerProcedure.WithParameters(InputParameter.Named("phoneNumber").WithValue(criteria.PhoneNumber));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(customerProcedure))
                {
                    while (datareader.Read())
                    {
                        Core.Data.CustomerProfile customerProfile = new Core.Data.CustomerProfile();
                        customerProfile.CustomerId = datareader.GetInt64OrDefault("CustomerID");
                        customerProfile.FirstName = datareader.GetStringOrDefault("FirstName");
                        customerProfile.LastName = datareader.GetStringOrDefault("LastName");
                        customerProfile.DateOfBirth = datareader.GetDateTimeOrDefault("DOB");
                        customerProfile.SSN = datareader.GetStringOrDefault("SSN");
                        customerProfile.ProfileStatus = Helper.GetProfileStatus(datareader.GetStringOrDefault("ProfileStatus"));

                        customerProfile.Address = new Address
                        {
                            Address1 = datareader.GetStringOrDefault("Address1")
                        };

                        customerProfile.Phone1 = new Phone()
                        {
                            Number = datareader.GetStringOrDefault("Phone1")
                        };

                        customerProfile.IdNumber = datareader.GetStringOrDefault("GovtIdentification");
                        customerProfile.CardNumber = datareader.GetStringOrDefault("cardNumber");
                        customerProfile.IdExpirationDate = datareader.GetDateTimeOrDefault("GovtIDExpirationDate");
                        customerProfiles.Add(customerProfile);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.SEARCH_CUSTOMERS_FAILED, ex);
            }



            return customerProfiles;
        }

        /// <summary>
        /// This method is used to create a customer session.
        /// </summary>
        /// <param name="customerSession"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public CustomerSession CreateCustomerSession(CustomerSession customerSession, ZeoContext context)
        {
            try
            {
                StoredProcedure coreCustomerProcedure = new StoredProcedure("usp_CreateCustomerSession");

                coreCustomerProcedure.WithParameters(InputParameter.Named("AgentSessionId").WithValue(context.AgentSessionId));
                coreCustomerProcedure.WithParameters(InputParameter.Named("CustomerId").WithValue(customerSession.CustomerId));
                coreCustomerProcedure.WithParameters(InputParameter.Named("TimeZone").WithValue(customerSession.TimezoneID));
                coreCustomerProcedure.WithParameters(InputParameter.Named("CardSearchType").WithValue((int)customerSession.CardSearchType));
                coreCustomerProcedure.WithParameters(InputParameter.Named("DTServerCreate").WithValue(customerSession.DTServerCreate));
                coreCustomerProcedure.WithParameters(InputParameter.Named("DTTerminalCreate").WithValue(customerSession.DTTerminalCreate));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(coreCustomerProcedure))
                {
                    while (datareader.Read())
                    {
                        //Return type of SCOPE_IDENTITY() is decimal.
                        customerSession.Id = Convert.ToInt64(datareader.GetDecimalOrDefault("CustomerSessionID"));
                        customerSession.IsGPRCustomer = datareader.GetBooleanOrDefault("IsGPRCustomer");
                        customerSession.CardNumber = datareader.GetStringOrDefault("CardNumber");
                        customerSession.CardPresent = datareader.GetBooleanOrDefault("CardPresent");
                        customerSession.ProfileStatus = GetProfileStatus(datareader.GetStringOrDefault("ProfileStatus"));
                        customerSession.CartId = datareader.GetInt64OrDefault("CartId");
                    }
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.CUSTOMER_SESSION_CREATE_FAILED, ex);
            }

            return customerSession;
        }

        public void UpdateCustomerRegistrationStatus(ProfileStatus status, string clientId, string errorReason,
            bool isRCIFSuccess, ZeoContext context)
        {
            try
            {
                StoredProcedure coreCustomerProcedure = new StoredProcedure("usp_UpdateCustomerRegistrationStatusbyCustomerId");
                coreCustomerProcedure.WithParameters(InputParameter.Named("errorReason").WithValue(errorReason));
                coreCustomerProcedure.WithParameters(InputParameter.Named("customerId").WithValue(context.CustomerId));
                coreCustomerProcedure.WithParameters(InputParameter.Named("status").WithValue((int)status));
                coreCustomerProcedure.WithParameters(InputParameter.Named("clientId").WithValue(clientId));
                coreCustomerProcedure.WithParameters(InputParameter.Named("IsRCIFSuccess").WithValue(isRCIFSuccess));
                coreCustomerProcedure.WithParameters(InputParameter.Named("DTServerLastModified").WithValue(DateTime.Now));
                coreCustomerProcedure.WithParameters(InputParameter.Named("DTTerminalLastModified").WithValue(GetTimeZoneTime(context.TimeZone)));

                DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(coreCustomerProcedure);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.UPDATE_ERRORREASON_FAILED, ex);
            }

        }


        #endregion

        #region private methods

        /// <summary>
        /// This method is used to get the CustomerId.
        /// </summary>
        /// <returns></returns>
        private long NextPAN()
        {
            bool isRealPAN = Convert.ToBoolean(ConfigurationManager.AppSettings["IsRealPAN"]);

            if (isRealPAN)
            {
                long panNumber = 100000000000000;

                //Check for PAN number tag in config. This will be mainly used for environments to get the CustomerId.
                //Since same RCIF is used for all the environments except Prod, we have chances that same customer id to be generated in different enviornments and RCIF throws up an error saying already exists. 
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["PANNumber"]))
                {
                    panNumber = Convert.ToInt64(ConfigurationManager.AppSettings["PANNumber"]);
                }

                return GenerateNewId("PAN", panNumber);
            }
            return GenerateNewId();
        }

        /// <summary>
        /// This method is used to get the CustomerId.
        /// </summary>
        /// <param name="sequenceName"></param>
        /// <param name="first15"></param>
        /// <returns></returns>
        private long GenerateNewId(string sequenceName, long first15)
        {
            long newId = 0;
            StoredProcedure nextPan = new StoredProcedure("GetNextSequenceNumber");
            nextPan.WithParameters(InputParameter.Named("sequenceName").WithValue(sequenceName));
            nextPan.WithParameters(InputParameter.Named("sequenceNumber").WithValue(string.Empty));

            using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(nextPan))
            {
                while (datareader.Read())
                {
                    first15 += datareader.GetInt64("SequenceNumber");
                    newId = (10 * first15);
                }
            }

            return newId;
        }

        private long GenerateNewId()
        {
            string r = "2";
            RandomCryptoServiceProvider randomCryptoServiceProvider = new RandomCryptoServiceProvider();
            for (int i = 0; i < 15; i++)
            {
                r += randomCryptoServiceProvider.Next(0, 9).ToString();
            }
            long newId = Convert.ToInt64(r);
            return newId;
        }

        #endregion
    }
}
