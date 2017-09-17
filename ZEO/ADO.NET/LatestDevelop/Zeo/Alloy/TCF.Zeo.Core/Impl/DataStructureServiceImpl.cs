using TCF.Zeo.Core.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Core.Data;

using System.Data;
using TCF.Zeo.Common.Data;

using P3Net.Data.Common;
using P3Net.Data;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Core.Data.Exceptions;

namespace TCF.Zeo.Core.Impl
{
    public class DataStructureServiceImpl : IDataStructuresService
    {
        /// <summary>
        /// This method is to get the collection of all phone types.
        /// </summary>
        /// <returns></returns>
        public List<string> PhoneTypes(ZeoContext context)
        {
            try
            {
                List<string> phoneTypes = new List<string>();

                StoredProcedure coreCustomerProcedure = new StoredProcedure("usp_GetPhoneTypes");

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(coreCustomerProcedure))
                {
                    while (datareader.Read())
                    {
                        string phoneType = datareader.GetStringOrDefault("Type");

                        phoneTypes.Add(phoneType);
                    }
                }

                return phoneTypes;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.GET_PHONETYPES_FAILED, ex);
            }
        }

        /// <summary>
        /// This method is to get the collection of mobile providers.
        /// </summary>
        /// <returns></returns>
        public List<string> MobileProviders(ZeoContext context)
        {
            try
            {
                List<string> providers = new List<string>();

                StoredProcedure coreCustomerProcedure = new StoredProcedure("usp_GetProviders");

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(coreCustomerProcedure))
                {
                    while (datareader.Read())
                    {
                        string provider = datareader.GetStringOrDefault("Name");

                        providers.Add(provider);
                    }
                }

                return providers;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.GET_MOBILEPROVIDERS_FAILED, ex);
                throw;
            }
        }

        /// <summary>
        /// This method is to get the collection of master countries.
        /// </summary>
        /// <param name="channelPartnerId"></param>
        /// <returns></returns>
        public List<MasterCountry> MasterCountries(long channelPartnerId, ZeoContext context)
        {
            try
            {
                List<MasterCountry> masterCountries = new List<MasterCountry>();
                MasterCountry masterCountry;

                StoredProcedure coreCustomerProcedure = new StoredProcedure("usp_GetMasterCountriesByChannelPartnerId");

                coreCustomerProcedure.WithParameters(InputParameter.Named("channelPartnerId").WithValue(channelPartnerId));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(coreCustomerProcedure))
                {
                    while (datareader.Read())
                    {
                        masterCountry = new MasterCountry();
                        masterCountry.Abbr2 = datareader.GetStringOrDefault("Abbr2");
                        masterCountry.Abbr3 = datareader.GetStringOrDefault("Abbr3");
                        masterCountry.Name = datareader.GetStringOrDefault("Name");

                        masterCountries.Add(masterCountry);
                    }
                }

                return masterCountries;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.GET_MASTERCOUNTRIES_FAILED, ex);
            }
        }

        /// <summary>
        /// This method is to get the collection of all states belongs to the country which is in 'country' parameter.
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        public List<string> States(string country, ZeoContext context)
        {
            try
            {
                List<string> states = new List<string>();

                StoredProcedure coreCustomerProcedure = new StoredProcedure("usp_GetStates");

                coreCustomerProcedure.WithParameters(InputParameter.Named("countryCode").WithValue(country));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(coreCustomerProcedure))
                {
                    while (datareader.Read())
                    {
                        string state = datareader.GetStringOrDefault("abbr");

                        states.Add(state);
                    }
                }

                return states;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.GET_STATES_FAILED, ex);
            }
        }

        /// <summary>
        /// This method is to get the collection of all states belongs to the USA.
        /// </summary>
        /// <returns></returns>
        public List<string> USStates(ZeoContext context)
        {
            try
            {
                List<string> states = new List<string>();

                StoredProcedure coreCustomerProcedure = new StoredProcedure("usp_GetStates");

                coreCustomerProcedure.WithParameters(InputParameter.Named("countryCode").WithValue("840")); //US country code is passed.

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(coreCustomerProcedure))
                {
                    while (datareader.Read())
                    {
                        string state = datareader.GetStringOrDefault("abbr");

                        states.Add(state);
                    }
                }

                return states;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.GET_USSTATES_FAILED, ex);
            }
        }

        /// <summary>
        /// This method is to get the collection of all countries belongs to the channel partner.
        /// </summary>
        /// <param name="channelPartnerId"></param>
        /// <returns></returns>
        public List<string> IdCountries(long channelPartnerId, ZeoContext context)
        {
            try
            {
                List<string> idCountries = new List<string>();

                StoredProcedure coreCustomerProcedure = new StoredProcedure("usp_GetIdCountriesByChannelPartnerId");

                coreCustomerProcedure.WithParameters(InputParameter.Named("channelPartnerId").WithValue(channelPartnerId));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(coreCustomerProcedure))
                {
                    while (datareader.Read())
                    {
                        string idCountry = datareader.GetStringOrDefault("Name");

                        idCountries.Add(idCountry);
                    }
                }

                return idCountries;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.GET_IDCOUNTRIES_FAILED, ex);
            }
        }

        /// <summary>
        /// This method is to get the collection of all states by channel partner id, country and Alloy id type.
        /// </summary>
        /// <param name="channelPartnerId"></param>
        /// <param name="country"></param>
        /// <param name="idType"></param>
        /// <returns></returns>
        public List<string> IdStates(long channelPartnerId, string country, string idType, ZeoContext context)
        {
            try
            {
                List<string> idStates = new List<string>();

                StoredProcedure coreCustomerProcedure = new StoredProcedure("usp_GetIdStatesByChannelPartnerId");

                coreCustomerProcedure.WithParameters(InputParameter.Named("channelPartnerId").WithValue(channelPartnerId));
                coreCustomerProcedure.WithParameters(InputParameter.Named("country").WithValue(country));
                coreCustomerProcedure.WithParameters(InputParameter.Named("idType").WithValue(idType));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(coreCustomerProcedure))
                {
                    while (datareader.Read())
                    {
                        string idState = datareader.GetStringOrDefault("Name");

                        idStates.Add(idState);
                    }
                }

                return idStates;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.GET_IDSTATES_FAILED, ex);
            }
        }

        /// <summary>
        /// This method is to get the collection of all Alloy id types by channel partner id and country.
        /// </summary>
        /// <param name="channelPartnerId"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        public List<string> IdTypes(long channelPartnerId, string country, ZeoContext context)
        {
            try
            {
                List<string> idTypes = new List<string>();

                StoredProcedure coreCustomerProcedure = new StoredProcedure("usp_GetIdTypesByChannelPartnerId");

                coreCustomerProcedure.WithParameters(InputParameter.Named("channelPartnerId").WithValue(channelPartnerId));
                coreCustomerProcedure.WithParameters(InputParameter.Named("countryName").WithValue(country));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(coreCustomerProcedure))
                {
                    while (datareader.Read())
                    {
                        string Name = datareader.GetStringOrDefault("Name");
                        idTypes.Add(Name);
                    }
                }

                return idTypes;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.GET_IDTYPES_FAILED, ex);
            }
        }

        /// <summary>
        /// This method is to get the collection of legal code.
        /// </summary>
        /// <returns></returns>
        public List<LegalCode> GetLegalCodes(ZeoContext context)
        {
            try
            {
                List<LegalCode> legalCodes = new List<LegalCode>();
                LegalCode legalCode;

                StoredProcedure coreCustomerProcedure = new StoredProcedure("usp_GetLegalCodes");

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(coreCustomerProcedure))
                {
                    while (datareader.Read())
                    {
                        legalCode = new LegalCode();
                        legalCode.Code = datareader.GetStringOrDefault("Code");
                        legalCode.Name = datareader.GetStringOrDefault("Name");

                        legalCodes.Add(legalCode);
                    }
                }

                return legalCodes;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.GET_LEGALCODES_FAILED, ex);
            }
        }

        /// <summary>
        /// This method is to get the collection of Occupations.
        /// </summary>
        /// <returns></returns>
        public List<Occupation> GetOccupations(ZeoContext context)
        {
            try
            {
                List<Occupation> occupations = new List<Occupation>();
                Occupation occupation;

                StoredProcedure coreCustomerProcedure = new StoredProcedure("usp_GetOccupations");

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(coreCustomerProcedure))
                {
                    while (datareader.Read())
                    {
                        occupation = new Occupation();
                        occupation.Code = datareader.GetStringOrDefault("Code");
                        occupation.Name = datareader.GetStringOrDefault("Name");

                        occupations.Add(occupation);
                    }
                }

                return occupations;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.GET_OCCUPATIONS_FAILED, ex);
            }
        }

        /// <summary>
        /// This method is to get the collection of all countries.
        /// </summary>
        /// <param name="channelPartnerId"></param>
        /// <returns></returns>
        public List<string> Countries(long channelPartnerId, ZeoContext context)
        {
            try
            {
                List<string> countries = new List<string>();

                StoredProcedure coreCustomerProcedure = new StoredProcedure("usp_GetCountries");

                coreCustomerProcedure.WithParameters(InputParameter.Named("ChannelPartnerId").WithValue(channelPartnerId));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(coreCustomerProcedure))
                {
                    while (datareader.Read())
                    {
                        string country = datareader.GetStringOrDefault("Name");

                        countries.Add(country);
                    }
                }

                return countries;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.GET_COUNTRIES_FAILED, ex);
            }
        }

        /// <summary>
        /// This method is used to get the master country by code.
        /// </summary>
        /// <param name="masterCountryAbbr2"></param>
        /// <returns></returns>
        public MasterCountry GetMasterCountryByCode(string masterCountryAbbr2, ZeoContext context)
        {
            try
            {
                MasterCountry masterCountry = null;

                StoredProcedure countryProcedure = new StoredProcedure("usp_GetMasterCountryByAbbr2");

                countryProcedure.WithParameters(InputParameter.Named("Abbr2").WithValue(masterCountryAbbr2));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(countryProcedure))
                {
                    while (datareader.Read())
                    {
                        masterCountry = new MasterCountry();
                        masterCountry.Name = datareader.GetStringOrDefault("Name");
                        masterCountry.Abbr2 = datareader.GetStringOrDefault("Abbr2");
                        masterCountry.Abbr3 = datareader.GetStringOrDefault("Abbr3");
                    }
                }

                return masterCountry;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.GET_MASTER_COUNTRY_BY_CODE_FAILED, ex);
            }
        }


        public IdType GetIdType(string idType, string country, string stateName, ZeoContext context)
        {
            try
            {
                IdType type = new IdType();
                StoredProcedure countryProcedure = new StoredProcedure("usp_GetIdType");

                countryProcedure.WithParameters(InputParameter.Named("channelPartnerId").WithValue(context.ChannelPartnerId));
                countryProcedure.WithParameters(InputParameter.Named("idType").WithValue(idType));
                countryProcedure.WithParameters(InputParameter.Named("country").WithValue(country));
                countryProcedure.WithParameters(InputParameter.Named("state").WithValue(stateName));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(countryProcedure))
                {
                    while (datareader.Read())
                    {
                        type.Name = datareader.GetStringOrDefault("Name");
                        type.Mask = datareader.GetStringOrDefault("Mask");
                        type.HasExpirationDate = datareader.GetBooleanOrDefault("HasExpirationDate");
                    }
                }

                return type;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.GET_IDTYPES_FAILED, ex);
            }
        }

        /// <summary>
        /// Get the state name from the state and country code.
        /// </summary>
        /// <param name="stateCode"></param>
        /// <param name="countryCode"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string GetStateNameByCode(string stateCode, string countryCode, ZeoContext context)
        {
            try
            {
                string stateName = string.Empty;

                StoredProcedure coreCustomerProcedure = new StoredProcedure("usp_GetStateNameByCode");

                coreCustomerProcedure.WithParameters(InputParameter.Named("StateCode").WithValue(stateCode));
                coreCustomerProcedure.WithParameters(InputParameter.Named("CountryCode").WithValue(countryCode));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(coreCustomerProcedure))
                {
                    while (datareader.Read())
                    {
                        stateName = datareader.GetStringOrDefault("Name");
                    }
                }

                return stateName;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CustomerException(CustomerException.GET_STATENAME_FAILED, ex);
            }
        }

        public List<KeyValuePair> GetBINDetails(ZeoContext context)
        {
            try
            {
                List<KeyValuePair> cardBINs = new List<KeyValuePair>();
                StoredProcedure cardBinProc = new StoredProcedure("usp_GetCardDetails");

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(cardBinProc))
                {
                    while (datareader.Read())
                    {
                        cardBINs.Add(new KeyValuePair
                        {
                            Key = datareader.GetStringOrDefault("CardBIN"),
                            Value = datareader.GetStringOrDefault("CardType")
                        });
                    }
                }

                return cardBINs;

            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw ex;
            }
        }

    }
}
