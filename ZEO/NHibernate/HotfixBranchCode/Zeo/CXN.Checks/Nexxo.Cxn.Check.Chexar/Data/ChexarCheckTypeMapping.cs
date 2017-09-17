using System;
using System.Collections.Generic;

using MGI.Cxn.Check.Data;

namespace MGI.Cxn.Check.Chexar.Data
{
	public class ChexarCheckTypeMapping
	{
		public virtual int ChexarType { get; set; }
		public virtual string Name { get; set; }
		public virtual CheckType CheckType { get; set; }
	}
}
