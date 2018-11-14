using MGI.Common.Sys;
using MGI.Biz.Partner.Contract;
using MGI.Biz.FundsEngine.Data;
using Spring.Aop;
using MGI.Biz.Compliance.Contract;
using MGI.Biz.FundsEngine.Contract;
using MGI.Biz.CPEngine.Data;
using MGI.Biz.Partner.Data;
using System;

namespace MGI.Biz.Partner.Impl.Advice
{
	public class ThrowsBizPartnerExceptionAdvice : IThrowsAdvice
	{
		public void AfterThrowing(BizAgentException bizAgentException)
		{
			throw bizAgentException;
		}

		public void AfterThrowing(BizCustomerException bizCustomerEx)
		{
			throw bizCustomerEx;
		}

		public void AfterThrowing(BizLocationException bizLocationEx)
		{
			throw bizLocationEx;
		}

		public void AfterThrowing(BizShoppingCartException bizShoppingCartEx)
		{
			throw bizShoppingCartEx;
		}

		public void AfterThrowing(BizTerminalException bizTerminalEx)
		{
			throw bizTerminalEx;
		}

		public void AfterThrowing(BizChannelPartnerException bizCPEx)
		{
			throw bizCPEx;
		}

		public void AfterThrowing(BizComplianceLimitException bizLimitEx)
		{
			throw bizLimitEx;
		}

        public void AfterThrowing(BizFundsException bizFundsEx)
        {
            throw bizFundsEx;
        }

		public void AfterThrowing(BizCPEngineException bizCheckEx)
		{
			throw bizCheckEx;
		}

		public void AfterThrowing(BizTransactionHistoryException bizCheckEx)
		{
			throw bizCheckEx;
		}

		public void AfterThrowing(Exception ex)
		{
			throw ex;
		}

	}
}
