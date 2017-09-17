using MGI.Cxn.Partner.TCF.RCIFService;
using MGI.Cxn.Partner.TCF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MGI.Common.Util;
using System.Diagnostics;
using System.Threading;

namespace MGI.Cxn.Partner.TCF.Impl
{
	public class IO
	{
		string errorMessage = "Error in RCIF Service";

		//TODO: Load From Spring
		public NLoggerCommon NLogger { get; set; }

		private const string PRODCODE = "ZEOSVC";
		private const string APPL_CD = "SVC";
		private const string ApplID = "SVC";
		private const string SvcNme = "ZEOSVC";

		private const string TaxCdSSN = "S";
		private const string TaxCdITIN = "I";

		private const string CheckProcessing = "CheckProcessing";
		/// <summary>
		/// Author : Abhijith
		/// Description : Logic for PreFlush 
		/// Called from Shopping Cart - Biz.Partner
		/// </summary>
		/// <param name="cart"></param>
		/// <param name="mgiContext"></param>
		/// <returns></returns>
		public bool PreFlush(CustomerTransactionDetails cart, MGIContext mgiContext)
		{
			int errorCode = PartnerException.PROVIDER_ERROR;
			string ErrMsg = string.Empty;
			string mtvnSvcVer = string.Empty;
			string msgUUID = string.Empty;

			Dictionary<string, object> ssoAttributes = new Dictionary<string, object>();

			if (mgiContext.Context.ContainsKey("SSOAttributes") && mgiContext.Context["SSOAttributes"] != null)
			{
				ssoAttributes = (Dictionary<string, object>)mgiContext.Context["SSOAttributes"];
			}

			ZeoCustomerPreFlushRequest request = new ZeoCustomerPreFlushRequest()
			{
				PreFlushSummary = GetPreFlushSummary(cart),
				MsgUUID = msgUUID,
				MtvnSvcVer = mtvnSvcVer,

				Svc = new Svc[1]
				{
					new Svc()
					{
						MsgData = new MsgData()
						{
							CustInfo = GetCustInfo(cart)
						},
						Security = new Security()
						{
							BasicAuth = new BasicAuth()
							{
								UsrID = NexxoUtil.GetDictionaryValueIfExists(ssoAttributes, "UserID"),
								tellerNbr = Convert.ToInt32(NexxoUtil.GetDictionaryValueIfExists(ssoAttributes, "TellerNum")),
								branchNbr = Convert.ToInt32(NexxoUtil.GetDictionaryValueIfExists(ssoAttributes, "BranchNum")),
								bankNbr = Convert.ToInt32(NexxoUtil.GetDictionaryValueIfExists(ssoAttributes, "BankNum")),
								subClientNodeID = NexxoUtil.GetDictionaryValueIfExists(ssoAttributes, "DPSBranchID"),
								lawsonID = NexxoUtil.GetDictionaryValueIfExists(ssoAttributes, "LawsonID"),
								lu = NexxoUtil.GetDictionaryValueIfExists(ssoAttributes, "LU"),
								cashDrawer = NexxoUtil.GetDictionaryValueIfExists(ssoAttributes, "CashDrawer"),
								amPmInd = NexxoUtil.GetDictionaryValueIfExists(ssoAttributes, "AmPmInd"),
								HostName = NexxoUtil.GetDictionaryValueIfExists(ssoAttributes, "MachineName"),
								BusinessDate = NexxoUtil.GetDictionaryValueIfExists(ssoAttributes, "BusinessDate"),
								tellerNbrSpecified = true,
								branchNbrSpecified = true,
								bankNbrSpecified = true,
								
							}
						},
						SvcParms = GetsvcParams()
					}
				},
				PrcsParms = new PrcsParms[1] 
				{ 
					new PrcsParms()
					{
						SrcID = ""	
					}
				}
			};

            ZeoCustomerService_MsgSetPortTypeClient client = new ZeoCustomerService_MsgSetPortTypeClient();
			ZeoCustomerPreFlushResponse response = client.CustomerPreFlush(request);

			if (response == null)
			{
				throw new PartnerException(PartnerException.TCIS_PreFlush, "Error in TCF service call");
			}
			else if (response != null && response.ErrCde != "0")
			{
				errorMessage = response.ErrMsg;
				throw new PartnerException(errorCode, errorMessage);
			}

			return true;
		}

