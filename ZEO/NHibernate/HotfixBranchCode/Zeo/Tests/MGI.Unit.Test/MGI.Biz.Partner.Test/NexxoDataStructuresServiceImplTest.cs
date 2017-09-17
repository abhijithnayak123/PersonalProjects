using MGI.Biz.Partner.Data;
using MGI.Common.Util;
using MGI.Unit.Test;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.Partner.Test
{
	[TestFixture]
	public class NexxoDataStructuresServiceImplTest : BaseClass_Fixture
	{
		public MGI.Biz.Partner.Contract.INexxoDataStructuresService BIZNexxoDataStructure { private get; set; }

		[Test]
		public void Lookup_Terminal()
		{
			MGIContext context = new MGIContext() { };

			NexxoIdType idType = BIZNexxoDataStructure.Find(100000000, 100000001, "Name", "India", "KA", context);

			Assert.NotNull(idType);
		}

		[Test]
		public void Get_Countries()
		{
			MGIContext context = new MGIContext() { };

			List<string> countries = BIZNexxoDataStructure.Countries(100000000, context);

			Assert.NotNull(countries);
			Assert.True(countries.Count > 0);
		}

		[Test]
		public void Get_States()
		{
			MGIContext context = new MGIContext() { };

			List<string> states = BIZNexxoDataStructure.States(100000000, "US", context);

			Assert.NotNull(states);
			Assert.True(states.Count > 0);
		}

		[Test]
		public void Get_US_States()
		{
			MGIContext context = new MGIContext() { };

			List<string> states = BIZNexxoDataStructure.USStates(100000000, context);

			Assert.NotNull(states);
			Assert.True(states.Count > 0);
		}


		[Test]
		public void Get_IdCountries()
		{
			MGIContext context = new MGIContext() { };

			List<string> countries = BIZNexxoDataStructure.IdCountries(100000000, 100000001, context);

			Assert.NotNull(countries);
			Assert.True(countries.Count > 0);
		}

		[Test]
		public void Get_IdStates()
		{
			MGIContext context = new MGIContext() { };

			List<string> states = BIZNexxoDataStructure.IdStates(100000000, 100000001, "US", "1", context);

			Assert.NotNull(states);
			Assert.True(states.Count > 0);
		}

		[Test]
		public void Get_IdTypes()
		{
			MGIContext context = new MGIContext() { };

			List<string> types = BIZNexxoDataStructure.IdTypes(100000000, 100000001, "US", context);

			Assert.NotNull(types);
			Assert.True(types.Count > 0);
		}


		[Test]
		public void Get_LegalCodes()
		{
			MGIContext context = new MGIContext() { };

			List<LegalCode> legalCodes = BIZNexxoDataStructure.GetLegalCodes(100000000, context);

			Assert.NotNull(legalCodes);
			Assert.True(legalCodes.Count > 0);
		}

		[Test]
		public void Get_Occupations()
		{
			MGIContext context = new MGIContext() { };

			List<Occupation> occupations = BIZNexxoDataStructure.GetOccupations(100000000, context);

			Assert.NotNull(occupations);
			Assert.True(occupations.Count > 0);
		}

		[Test]
		public void Get_PhoneTypes()
		{
			MGIContext context = new MGIContext() { };

			List<string> types = BIZNexxoDataStructure.PhoneTypes(100000000, context);

			Assert.NotNull(types);
			Assert.True(types.Count > 0);
		}

		[Test]
		public void Get_MobileProviders()
		{
			MGIContext context = new MGIContext() { };

			List<string> providers = BIZNexxoDataStructure.MobileProviders(100000000, context);

			Assert.NotNull(providers);
			Assert.True(providers.Count > 0);
		}

		[Test]
		public void Get_MasterCountries()
		{
			MGIContext context = new MGIContext() { };

			List<MasterCountry> countries = BIZNexxoDataStructure.MasterCountries(100000000, 100000001, context);

			Assert.NotNull(countries);
			Assert.True(countries.Count > 0);
		}

		[Test]
		public void Get_MasterCountries_By_Code()
		{
			MGIContext context = new MGIContext() { };

			MasterCountry country = BIZNexxoDataStructure.GetMasterCountryByCode(100000000, "US", context);

			Assert.NotNull(country);
		}
	}
}
