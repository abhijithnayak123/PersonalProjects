using AutoMapper;
using MGI.Cxn.BillPay.Contract;
using MGI.Cxn.BillPay.Data;
using MGI.Cxn.BillPay.WU.Data;
using MGI.Cxn.WU.Common.Contract;
using MGI.Cxn.WU.Common.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Serialization;
using DAS = MGI.Cxn.BillPay.WU.DASService;
using FusionGo = MGI.Cxn.BillPay.WU.FusionGoShopping;
using WUPayment = MGI.Cxn.BillPay.WU.MakePayment;
using WUValidate = MGI.Cxn.BillPay.WU.PaymentValidate;
using MGI.Common.Util;
using MGI.Common.DataProtection.Contract;
using MGI.Cxn.WU.Common.Impl;
using MGI.Common.TransactionalLogging.Data;
using System.ServiceModel;
using MGI.Common.Sys;

namespace MGI.Cxn.BillPay.WU.Impl
{
	public class IO : BaseIO, IIO
	{
        public IExceptionHelper ExceptionHelper { get; set; }

		public List<Location> GetLocations(string billerName, string accountNumber, decimal amount, WesternUnionAccount account, BillPaymentRequest billPaymentRequest, MGIContext mgiContext)
		{
			List<Location> locations = new List<Location>();

			string errorMessage = "Error while fetching locations from Western Union";

			long billAmount = ConvertDecimalToLong(amount);
			Location location = new Location();

			try
			{
				FusionGo.fusiongoshoppingrequest request = BuildFusionGoRequest(billPaymentRequest, location, account, mgiContext, billerName, accountNumber, billAmount);
				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<FusionGo.fusiongoshoppingrequest>(mgiContext.CustomerSessionId,request,"GetLocations", AlloyLayerName.CXN,
					ModuleName.BillPayment,"GetLocations -MGI.Cxn.BillPay.WU.Impl.IO", "REQUEST",mgiContext);
				#endregion
				FusionGo.fusiongoshoppingreply reply = InvokeFusionGoRequest(request);

				if (reply != null)
				{
					locations = ParseLocations(reply);
				}
				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<FusionGo.fusiongoshoppingreply>(mgiContext.CustomerSessionId, reply, "GetLocations", AlloyLayerName.CXN,
					ModuleName.BillPayment, "GetLocations -MGI.Cxn.BillPay.WU.Impl.IO", "RESPONSE", mgiContext);
				#endregion
			}
			catch (System.ServiceModel.FaultException<FusionGo.errorreply> fex)
			{
				errorMessage = string.Format(errorMessage, fex.Detail.error);
				MongoDBLogger.Error<BillPaymentRequest>(billPaymentRequest, "GetLocations", AlloyLayerName.CXN, ModuleName.BillPayment,
					"Error in GetLocations -MGI.Cxn.BillPay.WU.Impl.IO", errorMessage, fex.ToString());
                string code = ExceptionHelper.GetProviderErrorCode(fex.Detail.error);
				throw new BillpayProviderException(code, fex.Detail.error, fex);
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<BillPaymentRequest>(billPaymentRequest, "GetLocations", AlloyLayerName.CXN, ModuleName.BillPayment,
					"Error in GetLocations -MGI.Cxn.BillPay.WU.Impl.IO", ex.Message, ex.ToString());
                HandleException(ex);
				throw new BillPayException(BillPayException.LOCATION_RETRIEVAL_FAILED, ex);
			}
			return locations;
		}

