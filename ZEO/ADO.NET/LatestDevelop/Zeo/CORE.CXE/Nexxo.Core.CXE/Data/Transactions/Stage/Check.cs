using System;

namespace MGI.Core.CXE.Data.Transactions.Stage
{
    public class Check : CheckBase
	{
		public virtual void AddImages( byte[] front, byte[] back, string format )
		{
			CheckImages images = new CheckImages
			{
				Front = front,
				Back = back,
				Format = format,
				Check = this,
				DTServerCreate = DateTime.Now
			};
			this.Images = images;
		}
	}
}
