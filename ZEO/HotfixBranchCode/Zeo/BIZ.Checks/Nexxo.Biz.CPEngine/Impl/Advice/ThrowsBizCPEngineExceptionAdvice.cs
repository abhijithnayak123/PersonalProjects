using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Common.Sys;
using MGI.Biz.CPEngine.Contract;
using MGI.Biz.Compliance.Contract;

using Spring.Aop;

namespace MGI.Biz.CPEngine.Impl.Advice
{
	public class ThrowsBizCPEngineExceptionAdvice : IThrowsAdvice
	{
			public void AfterThrowing(BizCPEngineException bizCustEx)
			{
				throw bizCustEx;
			}

			public void AfterThrowing( BizComplianceLimitException bizLimitEx )
			{
				throw bizLimitEx;
			}

			public void AfterThrowing(NexxoException nexEx)
			{
				var bizEx = new BizCPEngineException(nexEx.MinorCode, nexEx.Message, nexEx.InnerException);

				throw bizEx;
			}
	}
}
