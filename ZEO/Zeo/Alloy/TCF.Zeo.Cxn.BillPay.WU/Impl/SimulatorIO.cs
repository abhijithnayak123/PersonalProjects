using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Cxn.BillPay.Data;
using TCF.Zeo.Cxn.BillPay.Data.Exceptions;
using TCF.Zeo.Cxn.BillPay.WU.Contract;
using TCF.Zeo.Cxn.BillPay.WU.Data;
using TCF.Zeo.Cxn.WU.Common.Data;
using System;
using System.Collections.Generic;
using System.Text;
using FusionGo = TCF.Zeo.Cxn.BillPay.WU.FusionGoShopping;
using WUPayment = TCF.Zeo.Cxn.BillPay.WU.MakePaymentStore;
using WUValidate = TCF.Zeo.Cxn.BillPay.WU.MakePaymentValidate;


namespace TCF.Zeo.Cxn.BillPay.WU.Impl
{
    public class SimulatorIO : BaseIO, IIO
    {
        public List<Field> GetBillerFields(string billerName, string locationName, ZeoContext context)
        {
            List<Field> field = null;
            switch (billerName)
            {
                case "WELLS FARGO IMPORTANT":
                    field = new List<Field>() {
                            new Field() { Label = "Attention", IsMandatory = false, MaxLength = 99, ValidationMessage = "99 Characters, No Lower Case", Values = null, DataType = "UPRSTRING", Mask = string.Empty, TagName = null } };
                    break;
                default:
                    field = new List<Field>();
                    break;
            }

            return field;
        }

        public string GetBillerMessage(string billerName, ZeoContext context)
        {
            string message = string.Format("The Biller {0} specific message from Western Union", billerName);

            return message;
        }

        public Fee GetDeliveryMethods(long wuTrxId, string billerName, string accountNumber, decimal amount, Location location, BillPaymentRequest billPaymentRequest, ZeoContext context)
        {
            Fee fee = null;

            FusionGo.fusiongoshoppingrequest request = null;

            long billAmount = ConvertDecimalToLong(amount);

            try
            {
                request = BuildFusionGoRequest(billPaymentRequest, location, context, billerName, accountNumber, billAmount);
                fee = GetMockDeliveryMethods(billerName, location, amount);
                //fee.TransactionId = transactionId;
            }
            catch (Exception ex)
            {
                throw new BillPayException(BillPayException.DELIVERY_METHODS_RETRIEVAL_FAILED, ex);
            }
            finally
            {
                //fee ?? new Fee();
                if (fee == null)
                {
                    fee = new Fee();
                }
                ProcessAPICall(wuTrxId,request, billPaymentRequest, context);
            }

            return fee;
        }

        public List<Location> GetLocations(string billerName, string accountNumber, decimal amount, BillPaymentRequest billPaymentRequest, ZeoContext context)
        {
            return GetMockLocationDetails(billerName);
        }

        public WUTransaction MakePayment(BillPayTransactionRequest trx, ZeoContext context)
        {
            WUTransaction wuTrx = new WUTransaction();
            WUPayment.channel channel = null;
            WUPayment.foreign_remote_system foreignRemoteSystem = null;
            WUPayment.makepaymentstorerequest request = null;
            WUPayment.makepaymentstorereply reply = null;

            try
            {
                WUBaseRequestResponse wuObjects = (WUBaseRequestResponse)context.Context["BaseWUObject"];
                BuildPaymentObjects(wuObjects, ref channel, ref foreignRemoteSystem);
                foreignRemoteSystem.reference_no = trx.ForeignRemoteSystemReferenceNo;

                Helper.RequestType requestStatus = (Helper.RequestType)Enum.Parse(typeof(Helper.RequestType), context.RequestType);

                request = new WUPayment.makepaymentstorerequest()
                {
                    channel = channel,
                    payment_transactions = BuildPaymentStoreTransactions(trx, requestStatus),
                    foreign_remote_system = foreignRemoteSystem,
                };

                reply = GetMockStoreReply(request, trx, requestStatus);

            }
            catch (Exception ex)
            {
                throw new BillPayException(BillPayException.BILLPAY_COMMIT_FAILED, ex);
            }
            finally
            {
                wuTrx = ProcessAPICall(request, reply, trx, context);
            }

            return wuTrx;
        }