		public Fee GetDeliveryMethods(string billerName, string accountNumber, decimal amount, Location location, WesternUnionAccount account, BillPaymentRequest billPaymentRequest, MGIContext mgiContext)
		{
			Fee fee = null;
			FusionGo.fusiongoshoppingrequest request = null;
			string errorMessage = "Error while retrieving delivery methods from Western Union";
			long billAmount = ConvertDecimalToLong(amount);

			try
			{
				request = BuildFusionGoRequest(billPaymentRequest, location, account, mgiContext, billerName, accountNumber, billAmount);

				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<FusionGo.fusiongoshoppingrequest>(mgiContext.CustomerSessionId, request, "GetDeliveryMethods", AlloyLayerName.CXN,
					ModuleName.BillPayment, "GetDeliveryMethods -MGI.Cxn.BillPay.WU.Impl.IO", "REQUEST", mgiContext);
				#endregion
				FusionGo.fusiongoshoppingreply reply = InvokeFusionGoRequest(request);

				if (reply != null)
				{
					fee = ParseDeliveryMethods(reply);
				}
				 
				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<FusionGo.fusiongoshoppingreply>(mgiContext.CustomerSessionId, reply, "GetDeliveryMethods", AlloyLayerName.CXN,
					ModuleName.BillPayment, "GetDeliveryMethods -MGI.Cxn.BillPay.WU.Impl.IO", "RESPONSE", mgiContext);
				#endregion
			}
			catch (System.ServiceModel.FaultException<FusionGo.errorreply> fex)
			{
				MongoDBLogger.Error<BillPaymentRequest>(billPaymentRequest, "GetDeliveryMethods", AlloyLayerName.CXN, ModuleName.BillPayment,
					"Error in GetDeliveryMethods -MGI.Cxn.BillPay.WU.Impl.IO", errorMessage, fex.ToString());
                string code = ExceptionHelper.GetProviderErrorCode(fex.Detail.error);;
				throw new BillpayProviderException(code, fex.Detail.error, fex);
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<BillPaymentRequest>(billPaymentRequest, "GetDeliveryMethods", AlloyLayerName.CXN, ModuleName.BillPayment,
						"Error in GetDeliveryMethods -MGI.Cxn.BillPay.WU.Impl.IO", ex.Message, ex.ToString());
                HandleException(ex);
				throw new BillPayException(BillPayException.DELIVERY_METHODS_RETRIEVAL_FAILED, ex);
			}
			finally
			{
				if (fee == null)
				{
					fee = new Fee();
				}
				fee.TransactionId = ProcessAPICall(request, account, billPaymentRequest, mgiContext);
			}

			return fee;
		}

		public string GetBillerMessage(string billerName, MGIContext mgiContext)
		{
			string message = string.Empty;
			string errMessage = string.Empty;
			string errorMessage = "Error while retrieving biller specific message from Western Union";

			try
			{
				var filters = new DAS.filters_type()
				{
					queryfilter1 = "en",
					queryfilter2 = billerName
				};

				DAS.h2hdasreply response = ExecuteDASInquiry(QQC_ACCOUNT_TEMPLATE, filters, mgiContext);
				message = ParseBillerMessage(response, out errMessage);
			}
            catch (System.ServiceModel.FaultException<DAS.errorreply> fex)
            {
                MongoDBLogger.Error<string>(billerName, "GetBillerMessage", AlloyLayerName.CXN, ModuleName.BillPayment,
                    "Error in GetBillerMessage -MGI.Cxn.BillPay.WU.Impl.IO", errorMessage, fex.ToString());
                string code = ExceptionHelper.GetProviderErrorCode(fex.Detail.error);;
                throw new BillpayProviderException(code, fex.Detail.error, fex);
            }
			catch (Exception ex)
			{
				MongoDBLogger.Error<string>(billerName, "GetBillerMessage", AlloyLayerName.CXN, ModuleName.BillPayment,
					"Error in GetBillerMessage -MGI.Cxn.BillPay.WU.Impl.IO", errorMessage, ex.ToString());
                HandleException(ex);
				throw new BillPayException(BillPayException.BILLER_MESSAGE_RETRIEVAL_FAILED, ex);
			}

			return message;
		}

