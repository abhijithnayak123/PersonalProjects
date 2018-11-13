using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using MGI.Common.Util;
using System.Security.Cryptography;
using MGI.Common.TransactionalLogging.Data;

namespace ChexarIO
{
	public class ChexarWebSvc : IChexarWebSvc
	{
		public ChexarWebSvc()
		{
		}

        public NLoggerCommon NLogger = new NLoggerCommon();
		public TLoggerCommon MongoDBLogger { get; set; }

		public ChexarLogin GetChexarLogin(string baseURL, string companyId, string companyUname, string companyPwd, string employeeUname, string employeePwd)
		{
			NLogger.Info(string.Format("Connecting to: {0}", baseURL), this.GetType().Name);

			ChexarCommonService.CommonService commonSvc = new ChexarCommonService.CommonService();
			commonSvc.Url = baseURL + "commonservice.asmx";

			ChexarLogin chexarLogin = new ChexarLogin
			{
				URL = baseURL
			};

			// Get company token
			try
			{
				NLogger.Info("Attempting CompanyLogin:");
				//Trace.Indent();
				NLogger.Info(string.Format("CompanyId: {0}", companyId));
				NLogger.Info(string.Format("Username: {0}", companyUname));
				NLogger.Info(string.Format("Password: {0}", companyPwd));
				//Trace.Unindent();

				#region AL-3371 Transactional Log User Story(Process check)
				List<string> details = new List<string>();
				details.Add("Company ID :" + Convert.ToString(companyId));
				details.Add("Company Uname :" + Convert.ToString(companyUname));
				details.Add("Company Pwd :" + Convert.ToString(companyPwd));

				MGIContext mgiContext = new MGIContext();
				MongoDBLogger.ListInfo<string>(0, details, "GetChexarLogin - REQUEST", AlloyLayerName.CXN,
					ModuleName.ProcessCheck, "GetChexarLogin - ChexarIO.ChexarWebSvc", mgiContext);
				#endregion

				chexarLogin.CompanyToken = commonSvc.CompanyLogin(companyId, companyUname, companyPwd);

				#region AL-3371 Transactional Log User Story(Process check)
				List<string> respDetails = new List<string>();
				details.Add("Company Token :" + Convert.ToString(chexarLogin.CompanyToken));
			

				MongoDBLogger.ListInfo<string>(0, details, "GetChexarLogin - RESPONSE", AlloyLayerName.CXN,
					ModuleName.ProcessCheck, "GetChexarLogin - ChexarIO.ChexarWebSvc", mgiContext);
				#endregion

				NLogger.Info(string.Format("Chexar Company token retrieved from Common Service is {0}", chexarLogin.CompanyToken), this.GetType().Name);
			}
			catch (Exception ex)
			{
				NLogger.Error(string.Format("EXCEPTION accessing Chexar Web Services : {0}", ex.Message), this.GetType().Name);

				#region AL-3371 Transactional Log User Story(Process check)
				List<string> details = new List<string>();
				details.Add("Base URL :" + Convert.ToString(baseURL));
				details.Add("Company ID :" + Convert.ToString(companyId));
				details.Add("Company Uname :" + Convert.ToString(companyUname));
				details.Add("Company Pwd :" + Convert.ToString(companyPwd));

				MongoDBLogger.ListError<string>(details, "GetChexarLogin", AlloyLayerName.CXN, ModuleName.ProcessCheck,
									"Error in GetChexarLogin - MGI.Cxn.Check.Certegy.Impl.Gateway", ex.Message, ex.StackTrace);
				#endregion


				throw ex;
			}

			if (chexarLogin.CompanyToken == string.Empty)
				throw new Exception("Chexar CompanyLogin failed");

			// Use the company token to retrieve employee ID and branch ID
			try
			{
				NLogger.Info("Attempting EmpLogin:");
				//Trace.Indent();
				NLogger.Info(string.Format("token: {0}", chexarLogin.CompanyToken));
				NLogger.Info(string.Format("Username: {0}", employeeUname));
				NLogger.Info(string.Format("Password: {0}", employeePwd));
				//Trace.Unindent();

				#region AL-3371 Transactional Log User Story(Process check) 
				List<string> details = new List<string>();
				details.Add("Token :" + Convert.ToString(chexarLogin.CompanyToken));
				details.Add("Username :" + Convert.ToString(employeeUname));
				details.Add("Password :" + Convert.ToString(employeePwd));

				MGIContext mgiContext = new MGIContext();
				MongoDBLogger.ListInfo<string>(0, details, "GetChexarLogin - REQUEST", AlloyLayerName.CXN,
					ModuleName.ProcessCheck, "GetChexarLogin Emp Login Request- ChexarIO.ChexarWebSvc", mgiContext);
				#endregion

				string xmlResult = commonSvc.EmpLogin(chexarLogin.CompanyToken, employeeUname, employeePwd);

				#region AL-3371 Transactional Log User Story(Process check)
				MongoDBLogger.Info<string>(0, xmlResult, "GetChexarLogin - RESPONSE", AlloyLayerName.CXN,
					ModuleName.ProcessCheck, "GetChexarLogin Emp Login Response - ChexarIO.ChexarWebSvc", mgiContext);
				#endregion

				XDocument x = ParseXDocument(xmlResult);
				NLogger.Info(x.ToString());

				XElement employee = x.Element("employee");

				if (ChexarXMLHelper.GetXMLValue(employee, "result").ToLower() == "success")
				{
					chexarLogin.EmployeeId = ChexarXMLHelper.GetIntToken(employee, "empid");
					chexarLogin.IngoBranchId = ChexarXMLHelper.GetIntToken(employee, "branchid");
				}
				else
				{
					NLogger.Info(string.Format("Employee login failed: {0}", ChexarXMLHelper.GetXMLValue(employee, "result")), this.GetType().Name);

					#region AL-3371 Transactional Log User Story(Process check)
					MongoDBLogger.Error<string>(ChexarXMLHelper.GetXMLValue(employee, "result"), "GetChexarLogin", AlloyLayerName.CXN, ModuleName.ProcessCheck,
										"Error in GetChexarLogin - ChexarIO.ChexarWebSvc", "Employee login failed", string.Empty);
					#endregion
					
					throw new Exception(string.Format("Employee login failed for '{0}': {1}", employeeUname, ChexarXMLHelper.GetXMLValue(employee, "result")));
				}
			}
			catch (Exception ex)
			{
				NLogger.Error(string.Format("EXCEPTION retrieving Chexar employee info from {0}: {1}", commonSvc.Url, ex.Message), this.GetType().Name);

				#region AL-3371 Transactional Log User Story(Process check) 
				MongoDBLogger.Error<ChexarLogin>(chexarLogin, "GetChexarLogin", AlloyLayerName.CXN, ModuleName.ProcessCheck,
									"Error in GetChexarLogin - ChexarIO.ChexarWebSvc", ex.Message, ex.StackTrace);
				#endregion
				
				throw ex;
			}

			return chexarLogin;
		}

