using AutoMapper;
using TCF.Zeo.Cxn.MoneyTransfer.WU.Data;
using TCF.Zeo.WU.DASMonitor.WUService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using TCF.Zeo.Cxn.WU.Common.Data;
using System.Text.RegularExpressions;
using HeartBeatSvc = TCF.Zeo.WU.DASMonitor.HeartBeatService;
using P3Net.Data.Common;
using System.Data;
using TCF.Zeo.Common.Data;
using P3Net.Data;
using System.IO;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Cxn.MoneyTransfer.Data;

namespace TCF.Zeo.WU.DASMonitor
{
    public class DASServiceImpl : IDASService
    {
        private const string Spanish = "es";
        private const string English = "en";

        channel channel = null;
        foreign_remote_system remotesystem = null;
        X509Certificate2 wuCertificate = null;
        static long channelPartnerID = Convert.ToInt32(ConfigurationManager.AppSettings.Get("ChannelPartnerID"));
        static int ProviderID = Convert.ToInt16(ConfigurationManager.AppSettings.Get("ProviderID"));
        string _wUServiceURL = ConfigurationManager.AppSettings.Get("WUServiceURL");
        static string BillerImportPartnerIDs = ConfigurationManager.AppSettings.Get("BillerImportPartnerIDs");
        IMapper mapper;

        #region Constructor

