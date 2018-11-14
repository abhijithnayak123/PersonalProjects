using System;
using System.Collections.Generic;
using System.Linq;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Cxn.Customer.Contract;
using TCF.Zeo.Cxn.Customer.Data;
using static TCF.Zeo.Common.Util.Helper;
using TCF.Zeo.Cxn.Customer.TCF.Data;
using System.Data;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Cxn.Common;
using P3Net.Data;
using P3Net.Data.Common;
using TCF.Zeo.Cxn.Customer.TCF.Contract;
using System.Configuration;
using TCF.Zeo.Cxn.Customer.Data.Exceptions;
using System.IO;

namespace TCF.Zeo.Cxn.Customer.TCF.Impl
{
    public class Gateway : IClientCustomerService
    {
        internal IIO IO { get; set; }

        public Gateway()
        {
            IO = GetIO();
        }

        #region Public Methods

        /// <summary>
        /// This method is used to search the customer in provider.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public List<CustomerProfile> SearchCoreCustomers(CustomerSearchCriteria criteria, ZeoContext context)
        {
            List<CustomerProfile> customers = new List<CustomerProfile>();

            try
            {
                RCIFCredential credential = RCIFCommon.GetCredential(context.ChannelPartnerId);  
                              
                customers = IO.SearchCoreCustomers(criteria, credential, context);

                customers.ForEach(c => c.CustomerType = CustomerType.RCIF);

                return customers;
            }
            catch (Exception ex)
            {
                return customers;
            }
        }

        /// <summary>
        /// This method is used to push the customer data to client.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string Add(CustomerProfile customer, ZeoContext context)
        {
            string partnerAccountNo = (customer != null) ? customer.PartnerAccountNumber : string.Empty;
            try
            {
                try
                {
                    RCIFCredential credential = RCIFCommon.GetCredential(context.ChannelPartnerId);
                    partnerAccountNo = IO.CreateCustomer(customer, credential, context);
                }
                catch (Exception ex)
                {
                    if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                    throw new CustomerException(CustomerException.CREATE_CUSTOMER_FAILED, ex);
                }

                UpdateTcisAccount(context, partnerAccountNo);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.ADD_ACCOUNT_FAILED, ex);
            }

            return partnerAccountNo;
        }


        /// <summary>
        /// This method is used to push the customer data to client.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public long AddCXNAccount(CustomerProfile customer, ZeoContext context)
        {
            TCISAccount account = new TCISAccount();

            long accountId = 0;
            try
            {
                CustomerProfile profile = GetAccountByCustomerId(context);
                account.ProfileStatus = ProfileStatus.Inactive;
                account.PartnerAccountNumber = customer.PartnerAccountNumber;
                account.DTTerminalCreate = Helper.GetTimeZoneTime(context.TimeZone);
                account.DTServerCreate = DateTime.Now;
                account.CustomerID = Convert.ToInt64(customer.CustomerID);
                account.CustomerSessionId = customer.CustomerSessionId;
                account.CustomerBankId = context.BankId;
                account.CustomerBranchId = context.BranchId;
                account.TcfCustInd = ValidateClientCustId(customer.PartnerAccountNumber);
                account.DTTerminalLastModified = Helper.GetTimeZoneTime(context.TimeZone);
                account.DTServerLastModified = DateTime.Now;

                if (profile == null)
                {
                    accountId = AddAccount(account);
                }
                else
                {
                    account.Id = profile.Id;
                    accountId = UpdateAccount(account);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.CREATE_CUSTOMER_FAILED, ex);
            }

            return accountId;
        }

        /// <summary>
        /// This method is used to get client account data.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public CustomerProfile GetAccountByCustomerId(ZeoContext context)
        {
            CustomerProfile customerProfile = null;
            try
            {
                StoredProcedure coreCustomerProcedure = new StoredProcedure("usp_TCF_GetTCISAccountByCustomerId");
                coreCustomerProcedure.WithParameters(InputParameter.Named("CustomerId").WithValue(context.CustomerId));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(coreCustomerProcedure))
                {
                    customerProfile = GetProfile(datareader);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.FIND_ACCOUNT_FAILED, ex);
            }

