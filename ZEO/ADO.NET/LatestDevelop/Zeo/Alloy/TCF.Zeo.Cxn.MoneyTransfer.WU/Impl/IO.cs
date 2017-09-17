using TCF.Zeo.Cxn.MoneyTransfer.WU.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Cxn.MoneyTransfer.Data;
using TCF.Zeo.Cxn.MoneyTransfer.WU.FeeInquiry;
using TCF.Zeo.Cxn.MoneyTransfer.WU.ModifySendMoney;
using TCF.Zeo.Cxn.MoneyTransfer.WU.ModifySendMoneySearch;
using TCF.Zeo.Cxn.MoneyTransfer.WU.ReceiveMoneyPay;
using TCF.Zeo.Cxn.MoneyTransfer.WU.ReceiveMoneySearch;
using TCF.Zeo.Cxn.MoneyTransfer.WU.Search;
using TCF.Zeo.Cxn.MoneyTransfer.WU.SendMoneyPayStatus;
using TCF.Zeo.Cxn.MoneyTransfer.WU.SendMoneyRefund;
using TCF.Zeo.Cxn.MoneyTransfer.WU.SendMoneyStore;
using TCF.Zeo.Cxn.MoneyTransfer.WU.SendMoneyValidation;
using static TCF.Zeo.Common.Util.Helper;
using DAS = TCF.Zeo.Cxn.MoneyTransfer.WU.DASService;
using System.ServiceModel;
using System.Xml.Serialization;
using System.IO;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Cxn.MoneyTransfer.WU.Data;
using TCF.Zeo.Cxn.MoneyTransfer.Data.Exceptions;
using CxnWu = TCF.Zeo.Cxn.MoneyTransfer.WU;
using System.Configuration;

namespace TCF.Zeo.Cxn.MoneyTransfer.WU.Impl
{
    public class IO : BaseIO, IIO
    {
        public string LPMTErrorMessage { get; set; }

        public feeinquiryreply FeeInquiry(feeinquiryrequest feeInquiryRequest, ZeoContext context)
        {
            try
            {
                PopulateWUObjects(context.ChannelPartnerId, context);
                feeInquiryRequest.host_based_taxes = CxnWu.FeeInquiry.host_based_taxes.Y;
                feeInquiryRequest.host_based_taxesSpecified = true;

                CxnWu.FeeInquiry.channel channel = null;
                CxnWu.FeeInquiry.foreign_remote_system foreignRemoteSystem = null;

                BuildFeeInquiryObjects(_response, ref channel, ref foreignRemoteSystem);

                foreignRemoteSystem.reference_no = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                feeInquiryRequest.channel = channel;
                feeInquiryRequest.foreign_remote_system = foreignRemoteSystem;

                CxnWu.FeeInquiry.FeeInquiryPortTypeClient feeInquiryClient = new CxnWu.FeeInquiry.FeeInquiryPortTypeClient("SOAP_HTTP_Port1", _serviceUrl);
                feeInquiryClient.ClientCredentials.ClientCertificate.Certificate = _certificate;
                CxnWu.FeeInquiry.feeinquiryreply response = null;
                response = feeInquiryClient.FeeInquiry(feeInquiryRequest);

                return response;
            }
            catch (FaultException<CxnWu.FeeInquiry.errorreply> ex)
            {
                string code = ExceptionHelper.GetWUErrorCode(ex.Detail.error);
                throw new MoneyTransferProviderException(code, ex.Detail.error, ex);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleException(ex);
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_FEEINQUIRY_FAILED, ex);
            }
        }

