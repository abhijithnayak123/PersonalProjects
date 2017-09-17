using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data
{
	public enum CompareTypes
	{
		Equal = 1,
		NotEqual,
		In,
		NotIn,
		GreaterThan,
		LessThan,
		GreaterThanOrEqual,
		LessThanOrEqual,
		Between
	}
}
