using MGI.Biz.Compliance.Contract;
using MGI.Common.Sys;

using Spring.Aop;

namespace MGI.Biz.Compliance.Impl.Advice
{
	public class ThrowsBizComplianceExceptionAdvice : IThrowsAdvice
	{
		public void AfterThrowing( BizComplianceException bizCmplEx )
		{
			throw bizCmplEx;
		}

		public void AfterThrowing( AlloyException nexEx )
		{
			var bizEx = new BizComplianceException( nexEx.ProviderCode, nexEx.Message, nexEx.InnerException );

			throw bizEx;
		}
	}
}