        public List<DeliveryService> GetDeliveryServices(DeliveryServiceRequest request, string state, string stateCode, string city, string deliveryService, ZeoContext context)
        {
            var deliveryServices = new List<DeliveryService>();
            try
            {

                //US1832 Get Delivery Service call to be made only once for Mexico in Send Money
                if (request.Type == DeliveryServiceType.Option && request.CountryCode == "MX")
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

                if (request.Type == DeliveryServiceType.Method)
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
                else if (request.Type == DeliveryServiceType.Option)
                {
                    filters.queryfilter4 = deliveryService;
                    filters.queryfilter5 = city;
                    filters.queryfilter6 = state;
                }

                DAS.h2hdasreply reply = ExecuteDASInquiry(DAS_DeliveryServices, filters, context);

                if (reply != null && reply.MTML != null && reply.MTML.Item != null)
                {
                    DAS.REPLYType dasResponse = reply.MTML.Item as DAS.REPLYType;

                    if (dasResponse != null && dasResponse.DATA_CONTEXT != null
                        && dasResponse.DATA_CONTEXT.RECORDSET != null && dasResponse.DATA_CONTEXT.RECORDSET.Items != null)
                    {
                        var items = dasResponse.DATA_CONTEXT.RECORDSET.Items.ToList().ConvertAll<DAS.DELIVERYSERVICE_Type>(t => (DAS.DELIVERYSERVICE_Type)t);
                        deliveryServices = items.Select
                            (d => new DeliveryService()
                            {
                                Code = d.SVC_CODE,
                                Name = d.SVC_NAME
                            }
                            ).ToList();
                    }
                }

                if (request.Type == DeliveryServiceType.Method && deliveryServices.Count == 0)
                {
                    //Added by Yashasvi- As per Santosh's email, if there are no delivery methods make Money in Minutes as default
                    deliveryServices.Add
                    (
                        new DeliveryService() { Code = "000", Name = "MONEY IN MINUTES" }
                    );
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleException(ex);
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETDELIVERYSERVICES_FAILED, ex);
            }
            return deliveryServices;
        }