            return customerProfile;
        }

        /// <summary>
        /// This method is used to get client account data.
        /// </summary>
        /// <param name="cxnAccountId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public CustomerProfile GetAccountById(long cxnAccountId, ZeoContext context)
        {
            CustomerProfile customerProfile = null;
            try
            {
                StoredProcedure coreCustomerProcedure = new StoredProcedure("usp_TCF_GetTCISAccountById");
                coreCustomerProcedure.WithParameters(InputParameter.Named("Id").WithValue(cxnAccountId));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(coreCustomerProcedure))
                {
                    customerProfile = GetProfile(datareader);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.FIND_ACCOUNT_FAILED, ex);
            }

            return customerProfile;
        }

        /// <summary>
        /// This method is used to search the customer with card number.
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public List<CustomerProfile> SearchCustomerWithCardNumber(string cardNumber, ZeoContext context)
        {
            List<CustomerProfile> customerProfiles = new List<CustomerProfile>();

            try
            {
                StoredProcedure customerProcedure = new StoredProcedure("usp_SearchCustomerByCardNumber");

                customerProcedure.WithParameters(InputParameter.Named("cardNumber").WithValue(cardNumber));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(customerProcedure))
                {
                    while (datareader.Read())
                    {
                        CustomerProfile customerProfile = new CustomerProfile();
                        customerProfile.CustomerId = datareader.GetStringOrDefault("CustomerID");
                        customerProfile.FirstName = datareader.GetStringOrDefault("FirstName");
                        customerProfile.LastName = datareader.GetStringOrDefault("LastName");
                        customerProfile.DateOfBirth = datareader.GetDateTimeOrDefault("DOB");
                        customerProfile.SSN = datareader.GetStringOrDefault("SSN");
                        customerProfile.ProfileStatus = Helper.GetProfileStatus(datareader.GetStringOrDefault("ProfileStatus"));
                        customerProfile.CardNumber = datareader.GetStringOrDefault("CardNumber");
                        customerProfile.Phone1 = new Phone()
                        {
                            Number = datareader.GetStringOrDefault("Phone1")
                        };

                        customerProfile.IdNumber = datareader.GetStringOrDefault("GovtIdentification");
                        customerProfile.Address = new Address
                        {
                            Address1 = datareader.GetStringOrDefault("Address1")
                        };
                        customerProfile.IdExpirationDate = datareader.GetDateTimeOrDefault("GovtIDExpirationDate");
                        customerProfiles.Add(customerProfile);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.SEARCH_CUSTOMER_FAILED, ex);
            }

            return customerProfiles;
        }


        /// <summary>
        /// This method is used to sync in the customer details with TCF.
        /// </summary>
        /// <param name="custProfile"></param>
        /// <returns></returns>
        public CustomerProfile CustomerSyncInFromClient(CustomerProfile custProfile, ZeoContext context)
        {
            CustomerProfile tcisCustomer = new CustomerProfile();

            try
            {
                CustomerSearchCriteria criteria = new CustomerSearchCriteria() { ClientCustId = custProfile.ClientCustomerId };
                RCIFCredential credential = RCIFCommon.GetCredential(context.ChannelPartnerId);
                tcisCustomer = IO.SearchCoreCustomers(criteria, credential, context).FirstOrDefault();

                // Keep Alloy Customer name detailsas it is(Tcf don't want customer name updated from RCIF at session authentication)//AL-626
                if (tcisCustomer != null)
                {
                    tcisCustomer.FirstName = custProfile.FirstName;
                    tcisCustomer.MiddleName = custProfile.MiddleName;
                    tcisCustomer.LastName = custProfile.LastName;
                    tcisCustomer.LastName2 = custProfile.LastName2;
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.SYNC_IN_FAILED, ex);
            }

            return tcisCustomer;
        }

        /// <summary>
        /// To get the matching customers from the Zeo database based on the PKY number, SSN
        /// </summary>
        /// <param name="customers"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public List<CustomerProfile> GetCustomersForSingleSearch(List<CustomerProfile> customers, string ssn, ZeoContext context)
        {
            List<CustomerProfile> customerProfiles = new List<CustomerProfile>();

            StringWriter writer = null;

            try
            {
                if(customers.Count != 0)
                    writer = GetPkyNumbersXml(customers);

                StoredProcedure customerProcedure = new StoredProcedure("usp_GetRcifCustomersByPky");

                DataParameter[] dataParameters = new DataParameter[]
                {
                    new DataParameter("pkyNumbers", DbType.Xml)
                    {
                        Value = Convert.ToString(writer)
                    },
                    new DataParameter("ssn", DbType.String)
                    {
                        Value = ssn
                    }
                };

                customerProcedure.WithParameters(dataParameters);

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(customerProcedure))
                {
                    while (datareader.Read())
                    {
                        CustomerProfile customerProfile = new CustomerProfile();
                        customerProfile.CustomerId = datareader.GetStringOrDefault("CustomerID");
                        customerProfile.FirstName = datareader.GetStringOrDefault("FirstName");
                        customerProfile.LastName = datareader.GetStringOrDefault("LastName");
                        customerProfile.DateOfBirth = datareader.GetDateTimeOrDefault("DOB");
                        customerProfile.Address = new Address
                        {
                            Address1 = datareader.GetStringOrDefault("Address1")
                        };
                        customerProfile.CardNumber = datareader.GetStringOrDefault("CardNumber");
                        customerProfile.IdNumber = datareader.GetStringOrDefault("IdNumber");
                        customerProfile.SSN = datareader.GetStringOrDefault("SSN");
                        customerProfile.Phone1 = new Phone
                        {
                            Number = datareader.GetStringOrDefault("Phone")
                        };
                        customerProfile.ClientCustomerId = datareader.GetStringOrDefault("PartnerAccountNumber");
                        customerProfile.CustomerType = CustomerType.ZEO;
                        customerProfile.IdExpirationDate = datareader.GetDateTimeOrDefault("GovtIDExpirationDate");
                        customerProfiles.Add(customerProfile);
                    }
                }
            }
            catch(Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.SEARCH_CUSTOMER_FAILED, ex);
            }

            return customerProfiles;
        }

        /// <summary>
        /// To get the customers from the ZEO database based on the PKY number, Last name, DOB or SSN.
        /// </summary>
        /// <param name="customers"></param>
        /// <param name="cxnCriteria"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public List<CustomerProfile> GetCustomersForAutoSearch(List<CustomerProfile> customers, CustomerSearchCriteria cxnCriteria, ZeoContext context)
        {
            List<CustomerProfile> customerProfiles = new List<CustomerProfile>();

            StringWriter writer = null;

            try
            {
                if (customers.Count != 0)
                    writer = GetPkyNumbersXml(customers);

                StoredProcedure customerProcedure = new StoredProcedure("usp_GetCustomersForAutoSearch");

                DataParameter[] dataParameters = new DataParameter[]
                {
                    new DataParameter("pkyNumbers", DbType.Xml)
                    {
                        Value = Convert.ToString(writer)
                    },
                    new DataParameter("ssn", DbType.String)
                    {
                        Value = cxnCriteria.SSN
                    },
                    new DataParameter("dob", DbType.DateTime)
                    {
                        Value = cxnCriteria.DateOfBirth
                    },
                    new DataParameter("lastName", DbType.String)
                    {
                        Value = cxnCriteria.Lastname
                    }
                };

                customerProcedure.WithParameters(dataParameters);

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(customerProcedure))
                {
                    while (datareader.Read())
                    {
                        CustomerProfile customerProfile = new CustomerProfile();
                        customerProfile.CustomerId = datareader.GetStringOrDefault("CustomerID");
                        customerProfile.FirstName = datareader.GetStringOrDefault("FirstName");
                        customerProfile.LastName = datareader.GetStringOrDefault("LastName");
                        customerProfile.DateOfBirth = datareader.GetDateTimeOrDefault("DOB");
                        customerProfile.Address = new Address
                        {
                            Address1 = datareader.GetStringOrDefault("Address1")
                        };
                        customerProfile.CardNumber = datareader.GetStringOrDefault("CardNumber");
                        customerProfile.IdNumber = datareader.GetStringOrDefault("IdNumber");
                        customerProfile.SSN = datareader.GetStringOrDefault("SSN");
                        customerProfile.Phone1 = new Phone
                        {
                            Number = datareader.GetStringOrDefault("Phone")
                        };
                        customerProfile.ClientCustomerId = datareader.GetStringOrDefault("PartnerAccountNumber");
                        customerProfile.CustomerType = CustomerType.ZEO;
                        customerProfile.IdExpirationDate = datareader.GetDateTimeOrDefault("GovtIDExpirationDate");
                        customerProfiles.Add(customerProfile);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.SEARCH_CUSTOMER_FAILED, ex);
            }

            return customerProfiles;
        }

        /// <summary>
        /// Register the Customer directly with out using the message broker.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string RegisterCustomer(CustomerProfile customer, ZeoContext context)
        {
            string partnerAccountNo = customer?.PartnerAccountNumber ?? string.Empty;

            try
            {
                RCIFCredential credential = RCIFCommon.GetCredential(context.ChannelPartnerId);

                bool isEWSScanningSuccess = IO.EWSScanningCustomerRegistration(customer, credential, context); //Pre scanning using the EWS Service

                if (!isEWSScanningSuccess)
                {
                    //update the flag in the database to of EWS Scanning to false.
                }

                partnerAccountNo = IO.RCIFCustomerRegistration(customer, credential, context); //RCIF CIF7450 call to register the customer

                if(!string.IsNullOrWhiteSpace(partnerAccountNo))
                    UpdateTcisAccount(context, partnerAccountNo);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;

                throw new CustomerException(CustomerException.CREATE_CUSTOMER_FAILED, ex);
            }

            return partnerAccountNo;
        }

        #endregion

        #region Private Methods

        private IIO GetIO()
        {
            string tcfProcessor = ConfigurationManager.AppSettings["TCFProcessor"].ToString();

            if (tcfProcessor.ToUpper() == "IO")
                return new IO();
            else
                return new SimulatorIO();
        }

        private long UpdateAccount(TCISAccount account)
        {
            StoredProcedure coreCustomerProcedure = new StoredProcedure("usp_TCF_UpdatetTCISAccount");

            coreCustomerProcedure.WithParameters(InputParameter.Named("PartnerAccountNumber").WithValue(account.PartnerAccountNumber));
            coreCustomerProcedure.WithParameters(InputParameter.Named("ProfileStatus").WithValue((int)account.ProfileStatus));
            coreCustomerProcedure.WithParameters(InputParameter.Named("DTTerminalLastModified").WithValue(account.DTTerminalLastModified));
            coreCustomerProcedure.WithParameters(InputParameter.Named("DTServerLastModified").WithValue(account.DTServerLastModified));
            coreCustomerProcedure.WithParameters(InputParameter.Named("TCISAccountID").WithValue(account.Id));
            coreCustomerProcedure.WithParameters(InputParameter.Named("RelationshipAccountNumber").WithValue(account.RelationshipAccountNumber));
            //update the TCFCustInd to zero after successful customer registration
            coreCustomerProcedure.WithParameters(InputParameter.Named("TcfCustInd").WithValue(account.TcfCustInd));
            int rowCount = DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(coreCustomerProcedure);

            long customerId = 0;

            customerId = rowCount > 0 ? Convert.ToInt64(account.Id) : 0;

            return customerId;
        }

        /// <summary>
        /// This method is used to get the TCIS account.
        /// </summary>
        /// <param name="cxnAccountId"></param>
        /// <returns></returns>
        private TCISAccount GetClientAccount(long cxnAccountId)
        {
            TCISAccount account = null;

            StoredProcedure coreCustomerProcedure = new StoredProcedure("usp_TCF_GetTCISAccountById");
            coreCustomerProcedure.WithParameters(InputParameter.Named("Id").WithValue(cxnAccountId));

            using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(coreCustomerProcedure))
            {
                while (datareader.Read())
                {
                    account = new TCISAccount();
                    account.PartnerAccountNumber = datareader.GetStringOrDefault("PartnerAccountNumber");
                    account.ProfileStatus = GetProfileStatus(datareader.GetStringOrDefault("ProfileStatus"));
                    account.TcfCustInd = datareader.GetBooleanOrDefault("TcfCustInd");
                    account.RelationshipAccountNumber = datareader.GetStringOrDefault("RelationshipAccountNumber");
                    account.CustomerID = datareader.GetInt64OrDefault("CustomerID");
                    account.Id = datareader.GetInt64OrDefault("TCISAccountID");
                }
            }

            return account;
        }

        /// <summary>
        /// This method is used to get the customer profile.
        /// </summary>
        /// <param name="datareader"></param>
        /// <returns></returns>
        private CustomerProfile GetProfile(IDataReader datareader)
        {
            CustomerProfile customerProfile = null;
            while (datareader.Read())
            {
                customerProfile = new CustomerProfile();
                customerProfile.Id = datareader.GetInt64OrDefault("TCISAccountID");
                customerProfile.CustomerId = datareader.GetStringOrDefault("CustomerID");
                customerProfile.CustomerSessionId = datareader.GetInt64OrDefault("CustomerSessionID");
                customerProfile.ProfileStatus = GetProfileStatus(datareader.GetStringOrDefault("ProfileStatus"));
                customerProfile.CustInd = datareader.GetBooleanOrDefault("TcfCustInd");
                customerProfile.PartnerAccountNumber = datareader.GetStringOrDefault("PartnerAccountNumber");
                customerProfile.RelationshipAccountNumber = datareader.GetStringOrDefault("RelationshipAccountNumber");
            }
            return customerProfile;
        }

        /// <summary>
        /// This method is used to add a TCIS account.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        private long AddAccount(TCISAccount account)
        {
            long customerId = 0;

            StoredProcedure coreCustomerProcedure = new StoredProcedure("usp_TCF_AddTCISAccount");

            coreCustomerProcedure.WithParameters(InputParameter.Named("PartnerAccountNumber").WithValue(account.PartnerAccountNumber));
            coreCustomerProcedure.WithParameters(InputParameter.Named("RelationshipAccountNumber").WithValue(account.RelationshipAccountNumber));
            coreCustomerProcedure.WithParameters(InputParameter.Named("ProfileStatus").WithValue((int)account.ProfileStatus));
            coreCustomerProcedure.WithParameters(InputParameter.Named("DTTerminalCreate").WithValue(account.DTTerminalCreate));
            coreCustomerProcedure.WithParameters(InputParameter.Named("BankId").WithValue(account.CustomerBankId));
            coreCustomerProcedure.WithParameters(InputParameter.Named("BranchId").WithValue(account.CustomerBranchId));
            coreCustomerProcedure.WithParameters(InputParameter.Named("TcfCustInd").WithValue(account.TcfCustInd));
            coreCustomerProcedure.WithParameters(InputParameter.Named("DTServerCreate").WithValue(account.DTServerCreate));
            coreCustomerProcedure.WithParameters(InputParameter.Named("CustomerID").WithValue(account.CustomerID));
            coreCustomerProcedure.WithParameters(InputParameter.Named("CustomerSessionID").WithValue(account.CustomerSessionId));

            using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(coreCustomerProcedure))
            {
                while (datareader.Read())
                {
                    customerId = datareader.GetInt64OrDefault("TCISAccountID");
                }
            }

            return customerId;
        }

        /// <summary>
        /// This method is used to validate client customer Id.
        /// </summary>
        /// <param name="clientCustId"></param>
        /// <returns></returns>
        private bool ValidateClientCustId(string clientCustId)
        {
            long number = 0;
            Int64.TryParse(clientCustId, out number);
            return number > 0;
        }

        private StringWriter GetPkyNumbersXml(List<CustomerProfile> customers)
        {
            DataTable dataTable = new DataTable();
            DataRow row;
            dataTable.Columns.Add("PKYNumber", typeof(string));

            for (int i = 0; i < customers.Count; i++)
            {
                row = dataTable.NewRow();
                row["PKYNumber"] = customers[i].ClientCustomerId;
                dataTable.Rows.Add(row);
            }
            StringWriter writer = new StringWriter();
            dataTable.TableName = "PKYTable";
            dataTable.WriteXml(writer);
            return writer;
        }

        private void UpdateTcisAccount(ZeoContext context, string partnerAccountNo)
        {
            if (ValidateClientCustId(partnerAccountNo))
            {
                TCISAccount account = GetClientAccount(context.CXNAccountId);

                account.ProfileStatus = ProfileStatus.Active;
                account.PartnerAccountNumber = partnerAccountNo;
                account.DTTerminalLastModified = GetTimeZoneTime(context.TimeZone);
                account.DTServerLastModified = DateTime.Now;
                //Update CXN.
                UpdateAccount(account);
            }
        }


        #endregion

    }
}
