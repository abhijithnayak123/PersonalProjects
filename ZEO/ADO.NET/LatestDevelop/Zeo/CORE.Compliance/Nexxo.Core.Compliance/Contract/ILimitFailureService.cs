using System;
using MGI.Core.Compliance.Data;

namespace MGI.Core.Compliance.Contract
{
	public interface ILimitFailureService
	{
		/// <summary>
		/// This method is to add limit failure
		/// </summary>
		/// <param name="limitFailure">This is limit failure details</param>		
		/// <returns></returns>
		void Add( LimitFailure limitFailure );
	}
}
