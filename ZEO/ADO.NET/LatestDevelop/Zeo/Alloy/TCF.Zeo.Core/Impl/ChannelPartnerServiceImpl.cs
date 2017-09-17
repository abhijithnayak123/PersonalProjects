using TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Data;
using TCF.Zeo.Core.Contract;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using P3Net.Data.Common;
using P3Net.Data;
using static TCF.Zeo.Common.Util.Helper;
using TCF.Zeo.Core.Data.Exceptions;

namespace TCF.Zeo.Core.Impl
{
    public class ChannelPartnerServiceImpl : IChannelPartnerService
    {
        /// <summary>
        /// This method is to get the collection of channel partner group details by channel partner name.
        /// </summary>
        /// <param name="channelPartner"></param>
        /// <returns></returns>
        public List<string> GetGroups(long channelPartnerId, ZeoContext context)
        {
            try
            {
                List<string> groups = new List<string>();

                StoredProcedure coreCustomerProcedure = new StoredProcedure("usp_GetGroupsByChannelPartner");

                coreCustomerProcedure.WithParameters(InputParameter.Named("ChannelPartnerId").WithValue(channelPartnerId));
                coreCustomerProcedure.WithParameters(InputParameter.Named("DTToday").WithValue(DateTime.Now));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(coreCustomerProcedure))
                {
                    while (datareader.Read())
                    {
                        groups.Add(datareader.GetStringOrDefault("Name"));
                    }
                }

                return groups;
            }
            catch (Exception ex)
            {
                throw new ChannelPartnerExceptions(ChannelPartnerExceptions.CHANNEL_PARTNER_GROUP_GET_FAILED, ex);
            }
        }

        public ChannelPartner ChannelPartnerConfig(long channelPartnerId, ZeoContext context)
        {
            try
            {
                ChannelPartner partner = null;
                StoredProcedure procedureToBeExecuted = new StoredProcedure("usp_GetChannelPartnerByChannelPartnerIdOrName");
                procedureToBeExecuted.WithParameters(InputParameter.Named("channelPartnerId").WithValue(channelPartnerId));

                using (IDataReader dataReader = DataHelper.GetConnectionManager().ExecuteReader(procedureToBeExecuted))
                {
                    partner = GetChannelPartnerDetails(dataReader);
                }

                return partner;
            }
            catch (Exception ex)
            {
                throw new ChannelPartnerExceptions(ChannelPartnerExceptions.CHANNEL_PARTNER_CONFIG_GET_FAILED, ex);
            }
        }

        public ChannelPartner ChannelPartnerConfig(string channelPartner, ZeoContext context)
        {
            try
            {
                ChannelPartner partner = null;
                StoredProcedure procedureToBeExecuted = new StoredProcedure("usp_GetChannelPartnerByChannelPartnerIdOrName");
                procedureToBeExecuted.WithParameters(InputParameter.Named("channelPartnerName").WithValue(channelPartner));

                using (IDataReader dataReader = DataHelper.GetConnectionManager().ExecuteReader(procedureToBeExecuted))
                {
                    partner = GetChannelPartnerDetails(dataReader);
                }

                return partner;
            }
            catch (Exception ex)
            {
                throw new ChannelPartnerExceptions(ChannelPartnerExceptions.CHANNEL_PARTNER_CONFIG_GET_FAILED, ex);
            }
        }

