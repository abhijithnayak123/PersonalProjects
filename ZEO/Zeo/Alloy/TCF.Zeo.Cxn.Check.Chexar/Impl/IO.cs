using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Net;
using TCF.Zeo.Cxn.Check.Chexar.Contract;
using TCF.Zeo.Cxn.Check.Chexar.Data;
using TCF.Zeo.Cxn.Check.Chexar.Data.Exceptions;
using TCF.Zeo.Cxn.Check.Data.Exceptions;
using TCF.Zeo.Common.Util;
using System.Diagnostics;
using TCF.Zeo.Common.Logging.Impl;

namespace TCF.Zeo.Cxn.Check.Chexar.Impl
{
    public class IO : IIO
    {

        public ChexarLogin GetChexarLogin(string baseURL, string companyId, string companyUname, string companyPwd, string employeeUname, string employeePwd)
        {

            ChexarCommonService.CommonService commonSvc = new ChexarCommonService.CommonService();
            commonSvc.Url = baseURL + "commonservice.asmx";

            ChexarLogin chexarLogin = new ChexarLogin
            {
                URL = baseURL
            };

            // Get company token
            try
            {
                chexarLogin.CompanyToken = commonSvc.CompanyLogin(companyId, companyUname, companyPwd);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw new CheckException(CheckException.CHECK_LOGIN_FAILED, ex);
            }

            if (chexarLogin.CompanyToken == string.Empty)
                throw new CheckException(CheckException.CHECK_LOGIN_FAILED);


            // Use the company token to retrieve employee ID and branch ID
            try
            {
                int errorCode = 0;
                string errorMessage = string.Empty;
                string elementName = "employee";

                string xmlResult = commonSvc.EmpLogin(chexarLogin.CompanyToken, employeeUname, employeePwd);
                XElement employee = GetErrorCodeAndMessage(xmlResult, elementName, out errorCode, out errorMessage);


                if (errorMessage.ToLower() == "success")
                {
                    chexarLogin.EmployeeId = ChexarXMLHelper.GetIntToken(employee, "empid");
                    chexarLogin.IngoBranchId = ChexarXMLHelper.GetIntToken(employee, "branchid");
                }
                else
                {
                    throw new ChexarProviderException(String.Format("{0:D4}", errorCode), errorMessage);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleException(ex);
                throw new CheckException(CheckException.CHECK_EMPLOYEE_LOGIN_ERROR, ex);
            }

            return chexarLogin;
        }

        #region Creating, polling and updating transactions
        public ChexarTicketStatus GetWaitTime(ChexarLogin login, int ticketNo)
        {
            try
            {
                ChexarTicketStatus ticketStatus = null;

                string result = getCallCenterSvc(login.URL).TicketStatus(login.CompanyToken, ticketNo);

                int errorCode = 0;
                string message = string.Empty;
                string elementName = "callcenter";
                XElement ticket = GetErrorCodeAndMessage(result, elementName, out errorCode, out message);

                if (message.ToLower() == "success")
                    ticketStatus = new ChexarTicketStatus(ticket);

                return ticketStatus;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleException(ex);
                throw new CheckException(CheckException.CHECK_GET_WAIT_TIME_FAILED, ex);
            }
        }

        //@@@ below method is not using.
        public List<ChexarPendingTransaction> GetPendingTransactions(ChexarLogin login)
        {
            List<ChexarPendingTransaction> pendingTransactions = new List<ChexarPendingTransaction>();

            string result = getTxnSvc(login.URL).OnHoldList(login.CompanyToken, login.EmployeeId, login.IngoBranchId);

            XDocument x = ParseXDocument(result);

            var pendingList = x.Element("transaction").Elements("invoice");

            foreach (XElement trans in pendingList)
                pendingTransactions.Add(new ChexarPendingTransaction(trans));


            return pendingTransactions;

        }

        //@@@ below method is not using.
        public int GetOpenTransactions(ChexarLogin login, out List<ChexarTransactionIO> openTransactions)
        {
            openTransactions = new List<ChexarTransactionIO>();

            string result = getTxnSvc(login.URL).GetOpenTrans(login.CompanyToken);

            XDocument x = ParseXDocument(result);

            var transList = x.Element("transaction").Elements("trans");

            foreach (XElement trans in transList)
                openTransactions.Add(new ChexarTransactionIO(trans));


            return 0;
        }

        //@@@ below method is not using.
        public List<ChexarInvoiceCheck> GetTransactionDetails(ChexarLogin login, int invoiceNo)
        {

            string result = getTxnSvc(login.URL).InvoiceGet(login.CompanyToken, invoiceNo);
            List<ChexarInvoiceCheck> checks = new List<ChexarInvoiceCheck>();

            XDocument x = ParseXDocument(result);

            XElement transaction = x.Element("transaction");

            if (ChexarXMLHelper.GetXMLValue(transaction, "result").ToLower() == "success")
            {
                XElement invoiceHeader = transaction.Element("header");

                foreach (XElement check in transaction.Elements("detail"))
                    checks.Add(new ChexarInvoiceCheck(invoiceHeader, check));
            }

            return checks;
        }

        public bool CloseTransaction(ChexarLogin login, int invoiceNo, out string errorMessage)
        {
            try
            {
                string result = getTxnSvc(login.URL).SetPrinted(login.CompanyToken, invoiceNo);
                int errorCode;
                string elementName = "transaction";
                GetErrorCodeAndMessage(result, elementName, out errorCode, out errorMessage);

                if (ResultSuccess(errorMessage))
                    return true;

                throw new ChexarProviderException(String.Format("{0:D4}", errorCode), errorMessage);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleException(ex);
                throw new CheckException(CheckException.CHECK_CLOSE_TRANSACTION_FAILED, ex);
            }
        }

        public ChexarInvoiceCheck GetTransactionStatus(ChexarLogin login, int invoiceNo, out string errorMessage)
        {
            try
            {
                ChexarInvoiceCheck check = null;

                string result = getTxnSvc(login.URL).ReOpen(login.CompanyToken, invoiceNo);

                int errorCode;
                string elementName = "transaction";
                XElement transaction = GetErrorCodeAndMessage(result, elementName, out errorCode, out errorMessage);

                if (ResultSuccess(errorMessage))
                {
                    XElement invoiceHeader = transaction.Element("header");
                    check = new ChexarInvoiceCheck(invoiceHeader, transaction.Elements("detail").First());
                }
                else
                    check = new ChexarInvoiceCheck();

                return check;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleException(ex);
                throw new CheckException(CheckException.CHECK_GET_TRANSACTION_FAILED, ex);
            }
        }

        public ChexarNewInvoiceResult CreateTransaction(ChexarLogin login, int badge, decimal amount, DateTime checkDate, int checkType, string checkNum, string routingNum, string accountNum, string micr, byte[] checkImgFront, byte[] checkImgBack, string checkImageFormat, byte[] checkImgFrontTIF, byte[] checkImgBackTIF, double[] geocodeLatLong, out int errorCode, out string errorMessage)
        {
            try
            {
                RNGCryptoServiceProvider _rngCryptoServiceProvider = new RNGCryptoServiceProvider();
                byte[] _uint32Buffer = new byte[4];
                _rngCryptoServiceProvider.GetBytes(_uint32Buffer);

                int fileName = BitConverter.ToInt32(_uint32Buffer, 0) & 0x7FFFFFFF;

                string response;

                if (geocodeLatLong == null || geocodeLatLong.Length != 2)
                    response = getTxnSvc(login.URL).InvoiceDetailAdd(login.CompanyToken, badge, login.IngoBranchId, login.EmployeeId, checkType, (double)amount, 0.00, string.Empty,
                        checkDate, string.Empty, string.Empty, VerifyMICR(micr), 0.00, checkImgFront, fileName + "front." + checkImageFormat, checkImgBack, fileName + "back." + checkImageFormat, checkImgFrontTIF, checkImgBackTIF, string.Empty);
                else
                    response = getTxnSvc(login.URL).InvoiceDetailAddGeocode(login.CompanyToken, badge, login.IngoBranchId, login.EmployeeId, checkType, (double)amount, 0.00, string.Empty,
                        checkDate, string.Empty, string.Empty, VerifyMICR(micr), 0.00, checkImgFront, fileName + "front." + checkImageFormat, checkImgBack, fileName + "back." + checkImageFormat, checkImgFrontTIF, checkImgBackTIF, string.Empty, (float)geocodeLatLong[0], (float)geocodeLatLong[1]);


                string elementName = "transaction";

                XElement transaction = GetErrorCodeAndMessage(response, elementName, out errorCode, out errorMessage);

                if (errorCode == 0)
                {
                    string txnPostTmpResponse = string.Empty;
                    int tmpTrandetId = ChexarXMLHelper.GetIntToken(transaction, "tmptrandetid");

                    try
                    {

                        txnPostTmpResponse = getTxnSvc(login.URL).PostTmp(login.CompanyToken, login.IngoBranchId, login.EmployeeId, badge);

                        transaction = GetErrorCodeAndMessage(txnPostTmpResponse, elementName, out errorCode, out errorMessage);

                        // Check if posttmp is successful and then create invoice
                        if (errorCode == 0)
                            return new ChexarNewInvoiceResult(transaction);

                        // INGO PostTmp method has failed; call InvoiceDetailDelete
                        else
                        {
                            string traceMessage = string.Format("PostTmp result : errorCode {0} , errorMessage : {1} ", errorCode, errorMessage);
                            // Call Transaction service InvoiceDetailDelete to avoid duplicate check error (AL-121)
                            InvoiceDetailDelete(login, getTxnSvc(login.URL), tmpTrandetId);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Call Transaction service InvoiceDetailDelete to avoid duplicate check error (AL-121)
                        InvoiceDetailDelete(login, getTxnSvc(login.URL), tmpTrandetId);

                        if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                        HandleException(ex);
                        throw new CheckException(CheckException.CHECK_TRANSACTION_CREATE_FAILED, ex);
                    }

                }

                return new ChexarNewInvoiceResult();
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleException(ex);
                throw new CheckException(CheckException.CHECK_TRANSACTION_CREATE_FAILED, ex);
            }
        }

        private bool InvoiceDetailDelete(ChexarLogin login, ChexarTransactionService.TransactionService txnSvc, int tmpTrandetId)
        {
            // Execute INGO Transaction service's InvoiceDetailDelete method to delete record from tmptrandet (AL-121)
            string invoiceDeleteResponse = txnSvc.InvoiceDetailDelete(login.CompanyToken, tmpTrandetId);

            // Parse response string to xml document
            XDocument deleteResponseXmlDocument = ParseXDocument(invoiceDeleteResponse);
            //Get transaction element from the xml response 
            XElement transaction = deleteResponseXmlDocument.Element("transaction");

            //Get errorcode and error message from response xml
            int errorCode = ChexarXMLHelper.GetIntToken(transaction, "errorcode");
            string errorMessage = ChexarXMLHelper.GetXMLValue(transaction, "result");

            //compose respose for trace			
            string traceMessage = string.Format("InvoiceDetailDelete result : {0}, tmpTrandetId : {1} ", errorMessage, tmpTrandetId);

            bool isDeleteSuccessful = errorCode == 0 ? true : false;
            return isDeleteSuccessful;

        }

        public bool CancelTransaction(ChexarLogin login, int invoiceNum, out string errorMessage)
        {
            try
            {
                ChexarInvoiceCheck check = GetTransactionStatus(login, invoiceNum, out errorMessage);

                string response = string.Empty;
                if (check.Approved)
                {


                    response = getTxnSvc(login.URL).InvoiceVoid(login.CompanyToken, invoiceNum);

                    int errorCode = 0;
                    string elementName = "transaction";
                    GetErrorCodeAndMessage(response, elementName, out errorCode, out errorMessage);

                    if (!ResultSuccess(errorMessage))
                        return false;
                }

                if (CloseTransaction(login, invoiceNum, out errorMessage))
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleException(ex);
                throw new CheckException(CheckException.CHECK_CANCEL_TRANSACTION_FAILED, ex);
            }
        }

        public ChexarMICRDetails GetMICRDetails(ChexarLogin login, int invoiceNum, out string errorMessage)
        {
            try
            {

                string response = getTxnSvc(login.URL).IsKnownMicr(login.CompanyToken, invoiceNum);

                int errorCode = 0;
                string elementName = "transaction";
                XElement transaction = GetErrorCodeAndMessage(response, elementName, out errorCode, out errorMessage);


                if (ResultSuccess(errorMessage) && transaction.Element("invoicedet") != null)
                    return new ChexarMICRDetails(transaction.Element("invoicedet"));
                else
                {
                    throw new ChexarProviderException(String.Format("{0:D4}", errorCode), errorMessage);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleException(ex);
                throw new CheckException(CheckException.GET_CHECK_MICR_DETAILS_FAILED, ex);
            }
        }
        #endregion

        #region Customer add, edit and lookup methods
        public int RegisterNewCustomer(ChexarLogin login, ChexarCustomerIO customerInfo, out string errorMessage)
        {
            string response = string.Empty;
            int badge = 0;
            errorMessage = string.Empty;
            int errorCode = 0;
            try
            {

                response = getCustSvc(login.URL).KnownCustAddAll(login.CompanyToken, customerInfo.FName, customerInfo.LName, customerInfo.ITIN, customerInfo.SSN,
                    customerInfo.DateOfBirth.GetValueOrDefault(), customerInfo.Address1, customerInfo.Address2, customerInfo.City, customerInfo.State, customerInfo.Zip, customerInfo.Phone,
                    string.Empty, 222, string.Empty, login.IngoBranchId, login.EmployeeId, 1, customerInfo.Occupation, customerInfo.Employer, string.Empty, string.Empty,
                    (int)customerInfo.IDType, customerInfo.IDCountry, customerInfo.IDExpDate, customerInfo.GovernmentId, 0, string.Empty, new DateTime(1900, 1, 1), string.Empty,
                    customerInfo.CardNumber.ToString(), customerInfo.EmployerPhone, string.Empty, string.Empty, string.Empty, string.Empty, customerInfo.CustomerScore);

                string elementName = "customer";
                XElement d = GetErrorCodeAndMessage(response, elementName, out errorCode, out errorMessage);

                if (errorMessage.ToLower() == "success")
                    badge = ChexarXMLHelper.GetIntToken(d, "BadgeNo");

                if (badge == 0)
                    throw new ChexarProviderException(String.Format("{0:D4}", errorCode), errorMessage);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleException(ex);
                errorMessage = ex.Message;
                throw new CheckException(CheckException.CHECK_REGISTER_FAILED, ex);
            }
            return badge;
        }

        // @@@ This method is not used
        public bool UpdateCustomer(ChexarLogin login, ChexarCustomerIO customerInfo, out string errorMessage)
        {
            string response = string.Empty;
            errorMessage = string.Empty;

            try
            {

                response = getCustSvc(login.URL).CustomerUpdateAll(login.CompanyToken, customerInfo.FName, customerInfo.LName, customerInfo.ITIN, customerInfo.SSN,
                    customerInfo.DateOfBirth.GetValueOrDefault(), customerInfo.Address1, customerInfo.Address2, customerInfo.City, customerInfo.State, customerInfo.Zip, customerInfo.Phone,
                    string.Empty, 222, string.Empty, 0, login.EmployeeId, 1, 116, customerInfo.Occupation, customerInfo.Employer, string.Empty, string.Empty,
                    (int)customerInfo.IDType, customerInfo.IDCountry, customerInfo.IDExpDate, customerInfo.GovernmentId, 0, string.Empty, new DateTime(1900, 1, 1), string.Empty,
                    customerInfo.CardNumber.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, customerInfo.Badge);

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }

            bool chexarResult = false;
            try
            {
                XDocument x = ParseXDocument(response);
                XElement customer = x.Element("customer");
                errorMessage = ChexarXMLHelper.GetXMLValue(customer, "result");
                chexarResult = ResultSuccess(errorMessage);
            }
            catch (Exception ex)
            {
                errorMessage = "Could not process Chexar response.";
            }

            return chexarResult;
        }

        // @@@ This method is not used
        public int GetCustomerIdByBadge(ChexarLogin login, int badgeNum)
        {

            string response = getCustSvc(login.URL).SearchBadge(login.CompanyToken, badgeNum, string.Empty);

            XDocument x = ParseXDocument(response);

            XElement customer = x.Element("customer").Elements("matches").Elements("match").First();

            return ChexarXMLHelper.GetIntToken(customer, "customerid");
        }

        // @@@ This method is not used
        public ChexarCustomerIO FindCustomerById(ChexarLogin login, int customerId)
        {

            string response = getCustSvc(login.URL).SearchCustID(login.CompanyToken, customerId, 1);

            XDocument x = ParseXDocument(response);

            return new ChexarCustomerIO(x.Element("customer"));
        }

        // @@@ This method is not used
        public ChexarCustomerIO FindCustomerByBDay(ChexarLogin login, DateTime dateOfBirth)
        {

            //string response = _ChexarCustomerService.SearchCustBDay(_ChexarCompanyToken, dateOfBirth, 1);
            string response = getCustSvc(login.URL).SearchBadge(login.CompanyToken, 0, dateOfBirth.ToString("MM/dd/yyyy"));

            XDocument x = ParseXDocument(response);

            return new ChexarCustomerIO(x.Element("customer"));
        }

        // @@@ This method is not used
        public ChexarCustomerIO FindCustomerByName(ChexarLogin login, string firstName, string lastName)
        {
            string response = getCustSvc(login.URL).SearchCustName(login.CompanyToken, firstName, lastName, 1);

            XDocument x = ParseXDocument(response);

            return new ChexarCustomerIO(x.Element("customer"));
        }

        // @@@ This method is not used
        public ChexarCustomerIO FindCustomerByPhone(ChexarLogin login, string phoneNumber)
        {
            phoneNumber = phoneNumber.Replace("(", "").Replace(")", "").Replace("-", "");

            phoneNumber = phoneNumber.Insert(3, "-").Insert(7, "-");

            string response = string.Empty;
            int chexarsuccesscode = 0;

            ChexarCustomerService.CustService custSvc = getCustSvc(login.URL);

            response = custSvc.SearchBadge(login.CompanyToken, 0, phoneNumber);

            XDocument x = ParseXDocument(response);

            if (x.Element("customer").Element("matches").Element("match") == null || Convert.ToInt32(x.Element("customer").Element("errorcode").Value) != chexarsuccesscode)
            {
                phoneNumber = phoneNumber.Replace("-", "");
                response = custSvc.SearchBadge(login.CompanyToken, 0, phoneNumber);
                x = ParseXDocument(response);
            }

            if (x.Element("customer").Element("matches").Element("match") != null && Convert.ToInt32(x.Element("customer").Element("errorcode").Value) == chexarsuccesscode)
            {
                string customerId = x.Element("customer").Element("matches").Element("match").Element("customerid").Value;
                int custId = 0;
                int.TryParse(customerId, out custId);
                return this.FindCustomerById(login, custId);
            }

            return null;
        }

        // @@@ This method is not used
        public ChexarCustomerIO FindCustomerBySSN(ChexarLogin login, string socialSecurityNumber)
        {
            socialSecurityNumber = socialSecurityNumber.Replace("-", "");


            string response = getCustSvc(login.URL).SearchBadge(login.CompanyToken, 0, socialSecurityNumber);
            XDocument x = ParseXDocument(response);

            return new ChexarCustomerIO(x.Element("customer"));
        }
        #endregion

        // @@@ below the data type lookup methods are not used
        #region Data Type lookup methods
        public void GetProductTypes(ChexarLogin login)
        {


            string response = getTxnSvc(login.URL).ProductTypeList(login.CompanyToken);


            XDocument x = ParseXDocument(response);
        }

        public void GetCountryList(ChexarLogin login)
        {

            string response = getCustSvc(login.URL).CountryList(login.CompanyToken);

            XDocument x = ParseXDocument(response.Remove(response.LastIndexOf("<customer>")) + "</customer>");
        }

        public void GetOnHoldCodes(ChexarLogin login)
        {

            string response = getCustSvc(login.URL).OnHoldCodeList(login.CompanyToken);

            XDocument x = ParseXDocument(response.Remove(response.LastIndexOf("<customer>")) + "</customer>");
        }

        public void GetIDTypes(ChexarLogin login)
        {
            string response = getCustSvc(login.URL).DocTypeList(login.CompanyToken, 1);

            XDocument x = ParseXDocument(response);
        }

        public void GetMembershipTypes(ChexarLogin login)
        {

            string response = getCustSvc(login.URL).MemberTypeList(login.CompanyToken);
            XDocument x = ParseXDocument(response);
        }

        public void GetProductList(ChexarLogin login)
        {

            string response = getTxnSvc(login.URL).ProductList(login.CompanyToken, 2);
            XDocument x = ParseXDocument(response);
        }
        #endregion

        #region private methods
        private bool ResultSuccess(string result)
        {
            return result.ToLower().Contains("success");
        }

        private XDocument ParseXDocument(string x)
        {
            return XDocument.Parse(x.Replace("&", "&amp;"));
        }

        private ChexarTransactionService.TransactionService getTxnSvc(string url)
        {
            ChexarTransactionService.TransactionService txnSvc = new ChexarTransactionService.TransactionService();
            txnSvc.Url = url + "transactionservice.asmx";
            txnSvc.Timeout = 90 * 1000;
            return txnSvc;
        }

        private ChexarCustomerService.CustService getCustSvc(string url)
        {
            ChexarCustomerService.CustService txnSvc = new ChexarCustomerService.CustService();
            txnSvc.Url = url + "custservice.asmx";
            return txnSvc;
        }

        private ChexarCallCenterService.CallCenterService getCallCenterSvc(string url)
        {
            ChexarCallCenterService.CallCenterService callCenterSvc = new ChexarCallCenterService.CallCenterService();
            callCenterSvc.Url = url + "callcenterservice.asmx";
            return callCenterSvc;
        }

        private string VerifyMICR(string micr)
        {
            string finalMicr = micr;
            string _micr = micr.Replace("?", string.Empty);
            if (string.IsNullOrWhiteSpace(_micr))
            {
                finalMicr = "-1";
            }
            return finalMicr;
        }

        private XElement GetErrorCodeAndMessage(string response, string elementName, out int errorCode, out string errorMessage)
        {
            XDocument x = ParseXDocument(response);
            XElement transaction = x.Element(elementName);

            errorCode = ChexarXMLHelper.GetIntToken(transaction, "errorcode");
            errorMessage = ChexarXMLHelper.GetXMLValue(transaction, "result");            

            return transaction;
        }


        private void HandleException(Exception ex)
        {
            Exception faultException = ex as FaultException;
            if (faultException != null)
            {
                throw new ChexarProviderException(ChexarProviderException.PROVIDER_FAULT_ERROR, string.Empty, faultException);
            }
            Exception endpointException = ex as EndpointNotFoundException;
            if (endpointException != null)
            {
                throw new ChexarProviderException(ChexarProviderException.PROVIDER_ENDPOINTNOTFOUND_ERROR, string.Empty, endpointException);
            }
            Exception commException = ex as CommunicationException;
            if (commException != null)
            {
                throw new ChexarProviderException(ChexarProviderException.PROVIDER_COMMUNICATION_ERROR, string.Empty, commException);
            }
            Exception timeOutException = ex as TimeoutException;
            if (timeOutException != null)
            {
                throw new ChexarProviderException(ChexarProviderException.PROVIDER_TIMEOUT_ERROR, string.Empty, timeOutException);
            }
            Exception webException = ex as WebException;  // when net work down, it will capture all web related exceptions.
            if (webException != null)
            {
                throw new ChexarProviderException(ChexarProviderException.PROVIDER_ENDPOINTNOTFOUND_ERROR, string.Empty, webException);
            }
        }

        #endregion



    }
}
