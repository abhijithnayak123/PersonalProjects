using TCF.Zeo.Cxn.MoneyTransfer.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCF.Zeo.Cxn.MoneyTransfer.Data;
using P3Net.Data.Common;
using TCF.Zeo.Common.Data;
using System.Data;
using P3Net.Data;
using TCF.Zeo.Cxn.MoneyTransfer.WU.Contract;
using static TCF.Zeo.Common.Util.Helper;
using WUCommonData = TCF.Zeo.Cxn.WU.Common.Data;
using TCF.Zeo.Cxn.WU.Common.Contract;
using AutoMapper;
using TCF.Zeo.Cxn.MoneyTransfer.WU.ReceiveMoneyPay;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Cxn.MoneyTransfer.WU.ModifySendMoneySearch;
using System.Configuration;
using System.Globalization;
using WUData = TCF.Zeo.Cxn.MoneyTransfer.WU.Data;
using System.IO;
using TCF.Zeo.Cxn.MoneyTransfer.WU.ReceiveMoneySearch;
using TCF.Zeo.Cxn.Common;
using TCF.Zeo.Cxn.MoneyTransfer.Data.Exceptions;

namespace TCF.Zeo.Cxn.MoneyTransfer.WU.Impl
{
    public class WUGateway : ProcessorDAL, IMoneyTransferService
    {

        #region Depedencies 

        public IIO WUIO { get; set; }
        public IWUCommonIO WUCommonIO { get; set; }
        public IMapper Mapper { get; set; }

        #endregion

        #region Properties

        private const string CountryName = "United States";
        private const string CountryCode = "US";
        private const string CountryCurrencyCode = "USD";
        private const string Language = "es";
        private bool IsHardCodedCounterId { get; } = Convert.ToBoolean(ConfigurationManager.AppSettings["IsHardCodedCounterId"]);
        private string AllowDuplicateTrxWU = Convert.ToString(ConfigurationManager.AppSettings["AllowDuplicateTrxWU"]);

        #endregion
        public WUGateway()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WUCommonData.Sender, WUTransaction>();

                cfg.CreateMap<PaymentDetails, WUTransaction>()
                    .ForMember(x => x.ExpectedPayoutCityName, s => s.MapFrom(c => c.ExpectedPayoutLocCity))
                    .ForMember(x => x.ExpectedPayoutStateCode, s => s.MapFrom(c => c.ExpectedPayoutStateCode))
                    .ForMember(x => x.Charges, s => s.MapFrom(c => c.Fee))
                    .ForMember(x => x.DeliveryServiceName, s => s.MapFrom(c => c.DeliveryMethod))
                    .ForMember(x => x.plus_charges_amount, s => s.MapFrom(c => c.OtherFees))
                    .ForMember(x => x.PromotionDiscount, s => s.MapFrom(c => c.PromotionDiscount / 100m));
                cfg.CreateMap<WUTransaction, WUCommonData.Sender>();
                cfg.CreateMap<PaymentDetails, WUCommonData.CardDetails>();
                cfg.CreateMap<WUCommonData.Receiver, WUData.ImportReceiver>()
                    .ForMember(d => d.PickupCountry, s => s.MapFrom(c => c.Address.item.country_code))
                    .ForMember(d => d.CountryCode, s => s.MapFrom(c => c.Address.item.country_code));
                cfg.CreateMap<WUCommonData.SwbFlaInfo, FeeInquiry.swb_fla_info>()
                    .ForMember(d => d.swb_operator_id, s => s.MapFrom(c => c.SwbOperatorId))
                    .ForMember(d => d.read_privacynotice_flag, s => s.MapFrom(x => x.ReadPrivacyNoticeFlag))
                    .ForMember(d => d.read_privacynotice_flagSpecified, s => s.MapFrom(x => x.FlagCertificationFlagSpecified))
                    .ForMember(d => d.fla_certification_flag, s => s.MapFrom(x => x.FlagCertificationFlag))
                    .ForMember(d => d.fla_certification_flagSpecified, s => s.MapFrom(x => x.ReadPrivacyNoticeFlagSpecified));

                cfg.CreateMap<WUCommonData.GeneralName, FeeInquiry.general_name>()
                    .ForMember(d => d.name_type, s => s.MapFrom(x => x.Type))
                    .ForMember(d => d.name_typeSpecified, s => s.MapFrom(x => x.NameTypeSpecified))
                    .ForMember(d => d.first_name, s => s.MapFrom(x => x.FirstName))
                    .ForMember(d => d.last_name, s => s.MapFrom(x => x.LastName));

                cfg.CreateMap<WUCommonData.SwbFlaInfo, SendMoneyValidation.swb_fla_info>()
                    .ForMember(d => d.swb_operator_id, s => s.MapFrom(c => c.SwbOperatorId))
                    .ForMember(d => d.read_privacynotice_flag, s => s.MapFrom(x => x.ReadPrivacyNoticeFlag))
                    .ForMember(d => d.read_privacynotice_flagSpecified, s => s.MapFrom(x => x.FlagCertificationFlagSpecified))
                    .ForMember(d => d.fla_certification_flag, s => s.MapFrom(x => x.FlagCertificationFlag))
                    .ForMember(d => d.fla_certification_flagSpecified, s => s.MapFrom(x => x.ReadPrivacyNoticeFlagSpecified));

                cfg.CreateMap<WUCommonData.GeneralName, SendMoneyValidation.general_name>()
                    .ForMember(d => d.name_type, s => s.MapFrom(x => x.Type))
                    .ForMember(d => d.name_typeSpecified, s => s.MapFrom(x => x.NameTypeSpecified))
                    .ForMember(d => d.first_name, s => s.MapFrom(x => x.FirstName))
                    .ForMember(d => d.last_name, s => s.MapFrom(x => x.LastName));

                cfg.CreateMap<WUCommonData.SwbFlaInfo, SendMoneyStore.swb_fla_info>()
                    .ForMember(d => d.swb_operator_id, s => s.MapFrom(c => c.SwbOperatorId))
                    .ForMember(d => d.read_privacynotice_flag, s => s.MapFrom(x => x.ReadPrivacyNoticeFlag))
                    .ForMember(d => d.read_privacynotice_flagSpecified, s => s.MapFrom(x => x.FlagCertificationFlagSpecified))
                    .ForMember(d => d.fla_certification_flag, s => s.MapFrom(x => x.FlagCertificationFlag))
                    .ForMember(d => d.fla_certification_flagSpecified, s => s.MapFrom(x => x.ReadPrivacyNoticeFlagSpecified));

                cfg.CreateMap<WUCommonData.GeneralName, SendMoneyStore.general_name>()
                    .ForMember(d => d.name_type, s => s.MapFrom(x => x.Type))
                    .ForMember(d => d.name_typeSpecified, s => s.MapFrom(x => x.NameTypeSpecified))
                    .ForMember(d => d.first_name, s => s.MapFrom(x => x.FirstName))
                    .ForMember(d => d.last_name, s => s.MapFrom(x => x.LastName));

                cfg.CreateMap<WUCommonData.SwbFlaInfo, SendMoneyRefund.swb_fla_info>()
                    .ForMember(d => d.swb_operator_id, s => s.MapFrom(c => c.SwbOperatorId))
                    .ForMember(d => d.read_privacynotice_flag, s => s.MapFrom(x => x.ReadPrivacyNoticeFlag))
                    .ForMember(d => d.read_privacynotice_flagSpecified, s => s.MapFrom(x => x.FlagCertificationFlagSpecified))
                    .ForMember(d => d.fla_certification_flag, s => s.MapFrom(x => x.FlagCertificationFlag))
                    .ForMember(d => d.fla_certification_flagSpecified, s => s.MapFrom(x => x.ReadPrivacyNoticeFlagSpecified));

                cfg.CreateMap<WUCommonData.GeneralName, SendMoneyRefund.general_name>()
                    .ForMember(d => d.name_type, s => s.MapFrom(x => x.Type))
                    .ForMember(d => d.name_typeSpecified, s => s.MapFrom(x => x.NameTypeSpecified))
                    .ForMember(d => d.first_name, s => s.MapFrom(x => x.FirstName))
                    .ForMember(d => d.last_name, s => s.MapFrom(x => x.LastName));

                cfg.CreateMap<WUCommonData.SwbFlaInfo, ModifySendMoney.swb_fla_info>()
                    .ForMember(d => d.swb_operator_id, s => s.MapFrom(c => c.SwbOperatorId))
                    .ForMember(d => d.read_privacynotice_flag, s => s.MapFrom(x => x.ReadPrivacyNoticeFlag))
                    .ForMember(d => d.read_privacynotice_flagSpecified, s => s.MapFrom(x => x.FlagCertificationFlagSpecified))
                    .ForMember(d => d.fla_certification_flag, s => s.MapFrom(x => x.FlagCertificationFlag))
                    .ForMember(d => d.fla_certification_flagSpecified, s => s.MapFrom(x => x.ReadPrivacyNoticeFlagSpecified));

                cfg.CreateMap<WUCommonData.GeneralName, ModifySendMoney.general_name>()
                    .ForMember(d => d.name_type, s => s.MapFrom(x => x.Type))
                    .ForMember(d => d.name_typeSpecified, s => s.MapFrom(x => x.NameTypeSpecified))
                    .ForMember(d => d.first_name, s => s.MapFrom(x => x.FirstName))
                    .ForMember(d => d.last_name, s => s.MapFrom(x => x.LastName));

                cfg.CreateMap<WUCommonData.SwbFlaInfo, ReceiveMoneySearch.swb_fla_info>()
                    .ForMember(d => d.swb_operator_id, s => s.MapFrom(c => c.SwbOperatorId))
                    .ForMember(d => d.read_privacynotice_flag, s => s.MapFrom(x => x.ReadPrivacyNoticeFlag))
                    .ForMember(d => d.read_privacynotice_flagSpecified, s => s.MapFrom(x => x.FlagCertificationFlagSpecified))
                    .ForMember(d => d.fla_certification_flag, s => s.MapFrom(x => x.FlagCertificationFlag))
                    .ForMember(d => d.fla_certification_flagSpecified, s => s.MapFrom(x => x.ReadPrivacyNoticeFlagSpecified));

                cfg.CreateMap<WUCommonData.GeneralName, ReceiveMoneySearch.general_name>()
                    .ForMember(d => d.name_type, s => s.MapFrom(x => x.Type))
                    .ForMember(d => d.name_typeSpecified, s => s.MapFrom(x => x.NameTypeSpecified))
                    .ForMember(d => d.first_name, s => s.MapFrom(x => x.FirstName))
                    .ForMember(d => d.last_name, s => s.MapFrom(x => x.LastName));

                cfg.CreateMap<WUCommonData.SwbFlaInfo, ReceiveMoneyPay.swb_fla_info>()
                    .ForMember(d => d.swb_operator_id, s => s.MapFrom(c => c.SwbOperatorId))
                    .ForMember(d => d.read_privacynotice_flag, s => s.MapFrom(x => x.ReadPrivacyNoticeFlag))
                    .ForMember(d => d.read_privacynotice_flagSpecified, s => s.MapFrom(x => x.FlagCertificationFlagSpecified))
                    .ForMember(d => d.fla_certification_flag, s => s.MapFrom(x => x.FlagCertificationFlag))
                    .ForMember(d => d.fla_certification_flagSpecified, s => s.MapFrom(x => x.ReadPrivacyNoticeFlagSpecified));

                cfg.CreateMap<WUCommonData.GeneralName, ReceiveMoneyPay.general_name>()
                    .ForMember(d => d.name_type, s => s.MapFrom(x => x.Type))
                    .ForMember(d => d.name_typeSpecified, s => s.MapFrom(x => x.NameTypeSpecified))
                    .ForMember(d => d.first_name, s => s.MapFrom(x => x.FirstName))
                    .ForMember(d => d.last_name, s => s.MapFrom(x => x.LastName));

                cfg.CreateMap<PaymentDetails, WUCommonData.CardDetails>()
                    .AfterMap((paymentDetail, CardDetails) => CardDetails.paymentDetails = new WUCommonData.PaymentDetails()
                    {
                        destination_country_currency = new WUCommonData.CountryCurrencyInfo() { country_code = paymentDetail.DestinationCountryCode, currency_code = paymentDetail.DestinationCurrencyCode },
                        originating_country_currency = new WUCommonData.CountryCurrencyInfo() { country_code = paymentDetail.OriginatingCountryCode, currency_code = paymentDetail.OriginatingCurrencyCode },
                        recording_country_currency = new WUCommonData.CountryCurrencyInfo() { country_code = paymentDetail.RecordingcountrycurrencyCountryCode, currency_code = paymentDetail.RecordingcountrycurrencyCurrencyCode }
                    });

