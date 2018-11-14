//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using MGI.Cxn.Check.Certegy.Certegy;
//using MGI.Cxn.Check.Certegy.Data;
//using System.Security.Cryptography.X509Certificates;
//using System.ServiceModel;
//using System.ServiceModel.Channels;
//using System.ServiceModel.Description;
//using System.ServiceModel.Security.Tokens;
//using System.Web.Services;
//using System.Web.Services.Protocols;
//using System.Globalization;
//using MGI.Cxn.Check.Certegy.Contract;
//using MGI.Common.Util;
//using MGI.Common.TransactionalLogging.Data;

//namespace MGI.Cxn.Check.Certegy.Impl
//{
//	public class IO : IIO
//	{
//		//PCA client = new PCAClient();
//		public TLoggerCommon MongoDBLogger { get; set; }

//		#region Private Members

//		private const string BindingName = "PCAPort";

//		#endregion

//		public echoResponse Diagnostics(Credential credential, MGIContext mgiContext)
//		{
//			PCAClient client = SetupInternetProxy(credential);
//			PCA proxy = client;

//			echoResponse response = null;

//			string echoNumber = "1000";
//			//siteId = "1109541077350101";

//			echoRequest echoRequest = new echoRequest()
//			{
//				SiteID = mgiContext.CertegySiteId,
//				EchoNumber = echoNumber
//			};

//			try
//			{
//				response = proxy.echo(echoRequest);
//				client.Close();
//			}
//			catch (TimeoutException ex)
//			{
//				client.Abort();
//				throw ex;
//			}
//			catch (FaultException ex)
//			{
//				client.Abort();
//				throw new CertegyProviderException(ex.Code.ToString(), ex.Message);
//			}
//			catch (CommunicationException ex)
//			{
//				client.Abort();
//				throw new CertegyProviderException(ex.Message);
//			}
//			catch (System.Exception ex)
//			{
//				client.Abort();
//				throw ex;
//			}

//			return response;
//		}

//		public authorizeResponse AuthorizeCheck(Transaction transaction, Credential credential, MGIContext mgiContext)
//		{
//			PCAClient client = SetupInternetProxy(credential);
//			PCA proxy = client;

//			string siteId = mgiContext.CertegySiteId;
//			authorizeResponse response = null;

//			//set up service request data for check authorization
//			authorizeRequest authorizeRequest = new authorizeRequest()
//			{
//				Check = new Certegy.Check()
//				{
//					Amount = transaction.CheckAmount.ToString(),
//					IssueDate = transaction.CheckDate.ToString("MMddyyyy"),
//					Type = transaction.CertegySubmitType,
//					MICR = new MICR()
//					{
//						EntryType = transaction.MicrEntryType,
//						ExpansionType = transaction.ExpansionType,
//						Line = transaction.Micr
//					},
//				},

//				Version = decimal.Parse(credential.Version),

//				Device = new Device()
//				{
//					ID = transaction.DeviceId,
//					IPAddress = credential.DeviceIP,
//					Type = credential.DeviceType
//				},

//				Enroll = new Enrollment()
//				{
//					Address = new Address()
//					{
//						City = transaction.CertegyAccount.City,
//						Line1 = transaction.CertegyAccount.Address1,
//						Line2 = transaction.CertegyAccount.Address2,
//						State = transaction.CertegyAccount.State,
//						Zip = transaction.CertegyAccount.Zip
//					},
//					DOB = new DOB()
//					{
//						Day = transaction.CertegyAccount.DateOfBirth.Day.ToString("00"),
//						Month = transaction.CertegyAccount.DateOfBirth.Month.ToString("00"),
//						Year = transaction.CertegyAccount.DateOfBirth.Year.ToString()
//					},
//					FirstName = transaction.CertegyAccount.FirstName,
//					ID = new ID()
//					{
//						Type = transaction.IdType,
//						//The Id card type is actually two letter code, need mapping table for alloy type to certegy types
//						Value = transaction.CertegyAccount.Idcardnumber,

