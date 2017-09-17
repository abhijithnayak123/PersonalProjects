using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using MGI.Common.Util;

namespace MGI.Channel.Shared.Server.Data
{
	[DataContract]
	public class Prospect
	{
		public Prospect()
		{
		}

		[DataMember]
		public string Gender { get; set; }
		[DataMember]
		public string FName { get; set; }
		[DataMember]
		public string MName { get; set; }
		[DataMember]
		public string LName { get; set; }
		[DataMember]
		public string LName2 { get; set; }
		[DataMember]
		public string MoMaName { get; set; }
		[DataMember]
		public DateTime? DateOfBirth { get; set; }
		[DataMember]
		public string Address1 { get; set; }
		[DataMember]
		public string Address2 { get; set; }
		[DataMember]
		public string City { get; set; }
		[DataMember]
		public string State { get; set; }
		[DataMember]
		public string PostalCode { get; set; }
		[DataMember]
		public string Phone1 { get; set; }
		[DataMember]
		public string Phone1Type { get; set; }
		[DataMember]
		public string Phone1Provider { get; set; }
		[DataMember]
		public string Phone2 { get; set; }
		[DataMember]
		public string Phone2Type { get; set; }
		[DataMember]
		public string Phone2Provider { get; set; }
		[DataMember]
		public string PIN { get; set; }
		[DataMember]
		public bool TextMsgOptIn { get; set; }
		[DataMember]
		public string Email { get; set; }
		[DataMember]
		public string Occupation { get; set; }
		[DataMember]
		public string OccupationDescription { get; set; }
		[DataMember]
		public string Employer { get; set; }
		[DataMember]
		public string EmployerPhone { get; set; }
		[DataMember]
		public string SSN { get; set; }
		[DataMember]
		public Identification ID { get; set; }
		[DataMember]
		public bool IsAccountHolder { get; set; }
		[DataMember]
		public string ReferralCode { get; set; }
		[DataMember]
		public bool DoNotCall { get; set; }
		[DataMember]
		public string CardNumber { get; set; }
		[DataMember]
		public string WUGoldCardNumber { get; set; }
		[DataMember]
		public bool WUSMSNotification { get; set; }
		[DataMember]
		public bool MailingAddressDifferent { get; set; }
		[DataMember]
		public string MailingAddress1 { get; set; }
		[DataMember]
		public string MailingAddress2 { get; set; }
		[DataMember]
		public string MailingCity { get; set; }
		[DataMember]
		public string MailingState { get; set; }
		[DataMember]
		public string MailingZipCode { get; set; }
		[DataMember]
		public Guid ChannelPartnerId { get; set; }
		[DataMember]
		public string ReceiptLanguage { get; set; }
		[DataMember]
		public ProfileStatus ProfileStatus { get; set; }
		[DataMember]
		public List<string> Groups { get; set; }
		[DataMember]
		public ProfileStatus ClientProfileStatus { get; set; }
		[DataMember]
		public string PartnerAccountNumber { get; set; }
		[DataMember]
		public string RelationshipAccountNumber { get; set; }
		[DataMember]
		public string BankId { get; set; }
		[DataMember]
		public string BranchId { get; set; }
		[DataMember]
		public string ProgramId { get; set; }
		[DataMember]
		public string Notes { get; set; }
		[DataMember]
		public string ClientID { get; set; }
		[DataMember]
		public string LegalCode { get; set; }
		[DataMember]
		public string PrimaryCountryCitizenShip { get; set; }
		[DataMember]
		public string SecondaryCountryCitizenShip { get; set; }
		[DataMember]
		public CustomerScreen CustomerScreen { get; set; }


		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("Prospect:");
			sb.AppendLine(string.Format("	Gender: {0}", Gender));
			sb.AppendLine(string.Format("	FName: {0}", FName));
			sb.AppendLine(string.Format("	MName: {0}", MName));
			sb.AppendLine(string.Format("	Gender: {0}", Gender));
			sb.AppendLine(string.Format("	LName: {0}", LName));
			sb.AppendLine(string.Format("	LName2: {0}", LName2));
			sb.AppendLine(string.Format("	MoMaName: {0}", MoMaName));
			sb.AppendLine(string.Format("	Date of Birth: {0}", DateOfBirth != null ? DateOfBirth.Value.ToShortDateString() : null));
			sb.AppendLine(string.Format("	Address1: {0}", Address1));
			sb.AppendLine(string.Format("	Address2: {0}", Address2));
			sb.AppendLine(string.Format("	City: {0}", City));
			sb.AppendLine(string.Format("	State: {0}", State));
			sb.AppendLine(string.Format("	PostalCode: {0}", PostalCode));
			sb.AppendLine(string.Format("	Phone1: {0}", Phone1));
			sb.AppendLine(string.Format("	Phone1Type: {0}", Phone1Type));
			sb.AppendLine(string.Format("	Phone1Provider: {0}", Phone1Provider));
			sb.AppendLine(string.Format("	Phone2: {0}", Phone2));
			sb.AppendLine(string.Format("	Phone2Type: {0}", Phone2Type));
			sb.AppendLine(string.Format("	Phone2Provider: {0}", Phone2Provider));
			sb.AppendLine(string.Format("	PIN Length: {0}", string.IsNullOrEmpty(PIN) ? 0 : PIN.Length));
			sb.AppendLine(string.Format("	TextMsgOptIn: {0}", TextMsgOptIn));
			sb.AppendLine(string.Format("	Email: {0}", Email));
			sb.AppendLine(string.Format("	Occupation: {0}", Occupation));
			sb.AppendLine(string.Format("	Description: {0}", OccupationDescription));
			sb.AppendLine(string.Format("	Employer: {0}", Employer));
			sb.AppendLine(string.Format("	EmployerPhone: {0}", EmployerPhone));
			sb.AppendLine(string.Format("	SSN Length: {0}", string.IsNullOrEmpty(SSN) ? 0 : SSN.Length));
			sb.AppendLine(string.Format("	{0}", ID));
			sb.AppendLine(string.Format("	IsAccountHolder: {0}", IsAccountHolder));
			sb.AppendLine(string.Format("	ReferralCode: {0}", ReferralCode));
			sb.AppendLine(string.Format("	DoNotCall: {0}", DoNotCall));
			sb.AppendLine(string.Format("	MailingAddressDifferent: {0}", MailingAddressDifferent));
			sb.AppendLine(string.Format("	MailingAddress1: {0}", MailingAddress1));
			sb.AppendLine(string.Format("	MailingAddress2: {0}", MailingAddress2));
			sb.AppendLine(string.Format("	MailingCity: {0}", MailingCity));
			sb.AppendLine(string.Format("	MailingState: {0}", MailingState));
			sb.AppendLine(string.Format("	MailingZipCode: {0}", MailingZipCode));
			sb.AppendLine(string.Format("   ChannelPartnerId : {0}", ChannelPartnerId));
			sb.AppendLine(string.Format("   ReceiptLanguage : {0}", ReceiptLanguage));
			sb.AppendLine(string.Format("   ProfileStatus : {0}", ProfileStatus));
			if (!string.IsNullOrEmpty(CardNumber))
				sb.AppendLine(string.Format("   CardNumber: {0}", string.Format("xx{0}", CardNumber.Substring(CardNumber.Length - 4))));
			else
				sb.AppendLine("   CardNumber is empty");
			if (!string.IsNullOrEmpty(WUGoldCardNumber))
				sb.AppendLine(string.Format("	WUGoldCardNumber: {0}", WUGoldCardNumber));
			else
				sb.AppendLine("   GoldCardNumber is empty");
			sb.AppendLine(string.Format("	WUSMSNotification: {0}", WUSMSNotification));
			sb.AppendLine(string.Format("	WUSMSNotification: {0}", WUSMSNotification));
			sb.AppendLine(string.Format("   Groups: {0}", string.Join(",", Groups.ToArray())));