        public DASServiceImpl()
        {
            #region Auto Mapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WUCountry, CountryTransalation>()
                 .AfterMap((s, d) =>
                 {
                     d.DTServerCreate = DateTime.Now;
                 })
                 .ForMember(c => c.CountryCode, o => o.MapFrom(s => s.CountryCode))
                 .ForMember(c => c.Name, o => o.MapFrom(s => s.Name));
                cfg.CreateMap<DASDELIVERYTRANSLATE_Type, DeliveryServiceTransalation>()
                .AfterMap((s, d) =>
                {
                    d.DTServerCreate = DateTime.Now;
                })
                .ForMember(c => c.EnglishName, o => o.MapFrom(s => s.ENGLISH_MESSAGE))
                .ForMember(c => c.Name, o => o.MapFrom(s => s.TRANSL_MESSAGE));

                cfg.CreateMap<ISOCOUNTRY_Type, WUCountry>()
                .AfterMap((s, d) =>
                {
                    d.DTServerCreate = DateTime.Now;
                })
                .ForMember(c => c.CountryCode, o => o.MapFrom(s => s.ISO_COUNTRY_CD))
                .ForMember(c => c.Name, o => o.MapFrom(s => s.COUNTRY_LONG));

                cfg.CreateMap<ISOCURRENCY_Type, WUCountryCurrency>()
                .ForMember(c => c.CountryCode, o => o.MapFrom(s => s.CURRENCY_CD.ToUpper()))
                .ForMember(c => c.CurrencyCode, o => o.MapFrom(s => s.CURRENCY_NAME.ToUpper()));

                cfg.CreateMap<USSTATEINFO_Type, WUState>()
                .AfterMap((s, d) =>
                {
                    d.ISOCountryCode = "US";
                    d.DTServerCreate = DateTime.Now;
                })
                .ForMember(c => c.StateCode, o => o.MapFrom(s => s.STATE_CODE.ToUpper()))
                .ForMember(c => c.Name, o => o.MapFrom(s => s.STATE_NAME.ToUpper()));

                cfg.CreateMap<MEXICOCITYSTATEINFO_Type, WUState>()
                .AfterMap((s, d) =>
                {
                    d.ISOCountryCode = "MX";
                    d.DTServerCreate = DateTime.Now;
                })
                .ForMember(c => c.StateCode, o => o.MapFrom(s => s.STATE_CODE.ToUpper()))
                .ForMember(c => c.Name, o => o.MapFrom(s => s.STATE_NAME.ToUpper())
                );

                cfg.CreateMap<QQCCOMPANYNAME_Type, WUQQCcompanyNames>()
                .AfterMap((s, d) =>
                {
                    d.DTServerCreate = DateTime.Now;
                    d.IsActive = true;
                    d.ChannelPartnerId = channelPartnerID;
                })
                .ForMember(c => c.CompanyName, o => o.MapFrom(s => s.COMPANY_NAME.ToUpper()))
                .ForMember(c => c.Country, o => o.MapFrom(s => s.COUNTRY.ToUpper()))
                .ForMember(c => c.CurrencyCode, o => o.MapFrom(s => s.CURRENCY_CD.ToUpper()))
                .ForMember(c => c.ISOCountryCode, o => o.MapFrom(s => s.ISO_COUNTRY_CD.ToUpper()));


                cfg.CreateMap<COUNTRY_CURRENCY_Type, WUCountryCurrency>()
                    .AfterMap((s, d) =>
                    {
                        d.DTServerCreate = DateTime.Now;
                    })
                    .ForMember(c => c.CountryCode, o => o.MapFrom(s => s.ISO_COUNTRY_CD.ToUpper()))
                    .ForMember(c => c.CountryName, o => o.MapFrom(s => s.COUNTRY_LONG.ToUpper()))
                    .ForMember(c => c.CountryNumCode, o => o.MapFrom(s => s.ISO_COUNTRY_NUM_CD.ToUpper()))
                    .ForMember(c => c.CurrencyCode, o => o.MapFrom(s => s.CURRENCY_CD.ToUpper()))
                    .ForMember(c => c.CurrencyName, o => o.MapFrom(s => s.CURRENCY_NAME.ToUpper()))
                    .ForMember(c => c.CurrencyNumCode, o => o.MapFrom(s => s.ISO_CURRENCY_NUM_CD.ToUpper())
                    );
                cfg.CreateMap<ERRORINFO_Type, WUErrorMessages>()
                .AfterMap((s, d) =>
                {
                    d.DTServerCreate = DateTime.Now;
                })
                .ForMember(c => c.ErrorCode, o => o.MapFrom(s => s.ERROR_CODE.ToUpper()))
                .ForMember(c => c.ErrorDesc, o => o.MapFrom(s => s.ERROR_DESC.ToUpper()));

                cfg.CreateMap<MEXICOCITYSTATEINFO_Type, WUCity>()
            .AfterMap((s, d) =>
            {
                d.DTServerCreate = DateTime.Now;
            })
            .ForMember(c => c.StateCode, o => o.MapFrom(s => s.STATE_CODE.ToUpper()))
            .ForMember(c => c.Name, o => o.MapFrom(s => s.CITY.ToUpper()));

                cfg.CreateMap<WUQQCcompanyNames, WUMasterCatalog>()
            .AfterMap((s, d) =>
            {
                d.DTServerCreate = DateTime.Now;
                d.ProviderId = ProviderID;
            })
            .ForMember(c => c.BillerName, o => o.MapFrom(s => s.CompanyName))
            .ForMember(c => c.ChannelPartnerId, o => o.MapFrom(s => s.ChannelPartnerId))
            .ForMember(c => c.IsActive, o => o.MapFrom(s => s.IsActive))
            .ForMember(c => c.ProviderCatalogId, o => o.MapFrom(s => s.Id));

                cfg.CreateMap<WUMasterCatalog, WUPartnerCatalog>()
            .AfterMap((s, d) =>
            {
                d.DTServerCreate = DateTime.Now;
            })
                .ForMember(c => c.BillerName, o => o.MapFrom(s => s.BillerName))
                .ForMember(c => c.ChannelPartnerId, o => o.MapFrom(s => s.ChannelPartnerId))
                .ForMember(c => c.ProviderId, o => o.MapFrom(s => s.ProviderId))
                .ForMember(c => c.Id, o => o.MapFrom(s => s.Id));
            });
            mapper = config.CreateMapper();
            #endregion
        }

        #endregion

        #region Public Methods

        public void CheckHeartBeat()
        {
            try
            {
                Console.WriteLine("==============" + "Check HearBeat Initiated." + "=========================");
                Logger.WriteLogHeartBeat("Check HearBeat Initiated.");

                WUCredential wuCredentials = new WUCredential();
                HeartBeatSvc.HeartBeatPortTypeClient hc;
                wuCredentials = GetWUCredential(channelPartnerID);
                ConfigureWUOject(wuCredentials);
                string url;
                if (_wUServiceURL == string.Empty)
                {
                    hc = new HeartBeatSvc.HeartBeatPortTypeClient("HeartBeat_SOAP_HTTP_Port", wuCredentials.WUServiceUrl.ToString());
                    url = wuCredentials.WUServiceUrl.ToString();
                }
                else
                {
                    hc = new HeartBeatSvc.HeartBeatPortTypeClient("HeartBeat_SOAP_HTTP_Port", _wUServiceURL);
                    url = _wUServiceURL;
                }
                hc.ClientCredentials.ClientCertificate.Certificate = wuCertificate;
                HeartBeatSvc.espheartbeatrequest request = new HeartBeatSvc.espheartbeatrequest();
                HeartBeatSvc.espheartbeatreply response = new HeartBeatSvc.espheartbeatreply();
                request.external_reference_no = DateTime.Now.ToString("yyyyMMddhhmmssff");
                response = hc.HeartBeatService(request);
                Logger.WriteLogHeartBeat("WebService URL :" + url + "\r\n" + "\t\t      " + "HeartBeat Strength:" + response.status.message.ToString());
            }
            catch (Exception ex)
            {
                Logger.WriteLogHeartBeat("DASMonitor Exception -  HeartBeat checking:" + ex.Message);
            }

        }

        public void PopulateWUCountries()
        {
            try
            {
                Console.WriteLine("==============" + "Populating WU Countries Initiated." + "=========================");
                Logger.WriteLog("==============" + "Populating WU Countries Initiated." + "===============================================");
                List<WUCountry> wuCountries = GetDestinationCountries(Convert.ToInt64(DateTime.Now.ToString("yyyyMMddhhmmssff")), English);

                if (wuCountries.Count > 0)
                {
                    populateData<WUCountry>(wuCountries, "usp_PopulateWUCountries", "Countries");
                }

                Logger.WriteLog("==============" + "Populating WU Countries Completed." + "===============================================");
                Console.WriteLine("==============" + "Populating WU Countries Completed." + "=========================");
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PopulateWUCountries failed " + ex.Message);
            }
        }

        public void PopulateWUCountryStates()
        {
            try
            {
                Console.WriteLine("==============" + "Populating WU States Initiated." + "=========================");
                Logger.WriteLog("==============" + "Populating WU States Initiated." + "===============================================");

                List<WUState> usStates = GetUSStates(Convert.ToInt64(DateTime.Now.ToString("yyyyMMddhhmmssff")));
                List<WUState> mxStates = GetMexicoCityState(Convert.ToInt64(DateTime.Now.ToString("yyyyMMddhhmmssff")));

                if (usStates.Count() > 0 & mxStates.Count() > 0)
                {
                    usStates.AddRange(mxStates);
                    populateData<WUState>(usStates, "usp_PopulateWUCountryStates", "States");
                }

                Logger.WriteLog("==============" + "Populating WU States Completed." + "===============================================");
                Console.WriteLine("==============" + "Populating WU States Completed." + "=========================");

            }
            catch (Exception ex)
            {
                Logger.WriteLog("PopulateWUCountryStates failed " + ex.Message);
            }
        }

        public void PopulateWUMexxicoCities()
        {
            try
            {
                Console.WriteLine("==============" + "Populating WU MexxicoCities Initiated." + "=========================");
                Logger.WriteLog("==============" + "Populating WU MexxicoCities Initiated." + "===============================================");

                List<WUCity> mxcities = GetMexicoCities(Convert.ToInt64(DateTime.Now.ToString("yyyyMMddhhmmssff")));
                if (mxcities.Count() > 0)
                {
                    populateData<WUCity>(mxcities, "usp_PopulateWUMexxicoCities", "City");
                }

                Logger.WriteLog("==============" + "Populating WU MexxicoCities Completed." + "===============================================");
                Console.WriteLine("==============" + "Populating WU MexxicoCities Completed." + "=========================");
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PopulateWUMexxicoCity failed " + ex.Message);
            }
        }

        public void PopulateWUQQCompanyNames()
        {
            try
            {
                Console.WriteLine("==============" + "Populating Mastercatalog and PartnerCatalog Initiated." + "=========================");
                Logger.WriteLog("==============" + "Populating Mastercatalog and PartnerCatalog Initiated." + "===============================================");

                List<WUQQCcompanyNames> wuBillers = GetBillers(Convert.ToInt64(DateTime.Now.ToString("yyyyMMddhhmmssff")));
                if (wuBillers.Count != 0)
                {
                    //Populating Catalog table.
                    try
                    {
                        populateData<WUQQCcompanyNames>(wuBillers, "usp_PopulateBillerDetails", "Billers");
                        Logger.WriteLog("==============" + "Populating tWUnion_Catalog  Succeeded.");

                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog("==============" + "Populating tWUnion_Catalog failed - " + ex.Message);
                    }
                    //Populating Mastercatalog and PartnerCatalog.						
                    try
                    {
                        string[] partnerIDs = BillerImportPartnerIDs.Split(',');
                        foreach (string partnerid in partnerIDs)
                        {
                            populateCatalog(partnerid);
                        }
                        Logger.WriteLog("==============" + "Populating Mastercatalog and PartnerCatalog  Succeeded.");

                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog("==============" + "Populating Mastercatalog and PartnerCatalog  failed - " + ex.Message);
                    }
                }

                Logger.WriteLog("==============" + "Populating Mastercatalog and PartnerCatalog Completed." + "===============================================");
                Console.WriteLine("==============" + "Populating Mastercatalog and PartnerCatalog Completed." + "=========================");
            }
            catch (Exception ex)
            {
                Logger.WriteLog("==============" + "PopulateWUQQCompanyNames failed " + ex.Message);
            }
        }

        public void PopulateWUCountriesCurrencies()
        {
            try
            {
                Console.WriteLine("==============" + "Populating WU CountriesCurrencies Initiated." + "=========================");
                Logger.WriteLog("==============" + "Populating WU CountriesCurrencies Initiated." + "===============================================");

                var transactionId = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddhhmmssff"));
                List<WUCountryCurrency> wuCountryCurrencies = GetCountriesCurrencies(Convert.ToInt64(DateTime.Now.ToString("yyyyMMddhhmmssff")));

                UpdateFraudLimits(transactionId, ref wuCountryCurrencies);

                if (wuCountryCurrencies.Count > 0)
                {
                    populateData<WUCountryCurrency>(wuCountryCurrencies, "usp_PopulateWUCountriesCurrency", "Currency");
                }

                Logger.WriteLog("==============" + "Populating WU CountriesCurrencies Completed." + "===============================================");
                Console.WriteLine("==============" + "Populating WU CountriesCurrencies Completed." + "=========================");
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PopulateWUCountriesCurrencies failed " + ex.Message);
            }
        }

        public void PopulateWUErrorMessagesInfo()
        {
            try
            {
                Console.WriteLine("==============" + "Populating WU ErrorMessagesInfo Initiated." + "=========================");
                Logger.WriteLog("==============" + "Populating WU ErrorMessagesInfo Initiated." + "===============================================");

                List<WUErrorMessages> wuerrormessages = GetErrorMesages(Convert.ToInt64(DateTime.Now.ToString("yyyyMMddhhmmssff")));

                if (wuerrormessages.Count > 0)
                {
                    populateData<WUErrorMessages>(wuerrormessages, "usp_PopulateWUErrorMessages", "Messages");
                }

                Logger.WriteLog("==============" + "Populating WU ErrorMessagesInfo Completed." + "===============================================");
                Console.WriteLine("==============" + "Populating WU ErrorMessagesInfo Completed." + "=========================");
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PopulateWUGetErrorMessagesInfo failed " + ex.Message);
            }
        }

        public void PopulateCountryTransalations()
        {
            try
            {
                Console.WriteLine("==============" + "Populating WU CountryTransalations Initiated." + "=========================");
                Logger.WriteLog("==============" + "Populating WU CountryTransalations Initiated." + "===============================================");

                List<WUCountry> wuCountries = GetDestinationCountries((Convert.ToInt64(DateTime.Now.ToString("yyyyMMddhhmmssff"))), Spanish);
                List<CountryTransalation> spanishCountryNames = mapper.Map<List<CountryTransalation>>(wuCountries);

                spanishCountryNames.ForEach(x =>
                {
                    x.Name = Regex.Unescape(x.Name);
                    x.Language = Spanish;
                });

                if (spanishCountryNames.Count > 0)
                {
                    populateData<CountryTransalation>(spanishCountryNames, "usp_PopulateCountryTranslations", "Translations");
                }

                Logger.WriteLog("==============" + "Populating WU CountryTransalations Completed." + "===============================================");
                Console.WriteLine("==============" + "Populating WU CountryTransalations Completed." + "=========================");
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PopulateWUCountries failed " + ex.Message);
            }
        }

        public void PopulateDeliveryServiceTransalations()
        {
            try
            {
                Console.WriteLine("==============" + "Populating WU DeliveryServiceTransalations Initiated." + "=========================");
                Logger.WriteLog("==============" + "Populating WU DeliveryServiceTransalations Initiated." + "===============================================");

                List<DeliveryServiceTransalation> wuDeliveryOption =
                    GetDeliveryTranslations((Convert.ToInt64(DateTime.Now.ToString("yyyyMMddhhmmssff"))), Spanish);

                wuDeliveryOption.ForEach(x =>
                {
                    x.Name = Regex.Unescape(x.Name);
                    x.EnglishName = x.EnglishName.Trim(' ');
                    x.Language = Spanish;
                });

                if (wuDeliveryOption.Count > 0)
                {
                    populateData<DeliveryServiceTransalation>(wuDeliveryOption, "usp_PopulateDeliveryServiceTranslatios", "Translations");
                }

                Logger.WriteLog("==============" + "Populating WU DeliveryServiceTransalations Completed." + "===============================================");
                Console.WriteLine("==============" + "Populating WU DeliveryServiceTransalations Completed." + "=========================");
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PopulateDeliveryServiceTransalations failed " + ex.Message);
            }
        }

        #endregion

        #region Private Methods

        private List<WUCountry> GetDestinationCountries(long transactionId, string language)
        {
            filters_type filters = new filters_type();
            filters.queryfilter1 = language;
            filters.queryfilter2 = "US USD";

            bool hasMoreRecords = true;
            List<WUCountry> destinationCountries = new List<WUCountry>();

            while (hasMoreRecords)
            {
                if (destinationCountries != null & destinationCountries.Count > 0)
                { filters.queryfilter3 = destinationCountries.Last().Name; }

                List<ISOCOUNTRY_Type> countries = getdasResponse(transactionId, DASServices.GetDestinationCountries.ToString(), out hasMoreRecords, filters).ConvertAll<ISOCOUNTRY_Type>(t => (ISOCOUNTRY_Type)t);
                destinationCountries.AddRange(mapper.Map<List<ISOCOUNTRY_Type>, List<WUCountry>>(countries));
            }
            return destinationCountries;
        }

        private List<WUState> GetUSStates(long transactionId)
        {
            filters_type filters = new filters_type();
            //TODO: check the query filter values.
            filters.queryfilter1 = English;

            bool hasMoreRecords = true;
            List<WUState> usStates = new List<WUState>();

            while (hasMoreRecords)
            {
                if (usStates.Count > 0)
                { filters.queryfilter2 = usStates.Last().Name; }

                List<USSTATEINFO_Type> states = getdasResponse(transactionId, DASServices.GetUSStateList.ToString(), out hasMoreRecords, filters).ConvertAll<USSTATEINFO_Type>(t => (USSTATEINFO_Type)t);
                usStates.AddRange(mapper.Map<List<USSTATEINFO_Type>, List<WUState>>(states));
            }

            return usStates;
        }

        private List<WUCity> GetMexicoCities(long transactionId)
        {
            filters_type filters = new filters_type();
            //TODO: check the query filter values.
            filters.queryfilter1 = English;

            bool hasMoreRecords = true;
            List<WUCity> mxStates = new List<WUCity>();
            List<MEXICOCITYSTATEINFO_Type> mexicoStates = null;

            while (hasMoreRecords)
            {
                if (mxStates.Count > 0)
                {
                    filters.queryfilter2 = mexicoStates.Last().CITY;
                    filters.queryfilter3 = mexicoStates.Last().STATE_NAME;
                }
                mexicoStates = getdasResponse(transactionId, DASServices.GetMexicoCityState.ToString(), out hasMoreRecords, filters).ConvertAll<MEXICOCITYSTATEINFO_Type>(t => (MEXICOCITYSTATEINFO_Type)t);
                mxStates.AddRange(mapper.Map<List<MEXICOCITYSTATEINFO_Type>, List<WUCity>>(mexicoStates));
            }

            return mxStates.Distinct().ToList();
        }

        private List<WUState> GetMexicoCityState(long transactionId)
        {
            filters_type filters = new filters_type();
            //TODO: check the query filter values.
            filters.queryfilter1 = English;

            bool hasMoreRecords = true;
            List<WUState> mxStates = new List<WUState>();
            List<MEXICOCITYSTATEINFO_Type> mexicoStates = null;

            while (hasMoreRecords)
            {
                if (mxStates.Count > 0)
                {
                    filters.queryfilter2 = mexicoStates.Last().CITY;
                    filters.queryfilter3 = mexicoStates.Last().STATE_NAME;
                }
                mexicoStates = getdasResponse(transactionId, DASServices.GetMexicoCityState.ToString(), out hasMoreRecords, filters).ConvertAll<MEXICOCITYSTATEINFO_Type>(t => (MEXICOCITYSTATEINFO_Type)t);
                mxStates.AddRange(mapper.Map<List<MEXICOCITYSTATEINFO_Type>, List<WUState>>(mexicoStates));
            }

            return mxStates.Distinct().ToList();
        }

        private List<WUCountryCurrency> GetCountriesCurrencies(long transactionId)
        {
            filters_type filters = new filters_type();
            filters.queryfilter1 = English;
            filters.queryfilter2 = "US USD";

            bool hasMoreRecords = true;
            List<WUCountryCurrency> countriesCurrencieslst = new List<WUCountryCurrency>();
            List<COUNTRY_CURRENCY_Type> countriesCurrencies = null;

            while (hasMoreRecords)
            {
                if (countriesCurrencies != null && countriesCurrencies.Count > 0)
                {
                    filters.queryfilter3 = countriesCurrencies.Last().COUNTRY_LONG;
                    filters.queryfilter4 = countriesCurrencies.Last().CURRENCY_NAME;
                }

                getdasResponse(transactionId, DASServices.GetCountriesCurrencies.ToString(), out hasMoreRecords, filters).ConvertAll<COUNTRY_CURRENCY_Type>(t => (COUNTRY_CURRENCY_Type)t);

                countriesCurrencies = getdasResponse(transactionId, DASServices.GetCountriesCurrencies.ToString(), out hasMoreRecords, filters).ConvertAll<COUNTRY_CURRENCY_Type>(t => (COUNTRY_CURRENCY_Type)t);
                countriesCurrencieslst.AddRange(mapper.Map<List<COUNTRY_CURRENCY_Type>, List<WUCountryCurrency>>(countriesCurrencies));
            }

            return countriesCurrencieslst;
        }

        private List<WUErrorMessages> GetErrorMesages(long transactionId)
        {
            filters_type filters = new filters_type();
            filters.queryfilter1 = English;

            bool hasMoreRecords = true;
            List<WUErrorMessages> errorMsglst = new List<WUErrorMessages>();
            List<ERRORINFO_Type> errormsgs = null;

            while (hasMoreRecords)
            {
                if (errormsgs != null && errormsgs.Count > 0)
                {
                    filters.queryfilter3 = errormsgs.Last().ERROR_CODE;
                }

                errormsgs = getdasResponse(transactionId, DASServices.GetErrorMessagesInfo.ToString(), out hasMoreRecords, filters).ConvertAll<ERRORINFO_Type>(t => (ERRORINFO_Type)t);
                errorMsglst.AddRange(mapper.Map<List<ERRORINFO_Type>, List<WUErrorMessages>>(errormsgs));
            }

            return errorMsglst;
        }

        private List<WUQQCcompanyNames> GetBillers(long transactionId)
        {
            filters_type filters = new filters_type();
            filters.queryfilter1 = English;
            filters.queryfilter2 = "US";
            filters.queryfilter7 = "FUSION";

            bool hasMoreRecords = true;
            List<WUQQCcompanyNames> billerlst = new List<WUQQCcompanyNames>();
            List<QQCCOMPANYNAME_Type> billers = null;

            while (hasMoreRecords)
            {
                if (billers != null && billers.Count > 0)
                { filters.queryfilter5 = billers.Last().CLIENT_ID; }

                billers = getdasResponse(transactionId, DASServices.GetQQCCompanyName.ToString(), out hasMoreRecords, filters).ConvertAll<QQCCOMPANYNAME_Type>(t => (QQCCOMPANYNAME_Type)t);
                billerlst.AddRange(mapper.Map<List<QQCCOMPANYNAME_Type>, List<WUQQCcompanyNames>>(billers));
            }
            return billerlst;
        }

        private void ConfigureWUOject(WUCredential wuCredentials)
        {
            if (string.IsNullOrWhiteSpace(wuCredentials.AccountIdentifier) || string.IsNullOrWhiteSpace(wuCredentials.CounterId))
            { throw new ArgumentNullException("Invalid AccountIdentifier or CounterId.AccountIdentier or CounterID cannot be null or empty string"); }

            channel = channel ?? SetChannelSetup(wuCredentials.ChannelName.ToString(), wuCredentials.ChannelVersion.ToString());
            remotesystem = remotesystem ?? SetRemoteSystem(wuCredentials.AccountIdentifier, wuCredentials.CounterId);
            wuCertificate = wuCertificate ?? SetWUCredentialCertificate(wuCredentials.WUClientCertificateSubjectName);
        }

        private X509Certificate2 SetWUCredentialCertificate(string wucertificatename)
        {
            X509Store certificateStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            // Open the store.
            certificateStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            // Find the certificate with the specified subject.
            X509Certificate2Collection certificates = certificateStore.Certificates.Find(X509FindType.FindBySubjectName, wucertificatename, false);
            certificateStore.Close();

            if (certificates.Count < 1)
            {
                throw new Exception("Certificate not found for WU Partner Integration");
            }
            return certificates[0];
        }

        private channel SetChannelSetup(string channelName, string channelVersion)
        {
            channel chn = new channel();
            chn.type = channel_type.H2H;
            chn.name = channelName;
            chn.version = channelVersion;
            chn.typeSpecified = true;
            return chn;
        }

        private foreign_remote_system SetRemoteSystem(string wuAccountIdentifier, string counterId)
        {
            foreign_remote_system frs = new foreign_remote_system();
            frs.identifier = wuAccountIdentifier;
            frs.counter_id = counterId;
            return frs;
        }

        private List<object> getdasResponse(long transactionId, string dasServiceName, out bool hasMoreRecords, filters_type queryfilters = null)
        {
            WUCredential wuCredentials = new WUCredential();
            DASInquiryPortTypeClient dc;
            wuCredentials = GetWUCredential(channelPartnerID);
            ConfigureWUOject(wuCredentials);

            List<object> responseList = new List<object>();

            if (_wUServiceURL == string.Empty)
            {
                dc = new DASInquiryPortTypeClient("SOAP_HTTP_Port", wuCredentials.WUServiceUrl.ToString());
            }
            else
            {
                dc = new DASInquiryPortTypeClient("SOAP_HTTP_Port", _wUServiceURL);
            }

            dc.ClientCredentials.ClientCertificate.Certificate = wuCertificate;

            h2hdasrequest request = new h2hdasrequest();
            request.channel = channel;
            remotesystem.reference_no = transactionId.ToString();
            request.foreign_remote_system = remotesystem;
            request.name = dasServiceName;

            request.filters = queryfilters;
            REPLYType responseItems = null;
            try
            {
                responseItems = (REPLYType)dc.DAS_Service(request).MTML.Item;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Logger.WriteLog("getdasResponse failed " + ex.Message);

            }

            hasMoreRecords = responseItems.DATA_CONTEXT.HEADER.DATA_MORE.ToString().ToUpper() == "Y" ? true : false;

            if (responseItems.DATA_CONTEXT.RECORDSET != null)
                responseList.AddRange(responseItems.DATA_CONTEXT.RECORDSET.Items.ToList<object>());

            return responseList;
        }

        private List<DeliveryServiceTransalation> GetDeliveryTranslations(long transactionId, string language)
        {
            filters_type filters = new filters_type()
            {
                queryfilter1 = language
            };

            bool hasMoreRecords = true;
            List<DeliveryServiceTransalation> translatedNames = new List<DeliveryServiceTransalation>();
            //MGI.Cxn.MoneyTransfer.WU.Data.DASServices
            while (hasMoreRecords)
            {
                List<DASDELIVERYTRANSLATE_Type> deliveryServiceName = getdasResponse((Convert.ToInt64(DateTime.Now.ToString("yyyyMMddhhmmssff"))), DASServices.GetDeliveryTranslations.ToString(), out hasMoreRecords, filters).ConvertAll<DASDELIVERYTRANSLATE_Type>(t => (DASDELIVERYTRANSLATE_Type)t);
                translatedNames.AddRange(mapper.Map<List<DASDELIVERYTRANSLATE_Type>, List<DeliveryServiceTransalation>>(deliveryServiceName));
            }
            return translatedNames;
        }

        private WUCredential GetWUCredential(long channelPartnerId)
        {
            StoredProcedure monitorProcedure = new StoredProcedure("usp_GetWUnionCredentials");

            monitorProcedure.WithParameters(InputParameter.Named("channerlPartnerId").WithValue(channelPartnerId));

            IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(monitorProcedure);

            WUCredential credential = null;

            while (datareader.Read())
            {
                credential = new WUCredential();
                credential.Id = datareader.GetInt64OrDefault("WUCredentialID");
                credential.WUServiceUrl = datareader.GetStringOrDefault("WUServiceUrl");
                credential.WUClientCertificateSubjectName = datareader.GetStringOrDefault("WUClientCertificateSubjectName");
                credential.AccountIdentifier = datareader.GetStringOrDefault("AccountIdentifier");
                credential.CounterId = datareader.GetStringOrDefault("CounterId");
                credential.ChannelName = datareader.GetStringOrDefault("ChannelName");
                credential.ChannelVersion = datareader.GetStringOrDefault("ChannelVersion");
                credential.ChannelPartnerId = datareader.GetInt64OrDefault("ChannelPartnerId");
            }
            return credential;
        }

        private void populateData<T>(List<T> dataList, string spName, string dataTableName)
        {
            DataTable table = Helper.ToDataTable<T>(dataList);
            if (table != null && table.HasRows())
            {
                StoredProcedure monitorProcedure = new StoredProcedure(spName);
                StringWriter writer = new StringWriter();

                table.TableName = dataTableName;
                table.WriteXml(writer);

                DataParameter[] dataParameters = new DataParameter[]
                {
                            new DataParameter(dataTableName, DbType.Xml)
                            {
                                Value =  writer.ToString()
                            }
                };

                monitorProcedure.WithParameters(dataParameters);
                DataHelper.GetConnectionManager().ExecuteNonQuery(monitorProcedure);
            }
        }

        private void populateCatalog(string channelPartnerId)
        {
            StoredProcedure monitorProcedure = new StoredProcedure("usp_PopulateBillerCatalog");
            monitorProcedure.WithParameters(InputParameter.Named("channelPartnerId").WithValue(channelPartnerId));
            monitorProcedure.WithParameters(InputParameter.Named("providerId").WithValue(ProviderID));
            monitorProcedure.WithParameters(InputParameter.Named("dtServerDate").WithValue(DateTime.Now));

            DataHelper.GetConnectionManager().ExecuteNonQuery(monitorProcedure);
        }


        private void UpdateFraudLimits(long transactionId, ref List<WUCountryCurrency> wuCountryCurrencies)
        {
            if (wuCountryCurrencies == null) return;
            foreach (var item in wuCountryCurrencies)
            {
                var dstcountry = GetDestinationCurrency(transactionId, item.CountryCode);
                item.FraudLimit = dstcountry.CONSUMER_LIMIT;
            }
        }

        /// <summary>
        /// Getting the Fraud Limit value for the country.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="countrycode"></param>
        /// <returns></returns>
        private ISOCURRENCY_Type GetDestinationCurrency(long transactionId, string countrycode)
        {
            filters_type filters = new filters_type();
            filters.queryfilter1 = "en";
            filters.queryfilter2 = "US USD";
            filters.queryfilter3 = countrycode;
            filters.queryfilter5 = "AC";
            bool hasMoreRecords = true;
            Logger.WriteLog("==============" + "GetDestinationCurrency - DASEnquiry : GetDestinationCurrency call started");

            var test = getdasResponse(transactionId, DASServices.GetErrorTranslations.ToString(), out hasMoreRecords, filters);//.ConvertAll<>(t => (ISOCURRENCY_Type)t);

            var response = getdasResponse(transactionId, DASServices.GetDestinationCurrencies.ToString(), out hasMoreRecords, filters).ConvertAll<ISOCURRENCY_Type>(t => (ISOCURRENCY_Type)t);
            Logger.WriteLog("==============" + "GetDestinationCurrency - DASEnquiry: GetDestinationCurrency ended");
            return response.FirstOrDefault();
        }

        #endregion
    }
}