		/// <summary>
		///  Author : Abhijith
		///  Description : Logic for PostFlush 
		///  Called from Shopping Cart - Biz.Partner
		/// </summary>
		/// <param name="cart"></param>
		/// <param name="mgiContext"></param>
		public void PostFlush(CustomerTransactionDetails cart, MGIContext mgiContext)
		{
			int i = 0;   // Todo - Remove
			StringBuilder errMsgs = new StringBuilder();
			int errorCode = PartnerException.PROVIDER_ERROR;
			string noResponseMessage = "Time out Error";
			string ErrMsg = string.Empty;
			string mtvnSvcVer = string.Empty;
			string msgUUID = string.Empty;
			PreFlushSummary[] preflushSummary = new PreFlushSummary[1];

			preflushSummary[0] = new PreFlushSummary();

			List<ZeoCustomerPostFlushResponse> responses = new List<ZeoCustomerPostFlushResponse>();

			Dictionary<string, object> ssoAttributes = new Dictionary<string, object>();

			if (mgiContext.Context.ContainsKey("SSOAttributes") && mgiContext.Context["SSOAttributes"] != null)
			{
				ssoAttributes = (Dictionary<string, object>)mgiContext.Context["SSOAttributes"];
			}

			Task<ZeoCustomerPostFlushResponse>[] tasks = new Task<ZeoCustomerPostFlushResponse>[cart.Transactions.Count];

			foreach (var transaction in cart.Transactions)
			{
				ZeoCustomerPostFlushRequest request = new ZeoCustomerPostFlushRequest()
				{
					MsgUUID = msgUUID,
					MtvnSvcVer = mtvnSvcVer,
					PrcsParms = new PrcsParms[1] 
						{ 
							new PrcsParms()
							{
								SrcID = ""	
							}
						},
					PreFlushSummary = new PreFlushSummary[1]
					{
						new PreFlushSummary()
						{
							SessionId = Convert.ToString(cart.Customer.CustomerSessionId),
							transaction = new PreFlushSummary_Transaction[1]
							{
								GetPostFlushSummary(transaction)
							},
						}
					},
					Svc = new Svc[1] 
					{ 
						new Svc()
						{
							MsgData = new MsgData()
							{
								CustInfo = GetCustInfo(cart)
							},
							Security = new Security()
							{
								BasicAuth = new BasicAuth()
								{
									UsrID = NexxoUtil.GetDictionaryValueIfExists(ssoAttributes, "UserID"),
									tellerNbr = Convert.ToInt32(NexxoUtil.GetDictionaryValueIfExists(ssoAttributes, "TellerNum")),
									branchNbr = Convert.ToInt32(NexxoUtil.GetDictionaryValueIfExists(ssoAttributes, "BranchNum")),
									bankNbr = Convert.ToInt32(NexxoUtil.GetDictionaryValueIfExists(ssoAttributes, "BankNum")),
									subClientNodeID = NexxoUtil.GetDictionaryValueIfExists(ssoAttributes, "DPSBranchID"),
									lawsonID = NexxoUtil.GetDictionaryValueIfExists(ssoAttributes, "LawsonID"),
									lu = NexxoUtil.GetDictionaryValueIfExists(ssoAttributes, "LU"),
									cashDrawer = NexxoUtil.GetDictionaryValueIfExists(ssoAttributes, "CashDrawer"),
									amPmInd = NexxoUtil.GetDictionaryValueIfExists(ssoAttributes, "AmPmInd"),
									HostName = NexxoUtil.GetDictionaryValueIfExists(ssoAttributes, "MachineName"),
									BusinessDate = NexxoUtil.GetDictionaryValueIfExists(ssoAttributes, "BusinessDate"),
									tellerNbrSpecified = true,
									branchNbrSpecified = true,
									bankNbrSpecified = true,
								}	
							},
							SvcParms = GetsvcParams()
						}
					}
				};

				//ZeoCustomerPostFlushResponse response = client.CustomerPostFlush(request);

				try
                {
                    ZeoCustomerService_MsgSetPortTypeClient client = new ZeoCustomerService_MsgSetPortTypeClient();
					tasks[i] = Task.Factory.StartNew(() => client.CustomerPostFlush(request));
				}
				catch(Exception ex)
				{
					throw new PartnerException(errorCode, noResponseMessage,ex);
				}

				i++; // Todo - Remove
			}

			Task.WaitAll(tasks);


			//Get the response of CashIn transaction from the response.
			// When Transaction Type is inserted in the response.
			//Starts Here

			Task<ZeoCustomerPostFlushResponse> cashInTask = tasks.Where(t => t.Result != null && t.Result.TranType == Convert.ToString(CashTransactionType.CashIn)).FirstOrDefault();

			if (cashInTask != null)
			{
				ZeoCustomerPostFlushResponse response = cashInTask.Result;

				if (response.ErrCde != "0")
					throw new PartnerException(errorCode, response.ErrMsg);
			}

			//Ends Here

		}

