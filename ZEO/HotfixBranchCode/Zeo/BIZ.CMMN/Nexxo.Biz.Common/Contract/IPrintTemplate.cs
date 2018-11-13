using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.Common.Contract
{
	public interface IPrintTemplate
	{
		/// <summary>
		/// This method to get check print template for MO and Check proccessing
		/// </summary>
		/// <param name="requestUri">REST API Url</param>
		/// <returns>Responce</returns>
		string GetPrintTemplate(string requestUri);
	}
}
