using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.Partner.Data
{
	public class PasswordStatus
	{
		public bool ChangeRequired { get; set; }
		public DateTime ExpirationDate { get; set; }
	}
}
