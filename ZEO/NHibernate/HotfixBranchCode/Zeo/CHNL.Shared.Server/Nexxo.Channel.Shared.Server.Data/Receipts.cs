using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MGI.Channel.Shared.Server.Data
{
	[DataContract]
	public class Receipt
	{
		public Receipt()
		{

		}

		[DataMember]
		public List<string> Lines { get; set; }

		override public string ToString()
		{
			string str = string.Empty;
			if (Lines != null)
			{
				str = string.Concat(str, "Lines = ");
				foreach (var line in Lines)
				{
					str = string.Concat(str, "Line = ", line, "\r\n");
				} 
			}
			return str;
		}
	}

	[DataContract]
	public class Receipts
	{
		[DataMember]
		public List<Receipt> receipts { get; set; }

        [DataMember]
        public List<string> receiptType { get; set; }

		override public string ToString()
		{
			string str = string.Empty;
			if (receipts != null)
			{
				str = string.Concat(str, "Receipts = ");
				foreach (var receipt in receipts)
				{
					str = string.Concat(str, "Receipt = ", receipt, "\r\n");
				} 
			}
			return str;
		}
	}

    [DataContract]
    public class ReceiptData
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string PrintData { get; set; }
        
        [DataMember]
        public int NumberOfCopies { get; set; }

        override public string ToString()
        {
            string str = string.Empty;
            str = string.Concat(str, "Name = ", Name, "\r\n");
            str = string.Concat(str, "PrintData = ", PrintData, "\r\n");
            str = string.Concat(str, "NumberOfCopies = ", NumberOfCopies.ToString(), "\r\n");
            return str;
        }

    }


}
