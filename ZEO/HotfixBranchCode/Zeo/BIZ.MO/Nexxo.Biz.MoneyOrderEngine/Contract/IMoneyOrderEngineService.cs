using System;
using System.Collections.Generic;

using MGI.Biz.Common.Data;
using MGI.Biz.MoneyOrderEngine.Data;
using MGI.Common.Util;

namespace MGI.Biz.MoneyOrderEngine.Contract
{
    public interface IMoneyOrderEngineService
    {
		/// <summary>
		/// Gets money order fee
		/// </summary>
		/// <param name="customerSessionId">customer session unique identifier</param>
		/// <param name="moneyOrder">contains money order information</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>money order transaction fee</returns>
        TransactionFee GetMoneyOrderFee(long customerSessionId, MoneyOrderData moneyOrder, MGIContext mgiContext);

		/// <summary>
		/// Stages money order transaction
		/// </summary>
		/// <param name="customerSessionId">customer session unique identifier</param>
		/// <param name="moneyOrderPurchase">contains money order purchase information</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>money order details</returns>
        MoneyOrder Add(long customerSessionId, MoneyOrderPurchase moneyOrderPurchase, MGIContext mgiContext);

		/// <summary>
		/// Commits monder order transactions
		/// </summary>
		/// <param name="customerSessionId">customer session unique identifier</param>
		/// <param name="moneyOrderId">contains money order unique identifer</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        void Commit(long customerSessionId, long moneyOrderId, MGIContext mgiContext);

		/// <summary>
		/// Updates money order transactions
		/// </summary>
		/// <param name="customerSessionId">customer session unique identifier</param>
		/// <param name="moneyOrder">contains money order information</param>
		/// <param name="moneyOrderId">contains money order unique identifer</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		void UpdateMoneyOrder(long customerSessionId, MoneyOrder moneyOrder, long moneyOrderId, MGIContext mgiContext);

		/// <summary>
		/// updates money order transaction status
		/// </summary>
		/// <param name="customerSessionId">customer session unique identifier</param>
		/// <param name="moneyOrderId">contains money order unique identifer</param>
		/// <param name="newMoneyOrderStatus">contian new money order status</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        void UpdateMoneyOrderStatus(long customerSessionId, long moneyOrderId, int newMoneyOrderStatus, MGIContext mgiContext);
		
		/// <summary>
		/// Returns the money order scanned details
		/// </summary>
		/// <param name="customerSessionId">customer session unique identifier</param>
		/// <param name="moneyOrderId">contains money order unique identifer</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>money order check printed details</returns>
        MoneyOrderCheckPrint GetMoneyOrderCheck(long customerSessionId, long moneyOrderId, MGIContext mgiContext);

		/// <summary>
		/// Gets money order stage information
		/// </summary>
		/// <param name="customerSessionId">customer session unique identifier</param>
		/// <param name="moneyOrderId">contains money order unique identifer</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>money order stage details</returns>
        MoneyOrder GetMoneyOrderStage(long customerSessionId, long moneyOrderId, MGIContext mgiContext);

		/// <summary>
		/// Resubmiting money order information
		/// </summary>
		/// <param name="customerSessionId">customer session unique identifier</param>
		/// <param name="moneyOrderId">contains money order unique identifer</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>bool</returns>
		bool Resubmit(long customerSessionId, long moneyOrderId, MGIContext mgiContext);


		/// <summary>
		/// To test money order print is working as expected part of Epson device diagnostics
		/// </summary>
		/// <param name="agentSessionId">agent session unique identifier</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>money order check print details</returns>
		MoneyOrderCheckPrint GetMoneyOrderDiagnostics(long agentSessionId, MGIContext mgiContext);
    }
}
