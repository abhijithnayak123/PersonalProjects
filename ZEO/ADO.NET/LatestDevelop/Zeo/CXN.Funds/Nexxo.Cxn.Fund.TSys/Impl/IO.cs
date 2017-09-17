using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ServiceModel;

using MGI.Common.Sys;
using MGI.Common.Util;

using MGI.Cxn.Fund.Contract;
using MGI.Cxn.Fund.TSys.Data;
using MGI.Cxn.Fund.TSys.Contract;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Cxn.Fund.TSys.Impl
{
	public class IO : IIO
	{
		private string _apiUsername;
		public string APIUserName { set { _apiUsername = value; } }

		private string _apiPassword;
		public string APIPassword { set { _apiPassword = value; } }

		private bool _cardNumberLogging;
		public bool CardNumberLogging { set { _cardNumberLogging = value; } }

		public NLoggerCommon NLogger { get; set; }
		public TLoggerCommon MongoDBLogger { private get; set; }

		enum FeeType
		{
			Activation,
			InitialCredit,
			Credit,
			Debit
		}

		#region ITSysIO implementation
		public TSysIONewUser ValidateNewCardAccount(long programId, string kitId, long cardNumber)
		{
			TSysUserSvc.userData user = getUser(programId, kitId, true);
			TSysCardSvc.cardData card = getCard(user.userId, cardNumber);

			if (card.status != TSysCardSvc.cardStatus.INACTIVE)
				throw new TSysProviderException(FundException.CARD_CANNOT_BE_ACTIVATED.ToString(), string.Format("card {0} cannot be activated. Current status: {1}", ISOCard.EncodeCardNumber(card.number), card.status));

			TSysAccountSvc.accountData account = getAccountByUserId(user.userId, true);

			return new TSysIONewUser
			{
				UserId = user.userId,
				CardId = card.number,
				AccountId = account.accountId,
				Balance = account.availableBalance
			};
		}

		public TSysIONewUser ValidateExistingCardAccount(long programId, string kitId)
		{
			TSysUserSvc.userData user = getUser(programId, kitId, false);
			TSysAccountSvc.accountData account = getAccountByUserId(user.userId, false);

			return new TSysIONewUser
			{
				UserId = user.userId,
				CardId = getActiveCard(user.userId, true),
				AccountId = account.accountId,
				Balance = account.availableBalance
			};
		}

		public void UpdateCardAccount(TSysIOProfile account)
		{
			var userSvc = getUserSvc();

			try
			{
				#region AL-3372 transaction information for GPR cards.
				MongoDBLogger.Info<TSysIOProfile>(0, account, "UpdateCardAccount - Call - updateUser", AlloyLayerName.CXN,
					  ModuleName.Funds, "Begin UpdateCardAccount - Call - updateUser - MGI.Cxn.Fund.TSys.Impl.IO", new MGIContext());
				#endregion
				// update basic profile info
				userSvc.updateUser(account.UserId, null, null, account.FirstName, account.MiddleName, account.LastName, null, null, account.DateOfBirth, null);
							
				NLogger.Info("basic user profile updated");
				
				// update address details
				userSvc.updateAddresses(account.UserId,
					new TSysUserSvc.updateAddressData
					{
						primary = new TSysUserSvc.address
						{
							street1 = account.Address1,
							street2 = account.Address2,
							city = account.City,
							stateProv = account.State,
							postalCode = account.ZipCode,
							//Hard coding it for now as we do not have the data in customer profile.
							country = "USA",//account.Country,
							type = TSysUserSvc.addressType.HOME
						}
					}
				);
				
				NLogger.Info("Addresses updated");
				
				// need to add phone type? assuming mobile for now
				userSvc.updatePhones(account.UserId,
					new TSysUserSvc.updatePhoneData
					{
						primary = new TSysUserSvc.inputPhone
						{
							type = TSysUserSvc.phoneType.MOBILE,
							Item = long.Parse(NexxoUtil.RawPhoneNumber(account.Phone))
						}
					}
				);

				NLogger.Info("phone updated");

				// update SSN
				userSvc.updateIdentification(account.UserId,
					new TSysUserSvc.updateIdentificationData
					{
						primary = new TSysUserSvc.identification
						{
							country = "UNITED STATES",
							type = TSysUserSvc.identificationType.SOCIAL_SECURITY_NUMBER,
							id = account.SSN,
							issueDateSpecified = false,
							expiryDateSpecified = false
						}
					}
				);

				#region AL-3372 transaction information for GPR cards.
				MongoDBLogger.Info<TSysIOProfile>(0, account, "UpdateCardAccount - Call - updateIdentification", AlloyLayerName.CXN,
					  ModuleName.Funds, "End UpdateCardAccount - Call - updateIdentification - MGI.Cxn.Fund.TSys.Impl.IO", new MGIContext());
				#endregion
				NLogger.Info("SSN updated");
			}
			catch (FaultException<TSysUserSvc.updateUserException> f)
			{
				NLogger.Error(string.Format("updateUser failed for {0}: {1}", account.UserId, f.Detail.error));

				//AL-3372 Transactional Log User Story
				MongoDBLogger.Error<TSysIOProfile>(account, "UpdateCardAccount", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in UpdateCardAccount - MGI.Cxn.Fund.TSys.Impl.IO", string.Format("Could not update user details for user {0}", account.UserId), string.Empty);

				throw new TSysProviderException(f.Detail.error.ToString(), string.Format("Could not update user details for user {0}", account.UserId));
			}
			catch (FaultException<TSysUserSvc.updateIdentificationException> f)
			{
				NLogger.Error(string.Format("updateIdentification failed for {0}: {1}", account.UserId, f.Detail.error));

				//AL-3372 Transactional Log User Story
				MongoDBLogger.Error<TSysIOProfile>(account, "UpdateCardAccount", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in UpdateCardAccount - MGI.Cxn.Fund.TSys.Impl.IO", string.Format("Could not update ID details for user {0}", account.UserId), string.Empty);
				
				throw new TSysProviderException(f.Detail.error.ToString(), string.Format("Could not update ID details for user {0}", account.UserId));
			}
			catch (FaultException<TSysUserSvc.updateAddressesException> f)
			{
				NLogger.Error(string.Format("updateAddresses failed for {0}: {1}", account.UserId, f.Detail.error));

				//AL-3372 Transactional Log User Story
				MongoDBLogger.Error<TSysIOProfile>(account, "UpdateCardAccount", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in UpdateCardAccount - MGI.Cxn.Fund.TSys.Impl.IO", string.Format("Could not update user address details for user {0}", account.UserId), string.Empty);
				
				throw new TSysProviderException(f.Detail.error.ToString(), string.Format("Could not update user address details for user {0}", account.UserId));
			}
			catch (FaultException<TSysUserSvc.updatePhonesException> f)
			{
				NLogger.Error(string.Format("updatePhones failed for {0}: {1}", account.UserId, f.Detail.error));

				//AL-3372 Transactional Log User Story
				MongoDBLogger.Error<TSysIOProfile>(account, "UpdateCardAccount", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in UpdateCardAccount - MGI.Cxn.Fund.TSys.Impl.IO", string.Format("updatePhones failed for {0}: {1}", account.UserId, f.Detail.error), string.Empty);
				
				throw new TSysProviderException(f.Detail.error.ToString(), string.Format("Could not update user phone details for user {0}", account.UserId));
			}
			catch (CommunicationException ex)
			{
				//AL-3372 Transactional Log User Story
				MongoDBLogger.Error<TSysIOProfile>(account, "UpdateCardAccount", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in UpdateCardAccount - MGI.Cxn.Fund.TSys.Impl.IO", "Tsys is not available at this time.", string.Empty);

				throw new FundException(FundException.COMMUNICATION_ERROR, "Tsys is not available at this time.", ex);
			}
		}

		public void ActivateCardAccount(TSysIOProfile tSysAccount)
		{
			var tSysUserSvc = getUserSvc();
			var tSysCardSvc = getCardSvc();

			TSysAccountSvc.accountData account = getAccount(tSysAccount.AccountId);

			TSysUserSvc.userData user;
			try
			{
				#region AL-3372 transaction information for GPR cards.
				MongoDBLogger.Info<TSysIOProfile>(0, tSysAccount, "ActivateCardAccount - Call - getUser", AlloyLayerName.CXN,
					  ModuleName.Funds, "Begin ActivateCardAccount - Call - getUser - MGI.Cxn.Fund.TSys.Impl.IO", new MGIContext());
				#endregion
				user = tSysUserSvc.getUser(tSysAccount.UserId);

				#region AL-3372 transaction information for GPR cards.
				MongoDBLogger.Info<TSysUserSvc.userData>(0, user, "ActivateCardAccount - Call - getUser", AlloyLayerName.CXN,
					  ModuleName.Funds, "End ActivateCardAccount - Call - getUser - MGI.Cxn.Fund.TSys.Impl.IO", new MGIContext());
				#endregion
			}
			catch (FaultException<TSysUserSvc.getUserException> f)
			{
				NLogger.Error(string.Format("getUser failed for userId {0}: {1}", tSysAccount.UserId, f.Detail.error));

				//AL-3372 Transactional Log User Story
				MongoDBLogger.Error<TSysIOProfile>(tSysAccount, "ActivateCardAccount - Call - getUser", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in ActivateCardAccount - Call - getUser - MGI.Cxn.Fund.TSys.Impl.IO", string.Format("Could not retrieve user {0}", tSysAccount.UserId), string.Empty);

				throw new TSysProviderException(f.Detail.error.ToString(), string.Format("Could not retrieve user {0}", tSysAccount.UserId));
			}

			TSysCardSvc.cardData card = getCard(user.userId, tSysAccount.CardNumber);

			//activate this card
			try
			{
				NLogger.Info(string.Format("Activating card ID: {0}, Number: {1}", card.cardId, ISOCard.EncodeCardNumber(card.number)));

				#region AL-3372 transaction information for GPR cards.
				MongoDBLogger.Info<TSysCardSvc.cardData>(0, card, "ActivateCardAccount - Call - activateCard", AlloyLayerName.CXN,
					  ModuleName.Funds, "Begin ActivateCardAccount - Call - activateCard - MGI.Cxn.Fund.TSys.Impl.IO", new MGIContext());
				#endregion
				tSysCardSvc.activateCard(card.cardId);

				#region AL-3372 transaction information for GPR cards.
				MongoDBLogger.Info<TSysCardSvc.cardData>(0, card, "ActivateCardAccount - Call - activateCard", AlloyLayerName.CXN,
					  ModuleName.Funds, "End ActivateCardAccount - Call - activateCard - MGI.Cxn.Fund.TSys.Impl.IO", new MGIContext());
				#endregion
			}
			catch (FaultException<TSysCardSvc.activateCardException> f)
			{
				NLogger.Error(string.Format("activateCard failed for cardId {0}: {1}", card.cardId, f.Detail.error));

				//AL-3372 Transactional Log User Story
				MongoDBLogger.Error<TSysCardSvc.cardData>(card, "ActivateCardAccount - Call - activateCard", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in ActivateCardAccount - Call - activateCard - MGI.Cxn.Fund.TSys.Impl.IO", "Could not create new card", string.Empty);

				throw new TSysProviderException(f.Detail.error.ToString(), "Could not create new card");
			}

			// create a new, permanent card for the user
			try
			{
				#region AL-3372 transaction information for GPR cards.
				MongoDBLogger.Info<TSysCardSvc.cardData>(0, card, "ActivateCardAccount - Call - createCard", AlloyLayerName.CXN,
					  ModuleName.Funds, "Begin ActivateCardAccount - Call - createCard - MGI.Cxn.Fund.TSys.Impl.IO", new MGIContext());
				#endregion
				TSysCardSvc.cardData newCard = tSysCardSvc.createCard(user.userId, true, string.Format("{0} {1}", tSysAccount.FirstName, tSysAccount.LastName), string.Empty, null, null, null, TSysCardSvc.issuanceType.PERSONALIZED, TSysCardSvc.deviceType.MAGSTRIPE, null, null, null, null);
								
				NLogger.Info(string.Format("New card created. ID: {0}, Number: {1}", newCard.cardId, cardNumberString(newCard.number)));
							
				tSysCardSvc.linkCardToAccount(newCard.cardId, account.accountId);

				#region AL-3372 transaction information for GPR cards.
				MongoDBLogger.Info<TSysCardSvc.cardData>(0, newCard, "ActivateCardAccount - Call - linkCardToAccount", AlloyLayerName.CXN,
					  ModuleName.Funds, "End ActivateCardAccount - Call - linkCardToAccount - MGI.Cxn.Fund.TSys.Impl.IO", new MGIContext());
				#endregion
			}
			catch (FaultException<TSysCardSvc.createCardException> f)
			{
				NLogger.Error(string.Format("createCard failed for {0}: {1}", user.userId, f.Detail.error));

				//AL-3372 Transactional Log User Story
				MongoDBLogger.Error<TSysCardSvc.cardData>(card, "ActivateCardAccount - Call - createCard", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in ActivateCardAccount - Call - createCard - MGI.Cxn.Fund.TSys.Impl.IO", string.Format("createCard failed for {0}: {1}", user.userId, f.Detail.error), string.Empty);

				throw new TSysProviderException(f.Detail.error.ToString(), "Could not create new card");
			}
			catch (FaultException<TSysCardSvc.linkCardToAccountException> f)
			{
				NLogger.Error(string.Format("linkCardToAccount failed for {0}: {1}", user.userId, f.Detail.error));

				//AL-3372 Transactional Log User Story
				MongoDBLogger.Error<TSysCardSvc.cardData>(card, "ActivateCardAccount - Call - linkCardToAccount", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in ActivateCardAccount - Call - linkCardToAccount - MGI.Cxn.Fund.TSys.Impl.IO", string.Format("linkCardToAccount failed for {0}: {1}", user.userId, f.Detail.error), string.Empty);

				throw new TSysProviderException(f.Detail.error.ToString(), "Could not link new card to account");
			}
			catch (CommunicationException ex)
			{
				//AL-3372 Transactional Log User Story
				MongoDBLogger.Error<TSysCardSvc.cardData>(card, "ActivateCardAccount", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in ActivateCardAccount - MGI.Cxn.Fund.TSys.Impl.IO", "Tsys is not available at this time.", string.Empty);

				throw new FundException(FundException.COMMUNICATION_ERROR, "Tsys is not available at this time.", ex);
			}
		}

		public void ValidateCard(long userId, long accountId, long cardNumber)
		{
			#region AL-3372 transaction information for GPR cards.
			List<string> details = new List<string>();
			details.Add("User Id :" + Convert.ToString(userId));
			details.Add("Account Id :" + Convert.ToString(accountId));
			details.Add("Card Number :" + Convert.ToString(cardNumber));

			MongoDBLogger.ListInfo<string>(0, details, "ValidateCard", AlloyLayerName.CXN,
				  ModuleName.Funds, "Begin ValidateCard - MGI.Cxn.Fund.TSys.Impl.IO", new MGIContext());
			#endregion
			validateAccount(accountId);

			// check the card status
			var tSysCardSvc = getCardSvc();

			TSysCardSvc.cardData card = getCard(userId, cardNumber);

			if (card == null)
			{
				//AL-3372 Transactional Log User Story
				MongoDBLogger.Error<TSysCardSvc.cardData>(card, "ValidateCard", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in ValidateCard - MGI.Cxn.Fund.TSys.Impl.IO", string.Format("Card number provided is not associated with account {0}", accountId), string.Empty);

				throw new TSysProviderException(FundException.CARD_NOT_ASSOCIATED_WITH_THIS_ACCOUNT.ToString(), string.Format("Card number provided is not associated with account {0}", accountId));
             }
			 
			NLogger.Info(string.Format("card: {0}, status: {1}", cardNumberString(card.number), card.status));

			if (card.status != TSysCardSvc.cardStatus.ACTIVATED)
			{
				//AL-3372 Transactional Log User Story
				MongoDBLogger.Error<TSysCardSvc.cardData>(card, "ValidateCard", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in ValidateCard - MGI.Cxn.Fund.TSys.Impl.IO", string.Format("Card status: {0}", card.status.ToString()), string.Empty);

				throw new TSysProviderException(FundException.CARD_NOT_ACTIVE.ToString(), string.Format("Card status: {0}", card.status.ToString()));
		}
		
				#region AL-3372 transaction information for GPR cards.
				MongoDBLogger.Info<TSysCardSvc.cardData>(0, card, "ValidateCard", AlloyLayerName.CXN,
			    ModuleName.Funds, "End ValidateCard - MGI.Cxn.Fund.TSys.Impl.IO", new MGIContext());
				#endregion
		}

		public long GetActiveCard(long userId, long accountId)
		{
			validateAccount(accountId);
			return getActiveCard(userId, false);
		}

		public string Load(string cardNumber, decimal amount, string description)
		{
			var tSysLoadSvc = getLoadSvc();

			string confirmationId = string.Empty;
			try
			{
				#region AL-3372 transaction information for GPR cards.
				List<string> details = new List<string>();
				details.Add("Card Number :" + cardNumber);
				details.Add("Amount :" + Convert.ToString(amount));
				details.Add("Description :" + description);

				MongoDBLogger.ListInfo<string>(0, details, "Load", AlloyLayerName.CXN,
					  ModuleName.Funds, "Begin Load - MGI.Cxn.Fund.TSys.Impl.IO", new MGIContext());
				#endregion
				confirmationId = tSysLoadSvc.load(cardNumber, convertDollarsToCents(amount), description, null);
			}
			catch (FaultException<TSysPrepaidLoadSvc.loadException> f)
			{
				NLogger.Error(string.Format("Load failed: {0}", f.Detail.error));

				//AL-3372 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Card Number :" + cardNumber);
				details.Add("Amount :" + Convert.ToString(amount));
				details.Add("Description :" + description);

				MongoDBLogger.ListError<string>(details, "Load", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in Load - MGI.Cxn.Fund.TSys.Impl.IO", string.Format("Could not load {0} to card {1}: {2}", amount.ToString("c2"), ISOCard.EncodeCardNumber(long.Parse(cardNumber))), string.Empty);

				throw new TSysProviderException(f.Detail.error.ToString(), string.Format("Could not load {0} to card {1}: {2}", amount.ToString("c2"), ISOCard.EncodeCardNumber(long.Parse(cardNumber)), f.Detail.error));
			}
			catch (CommunicationException ex)
			{
				//AL-3372 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Card Number :" + cardNumber);
				details.Add("Amount :" + Convert.ToString(amount));
				details.Add("Description :" + description);

				MongoDBLogger.ListError<string>(details, "Load", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in Load - MGI.Cxn.Fund.TSys.Impl.IO", "Tsys is not available at this time.", string.Empty);

				throw new FundException(FundException.COMMUNICATION_ERROR, "Tsys is not available at this time.", ex);
			}

			#region AL-3372 transaction information for GPR cards.
			List<string> cardDetails = new List<string>();
			cardDetails.Add("Card Number :" + cardNumber);
			cardDetails.Add("Amount :" + Convert.ToString(amount));
			cardDetails.Add("Description :" + description);

			MongoDBLogger.ListInfo<string>(0, cardDetails, "Load", AlloyLayerName.CXN,
				  ModuleName.Funds, "End Load - MGI.Cxn.Fund.TSys.Impl.IO", new MGIContext());
			#endregion
			return confirmationId;
		}

		public string Withdraw(long accountId, decimal amount, string description)
		{
			var tSysTrxSvc = getTrxSvc();

			long confirmationId = 0;
			try
			{
				#region AL-3372 transaction information for GPR cards.
				List<string> details = new List<string>();
				details.Add("Account Id :" + Convert.ToString(accountId));
				details.Add("Amount :" + Convert.ToString(amount));
				details.Add("Description :" + description);

				MongoDBLogger.ListInfo<string>(0, details, "Withdraw", AlloyLayerName.CXN,
					  ModuleName.Funds, "Begin Withdraw - MGI.Cxn.Fund.TSys.Impl.IO", new MGIContext());
				#endregion
				confirmationId = tSysTrxSvc.adjust(accountId, convertDollarsToCents(amount), TSysTransactionSvc.adjustType.DEBIT, TSysTransactionSvc.transactionSubClass.COURTESY_ADJUSTMENT, description, null);
			}
			catch (FaultException<TSysTransactionSvc.adjustException> f)
			{
				NLogger.Error(string.Format("Adjust failed: {0}", f.Detail.error));

				//AL-3372 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Account Id :" + Convert.ToString(accountId));
				details.Add("Amount :" + Convert.ToString(amount));
				details.Add("Description :" + description);

				MongoDBLogger.ListError<string>(details, "Withdraw", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in Withdraw - MGI.Cxn.Fund.TSys.Impl.IO", string.Format("Could not debit account {0}: {1}", accountId, f.Detail.error), string.Empty);

				throw new TSysProviderException(f.Detail.error.ToString(), string.Format("Could not debit account {0}: {1}", accountId, f.Detail.error));
			}
			catch (CommunicationException ex)
			{
				//AL-3372 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Account Id :" + Convert.ToString(accountId));
				details.Add("Amount :" + Convert.ToString(amount));
				details.Add("Description :" + description);

				MongoDBLogger.ListError<string>(details, "Withdraw", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in Withdraw - MGI.Cxn.Fund.TSys.Impl.IO", "Tsys is not available at this time.", string.Empty);

				throw new FundException(FundException.COMMUNICATION_ERROR, "Tsys is not available at this time.", ex);
			}

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<string>(0, Convert.ToString(confirmationId), "Withdraw", AlloyLayerName.CXN,
				  ModuleName.Funds, "End Withdraw - MGI.Cxn.Fund.TSys.Impl.IO", new MGIContext());
			#endregion
			return confirmationId.ToString();
		}

		public decimal GetBalance(long accountId)
		{
			return validateAccount(accountId);
		}

		public string ApplyFee(long accountId, decimal fee, string description)
		{
			var feeSvc = getFeeSvc();

			TSysFeeSvc.feeTransactionData feeTransaction;
			try
			{
				#region AL-3372 transaction information for GPR cards.
				List<string> details = new List<string>();
				details.Add("Account Id :" + Convert.ToString(accountId));
				details.Add("Fee :" + Convert.ToString(fee));
				details.Add("Description :" + description);

				MongoDBLogger.ListInfo<string>(0, details, "ApplyFee", AlloyLayerName.CXN,
					  ModuleName.Funds, "Begin ApplyFee - MGI.Cxn.Fund.TSys.Impl.IO", new MGIContext());
				#endregion
				TSysFeeSvc.feeTransactionDetail transactionDetail = feeSvc.applyFee(accountId, TSysFeeSvc.feeSubClass.ISSUE_CARD_FEE, convertDollarsToCents(fee), description);
				feeTransaction = transactionDetail.feeTransactionList[0];
			}
			catch (FaultException<TSysFeeSvc.applyFeeException> f)
			{
				NLogger.Error(string.Format("ApplyFee failed for account {0}: {1}", accountId, f.Detail.error));

				//AL-3372 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Account Id :" + Convert.ToString(accountId));
				details.Add("Fee :" + Convert.ToString(fee));
				details.Add("Description :" + description);

				MongoDBLogger.ListError<string>(details, "ApplyFee", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in ApplyFee - MGI.Cxn.Fund.TSys.Impl.IO", string.Format("Could not apply fee to account {0}: {1}", accountId, f.Detail.error), string.Empty);

				throw new TSysProviderException(f.Detail.error.ToString(), string.Format("Could not apply fee to account {0}: {1}", accountId, f.Detail.error));
			}
			catch (CommunicationException ex)
			{
				//AL-3372 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Account Id :" + Convert.ToString(accountId));
				details.Add("Fee :" + Convert.ToString(fee));
				details.Add("Description :" + description);

				MongoDBLogger.ListError<string>(details, "ApplyFee", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in ApplyFee - MGI.Cxn.Fund.TSys.Impl.IO", "Tsys is not available at this time.", string.Empty);

				throw new FundException(FundException.COMMUNICATION_ERROR, "Tsys is not available at this time.", ex);
			}

			if (feeTransaction.transaction_status.ToUpper() != "CHARGED")
			{
				//AL-3372 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Account Id :" + Convert.ToString(accountId));
				details.Add("Fee :" + Convert.ToString(fee));
				details.Add("Description :" + description);

				MongoDBLogger.ListError<string>(details, "ApplyFee", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in ApplyFee - MGI.Cxn.Fund.TSys.Impl.IO", string.Format("Unexpected fee transaction: {0}", feeTransaction.transaction_status), string.Empty);
				
				throw new TSysProviderException(FundException.FEE_CHARGE_FAILURE.ToString(), string.Format("Unexpected fee transaction: {0}", feeTransaction.transaction_status));
			}

			#region AL-3372 transaction information for GPR cards.
			List<string> acntDetails = new List<string>();
			acntDetails.Add("Account Id :" + Convert.ToString(accountId));
			acntDetails.Add("Fee :" + Convert.ToString(fee));
			acntDetails.Add("Description :" + description);

			MongoDBLogger.ListInfo<string>(0, acntDetails, "ApplyFee", AlloyLayerName.CXN,
				  ModuleName.Funds, "End ApplyFee - MGI.Cxn.Fund.TSys.Impl.IO", new MGIContext());
			#endregion
			return feeTransaction.transactionId.ToString();
		}
		#endregion

		#region private methods
		private TSysAccountSvc.accountData getAccount(long accountId)
		{
			var tSysAccountSvc = getAccountSvc();
			TSysAccountSvc.accountData acct;

			try
			{
				//AL-3372 Transactional Log User Story
				MongoDBLogger.Info<string>(0, Convert.ToString(accountId), "getAccount", AlloyLayerName.CXN,
					  ModuleName.Funds, "Begin getAccount - MGI.Cxn.Fund.TSys.Impl.IO", new MGIContext());

				acct = tSysAccountSvc.getAccount(accountId);
			}
			catch (FaultException<TSysAccountSvc.getAccountException> f)
			{
				NLogger.Error(string.Format("getAccount failed for account {0}: {1}", accountId, f.Detail.error));

				//AL-3372 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(accountId), "getAccount", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in getAccount - MGI.Cxn.Fund.TSys.Impl.IO", string.Format("Could not retrieve TSys account {0}", accountId), string.Empty);

				throw new TSysProviderException(f.Detail.error.ToString(), string.Format("Could not retrieve TSys account {0}", accountId));
			}

			NLogger.Info(string.Format("Account ID: {0}, marked fraud: {1}, balance: {2}, userId: {3}", acct.accountId, acct.markedFraudulent, convertCentsToDollars(acct.availableBalance).ToString("c2"), acct.userId));

			//AL-3372 Transactional Log User Story
			MongoDBLogger.Info<TSysAccountSvc.accountData>(0, acct, "getAccount", AlloyLayerName.CXN,
				  ModuleName.Funds, "End getAccount - MGI.Cxn.Fund.TSys.Impl.IO", new MGIContext());

			return acct;
		}

		private TSysAccountSvc.accountData getAccountByUserId(long userId, bool checkFraudStatus)
		{
			var tSysAccountSvc = getAccountSvc();

			TSysAccountSvc.accountData[] accounts;

			try
			{
				accounts = tSysAccountSvc.getUserAccounts(userId);

				if (accounts.Length > 1)
					NLogger.Info("Multiple accounts!");
				else if (accounts.Length == 0)
					throw new TSysProviderException(FundException.NO_ACCOUNT_ASSOCIATED_TO_USER.ToString(), string.Format("No account found for user {0}", userId));
				else if (checkFraudStatus && accounts[0].markedFraudulent)
				{
					string fraudType = accounts[0].fraudTypeSpecified ? accounts[0].fraudType.ToString() : "Unspecified";
					throw new TSysProviderException(FundException.ACCOUNT_FLAGGED_AS_FRAUDULENT.ToString(), string.Format("Account validation failed. Fraud type: {0}", fraudType));
				}
			}
			catch (FaultException<TSysAccountSvc.getUserAccountsException> f)
			{
				NLogger.Error(string.Format("getUserAccounts failed for userId {0}: {1}", userId, f.Detail.error));
				throw new TSysProviderException(f.Detail.error.ToString(), "Could not retrieve accounts");
			}
			catch (CommunicationException ex)
			{
				throw new FundException(FundException.COMMUNICATION_ERROR, "Tsys is not available at this time.", ex);
			}

			return accounts[0];
		}

		private TSysUserSvc.userData getUser(long programId, string kitId, bool validateStatus)
		{
			var tSysUserSvc = getUserSvc();

			TSysUserSvc.userData user;

			try
			{
				//AL-3372 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Program Id :" + Convert.ToString(programId));
				details.Add("Kit Id :" + kitId);
				details.Add("Validate Status :" + (validateStatus ? "Yes" : "No"));

				MongoDBLogger.ListInfo<string>(0, details, "getUser", AlloyLayerName.CXN,
					  ModuleName.Funds, "Begin getUser - MGI.Cxn.Fund.TSys.Impl.IO", new MGIContext());
				  
				user = tSysUserSvc.getUserByExternalKey(programId, kitId);

				NLogger.Info(string.Format("UserId: {0}", user.userId));

				if (validateStatus && user.status != TSysUserSvc.userStatus.OPEN)
					throw new TSysProviderException(FundException.USER_NOT_OPEN.ToString(), string.Format("User with key {0} has status {1}", kitId, user.status));
			}
			catch (FaultException<TSysUserSvc.getUserByExternalKeyException> f)
			{
				//AL-3372 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Program Id :" + Convert.ToString(programId));
				details.Add("Kit Id :" + kitId);
				details.Add("Validate Status :" + (validateStatus ? "Yes" : "No"));

				MongoDBLogger.ListError<string>(details, "getAccount", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in getAccount - MGI.Cxn.Fund.TSys.Impl.IO", string.Format("getExternalUserByKey failed for externalId {0}: {1}", kitId, f.Detail.error), string.Empty);

				NLogger.Error(string.Format("getExternalUserByKey failed for externalId {0}: {1}", kitId, f.Detail.error));
				throw new TSysProviderException(f.Detail.error.ToString(), "Could not find user");
			}
			catch (CommunicationException ex)
			{
				//AL-3372 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Program Id :" + Convert.ToString(programId));
				details.Add("Kit Id :" + kitId);
				details.Add("Validate Status :" + (validateStatus ? "Yes" : "No"));

				MongoDBLogger.ListError<string>(details, "getAccount", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in getAccount - MGI.Cxn.Fund.TSys.Impl.IO", "Tsys is not available at this time.", string.Empty);

				throw new FundException(FundException.COMMUNICATION_ERROR, "Tsys is not available at this time.", ex);
			}

			//AL-3372 Transactional Log User Story
			MongoDBLogger.Info<TSysUserSvc.userData>(0, user, "getUser", AlloyLayerName.CXN,
				  ModuleName.Funds, "End getUser - MGI.Cxn.Fund.TSys.Impl.IO", new MGIContext());

			return user;
		}

		private decimal validateAccount(long accountId)
		{
			try
			{
				// check the account status
				TSysAccountSvc.accountData account = getAccount(accountId);

				if (account.markedFraudulent)
				{
					string fraudType = account.fraudTypeSpecified ? account.fraudType.ToString() : "Unspecified";
					throw new TSysProviderException(FundException.ACCOUNT_FLAGGED_AS_FRAUDULENT.ToString(), string.Format("Account validation failed. Fraud type: {0}", fraudType));
				}

				return convertCentsToDollars(account.availableBalance);
			}
			catch (EndpointNotFoundException ex)
			{
				throw new FundException(FundException.COMMUNICATION_ERROR, "Unable to connect to the Tsys server", ex);
			}
			catch (CommunicationException ex)
			{
				throw new FundException(FundException.COMMUNICATION_ERROR, "Unable to connect to the Tsys server", ex);
			}
		}

		private long getActiveCard(long userId, bool acceptAny)
		{
			var tSysCardSvc = getCardSvc();

			TSysCardSvc.cardData[] cards;
			try
			{
				//AL-3372 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("User Id :" + Convert.ToString(userId));
				details.Add("Accept Any :" + (acceptAny ? "Yes" : "No"));

				MongoDBLogger.ListInfo<string>(0, details, "getActiveCard", AlloyLayerName.CXN,
					  ModuleName.Funds, "Begin getActiveCard - MGI.Cxn.Fund.TSys.Impl.IO", new MGIContext());

				cards = tSysCardSvc.getUserCards(userId);


			}
			catch (FaultException<TSysCardSvc.getUserCardsException> f)
			{
				NLogger.Error(string.Format("getUserCards failed for {0}: {1}", userId, f.Detail.error));


				//AL-3372 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("User Id :" + Convert.ToString(userId));
				details.Add("Accept Any :" + (acceptAny ? "Yes" : "No"));

				MongoDBLogger.ListError<string>(details, "getActiveCard", AlloyLayerName.CXN,
					  ModuleName.Funds, "Error in getActiveCard - MGI.Cxn.Fund.TSys.Impl.IO", string.Format("Could not retrieve cards for user {0}", userId),
						string.Empty);

				throw new TSysProviderException(f.Detail.error.ToString(), string.Format("Could not retrieve cards for user {0}", userId));
			}
			catch (CommunicationException ex)
			{
				//AL-3372 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("User Id :" + Convert.ToString(userId));
				details.Add("Accept Any :" + (acceptAny ? "Yes" : "No"));

				MongoDBLogger.ListError<string>(details, "getActiveCard", AlloyLayerName.CXN,
					  ModuleName.Funds, "Error in getActiveCard - MGI.Cxn.Fund.TSys.Impl.IO", string.Format("Could not retrieve cards for user {0}", userId),
						string.Empty);

				throw new FundException(FundException.COMMUNICATION_ERROR, "Tsys is not available at this time.", ex);
			}

			TSysCardSvc.cardData card = cards.FirstOrDefault(c => c.status == TSysCardSvc.cardStatus.ACTIVATED);

			// if any card is ok, then just return the first one
			if (card == null && acceptAny && cards.Length > 0)
				card = cards[0];

			if (card == null)
				throw new TSysProviderException(FundException.NO_ACTIVE_CARD.ToString(), string.Format("No active card associated with user {0}", userId));

			//AL-3372 Transactional Log User Story
			MongoDBLogger.ListInfo<TSysCardSvc.cardData>(0, cards.ToList(), "getActiveCard", AlloyLayerName.CXN,
				  ModuleName.Funds, "End getActiveCard - MGI.Cxn.Fund.TSys.Impl.IO", new MGIContext());

			return card.number;
		}

		private TSysCardSvc.cardData getCard(long userId, long cardNumber)
		{
			var tSysCardSvc = getCardSvc();

			TSysCardSvc.cardData[] cards;
			try
			{
				//AL-3372 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("User Id : " + Convert.ToString(userId));
				details.Add("Card Number : " + Convert.ToString(cardNumber));

				MongoDBLogger.ListInfo<string>(0, details, "getCard", AlloyLayerName.CXN,
					  ModuleName.Funds, "Begin getCard - MGI.Cxn.Fund.TSys.Impl.IO", new MGIContext());

				cards = tSysCardSvc.getUserCards(userId);
			}
			catch (FaultException<TSysCardSvc.getUserCardsException> f)
			{
				NLogger.Error(string.Format("getUserCards failed for {0}: {1}", userId, f.Detail.error));

				//AL-3372 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("User Id : " + Convert.ToString(userId));
				details.Add("Card Number : " + Convert.ToString(cardNumber));
				MongoDBLogger.ListError<string>(details, "GetAccount", AlloyLayerName.CXN,
					ModuleName.Funds, "Error in getCard - MGI.Cxn.Fund.TSys.Impl.IO",
					string.Format("getUserCards failed for {0}: {1}", userId, f.Detail.error), string.Empty);

				throw new TSysProviderException(f.Detail.error.ToString(), string.Format("Could not retrieve cards for user {0}", userId));
			}
			catch (CommunicationException ex)
			{
				//AL-3372 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("User Id : " + Convert.ToString(userId));
				details.Add("Card Number : " + Convert.ToString(cardNumber));

				MongoDBLogger.ListError<string>(details, "getCard", AlloyLayerName.CXN, ModuleName.Funds,
					"Error in getCard - MGI.Cxn.Fund.TSys.Impl.IO", "Tsys is not available at this time.", string.Empty);

				throw new FundException(FundException.COMMUNICATION_ERROR, "Tsys is not available at this time.", ex);
			}

			foreach (TSysCardSvc.cardData cc in cards)
				NLogger.Info(string.Format("cardNumber: {0}, status: {1}", cardNumberString(cc.number), cc.status));

			TSysCardSvc.cardData card = cards.FirstOrDefault(c => c.number == cardNumber);
			if (card == null)
			{
				//AL-3372 Transactional Log User Story
				MongoDBLogger.Error<TSysCardSvc.cardData>(card, "getCard", AlloyLayerName.CXN,
					  ModuleName.Funds, "Error in getCard - MGI.Cxn.Fund.TSys.Impl.IO", string.Format("Card number provided is not associated with user {0}", userId), string.Empty);

				throw new TSysProviderException(FundException.CARD_NOT_ASSOCIATED_WITH_THIS_ACCOUNT.ToString(), string.Format("Card number provided is not associated with user {0}", userId));
			}

			NLogger.Info(string.Format("card match: {0}, status: {1}, exp date: {2}", ISOCard.EncodeCardNumber(card.number), card.status, card.expiryDate));

			//AL-3372 Transactional Log User Story
			MongoDBLogger.ListInfo<TSysCardSvc.cardData>(0, cards.ToList(), "getCard", AlloyLayerName.CXN,
				  ModuleName.Funds, "End getCard - MGI.Cxn.Fund.TSys.Impl.IO", new MGIContext());

			return card;
		}

		private TSysAccountSvc.accountServiceClient getAccountSvc()
		{
			var svc = new TSysAccountSvc.accountServiceClient();
			addAuthInfo(svc.ClientCredentials);
			return svc;
		}

		private TSysCardSvc.cardServiceClient getCardSvc()
		{
			var svc = new TSysCardSvc.cardServiceClient();
			addAuthInfo(svc.ClientCredentials);
			return svc;
		}

		private TSysUserSvc.userServiceClient getUserSvc()
		{
			var svc = new TSysUserSvc.userServiceClient();
			addAuthInfo(svc.ClientCredentials);
			return svc;
		}

		private TSysTransactionSvc.transactionServiceClient getTrxSvc()
		{
			var svc = new TSysTransactionSvc.transactionServiceClient();
			addAuthInfo(svc.ClientCredentials);
			return svc;
		}

		private TSysFeeSvc.feeServiceClient getFeeSvc()
		{
			var svc = new TSysFeeSvc.feeServiceClient();
			addAuthInfo(svc.ClientCredentials);
			return svc;
		}

		private TSysPrepaidLoadSvc.prepaidLoadServiceClient getLoadSvc()
		{
			var svc = new TSysPrepaidLoadSvc.prepaidLoadServiceClient();
			addAuthInfo(svc.ClientCredentials);
			return svc;
		}

		private void addAuthInfo(System.ServiceModel.Description.ClientCredentials credentials)
		{
			credentials.UserName.UserName = _apiUsername;
			credentials.UserName.Password = _apiPassword;
		}

		private long convertDollarsToCents(decimal dollarAmount)
		{
			return (long)(dollarAmount * 100);
		}

		private decimal convertCentsToDollars(long centsAmount)
		{
			return (decimal)(centsAmount / 100m);
		}

		private string cardNumberString(long cardNumber)
		{
			return _cardNumberLogging ? cardNumber.ToString() : ISOCard.EncodeCardNumber(cardNumber);
		}
		#endregion
	}
}
