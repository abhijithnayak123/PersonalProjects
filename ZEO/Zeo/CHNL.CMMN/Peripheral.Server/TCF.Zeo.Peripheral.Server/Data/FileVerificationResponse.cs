using System.Collections.Generic;

namespace TCF.Zeo.Peripheral.Server.Data
{
	public class FileVerificationResponse
	{
		public List<PSFile> FileStatus { get; set; }
		public string Path { get; set; }
	}
}
