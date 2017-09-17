using MGI.Common.DataAccess.Contract;
using MGI.Common.DataProtection.Contract;

using MGI.Cxn.Fund.FirstView.Data;
using MGI.Cxn.Fund.Contract;

namespace MGI.Cxn.Fund.FirstView.Impl
{
    public class FundsProcessorImpl
    {
        public IRepository<FirstViewTransaction> TransactionMappingRepository { private get; set; }
        public IReadOnlyRepository<FirstViewCredentials> CredentialRepository { private get; set; }
        public IReadOnlyRepository<FirstViewIdTypes> IdTypeRepository { private get; set; }
        public IRepository<FirstViewCard> GPRCardRepository { private get; set; }
		public IDataProtectionService DataProtectionSvc { private get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="terminalID"></param>
        /// <param name="processorID"></param>
        /// <returns></returns>
        public FirstViewCredentials GetCredentials(long channelPartnerId)
        {
			return CredentialRepository.FindBy(x => x.ChannelPartnerId == channelPartnerId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionMapping"></param>
        /// <returns></returns>
        public long Create(FirstViewTransaction transactionMapping)
        {
			//encryptAccountNumber(transactionMapping.Account);
            TransactionMappingRepository.AddWithFlush(transactionMapping);
            return transactionMapping.Id;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionMapping"></param>
        /// <returns></returns>
        public FirstViewTransaction GetTransactionMapping(long fundID)
        {
            FirstViewTransaction transactionMapping = TransactionMappingRepository.FindBy(x => x.Id == fundID);
			//decryptAccountNumber(transactionMapping.Account);
			return transactionMapping;
        }
      
        public long AddGprCard(FirstViewCard gprCard)
		{
			encryptAccountNumber(gprCard);
            GPRCardRepository.AddWithFlush(gprCard);
            return gprCard.Id;
        }

        public FirstViewCard Get(long CXNId)
        {
            FirstViewCard gprCard = GPRCardRepository.FindBy(x => x.Id == CXNId);
			//decryptAccountNumber(gprCard);
			//GPRCardRepository.Evict(gprCard);
			return gprCard;
        }

        public FirstViewCard Get(string bsAccountNumber)
        {
			FirstViewCard gprCard = GPRCardRepository.FindBy(x => x.BSAccountNumber == DataProtectionSvc.Encrypt(bsAccountNumber, 0) && x.IsActive == true);
			//decryptAccountNumber(gprCard);
			//GPRCardRepository.Evict(gprCard);
			return gprCard;
        }

		public long UpdateCard(FirstViewCard gprCard)
        {
			encryptAccountNumber(gprCard);
            GPRCardRepository.UpdateWithFlush(gprCard);
            return gprCard.Id;
        }

        public FirstViewIdTypes GetIdTypes(int NexxoIdTypeId)
        {
            return IdTypeRepository.FindBy(x => x.NexxoIdTypeId == NexxoIdTypeId);
        }

        public long UpdateTransaction(FirstViewTransaction fvTrx)
        {
            if (TransactionMappingRepository.SaveOrUpdate(fvTrx))
                return fvTrx.Id;
            else
                throw new FundException(FundException.POST_TRANSACTION_FAILED,"Error updating transaction amount");
        }

		#region private methods
		private void encryptAccountNumber(FirstViewCard gprCard)
		{
			if(!string.IsNullOrEmpty(gprCard.AccountNumber))
				gprCard.AccountNumber = DataProtectionSvc.Encrypt(gprCard.AccountNumber, 0);
			if (!string.IsNullOrEmpty(gprCard.BSAccountNumber))
				gprCard.BSAccountNumber = DataProtectionSvc.Encrypt(gprCard.BSAccountNumber, 0);
		}

		public int GetExceptionMinorCode(CardResponse response)
        {
            var msg = response.ERR_NUMBER + " " + response.ERRMSG;
            int errorCode = 0;

            switch (msg)
            {
                case "1 Card Number or Account Number or DDA Number Cannot be Blank":
                    errorCode = (int)FundException.CARDNUMBER_CANNOT_BE_BLANK;
                    break;
                case "ERM0010 Invalid Card Number/Password/PIN":
                    errorCode = (int)FundException.INVALID_CARD_NUMBER;
                    break;
                default:
                    errorCode = (int)FundException.ACCESS_DENIED; // this should some common errorcode fr default-has to be checked
                    break;
            }
            return errorCode;
        }

        public int GetExceptionMinorCode(TransactionResponse response)
        {
            var msg = response.PostingFlag + " " + response.PostingNote;
            int errorCode = 0;

            switch (msg)
            {
                case "2 Access Denied":
                    errorCode = (int)FundException.ACCESS_DENIED;
                    break;
                case "2 Invalid account number":
                    errorCode = (int)FundException.INVALID_ACCOUNT_NUMBER;
                    break;
                case "2 Invalid admin number":
                    errorCode = (int)FundException.INVALID_ADMIN_NUMBER;
                    break;
                case "2 Invalid card number":
                    errorCode = (int)FundException.INVALID_CARD_NUMBER;
                    break;
                case "2 Invalid transaction amount":
                    errorCode = (int)FundException.INVALID_TRANSACTION_AMOUNT;
                    break;
                case "4 Invalid tran type":
                    errorCode = (int)FundException.INVALID_TRANSACTION_TYPE;
                    break;
                case "2 Invalid message type identifier":
                    errorCode = (int)FundException.INVALID_MSG_TYPE_IDENTIFIER;
                    break;
                case "2 Tran Code is not part of Message Type Identifier class.":
                    errorCode = (int)FundException.TRAN_CODE_IS_NOT_PART_OF_MSG_TYPE_IDENTIFIER;
                    break;
                case "2 Invalid transaction currency code":
                    errorCode = (int)FundException.INVALID_TRANSACTION_CURRENCY_CODE;
                    break;
                case "2 Mismatched Transaction Currency Code.":
                    errorCode = (int)FundException.MISMATCH_TRANSACTION_CURRENCY_CODE;
                    break;
                case "2 Disallow Duplicate Transaction within 1 minute.":
                    errorCode = (int)FundException.DISALLOW_DUPLICATE_TRANSACTION;
                    break;
                case "2 Invalid effective date":
                    errorCode = (int)FundException.INVALID_EFFECTIVE_DATE;
                    break;
                case "2 Future dated transactions are not allowed":
                    errorCode = (int)FundException.FUTURE_DATE_TRANSACTIONS_NOT_ALLOWED;
                    break;
                case "2 Transaction Rejected:  Invalid Deferment":
                    errorCode = (int)FundException.INVALID_DEFERMENT;
                    break;
                case "2 Transaction Rejected: Reversal target not found":
                    errorCode = (int)FundException.REVERSAL_TARGET_NOT_FOUND;
                    break;
                case "2 Transaction Rejected: Payment reversal target not found":
                    errorCode = (int)FundException.PAYMENT_REVERSAL_TARGET_NOT_FOUND;
                    break;
                case "2 Platform posting failed for cardholder or company":
                    errorCode = (int)FundException.POSTING_FAILED_FOR_CARDHOLDER;
                    break;
                case "2 Batch Account Posting Failed.":
                    errorCode = (int)FundException.BATCH_ACCOUNT_POSTING_FAILED;
                    break;
                case "2 Plan type does not match logic module.":
                    errorCode = (int)FundException.PLAN_TYPE_DOES_NOT_MATCH;
                    break;
                case "2 An Error Occurred  During Account Load":
                    errorCode = (int)FundException.ERROR_OCCURRED_DURING_ACCOUNT_LOAD;
                    break;
                case "2 Transaction Request is Accepted":
                    errorCode = (int)FundException.TRANSACTION_REQUEST_IS_ACCEPTED;
                    break;
                case "2 An Error Occurred while saving":
                    errorCode = (int)FundException.ERROR_OCCURRED_WHILE_SAVING;
                    break;
                case "2 Load Amt Grt/Lsr Min/Max Load Amt":
                    errorCode = (int)FundException.LOAD_AMT_GRT_LSR_MIN_MAX_LOAD_AMT;
                    break;
                case "2 Exceed Limit of Daily Loads":
                    errorCode = (int)FundException.EXCEED_LIMIT_OF_DAILY_LOADS;
                    break;
                case "2 Exceed Limit Weekly Loads":
                    errorCode = (int)FundException.EXCEED_LIMTIT_WEEKLY_LOADS;
                    break;
                case "2 Exceed Limit Monthly Loads":
                    errorCode = (int)FundException.EXCEED_LIMTIT_MONTHLY_LOADS;
                    break;
                case "2 Exceed Limit Yearly Loads":
                    errorCode = (int)FundException.EXCEED_LIMTIT_YEARLY_LOADS;
                    break;
                case "2 Exceed Limit Lifetime Loads":
                    errorCode = (int)FundException.EXCEED_LIMTIT_LIFETIME_LOADS;
                    break;
                case "2 Maximum Reloads Must Be Define":
                    errorCode = (int)FundException.MAXIMUM_RELOADS_MUST_BE_DEFINE;
                    break;
                case "2 Reload not allowed":
                    errorCode = (int)FundException.RELOAD_NOT_ALLOWED;
                    break;
                case "2 Pending Load Request into System":
                    errorCode = (int)FundException.PENDING_LOAD_REQUEST_INTO_SYSTEM;
                    break; 
                default:
                    errorCode = (int)FundException.ACCESS_DENIED; // this should some common errorcode fr default-has to be checked
                    break;               
            }

            return errorCode;
        }

        public int GetExceptionMinorCode(AccountResponse response)
        {
            var msg = response.ResErrorCode + " " + response.ResErrorMsg;
            int errorCode = 0;

            switch (msg)
            {
                case "ErrCard01 Request Rejected, Invalid Card/DDA Number":
                    errorCode = (int)FundException.INVALID_CARD_OR_DDA_NUMBER;
                    break;
                case "Errcnt01 Invalid Country Code":
                    errorCode = (int)FundException.INVALID_COUNTRY_CODE;
                    break;
                case "Errcnt01 Invalid State or different than given Country":
                    errorCode = (int)FundException.INVALID_STATE_DIFF_THAN_GIVEN_COUTNRY;
                    break;
                case "Errcnt01 Country Code is invalid":
                    errorCode = (int)FundException.INVALID_COUNTRY_CODE;
                    break;
                case "Errcnt01 Request Rejected, State does not belong to given country/Invalid State":
                    errorCode = (int)FundException.INVALID_STATE_DIFF_THAN_GIVEN_COUTNRY;
                    break;
                case "Errcp01 Request Rejected, Cannot Update Card":
                    errorCode = (int)FundException.CAN_NOT_UPDATE_CARD;
                    break;
                case "Errcp02 Request Rejected, Cannot Update Card":
                    errorCode = (int)FundException.CAN_NOT_UPDATE_CARD;
                    break;
                case "ErrCur01 Invalid UserField1 Currency":
                    errorCode = (int)FundException.INVALID_USERFIELD1_CURRENCY;
                    break;
                case "ErrCur01 Invalid UserField2 Currency":
                    errorCode = (int)FundException.INVALID_USERFIELD2_CURRENCY;
                    break;
                case "ErrCur01 Invalid UserField3 Currency":
                    errorCode = (int)FundException.INVALID_USERFIELD3_CURRENCY;
                    break;
                case "ErrCur01 Invalid UserField4 Currency":
                    errorCode = (int)FundException.INVALID_USERFIELD4_CURRENCY;
                    break;
                case "ErrCur01 Invalid UserField5 Currency":
                    errorCode = (int)FundException.INVALID_USERFIELD5_CURRENCY;
                    break;
                case "Errdob01 Request Rejected, Invalid Date of Birth":
                    errorCode = (int)FundException.INVALID_DATE_OF_BIRTH;
                    break;
                case "Errdob01 Date of birth must be equal to or less than present date":
                    errorCode = (int)FundException.DOB_MUST_BE_EQUAL_OR_LESS_THAN_PRESENT_DATE;
                    break;
                case "ErrEml01 Request Rejected, Invalid Email ID":
                    errorCode = (int)FundException.INVALID_EMAIL_ID;
                    break;
                case "ErrExd01 Request Rejected, Invalid Expiration Date":
                    errorCode = (int)FundException.INVALID_EXPIRATION_DATE;
                    break;
                case "ErrExd01 ID expiration date must have a future date":
                    errorCode = (int)FundException.ID_EXPIRATION_MUST_HAVE_FUTURE_DATE;
                    break;
                case "Errgovid01 Request Rejected, Invalid Government ID":
                    errorCode = (int)FundException.INVALID_GOVT_ID;
                    break;
                case "ErrIsd01 Request Rejected, Invalid Issue Date":
                    errorCode = (int)FundException.INVALID_ISSUE_DATE;
                    break;
                case "ErrIsud01 ID Issue Date must be equal or less than present date and greater than Date of Birth":
                    errorCode = (int)FundException.ISSUE_DATE_LESSTHAN_GREATER_THAN_DOB;
                    break;
                case "ErrMand01 ID Number / Country / State / Issue Date cannot be left blank":
                    errorCode = (int)FundException.ID_STATE_COUNTRY_ISSUE_DATE_CANNOT_BE_LEFT_BLANK;
                    break;
                case "ErrMand01 ID Number/Country can not be blank":
                    errorCode = (int)FundException.ID_NUMBER_COUNTRY_CANNOT_BE_LEFT_BLANK;
                    break;
                case "ErrPlst01 Generate New Cards Tape value is Do Not Generate Plastics on product":
                    errorCode = (int)FundException.GENERATE_NEW_CARDS_TAPE_VALUE;
                    break;
                case "Errqc01 Request Rejected, No Record Found For User":
                    errorCode = (int)FundException.NO_RECORDS_FOUND_FOR_USER;
                    break;
                case "Errqc01 Request Rejected, Cannot Update Card":
                    errorCode = (int)FundException.CAN_NOT_UPDATE_CARD;
                    break;
                case "ErrReFlg01 Ship Address1/State/City/ZipCode Can not null":
                    errorCode = (int)FundException.SHIP_ADDRESS1_STATE_CITY_ZIP_CANNOT_NULL;
                    break;
                case "ErrReFlg01 Request Rejected, Invalid Reissue Flag Value":
                    errorCode = (int)FundException.INVALID_REISSUE_FLAG_VALUE;
                    break;
                case "ErrReis01 Card may not be re-issue because it is recently re-issued":
                    errorCode = (int)FundException.CARD_MAY_NOT_BE_REISSUE_RECENTLY_REISUUED;
                    break;
                case "ErrReis01 Card may not be re-issue because it is recently re-issued/Card is not Active":
                    errorCode = (int)FundException.CARD_MAY_NOT_BE_REISSUE_CARD_IS_NOT_ACTIVE;
                    break;
                case "Errst01 Request Rejected, Invalid State":
                    errorCode = (int)FundException.INVALID_STATE;
                    break;
                case "Errst01 Request Rejected, Invalid Ship State":
                    errorCode = (int)FundException.INVALID_SHIP_STATE;
                    break;
                case "Errstat01 State Can Not be Null":
                    errorCode = (int)FundException.STATE_CANNOT_BE_NULL;
                    break;
                case "ErrUfd01 Invalid UserField1 Date":
                    errorCode = (int)FundException.INVALID_USERFIELD1_DATE;
                    break;
                case "ErrUfd01 Invalid UserField2 Date":
                    errorCode = (int)FundException.INVALID_USERFIELD2_DATE;
                    break;
                case "ErrUfd01 Invalid UserField3 Date":
                    errorCode = (int)FundException.INVALID_USERFIELD3_DATE;
                    break;
                case "ErrUfd01 Invalid UserField4 Date":
                    errorCode = (int)FundException.INVALID_USERFIELD4_DATE;
                    break;
                case "ErrUfd01 Invalid UserField5 Date":
                    errorCode = (int)FundException.INVALID_USERFIELD5_DATE;
                    break;
                case "Errvt01 Request Rejected, SSN should never ever start with a number greater than 799 for the first 3 digits.":
                    errorCode = (int)FundException.SSN_SHOULD_NEVER_START_WITH_799;
                    break;
                case "Errvt01 Request Rejected, Invalid SSN Number":
                    errorCode = (int)FundException.INVALID_SSN_NUMBER;
                    break;
                case "Errvt01 Request Rejected, Invalid ID Number":
                    errorCode = (int)FundException.INVALID_ID_NUMBER;
                    break;
                default:
                    errorCode = (int)FundException.ACCESS_DENIED; // this should some common errorcode fr default-has to be checked
                    break;
            }
            return errorCode;
        }

		#endregion
    }
}