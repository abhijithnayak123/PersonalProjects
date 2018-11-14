using System.Collections.Generic;

namespace MGI.Cxn.MoneyTransfer.Data
{
	public class Field
	{
		public string Label { get; set; }
		public string TagName { get; set; }
		public string DataType { get; set; }
		public int MaxLength { get; set; }
		public string RegularExpression { get; set; }
		public bool IsMandatory { get; set; }
		public bool IsDynamic { get; set; }

		public Dictionary<string, object> MetaData { get; set; }
		public Dictionary<string, string> Values { get; set; }
	}
}