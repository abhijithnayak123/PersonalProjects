using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.Check.Certegy.Data
{
	public class CheckImage
	{
		public virtual System.Guid CertegyCheckImagePK { get; set; }
		
		public virtual byte[] Front { get; set; }
		public virtual byte[] Back { get; set; }
		public virtual string Format { get; set; }
		public virtual byte[] FrontTIF { get; set; }
		public virtual byte[] BackTIF { get; set; }

		public virtual Transaction CertegyTrx { get; set; }

		public virtual System.DateTime DTServerCreate { get; set; }
		public virtual DateTime? DTServerLastModified { get; set; }
		public virtual System.DateTime DTTerminalCreate { get; set; }
		public virtual DateTime? DTTerminalLastModified { get; set; }
	}
}