		/// <summary>
		/// Author : Abhijith
		/// Description : Creating all the tags for customer details.
		/// </summary>
		/// <param name="cart"></param>
		/// <returns></returns>
		private CustInfo[] GetCustInfo(CustomerTransactionDetails cart)
		{
			PersonalInfo personalInfo = new PersonalInfo()
			{
				FName = cart.Customer.FirstName,
				MName = cart.Customer.MiddleName,
				LName = string.IsNullOrWhiteSpace(cart.Customer.SecondLastName) ? cart.Customer.LastName : string.Format("{0} {1}", cart.Customer.LastName, cart.Customer.SecondLastName),
				Addr1 = cart.Customer.Address1,
				Addr2 = cart.Customer.Address2,
				City = cart.Customer.City,
				State = cart.Customer.State,
				zip = cart.Customer.Zip,
				Ph1 = cart.Customer.Phone1,
				Ph1Type1 = cart.Customer.Ph1Type1 != null ? MapToClientPhoneTypes(cart.Customer.Ph1Type1) : null,
				Ph2 = cart.Customer.Phone2,
				Ph2Type2 = cart.Customer.Ph2Type2 != null ? MapToClientPhoneTypes(cart.Customer.Ph2Type2) : null,
				Ph2Prov = cart.Customer.Ph2Prov,
				email = cart.Customer.Email,
				ssn = cart.Customer.SSN,
				TaxCd = !string.IsNullOrWhiteSpace(cart.Customer.SSN) && (cart.Customer.SSN.Substring(0, 1) == "9") ? TaxCdITIN : !string.IsNullOrWhiteSpace(cart.Customer.SSN) ? TaxCdSSN : null,
				NexxoPan = Convert.ToString(cart.Customer.AlloyID),
				ClientCustId = !string.IsNullOrWhiteSpace(cart.Customer.ClientCustId) ? cart.Customer.ClientCustId : "0",
				gender = cart.Customer.Gender != null ? cart.Customer.Gender.Substring(0, 1) : null,
				tcfCustind = cart.Customer.CustInd ? "Y" : "N",
				PRODCODE = PRODCODE,
				APPL_CD = APPL_CD
			};

			DateTime dateOfBirth = Convert.ToDateTime(cart.Customer.DateOfBirth);

			Identification identification = new Identification()
			{
				dob = dateOfBirth != null ? dateOfBirth.ToString("yyyyMMdd") : null,
				maiden = cart.Customer != null ? cart.Customer.Maiden : string.Empty,
				idType = cart.Customer != null ? Convert.ToString(MapToClientIdType(cart.Customer.IdType)) : string.Empty,
				idIssuer = cart.Customer != null ? cart.Customer.IdIssuer : string.Empty,
				idIssuerCountry = cart.Customer != null ? cart.Customer.IdIssuerCountry : string.Empty,
				idNbr = cart.Customer != null ? cart.Customer.Identification : string.Empty,
				idIssueDate = cart.Customer != null && cart.Customer.IssueDate != null ? cart.Customer.IssueDate.Value.ToString("yyyyMMdd") : null,
				idExpDate = cart.Customer != null && cart.Customer.ExpirationDate != null ? cart.Customer.ExpirationDate.Value.ToString("yyyyMMdd") : null,
				legalCode = cart.Customer != null ? cart.Customer.LegalCode : string.Empty,
				citizenshipCountry1 = cart.Customer != null ? cart.Customer.PrimaryCountryCitizenship : string.Empty,
				citizenshipCountry2 = cart.Customer != null ? cart.Customer.SecondaryCountryCitizenship : string.Empty,
			};

			EmploymentInfo employmentInfo = new EmploymentInfo()
			{
				Occupation = cart.Customer != null ? cart.Customer.Occupation : string.Empty,
				OccDesc = cart.Customer != null ? cart.Customer.OccupationDescription : string.Empty,
				EmployerName = cart.Customer != null ? cart.Customer.EmployerName : string.Empty,
				EmployerPhoneNum = cart.Customer != null ? cart.Customer.EmployerPhoneNum : string.Empty
			};

			CustInfo[] custInfo = new CustInfo[1];

			custInfo[0] = new CustInfo()
			{
				PersonalInfo = personalInfo,
				Identification = identification,
				EmploymentInfo = employmentInfo
			};

			return custInfo;
		}

