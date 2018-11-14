using AutoMapper;
using MGI.Common.DataAccess.Contract;
using MGI.Cxn.MoneyTransfer.Contract;
using MGI.Cxn.MoneyTransfer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using MGData = MGI.Cxn.MoneyTransfer.MG.Data;

namespace MGI.Cxn.MoneyTransfer.MG.Impl
{
	public partial class Gateway : IMoneyTransferSetup
	{
		#region Dependencies
		public IRepository<MGData.Country> CountryRepo { private get; set; }
		public IRepository<MGData.State> StateRepo { private get; set; }
		public IRepository<MGData.DeliveryOption> DeliveryOptionRepo { private get; set; }
		public IRepository<MGData.StateRegulator> StateRegulatorRepo { private get; set; }
		//public IRepository<MGData.Currency> CurrencyRepo { private get; set; }
		public IRepository<MGData.CountryCurrency> CountryCurrencyRepo { private get; set; }

		#endregion

		#region Public Methods

		public List<MasterData> GetCountries()
		{
			List<MasterData> countries = new List<MasterData>();
			var moneyGramCountries = CountryRepo.FilterBy(c => c.Receiveactive == true);

			if (moneyGramCountries != null)
			{
				countries = moneyGramCountries.Select(c => new MasterData() { Code = c.Code, Name = c.Name }).ToList();
			}

			return countries;
		}

		public List<MasterData> GetStates(string countryCode)
		{
			List<MasterData> states = new List<MasterData>();
			var moneyGramStates = StateRepo.FilterBy(s => s.Countrycode == countryCode);

			if (moneyGramStates != null)
			{
				states =( moneyGramStates.Select(c => new MasterData() { Code = c.Code, Name = c.Name }).ToList());
                states = states.OrderBy(x => x.Name).ToList();   
			}

			return states;
		}

		public string GetCurrencyCode(string countryCode)
		{
			string currency = string.Empty;

			if (string.IsNullOrWhiteSpace(countryCode))
				return currency;

			var countryCurrencies = CountryCurrencyRepo.FilterBy(c => c.CountryCode == countryCode);

			var countryCurrency = countryCurrencies.FirstOrDefault();

			if (countryCurrency != null)
			{
				currency = countryCurrency.Localcurrency;
			}

			return currency;
		}

		public List<MasterData> GetCurrencyCodeList(string countryCode)
		{
			List<MasterData> currencies = new List<MasterData>();

			if (string.IsNullOrWhiteSpace(countryCode))
				return currencies;

			var countryCurrencies = CountryCurrencyRepo.FilterBy(c => c.CountryCode == countryCode);

			if (countryCurrencies != null)
			{
				currencies = countryCurrencies.Select(c => new MasterData() { Code = c.CountryCode, Name = c.Localcurrency }).ToList();
			}

			return currencies;
		}

		#endregion

		#region Methods not implemented
		public List<MasterData> GetAmountTypes()
		{
			throw new NotImplementedException();
		}

		public List<MasterData> GetStatuses()
		{
			throw new NotImplementedException();
		}

		public List<MasterData> GetRelationships()
		{
			throw new NotImplementedException();
		}

		public List<MasterData> GetPickupOptions()
		{
			throw new NotImplementedException();
		}

		public List<MasterData> GetCities(string stateCode)
		{
			return new List<MoneyTransfer.Data.MasterData>() { };
		}


		#endregion

		#region Private Methods

		private string GetCountryCode(string countyName)
		{
			string countryCode = string.Empty;

			if (!string.IsNullOrWhiteSpace(countyName))
			{
				var country = CountryRepo.FindBy(c => c.Name == countyName);
				if (country != null)
				{
					countryCode = country.Code;
				}
			}

			return countryCode;
		}

		private string GetStateCode(string stateName,string countryCode)
		{
			string stateCode = string.Empty;

			if (!string.IsNullOrWhiteSpace(stateName))
			{
				var state = StateRepo.FindBy(c => c.Name == stateName);
				if (state != null)
				{
					stateCode = state.Code;
				}
			}
			else
			{
                var states = StateRepo.FilterBy(c => c.Countrycode == countryCode);
                if (states != null && states.Any())
                {
                    stateCode = states.FirstOrDefault().Code;
                }
			}

			return stateCode;
		}

		#endregion

	}
}
