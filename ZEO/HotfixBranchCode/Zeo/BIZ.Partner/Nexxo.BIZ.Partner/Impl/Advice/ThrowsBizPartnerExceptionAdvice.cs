using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Common.Sys;
using MGI.Biz.Partner.Contract;

using Spring.Aop;
using MGI.Biz.Compliance.Contract;
using MGI.Biz.FundsEngine.Contract;

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

		public void AfterThrowing(BizDataStructureException bizDataStructureEx)
		{
			throw bizDataStructureEx;
		}

		public void AfterThrowing(BizLocationException bizLocationEx)
		{
			throw bizLocationEx;
		}

		public void AfterThrowing(BizReceiptException bizReceiptEx)
		{
			throw bizReceiptEx;
		}

		public void AfterThrowing(BizShoppingCartException bizShoppingCartEx)
		{
			throw bizShoppingCartEx;
		}

		public void AfterThrowing(BizTerminalException bizTerminalEx)
		{
			throw bizTerminalEx;
		}

		public void AfterThrowing(BizUserException bizUserEx)
		{
			throw bizUserEx;
		}

		public void AfterThrowing(BizPartnerException bizPtnrException)
		{
			throw bizPtnrException;
		}

		public void AfterThrowing(BizComplianceLimitException bizLimitEx)
		{
			throw bizLimitEx;
		}

        public void AfterThrowing(BizFundsException bizFundsEx)
        {
            throw bizFundsEx;
        }

		public void AfterThrowing(NexxoException nexEx)
		{
			var bizEx = new BizPartnerException(nexEx.MinorCode, nexEx.Message, nexEx.InnerException);

			throw bizEx;
		}

	}
}
