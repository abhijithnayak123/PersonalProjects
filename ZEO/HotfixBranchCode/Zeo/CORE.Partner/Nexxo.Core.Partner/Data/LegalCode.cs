﻿using MGI.Common.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data
{
	public class LegalCode : NexxoModel
	{
		public virtual string Code { get; set; }
		public virtual string Name { get; set; }
	}
}
