#region External Reference

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

#region Service Reference

using DAS = TCF.Zeo.Cxn.BillPay.WU.DASService;
using FusionGo = TCF.Zeo.Cxn.BillPay.WU.FusionGoShopping;
using WUPayment = TCF.Zeo.Cxn.BillPay.WU.MakePaymentStore;
using WUValidate = TCF.Zeo.Cxn.BillPay.WU.MakePaymentValidate;

#endregion

#region Alloy Reference

using TCF.Zeo.Common.Data;
using TCF.Zeo.Cxn.BillPay.Data;
using TCF.Zeo.Cxn.BillPay.WU.Data;
using TCF.Zeo.Cxn.BillPay.WU.Contract;
using TCF.Zeo.Cxn.WU.Common.Data;
using TCF.Zeo.Common.Util;
using System.ServiceModel;
using TCF.Zeo.Cxn.BillPay.WU.Data.Exceptions;
using TCF.Zeo.Cxn.BillPay.Data.Exceptions;

#endregion



namespace TCF.Zeo.Cxn.BillPay.WU.Impl
{
    public class IO : BaseIO, IIO
    {
        public IExceptionHelper ExceptionHelper = new BillpayProviderException();

        #region IIO Methods

        public Fee GetDeliveryMethods(long wuTrxId, string billerName, string accountNumber, decimal amount, Location location, BillPaymentRequest billPaymentRequest, ZeoContext context)
        {
            Fee fee = new Fee();
            FusionGo.fusiongoshoppingrequest request = null;

            try
            {
                long billAmount = ConvertDecimalToLong(amount);
                request = BuildFusionGoRequest(billPaymentRequest, location, context, billerName, accountNumber, billAmount);

                FusionGo.fusiongoshoppingreply reply = InvokeFusionGoRequest(request);

                if (reply != null)
                {
                    fee = ParseDeliveryMethods(reply);
                }
            }
            catch (System.ServiceModel.FaultException<FusionGo.errorreply> fex)
            {
                string code = ExceptionHelper.GetProviderErrorCode(fex.Detail.error); ;
                throw new BillpayProviderException(code, fex.Detail.error, fex);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleException(ex);
                throw new BillPayException(BillPayException.DELIVERY_METHODS_RETRIEVAL_FAILED, ex);
            }
            finally
            {
                fee.TransactionId = ProcessAPICall(wuTrxId, request, billPaymentRequest, context);
            }

            return fee;
        }

        public List<Location> GetLocations(string billerName, string accountNumber, decimal amount, BillPaymentRequest billPaymentRequest, ZeoContext context)
        {
            List<Location> locations = new List<Location>();
            long billAmount = ConvertDecimalToLong(amount);
            Location location = new Location();

            try
            {
                FusionGo.fusiongoshoppingrequest request = BuildFusionGoRequest(billPaymentRequest, location, context, billerName, accountNumber, billAmount);

                FusionGo.fusiongoshoppingreply reply = InvokeFusionGoRequest(request);

                if (reply != null)
                    locations = ParseLocations(reply);

            }
            catch (System.ServiceModel.FaultException<FusionGo.errorreply> fex)
            {
                string code = ExceptionHelper.GetProviderErrorCode(fex.Detail.error);
                throw new BillpayProviderException(code, fex.Detail.error, fex);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleException(ex);
                throw new BillPayException(BillPayException.LOCATION_RETRIEVAL_FAILED, ex);
            }
            return locations;
        }

