using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.Fund.DataProtectionMigrator
{
	public interface IDataMigrator
	{
		void Run(bool isTest, int oldSlot, int newSlot);
	}
}
