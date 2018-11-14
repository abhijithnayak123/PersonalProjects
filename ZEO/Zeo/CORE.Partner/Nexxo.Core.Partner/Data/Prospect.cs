using System;
using System.Collections.Generic;
using System.Linq;
using MGI.Common.Util;

namespace MGI.Core.Partner.Data
{
	public class Prospect : BaseProfile
	{
		public virtual long AlloyID { get; set; }
		public virtual ProspectGovernmentId GovernmentId { get; set; }
		public virtual ProspectEmploymentDetails EmploymentDetails { get; set; }
		public virtual bool IsAccountHolder { get; set; }
		public virtual string ReferralCode { get; set; }
        public virtual string ReceiptLanguage { get; set; }
		public virtual ProfileStatus ProfileStatus { get; set; }
		public virtual IList<ProspectGroupSetting> Groups { get; set; }
		public virtual string Notes { get; set; }
		public virtual ChannelPartner ChannelPartner { get; set; }
		public virtual string IDCode { get; set; }
		public Prospect()
		{
			Groups = new List<ProspectGroupSetting>();
		}

		public virtual void AddGovernmentId( NexxoIdType idType, string identification, Nullable<DateTime> issueDate, Nullable<DateTime> expirationDate )
		{
			this.GovernmentId = new ProspectGovernmentId
			{
				IdType = idType,
				Identification = identification,
                IssueDate = issueDate,
                ExpirationDate = expirationDate,
				DTServerCreate = DateTime.Now,
				Prospect = this
			};
		}

		public virtual void UpdateGovernmentId( NexxoIdType idType, string identification, Nullable<DateTime> issueDate, Nullable<DateTime> expirationDate )
		{
			this.GovernmentId.IdType = idType;
			this.GovernmentId.Identification = identification;
            this.GovernmentId.IssueDate = issueDate;
            this.GovernmentId.ExpirationDate = expirationDate;
			this.GovernmentId.DTServerLastModified = DateTime.Now;
		}

		public virtual void AddEmploymentDetails(string occupation, string description, string employer, string employerPhone)
		{
			this.EmploymentDetails = new ProspectEmploymentDetails
			{
				Occupation = occupation,
				OccupationDescription = description,
				Employer = employer,
				EmployerPhone = employerPhone,
				DTServerCreate = DateTime.Now,
				Prospect = this
			};
		}

		public virtual void UpdateEmploymentDetails(string occupation, string description, string employer, string employerPhone)
		{
			this.EmploymentDetails.Occupation = occupation;
			this.EmploymentDetails.OccupationDescription = description;
			this.EmploymentDetails.Employer = employer;
			this.EmploymentDetails.EmployerPhone = employerPhone;
			this.EmploymentDetails.DTServerLastModified = DateTime.Now;
		}

		public virtual ProspectGroupSetting AddtoGroup(ChannelPartnerGroup g)
		{
			ProspectGroupSetting groupSetting = new ProspectGroupSetting(g);
			groupSetting.prospect = this;
			Groups.Add(groupSetting);
			return groupSetting;
		}
	}
}