        public List<ChannelPartnerProductProvider> GetProvidersbyChannelPartnerName(string channelPartnerName, ZeoContext context)
        {
            try
            {
                List<ChannelPartnerProductProvider> channelPartnerProductProviders = new List<ChannelPartnerProductProvider>();
                StoredProcedure procedureToBeExecuted = new StoredProcedure("usp_GetProvidersByChannelPartnerIdOrName");
                procedureToBeExecuted.WithParameters(InputParameter.Named("channelPartnerName").WithValue(channelPartnerName));

                using (IDataReader dataReader = DataHelper.GetConnectionManager().ExecuteReader(procedureToBeExecuted))
                {
                    while (dataReader.Read())
                    {
                        ChannelPartnerProductProvider provider = new ChannelPartnerProductProvider
                        {
                            Sequence = dataReader.GetInt32OrDefault("Sequence"),
                            IsTnCForcePrintRequired = dataReader.GetBooleanOrDefault("IsTnCForcePrintRequired"),
                            MinimumTransactAge = dataReader.GetInt32OrDefault("MinimumTransactAge"),
                            CheckEntryType = (CheckEntryTypes)dataReader.GetInt16OrDefault("CheckEntryType"),
                            IsSSNRequired = dataReader.GetBooleanOrDefault("IsSSNRequired"),
                            IsSWBRequired = dataReader.GetBooleanOrDefault("IsSWBRequired"),
                            CanParkReceiveMoney = dataReader.GetBooleanOrDefault("CanParkReceiveMoney"),
                            ReceiptCopies = dataReader.GetInt32OrDefault("ReceiptCopies"),
                            ReceiptReprintCopies = dataReader.GetInt32OrDefault("ReceiptReprintCopies"),
                            ProductName = dataReader.GetStringOrDefault("ProductName"),
                            ProcessorName = dataReader.GetStringOrDefault("ProcessorName"),
                            ProcessorId = dataReader.GetInt64OrDefault("Code")
                        };
                        channelPartnerProductProviders.Add(provider);
                    }
                }

                return channelPartnerProductProviders;
            }
            catch (Exception ex)
            {
                throw new ChannelPartnerExceptions(ChannelPartnerExceptions.CHANNEL_PARTNER_PROVIDERS_GET_FAILED, ex);
            }
        }


        public List<TipsAndOffers> GetTipsAndOffers(long channelPartnerId, string lang, string viewName, ZeoContext context)
        {
            try
            {
                List<TipsAndOffers> tipsAndOffers = new List<TipsAndOffers>();

                StoredProcedure procedureToBeExecuted = new StoredProcedure("usp_GetTipsAndOffersByChannelPartnerAndViewName");
                procedureToBeExecuted.WithParameters(InputParameter.Named("ChannelPartnerId").WithValue(channelPartnerId));
                procedureToBeExecuted.WithParameters(InputParameter.Named("ViewName").WithValue(viewName));
                procedureToBeExecuted.WithParameters(InputParameter.Named("Lang").WithValue(lang));

                using (IDataReader dataReader = DataHelper.GetConnectionManager().ExecuteReader(procedureToBeExecuted))
                {
                    while (dataReader.Read())
                    {
                        tipsAndOffers.Add(new TipsAndOffers
                        {
                            TipsAndOffersValue = dataReader.GetStringOrDefault("TipsAndOffersValue"),
                            OptionalFilter = dataReader.GetStringOrDefault("OptionalFilter")
                        });
                    }
                }

                return tipsAndOffers;
            }
            catch (Exception ex)
            {
                throw new ChannelPartnerExceptions(ChannelPartnerExceptions.TIPS_AND_OFFERS_GET_FAILED, ex);
            }
        }