		public List<Field> GetBillerFields(string billerName, string locationName, MGIContext mgiContext)
		{
			List<Field> fields = new List<Field>();
            string errorMessage = "Error while retrieving specific biller from Western Union";
			string errMessage = string.Empty;
			string template = string.Empty;
            DAS.QQCCOMPANYNAME_Type biller = null;
            try
            {
                biller = GetBiller(billerName, locationName, mgiContext);
            }
            catch (System.ServiceModel.FaultException<DAS.errorreply> fex)
            {
                List<string> details = new List<string>();
                details.Add("Biller Name:" + billerName);
                details.Add("Location Name:" + locationName);
                MongoDBLogger.ListError<string>(details, "GetBillerFields", AlloyLayerName.CXN, ModuleName.BillPayment,
                    "Error in GetBillerFields -MGI.Cxn.BillPay.WU.Impl.IO", errorMessage, fex.ToString());
                string code = ExceptionHelper.GetProviderErrorCode(fex.Detail.error);;
                throw new BillpayProviderException(code, fex.Detail.error, fex);
            }
            catch (Exception ex)
            {
                List<string> details = new List<string>();
                details.Add("Biller Name:" + billerName);
                details.Add("Location Name:" + locationName);
                MongoDBLogger.ListError<string>(details, "GetBillerFields", AlloyLayerName.CXN, ModuleName.BillPayment,
                    "Error in GetBillerFields -MGI.Cxn.BillPay.WU.Impl.IO", errorMessage, ex.ToString());
                HandleException(ex);
                throw new BillPayException(BillPayException.BILLER_RETRIEVAL_FAILED, ex);
            }
            
			try
			{				
				if (biller != null)
				{
					template = biller.TEMPLT;
				}
				if (!string.IsNullOrWhiteSpace(template))
				{
					var filters = new DAS.filters_type()
					{
						queryfilter1 = "en",
						queryfilter2 = template
					};

					DAS.h2hdasreply response = ExecuteDASInquiry(QQC_FEILDS_TEMPLATE, filters, mgiContext);
					fields = ParseBillerFields(response, out errMessage);
				}
            }
            catch (System.ServiceModel.FaultException<DAS.errorreply> fex)
            {
                List<string> details = new List<string>();
                details.Add("Biller Name:" + billerName);
                details.Add("Location Name:" + locationName);
                MongoDBLogger.ListError<string>(details, "GetBillerFields", AlloyLayerName.CXN, ModuleName.BillPayment,
                    "Error in GetBillerFields -MGI.Cxn.BillPay.WU.Impl.IO", errorMessage, fex.ToString());
                string code = ExceptionHelper.GetProviderErrorCode(fex.Detail.error);;
                throw new BillpayProviderException(code, fex.Detail.error, fex);
            }
			catch (Exception ex)
			{
				List<string> details = new List<string>();
				details.Add("Biller Name:" + billerName);
				details.Add("Location Name:" + locationName);
				MongoDBLogger.ListError<string>(details, "GetBillerFields", AlloyLayerName.CXN, ModuleName.BillPayment,
					"Error in GetBillerFields -MGI.Cxn.BillPay.WU.Impl.IO", errorMessage, ex.ToString());
                HandleException(ex);
				throw new BillPayException(BillPayException.BILLER_FIELDS_RETRIEVAL_FAILED, ex);
			}

			return fields;
		}