        public WUTransaction ValidatePayment(long wuTrxId, BillPaymentRequest billPaymentRequest, ZeoContext context)
        {
            WUTransaction trx = new WUTransaction();
            WUBaseRequestResponse wuObjects = (WUBaseRequestResponse)context.Context["BaseWUObject"];

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
                reply = GetMockReply(billPaymentRequest);
            }
            catch (Exception ex)
            {
                throw new BillPayException(BillPayException.BILLPAY_VALIDATE_FAILED, ex);
            }
            finally
            {
                if (request != null)
                {
                    trx = ProcessAPICall(wuTrxId, request, reply, context);
                }
            }

            return trx;
        }


        #region Private
        private Fee GetMockDeliveryMethods(string billerName, Location location, decimal amount)
        {
            List<DeliveryMethod> delivery = null;
            Fee fee = null;
            string locationName = billerName;
            if (location != null)
            {
                locationName = location.Name;
            }
            switch (locationName)
            {
                case "KINGFISH FL":
                case "RACCHES VA":
                case "RACWEST NC":
                case "REGA NC":
                case "REGI NC":
                    delivery = new List<DeliveryMethod>(){
                    new DeliveryMethod(){ Code = "000", FeeAmount = GetFee(amount), Tax = 0M , Text = "Urgent"},
                    new DeliveryMethod(){ Code = "100", FeeAmount = GetFee(amount), Tax = 0M , Text = "2nd Business Day"},
                    new DeliveryMethod(){ Code = "200", FeeAmount = GetFee(amount), Tax = 0M , Text = "3rd Business Day"},
                    new DeliveryMethod(){ Code = "300", FeeAmount = GetFee(amount), Tax = 0M , Text = "Next Business Day"}
                    };
                    fee = new Fee()
                    {
                        SessionCookie = "070650201Y131012345618301411" + locationName + "1519" + billerName + "19040000",
                        DeliveryMethods = delivery,
                        AccountHolderName = string.Empty,
                        AvailableBalance = string.Empty,
                        CityCode = locationName,
                        TransactionId = 0
                    };
                    break;
                default:
                    delivery = new List<DeliveryMethod>() {
                    new DeliveryMethod(){ Code = "000", FeeAmount = GetFee(amount), Tax = 0M, Text ="Urgent"},
                    new DeliveryMethod(){ Code = "100", FeeAmount = GetFee(amount), Tax = 0M , Text = "2nd Business Day"},
                    new DeliveryMethod(){ Code = "200", FeeAmount = GetFee(amount), Tax = 0M , Text = "3rd Business Day"}
                    };
                    fee = new Fee()
                    {
                        SessionCookie = "0707613065372741412" + locationName + "1521" + billerName + "1713ZWFFASTPAY IA19040000",
                        DeliveryMethods = delivery,
                        AccountHolderName = string.Empty,
                        AvailableBalance = string.Empty,
                        CityCode = locationName,
                        TransactionId = 0
                    };
                    break;
            }

            return fee;
        }

        private decimal GetFee(decimal amount)
        {
            decimal percentage = 6.95M;
            if (amount >= 100 && amount <= 200)
            {
                percentage = Convert.ToDecimal((amount * 10 / 100).ToString("0.00"));
            }
            if (amount >= 201 && amount <= 499)
            {
                percentage = Convert.ToDecimal((amount * 9 / 100).ToString("0.00"));
            }
            if (amount >= 500)
            {
                percentage = Convert.ToDecimal((amount * 7 / 100).ToString("0.00"));
            }
            return percentage;
        }

        private List<Location> GetMockLocationDetails(string billerName)
        {
            List<Location> locations = null;
            switch (billerName.ToUpper())
            {
                case "REGIONAL ACCEPTANCE":
                    locations = new List<Location>() {
                        new Location() { Id = "1", Name = "KINGFISH FL", Type = "03" },
                        new Location() { Id = "2", Name = "RACCHES VA", Type = "03" },
                        new Location() { Id = "3", Name = "RACWEST NC", Type = "03" },
                        new Location() { Id = "4", Name = "RECRCY NC", Type = "03" },
                        new Location() { Id = "5", Name = "REGA NC", Type = "03" },
                        new Location() { Id = "6", Name = "REGI NC", Type = "03" },
                        new Location() { Id = "7", Name = "REGIONALCREDIT TX", Type = "03" }
                    };
                    break;
                default:
                    locations = new List<Location>();
                    break;
            }
            return locations;
        }