//					},
//					LastName = transaction.CertegyAccount.LastName,
//					Phone = transaction.CertegyAccount.Phone,
//				},
//				SiteID = siteId,
//				SSN = transaction.CertegyAccount.Ssn,
//				TranType = transaction.TranType,
//				TransID = transaction.Id.ToString()
//			};

//			authorizeRequest.Enroll.LastName = GetLastName(transaction.CertegyAccount.LastName, transaction.CertegyAccount.SecondLastName);

//			try
//			{
//				#region AL-3371 Transactional Log User Story(Process check)
//				List<string> details = new List<string>();
//				details.Add("Amount :" + Convert.ToString(transaction.CheckAmount));
//				details.Add("CheckDate :" + transaction.CheckDate.ToString("MMddyyyy"));
//				details.Add("Type :" + Convert.ToString(transaction.CertegySubmitType));
//				details.Add("MICR EntryType :" + Convert.ToString(transaction.MicrEntryType));
//				details.Add("MICR ExpansionType :" + Convert.ToString(transaction.ExpansionType));
//				details.Add("MICR Line :" + Convert.ToString(transaction.Micr));
//				details.Add("Device Id :" + Convert.ToString(transaction.DeviceId));
//				details.Add("Device IP Address :" + Convert.ToString(credential.DeviceIP));
//				details.Add("Device Type :" + Convert.ToString(credential.DeviceType));
//				details.Add("CertegyAccount FirstName :" + Convert.ToString(transaction.CertegyAccount.FirstName));
//				details.Add("CertegyAccount LastName :" + Convert.ToString(transaction.CertegyAccount.LastName));
//				details.Add("CertegyAccount Phone :" + Convert.ToString(transaction.CertegyAccount.Phone));
//				details.Add("CertegyAccount City :" + Convert.ToString(transaction.CertegyAccount.City));
//				details.Add("CertegyAccount SSN :" + Convert.ToString(transaction.CertegyAccount.Ssn));
//				details.Add("TransactionID :" + Convert.ToString(transaction.Id));

//				MongoDBLogger.ListInfo<string>(0, details, "AuthorizeCheck - REQUEST", AlloyLayerName.CXN,
//					ModuleName.ProcessCheck, "AuthorizeCheck - MGI.Cxn.Check.Certegy.Impl.IO", mgiContext);
//				#endregion

//				// Call Certegy service authorize method
//				response = proxy.authorize(authorizeRequest);


//				#region AL-3371 Transactional Log User Story(Process check)
//				List<string> respDetails = new List<string>();
//				respDetails.Add("Approval Number :" + Convert.ToString(response.ApprovalNumber));
//				respDetails.Add("Certegy UID :" + Convert.ToString(response.CertegyUID));
//				respDetails.Add("CheckABA :" + Convert.ToString(response.CheckABA));
//				respDetails.Add("CheckAcct :" + Convert.ToString(response.CheckAcct));
//				respDetails.Add("Check Number :" + Convert.ToString(response.CheckNumber));
//				respDetails.Add("CheckType :" + Convert.ToString(response.CheckType));
//				respDetails.Add("Fee :" + Convert.ToString(response.Fee));

//				MongoDBLogger.ListInfo<string>(0, respDetails, "AuthorizeCheck - RESPONSE", AlloyLayerName.CXN,
//					ModuleName.ProcessCheck, "AuthorizeCheck - MGI.Cxn.Check.Certegy.Impl.IO", mgiContext);
//				#endregion
//			}

//			catch (TimeoutException ex)
//			{
//				client.Abort();

//				//AL-3371 Transactional Log User Story(Process check)
//				MongoDBLogger.Error<Transaction>(transaction, "AuthorizeCheck", AlloyLayerName.CXN, ModuleName.ProcessCheck,
//						"Error in AuthorizeCheck - MGI.Cxn.Check.Certegy.Impl.IO", ex.Message, ex.StackTrace);
				
