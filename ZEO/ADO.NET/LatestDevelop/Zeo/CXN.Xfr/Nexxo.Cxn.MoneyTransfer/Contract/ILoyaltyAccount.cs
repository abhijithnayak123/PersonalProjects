using MGI.Cxn.MoneyTransfer.Data;
using System.Collections.Generic;

namespace MGI.Cxn.MoneyTransfer.Contract
{
	interface ILoyaltyAccount
	{
		List<MasterData> GetBannerMsgs(Dictionary<string, object> context);

		MGI.Cxn.MoneyTransfer.Data.CardDetails WUCardEnrollment(Account sender, PaymentDetails paymentDetails, Dictionary<string, object> context);

		MGI.Cxn.MoneyTransfer.Data.CardLookupDetails WUCardLookup(long customerAccountId, MGI.Cxn.MoneyTransfer.Data.CardLookupDetails LookupDetails, Dictionary<string, object> context);

		bool GetWUCardAccount(long customerAccountId);

		MGI.Cxn.MoneyTransfer.Data.Account DisplayWUCardAccountInfo(long cxnAccountId);
		bool UseGoldcard(long accountId, string WUGoldCardNumber, Dictionary<string, object> cxnContext);

		CardInfo GetCardInfo(string cardNumber, Dictionary<string, object> context);
	}
}
