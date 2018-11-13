using System;

using NUnit.Framework;

namespace MGI.Biz.Customer.Test
{
	public static class TestExceptionHelper
	{
		/// <summary>
		/// Make sure the NexxoException minor code matches
		/// </summary>
		/// <typeparam name="T">NexxoException type</typeparam>
		/// <param name="code">Code that's being checked</param>
		/// <param name="minorCode">Minor code to match</param>
		public static void MinorCodeMatch<T>( TestDelegate code, int minorCode ) where T : MGI.Common.Sys.NexxoException
		{
			try
			{
				code();
				Assert.IsTrue( false );
			}
			catch ( T ex )
			{
				Assert.IsTrue( ex.MinorCode == minorCode );
			}
		}
	}
}