        private WUPayment.makepaymentstorereply GetMockStoreReply(WUPayment.makepaymentstorerequest request, BillPayTransactionRequest trx, Helper.RequestType requestStatus)
        {
            WUPayment.makepaymentstorereply reply = new WUPayment.makepaymentstorereply();

            reply.instant_notification = new WUPayment.instant_notification()
            {
                addl_service_block = new WUPayment.addl_service_block() { addl_service_length = 377 },
                addl_service_charges = "1103FEE120399513039950103MSG020100301096150101002010030109702NN9810USAUSA##J1"
            };

            reply.df_fields = new WUPayment.df_fields()
            {
                df_transaction_flag = WUPayment.yes_no.N,
                delivery_service_name = _deliveryCodeMapping[trx.DeliveryCode],
                pds_required_flag = WUPayment.yes_no.N
            };

            reply.payment_transactions = BuildPaymentStoreTransactions(trx, requestStatus);

            reply.payment_transactions.payment_transaction[0].filing_date = trx.FillingDate;

            reply.payment_transactions.payment_transaction[0].filing_time = trx.FillingTime;

            reply.payment_transactions.payment_transaction[0].mtcn = trx.Mtcn;

            reply.payment_transactions.payment_transaction[0].new_mtcn = trx.NewMTCN;

            reply.payment_transactions.payment_transaction[0].wu_card = new WUPayment.wu_card()
            {
                wu_card_pin_text = new string[10]
            };

            return reply;
        }

        internal string FindAndTokenizeCardNumber(string actualString, string cardNumber)
        {
            if (actualString.Contains(cardNumber))
            {
                string tokenizedCardNumber = Encrypt(cardNumber);
                actualString = actualString.Replace(cardNumber, tokenizedCardNumber);
            }

            return actualString;
        }

        private string GetPromotionMessageArea(string[] stringArray)
        {
            StringBuilder message = new StringBuilder();
            if (stringArray != null)
            {
                foreach (var item in stringArray)
                {
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        message.Append(item);
                    }
                }
            }
            return message.ToString();
        }

        private WUValidate.makepaymentvalidationreply GetMockReply(BillPaymentRequest billPaymentRequest)
        {
            WUValidate.makepaymentvalidationreply reply = new WUValidate.makepaymentvalidationreply();

            reply.payment_transactions = BuildPaymentTransactions(billPaymentRequest);

            reply.df_fields = new WUValidate.df_fields()
            {
                df_transaction_flag = WUValidate.yes_no.N,
                delivery_service_name = _deliveryCodeMapping[billPaymentRequest.DeliveryCode],
                pds_required_flag = WUValidate.yes_no.N
            };

            reply.payment_transactions.payment_transaction[0].financials.destination_principal_amount = ConvertDecimalToLong(billPaymentRequest.Amount);

            reply.payment_transactions.payment_transaction[0].financials.total_discounted_charges = reply.payment_transactions.payment_transaction[0].financials.charges;

            reply.payment_transactions.payment_transaction[0].financials.total_undiscounted_charges = reply.payment_transactions.payment_transaction[0].financials.charges;

            reply.payment_transactions.payment_transaction[0].filing_date = string.Format(DateTime.Now.ToString("MM-dd"));

            reply.payment_transactions.payment_transaction[0].filing_time = string.Format(DateTime.Now.ToString("HHMMt") + " " + TimeZoneInfo.Utc.DisplayName);

            reply.payment_transactions.payment_transaction[0].mtcn = Convert.ToString(Helper.GenerateRandomNumber(10));

            reply.payment_transactions.payment_transaction[0].new_mtcn = string.Format(Helper.GenerateRandomNumber(6) + reply.payment_transactions.payment_transaction[0].mtcn);

            return reply;
        }
        #endregion
    }
}