//				throw ex;
//			}
//			catch (FaultException ex)
//			{
//				client.Abort();

//				//AL-3371 Transactional Log User Story(Process check)
//				MongoDBLogger.Error<Transaction>(transaction, "AuthorizeCheck", AlloyLayerName.CXN, ModuleName.ProcessCheck,
//						"Error in AuthorizeCheck - MGI.Cxn.Check.Certegy.Impl.IO", ex.Message, ex.StackTrace);
				
//				throw new CertegyProviderException(ex.Message);
//			}
//			catch (CommunicationException ex)
//			{
//				client.Abort();

//				//AL-3371 Transactional Log User Story(Process check)
//				MongoDBLogger.Error<Transaction>(transaction, "AuthorizeCheck", AlloyLayerName.CXN, ModuleName.ProcessCheck,
//						"Error in AuthorizeCheck - MGI.Cxn.Check.Certegy.Impl.IO", ex.Message, ex.StackTrace);
				
//				throw ex;
//			}
//			catch (System.Exception ex)
//			{
//				client.Abort();

//				//AL-3371 Transactional Log User Story(Process check)
//				MongoDBLogger.Error<Transaction>(transaction, "AuthorizeCheck", AlloyLayerName.CXN, ModuleName.ProcessCheck,
//						"Error in AuthorizeCheck - MGI.Cxn.Check.Certegy.Impl.IO", ex.Message, ex.StackTrace);
				
//				throw ex;
//			}

//			client.Close();

//			return response;

//		}

//		public reverseResponse ReverseCheck(Transaction transaction, Credential credential, MGIContext mgiContext)
//		{
//			PCAClient client = SetupInternetProxy(credential);
//			PCA proxy = client;

//			string siteId = mgiContext.CertegySiteId;
//			reverseResponse response = null;

//			reverseRequest checkReversalRequest = new reverseRequest()
//			{
//				Check = new Certegy.Check()
//				{
//					Amount = transaction.CheckAmount.ToString("F", CultureInfo.InvariantCulture),
//					MICR = new MICR()
//					{
//						EntryType = transaction.MicrEntryType,
//						ExpansionType = transaction.ExpansionType,
//						Line = transaction.Micr
//					}
//				},
//				SiteID = siteId,
//				SSN = transaction.CertegyAccount.Ssn,
//				TransID = transaction.Id.ToString(),
//				Version = decimal.Parse(credential.Version)
//			};

//			try
//			{
//				#region AL-3371 Transactional Log User Story(Process check)
//				List<string> details = new List<string>();
//				details.Add("Amount :" + transaction.CheckAmount.ToString("F", CultureInfo.InvariantCulture));
//				details.Add("MICR EntryType :" + Convert.ToString(transaction.MicrEntryType));
//				details.Add("MICR ExpansionType :" + Convert.ToString(transaction.ExpansionType));
//				details.Add("MICR Line :" + Convert.ToString(transaction.Micr));
//				details.Add("siteId :" + Convert.ToString(siteId));
//				details.Add("SSN :" + Convert.ToString(transaction.CertegyAccount.Ssn));
//				details.Add("Transaction ID :" + Convert.ToString(transaction.Id));
//				details.Add("Version :" + Convert.ToString(credential.Version));

//				MongoDBLogger.ListInfo<string>(0, details, "ReverseCheck - REQUEST", AlloyLayerName.CXN,
//					ModuleName.ProcessCheck, "ReverseCheck - MGI.Cxn.Check.Certegy.Impl.IO", mgiContext);
//				#endregion

//				response = proxy.reverse(checkReversalRequest);