		public long ValidatePayment(BillPaymentRequest billPaymentRequest, WesternUnionTrx trx, MGIContext mgiContext)
		{
			long trxId = 0;
			WUBaseRequestResponse wuObjects = (WUBaseRequestResponse)mgiContext.Context["BaseWUObject"];

			trx.WesternUnionAccount.Address1 = NexxoUtil.MassagingValue(trx.WesternUnionAccount.Address1);
			trx.WesternUnionAccount.Address2 = NexxoUtil.MassagingValue(trx.WesternUnionAccount.Address2);
			trx.WesternUnionAccount.City = NexxoUtil.MassagingValue(trx.WesternUnionAccount.City);
			trx.WesternUnionAccount.FirstName = NexxoUtil.MassagingValue(trx.WesternUnionAccount.FirstName);
			trx.WesternUnionAccount.LastName = NexxoUtil.MassagingValue(trx.WesternUnionAccount.LastName);
			trx.WesternUnionAccount.State = NexxoUtil.MassagingValue(trx.WesternUnionAccount.State);
			trx.WesternUnionAccount.Street = NexxoUtil.MassagingValue(trx.WesternUnionAccount.Street);
			billPaymentRequest.Occupation = WesternUnionIO.TrimOccupation(NexxoUtil.MassagingValue(billPaymentRequest.Occupation));


			WUValidate.channel channel = null;
			WUValidate.foreign_remote_system foreignRemoteSystem = null;

			string errorMessage = "Error while validating bill payment with Western Union";

			WUValidate.makepaymentvalidationrequest request = null;
			WUValidate.makepaymentvalidationreply reply = null;

			try
			{
				BuildValidateObjects(wuObjects, ref channel, ref foreignRemoteSystem);

				foreignRemoteSystem.reference_no = trx.ForeignRemoteSystemReferenceNo;
				if (string.IsNullOrWhiteSpace(billPaymentRequest.Location))
				{
					if (mgiContext.CityCode != null)
					{
						billPaymentRequest.Location = mgiContext.CityCode;
					}
				}

				request = new WUValidate.makepaymentvalidationrequest()
				{
					channel = channel,
					payment_transactions = BuildPaymentTransactions(billPaymentRequest, trx.WesternUnionAccount),
					foreign_remote_system = foreignRemoteSystem
				};

				request.swb_fla_info = Mapper.Map<SwbFlaInfo, WUValidate.swb_fla_info>(WesternUnionIO.BuildSwbFlaInfo(mgiContext));
				request.swb_fla_info.fla_name = Mapper.Map<GeneralName, WUValidate.general_name>(WesternUnionIO.BuildGeneralName(mgiContext));
				WUValidate.MakePaymentValidate_Service_PortTypeClient client = new WUValidate.MakePaymentValidate_Service_PortTypeClient(VALIDATE_ENDPOINT_NAME, _serviceUrl);
				client.ClientCredentials.ClientCertificate.Certificate = _certificate;

				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<WUValidate.makepaymentvalidationrequest>(mgiContext.CustomerSessionId, request, "ValidatePayment", AlloyLayerName.CXN,
					ModuleName.BillPayment, "ValidatePayment -MGI.Cxn.BillPay.WU.Impl.IO", "REQUEST", mgiContext);
				#endregion
				reply = client.MakePaymentValidate(request);

				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<WUValidate.makepaymentvalidationreply>(mgiContext.CustomerSessionId, reply, "ValidatePayment", AlloyLayerName.CXN,
					ModuleName.BillPayment, "ValidatePayment -MGI.Cxn.BillPay.WU.Impl.IO", "RESPONSE", mgiContext);
				#endregion
			}
			catch (System.ServiceModel.FaultException<WUValidate.errorreply> fex)
			{
                MongoDBLogger.Error<BillPaymentRequest>(billPaymentRequest, "ValidatePayment", AlloyLayerName.CXN, ModuleName.BillPayment,
                    "Error in  -MGI.Cxn.BillPay.WU.Impl.IO", errorMessage, fex.ToString());

                string code = ExceptionHelper.GetProviderErrorCode(fex.Detail.error);;
				throw new BillpayProviderException(code, fex.Detail.error, fex);
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<BillPaymentRequest>(billPaymentRequest, "ValidatePayment", AlloyLayerName.CXN, ModuleName.BillPayment,
					"Error in  -MGI.Cxn.BillPay.WU.Impl.IO", errorMessage, ex.ToString());
                HandleException(ex);	
				throw new BillPayException(BillPayException.BILLPAY_VALIDATE_FAILED, ex);
			}
			finally
			{
				if (request != null && reply != null)
				{
					trxId = ProcessAPICall(request, reply, trx.WesternUnionAccount, mgiContext);
				}
			}
			return trxId;
		}

