using System;
using System.Diagnostics;
using AopAlliance.Intercept;
using Spring.Aop;

namespace TCF.Zeo.Peripheral.CheckScanner.EpsonTMS9000.Impl
{
	public class EpsonScanBeforeAdvise : IMethodInterceptor
	{
		public object Invoke(IMethodInvocation invocation)
		{
			Trace.WriteLine(String.Format(
				"Intercepted call : about to invoke method '{0}'", invocation.Method.Name), "Spring Advise");

			object returnValue = invocation.Proceed();

			Trace.WriteLine(String.Format(
				"Intercepted call : returned '{0}'", returnValue), "Spring Advise");

			return returnValue;
		}
	}

	public class EpsonScanFaultAdvise : IThrowsAdvice
	{
		public object AfterThrowing(Exception ex)
		{
			Trace.WriteLine(String.Format(
							"Intercepted Exception : returned '{0}'", ex.StackTrace), "Spring Exception");
			//epsonException.SetError(com.epson.bank.driver.ErrorCode.ERR_HANDLE, "Unhandled Exception. Critical Error");
			return ex;
		}
	}
}
