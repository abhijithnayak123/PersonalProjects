using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGI.Common.TransactionalLogging.Data
{
    [BsonIgnoreExtraElements]
    public class TransactionLogEntry
    {
		public TransactionLogEntry()
		{
			Desc = new List<string>();
		}

		//[BsonId] //Commented because we are using same object for logging Begin and End of the method. So in this case
				   // _id will throw error while saving to mongoDB.
		public ObjectId Id { get; set; }
		public string Timestamps { get; set; }
		public string AgentSessionId { get; set; }
		public string CustomerSessionId { get; set; }
		public string ModuleName { get; set; }
		public string LocationId { get; set; }
		public string LocationName { get; set; }
		public string ChannelPartnerName { get; set; }
		public string TerminalName { get; set; }
		public string CXNType { get; set; }
		public string AgentName { get; set; }
		public EventSeverity EventSeverity { get; set; }
		public TransactionLogApplicationServer ApplicationServer { get; set; }
		public AlloyLayerName AlloyLayerName { get; set; }
		public string MethodName { get; set; }
		public string Message { get; set; }
		public List<string> Desc { get; set; }
		public string HostDevice { get; set; }
		public string BranchId { get; set; }
		public string BankId { get; set; }     

    }
}