		/// <summary>
		/// Author : Abhijith
		/// Description : Creating all the details of transactions to pass it to the request of Pre flush.
		/// </summary>
		/// <param name="cart"></param>
		/// <returns></returns>
		private PreFlushSummary[] GetPreFlushSummary(CustomerTransactionDetails cart)
		{
			PreFlushSummary_Transaction transactionSummary = new PreFlushSummary_Transaction();
			PreFlushSummary[] preflushSummary = new PreFlushSummary[1];

			preflushSummary[0] = new PreFlushSummary();

			PreFlushSummary_Transaction[] pfSummartTransaction = new PreFlushSummary_Transaction[cart.Transactions.Count];

			preflushSummary[0].SessionId = Convert.ToString(cart.Customer.CustomerSessionId);
			preflushSummary[0].transaction = pfSummartTransaction;

			DateTime dateOfBirth = Convert.ToDateTime(cart.Customer.DateOfBirth);

			for (var i = 0; i <= cart.Transactions.Count - 1; i++)
			{
				//dateOfBirth = Convert.ToDateTime(cart.Transactions[i].ToDOB);
				string type = string.Empty;

				string transType = cart.Transactions[i].Type;

				//TransactionType transType = TransactionType.Cash;
				//Enum.TryParse(cart.Transactions[i].Type, out transType);

				if (transType == Convert.ToString(TransactionType.Cash))
				{
					type = cart.Transactions[i].CashType != null ? cart.Transactions[i].CashType : cart.Transactions[i].Type;
					transactionSummary = PopulateCash(cart.Transactions[i], type);
				}
				else if (transType == Convert.ToString(TransactionType.MoneyTransfer))
				{
					type = cart.Transactions[i].TransferType != null ? cart.Transactions[i].TransferType : cart.Transactions[i].Type;
					transactionSummary = PopulateMoneyTransfer(cart.Transactions[i], type);
				}
				else if (transType == Convert.ToString(TransactionType.BillPay))
				{
					//type = cart.Transactions[i].TransferType != null ? cart.Transactions[i].TransferType : cart.Transactions[i].Type;
					transactionSummary = PopulateBillPay(cart.Transactions[i]);
				}
				else if (transType == Convert.ToString(TransactionType.MoneyOrder))
				{
					//type = cart.Transactions[i].TransferType != null ? cart.Transactions[i].TransferType : cart.Transactions[i].Type;
					transactionSummary = PopulateMoneyOrder(cart.Transactions[i]);
				}
				else if (transType == Convert.ToString(TransactionType.Check))
				{
					//type = cart.Transactions[i].TransferType != null ? cart.Transactions[i].TransferType : cart.Transactions[i].Type;
					transactionSummary = PopulateCheck(cart.Transactions[i]);
				}
				else if (transType == Convert.ToString(TransactionType.Funds))
				{
					type = cart.Transactions[i].TransferType != null ? cart.Transactions[i].TransferType : cart.Transactions[i].Type;
					transactionSummary = PopulateFunds(cart.Transactions[i], type);
				}
				//else
				//{
				//	type = cart.Transactions[i].Type;
				//}


				preflushSummary[0].transaction[i] = transactionSummary;

			}

			return preflushSummary;
		}

