using MGI.Cxn.MoneyTransfer.Data;
using System.Collections.Generic;

namespace MGI.Cxn.MoneyTransfer.Contract
{
	public interface IReceiver
	{
		long AddReceiver(Receiver receiver, Dictionary<string, object> context);
		long UpdateReceiver(Receiver receiver, Dictionary<string, object> context);
		long DeleteReceiver(long receiverId, Dictionary<string, object> context);
		List<Receiver> GetReceivers(long CustomerId);
		List<Receiver> GetReceivers(long CustomerId, string lastName);
		Receiver GetReceiver(long Id);
		Receiver GetReceiver(long customerId, string fullName);
		bool ImportReceivers(ImportRequest request, Dictionary<string, object> cxnContext);
	}
}
