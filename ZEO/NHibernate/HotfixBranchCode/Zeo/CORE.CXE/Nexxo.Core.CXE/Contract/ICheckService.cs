using System;

using MGI.Core.CXE.Data;

using CheckCommit = MGI.Core.CXE.Data.Transactions.Commit.Check;
using CheckStage = MGI.Core.CXE.Data.Transactions.Stage.Check;

namespace MGI.Core.CXE.Contract
{
	public interface ICheckService
	{
		/// <summary>
		/// This method is to get the committed check
		/// </summary>
		/// <param name="Id">This is unique identifier of check commit</param>
		/// <returns>Committed check details</returns>
		CheckCommit Get(long Id);

		/// <summary>
		/// This method is to get the check stage status
		/// </summary>
		/// <param name="Id">This is unique identifier of check stage</param>
		/// <returns>Status of check stage</returns>
		int GetStatus(long Id);

		/// <summary>
		/// This method is to update the check stage with new status
		/// </summary>
		/// <param name="Id">This is unique identifier of check stage</param>
		/// <param name="newStatus">This is Status to be updated in check stage</param>
		/// <param name="timezone">This is time zone</param>
		/// <returns></returns>
		void Update(long Id, int newStatus, string timezone);

		/// <summary>
		/// This method is to update the check type and fee of check stage
		/// </summary>
		/// <param name="Id">Unique identifier of check stage</param>
		/// <param name="checkType">Check Type to be updated in check stage</param>
		/// <param name="fee">Fee to be updated in check stage</param>
		/// <param name="timezone" >This is time zone </param>
		void Update(long Id, int checkType, decimal fee, string timezone);

		/// <summary>
		/// This method is to create the check stage
		/// </summary>
		/// <param name="check">This is check stage details</param>
		/// <param name="timezone" >This is time zone </param>
		/// <returns>Unique identifier of check stage</returns>
		long Create(CheckStage check, string timezone);

		/// <summary>
		/// This is method is to commit the staged check
		/// </summary>
		/// <param name="Id">Unique identifier of check stage</param>
		/// <returns></returns>
		void Commit(long Id);

		/// <summary>
		/// This method is to get the check images 
		/// </summary>
		/// <param name="Id">Unique identifier of check stage</param>
		/// <returns>CheckImages object containing front and back images</returns>
		CheckImages GetImages(long Id);

		/// <summary>
		/// This method is to update the check type, amount and fee of check stage
		/// </summary>
		/// <param name="Id">Unique identifier of check stage</param>
		/// <param name="checkType">Check type to be updated in check stage</param>
		/// <param name="fee">Fee to be updated in check stage</param>
		/// <param name="amount">Amount to be updated in check stage</param>
		/// <param name="timezone">This is time zone</param>
		/// <returns></returns>
		void Update(long Id, int checkType, decimal amount, decimal fee, string timezone);

	}
}