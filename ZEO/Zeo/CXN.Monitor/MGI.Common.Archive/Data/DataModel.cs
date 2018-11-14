using System;

namespace MGI.Common.Archive.Data
{
	public class DataModel
	{
		public DateTime RunTime { get; set; }
		public string ConnectionString { get; set; }
		public string OutputDirectory { get; set; }
		public TransactionType TransactionType { get; set; }
		public int ArchivePriorToDays { get; set; }

		public DataModel()
		{
			RunTime = DateTime.Now;
		}
	}
}
