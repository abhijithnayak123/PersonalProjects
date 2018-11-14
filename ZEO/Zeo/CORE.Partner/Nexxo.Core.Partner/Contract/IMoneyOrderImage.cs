using MGI.Core.Partner.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Contract
{
	public interface IMoneyOrderImage
	{

		/// <summary>
		/// This method is to create the money order image
		/// </summary>
		/// <param name="moImage">This is money order image details</param>
		/// <param name="timezone">This is time zone</param>
		/// <returns>Unique identifier of money order image</returns>
		long Create(MoneyOrderImage moImage, string timezone);

		/// <summary>
		/// This method is to update the money order image
		/// </summary>
		/// <param name="moImage">This is money order image details to be updated</param>
		/// <param name="timezone">This is time zone</param>
		/// <returns></returns>
		void Update(MoneyOrderImage moImage, string timezone);

		/// <summary>
		/// This method is to get the money order images by transaction id
		/// </summary>
		/// <param name="trnxId">This is transaction id</param>
		/// <returns>Money order image details</returns>
		MoneyOrderImage FindMoneyOrderByTxnId(Guid trnxId);
	}
}