		/// <summary>
		/// Author : Abhijith
		/// Description : Populate MoneyTransfer related transactions.
		/// </summary>
		/// <param name="tran"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		private PreFlushSummary_Transaction PopulateMoneyTransfer(Transaction tran, string type)
		{
			PreFlushSummary_Transaction transaction = new PreFlushSummary_Transaction();

			if (string.Compare(type, "SendMoney", true) == 0)
			{
				transaction = PopulateSendMoney(tran, type);
			}
			else if (string.Compare(type, "ReceiveMoney", true) == 0)
			{
				transaction = PopulateReceiveMoney(tran, type);
			}

			return transaction;
		}

		/// <summary>
		/// Author : Abhijith
		/// Description : Populate Billpay related transactions.
		/// </summary>
		/// <param name="billPayTran"></param>
		/// <returns></returns>
		private PreFlushSummary_Transaction PopulateBillPay(Transaction billPayTran)
		{
			PreFlushSummary_Transaction tran = new PreFlushSummary_Transaction()
			{
				tranID = billPayTran.ID,
				tranType = Convert.ToString(TransactionType.BillPay),
				acctNbr = billPayTran.AccountNumber,
				payee = billPayTran.Payee,
				MTCN = billPayTran.MTCN,
				amount = NexxoUtil.RoundOffDecimal(billPayTran.Amount, 2),
				fee = NexxoUtil.RoundOffDecimal(billPayTran.Fee, 2),
				total = NexxoUtil.RoundOffDecimal(billPayTran.GrossTotalAmount, 2)
			};

			return tran;
		}

		/// <summary>
		///  Author : Abhijith
		///  Description : Populate MoneyOrder related transactions.
		/// </summary>
		/// <param name="moTran"></param>
		/// <returns></returns>
		private PreFlushSummary_Transaction PopulateMoneyOrder(Transaction moTran)
		{
			PreFlushSummary_Transaction tran = new PreFlushSummary_Transaction()
			{
				tranID = moTran.ID,
				tranType = TransactionType.MoneyOrder.ToString(),
				CheckNbr = moTran.CheckNumber,
				amount = NexxoUtil.RoundOffDecimal(moTran.Amount, 2),
				fee = NexxoUtil.RoundOffDecimal(moTran.Fee, 2),
				total = NexxoUtil.RoundOffDecimal(moTran.GrossTotalAmount, 2),
			};

			return tran;
		}

		/// <summary>
		/// Author : Abhijith
		/// Description : Populate Check Processing related transactions.
		/// </summary>
		/// <param name="cpTran"></param>
		/// <returns></returns>
		private PreFlushSummary_Transaction PopulateCheck(Transaction cpTran)
		{
			PreFlushSummary_Transaction tran = new PreFlushSummary_Transaction()
			{
				tranID = cpTran.ID.ToString(),
				tranType = CheckProcessing,
				CheckNbr = cpTran.CheckNumber,
				CheckType = cpTran.CheckType,
				ConfirmationNbr = cpTran.ConfirmationNumber,
				amount = NexxoUtil.RoundOffDecimal(cpTran.Amount, 2),
				fee = NexxoUtil.RoundOffDecimal(cpTran.Fee, 2),
				total = NexxoUtil.RoundOffDecimal(cpTran.GrossTotalAmount, 2)
			};

			return tran;
		}

