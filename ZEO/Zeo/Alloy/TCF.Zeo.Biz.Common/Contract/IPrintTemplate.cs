using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCF.Zeo.Biz.Common.Contract
{
	public interface IPrintTemplate
	{
		/// <summary>
		/// This method to get check print template for MO and Check proccessing
		/// </summary>
		/// <param name="fileNames">REST API Url</param>
		/// <returns>Responce</returns>
		string GetPrintTemplate(List<string> fileNames);
	}
}