                cfg.CreateMap<Account, WUCommonData.CardDetails>()
                    .AfterMap((sender, CardDetails) => CardDetails.sender = new WUCommonData.Sender()
                    {
                        FirstName = sender.FirstName,
                        LastName = sender.LastName,
                        AddressAddrLine1 = sender.Address,
                        AddressCity = sender.City,
                        AddressState = sender.State,
                        AddressPostalCode = sender.PostalCode,
                        ContactPhone = sender.ContactPhone,
                    });
                cfg.CreateMap<WUCommonData.CardDetails, CardDetails>();
            });
            Mapper = config.CreateMapper();
        }

        public long AddReceiver(Receiver receiver)
        {
            try
            {
                long receiverId = 0;
                bool isReceiverExists = false;

                StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_SaveReceiver");

                moneyTransferProcedure.WithParameters(InputParameter.Named("receiverId").WithValue(receiver.Id));
                moneyTransferProcedure.WithParameters(InputParameter.Named("customerId").WithValue(receiver.CustomerId));
                moneyTransferProcedure.WithParameters(InputParameter.Named("firstName").WithValue(receiver.FirstName));
                moneyTransferProcedure.WithParameters(InputParameter.Named("lastName").WithValue(receiver.LastName));
                moneyTransferProcedure.WithParameters(InputParameter.Named("secondLastName").WithValue(receiver.SecondLastName));
                moneyTransferProcedure.WithParameters(InputParameter.Named("deliveryMethod").WithValue(receiver.DeliveryMethod));
                moneyTransferProcedure.WithParameters(InputParameter.Named("deliveryOption").WithValue(receiver.DeliveryOption));
                moneyTransferProcedure.WithParameters(InputParameter.Named("status").WithValue(receiver.Status));
                moneyTransferProcedure.WithParameters(InputParameter.Named("goldCardNumber").WithValue(receiver.GoldCardNumber));
                moneyTransferProcedure.WithParameters(InputParameter.Named("pickupCity").WithValue(receiver.PickupCity));
                moneyTransferProcedure.WithParameters(InputParameter.Named("pickupCountry").WithValue(receiver.PickupCountry));
                moneyTransferProcedure.WithParameters(InputParameter.Named("pickupStateProvince").WithValue(receiver.PickupState_Province));
                moneyTransferProcedure.WithParameters(InputParameter.Named("receiverIndexNo").WithValue(receiver.ReceiverIndexNo));
                moneyTransferProcedure.WithParameters(InputParameter.Named("address").WithValue(receiver.Address));
                moneyTransferProcedure.WithParameters(InputParameter.Named("stateProvince").WithValue(receiver.State_Province));
                moneyTransferProcedure.WithParameters(InputParameter.Named("city").WithValue(receiver.City));
                moneyTransferProcedure.WithParameters(InputParameter.Named("zipCode").WithValue(receiver.ZipCode));
                moneyTransferProcedure.WithParameters(InputParameter.Named("phoneNumber").WithValue(receiver.PhoneNumber));
                moneyTransferProcedure.WithParameters(InputParameter.Named("dtTerminalDate").WithValue(receiver.DTTerminalCreate));
                moneyTransferProcedure.WithParameters(InputParameter.Named("dtServerDate").WithValue(receiver.DTServerCreate));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(moneyTransferProcedure))
                {
                    while (datareader.Read())
                    {
                        receiverId = datareader.GetInt64OrDefault("receiverID");
                        isReceiverExists = datareader.GetBooleanOrDefault("isReceiverExists");
                    }
                }

                if (isReceiverExists)
                    throw new MoneyTransferException(MoneyTransferException.RECEIVER_ALREADY_EXISTED);

                return receiverId;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_ADDRECEIVER_FAILED, ex);
            }
        }

        public long UpdateReceiver(Receiver receiver)
        {
            try
            {
                bool isReceiverExists = false;

                StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_SaveReceiver");

                moneyTransferProcedure.WithParameters(InputParameter.Named("receiverId").WithValue(receiver.Id));
                moneyTransferProcedure.WithParameters(InputParameter.Named("customerId").WithValue(receiver.CustomerId));
                moneyTransferProcedure.WithParameters(InputParameter.Named("firstName").WithValue(receiver.FirstName));
                moneyTransferProcedure.WithParameters(InputParameter.Named("lastName").WithValue(receiver.LastName));
                moneyTransferProcedure.WithParameters(InputParameter.Named("secondLastName").WithValue(receiver.SecondLastName));
                moneyTransferProcedure.WithParameters(InputParameter.Named("deliveryMethod").WithValue(receiver.DeliveryMethod));
                moneyTransferProcedure.WithParameters(InputParameter.Named("deliveryOption").WithValue(receiver.DeliveryOption));
                moneyTransferProcedure.WithParameters(InputParameter.Named("status").WithValue(receiver.Status));
                moneyTransferProcedure.WithParameters(InputParameter.Named("goldCardNumber").WithValue(receiver.GoldCardNumber));
                moneyTransferProcedure.WithParameters(InputParameter.Named("pickupCity").WithValue(receiver.PickupCity));
                moneyTransferProcedure.WithParameters(InputParameter.Named("pickupCountry").WithValue(receiver.PickupCountry));
                moneyTransferProcedure.WithParameters(InputParameter.Named("pickupStateProvince").WithValue(receiver.PickupState_Province));
                moneyTransferProcedure.WithParameters(InputParameter.Named("receiverIndexNo").WithValue(receiver.ReceiverIndexNo));
                moneyTransferProcedure.WithParameters(InputParameter.Named("dtTerminalDate").WithValue(receiver.DTTerminalLastModified));
                moneyTransferProcedure.WithParameters(InputParameter.Named("dtServerDate").WithValue(receiver.DTServerLastModified));
                moneyTransferProcedure.WithParameters(InputParameter.Named("address").WithValue(receiver.Address));
                moneyTransferProcedure.WithParameters(InputParameter.Named("stateProvince").WithValue(receiver.State_Province));
                moneyTransferProcedure.WithParameters(InputParameter.Named("city").WithValue(receiver.City));
                moneyTransferProcedure.WithParameters(InputParameter.Named("zipCode").WithValue(receiver.ZipCode));
                moneyTransferProcedure.WithParameters(InputParameter.Named("phoneNumber").WithValue(receiver.PhoneNumber));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(moneyTransferProcedure))
                {
                    while (datareader.Read())
                    {
                        isReceiverExists = datareader.GetBooleanOrDefault("isReceiverExists");
                    }
                }

                if (isReceiverExists)
                    throw new MoneyTransferException(MoneyTransferException.RECEIVER_ALREADY_EXISTED);

                return receiver.Id;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_UPDATERECEIVER_FAILED, ex);
            }
        }

        public void UpdateReceiverDetails(long transactionId, long customerSessionId, DateTime dtTerminalDate, DateTime dtServerDate)
        {
            try
            {
                StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_UpdateReceiverDetails");

                moneyTransferProcedure.WithParameters(InputParameter.Named("wuTrxId").WithValue(transactionId));
                moneyTransferProcedure.WithParameters(InputParameter.Named("customerSessionId").WithValue(customerSessionId));
                moneyTransferProcedure.WithParameters(InputParameter.Named("dtTerminalDate").WithValue(dtTerminalDate));
                moneyTransferProcedure.WithParameters(InputParameter.Named("dtServerDate").WithValue(dtServerDate));

                DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(moneyTransferProcedure);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_UPDATERECEIVER_FAILED, ex);
            }
        }

        public Receiver GetReceiver(long receiverId)
        {
            try
            {
                Receiver receiver = null;

                StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_GetReceiverById");

                moneyTransferProcedure.WithParameters(InputParameter.Named("receiverId").WithValue(receiverId));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(moneyTransferProcedure))
                {
                    while (datareader.Read())
                    {
                        receiver = new Receiver();
                        receiver.CustomerId = datareader.GetInt64OrDefault("CustomerID");
                        receiver.Id = receiverId;
                        receiver.FirstName = datareader.GetStringOrDefault("FirstName");
                        receiver.LastName = datareader.GetStringOrDefault("LastName");
                        receiver.SecondLastName = datareader.GetStringOrDefault("SecondLastName");
                        receiver.Status = datareader.GetStringOrDefault("Status");
                        receiver.GoldCardNumber = datareader.GetStringOrDefault("GoldCardNumber");
                        receiver.PickupCountry = datareader.GetStringOrDefault("PickupCountry");
                        receiver.PickupCity = datareader.GetStringOrDefault("PickupCity");
                        receiver.PickupState_Province = datareader.GetStringOrDefault("PickupStateProvince");
                        receiver.DeliveryMethod = datareader.GetStringOrDefault("DeliveryMethod");
                        receiver.DeliveryOption = datareader.GetStringOrDefault("DeliveryOption");
                        receiver.ReceiverIndexNo = datareader.GetStringOrDefault("ReceiverIndexNo");
                        receiver.Address = datareader.GetStringOrDefault("Address");
                        receiver.City = datareader.GetStringOrDefault("City");
                        receiver.State_Province = datareader.GetStringOrDefault("StateProvince");
                        receiver.ZipCode = datareader.GetStringOrDefault("ZipCode");
                        receiver.PhoneNumber = datareader.GetStringOrDefault("PhoneNumber");
                    }
                }

                return receiver;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Receiver> GetFrequentReceivers(long customerId)
        {
            try
            {
                List<Receiver> receivers = new List<Receiver>();
                Receiver receiver = null;

                StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_GetFrequentReceivers");

                moneyTransferProcedure.WithParameters(InputParameter.Named("customerId").WithValue(customerId));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(moneyTransferProcedure))
                {
                    while (datareader.Read())
                    {
                        receiver = new Receiver();
                        receiver.FirstName = datareader.GetStringOrDefault("FirstName");
                        receiver.LastName = datareader.GetStringOrDefault("LastName");
                        receiver.SecondLastName = datareader.GetStringOrDefault("SecondLastName");
                        receiver.Gender = datareader.GetStringOrDefault("Gender");
                        receiver.PickupCountry = datareader.GetStringOrDefault("PickupCountry");
                        receiver.State_Province = datareader.GetStringOrDefault("StateProvince");
                        receiver.City = datareader.GetStringOrDefault("City");
                        receiver.Status = datareader.GetStringOrDefault("Status");
                        receiver.Id = datareader.GetInt64OrDefault("ReceiverID");
                        receivers.Add(receiver);
                    }
                }

                return receivers;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETFREQUENTRECEIVERS_FAILED, ex);
            }
        }

        public List<MasterData> GetXfrCities(string stateCode)
        {
            try
            {
                List<MasterData> masterCities = new List<MasterData>();

                StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_GetWUcities");

                moneyTransferProcedure.WithParameters(InputParameter.Named("stateCode").WithValue(stateCode));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(moneyTransferProcedure))
                {
                    while (datareader.Read())
                    {
                        masterCities.Add(new MasterData
                        {
                            Id = datareader.GetInt64OrDefault("Id"),
                            Code = datareader.GetStringOrDefault("Name").ToString(),
                            Name = datareader.GetStringOrDefault("Name")
                        });
                    }
                }

                return masterCities;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETCITIES_FAILED, ex);
            }
        }

        public List<MasterData> GetXfrCountries()
        {
            try
            {
                List<MasterData> masterCountries = new List<MasterData>();

                StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_GetWUCountries");

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(moneyTransferProcedure))
                {
                    while (datareader.Read())
                    {
                        masterCountries.Add(new MasterData
                        {
                            Code = datareader.GetStringOrDefault("Code"),
                            Name = datareader.GetStringOrDefault("Name")
                        });
                    }
                }

                return masterCountries;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETCOUNTRIES_FAILED, ex);
            }
        }

        public List<MasterData> GetXfrStates(string countryCode)
        {
            try
            {
                List<MasterData> masterStates = new List<MasterData>();

                StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_GetWUstates");

                moneyTransferProcedure.WithParameters(InputParameter.Named("countryCode").WithValue(countryCode));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(moneyTransferProcedure))
                {
                    while (datareader.Read())
                    {
                        masterStates.Add(new MasterData
                        {
                            Code = datareader.GetStringOrDefault("Code"),
                            Id = datareader.GetInt64OrDefault("Id"),
                            Name = datareader.GetStringOrDefault("Name")
                        });
                    }
                }

                return masterStates;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETSTATES_FAILED, ex);
            }
        }

        private WUAccount GetWUAccount(long customerSessionId)
        {
            WUAccount wuAccount = null;

            StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_GetWUCardAccountInfo");

            moneyTransferProcedure.WithParameters(InputParameter.Named("customerSessionId").WithValue(customerSessionId));

            using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(moneyTransferProcedure))
            {
                while (datareader.Read())
                {
                    wuAccount = new WUAccount();
                    wuAccount.Id = datareader.GetInt64OrDefault("WUAccountId");
                    wuAccount.PreferredCustomerAccountNumber = datareader.GetStringOrDefault("PreferredCustomerAccountNumber");
                    wuAccount.NameType = datareader.GetStringOrDefault("NameType");
                    wuAccount.PreferredCustomerLevelCode = datareader.GetStringOrDefault("PreferredCustomerLevelCode");
                    wuAccount.SmsNotificationFlag = datareader.GetStringOrDefault("SmsNotificationFlag");
                    wuAccount.FirstName = datareader.GetStringOrDefault("FirstName");
                    wuAccount.MiddleName = datareader.GetStringOrDefault("MiddleName");
                    wuAccount.LastName = datareader.GetStringOrDefault("LastName");
                    wuAccount.SecondLastName = datareader.GetStringOrDefault("SecondLastName");
                }
            }

            return wuAccount;
        }

        public List<DeliveryService> GetDeliveryServices(DeliveryServiceRequest request, ZeoContext context)
        {
            try
            {
                WUIO = GetMTProcessor();
                CheckCounterId(context.WUCounterId);
                var deliveryServices = new List<DeliveryService>();

                string state = string.Empty;
                string stateCode = string.Empty;
                string city = string.Empty;
                string deliveryService = string.Empty;

                ValidateDeliveryServices(request, context);

                state = Convert.ToString(request.MetaData["State"]);
                stateCode = Convert.ToString(request.MetaData["StateCode"]);
                city = Convert.ToString(request.MetaData["City"]);

                if (request.Type == DeliveryServiceType.Option)
                {
                    deliveryService = Convert.ToString(request.MetaData["DeliveryService"]);
                }

                deliveryServices = WUIO.GetDeliveryServices(request, state, stateCode, city, deliveryService, context);
                return deliveryServices;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETDELIVERYSERVICES_FAILED, ex);
            }
        }

        public FeeResponse GetFee(FeeRequest feeRequest, ZeoContext context)
        {
            FeeInquiry.feeinquiryrequest feeInquiryRequest = BuildFeeEnquiryRequest(feeRequest);
            long wuTransactionId = 0;
            try
            {
                CheckCounterId(context.WUCounterId);
                WUCommonIO = GetCommonProcessor();
                WUIO = GetMTProcessor();
                feeInquiryRequest.swb_fla_info = Mapper.Map<WUCommonData.SwbFlaInfo, FeeInquiry.swb_fla_info>(WUCommonIO.BuildSwbFlaInfo(context));
                feeInquiryRequest.swb_fla_info.fla_name = Mapper.Map<WUCommonData.GeneralName, FeeInquiry.general_name>(WUCommonIO.BuildGeneralName(context));
                FeeResponse feeResponse = null;
                decimal transferAmount = feeRequest.Amount > 0 ? feeRequest.Amount : feeRequest.ReceiveAmount;

                //Checking for CXN Transaction Id, If its not there then create it and then update it based on the response got from Fee IO call.  
                wuTransactionId = feeRequest.TransactionId;
                if (feeRequest.TransactionId == 0)
                {
                    wuTransactionId = CreateTrx(feeInquiryRequest, feeRequest, context);
                    feeRequest.TransactionId = wuTransactionId;
                }

                FeeInquiry.feeinquiryreply reply = WUIO.FeeInquiry(feeInquiryRequest, context);
                feeResponse = MapfeeEnquiryResponse(reply);
                feeResponse.WuTrxId = wuTransactionId;
                UpdateTrx(wuTransactionId, feeRequest, feeResponse, context);

                return feeResponse;
            }
            catch (Exception ex)
            {
                if (wuTransactionId != 0)
                {
                    UpdateTrx(feeRequest, feeInquiryRequest, context);
                }
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETFEE_FAILED, ex);
            }
        }


        public ValidateResponse Validate(ValidateRequest validateRequest, ZeoContext context)
        {
            try
            {
                CheckCounterId(context.WUCounterId);

                WUCommonIO = GetCommonProcessor();
                WUIO = GetMTProcessor();
                long customerSessionId = context.CustomerSessionId;
                GetValidateRequest(customerSessionId, validateRequest.TransactionId, ref validateRequest);

                bool hasLPMTError = false;
                if (validateRequest != null)
                {
                    if (string.IsNullOrEmpty(context.ReferenceNumber))
                    {
                        context.ReferenceNumber = validateRequest.ReferenceNo;
                    }

                    if (validateRequest.TransferType == MoneyTransferType.Receive)
                    {

                        if (string.IsNullOrEmpty(context.RMTrxType))
                        {
                            context.RMTrxType = ReceiveMoneyPay.mt_requested_status.HOLD.ToString();
                        }

                        ReceiveMoneyPay.receivemoneypayrequest receivemoneypayrequest = PopulateReceiveMoneyPayRequest(customerSessionId, validateRequest, context);

                        ReceiveMoneyPay.receivemoneypayreply reply = WUIO.ReceiveMoneyPay(receivemoneypayrequest, context);

                        WUTransaction transaction = new WUTransaction();

                        transaction.PaidDateTime = reply.paid_date_time;
                        transaction.Id = validateRequest.TransactionId;
                        transaction.MTCN = reply.mtcn;
                        transaction.pay_or_do_not_pay_indicator = pay_or_do_not_pay_indicator.P.ToString();
                        transaction.SenderComplianceDetailsComplianceDataBuffer = reply.receiver.compliance_details.compliance_data_buffer;
                        transaction.TranascationType = MoneyTransferType.Receive.ToString();
                        transaction.DTServerLastModified = DateTime.Now;
                        transaction.DestinationPrincipalAmount = validateRequest.DestinationPrincipalAmount;
                        transaction.DTTerminalLastModified = GetTimeZoneTime(context.TimeZone);

                        UpdateWUTransaction(transaction, UpdateTxType.ValidateRequest, 0);

                    }
                    if (validateRequest.TransferType == MoneyTransferType.Send)
                    {
                        bool proceedWithLPMTError = Convert.ToBoolean(GetDictionaryValue(validateRequest.MetaData, "ProceedWithLPMTError"));

                        if (!proceedWithLPMTError)
                        {
                            SendMoneyValidation.sendmoneyvalidationrequest sendMoneyValidationRequest = MapSendMoneyValidateRequest(validateRequest, customerSessionId);
                            if (sendMoneyValidationRequest != null)
                            {
                                sendMoneyValidationRequest.swb_fla_info = Mapper.Map<WUCommonData.SwbFlaInfo, SendMoneyValidation.swb_fla_info>(WUCommonIO.BuildSwbFlaInfo(context));
                                sendMoneyValidationRequest.swb_fla_info.fla_name = Mapper.Map<WUCommonData.GeneralName, SendMoneyValidation.general_name>(WUCommonIO.BuildGeneralName(context));
                                SendMoneyValidation.sendmoneyvalidationreply response = WUIO.SendMoneyValidate(sendMoneyValidationRequest, context);
                                if (response != null)
                                {
                                    UpdateTrx(validateRequest.TransactionId, response, validateRequest, context.TimeZone);

                                }
                            }
                        }

                        context.SMTrxType = SendMoneyStore.mt_requested_status.HOLD.ToString();

                        hasLPMTError = Commit(validateRequest.TransactionId, context);
                    }
                }
                return new ValidateResponse() { TransactionId = validateRequest.TransactionId, HasLPMTError = hasLPMTError };

            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_VALIDATE_FAILED, ex);
            }
        }

        public SearchResponse Search(SearchRequest request, ZeoContext context)
        {
            try
            {
                CheckCounterId(context.WUCounterId);
                if (request.SearchRequestType == SearchRequestType.Modify)
                {
                    return SearchModify(request, context);
                }
                else
                {
                    return SearchRefund(request, context);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_SEARCH_FAILED, ex);
            }
        }

        public string GetStatus(string confirmationNumber, ZeoContext context)
        {
            try
            {
                WUIO = GetMTProcessor();
                CheckCounterId(context.WUCounterId);
                SendMoneyPayStatus.paystatusinquiryrequestdata searchRequest = new SendMoneyPayStatus.paystatusinquiryrequestdata()
                {
                    mtcn = confirmationNumber
                };

                SendMoneyPayStatus.paystatusinquiryreply searchResponse = WUIO.GetPayStatus(searchRequest, context);
                return searchResponse.payment_transactions.payment_transaction[0].pay_status_description;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETSTATUS_FAILED, ex);
            }
        }

        public List<Reason> GetRefundReasons(ReasonRequest request, ZeoContext context)
        {
            try
            {
                WUIO = GetMTProcessor();
                return WUIO.GetRefundReasons(request, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETREFUNDREASONS_FAILED, ex);
            }
        }

        public void Modify(long wuTrxId, ZeoContext context)
        {
            try
            {
                CheckCounterId(context.WUCounterId);
                WUIO = GetMTProcessor();
                ModifySendMoneyRequest modifyRequest = GetSendMoneyModifyRequest(wuTrxId);

                SearchRequest searchRequest = new SearchRequest();

                if (modifyRequest != null && modifyRequest.Transaction != null)
                {
                    searchRequest.ConfirmationNumber = modifyRequest.Transaction.MTCN;
                    searchRequest.ReferenceNumber = modifyRequest.Transaction.ReferenceNo;
                    context.ReferenceNumber = modifyRequest.Transaction.ReferenceNo;
                }

                ModifySendMoneySearch.modifysendmoneysearchreply modifySendMoneySearchResponse = GetModifySearchResponse(searchRequest, context);

                UpdateSendMoneySearchTransaction(wuTrxId, modifySendMoneySearchResponse, context);
                modifyRequest.Transaction.Sender_unv_Buffer = modifySendMoneySearchResponse.payment_transactions.payment_transaction[0].sender.unv_buffer;
                modifyRequest.Transaction.Receiver_unv_Buffer = modifySendMoneySearchResponse.payment_transactions.payment_transaction[0].receiver.unv_buffer;
                modifyRequest.Transaction.MoneyTransferKey = modifySendMoneySearchResponse.payment_transactions.payment_transaction[0].money_transfer_key;
                string status = modifySendMoneySearchResponse.payment_transactions.payment_transaction[0].fusion != null ? modifySendMoneySearchResponse.payment_transactions.payment_transaction[0].fusion.fusion_status : string.Empty;
                if (status.Equals("W/C"))
                {
                    ModifySendMoney.modifysendmoneyrequest modifySendMoneyRequest = PopulateModifySendMoneyRequest(modifyRequest, context);
                    ModifySendMoney.modifysendmoneyreply response = WUIO.Modify(modifySendMoneyRequest, context);
                    if (response != null)
                    {
                        UpdateSendMoneyModifyTransaction(wuTrxId, response, context);
                    }
                }
                else if (status.Equals("HOLD"))
                {
                    throw new MoneyTransferException(MoneyTransferException.MODIFY_TRANSACTION_NOT_ALLOWED);
                }
                else
                {
                    throw new MoneyTransferException(MoneyTransferException.TRANSACTION_ALREADY_PAID);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_MODIFY_FAILED, ex);
            }
        }

        public string Refund(RefundRequest refundRequest, ZeoContext context)
        {
            try
            {
                WUIO = GetMTProcessor();
                CheckCounterId(context.WUCounterId);
                //context.Add("ReferenceNo", trx.ReferenceNo);

                GetRefundRequest(refundRequest, context.CustomerSessionId);

                SendMoneyRefund.refundrequest request = PopulateRefundSendMoneyRequest(refundRequest, context);

                context.ReferenceNumber = DateTime.Now.ToString("yyyyMMddhhmmssff");
                SendMoneyRefund.refundreply response = WUIO.Refund(request, context);

                if (response != null)
                {
                    WUTransaction transaction = new WUTransaction();
                    transaction.Id = refundRequest.TransactionId;
                    transaction.ReferenceNo = context.ReferenceNumber;
                    transaction.DTServerLastModified = DateTime.Now;
                    transaction.DTTerminalLastModified = GetTimeZoneTime(context.TimeZone);
                    transaction.MTCN = response.mtcn;
                    transaction.TempMTCN = response.mtcn;
                    transaction.Charges = response.financials.charges / 100;
                    transaction.GrossTotalAmount = response.financials.gross_total_amount / 100;
                    transaction.DestinationPrincipalAmount = response.financials.pay_amount / 100;
                    transaction.OriginatorsPrincipalAmount = response.financials.principal_amount / 100;

                    UpdateSendMoneyRefundTransaction(transaction);
                }
                return response.mtcn;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_REFUND_FAILED, ex);
            }
        }

        public ModifyResponse StageModify(ModifyRequest modifySendMoney, ZeoContext context)
        {
            int modifySubType = (int)TransactionSubType.Modify;
            DateTime terminalDate = Helper.GetTimeZoneTime(context.TimeZone);

            //TODO need to create cancel and modify record in tWUTrx
            try
            {
                ModifyResponse modifyResp = null;

                StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_SendMoneyModifyorRefund");

                moneyTransferProcedure.WithParameters(InputParameter.Named("WUTrxId").WithValue(modifySendMoney.TransactionId));
                moneyTransferProcedure.WithParameters(InputParameter.Named("customerId").WithValue(context.CustomerId));
                moneyTransferProcedure.WithParameters(InputParameter.Named("receiverFname").WithValue(modifySendMoney.FirstName));
                moneyTransferProcedure.WithParameters(InputParameter.Named("receiverLname").WithValue(modifySendMoney.LastName));
                moneyTransferProcedure.WithParameters(InputParameter.Named("receiverSecondLname").WithValue(modifySendMoney.SecondLastName));
                moneyTransferProcedure.WithParameters(InputParameter.Named("referenceNo").WithValue(DateTime.Now.ToString("yyyyMMddhhmmssff")));
                moneyTransferProcedure.WithParameters(InputParameter.Named("testQuestion").WithValue(modifySendMoney.TestQuestion));
                moneyTransferProcedure.WithParameters(InputParameter.Named("testAnswer").WithValue(modifySendMoney.TestAnswer));
                moneyTransferProcedure.WithParameters(InputParameter.Named("transactionSubType").WithValue(modifySubType));
                moneyTransferProcedure.WithParameters(InputParameter.Named("dtTerminalLastModified").WithValue(terminalDate));
                moneyTransferProcedure.WithParameters(InputParameter.Named("dtServerLastModified").WithValue(DateTime.Now));
                bool IsOtherReceiverExists = false;
                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(moneyTransferProcedure))
                {
                    while (datareader.Read())
                    {
                        modifyResp = new ModifyResponse();
                        modifyResp.CancelTransactionId = datareader.GetInt64OrDefault("CancelTransactionId");
                        modifyResp.ModifyTransactionId = datareader.GetInt64OrDefault("ModifyorRefundTransactionId");
                        IsOtherReceiverExists = datareader.GetBooleanOrDefault("IsOtherReceiverExists");
                    }
                }

                if (IsOtherReceiverExists)
                {
                    throw new MoneyTransferException(MoneyTransferException.RECEIVER_ALREADY_EXISTED);
                }

                return modifyResp;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_STAGEMODIFY_FAILED, ex);
            }

        }

        public long AddAccount(Account account, ZeoContext context)
        {
            try
            {
                WUAccount wuAccount = new WUAccount()
                {
                    NameType = "D",
                    PreferredCustomerAccountNumber = account.LoyaltyCardNumber,
                    PreferredCustomerLevelCode = account.LevelCode,
                    DTServerCreate = account.DTServerCreate,
                    DTTerminalCreate = account.DTTerminalCreate,
                    CustomerId = account.CustomerID,
                    CustomerSessionId = account.CustomerSessionId
                };

                return CreateWUAccount(wuAccount);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_ADDACCOUNT_FAILED, ex);
            }
        }

        /// <summary>
        /// This method is used for updating the WU card number in Billpay and MoneyTransfer account tables. 
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <param name="account"></param>
        public void UpdateAccountWithCardNumber(long customerSessionId, Account account)
        {
            try
            {
                StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_CardEnrollment");

                moneyTransferProcedure.WithParameters(InputParameter.Named("customerSessionId").WithValue(customerSessionId));
                moneyTransferProcedure.WithParameters(InputParameter.Named("preferredCustomerAccountNumber").WithValue(account.LoyaltyCardNumber));
                moneyTransferProcedure.WithParameters(InputParameter.Named("dtServerDate").WithValue(account.DTServerLastModified));
                moneyTransferProcedure.WithParameters(InputParameter.Named("dtTerminalDate").WithValue(account.DTTerminalLastModified));

                DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(moneyTransferProcedure);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_UPDATEWUCARD_FAILED, ex);
            }
        }

        public bool Commit(long transactionId, ZeoContext context)
        {
            try
            {
                CheckCounterId(context.WUCounterId);
                WUIO = GetMTProcessor();
                Data.CommitRequest commitRequest = GetCommiRequest(context.CustomerSessionId, transactionId);

                if (string.IsNullOrEmpty(context.ReferenceNumber))
                {
                    context.ReferenceNumber = commitRequest.ReferenceNo;
                }

                if (commitRequest.TranascationType == ((int)MoneyTransferType.Receive).ToString())
                {

                    if (context.RMTrxType == (Helper.RequestType.Release).ToString())
                        context.RMTrxType = SendMoneyStore.mt_requested_status.RELEASE.ToString();
                    else if (context.RMTrxType == (Helper.RequestType.Cancel).ToString())
                        context.RMTrxType = SendMoneyStore.mt_requested_status.CANCEL.ToString();

                    ReceiveMoneyPay.receivemoneypayrequest receivemoneypayrequest = PopulateReceiveMoneyPayRequest(commitRequest, context);

                    ReceiveMoneyPay.receivemoneypayreply reply = WUIO.ReceiveMoneyPay(receivemoneypayrequest, context);

                    WUTransaction transaction = new WUTransaction();
                    transaction.MTCN = reply.mtcn;
                    transaction.PaidDateTime = reply.paid_date_time;
                    transaction.MessageArea = string.Concat(reply.host_message_set1, reply.host_message_set2, reply.host_message_set3);
                    transaction.DTServerLastModified = DateTime.Now;
                    transaction.DTTerminalLastModified = GetTimeZoneTime(context.TimeZone);
                    transaction.TranascationType = MoneyTransferType.Receive.ToString();
                    transaction.Id = transactionId;

                    UpdateWUTransaction(transaction, UpdateTxType.SendMoneyStore, 0);

                    return true;
                }
                else if (commitRequest.TranascationType == ((int)MoneyTransferType.Send).ToString())
                {

                    bool hasLPMTError;
                    SendMoneyStore.sendmoneystorerequest sendMoneyStoreRequest = PopulateSendMoneyStoreRequest(commitRequest, context);

                    SendMoneyStore.sendmoneystorereply sendMoneyStoreReply = WUIO.SendMoneyStore(sendMoneyStoreRequest, context, out hasLPMTError);

                    if (hasLPMTError)
                    {
                        return true;
                    }

                    string totalPointsEarned = string.Empty;

                    if (!string.IsNullOrWhiteSpace(commitRequest.GCNumber))
                    {
                        totalPointsEarned = GetCardPoints(commitRequest.GCNumber, context);
                    }

                    UpdateTrx(transactionId, sendMoneyStoreReply, totalPointsEarned, context);
                }
                return false;
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(context.SMTrxType) && context.SMTrxType.ToLower() == (Helper.RequestType.Cancel).ToString().ToLower())
                {
                    if (ex.Message.ToUpper().Trim().Equals("U9767 - MT NOT FOUND OR ALREADY BEEN CANCELLED"))
                        return false;
                }
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_COMMIT_FAILED, ex);
            }
        }

        public List<MasterData> GetBannerMsgs(ZeoContext context)
        {
            try
            {
                WUCommonIO = GetCommonProcessor();
                CheckCounterId(context.WUCounterId);
                List<MoneyTransfer.Data.MasterData> BannerMsgs = new List<MoneyTransfer.Data.MasterData>();
                List<WUCommonData.AgentBanners> response = new List<WUCommonData.AgentBanners>();

                response = WUCommonIO.GetWUAgentBannerMsgs(context);

                BannerMsgs = response.Select(i => new MoneyTransfer.Data.MasterData() { Code = i.ERR_CODE.ToString(), Name = i.ERR_MESSAGE }).ToList();
                return BannerMsgs;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETBANNERMSGS_FAILED, ex);
            }
        }

        /// <summary>
        /// This method is used for WU card enrollment.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public CardDetails WUCardEnrollment(ZeoContext context)
        {
            try
            {
                WUCommonIO = GetCommonProcessor();
                WUCommonData.CardDetails carddetails = new WUCommonData.CardDetails();
                WUCommonData.WUEnrollmentRequest enrollmentReq = new WUCommonData.WUEnrollmentRequest();
                CheckCounterId(context.WUCounterId);
                enrollmentReq = GetEnrollmentRequest(context.CustomerSessionId);

                if (enrollmentReq != null)
                {
                    string countryCode = enrollmentReq.originating_country_currency.country_code;

                    enrollmentReq.originating_country_currency = new WUCommonData.CountryCurrencyInfo()
                    {
                        //Get the currency code based on country code. Since CurrencyCode uses the "RegionInfo" class, 
                        //could not move this logic to database.
                        currency_code = GetCurrencyCode(countryCode)
                    };
                }

                carddetails = WUCommonIO.WUCardEnrollment(enrollmentReq, context);
                CardDetails cxncarddDetails = Mapper.Map<WUCommonData.CardDetails, CardDetails>(carddetails);

                return cxncarddDetails;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_WUCARDENROLLMENT_FAILED, ex);
            }
        }

        public CardLookupDetails WUCardLookup(CardLookupDetails lookupDetails, ZeoContext context)
        {
            try
            {
                WUCommonIO = GetCommonProcessor();
                CheckCounterId(context.WUCounterId);
                WUCommonData.CardLookupDetails cardlookupdetails = new WUCommonData.CardLookupDetails();

                WUCommonData.CardLookUpRequest cxncardlookupreq = new WUCommonData.CardLookUpRequest()
                {
                    ForiegnSystemId = lookupDetails.ForiegnSystemId,
                    ForiegnRefNum = lookupDetails.ForiegnRefNum,
                    CounterId = lookupDetails.CounterId,
                    AccountNumber = lookupDetails.AccountNumber,
                    Firstname = lookupDetails.FirstName,
                    LastName = lookupDetails.LastName,
                    MiddleName = lookupDetails.MiddleName,
                    PostalCode = lookupDetails.CountryCode,
                    CurrencyCode = lookupDetails.CurrencyCode,
                    LevelCode = lookupDetails.LevelCode,
                    sender = new WUCommonData.Sender()
                    {
                        AddressAddrLine1 = lookupDetails.AddressAddrLine1,
                        AddressCity = lookupDetails.AddressCity,
                        AddressPostalCode = lookupDetails.AddressPostalCode,
                        AddressState = lookupDetails.AddressState,
                        ContactPhone = lookupDetails.ContactPhone,
                        PreferredCustomerAccountNumber = lookupDetails.AccountNumber //senderinfo.PreferredCustomerAccountNumber,
                    }
                };
                cardlookupdetails = WUCommonIO.WUCardLookup(cxncardlookupreq, context);
                CardLookupDetails cxncardlookupDetails = new CardLookupDetails();

                int cnt = 0;
                cxncardlookupDetails.Sender = new Account[cardlookupdetails.Sender.Count()];
                foreach (WUCommonData.Sender sender in cardlookupdetails.Sender)
                {
                    cxncardlookupDetails.Sender[cnt] = new Account()
                    {
                        Address = sender.AddressAddrLine1,
                        FirstName = sender.FirstName,
                        LastName = sender.LastName,
                        LoyaltyCardNumber = sender.PreferredCustomerAccountNumber,
                        MobilePhone = sender.MobilePhone,
                        PostalCode = sender.AddressPostalCode
                    };
                    cnt++;
                }

                return cxncardlookupDetails;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_WUCARDLOOKUP_FAILED, ex);
            }
        }

        public bool GetPastReceivers(long customerSessionId, string cardNumber, ZeoContext context)
        {
            WUCommonIO = GetCommonProcessor();
            List<WUCommonData.Receiver> cardReceivers = new List<WUCommonData.Receiver>();

            try
            {
                CheckCounterId(context.WUCounterId);
                WUCommonData.CardLookUpRequest cxncardlookupreq = new WUCommonData.CardLookUpRequest()
                {
                    sender = new WUCommonData.Sender()
                    {
                        PreferredCustomerAccountNumber = cardNumber
                    }
                };

                cardReceivers = WUCommonIO.WUPastBillersReceivers(cxncardlookupreq, context);
                if (cardReceivers != null)
                {
                    cardReceivers = cardReceivers.FindAll(r => r.NameType == WUCommonData.WUEnums.name_type.D || r.NameType == WUCommonData.WUEnums.name_type.M);

                    //Filter the receivers who does not have FirstName, LastName, SecondLastName, Address, CountryCode etc.
                    cardReceivers = cardReceivers.Where(r => (r.NameType == WUCommonData.WUEnums.name_type.D || r.NameType == WUCommonData.WUEnums.name_type.M)
                             && !string.IsNullOrWhiteSpace(r.FirstName) && !string.IsNullOrWhiteSpace(r.LastName)
                             && r.Address != null && r.Address.item != null && !string.IsNullOrWhiteSpace(r.Address.item.country_code)).ToList();

                    List<WUData.ImportReceiver> importReceivers = new List<WUData.ImportReceiver>();

                    foreach (var item in cardReceivers)
                    {
                        WUData.ImportReceiver importReceiver = new WUData.ImportReceiver();
                        importReceiver.FirstName = item.FirstName;
                        importReceiver.LastName = item.LastName;
                        importReceiver.SecondLastName = item.SecondLastName;
                        importReceiver.PickupCountry = item.Address.item.country_code;
                        importReceiver.CountryCode = item.Address.item.country_code;
                        importReceiver.ReceiverIndexNumber = item.ReceiverIndexNumber;

                        if (importReceivers.Count() > 0)
                        {
                            if (!string.IsNullOrWhiteSpace(importReceiver.SecondLastName))
                            {
                                if (importReceivers.Where(x => x.FirstName == importReceiver.FirstName && x.LastName == importReceiver.LastName && x.SecondLastName == null && x.PickupCountry == importReceiver.PickupCountry).Count() > 0)
                                {
                                    importReceivers.RemoveAll(x => x.FirstName == importReceiver.FirstName && x.LastName == importReceiver.LastName && x.SecondLastName == null && x.PickupCountry == importReceiver.PickupCountry);
                                    importReceivers.Add(importReceiver);
                                }
                                else if (importReceivers.Where(x => x.FirstName == importReceiver.FirstName && x.LastName == importReceiver.LastName && x.SecondLastName == importReceiver.SecondLastName && x.PickupCountry == importReceiver.PickupCountry).Count() == 0)
                                    importReceivers.Add(importReceiver);
                            }
                            else
                            {
                                if (importReceivers.Where(x => x.FirstName == importReceiver.FirstName && x.LastName == importReceiver.LastName && x.PickupCountry == importReceiver.PickupCountry).Count() == 0)
                                    importReceivers.Add(importReceiver);
                            }
                        }
                        else
                        {
                            importReceivers.Add(importReceiver);
                        }
                    }

                    //List<WUData.ImportReceiver> importReceivers = Mapper.Map<List<WUCommonData.Receiver>, List<WUData.ImportReceiver>>(cardReceivers);

                    //Update the CustomerId and GoldCardNumber for each Receiver as it will have the same value. 
                    importReceivers = importReceivers.Select(c =>
                    {
                        c.CustomerId = context.CustomerId;
                        c.Status = "Active"; c.GoldCardNumber = cardNumber; return c;
                    }).ToList();

                    ImportReceivers(importReceivers, context);
                }

                return cardReceivers != null;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETPASTRECEIVERS_FAILED, ex);
            }
        }

        public bool UseGoldcard(string WUGoldCardNumber, ZeoContext context)
        {
            try
            {
                WUCommonIO = GetCommonProcessor();
                CheckCounterId(context.WUCounterId);
                WUCommonData.CardLookupDetails cardlookupdetails = new WUCommonData.CardLookupDetails();
                WUCommonData.CardLookUpRequest cxncardlookupreq = new WUCommonData.CardLookUpRequest()
                {
                    sender = new WUCommonData.Sender()
                    {
                        PreferredCustomerAccountNumber = WUGoldCardNumber,
                    }
                };
                cardlookupdetails = WUCommonIO.WUCardLookupForCardNumber(cxncardlookupreq, context);
                CardLookupDetails cxncardlookupDetails = new CardLookupDetails();

                cxncardlookupDetails.Sender = new Account[1];

                cxncardlookupDetails.Sender[0] = new Account()
                {
                    Address = cardlookupdetails.Sender[0].AddressAddrLine1,
                    FirstName = cardlookupdetails.Sender[0].FirstName,
                    LastName = cardlookupdetails.Sender[0].LastName,
                    LoyaltyCardNumber = cardlookupdetails.Sender[0].PreferredCustomerAccountNumber,
                    MobilePhone = cardlookupdetails.Sender[0].MobilePhone,
                    PostalCode = cardlookupdetails.Sender[0].AddressPostalCode
                };

                //TODO - Abhi - Check Do we need to update the customer details in tCustomer table or we need to skip this.
                WUAccount wuAccount = GetWUAccount(context.CustomerSessionId);
                if (wuAccount != null)
                {
                    if (!(cxncardlookupDetails.Sender[0].FirstName.Trim().ToLower().Equals(wuAccount.FirstName.Trim().ToLower())
                        && cxncardlookupDetails.Sender[0].LastName.Trim().ToLower().Equals(wuAccount.LastName.Trim().ToLower())))
                        throw new WUCommonData.WUCommonException(AlloyUtil.GetProductCode(context.ProductType), MoneyTransferException.CUSTOMER_NAME_NOT_MATCH, null);
                }
                Account account = new Account()
                {
                    LoyaltyCardNumber = WUGoldCardNumber
                    ,
                    DTServerLastModified = DateTime.Now
                    ,
                    DTTerminalLastModified = GetTimeZoneTime(context.TimeZone)
                };

                UpdateAccountWithCardNumber(context.CustomerSessionId, account);

                return true;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_USEGOLDCARD_FAILED, ex);
            }
        }

        public string GetCurrencyCode(string countryCode)
        {
            try
            {
                List<Data.WUCountryCurrency> currencies = new List<Data.WUCountryCurrency>();

                StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_GetWUnionCountryCurrencies");

                moneyTransferProcedure.WithParameters(InputParameter.Named("countryCode").WithValue(countryCode));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(moneyTransferProcedure))
                {
                    while (datareader.Read())
                    {
                        currencies.Add(new Data.WUCountryCurrency()
                        {
                            CountryCode = datareader.GetStringOrDefault("CountryCode"),
                            CountryName = datareader.GetStringOrDefault("CountryName"),
                            CountryNumCode = datareader.GetStringOrDefault("CountryNumCode"),
                            CurrencyCode = datareader.GetStringOrDefault("CurrencyCode"),
                            CurrencyName = datareader.GetStringOrDefault("CurrencyName"),
                            CurrencyNumCode = datareader.GetStringOrDefault("CurrencyNumCode")
                        });
                    }
                }

                if (currencies.Count > 1)
                {
                    try
                    {
                        RegionInfo myRI1 = new RegionInfo(countryCode);
                        var isoCurrencyList = currencies.Where(c => c.CurrencyCode == myRI1.ISOCurrencySymbol).ToList();
                        currencies = isoCurrencyList.Count > 0 ? isoCurrencyList : currencies;
                    }
                    catch (Exception)
                    { }
                }

                var countryCurrency = currencies.FirstOrDefault();

                if (countryCurrency == null)
                    return string.Empty;

                return countryCurrency.CurrencyCode;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETCURRENCYCODE_FAILED, ex);
            }
        }

        public List<MasterData> GetCurrencyCodeList(string countryCode)
        {
            try
            {
                List<MasterData> currencies = new List<MasterData>(); ;

                StoredProcedure customerProcedure = new StoredProcedure("usp_GetWUnionCountryCurrencies");

                customerProcedure.WithParameters(InputParameter.Named("countryCode").WithValue(countryCode));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(customerProcedure))
                {
                    while (datareader.Read())
                    {
                        currencies.Add(new MasterData()
                        {
                            Id = datareader.GetInt64OrDefault("Id"),
                            Code = datareader.GetStringOrDefault("CurrencyCode"),
                            Name = datareader.GetStringOrDefault("CurrencyName")
                        });
                    }
                }

                return currencies;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETCURRENCYCODELIST_FAILED, ex);
            }
        }

        public bool DeleteFavoriteReceiver(Receiver receiver)
        {
            try
            {
                StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_DeleteReceiver");

                moneyTransferProcedure.WithParameters(InputParameter.Named("receiverId").WithValue(receiver.Id));
                moneyTransferProcedure.WithParameters(InputParameter.Named("status").WithValue(receiver.Status));
                moneyTransferProcedure.WithParameters(InputParameter.Named("dtServerLastModified").WithValue(receiver.DTServerLastModified));
                moneyTransferProcedure.WithParameters(InputParameter.Named("dtTerminalLastModified").WithValue(receiver.DTTerminalLastModified));

                IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(moneyTransferProcedure);

                return true;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_DELETEFAVORITERECEIVER_FAILED, ex);
            }
        }

        public bool IsSendMoneyModifyRefundAvailable(long transactionId, long customerId)
        {
            try
            {
                bool isAvailable = false;

                StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_IsSendMoneyModifiedOrRefunded");

                moneyTransferProcedure.WithParameters(InputParameter.Named("customerId").WithValue(customerId));
                moneyTransferProcedure.WithParameters(InputParameter.Named("mtTransactionId").WithValue(transactionId));

                using (IDataReader reader = DataConnectionHelper.GetConnectionManager().ExecuteReader(moneyTransferProcedure))
                {
                    if (reader.Read())
                    {
                        isAvailable = reader.GetBooleanOrDefault("isAvailable");
                    }
                }

                return isAvailable;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_ISSENDMONEYMODIFYREFUNDAVAILABLE_FAILED, ex);
            }
        }

        private static void ValidateDeliveryServices(DeliveryServiceRequest request, ZeoContext context)
        {
            if (string.IsNullOrWhiteSpace(request.CountryCode) || string.IsNullOrWhiteSpace(request.CountryCurrency))
            {
                throw new MoneyTransferException(MoneyTransferException.DESTINATION_COUNTRY_CODE_NOT_FOUND);
            }

            if (!request.MetaData.ContainsKey("State"))
            {
                throw new MoneyTransferException(MoneyTransferException.DESTINATION_STATE_NOT_FOUND);
            }

            if (!request.MetaData.ContainsKey("StateCode"))
            {
                throw new MoneyTransferException(MoneyTransferException.DESTINATION_STATE_CODE_NOT_FOUND);
            }

            if (!request.MetaData.ContainsKey("City"))
            {
                throw new MoneyTransferException(MoneyTransferException.DESTINATION_CITY_NOT_FOUND);
            }

            if (request.Type == DeliveryServiceType.Option)
            {
                if (!request.MetaData.ContainsKey("DeliveryService"))
                {
                    throw new MoneyTransferException(MoneyTransferException.DELIVERYSERVICE_NOT_FOUND);
                }
            }
        }

        private FeeInquiry.feeinquiryrequest BuildFeeEnquiryRequest(FeeRequest feeRequest)
        {

            FeeInquiry.iso_code isoCode = new FeeInquiry.iso_code() { country_code = CountryCode, currency_code = CountryCurrencyCode };

            FeeInquiry.iso_code destinationIsoCode = new FeeInquiry.iso_code()
            {
                country_code = feeRequest.ReceiveCountryCode,
                currency_code = feeRequest.ReceiveCountryCurrency
            };

            FeeInquiry.feeinquiryrequest feeInquiryRequest = new FeeInquiry.feeinquiryrequest
            {
                financials = new FeeInquiry.financials()
                {
                    originators_principal_amount = ConvertDecimalToLong(feeRequest.Amount),
                    originators_principal_amountSpecified = feeRequest.Amount > 0,
                    destination_principal_amount = ConvertDecimalToLong(feeRequest.ReceiveAmount),
                    destination_principal_amountSpecified = feeRequest.ReceiveAmount > 0
                },
                payment_details = new FeeInquiry.payment_details()
                {
                    originating_country_currency = new FeeInquiry.country_currency_info() { iso_code = isoCode },
                    destination_country_currency = new FeeInquiry.country_currency_info() { iso_code = destinationIsoCode },
                    recording_country_currency = new FeeInquiry.country_currency_info() { iso_code = isoCode },
                    transaction_type = feeRequest.Amount > 0 ? FeeInquiry.transaction_type.WMN : FeeInquiry.transaction_type.WMF,
                    transaction_typeSpecified = true,
                    payment_type = FeeInquiry.payment_type.Cash,
                    payment_typeSpecified = true,
                    fix_on_send = feeRequest.Amount > 0 ? FeeInquiry.yes_no.Y : FeeInquiry.yes_no.N
                },
                delivery_services = new FeeInquiry.delivery_services()
                {
                    code = feeRequest.DeliveryServiceCode != null ? feeRequest.DeliveryServiceCode : string.Empty
                },
                promotions = new FeeInquiry.promotions()
                {
                    coupons_promotions = !string.IsNullOrWhiteSpace(feeRequest.PromoCode) ? feeRequest.PromoCode : string.Empty
                }
            };

            if (!string.IsNullOrWhiteSpace(feeRequest.ReceiverSecondLastName))
            {
                feeInquiryRequest.receiver = new FeeInquiry.receiver()
                {
                    name = new FeeInquiry.general_name()
                    {
                        given_name = string.IsNullOrWhiteSpace(feeRequest.ReceiverFirstName)
                                    ? string.Empty
                                    : MassagingValue(feeRequest.ReceiverFirstName),
                        paternal_name = string.IsNullOrWhiteSpace(feeRequest.ReceiverLastName)
                                    ? string.Empty
                                    : MassagingValue(feeRequest.ReceiverLastName),
                        maternal_name = string.IsNullOrWhiteSpace(feeRequest.ReceiverSecondLastName)
                                    ? string.Empty
                                    : MassagingValue(feeRequest.ReceiverSecondLastName),
                        name_type = FeeInquiry.name_type.M,
                        name_typeSpecified = true

                    }
                };
            }
            else
            {
                feeInquiryRequest.receiver = new FeeInquiry.receiver()
                {
                    name = new FeeInquiry.general_name()
                    {
                        first_name = string.IsNullOrWhiteSpace(feeRequest.ReceiverFirstName)
                                            ? string.Empty
                                            : MassagingValue(feeRequest.ReceiverFirstName),
                        last_name = string.IsNullOrWhiteSpace(feeRequest.ReceiverLastName)
                                            ? string.Empty
                                            : MassagingValue(feeRequest.ReceiverLastName),
                        middle_name = string.IsNullOrWhiteSpace(feeRequest.ReceiverMiddleName)
                                            ? string.Empty
                                            : MassagingValue(feeRequest.ReceiverMiddleName),
                        name_type = FeeInquiry.name_type.D,
                        name_typeSpecified = true
                    }
                };
            }
            feeInquiryRequest.preferred_customer_no = feeRequest.PreferredCustomerAccountNumber;

            string[] personalMessages = null;

            if (!string.IsNullOrWhiteSpace(feeRequest.PersonalMessage))
            {
                personalMessages = MessageBlockSplit(feeRequest.PersonalMessage).ToArray();
            }

            if (personalMessages != null)
            {

                FeeInquiry.messages messages = new FeeInquiry.messages();
                messages.text = personalMessages;

                feeInquiryRequest.delivery_services.message = new FeeInquiry.message_details()
                {
                    message_details1 = messages
                };
            }

            return feeInquiryRequest;
        }

        private long CreateTrx(FeeInquiry.feeinquiryrequest feeInquiryRequest, FeeRequest feeRequest, ZeoContext context)
        {
            long transactionId = 0;
            WUTransaction transaction = new WUTransaction()
            {
                DestinationPrincipalAmount = ConvertLongToDecimal(feeInquiryRequest.financials.destination_principal_amount),
                OriginatorsPrincipalAmount = ConvertLongToDecimal(feeInquiryRequest.financials.originators_principal_amount),

                DestinationCountryCode = feeInquiryRequest.payment_details.destination_country_currency.iso_code.country_code,
                DestinationCurrencyCode = feeInquiryRequest.payment_details.destination_country_currency.iso_code.currency_code,

                OriginatingCountryCode = feeInquiryRequest.payment_details.originating_country_currency.iso_code.country_code,
                OriginatingCurrencyCode = feeInquiryRequest.payment_details.originating_country_currency.iso_code.currency_code,

                recordingCountryCode = feeInquiryRequest.payment_details.originating_country_currency.iso_code.country_code,
                recordingCurrencyCode = feeInquiryRequest.payment_details.originating_country_currency.iso_code.currency_code,
                IsFixedOnSend = feeInquiryRequest.payment_details.fix_on_send == FeeInquiry.yes_no.Y,

                DeliveryOption = feeInquiryRequest.delivery_services.code,
                PersonalMessage = feeRequest.PersonalMessage,
                PromotionsCode = feeInquiryRequest.promotions.coupons_promotions,
                GCNumber = feeInquiryRequest.preferred_customer_no,
                IsDomesticTransfer = feeRequest.IsDomesticTransfer,
                ReferenceNo = feeRequest.ReferenceNo
            };

            if (!string.IsNullOrWhiteSpace(feeRequest.ReceiverSecondLastName))
            {
                transaction.ReceiverFirstName = feeInquiryRequest.receiver.name.given_name;
                transaction.ReceiverLastName = feeInquiryRequest.receiver.name.paternal_name;
                transaction.ReceiverSecondLastName = feeRequest.ReceiverSecondLastName;
            }
            else
            {
                transaction.ReceiverFirstName = feeInquiryRequest.receiver.name.first_name;
                transaction.ReceiverLastName = feeInquiryRequest.receiver.name.last_name;
            }

            transaction.DTServerCreate = DateTime.Now;
            transaction.DTTerminalCreate = GetTimeZoneTime(context.TimeZone);
            transaction.WUAccountId = feeRequest.AccountId;
            transaction.ReceiverId = feeRequest.ReceiverId;
            transaction.TranascationType = Convert.ToString((int)MoneyTransferType.Send);
            transaction.ChannelPartnerId = context.ChannelPartnerId;
            transaction.ProviderId = context.ProviderId;
            transaction.Id = feeRequest.TransactionId;
            transactionId = CreateWUTransaction(transaction);

            return transactionId;
        }

        private void UpdateTrx(FeeRequest feeRequest, FeeInquiry.feeinquiryrequest feeInquiryRequest, ZeoContext context)
        {
            WUTransaction transaction = new WUTransaction();

            transaction.ReceiverFirstName = feeInquiryRequest.receiver.name.first_name;
            transaction.ReceiverLastName = feeInquiryRequest.receiver.name.last_name;

            transaction.DestinationPrincipalAmount = ConvertLongToDecimal(feeInquiryRequest.financials.destination_principal_amount);
            transaction.OriginatorsPrincipalAmount = ConvertLongToDecimal(feeInquiryRequest.financials.originators_principal_amount);

            transaction.DestinationCountryCode = feeInquiryRequest.payment_details.destination_country_currency.iso_code.country_code;
            transaction.DestinationCurrencyCode = feeInquiryRequest.payment_details.destination_country_currency.iso_code.currency_code;

            transaction.recordingCountryCode = feeInquiryRequest.payment_details.originating_country_currency.iso_code.country_code;
            transaction.recordingCurrencyCode = feeInquiryRequest.payment_details.originating_country_currency.iso_code.currency_code;
            transaction.IsFixedOnSend = feeInquiryRequest.payment_details.fix_on_send == FeeInquiry.yes_no.Y;
            transaction.DeliveryOption = feeInquiryRequest.delivery_services.code;

            transaction.PersonalMessage = feeRequest.PersonalMessage;
            transaction.PromotionsCode = feeInquiryRequest.promotions.coupons_promotions;
            transaction.GCNumber = feeInquiryRequest.preferred_customer_no;

            transaction.DTTerminalLastModified = GetTimeZoneTime(context.TimeZone);
            transaction.DTServerLastModified = DateTime.Now;
            transaction.GrossTotalAmount = transaction.OriginatorsPrincipalAmount + transaction.Charges + transaction.plus_charges_amount + transaction.message_charge + transaction.OtherCharges;
            transaction.TaxAmount = transaction.county_tax + transaction.municipal_tax + transaction.state_tax;
            transaction.TranascationType = "Send";
            transaction.Id = feeRequest.TransactionId;

            UpdateWUTransaction(transaction, UpdateTxType.FeeRequest, 0);
        }

        private FeeResponse MapfeeEnquiryResponse(FeeInquiry.feeinquiryreply feeInqResponse)
        {
            var feeInformations = new List<FeeInformation>
            {
                new FeeInformation()
                {
                    Amount = ConvertLongToDecimal(feeInqResponse.financials.originators_principal_amount),
                    ReceiveAmount = ConvertLongToDecimal(feeInqResponse.financials.destination_principal_amount),
                    TotalAmount = ConvertLongToDecimal(feeInqResponse.financials.gross_total_amount),
                    Fee = ConvertLongToDecimal(feeInqResponse.financials.charges),
                    MetaData = new Dictionary<string, object>()
                    {
                        {"PlusCharges", ConvertLongToDecimal(feeInqResponse.financials.plus_charges_amount)},
                        {"PayAmount", ConvertLongToDecimal(feeInqResponse.financials.pay_amount)},
                        {"Tolls", ConvertLongToDecimal(feeInqResponse.financials.tolls)},
                        {"CanadianDollarExchangeFee", ConvertLongToDecimal(feeInqResponse.financials.canadian_dollar_exchange_fee)},
                        {"BaseMessageCharge", feeInqResponse.fee_inquiry_message.base_message_charge},
                        {"TotalDiscount", ConvertLongToDecimal(feeInqResponse.financials.total_discount)},
                        {"MunicipalTax", ConvertLongToDecimal(feeInqResponse.financials.taxes.municipal_tax)},
                        {"StateTax", ConvertLongToDecimal(feeInqResponse.financials.taxes.state_tax)},
                        {"CountyTax", ConvertLongToDecimal(feeInqResponse.financials.taxes.county_tax)},
                        {"TransferTax", ConvertLongToDecimal(feeInqResponse.financials.taxes.municipal_tax)+ConvertLongToDecimal(feeInqResponse.financials.taxes.state_tax)+ConvertLongToDecimal(feeInqResponse.financials.taxes.county_tax)}
                    },
                    ExchangeRate = Convert.ToDecimal(feeInqResponse.payment_details.exchange_rate),
                    MessageFee = ConvertLongToDecimal(feeInqResponse.financials.message_charge),
                    Discount = ConvertLongToDecimal(feeInqResponse.promotions.promo_discount_amount),
                }
            };

            var feeResponse = new FeeResponse()
            {
                FeeInformations = feeInformations,
                MetaData = new Dictionary<string, object>()
                {
                    {"TestQuestionOption", feeInqResponse.delivery_services != null ? feeInqResponse.delivery_services.test_question_available : string.Empty},
                    {"IsFixedOnSend", feeInqResponse.payment_details.fix_on_send == FeeInquiry.yes_no.Y}
                }
            };

            return feeResponse;
        }

        private void UpdateTrx(long wuTransactionId, FeeRequest feeRequest, FeeResponse feeResponse, ZeoContext context)
        {
            WUTransaction transaction = new WUTransaction();

            FeeInformation feeInformation = feeResponse.FeeInformations.FirstOrDefault();
            if (feeInformation != null)
            {
                transaction.GrossTotalAmount = feeInformation.TotalAmount;
                transaction.DestinationPrincipalAmount = feeInformation.ReceiveAmount;
                transaction.OriginatorsPrincipalAmount = feeInformation.Amount;
                transaction.message_charge = feeInformation.MessageFee;
                transaction.Charges = feeInformation.Fee;
                transaction.ExchangeRate = feeInformation.ExchangeRate;
                transaction.plus_charges_amount = Convert.ToDecimal(feeInformation.MetaData["PlusCharges"]);
                transaction.municipal_tax = Convert.ToDecimal(feeInformation.MetaData["MunicipalTax"]);
                transaction.state_tax = Convert.ToDecimal(feeInformation.MetaData["StateTax"]);
                transaction.county_tax = Convert.ToDecimal(feeInformation.MetaData["CountyTax"]);
                transaction.TaxAmount = Convert.ToDecimal(feeInformation.MetaData["TransferTax"]);
            }
            transaction.DestinationCurrencyCode = feeRequest.ReceiveCountryCurrency;
            transaction.DestinationState = Convert.ToString(feeRequest.MetaData["StateName"]);
            transaction.DTTerminalLastModified = GetTimeZoneTime(context.TimeZone);
            transaction.DTServerLastModified = DateTime.Now;
            transaction.Id = wuTransactionId;
            transaction.TranascationType = "Send";
            transaction.PromotionsCode = feeRequest.PromoCode;

            //transaction.GrossTotalAmount = transaction.OriginatorsPrincipalAmount + transaction.Charges
            //	+ transaction.plus_charges_amount + transaction.message_charge + transaction.OtherCharges;
            // This is already calculated and comes from WU in gross_total_amount property of fee-inquiry-reply 
            //transaction.GrossTotalAmount = Convert.ToDecimal(feeInformation.TotalAmount);
            transaction.TestQuestionAvaliable = Convert.ToString(feeResponse.MetaData["TestQuestionOption"]);

            decimal fee = transaction.Charges + transaction.AdditionalCharges + transaction.municipal_tax + transaction.state_tax + transaction.county_tax;

            UpdateWUTransaction(transaction, UpdateTxType.FeeRequest, fee);

            return;
        }

        private IEnumerable<string> MessageBlockSplit(string strMessage)
        {
            var length = 69;
            for (int i = 0; i < strMessage.Length; i += length)
            {
                yield return strMessage.Substring(i, Math.Min((strMessage.Length - i), Math.Min(length, strMessage.Length)));
            }
        }

        private receivemoneypayrequest PopulateReceiveMoneyPayRequest(long customerSessionId, ValidateRequest validateRequest, ZeoContext context)
        {
            WUCommonIO = GetCommonProcessor();

            string issueCountry = validateRequest.PrimaryCountryCodeOfIssue;
            if (validateRequest.PrimaryIdType != null)
                if (validateRequest.PrimaryIdType.Equals("PASSPORT")
                    || validateRequest.PrimaryIdType.Equals("EMPLOYMENT AUTHORIZATION CARD (EAD)")
                    || validateRequest.PrimaryIdType.Equals("PERMANENT RESIDENT CARD")
                    || validateRequest.PrimaryIdType.Equals("MILITARY ID"))
                {
                    issueCountry = validateRequest.PrimaryIdCountryOfIssue;
                }

            var countryCurrencyInfo = new ReceiveMoneyPay.country_currency_info()
            {
                iso_code = new ReceiveMoneyPay.iso_code()
                {
                    country_code = CountryCode,
                    currency_code = CountryCurrencyCode
                },
                country_name = CountryName
            };

            var receiveMoneyPayRequest = new receivemoneypayrequest
            {
                mtcn = validateRequest.MTCN,
                new_mtcn = validateRequest.TempMTCN,
                money_transfer_key = validateRequest.MoneyTransferKey,
                receiver = new ReceiveMoneyPay.receiver
                {
                    name = new ReceiveMoneyPay.general_name()
                    {
                        first_name = validateRequest.FirstName,
                        last_name = validateRequest.LastName,
                        name_type = ReceiveMoneyPay.name_type.D,
                        name_typeSpecified = true
                    },
                    address = new ReceiveMoneyPay.address
                    {
                        addr_line1 = validateRequest.Address,
                        city = validateRequest.City,
                        state = validateRequest.State,
                        postal_code = validateRequest.PostalCode,
                        Item = countryCurrencyInfo
                    },
                    compliance_details = new ReceiveMoneyPay.compliance_details()
                    {
                        template_id = WUCommonData.ComplianceTemplate.RECEIVE_MONEY,
                        id_details = new ReceiveMoneyPay.id_details()
                        {
                            id_type = !string.IsNullOrEmpty(validateRequest.PrimaryIdType) ? WUCommonIO.GetGovtIDType(validateRequest.PrimaryIdType) : string.Empty,
                            id_number = validateRequest.PrimaryIdNumber,
                            id_country_of_issue = issueCountry,
                            id_place_of_issue = !string.IsNullOrEmpty(validateRequest.PrimaryIdPlaceOfIssue) ? validateRequest.PrimaryIdPlaceOfIssue : string.Empty
                        },
                        Current_address = new ReceiveMoneyPay.compliance_address()
                        {
                            addr_line1 = validateRequest.Address,
                            city = validateRequest.City,
                            state_code = validateRequest.State,
                            postal_code = validateRequest.PostalCode,
                            country = CountryCode
                        },
                        date_of_birth = validateRequest.DateOfBirth,
                        occupation = AlloyUtil.TrimString(MassagingValue(validateRequest.Occupation), 29),
                        contact_phone = validateRequest.ContactPhone,
                        Country_of_Birth = validateRequest.CountryOfBirthAbbr2,
                        ack_flag = "X",
                        third_party_details = new ReceiveMoneyPay.third_party_details()
                        {
                            flag_pay = ReceiveMoneyPay.flag_pay.N,
                            flag_paySpecified = true
                        }
                    }
                },
                delivery_services = new ReceiveMoneyPay.delivery_services()
                {
                    identification_question = new ReceiveMoneyPay.identification_question()
                    {
                        question = validateRequest.TestQuestion,
                        answer = validateRequest.TestAnswer
                    }
                },
                financials = new ReceiveMoneyPay.financials()
                {
                    gross_total_amount = ConvertDecimalToLong(validateRequest.GrossTotalAmount),
                    gross_total_amountSpecified = true,
                    pay_amount = ConvertDecimalToLong(validateRequest.AmountToReceiver),
                    pay_amountSpecified = true,
                    principal_amount = ConvertDecimalToLong(validateRequest.DestinationPrincipalAmount),
                    principal_amountSpecified = true,
                    charges = ConvertDecimalToLong(validateRequest.Charges),
                    chargesSpecified = true
                },
                payment_details = new ReceiveMoneyPay.payment_details
                {
                    destination_country_currency = new ReceiveMoneyPay.country_currency_info()
                    {
                        iso_code = new ReceiveMoneyPay.iso_code()
                        {
                            country_code = validateRequest.DestinationCountryCode,
                            currency_code = validateRequest.DestinationCurrencyCode
                        }
                    },
                    originating_country_currency = countryCurrencyInfo,
                    original_destination_country_currency = countryCurrencyInfo,
                    expected_payout_location = new ReceiveMoneyPay.expected_payout_location()
                    {
                        state_code = validateRequest.ExpectedPayoutStateCode,
                        city = validateRequest.ExpectedPayoutCityName
                    },
                    exchange_rate = Convert.ToDouble(validateRequest.ExchangeRate),
                    exchange_rateSpecified = true
                }
            };
            //Appending second_id tag if the customer has SSN/ITIN 
            if (!string.IsNullOrWhiteSpace(validateRequest.SecondIdNumber))
            {
                receiveMoneyPayRequest.receiver.compliance_details.second_id = new ReceiveMoneyPay.id_details()
                {
                    id_type = !string.IsNullOrEmpty(validateRequest.SecondIdType) ? WUCommonIO.GetGovtIDType(validateRequest.SecondIdType) : string.Empty,
                    id_number = validateRequest.SecondIdNumber,
                    id_country_of_issue = !string.IsNullOrEmpty(CountryName) ? CountryName : string.Empty
                };
            }



            if (validateRequest.PrimaryIdType != null)
                if (receiveMoneyPayRequest.receiver.compliance_details.id_details != null &&
                    receiveMoneyPayRequest.receiver.compliance_details.id_details.id_country_of_issue != null)
                {
                    if (receiveMoneyPayRequest.receiver.compliance_details.id_details.id_country_of_issue.Equals("US")
                        &&
                        (validateRequest.PrimaryIdType.Equals("DRIVER'S LICENSE") ||
                         validateRequest.PrimaryIdType.Equals("U.S. STATE IDENTITY CARD")))
                    {
                        receiveMoneyPayRequest.receiver.compliance_details.id_details.id_country_of_issue =
                          validateRequest.PrimaryCountryCodeOfIssue + "/" + validateRequest.PrimaryIdPlaceOfIssue;
                    }
                    else if (receiveMoneyPayRequest.receiver.compliance_details.id_details.id_country_of_issue.Equals("MX"))
                    {
                        receiveMoneyPayRequest.receiver.compliance_details.id_details.id_country_of_issue = "Mexico";
                    }
                }

            if (!string.IsNullOrWhiteSpace(validateRequest.originating_city))
            {
                receiveMoneyPayRequest.payment_details.originating_city = validateRequest.originating_city;
            }

            if (!string.IsNullOrEmpty(context.RMTrxType) && context.RMTrxType == ReceiveMoneyPay.mt_requested_status.HOLD.ToString())
            {
                receiveMoneyPayRequest.payment_details.mt_requested_status = ReceiveMoneyPay.mt_requested_status.HOLD;
                receiveMoneyPayRequest.pay_or_do_not_pay_indicator = pay_or_do_not_pay_indicator.P;
            }
            if (!string.IsNullOrEmpty(context.RMTrxType) && (context.RMTrxType == ReceiveMoneyPay.mt_requested_status.RELEASE.ToString()))
            {
                receiveMoneyPayRequest.payment_details.mt_requested_status = ReceiveMoneyPay.mt_requested_status.RELEASE;
                receiveMoneyPayRequest.pay_or_do_not_pay_indicator = pay_or_do_not_pay_indicator.P;
            }
            else if (!string.IsNullOrEmpty(context.RMTrxType) && context.RMTrxType == ReceiveMoneyPay.mt_requested_status.CANCEL.ToString())
            {
                receiveMoneyPayRequest.payment_details.mt_requested_status = ReceiveMoneyPay.mt_requested_status.CANCEL;
            }

            receiveMoneyPayRequest.payment_details.mt_requested_statusSpecified = true;

            receiveMoneyPayRequest.swb_fla_info = Mapper.Map<WUCommonData.SwbFlaInfo, ReceiveMoneyPay.swb_fla_info>(WUCommonIO.BuildSwbFlaInfo(context));
            receiveMoneyPayRequest.swb_fla_info.fla_name = Mapper.Map<WUCommonData.GeneralName, ReceiveMoneyPay.general_name>(WUCommonIO.BuildGeneralName(context));
            return receiveMoneyPayRequest;
        }

        private SendMoneyValidation.sendmoneyvalidationrequest MapSendMoneyValidateRequest(ValidateRequest validateRequest, long customerSessionId)
        {
            WUCommonIO = GetCommonProcessor();
            string expectedPayoutStateCode = Convert.ToString(validateRequest.MetaData["ExpectedPayoutStateCode"]);
            string expectedPayoutCity = Convert.ToString(validateRequest.MetaData["ExpectedPayoutCity"]);

            SendMoneyValidation.sendmoneyvalidationrequest request = new SendMoneyValidation.sendmoneyvalidationrequest();
            SendMoneyValidation.country_currency_info country = new SendMoneyValidation.country_currency_info()
            {
                iso_code = new SendMoneyValidation.iso_code() { country_code = CountryCode, currency_code = CountryCurrencyCode }
            };


            SendMoneyValidation.sender sender = new SendMoneyValidation.sender()
            {
                preferred_customer = new SendMoneyValidation.preferred_customer()
                {
                    account_nbr = validateRequest.PreferredCustomerAccountNumber
                },
                address = new SendMoneyValidation.address()
                {
                    addr_line1 = string.IsNullOrEmpty(validateRequest.Address) ? string.Empty : MassagingValue(validateRequest.Address),
                    city = string.IsNullOrEmpty(validateRequest.City) ? string.Empty : MassagingValue(validateRequest.City),
                    state = string.IsNullOrEmpty(validateRequest.State) ? string.Empty : MassagingValue(validateRequest.State),
                    postal_code = string.IsNullOrEmpty(validateRequest.PostalCode) ? string.Empty : MassagingValue(validateRequest.PostalCode),
                    Item = country
                },
                contact_phone = string.IsNullOrEmpty(validateRequest.ContactPhone) ? string.Empty : MassagingValue(validateRequest.ContactPhone),
                sms_notification_flag = (validateRequest.SmsNotificationFlag == WUCommonData.WUEnums.yes_no.Y.ToString()) ? SendMoneyValidation.sms_notification.Y : SendMoneyValidation.sms_notification.N,
                sms_notification_flagSpecified = (validateRequest.SmsNotificationFlag == WUCommonData.WUEnums.yes_no.Y.ToString()) ? true : false,
                fraud_warning_consent = SendMoneyValidation.fraud_warning_consent.Y,
                fraud_warning_consentSpecified = true
            };

            SendMoneyValidation.consumer_fraud_prompts prompts = new SendMoneyValidation.consumer_fraud_prompts()
            {
                question1 = validateRequest.ConsumerFraudPromptQuestion ? "Y" : "N"
            };

            SendMoneyValidation.general_name name = null;
            if (!string.IsNullOrEmpty(validateRequest.SecondLastName))
            {

                name = new SendMoneyValidation.general_name()
                {
                    given_name = string.IsNullOrWhiteSpace(validateRequest.FirstName) ? string.Empty : MassagingValue(validateRequest.FirstName),
                    paternal_name = string.IsNullOrWhiteSpace(validateRequest.LastName) ? string.Empty : MassagingValue(validateRequest.LastName),
                    maternal_name = string.IsNullOrWhiteSpace(validateRequest.SecondLastName) ? string.Empty : MassagingValue(validateRequest.SecondLastName),
                    name_type = SendMoneyValidation.name_type.M,
                    name_typeSpecified = true
                };
            }
            else
            {
                name = new SendMoneyValidation.general_name()
                {
                    first_name = string.IsNullOrEmpty(validateRequest.FirstName) ? string.Empty : MassagingValue(validateRequest.FirstName),
                    last_name = string.IsNullOrEmpty(validateRequest.LastName) ? string.Empty : MassagingValue(validateRequest.LastName),
                    middle_name = string.IsNullOrEmpty(validateRequest.MiddleName) ? string.Empty : MassagingValue(validateRequest.MiddleName),
                    name_type = SendMoneyValidation.name_type.D,
                    name_typeSpecified = true
                };
            }
            sender.name = name;
            request.sender = sender;
            request.consumer_fraud_prompts = prompts;

            SendMoneyValidation.receiver requestReciever = new SendMoneyValidation.receiver();
            SendMoneyValidation.general_name recievername = null;
            if (!string.IsNullOrWhiteSpace(validateRequest.ReceiverSecondLastName))
            {
                recievername = new SendMoneyValidation.general_name()
                {
                    given_name = string.IsNullOrEmpty(validateRequest.ReceiverFirstName) ? string.Empty : MassagingValue(validateRequest.ReceiverFirstName),
                    maternal_name = string.IsNullOrEmpty(validateRequest.ReceiverSecondLastName) ? string.Empty : MassagingValue(validateRequest.ReceiverSecondLastName),
                    paternal_name = string.IsNullOrEmpty(validateRequest.ReceiverLastName) ? string.Empty : MassagingValue(validateRequest.ReceiverLastName),
                    name_type = SendMoneyValidation.name_type.M
                };
            }
            else
            {
                recievername = new SendMoneyValidation.general_name()
                {
                    last_name = string.IsNullOrEmpty(validateRequest.ReceiverLastName) ? string.Empty : MassagingValue(validateRequest.ReceiverLastName),
                    first_name = string.IsNullOrEmpty(validateRequest.ReceiverFirstName) ? string.Empty : MassagingValue(validateRequest.ReceiverFirstName),
                    middle_name = string.IsNullOrEmpty(validateRequest.ReceiverMiddleName) ? string.Empty : MassagingValue(validateRequest.ReceiverMiddleName),
                    name_type = SendMoneyValidation.name_type.D
                };
            }
            recievername.name_typeSpecified = true;
            requestReciever.name = recievername;
            request.receiver = requestReciever;



            if (!string.IsNullOrWhiteSpace(validateRequest.PromotionsCode))
            {
                request.promotions = new SendMoneyValidation.promotions()
                {
                    coupons_promotions = validateRequest.PromotionsCode,
                };
            }

            SendMoneyValidation.payment_details paymentdetails = new SendMoneyValidation.payment_details()
            {
                expected_payout_location = new SendMoneyValidation.expected_payout_location()
                {
                    state_code = string.IsNullOrEmpty(expectedPayoutStateCode) ? string.Empty : expectedPayoutStateCode,
                    city = string.IsNullOrEmpty(expectedPayoutCity) ? string.Empty : expectedPayoutCity
                },
                recording_country_currency = country,
                originating_country_currency = country,
                destination_country_currency = new SendMoneyValidation.country_currency_info()
                {
                    iso_code = new SendMoneyValidation.iso_code()
                    {
                        country_code = validateRequest.DestinationCountryCode,
                        currency_code = validateRequest.DestinationCurrencyCode
                    }
                },
                transaction_type = SendMoneyValidation.transaction_type.WMN,
                transaction_typeSpecified = true,
                payment_type = SendMoneyValidation.payment_type.Cash,
                payment_typeSpecified = true,
                duplicate_detection_flag = AllowDuplicateTrxWU,
            };
            request.payment_details = paymentdetails;

            if (!string.IsNullOrWhiteSpace(validateRequest.DeliveryService))
            {
                request.delivery_services = new SendMoneyValidation.delivery_services()
                {
                    code = validateRequest.DeliveryService
                };

                if (!string.IsNullOrWhiteSpace(validateRequest.IdentificationQuestion))
                {
                    request.delivery_services.identification_question = new SendMoneyValidation.identification_question()
                    {
                        question = validateRequest.IdentificationQuestion,
                        answer = validateRequest.IdentificationAnswer,
                    };
                }
            }
            if (!string.IsNullOrWhiteSpace(validateRequest.PersonalMessage))
            {
                string[] personalMessages = MessageBlockSplit(validateRequest.PersonalMessage).ToArray();

                int msgcnt = personalMessages.Length;
                SendMoneyValidation.message_details msgs = new SendMoneyValidation.message_details()
                {
                    message_details1 = new SendMoneyValidation.messages()
                    {
                        text = personalMessages,
                        context = msgcnt.ToString()
                    }
                };
                request.delivery_services.message = msgs;
            }

            request.financials = new SendMoneyValidation.financials()
            {
                originators_principal_amount = ConvertDecimalToLong(validateRequest.OriginatorsPrincipalAmount),
                originators_principal_amountSpecified = validateRequest.DestinationPrincipalAmount > 0,
            };

            //When a Passport is used as the form of ID in the customer profile
            //In order for WU transaction to work correctly with Passports the id_country_of_issue field should use country name instead of country code 	

            string issueCountry = validateRequest.PrimaryCountryCodeOfIssue;
            if (validateRequest.PrimaryIdType != null)
            {
                if (validateRequest.PrimaryIdType.Equals("PASSPORT")
                    || validateRequest.PrimaryIdType.Equals("EMPLOYMENT AUTHORIZATION CARD (EAD)")
                    || validateRequest.PrimaryIdType.Equals("PERMANENT RESIDENT CARD")
                    || validateRequest.PrimaryIdType.Equals("MILITARY ID"))
                {
                    issueCountry = validateRequest.PrimaryIdCountryOfIssue;
                }
            }
            request.sender.compliance_details = new SendMoneyValidation.compliance_details()
            {
                template_id = WUCommonData.ComplianceTemplate.SEND_MONEY,
                id_details = new SendMoneyValidation.id_details()
                {
                    id_type = !string.IsNullOrEmpty(validateRequest.PrimaryIdType) ? WUCommonIO.GetGovtIDType(validateRequest.PrimaryIdType) : string.Empty,
                    id_number = validateRequest.PrimaryIdNumber,
                    id_country_of_issue = issueCountry,
                    id_place_of_issue = !string.IsNullOrEmpty(validateRequest.PrimaryIdPlaceOfIssue) ? validateRequest.PrimaryIdPlaceOfIssue : string.Empty //TODO State code
                },
                Current_address = new SendMoneyValidation.compliance_address()
                {
                    addr_line1 = MassagingValue(validateRequest.Address),
                    city = MassagingValue(validateRequest.City),
                    state_code = MassagingValue(validateRequest.State),
                    postal_code = MassagingValue(validateRequest.PostalCode),
                    country = CountryCode
                },
                date_of_birth = validateRequest.DateOfBirth,
                occupation = AlloyUtil.TrimString(MassagingValue(validateRequest.Occupation), 29),
                Country_of_Birth = validateRequest.CountryOfBirthAbbr2,
                ack_flag = "X",
                third_party_details = new SendMoneyValidation.third_party_details() { flag_pay = SendMoneyValidation.flag_pay.N, flag_paySpecified = true }
            };

            if (!string.IsNullOrEmpty(validateRequest.SecondIdNumber))
            {
                request.sender.compliance_details.second_id = new SendMoneyValidation.id_details()
                {
                    id_type = WUCommonIO.GetGovtIDType(validateRequest.SecondIdType),
                    id_number = validateRequest.SecondIdNumber,
                    id_country_of_issue = "United States"//sendMoneyValidateRequest.SecondIdCountryOfIssue
                };
            }

            if (validateRequest.SmsNotificationFlag == "Y")
            {
                request.sender.mobile_phone = new SendMoneyValidation.mobile_phone()
                {
                    phone_number = new SendMoneyValidation.international_phone_number()
                    {
                        country_code = "1",
                        national_number = validateRequest.MobilePhone
                    }
                };
            }

            if (request.sender.compliance_details.id_details != null && request.sender.compliance_details.id_details.id_country_of_issue != null)
            {
                if (validateRequest.PrimaryIdType != null)
                    if (request.sender.compliance_details.id_details.id_country_of_issue.Equals("US")
                        && (validateRequest.PrimaryIdType.Equals("DRIVER'S LICENSE") || validateRequest.PrimaryIdType.Equals("U.S. STATE IDENTITY CARD")
                        || validateRequest.PrimaryIdType.Equals("NEW YORK CITY ID") || validateRequest.PrimaryIdType.Equals("NEW YORK BENEFITS ID")))
                    {
                        request.sender.compliance_details.id_details.id_country_of_issue = string.Format("{0}/{1}", validateRequest.PrimaryCountryCodeOfIssue, validateRequest.PrimaryIdPlaceOfIssue);//For above goverment id type we need to send country code and state code.
                    }
                    else if (request.sender.compliance_details.id_details.id_country_of_issue.Equals("MX"))
                    {
                        request.sender.compliance_details.id_details.id_country_of_issue = "Mexico";
                    }
            }

            return request;
        }

        private SearchResponse SearchModify(SearchRequest searchRequest, ZeoContext context)
        {
            ModifySendMoneySearch.modifysendmoneysearchreply modifySendMoneySearchResponse = GetModifySearchResponse(searchRequest, context);

            var receiver = modifySendMoneySearchResponse.payment_transactions.payment_transaction[0].receiver;

            MoneyTransfer.Data.SearchResponse searchResponse = new MoneyTransfer.Data.SearchResponse();
            if (receiver.name.name_type == ModifySendMoneySearch.name_type.D)
            {
                searchResponse.FirstName = string.IsNullOrEmpty(receiver.name.first_name) ? string.Empty : Convert.ToString(receiver.name.first_name);
                searchResponse.SecondLastName = string.IsNullOrEmpty(receiver.name.middle_name) ? string.Empty : Convert.ToString(receiver.name.middle_name);
                searchResponse.LastName = string.IsNullOrEmpty(receiver.name.last_name) ? string.Empty : Convert.ToString(receiver.name.last_name);
            }
            else
            {
                searchResponse.FirstName = string.IsNullOrEmpty(receiver.name.given_name) ? string.Empty : Convert.ToString(receiver.name.given_name);
                searchResponse.SecondLastName = string.IsNullOrEmpty(receiver.name.maternal_name) ? string.Empty : Convert.ToString(receiver.name.maternal_name);
                searchResponse.LastName = string.IsNullOrEmpty(receiver.name.paternal_name) ? string.Empty : Convert.ToString(receiver.name.paternal_name);
            }

            if (modifySendMoneySearchResponse.delivery_services != null)
            {
                searchResponse.TestQuestionAvailable = modifySendMoneySearchResponse.delivery_services.test_question_available;

                if (modifySendMoneySearchResponse.delivery_services.identification_question != null)
                {
                    searchResponse.TestQuestion = modifySendMoneySearchResponse.delivery_services.identification_question.question;
                    searchResponse.TestAnswer = modifySendMoneySearchResponse.delivery_services.identification_question.answer;
                }
            }
            return searchResponse;
        }

        private modifysendmoneysearchreply GetModifySearchResponse(SearchRequest searchRequest, ZeoContext context)
        {
            WUIO = GetMTProcessor();
            ModifySendMoneySearch.modifysendmoneysearchrequest modifySearchSendRequest = new ModifySendMoneySearch.modifysendmoneysearchrequest();
            ModifySendMoneySearch.payment_transaction transaction = new ModifySendMoneySearch.payment_transaction();
            transaction.mtcn = searchRequest.ConfirmationNumber;

            modifySearchSendRequest.payment_transaction = transaction;

            ModifySendMoneySearch.modifysendmoneysearchreply sendMoneyModifySearchResponse = WUIO.ModifySearch(modifySearchSendRequest, context);
            return sendMoneyModifySearchResponse;
        }

        private SearchResponse SearchRefund(SearchRequest searchRefundRequest, ZeoContext context)
        {
            WUTransaction trx = new WUTransaction();
            searchRefundRequest.ReferenceNumber = DateTime.Now.ToString("yyyyMMddhhmmssff");
            searchRefundRequest.DTServerLastModified = DateTime.Now;
            searchRefundRequest.DTTerminalLastModified = GetTimeZoneTime(context.TimeZone);

            SearchResponse searchResponse = new SearchResponse();
            Search.searchreply response = new Search.searchreply();
            if (searchRefundRequest.SearchRequestType == SearchRequestType.RefundWithStage)
            {
                searchResponse = CreateRefundTransaction(searchRefundRequest, context);

                response = GetSearchRefundResponse(searchRefundRequest, context);

                UpdateSendMoneyRefundSearchTransaction(searchResponse.RefundTransactionId, response, context);
                //searchResponse.CancelTransactionId = searchRefundRequest.CancelTransactionId;
                //searchResponse.RefundTransactionId = searchRefundRequest.ModifyOrRefundTransactionId;

                searchResponse.RefundStatus = response.refund_cancel_flag;
            }
            else
            {
                response = GetSearchRefundResponse(searchRefundRequest, context);
                searchResponse.RefundStatus = response.refund_cancel_flag;
            }

            return searchResponse;
        }

        private Search.searchreply GetSearchRefundResponse(Cxn.MoneyTransfer.Data.SearchRequest searchRequest, ZeoContext context)
        {
            Search.searchrequest searchReq = new Search.searchrequest();
            searchReq.payment_transaction = new Search.payment_transaction();
            searchReq.payment_transaction.mtcn = searchRequest.ConfirmationNumber;
            searchReq.search_flag = WU.Search.agentcsc_flags.REFUND;
            searchReq.search_flagSpecified = true;
            searchReq.device = new Search.gwp_gbs_device();
            searchReq.device.type = WU.Search.gwp_gbs_device_type.AGENT;
            searchReq.device.typeSpecified = true;
            context.ReferenceNumber = searchRequest.ReferenceNumber;
            WUIO = GetMTProcessor();
            Search.searchreply reply = WUIO.Search(searchReq, context);

            return reply;
        }

        private void UpdateSendMoneyRefundSearchTransaction(long wuTrxId, Search.searchreply searchSendMoneyResponse, ZeoContext context)
        {
            var item = searchSendMoneyResponse.payment_transactions.payment_transaction[0];

            StoredProcedure trxnFeeAdjustmentProcedure = new StoredProcedure("usp_UpdateSendMoneyRefundTransaction");

            trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("WUTrxId").WithValue(wuTrxId));
            trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("recordingCountryCode").WithValue(item.payment_details.originating_country_currency.iso_code.country_code));
            trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("recordingCurrencyCode").WithValue(item.payment_details.originating_country_currency.iso_code.currency_code));
            trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("originatorsPrincipalAmount").WithValue(item.financials.principal_amount / 100));
            trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("destinationPrincipalAmount").WithValue(item.financials.pay_amount / 100));
            trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("grossTotalAmount").WithValue(item.financials.gross_total_amount / 100));
            trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("charges").WithValue(item.financials.charges / 100));
            trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("moneyTransferKey").WithValue(item.money_transfer_key));
            trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("tempMTCN").WithValue(item.new_mtcn));
            trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("refundType").WithValue(2)); //RefundSearchTransaction.
            trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("dtTerminalDate").WithValue(GetTimeZoneTime(context.TimeZone)));
            trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("dtServerDate").WithValue(DateTime.Now));

            DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(trxnFeeAdjustmentProcedure);

        }

        private ModifySendMoney.modifysendmoneyrequest PopulateModifySendMoneyRequest(ModifySendMoneyRequest modifyRequest, ZeoContext context)
        {
            ModifySendMoney.general_name receiverName = null;
            WUCommonIO = GetCommonProcessor();
            ModifySendMoney.modifysendmoneyrequest modifysendmoneyrequest = null;

            var countryinfo = new ModifySendMoney.country_currency_info()
            {
                iso_code = new ModifySendMoney.iso_code()
                {
                    country_code = CountryCode,
                    currency_code = CountryCurrencyCode
                }
            };

            if (modifyRequest != null && modifyRequest.Transaction != null && modifyRequest.Receiver != null
                    && modifyRequest.Account != null)
            {
                if (!string.IsNullOrWhiteSpace(modifyRequest.Transaction.ReceiverSecondLastName))
                {
                    receiverName = new ModifySendMoney.general_name()
                    {
                        given_name = modifyRequest.Receiver.FirstName,
                        paternal_name = modifyRequest.Receiver.LastName,
                        maternal_name = modifyRequest.Receiver.SecondLastName,
                        name_type = ModifySendMoney.name_type.M,
                        name_typeSpecified = true
                    };
                }
                else
                {
                    receiverName = new ModifySendMoney.general_name()
                    {
                        first_name = modifyRequest.Receiver.FirstName,
                        last_name = modifyRequest.Receiver.LastName,
                        name_type = ModifySendMoney.name_type.D,
                        name_typeSpecified = true
                    };
                }

                modifysendmoneyrequest = new ModifySendMoney.modifysendmoneyrequest()
                {


                    sender = new ModifySendMoney.sender
                    {
                        preferred_customer = new ModifySendMoney.preferred_customer()
                        {
                            account_nbr = modifyRequest.Account.PreferredCustomerAccountNumber,
                        },
                        name = new ModifySendMoney.general_name
                        {
                            first_name = modifyRequest.Account.FirstName,
                            last_name = modifyRequest.Account.LastName,
                            name_type = ModifySendMoney.name_type.D,
                            name_typeSpecified = true
                        },
                        address = new ModifySendMoney.address()
                        {
                            addr_line1 = modifyRequest.Account.Address,
                            city = modifyRequest.Account.City,
                            state = modifyRequest.Account.State,
                            postal_code = modifyRequest.Account.PostalCode,
                            Item = countryinfo,

                        },
                        contact_phone = modifyRequest.Account.ContactPhone,
                        compliance_details = new ModifySendMoney.compliance_details()
                        {
                            compliance_data_buffer = modifyRequest.Transaction.SenderComplianceDetailsComplianceDataBuffer
                        },

                        unv_buffer = modifyRequest.Transaction.Sender_unv_Buffer
                    },
                    receiver = new ModifySendMoney.receiver()
                    {
                        name = receiverName,
                        //contact_phone = modifyRequest.Receiver.PhoneNumber, //Check if it is required.
                        unv_buffer = modifyRequest.Transaction.Receiver_unv_Buffer,
                        address = new ModifySendMoney.address()
                        {
                            city = modifyRequest.Receiver.City
                        }
                    },
                    promotions = new ModifySendMoney.promotions()
                    {
                        promo_sequence_no = modifyRequest.Transaction.PromotionSequenceNo,
                        coupons_promotions = modifyRequest.Transaction.PromotionsCode ?? string.Empty,
                        promo_code_description = modifyRequest.Transaction.PromoCodeDescription ?? string.Empty,
                        promo_name = modifyRequest.Transaction.PromoName ?? string.Empty,
                        promo_discount_amount = Convert.ToInt64(modifyRequest.Transaction.PromotionDiscount),
                        sender_promo_code = modifyRequest.Transaction.PromotionsCode ?? string.Empty,
                        promo_message = modifyRequest.Transaction.PromoMessage ?? string.Empty,
                        promo_discount_amountSpecified = modifyRequest.Transaction.PromotionDiscount > 0
                    },
                    payment_details = new ModifySendMoney.payment_details()
                    {
                        expected_payout_location = new ModifySendMoney.expected_payout_location()
                        {
                            city = modifyRequest.Transaction.ExpectedPayoutCityName,
                            state_code = modifyRequest.Transaction.ExpectedPayoutStateCode
                        },

                        originating_country_currency = countryinfo,

                        recording_country_currency = countryinfo,
                        destination_country_currency = new ModifySendMoney.country_currency_info()
                        {
                            iso_code = new ModifySendMoney.iso_code()
                            {
                                country_code = modifyRequest.Transaction.DestinationCountryCode,
                                currency_code = modifyRequest.Transaction.DestinationCurrencyCode
                            }
                        },
                        originating_city = modifyRequest.Transaction.originating_city,
                        originating_state = modifyRequest.Transaction.originating_state,
                        transaction_type = ModifySendMoney.transaction_type.WMN,
                        transaction_typeSpecified = true,
                        payment_type = ModifySendMoney.payment_type.Cash,
                        payment_typeSpecified = true,
                        duplicate_detection_flag = AllowDuplicateTrxWU,
                        exchange_rate = Convert.ToInt64(modifyRequest.Transaction.ExchangeRate),
                        money_transfer_type = ModifySendMoney.money_transfer_type.WMN,
                        original_destination_country_currency = new ModifySendMoney.country_currency_info()
                        {
                            iso_code = new ModifySendMoney.iso_code()
                            {
                                currency_code = modifyRequest.Transaction.OriginalDestinationCurrencyCode,
                                country_code = modifyRequest.Transaction.OriginalDestinationCountryCode
                            }
                        },

                    },

                    delivery_services = new ModifySendMoney.delivery_services()
                    {
                        code = string.IsNullOrEmpty(modifyRequest.Transaction.DeliveryOption) ? modifyRequest.Transaction.DeliveryServiceName : modifyRequest.Transaction.DeliveryOption,
                    },
                    financials = new ModifySendMoney.financials()
                    {
                        taxes = new ModifySendMoney.taxes()
                        {
                            municipal_tax = ConvertDecimalToLong(modifyRequest.Transaction.municipal_tax),
                            municipal_taxSpecified = true,
                            state_tax = ConvertDecimalToLong(modifyRequest.Transaction.state_tax),
                            state_taxSpecified = true,
                            county_tax = ConvertDecimalToLong(modifyRequest.Transaction.county_tax),
                            county_taxSpecified = true,
                        },
                        gross_total_amount = ConvertDecimalToLong(modifyRequest.Transaction.GrossTotalAmount),
                        gross_total_amountSpecified = modifyRequest.Transaction.GrossTotalAmount > 0,
                        plus_charges_amount = ConvertDecimalToLong(modifyRequest.Transaction.plus_charges_amount),
                        plus_charges_amountSpecified = modifyRequest.Transaction.plus_charges_amount > 0,
                        charges = ConvertDecimalToLong(modifyRequest.Transaction.Charges),
                        chargesSpecified = modifyRequest.Transaction.Charges > 0,
                        principal_amount = ConvertDecimalToLong(modifyRequest.Transaction.Principal_Amount),
                        principal_amountSpecified = true
                    },
                    mtcn = modifyRequest.Transaction.MTCN,
                    new_mtcn = modifyRequest.Transaction.TempMTCN,
                    money_transfer_key = modifyRequest.Transaction.MoneyTransferKey,
                    confirmed_id = ModifySendMoney.modifysendmoneyrequestConfirmed_id.Y,
                    confirmed_idSpecified = true,


                    df_fields = new ModifySendMoney.df_fields()
                    {
                        amount_to_receiver = Convert.ToDouble(modifyRequest.Transaction.AmountToReceiver),
                        amount_to_receiverSpecified = modifyRequest.Transaction.AmountToReceiver > 0,
                        pay_side_charges = Convert.ToDouble(modifyRequest.Transaction.PaySideCharges),
                        pay_side_chargesSpecified = modifyRequest.Transaction.PaySideCharges > 0,
                        pay_side_tax = Convert.ToDouble(modifyRequest.Transaction.PaySideTax),
                        pay_side_taxSpecified = modifyRequest.Transaction.PaySideTax > 0,
                        delivery_service_name = modifyRequest.Transaction.DeliveryServiceDesc ?? string.Empty,
                        pds_required_flag = modifyRequest.Transaction.PdsRequiredFlag ? ModifySendMoney.yes_no.Y : ModifySendMoney.yes_no.N,
                        pds_required_flagSpecified = true,
                        df_transaction_flag = modifyRequest.Transaction.DfTransactionFlag ? ModifySendMoney.yes_no.Y : ModifySendMoney.yes_no.N,
                        df_transaction_flagSpecified = true,
                        available_for_pickup = modifyRequest.Transaction.AvailableForPickup,
                        available_for_pickup_est = modifyRequest.Transaction.AvailableForPickupEST
                    }
                };

                if (modifyRequest.Transaction.message_charge > 0)
                {
                    modifysendmoneyrequest.financials.message_charge = ConvertDecimalToLong(modifyRequest.Transaction.message_charge);
                    modifysendmoneyrequest.financials.message_chargeSpecified = true;
                }

                if (modifyRequest.Transaction.OriginatorsPrincipalAmount > 0)
                {
                    modifysendmoneyrequest.financials.originators_principal_amount = ConvertDecimalToLong(modifyRequest.Transaction.OriginatorsPrincipalAmount);
                    modifysendmoneyrequest.financials.originators_principal_amountSpecified = true;
                }

                if (modifyRequest.Transaction.DestinationPrincipalAmount > 0)
                {
                    modifysendmoneyrequest.financials.destination_principal_amount = ConvertDecimalToLong(modifyRequest.Transaction.DestinationPrincipalAmount);
                    modifysendmoneyrequest.financials.destination_principal_amountSpecified = true;
                }

                if (modifyRequest.Transaction.total_discount > 0)
                {
                    modifysendmoneyrequest.financials.total_discount = ConvertDecimalToLong(modifyRequest.Transaction.total_discount);
                    modifysendmoneyrequest.financials.total_discountSpecified = true;
                }

                if (modifyRequest.Transaction.total_discounted_charges > 0)
                {
                    modifysendmoneyrequest.financials.total_discounted_charges = ConvertDecimalToLong(modifyRequest.Transaction.total_discounted_charges);
                    modifysendmoneyrequest.financials.total_discounted_chargesSpecified = true;
                }

                if (modifyRequest.Transaction.total_undiscounted_charges > 0)
                {
                    modifysendmoneyrequest.financials.total_undiscounted_charges = ConvertDecimalToLong(modifyRequest.Transaction.total_undiscounted_charges);
                    modifysendmoneyrequest.financials.total_undiscounted_chargesSpecified = true;
                }

                if (modifyRequest.Account.SmsNotificationFlag == "Y")
                {
                    modifysendmoneyrequest.sender.mobile_phone = new ModifySendMoney.mobile_phone()
                    {
                        phone_number = new ModifySendMoney.international_phone_number()
                        {
                            country_code = "1",
                            national_number = modifyRequest.Account.MobilePhone
                        }
                    };
                }

                if (!string.IsNullOrWhiteSpace(modifyRequest.Transaction.TestQuestion))
                {
                    modifysendmoneyrequest.delivery_services.identification_question = new ModifySendMoney.identification_question()
                    {
                        question = modifyRequest.Transaction.TestQuestion,
                        answer = modifyRequest.Transaction.TestAnswer
                    };
                }

                if (!string.IsNullOrWhiteSpace(modifyRequest.Transaction.PersonalMessage))
                {
                    string[] personalMessages = MessageBlockSplit(modifyRequest.Transaction.PersonalMessage).ToArray();

                    int msgcnt = personalMessages.Length;
                    var msgs = new ModifySendMoney.message_details()
                    {
                        message_details1 = new ModifySendMoney.messages()
                        {
                            text = personalMessages,
                            context = msgcnt.ToString()
                        }
                    };
                    modifysendmoneyrequest.delivery_services.message = msgs;
                }

                if (!string.IsNullOrWhiteSpace(modifyRequest.Transaction.instant_notification_addl_service_charges))
                {
                    modifysendmoneyrequest.instant_notification = new ModifySendMoney.instant_notification()
                    {
                        addl_service_charges = modifyRequest.Transaction.instant_notification_addl_service_charges
                    };
                }

            }

            modifysendmoneyrequest.swb_fla_info = Mapper.Map<WUCommonData.SwbFlaInfo, ModifySendMoney.swb_fla_info>(WUCommonIO.BuildSwbFlaInfo(context));
            modifysendmoneyrequest.swb_fla_info.fla_name = Mapper.Map<WUCommonData.GeneralName, ModifySendMoney.general_name>(WUCommonIO.BuildGeneralName(context));

            return modifysendmoneyrequest;

        }

        private void UpdateSendMoneyModifyTransaction(long wuTrxId, ModifySendMoney.modifysendmoneyreply response, ZeoContext context)
        {
            StoredProcedure trxnmodifyProcedure = new StoredProcedure("usp_UpdateSendMoneyModifyTransaction");

            trxnmodifyProcedure.WithParameters(InputParameter.Named("WUTrxId").WithValue(wuTrxId));
            trxnmodifyProcedure.WithParameters(InputParameter.Named("MTCN").WithValue(response.mtcn));
            trxnmodifyProcedure.WithParameters(InputParameter.Named("ModifyType").WithValue(1)); //ModifyTransaction.
            trxnmodifyProcedure.WithParameters(InputParameter.Named("DTTerminalDate").WithValue(GetTimeZoneTime(context.TimeZone)));
            trxnmodifyProcedure.WithParameters(InputParameter.Named("DTServerDate").WithValue(DateTime.Now));

            DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(trxnmodifyProcedure);
        }

        private receivemoneypayrequest PopulateReceiveMoneyPayRequest(Data.CommitRequest commitRequest, ZeoContext context)
        {
            WUCommonIO = GetCommonProcessor();

            var countryCurrencyInfo = new ReceiveMoneyPay.country_currency_info()
            {
                iso_code = new ReceiveMoneyPay.iso_code()
                {
                    country_code = CountryCode,
                    currency_code = CountryCurrencyCode
                },
                country_name = CountryName
            };

            var receiveMoneyPayRequest = new receivemoneypayrequest
            {
                mtcn = commitRequest.MTCN,
                new_mtcn = commitRequest.TempMTCN,
                money_transfer_key = commitRequest.MoneyTransferKey,
                receiver = new ReceiveMoneyPay.receiver
                {
                    name = new ReceiveMoneyPay.general_name()
                    {
                        first_name = commitRequest.FirstName,
                        last_name = commitRequest.LastName,
                        name_type = ReceiveMoneyPay.name_type.D,
                        name_typeSpecified = true
                    },
                    address = new ReceiveMoneyPay.address
                    {
                        addr_line1 = commitRequest.Address,
                        city = commitRequest.City,
                        state = commitRequest.State,
                        postal_code = commitRequest.PostalCode,
                        Item = countryCurrencyInfo
                    },
                    compliance_details = new ReceiveMoneyPay.compliance_details()
                    {
                        compliance_data_buffer = commitRequest.SenderComplianceDetailsComplianceDataBuffer
                    }
                },
                delivery_services = new ReceiveMoneyPay.delivery_services()
                {
                    identification_question = new ReceiveMoneyPay.identification_question()
                    {
                        question = commitRequest.TestQuestion,
                        answer = commitRequest.TestAnswer
                    }
                },
                financials = new ReceiveMoneyPay.financials()
                {
                    gross_total_amount = ConvertDecimalToLong(commitRequest.GrossTotalAmount),
                    gross_total_amountSpecified = true,
                    pay_amount = ConvertDecimalToLong(commitRequest.AmountToReceiver),
                    pay_amountSpecified = true,
                    principal_amount = ConvertDecimalToLong(commitRequest.DestinationPrincipalAmount),
                    principal_amountSpecified = true,
                    charges = ConvertDecimalToLong(commitRequest.Charges),
                    chargesSpecified = true
                },
                payment_details = new ReceiveMoneyPay.payment_details
                {
                    destination_country_currency = new ReceiveMoneyPay.country_currency_info()
                    {
                        iso_code = new ReceiveMoneyPay.iso_code()
                        {
                            country_code = commitRequest.DestinationCountryCode,
                            currency_code = commitRequest.DestinationCurrencyCode
                        }
                    },
                    originating_country_currency = countryCurrencyInfo,
                    original_destination_country_currency = countryCurrencyInfo,
                    expected_payout_location = new ReceiveMoneyPay.expected_payout_location()
                    {
                        state_code = commitRequest.ExpectedPayoutStateCode,
                        city = commitRequest.ExpectedPayoutCityName
                    },
                    exchange_rate = Convert.ToDouble(commitRequest.ExchangeRate),
                    exchange_rateSpecified = true
                }
            };

            if (!string.IsNullOrWhiteSpace(commitRequest.originating_city))
            {
                receiveMoneyPayRequest.payment_details.originating_city = commitRequest.originating_city;
            }

            if (!string.IsNullOrEmpty(context.RMTrxType) && context.RMTrxType == ReceiveMoneyPay.mt_requested_status.HOLD.ToString())
            {
                receiveMoneyPayRequest.payment_details.mt_requested_status = ReceiveMoneyPay.mt_requested_status.HOLD;
                receiveMoneyPayRequest.pay_or_do_not_pay_indicator = pay_or_do_not_pay_indicator.P;
            }
            if (!string.IsNullOrEmpty(context.RMTrxType) && context.RMTrxType == ReceiveMoneyPay.mt_requested_status.RELEASE.ToString())
            {
                receiveMoneyPayRequest.payment_details.mt_requested_status = ReceiveMoneyPay.mt_requested_status.RELEASE;
                receiveMoneyPayRequest.pay_or_do_not_pay_indicator = pay_or_do_not_pay_indicator.P;
            }
            else if (!string.IsNullOrEmpty(context.RMTrxType) && context.RMTrxType == ReceiveMoneyPay.mt_requested_status.CANCEL.ToString())
            {
                receiveMoneyPayRequest.payment_details.mt_requested_status = ReceiveMoneyPay.mt_requested_status.CANCEL;
            }

            receiveMoneyPayRequest.payment_details.mt_requested_statusSpecified = true;

            receiveMoneyPayRequest.swb_fla_info = Mapper.Map<WUCommonData.SwbFlaInfo, ReceiveMoneyPay.swb_fla_info>(WUCommonIO.BuildSwbFlaInfo(context));
            receiveMoneyPayRequest.swb_fla_info.fla_name = Mapper.Map<WUCommonData.GeneralName, ReceiveMoneyPay.general_name>(WUCommonIO.BuildGeneralName(context));
            return receiveMoneyPayRequest;
        }

        private SendMoneyStore.sendmoneystorerequest PopulateSendMoneyStoreRequest(Data.CommitRequest commitRequest, ZeoContext context)
        {
            WUCommonIO = GetCommonProcessor();

            var requestedStatus = SendMoneyStore.mt_requested_status.HOLD;

            if (context.SMTrxType == (Helper.RequestType.Release).ToString())
                requestedStatus = SendMoneyStore.mt_requested_status.RELEASE;
            else if (context.SMTrxType == (Helper.RequestType.Cancel).ToString())
                requestedStatus = SendMoneyStore.mt_requested_status.CANCEL;
            else if (context.SMTrxType == (Helper.RequestType.Hold).ToString())
                requestedStatus = SendMoneyStore.mt_requested_status.HOLD;

            SendMoneyStore.general_name receiverName = null;
            SendMoneyStore.general_name senderName = null;

            var countryinfo = new SendMoneyStore.country_currency_info()
            {
                iso_code = new SendMoneyStore.iso_code()
                {
                    country_code = CountryCode,
                    currency_code = CountryCurrencyCode
                }
            };

            if (!string.IsNullOrWhiteSpace(commitRequest.ReceiverSecondLastName))
            {
                receiverName = new SendMoneyStore.general_name()
                {
                    given_name = commitRequest.ReceiverFirstName,
                    paternal_name = commitRequest.ReceiverLastName,
                    maternal_name = commitRequest.ReceiverSecondLastName,
                    name_type = SendMoneyStore.name_type.M,
                    name_typeSpecified = true
                };
            }
            else
            {
                receiverName = new SendMoneyStore.general_name()
                {
                    first_name = commitRequest.ReceiverFirstName,
                    last_name = commitRequest.ReceiverLastName,
                    name_type = SendMoneyStore.name_type.D,
                    name_typeSpecified = true
                };
            }
            if (!string.IsNullOrWhiteSpace(commitRequest.SecondLastName))
            {
                senderName = new SendMoneyStore.general_name()
                {
                    given_name = commitRequest.FirstName,
                    paternal_name = commitRequest.LastName,
                    maternal_name = commitRequest.SecondLastName,
                    name_type = SendMoneyStore.name_type.M,
                    name_typeSpecified = true
                };
            }
            else
            {
                senderName = new SendMoneyStore.general_name()
                {
                    first_name = commitRequest.FirstName,
                    last_name = commitRequest.LastName,
                    middle_name = commitRequest.MiddleName,
                    name_type = SendMoneyStore.name_type.D,
                    name_typeSpecified = true
                };
            }


            var sendMoneyStoreRequest = new SendMoneyStore.sendmoneystorerequest()
            {
                sender = new SendMoneyStore.sender
                {
                    name = senderName,
                    preferred_customer = new SendMoneyStore.preferred_customer()
                    {
                        account_nbr = commitRequest.PreferredCustomerAccountNumber,
                    },
                    address = new SendMoneyStore.address()
                    {
                        addr_line1 = commitRequest.Address,
                        city = commitRequest.City,
                        state = commitRequest.State,
                        postal_code = commitRequest.PostalCode,
                        Item = countryinfo
                    },
                    contact_phone = commitRequest.ContactPhone,
                    compliance_details = new SendMoneyStore.compliance_details()
                    {
                        compliance_data_buffer = commitRequest.SenderComplianceDetailsComplianceDataBuffer
                    },
                    sms_notification_flag = (commitRequest.SmsNotificationFlag == WUCommonData.WUEnums.yes_no.Y.ToString()) ? SendMoneyStore.sms_notification.Y : SendMoneyStore.sms_notification.N,
                    sms_notification_flagSpecified = (commitRequest.SmsNotificationFlag == WUCommonData.WUEnums.yes_no.Y.ToString()) ? true : false,
                    fraud_warning_consent = SendMoneyStore.fraud_warning_consent.Y,
                    fraud_warning_consentSpecified = true
                },
                receiver = new SendMoneyStore.receiver()
                {
                    name = receiverName,
                    contact_phone = commitRequest.ReceiverContactNumber
                },
                promotions = new SendMoneyStore.promotions()
                {
                    promo_sequence_no = commitRequest.PromotionSequenceNo,
                    coupons_promotions = commitRequest.PromotionsCode ?? string.Empty,
                    promo_code_description = commitRequest.PromoCodeDescription ?? string.Empty,
                    promo_name = commitRequest.PromoName ?? string.Empty,
                    promo_discount_amount = ConvertDecimalToLong(commitRequest.PromotionDiscount),
                    sender_promo_code = commitRequest.PromotionsCode ?? string.Empty,
                    promo_message = commitRequest.PromoMessage ?? string.Empty,
                    promo_discount_amountSpecified = commitRequest.PromotionDiscount > 0
                },
                payment_details = new SendMoneyStore.payment_details()
                {
                    expected_payout_location = new SendMoneyStore.expected_payout_location()
                    {
                        city = commitRequest.ExpectedPayoutCityName,
                        state_code = commitRequest.ExpectedPayoutStateCode
                    },
                    originating_country_currency = countryinfo,
                    recording_country_currency = countryinfo,
                    destination_country_currency = new SendMoneyStore.country_currency_info()
                    {
                        iso_code = new SendMoneyStore.iso_code()
                        {
                            country_code = commitRequest.DestinationCountryCode,
                            currency_code = commitRequest.DestinationCurrencyCode
                        }
                    },
                    transaction_type = SendMoneyStore.transaction_type.WMN,
                    transaction_typeSpecified = true,
                    payment_type = SendMoneyStore.payment_type.Cash,
                    payment_typeSpecified = true,
                    duplicate_detection_flag = AllowDuplicateTrxWU,
                    mt_requested_status = requestedStatus,
                    mt_requested_statusSpecified = true
                },
                delivery_services = new SendMoneyStore.delivery_services()
                {
                    code = string.IsNullOrEmpty(commitRequest.DeliveryOption) ? commitRequest.DeliveryServiceName : commitRequest.DeliveryOption,
                    identification_question = new SendMoneyStore.identification_question
                    {
                        question = commitRequest.TestQuestion,
                        answer = commitRequest.TestAnswer
                    }
                },
                financials = new SendMoneyStore.financials()
                {
                    taxes = new SendMoneyStore.taxes()
                    {
                        municipal_tax = ConvertDecimalToLong(commitRequest.municipal_tax),
                        municipal_taxSpecified = true,
                        state_tax = ConvertDecimalToLong(commitRequest.state_tax),
                        state_taxSpecified = true,
                        county_tax = ConvertDecimalToLong(commitRequest.county_tax),
                        county_taxSpecified = true,
                    },
                    gross_total_amount = ConvertDecimalToLong(commitRequest.GrossTotalAmount),
                    gross_total_amountSpecified = commitRequest.GrossTotalAmount > 0,
                    plus_charges_amount = ConvertDecimalToLong(commitRequest.plus_charges_amount),
                    plus_charges_amountSpecified = commitRequest.plus_charges_amount > 0,
                    charges = ConvertDecimalToLong(commitRequest.Charges),
                    chargesSpecified = commitRequest.Charges > 0
                },
                mtcn = commitRequest.MTCN,
                new_mtcn = commitRequest.TempMTCN,
                df_fields = new SendMoneyStore.df_fields()
                {
                    amount_to_receiver = Convert.ToDouble(commitRequest.AmountToReceiver),
                    amount_to_receiverSpecified = commitRequest.AmountToReceiver > 0,
                    pay_side_charges = Convert.ToDouble(commitRequest.PaySideCharges),
                    pay_side_chargesSpecified = commitRequest.PaySideCharges > 0,
                    pay_side_tax = Convert.ToDouble(commitRequest.PaySideTax),
                    pay_side_taxSpecified = commitRequest.PaySideTax > 0,
                    delivery_service_name = commitRequest.DeliveryServiceDesc ?? string.Empty,
                    pds_required_flag = commitRequest.PdsRequiredFlag ? SendMoneyStore.yes_no.Y : SendMoneyStore.yes_no.N,
                    pds_required_flagSpecified = true,
                    df_transaction_flag = commitRequest.DfTransactionFlag ? SendMoneyStore.yes_no.Y : SendMoneyStore.yes_no.N,
                    df_transaction_flagSpecified = true
                },
                consumer_fraud_prompts = new SendMoneyStore.consumer_fraud_prompts()
                {
                    question1 = commitRequest.ConsumerFraudPromptQuestion
                }
            };

            if (commitRequest.message_charge > 0)
            {
                sendMoneyStoreRequest.financials.message_charge = ConvertDecimalToLong(commitRequest.message_charge);
                sendMoneyStoreRequest.financials.message_chargeSpecified = true;
            }

            if (commitRequest.OriginatorsPrincipalAmount > 0)
            {
                sendMoneyStoreRequest.financials.originators_principal_amount = ConvertDecimalToLong(commitRequest.OriginatorsPrincipalAmount);
                sendMoneyStoreRequest.financials.originators_principal_amountSpecified = true;
            }

            if (commitRequest.DestinationPrincipalAmount > 0)
            {
                sendMoneyStoreRequest.financials.destination_principal_amount = ConvertDecimalToLong(commitRequest.DestinationPrincipalAmount);
                sendMoneyStoreRequest.financials.destination_principal_amountSpecified = true;
            }

            if (commitRequest.total_discount > 0)
            {
                sendMoneyStoreRequest.financials.total_discount = ConvertDecimalToLong(commitRequest.total_discount);
                sendMoneyStoreRequest.financials.total_discountSpecified = true;
            }

            if (commitRequest.total_discounted_charges > 0)
            {
                sendMoneyStoreRequest.financials.total_discounted_charges = ConvertDecimalToLong(commitRequest.total_discounted_charges);
                sendMoneyStoreRequest.financials.total_discounted_chargesSpecified = true;
            }

            if (commitRequest.total_undiscounted_charges > 0)
            {
                sendMoneyStoreRequest.financials.total_undiscounted_charges = ConvertDecimalToLong(commitRequest.total_undiscounted_charges);
                sendMoneyStoreRequest.financials.total_undiscounted_chargesSpecified = true;
            }


            if (commitRequest.SmsNotificationFlag == "Y")
            {
                sendMoneyStoreRequest.sender.mobile_phone = new SendMoneyStore.mobile_phone()
                {
                    phone_number = new SendMoneyStore.international_phone_number()
                    {
                        country_code = "1",
                        national_number = commitRequest.MobilePhone
                    }
                };
            }

            if (!string.IsNullOrWhiteSpace(commitRequest.TestQuestion))
            {
                sendMoneyStoreRequest.delivery_services.identification_question = new SendMoneyStore.identification_question()
                {
                    question = commitRequest.TestQuestion,
                    answer = commitRequest.TestAnswer
                };
            }

            if (!string.IsNullOrWhiteSpace(commitRequest.PersonalMessage))
            {
                string[] personalMessages = MessageBlockSplit(commitRequest.PersonalMessage).ToArray();

                int msgcnt = personalMessages.Length;
                var msgs = new SendMoneyStore.message_details()
                {
                    message_details1 = new SendMoneyStore.messages()
                    {
                        text = personalMessages,
                        context = msgcnt.ToString()
                    }
                };
                sendMoneyStoreRequest.delivery_services.message = msgs;
            }

            if (!string.IsNullOrWhiteSpace(commitRequest.instant_notification_addl_service_charges))
            {
                sendMoneyStoreRequest.instant_notification = new SendMoneyStore.instant_notification()
                {
                    addl_service_charges = commitRequest.instant_notification_addl_service_charges
                };
            }
            sendMoneyStoreRequest.swb_fla_info = Mapper.Map<WUCommonData.SwbFlaInfo, SendMoneyStore.swb_fla_info>(WUCommonIO.BuildSwbFlaInfo(context));
            sendMoneyStoreRequest.swb_fla_info.fla_name = Mapper.Map<WUCommonData.GeneralName, SendMoneyStore.general_name>(WUCommonIO.BuildGeneralName(context));

            return sendMoneyStoreRequest;
        }

        private string GetCardPoints(string cardNumber, ZeoContext context)
        {
            Helper.RequestType requestStatus = (Helper.RequestType)Enum.Parse(typeof(Helper.RequestType), context.SMTrxType, true);
            if (requestStatus == Helper.RequestType.Release)
            {
                WUCommonData.CardLookUpRequest cxncardlookupreq = new WUCommonData.CardLookUpRequest()
                {
                    sender = new WUCommonData.Sender()
                    {
                        PreferredCustomerAccountNumber = cardNumber,
                    }
                };
                WUCommonData.CardLookupDetails cardlookupdetails = WUCommonIO.WUCardLookupForCardNumber(cxncardlookupreq, context);
                return cardlookupdetails.WuCardTotalPointsEarned;
            }
            return null;
        }

        private void UpdateTrx(long transactionId, SendMoneyStore.sendmoneystorereply sendMoneyStoreReply, string totalPointsEarned, ZeoContext context)
        {
            WUTransaction transaction = new WUTransaction();
            transaction.MTCN = sendMoneyStoreReply.mtcn;
            transaction.TempMTCN = sendMoneyStoreReply.new_mtcn;
            if (sendMoneyStoreReply.df_fields != null)
            {
                transaction.AmountToReceiver = Convert.ToDecimal(sendMoneyStoreReply.df_fields.amount_to_receiver);
                transaction.AvailableForPickup = sendMoneyStoreReply.df_fields.available_for_pickup;
                if (!string.IsNullOrWhiteSpace(sendMoneyStoreReply.df_fields.available_for_pickup))
                {
                    transaction.DTAvailableForPickup = ParseDate(sendMoneyStoreReply.df_fields.available_for_pickup);
                }
                transaction.DfTransactionFlag = sendMoneyStoreReply.df_fields.df_transaction_flag == SendMoneyStore.yes_no.Y;
                transaction.PdsRequiredFlag = sendMoneyStoreReply.df_fields.pds_required_flag == SendMoneyStore.yes_no.Y;
                transaction.PaySideCharges = Convert.ToDecimal(sendMoneyStoreReply.df_fields.pay_side_charges);
                transaction.PaySideTax = Convert.ToDecimal(sendMoneyStoreReply.df_fields.pay_side_tax);
                transaction.DelayHours = sendMoneyStoreReply.df_fields.delay_hours;
                transaction.DeliveryServiceName = sendMoneyStoreReply.df_fields.delivery_service_name;

                if (sendMoneyStoreReply.df_fields.consumer_bureau_info != null && sendMoneyStoreReply.df_fields.consumer_bureau_info.state != null)
                {
                    transaction.AgencyName = sendMoneyStoreReply.df_fields.consumer_bureau_info.state.agencyname;
                    transaction.Url = sendMoneyStoreReply.df_fields.consumer_bureau_info.state.url1;
                    transaction.PhoneNumber = sendMoneyStoreReply.df_fields.consumer_bureau_info.state.phonenumber1;
                }
            }

            if (sendMoneyStoreReply.pin_text_message_set_2 != null && sendMoneyStoreReply.promo_text_message != null && sendMoneyStoreReply.auto_enroll_text != null)
            {
                string messageArea = string.Empty;

                messageArea = GetStringFromArray(sendMoneyStoreReply.pin_text_message_set_2);
                messageArea += GetStringFromArray(sendMoneyStoreReply.promo_text_message);
                messageArea += GetStringFromArray(sendMoneyStoreReply.auto_enroll_text);

                transaction.MessageArea = messageArea;
            }

            transaction.FilingDate = sendMoneyStoreReply.filing_date;
            transaction.FilingTime = sendMoneyStoreReply.filing_time;
            transaction.WuCardTotalPointsEarned = totalPointsEarned;

            transaction.DTTerminalLastModified = GetTimeZoneTime(context.TimeZone);
            transaction.DTServerLastModified = DateTime.Now;
            transaction.Id = transactionId;
            transaction.TranascationType = MoneyTransferType.Send.ToString();

            UpdateWUTransaction(transaction, UpdateTxType.SendMoneyStore, 0);
        }

        private DateTime ParseDate(string dateString)
        {
            return DateTime.ParseExact(dateString, "MMddyyyy", System.Globalization.CultureInfo.InvariantCulture);
        }

        private string GetStringFromArray(string[] stringArray)
        {
            StringBuilder message = new StringBuilder();
            foreach (var item in stringArray)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    message.Append(item);
                }
            }
            return message.ToString();
        }

        private void UpdateTrx(long transactionId, SendMoneyValidation.sendmoneyvalidationreply reply, ValidateRequest validateRequest, string timezone)
        {
            WUTransaction transaction = new WUTransaction();

            transaction.MTCN = reply.mtcn;
            transaction.TempMTCN = reply.new_mtcn;

            if (reply.df_fields != null)
            {
                transaction.DeliveryServiceDesc = (!string.IsNullOrWhiteSpace(reply.df_fields.delivery_service_name)) ? reply.df_fields.delivery_service_name.Trim() : transaction.DeliveryServiceDesc;
                transaction.PdsRequiredFlag = reply.df_fields.pds_required_flag == SendMoneyValidation.yes_no.Y;
                transaction.DfTransactionFlag = reply.df_fields.df_transaction_flag == SendMoneyValidation.yes_no.Y;
                //AL-571 added (PaySideTax,PaySideCharges)
                transaction.PaySideTax = Convert.ToDecimal(reply.df_fields.pay_side_tax);
                transaction.PaySideCharges = Convert.ToDecimal(reply.df_fields.pay_side_charges);

                transaction.AmountToReceiver = Convert.ToDecimal(reply.df_fields.amount_to_receiver);
            }

            transaction.municipal_tax = ConvertLongToDecimal(reply.financials.taxes.municipal_tax);
            transaction.state_tax = ConvertLongToDecimal(reply.financials.taxes.state_tax);
            transaction.county_tax = ConvertLongToDecimal(reply.financials.taxes.county_tax);
            transaction.plus_charges_amount = ConvertLongToDecimal(reply.financials.plus_charges_amount);
            transaction.message_charge = ConvertLongToDecimal(reply.financials.message_charge);
            transaction.total_discount = ConvertLongToDecimal(reply.financials.total_discount);
            transaction.total_discounted_charges = ConvertLongToDecimal(reply.financials.total_discounted_charges);
            transaction.total_undiscounted_charges = ConvertLongToDecimal(reply.financials.total_undiscounted_charges);
            transaction.PromoCodeDescription = (reply.promotions.promo_code_description);
            transaction.PromoName = (reply.promotions.promo_name);
            transaction.PromoMessage = (reply.promotions.promo_message);
            transaction.PromotionDiscount = ConvertLongToDecimal(reply.promotions.promo_discount_amount);
            transaction.PromotionsCode = (reply.promotions.sender_promo_code);
            transaction.PromotionSequenceNo = (reply.promotions.promo_sequence_no);
            transaction.GrossTotalAmount = ConvertLongToDecimal(reply.financials.gross_total_amount);
            transaction.Id = transactionId;
            transaction.TranascationType = Convert.ToString(validateRequest.TransferType);

            transaction.AdditionalCharges = transaction.plus_charges_amount + transaction.message_charge;

            if (!string.IsNullOrWhiteSpace(validateRequest.PersonalMessage))
            {
                transaction.PersonalMessage = validateRequest.PersonalMessage;
            }

            transaction.FilingDate = reply.filing_date;
            transaction.FilingTime = reply.filing_time;

            transaction.instant_notification_addl_service_charges = (reply.instant_notification != null) ? reply.instant_notification.addl_service_charges : string.Empty;

            if (reply.payment_details != null)
            {
                transaction.originating_city = reply.payment_details.originating_city;
                transaction.originating_state = reply.payment_details.originating_state;
                transaction.ExpectedPayoutCityName = reply.payment_details.expected_payout_location.city;
                transaction.ExpectedPayoutStateCode = reply.payment_details.expected_payout_location.state_code;
                transaction.IsFixedOnSend = reply.payment_details.fix_on_send == SendMoneyValidation.yes_no.Y;
            }

            if (validateRequest.DeliveryService != null)
            {
                transaction.DeliveryOption = validateRequest.DeliveryService;
                //NLogger.Info(string.Format("'{0}'", transaction.DeliveryServiceDesc), "Delivery services to get translations");
                //transaction.TransalatedDeliveryServiceName = GetDeliveryServiceTransalation(transaction.DeliveryServiceDesc, Language);//TODO
            }

            transaction.SenderComplianceDetailsComplianceDataBuffer = reply.sender.compliance_details.compliance_data_buffer;
            transaction.TaxAmount = transaction.county_tax + transaction.municipal_tax + transaction.state_tax;
            transaction.TestQuestion = validateRequest.IdentificationQuestion;
            transaction.TestAnswer = validateRequest.IdentificationAnswer;
            transaction.DTServerLastModified = DateTime.Now;
            transaction.DTTerminalLastModified = GetTimeZoneTime(timezone);
            transaction.Charges = validateRequest.Charges;
            decimal fee = transaction.Charges + transaction.AdditionalCharges + transaction.municipal_tax + transaction.state_tax + transaction.county_tax;
            transaction.ConsumerFraudPromptQuestion = validateRequest.ConsumerFraudPromptQuestion ? "Y" : "N";

            UpdateWUTransaction(transaction, UpdateTxType.ValidateRequest, fee);
        }

        private SendMoneyRefund.refundrequest PopulateRefundSendMoneyRequest(TCF.Zeo.Cxn.MoneyTransfer.Data.RefundRequest refundRequest, ZeoContext context)
        {
            SendMoneyRefund.general_name receiverName = null;

            var countryinfo = new SendMoneyRefund.country_currency_info()
            {
                iso_code = new SendMoneyRefund.iso_code()
                {
                    country_code = CountryCode,
                    currency_code = CountryCurrencyCode
                }
            };

            if (!string.IsNullOrWhiteSpace(refundRequest.ReceiverSecondLastName))
            {
                receiverName = new SendMoneyRefund.general_name()
                {
                    given_name = refundRequest.ReceiverFirstName,
                    paternal_name = refundRequest.ReceiverLastName,
                    maternal_name = refundRequest.ReceiverSecondLastName,
                    name_type = SendMoneyRefund.name_type.M,
                    name_typeSpecified = true
                };
            }
            else
            {
                receiverName = new SendMoneyRefund.general_name()
                {
                    first_name = refundRequest.ReceiverFirstName,
                    last_name = refundRequest.ReceiverLastName,
                    name_type = SendMoneyRefund.name_type.D,
                    name_typeSpecified = true
                };
            }

            var refundrequest = new SendMoneyRefund.refundrequest()
            {
                sender = new SendMoneyRefund.sender
                {
                    preferred_customer = new SendMoneyRefund.preferred_customer()
                    {
                        account_nbr = refundRequest.PreferredCustomerAccountNumber,
                    },
                    name = new SendMoneyRefund.general_name
                    {
                        first_name = refundRequest.FirstName,
                        last_name = refundRequest.LastName,
                        middle_name = refundRequest.MiddleName,
                        name_type = SendMoneyRefund.name_type.D,
                        name_typeSpecified = true
                    },
                    address = new SendMoneyRefund.address()
                    {
                        addr_line1 = refundRequest.Address,
                        city = refundRequest.City,
                        state = refundRequest.State,
                        postal_code = refundRequest.PostalCode,
                        Item = countryinfo
                    },
                    contact_phone = refundRequest.ContactPhone,
                    compliance_details = new SendMoneyRefund.compliance_details()
                    {
                        compliance_data_buffer = refundRequest.SenderComplianceDetailsComplianceDataBuffer
                    }
                },
                receiver = new SendMoneyRefund.receiver()
                {
                    name = receiverName,
                    contact_phone = refundRequest.PhoneNumber
                },
                payment_details = new SendMoneyRefund.payment_details()
                {
                    expected_payout_location = new SendMoneyRefund.expected_payout_location()
                    {
                        city = refundRequest.ExpectedPayoutCityName,
                        state_code = refundRequest.ExpectedPayoutStateCode
                    },
                    originating_country_currency = countryinfo,
                    recording_country_currency = countryinfo,
                    destination_country_currency = new SendMoneyRefund.country_currency_info()
                    {
                        iso_code = new SendMoneyRefund.iso_code()
                        {
                            country_code = refundRequest.DestinationCountryCode,
                            currency_code = refundRequest.DestinationCurrencyCode
                        }
                    },
                    transaction_type = SendMoneyRefund.transaction_type.WMN,
                    transaction_typeSpecified = true,
                    payment_type = SendMoneyRefund.payment_type.Refund,
                    payment_typeSpecified = true,
                    duplicate_detection_flag = AllowDuplicateTrxWU
                },

                financials = new SendMoneyRefund.financials()
                {
                    originators_principal_amount = ConvertDecimalToLong(refundRequest.OriginatorsPrincipalAmount),
                    originators_principal_amountSpecified = refundRequest.OriginatorsPrincipalAmount > 0,
                    destination_principal_amount = ConvertDecimalToLong(refundRequest.DestinationPrincipalAmount),
                    destination_principal_amountSpecified = refundRequest.DestinationPrincipalAmount > 0,
                    gross_total_amount = ConvertDecimalToLong(refundRequest.GrossTotalAmount),
                    gross_total_amountSpecified = refundRequest.GrossTotalAmount > 0,
                    pay_amount = ConvertDecimalToLong(refundRequest.AmountToReceiver),
                    pay_amountSpecified = refundRequest.AmountToReceiver > 0,
                    principal_amount = ConvertDecimalToLong(refundRequest.OriginatorsPrincipalAmount),
                    principal_amountSpecified = refundRequest.OriginatorsPrincipalAmount > 0,
                    plus_charges_amount = ConvertDecimalToLong(refundRequest.plus_charges_amount),
                    plus_charges_amountSpecified = refundRequest.plus_charges_amount > 0,
                    charges = ConvertDecimalToLong(refundRequest.Charges),
                    chargesSpecified = refundRequest.Charges > 0
                },
                mtcn = refundRequest.MTCN,
                new_mtcn = refundRequest.TempMTCN,
                money_transfer_key = refundRequest.MoneyTransferKey,
                encompass_reason_code = refundRequest.ReasonDesc == null ? "" : refundRequest.ReasonDesc.Substring(0, 3),
                comments = refundRequest.Comments,
                refund_cancel_flag = refundRequest.RefundStatus
            };

            WUCommonIO = GetCommonProcessor();
            refundrequest.swb_fla_info = Mapper.Map<WUCommonData.SwbFlaInfo, SendMoneyRefund.swb_fla_info>(WUCommonIO.BuildSwbFlaInfo(context));
            refundrequest.swb_fla_info.fla_name = Mapper.Map<WUCommonData.GeneralName, SendMoneyRefund.general_name>(WUCommonIO.BuildGeneralName(context));

            return refundrequest;
        }

        /// <summary>
        /// Get WU transaction
        /// </summary>
        /// <param name="wuTransactionId"></param>
        /// <returns></returns>
        public WUTransaction GetWUTransaction(long wuTrxId)
        {
            try
            {
                WUTransaction wUTransaction = null;
                StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_GetWUTransaction");

                moneyTransferProcedure.WithParameters(InputParameter.Named("wuTrxId").WithValue(wuTrxId));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(moneyTransferProcedure))
                {
                    while (datareader.Read())
                    {
                        wUTransaction = new WUTransaction()
                        {
                            OriginatorsPrincipalAmount = datareader.GetDecimalOrDefault("OriginatorsPrincipalAmount"),
                            OriginatingCountryCode = datareader.GetStringOrDefault("OriginatingCountryCode"),
                            OriginatingCurrencyCode = datareader.GetStringOrDefault("OriginatingCurrencyCode"),
                            TranascationType = datareader.GetStringOrDefault("TranascationType"),
                            PromotionsCode = datareader.GetStringOrDefault("PromotionsCode"),
                            ExchangeRate = datareader.GetDecimalOrDefault("ExchangeRate"),
                            DestinationPrincipalAmount = datareader.GetDecimalOrDefault("DestinationPrincipalAmount"),
                            GrossTotalAmount = datareader.GetDecimalOrDefault("GrossTotalAmount"),
                            Charges = datareader.GetDecimalOrDefault("Charges"),
                            TaxAmount = datareader.GetDecimalOrDefault("TaxAmount"),
                            MTCN = datareader.GetStringOrDefault("Mtcn"),
                            DTTerminalCreate = datareader.GetDateTimeOrDefault("DTTerminalCreate"),
                            DTTerminalLastModified = datareader.GetDateTimeOrDefault("DTTerminalLastModified"),
                            PromotionDiscount = datareader.GetDecimalOrDefault("PromotionDiscount"),
                            OtherCharges = datareader.GetDecimalOrDefault("OtherCharges"),
                            MoneyTransferKey = datareader.GetStringOrDefault("MoneyTransferKey"),
                            AdditionalCharges = datareader.GetDecimalOrDefault("AdditionalCharges"),
                            DestinationCountryCode = datareader.GetStringOrDefault("DestinationCountryCode"),
                            DestinationCurrencyCode = datareader.GetStringOrDefault("DestinationCurrencyCode"),
                            DestinationState = datareader.GetStringOrDefault("DestinationState"),
                            IsDomesticTransfer = datareader.GetBooleanOrDefault("IsDomesticTransfer"),
                            IsFixedOnSend = datareader.GetBooleanOrDefault("IsFixedOnSend"),
                            PhoneNumber = datareader.GetStringOrDefault("PhoneNumber"),
                            Url = datareader.GetStringOrDefault("Url"),
                            AgencyName = datareader.GetStringOrDefault("AgencyName"),
                            ChannelPartnerId = datareader.GetInt64OrDefault("ChannelPartnerId"),
                            ProviderId = datareader.GetInt32OrDefault("ProviderId"),
                            TestQuestion = datareader.GetStringOrDefault("TestQuestion"),
                            TempMTCN = datareader.GetStringOrDefault("TempMTCN"),
                            ExpectedPayoutStateCode = datareader.GetStringOrDefault("ExpectedPayoutStateCode"),
                            ExpectedPayoutCityName = datareader.GetStringOrDefault("ExpectedPayoutCityName"),
                            TestAnswer = datareader.GetStringOrDefault("TestAnswer"),
                            TestQuestionAvaliable = datareader.GetStringOrDefault("TestQuestionAvaliable"),
                            GCNumber = datareader.GetStringOrDefault("GCNumber"),
                            SenderName = datareader.GetStringOrDefault("SenderName"),
                            PdsRequiredFlag = datareader.GetBooleanOrDefault("PdsRequiredFlag"),
                            DfTransactionFlag = datareader.GetBooleanOrDefault("DfTransactionFlag"),
                            DeliveryServiceName = datareader.GetStringOrDefault("DeliveryServiceName"),
                            DTAvailableForPickup = datareader.GetDateTimeOrDefault("DTAvailableForPickup"),
                            DTServerCreate = datareader.GetDateTimeOrDefault("DTServerCreate"),
                            DTServerLastModified = datareader.GetDateTimeOrDefault("DTServerLastModified"),
                            ReceiverFirstName = datareader.GetStringOrDefault("RecieverFirstName"),
                            ReceiverLastName = datareader.GetStringOrDefault("RecieverLastName"),
                            ReceiverSecondLastName = datareader.GetStringOrDefault("RecieverSecondLastName"),
                            PromoCodeDescription = datareader.GetStringOrDefault("PromoCodeDescription"),
                            PromoName = datareader.GetStringOrDefault("PromoName"),
                            PromoMessage = datareader.GetStringOrDefault("PromoMessage"),
                            PromotionError = datareader.GetStringOrDefault("PromotionError"),
                            SenderComplianceDetailsComplianceDataBuffer = datareader.GetStringOrDefault("Sender_ComplianceDetails_ComplianceData_Buffer"),
                            recordingCountryCode = datareader.GetStringOrDefault("recordingCountryCode"),
                            recordingCurrencyCode = datareader.GetStringOrDefault("recordingCurrencyCode"),
                            originating_city = datareader.GetStringOrDefault("originating_city"),
                            originating_state = datareader.GetStringOrDefault("originating_state"),
                            municipal_tax = datareader.GetDecimalOrDefault("municipal_tax"),
                            state_tax = datareader.GetDecimalOrDefault("state_tax"),
                            county_tax = datareader.GetDecimalOrDefault("county_tax"),
                            plus_charges_amount = datareader.GetDecimalOrDefault("plus_charges_amount"),
                            message_charge = datareader.GetDecimalOrDefault("message_charge"),
                            total_undiscounted_charges = datareader.GetDecimalOrDefault("total_undiscounted_charges"),
                            total_discount = datareader.GetDecimalOrDefault("total_discount"),
                            total_discounted_charges = datareader.GetDecimalOrDefault("total_discounted_charges"),
                            instant_notification_addl_service_charges = datareader.GetStringOrDefault("instant_notification_addl_service_charges"),
                            PaySideCharges = datareader.GetDecimalOrDefault("PaySideCharges"),
                            PaySideTax = datareader.GetDecimalOrDefault("PaySideTax"),
                            AmountToReceiver = datareader.GetDecimalOrDefault("AmountToReceiver"),
                            SMSNotificationFlag = datareader.GetStringOrDefault("SMSNotificationFlag"),
                            PersonalMessage = datareader.GetStringOrDefault("PersonalMessage"),
                            DeliveryServiceDesc = datareader.GetStringOrDefault("DeliveryServiceDesc"),
                            ReferenceNo = datareader.GetStringOrDefault("ReferenceNo"),
                            pay_or_do_not_pay_indicator = datareader.GetStringOrDefault("pay_or_do_not_pay_indicator"),
                            OriginalDestinationCountryCode = datareader.GetStringOrDefault("OriginalDestinationCountryCode"),
                            OriginalDestinationCurrencyCode = datareader.GetStringOrDefault("OriginalDestinationCurrencyCode"),
                            FilingDate = datareader.GetStringOrDefault("FilingDate"),
                            FilingTime = datareader.GetStringOrDefault("FilingTime"),
                            PaidDateTime = datareader.GetStringOrDefault("PaidDateTime"),
                            AvailableForPickup = datareader.GetStringOrDefault("AvailableForPickup"),
                            DelayHours = datareader.GetStringOrDefault("DelayHours"),
                            AvailableForPickupEST = datareader.GetStringOrDefault("AvailableForPickupEST"),
                            WuCardTotalPointsEarned = datareader.GetStringOrDefault("WUCard_TotalPointsEarned"),
                            OriginalTransactionID = datareader.GetInt64OrDefault("OriginalTransactionID"),
                            TransactionSubType = datareader.GetStringOrDefault("TransactionSubType"),
                            ReasonCode = datareader.GetStringOrDefault("ReasonCode"),
                            ReasonDescription = datareader.GetStringOrDefault("ReasonDescription"),
                            Comments = datareader.GetStringOrDefault("Comments"),
                            DeliveryOption = datareader.GetStringOrDefault("DeliveryOption"),
                            DeliveryOptionDesc = datareader.GetStringOrDefault("DeliveryOptionDesc"),
                            PromotionSequenceNo = datareader.GetStringOrDefault("PromotionSequenceNo"),
                            CounterId = datareader.GetStringOrDefault("CounterId"),
                            Principal_Amount = datareader.GetDecimalOrDefault("PrincipalAmount"),
                            Receiver_unv_Buffer = datareader.GetStringOrDefault("Receiver_unv_Buffer"),
                            Sender_unv_Buffer = datareader.GetStringOrDefault("Sender_unv_Buffer"),
                            TransalatedDeliveryServiceName = datareader.GetStringOrDefault("TransalatedDeliveryServiceName"),
                            MessageArea = datareader.GetStringOrDefault("MessageArea"),
                            WUAccountId = datareader.GetInt64OrDefault("WUAccountID"),
                            ReceiverId = datareader.GetInt64OrDefault("WUReceiverID"),
                            Address = datareader.GetStringOrDefault("Address"),
                            City = datareader.GetStringOrDefault("City"),
                            State = datareader.GetStringOrDefault("State"),
                            PostalCode = datareader.GetStringOrDefault("PostalCode")
                        };
                    }

                }
                return wUTransaction;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETTRANSACTION_FAILED, ex);
            }

        }

        public WUTransaction GetReceiveTransaction(string confirmationNumber, long wuTrxId, ZeoContext context)
        {
            try
            {
                WUCommonIO = GetCommonProcessor();
                CheckCounterId(context.WUCounterId);
                WUIO = GetMTProcessor();

                WUTransaction transaction = new WUTransaction();
                transaction.MTCN = confirmationNumber;
                transaction.ReferenceNo = context.ReferenceNumber;
                transaction.ChannelPartnerId = context.ChannelPartnerId;
                transaction.ProviderId = context.ProviderId;
                transaction.TranascationType = Convert.ToString((int)MoneyTransferType.Receive);
                transaction.DTServerCreate = DateTime.Now;
                transaction.DTTerminalCreate = GetTimeZoneTime(context.TimeZone);

                if (wuTrxId == 0)
                {
                    wuTrxId = CreateReceiveMoneyTransaction(transaction, context);
                }
                transaction.Id = wuTrxId;

                receivemoneysearchrequest receiveMoneySearchRequest = PopulateReceiveMoneySearchRequest(confirmationNumber);

                receiveMoneySearchRequest.swb_fla_info = Mapper.Map<WUCommonData.SwbFlaInfo, ReceiveMoneySearch.swb_fla_info>(WUCommonIO.BuildSwbFlaInfo(context));
                receiveMoneySearchRequest.swb_fla_info.fla_name = Mapper.Map<WUCommonData.GeneralName, ReceiveMoneySearch.general_name>(WUCommonIO.BuildGeneralName(context));

                receivemoneysearchreply response = WUIO.SearchReceiveMoney(receiveMoneySearchRequest, context);

                ReceiveMoneySearch.payment_transaction paymentTransaction = response.payment_transactions.payment_transaction.FirstOrDefault();

                if(!string.IsNullOrWhiteSpace(paymentTransaction?.payment_details?.originating_country_currency?.iso_code?.country_code) &&
                    Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["ConsiderBlockedCountries"]))
                {
                    string countryCode = paymentTransaction.payment_details.originating_country_currency.iso_code.country_code;

                    //Checking whether the Country is blocked from TCF End for Send Money and Receive Money.
                    bool isBlocked = CheckCountryBlockedInZEO(countryCode);

                    if (isBlocked)
                        throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_COUNTRY_IS_BLOCKED);
                }

                UpdateReceiveMoneyTransaction(wuTrxId, response, context.TimeZone, transaction);

                decimal netAmount = ConvertLongToDecimal(paymentTransaction.financials.principal_amount);
                string senderStateListCode = paymentTransaction.payment_details != null ? paymentTransaction.payment_details.originating_city : string.Empty;

                string senderStateCode = GetSenderStateCode(wuTrxId, senderStateListCode);

                string receiverName = string.Format("{0} {1} {2}", paymentTransaction.receiver.name.first_name, paymentTransaction.receiver.name.middle_name, paymentTransaction.receiver.name.last_name);
                var nameType = paymentTransaction.receiver.name.name_type;
                if (nameType == ReceiveMoneySearch.name_type.M)
                {
                    receiverName = string.Format("{0} {1} {2}",
                    paymentTransaction.receiver.name.given_name,
                    paymentTransaction.receiver.name.paternal_name,
                    paymentTransaction.receiver.name.maternal_name);
                }

                transaction.MetaData = new Dictionary<string, object>()
            {
                {"ReceiverName", receiverName},
                {"NetAmount", netAmount},
                {"SenderStateCode", senderStateCode},
            };

                return transaction;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETRECEIVEMONEY_FAILED, ex);
            }
        }

        private receivemoneysearchrequest PopulateReceiveMoneySearchRequest(string confirmationNumber)
        {
            var receiveMoneySearchRequest = new receivemoneysearchrequest()
            {
                payment_transaction = new ReceiveMoneySearch.payment_transaction()
                {
                    mtcn = confirmationNumber
                }
            };

            return receiveMoneySearchRequest;
        }

        private void UpdateReceiveMoneyTransaction(long transactionId, receivemoneysearchreply reply, string timeZone, WUTransaction transaction)
        {

            ReceiveMoneySearch.payment_transaction paymentTransaction = reply.payment_transactions.payment_transaction.FirstOrDefault();

            if (!string.IsNullOrEmpty(paymentTransaction.pay_status_description) &&
                paymentTransaction.pay_status_description.ToLower() == "paid")
            {
                throw new MoneyTransferException(MoneyTransferException.TRANSACTION_ALREADY_PAID);
            }

            var receiverNameType = paymentTransaction.receiver.name.name_type;
            string originatingCity = string.Empty;
            string expectedLocationStateCode = string.Empty;
            string expectedLocationCityCode = string.Empty;
            string securityQuestion = string.Empty;
            string securityAnswer = string.Empty;
            string senderName = string.Format("{0} {1} {2}", paymentTransaction.sender.name.first_name, paymentTransaction.sender.name.middle_name, paymentTransaction.sender.name.last_name);
            var senderNameType = paymentTransaction.sender.name.name_type;

            if (senderNameType == ReceiveMoneySearch.name_type.M)
            {
                senderName = string.Format("{0} {1} {2}", paymentTransaction.sender.name.given_name, paymentTransaction.sender.name.paternal_name, paymentTransaction.sender.name.maternal_name);
            }

            if (reply.delivery_services != null && reply.delivery_services.identification_question != null)
            {
                securityQuestion = reply.delivery_services.identification_question.question;
                securityAnswer = reply.delivery_services.identification_question.answer;
            }

            if (paymentTransaction.payment_details.expected_payout_location != null)
            {
                expectedLocationStateCode = paymentTransaction.payment_details.expected_payout_location.state_code;
                expectedLocationCityCode = paymentTransaction.payment_details.expected_payout_location.city;
            }

            var personalMessageBuilder = new StringBuilder();

            if (reply.delivery_services != null && reply.delivery_services.message != null
                && reply.delivery_services.message.message_details1 != null)
            {
                foreach (string message in reply.delivery_services.message.message_details1.text)
                {
                    personalMessageBuilder.Append(message);
                }
            }

            if (paymentTransaction.payment_details.originating_city != null)
            {
                originatingCity = paymentTransaction.payment_details.originating_city;
            }

            transaction.Charges = ConvertLongToDecimal(paymentTransaction.financials.charges);
            transaction.DestinationPrincipalAmount = ConvertLongToDecimal(paymentTransaction.financials.principal_amount);
            transaction.ExchangeRate = Convert.ToDecimal(paymentTransaction.payment_details.exchange_rate);
            transaction.GrossTotalAmount = ConvertLongToDecimal(paymentTransaction.financials.gross_total_amount);
            transaction.AmountToReceiver = ConvertLongToDecimal(paymentTransaction.financials.pay_amount);
            transaction.MoneyTransferKey = paymentTransaction.money_transfer_key;
            transaction.DTTerminalLastModified = GetTimeZoneTime(timeZone);
            transaction.DTServerLastModified = DateTime.Now;
            transaction.TempMTCN = paymentTransaction.new_mtcn;
            transaction.MTCN = paymentTransaction.mtcn;
            transaction.OriginatingCurrencyCode = paymentTransaction.payment_details.originating_country_currency.iso_code.currency_code;
            transaction.DestinationCurrencyCode = paymentTransaction.payment_details.destination_country_currency.iso_code.currency_code;
            transaction.SenderName = senderName;
            transaction.OriginatingCountryCode = paymentTransaction.payment_details.originating_country_currency.iso_code.country_code;
            transaction.DestinationCountryCode = paymentTransaction.payment_details.destination_country_currency.iso_code.country_code;
            transaction.TestQuestion = securityQuestion;
            transaction.TestAnswer = securityAnswer;
            transaction.PersonalMessage = Convert.ToString(personalMessageBuilder);
            transaction.ExpectedPayoutStateCode = expectedLocationStateCode;
            transaction.ExpectedPayoutCityName = expectedLocationCityCode;
            transaction.OriginalDestinationCountryCode = paymentTransaction.payment_details.original_destination_country_currency.iso_code.country_code;
            transaction.OriginalDestinationCurrencyCode = paymentTransaction.payment_details.original_destination_country_currency.iso_code.currency_code;
            transaction.originating_city = originatingCity;
            transaction.ReceiverFirstName = string.IsNullOrEmpty(paymentTransaction.receiver.name.given_name) ? paymentTransaction.receiver.name.first_name : paymentTransaction.receiver.name.given_name;
            transaction.ReceiverLastName = string.IsNullOrEmpty(paymentTransaction.receiver.name.paternal_name) ? paymentTransaction.receiver.name.last_name : paymentTransaction.receiver.name.paternal_name;
            transaction.ReceiverSecondLastName = receiverNameType == ReceiveMoneySearch.name_type.M ? paymentTransaction.receiver.name.maternal_name : string.Empty;

            UpdateReceiveMoneyTransaction(transaction);
        }

        private ModifySendMoneyRequest GetSendMoneyModifyRequest(long wuTrxId)
        {
            ModifySendMoneyRequest modifyReq = null;

            StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_PopulateSendMoneyModifyRequest");

            moneyTransferProcedure.WithParameters(InputParameter.Named("WUTrxId").WithValue(wuTrxId));

            using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(moneyTransferProcedure))
            {
                while (datareader.Read())
                {
                    modifyReq = new ModifySendMoneyRequest();
                    modifyReq.Transaction = new WUTransaction()
                    {
                        OriginatorsPrincipalAmount = datareader.GetDecimalOrDefault("OriginatorsPrincipalAmount"),
                        OriginatingCountryCode = datareader.GetStringOrDefault("OriginatingCountryCode"),
                        OriginatingCurrencyCode = datareader.GetStringOrDefault("OriginatingCurrencyCode"),
                        TranascationType = datareader.GetStringOrDefault("TranascationType"),
                        PromotionsCode = datareader.GetStringOrDefault("PromotionsCode"),
                        ExchangeRate = datareader.GetDecimalOrDefault("ExchangeRate"),
                        DestinationPrincipalAmount = datareader.GetDecimalOrDefault("DestinationPrincipalAmount"),
                        GrossTotalAmount = datareader.GetDecimalOrDefault("GrossTotalAmount"),
                        Charges = datareader.GetDecimalOrDefault("Charges"),
                        TaxAmount = datareader.GetDecimalOrDefault("TaxAmount"),
                        MTCN = datareader.GetStringOrDefault("Mtcn"),
                        DTTerminalCreate = datareader.GetDateTimeOrDefault("DTTerminalCreate"),
                        DTTerminalLastModified = datareader.GetDateTimeOrDefault("DTTerminalLastModified"),
                        PromotionDiscount = datareader.GetDecimalOrDefault("PromotionDiscount"),
                        OtherCharges = datareader.GetDecimalOrDefault("OtherCharges"),
                        MoneyTransferKey = datareader.GetStringOrDefault("MoneyTransferKey"),
                        AdditionalCharges = datareader.GetDecimalOrDefault("AdditionalCharges"),
                        DestinationCountryCode = datareader.GetStringOrDefault("DestinationCountryCode"),
                        DestinationCurrencyCode = datareader.GetStringOrDefault("DestinationCurrencyCode"),
                        DestinationState = datareader.GetStringOrDefault("DestinationState"),
                        IsDomesticTransfer = datareader.GetBooleanOrDefault("IsDomesticTransfer"),
                        IsFixedOnSend = datareader.GetBooleanOrDefault("IsFixedOnSend"),
                        PhoneNumber = datareader.GetStringOrDefault("PhoneNumber"),
                        Url = datareader.GetStringOrDefault("Url"),
                        AgencyName = datareader.GetStringOrDefault("AgencyName"),
                        ChannelPartnerId = datareader.GetInt64OrDefault("ChannelPartnerId"),
                        ProviderId = datareader.GetInt32OrDefault("ProviderId"),
                        TestQuestion = datareader.GetStringOrDefault("TestQuestion"),
                        TempMTCN = datareader.GetStringOrDefault("TempMTCN"),
                        ExpectedPayoutStateCode = datareader.GetStringOrDefault("ExpectedPayoutStateCode"),
                        ExpectedPayoutCityName = datareader.GetStringOrDefault("ExpectedPayoutCityName"),
                        TestAnswer = datareader.GetStringOrDefault("TestAnswer"),
                        TestQuestionAvaliable = datareader.GetStringOrDefault("TestQuestionAvaliable"),
                        GCNumber = datareader.GetStringOrDefault("GCNumber"),
                        SenderName = datareader.GetStringOrDefault("SenderName"),
                        PdsRequiredFlag = datareader.GetBooleanOrDefault("PdsRequiredFlag"),
                        DfTransactionFlag = datareader.GetBooleanOrDefault("DfTransactionFlag"),
                        DeliveryServiceName = datareader.GetStringOrDefault("DeliveryServiceName"),
                        DTAvailableForPickup = datareader.GetDateTimeOrDefault("DTAvailableForPickup"),
                        DTServerCreate = datareader.GetDateTimeOrDefault("DTServerCreate"),
                        DTServerLastModified = datareader.GetDateTimeOrDefault("DTServerLastModified"),
                        ReceiverFirstName = datareader.GetStringOrDefault("RecieverFirstName"),
                        ReceiverLastName = datareader.GetStringOrDefault("RecieverLastName"),
                        ReceiverSecondLastName = datareader.GetStringOrDefault("RecieverSecondLastName"),
                        PromoCodeDescription = datareader.GetStringOrDefault("PromoCodeDescription"),
                        PromoName = datareader.GetStringOrDefault("PromoName"),
                        PromoMessage = datareader.GetStringOrDefault("PromoMessage"),
                        PromotionError = datareader.GetStringOrDefault("PromotionError"),
                        SenderComplianceDetailsComplianceDataBuffer = datareader.GetStringOrDefault("Sender_ComplianceDetails_ComplianceData_Buffer"),
                        recordingCountryCode = datareader.GetStringOrDefault("recordingCountryCode"),
                        recordingCurrencyCode = datareader.GetStringOrDefault("recordingCurrencyCode"),
                        originating_city = datareader.GetStringOrDefault("originating_city"),
                        originating_state = datareader.GetStringOrDefault("originating_state"),
                        municipal_tax = datareader.GetDecimalOrDefault("municipal_tax"),
                        state_tax = datareader.GetDecimalOrDefault("state_tax"),
                        county_tax = datareader.GetDecimalOrDefault("county_tax"),
                        plus_charges_amount = datareader.GetDecimalOrDefault("plus_charges_amount"),
                        message_charge = datareader.GetDecimalOrDefault("message_charge"),
                        total_undiscounted_charges = datareader.GetDecimalOrDefault("total_undiscounted_charges"),
                        total_discount = datareader.GetDecimalOrDefault("total_discount"),
                        total_discounted_charges = datareader.GetDecimalOrDefault("total_discounted_charges"),
                        instant_notification_addl_service_charges = datareader.GetStringOrDefault("instant_notification_addl_service_charges"),
                        PaySideCharges = datareader.GetDecimalOrDefault("PaySideCharges"),
                        PaySideTax = datareader.GetDecimalOrDefault("PaySideTax"),
                        AmountToReceiver = datareader.GetDecimalOrDefault("AmountToReceiver"),
                        SMSNotificationFlag = datareader.GetStringOrDefault("SMSNotificationFlag"),
                        PersonalMessage = datareader.GetStringOrDefault("PersonalMessage"),
                        DeliveryServiceDesc = datareader.GetStringOrDefault("DeliveryServiceDesc"),
                        ReferenceNo = datareader.GetStringOrDefault("ReferenceNo"),
                        pay_or_do_not_pay_indicator = datareader.GetStringOrDefault("pay_or_do_not_pay_indicator"),
                        OriginalDestinationCountryCode = datareader.GetStringOrDefault("OriginalDestinationCountryCode"),
                        OriginalDestinationCurrencyCode = datareader.GetStringOrDefault("OriginalDestinationCurrencyCode"),
                        FilingDate = datareader.GetStringOrDefault("FilingDate"),
                        FilingTime = datareader.GetStringOrDefault("FilingTime"),
                        PaidDateTime = datareader.GetStringOrDefault("PaidDateTime"),
                        AvailableForPickup = datareader.GetStringOrDefault("AvailableForPickup"),
                        DelayHours = datareader.GetStringOrDefault("DelayHours"),
                        AvailableForPickupEST = datareader.GetStringOrDefault("AvailableForPickupEST"),
                        WuCardTotalPointsEarned = datareader.GetStringOrDefault("WUCard_TotalPointsEarned"),
                        OriginalTransactionID = datareader.GetInt64OrDefault("OriginalTransactionID"),
                        TransactionSubType = datareader.GetStringOrDefault("TransactionSubType"),
                        ReasonCode = datareader.GetStringOrDefault("ReasonCode"),
                        ReasonDescription = datareader.GetStringOrDefault("ReasonDescription"),
                        Comments = datareader.GetStringOrDefault("Comments"),
                        DeliveryOption = datareader.GetStringOrDefault("DeliveryOption"),
                        DeliveryOptionDesc = datareader.GetStringOrDefault("DeliveryOptionDesc"),
                        PromotionSequenceNo = datareader.GetStringOrDefault("PromotionSequenceNo"),
                        CounterId = datareader.GetStringOrDefault("CounterId"),
                        Principal_Amount = datareader.GetDecimalOrDefault("PrincipalAmount"),
                        Receiver_unv_Buffer = datareader.GetStringOrDefault("Receiver_unv_Buffer"),
                        Sender_unv_Buffer = datareader.GetStringOrDefault("Sender_unv_Buffer"),
                        TransalatedDeliveryServiceName = datareader.GetStringOrDefault("TransalatedDeliveryServiceName"),
                        MessageArea = datareader.GetStringOrDefault("MessageArea"),
                        Id = datareader.GetInt64OrDefault("TransactionID"),
                        WUAccountId = datareader.GetInt64OrDefault("WUAccountID"),
                        ReceiverId = datareader.GetInt64OrDefault("WUReceiverID")
                    };

                    modifyReq.Account = new WUAccount()
                    {
                        Address = datareader.GetStringOrDefault("Address"),
                        City = datareader.GetStringOrDefault("City"),
                        State = datareader.GetStringOrDefault("State"),
                        FirstName = datareader.GetStringOrDefault("FirstName"),
                        LastName = datareader.GetStringOrDefault("LastName"),
                        MiddleName = datareader.GetStringOrDefault("MiddleName"),
                        SecondLastName = datareader.GetStringOrDefault("SecondLastName"),
                        PostalCode = datareader.GetStringOrDefault("PostalCode"),
                        ContactPhone = datareader.GetStringOrDefault("ContactPhone"),
                        MobilePhone = datareader.GetStringOrDefault("MobilePhone")
                    };
                    modifyReq.Receiver = new Receiver()
                    {
                        FirstName = datareader.GetStringOrDefault("ReceiverFirstName"),
                        LastName = datareader.GetStringOrDefault("ReceiverLastName"),
                        SecondLastName = datareader.GetStringOrDefault("ReceiverSecondLastName"),
                        City = datareader.GetStringOrDefault("PickupCity"),
                        PickupCountry = datareader.GetStringOrDefault("PickupCountry")
                    };
                }

            }

            return modifyReq;
        }

        private void CheckCounterId(string wUCounterId)
        {
            if (!IsHardCodedCounterId && string.IsNullOrEmpty(wUCounterId))
            {
                throw new MoneyTransferException(MoneyTransferException.COUNTERID_NOT_FOUND);
            }
        }

        private void ImportReceivers(List<WUData.ImportReceiver> importReceivers, ZeoContext context)
        {
            DataTable receiverTable = GetImportReceiversToTable(importReceivers, context);
            if (importReceivers.Count > 0)
            {
                StoredProcedure importReceiverProc = new StoredProcedure("usp_ImportReceivers");
                StringWriter writer = new StringWriter();

                receiverTable.TableName = "Receivers";
                receiverTable.WriteXml(writer);
                DataParameter[] dataParameters = new DataParameter[]
                {
                        new DataParameter("pastReceivers", DbType.Xml)
                        {
                            Value = writer.ToString()
                        }
                };

                importReceiverProc.WithParameters(dataParameters);

                DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(importReceiverProc);
            }
        }

        private DataTable GetImportReceiversToTable(List<WUData.ImportReceiver> importReceivers, ZeoContext context)
        {
            DataTable table = new DataTable();
            table.Columns.Add("NameType", typeof(string));
            table.Columns.Add("FirstName", typeof(string));
            table.Columns.Add("LastName", typeof(string));
            table.Columns.Add("SecondLastName", typeof(string));
            table.Columns.Add("ReceiverIndexNo", typeof(string));
            table.Columns.Add("Address", typeof(string));
            table.Columns.Add("CountryCode", typeof(string));
            table.Columns.Add("Status", typeof(string));
            table.Columns.Add("PickupCountry", typeof(string));
            table.Columns.Add("CustomerId", typeof(long));
            table.Columns.Add("GoldCardNumber", typeof(string));
            table.Columns.Add("DTTerminalDate", typeof(DateTime));
            table.Columns.Add("DTServerDate", typeof(DateTime));

            foreach (WUData.ImportReceiver receiver in importReceivers)
            {
                DataRow dr = table.NewRow();
                dr["NameType"] = receiver.NameType;
                dr["FirstName"] = receiver.FirstName;
                dr["LastName"] = receiver.LastName;
                dr["SecondLastName"] = receiver.SecondLastName;
                dr["ReceiverIndexNo"] = receiver.ReceiverIndexNumber;
                // dr["Address"] = receiver.Address;
                dr["CountryCode"] = receiver.CountryCode;
                dr["Status"] = receiver.Status;
                dr["PickupCountry"] = receiver.PickupCountry;
                dr["CustomerId"] = receiver.CustomerId;
                dr["GoldCardNumber"] = receiver.GoldCardNumber;
                dr["DTTerminalDate"] = GetTimeZoneTime(context.TimeZone);
                dr["DTServerDate"] = DateTime.Now;
                table.Rows.Add(dr);
            }

            return table;

        }

        private static IWUCommonIO GetCommonProcessor()
        {
            string commonProcessor = ConfigurationManager.AppSettings["MTProcessor"].ToString();

            if (commonProcessor.ToUpper() == "IO")
                return new TCF.Zeo.Cxn.WU.Common.Impl.IO();
            else
                return new TCF.Zeo.Cxn.WU.Common.Impl.SimulatorIO();
        }

        private static IIO GetMTProcessor()
        {
            string commonProcessor = ConfigurationManager.AppSettings["MTProcessor"].ToString();

            if (commonProcessor.ToUpper() == "IO")
                return new IO();
            else
                return new SimulatorIO();

        }

        public void UpdateMoneyTransferGoldCardPoints(long transactionId, string cardNumber, ZeoContext context)
        {
            context.RMTrxType = context.SMTrxType = Helper.RequestType.Release.ToString();
            WUCommonIO = GetCommonProcessor();
            UpdateBillPayGoldCardPoints(transactionId, GetCardPoints(cardNumber, context), (int)Helper.ProductCode.MoneyTransfer);
        }

        public decimal GetFraudLimit(string countryCode, ZeoContext context)
        {
            try
            {
                decimal fraudLimit = 0;
                StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_GetFraudLimit");
                moneyTransferProcedure.WithParameters(InputParameter.Named("countryCode").WithValue(countryCode));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(moneyTransferProcedure))
                {
                    while (datareader.Read())
                    {
                        fraudLimit = datareader.GetDecimalOrDefault("FraudLimit");
                    }

                }
                return fraudLimit;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETFRAUDLIMIT_FAILED, ex);
            }
        }

        public decimal GetDestinationAmount(FeeRequest feeRequest, ZeoContext context)
        {
            FeeInquiry.feeinquiryrequest feeInquiryRequest = BuildFeeEnquiryRequest(feeRequest);
            WUCommonIO = GetCommonProcessor();
            WUIO = GetMTProcessor();
            feeInquiryRequest.swb_fla_info = Mapper.Map<WUCommonData.SwbFlaInfo, FeeInquiry.swb_fla_info>(WUCommonIO.BuildSwbFlaInfo(context));
            feeInquiryRequest.swb_fla_info.fla_name = Mapper.Map<WUCommonData.GeneralName, FeeInquiry.general_name>(WUCommonIO.BuildGeneralName(context));
            decimal destinationAmount = decimal.Zero;

            try
            {
                FeeInquiry.feeinquiryreply reply = WUIO.FeeInquiry(feeInquiryRequest, context);
                if (reply != null && reply.financials != null)
                {
                    destinationAmount = ConvertLongToDecimal(reply.financials.destination_principal_amount);
                }

                return destinationAmount;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GETFRAUDLIMIT_FAILED, ex);
            }

        }

        private bool CheckCountryBlockedInZEO(string countryCode)
        {
             bool isBlocked = false;
             StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_CheckIsCountryBlocked");
             moneyTransferProcedure.WithParameters(InputParameter.Named("countryCode").WithValue(countryCode));
             
             using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(moneyTransferProcedure))
             {
                 while (datareader.Read())
                 {
                     isBlocked = datareader.GetBooleanOrDefault("IsBlocked");
                 }
             
             }
             return isBlocked;
        }

        public List<MasterData> GetUnblockedCountries(ZeoContext context)
        {
            try
            {
                List<MasterData> masterCountries = new List<MasterData>();

                StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_GetUnBlockedCountries");

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(moneyTransferProcedure))
                {
                    while (datareader.Read())
                    {
                        masterCountries.Add(new MasterData
                        {
                            Code = datareader.GetStringOrDefault("ISOCountryCode"),
                            Name = datareader.GetStringOrDefault("CountryName")
                        });
                    }
                }

                return masterCountries;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.GET_UNBLOCKED_COUNTRIES_FAILED, ex);
            }
        }
    }
}