		/// <summary>
		/// Author : Abhijith
		/// Description : Populate Fund related transactions.
		/// </summary>
		/// <param name="feTran"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		private PreFlushSummary_Transaction PopulateFunds(Transaction feTran, string type)
		{
			PreFlushSummary_Transaction transaction = new PreFlushSummary_Transaction();

			if (string.Compare(type, "PrePaidLoad", true) == 0)
			{
				transaction = PopulatePrepaidLoad(feTran, type);
			}
			else if (string.Compare(type, "PrePaidWithdraw", true) == 0)
			{
				transaction = PopulatePrePaidWithdraw(feTran, type);
			}

			return transaction;
		}

		/// <summary>
		/// Author : Abhijith
		/// Description : Populate Fund- Prepaid Load related transactions.
		/// </summary>
		/// <param name="feTran"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		private PreFlushSummary_Transaction PopulatePrepaidLoad(Transaction feTran, string type)
		{
			PreFlushSummary_Transaction tran = new PreFlushSummary_Transaction()
			{
				tranID = Convert.ToString(feTran.ID),
				tranType = type,
				InitialPurchase = feTran.InitialPurchase,
				purchasefee = NexxoUtil.RoundOffDecimal(feTran.PurchaseFee, 2),
				NewCardBalance = NexxoUtil.RoundOffDecimal(feTran.NewCardBalance, 2),
				CardNbr = feTran.CardNumber,
				AliasId = feTran.AliasId,
				ConfirmationNbr = feTran.ConfirmationNumber,
				LoadAmount = NexxoUtil.RoundOffDecimal(feTran.LoadAmount, 2),
				fee = NexxoUtil.RoundOffDecimal(feTran.Fee, 2),
				total = NexxoUtil.RoundOffDecimal(feTran.GrossTotalAmount, 2)
			};

			return tran;
		}

		/// <summary>
		/// Author : Abhijith
		/// Description : Populate Fund- Prepaid Withdraw related transactions.
		/// </summary>
		/// <param name="feTran"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		private PreFlushSummary_Transaction PopulatePrePaidWithdraw(Transaction feTran, string type)
		{
			PreFlushSummary_Transaction tran = new PreFlushSummary_Transaction()
			{
				tranID = Convert.ToString(feTran.ID),
				tranType = type,
				NewCardBalance = NexxoUtil.RoundOffDecimal(feTran.NewCardBalance, 2),
				CardNbr = feTran.CardNumber,
				AliasId = feTran.AliasId,
				ConfirmationNbr = feTran.ConfirmationNumber,
				WithdrawAmount = NexxoUtil.RoundOffDecimal(feTran.WithdrawAmount, 2),
				fee = NexxoUtil.RoundOffDecimal(feTran.Fee, 2),
				total = NexxoUtil.RoundOffDecimal(feTran.GrossTotalAmount, 2)
			};

			return tran;
		}

		/// <summary>
		/// Author : Abhijith
		/// Description : Populate Fund- Prepaid Activate related transactions.
		/// </summary>
		/// <param name="feTran"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		private PreFlushSummary_Transaction PopulatePrepaidActivate(Transaction feTran, string type)
		{
			PreFlushSummary_Transaction tran = new PreFlushSummary_Transaction()
			{
				tranID = Convert.ToString(feTran.ID),
				tranType = type,
				InitialPurchase = feTran.InitialPurchase,
				purchasefee = NexxoUtil.RoundOffDecimal(feTran.PurchaseFee, 2),
				NewCardBalance = NexxoUtil.RoundOffDecimal(feTran.NewCardBalance, 2),
				CardNbr = feTran.CardNumber,
				AliasId = feTran.AliasId,
				ConfirmationNbr = feTran.ConfirmationNumber,
				LoadAmount = NexxoUtil.RoundOffDecimal(feTran.LoadAmount, 2),
				fee = NexxoUtil.RoundOffDecimal(feTran.Fee, 2),
				total = NexxoUtil.RoundOffDecimal(feTran.GrossTotalAmount, 2)
			};

			return tran;
		}

