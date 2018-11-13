using MGI.Common.DataAccess.Contract;
using MGI.Cxn.MoneyTransfer.Contract;
using MGI.Cxn.MoneyTransfer.WU.Data;
using MGI.Cxn.WU.Common.Contract;
using MGI.Cxn.WU.Common.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MGI.Common.TransactionalLogging.Data;
using MGI.Common.Util;

namespace MGI.Cxn.MoneyTransfer.WU.Impl
{
	public partial class WUGateway : IMoneyTransfer
	{
		public IRepository<WUCountry> WUCountryRepo { private get; set; }
		public IRepository<WUState> WUStateRepo { private get; set; }
		public IRepository<WUCity> WUCityRepo { private get; set; }
		public IRepository<WUCountryCurrencyDeliveryMethod> WUDeliveryMethodRepo { private get; set; }
		public IRepository<WUDeliveryOption> WUDeliveryOptionRepo { private get; set; }
		public IRepository<WUPaymentMethod> WUPaymentMethodRepo { private get; set; }
		public IRepository<WUPickupMethod> WUPickupMethodRepo { private get; set; }
		public IRepository<WUPickupDetail> WUPickupDetailRepo { private get; set; }
		public IRepository<WUCountryCurrency> WUCountryCurrencyRepo { private get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public List<MoneyTransfer.Data.MasterData> GetCountries()
		{
			try
			{
				var country = WUCountryRepo.All();

				if (country != null)
				{
					return country.Select(i => new MoneyTransfer.Data.MasterData() { Id = i.Id, Code = i.CountryCode, Name = i.Name }).ToList();
				}
				else
				{
					return new List<MoneyTransfer.Data.MasterData>() { };
				}
			}
			catch (Exception ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<string>(string.Empty, "GetCountries", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in GetCountries - MGI.Cxn.MoneyTransfer.WU.Impl.WUSetup", ex.Message, ex.StackTrace);
				throw new Exception("Error in retrieving Countries", ex.InnerException);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="countryCode"></param>
		/// <returns></returns>
		public List<MoneyTransfer.Data.MasterData> GetStates(string countryCode)
		{
			List<MoneyTransfer.Data.MasterData> states = new List<MoneyTransfer.Data.MasterData>();

			if (countryCode.Equals("US", StringComparison.OrdinalIgnoreCase)
				|| countryCode.Equals("MX", StringComparison.OrdinalIgnoreCase)
				|| countryCode.Equals("CA", StringComparison.OrdinalIgnoreCase))
			{
				try
				{
					var state = WUStateRepo.FilterBy(c => c.ISOCountryCode == countryCode);

					if (state != null)
					{
						return state.Select(i => new MoneyTransfer.Data.MasterData() { Id = i.Id, Code = i.StateCode, Name = i.Name }).ToList();
					}

				}
				catch (Exception ex)
				{
					//AL-3370 Transactional Log User Story
					MongoDBLogger.Error<string>(countryCode, "GetStates", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
						"Error in GetStates - MGI.Cxn.MoneyTransfer.WU.Impl.WUSetup", ex.Message, ex.StackTrace);
					throw new Exception("Error in retrieving States", ex.InnerException);
				}
			}

			return states;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="stateCode"></param>
		/// <returns></returns>
		public List<MoneyTransfer.Data.MasterData> GetCities(string stateCode)
		{
			try
			{
				var cities = WUCityRepo.FilterBy(c => c.StateCode == stateCode);

				if (cities != null)
				{
					return cities.Select(i => new MoneyTransfer.Data.MasterData() { Id = i.Id, Code = i.Id.ToString(), Name = i.Name }).ToList();
				}
				else
				{
					return new List<MoneyTransfer.Data.MasterData>() { };
				}
			}
			catch (Exception ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<string>(stateCode, "GetCities", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in GetCities - MGI.Cxn.MoneyTransfer.WU.Impl.WUSetup", ex.Message, ex.StackTrace);
				throw new Exception("Error in retrieving Cities", ex.InnerException);
			}
		}



		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public List<MoneyTransfer.Data.MasterData> GetStatuses()
		{
			try
			{
				List<WUStatus> Statuses = Enum.GetValues(typeof(WUStatus))
											.Cast<WUStatus>()
											.ToList();

				List<MoneyTransfer.Data.MasterData> statusList = new List<MoneyTransfer.Data.MasterData>();

				foreach (var status in Statuses)
				{
					statusList.Add(new MoneyTransfer.Data.MasterData { Id = (long)status, Code = status.ToString(), Name = status.ToString() });
				}
				return statusList;
			}
			catch (Exception ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<string>(string.Empty, "GetStatuses", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in GetStatuses - MGI.Cxn.MoneyTransfer.WU.Impl.WUSetup", ex.Message, ex.StackTrace);
				throw new Exception("Error in retrieving GetStatuses", ex.InnerException);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="countryCode"></param>
		/// <returns></returns>
		public string GetCurrencyCode(string countryCode)
		{
			try
			{
				var countriesCurrency = WUCountryCurrencyRepo.All();
				if (countriesCurrency != null)
				{
					WUCountryCurrency countryCurrency = null;
					List<WUCountryCurrency> currencyList = countriesCurrency.Where(c => c.CountryCode == countryCode).ToList();
					try
					{
						if (currencyList.Count > 1)
						{
							RegionInfo myRI1 = new RegionInfo(countryCode);
							var isoCurrencyList = currencyList.Where(c => c.CurrencyCode == myRI1.ISOCurrencySymbol).ToList();
							currencyList = isoCurrencyList.Count > 0 ? isoCurrencyList : currencyList;
						}
					}
					catch (Exception) { }

					int i = currencyList.Count;

					countryCurrency = currencyList.FirstOrDefault();
					return countryCurrency != null ? countryCurrency.CurrencyCode : string.Empty;
				}
				else
				{
					return string.Empty;
				}
			}
			catch (Exception ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<string>(countryCode, "GetCurrencyCode", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in GetCurrencyCode - MGI.Cxn.MoneyTransfer.WU.Impl.WUSetup", ex.Message, ex.StackTrace);
				throw new Exception(string.Format("Error in retrieving Currency Code for CountryCode : {0}", countryCode), ex.InnerException);
			}
		}

		public List<MoneyTransfer.Data.MasterData> GetCurrencyCodeList(string countryCode)
		{
			try
			{
				var countriesCurrency = WUCountryCurrencyRepo.FilterBy(c => c.CountryCode == countryCode);
				if (countriesCurrency != null)
				{
					return countriesCurrency.Select(i => new MoneyTransfer.Data.MasterData() { Id = i.Id, Code = i.CurrencyCode, Name = i.CurrencyName }).ToList();
				}
				else
				{
					return new List<MoneyTransfer.Data.MasterData>() { };
				}
			}
			catch (Exception ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<string>(countryCode, "GetCurrencyCodeList", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in GetCurrencyCodeList - MGI.Cxn.MoneyTransfer.WU.Impl.WUSetup", ex.Message, ex.StackTrace);
				throw new Exception(string.Format("Error in retrieving Currency Code for CountryCode : {0}", countryCode), ex.InnerException);
			}
		}
	}
}