        public ChannelPartnerCertificate GetChannelPartnerCertificateInfo(long channelPartnerId, string issuer, ZeoContext context)
        {
            ChannelPartnerCertificate channelPartnerCertificate = null;
            try
            {
                StoredProcedure procedureToBeExecuted = new StoredProcedure("usp_GetChannelPartnerCertificateByPartnerIdAndIssuerName");
                procedureToBeExecuted.WithParameters(InputParameter.Named("ChannelPartnerId").WithValue(channelPartnerId));
                procedureToBeExecuted.WithParameters(InputParameter.Named("IssuerName").WithValue(issuer));

                using (IDataReader dataReader = DataHelper.GetConnectionManager().ExecuteReader(procedureToBeExecuted))
                {
                    while (dataReader.Read())
                    {
                        channelPartnerCertificate = new ChannelPartnerCertificate
                        {
                            ChannelPartnerCertificateId = dataReader.GetInt64OrDefault("ChannelPartnerCertificateId"),
                            Issuer = dataReader.GetStringOrDefault("Issuer"),
                            ThumbPrint = dataReader.GetStringOrDefault("ThumbPrint"),
                            ChannelPartner = new ChannelPartner { Id = dataReader.GetInt32OrDefault("ChannelPartnerId") }
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ChannelPartnerExceptions(ChannelPartnerExceptions.CHANNEL_PARTNER_CERTIFICATE_INFO_GET_FAILED, ex);
            }

            return channelPartnerCertificate;
        }
        private static string GetAuthenticationType(bool HasNonGPRCard, bool AllowPhoneNumberAuthentication)
        {
            string authenticationType = (HasNonGPRCard && AllowPhoneNumberAuthentication) ?
                                                 "MembershipCardAndPhone" : (HasNonGPRCard) ?
                                                 "MembershipCard" : (AllowPhoneNumberAuthentication) ?
                                                 "Phone" : "GPRCard";
            return authenticationType;
        }

        private ChannelPartner GetChannelPartnerDetails(IDataReader dataReader)
        {
            ChannelPartner channelPartner = null;

            while (dataReader.Read())
            {
                channelPartner = new ChannelPartner
                {
                    Id = dataReader.GetInt64OrDefault("ChannelPartnerId"),
                    DisableWithdrawCNP = dataReader.GetBooleanOrDefault("DisableWithdrawCNP"),
                    CashOverCounter = dataReader.GetBooleanOrDefault("CashOverCounter"),
                    FrankData = dataReader.GetStringOrDefault("FrankData"),
                    IsCheckFrank = dataReader.GetBooleanOrDefault("IsCheckFrank"),
                    IsNotesEnable = dataReader.GetBooleanOrDefault("IsNotesEnable"),
                    IsReferralSectionEnable = dataReader.GetBooleanOrDefault("IsReferralSectionEnable"),
                    IsMGIAlloyLogoEnable = dataReader.GetBooleanOrDefault("IsMGIAlloyLogoEnable"),
                    MasterSSN = dataReader.GetStringOrDefault("MasterSSN"),
                    IsMailingAddressEnable = dataReader.GetBooleanOrDefault("IsMailingAddressEnable"),
                    CanEnableProfileStatus = dataReader.GetBooleanOrDefault("CanEnableProfileStatus"),
                    CustomerMinimumAge = dataReader.GetInt32OrDefault("CustomerMinimumAge"),
                    Name = dataReader.GetStringOrDefault("Name"),
                    FeesFollowCustomer = dataReader.GetBooleanOrDefault("FeesFollowCustomer"),
                    CashFeeDescriptionEN = dataReader.GetStringOrDefault("CashFeeDescriptionEN"),
                    CashFeeDescriptionES = dataReader.GetStringOrDefault("CashFeeDescriptionES"),
                    DebitFeeDescriptionEN = dataReader.GetStringOrDefault("DebitFeeDescriptionEN"),
                    DebitFeeDescriptionES = dataReader.GetStringOrDefault("DebitFeeDescriptionES"),
                    ConvenienceFeeCash = dataReader.GetDecimalOrDefault("ConvenienceFeeCash"),
                    ConvenienceFeeDebit = dataReader.GetDecimalOrDefault("ConvenienceFeeDebit"),
                    ConvenienceFeeDescriptionEN = dataReader.GetStringOrDefault("ConvenienceFeeDescriptionEN"),
                    ConvenienceFeeDescriptionES = dataReader.GetStringOrDefault("ConvenienceFeeDescriptionES"),
                    CanCashCheckWOGovtId = dataReader.GetBooleanOrDefault("CanCashCheckWOGovtId"),
                    LogoFileName = dataReader.GetStringOrDefault("LogoFileName"),
                    IsEFSPartner = dataReader.GetBooleanOrDefault("IsEFSPartner"),
                    UsePINForNonGPR = dataReader.GetBooleanOrDefault("UsePINForNonGPR"),
                    IsCUPartner = dataReader.GetBooleanOrDefault("IsCUPartner"),
                    HasNonGPRCard = dataReader.GetBooleanOrDefault("HasNonGPRCard"),
                    ManagesCash = dataReader.GetBooleanOrDefault("ManagesCash"),
                    AllowPhoneNumberAuthentication = dataReader.GetBooleanOrDefault("AllowPhoneNumberAuthentication"),
                    TIM = dataReader.GetInt16OrDefault("TIM"),
                    ComplianceProgramName = dataReader.GetStringOrDefault("ComplianceProgramName"),
                    CardPresenceVerificationConfig = dataReader.GetInt32OrDefault("CardPresenceVerificationConfig"),
                    AuthenticationType = GetAuthenticationType(dataReader.GetBooleanOrDefault("HasNonGPRCard"), dataReader.GetBooleanOrDefault("AllowPhoneNumberAuthentication"))
                };
            }

            return channelPartner;
        }
    }
}