		/// <summary>
		/// Author : Abhijith
		/// Description : Populate Cash related transactions.
		/// </summary>
		/// <param name="cashTran"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		private PreFlushSummary_Transaction PopulateCash(Transaction cashTran, string type)
		{
			PreFlushSummary_Transaction tran = new PreFlushSummary_Transaction();

			if (tran != null)
			{
				tran.tranID = Convert.ToString(cashTran.ID);
				tran.tranType = type;
				tran.acctNbr = cashTran.AccountNumber;
				tran.amount = NexxoUtil.RoundOffDecimal(cashTran.Amount, 2);
				tran.total = NexxoUtil.RoundOffDecimal(cashTran.GrossTotalAmount, 2);
			}
			return tran;
		}

		/// <summary>
		/// Author : Abhijith
		/// Description : Populate MoneyTransfer- SendMoney related transactions.
		/// </summary>
		/// <param name="tran"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		private PreFlushSummary_Transaction PopulateSendMoney(Transaction tran, string type)
		{
			PreFlushSummary_Transaction transactionSummary = new PreFlushSummary_Transaction()
			{
				tranID = tran.ID,
				tranType = type,
				MTCN = tran.MTCN,
				amount = NexxoUtil.RoundOffDecimal(tran.Amount, 2),
				fee = NexxoUtil.RoundOffDecimal(tran.Fee, 2),
				total = NexxoUtil.RoundOffDecimal(tran.GrossTotalAmount, 2),
				ToFirstName = tran.ToFirstName,
				ToMiddleName = tran.ToMiddleName,
				ToLastName = tran.ToLastName,
				ToSecondLastName = tran.ToSecondLastName,
				ToGender = tran.ToGender != null ? tran.ToGender.Substring(0, 1) : null,
				ToCountry = tran.ToCountry,
				ToAddress = tran.ToAddress,
				ToCity = tran.ToCity,
				ToState_Province = tran.ToState_Province,
				ToZipCode = tran.ToZipCode,
				ToPhoneNumber = tran.ToPhoneNumber,
				ToPickupCountry = tran.ToPickUpCountry,
				ToPickupState_Province = tran.ToPickUpState_Province,
				ToPickupCity = tran.ToPickUpCity,
				ToDeliveryMethod = tran.ToDeliveryMethod,
				ToDeliveryOption = tran.ToDeliveryOption,
				ToOccupation = tran.ToOccupation,
				ToDOB = Convert.ToString(tran.ToDOB),
				ToCountryOfBirth = tran.ToCountryOfBirth
			};

			return transactionSummary;
		}

		/// <summary>
		/// Author : Abhijith
		/// Description : Populate MoneyTransfer- ReceiveMoney related transactions.
		/// </summary>
		/// <param name="tran"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		private PreFlushSummary_Transaction PopulateReceiveMoney(Transaction tran, string type)
		{
			PreFlushSummary_Transaction transactionSummary = new PreFlushSummary_Transaction()
			{
				tranID = tran.ID,
				tranType = type,
				MTCN = tran.MTCN,
				FirstName = tran.ToFirstName, // Receiver's FirstName
				ToMiddleName = tran.ToMiddleName, // Receiver's MiddleName
				LastName = tran.ToLastName, // Receiver's LastName
				SecondLastName = tran.ToSecondLastName, // Receiver's Second LastName
				PickupCountry = tran.ToPickUpCountry,
				amount = NexxoUtil.RoundOffDecimal(tran.Amount, 2),
				fee = NexxoUtil.RoundOffDecimal(tran.Fee, 2),
				total = NexxoUtil.RoundOffDecimal(tran.GrossTotalAmount, 2)
			};

			return transactionSummary;
		}

