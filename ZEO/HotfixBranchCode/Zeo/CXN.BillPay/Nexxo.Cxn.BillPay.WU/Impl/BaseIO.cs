using AutoMapper;
using MGI.Cxn.WU.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using WUValidate = MGI.Cxn.BillPay.WU.PaymentValidate;
using DAS = MGI.Cxn.BillPay.WU.DASService;
using FusionGo = MGI.Cxn.BillPay.WU.FusionGoShopping;
using WUPayment = MGI.Cxn.BillPay.WU.MakePayment;
using MGI.Cxn.BillPay.WU.Data;
using MGI.Cxn.BillPay.Data;
using MGI.Common.Util;
using MGI.Cxn.WU.Common.Contract;
using MGI.Common.DataProtection.Contract;
using MGI.Cxn.BillPay.Contract;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Cxn.BillPay.WU.Impl
{
	public class BaseIO
	{
		public ProcessorDAL ProcessorDAL { get; set; }
		public IWUCommonIO WesternUnionIO { get; set; }
		public string AllowDuplicateTrxWU { get; set; }
		public IDataProtectionService BPDataProtectionSvc { get; set; }
		public TLoggerCommon MongoDBLogger { get; set; }
		#region Member
		internal X509Certificate2 _certificate = null;
		internal string _serviceUrl = string.Empty;
		internal string _certificateName = string.Empty;
		internal Dictionary<string, string> _deliveryCodeMapping = new Dictionary<string, string>();
		internal const string FUSION_ENDPOINT_NAME = "FusionGo";
		internal const string DAS_ENDPOINT_NAME = "DASInquiry";
		internal const string VALIDATE_ENDPOINT_NAME = "WUValidate";
		internal const string PAYMENT_ENDPOINT_NAME = "WUPayment";
		internal const string PREFERRED_CUSTOMER_DA5 = "DA5";
		internal const string US_COUNTRY_NAME = "United States";
		internal const string QQC_COMPANY_NAME = "GetQQCCompanyName";
		internal const string QQC_FEILDS_TEMPLATE = "GetQQCFieldsTemplate";
		internal const string QQC_ACCOUNT_TEMPLATE = "GetQQCAccountTemplate";
		#endregion

		#region AutoMapper Mapping
		public BaseIO()
		{
			Mapper.CreateMap<Channel, WUValidate.channel>();
			Mapper.CreateMap<Channel, FusionGo.channel>();
			Mapper.CreateMap<Channel, WUPayment.channel>();
			Mapper.CreateMap<Channel, DAS.channel>();

			Mapper.CreateMap<ForeignRemoteSystem, WUValidate.foreign_remote_system>()
				.ForMember(d => d.counter_id, s => s.MapFrom(c => c.CoutnerId));
			Mapper.CreateMap<ForeignRemoteSystem, FusionGo.foreign_remote_system>()
				.ForMember(d => d.counter_id, s => s.MapFrom(c => c.CoutnerId));
			Mapper.CreateMap<ForeignRemoteSystem, WUPayment.foreign_remote_system>()
				.ForMember(d => d.counter_id, s => s.MapFrom(c => c.CoutnerId));
			Mapper.CreateMap<ForeignRemoteSystem, DAS.foreign_remote_system>()
				.ForMember(d => d.counter_id, s => s.MapFrom(c => c.CoutnerId));
			//US2054
			Mapper.CreateMap<SwbFlaInfo, FusionGo.swb_fla_info>()
				.ForMember(d => d.swb_operator_id, s => s.MapFrom(c => c.SwbOperatorId))
				.ForMember(d => d.read_privacynotice_flag, s => s.MapFrom(x => x.ReadPrivacyNoticeFlag))
				.ForMember(d => d.read_privacynotice_flagSpecified, s => s.MapFrom(x => x.FlagCertificationFlagSpecified))
				.ForMember(d => d.fla_certification_flag, s => s.MapFrom(x => x.FlagCertificationFlag))
				.ForMember(d => d.fla_certification_flagSpecified, s => s.MapFrom(x => x.ReadPrivacyNoticeFlagSpecified));

			Mapper.CreateMap<GeneralName, FusionGo.general_name>()
				.ForMember(d => d.name_type, s => s.MapFrom(x => x.Type))
				.ForMember(d => d.name_typeSpecified, s => s.MapFrom(x => x.NameTypeSpecified))
				.ForMember(d => d.first_name, s => s.MapFrom(x => x.FirstName))
				.ForMember(d => d.last_name, s => s.MapFrom(x => x.LastName));

			Mapper.CreateMap<SwbFlaInfo, WUValidate.swb_fla_info>()
				.ForMember(d => d.swb_operator_id, s => s.MapFrom(c => c.SwbOperatorId))
				.ForMember(d => d.read_privacynotice_flag, s => s.MapFrom(x => x.ReadPrivacyNoticeFlag))
				.ForMember(d => d.read_privacynotice_flagSpecified, s => s.MapFrom(x => x.FlagCertificationFlagSpecified))
				.ForMember(d => d.fla_certification_flag, s => s.MapFrom(x => x.FlagCertificationFlag))
				.ForMember(d => d.fla_certification_flagSpecified, s => s.MapFrom(x => x.ReadPrivacyNoticeFlagSpecified));

			Mapper.CreateMap<GeneralName, WUValidate.general_name>()
				.ForMember(d => d.name_type, s => s.MapFrom(x => x.Type))
				.ForMember(d => d.name_typeSpecified, s => s.MapFrom(x => x.NameTypeSpecified))
				.ForMember(d => d.first_name, s => s.MapFrom(x => x.FirstName))
				.ForMember(d => d.last_name, s => s.MapFrom(x => x.LastName));

			Mapper.CreateMap<SwbFlaInfo, WUPayment.swb_fla_info>()
				.ForMember(d => d.swb_operator_id, s => s.MapFrom(c => c.SwbOperatorId))
				.ForMember(d => d.read_privacynotice_flag, s => s.MapFrom(x => x.ReadPrivacyNoticeFlag))
				.ForMember(d => d.read_privacynotice_flagSpecified, s => s.MapFrom(x => x.FlagCertificationFlagSpecified))
				.ForMember(d => d.fla_certification_flag, s => s.MapFrom(x => x.FlagCertificationFlag))
				.ForMember(d => d.fla_certification_flagSpecified, s => s.MapFrom(x => x.ReadPrivacyNoticeFlagSpecified));

			Mapper.CreateMap<GeneralName, WUPayment.general_name>()
				.ForMember(d => d.name_type, s => s.MapFrom(x => x.Type))
				.ForMember(d => d.name_typeSpecified, s => s.MapFrom(x => x.NameTypeSpecified))
				.ForMember(d => d.first_name, s => s.MapFrom(x => x.FirstName))
				.ForMember(d => d.last_name, s => s.MapFrom(x => x.LastName));

			PopulateDeliveryMethods();
		}
		#endregion

		#region Build Methods

		internal void BuildWUCommonObjects(WUBaseRequestResponse wuObjects)
		{
			_certificate = wuObjects.ClientCertificate;
			_serviceUrl = wuObjects.ServiceUrl;
		}

		internal void BuildValidateObjects(WUBaseRequestResponse wuObjects, ref WUValidate.channel channel, ref WUValidate.foreign_remote_system foreignRemoteSystem)
		{
			channel = Mapper.Map<WUValidate.channel>(wuObjects.Channel);
			foreignRemoteSystem = Mapper.Map<WUValidate.foreign_remote_system>(wuObjects.ForeignRemoteSystem);
			BuildWUCommonObjects(wuObjects);
		}

		internal void BuildPaymentObjects(WUBaseRequestResponse wuObjects, ref WUPayment.channel channel, ref WUPayment.foreign_remote_system foreignRemoteSystem)
		{
			channel = Mapper.Map<WUPayment.channel>(wuObjects.Channel);
			foreignRemoteSystem = Mapper.Map<WUPayment.foreign_remote_system>(wuObjects.ForeignRemoteSystem);
			BuildWUCommonObjects(wuObjects);
		}

		internal void BuildFusionGoObjects(WUBaseRequestResponse wuObjects, ref FusionGo.channel channel, ref FusionGo.foreign_remote_system foreignRemoteSystem)
		{
			channel = Mapper.Map<FusionGo.channel>(wuObjects.Channel);
			foreignRemoteSystem = Mapper.Map<FusionGo.foreign_remote_system>(wuObjects.ForeignRemoteSystem);
			BuildWUCommonObjects(wuObjects);
		}

		internal void BuildDASObjects(WUBaseRequestResponse wuObjects, ref DAS.channel channel, ref DAS.foreign_remote_system foreignRemoteSystem)
		{
			channel = Mapper.Map<DAS.channel>(wuObjects.Channel);
			foreignRemoteSystem = Mapper.Map<DAS.foreign_remote_system>(wuObjects.ForeignRemoteSystem);
			BuildWUCommonObjects(wuObjects);
		}

		internal long ConvertDecimalToLong(decimal amount)
		{
			return Convert.ToInt64(amount * 100);
		}

		internal decimal ConvertLongToDecimal(decimal amount)
		{
			return Convert.ToDecimal(amount / 100m);
		}

		internal FusionGo.fusiongoshoppingrequest BuildFusionGoRequest(BillPaymentRequest validateRequest, Location location, WesternUnionAccount account, MGIContext mgiContext, string billerName, string accountNumber, long billAmount)
		{

			FusionGo.iso_code isoCode = new FusionGo.iso_code() { country_code = "US", currency_code = "USD" };
			FusionGo.channel channel = null;
			FusionGo.foreign_remote_system foreignRemoteSystem = null;

			WUBaseRequestResponse wuObjects = (WUBaseRequestResponse)mgiContext.Context["BaseWUObject"];
			BuildFusionGoObjects(wuObjects, ref channel, ref foreignRemoteSystem);
			foreignRemoteSystem.reference_no = DateTime.Now.ToString("yyyyMMddHHmmssffff");

			var routingParams = new FusionGo.routing_params[1];
			if (location != null)
			{
				routingParams[0] = new FusionGo.routing_params()
				{
					routing_param = new string[] { location.Name },
					routing_type = location.Type
				};
			}

			string issueCountry = validateRequest.PrimaryIdCountryOfIssue;
			if (validateRequest.PrimaryIdCountryOfIssue != null)
			{
				if (validateRequest.PrimaryIdType.Equals("PASSPORT")
					|| validateRequest.PrimaryIdType.Equals("EMPLOYMENT AUTHORIZATION CARD (EAD)")
					|| validateRequest.PrimaryIdType.Equals("GREEN CARD / PERMANENT RESIDENT CARD")
					|| validateRequest.PrimaryIdType.Equals("MILITARY ID"))
				{
					issueCountry = WesternUnionIO.GetCountryName(validateRequest.PrimaryIdCountryOfIssue);
				}
			}
			var senderinfo = new FusionGo.sender();
			if (account != null)
			{

				senderinfo = new FusionGo.sender()
				{
					name = new FusionGo.general_name()
					{
						first_name = account.FirstName,
						last_name = account.LastName,
						name_type = FusionGo.name_type.D,
						name_typeSpecified = true
					},
					address = new FusionGo.address()
					{
						addr_line1 = account.Address1,
						city = account.City,
						state = account.State,
						postal_code = account.PostalCode,
						local_area = account.Address2,
						street = account.Street,
						Item = new FusionGo.country_currency_info()
						{
							iso_code = isoCode,
							country_name = US_COUNTRY_NAME
						}
					},
					compliance_details = new FusionGo.compliance_details()
					{
						template_id = ComplianceTemplate.BILL_PAY,
						id_details = new FusionGo.id_details()
						{
							id_type = string.IsNullOrWhiteSpace(validateRequest.PrimaryIdType) ? string.Empty : WesternUnionIO.GetGovtIDType(validateRequest.PrimaryIdType),
							id_number = validateRequest.PrimaryIdNumber,
							id_country_of_issue = issueCountry
						},
						date_of_birth = (account.DateOfBirth == null || account.DateOfBirth == DateTime.MinValue) ? null : account.DateOfBirth.Value.ToString("MMddyyyy"),
						occupation = !string.IsNullOrEmpty(validateRequest.Occupation) ? WesternUnionIO.TrimOccupation(NexxoUtil.MassagingValue(validateRequest.Occupation)) : string.Empty,
						third_party_details = new FusionGo.third_party_details() { flag_pay = "N" }
					},
					mobile_phone = new FusionGo.mobile_phone()
					{
						phone_number = new FusionGo.international_phone_number()
						{
							country_code = "1",
							national_number = ""
						}
					},
					contact_phone = account.ContactPhone,
					sms_notification_flag = FusionGo.sms_notification.Y,
					sms_notification_flagSpecified = true
				};
                if (!string.IsNullOrWhiteSpace(validateRequest.SecondIdNumber))//AL-6260 Should not send <second_id>in WU Request for without SSN/ITIN customer
                {
                    senderinfo.compliance_details.second_id = new FusionGo.id_details()
                    {
                        id_type = string.IsNullOrWhiteSpace(validateRequest.SecondIdType) ? string.Empty : WesternUnionIO.GetGovtIDType(validateRequest.SecondIdType),
                        id_number = validateRequest.SecondIdNumber,
                        id_country_of_issue = US_COUNTRY_NAME//validateRequest.SecondIdCountryOfIssue
                    };
                }

				if (senderinfo.compliance_details.id_details != null && senderinfo.compliance_details.id_details.id_country_of_issue != null)
				{

					if (senderinfo.compliance_details.id_details.id_country_of_issue.Equals("US")
						&& (validateRequest.PrimaryIdType.Equals("DRIVER'S LICENSE") || validateRequest.PrimaryIdType.Equals("U.S. STATE IDENTITY CARD")))
					{
						senderinfo.compliance_details.id_details.id_country_of_issue = validateRequest.PrimaryIdCountryOfIssue + "/" + validateRequest.PrimaryIdPlaceOfIssue;
					}
					else if (senderinfo.compliance_details.id_details.id_country_of_issue.Equals("MX"))
					{
						senderinfo.compliance_details.id_details.id_country_of_issue = "Mexico";
					}
				}

			}


			FusionGo.fusiongoshoppingrequest request = new FusionGo.fusiongoshoppingrequest()
			{
				channel = channel,
				qp_company = new FusionGo.qp_company()
				{
					company_name = billerName,
					debtor_account_number = accountNumber,
					country = new FusionGo.country_currency_info() { iso_code = isoCode },
					city_code = validateRequest.Location,

				},
				payment_details = new FusionGo.payment_details()
				{
					recording_country_currency = new FusionGo.country_currency_info() { iso_code = isoCode },
					destination_country_currency = new FusionGo.country_currency_info() { iso_code = isoCode },
					originating_country_currency = new FusionGo.country_currency_info() { iso_code = isoCode },
					payment_typeSpecified = true,
					payment_type = FusionGo.payment_type.Cash,
					auth_status = "VLD",
					fix_on_send = FusionGo.yes_no.@false,
					money_transfer_typeSpecified = true,
					money_transfer_type = FusionGo.money_transfer_type.QQC,
				},
				foreign_remote_system = foreignRemoteSystem,
				financials = new FusionGo.financials()
				{
					originators_principal_amount = billAmount,
					originators_principal_amountSpecified = true
				},
				fusion = new FusionGo.fusion()
				{
					fusion_screen = FusionGo.fusion_screen.MP,
					fusion_screenSpecified = true,
					routing = location != null ? routingParams : null
				},
				sender = senderinfo

			};
			request.swb_fla_info = Mapper.Map<SwbFlaInfo, FusionGo.swb_fla_info>(WesternUnionIO.BuildSwbFlaInfo(mgiContext));
			request.swb_fla_info.fla_name = Mapper.Map<GeneralName, FusionGo.general_name>(WesternUnionIO.BuildGeneralName(mgiContext));

			return request;

		}

		internal WUValidate.payment_transactions BuildPaymentTransactions(BillPaymentRequest validateRequest, WesternUnionAccount account)
		{
			WUValidate.iso_code isoCode = new WUValidate.iso_code() { country_code = "US", currency_code = "USD" };

			//When a Passport is used as the form of ID in the customer profile
			//In order for WU transaction to work correctly with Passports the id_country_of_issue field should use country name instead of country code 				
			string issueCountry = validateRequest.PrimaryIdCountryOfIssue;

			if (validateRequest.PrimaryIdType != null)
				if (validateRequest.PrimaryIdType.Equals("PASSPORT")
				|| validateRequest.PrimaryIdType.Equals("EMPLOYMENT AUTHORIZATION CARD (EAD0")
				|| validateRequest.PrimaryIdType.Equals("GREEN CARD / PERMANENT RESIDENT CARD")
				|| validateRequest.PrimaryIdType.Equals("MILITARY ID"))
				{
					issueCountry = WesternUnionIO.GetCountryName(validateRequest.PrimaryIdCountryOfIssue);
				}

			WUValidate.payment_transaction transaction = new WUValidate.payment_transaction()
			{
				sender = new WUValidate.sender()
				{
					name = new WUValidate.general_name()
					{
						first_name = account.FirstName,
						last_name = account.LastName,
						name_type = WUValidate.name_type.D,
						name_typeSpecified = true
					},
					address = new WUValidate.address()
					{
						addr_line1 = account.Address1,
						city = account.City,
						state = account.State,
						postal_code = account.PostalCode,
						local_area = account.Address2,
						street = account.Street,
						Item = new WUValidate.country_currency_info()
						{
							iso_code = isoCode,
							country_name = US_COUNTRY_NAME
						}
					},
					preferred_customer = new WUValidate.preferred_customer()
					{
						account_nbr = account.CardNumber,
						level_code = PREFERRED_CUSTOMER_DA5
					},
					email = account.Email,
					date_of_birth = (account.DateOfBirth == DateTime.MinValue || account.DateOfBirth == null) ? null : account.DateOfBirth.Value.ToString("MM/dd/yyyy"),
					//					
					compliance_details = new WUValidate.compliance_details()
					{
						template_id = ComplianceTemplate.BILL_PAY,
						id_details = new WUValidate.id_details()
						{
							id_type = !string.IsNullOrEmpty(validateRequest.PrimaryIdType) ? WesternUnionIO.GetGovtIDType(validateRequest.PrimaryIdType) : string.Empty,
							id_number = !string.IsNullOrEmpty(validateRequest.PrimaryIdNumber) ? validateRequest.PrimaryIdNumber : string.Empty,
							id_country_of_issue = !string.IsNullOrEmpty(issueCountry) ? issueCountry : string.Empty,
							id_place_of_issue = !string.IsNullOrEmpty(validateRequest.PrimaryIdPlaceOfIssue) ? validateRequest.PrimaryIdPlaceOfIssue : string.Empty
						},
						Current_address = new WUValidate.compliance_address()
						{
							addr_line1 = account.Address1,
							addr_line2 = account.Address2,
							city = account.City,
							state_code = account.State,
							postal_code = account.PostalCode,
							street = account.Street
						},
						date_of_birth = (account.DateOfBirth == DateTime.MinValue || account.DateOfBirth == null) ? null : account.DateOfBirth.Value.ToString("MM/dd/yyyy"),
						//					
						occupation = WesternUnionIO.TrimOccupation(NexxoUtil.MassagingValue(validateRequest.Occupation)),
						ack_flag = "X",
						third_party_details = new WUValidate.third_party_details() { flag_pay = "N" },
						Country_of_Birth = validateRequest.CountryOfBirth
					},
					contact_phone = account.ContactPhone
				},
				qp_company = new WUValidate.qp_company()
				{
					company_name = validateRequest.BillerName,
					city_code = validateRequest.Location,
					debtor_account_number = validateRequest.AccountNumber,
					country = new WUValidate.country_currency_info() { iso_code = isoCode },
					account_holder_name = validateRequest.AccountHolder,
					reference_no = validateRequest.Reference,
					available_balance = validateRequest.AailableBalance,
					department = new string[] { validateRequest.Attention }
				},
				financials = new WUValidate.financials()
				{
					originators_principal_amountSpecified = true,
					originators_principal_amount = ConvertDecimalToLong(validateRequest.Amount),
					gross_total_amount = ConvertDecimalToLong((validateRequest.Amount + validateRequest.Fee)),
					gross_total_amountSpecified = true,
					charges = ConvertDecimalToLong(validateRequest.Fee),
					chargesSpecified = true,
				},
				payment_details = new WUValidate.payment_details()
				{
					recording_country_currency = new WUValidate.country_currency_info() { iso_code = isoCode },
					destination_country_currency = new WUValidate.country_currency_info() { iso_code = isoCode },
					originating_country_currency = new WUValidate.country_currency_info() { iso_code = isoCode },
					transaction_typeSpecified = true,
					transaction_type = WUValidate.transaction_type.QQC,
					payment_typeSpecified = true,
					payment_type = WUValidate.payment_type.Cash,
					exchange_rate = 1.00000000,
					exchange_rateSpecified = true,
					fix_on_send = WUValidate.yes_no.@false,
					fix_on_sendSpecified = true,
					receipt_opt_out = WUValidate.yes_no.@false,
					receipt_opt_outSpecified = true,
					auth_status = "VLD",
					duplicate_detection_flag = AllowDuplicateTrxWU
				},
				delivery_services = new WUValidate.delivery_services()
				{
					code = validateRequest.DeliveryCode
				},
				fusion = new WUValidate.fusion()
				{
					fusion_screen = WUValidate.fusion_screen.MP,
					fusion_screenSpecified = true
				},
				conv_session_cookie = validateRequest.SessionCookie,
				promotions = new WUValidate.promotions()
				{
					coupons_promotions = string.IsNullOrWhiteSpace(validateRequest.CouponCode) ? string.Empty : validateRequest.CouponCode
				}
			};
            if (!string.IsNullOrWhiteSpace(validateRequest.SecondIdNumber)) //AL-6260 Should not send <second_id>in WU Request for without SSN/ITIN customer
            {
                transaction.sender.compliance_details.second_id = new WUValidate.id_details()
                {
                    id_type = !string.IsNullOrEmpty(validateRequest.SecondIdType) ? WesternUnionIO.GetGovtIDType(validateRequest.SecondIdType) : string.Empty,
                    id_number = !string.IsNullOrEmpty(validateRequest.SecondIdNumber) ? validateRequest.SecondIdNumber : string.Empty,
                    id_country_of_issue = "United States"//validateRequest.SecondIdCountryOfIssue
                };
            }

			if (transaction.sender.compliance_details.id_details != null && transaction.sender.compliance_details.id_details.id_country_of_issue != null)
			{

				if (transaction.sender.compliance_details.id_details.id_country_of_issue.Equals("US")
					&& (validateRequest.PrimaryIdType.Equals("DRIVER'S LICENSE") || validateRequest.PrimaryIdType.Equals("U.S. STATE IDENTITY CARD")
					|| validateRequest.PrimaryIdType.Equals("NEW YORK CITY ID") || validateRequest.PrimaryIdType.Equals("NEW YORK BENEFITS ID")))
				{
					transaction.sender.compliance_details.id_details.id_country_of_issue = validateRequest.PrimaryIdCountryOfIssue + "/" + validateRequest.PrimaryIdPlaceOfIssue;
				}
				else if (transaction.sender.compliance_details.id_details.id_country_of_issue.Equals("MX"))
				{
					transaction.sender.compliance_details.id_details.id_country_of_issue = "Mexico";
				}
			}
			WUValidate.payment_transactions payments = new WUValidate.payment_transactions();
			payments.payment_transaction = new WUValidate.payment_transaction[1] { transaction };

			return payments;
		}

		internal WUPayment.payment_transactions BuildPaymentStoreTransactions(WesternUnionTrx trx, RequestType requestStatus)
		{
			WUPayment.iso_code isoCode = new WUPayment.iso_code() { country_code = "US", currency_code = "USD" };

			WUPayment.mt_requested_status requestedStatus = GetRequestType(requestStatus);

			WUPayment.payment_transaction transaction = new WUPayment.payment_transaction()
			{
				sender = new WUPayment.sender()
				{
					name = new WUPayment.general_name()
					{
						first_name = trx.WesternUnionAccount.FirstName,
						last_name = trx.WesternUnionAccount.LastName,
						name_type = WUPayment.name_type.D,
						name_typeSpecified = true
					},
					address = new WUPayment.address()
					{
						addr_line1 = trx.WesternUnionAccount.Address1,
						city = trx.WesternUnionAccount.City,
						state = trx.WesternUnionAccount.State,
						postal_code = trx.WesternUnionAccount.PostalCode,
						local_area = trx.WesternUnionAccount.Address2,
						street = trx.WesternUnionAccount.Street,
						Item = new WUPayment.country_currency_info()
						{
							iso_code = isoCode,
							country_name = US_COUNTRY_NAME
						}
					},
					preferred_customer = new WUPayment.preferred_customer()
					{
						account_nbr = trx.WesternUnionAccount.CardNumber,
						level_code = PREFERRED_CUSTOMER_DA5
					},
					email = trx.WesternUnionAccount.Email,
					contact_phone = trx.WesternUnionAccount.ContactPhone,
					date_of_birth = (trx.WesternUnionAccount.DateOfBirth == DateTime.MinValue || trx.WesternUnionAccount.DateOfBirth == null) ? null : trx.WesternUnionAccount.DateOfBirth.Value.ToString("MM/dd/yyyy"),
					//					
					compliance_details = new WUPayment.compliance_details()
					{
						compliance_data_buffer = trx.SenderComplianceDetailsComplianceDataBuffer
					}
				},
				qp_company = new WUPayment.qp_company()
				{
					company_name = trx.BillerName,
					city_code = trx.BillerCityCode,
					debtor_account_number = Decrypt(trx.CustomerAccountNumber),
					country = new WUPayment.country_currency_info() { iso_code = isoCode },
					department = new string[] { trx.QPCompanyDepartment }
				},
				financials = new WUPayment.financials()
				{
					originators_principal_amount = ConvertDecimalToLong(trx.FinancialsOriginatorsPrincipalAmount),
					originators_principal_amountSpecified = true,
					destination_principal_amount = ConvertDecimalToLong(trx.FinancialsDestinationPrincipalAmount),
					destination_principal_amountSpecified = true,
					gross_total_amount = ConvertDecimalToLong(trx.FinancialsOriginatorsPrincipalAmount + trx.FinancialsFee),
					gross_total_amountSpecified = true,
					plus_charges_amount = trx.FinancialsPlusChargesAmount > 0 ? ConvertDecimalToLong((decimal)trx.FinancialsPlusChargesAmount) : 0,
					plus_charges_amountSpecified = true,
					charges = ConvertDecimalToLong(trx.FinancialsFee),
					chargesSpecified = true,
					total_undiscounted_charges = trx.FinancialsUndiscountedCharges > 0 ? ConvertDecimalToLong((decimal)trx.FinancialsUndiscountedCharges) : 0,
					total_undiscounted_chargesSpecified = true,
					total_discount = trx.FinancialsTotalDiscount > 0 ? ConvertDecimalToLong((decimal)trx.FinancialsTotalDiscount) : 0,
					total_discountSpecified = true,
					total_discounted_charges = trx.FinancialsDiscountedCharges > 0 ? ConvertDecimalToLong((decimal)trx.FinancialsDiscountedCharges) : 0,
					total_discounted_chargesSpecified = true
				},
				payment_details = new WUPayment.payment_details()
				{
					recording_country_currency = new WUPayment.country_currency_info() { iso_code = isoCode },
					destination_country_currency = new WUPayment.country_currency_info() { iso_code = isoCode },
					originating_country_currency = new WUPayment.country_currency_info() { iso_code = isoCode },
					transaction_typeSpecified = true,
					transaction_type = WUPayment.transaction_type.QQC,
					payment_typeSpecified = true,
					payment_type = WUPayment.payment_type.Cash,
					exchange_rate = trx.PaymentDetailsExchangeRate,
					exchange_rateSpecified = true,
					fix_on_send = WUPayment.yes_no.@false,
					fix_on_sendSpecified = true,
					receipt_opt_out = WUPayment.yes_no.@false,
					receipt_opt_outSpecified = true,
					auth_status = trx.PaymentDetailsAuthStatus,
					mt_requested_status = requestedStatus,
					mt_requested_statusSpecified = true,
					duplicate_detection_flag = AllowDuplicateTrxWU
				},
				promotions = new WUPayment.promotions()
				{
					promo_code_description = !string.IsNullOrWhiteSpace(trx.PromotionsPromoCodeDescription) ? trx.PromotionsPromoCodeDescription : string.Empty,
					promo_sequence_no = !string.IsNullOrWhiteSpace(trx.PromotionsPromoSequenceNo) ? trx.PromotionsPromoSequenceNo : string.Empty,
					promo_name = !string.IsNullOrWhiteSpace(trx.PromotionsPromoName) ? trx.PromotionsPromoName : string.Empty,
					promo_message = !string.IsNullOrWhiteSpace(trx.PromotionsPromoMessage) ? trx.PromotionsPromoMessage : string.Empty,
					promo_discount_amount = ConvertDecimalToLong(trx.PromotionsPromoDiscountAmount),
					promotion_error = !string.IsNullOrWhiteSpace(trx.PromotionsPromotionError) ? trx.PromotionsPromotionError : string.Empty,
					sender_promo_code = !string.IsNullOrWhiteSpace(trx.PromotionsSenderPromoCode) ? trx.PromotionsSenderPromoCode : string.Empty
				},
				delivery_services = new WUPayment.delivery_services()
				{
					code = trx.DeliveryCode
				},
				fusion = new WUPayment.fusion()
				{
					fusion_screen = WUPayment.fusion_screen.MP,
					fusion_screenSpecified = true
				},
				conv_session_cookie = FindAndDetokenizeCardNumber(trx.ConvSessionCookie, trx.CustomerAccountNumber),
				mtcn = trx.Mtcn,
				new_mtcn = trx.NewMTCN
			};

			WUPayment.payment_transactions payments = new WUPayment.payment_transactions();
			payments.payment_transaction = new WUPayment.payment_transaction[1] { transaction };

			return payments;
		}

		internal static WUPayment.mt_requested_status GetRequestType(RequestType requestStatus)
		{
			WUPayment.mt_requested_status requestedStatus = WUPayment.mt_requested_status.HOLD;

			if (requestStatus == RequestType.HOLD)
			{
				requestedStatus = WUPayment.mt_requested_status.HOLD;
			}
			else if (requestStatus == RequestType.RELEASE)
			{
				requestedStatus = WUPayment.mt_requested_status.RELEASE;
			}
			else if (requestStatus == RequestType.CANCEL)
			{
				requestedStatus = WUPayment.mt_requested_status.CANCEL;
			}
			return requestedStatus;
		}

		internal string ParseBillerMessage(DAS.h2hdasreply response, out string errorMessage)
		{
			string message = string.Empty;
			errorMessage = string.Empty;
			if (response != null)
			{
				DAS.REPLYType rType = (DAS.REPLYType)response.MTML.Item;
				if (rType != null && rType.DATA_CONTEXT != null)
				{
					if (rType.DATA_CONTEXT.RECORDSET != null)
					{
						var templateLineTypes = rType.DATA_CONTEXT.RECORDSET.Items;
						if (templateLineTypes != null && templateLineTypes.Count() > 0)
						{
							DASService.DASTEMPLATELINE_Type firstTemplateLineType = (DASService.DASTEMPLATELINE_Type)templateLineTypes.FirstOrDefault();
							if (firstTemplateLineType != null && !string.IsNullOrWhiteSpace(firstTemplateLineType.DESCRIPTION))
							{
								string count = firstTemplateLineType.DESCRIPTION.Split(';')[0];
								int messageCount = Convert.ToInt32(count);

								StringBuilder messageBuilder = new StringBuilder();

								for (int index = 1; index <= messageCount; index++)
								{
									DASService.DASTEMPLATELINE_Type templateLineType = (DASService.DASTEMPLATELINE_Type)templateLineTypes[index];
									string description = templateLineType.DESCRIPTION;
									if (!string.IsNullOrWhiteSpace(description))
									{
										description = description.Split(';')[0];
										description = description.Substring(7);
										messageBuilder.Append(description);
										messageBuilder.Append(" ");
									}
								}
								message = messageBuilder.ToString();
							}
						}
					}
					if (rType.DATA_CONTEXT.HEADER != null)
					{
						errorMessage = rType.DATA_CONTEXT.HEADER.ERROR_MSG;
					}
				}
			}
			return message;
		}

		internal List<Field> ParseBillerFields(DAS.h2hdasreply response, out string errorMessage)
		{
			List<Field> fields = new List<Field>();
			errorMessage = string.Empty;
			if (response != null)
			{
				DAS.REPLYType rType = (DAS.REPLYType)response.MTML.Item;
				if (rType != null && rType.DATA_CONTEXT != null)
				{
					if (rType.DATA_CONTEXT.RECORDSET != null)
					{
						var templateLineTypes = rType.DATA_CONTEXT.RECORDSET.Items;
						if (templateLineTypes != null && templateLineTypes.Count() > 0)
						{
							DASService.DASQQCFIELDSTEMPLATE_Type firstFieldTemplate = (DASService.DASQQCFIELDSTEMPLATE_Type)templateLineTypes.FirstOrDefault();
							if (firstFieldTemplate != null && !string.IsNullOrWhiteSpace(firstFieldTemplate.DESCRIPTION))
							{
								string count = firstFieldTemplate.DESCRIPTION.Split(';')[0];
								int fieldCount = Convert.ToInt32(count);

								for (int index = 1; index <= fieldCount; index++)
								{
									DASService.DASQQCFIELDSTEMPLATE_Type templateLineType = (DASService.DASQQCFIELDSTEMPLATE_Type)templateLineTypes[index];
									string description = templateLineType.DESCRIPTION;
									if (!string.IsNullOrWhiteSpace(description))
									{
										string[] fieldArray = description.Split(';');
										fields.Add(new Field()
										{
											Label = fieldArray[0].Trim('*', ' '),
											MaxLength = Convert.ToInt32(fieldArray[1]),
											ValidationMessage = BuildValidationMessage(fieldArray[5].Trim(), fieldArray[0].Trim(), fieldArray[1]),
											DataType = fieldArray[5].Trim(),
											Mask = fieldArray[8].Trim(),
											IsMandatory = fieldArray[3].Contains("Required")
										});
									}
								}
							}
						}
					}
					if (rType.DATA_CONTEXT.HEADER != null)
					{
						errorMessage = rType.DATA_CONTEXT.HEADER.ERROR_MSG;
					}
				}
			}
			return fields;
		}

		internal DASService.QQCCOMPANYNAME_Type ParseBiller(DAS.h2hdasreply response, string billerName, out string errorMessage)
		{
			DASService.QQCCOMPANYNAME_Type biller = null;
			errorMessage = string.Empty;
			if (response != null)
			{
				DAS.REPLYType rType = (DAS.REPLYType)response.MTML.Item;
				if (rType != null && rType.DATA_CONTEXT != null)
				{
					if (rType.DATA_CONTEXT.RECORDSET != null)
					{
						var templateLineTypes = rType.DATA_CONTEXT.RECORDSET.Items;

						if (templateLineTypes != null && templateLineTypes.Length > 0)
						{
							List<DASService.QQCCOMPANYNAME_Type> matchingBillers = new List<DAS.QQCCOMPANYNAME_Type>();
							for (int index = 0; index < templateLineTypes.Length; index++)
							{
								DASService.QQCCOMPANYNAME_Type thisBiller = (DASService.QQCCOMPANYNAME_Type)templateLineTypes[index];
								if (string.Compare(thisBiller.COMPANY_NAME, billerName, true) == 0)
								{
									matchingBillers.Add(thisBiller);
								}
							}
							if (matchingBillers.Count > 0)
							{
								biller = (DASService.QQCCOMPANYNAME_Type)templateLineTypes.FirstOrDefault();
							}
						}
					}
					if (rType.DATA_CONTEXT.HEADER != null)
					{
						errorMessage = rType.DATA_CONTEXT.HEADER.ERROR_MSG;
					}
				}
			}
			return biller;
		}

		internal string ParseBillerTemplate(DAS.h2hdasreply response, out string errorMessage)
		{
			string template = string.Empty;
			errorMessage = string.Empty;
			if (response != null)
			{
				DAS.REPLYType rType = (DAS.REPLYType)response.MTML.Item;
				if (rType != null && rType.DATA_CONTEXT != null)
				{
					if (rType.DATA_CONTEXT.RECORDSET != null)
					{
						var templateLineTypes = rType.DATA_CONTEXT.RECORDSET.Items;
						if (templateLineTypes != null && templateLineTypes.Count() > 0)
						{
							DASService.QQCCOMPANYNAME_Type firstFieldTemplate = (DASService.QQCCOMPANYNAME_Type)templateLineTypes.FirstOrDefault();
							if (firstFieldTemplate != null)
							{
								template = firstFieldTemplate.TEMPLT;
							}
						}
					}
					if (rType.DATA_CONTEXT.HEADER != null)
					{
						errorMessage = rType.DATA_CONTEXT.HEADER.ERROR_MSG;
					}
				}
			}
			return template;
		}

		internal FusionGo.fusiongoshoppingreply InvokeFusionGoRequest(FusionGo.fusiongoshoppingrequest request)
		{
			FusionGo.FusionGoShopping_Service_PortTypeClient client = new FusionGo.FusionGoShopping_Service_PortTypeClient(FUSION_ENDPOINT_NAME, _serviceUrl);
			client.ClientCredentials.ClientCertificate.Certificate = _certificate;

			#region AL-1014 Transactional Log User Story
			MGIContext mgiContext = new MGIContext();
			MongoDBLogger.Info<FusionGo.fusiongoshoppingrequest>(0, request, "InvokeFusionGoRequest", AlloyLayerName.CXN,
				ModuleName.BillPayment, "InvokeFusionGoRequest -MGI.Cxn.BillPay.WU.Impl.BaseIO", "REQUEST", mgiContext);
			#endregion
			FusionGo.fusiongoshoppingreply reply = client.FusionGoShopping(request);

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<FusionGo.fusiongoshoppingreply>(0, reply, "InvokeFusionGoRequest", AlloyLayerName.CXN,
				ModuleName.BillPayment, "InvokeFusionGoRequest -MGI.Cxn.BillPay.WU.Impl.BaseIO", "RESPONSE", mgiContext);
			#endregion
			return reply;
		}

		internal Fee ParseDeliveryMethods(FusionGo.fusiongoshoppingreply reply)
		{
			Fee fee = null;
			string accountHolderName = string.Empty;
			string availableBalance = string.Empty;

			if (reply != null)
			{
				if (reply.payment_transactions.payment_transaction.Length > 0)
				{
					var paymentTransactions = reply.payment_transactions.payment_transaction;
					if (paymentTransactions.FirstOrDefault().financials != null)
					{
						var paymentTransaction = paymentTransactions.FirstOrDefault();

						if (paymentTransaction != null && paymentTransaction.qp_company != null)
						{
							var customerInfo = paymentTransaction.qp_company;

							if (!string.IsNullOrWhiteSpace(customerInfo.account_holder_name))
							{
								accountHolderName = customerInfo.account_holder_name;
							}
							if (!string.IsNullOrWhiteSpace(customerInfo.available_balance))
							{
								availableBalance = customerInfo.available_balance;
							}
						}
						FusionGo.financials financials = paymentTransactions.FirstOrDefault().financials;
						if (financials != null)
						{
							var deliveryMethods = new List<DeliveryMethod>();

							foreach (var delivery in financials.speed_of_delivery)
							{
								deliveryMethods.Add(new DeliveryMethod()
								{
									Code = delivery.speed_of_delivery_code,
									Text = _deliveryCodeMapping[delivery.speed_of_delivery_code],
									FeeAmount = ConvertLongToDecimal(delivery.charges)
								});
							}

							fee = new Fee()
							{
								SessionCookie = paymentTransactions.FirstOrDefault().conv_session_cookie,
								AccountHolderName = accountHolderName,
								AvailableBalance = availableBalance,
								DeliveryMethods = deliveryMethods,
								CityCode = reply.payment_transactions.payment_transaction[0].qp_company.city_code
							};
						}
					}
				}
			}
			return fee;
		}

		internal List<Location> ParseLocations(FusionGo.fusiongoshoppingreply reply)
		{
			List<Location> locations = new List<Location>();

			if (reply.payment_transactions.payment_transaction.Length > 0)
			{
				var paymentTransactions = reply.payment_transactions.payment_transaction;
				if (paymentTransactions.FirstOrDefault().fusion != null)
				{
					FusionGo.fusion fusion = paymentTransactions.FirstOrDefault().fusion;
					foreach (var routing in fusion.routing)
					{
						locations.Add(new Location()
						{
							Name = routing.routing_param.FirstOrDefault(),
							Type = routing.routing_type
						});
					}
				}
			}
			return locations;
		}

		internal FusionGo.payment_details SetFusionPaymentDetails(FusionGo.iso_code isoCode)
		{
			return new FusionGo.payment_details()
			{
				recording_country_currency = new FusionGo.country_currency_info() { iso_code = isoCode },
				destination_country_currency = new FusionGo.country_currency_info() { iso_code = isoCode },
				originating_country_currency = new FusionGo.country_currency_info() { iso_code = isoCode },
				payment_type = FusionGo.payment_type.Cash,
				auth_status = "VLD",
				money_transfer_type = FusionGo.money_transfer_type.QQC,
				fix_on_send = FusionGo.yes_no.@false,
				money_transfer_typeSpecified = true
			};
		}

		internal long ProcessAPICall(WUValidate.makepaymentvalidationrequest request, WUValidate.makepaymentvalidationreply reply, WesternUnionAccount account, MGIContext context)
		{
			long trxId = 0;
			WUValidate.payment_transaction reqPaymentTransaction = request.payment_transactions.payment_transaction.FirstOrDefault();

			WUValidate.payment_transaction paymentTransaction = null;

			if (reply != null)
			{
				paymentTransaction = reply.payment_transactions.payment_transaction.FirstOrDefault();
			};

			if (reqPaymentTransaction != null)
			{
				WesternUnionTrx trx = BuildTransaction(request, reply, account, context, reqPaymentTransaction, paymentTransaction);
				trxId = ProcessorDAL.AddTrx(trx);
			}
			return trxId;
		}

		internal WesternUnionTrx BuildTransaction(WUValidate.makepaymentvalidationrequest request, WUValidate.makepaymentvalidationreply reply, WesternUnionAccount account, MGIContext mgiContext, WUValidate.payment_transaction reqPaymentTransaction, WUValidate.payment_transaction paymentTransaction)
		{
			WesternUnionTrx trx = new WesternUnionTrx()
			{
				WesternUnionAccount = account,
				ChannelName = request.channel.name,
				ChannelType = request.channel.type.ToString(),
				ChannelVersion = request.channel.version,
				SenderFirstName = reqPaymentTransaction.sender.name.first_name,
				SenderLastname = reqPaymentTransaction.sender.name.last_name,
				SenderAddressLine1 = reqPaymentTransaction.sender.address.addr_line1,
				SenderCity = reqPaymentTransaction.sender.address.city,
				SenderState = reqPaymentTransaction.sender.address.state,
				SenderPostalCode = reqPaymentTransaction.sender.address.postal_code,
				SenderCountryCode = reqPaymentTransaction.sender.address.Item.iso_code.country_code,
				SenderCurrencyCode = reqPaymentTransaction.sender.address.Item.iso_code.currency_code,
				SenderAddressLine2 = reqPaymentTransaction.sender.address.local_area,
				SenderStreet = reqPaymentTransaction.sender.address.street,
				WesternUnionCardNumber = reqPaymentTransaction.sender.preferred_customer.account_nbr,
				LevelCode = reqPaymentTransaction.sender.preferred_customer.level_code,
				SenderEmail = reqPaymentTransaction.sender.email,
				SenderContactPhone = reqPaymentTransaction.sender.contact_phone,
				SenderDateOfBirth = reqPaymentTransaction.sender.date_of_birth,

				SenderComplianceDetailsTemplateID = reqPaymentTransaction.sender.compliance_details.template_id,
				SenderComplianceDetailsIdDetailsIdType = reqPaymentTransaction.sender.compliance_details.id_details.id_type,
				SenderComplianceDetailsIdDetailsIdNumber = reqPaymentTransaction.sender.compliance_details.id_details.id_number,
				SenderComplianceDetailsIdDetailsIdPlaceOfIssue = reqPaymentTransaction.sender.compliance_details.id_details.id_place_of_issue,
				SenderComplianceDetailsIdDetailsIdCountryOfIssue = reqPaymentTransaction.sender.compliance_details.id_details.id_country_of_issue,
                SenderComplianceDetailsSecondIDIdType = (reqPaymentTransaction.sender.compliance_details.second_id != null) ? reqPaymentTransaction.sender.compliance_details.second_id.id_type : null, //AL-6260
                SenderComplianceDetailsSecondIDIdNumber=(reqPaymentTransaction.sender.compliance_details.second_id != null)? reqPaymentTransaction.sender.compliance_details.second_id.id_number :null, //AL-6260
                SenderComplianceDetailsSecondIDIdCountryOfIssue = (reqPaymentTransaction.sender.compliance_details.second_id != null)? reqPaymentTransaction.sender.compliance_details.second_id.id_country_of_issue :null,//AL-6260 
				SenderComplianceDetailsDateOfBirth = reqPaymentTransaction.sender.compliance_details.date_of_birth,
				SenderComplianceDetailsOccupation = WesternUnionIO.TrimOccupation(NexxoUtil.MassagingValue(reqPaymentTransaction.sender.compliance_details.occupation)),
				SenderComplianceDetailsAckFlag = reqPaymentTransaction.sender.compliance_details.ack_flag,
				SenderComplianceDetailsIActOnMyBehalf = reqPaymentTransaction.sender.compliance_details.I_act_on_My_Behalf,
				SenderComplianceDetailsCurrentAddressAddrLine1 = reqPaymentTransaction.sender.compliance_details.Current_address.addr_line1,
				SenderComplianceDetailsCurrentAddressAddrLine2 = reqPaymentTransaction.sender.compliance_details.Current_address.addr_line2,
				SenderComplianceDetailsCurrentAddressCity = reqPaymentTransaction.sender.compliance_details.Current_address.city,
				SenderComplianceDetailsCurrentAddressStateCode = reqPaymentTransaction.sender.compliance_details.Current_address.state_code,
				SenderComplianceDetailsCurrentAddressPostalCode = reqPaymentTransaction.sender.compliance_details.Current_address.postal_code,
				SenderComplianceDetailsContactPhone = reqPaymentTransaction.sender.compliance_details.contact_phone,
				SenderComplianceDetailsComplianceDataBuffer = (paymentTransaction != null) ? paymentTransaction.sender.compliance_details.compliance_data_buffer : string.Empty,

				BillerName = reqPaymentTransaction.qp_company.company_name,
				BillerCityCode = reqPaymentTransaction.qp_company.city_code,
				CustomerAccountNumber = Encrypt(reqPaymentTransaction.qp_company.debtor_account_number),
				CountryCode = reqPaymentTransaction.qp_company.country.iso_code.country_code,
				CurrencyCode = reqPaymentTransaction.qp_company.country.iso_code.currency_code,
				QPCompanyDepartment = reqPaymentTransaction.qp_company.department[0],
				FinancialsOriginatorsPrincipalAmount = (paymentTransaction != null) ? ConvertLongToDecimal(paymentTransaction.financials.originators_principal_amount) : 0.0M,
				FinancialsDestinationPrincipalAmount = (paymentTransaction != null) ? ConvertLongToDecimal(paymentTransaction.financials.destination_principal_amount) : 0.0M,
				FinancialsGrossTotalAmount = (paymentTransaction != null) ? ConvertLongToDecimal(paymentTransaction.financials.gross_total_amount) : 0.0M,
				FinancialsFee = (paymentTransaction != null) ? ConvertLongToDecimal(paymentTransaction.financials.charges) : 0.0M,
				FinancialsDiscountedCharges = (paymentTransaction != null) ? ConvertLongToDecimal(paymentTransaction.financials.total_discounted_charges) : 0.0M,
				FinancialsUndiscountedCharges = (paymentTransaction != null) ? ConvertLongToDecimal(paymentTransaction.financials.total_undiscounted_charges) : 0.0M,
				FinancialsPlusChargesAmount = (paymentTransaction != null) ? ConvertLongToDecimal(paymentTransaction.financials.plus_charges_amount) : 0.0M,
				FinancialsTotalDiscount = (paymentTransaction != null) ? ConvertLongToDecimal(paymentTransaction.financials.total_discount) : 0.0M,

				PaymentDetailsRecordingCountryCode = reqPaymentTransaction.payment_details.recording_country_currency.iso_code.country_code,
				PaymentDetailsRecordingCountryCurrency = reqPaymentTransaction.payment_details.recording_country_currency.iso_code.currency_code,
				PaymentDetailsDestinationCountryCode = reqPaymentTransaction.payment_details.destination_country_currency.iso_code.country_code,
				PaymentDetailsDestinationCountryCurrency = reqPaymentTransaction.payment_details.destination_country_currency.iso_code.currency_code,
				PaymentDetailsOriginatingCountryCode = reqPaymentTransaction.payment_details.originating_country_currency.iso_code.country_code,
				PaymentDetailsOriginatingCountryCurrency = reqPaymentTransaction.payment_details.originating_country_currency.iso_code.currency_code,
				PaymentDetailsTransactionType = reqPaymentTransaction.payment_details.transaction_type.ToString(),
				PaymentDetailsAuthStatus = reqPaymentTransaction.payment_details.auth_status,
				PaymentDetailsPaymentType = reqPaymentTransaction.payment_details.payment_type.ToString(),
				PaymentDetailsExchangeRate = reqPaymentTransaction.payment_details.exchange_rate,
				PaymentDetailsFixOnSend = reqPaymentTransaction.payment_details.fix_on_send.ToString(),
				PaymentDetailsReceiptOptOut = reqPaymentTransaction.payment_details.receipt_opt_out.ToString(),
				PaymentDetailsOriginatingCity = (paymentTransaction != null) ? paymentTransaction.payment_details.originating_city : string.Empty,
				PaymentDetailsOriginatingState = (paymentTransaction != null) ? paymentTransaction.payment_details.originating_state : string.Empty,

				DeliveryCode = reqPaymentTransaction.delivery_services.code,
				FusionScreen = reqPaymentTransaction.fusion.fusion_screen.ToString(),
				ConvSessionCookie = FindAndTokenizeCardNumber(reqPaymentTransaction.conv_session_cookie, reqPaymentTransaction.qp_company.debtor_account_number),

				ForeignRemoteSystemCounterId = request.foreign_remote_system.counter_id,
				ForeignRemoteSystemIdentifier = request.foreign_remote_system.identifier,
				ForeignRemoteSystemReferenceNo = request.foreign_remote_system.reference_no,

				ChannelParterId = Convert.ToInt32(mgiContext.ChannelPartnerId),
				ProviderId = Convert.ToInt32(mgiContext.ProviderId),

				DTServerCreate = DateTime.Now,
				DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone),
				Mtcn = (paymentTransaction != null) ? paymentTransaction.mtcn : string.Empty,
				NewMTCN = (paymentTransaction != null) ? paymentTransaction.new_mtcn : string.Empty,

				InstantNotificationAddlServiceCharges = (reply != null && reply.instant_notification != null) ? reply.instant_notification.addl_service_charges : string.Empty,
				InstantNotificationAddlServiceLength = (reply != null && reply.instant_notification != null && reply.instant_notification.addl_service_block != null) ? reply.instant_notification.addl_service_block.addl_service_length : Convert.ToInt16(0),

				FillingDate = (paymentTransaction != null) ? paymentTransaction.filing_date : string.Empty,
				FillingTime = (paymentTransaction != null) ? paymentTransaction.filing_time : string.Empty,
				DfFieldsDeliveryServiceName = (reply != null && reply.df_fields != null) ? reply.df_fields.delivery_service_name : string.Empty,
				DfFieldsTransactionFlag = (reply != null && reply.df_fields != null) ? reply.df_fields.df_transaction_flag.ToString() : string.Empty,
				DfFieldsPdsrequiredflag = (reply != null && reply.df_fields != null) ? reply.df_fields.pds_required_flag.ToString() : string.Empty,

				PromotionsCouponsPromotions = (reqPaymentTransaction != null && reqPaymentTransaction.promotions != null) ? reqPaymentTransaction.promotions.coupons_promotions : string.Empty,
				PromotionsPromoCodeDescription = (paymentTransaction != null && paymentTransaction.promotions != null) ? paymentTransaction.promotions.promo_code_description : string.Empty,
				PromotionsPromoDiscountAmount = (paymentTransaction != null && paymentTransaction.promotions != null) ? ConvertLongToDecimal(paymentTransaction.promotions.promo_discount_amount) : 0.0M,
				PromotionsPromoMessage = (paymentTransaction != null && paymentTransaction.promotions != null) ? paymentTransaction.promotions.promo_message : string.Empty,
				PromotionsPromoName = (paymentTransaction != null && paymentTransaction.promotions != null) ? paymentTransaction.promotions.promo_name : string.Empty,
				PromotionsPromoSequenceNo = (paymentTransaction != null && paymentTransaction.promotions != null) ? paymentTransaction.promotions.promo_sequence_no : string.Empty,
				PromotionsPromotionError = (paymentTransaction != null && paymentTransaction.promotions != null) ? paymentTransaction.promotions.promotion_error : string.Empty,
				PromotionsSenderPromoCode = (paymentTransaction != null && paymentTransaction.promotions != null) ? paymentTransaction.promotions.sender_promo_code : string.Empty
				//,
				//RequestXml = XMLSerializeHelper.Serialize<WUValidate.makepaymentvalidationrequest>(request),
				//ResponseXml = XMLSerializeHelper.Serialize<WUValidate.makepaymentvalidationreply>(reply)
			};
			return trx;
		}

		internal long ProcessAPICall(WUPayment.makepaymentstorerequest request, WUPayment.makepaymentstorereply reply, WesternUnionAccount account, MGIContext mgiContext)
		{
			long trxId = 0;
			WUPayment.payment_transaction reqPaymentTransaction = request.payment_transactions.payment_transaction.FirstOrDefault();
			WUPayment.payment_transaction paymentTransaction = null;
			if (reply != null)
			{
				paymentTransaction = reply.payment_transactions.payment_transaction.FirstOrDefault();
			}

			string totalvalue = string.Empty;
			if (!string.IsNullOrWhiteSpace(account.CardNumber))
			{
				totalvalue = GetWUTotalPointsEarned(account.CardNumber, mgiContext);
			}

			if (reqPaymentTransaction != null)
			{
				WesternUnionTrx trx = new WesternUnionTrx()
				{
					WesternUnionAccount = account,
					ChannelName = request.channel.name,
					ChannelType = request.channel.type.ToString(),
					ChannelVersion = request.channel.version,
					SenderFirstName = reqPaymentTransaction.sender.name.first_name,
					SenderLastname = reqPaymentTransaction.sender.name.last_name,
					SenderAddressLine1 = reqPaymentTransaction.sender.address.addr_line1,
					SenderCity = reqPaymentTransaction.sender.address.city,
					SenderState = reqPaymentTransaction.sender.address.state,
					SenderPostalCode = reqPaymentTransaction.sender.address.postal_code,
					SenderCountryCode = reqPaymentTransaction.sender.address.Item.iso_code.country_code,
					SenderCurrencyCode = reqPaymentTransaction.sender.address.Item.iso_code.currency_code,
					SenderAddressLine2 = reqPaymentTransaction.sender.address.local_area,
					SenderStreet = reqPaymentTransaction.sender.address.street,
					WesternUnionCardNumber = reqPaymentTransaction.sender.preferred_customer.account_nbr,
					LevelCode = reqPaymentTransaction.sender.preferred_customer.level_code,
					SenderEmail = reqPaymentTransaction.sender.email,
					SenderContactPhone = reqPaymentTransaction.sender.contact_phone,
					SenderDateOfBirth = reqPaymentTransaction.sender.date_of_birth,
					BillerName = reqPaymentTransaction.qp_company.company_name,
					BillerCityCode = reqPaymentTransaction.qp_company.city_code,
					CustomerAccountNumber = Encrypt(reqPaymentTransaction.qp_company.debtor_account_number),
					CountryCode = reqPaymentTransaction.qp_company.country.iso_code.country_code,
					CurrencyCode = reqPaymentTransaction.qp_company.country.iso_code.currency_code,
					QPCompanyDepartment = reqPaymentTransaction.qp_company.department[0],
					FinancialsOriginatorsPrincipalAmount = ConvertLongToDecimal(reqPaymentTransaction.financials.originators_principal_amount),
					FinancialsDestinationPrincipalAmount = ConvertLongToDecimal(reqPaymentTransaction.financials.destination_principal_amount),
					FinancialsGrossTotalAmount = ConvertLongToDecimal(reqPaymentTransaction.financials.gross_total_amount),
					FinancialsFee = ConvertLongToDecimal(reqPaymentTransaction.financials.charges),
					FinancialsDiscountedCharges = (reqPaymentTransaction != null) ? ConvertLongToDecimal(reqPaymentTransaction.financials.total_discounted_charges) : 0.0M,
					FinancialsUndiscountedCharges = (reqPaymentTransaction != null) ? ConvertLongToDecimal(reqPaymentTransaction.financials.total_undiscounted_charges) : 0.0M,
					FinancialsTotalDiscount = (reqPaymentTransaction != null) ? ConvertLongToDecimal(reqPaymentTransaction.financials.total_discount) : 0.0M,
					FinancialsPlusChargesAmount = ConvertLongToDecimal(reqPaymentTransaction.financials.plus_charges_amount),

					PaymentDetailsRecordingCountryCode = reqPaymentTransaction.payment_details.recording_country_currency.iso_code.country_code,
					PaymentDetailsRecordingCountryCurrency = reqPaymentTransaction.payment_details.recording_country_currency.iso_code.currency_code,
					PaymentDetailsDestinationCountryCode = reqPaymentTransaction.payment_details.destination_country_currency.iso_code.country_code,
					PaymentDetailsDestinationCountryCurrency = reqPaymentTransaction.payment_details.destination_country_currency.iso_code.currency_code,
					PaymentDetailsOriginatingCountryCode = reqPaymentTransaction.payment_details.originating_country_currency.iso_code.country_code,
					PaymentDetailsOriginatingCountryCurrency = reqPaymentTransaction.payment_details.originating_country_currency.iso_code.currency_code,
					PaymentDetailsTransactionType = reqPaymentTransaction.payment_details.transaction_type.ToString(),
					PaymentDetailsAuthStatus = reqPaymentTransaction.payment_details.auth_status,
					PaymentDetailsPaymentType = reqPaymentTransaction.payment_details.payment_type.ToString(),
					PaymentDetailsExchangeRate = reqPaymentTransaction.payment_details.exchange_rate,
					PaymentDetailsFixOnSend = reqPaymentTransaction.payment_details.fix_on_send.ToString(),
					PaymentDetailsReceiptOptOut = reqPaymentTransaction.payment_details.receipt_opt_out.ToString(),
					DeliveryCode = reqPaymentTransaction.delivery_services.code,
					FusionScreen = reqPaymentTransaction.fusion.fusion_screen.ToString(),
					ConvSessionCookie = FindAndTokenizeCardNumber(reqPaymentTransaction.conv_session_cookie, reqPaymentTransaction.qp_company.debtor_account_number),
					ForeignRemoteSystemCounterId = request.foreign_remote_system.counter_id,
					ForeignRemoteSystemIdentifier = request.foreign_remote_system.identifier,
					ForeignRemoteSystemReferenceNo = request.foreign_remote_system.reference_no,

					ChannelParterId = Convert.ToInt32(mgiContext.ChannelPartnerId),
					ProviderId = Convert.ToInt32(mgiContext.ProviderId),
					DTServerCreate = DateTime.Now,
					DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone),

					Mtcn = (paymentTransaction != null) ? paymentTransaction.mtcn : string.Empty,
					NewMTCN = (paymentTransaction != null) ? paymentTransaction.new_mtcn : string.Empty,
					InstantNotificationAddlServiceCharges = (reply != null) ? reply.instant_notification.addl_service_charges : string.Empty,
					InstantNotificationAddlServiceLength = (reply != null) ? reply.instant_notification.addl_service_block.addl_service_length : Convert.ToInt16(0),
					PaymentDetailsOriginatingCity = (paymentTransaction != null) ? paymentTransaction.payment_details.originating_city : string.Empty,
					PaymentDetailsOriginatingState = (paymentTransaction != null) ? paymentTransaction.payment_details.originating_state : string.Empty,
					FillingDate = (paymentTransaction != null) ? paymentTransaction.filing_date : string.Empty,
					FillingTime = (paymentTransaction != null) ? paymentTransaction.filing_time : string.Empty,

					PromotionsPromoCodeDescription = (reqPaymentTransaction != null && reqPaymentTransaction.promotions != null) ? reqPaymentTransaction.promotions.promo_code_description : string.Empty,
					PromotionsPromoDiscountAmount = (reqPaymentTransaction != null && reqPaymentTransaction.promotions != null) ? ConvertLongToDecimal(reqPaymentTransaction.promotions.promo_discount_amount) : 0.0M,
					PromotionsPromoMessage = (reqPaymentTransaction != null && reqPaymentTransaction.promotions != null) ? reqPaymentTransaction.promotions.promo_message : string.Empty,
					PromotionsPromoName = (reqPaymentTransaction != null && reqPaymentTransaction.promotions != null) ? reqPaymentTransaction.promotions.promo_name : string.Empty,
					PromotionsPromoSequenceNo = (reqPaymentTransaction != null && reqPaymentTransaction.promotions != null) ? reqPaymentTransaction.promotions.promo_sequence_no : string.Empty,
					PromotionsPromotionError = (reqPaymentTransaction != null && reqPaymentTransaction.promotions != null) ? reqPaymentTransaction.promotions.promotion_error : string.Empty,
					PromotionsSenderPromoCode = (reqPaymentTransaction != null && reqPaymentTransaction.promotions != null) ? reqPaymentTransaction.promotions.sender_promo_code : string.Empty,

					SenderComplianceDetailsComplianceDataBuffer = (paymentTransaction != null && paymentTransaction.sender.compliance_details.compliance_data_buffer != null) ? paymentTransaction.sender.compliance_details.compliance_data_buffer : string.Empty,

					//RequestXml = XMLSerializeHelper.Serialize<WUPayment.makepaymentstorerequest>(request),
					//ResponseXml = XMLSerializeHelper.Serialize<WUPayment.makepaymentstorereply>(reply),
					//Open # US 1706 02.03.2014: Adds WU Gold Card Total earned Value to WuCardTotalPointsEarned//
					WuCardTotalPointsEarned = (!string.IsNullOrWhiteSpace(totalvalue)) ? totalvalue : string.Empty,
					//Close # US 1706 02.03.2014//

					MessageArea = string.Concat(GetPromotionMessageArea(paymentTransaction.promotions.promo_text), GetPromotionMessageArea(paymentTransaction.wu_card.wu_card_pin_text))
				};

				trxId = ProcessorDAL.AddTrx(trx);
			}
			return trxId;
		}

		internal DAS.h2hdasreply ExecuteDASInquiry(string methodName, DAS.filters_type filters, MGIContext mgiContext)
		{
			DAS.channel channel = null;
			DAS.foreign_remote_system foreignRemoteSystem = null;

			WUBaseRequestResponse wuObjects = (WUBaseRequestResponse)mgiContext.Context["BaseWUObject"];
			BuildDASObjects(wuObjects, ref channel, ref foreignRemoteSystem);
			foreignRemoteSystem.reference_no = DateTime.Now.ToString("yyyyMMddHHmmssffff");

			DAS.DASInquiryPortTypeClient dasInquiry = new DAS.DASInquiryPortTypeClient(DAS_ENDPOINT_NAME, _serviceUrl);
			dasInquiry.ClientCredentials.ClientCertificate.Certificate = _certificate;

			DAS.h2hdasrequest request = new DAS.h2hdasrequest()
			{
				channel = channel,
				foreign_remote_system = foreignRemoteSystem,
				name = methodName,
				filters = filters
			};
			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<DAS.h2hdasrequest>(mgiContext.CustomerSessionId, request, "ExecuteDASInquiry", AlloyLayerName.CXN,
				ModuleName.BillPayment, "ExecuteDASInquiry -MGI.Cxn.BillPay.WU.Impl.BaseIO", "REQUEST", mgiContext);
			#endregion
			DAS.h2hdasreply response = null;
			try
			{
				response = dasInquiry.DAS_Service(request);
			}
			catch (System.ServiceModel.FaultException<DAS.errorreply> fex)
			{
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<DAS.h2hdasrequest>(request, "ExecuteDASInquiry", AlloyLayerName.CXN, ModuleName.BillPayment,
					"Error in ExecuteDASInquiry -MGI.Cxn.BillPay.WU.Impl.BaseIO", fex.Detail.error, fex.StackTrace);
				throw new BillPayException(BillPayException.PROVIDER_ERROR, fex.Detail.error, fex);
			}
			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<DAS.h2hdasreply>(mgiContext.CustomerSessionId, response, "ExecuteDASInquiry", AlloyLayerName.CXN,
				ModuleName.BillPayment, "ExecuteDASInquiry -MGI.Cxn.BillPay.WU.Impl.BaseIO", "RESPONSE", mgiContext);
			#endregion
			return response;
		}

		internal DAS.QQCCOMPANYNAME_Type GetBiller(string billerName, string locationName, MGIContext mgiContext)
		{
			DAS.QQCCOMPANYNAME_Type biller = null;

			int errorCode = BillPayException.BILLER_FIELDS_RETRIEVAL_FAILED;
			string errorMessage = "Error while retrieving biller information from Western Union: {0}";
			string errMessage = string.Empty;
			try
			{
				var filters = new DAS.filters_type()
				{
					queryfilter1 = "en",
					queryfilter2 = "US",
					queryfilter3 = billerName,
					queryfilter4 = locationName,
					queryfilter7 = "FUSION"
				};

				DAS.h2hdasreply response = ExecuteDASInquiry(QQC_COMPANY_NAME, filters, mgiContext);

				biller = ParseBiller(response, billerName, out errMessage);
			}
			catch (Exception ex)
			{
				errorMessage = string.Format(errorMessage, ex.Message);

				//AL-1014 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Biller Name:" + billerName);
				details.Add("Location Name:" + locationName);
				MongoDBLogger.ListError<string>(details, "GetBiller", AlloyLayerName.CXN, ModuleName.BillPayment,
					"Error in GetBiller -MGI.Cxn.BillPay.WU.Impl.BaseIO", errorMessage, ex.StackTrace);

				throw new BillPayException(errorCode, errorMessage, ex);
			}
			return biller;
		}

		internal long ProcessAPICall(FusionGo.fusiongoshoppingrequest request, WesternUnionAccount account, BillPaymentRequest billPaymentRequest, MGIContext mgiContext)
		{
			long trxId = 0;

			if (request != null)
			{
				WesternUnionTrx trx = new WesternUnionTrx()
				{
					WesternUnionAccount = account,
					ChannelName = request.channel.name,
					ChannelType = request.channel.type.ToString(),
					ChannelVersion = request.channel.version,
					SenderFirstName = account.FirstName,
					SenderLastname = account.LastName,
					SenderAddressLine1 = account.Address1,
					SenderCity = account.City,
					SenderState = account.State,
					SenderPostalCode = account.PostalCode,
					SenderAddressLine2 = account.Address2,
					SenderStreet = account.Street,

					SenderDateOfBirth = (account.DateOfBirth == DateTime.MinValue || account.DateOfBirth == null) ? null : account.DateOfBirth.Value.ToString("MM/dd/yyyy"),
					WesternUnionCardNumber = account.CardNumber,
					BillerName = billPaymentRequest.BillerName,
					CustomerAccountNumber = Encrypt(billPaymentRequest.AccountNumber),

					ForeignRemoteSystemCounterId = request.foreign_remote_system.counter_id,
					ForeignRemoteSystemIdentifier = request.foreign_remote_system.identifier,
					ForeignRemoteSystemReferenceNo = request.foreign_remote_system.reference_no,

					ChannelParterId = Convert.ToInt32(mgiContext.ChannelPartnerId),
					ProviderId = mgiContext.ProviderId,
					DTServerCreate = DateTime.Now,
					DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone)
					//,
					//RequestXml = requestXml,
					//ResponseXml = responseXml
				};
				trxId = ProcessorDAL.AddTrx(trx);
			}
			return trxId;
		}

		internal string GetWUTotalPointsEarned(string WesternUnionCardNumber, MGIContext mgiContext)
		{
			RequestType requestStatus = (RequestType)Enum.Parse(typeof(RequestType), mgiContext.RequestType);
			if (requestStatus == RequestType.RELEASE)
			{
				MGI.Cxn.WU.Common.Data.CardLookUpRequest cxncardlookupreq = new MGI.Cxn.WU.Common.Data.CardLookUpRequest()
				{
					sender = new MGI.Cxn.WU.Common.Data.Sender()
					{
						PreferredCustomerAccountNumber = WesternUnionCardNumber,
					}
				};
				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<MGI.Cxn.WU.Common.Data.CardLookUpRequest>(mgiContext.CustomerSessionId, cxncardlookupreq, "GetWUTotalPointsEarned", AlloyLayerName.CXN,
					ModuleName.BillPayment, "GetWUTotalPointsEarned -MGI.Cxn.BillPay.WU.Impl.BaseIO", "REQUEST", mgiContext);
				#endregion
				MGI.Cxn.WU.Common.Data.CardLookupDetails cardlookupdetails = WesternUnionIO.WUCardLookupForCardNumber(cxncardlookupreq, mgiContext);

				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<MGI.Cxn.WU.Common.Data.CardLookupDetails>(mgiContext.CustomerSessionId, cardlookupdetails, "GetWUTotalPointsEarned", AlloyLayerName.CXN,
					ModuleName.BillPayment, "GetWUTotalPointsEarned -MGI.Cxn.BillPay.WU.Impl.BaseIO", "RESPONSE", mgiContext);
				#endregion
				return cardlookupdetails.WuCardTotalPointsEarned;
			}
			return null;
		}

		internal string BuildValidationMessage(string type, string label, string maxLength)
		{
			string message = string.Empty;

			if (string.Equals(type, "xview", StringComparison.OrdinalIgnoreCase))
			{
				message = "Field is locked, No Entry Required";
			}
			else if (!string.IsNullOrWhiteSpace(label) && label.ToLower().Contains("date"))
			{
				message = "10 Characters (MM/DD/YYYY)";
			}
			else
			{
				message = string.Format("{0} Characters, No Lower Case", maxLength);
			}

			return message;
		}

		internal string Encrypt(string plainString)
		{
			if (!string.IsNullOrWhiteSpace(plainString) && plainString.IsCreditCardNumber())
			{
				return BPDataProtectionSvc.Encrypt(plainString, 0);
			}
			return plainString;
		}

		internal string Decrypt(string encryptedString)
		{
			string decryptString = encryptedString;
			if (!string.IsNullOrWhiteSpace(encryptedString))
			{
				try
				{
					decryptString = BPDataProtectionSvc.Decrypt(encryptedString, 0);
				}
				catch
				{
					decryptString = encryptedString;
					//AL-1014 Transactional Log User Story
					MongoDBLogger.Error<string>(encryptedString, "Decrypt", AlloyLayerName.CXN, ModuleName.BillPayment,
						"Error in Decrypt -MGI.Cxn.BillPay.WU.Impl.BaseIO", string.Empty, string.Empty);
				}
			}
			return decryptString;
		}

		internal string FindAndTokenizeCardNumber(string actualString, string cardNumber)
		{
			if (actualString.Contains(cardNumber))
			{
				string tokenizedCardNumber = Encrypt(cardNumber);
				actualString = actualString.Replace(cardNumber, tokenizedCardNumber);
			}

			return actualString;
		}

		internal string FindAndDetokenizeCardNumber(string actualString, string cardNumber)
		{
			if (actualString.Contains(cardNumber))
			{
				string deTokenizedCardNumber = Decrypt(cardNumber);
				actualString = actualString.Replace(cardNumber, deTokenizedCardNumber);
			}

			return actualString;
		}

		private void PopulateDeliveryMethods()
		{
			_deliveryCodeMapping.Add("000", "Urgent");
			_deliveryCodeMapping.Add("100", "2nd Business Day");
			_deliveryCodeMapping.Add("200", "3rd Business Day");
			_deliveryCodeMapping.Add("300", "Next Business Day");
		}

		private string GetPromotionMessageArea(string[] stringArray)
		{
			StringBuilder message = new StringBuilder();
			if (stringArray != null)
			{
				foreach (var item in stringArray)
				{
					if (!string.IsNullOrWhiteSpace(item))
					{
						message.Append(item);
					}
				}
			}
			return message.ToString();
		}

		#endregion
	}

}