        public paystatusinquiryreply GetPayStatus(paystatusinquiryrequestdata searchRequest, ZeoContext context)
        {
            try
            {
                PopulateWUObjects(context.ChannelPartnerId, context);

                CxnWu.SendMoneyPayStatus.channel channel = null;
                CxnWu.SendMoneyPayStatus.foreign_remote_system foreignRemoteSystem = null;

                BuildPayStatusObjects(_response, ref channel, ref foreignRemoteSystem);

                foreignRemoteSystem.reference_no = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                searchRequest.channel = channel;
                searchRequest.foreign_remote_system = foreignRemoteSystem;

                PayStatusPortTypeClient client = new PayStatusPortTypeClient("WU_SendMoneyPayStatus", _serviceUrl);
                client.ClientCredentials.ClientCertificate.Certificate = _certificate;
                paystatusinquiryreply response = client.PayStatus(searchRequest);
                return response;
            }
            catch (FaultException<CxnWu.SendMoneyPayStatus.errorreply> ex)
            {
                string code = ExceptionHelper.GetWUErrorCode(ex.Detail.error);
                throw new MoneyTransferProviderException(code, ex.Detail.error, ex);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleException(ex);
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETPAYSTATUS_FAILED, ex);
            }
        }

        public List<Reason> GetRefundReasons(ReasonRequest request, ZeoContext context)
        {
            try
            {
                var reasons = new List<Reason>();

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

                DAS.h2hdasreply reply = ExecuteDASInquiry(methodName, filters, context);

                if (reply != null && reply.MTML != null && reply.MTML.Item != null)
                {
                    var dasResponse = reply.MTML.Item as DAS.REPLYType;

                    if (dasResponse != null && dasResponse.DATA_CONTEXT != null
                        && dasResponse.DATA_CONTEXT.RECORDSET != null && dasResponse.DATA_CONTEXT.RECORDSET.Items != null)
                    {
                        var items = dasResponse.DATA_CONTEXT.RECORDSET.Items.ToList().ConvertAll<DAS.REASONLIST_Type>(t => (DAS.REASONLIST_Type)t);
                        reasons = items.Select(d => new Reason() { Code = d.REASON_CODE, Name = d.REASON_DESC }).ToList();
                    }
                }

                return reasons;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleException(ex);
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETREFUNDREASONS_FAILED, ex);
            }
        }

        public modifysendmoneyreply Modify(modifysendmoneyrequest modifySendMoneyRequest, ZeoContext context)
        {
            try
            {
                PopulateWUObjects(context.ChannelPartnerId, context);

                CxnWu.ModifySendMoney.channel channel = null;
                CxnWu.ModifySendMoney.foreign_remote_system foreignRemoteSystem = null;

                BuildModifySendMoneyObjects(_response, ref channel, ref foreignRemoteSystem);

                foreignRemoteSystem.reference_no = context.ReferenceNumber;
                modifySendMoneyRequest.channel = channel;
                modifySendMoneyRequest.foreign_remote_system = foreignRemoteSystem;

                ModifySendMoneyPortTypeClient client = new ModifySendMoneyPortTypeClient("ModifySendMoney", _serviceUrl);
                client.ClientCredentials.ClientCertificate.Certificate = _certificate;

                modifysendmoneyreply response = client.ModifySendMoney(modifySendMoneyRequest);

                return response;
            }
            catch (FaultException<CxnWu.ModifySendMoney.errorreply> ex)
            {
                string code = ExceptionHelper.GetWUErrorCode(ex.Detail.error);
                throw new MoneyTransferProviderException(code, ex.Detail.error, ex);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleException(ex);
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_MODIFY_FAILED, ex);
            }
        }

        public modifysendmoneysearchreply ModifySearch(modifysendmoneysearchrequest request, ZeoContext context)
        {
            try
            {
                PopulateWUObjects(context.ChannelPartnerId, context);

                CxnWu.ModifySendMoneySearch.channel channel = null;
                CxnWu.ModifySendMoneySearch.foreign_remote_system foreignRemoteSystem = null;

                BuildModifySearchObjects(_response, ref channel, ref foreignRemoteSystem);

                foreignRemoteSystem.reference_no = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                request.channel = channel;
                request.foreign_remote_system = foreignRemoteSystem;
                modifysendmoneysearchreply response = null;
                ModifySendMoneySearchPortTypeClient client = new ModifySendMoneySearchPortTypeClient("ModifySendMoneySearch", _serviceUrl);
                client.ClientCredentials.ClientCertificate.Certificate = _certificate;
                //response=client.SendMoneyStore_H2H(request);
                response = client.ModifySendMoneySearch(request);
                return response;
            }
            catch (FaultException<CxnWu.ModifySendMoneySearch.errorreply> ex)
            {
                string code = ExceptionHelper.GetWUErrorCode(ex.Detail.error);
                throw new MoneyTransferProviderException(code, ex.Detail.error, ex);
            }
            catch (FaultException<CxnWu.SendMoneyPayStatus.errorreply> ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleException(ex);
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_MODIFYSEARCH_FAILED, ex);
            }
        }

        public receivemoneypayreply ReceiveMoneyPay(receivemoneypayrequest receiveMoneyPayRequest, ZeoContext context)
        {
            try
            {
                PopulateWUObjects(context.ChannelPartnerId, context);

                CxnWu.ReceiveMoneyPay.channel channel = null;
                CxnWu.ReceiveMoneyPay.foreign_remote_system foreignRemoteSystem = null;

                BuildReceiveMoneyPayObjects(_response, ref channel, ref foreignRemoteSystem);

                foreignRemoteSystem.reference_no = context.ReferenceNumber;

                receiveMoneyPayRequest.channel = channel;
                receiveMoneyPayRequest.foreign_remote_system = foreignRemoteSystem;


                var client = new ReceiveMoneyPayPortTypeClient("ReceiveMoneyPay", _serviceUrl);
                client.ClientCredentials.ClientCertificate.Certificate = _certificate;

                receivemoneypayreply reply = null;
                reply = client.ReceiveMoneyPay(receiveMoneyPayRequest);
                return reply;
            }
            catch (FaultException<CxnWu.ReceiveMoneyPay.errorreply> ex)
            {
                string code = ExceptionHelper.GetWUErrorCode(ex.Detail.error);
                throw new MoneyTransferProviderException(code, ex.Detail.error, ex);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleException(ex);
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_RECEIVEMONEYPAY_FAILED, ex);
            }
        }

        public refundreply Refund(refundrequest refundRequest, ZeoContext context)
        {
            try
            {
                PopulateWUObjects(context.ChannelPartnerId, context);

                CxnWu.SendMoneyRefund.channel channel = null;
                CxnWu.SendMoneyRefund.foreign_remote_system foreignRemoteSystem = null;

                BuildRefundSendMoneyObjects(_response, ref channel, ref foreignRemoteSystem);

                foreignRemoteSystem.reference_no = context.ReferenceNumber;
                refundRequest.channel = channel;
                refundRequest.foreign_remote_system = foreignRemoteSystem;
                refundRequest.device = new CxnWu.SendMoneyRefund.gwp_gbs_device()
                {
                    type = CxnWu.SendMoneyRefund.gwp_gbs_device_type.AGENT,
                    typeSpecified = true
                };

                RefundPortTypeClient client = new RefundPortTypeClient("WU_SendMoneyRefund", _serviceUrl);
                client.ClientCredentials.ClientCertificate.Certificate = _certificate;

                refundreply response = client.Refund(refundRequest);
                return response;
            }
            catch (FaultException<CxnWu.SendMoneyRefund.errorreply> ex)
            {
                string code = ExceptionHelper.GetWUErrorCode(ex.Detail.error);
                throw new MoneyTransferProviderException(code, ex.Detail.error, ex);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleException(ex);
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_REFUND_FAILED, ex);
            }
        }

        public searchreply Search(searchrequest searchRequest, ZeoContext context)
        {
            try
            {
                PopulateWUObjects(context.ChannelPartnerId, context);

                CxnWu.Search.channel channel = null;
                CxnWu.Search.foreign_remote_system foreignRemoteSystem = null;

                BuildRefundSearchObjects(_response, ref channel, ref foreignRemoteSystem);

                foreignRemoteSystem.reference_no = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                searchRequest.channel = channel;
                searchRequest.foreign_remote_system = foreignRemoteSystem;

                SearchPortTypeClient SearchClient = new CxnWu.Search.SearchPortTypeClient("WU_Search", _serviceUrl);
                SearchClient.ClientCredentials.ClientCertificate.Certificate = _certificate;
                searchreply searchreply = SearchClient.Search(searchRequest);
                return searchreply;
            }
            catch (FaultException<CxnWu.Search.errorreply> ex)
            {
                string code = ExceptionHelper.GetWUErrorCode(ex.Detail.error);
                throw new MoneyTransferProviderException(code, ex.Detail.error, ex);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleException(ex);
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_SEARCH_FAILED, ex);
            }
        }

        public receivemoneysearchreply SearchReceiveMoney(receivemoneysearchrequest receiveMoneySearchRequest, ZeoContext context)
        {
            try
            {
                PopulateWUObjects(context.ChannelPartnerId, context);

                CxnWu.ReceiveMoneySearch.channel channel = null;
                CxnWu.ReceiveMoneySearch.foreign_remote_system foreignRemoteSystem = null;

                BuildReceiveMoneySearchObjects(_response, ref channel, ref foreignRemoteSystem);

                foreignRemoteSystem.reference_no = context.ReferenceNumber;

                receiveMoneySearchRequest.channel = channel;
                receiveMoneySearchRequest.foreign_remote_system = foreignRemoteSystem;


                var client = new ReceiveMoneySearchPortTypeClient("ReceiveMoneySearch", _serviceUrl);
                client.ClientCredentials.ClientCertificate.Certificate = _certificate;

                receivemoneysearchreply receivemoneysearchReply = null;

                receivemoneysearchReply = client.ReceiveMoneySearch(receiveMoneySearchRequest);

                return receivemoneysearchReply;
            }
            catch (FaultException<CxnWu.ReceiveMoneySearch.errorreply> ex)
            {
                string code = ExceptionHelper.GetWUErrorCode(ex.Detail.error);
                throw new MoneyTransferProviderException(code, ex.Detail.error, ex);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleException(ex);
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_SEARCHRECEIVEMONEY_FAILED, ex);
            }
        }

        public sendmoneystorereply SendMoneyStore(sendmoneystorerequest sendMoneyStoreRequest, ZeoContext context, out bool hasLPMTError)
        {
            var sendMoneyStoreReply = new sendmoneystorereply();
            try
            {
                hasLPMTError = false;

                string errorMessage = string.Format("Error while retrieving credentials for channel partner: {0}", context.ChannelPartnerId);
                PopulateWUObjects(context.ChannelPartnerId, context);

                CxnWu.SendMoneyStore.channel channel = null;
                CxnWu.SendMoneyStore.foreign_remote_system foreignRemoteSystem = null;

                BuildSendMoneyStoreObjects(_response, ref channel, ref foreignRemoteSystem);

                foreignRemoteSystem.reference_no = context.ReferenceNumber;
                sendMoneyStoreRequest.host_based_taxes = CxnWu.SendMoneyStore.host_based_taxes.Y;
                sendMoneyStoreRequest.host_based_taxesSpecified = true;
                sendMoneyStoreRequest.channel = channel;
                sendMoneyStoreRequest.foreign_remote_system = foreignRemoteSystem;

                var sendMoneyStoreClient = new SendMoneyStorePortTypeClient("SOAP_HTTP_Port", _serviceUrl);
                sendMoneyStoreClient.ClientCredentials.ClientCertificate.Certificate = _certificate;

                sendMoneyStoreReply = sendMoneyStoreClient.SendMoneyStore_H2H(sendMoneyStoreRequest);

            }
            catch (System.ServiceModel.FaultException<CxnWu.SendMoneyStore.errorreply> ex)
            {
                LPMTErrorMessage = ConfigurationManager.AppSettings["LPMTErrorMessage"].ToString();

                if (LPMTErrorMessage.Equals(ex.Detail.error, StringComparison.OrdinalIgnoreCase))
                {
                    hasLPMTError = true;
                }
                else
                {
                    string code = ExceptionHelper.GetWUErrorCode(ex.Detail.error);
                    throw new MoneyTransferProviderException(code, ex.Detail.error, ex);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleException(ex);
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_SENDMONEYSTORE_FAILED, ex);
            }
            return sendMoneyStoreReply;
        }

        public sendmoneyvalidationreply SendMoneyValidate(sendmoneyvalidationrequest validationRequest, ZeoContext context)
        {
            CxnWu.SendMoneyValidation.sendmoneyvalidationreply response = null;

            try
            {
                PopulateWUObjects(context.ChannelPartnerId, context);

                CxnWu.SendMoneyValidation.channel channel = null;
                CxnWu.SendMoneyValidation.foreign_remote_system foreignRemoteSystem = null;

                BuildValidationObjects(_response, ref channel, ref foreignRemoteSystem);

                foreignRemoteSystem.reference_no = context.ReferenceNumber;

                validationRequest.host_based_taxes = CxnWu.SendMoneyValidation.host_based_taxes.Y;
                validationRequest.host_based_taxesSpecified = true;
                validationRequest.channel = channel;
                validationRequest.foreign_remote_system = foreignRemoteSystem;

                CxnWu.SendMoneyValidation.SendMoneyValidatePortTypeClient sendMoneyValidateTypeClient = new CxnWu.SendMoneyValidation.SendMoneyValidatePortTypeClient("SOAP_HTTP_Port4", _serviceUrl);
                sendMoneyValidateTypeClient.ClientCredentials.ClientCertificate.Certificate = _certificate;
                response = sendMoneyValidateTypeClient.sendmoneyValidation(validationRequest);
            }
            catch (System.ServiceModel.FaultException<CxnWu.SendMoneyValidation.errorreply> ex)
            {
                string code = ExceptionHelper.GetWUErrorCode(ex.Detail.error);
                throw new MoneyTransferProviderException(code, ex.Detail.error, ex);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleException(ex);
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_VALIDATE_FAILED, ex);
            }
            return response;
        }
    }
}