		/// <summary>
		/// Author : Abhijith
		/// Description : Post Flush Related call
		/// Caller : Biz.Partner - ShoppingCartImpl
		/// Update : Added a new logic of passing one by one transaction in PostFlush Request.
		/// </summary>
		/// <param name="tran"></param>
		/// <returns></returns>
		private PreFlushSummary_Transaction GetPostFlushSummary(Transaction tran)
		{
			PreFlushSummary_Transaction transactionSummary = new PreFlushSummary_Transaction();

			string type = string.Empty;

			string transType = tran.Type;

			if (transType == Convert.ToString(TransactionType.Cash))
			{
				type = tran.CashType != null ? tran.CashType : tran.Type;
				transactionSummary = PopulateCash(tran, type);
			}
			else if (transType == Convert.ToString(TransactionType.MoneyTransfer))
			{
				type = tran.TransferType != null ? tran.TransferType : tran.Type;
				transactionSummary = PopulateMoneyTransfer(tran, type);
			}
			else if (transType == Convert.ToString(TransactionType.BillPay))
			{
				//type = cart.Transactions[i].TransferType != null ? cart.Transactions[i].TransferType : cart.Transactions[i].Type;
				transactionSummary = PopulateBillPay(tran);
			}
			else if (transType == Convert.ToString(TransactionType.MoneyOrder))
			{
				//type = cart.Transactions[i].TransferType != null ? cart.Transactions[i].TransferType : cart.Transactions[i].Type;
				transactionSummary = PopulateMoneyOrder(tran);
			}
			else if (transType == Convert.ToString(TransactionType.Check))
			{
				//type = cart.Transactions[i].TransferType != null ? cart.Transactions[i].TransferType : cart.Transactions[i].Type;
				transactionSummary = PopulateCheck(tran);
			}
			else if (transType == Convert.ToString(TransactionType.Funds))
			{
				type = tran.TransferType != null ? tran.TransferType : tran.Type;
				transactionSummary = PopulateFunds(tran, type);
			}

			return transactionSummary;
		}

		/// <summary>
		/// Author : Abhijith
		/// Description : Populate Svc related informations.
		/// </summary>
		/// <returns></returns>
		private SvcParms[] GetsvcParams()
		{
			SvcParms[] svcParams = new SvcParms[1];
			svcParams[0] = new SvcParms()
			{
				ApplID = "",
				SvcID = "",
				SvcVer = "",
				SvcNme = "",
				RqstUUID = ""
			};

			return svcParams;
		}

		private enum TransactionType
		{
			Cash,
			Check,
			Funds,
			BillPay,
			MoneyOrder,
			MoneyTransfer
		}

		private string MapToClientIdType(string AlloyIdType)
		{
			string idType = string.Empty;

			Dictionary<string, string> idMappying = new Dictionary<string, string>()
			{
				{"DRIVER'S LICENSE"						,"D"		},
				{"MILITARY ID"							,"U"		},
				{"PASSPORT"								,"P"		},
				{"U.S. STATE IDENTITY CARD"				,"S"		},
				{"MATRICULA CONSULAR"					,"M"		}
			};

			if (idMappying.ContainsKey(AlloyIdType))
			{
				idType = idMappying[AlloyIdType];
			}
			return idType;
		}

		string MapToClientPhoneTypes(string mapPhoneType)
		{
			string idType = string.Empty;
			Dictionary<string, string> phoneTypes = new Dictionary<string, string>()
			{
				{"Home", "H"},
				{"Work", "W"},
				{"Cell", "M"},
				{"Other", "O"},
			};

			if (phoneTypes.ContainsKey(mapPhoneType))
			{
				idType = phoneTypes[mapPhoneType];
			}
			return idType;
		}
	}
}
