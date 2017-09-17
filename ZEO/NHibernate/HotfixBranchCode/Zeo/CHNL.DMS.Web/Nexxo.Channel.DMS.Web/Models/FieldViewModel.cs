using System.Collections.Generic;
using System.Web.Mvc;

namespace MGI.Channel.DMS.Web.Models
{
	public class FieldViewModel
	{
		public string Label { get; set; }

		public string TagName { get; set; }

		public string DataType { get; set; }

		public int MaxLength { get; set; }

		public bool IsRequired { get; set; }

		public string ValidationMessage { get; set; }

		public string RegularExpression { get; set; }

		public bool IsDynamic { get; set; }

		public IEnumerable<SelectListItem> Values { get; set; }

		public string Value { get; set; }
	}
}