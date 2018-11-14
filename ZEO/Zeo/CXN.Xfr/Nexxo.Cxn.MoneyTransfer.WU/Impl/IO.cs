using MGI.Common.DataAccess.Contract;
using MGI.Common.Util;
using MGI.Cxn.MoneyTransfer.Data;
using MGI.Cxn.MoneyTransfer.WU.Data;
using MGI.Cxn.MoneyTransfer.WU.Impl;
using MGI.Cxn.MoneyTransfer.WU.ModifySendMoney;
using MGI.Cxn.MoneyTransfer.WU.ModifySendMoneySearch;
using MGI.Cxn.MoneyTransfer.WU.ReceiveMoneyPay;
using MGI.Cxn.MoneyTransfer.WU.ReceiveMoneySearch;
using MGI.Cxn.MoneyTransfer.WU.Search;
using MGI.Cxn.MoneyTransfer.WU.SendMoneyPayStatus;
using MGI.Cxn.MoneyTransfer.WU.SendMoneyRefund;
using MGI.Cxn.MoneyTransfer.WU.SendMoneyStore;
using MGI.Cxn.WU.Common.Contract;
using MGI.Cxn.WU.Common.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Xml.Serialization;
using DAS = MGI.Cxn.MoneyTransfer.WU.DASService;
using Modifysendmoney = MGI.Cxn.MoneyTransfer.WU.ModifySendMoney;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Cxn.MoneyTransfer.WU.Impl
{
	public class IO : BaseIO, IIO
	{
		#region Public Methods

		public SendMoneyValidation.sendmoneyvalidationreply SendMoneyValidate(SendMoneyValidation.sendmoneyvalidationrequest validationRequest, MGIContext mgiContext)
		{
			SendMoneyValidation.sendmoneyvalidationreply response = null;

			try
			{
				PopulateWUObjects(mgiContext.ChannelPartnerId, mgiContext);

				SendMoneyValidation.channel channel = null;
				SendMoneyValidation.foreign_remote_system foreignRemoteSystem = null;

				BuildValidationObjects(_response, ref channel, ref foreignRemoteSystem);

				foreignRemoteSystem.reference_no = mgiContext.ReferenceNumber;

				validationRequest.host_based_taxes = Cxn.MoneyTransfer.WU.SendMoneyValidation.host_based_taxes.Y;
				validationRequest.host_based_taxesSpecified = true;
				validationRequest.channel = channel;
				validationRequest.foreign_remote_system = foreignRemoteSystem;

				SendMoneyValidation.SendMoneyValidatePortTypeClient sendMoneyValidateTypeClient = new SendMoneyValidation.SendMoneyValidatePortTypeClient("SOAP_HTTP_Port4", _serviceUrl);
				sendMoneyValidateTypeClient.ClientCredentials.ClientCertificate.Certificate = _certificate;

				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<SendMoneyValidation.sendmoneyvalidationrequest>(mgiContext.CustomerSessionId, validationRequest, "SendMoneyValidate", AlloyLayerName.CXN,
					ModuleName.MoneyTransfer, "SendMoneyValidate REQUEST - MGI.Cxn.MoneyTransfer.WU.Impl.IO", mgiContext);
				#endregion
				response = sendMoneyValidateTypeClient.sendmoneyValidation(validationRequest);

				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<SendMoneyValidation.sendmoneyvalidationreply>(mgiContext.CustomerSessionId, response, "SendMoneyValidate", AlloyLayerName.CXN,
					ModuleName.MoneyTransfer, "SendMoneyValidate RESPONSE - MGI.Cxn.MoneyTransfer.WU.Impl.IO", mgiContext);
				#endregion
			}
			catch (System.ServiceModel.FaultException<SendMoneyValidation.errorreply> ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<SendMoneyValidation.sendmoneyvalidationrequest>(validationRequest, "SendMoneyValidate", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in SendMoneyValidate - MGI.Cxn.MoneyTransfer.WU.Impl.IO", ex.Message, ex.StackTrace);

				//       User Story Number: AL-471 | Web |   Developed by: Sunil Shetty     Date: 25.06.2015
				//       Purpose: Exception message is sent to the GetSSNExceptionMessage and if its not SSN Exception then default provider error is shown
				string code = ExceptionHelper.GetProviderErrorCode(ex.Detail.error);
                throw new MoneyTransferProviderException(code, ex.Detail.error, ex);
			}
			catch (Exception ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<SendMoneyValidation.sendmoneyvalidationrequest>(validationRequest, "SendMoneyValidate", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in SendMoneyValidate - MGI.Cxn.MoneyTransfer.WU.Impl.IO", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;				
				HandleException(ex);
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_VALIDATE_FAILED, ex);
			}
			return response;
		}

		public sendmoneystorereply SendMoneyStore(sendmoneystorerequest sendMoneyStoreRequest, MGIContext mgiContext, out bool hasLPMTError)
		{
			var sendMoneyStoreReply = new sendmoneystorereply();
			try
			{
				hasLPMTError = false;

				string errorMessage = string.Format("Error while retrieving credentials for channel partner: {0}", mgiContext.ChannelPartnerId);
				PopulateWUObjects(mgiContext.ChannelPartnerId, mgiContext);

				SendMoneyStore.channel channel = null;
				SendMoneyStore.foreign_remote_system foreignRemoteSystem = null;

				BuildSendMoneyStoreObjects(_response, ref channel, ref foreignRemoteSystem);

				foreignRemoteSystem.reference_no = mgiContext.ReferenceNumber;
				sendMoneyStoreRequest.host_based_taxes = Cxn.MoneyTransfer.WU.SendMoneyStore.host_based_taxes.Y;
				sendMoneyStoreRequest.host_based_taxesSpecified = true;
				sendMoneyStoreRequest.channel = channel;
				sendMoneyStoreRequest.foreign_remote_system = foreignRemoteSystem;

				var sendMoneyStoreClient = new SendMoneyStorePortTypeClient("SOAP_HTTP_Port", _serviceUrl);
				sendMoneyStoreClient.ClientCredentials.ClientCertificate.Certificate = _certificate;

				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<sendmoneystorerequest>(mgiContext.CustomerSessionId, sendMoneyStoreRequest, "SendMoneyStore", AlloyLayerName.CXN,
					ModuleName.MoneyTransfer, "SendMoneyStore REQUEST - MGI.Cxn.MoneyTransfer.WU.Impl.IO", mgiContext);
				#endregion
				sendMoneyStoreReply = sendMoneyStoreClient.SendMoneyStore_H2H(sendMoneyStoreRequest);

				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<sendmoneystorereply>(mgiContext.CustomerSessionId, sendMoneyStoreReply, "SendMoneyStore", AlloyLayerName.CXN,
					ModuleName.MoneyTransfer, "SendMoneyStore RESPONSE - MGI.Cxn.MoneyTransfer.WU.Impl.IO", mgiContext);
				#endregion
			}
			catch (System.ServiceModel.FaultException<SendMoneyStore.errorreply> ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<sendmoneystorerequest>(sendMoneyStoreRequest, "SendMoneyStore", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in SendMoneyStore - MGI.Cxn.MoneyTransfer.WU.Impl.IO", ex.Message, ex.StackTrace);
				string code = ExceptionHelper.GetProviderErrorCode(ex.Detail.error);
				throw new MoneyTransferProviderException(code, ex.Detail.error, ex);	
			}
			catch (Exception ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<sendmoneystorerequest>(sendMoneyStoreRequest, "SendMoneyStore", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in SendMoneyStore -  MGI.Cxn.MoneyTransfer.WU.Impl.IO", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;				
				HandleException(ex);
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_SENDMONEYSTORE_FAILED, ex);
			}
			return sendMoneyStoreReply;
		}

		public searchreply Search(searchrequest searchRequest, MGIContext mgiContext)
		{
			try
			{
				PopulateWUObjects(mgiContext.ChannelPartnerId, mgiContext);

				Search.channel channel = null;
				Search.foreign_remote_system foreignRemoteSystem = null;

				BuildRefundSearchObjects(_response, ref channel, ref foreignRemoteSystem);

				foreignRemoteSystem.reference_no = DateTime.Now.ToString("yyyyMMddHHmmssffff");
				searchRequest.channel = channel;
				searchRequest.foreign_remote_system = foreignRemoteSystem;

				SearchPortTypeClient SearchClient = new Search.SearchPortTypeClient("WU_Search", _serviceUrl);
				SearchClient.ClientCredentials.ClientCertificate.Certificate = _certificate;

				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<searchrequest>(mgiContext.CustomerSessionId, searchRequest, "Search", AlloyLayerName.CXN,
					ModuleName.MoneyTransfer, "Search REQUEST - MGI.Cxn.MoneyTransfer.WU.Impl.IO", mgiContext);
				#endregion
				searchreply searchreply = SearchClient.Search(searchRequest);

				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<searchreply>(mgiContext.CustomerSessionId, searchreply, "Search", AlloyLayerName.CXN,
					ModuleName.MoneyTransfer, "Search RESPONSE - MGI.Cxn.MoneyTransfer.WU.Impl.IO", mgiContext);
				#endregion
				return searchreply;
			}
			catch (FaultException<Search.errorreply> ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<searchrequest>(searchRequest, "Search", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in Search - MGI.Cxn.MoneyTransfer.WU.Impl.IO", ex.Message, ex.StackTrace);

				string code = ExceptionHelper.GetProviderErrorCode(ex.Detail.error);
				throw new MoneyTransferProviderException(code, ex.Detail.error, ex);
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<searchrequest>(searchRequest, "Search", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in Search - MGI.Cxn.MoneyTransfer.WU.Impl.IO", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				HandleException(ex);
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_SEARCH_FAILED, ex);
			}
		}

		public refundreply Refund(refundrequest refundRequest, MGIContext mgiContext)
		{

			try
			{
				PopulateWUObjects(mgiContext.ChannelPartnerId, mgiContext);

				SendMoneyRefund.channel channel = null;
				SendMoneyRefund.foreign_remote_system foreignRemoteSystem = null;

				BuildRefundSendMoneyObjects(_response, ref channel, ref foreignRemoteSystem);

				foreignRemoteSystem.reference_no = mgiContext.ReferenceNumber;
				refundRequest.channel = channel;
				refundRequest.foreign_remote_system = foreignRemoteSystem;
				refundRequest.device = new SendMoneyRefund.gwp_gbs_device()
				{
					type = MGI.Cxn.MoneyTransfer.WU.SendMoneyRefund.gwp_gbs_device_type.AGENT,
					typeSpecified = true
				};

				RefundPortTypeClient client = new RefundPortTypeClient("WU_SendMoneyRefund", _serviceUrl);
				client.ClientCredentials.ClientCertificate.Certificate = _certificate;
				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<refundrequest>(mgiContext.CustomerSessionId, refundRequest, "Refund", AlloyLayerName.CXN,
					ModuleName.MoneyTransfer, "Refund REQUEST - MGI.Cxn.MoneyTransfer.WU.Impl.IO", mgiContext);
				#endregion
				refundreply response = client.Refund(refundRequest);

				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<refundreply>(mgiContext.CustomerSessionId, response, "Refund", AlloyLayerName.CXN,
					ModuleName.MoneyTransfer, "Refund RESPONSE - MGI.Cxn.MoneyTransfer.WU.Impl.IO", mgiContext);
				#endregion
				return response;
			}
			catch (FaultException<SendMoneyRefund.errorreply> ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<refundrequest>(refundRequest, "Refund", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in Refund - MGI.Cxn.MoneyTransfer.WU.Impl.IO", ex.Message, ex.StackTrace);
				string code = ExceptionHelper.GetProviderErrorCode(ex.Detail.error);
				throw new MoneyTransferProviderException(code, ex.Detail.error, ex);
			}
			catch (Exception ex) 
			{
				MongoDBLogger.Error<refundrequest>(refundRequest, "Refund", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in Refund - MGI.Cxn.MoneyTransfer.WU.Impl.IO", ex.Message, ex.StackTrace);
				
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;				
				HandleException(ex);
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_REFUND_FAILED, ex);
			}
		}

		public paystatusinquiryreply GetPayStatus(paystatusinquiryrequestdata searchRequest, MGIContext mgiContext)
		{
			try
			{
				PopulateWUObjects(mgiContext.ChannelPartnerId, mgiContext);

				SendMoneyPayStatus.channel channel = null;
				SendMoneyPayStatus.foreign_remote_system foreignRemoteSystem = null;

				BuildPayStatusObjects(_response, ref channel, ref foreignRemoteSystem);

				foreignRemoteSystem.reference_no = DateTime.Now.ToString("yyyyMMddHHmmssffff");
				searchRequest.channel = channel;
				searchRequest.foreign_remote_system = foreignRemoteSystem;

				PayStatusPortTypeClient client = new PayStatusPortTypeClient("WU_SendMoneyPayStatus", _serviceUrl);
				client.ClientCredentials.ClientCertificate.Certificate = _certificate;

				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<paystatusinquiryrequestdata>(mgiContext.CustomerSessionId, searchRequest, "GetPayStatus", AlloyLayerName.CXN,
					ModuleName.MoneyTransfer, "GetPayStatus REQUEST - MGI.Cxn.MoneyTransfer.WU.Impl.IO", mgiContext);
				#endregion
				paystatusinquiryreply response = client.PayStatus(searchRequest);

				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<paystatusinquiryreply>(mgiContext.CustomerSessionId, response, "GetPayStatus", AlloyLayerName.CXN,
					ModuleName.MoneyTransfer, "GetPayStatus RESPONSE - MGI.Cxn.MoneyTransfer.WU.Impl.IO", mgiContext);
				#endregion
				return response;
			}
			catch (FaultException<SendMoneyPayStatus.errorreply> ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<paystatusinquiryrequestdata>(searchRequest, "GetPayStatus", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in GetPayStatus - MGI.Cxn.MoneyTransfer.WU.Impl.IO", ex.Message, ex.StackTrace);
				string code = ExceptionHelper.GetProviderErrorCode(ex.Detail.error);
				throw new MoneyTransferProviderException(code, ex.Detail.error, ex);
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<paystatusinquiryrequestdata>(searchRequest, "GetPayStatus", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in GetPayStatus - MGI.Cxn.MoneyTransfer.WU.Impl.IO", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;				
				HandleException(ex);
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETPAYSTATUS_FAILED, ex);
			}
		}


		public ModifySendMoneySearch.modifysendmoneysearchreply ModifySearch(ModifySendMoneySearch.modifysendmoneysearchrequest request, MGIContext mgiContext)
		{
			try
			{
				PopulateWUObjects(mgiContext.ChannelPartnerId, mgiContext);

				ModifySendMoneySearch.channel channel = null;
				ModifySendMoneySearch.foreign_remote_system foreignRemoteSystem = null;

				BuildModifySearchObjects(_response, ref channel, ref foreignRemoteSystem);

				foreignRemoteSystem.reference_no = DateTime.Now.ToString("yyyyMMddHHmmssffff");
				request.channel = channel;
				request.foreign_remote_system = foreignRemoteSystem;
				modifysendmoneysearchreply response = null;
				ModifySendMoneySearchPortTypeClient client = new ModifySendMoneySearchPortTypeClient("ModifySendMoneySearch", _serviceUrl);
				client.ClientCredentials.ClientCertificate.Certificate = _certificate;

				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<modifysendmoneysearchrequest>(mgiContext.CustomerSessionId, request, "ModifySearch", AlloyLayerName.CXN,
					ModuleName.MoneyTransfer, "ModifySearch REQUEST - MGI.Cxn.MoneyTransfer.WU.Impl.IO", mgiContext);
				#endregion
				response = client.ModifySendMoneySearch(request);

				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<modifysendmoneysearchreply>(mgiContext.CustomerSessionId, response, "ModifySearch", AlloyLayerName.CXN,
					ModuleName.MoneyTransfer, "ModifySearch RESPONSE - MGI.Cxn.MoneyTransfer.WU.Impl.IO", mgiContext);
				#endregion

				return response;
			}
            //AL-2547 starts
            catch (FaultException<ModifySendMoneySearch.errorreply> ex)
            {
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<modifysendmoneysearchrequest>(request, "ModifySearch", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in ModifySearch - MGI.Cxn.MoneyTransfer.WU.Impl.IO", ex.Message, ex.StackTrace);
				string code = ExceptionHelper.GetProviderErrorCode(ex.Detail.error);
				throw new MoneyTransferProviderException(code, ex.Detail.error, ex);
            }//AL-2547 end
			catch (FaultException<SendMoneyPayStatus.errorreply> ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<modifysendmoneysearchrequest>(request, "ModifySearch", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in ModifySearch - MGI.Cxn.MoneyTransfer.WU.Impl.IO", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;				
				HandleException(ex);
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_MODIFYSEARCH_FAILED, ex);
			}
		}

		public ModifySendMoney.modifysendmoneyreply Modify(ModifySendMoney.modifysendmoneyrequest modifySendMoneyRequest, MGIContext mgiContext)
		{
			try
			{
				PopulateWUObjects(mgiContext.ChannelPartnerId, mgiContext);

				ModifySendMoney.channel channel = null;
				ModifySendMoney.foreign_remote_system foreignRemoteSystem = null;

				BuildModifySendMoneyObjects(_response, ref channel, ref foreignRemoteSystem);

				foreignRemoteSystem.reference_no = mgiContext.ReferenceNumber;
				modifySendMoneyRequest.channel = channel;
				modifySendMoneyRequest.foreign_remote_system = foreignRemoteSystem;

				ModifySendMoneyPortTypeClient client = new ModifySendMoneyPortTypeClient("ModifySendMoney", _serviceUrl);
				client.ClientCredentials.ClientCertificate.Certificate = _certificate;

				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<modifysendmoneyrequest>(mgiContext.CustomerSessionId, modifySendMoneyRequest, "Modify", AlloyLayerName.CXN,
					ModuleName.MoneyTransfer, "Modify REQUEST - MGI.Cxn.MoneyTransfer.WU.Impl.IO", mgiContext);
				#endregion
				modifysendmoneyreply response = client.ModifySendMoney(modifySendMoneyRequest);

				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<modifysendmoneyreply>(mgiContext.CustomerSessionId, response, "Modify", AlloyLayerName.CXN,
					ModuleName.MoneyTransfer, "Modify RESPONSE - MGI.Cxn.MoneyTransfer.WU.Impl.IO", mgiContext);
				#endregion
				return response;
			}
			catch (FaultException<ModifySendMoney.errorreply> ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<ModifySendMoney.modifysendmoneyrequest>(modifySendMoneyRequest, "Modify", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in Modify - MGI.Cxn.MoneyTransfer.WU.Impl.IO", ex.Message, ex.StackTrace);
				string code = ExceptionHelper.GetProviderErrorCode(ex.Detail.error);
				throw new MoneyTransferProviderException(code, ex.Detail.error, ex);
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<ModifySendMoney.modifysendmoneyrequest>(modifySendMoneyRequest, "Modify", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in Modify - MGI.Cxn.MoneyTransfer.WU.Impl.IO", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;				
				HandleException(ex);
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_MODIFY_FAILED, ex);
			}
		}


		#endregion

		#region Private Methods


		#endregion

		public List<MoneyTransfer.Data.DeliveryService> GetDeliveryServices(MoneyTransfer.Data.DeliveryServiceRequest request,
			string state, string stateCode, string city, string deliveryService, MGIContext mgiContext)
		{
			var deliveryServices = new List<MoneyTransfer.Data.DeliveryService>();
			try
			{

				//US1832 Get Delivery Service call to be made only once for Mexico in Send Money
				if (request.Type == MoneyTransfer.Data.DeliveryServiceType.Option && request.CountryCode == "MX")
				{
					return deliveryServices;
				}

				string receiveCountryInfo = string.Format("{0} {1}", request.CountryCode, request.CountryCurrency);

				DAS.filters_type filters = new DAS.filters_type()
				{
					queryfilter1 = "en",
					queryfilter2 = "US USD",
					queryfilter3 = receiveCountryInfo,
				};

				if (request.Type == MoneyTransfer.Data.DeliveryServiceType.Method)
				{
					// Mexican specific business logic for Delivery Methods
					if (request.CountryCode == "MX")
					{
						filters.queryfilter4 = "000";
						filters.queryfilter5 = city;
						filters.queryfilter6 = state;
					}
					else if (request.CountryCode == "US" || request.CountryCode == "CA")
					{
						filters.queryfilter5 = stateCode;
					}
				}
				else if (request.Type == MoneyTransfer.Data.DeliveryServiceType.Option)
				{
					filters.queryfilter4 = deliveryService;
					filters.queryfilter5 = city;
					filters.queryfilter6 = state;
				}

				DAS.h2hdasreply reply = ExecuteDASInquiry(DAS_DeliveryServices, filters, mgiContext);

				if (reply != null && reply.MTML != null && reply.MTML.Item != null)
				{
					DAS.REPLYType dasResponse = reply.MTML.Item as DAS.REPLYType;

					if (dasResponse != null && dasResponse.DATA_CONTEXT != null
						&& dasResponse.DATA_CONTEXT.RECORDSET != null && dasResponse.DATA_CONTEXT.RECORDSET.Items != null)
					{
						var items = dasResponse.DATA_CONTEXT.RECORDSET.Items.ToList().ConvertAll<DAS.DELIVERYSERVICE_Type>(t => (DAS.DELIVERYSERVICE_Type)t);
						deliveryServices = items.Select
							(d => new MoneyTransfer.Data.DeliveryService()
								{
									Code = d.SVC_CODE,
									Name = d.SVC_NAME
								}
							).ToList();
					}
				}

				if (request.Type == MoneyTransfer.Data.DeliveryServiceType.Method && deliveryServices.Count == 0)
				{
					//Added by Yashasvi- As per Santosh's email, if there are no delivery methods make Money in Minutes as default
					deliveryServices.Add
					(
						new MoneyTransfer.Data.DeliveryService() { Code = "000", Name = "MONEY IN MINUTES" }
					);
				}
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<MoneyTransfer.Data.DeliveryServiceRequest>(request, "GetDeliveryServices", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in GetDeliveryServices - MGI.Cxn.MoneyTransfer.WU.Impl.BaseIO", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;				
				HandleException(ex);
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETDELIVERYSERVICES_FAILED, ex);
			}
			return deliveryServices;
		}


		public FeeInquiry.feeinquiryreply FeeInquiry(FeeInquiry.feeinquiryrequest feeInquiryRequest, MGIContext mgiContext)
		{
			try
			{
				PopulateWUObjects(mgiContext.ChannelPartnerId, mgiContext);
				feeInquiryRequest.host_based_taxes = Cxn.MoneyTransfer.WU.FeeInquiry.host_based_taxes.Y;
				feeInquiryRequest.host_based_taxesSpecified = true;

				FeeInquiry.channel channel = null;
				FeeInquiry.foreign_remote_system foreignRemoteSystem = null;

				BuildFeeInquiryObjects(_response, ref channel, ref foreignRemoteSystem);

				foreignRemoteSystem.reference_no = DateTime.Now.ToString("yyyyMMddHHmmssffff");
				feeInquiryRequest.channel = channel;
				feeInquiryRequest.foreign_remote_system = foreignRemoteSystem;

				FeeInquiry.FeeInquiryPortTypeClient feeInquiryClient = new FeeInquiry.FeeInquiryPortTypeClient("SOAP_HTTP_Port1", _serviceUrl);
				feeInquiryClient.ClientCredentials.ClientCertificate.Certificate = _certificate;
				FeeInquiry.feeinquiryreply response = null;
				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<FeeInquiry.feeinquiryrequest>(mgiContext.CustomerSessionId, feeInquiryRequest, "FeeInquiry", AlloyLayerName.CXN,
					ModuleName.MoneyTransfer, "FeeInquiry REQUEST - MGI.Cxn.MoneyTransfer.WU.Impl.IO", mgiContext);
				#endregion
				response = feeInquiryClient.FeeInquiry(feeInquiryRequest);

				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<FeeInquiry.feeinquiryreply>(mgiContext.CustomerSessionId, response, "FeeInquiry", AlloyLayerName.CXN,
					ModuleName.MoneyTransfer, "FeeInquiry RESPONSE - MGI.Cxn.MoneyTransfer.WU.Impl.IO", mgiContext);
				#endregion
			
				return response;
			}
			catch (FaultException<FeeInquiry.errorreply> ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<FeeInquiry.feeinquiryrequest>(feeInquiryRequest, "FeeInquiry", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in FeeInquiry - MGI.Cxn.MoneyTransfer.WU.Impl.IO", ex.Message, ex.StackTrace);
				string code = ExceptionHelper.GetProviderErrorCode(ex.Detail.error);
				throw new MoneyTransferProviderException(code, ex.Detail.error, ex);
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<FeeInquiry.feeinquiryrequest>(feeInquiryRequest, "FeeInquiry", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in FeeInquiry - MGI.Cxn.MoneyTransfer.WU.Impl.IO", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;				
				HandleException(ex);
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_FEEINQUIRY_FAILED, ex);
			}
		}

		public receivemoneysearchreply SearchReceiveMoney(receivemoneysearchrequest receiveMoneySearchRequest, MGIContext mgiContext)
		{
			try
			{
				PopulateWUObjects(mgiContext.ChannelPartnerId, mgiContext);

				ReceiveMoneySearch.channel channel = null;
				ReceiveMoneySearch.foreign_remote_system foreignRemoteSystem = null;

				BuildReceiveMoneySearchObjects(_response, ref channel, ref foreignRemoteSystem);

				foreignRemoteSystem.reference_no = mgiContext.ReferenceNumber;

				receiveMoneySearchRequest.channel = channel;
				receiveMoneySearchRequest.foreign_remote_system = foreignRemoteSystem;


				var client = new ReceiveMoneySearchPortTypeClient("ReceiveMoneySearch", _serviceUrl);
				client.ClientCredentials.ClientCertificate.Certificate = _certificate;

				receivemoneysearchreply receivemoneysearchReply = null;
				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<receivemoneysearchrequest>(mgiContext.CustomerSessionId, receiveMoneySearchRequest, "SearchReceiveMoney", AlloyLayerName.CXN,
					ModuleName.ReceiveMoney, "SearchReceiveMoney REQUEST - MGI.Cxn.MoneyTransfer.WU.Impl.IO", mgiContext);
				#endregion
				receivemoneysearchReply = client.ReceiveMoneySearch(receiveMoneySearchRequest);

				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<receivemoneysearchreply>(mgiContext.CustomerSessionId, receivemoneysearchReply, "SearchReceiveMoney", AlloyLayerName.CXN,
					ModuleName.ReceiveMoney, "SearchReceiveMoney RESPONSE - MGI.Cxn.MoneyTransfer.WU.Impl.IO", mgiContext);
				#endregion
				
				return receivemoneysearchReply;
			}
			catch (FaultException<ReceiveMoneySearch.errorreply> ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<receivemoneysearchrequest>(receiveMoneySearchRequest, "SearchReceiveMoney", AlloyLayerName.CXN, ModuleName.ReceiveMoney,
					"Error in SearchReceiveMoney - MGI.Cxn.MoneyTransfer.WU.Impl.IO", ex.Message, ex.StackTrace);
				string code = ExceptionHelper.GetProviderErrorCode(ex.Detail.error);
				throw new MoneyTransferProviderException(code, ex.Detail.error, ex);
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<receivemoneysearchrequest>(receiveMoneySearchRequest, "SearchReceiveMoney", AlloyLayerName.CXN, ModuleName.ReceiveMoney,
					"Error in SearchReceiveMoney - MGI.Cxn.MoneyTransfer.WU.Impl.IO", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;				
				HandleException(ex);
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_SEARCHRECEIVEMONEY_FAILED, ex);
			}

		}

		public receivemoneypayreply ReceiveMoneyPay(receivemoneypayrequest receiveMoneyPayRequest, MGIContext mgiContext)
		{
			try
			{
				PopulateWUObjects(mgiContext.ChannelPartnerId, mgiContext);

				ReceiveMoneyPay.channel channel = null;
				ReceiveMoneyPay.foreign_remote_system foreignRemoteSystem = null;

				BuildReceiveMoneyPayObjects(_response, ref channel, ref foreignRemoteSystem);

				foreignRemoteSystem.reference_no = mgiContext.ReferenceNumber;

				receiveMoneyPayRequest.channel = channel;
				receiveMoneyPayRequest.foreign_remote_system = foreignRemoteSystem;


				var client = new ReceiveMoneyPayPortTypeClient("ReceiveMoneyPay", _serviceUrl);
				client.ClientCredentials.ClientCertificate.Certificate = _certificate;

				receivemoneypayreply reply = null;

				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<receivemoneypayrequest>(mgiContext.CustomerSessionId, receiveMoneyPayRequest, "ReceiveMoneyPay", AlloyLayerName.CXN,
					ModuleName.ReceiveMoney, "ReceiveMoneyPay REQUEST - MGI.Cxn.MoneyTransfer.WU.Impl.IO", mgiContext);
				#endregion
				reply = client.ReceiveMoneyPay(receiveMoneyPayRequest);

				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<receivemoneypayreply>(mgiContext.CustomerSessionId, reply, "ReceiveMoneyPay", AlloyLayerName.CXN,
					ModuleName.ReceiveMoney, "ReceiveMoneyPay RESPONSE - MGI.Cxn.MoneyTransfer.WU.Impl.IO", mgiContext);
				#endregion
				
				return reply;
			}
			catch (FaultException<ReceiveMoneyPay.errorreply> ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<receivemoneypayrequest>(receiveMoneyPayRequest, "ReceiveMoneyPay", AlloyLayerName.CXN, ModuleName.ReceiveMoney,
				"Error in ReceiveMoneyPay - MGI.Cxn.MoneyTransfer.WU.Impl.IO", ex.Message, ex.StackTrace);
				string code = ExceptionHelper.GetProviderErrorCode(ex.Detail.error);
				throw new MoneyTransferProviderException(code, ex.Detail.error, ex);
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<receivemoneypayrequest>(receiveMoneyPayRequest, "ReceiveMoneyPay", AlloyLayerName.CXN, ModuleName.ReceiveMoney,
				"Error in ReceiveMoneyPay - MGI.Cxn.MoneyTransfer.WU.Impl.IO", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;				
				HandleException(ex);
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_RECEIVEMONEYPAY_FAILED, ex);
			}
		}


		public List<MoneyTransfer.Data.Reason> GetRefundReasons(MoneyTransfer.Data.ReasonRequest request, MGIContext mgiContext)
		{
			try
			{
				var reasons = new List<MoneyTransfer.Data.Reason>();

				string methodName = "GetReasonList";
				string reasonType = string.Empty;

				switch (request.TransactionType.ToUpper())
				{
					case "REFUND":
						reasonType = "RREF";
						break;
					case "REFUND,F":
						reasonType = "RRDF";
						break;
					case "REFUND,N":
						reasonType = "RREF";
						break;
					case "CANCEL":
						reasonType = "RCAN";
						break;
				}

				var filters = new DAS.filters_type
				{
					queryfilter1 = "en",
					queryfilter2 = reasonType
				};

				DAS.h2hdasreply reply = ExecuteDASInquiry(methodName, filters, mgiContext);

				if (reply != null && reply.MTML != null && reply.MTML.Item != null)
				{
					var dasResponse = reply.MTML.Item as DAS.REPLYType;

					if (dasResponse != null && dasResponse.DATA_CONTEXT != null
						&& dasResponse.DATA_CONTEXT.RECORDSET != null && dasResponse.DATA_CONTEXT.RECORDSET.Items != null)
					{
						var items = dasResponse.DATA_CONTEXT.RECORDSET.Items.ToList().ConvertAll<DAS.REASONLIST_Type>(t => (DAS.REASONLIST_Type)t);
						reasons = items.Select(d => new MoneyTransfer.Data.Reason() { Code = d.REASON_CODE, Name = d.REASON_DESC }).ToList();
					}
				}
				
				return reasons;
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<MoneyTransfer.Data.ReasonRequest>(request, "GetRefundReasons", AlloyLayerName.CXN, ModuleName.ReceiveMoney,
				"Error in GetRefundReasons - MGI.Cxn.MoneyTransfer.WU.Impl.IO", ex.Message, ex.StackTrace);
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;				
				HandleException(ex);
				throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETREFUNDREASONS_FAILED, ex);
			}

		}

	}
}