        public WUTransaction MakePayment(BillPayTransactionRequest trxRequest, ZeoContext context)
        {

            WUPayment.channel channel = null;
            WUPayment.foreign_remote_system foreignRemoteSystem = null;

            trxRequest.Address1 = AlloyUtil.MassagingValue(trxRequest.Address1);
            trxRequest.Address2 = AlloyUtil.MassagingValue(trxRequest.Address2);
            trxRequest.City = AlloyUtil.MassagingValue(trxRequest.City);
            trxRequest.FirstName = AlloyUtil.MassagingValue(trxRequest.FirstName);
            trxRequest.LastName = AlloyUtil.MassagingValue(trxRequest.LastName);
            trxRequest.State = AlloyUtil.MassagingValue(trxRequest.State);
            trxRequest.Street = AlloyUtil.MassagingValue(trxRequest.Street);

            WUPayment.makepaymentstorerequest request = null;
            WUPayment.makepaymentstorereply reply = null;
            WUTransaction transaction = new WUTransaction();
            try
            {
                WUBaseRequestResponse wuObjects = (WUBaseRequestResponse)context.Context["BaseWUObject"];
                BuildPaymentObjects(wuObjects, ref channel, ref foreignRemoteSystem);
                foreignRemoteSystem.reference_no = trxRequest.ForeignRemoteSystemReferenceNo;

                Helper.RequestType requestStatus = (Helper.RequestType)Enum.Parse(typeof(Helper.RequestType), context.RequestType, true);

                request = new WUPayment.makepaymentstorerequest()
                {
                    channel = channel,
                    payment_transactions = BuildPaymentStoreTransactions(trxRequest, requestStatus),
                    foreign_remote_system = foreignRemoteSystem,
                };

                WUPayment.MakePaymentStore_Service_PortTypeClient client = new WUPayment.MakePaymentStore_Service_PortTypeClient(PAYMENT_ENDPOINT_NAME, _serviceUrl);
                client.ClientCredentials.ClientCertificate.Certificate = _certificate;

                request.swb_fla_info = mapper.Map<SwbFlaInfo, WUPayment.swb_fla_info>(WUCommonIO.BuildSwbFlaInfo(context));
                request.swb_fla_info.fla_name = mapper.Map<GeneralName, WUPayment.general_name>(WUCommonIO.BuildGeneralName(context));

                reply = client.makePaymentStore(request);
            }
            catch (System.ServiceModel.FaultException<WUPayment.errorreply> fex)
            {
                string code = ExceptionHelper.GetProviderErrorCode(fex.Detail.error);
                throw new BillpayProviderException(code, fex.Detail.error, fex);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleException(ex);
                throw new BillPayException(BillPayException.BILLPAY_COMMIT_FAILED, ex);
            }
            finally
            {
                if (reply != null)
                    transaction = ProcessAPICall(request, reply, trxRequest, context);
            }

            return transaction;
        }

        public WUTransaction ValidatePayment(long wuTrxId, BillPaymentRequest billPaymentRequest, ZeoContext context)
        {
            WUTransaction transaction = new WUTransaction();

            WUBaseRequestResponse wuObjects = (WUBaseRequestResponse)context.Context["BaseWUObject"];

            billPaymentRequest.Address1 = AlloyUtil.MassagingValue(billPaymentRequest.Address1);
            billPaymentRequest.Address2 = AlloyUtil.MassagingValue(billPaymentRequest.Address2);
            billPaymentRequest.City = AlloyUtil.MassagingValue(billPaymentRequest.City);
            billPaymentRequest.FirstName = AlloyUtil.MassagingValue(billPaymentRequest.FirstName);
            billPaymentRequest.LastName = AlloyUtil.MassagingValue(billPaymentRequest.LastName);
            billPaymentRequest.State = AlloyUtil.MassagingValue(billPaymentRequest.State);
            billPaymentRequest.Street = AlloyUtil.MassagingValue(billPaymentRequest.Street);
            billPaymentRequest.Occupation = WUCommonIO.TrimOccupation(AlloyUtil.MassagingValue(billPaymentRequest.Occupation));
            billPaymentRequest.Email = billPaymentRequest.Email;

            WUValidate.channel channel = null;
            WUValidate.foreign_remote_system foreignRemoteSystem = null;

            WUValidate.makepaymentvalidationrequest request = null;
            WUValidate.makepaymentvalidationreply reply = null;

            try
            {
                BuildValidateObjects(wuObjects, ref channel, ref foreignRemoteSystem);

                foreignRemoteSystem.reference_no = billPaymentRequest.ForeignRemoteSystemReferenceNo;

                request = new WUValidate.makepaymentvalidationrequest()
                {
                    channel = channel,
                    payment_transactions = BuildPaymentTransactions(billPaymentRequest),
                    foreign_remote_system = foreignRemoteSystem
                };

                request.swb_fla_info = mapper.Map<SwbFlaInfo, WUValidate.swb_fla_info>(WUCommonIO.BuildSwbFlaInfo(context));
                request.swb_fla_info.fla_name = mapper.Map<GeneralName, WUValidate.general_name>(WUCommonIO.BuildGeneralName(context));
                WUValidate.MakePaymentValidate_Service_PortTypeClient client = new WUValidate.MakePaymentValidate_Service_PortTypeClient(VALIDATE_ENDPOINT_NAME, _serviceUrl);
                client.ClientCredentials.ClientCertificate.Certificate = _certificate;

                reply = client.MakePaymentValidate(request);

            }
            catch (System.ServiceModel.FaultException<WUValidate.errorreply> fex)
            {
                string code = ExceptionHelper.GetProviderErrorCode(fex.Detail.error);
                throw new BillpayProviderException(code, fex.Detail.error, fex);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleException(ex);
                throw new BillPayException(BillPayException.BILLPAY_VALIDATE_FAILED, ex);
            }
            finally
            {
                if (request != null && reply != null)
                {
                    transaction = ProcessAPICall(wuTrxId, request, reply, context);
                }
            }
            return transaction;
        }