		#region Creating, polling and updating transactions
		public ChexarTicketStatus GetWaitTime(ChexarLogin login, int ticketNo)
		{
			ChexarTicketStatus ticketStatus = null;

			#region AL-3371 Transactional Log User Story(Process check)
			List<string> details = new List<string>();
			details.Add("Token :" + (login != null ? Convert.ToString(login.CompanyToken) : string.Empty));
			details.Add("TicketNo :" + Convert.ToString(ticketNo));

			MGIContext mgiContext = new MGIContext();
			MongoDBLogger.ListInfo<string>(0, details, "GetWaitTime - REQUEST", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "GetWaitTime - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

			string result = getCallCenterSvc(login.URL).TicketStatus(login.CompanyToken, ticketNo);

			XDocument x = ParseXDocument(result);
			NLogger.Info(x.ToString());

			XElement ticket = x.Element("callcenter");

			if (ChexarXMLHelper.GetXMLValue(ticket, "result").ToLower() == "success")
				ticketStatus = new ChexarTicketStatus(ticket);


			#region AL-3371 Transactional Log User Story(Process check)
			MongoDBLogger.Info<ChexarTicketStatus>(0, ticketStatus, "GetWaitTime - RESPONSE", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "GetWaitTime - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion


			return ticketStatus;
		}

		public List<ChexarPendingTransaction> GetPendingTransactions(ChexarLogin login)
		{
			List<ChexarPendingTransaction> pendingTransactions = new List<ChexarPendingTransaction>();

			ChexarTransactionService.TransactionService txnService = getTxnSvc(login.URL);

			#region AL-3371 Transactional Log User Story(Process check)
			List<string> details = new List<string>();
			details.Add("Token :" + (login != null ? Convert.ToString(login.CompanyToken) : string.Empty));
			details.Add("Employee Id :" + (login != null ? Convert.ToString(login.EmployeeId) : string.Empty));
			details.Add("Ingo Branch Id :" + (login != null ? Convert.ToString(login.IngoBranchId) : string.Empty));

			MGIContext mgiContext = new MGIContext();
			MongoDBLogger.ListInfo<string>(0, details, "GetPendingTransactions - REQUEST", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "GetPendingTransactions - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

			string result = getTxnSvc(login.URL).OnHoldList(login.CompanyToken, login.EmployeeId, login.IngoBranchId);

			XDocument x = ParseXDocument(result);
			NLogger.Info(x.ToString());

			var pendingList = x.Element("transaction").Elements("invoice");

			foreach (XElement trans in pendingList)
				pendingTransactions.Add(new ChexarPendingTransaction(trans));

			#region AL-3371 Transactional Log User Story(Process check)
			MongoDBLogger.ListInfo<ChexarPendingTransaction>(0, pendingTransactions, "GetPendingTransactions - RESPONSE", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "GetPendingTransactions - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion


			return pendingTransactions;
		}

		public int GetOpenTransactions(ChexarLogin login, out List<ChexarTransaction> openTransactions)
		{
			openTransactions = new List<ChexarTransaction>();

			#region AL-3371 Transactional Log User Story(Process check)
			string token = login != null ? login.CompanyToken : string.Empty;

			MGIContext mgiContext = new MGIContext();
			MongoDBLogger.Info<string>(0, token, "GetOpenTransactions - REQUEST", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "GetOpenTransactions - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

			string result = getTxnSvc(login.URL).GetOpenTrans(login.CompanyToken);

			XDocument x = ParseXDocument(result);
			NLogger.Info(x.ToString());

			var transList = x.Element("transaction").Elements("trans");

			foreach (XElement trans in transList)
				openTransactions.Add(new ChexarTransaction(trans));

			#region AL-3371 Transactional Log User Story(Process check)
			MongoDBLogger.ListInfo<ChexarTransaction>(0, openTransactions, "GetOpenTransactions - RESPONSE", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "GetOpenTransactions - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

			return 0;
		}

		public List<ChexarInvoiceCheck> GetTransactionDetails(ChexarLogin login, int invoiceNo)
		{
			#region AL-3371 Transactional Log User Story(Process check)
			MGIContext mgiContext = new MGIContext();
			MongoDBLogger.Info<ChexarLogin>(0, login, "GetTransactionDetails - REQUEST", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "GetTransactionDetails - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

			string result = getTxnSvc(login.URL).InvoiceGet(login.CompanyToken, invoiceNo);
			List<ChexarInvoiceCheck> checks = new List<ChexarInvoiceCheck>();

			XDocument x = ParseXDocument(result);
			NLogger.Info(x.ToString());

			XElement transaction = x.Element("transaction");

			if (ChexarXMLHelper.GetXMLValue(transaction, "result").ToLower() == "success")
			{
				XElement invoiceHeader = transaction.Element("header");

				foreach (XElement check in transaction.Elements("detail"))
					checks.Add(new ChexarInvoiceCheck(invoiceHeader, check));
			}

			#region AL-3371 Transactional Log User Story(Process check) 
			MongoDBLogger.ListInfo<ChexarInvoiceCheck>(0, checks, "GetTransactionDetails - RESPONSE", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "GetTransactionDetails - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

			return checks;
		}

		public bool CloseTransaction(ChexarLogin login, int invoiceNo, out string errorMessage)
		{
			#region AL-3371 Transactional Log User Story(Process check)
			MGIContext mgiContext = new MGIContext();
			MongoDBLogger.Info<ChexarLogin>(0, login, "CloseTransaction - REQUEST", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "CloseTransaction - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

			string result = getTxnSvc(login.URL).SetPrinted(login.CompanyToken, invoiceNo);

			XDocument x = ParseXDocument(result);
			NLogger.Info(x.ToString());

			XElement transaction = x.Element("transaction");
			errorMessage = ChexarXMLHelper.GetXMLValue(transaction, "result");

			#region AL-3371 Transactional Log User Story(Process check)
			MongoDBLogger.Info<string>(0, result, "CloseTransaction - RESPONSE", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "CloseTransaction - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

			return ResultSuccess(errorMessage);
		}

		public ChexarInvoiceCheck GetTransactionStatus(ChexarLogin login, int invoiceNo, out string errorMessage)
		{
			ChexarInvoiceCheck check = null;

			#region AL-3371 Transactional Log User Story(Process check)
			MGIContext mgiContext = new MGIContext();
			MongoDBLogger.Info<ChexarLogin>(0, login, "GetTransactionStatus - REQUEST", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "GetTransactionStatus - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

			string result = getTxnSvc(login.URL).ReOpen(login.CompanyToken, invoiceNo);

			XDocument x = ParseXDocument(result);
			NLogger.Info(x.ToString());

			XElement transaction = x.Element("transaction");
			errorMessage = ChexarXMLHelper.GetXMLValue(transaction, "result");

			if (ResultSuccess(errorMessage))
			{
				XElement invoiceHeader = transaction.Element("header");
				check = new ChexarInvoiceCheck(invoiceHeader, transaction.Elements("detail").First());
			}
			else
				check = new ChexarInvoiceCheck();

			#region AL-3371 Transactional Log User Story(Process check)
			MongoDBLogger.Info<ChexarInvoiceCheck>(0, check, "GetTransactionStatus - RESPONSE", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "GetTransactionStatus - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

			return check;
		}

		public ChexarNewInvoiceResult CreateTransaction(ChexarLogin login, int badge, decimal amount, DateTime checkDate, int checkType, string checkNum, string routingNum, string accountNum, string micr, byte[] checkImgFront, byte[] checkImgBack, string checkImageFormat, byte[] checkImgFrontTIF, byte[] checkImgBackTIF, double[] geocodeLatLong, out int errorCode, out string errorMessage)
		{
			/****************************Begin TA-50 Changes************************************************/
			//      User Story Number: TA-50 | ALL |   Developed by: Sunil Shetty     Date: 03.03.2015
			//      Purpose: On Vera Code Scan, the below methods were found having Insufficient Entropy which caused due to usage random.next, we are now using RNGCryptoServiceProvider to get random number
			RNGCryptoServiceProvider _rngCryptoServiceProvider = new RNGCryptoServiceProvider();
			byte[] _uint32Buffer = new byte[4];
			_rngCryptoServiceProvider.GetBytes(_uint32Buffer);
			//maximum value of Int32 is 2,147,483,647, hexadecimal 0x7FFFFFFF			
			int fileName = BitConverter.ToInt32(_uint32Buffer, 0) & 0x7FFFFFFF;
			/****************************End TA-50 Changes************************************************/

			ChexarTransactionService.TransactionService txnSvc = getTxnSvc(login.URL);

			#region AL-3371 Transactional Log User Story(Process check)
			List<string> details = new List<string>();
			details.Add("Token :" + (login != null ? Convert.ToString(login.CompanyToken) : string.Empty));
			details.Add("Badge :" + Convert.ToString(badge));
			details.Add("IngoBranchId :" + (login != null ? Convert.ToString(login.IngoBranchId) : string.Empty));
			details.Add("EmployeeId :" + (login != null ? Convert.ToString(login.EmployeeId) : string.Empty));
			details.Add("Check Type :" + checkType);
			details.Add("Amount :" + Convert.ToString(amount));
			details.Add("CheckDate :" + Convert.ToString(checkDate.ToString("MMddyyyy")));
			details.Add("MICR :" + Convert.ToString(micr));

			MGIContext mgiContext = new MGIContext();
			MongoDBLogger.ListInfo<string>(0, details, "GetWaitTime - REQUEST", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "GetWaitTime - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

			string response;

			if (geocodeLatLong == null || geocodeLatLong.Length != 2)
				response = txnSvc.InvoiceDetailAdd(login.CompanyToken, badge, login.IngoBranchId, login.EmployeeId, checkType, (double)amount, 0.00, string.Empty,
					checkDate, string.Empty, string.Empty, VerifyMICR(micr), 0.00, checkImgFront, fileName + "front." + checkImageFormat, checkImgBack, fileName + "back." + checkImageFormat, checkImgFrontTIF, checkImgBackTIF, string.Empty);
			else
				response = getTxnSvc(login.URL).InvoiceDetailAddGeocode(login.CompanyToken, badge, login.IngoBranchId, login.EmployeeId, checkType, (double)amount, 0.00, string.Empty,
                    checkDate, string.Empty, string.Empty, VerifyMICR(micr), 0.00, checkImgFront, fileName + "front." + checkImageFormat, checkImgBack, fileName + "back." + checkImageFormat, checkImgFrontTIF, checkImgBackTIF, string.Empty, (float)geocodeLatLong[0], (float)geocodeLatLong[1]);

			#region AL-3371 Transactional Log User Story(Process check)
			MongoDBLogger.Info<string>(0, response, "GetWaitTime - RESPONSE", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "GetWaitTime - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

			XDocument x = ParseXDocument(response);
			NLogger.Info(x.ToString());
			XElement transaction = x.Element("transaction");

			errorCode = ChexarXMLHelper.GetIntToken(transaction, "errorcode");
			errorMessage = ChexarXMLHelper.GetXMLValue(transaction, "result");

			if (errorCode == 0)
			{
				string txnPostTmpResponse = string.Empty;
				int tmpTrandetId = ChexarXMLHelper.GetIntToken(transaction, "tmptrandetid");

				try
				{
					NLogger.Info("Begin INGO service call PostTmp");

					txnPostTmpResponse = txnSvc.PostTmp(login.CompanyToken, login.IngoBranchId, login.EmployeeId, badge);

					NLogger.Info("Completed INGO service call PostTmp");

					x = ParseXDocument(txnPostTmpResponse);
					NLogger.Info(x.ToString());

					transaction = x.Element("transaction");
					errorCode = ChexarXMLHelper.GetIntToken(transaction, "errorcode");
					errorMessage = ChexarXMLHelper.GetXMLValue(transaction, "result");

					// Check if posttmp is successful and then create invoice
					if (errorCode == 0)
						return new ChexarNewInvoiceResult(transaction);

					// INGO PostTmp method has failed; call InvoiceDetailDelete
					else
					{
						NLogger.Info("INGO Service PostTmp method failed");
						string traceMessage = string.Format("PostTmp result : errorCode {0} , errorMessage : {1} ", errorCode, errorMessage);
						NLogger.Info(traceMessage);
						// Call Transaction service InvoiceDetailDelete to avoid duplicate check error (AL-121)
						InvoiceDetailDelete(login, txnSvc, tmpTrandetId);
					}
				}
				catch (Exception ex)
				{
					NLogger.Error("Exception caught: INGO Service PostTmp method");
					NLogger.Error(ex.Message);
					// Call Transaction service InvoiceDetailDelete to avoid duplicate check error (AL-121)
					InvoiceDetailDelete(login, txnSvc, tmpTrandetId);

					#region AL-3371 Transactional Log User Story(Process check) 
					MongoDBLogger.Error<string>(response, "GetWaitTime", AlloyLayerName.CXN, ModuleName.ProcessCheck,
										"Error in GetWaitTime - ChexarIO.ChexarWebSvc", ex.Message, ex.StackTrace);
					#endregion


					throw;	
				}

			}

			return new ChexarNewInvoiceResult();
		}

		private bool InvoiceDetailDelete(ChexarLogin login, ChexarTransactionService.TransactionService txnSvc, int tmpTrandetId)
		{
			NLogger.Info("Begin INGO service call InvoiceDetailDelete");

			#region AL-3371 Transactional Log User Story(Process check)
			List<string> details = new List<string>();
			details.Add("Token :" + (login != null ? Convert.ToString(login.CompanyToken) : string.Empty));
			details.Add("tmpTrandetId :" + Convert.ToString(tmpTrandetId));

			MGIContext mgiContext = new MGIContext();
			MongoDBLogger.ListInfo<string>(0, details, "InvoiceDetailDelete - REQUEST", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "InvoiceDetailDelete - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

			// Execute INGO Transaction service's InvoiceDetailDelete method to delete record from tmptrandet (AL-121)
			string invoiceDeleteResponse = txnSvc.InvoiceDetailDelete(login.CompanyToken, tmpTrandetId);

			#region AL-3371 Transactional Log User Story(Process check)
			MongoDBLogger.Info<string>(0, invoiceDeleteResponse, "InvoiceDetailDelete - RESPONSE", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "InvoiceDetailDelete - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion


			NLogger.Info("Completed INGO service call InvoiceDetailDelete");

			// Parse response string to xml document
			XDocument deleteResponseXmlDocument = ParseXDocument(invoiceDeleteResponse);

			NLogger.Info(deleteResponseXmlDocument.ToString(), "InvoiceDetailDelete Method Response");

			//Get transaction element from the xml response 
			XElement transaction = deleteResponseXmlDocument.Element("transaction");

			//Get errorcode and error message from response xml
			int errorCode = ChexarXMLHelper.GetIntToken(transaction, "errorcode");
			string errorMessage = ChexarXMLHelper.GetXMLValue(transaction, "result");

			//compose respose for trace			
			string traceMessage = string.Format("InvoiceDetailDelete result : {0}, tmpTrandetId : {1} ", errorMessage, tmpTrandetId);

			bool isDeleteSuccessful = errorCode == 0 ? true : false;

			NLogger.Debug(traceMessage);

			return isDeleteSuccessful;

		}

		public bool CancelTransaction(ChexarLogin login, int invoiceNum, out string errorMessage)
		{
			ChexarIO.ChexarInvoiceCheck check = GetTransactionStatus(login, invoiceNum, out errorMessage);

			string response = string.Empty;
			if (check.Approved)
			{

				#region AL-3371 Transactional Log User Story(Process check)
				List<string> details = new List<string>();
				details.Add("Token :" + (login != null ? Convert.ToString(login.CompanyToken) : string.Empty));
				details.Add("InvoiceNo :" + Convert.ToString(invoiceNum));

				MGIContext mgiContext = new MGIContext();
				MongoDBLogger.ListInfo<string>(0, details, "CancelTransaction - REQUEST", AlloyLayerName.CXN,
					ModuleName.ProcessCheck, "CancelTransaction - ChexarIO.ChexarWebSvc", mgiContext);
				#endregion


				response = getTxnSvc(login.URL).InvoiceVoid(login.CompanyToken, invoiceNum);


				#region AL-3371 Transactional Log User Story(Process check)
				MongoDBLogger.Info<string>(0, response, "CancelTransaction - RESPONSE", AlloyLayerName.CXN,
					ModuleName.ProcessCheck, "CancelTransaction - ChexarIO.ChexarWebSvc", mgiContext);
				#endregion


				XDocument x = ParseXDocument(response);
				NLogger.Info(x.ToString());
				XElement transaction = x.Element("transaction");

				errorMessage = ChexarXMLHelper.GetXMLValue(transaction, "result");

				if (!ResultSuccess(errorMessage))
					return false;
			}

			if (CloseTransaction(login, invoiceNum, out errorMessage))
				return true;

			return false;
		}

		public ChexarMICRDetails GetMICRDetails(ChexarLogin login, int invoiceNum, out string errorMessage)
		{
			#region AL-3371 Transactional Log User Story(Process check)
			List<string> details = new List<string>();
			details.Add("Token :" + (login != null ? Convert.ToString(login.CompanyToken) : string.Empty));
			details.Add("InvoiceNo :" + Convert.ToString(invoiceNum));

			MGIContext mgiContext = new MGIContext();
			MongoDBLogger.ListInfo<string>(0, details, "GetMICRDetails - REQUEST", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "GetMICRDetails - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

			string response = getTxnSvc(login.URL).IsKnownMicr(login.CompanyToken, invoiceNum);

			#region AL-3371 Transactional Log User Story(Process check) 
			MongoDBLogger.Info<string>(0, response, "GetMICRDetails - RESPONSE", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "GetMICRDetails - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

			XDocument x = ParseXDocument(response);
			NLogger.Info(x.ToString());
			XElement transaction = x.Element("transaction");
			errorMessage = ChexarXMLHelper.GetXMLValue(transaction, "result");

			if (ResultSuccess(errorMessage) && transaction.Element("invoicedet") != null)
				return new ChexarMICRDetails(transaction.Element("invoicedet"));
			else
			{
				NLogger.Info("IsKnownMicr() did not return a result");
				return null;
			}
		}
		#endregion

		#region Customer add, edit and lookup methods
		public int RegisterNewCustomer(ChexarLogin login, ChexarCustomerIO customerInfo, out string errorMessage)
		{
			string response = string.Empty;
			int badge = 0;
			errorMessage = string.Empty;

			try
			{
				#region AL-3371 Transactional Log User Story(Process check)
				List<string> details = new List<string>();
				details.Add("Token :" + (login != null ? Convert.ToString(login.CompanyToken) : string.Empty));
				details.Add("First Name :" + (customerInfo != null ? Convert.ToString(customerInfo.FName) : string.Empty));
				details.Add("Last Name :" + (customerInfo != null ? Convert.ToString(customerInfo.LName) : string.Empty));
				details.Add("ITIN :" + (customerInfo != null ? Convert.ToString(customerInfo.ITIN) : string.Empty));
				details.Add("SSN :" + (customerInfo != null ? Convert.ToString(customerInfo.SSN) : string.Empty));
				details.Add("City :" + (customerInfo != null ? Convert.ToString(customerInfo.City) : string.Empty));
				details.Add("IngoBranchId :" + (login != null ? Convert.ToString(login.IngoBranchId) : string.Empty));
				details.Add("GovernmentId :" + (customerInfo != null ? Convert.ToString(customerInfo.GovernmentId) : string.Empty));
				details.Add("CardNumber :" + (customerInfo != null ? Convert.ToString(customerInfo.CardNumber) : string.Empty));

				MGIContext mgiContext = new MGIContext();
				MongoDBLogger.ListInfo<string>(0, details, "RegisterNewCustomer - REQUEST", AlloyLayerName.CXN,
					ModuleName.ProcessCheck, "RegisterNewCustomer - ChexarIO.ChexarWebSvc", mgiContext);
				#endregion

				response = getCustSvc(login.URL).KnownCustAddAll(login.CompanyToken, customerInfo.FName, customerInfo.LName, customerInfo.ITIN, customerInfo.SSN,
					customerInfo.DateOfBirth.GetValueOrDefault(), customerInfo.Address1, customerInfo.Address2, customerInfo.City, customerInfo.State, customerInfo.Zip, customerInfo.Phone,
					string.Empty, 222, string.Empty, login.IngoBranchId, login.EmployeeId, 1, customerInfo.Occupation, customerInfo.Employer, string.Empty, string.Empty,
					(int)customerInfo.IDType, customerInfo.IDCountry, customerInfo.IDExpDate, customerInfo.GovernmentId, 0, string.Empty, new DateTime(1900, 1, 1), string.Empty,
					customerInfo.CardNumber.ToString(), customerInfo.EmployerPhone, string.Empty, string.Empty, string.Empty, string.Empty, customerInfo.CustomerScore);

				#region AL-3371 Transactional Log User Story(Process check)
				MongoDBLogger.Info<string>(0, response, "RegisterNewCustomer - RESPONSE", AlloyLayerName.CXN,
					ModuleName.ProcessCheck, "RegisterNewCustomer - ChexarIO.ChexarWebSvc", mgiContext);
				#endregion
			}
			catch (Exception ex)
			{
				NLogger.Error("EXCEPTION in RegisterNewCustomer() while sending request to Chexar: " + ex.Message, this.GetType().Name);
				errorMessage = ex.Message;
				return 0;
			}

			try
			{
				XDocument x = ParseXDocument(response);
				NLogger.Info(x.ToString());

				XElement d = x.Element("customer");

				errorMessage = ChexarXMLHelper.GetXMLValue(d, "result");

				if (errorMessage.ToLower() == "success")
					badge = ChexarXMLHelper.GetIntToken(d, "BadgeNo");
			}
			catch (Exception ex)
			{
				NLogger.Error("EXCEPTION in RegisterNewCustomer() while processing Chexar response: " + ex.Message, this.GetType().Name);
				errorMessage = "Could not process Chexar response.";
			}

			return badge;
		}

		public bool UpdateCustomer(ChexarLogin login, ChexarCustomerIO customerInfo, out string errorMessage)
		{
			string response = string.Empty;
			errorMessage = string.Empty;

			try
			{
				#region AL-3371 Transactional Log User Story(Process check)
				List<string> details = new List<string>();
				details.Add("Token :" + (login != null ? Convert.ToString(login.CompanyToken) : string.Empty));
				details.Add("First Name :" + (customerInfo != null ? Convert.ToString(customerInfo.FName) : string.Empty));
				details.Add("Last Name :" + (customerInfo != null ? Convert.ToString(customerInfo.LName) : string.Empty));
				details.Add("ITIN :" + (customerInfo != null ? Convert.ToString(customerInfo.ITIN) : string.Empty));
				details.Add("SSN :" + (customerInfo != null ? Convert.ToString(customerInfo.SSN) : string.Empty));
				details.Add("City :" + (customerInfo != null ? Convert.ToString(customerInfo.City) : string.Empty));
				details.Add("IngoBranchId :" + (login != null ? Convert.ToString(login.IngoBranchId) : string.Empty));
				details.Add("GovernmentId :" + (customerInfo != null ? Convert.ToString(customerInfo.GovernmentId) : string.Empty));
				details.Add("CardNumber :" + (customerInfo != null ? Convert.ToString(customerInfo.CardNumber) : string.Empty));

				MGIContext mgiContext = new MGIContext();
				MongoDBLogger.ListInfo<string>(0, details, "UpdateCustomer - REQUEST", AlloyLayerName.CXN,
					ModuleName.ProcessCheck, "UpdateCustomer - ChexarIO.ChexarWebSvc", mgiContext);
				#endregion

				response = getCustSvc(login.URL).CustomerUpdateAll(login.CompanyToken, customerInfo.FName, customerInfo.LName, customerInfo.ITIN, customerInfo.SSN,
					customerInfo.DateOfBirth.GetValueOrDefault(), customerInfo.Address1, customerInfo.Address2, customerInfo.City, customerInfo.State, customerInfo.Zip, customerInfo.Phone,
					string.Empty, 222, string.Empty, 0, login.EmployeeId, 1, 116, customerInfo.Occupation, customerInfo.Employer, string.Empty, string.Empty,
					(int)customerInfo.IDType, customerInfo.IDCountry, customerInfo.IDExpDate, customerInfo.GovernmentId, 0, string.Empty, new DateTime(1900, 1, 1), string.Empty,
					customerInfo.CardNumber.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, customerInfo.Badge);

				#region AL-3371 Transactional Log User Story(Process check)
				MongoDBLogger.Info<string>(0, response, "UpdateCustomer - RESPONSE", AlloyLayerName.CXN,
					ModuleName.ProcessCheck, "UpdateCustomer - ChexarIO.ChexarWebSvc", mgiContext);
				#endregion
			
			}
			catch (Exception ex)
			{
				NLogger.Error("EXCEPTION in UpdateCustomer() sending update to Chexar: " + ex.Message, this.GetType().Name);
				errorMessage = ex.Message;
				return false;
			}

			bool chexarResult = false;
			try
			{
				XDocument x = ParseXDocument(response);
				NLogger.Info(x.ToString());
				XElement customer = x.Element("customer");
				errorMessage = ChexarXMLHelper.GetXMLValue(customer, "result");
				chexarResult = ResultSuccess(errorMessage);
			}
			catch (Exception ex)
			{
				NLogger.Error("EXCEPTION in UpdateCustomer() while processing Chexar response: " + ex.Message, this.GetType().Name);
				errorMessage = "Could not process Chexar response.";
			}

			return chexarResult;
		}

		public int GetCustomerIdByBadge(ChexarLogin login, int badgeNum)
		{
			#region AL-3371 Transactional Log User Story(Process check)
			List<string> details = new List<string>();
			details.Add("Token :" + (login != null ? Convert.ToString(login.CompanyToken) : string.Empty));
			details.Add("Badge Num :" + Convert.ToString(badgeNum));

			MGIContext mgiContext = new MGIContext();
			MongoDBLogger.ListInfo<string>(0, details, "GetCustomerIdByBadge - REQUEST", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "GetCustomerIdByBadge - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

			string response = getCustSvc(login.URL).SearchBadge(login.CompanyToken, badgeNum, string.Empty);

			#region AL-3371 Transactional Log User Story(Process check)
			MongoDBLogger.Info<string>(0, response, "GetCustomerIdByBadge - RESPONSE", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "GetCustomerIdByBadge - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

			XDocument x = ParseXDocument(response);
			NLogger.Info(x.ToString());

			XElement customer = x.Element("customer").Elements("matches").Elements("match").First();

			return ChexarXMLHelper.GetIntToken(customer, "customerid");
		}

		public ChexarCustomerIO FindCustomerById(ChexarLogin login, int customerId)
		{
			#region AL-3371 Transactional Log User Story(Process check)
			List<string> details = new List<string>();
			details.Add("Token :" + (login != null ? Convert.ToString(login.CompanyToken) : string.Empty));
			details.Add("Customer Id :" + Convert.ToString(customerId));

			MGIContext mgiContext = new MGIContext();
			MongoDBLogger.ListInfo<string>(0, details, "FindCustomerById - REQUEST", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "FindCustomerById - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion
			
			string response = getCustSvc(login.URL).SearchCustID(login.CompanyToken, customerId, 1);

			#region AL-3371 Transactional Log User Story(Process check)
			MongoDBLogger.Info<string>(0, response, "FindCustomerById - RESPONSE", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "FindCustomerById - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

			XDocument x = ParseXDocument(response);
			NLogger.Info(x.ToString());

			return new ChexarCustomerIO(x.Element("customer"));
		}
        public ChexarCustomerIO FindCustomerByBDay(ChexarLogin login, DateTime dateOfBirth)
        {
			#region AL-3371 Transactional Log User Story(Process check) 
			List<string> details = new List<string>();
			details.Add("Token :" + (login != null ? Convert.ToString(login.CompanyToken) : string.Empty));
			details.Add("DateOfBirth :" + Convert.ToString(dateOfBirth.ToString("MM/dd/yyyy")));

			MGIContext mgiContext = new MGIContext();
			MongoDBLogger.ListInfo<string>(0, details, "FindCustomerByBDay - REQUEST", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "FindCustomerByBDay - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

            //string response = _ChexarCustomerService.SearchCustBDay(_ChexarCompanyToken, dateOfBirth, 1);
			string response = getCustSvc(login.URL).SearchBadge(login.CompanyToken, 0, dateOfBirth.ToString("MM/dd/yyyy"));

			#region AL-3371 Transactional Log User Story(Process check)
			MongoDBLogger.Info<string>(0, response, "FindCustomerByBDay - RESPONSE", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "FindCustomerByBDay - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

            XDocument x = ParseXDocument(response);
            NLogger.Info(x.ToString());

			return new ChexarCustomerIO(x.Element("customer"));
		}

        public ChexarCustomerIO FindCustomerByName(ChexarLogin login, string firstName,string lastName)
        {
			#region AL-3371 Transactional Log User Story(Process check) 
			List<string> details = new List<string>();
			details.Add("Token :" + (login != null ? Convert.ToString(login.CompanyToken) : string.Empty));
			details.Add("First Name :" + Convert.ToString(firstName));
			details.Add("Last Name :" + Convert.ToString(lastName));

			MGIContext mgiContext = new MGIContext();
			MongoDBLogger.ListInfo<string>(0, details, "FindCustomerByName - REQUEST", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "FindCustomerByName - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

			string response = getCustSvc(login.URL).SearchCustName(login.CompanyToken, firstName, lastName, 1);

			#region AL-3371 Transactional Log User Story(Process check)
			MongoDBLogger.Info<string>(0, response, "FindCustomerByName - RESPONSE", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "FindCustomerByName - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

            XDocument x = ParseXDocument(response);
            NLogger.Info(x.ToString());

			return new ChexarCustomerIO(x.Element("customer"));
		}

		public ChexarCustomerIO FindCustomerByPhone(ChexarLogin login, string phoneNumber)
		{
			phoneNumber = phoneNumber.Replace("(", "").Replace(")", "").Replace("-", "");

			phoneNumber = phoneNumber.Insert(3, "-").Insert(7, "-");

			string response = string.Empty;
			int chexarsuccesscode = 0;

			#region AL-3371 Transactional Log User Story(Process check)
			List<string> details = new List<string>();
			details.Add("Token :" + (login != null ? Convert.ToString(login.CompanyToken) : string.Empty));
			details.Add("Phone Number :" + Convert.ToString(phoneNumber));

			MGIContext mgiContext = new MGIContext();
			MongoDBLogger.ListInfo<string>(0, details, "FindCustomerByPhone - REQUEST", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "FindCustomerByPhone - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion
			ChexarCustomerService.CustService custSvc = getCustSvc(login.URL);

			response = custSvc.SearchBadge(login.CompanyToken, 0, phoneNumber);

			#region AL-3371 Transactional Log User Story(Process check)
			MongoDBLogger.Info<string>(0, response, "FindCustomerByName - RESPONSE", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "FindCustomerByName - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

            XDocument x = ParseXDocument(response);

			//Chexar does not save phone number in any specific format
			//Hence doing it second time.
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
			NLogger.Info(x.ToString());

			return null;
		}

		public ChexarCustomerIO FindCustomerBySSN(ChexarLogin login, string socialSecurityNumber)
		{
			socialSecurityNumber = socialSecurityNumber.Replace("-", "");

			#region AL-3371 Transactional Log User Story(Process check)
			List<string> details = new List<string>();
			details.Add("Token :" + (login != null ? Convert.ToString(login.CompanyToken) : string.Empty));
			details.Add("Social Security Number :" + Convert.ToString(socialSecurityNumber));

			MGIContext mgiContext = new MGIContext();
			MongoDBLogger.ListInfo<string>(0, details, "FindCustomerBySSN - REQUEST", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "FindCustomerBySSN - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

			string response = getCustSvc(login.URL).SearchBadge(login.CompanyToken, 0, socialSecurityNumber);

			#region AL-3371 Transactional Log User Story(Process check)
			MongoDBLogger.Info<string>(0, response, "FindCustomerByName - RESPONSE", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "FindCustomerByName - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

            XDocument x = ParseXDocument(response);
            NLogger.Info(x.ToString());

			return new ChexarCustomerIO(x.Element("customer"));
		}
		#endregion

		#region Data Type lookup methods
		public void GetProductTypes(ChexarLogin login)
		{
			#region AL-3371 Transactional Log User Story(Process check)
			string token = login.CompanyToken;

			MGIContext mgiContext = new MGIContext();
			MongoDBLogger.Info<string>(0, token, "GetProductTypes - REQUEST", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "GetProductTypes - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion


			string response = getTxnSvc(login.URL).ProductTypeList(login.CompanyToken);

			#region AL-3371 Transactional Log User Story(Process check)
			MongoDBLogger.Info<string>(0, response, "GetProductTypes - RESPONSE", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "GetProductTypes - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

			XDocument x = ParseXDocument(response);
			NLogger.Info(x.ToString());
		}

		public void GetCountryList(ChexarLogin login)
		{

			#region AL-3371 Transactional Log User Story(Process check)
			MGIContext mgiContext = new MGIContext();
			MongoDBLogger.Info<ChexarLogin>(0, login, "GetCountryList - REQUEST", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "GetCountryList - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

			string response = getCustSvc(login.URL).CountryList(login.CompanyToken);

			#region AL-3371 Transactional Log User Story(Process check)
			MongoDBLogger.Info<string>(0, response, "GetCountryList - RESPONSE", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "GetCountryList - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

			XDocument x = ParseXDocument(response.Remove(response.LastIndexOf("<customer>")) + "</customer>");
			NLogger.Info(x.ToString());
		}

		public void GetOnHoldCodes(ChexarLogin login)
		{
			#region AL-3371 Transactional Log User Story(Process check)
			MGIContext mgiContext = new MGIContext();
			MongoDBLogger.Info<ChexarLogin>(0, login, "GetOnHoldCodes - REQUEST", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "GetOnHoldCodes - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

			string response = getCustSvc(login.URL).OnHoldCodeList(login.CompanyToken);

			#region AL-3371 Transactional Log User Story(Process check)
			MongoDBLogger.Info<string>(0, response, "GetOnHoldCodes - RESPONSE", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "GetOnHoldCodes - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

			XDocument x = ParseXDocument(response.Remove(response.LastIndexOf("<customer>")) + "</customer>");
			NLogger.Info(x.ToString());
		}

		public void GetIDTypes(ChexarLogin login)
		{
			#region AL-3371 Transactional Log User Story(Process check)
			MGIContext mgiContext = new MGIContext();
			MongoDBLogger.Info<ChexarLogin>(0, login, "GetIDTypes - REQUEST", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "GetIDTypes - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

			string response = getCustSvc(login.URL).DocTypeList(login.CompanyToken, 1);

			#region AL-3371 Transactional Log User Story(Process check)
			MongoDBLogger.Info<string>(0, response, "GetIDTypes - RESPONSE", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "GetIDTypes - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

			XDocument x = ParseXDocument(response);
			NLogger.Info(x.ToString());
		}

		public void GetMembershipTypes(ChexarLogin login)
		{
			#region AL-3371 Transactional Log User Story(Process check)
			MGIContext mgiContext = new MGIContext();
			MongoDBLogger.Info<ChexarLogin>(0, login, "GetMembershipTypes - REQUEST", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "GetMembershipTypes - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

			string response = getCustSvc(login.URL).MemberTypeList(login.CompanyToken);

			#region AL-3371 Transactional Log User Story(Process check)
			MongoDBLogger.Info<string>(0, response, "GetMembershipTypes - RESPONSE", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "GetMembershipTypes - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

			XDocument x = ParseXDocument(response);
			NLogger.Info(x.ToString());
		}

		public void GetProductList(ChexarLogin login)
		{
			#region AL-3371 Transactional Log User Story(Process check)
			MGIContext mgiContext = new MGIContext();
			MongoDBLogger.Info<ChexarLogin>(0, login, "GetProductList - REQUEST", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "GetProductList - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

			string response = getTxnSvc(login.URL).ProductList(login.CompanyToken, 2);

			#region AL-3371 Transactional Log User Story(Process check)
			MongoDBLogger.Info<string>(0, response, "GetProductList - RESPONSE", AlloyLayerName.CXN,
				ModuleName.ProcessCheck, "GetProductList - ChexarIO.ChexarWebSvc", mgiContext);
			#endregion

			XDocument x = ParseXDocument(response);
			NLogger.Info(x.ToString());
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
		#endregion
	}
}
