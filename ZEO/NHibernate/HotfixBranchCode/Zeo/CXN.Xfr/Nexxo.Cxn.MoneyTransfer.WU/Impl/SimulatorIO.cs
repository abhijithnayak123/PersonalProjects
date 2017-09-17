using AutoMapper;
using MGI.Common.DataAccess.Contract;
using MGI.Common.Util;
using MGI.Cxn.MoneyTransfer.Data;
using MGI.Cxn.MoneyTransfer.WU.Data;
using MGI.Cxn.MoneyTransfer.WU.FeeInquiry;
using MGI.Cxn.MoneyTransfer.WU.Impl;
using MGI.Cxn.MoneyTransfer.WU.ModifySendMoney;
using MGI.Cxn.MoneyTransfer.WU.ModifySendMoneySearch;
using MGI.Cxn.MoneyTransfer.WU.ReceiveMoneySearch;
using MGI.Cxn.MoneyTransfer.WU.Search;
using MGI.Cxn.MoneyTransfer.WU.SendMoneyPayStatus;
using MGI.Cxn.MoneyTransfer.WU.SendMoneyRefund;
using MGI.Cxn.WU.Common.Contract;
using MGI.Cxn.WU.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MGI.Cxn.MoneyTransfer.WU.Impl
{
	public class SimulatorIO : BaseIO, IIO
	{
		public SimulatorIO()
		{
			Mapper.CreateMap<SendMoneyValidation.sendmoneyvalidationrequest, SendMoneyValidation.sendmoneyvalidationreply>();
		}
		//public IRepository<WUCredential> WUCredentialRepo { private get; set; }
		//public IWUCommon WUCommon { private get; set; }
		//public bool IsHardCodedCounterId { get; set; }
		//public string LPMTErrorMessage { get; set; }

		public List<MoneyTransfer.Data.DeliveryService> GetDeliveryServices(MoneyTransfer.Data.DeliveryServiceRequest request,
			string state, string stateCode, string city, string deliveryService, MGIContext mgiContext)
		{
			List<DeliveryService> serviceName = null;
			if (!string.IsNullOrEmpty(deliveryService))
			{
				return GetMockDeliveryServices(deliveryService, serviceName);
			}

			else
			{
				return GetDeliveryServiceBasedCountry(request.CountryCode, serviceName);
			}

		}

		private List<DeliveryService> GetMockDeliveryServices(string deliveryService, List<DeliveryService> serviceName)
		{
			switch (deliveryService)
			{
				case "000":
				case "400":
				case "402":
				case "102":
				case "101":
				case "100":
				case "070":
				case "069":
					serviceName = new List<DeliveryService>() { 
						new DeliveryService() { Code="002", Name = "PHONE NOTIFICATION" }
					};
					break;

				default: serviceName = new List<DeliveryService>();
					break;
			}
			return serviceName;
		}

		private List<MoneyTransfer.Data.DeliveryService> GetDeliveryServiceBasedCountry(string countryCode, List<DeliveryService> serviceName)
		{
			switch (countryCode.ToUpper())
			{
				case "IN":
					serviceName = new List<DeliveryService>(){
						new DeliveryService(){ Code="000",Name="MONEY IN MINUTES"}
					};
					break;
				case "MX":
					serviceName = new List<DeliveryService>(){
						new DeliveryService(){Code ="400",Name="DINERO  EN MINUTOS/IN MINUTES"},
						new DeliveryService(){Code ="402",Name="DINERO DIA SIGUIENTE/NEXT DAY"},
						new DeliveryService(){Code ="102",Name="GIRO  PAISANO/IN MINUTES"},
						new DeliveryService(){Code ="101",Name="GIRO WITH NOTIFICATION"},
						new DeliveryService(){Code ="100",Name="GIRO WITHOUT NOTIFICATION"}
					};
					break;
				case "US":
					serviceName = new List<DeliveryService>(){
						new DeliveryService(){Code ="069",Name="MONEY IN MINUTES"},
						new DeliveryService(){Code ="070",Name="NEXT DAY DELIVERY SERVICE"}
					};

					break;
				default: serviceName = new List<DeliveryService>();
					break;
			}
			return serviceName;
		}

		private string GetDeliveryServiceName(string deliveryServiceName)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("400", "DINERO  EN MINUTOS/IN MINUTES");
			dictionary.Add("000", "MONEY IN MINUTES");
			dictionary.Add("401", "DINERO DIA SIGUIENTE/NEXT DAY");
			dictionary.Add("102", "GIRO  PAISANO/IN MINUTES");
			dictionary.Add("101", "GIRO WITH NOTIFICATION");
			dictionary.Add("100", "GIRO WITHOUT NOTIFICATION");
			dictionary.Add("069", "MONEY IN MINUTES");
			dictionary.Add("070", "NEXT DAY DELIVERY SERVICE");
			return dictionary[deliveryServiceName];

		}

		public FeeInquiry.feeinquiryreply FeeInquiry(FeeInquiry.feeinquiryrequest feeInquiryRequest, MGIContext mgiContext)
		{
			FeeInquiry.feeinquiryreply reply = new FeeInquiry.feeinquiryreply();
			reply.financials = new WU.FeeInquiry.financials()
			{
				originators_principal_amount = feeInquiryRequest.financials.originators_principal_amount,
				destination_principal_amount = feeInquiryRequest.financials.destination_principal_amount,
				gross_total_amount = feeInquiryRequest.financials.gross_total_amount,
				charges = feeInquiryRequest.financials.charges,
				plus_charges_amount = feeInquiryRequest.financials.plus_charges_amount,
				pay_amount = feeInquiryRequest.financials.pay_amount,
				tolls = feeInquiryRequest.financials.tolls,
				canadian_dollar_exchange_fee = feeInquiryRequest.financials.canadian_dollar_exchange_fee,
				total_discount = feeInquiryRequest.financials.total_discount,
				message_charge = feeInquiryRequest.financials.message_charge,
			};

			reply.promotions = new WU.FeeInquiry.promotions()
			{
				promo_discount_amount = feeInquiryRequest.promotions.promo_discount_amount,

			};

			reply.payment_details = new WU.FeeInquiry.payment_details()
			{
				exchange_rate = feeInquiryRequest.payment_details.exchange_rate,

			};
			reply.financials.taxes = new WU.FeeInquiry.taxes()
			{
				state_tax = 1,
				municipal_tax = 1,
				county_tax = 1,
			};
			reply.fee_inquiry_message = new WU.FeeInquiry.fee_inquiry_message()
			{
				base_message_charge = Convert.ToString(0)
			};
			return reply;
		}

		public SendMoneyValidation.sendmoneyvalidationreply SendMoneyValidate(SendMoneyValidation.sendmoneyvalidationrequest validationRequest, MGIContext mgiContext)
		{

			SendMoneyValidation.sendmoneyvalidationreply reply = new SendMoneyValidation.sendmoneyvalidationreply();
			reply.sender = new WU.SendMoneyValidation.sender()
			{
				name=new SendMoneyValidation.general_name()
				{
				first_name= validationRequest.sender.name.first_name,
				middle_name=validationRequest.sender.name.middle_name,
				last_name=validationRequest.sender.name.last_name
				},
				address = new WU.SendMoneyValidation.address()
				{
					addr_line1=validationRequest.sender.address.addr_line1,
					city=validationRequest.sender.address.city,
					state=validationRequest.sender.address.state,
					postal_code=validationRequest.sender.address.postal_code,
					Item = new WU.SendMoneyValidation.country_currency_info()
					{
						iso_code = new WU.SendMoneyValidation.iso_code()
						{
							country_code = "US",
							currency_code = "USD"
						},
					},
			},
				preferred_customer = new WU.SendMoneyValidation.preferred_customer()
				{
					account_nbr=validationRequest.sender.preferred_customer.account_nbr
				},
				compliance_details= new WU.SendMoneyValidation.compliance_details()
				{
					template_id=validationRequest.sender.compliance_details.template_id,
					id_details = new WU.SendMoneyValidation.id_details()
					{
						id_type=validationRequest.sender.compliance_details.id_details.id_type,
						id_country_of_issue=validationRequest.sender.compliance_details.id_details.id_country_of_issue,
						id_place_of_issue=validationRequest.sender.compliance_details.id_details.id_place_of_issue,
						id_number=validationRequest.sender.compliance_details.id_details.id_number
					},
					second_id = new WU.SendMoneyValidation.id_details()
					{
						id_type= validationRequest.sender.compliance_details.id_details.id_type,
						id_country_of_issue = validationRequest.sender.compliance_details.id_details.id_country_of_issue,
						id_place_of_issue = validationRequest.sender.compliance_details.id_details.id_place_of_issue,
						id_number = validationRequest.sender.compliance_details.id_details.id_number
					},
					third_party_details=new WU.SendMoneyValidation.third_party_details()
					{
						flag_pay = validationRequest.sender.compliance_details.third_party_details.flag_pay,
					},
					date_of_birth=validationRequest.sender.compliance_details.date_of_birth,
					Current_address = new WU.SendMoneyValidation.compliance_address()
					{
						addr_line1=validationRequest.sender.compliance_details.Current_address.addr_line1,
						addr_line2 = validationRequest.sender.compliance_details.Current_address.addr_line2,
						city = validationRequest.sender.compliance_details.Current_address.city,
						state_name = validationRequest.sender.compliance_details.Current_address.state_name,
						postal_code = validationRequest.sender.compliance_details.Current_address.postal_code,
						country = validationRequest.sender.compliance_details.Current_address.country
					},
					Country_of_Birth = validationRequest.sender.compliance_details.Country_of_Birth,
					ack_flag=validationRequest.sender.compliance_details.ack_flag,
				},
			contact_phone=validationRequest.sender.contact_phone
		};

			reply.receiver = new WU.SendMoneyValidation.receiver()
			{
				name = new WU.SendMoneyValidation.general_name()
				{
					name_type=validationRequest.receiver.name.name_type,
					first_name=validationRequest.receiver.name.first_name,
					middle_name=validationRequest.receiver.name.middle_name,
					last_name=validationRequest.receiver.name.last_name
				},
			};
			reply.payment_details = new WU.SendMoneyValidation.payment_details()
			{
				expected_payout_location = new SendMoneyValidation.expected_payout_location()
				{
					state_code=validationRequest.payment_details.originating_state,
					city=validationRequest.payment_details.originating_city
				},
				recording_country_currency = new WU.SendMoneyValidation.country_currency_info()
				{
					iso_code = new SendMoneyValidation.iso_code()
					{
						country_code="US",
						currency_code="USD"
					},
				},
				destination_country_currency = new WU.SendMoneyValidation.country_currency_info()
				{
					iso_code=new WU.SendMoneyValidation.iso_code() 
					{ country_code=validationRequest.payment_details.destination_country_currency.iso_code.country_code,
					  currency_code = validationRequest.payment_details.destination_country_currency.iso_code.currency_code
					},
				},
				originating_country_currency = new WU.SendMoneyValidation.country_currency_info()
				{
					iso_code = new WU.SendMoneyValidation.iso_code()
					{
						country_code = validationRequest.payment_details.destination_country_currency.iso_code.country_code,
						currency_code = validationRequest.payment_details.destination_country_currency.iso_code.currency_code
					},
				},
				transaction_type = validationRequest.payment_details.transaction_type,
				payment_type=validationRequest.payment_details.payment_type,
				duplicate_detection_flag = validationRequest.payment_details.duplicate_detection_flag
			};
			reply.financials = new SendMoneyValidation.financials()
			{

				originators_principal_amount=validationRequest.financials.originators_principal_amount,
			};
			reply.foreign_remote_system = new WU.SendMoneyValidation.foreign_remote_system()
			{
				identifier = "WGHH614900T",
				reference_no = "2015081802285803",
				counter_id = "1313992501"
			};

			reply.mtcn = Convert.ToString(NexxoUtil.GetLongRandomNumber(10));
			reply.new_mtcn = string.Format(NexxoUtil.GetLongRandomNumber(6) + reply.mtcn);
			reply.filing_date = string.Format(DateTime.Now.ToString("MM-dd"));
			reply.filing_time = string.Format(DateTime.Now.ToString("HHMMt") + " " + TimeZoneInfo.Utc.DisplayName);
			reply.df_fields = new WU.SendMoneyValidation.df_fields()
			{
				pds_required_flag = new SendMoneyValidation.yes_no(),
				df_transaction_flag = new SendMoneyValidation.yes_no(),
				delivery_service_name = GetDeliveryServiceName(validationRequest.delivery_services.code),
			};
			reply.financials.taxes = new WU.SendMoneyValidation.taxes()
			{
				state_tax = 1,
				municipal_tax = 1,
				county_tax = 1,
			};
			reply.promotions = new WU.SendMoneyValidation.promotions() { };

			return reply;
		
		}

		public ReceiveMoneyPay.receivemoneypayreply ReceiveMoneyPay(ReceiveMoneyPay.receivemoneypayrequest receiveMoneyPayRequest, MGIContext mgiContext)
		{
			ReceiveMoneyPay.receivemoneypayreply receiveMoneyPayResponse = new ReceiveMoneyPay.receivemoneypayreply()
			{
				mtcn = Convert.ToString(NexxoUtil.GetLongRandomNumber(10)),
				paid_date_time = DateTime.Now.ToString(),
				receiver = new ReceiveMoneyPay.receiver
				{
					compliance_details = new ReceiveMoneyPay.compliance_details()
					{
						compliance_data_buffer = "010801USA5_P020110308K32109820402CA050110605US/CA071001/01/19900806Doctor2401N2002US2911WOOD STREET3010CALIFORNIA310590001321098652323233302US4002CA66093456789879901XC213United StatesB4013"
					}
				}
			};
			return receiveMoneyPayResponse;
		}

		public SendMoneyStore.sendmoneystorereply SendMoneyStore(SendMoneyStore.sendmoneystorerequest sendMoneyStoreRequest, MGIContext mgiContext, out bool hasLPMTError)
		{
			SendMoneyStore.sendmoneystorereply reply = new SendMoneyStore.sendmoneystorereply();
			hasLPMTError = false;
			reply.instant_notification = new WU.SendMoneyStore.instant_notification()
			{
				addl_service_block = new SendMoneyStore.addl_service_block() { addl_service_length = 377 },
				addl_service_charges = "1103FEE120399513039950103MSG020100301096150101002010030109702NN9810USAUSA##J1"
			};
			reply.sender = new WU.SendMoneyStore.sender()
			{
				name = new SendMoneyStore.general_name()
				{
					first_name = sendMoneyStoreRequest.sender.name.first_name,
					last_name = sendMoneyStoreRequest.sender.name.last_name
				},
				address = new WU.SendMoneyStore.address()
				{
					addr_line1 = sendMoneyStoreRequest.sender.address.addr_line1,
					city = sendMoneyStoreRequest.sender.address.city,
					state = sendMoneyStoreRequest.sender.address.state,
					postal_code = sendMoneyStoreRequest.sender.address.postal_code,
					Item = new WU.SendMoneyStore.country_currency_info()
					{
						iso_code = new WU.SendMoneyStore.iso_code()
						{
							country_code = sendMoneyStoreRequest.sender.address.Item.iso_code.country_code,
							currency_code = sendMoneyStoreRequest.sender.address.Item.iso_code.currency_code
						}
					},

				},
				preferred_customer = new WU.SendMoneyStore.preferred_customer()
				{
					account_nbr = sendMoneyStoreRequest.sender.preferred_customer.account_nbr,
				},
				compliance_details = new WU.SendMoneyStore.compliance_details()
				{
					compliance_data_buffer = "010801USA5_P020110308K32109820402CA050110605US/CA071001/01/19900806Doctor2401N2002US2911WOOD STREET3010CALIFORNIA310590001321098652323233302US4002CA66093456789879901XC213United StatesB4013"

				},

				contact_phone = sendMoneyStoreRequest.sender.contact_phone,

			};
			reply.receiver = new WU.SendMoneyStore.receiver()
			{
				name = new WU.SendMoneyStore.general_name()
				{
					first_name = sendMoneyStoreRequest.receiver.name.first_name,
					last_name = sendMoneyStoreRequest.receiver.name.last_name
				},
				address = new WU.SendMoneyStore.address()
				{
					Item = new WU.SendMoneyStore.country_currency_info()
					{
						iso_code = new WU.SendMoneyStore.iso_code()
						{
							country_code = "US",
							currency_code = "USD"
						}
					},
				},
			};
			reply.payment_details = new WU.SendMoneyStore.payment_details()
			{
				expected_payout_location = new WU.SendMoneyStore.expected_payout_location()
				{
					state_code = sendMoneyStoreRequest.payment_details.expected_payout_location.state_code,
					city = sendMoneyStoreRequest.payment_details.expected_payout_location.city
				},
				recording_country_currency = new WU.SendMoneyStore.country_currency_info()
				{
					iso_code = new WU.SendMoneyStore.iso_code()
					{
						country_code = sendMoneyStoreRequest.payment_details.recording_country_currency.iso_code.country_code,
						currency_code = sendMoneyStoreRequest.payment_details.recording_country_currency.iso_code.currency_code
					},
				},
				destination_country_currency = new WU.SendMoneyStore.country_currency_info()
				{
					iso_code = new WU.SendMoneyStore.iso_code()
					{
						country_code = sendMoneyStoreRequest.payment_details.destination_country_currency.iso_code.country_code,
						currency_code = sendMoneyStoreRequest.payment_details.destination_country_currency.iso_code.currency_code
					}
				},
				originating_country_currency = new WU.SendMoneyStore.country_currency_info()
				{
					iso_code = new WU.SendMoneyStore.iso_code()
					{
						country_code = "US",
						currency_code = "USD"
					},
				},

				originating_city = "METTER",
				originating_state = "GA",
				transaction_type = sendMoneyStoreRequest.payment_details.transaction_type,
				payment_type = sendMoneyStoreRequest.payment_details.payment_type,
				exchange_rate = double.Parse("1.0000"),
				fix_on_send = sendMoneyStoreRequest.payment_details.fix_on_send,
				duplicate_detection_flag = sendMoneyStoreRequest.payment_details.duplicate_detection_flag,
			};

			reply.financials = new WU.SendMoneyStore.financials()
			{
				taxes = new WU.SendMoneyStore.taxes()
				{
					municipal_tax = 0,
					state_tax = 0,
					county_tax = 0,
				},
				originators_principal_amount = sendMoneyStoreRequest.financials.originators_principal_amount,
				destination_principal_amount = sendMoneyStoreRequest.financials.destination_principal_amount,
				gross_total_amount = sendMoneyStoreRequest.financials.gross_total_amount,
				plus_charges_amount = sendMoneyStoreRequest.financials.plus_charges_amount,
				charges = sendMoneyStoreRequest.financials.charges,
				message_charge = sendMoneyStoreRequest.financials.message_charge,
				total_undiscounted_charges = sendMoneyStoreRequest.financials.total_undiscounted_charges,
				total_discounted_charges = sendMoneyStoreRequest.financials.total_discounted_charges,

			};
			reply.promotions = new WU.SendMoneyStore.promotions()
			{
				promo_code_description = sendMoneyStoreRequest.promotions.promo_code_description,
				promo_message = sendMoneyStoreRequest.promotions.promo_message,
				sender_promo_code = sendMoneyStoreRequest.promotions.sender_promo_code,
			};
			reply.df_fields = new WU.SendMoneyStore.df_fields()
			{
				pds_required_flag = sendMoneyStoreRequest.df_fields.pds_required_flag,
				df_transaction_flag = sendMoneyStoreRequest.df_fields.df_transaction_flag,
				partner_marketing_languages = sendMoneyStoreRequest.df_fields.partner_marketing_languages,
				amount_to_receiver = sendMoneyStoreRequest.df_fields.amount_to_receiver,
				delivery_service_name = sendMoneyStoreRequest.df_fields.delivery_service_name
			};
			reply.foreign_remote_system = new WU.SendMoneyStore.foreign_remote_system()
			{
				identifier = "WGHH614900T",
				reference_no = "2015081802285803",
				counter_id = "1313992501"
			};
			reply.mtcn = sendMoneyStoreRequest.mtcn;
			reply.new_mtcn = sendMoneyStoreRequest.new_mtcn;
			reply.filing_date = string.Format(DateTime.Now.ToString("MM-dd"));
			reply.filing_time = string.Format(DateTime.Now.ToString("HHMMt") + " " + TimeZoneInfo.Utc.DisplayName);

			return reply;

		}

		public SendMoneyPayStatus.paystatusinquiryreply GetPayStatus(SendMoneyPayStatus.paystatusinquiryrequestdata searchrequest, MGIContext mgiContext)
		{
			paystatusinquiryreply response = new paystatusinquiryreply()
			{
				payment_transactions = new SendMoneyPayStatus.payment_transactions()
			};

			SendMoneyPayStatus.payment_transaction payment = new SendMoneyPayStatus.payment_transaction()
			{
				pay_status_description = "W/C"
			};

			response.payment_transactions.payment_transaction = new SendMoneyPayStatus.payment_transaction[1] { payment };

			return response;
		}

		public ModifySendMoney.modifysendmoneyreply Modify(ModifySendMoney.modifysendmoneyrequest modifySendMoneyRequest, MGIContext mgiContext)
		{
			PopulateWUObjects(mgiContext.ChannelPartnerId, mgiContext);

			ModifySendMoney.channel channel = null;
			ModifySendMoney.foreign_remote_system foreignRemoteSystem = null;

			BuildModifySendMoneyObjects(_response, ref channel, ref foreignRemoteSystem);

			foreignRemoteSystem.reference_no = mgiContext.ReferenceNumber;
			modifySendMoneyRequest.channel = channel;
			modifySendMoneyRequest.foreign_remote_system = foreignRemoteSystem;

			ModifySendMoneyPortTypeClient client = new ModifySendMoneyPortTypeClient("ModifySendMoney", _serviceUrl);
			client.ClientCredentials.ClientCertificate.Certificate = _certificate;

			ModifySendMoney.modifysendmoneyreply reply = new modifysendmoneyreply()
			{
				payment_details = new ModifySendMoney.payment_details()
				{
					originating_city = "ROBBINSDALE",
					originating_state = "MN",
					fix_on_send = ModifySendMoney.yes_no.Y
				},
				financials = new ModifySendMoney.financials()
				{
					originators_principal_amount = modifySendMoneyRequest.financials.originators_principal_amount,
					destination_principal_amount = modifySendMoneyRequest.financials.destination_principal_amount,
					gross_total_amount = modifySendMoneyRequest.financials.gross_total_amount,
					charges = modifySendMoneyRequest.financials.charges,
					message_charge = modifySendMoneyRequest.financials.message_charge,
					total_undiscounted_charges = modifySendMoneyRequest.financials.total_undiscounted_charges,
					total_discounted_charges = modifySendMoneyRequest.financials.total_discounted_charges
				},
				mtcn = modifySendMoneyRequest.mtcn,
				new_mtcn = modifySendMoneyRequest.new_mtcn,
				filing_date = DateTime.Now.ToString("DD-mm"),
				filing_time = string.Format(DateTime.Now.ToString("HHMMt") + " " + TimeZoneInfo.Utc.DisplayName),
				foreign_remote_system = new ModifySendMoney.foreign_remote_system()
				{
					identifier = modifySendMoneyRequest.foreign_remote_system.identifier,
					reference_no = modifySendMoneyRequest.foreign_remote_system.reference_no,
					counter_id = modifySendMoneyRequest.foreign_remote_system.counter_id
				}
			};
			return reply;
		}

		public ModifySendMoneySearch.modifysendmoneysearchreply ModifySearch(ModifySendMoneySearch.modifysendmoneysearchrequest request, MGIContext mgiContext)
		{
			PopulateWUObjects(mgiContext.ChannelPartnerId, mgiContext);

			ModifySendMoneySearch.channel channel = null;
			ModifySendMoneySearch.foreign_remote_system foreignRemoteSystem = null;

			BuildModifySearchObjects(_response, ref channel, ref foreignRemoteSystem);

			foreignRemoteSystem.reference_no = DateTime.Now.ToString("yyyyMMddHHmmssffff");
			request.channel = channel;
			request.foreign_remote_system = foreignRemoteSystem;

			ModifySendMoneySearchPortTypeClient client = new ModifySendMoneySearchPortTypeClient("ModifySendMoneySearch", _serviceUrl);
			client.ClientCredentials.ClientCertificate.Certificate = _certificate;

			modifysendmoneysearchreply searchreply = new modifysendmoneysearchreply()
			{
				payment_transactions = new ModifySendMoneySearch.payment_transactions()
			};

			ModifySendMoneySearch.payment_transaction payment = new WU.ModifySendMoneySearch.payment_transaction()
			{
				sender = new ModifySendMoneySearch.sender()
				{
					name = new ModifySendMoneySearch.general_name()
					{
						first_name = "Chandler",
						last_name = "Bing"
					},
					address = new ModifySendMoneySearch.address()
					{
						city = "CALIFORNIA",
						state = "CA",
						state_zip = "90001",
						street = "LANE NO 2",
						Item = new ModifySendMoneySearch.country_currency_info()
						{
							iso_code = new ModifySendMoneySearch.iso_code()
							{
								country_code = "US",
								currency_code = "USD"
							}
						}
					},
					contact_phone = "9329829384",
					unv_buffer = "0003USA0107ADDRESS0303LOS0402CA06059000907109875421845741010/10/1950",
				},
				receiver = new ModifySendMoneySearch.receiver()
				{
					name = new ModifySendMoneySearch.general_name()
					{
						name_type = MGI.Cxn.MoneyTransfer.WU.ModifySendMoneySearch.name_type.D,
						first_name = "Chandler",
						last_name = "Bing"
					},
					address = new ModifySendMoneySearch.address()
					{
						city = "CALIFORNIA",
						state = "CA",
						state_zip = "90001",
						street = "LANE NO 2",
						Item = new ModifySendMoneySearch.country_currency_info()
						{
							iso_code = new ModifySendMoneySearch.iso_code()
							{
								country_code = "US",
								currency_code = "USD"
							}
						}
					},
					unv_buffer = "0003USA0902US"
				},
				financials = new ModifySendMoneySearch.financials()
				{
					taxes = new ModifySendMoneySearch.taxes()
					{
						municipal_tax = 1,
						state_tax = 1,
						county_tax = 0
					},
					originators_principal_amount = 10000,
					destination_principal_amount = 10000,
					gross_total_amount = 1000,
					plus_charges_amount = 0,
					principal_amount = 1000,
					charges = 100,
					message_charge = 0,
					total_undiscounted_charges = 100,
					total_discounted_charges = 100
				},
				payment_details = new ModifySendMoneySearch.payment_details()
				{
					expected_payout_location = new ModifySendMoneySearch.expected_payout_location()
					{
						state_code = "CA",
						city = "CALIFORNIA"
					},
					destination_country_currency = new ModifySendMoneySearch.country_currency_info()
					{
						iso_code = new ModifySendMoneySearch.iso_code()
						{
							country_code = "US",
							currency_code = "USD"
						}
					},
					originating_country_currency = new ModifySendMoneySearch.country_currency_info()
					{
						iso_code = new ModifySendMoneySearch.iso_code()
						{
							country_code = "US",
							currency_code = "USD"
						}
					},
					originating_city = "METTERGA3",
					originating_state = "GA",
					transaction_type = MGI.Cxn.MoneyTransfer.WU.ModifySendMoneySearch.transaction_type.WMN,
					exchange_rate = 1.000,
					money_transfer_type = MGI.Cxn.MoneyTransfer.WU.ModifySendMoneySearch.money_transfer_type.WMN,
					original_destination_country_currency = new ModifySendMoneySearch.country_currency_info()
					{
						iso_code = new ModifySendMoneySearch.iso_code()
						{
							country_code = "US",
							currency_code = "USD"
						}
					}
				},
				filing_date = DateTime.Now.ToString("MM-dd-yy"),
				filing_time = string.Format(DateTime.Now.ToString("HHMMt") + " " + TimeZoneInfo.Utc.DisplayName),
				money_transfer_key = "2756959745",
				pay_status_description = "W/C",
				mtcn = request.payment_transaction.mtcn,
				new_mtcn = DateTime.Now.ToString("yymmdd").ToString() + request.payment_transaction.mtcn,
				fusion = new ModifySendMoneySearch.fusion()
				{
					fusion_status = "W/C"
				}
			};

			searchreply.payment_transactions.payment_transaction = new MGI.Cxn.MoneyTransfer.WU.ModifySendMoneySearch.payment_transaction[1] { payment };
			searchreply.foreign_remote_system = new MGI.Cxn.MoneyTransfer.WU.ModifySendMoneySearch.foreign_remote_system()
			{
				identifier = request.foreign_remote_system.identifier,
				reference_no = request.foreign_remote_system.reference_no,
				counter_id = request.foreign_remote_system.counter_id,
			};

			return searchreply;
		}

		public Search.searchreply Search(Search.searchrequest searchRequest, MGIContext mgiContext)
		{
			PopulateWUObjects(mgiContext.ChannelPartnerId, mgiContext);

			Search.channel channel = null;
			Search.foreign_remote_system foreignRemoteSystem = null;

			BuildRefundSearchObjects(_response, ref channel, ref foreignRemoteSystem);

			foreignRemoteSystem.reference_no = DateTime.Now.ToString("yyyyMMddHHmmssffff");
			searchRequest.channel = channel;
			searchRequest.foreign_remote_system = foreignRemoteSystem;

			SearchPortTypeClient SearchClient = new Search.SearchPortTypeClient("WU_Search", _serviceUrl);
			SearchClient.ClientCredentials.ClientCertificate.Certificate = _certificate;

			searchreply searchreply = new searchreply()
			{
				payment_transactions = new Search.payment_transactions()
			};


			Search.payment_transaction payment = new WU.Search.payment_transaction()
			{
				sender = new Search.sender()
				{
					name = new Search.general_name()
					{
						first_name = "Chandler",
						last_name = "Bing"
					},
					address = new Search.address()
					{
						city = "CALIFORNIA",
						state = "CA",
						state_zip = "90001",
						street = "LANE NO 2",
						Item = new Search.country_currency_info()
						{
							iso_code = new Search.iso_code()
							{
								country_code = "US",
								currency_code = "USD"
							}
						}
					},
					contact_phone = "9329829384"
				},
				receiver = new Search.receiver()
				{
					name = new Search.general_name()
					{
						first_name = "XYZ",
						last_name = "ABC"
					},
					address = new Search.address()
					{
						Item = new Search.country_currency_info()
						{
							iso_code = new Search.iso_code()
							{
								country_code = "US",
								currency_code = "USD"
							}
						}
					}
				},
				financials = new Search.financials()
				{
					taxes = new Search.taxes()
					{
						municipal_tax = 24,
						state_tax = 48
					},
					originators_principal_amount = 14000,
					gross_total_amount = 15000,
					pay_amount = 14000,
					principal_amount = 14000,
					charges = 1200
				},
				payment_details = new Search.payment_details()
				{
					expected_payout_location = new Search.expected_payout_location()
					{
						state_code = "CA"
					},
					destination_country_currency = new Search.country_currency_info()
					{
						iso_code = new Search.iso_code()
						{
							country_code = "US",
							currency_code = "USD"
						}
					},
					originating_country_currency = new Search.country_currency_info()
					{
						iso_code = new Search.iso_code()
						{
							country_code = "US",
							currency_code = "USD"
						}
					},
					originating_city = "ANOKAMIN",
					exchange_rate = 1,
					original_destination_country_currency = new Search.country_currency_info()
					{
						iso_code = new Search.iso_code()
						{
							country_code = "US",
							currency_code = "USD"
						}
					}
				},
				filing_date = DateTime.Now.ToString("MM-dd-yy"),
				filing_time = string.Format(DateTime.Now.ToString("HHMMt") + " " + TimeZoneInfo.Utc.DisplayName),
				money_transfer_key = "1209076609",
				pay_status_description = "W/C",
				mtcn = searchRequest.payment_transaction.mtcn,
				new_mtcn = DateTime.Now.ToString("yymmdd").ToString() + searchRequest.payment_transaction.mtcn,
				fusion = new Search.fusion()
				{
					fusion_status = "W/C"
				},
				wu_network_agent_indicator = "N"
			};
			searchreply.payment_transactions.payment_transaction = new Search.payment_transaction[1] { payment };
			searchreply.refund_cancel_flag = "N";


			searchreply.foreign_remote_system = new Search.foreign_remote_system()
			{
				identifier = searchRequest.foreign_remote_system.identifier,
				reference_no = searchRequest.foreign_remote_system.reference_no,
				counter_id = searchRequest.foreign_remote_system.counter_id,
			};
			return searchreply;
		}

		public SendMoneyRefund.refundreply Refund(SendMoneyRefund.refundrequest refundRequest, MGIContext mgiContext)
		{
			refundreply refund = new refundreply()
			{
				advisory_text = DateTime.Now.ToString(),
				payment_details = new SendMoneyRefund.payment_details()
				{
					fix_on_send = SendMoneyRefund.yes_no.Y
				},
				financials = new SendMoneyRefund.financials()
				{
					originators_principal_amount = 0,
					destination_principal_amount = 0,
					gross_total_amount = 0,
					plus_charges_amount = 0,
					charges = 0
				},
				promotions = new SendMoneyRefund.promotions()
				{
					promo_discount_amount = 0
				},
				mtcn = refundRequest.mtcn,
				filing_date = DateTime.Now.ToString("DD-mm"),
				filing_time = string.Format(DateTime.Now.ToString("HHMMt") + " " + TimeZoneInfo.Utc.DisplayName),
				foreign_remote_system = new SendMoneyRefund.foreign_remote_system()
				{
					identifier = "WGHH673600T",
					reference_no = "2015020912153427",
					counter_id = "990000401"
				}
			};

			return refund;
		}

		public List<Reason> GetRefundReasons(ReasonRequest request, MGIContext mgiContext)
		{
			List<Reason> refundReasons = new List<Reason>();

			refundReasons.Add(new Reason()
			{
				Code = "W9200",
				Name = "RCM - Customer Changed Mind"
			});

			refundReasons.Add(new Reason()
				{
					Code = "W9201",
					Name = "RCF - Consumer Fraud Suspected"
				});

			refundReasons.Add(new Reason()
			{
				Code = "W9202",
				Name = "RAE - Input Error"
			});
			refundReasons.Add(new Reason()
			{
				Code = "W9203",
				Name = "RFD - Wrong Information"
			});
			refundReasons.Add(new Reason()
			{
				Code = "W9204",
				Name = "RSE - System Error"
			});
			refundReasons.Add(new Reason()
			{
				Code = "W9199",
				Name = "RNA - Money not available by date on receipt"
			});
			refundReasons.Add(new Reason()
			{
				Code = "W9198",
				Name = "RWR - Wrong amount available to recipient"
			});

			return refundReasons;
		}

		public receivemoneysearchreply SearchReceiveMoney(receivemoneysearchrequest receiveMoneySearchRequest, MGIContext mgiContext)
		{
			MGI.Cxn.MoneyTransfer.WU.ReceiveMoneySearch.payment_transactions paymentTransactions = new ReceiveMoneySearch.payment_transactions();
			paymentTransactions.payment_transaction = new ReceiveMoneySearch.payment_transaction[1];

			paymentTransactions.payment_transaction[0] = new ReceiveMoneySearch.payment_transaction()
			{
				filing_date = "08-24-15",
				filing_time = "0245A EDT",
				money_transfer_key = "1436524161",
				pay_status_description = "W/C",
				receiver = new ReceiveMoneySearch.receiver()
				{
					name = new ReceiveMoneySearch.general_name()
					{
						first_name = "NEXXO",
						last_name = "MONEYGRAM",
						maternal_name = "WESTERN"
					},

				},
				sender = new ReceiveMoneySearch.sender()
				{
					name = new ReceiveMoneySearch.general_name()
					{
						first_name = "KARTHICK",
						last_name = "RAJKUMAR",
						middle_name = ""
					},

					address = new ReceiveMoneySearch.address()
					{
						city = "BURLINGAME",
						state = "CA",
						state_zip = "90005",
						street = "ANFRANCISCO",

					},

				},


			};

			paymentTransactions.payment_transaction[0].payment_details = new ReceiveMoneySearch.payment_details()
			{
				expected_payout_location = new ReceiveMoneySearch.expected_payout_location()
				{
					state_code = "CA"
				},
				originating_city = "METTERGA3",
				exchange_rate = double.Parse("1.0000"),
				destination_country_currency = new ReceiveMoneySearch.country_currency_info()
				{
					iso_code = new ReceiveMoneySearch.iso_code()
					{
						country_code = "US",
						currency_code = "USD"
					},
				},

				original_destination_country_currency = new ReceiveMoneySearch.country_currency_info()
				{
					iso_code = new ReceiveMoneySearch.iso_code()
					{
						country_code = "US",
						currency_code = "USD"
					},
				},

				originating_country_currency = new ReceiveMoneySearch.country_currency_info()
				{
					iso_code = new ReceiveMoneySearch.iso_code()
					{
						country_code = "US",
						currency_code = "USD"
					}
				}

			};
			paymentTransactions.payment_transaction[0].fusion = new ReceiveMoneySearch.fusion()
			{
				fusion_status = "W/C"
			};

			paymentTransactions.payment_transaction[0].financials = new ReceiveMoneySearch.financials()
			{
				gross_total_amount = 24900,// Convert.ToInt32("24900"),
				pay_amount = 22500,
				principal_amount = 22500,
				charges = 2400
			};

			receivemoneysearchreply receivemoneysearchReply = new receivemoneysearchreply()
			{
				foreign_remote_system = new ReceiveMoneySearch.foreign_remote_system()
				{
					counter_id = "WGHH614900T",
					reference_no = "2015082412195319",
					identifier = "WGHH614900T"
				},
				payment_transactions = paymentTransactions
			};

			return receivemoneysearchReply;
		}
	}
}