        public List<Field> GetBillerFields(string billerName, string locationName, ZeoContext context)
        {
            List<Field> fields = new List<Field>();
            string errMessage = string.Empty;
            string template = string.Empty;
            DAS.QQCCOMPANYNAME_Type biller = null;
            try
            {
                biller = GetBiller(billerName, locationName, context);
            }
            catch (System.ServiceModel.FaultException<DAS.errorreply> fex)
            {
                string code = ExceptionHelper.GetProviderErrorCode(fex.Detail.error);
                throw new BillpayProviderException(code, fex.Detail.error, fex);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleException(ex);
                throw new BillPayException(BillPayException.BILLER_RETRIEVAL_FAILED, ex);
            }

            try
            {
                if (biller != null)
                    template = biller.TEMPLT;

                if (!string.IsNullOrWhiteSpace(template))
                {
                    var filters = new DAS.filters_type()
                    {
                        queryfilter1 = "en",
                        queryfilter2 = template
                    };

                    DAS.h2hdasreply response = ExecuteDASInquiry(QQC_FEILDS_TEMPLATE, filters, context);

                    fields = ParseBillerFields(response, out errMessage);
                }
            }
            catch (FaultException<DAS.errorreply> fex)
            {
                string code = ExceptionHelper.GetProviderErrorCode(fex.Detail.error);
                throw new BillpayProviderException(code, fex.Detail.error, fex);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleException(ex);
                throw new BillPayException(BillPayException.BILLER_FIELDS_RETRIEVAL_FAILED, ex);
            }

            return fields;
        }

        public string GetBillerMessage(string billerName, ZeoContext context)
        {
            string message = string.Empty;
            string errMessage = string.Empty;
            try
            {
                var filters = new DAS.filters_type()
                {
                    queryfilter1 = "en",
                    queryfilter2 = billerName
                };

                DAS.h2hdasreply response = ExecuteDASInquiry(QQC_ACCOUNT_TEMPLATE, filters, context);
                message = ParseBillerMessage(response, out errMessage);
            }
            catch (System.ServiceModel.FaultException<DAS.errorreply> fex)
            {
                string code = ExceptionHelper.GetProviderErrorCode(fex.Detail.error);
                throw new BillpayProviderException(code, fex.Detail.error, fex);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleException(ex);
                throw new BillPayException(BillPayException.BILLER_MESSAGE_RETRIEVAL_FAILED, ex);
            }

            return message;
        }

        #endregion

        #region Private Methods
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

        private string ParseBillerMessage(DAS.h2hdasreply response, out string errorMessage)
        {
            string message = string.Empty;
            errorMessage = string.Empty;
            if (response != null)
            {
                DAS.REPLYType rType = (DAS.REPLYType)response.MTML.Item;
                if (rType != null && rType.DATA_CONTEXT != null)
                {
                    if (rType.DATA_CONTEXT.RECORDSET != null)
                    {
                        var templateLineTypes = rType.DATA_CONTEXT.RECORDSET.Items;
                        if (templateLineTypes != null && templateLineTypes.Count() > 0)
                        {
                            DASService.DASTEMPLATELINE_Type firstTemplateLineType = (DASService.DASTEMPLATELINE_Type)templateLineTypes.FirstOrDefault();
                            if (firstTemplateLineType != null && !string.IsNullOrWhiteSpace(firstTemplateLineType.DESCRIPTION))
                            {
                                string count = firstTemplateLineType.DESCRIPTION.Split(';')[0];
                                int messageCount = Convert.ToInt32(count);

                                StringBuilder messageBuilder = new StringBuilder();

                                for (int index = 1; index <= messageCount; index++)
                                {
                                    DASService.DASTEMPLATELINE_Type templateLineType = (DASService.DASTEMPLATELINE_Type)templateLineTypes[index];
                                    string description = templateLineType.DESCRIPTION;
                                    if (!string.IsNullOrWhiteSpace(description))
                                    {
                                        description = description.Split(';')[0];
                                        description = description.Substring(7);
                                        messageBuilder.Append(description);
                                        messageBuilder.Append(" ");
                                    }
                                }
                                message = messageBuilder.ToString();
                            }
                        }
                    }
                    if (rType.DATA_CONTEXT.HEADER != null)
                    {
                        errorMessage = rType.DATA_CONTEXT.HEADER.ERROR_MSG;
                    }
                }
            }
            return message;
        }

        #endregion
    }
}