			sb.AppendLine(string.Format("   CustomerProfileStatus : {0}", ClientProfileStatus));
			if (!string.IsNullOrEmpty(PartnerAccountNumber))
				sb.AppendLine(string.Format("   PartnerAccountNumber : {0}", PartnerAccountNumber.Substring(0, 6) + "XXXXXX" + PartnerAccountNumber.Substring(PartnerAccountNumber.Length - 4, 4)));

			else
				sb.AppendLine("   PartnerAccountNumber is empty");
			if (!string.IsNullOrEmpty(RelationshipAccountNumber))
				sb.AppendLine(string.Format("   RelationshipAccountNumber : {0}", RelationshipAccountNumber));
			else
				sb.AppendLine("   RelationshipAccountNumber is empty");
			if (!string.IsNullOrEmpty(BankId))
				sb.AppendLine(string.Format("   BankId : {0}", BankId));
			else
				sb.AppendLine("   BankId is empty");
			if (!string.IsNullOrEmpty(BranchId))
				sb.AppendLine(string.Format("   BranchId : {0}", BranchId));
			else
				sb.AppendLine("   BranchId is empty");
			sb.AppendLine(string.Format("   ProgramId : {0}", ProgramId));
			sb.AppendLine(string.Format("	ClientID: {0}", ClientID));
			sb.AppendLine(string.Format("	LegalCode: {0}", LegalCode));
			sb.AppendLine(string.Format("	PrimaryCountryCitizenShip: {0}", PrimaryCountryCitizenShip));
			sb.AppendLine(string.Format("	SecondaryCountryCitizenShip: {0}", SecondaryCountryCitizenShip));
			sb.AppendLine(string.Format("	CustomerScreen: {0}", CustomerScreen));

			return sb.ToString();
		}
	}
}

