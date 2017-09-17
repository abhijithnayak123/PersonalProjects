using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using MGI.Common.Util;
namespace MGI.Biz.CPEngine.Impl
{
	public static class CheckMapper
	{

		public static INexxoDataStructuresService NexxoIdTypeService { private get; set; }
		public static MGI.Cxn.Check.Data.CheckAccount ToCxnAccount(MGI.Core.CXE.Data.Customer customer, NexxoIdType idType)
		{
			MGI.Cxn.Check.Data.CheckAccount CheckAccount = new Cxn.Check.Data.CheckAccount()
			{
				FirstName = customer.FirstName,
				LastName = customer.LastName,
				SecondLastName = customer.LastName2,
				Address1 = customer.Address1,
				City = customer.City,
				State = customer.State,
				Zip = customer.ZipCode,

				Phone = customer.Phone1,
				SSN = customer.SSN,
				IDCode = string.IsNullOrEmpty(customer.SSN) ? string.Empty : NexxoUtil.GetIDCode(customer.SSN),


				IDState = idType != null && idType.StateId != null ? idType.StateId.Abbr : string.Empty
			};
			if (customer.DateOfBirth != null)
				CheckAccount.DateOfBirth = customer.DateOfBirth;
			if (customer.EmploymentDetails == null)
			{
				CheckAccount.Occupation = string.Empty;
				CheckAccount.Employer = string.Empty;
				CheckAccount.EmployerPhone = string.Empty;
			}
			else
			{
				CheckAccount.Occupation = customer.EmploymentDetails.Occupation;
				CheckAccount.Employer = customer.EmploymentDetails.Employer;
				CheckAccount.EmployerPhone = customer.EmploymentDetails.EmployerPhone;
			}

			if (customer.GovernmentId == null)
			{
				CheckAccount.GovernmentId = string.Empty; // customer.GovernmentId == null ? string.Empty : customer.GovernmentId.Identification;
				CheckAccount.IDCountry = null; // customer.GovernmentId == null ? string.Empty : idType.CountryId.Name;
				CheckAccount.IDExpireDate = null; // customer.GovernmentId == null ? null : customer.GovernmentId.ExpirationDate == DateTime.MinValue ? null : customer.GovernmentId.ExpirationDate;
				CheckAccount.IDType = string.Empty; // customer.GovernmentId == null ? string.Empty : idType.Name;
				CheckAccount.IDIssueDate = null; // customer.GovernmentId == null ? null : customer.GovernmentId.IssueDate == DateTime.MinValue ? null : customer.GovernmentId.IssueDate;
			}
			else
			{
				CheckAccount.GovernmentId = customer.GovernmentId.Identification;
				// 2080 Changes
				CheckAccount.IDCountry = idType != null ? idType.CountryId.Name : null;
				CheckAccount.IDExpireDate = customer.GovernmentId.ExpirationDate == DateTime.MinValue ? null : customer.GovernmentId.ExpirationDate;
				// 2080 Changes
				CheckAccount.IDType = idType != null ? idType.Name : null;
				CheckAccount.IDIssueDate = customer.GovernmentId.IssueDate == DateTime.MinValue ? null : customer.GovernmentId.IssueDate;
			}



			return CheckAccount;
		}
	}
}
