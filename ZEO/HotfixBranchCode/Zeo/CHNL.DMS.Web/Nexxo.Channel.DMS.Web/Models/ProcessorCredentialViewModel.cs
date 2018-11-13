using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Linq;

namespace MGI.Channel.DMS.Web.Models
{
	public class ProcessorCredentialViewModel : BaseModel
	{
		public ProcessorCredentialViewModel()
		{
			//Removing the null values Eg : Transaction History has processor name null. 
			Processors = Providers.Where(p => p.ProcessorName != null && p.ProcessorName != string.Empty).ToList();

			//Select the distinct values of processor names.
			Processors = Processors.ToList().GroupBy(m => m.ProcessorName).Select(b => b.First()).ToList(); 
		}
		public long LocationID { get; set; }

		public List<ProcessorCrendential> Credentials { get; set; }

		public List<Product> Processors { get; set; }

	}

}