//				#region AL-3371 Transactional Log User Story(Process check)
//				List<string> respDetails = new List<string>();
//				respDetails.Add("ResponseCode :" + Convert.ToString(response.ResponseCode));
//				respDetails.Add("Certegy UID :" + Convert.ToString(response.CertegyUID));
//				respDetails.Add("Roundtrip :" + Convert.ToString(response.Roundtrip));
//				respDetails.Add("Transaction ID :" + Convert.ToString(response.TransID));
				

//				MongoDBLogger.ListInfo<string>(0, respDetails, "ReverseCheck - RESPONSE", AlloyLayerName.CXN,
//					ModuleName.ProcessCheck, "ReverseCheck - MGI.Cxn.Check.Certegy.Impl.IO", mgiContext);
//				#endregion
//			}
//			catch (TimeoutException ex)
//			{
//				client.Abort();

//				//AL-3371 Transactional Log User Story(Process check)
//				MongoDBLogger.Error<Transaction>(transaction, "ReverseCheck", AlloyLayerName.CXN, ModuleName.ProcessCheck,
//						"Error in ReverseCheck - MGI.Cxn.Check.Certegy.Impl.IO", ex.Message, ex.StackTrace);
				
//				throw ex;
//			}
//			catch (FaultException ex)
//			{
//				client.Abort();

//				//AL-3371 Transactional Log User Story(Process check)
//				MongoDBLogger.Error<Transaction>(transaction, "ReverseCheck", AlloyLayerName.CXN, ModuleName.ProcessCheck,
//						"Error in ReverseCheck - MGI.Cxn.Check.Certegy.Impl.IO", ex.Message, ex.StackTrace);
				
//				throw new CertegyProviderException(ex.Code.ToString(), ex.Message);
//			}
//			catch (CommunicationException ex)
//			{
//				client.Abort();

//				//AL-3371 Transactional Log User Story(Process check)
//				MongoDBLogger.Error<Transaction>(transaction, "ReverseCheck", AlloyLayerName.CXN, ModuleName.ProcessCheck,
//						"Error in ReverseCheck - MGI.Cxn.Check.Certegy.Impl.IO", ex.Message, ex.StackTrace);
				
//				throw ex;
//			}
//			catch (System.Exception ex)
//			{
//				client.Abort();

//				//AL-3371 Transactional Log User Story(Process check)
//				MongoDBLogger.Error<Transaction>(transaction, "ReverseCheck", AlloyLayerName.CXN, ModuleName.ProcessCheck,
//						"Error in ReverseCheck - MGI.Cxn.Check.Certegy.Impl.IO", ex.Message, ex.StackTrace);
				
//				throw ex;
//			}

//			client.Close();

//			return response;
//		}

//		private static PCAClient SetupInternetProxy(Credential credential)
//		{
//			PCAClient certegyServicesClient = new PCAClient(BindingName, credential.ServiceUrl);

//			certegyServicesClient.Endpoint.Behaviors.Add(new MustUnderstandBehavior(false));

//			EndpointAddress address = new EndpointAddress(credential.ServiceUrl);

//			certegyServicesClient.Endpoint.Address = address;

//			//certegyServicesClient.ClientCredentials.ClientCertificate.Certificate.Import()
//			certegyServicesClient.ClientCredentials.ClientCertificate
//				.SetCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySubjectName, credential.CertificateName);

//			return certegyServicesClient;
//		}

//		private static string GetLastName(string customerLastName, string customerSecondLastName)
//		{
//			int maxChars = 25;
//			string lastName = customerLastName.Trim();

//			if (lastName.Length < maxChars)
//			{
//				if (!string.IsNullOrWhiteSpace(customerSecondLastName))
//				{
//					lastName = string.Concat(lastName, customerSecondLastName.Trim());
//					lastName = lastName.Length > maxChars ? customerLastName.Trim() : lastName;
//				}
//			}
//			else
//			{
//				lastName = lastName.Substring(0, maxChars);
//			}

//			return lastName;
//		}
//	}
//}
