using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Common.Sys;
using MGI.Biz.Compliance.Contract;
using Spring.Aop;
using MGI.Biz.CPEngine.Data;

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

			public void AfterThrowing(ProviderException providerEx)
			{
				throw providerEx;
			}
			public void AfterThrowing(Exception ex)
			{
				var bizEx = new BizCPEngineException(BizCPEngineException.CPENGINE_EXCEPTION, ex);
			}
	}
}