		public long MakePayment(WesternUnionTrx trx, MGIContext mgiContext)
		{
			long trxId = 0;
			WUPayment.channel channel = null;
			WUPayment.foreign_remote_system foreignRemoteSystem = null;

			trx.WesternUnionAccount.Address1 = NexxoUtil.MassagingValue(trx.WesternUnionAccount.Address1);
			trx.WesternUnionAccount.Address2 = NexxoUtil.MassagingValue(trx.WesternUnionAccount.Address2);
			trx.WesternUnionAccount.City = NexxoUtil.MassagingValue(trx.WesternUnionAccount.City);
			trx.WesternUnionAccount.FirstName = NexxoUtil.MassagingValue(trx.WesternUnionAccount.FirstName);
			trx.WesternUnionAccount.LastName = NexxoUtil.MassagingValue(trx.WesternUnionAccount.LastName);
			trx.WesternUnionAccount.State = NexxoUtil.MassagingValue(trx.WesternUnionAccount.State);
			trx.WesternUnionAccount.Street = NexxoUtil.MassagingValue(trx.WesternUnionAccount.Street);

			string errorMessage = "Error while committing bill payment with Western Union";

			WUPayment.makepaymentstorerequest request = null;
			WUPayment.makepaymentstorereply reply = null;

			try
			{
				WUBaseRequestResponse wuObjects = (WUBaseRequestResponse)mgiContext.Context["BaseWUObject"];
				BuildPaymentObjects(wuObjects, ref channel, ref foreignRemoteSystem);
				foreignRemoteSystem.reference_no = trx.ForeignRemoteSystemReferenceNo;

				RequestType requestStatus = (RequestType)Enum.Parse(typeof(RequestType), mgiContext.RequestType);

				request = new WUPayment.makepaymentstorerequest()
				{
					channel = channel,
					payment_transactions = BuildPaymentStoreTransactions(trx, requestStatus),
					foreign_remote_system = foreignRemoteSystem,
				};

				WUPayment.MakePaymentStore_Service_PortTypeClient client = new WUPayment.MakePaymentStore_Service_PortTypeClient(PAYMENT_ENDPOINT_NAME, _serviceUrl);
				client.ClientCredentials.ClientCertificate.Certificate = _certificate;

				request.swb_fla_info = Mapper.Map<SwbFlaInfo, WUPayment.swb_fla_info>(WesternUnionIO.BuildSwbFlaInfo(mgiContext));
				request.swb_fla_info.fla_name = Mapper.Map<GeneralName, WUPayment.general_name>(WesternUnionIO.BuildGeneralName(mgiContext));

				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<WUPayment.makepaymentstorerequest>(mgiContext.CustomerSessionId, request, "MakePayment", AlloyLayerName.CXN,
					ModuleName.BillPayment, "MakePayment -MGI.Cxn.BillPay.WU.Impl.IO", "REQUEST", mgiContext);
				#endregion
				reply = client.makePaymentStore(request);
				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<WUPayment.makepaymentstorereply>(mgiContext.CustomerSessionId, reply, "MakePayment", AlloyLayerName.CXN,
					ModuleName.BillPayment, "MakePayment -MGI.Cxn.BillPay.WU.Impl.IO", "RESPONSE", mgiContext);
				#endregion
			}
			catch (System.ServiceModel.FaultException<WUPayment.errorreply> fex)
			{
				MongoDBLogger.Error<WesternUnionTrx>(trx, "MakePayment", AlloyLayerName.CXN, ModuleName.BillPayment,
					"Error in MakePayment -MGI.Cxn.BillPay.WU.Impl.IO", errorMessage, fex.ToString());
                string code = ExceptionHelper.GetProviderErrorCode(fex.Detail.error);;
                throw new BillpayProviderException(code, fex.Detail.error, fex);
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<WesternUnionTrx>(trx, "MakePayment", AlloyLayerName.CXN, ModuleName.BillPayment,
					"Error in MakePayment -MGI.Cxn.BillPay.WU.Impl.IO", ex.Message, ex.ToString());
				
				throw new BillPayException(BillPayException.BILLPAY_COMMIT_FAILED, ex);
			}
			finally
			{
				if(reply != null)
				trxId = ProcessAPICall(request, reply, trx.WesternUnionAccount, mgiContext);
			}

			return trxId;
		}

        private void HandleException(Exception ex)
        {
            Exception faultException = ex as FaultException;
            if (faultException != null)
            {
                throw new BillpayProviderException(BillpayProviderException.PROVIDER_FAULT_ERROR, string.Empty, faultException);
            }
            Exception endpointException = ex as EndpointNotFoundException;
            if (endpointException != null)
            {
                throw new BillpayProviderException(BillpayProviderException.PROVIDER_ENDPOINTNOTFOUND_ERROR, string.Empty, endpointException);
            }
            Exception commException = ex as CommunicationException;
            if (commException != null)
            {
                throw new BillpayProviderException(BillpayProviderException.PROVIDER_COMMUNICATION_ERROR, string.Empty, commException);
            }
            Exception timeOutException = ex as TimeoutException;
            if (timeOutException != null)
            {
                throw new BillpayProviderException(BillpayProviderException.PROVIDER_TIMEOUT_ERROR, string.Empty, timeOutException);
            }
        }
	}
}
