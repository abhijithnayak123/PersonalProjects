using System;

namespace MGI.Common.DataAccess.Contract
{
	public interface IIDNumberGenerator
	{
		long NextIDNumber( string type );
	}
}
