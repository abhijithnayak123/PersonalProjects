using MGI.Core.Partner.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPtnrDataStructureService = MGI.Core.Partner.Contract.INexxoDataStructuresService;

namespace MGI.Biz.TCF.Impl
{
	public class BaseClass
	{
		const int FirstNameMaxLength = 20;
		const int LastNameMaxLength = 20;
		const int MiddleNameMaxLength = 15;
		const int FullNameLength = 40;

		public static IPtnrDataStructureService PTNRDataStructureService { private get; set; }

		public static string GetMasterCountryName(string countryCode)
		{
			MasterCountry masterCountry = PTNRDataStructureService.GetMasterCountryByCode(countryCode);
			string countryName = masterCountry != null ? masterCountry.Name : null;

			return countryName;
		}

		public static Dictionary<string, string> TruncateFullName(string firstName, string middleName, string lastName, string secondLastName)
		{
			firstName = firstName.Substring(0, firstName.Length >= FirstNameMaxLength ? FirstNameMaxLength : firstName.Length);
			middleName = middleName.Substring(0, middleName.Length >= MiddleNameMaxLength ? MiddleNameMaxLength : middleName.Length);

			int lastNameLength;
			if (firstName.Length == FirstNameMaxLength)
			{
				lastName = lastName.Substring(0, lastName.Length >= LastNameMaxLength - 1 ? LastNameMaxLength - 1 : lastName.Length);

				lastNameLength = FullNameLength - (FirstNameMaxLength + lastName.Length + 2) ;
			}
			else
			{
				lastName = lastName.Substring(0, lastName.Length >= LastNameMaxLength ? LastNameMaxLength : lastName.Length);

				lastNameLength = LastNameMaxLength - (lastName.Length + 1);
			}

			if (!string.IsNullOrWhiteSpace(secondLastName) && lastNameLength > 0)
			{
				secondLastName = secondLastName.Substring(0, secondLastName.Length >= lastNameLength ? lastNameLength : secondLastName.Length);
			}
			else
			{
				secondLastName = string.Empty;
			}

			string lastNamePlus = lastName;
			if (!string.IsNullOrWhiteSpace(secondLastName))
			{
				lastNamePlus = string.Format("{0} {1}", lastName, secondLastName);
			}

			Dictionary<string, string> fullName = new Dictionary<string, string>();

			//Full name should be 40 incliding spaces
			if ((firstName + middleName + lastNamePlus).Length <= FullNameLength - 2)
			{
				//Do not Truncate
			}
			else if ((firstName + lastNamePlus).Length <= FullNameLength - 3)
			{
				middleName = string.IsNullOrWhiteSpace(middleName) ? string.Empty : middleName.Substring(0, 1);
			}
			else
			{
				middleName = string.Empty;
			}

			fullName.Add("FirstName", firstName);
			fullName.Add("MiddleName", middleName);
			fullName.Add("LastName", lastName);
			fullName.Add("SecondLastName", secondLastName);

			return fullName;
		}
		
		public static bool IsValidZipCode(string zipCode)
		{
			bool isValid = false;

			if (!string.IsNullOrWhiteSpace(zipCode))
			{
				string x = zipCode.Replace("0", "");
				isValid = !string.IsNullOrWhiteSpace(x);
			}

			return isValid;
		}

	}
}
